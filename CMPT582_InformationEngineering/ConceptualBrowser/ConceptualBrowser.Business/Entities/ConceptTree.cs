using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class ConceptTree
    {
        public int HeapSize { get; set; }
        public int[] Heap { get; set; } //heap of concept Nos
        public int Huge { get; set; } = 10000;
        public int step { get; set; } = 0;

        public ConceptTree(int heapsize)
        {
            HeapSize = heapsize;
            Heap = new int[heapsize];
            InitializeHeap();
        }

        // Initialize heap[]
        public void InitializeHeap()
        {
            for (int i = 0; i < HeapSize; i++)
                Heap[i] = Huge;
        }

        // Insert an element to the heap.
        public void InsertElement(int n, int step, Coverage coverage)
        {
            for (; (step > 0) && (coverage.GetConcept(Heap[Parent(step)]).Gain < coverage.GetConcept(n).Gain);)
            {
                if ((coverage.GetConcept(Heap[Parent(step)]).Gain == coverage.GetConcept(n).Gain) 
                    && ((coverage.GetConcept(Heap[Parent(step)]).NodesTree.TotalNodes) < (coverage.GetConcept(n).NodesTree.TotalNodes)))
                {
                    Heap[step] = Heap[Parent(step)];
                    step = Parent(step);
                }
            }
            Heap[step] = n;
        }

        // return parent of i
        public int Parent(int i)
        {
            return (i - 1) / 2;
        }
    }
}
