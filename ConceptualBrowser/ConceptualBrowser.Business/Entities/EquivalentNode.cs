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
        public List<int> Indexes { get; set; } = new List<int>();
        

        public EquivalentNode()
        {

        }
        public EquivalentNode(int index, List<int> indexes)
        {
            Index = index;
            Indexes = indexes;
        }

        public bool InNode(int index)
        {
            return Indexes.Contains(index);
        }

        public EquivalentNode Clone()
        {
            EquivalentNode equivalentNode = new EquivalentNode { Index = Index };
            //equivalentNode.Nodes = new List<int>(Nodes.Count);
            for (int i = 0; i < Indexes.Count; i++)
                equivalentNode.Indexes.Add(Indexes[i]);
            return equivalentNode;
        }
    }
}
