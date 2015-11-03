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
        public List<int> Nodes { get; set; } = new List<int>();

        public EquivalentNode()
        {

        }
        public EquivalentNode(int index, List<int> nodes)
        {
            Index = index;
            Nodes = nodes;
        }

        public bool InNode(int index)
        {
            return Nodes.Contains(index);
        }

        public EquivalentNode Clone()
        {
            EquivalentNode equivalentNode = new EquivalentNode();
            equivalentNode.Index = Index;
            //equivalentNode.Nodes = new List<int>(Nodes.Count);
            for (int i = 0; i < Nodes.Count; i++)
                equivalentNode.Nodes.Add(Nodes[i]);
            return equivalentNode;
        }
    }
}
