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
        public List<KeywordNode> Keywords { get; set; } //Changing Dictionary to List
        public int TotalResults { get; set; }
        public bool Status { get; set; }
        public List<RootNode> Roots { get; set; } = new List<RootNode>();
        public List<string> PrimaryConceptsName { get; set; } = new List<string>();


        public int KeywordsSentencesSum { get; set; }

        //int d = 0; Duplicate coverage statistical counter. Not Required. Just for testing

        private readonly IStemmer Stemmer;

        public int TotalUniqueCovered { get; set; }


        public BinaryRelation(IStemmer stemmer)
        {
            Keywords = new List<KeywordNode>();
            Stemmer = stemmer;
        }

        public BinaryRelation(int total, IStemmer stemmer)
        {
            TotalResults = total;
            Keywords = new List<KeywordNode>();
            Stemmer = stemmer;
        }
        public RootNode GetRootNode(String keyword)
        {
            return Roots.FirstOrDefault(r => r.Root.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public void AppendToBinaryRelation(List<String> words, Sentence sentence)
        {
            sentence.KeywordNodes = new List<KeywordNode>();
            foreach (string word in words)
            {
                //These temporary Variables are SUPER VARIABLES. DONT EVEN THINK TO REMOVE THEM. THIS WILL SKIP A LOT OF
                //CONCEPTS. AND CAN TAKE FOR EVER TO UNDERSTAND!!!
                Sentence tempSentence = new Sentence(sentence.SentenceIndex, sentence.CoveredByConceptNumber, sentence.Rank, sentence.KeywordNodes, sentence.OriginalSentence); // Why to create a tempSentence? -s refers to this variable
                String tempWord = word; //Why again? Why create tempVariables?? -k refers to this variable


                String stem = Stemmer.Stem(tempWord.ToLower());//-k
                RootNode root = new RootNode();
                
                //USE By Reference
                KeywordNode keyword = Keywords.FirstOrDefault(v => v.Keyword == stem);

                if (keyword != null)
                {
                    //If the stram of the word already exists in the List of keywords with the Binary Relations Then 

                    //KeywordNode tempKeywordNode = Keywords.FirstOrDefault(v => v.Keyword == stem);
                    if (!keyword.Sentences.Any( n => n.SentenceIndex == tempSentence.SentenceIndex)) 
                    {
                        //If the word exists in the list of key words, then it checks if the word has any sentences that has a sentenceIndex
                        //that matches the sentence that word was found in (if not its probably repeated in the same sentence and ignored)... THEN
                        //Increase the Rank of the Keyword
                        //For some reason if the word doesnt exists in Roots List of Binary Relation, then Add it to Binary Relation .. Word or Stem??
                        //Add the sentence to the list of the sentences to the Keyword that has the stem of this word

                        keyword.KeywordRank++; 
                        root = Roots.FirstOrDefault(r => r.Root.Equals(stem, StringComparison.OrdinalIgnoreCase));
                        if (!root.ExistsInOriginalWords(tempWord))//-k
                            root.OriginalWords.Add(tempWord);//-k

                        //tempSentence.KeywordNodes.Add(keyword);//Addition for Speed
                        sentence.KeywordNodes.Add(keyword);
                        keyword.Sentences.Add(tempSentence); //Referencing issue may be???//-s

                        
                        
                    }
                }
                else
                {
                    //If the stem of the word doesnt already exists in the Binary Relation List of Keywords Then
                    //It creates a Keyword Node and adds it to its list. 
                    //It also creates a Root node and add it to list
                    List<Sentence> sentences = new List<Sentence>();
                    sentences.Add(tempSentence);//-s
                    List<string> orginalWords = new List<string>();
                    orginalWords.Add(tempWord);//-k

                    root = new RootNode(stem, orginalWords);
                    Roots.Add(root);

                    KeywordNode temp = new KeywordNode(stem, Keywords.Count, 1, sentences);

                    //tempSentence.KeywordNodes.Add(temp);//Addition for Speed - By Reference!!
                    sentence.KeywordNodes.Add(temp);
                    Keywords.Add(temp);
                }
            }
        }

        public int GetTupleCount()
        {
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
                if (keyword.Sentences.FirstOrDefault(n => n.SentenceIndex == pair[1]).CoveredByConceptNumber == -1) //THis Part is important as it is used for Randomizations!
                {
                    TotalUniqueCovered++;
                    //Console.WriteLine("Unique Cover" + TotalUniqueCovered);
                }

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
