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
        public List<List<Sentence>> Heap = new List<List<Sentence>>(); //Simple List or a Nested List. i.e. A list ontaining lists of strings.
        public int Huge { get; set; } = 0; //Minimum Accepted Rank
        public int Step { get; set; }
        public bool IsNewElement { get; set; } = true;
        public int Height { get; set; } = 0;
        public int TotalNodes { get; set; } = 0;

        public NodeTree(int heapSize)
        {
            HeapSize = heapSize;
            Heap = new List<List<Sentence>>();
            InitializeHeap();
        }

        public void InitializeHeap()
        {
            for (int i = 0; i <= HeapSize; i++)
            {
                List<Sentence> sentences = new List<Sentence>();
                Heap.Add(sentences); //or Add?

            }
        }

        public void InsertNode(Sentence sentence, int step)
        {
            double j = 0;
            if (step != 0)
                j = GetRankAt(Parent(step));
            for (; (step > 0) && (j >= sentence.Rank.CalRank);)
            {
                if (j == sentence.Rank.CalRank)
                {
                    List<Sentence> sentences = new List<Sentence>();
                    sentences.AddRange((Heap[Parent(step)]));
                    Heap.RemoveAt(Parent(step));
                    sentences.Add(sentence);
                    Heap[Parent(step)] = sentences;
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
            List<Sentence> tempSentences = new List<Sentence>();
            tempSentences.Add(sentence);
            Heap[step] =  tempSentences;
            this.TotalNodes++;
            this.Height++;
            IsNewElement = true;
        }

        public double GetRankAt(int pos)
        {
            return Heap[pos][0].Rank.CalRank;
        }
        public int Parent(int i)
        {
            return i - 1;
        }
    }
}
