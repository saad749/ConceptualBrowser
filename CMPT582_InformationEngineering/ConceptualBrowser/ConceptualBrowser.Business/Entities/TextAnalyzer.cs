using ConceptualBrowser.Business.Common;
using ConceptualBrowser.Business.Common.Stemmer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    class TextAnalyzer : ITextAnalyzer
    {
        public IStemmer Stemmer { get; set; }
        public IEmptyWords EmptyWords { get; set; }

        public TextAnalyzer(IStemmer stemmer, IEmptyWords emptywords)
        {
            Stemmer = stemmer;
            EmptyWords = emptywords;
        }
        public List<String> GetSentences(string text)
        {
            List<String> sentences = new List<string>();
            char[] delimiters = new char[] { '.', ',', ';' };

            sentences = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList<String>();

            return sentences;
        }

        public List<String> GetSentencesWithDelimiters(string text)
        {
            List<String> sentences = new List<string>();
            char[] delimiters = new char[] { '.', ',', ';' };

            string pattern = @"(\.|,|;)";

            sentences = Regex.Split(text, pattern).Where(s => !String.IsNullOrWhiteSpace(s)).ToList<String>();

            return sentences;
        }

        public List<string> Tokenizer(string sentence)
        {
            List<string> keywords = new List<string>();
            char[] splitChars = 
                new char[] { ',', ' ', '\n', '\t', '©', '-', '<', '>', '/', '\\', '.', '(', ')', '?', '@', '^', '#', '%', '&', '*', '$', '!', ';', ':', '\"', '{', '}', '~', '\'', '[',']', '“' };
            string[] tokens = sentence.Split(splitChars);
            foreach (String word in tokens)
            {
                if(word.Length > 1)
                {
                    String root = Stemmer.Stem(word);
                    if (!EmptyWords.IsEmptyWord(root) && !EmptyWords.IsEmptyWord(word))
                        keywords.Add(word);
                }
            }
            return keywords;
        }




    }
}
