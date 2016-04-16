using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConceptualBrowser.Business.Entities;

namespace ConceptualBrowser.Business.Common.Helpers
{
    public static class LogHelper
    {
        public static void PrintRootNodes(List<RootNode> roots, string additionalMessage = "")
        {
            Console.Write(additionalMessage);
            foreach (RootNode rootNode in roots)
            {
                PrintRootNode(rootNode);
            }
            Console.WriteLine();
        }
        public static void PrintRootNode(RootNode rootNode, string additionalMessage = "")
        {
            Console.Write(additionalMessage);
            Console.WriteLine("Root: " + rootNode.Root);
            Console.WriteLine("Covered: " + rootNode.Covered);
            Console.Write("Original Words: \t");
            foreach (string originalWord in rootNode.OriginalWords)
            {
                Console.Write(originalWord + " ");
            }
            Console.WriteLine();

        }
        public static void PrintKeywords(List<KeywordNode> keywordNodes, string additionalMessage = "")
        {
            Console.Write(additionalMessage);
            Console.WriteLine("Keywords: ");
            foreach (KeywordNode keywordNode in keywordNodes)
            {
                PrintKeyword(keywordNode);
                Console.WriteLine();
            }
        }
        public static void PrintKeyword(KeywordNode keywordNode, string additionalMessage = "")
        {
            Console.Write(additionalMessage);
            Console.WriteLine("Keyword -  Value: " + keywordNode.Keyword + "\t" + keywordNode.Number + "\t" + keywordNode.KeywordRank);
            PrintSentences(keywordNode.Sentences);
        }
        public static void PrintSentences(List<Sentence> sentences, string additionalMessage = "")
        {
            Console.Write(additionalMessage);
            Console.WriteLine("Sentences: ");
            foreach (Sentence node in sentences)
            {
                PrintSentence(node);
            }
        }
        public static void PrintSentence(Sentence sentence, string additionalMessage = "")
        {
            Console.Write(additionalMessage);
            Console.WriteLine("Sentence - Word: " + sentence.SentenceIndex + "\tNumber: " + sentence.SentenceIndex 
                    + "\tCoveredBy: " + sentence.CoveredByConceptNumber
                    + "\tKeywordNumber: " + sentence.KeywordNumber + "\tKeywordString: " + sentence.KeywordString);
        }

        public static void PrintListArray(List<int[]> listArray, string additionalMessage = "")
        {
            Console.WriteLine(additionalMessage);
            foreach (int[] ints in listArray)
            {
                foreach (int i in ints)
                {
                    Console.Write(i + " | ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("End!");
        }
    }
}
