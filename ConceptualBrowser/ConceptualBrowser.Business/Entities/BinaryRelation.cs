using ConceptualBrowser.Business.Common.Stemmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConceptualBrowser.Business.Common.Helpers;
using ConceptualBrowser.Business.Common;

namespace ConceptualBrowser.Business.Entities
{
    public class BinaryRelation
    {
        public List<KeywordNode> Keywords { get; set; } //Changing Dictionary to List
        public int TotalResults { get; set; }
        public List<RootNode> Roots { get; set; } = new List<RootNode>();
        public List<string> PrimaryConceptsName { get; set; } = new List<string>();
        public List<Sentence> Sentences { get; set; }
        public int TotalSentences { get; set; }
        public int MaxRank { get; set; } = 2; //2 because the value can never be mroe than 1 //C# 6.0 allows property initializers// WHY 200?????
        public int KeywordsSentencesSum { get; set; }
        public int TotalUniqueCovered { get; set; }
        public TextAnalyzer TextAnalyzer { get; set; }

        public BinaryRelation(string languageCode, string text)
        {
            Keywords = new List<KeywordNode>();

            TextAnalyzer = new TextAnalyzer(languageCode);
            List<String> sentenceList = TextAnalyzer.GetSentences(TextAnalyzer.RemoveDiacritics(text));
            CreateBinaryRelation(sentenceList);
        }

        public void CreateBinaryRelation(List<string> sentenceStringList)
        {
            int tempTotalWords = 0;
            Sentences = new List<Sentence>();
            for (int i = 0; i < sentenceStringList.Count; i++)
            {
                int[] ranks = new int[] { i + 1, i + 1 };
                int[] totals = new int[] { (sentenceStringList.Count + 2) / 2, (sentenceStringList.Count + 2) / 2 };
                Rank rank = new Rank(2, ranks, totals);

                Sentences.Add(new Sentence(i, Constant.NotCovered, rank, sentenceStringList[i]));
            }

            TotalSentences = Sentences.Count;
            for (int i = 0; i < sentenceStringList.Count; i++)
            {
                List<string> wordsList = TextAnalyzer.Tokenizer(sentenceStringList[i]);
                //Word2Vec -- wordsList can be shortened here by using word2vec.
                tempTotalWords += wordsList.Count;
                AppendToBinaryRelation(wordsList, Sentences[i]);
            }

            Console.WriteLine("Total Words: " + tempTotalWords);

            this.KeywordsRank();
            this.AddHighestRankKeywords();

            KeywordsSentencesSum = Keywords.SelectMany(s => s.Sentences).Count();
            Console.WriteLine("KeywordsSentencesSum: " + KeywordsSentencesSum);
        }

        public void KeywordsRank()
        {
            List<KeywordNode> keywords = new List<KeywordNode>();
            //keywords = BinaryRelation.Keywords.Values.ToList();
            keywords = Keywords.ToList();

            for (int i = 0; i < keywords.Count; i++)
            {
                KeywordNode key = keywords[i];
                key.KeywordRank = (1 / key.KeywordRank); //This changes the keyword rank in the BinaryRelation Keywords List.
            }
        }

        private void AddHighestRankKeywords()
        {
            double max = MaxRank;
            int KeywordNo = -1;
            String keywordString = null;

            for (int i = 0; i < Sentences.Count; i++)
            {
                Sentence sentence = Sentences[i];
                for (int j = 0; j < sentence.KeywordNodes.Count; j++)
                {
                    KeywordNode keywordNode = sentence.KeywordNodes[j];

                    if (keywordNode.KeywordRank < max)
                    {
                        max = keywordNode.KeywordRank;
                        KeywordNo = keywordNode.KeywordIndex;
                        keywordString = keywordNode.Keyword;
                    }
                }
                if (keywordString == null)
                {
                    max = MaxRank;
                    KeywordNo = -1;
                    keywordString = null;
                    i--;
                }
                else
                {
                    sentence.KeywordNumber = KeywordNo;
                    String key = (GetRootNode(keywordString)).getMaxLengthWord();
                    sentence.KeywordString = key;
                    max = MaxRank;
                }
            }
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
                Sentence tempSentence = new Sentence(sentence.SentenceIndex, sentence.LastCoveredByConceptNumber, sentence.Rank, sentence.KeywordNodes, sentence.OriginalSentence); // Why to create a tempSentence? -s refers to this variable
                String tempWord = word; //Why again? Why create tempVariables?? -k refers to this variable


                String stem = TextAnalyzer.Stem(tempWord.ToLower());//-k
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

        /// <summary>
        /// Returns Count of All Sentences in the Binary Relation
        /// </summary>
        /// <returns></returns>
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
                if (keyword.Sentences.FirstOrDefault(n => n.SentenceIndex == pair[1]).LastCoveredByConceptNumber == -1) //THis Part is important as it is used for Randomizations!
                {
                    TotalUniqueCovered++;
                    //Console.WriteLine("Unique Cover" + TotalUniqueCovered);
                }

                keyword.Sentences.FirstOrDefault(n => n.SentenceIndex == pair[1]).LastCoveredByConceptNumber = current;
                keyword.Sentences.FirstOrDefault(n => n.SentenceIndex == pair[1]).CovertedbyConceptNumbers.Add(current); //Added to keep track of concept numbers
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
