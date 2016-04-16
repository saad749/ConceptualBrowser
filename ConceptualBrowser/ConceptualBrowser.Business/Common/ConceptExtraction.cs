﻿using ConceptualBrowser.Business.Entities;
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


namespace ConceptualBrowser.Business.Common
{
    public class ConceptExtraction
    {
        List<Sentence> sentences = new List<Sentence>();
        public List<OptimalConceptTreeItem> Extract(String text, string languageCode)
        {
            IStemmer stemmer;
            IEmptyWords emptyWords;
            try
            {
                stemmer = Stemmers.GetStemmer(languageCode);
                emptyWords = new EmptyWords(languageCode);
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            ITextAnalyzer textAnalyzer = new TextAnalyzer(stemmer, emptyWords);
            //List<String> sentencesWithDelimiters = textAnalyzer.GetSentencesWithDelimiters(text);
            List<String> sentenceStringList = textAnalyzer.GetSentences(text);

            
            for (int i = 0; i < sentenceStringList.Count;i++)
            {
                int[] ranks = new int[] { i + 1, i + 1};
                int[] totals = new int[] { (sentenceStringList.Count + 2) / 2, (sentenceStringList.Count + 2) / 2 };
                Rank rank = new Rank(2, ranks, totals);

                this.sentences.Add(new Sentence(i, Constant.NotCovered, rank));
            }
            
            Coverage coverage = new Coverage(stemmer, emptyWords);
            //Why on Earth Should I send Sentence String List and Create Sentences from Sentences With Delimeters? Doesnt Sounds right!
            //Experiment Shows there is no difference, if we have Sentences created from sentenceStringList of sentencesWithDelimeters.
            //But Why to have more sentences??? With Deliemters give 82, without gives 42!!
            coverage.CreateBinaryRelation(sentenceStringList, this.sentences);
            

            List<OptimalConcept> optimals = coverage.ExtractAll();
            List<OptimalConceptTreeItem> optimalTree = CreateTree(optimals.OrderByDescending(o => o.Gain).ToList());
            return optimalTree;


            //Fragments of Commented code to Imrpove Readbility. The code is commented to be able to be easily Reused when needed
            //PrintBinaryRelation(coverage.BinaryRelation);
            //PerformanceMonitor monitor = new PerformanceMonitor
            //{
            //    Checkpoints = new List<Tuple<string, long>>(),
            //    Stopwatch = new Stopwatch()
            //};
            //monitor.Stopwatch.Start();
            //monitor.Checkpoints.Add(new Tuple<string, long>("ConceptExtraction.Extract()", monitor.Stopwatch.ElapsedTicks));
            //monitor.Checkpoints.Add(new Tuple<string, long>("Before Text Analyzing", monitor.Stopwatch.ElapsedTicks));
            //monitor.Checkpoints.Add(new Tuple<string, long>("After Text Analyzing", monitor.Stopwatch.ElapsedTicks));
            //monitor.Checkpoints.Add(new Tuple<string, long>("Before Initial Node Creation", monitor.Stopwatch.ElapsedTicks));
            //monitor.Checkpoints.Add(new Tuple<string, long>("After Initial Node Creation", monitor.Stopwatch.ElapsedTicks));
            //monitor.Checkpoints.Add(new Tuple<string, long>("Before Creating Binary Relation", monitor.Stopwatch.ElapsedTicks));
            //monitor.Checkpoints.Add(new Tuple<string, long>("After Creating Binary Relation", monitor.Stopwatch.ElapsedTicks));
            //monitor.Checkpoints.Add(new Tuple<string, long>("Before ExtractAll", monitor.Stopwatch.ElapsedTicks));
            //monitor.Checkpoints.Add(new Tuple<string, long>("After ExtractAll", monitor.Stopwatch.ElapsedTicks));
            //monitor.Stopwatch.Stop();
            //StringBuilder sb = new StringBuilder();
            //foreach (var tuple in monitor.Checkpoints)
            //{
            //    sb.Append(tuple.Item1);
            //    sb.Append("\t");
            //    sb.Append(tuple.Item2);
            //    sb.AppendLine();
            //}
            //File.WriteAllText("PerformanceLog.txt", sb.ToString());

            //List<String> sentencesWithDelimiters = textAnalyzer.GetSentencesWithDelimiters(text);
            //List<String> sentenceStringList = textAnalyzer.GetSentences(text);
            //for (int i = 0; i < sentencesWithDelimiters.Count; i++)
            //{
            //    int[] ranks = new int[] { i + 1, i + 1 };
            //    int[] totals = new int[] { (sentencesWithDelimiters.Count + 2) / 2, (sentencesWithDelimiters.Count + 2) / 2 };
            //    Rank rank = new Rank(2, ranks, totals);
            //    this.sentences.Add(new Sentence(i, Constant.NotCovered, rank));
            //}
        }

        public List<OptimalConceptTreeItem> CreateTree(List<OptimalConcept> optimals)
        {
            List<OptimalConceptTreeItem> treeItems = new List<OptimalConceptTreeItem>();

            int i = 1;
            int? parentId = null;
            foreach(OptimalConcept optimal in optimals)
            {
                if (i != 0)
                {
                    parentId = ((i + 1) - 1) / 2;
                }
                treeItems.Add(new OptimalConceptTreeItem()
                {
                    Id = i,
                    ParentId = parentId,
                    OptimalConcept = optimal
                });
                i++;
            }

            return treeItems;
        }


        /// <summary>
        /// This method is just to test the values of BR for comparison
        /// ublic int TotalResults { get; set; }
        //public bool Status { get; set; }
        //public List<RootNode> Roots { get; set; } = new List<RootNode>();
        //public int MaxRank { get; set; }
        /// </summary>
        /// <param name="binaryRelation"></param>
        private void PrintBinaryRelation(BinaryRelation binaryRelation)
        {
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