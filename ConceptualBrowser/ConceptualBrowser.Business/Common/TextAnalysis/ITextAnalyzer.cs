using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business
{
    public interface ITextAnalyzer
    {
        List<String> GetSentencesWithDelimiters(string text);
        List<String> GetSentences(string text);
        List<string> Tokenizer(string sentence);
        string RemoveDiacritics(string InputStr);
    }
}
