using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class NodeTree
    {
        public int HeapSize { get; set; }
        public List<List<Node>> Heap = new List<List<Node>>(); //Simple List or a Nested List. i.e. A list ontaining lists of strings.
        public int Huge { get; set; } = 0; //Minimum Accepted Rank
        public int Step { get; set; }
        public bool IsNewElement { get; set; } = true;
        public int Height { get; set; } = 0;
        public int TotalNodes { get; set; } = 0;

        public NodeTree(int heapSize)
        {
            HeapSize = heapSize;
            Heap = new List<List<Node>>();
            InitializeHeap();
        }

        public void InitializeHeap()
        {
            for (int i = 0; i <= HeapSize; i++)
            {
                List<Node> nodes = new List<Node>();
                Heap.Add(nodes); //or Add?

            }
        }

        public void InsertNode(Node node, int step)
        {
            double j = 0;
            if (step != 0)
                j = GetRankAt(Parent(step));
            for (; (step > 0) && (j >= node.Rank.CalRank);)
            {
                if (j == node.Rank.CalRank)
                {
                    List<Node> nodes = new List<Node>();
                    nodes.AddRange((Heap[Parent(step)]));
                    Heap.RemoveAt(Parent(step));
                    nodes.Add(node);
                    Heap[Parent(step)] = nodes;
                    IsNewElement = false;
                    this.TotalNodes++;
                    return;
                }
                else
                {
                    Heap[step] = Heap[Parent(step)];
                    step = Parent(step);
                    if (step != 0)
                        j = GetRankAt(Parent(step));
                }
            }
            List<Node> temp = new List<Node>();
            temp.Add(node);
            Heap[step] =  temp;
            this.TotalNodes++;
            this.Height++;
            IsNewElement = true;
        }

        public double GetRankAt(int pos)
        {
            return Heap[pos][0].Rank.CalRank;

            //List<Node> nodes = new List<Node>();
            //nodes.AddRange(Heap[pos]);
            //Node node = nodes[0];
            //return node.Rank.CalRank;
        }
        public int Parent(int i)
        {
            return i - 1;
        }
    }
}
