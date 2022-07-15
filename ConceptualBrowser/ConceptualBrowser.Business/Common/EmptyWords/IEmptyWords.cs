using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.EmptyWords
{
    public interface IEmptyWords
    {
        bool IsEmptyWord(string word);
        string RemoveStopWords(string text);
    }


}
