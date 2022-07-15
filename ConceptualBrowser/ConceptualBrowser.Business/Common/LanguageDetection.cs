using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalyst;
using Catalyst.Models;
using Mosaik.Core;

namespace ConceptualBrowser.Business.Common
{
    public static class LanguageDetection
    {
        public static async Task<string> DetectLanguage(string sampleText)
        {
            var cld2LanguageDetector = await LanguageDetector.FromStoreAsync(Language.Any, Mosaik.Core.Version.Latest, "");

            var detectedLanguage = cld2LanguageDetector.Detect(sampleText);

            string language;
            switch (detectedLanguage)
            {
                case Language.Any:
                    language = "eng";
                    break;
                case Language.Arabic:
                    language = "ara";
                    break;
                case Language.Bulgarian:
                    language = "bul";
                    break;
                case Language.Catalan:
                    language = "cat";
                    break;
                case Language.Czech:
                    language = "ces";
                    break;
                case Language.Danish:
                    language = "dan";
                    break;
                case Language.German:
                    language = "deu";
                    break;
                case Language.Greek_Modern:
                    language = "ell";
                    break;
                case Language.English:
                    language = "eng";
                    break;
                case Language.Basque:
                    language = "eus";
                    break;
                case Language.Finnish:
                    language = "fin";
                    break;
                case Language.French:
                    language = "fra";
                    break;
                case Language.Irish:
                    language = "gle";
                    break;
                case Language.Hebrew:
                    language = "heb";
                    break;
                case Language.Hindi:
                    language = "hin";
                    break;
                case Language.Hungarian:
                    language = "hun";
                    break;
                case Language.Armenian:
                    language = "hye";
                    break;
                case Language.Indonesian:
                    language = "ind";
                    break;
                case Language.Italian:
                    language = "ita";
                    break;
                case Language.Lithuanian:
                    language = "lit";
                    break;
                case Language.Malay:
                    language = "msa";
                    break;
                case Language.Nepali:
                    language = "nep";
                    break;
                case Language.Dutch:
                    language = "nld";
                    break;
                case Language.Norwegian:
                    language = "nor";
                    break;
                case Language.Polish:
                    language = "pol";
                    break;
                case Language.Portuguese:
                    language = "por";
                    break;
                case Language.Romanian:
                    language = "ron";
                    break;
                case Language.Russian:
                    language = "rus";
                    break;
                case Language.Slovak:
                    language = "slk";
                    break;
                case Language.Spanish:
                    language = "spa";
                    break;
                case Language.Serbian:
                    language = "srp";
                    break;
                case Language.Swedish:
                    language = "swe";
                    break;
                case Language.Tamil:
                    language = "tam";
                    break;
                case Language.Turkish:
                    language = "tur";
                    break;
                case Language.Ukrainian:
                    language = "ukr";
                    break;
                case Language.Urdu:
                    language = "urd";
                    break;
                case Language.Vietnamese:
                    language = "vie";
                    break;
                case Language.Persian:
                    language = "fas";
                    break;
                default:
                    language = "eng";
                    break;
            }

            return language;
        }
    }
}

