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
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();// list of words associated with this KeywordNode
        public double KeywordRank { get; set; } // rank or gain of keywordNode

        public KeywordNode(string keyWord, int number, int rank, List<Sentence> sentences)
        {
            Keyword = keyWord;
            Number = number;
            KeywordRank = rank;
            Sentences = sentences;
        }

        public bool ContainsSentenceIndex(int sentenceIndex)
        {
            return Sentences.Any(n => n.SentenceIndex == sentenceIndex);
        }
    }
}
