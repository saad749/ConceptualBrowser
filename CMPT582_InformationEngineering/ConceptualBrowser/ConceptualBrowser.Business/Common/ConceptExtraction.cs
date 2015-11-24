using ConceptualBrowser.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ConceptualBrowser.Business.Common.Stemmer;


namespace ConceptualBrowser.Business.Common
{
    public class ConceptExtraction
    {
        List<Node> nodes = new List<Node>();
        public List<OptimalConceptTreeItem> Extract(String text, string languageCode)
        {
            IStemmer stemmer = Stemmers.GetStemmer(languageCode);
            IEmptyWords emptyWords = new EmptyWords(languageCode);
            ITextAnalyzer textAnalyzer = new TextAnalyzer(stemmer, emptyWords);


            List<String> sentencesWithDelimiters = textAnalyzer.GetSentencesWithDelimiters(text);
            List<String> sentences = textAnalyzer.GetSentences(text);

            for (int i = 0; i < sentencesWithDelimiters.Count;i++)
            {
                int[] ranks = new int[] { i + 1, i + 1};
                int[] totals = { (sentencesWithDelimiters.Count + 2) / 2, (sentencesWithDelimiters.Count + 2) / 2 };
                Rank rank = new Rank(2, ranks, totals);

                nodes.Add(new Node(i + "", -1, i, rank));
            }

            Coverage coverage = new Coverage(stemmer, emptyWords);
            coverage.CreateBinaryRelation(sentences, nodes, false);

            List<OptimalConcept> optimals = coverage.ExtractAll();

            List<OptimalConceptTreeItem> optimalTree = CreateTree(optimals.OrderByDescending(o => o.Gain).ToList());

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

        

        
    }
}
