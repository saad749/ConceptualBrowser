using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTextCat;

namespace ConceptualBrowser.Business.Common
{
    public class LanguageDetection
    {
        public string DetectLanguage(string sampleText)
        {
            var factory = new RankedLanguageIdentifierFactory();
            var identifier = factory.Load("Assets/Core14.profile.xml");
            var languages = identifier.Identify(sampleText);
            var mostCertainLanguage = languages.FirstOrDefault();

            if (mostCertainLanguage != null)
            {
                return mostCertainLanguage.Item1.Iso639_3;
            }

            Console.WriteLine("The language couldn’t be identified with an acceptable degree of certainty");

            return "N/A";
        }
    }
}
