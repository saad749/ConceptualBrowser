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
            Console.WriteLine("Keyword - Value: " + keywordNode.Keyword + "\t" + keywordNode.Number + "\t" + keywordNode.KeywordRank);
            PrintNodes(keywordNode.Nodes);
        }
        public static void PrintNodes(List<Node> nodes, string additionalMessage = "")
        {
            Console.Write(additionalMessage);
            Console.WriteLine("Nodes: ");
            foreach (Node node in nodes)
            {
                PrintNode(node);
            }
        }
        public static void PrintNode(Node node, string additionalMessage = "")
        {
            Console.Write(additionalMessage);
            Console.WriteLine("Word: " + node.Word + "\tNumber: " + node.Number + "\tCoveredBy: " + node.CoveredBy
                    + "\tKeywordNumber: " + node.KeywordNumber + "\tKeywordString: " + node.KeywordString);
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
