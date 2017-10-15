using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accord.MachineLearning.Bayes;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics.Distributions.Fitting;
using Accord.MachineLearning;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using System.IO;
using System.Diagnostics;

namespace PoliticalDeterminer.Services
{
    public class TextAnalyzer
    {
        private static TextAnalyzer INSTANCE = null;

        private NaiveBayes<NormalDistribution> nbClassifier;

        private BagOfWords bagOfWords;
        private string[] stopWords;

        private TextAnalyzer()
        {
            var teacher = new NaiveBayesLearning<NormalDistribution, NormalOptions>()
            {
                Options = {InnerOption = {Regularization = 1e-6}}
            };

            string liberalTrainingPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data/liberal_training.txt");
            string conservativeTrainingPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data/conservative_training.txt");
            string stopWordsPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data/stop_words.txt");

            string[] liberalSamples = File.ReadAllLines(liberalTrainingPath);
            string[] conservativeSamples = File.ReadAllLines(conservativeTrainingPath);
            stopWords = File.ReadAllLines(stopWordsPath);

            string[] samples = liberalSamples.Concat(conservativeSamples).ToArray();

            string[][] words = samples.Tokenize();

            if (words.Length == 0)
                return;

            words = TrimStopWords(words);

            bagOfWords = new BagOfWords();

            bagOfWords.Learn(words);

            int[] outputs = new int[samples.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                if (i < liberalSamples.Length)
                    outputs[i] = 0;
                else
                    outputs[i] = 1;
            }

            double[][] bow = bagOfWords.Transform(words);

            nbClassifier = teacher.Learn(bow, outputs);
        }

        private string[][] TrimStopWords(string[][] words)
        {
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

        public float Analyze(string[] text)
        {
            string[][] words = text.Tokenize();
            words = TrimStopWords(words);
            double[][] transform = bagOfWords.Transform(words);
            int[] results = nbClassifier.Decide(transform);
            if (results.Length == 0)
                return 0.5f;
            return (float) results.Average();
        }

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