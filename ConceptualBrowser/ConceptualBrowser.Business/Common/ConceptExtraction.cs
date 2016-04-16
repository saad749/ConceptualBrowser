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


namespace ConceptualBrowser.Business.Common
{
    public class ConceptExtraction
    {
        List<Node> nodes = new List<Node>();
        public List<OptimalConceptTreeItem> Extract(String text, string languageCode)
        {
            PerformanceMonitor monitor = new PerformanceMonitor
            {
                Checkpoints = new List<Tuple<string, long>>(),
                Stopwatch = new Stopwatch()
            };
            monitor.Stopwatch.Start();
            monitor.Checkpoints.Add( new Tuple<string, long>("ConceptExtraction.Extract()", monitor.Stopwatch.ElapsedTicks));

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

            monitor.Checkpoints.Add(new Tuple<string, long>("Before Text Analyzing", monitor.Stopwatch.ElapsedTicks));
            List<String> sentencesWithDelimiters = textAnalyzer.GetSentencesWithDelimiters(text);
            List<String> sentences = textAnalyzer.GetSentences(text);
            monitor.Checkpoints.Add(new Tuple<string, long>("After Text Analyzing", monitor.Stopwatch.ElapsedTicks));

            monitor.Checkpoints.Add(new Tuple<string, long>("Before Initial Node Creation", monitor.Stopwatch.ElapsedTicks));
            for (int i = 0; i < sentencesWithDelimiters.Count;i++)
            {
                int[] ranks = new int[] { i + 1, i + 1};
                int[] totals = { (sentencesWithDelimiters.Count + 2) / 2, (sentencesWithDelimiters.Count + 2) / 2 };
                Rank rank = new Rank(2, ranks, totals);

                nodes.Add(new Node(i + "", -1, i, rank));
            }
            monitor.Checkpoints.Add(new Tuple<string, long>("After Initial Node Creation", monitor.Stopwatch.ElapsedTicks));

            monitor.Checkpoints.Add(new Tuple<string, long>("Before Creating Binary Relation", monitor.Stopwatch.ElapsedTicks));
            Coverage coverage = new Coverage(stemmer, emptyWords);
            coverage.CreateBinaryRelation(sentences, nodes, false);
            //PrintBinaryRelation(coverage.BinaryRelation);

            monitor.Checkpoints.Add(new Tuple<string, long>("After Creating Binary Relation", monitor.Stopwatch.ElapsedTicks));


            monitor.Checkpoints.Add(new Tuple<string, long>("Before ExtractAll", monitor.Stopwatch.ElapsedTicks));
            List<OptimalConcept> optimals = coverage.ExtractAll();
            monitor.Checkpoints.Add(new Tuple<string, long>("After ExtractAll", monitor.Stopwatch.ElapsedTicks));


            List<OptimalConceptTreeItem> optimalTree = CreateTree(optimals.OrderByDescending(o => o.Gain).ToList());


            monitor.Stopwatch.Stop();
            StringBuilder sb = new StringBuilder();
            foreach (var tuple in monitor.Checkpoints)
            {
                sb.Append(tuple.Item1);
                sb.Append("\t");
                sb.Append(tuple.Item2);
                sb.AppendLine();
            }
            File.WriteAllText("PerformanceLog.txt", sb.ToString());

            return optimalTree;
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
        /// </summary>
        /// <param name="binaryRelation"></param>
        private void PrintBinaryRelation(BinaryRelation binaryRelation)
        {
            //Console.WriteLine();
            //Console.WriteLine("Keywords Dictionary:");
            //LogHelper.PrintKeywords(binaryRelation.Keywords.Values.ToList());

            //Console.WriteLine();

            //Console.WriteLine("Total Results: " + binaryRelation.TotalResults);

            //Console.WriteLine("Status: " + binaryRelation.Status);
            //Console.WriteLine();

            //Console.WriteLine("Roots in Binary Relations: ");
            //LogHelper.PrintRootNodes(binaryRelation.Roots);
        }
    }
}
