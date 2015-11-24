using ConceptualBrowser.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ConceptualBrowser.Business.Common.Stemmer;
using NTextCat;

namespace ConceptualBrowser.Business.Common
{
    public class ConceptExtraction
    {
        List<Node> nodes = new List<Node>();
        public List<OptimalConceptTreeItem> Extract(String text)
        {
            Stopwatch sw = new Stopwatch();
            int sampleStringLength = text.Length > 50 ? 50 : text.Length;
            string textSample = text.Substring(0, sampleStringLength);

            string languageCode = DetectLanguage(textSample);
           

            //IStemmer stemmer = new EnglishStemmer();
            //IEmptyWords emptyWords = new EmptyWords();
            IStemmer stemmer = GetStemmer(languageCode);
            IEmptyWords emptyWords = new EmptyWordsFrench();

            ITextAnalyzer textAnalyzer = new TextAnalyzer(stemmer, emptyWords);

            List<String> sentencesWithDelimiters = textAnalyzer.GetSentencesWithDelimiters(text);


            for (int i = 0; i < sentencesWithDelimiters.Count;i++)
            {
                int[] ranks = new int[] { i + 1, i + 1};
                int[] totals = { (sentencesWithDelimiters.Count + 2) / 2, (sentencesWithDelimiters.Count + 2) / 2 };
                Rank rank = new Rank(2, ranks, totals);

                nodes.Add(new Node(i + "", -1, i, rank));
            }

            List<String> sentences = textAnalyzer.GetSentences(text);

            Coverage coverage = new Coverage(stemmer, emptyWords);
            coverage.CreateBinaryRelation(sentences, nodes, false);

            List<OptimalConcept> optimals = coverage.ExtractAll();

            List<OptimalConceptTreeItem> optimalTree = CreateTree(optimals.OrderByDescending(o => o.Gain).ToList());

            return optimalTree;
        }

        public List<OptimalConceptTreeItem> CreateTree(List<OptimalConcept> optimals)
        {
            List<OptimalConceptTreeItem> treeItems = new List<OptimalConceptTreeItem>();

            int i = 1;
            int? parentId = null;
            foreach(OptimalConcept optimal in optimals)
            {
                if (i != 0)
                {
                    parentId = ((i + 1) - 1) / 2;
                }
                treeItems.Add(new OptimalConceptTreeItem()
                {
                    Id = i,
                    ParentId = parentId,
                    OptimalConcept = optimal
                });
                i++;
            }

            return treeItems;
        }

        private string DetectLanguage(string sampleText)
        {
            var factory = new RankedLanguageIdentifierFactory();
            var identifier = factory.Load("Core14.profile.xml");
            var languages = identifier.Identify(sampleText);
            var mostCertainLanguage = languages.FirstOrDefault();

            if (mostCertainLanguage != null)
            {
                return mostCertainLanguage.Item1.Iso639_3;
            }

            Console.WriteLine("The language couldn’t be identified with an acceptable degree of certainty");

            return "N/A";
        }

        private IStemmer GetStemmer(string languageCode)
        {
            switch (languageCode)
            {
                case "eng":
                    return new EnglishStemmer();
                case "fra":
                    return new FrenchStemmer();
                case "spa":
                    return new SpanishStemmer();
                case "ces":
                    return new CzechStemmer();
                case "dan":
                    return new DanishStemmer();
                case "nld":
                    return new DutchStemmer();
                case "fin":
                    return new FinnishStemmer();
                case "deu":
                    return new GermanStemmer();
                case "hun":
                    return new HungarianStemmer();
                case "ita":
                    return new ItalianStemmer();
                case "nor":
                    return new NorwegianStemmer();
                case "por":
                    return new PortugueseStemmer();
                case "ron":
                    return  new RomanianStemmer();
                case "rus":
                    return new RussianStemmer();
                default:
                    return null;

            }
        }
    }
}
