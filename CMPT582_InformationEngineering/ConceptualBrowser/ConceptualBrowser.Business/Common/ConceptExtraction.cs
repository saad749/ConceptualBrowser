using ConceptualBrowser.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConceptualBrowser.Business.Common
{
    public class ConceptExtraction
    {
        List<Node> nodes = new List<Node>();
        public List<OptimalConcept> Extract(String text)
        {
            Stopwatch sw = new Stopwatch();

            ITextAnalyzer textAnalyzer = new TextAnalyzer();

            List<String> sentencesWithDelimiters = textAnalyzer.GetSentencesWithDelimiters(text);


            for (int i = 0; i < sentencesWithDelimiters.Count;i++)
            {
                int[] ranks = new int[] { i + 1, i + 1};
                int[] totals = { (sentencesWithDelimiters.Count + 2) / 2, (sentencesWithDelimiters.Count + 2) / 2 };
                Rank rank = new Rank(2, ranks, totals);

                nodes.Add(new Node(i + "", -1, i, rank));
            }

            List<String> sentences = textAnalyzer.GetSentences(text);

            Coverage coverage = new Coverage();
            coverage.CreateBinaryRelation(sentences, nodes, false);

            List<OptimalConcept> optimals = coverage.ExtractAll();

            return optimals.OrderByDescending(o => o.Gain).ToList();
        }
    }
}
