using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accord.MachineLearning.Bayes;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics.Distributions.Fitting;
using Accord.MachineLearning;
using Accord.Statistics.Models.Regression;
using System.IO;

namespace PoliticalDeterminer.Services
{
    public class TextAnalyzer
    {
        private static TextAnalyzer INSTANCE = null;

        private const float DEFAULT_RANK = 0.5f;

        private NaiveBayes<NormalDistribution> nbClassifier;

        private BagOfWords bagOfWords;
        private string[] stopWords;

        //Make this class a singleton so that it is not retrained for every class it is used by
        private TextAnalyzer()
        {
            //Usage of a Naive Bayes classifier
            //Create the trainer, allowing for some regularlizatiton
            var teacher = new NaiveBayesLearning<NormalDistribution, NormalOptions>()
            {
                Options = {InnerOption = {Regularization = 1e-6}}
            };

            //Read in the training data and stop words
            string liberalTrainingPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data/liberal_training.txt");
            string conservativeTrainingPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data/conservative_training.txt");
            string stopWordsPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data/stop_words.txt");
            
            string[] liberalSamples = File.ReadAllLines(liberalTrainingPath);
            string[] conservativeSamples = File.ReadAllLines(conservativeTrainingPath);
            stopWords = File.ReadAllLines(stopWordsPath);

            //Concat the samples into one array (They are first read into their own array to allow us to know the amount of samples in each file)
            string[] samples = liberalSamples.Concat(conservativeSamples).ToArray();

            //Break the text up into individual words
            string[][] words = samples.Tokenize();

            //If for some reason we didn't actually read any training data, throw an exception cuz the classifier wont work
            if (words.Length == 0)
                throw new Exception("No training data for TextAnalyzer");

            //Remove common english words
            words = TrimStopWords(words);

            //Create a bag of words using the tokenized sample data
            bagOfWords = new BagOfWords();
            bagOfWords.Learn(words);

            //Populate the output array using the known lengths of the sample files
            int[] outputs = new int[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                if (i < liberalSamples.Length)
                    outputs[i] = 0;
                else
                    outputs[i] = 1;
            }

            //Train the classifier
            double[][] inputs = bagOfWords.Transform(words);
            nbClassifier = teacher.Learn(inputs, outputs);
        }

        private string[][] TrimStopWords(string[][] words)
        {
            //Iterate through every word in the matrix and remove any that are contained in the stopWords list
            for (int i = 0; i < words.Length; i++)
            {
                List<string> goodWords = new List<string>();
                for (int j = 0; j < words[i].Length; j++)
                {
                    if (!stopWords.Contains(words[i][j]))
                    {
                        goodWords.Add(words[i][j]);
                    }
                }
                words[i] = goodWords.ToArray();
            }
            return words;
        }

        /// <summary>
        /// Analyzes an array of text using a Naive Bayes classifier and returns estimated political leaning
        /// </summary>
        /// <param name="texts">Array of texts to classify</param>
        /// <returns>Average of all political leanings for the texts array. 0.0 is conservative, 1.0 is liberal, 0.5 is moderate</returns>
        public float Analyze(string[] texts)
        {
            if (texts.Length == 0)
                return DEFAULT_RANK;
            //Similar process to training: Tokenize, remove common words, throw into the bag of words
            string[][] words = texts.Tokenize();
            words = TrimStopWords(words);
            double[][] transform = bagOfWords.Transform(words);

            //Get the actual results and average them if possible
            int[] results = nbClassifier.Decide(transform);
            if (results.Length == 0)
                return DEFAULT_RANK;
            return (float) results.Average();
        }

        /// <summary>
        /// Analyzes an single text using a Naive Bayes classifier and returns estimated political leaning
        /// </summary>
        /// <param name="text">The text to classify</param>
        /// <returns>Estimated political leaning for the text. 0 is conservative, 1 is liberal</returns>
        public int Analyze(string text)
        {
            return (int)Analyze(new string[] { text });
        }

        public static TextAnalyzer GetInstance()
        {
            if(INSTANCE == null)
            {
                INSTANCE = new TextAnalyzer();
            }
            return INSTANCE;
        }
    }
}