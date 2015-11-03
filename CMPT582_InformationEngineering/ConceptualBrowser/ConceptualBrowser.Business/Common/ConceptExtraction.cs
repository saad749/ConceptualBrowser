﻿using ConceptualBrowser.Business.Entities;
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
        public List<OptimalConceptTreeItem> Extract(String text)
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

            List<OptimalConceptTreeItem> optimalTree = CreateTree(optimals.OrderByDescending(o => o.Gain).ToList());

            return optimalTree;
        }

        public List<OptimalConceptTreeItem> CreateTree(List<OptimalConcept> optimals)
        {
            List<OptimalConceptTreeItem> treeItems = new List<OptimalConceptTreeItem>();

            int i = 0;
            int? parentId = null;
            foreach(OptimalConcept optimal in optimals)
            {
                if (i != 0)
                    parentId = (i - 1) / 2;
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
