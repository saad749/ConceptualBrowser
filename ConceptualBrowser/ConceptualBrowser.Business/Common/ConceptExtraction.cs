using ConceptualBrowser.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using ConceptualBrowser.Business.Common.Performance;
using ConceptualBrowser.Business.Common.Stemmer;
using ConceptualBrowser.Business.Common.Helpers;
using System.ComponentModel;
using CsvHelper;

namespace ConceptualBrowser.Business.Common
{
    public class ConceptExtraction
    {
        List<Sentence> sentences = new List<Sentence>();
        public List<OptimalConcept> Extract(String text, string languageCode, double coveragePercentage, BackgroundWorker backgroundWorker)
        {
            Coverage coverage = new Coverage(languageCode, text);

            List<OptimalConcept> optimals = coverage.ExtractAll(coveragePercentage, backgroundWorker);

            return optimals;
        }

        /// <summary>
        /// This method is just to test the values of BR for comparison
        /// </summary>
        /// <param name="binaryRelation"></param>
        private void PrintBinaryRelation(BinaryRelation binaryRelation)
        {
            var keywords = binaryRelation.Keywords;

            var stems = keywords.Select(x => x.Keyword).OrderBy(x => x).ToList();

            var sentences = keywords
                .SelectMany(x => x.Sentences)
                .GroupBy(p => p.SentenceIndex)
                .Select(g => g.FirstOrDefault())
                .Distinct()
                .OrderBy(x => x.SentenceIndex)
                .ToList();

            string[,] matrix = new string[sentences.Count + 1, keywords.Count + 1];

            for (int i = 0; i < keywords.Count; i++)
            {
                matrix[0, i + 1] = "\"" + keywords[i].Keyword.Trim() + "\"";
            }

            for (int j = 0; j < sentences.Count; j++)
            {
                matrix[j + 1, 0] = sentences[j].SentenceIndex.ToString();
            }

            for (int i = 0; i < keywords.Count; i++)
            {
                for (int j = 0; j < sentences.Count; j++)
                {
                    if(sentences[j].KeywordNodes.Any(x => x.Keyword == keywords[i].Keyword))
                        matrix[j + 1, i + 1] = 1.ToString();
                    else
                        matrix[j + 1, i + 1] = 0.ToString();
                }
            }


            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sb.Append(matrix[i, j] + ",");
                }
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText("Output\\matrix.csv", sb.ToString(), Encoding.UTF8);

            //Console.WriteLine();
            //Console.WriteLine("Keywords Dictionary:");
            //LogHelper.PrintKeywords(binaryRelation.Keywords);
            //Console.WriteLine();

            //Console.WriteLine("Total Results: " + binaryRelation.TotalResults);

            //Console.WriteLine("Status: " + binaryRelation.Status);
            //Console.WriteLine();

            //Console.WriteLine("Roots in Binary Relations: ");
            //LogHelper.PrintRootNodes(binaryRelation.Roots);
        }
        
    }
}
