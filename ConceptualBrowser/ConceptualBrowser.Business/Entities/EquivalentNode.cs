using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class EquivalentNode
    {
        public int Index { get; set; }
        public List<int> SentenceIndexes { get; set; } = new List<int>();

        public EquivalentNode()
        {

        }
        public EquivalentNode(int index, List<int> nodes)
        {
            Index = index;
            SentenceIndexes = nodes;
        }

        public bool InNode(int index)
        {
            return SentenceIndexes.Contains(index);
        }

        public EquivalentNode Clone()
        {
            EquivalentNode equivalentNode = new EquivalentNode { Index = Index };
            //equivalentNode.Nodes = new List<int>(Nodes.Count);
            for (int i = 0; i < SentenceIndexes.Count; i++)
                equivalentNode.SentenceIndexes.Add(SentenceIndexes[i]);
            return equivalentNode;
        }
    }
}
