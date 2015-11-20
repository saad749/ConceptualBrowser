using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class Node
    {
        public string Word { get; set; }
        public int CoveredBy { get; set; } = 1;
        public int Number { get; set; }
        public Rank Rank { get; set; }
        public int KeywordNumber { get; set; }
        public string KeywordString { get; set; }


        public Node()
        {

        }

        public Node(string word, int coveredBy, int number, Rank rank)
        {
            Word = word;
            CoveredBy = coveredBy;
            Number = number;
            Rank = rank;
            rank.CalculateRank();
        }
    }
}
