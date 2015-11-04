using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class KeywordNode
    {
        public int Number { get; set; } //Number of Keyword Node
        public string Keyword { get; set; }// name of keywordNode
        public List<Node> Nodes { get; set; } = new List<Node>();// list of words associated with this KeywordNode
        public double KeywordRank { get; set; } // rank or gain of keywordNode

        public KeywordNode(string keyWord, int number, int rank, List<Node> nodes)
        {
            Keyword = keyWord;
            Number = number;
            KeywordRank = rank;
            Nodes = nodes;
        }

        public bool ContainsWord(string word)
        {
            return Nodes.Any(n => n.Word.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
