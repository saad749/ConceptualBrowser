using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class RootNode
    {
        public bool Covered { get; set; } = false;

        public string Root { get; set; }

        public List<string> OriginalWords { get; set; } = new List<string>();

        public RootNode(String root, List<string> originalWords)
        {
            Root = root;
            OriginalWords.AddRange(originalWords);
        }

        public RootNode()
        {
            OriginalWords = new List<string>();
        }

        // get the keyword String that has the highest length from the list of original_words 
        public string getMaxLengthWord()
        {
            return OriginalWords.OrderByDescending(w => w.Length).FirstOrDefault();
        }

        public bool ExistsInOriginalWords(string keyword)
        {
            return OriginalWords.Exists(w => w.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        }

        

    }
}
