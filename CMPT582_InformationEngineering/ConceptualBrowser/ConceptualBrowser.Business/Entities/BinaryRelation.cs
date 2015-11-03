using ConceptualBrowser.Business.Common.Stemmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class BinaryRelation
    {
        public Dictionary<int, KeywordNode> Keywords { get; set; }

        public int TotalResults { get; set; }
        public bool Status { get; set; }
        public List<RootNode> Roots { get; set; } = new List<RootNode>();
        public int MaxRank { get; set; }
        public List<string> PrimaryConceptsName { get; set; } = new List<string>();


        public BinaryRelation()
        {
            Keywords = new Dictionary<int, KeywordNode>();
        }

        public BinaryRelation(int total, int max)
        {
            TotalResults = total;
            Keywords = new Dictionary<int, KeywordNode>();
        }
        public RootNode GetRootNode(String keyword)
        {
            return Roots.Where(r => r.Root.Equals(keyword, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public void AppendToBinaryRelation(List<String> words, Node node, bool rankedDocs)
        {

            IStemmer stemmer = new EnglishStemmer();

            for (int i = 0; i < words.Count; i++)
            {
                Node tempNode = new Node(node.Word, node.CoveredBy, node.Number, node.Rank);

                String keyword = words[i];

                String stem = stemmer.Stem(keyword.ToLower());

                RootNode root = new RootNode();

                KeywordNode tempKeywordNode;

                if (Keywords.Values.Any(v => v.Keyword == stem))// Keywords.ContainsKey((stem)))
                {
                    tempKeywordNode = Keywords.Values.Where(v => v.Keyword == stem).FirstOrDefault();
                    //if (!keyWord_HashTable.get(keyWord_HashTable.getPosition()).containsURL(node.getURL()))
                    if (!tempKeywordNode.Nodes.Any( n => n.Word == node.Word)) //.containsURL(node.getURL()))
                    {
                        if (!rankedDocs)
                        {
                            //keyWord_HashTable.get(keyWord_HashTable.getPosition()).setKeywordRank(keyWord_HashTable.get(keyWord_HashTable.getPosition()).getKeywordRank() + 1);
                            tempKeywordNode.KeywordRank++;
                        }

                        root = Roots.Where(r => r.Root.Equals(stem, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                        if (!root.ExistsInOriginalWords(keyword))
                            root.OriginalWords.Add(keyword);
                        tempKeywordNode.Nodes.Add(tempNode); //Referencing issue may be???
                    }

                }
                else
                {
                    List<Node> nodes = new List<Node>();
                    nodes.Add(tempNode);
                    List<string> orginalWords = new List<string>();
                    orginalWords.Add(keyword);

                    root = new RootNode(stem, orginalWords);

                    Roots.Add(root);

                    KeywordNode temp = new KeywordNode(stem, Keywords.Count, 1, nodes);

                    Keywords.Add(Keywords.Count, temp);
                }
            }
        }

        public int GetTupleCount()
        {
            return Keywords.Values.ToList().Sum(k => k.Nodes.Count);
        }

        public void MarkAsCovered(List<int[]> tuples, int current)
        {
            for (int i = 0; i < tuples.Count; i++)
            {
                int[] pair = tuples[i];
                KeywordNode keyword = Keywords[pair[0]];
                //((URLNode)keyword.getURLNodeHasNo(pair[1])).setCoveredBy(current);
                keyword.Nodes.Where(n => n.Number == pair[1]).FirstOrDefault().CoveredBy = current;
            }
        }

        public bool InPrimaryConceptsName(string name)
        {
            return PrimaryConceptsName.Any(p => p.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        //	 set all RootNodes status as false, not covered
        public void ResetRootNodes()
        {
            Roots.ForEach(r => r.Covered = false);
        }

    }
}
