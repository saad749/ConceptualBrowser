using ConceptualBrowser.Business.Common.Stemmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConceptualBrowser.Business.Common.Helpers;

namespace ConceptualBrowser.Business.Entities
{
    public class BinaryRelation
    {
        //public Dictionary<int, KeywordNode> Keywords { get; set; }// Why is it a Dictionary? Any other possibility?
        public List<KeywordNode> Keywords { get; set; } //Changing Dictionary to List

        public int TotalResults { get; set; }
        public bool Status { get; set; }
        public List<RootNode> Roots { get; set; } = new List<RootNode>();
        //public int MaxRank { get; set; } //Not Used
        public List<string> PrimaryConceptsName { get; set; } = new List<string>();

        private readonly IStemmer Stemmer;

        public BinaryRelation(IStemmer stemmer)
        {
            //Keywords = new Dictionary<int, KeywordNode>();
            Keywords = new List<KeywordNode>();
            Stemmer = stemmer;
        }

        public BinaryRelation(int total, IStemmer stemmer)//, int max)
        {
            TotalResults = total;
            //Keywords = new Dictionary<int, KeywordNode>();
            Keywords = new List<KeywordNode>();
            Stemmer = stemmer;
        }
        public RootNode GetRootNode(String keyword)
        {
            return Roots.FirstOrDefault(r => r.Root.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public void AppendToBinaryRelation(List<String> words, Sentence sentence)
        {
            //IStemmer stemmer = new EnglishStemmer();

            foreach (string word in words)
            {
                //These temporary Variables are SUPER VARIABLES. DONT EVEN THINK TO REMOVE THEM. THIS WILL SKIP A LOT OF
                //CONCEPTS. AND CAN TAKE FOR EVER TO UNDERSTAND!!!
                Sentence tempSentence = new Sentence(sentence.SentenceIndex, sentence.CoveredByConceptNumber, sentence.Rank); // Why to create a tempSentence?
                String keyword = word; //Why again? Why create tempVariables??


                String stem = Stemmer.Stem(keyword.ToLower());//-k
                RootNode root = new RootNode();

                //if (Keywords.Values.Any(v => v.Keyword == stem))
                if (Keywords.Any(v => v.Keyword == stem))
                {
                    //KeywordNode tempKeywordNode = Keywords.Values.FirstOrDefault(v => v.Keyword == stem);
                    //KeywordNode tempKeywordNode = Keywords.FirstOrDefault(v => v.Keyword == stem); //This was probably causing an unreferenced issue
                    if (!Keywords.FirstOrDefault(v => v.Keyword == stem).Sentences.Any( n => n.SentenceIndex == tempSentence.SentenceIndex)) 
                    {
                        Keywords.FirstOrDefault(v => v.Keyword == stem).KeywordRank++;
                        root = Roots.FirstOrDefault(r => r.Root.Equals(stem, StringComparison.OrdinalIgnoreCase));
                        if (!root.ExistsInOriginalWords(keyword))//-k
                            root.OriginalWords.Add(keyword);//-k
                        Keywords.FirstOrDefault(v => v.Keyword == stem).Sentences.Add(tempSentence); //Referencing issue may be???//-s
                    }
                }
                else
                {
                    List<Sentence> sentences = new List<Sentence>();
                    sentences.Add(tempSentence);//-s
                    List<string> orginalWords = new List<string>();
                    orginalWords.Add(keyword);//-k

                    root = new RootNode(stem, orginalWords);
                    Roots.Add(root);

                    KeywordNode temp = new KeywordNode(stem, Keywords.Count, 1, sentences);
                    //Keywords.Add(Keywords.Count, temp);
                    Keywords.Add(temp);
                }
            }
        }

        public int GetTupleCount()
        {
            //return Keywords.Values.ToList().Sum(k => k.Nodes.Count);
            return Keywords.ToList().Sum(k => k.Sentences.Count);
        }

        public void MarkAsCovered(List<int[]> tuples, int current)
        {
            //LogHelper.PrintListArray(tuples, "MarkAsCovered -- List Array - ");
            for (int i = 0; i < tuples.Count; i++)
            {
                int[] pair = tuples[i];
                KeywordNode keyword = Keywords[pair[0]];
                //LogHelper.PrintKeyword(keyword, "Marked As Covered: ");
                keyword.Sentences.FirstOrDefault(n => n.SentenceIndex == pair[1]).CoveredByConceptNumber = current;
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
