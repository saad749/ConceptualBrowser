using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class KeywordNode
    {
        /// <summary>
        /// KeywordIndex in the list of Keywords. A number assigned to the word (probably) related to chronological order of appearance.
        /// </summary>
        public int KeywordIndex { get; set; } //Number of Keyword Node
        /// <summary>
        /// Word Stem of the Words this Keyword Represents
        /// </summary>
        public string Keyword { get; set; } // Name of keywordNode
        /// <summary>
        /// Number of times this Keyword appeared in the text.
        /// </summary>
        public double KeywordRank { get; set; }

        //public HashSet<int> SentenceIndexes { get; set; }
        /// <summary>
        /// Sentences in which the word occured
        /// </summary>
        [JsonIgnore]
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();// list of words associated with this KeywordNode

        public KeywordNode(string keyWord, int number, int rank, List<Sentence> sentences)
        {
            Keyword = keyWord;
            KeywordIndex = number;
            KeywordRank = rank;
            Sentences = sentences;
            //SentenceIndexes = new HashSet<int>(sentences.Select(s => s.SentenceIndex).ToList());
        }

        public bool ContainsSentenceIndex(int sentenceIndex)
        {
            //return SentenceIndexes.Contains(sentenceIndex);
            return Sentences.Any(n => n.SentenceIndex == sentenceIndex);
        }
    }
}
