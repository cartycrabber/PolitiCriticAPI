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

namespace PoliticalDeterminer.Services
{
    public class TextAnalyzer
    {
        private static TextAnalyzer INSTANCE = null;

        private NaiveBayes<NormalDistribution> nbClassifier;

        private TextAnalyzer()
        {
            var teacher = new NaiveBayesLearning<NormalDistribution>();

            string[] sampleText = System.IO.File.ReadAllLines(".../PoliticalDeterminerAPI/Data/TrainingData1.txt");

            string[][] words = sampleText.Tokenize();

            BagOfWords bagOfWords = new BagOfWords();

            //bagOfWords.Learn(words);

            int[] outputs = new int[sampleText.Length];

            for (int i = 0; i < sampleText.Length; i++)
            {
                //Index of the first space in the line (used to find the first word)
                int spaceIndex = sampleText[i].IndexOf(" ");
                //Set output to the number at the beginning of the line
                outputs[i] = Convert.ToInt32(sampleText[i].Substring(0, spaceIndex));
                //remove the number from the sample
                sampleText[i] = sampleText[i].Substring(spaceIndex).Trim();
            }

            nbClassifier = teacher.Learn(bagOfWords.Transform(words), outputs);
        }

        public float Analyze(string[] text)
        {
            return (float) nbClassifier.Decide(new BagOfWords().Transform(text.Tokenize())).Average();
        }

        public float Analyze(string text)
        {
            return Analyze(new string[] { text });
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