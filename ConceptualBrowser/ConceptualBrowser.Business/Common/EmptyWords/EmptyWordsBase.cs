using Iso639;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConceptualBrowser.Business.Common.EmptyWords
{
    internal abstract class EmptyWordsBase : IEmptyWords
    {
        public Language Language { get; set; }
        public string EmptyWordsList { get; set; }
        public List<string> EmptyWords
        {
            get
            {
                return EmptyWordsList.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            private set { }
        }

        public EmptyWordsBase(string language_iso639_3)
        {
            Language = Language.FromPart3(language_iso639_3);
        }
        public bool IsEmptyWord(string word)
        {
            return EmptyWords.Any(w => String.Equals(word.ToLower(Language.Culture), w.ToLower(Language.Culture)));
        }

        public string RemoveStopWords(string text)
        {
            text = text.Split(' ').Where(x => !EmptyWords.Contains(x)).DefaultIfEmpty().Aggregate((current, next) => current + " " + next);

            return text ?? String.Empty;
        }

    }
}
