using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class Sentence
    {
        public int SentenceIndex { get; set; } //Renamed from Word to SentenceIndex
        public int CoveredByConceptNumber { get; set; } = -1; // A signle sentence can be covered by multiple concepts; Why 1 only??
        public Rank Rank { get; set; }
        public int KeywordNumber { get; set; }
        public string KeywordString { get; set; }

        public List<KeywordNode> KeywordNodes { get; set; }



        public Sentence()
        {

        }

        public Sentence(int sentenceIndex, int coveredByConceptNumber, Rank rank)
        {
            SentenceIndex = sentenceIndex;
            CoveredByConceptNumber = coveredByConceptNumber;
            Rank = rank;
            rank.CalculateRank();
        }

        /// <summary>
        /// Constructor to Allow cross referencing the keywords that occur in this sentence
        /// </summary>
        /// <param name="sentenceIndex"></param>
        /// <param name="coveredByConceptNumber"></param>
        /// <param name="rank"></param>
        /// <param name="keywords"></param>
        public Sentence(int sentenceIndex, int coveredByConceptNumber, Rank rank, List<KeywordNode> keywords)
        {
            SentenceIndex = sentenceIndex;
            CoveredByConceptNumber = coveredByConceptNumber;
            Rank = rank;
            rank.CalculateRank();
            KeywordNodes = keywords;
        }
    }
}
