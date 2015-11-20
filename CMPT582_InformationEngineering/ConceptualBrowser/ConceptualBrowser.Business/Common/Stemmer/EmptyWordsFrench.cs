﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public class EmptyWordsFrench : IEmptyWords
    {
        public List<string> EmptyWordRoots = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8",
            "9", "0", ",", "alors","au","aucuns","aussi","autre","avant","avec","avoir","bon","car","ce","cela",
            "ces","ceux","chaque","ci","comme","comment","dans","des","du","dedans","dehors","depuis","devrait","doit",
            "donc","dos","début","elle","elles","en","encore","essai","est","et","eu","fait","faites","fois","font","hors",
            "ici","il","ils","je","juste","la","le","les","leur","là","ma","maintenant","mais","mes","mine","moins","mon","mot",
            "même","ni","nommés","notre","nous","ou","où","par","parce","pas","peut","peu","plupart","pour","pourquoi","quand","que",
            "quel","quelle","quelles","quels","qui","sa","sans","ses","seulement","si","sien","son","sont","sous","soyez","sujet","sur",
            "ta","tandis","tellement","tels","tes","ton","tous","tout","trop","très","tu","voient","vont","votre","vous","vu","ça","étaient",
            "état","étions","été","être","a","à","â","abord","afin","ah","ai","aie","ainsi","allaient","allo","allô","allons","après","assez",
            "attendu","aucun","aucune","aujourd","aujourd'hui","auquel","aura","auront","autres","aux","auxquelles","auxquels","avaient","avais",
            "avait","ayant","b","bah","beaucoup","bien","bigre","boum","bravo","brrr","c","ceci","celle","celle-ci","celle-là","celles",
            "celles-ci","celles-là","celui","celui-ci","celui-là","cent","cependant","certain","certaine","certaines","certains",
            "certes","cet","cette","ceux-ci","ceux-là","chacun","cher","chère","chères","chers","chez","chiche","chut","cinq",
            "cinquantaine","cinquante","cinquantième","cinquième","clac","clic","combien","compris","concernant","contre",
            "couic","crac","d","da","de","debout","delà","derrière","dès","désormais","desquelles","desquels","dessous",
            "dessus","deux","deuxième","deuxièmement","devant","devers","devra","différent","différente","différentes",
            "différents","dire","divers","diverse","diverses","dix","dix-huit","dixième","dix-neuf","dix-sept",
            "doivent","dont","douze","douzième","dring","duquel","durant","e","effet","eh","elle-même",
            "elles-mêmes","entre","envers","environ","es","ès","etant","étais","était","étant","etc",
            "etre","euh","eux","eux-mêmes","excepté","f","façon","fais","faisaient","faisant","feront",
            "fi","flac","floc","g","gens","h","ha","hé","hein","hélas","hem","hep","hi","ho","holà","hop",
            "hormis","hou","houp","hue","hui","huit","huitième","hum","hurrah","i","importe","j","jusqu",
            "jusque","k","l","laquelle","las","lequel","lès","lesquelles","lesquels","leurs","longtemps",
            "lorsque","lui","lui-même","m","maint","malgré","me","mêmes","merci","mien","mienne","miennes",
            "miens","mille","mince","moi","moi-même","moyennant","n","na","ne","néanmoins","neuf","neuvième",
            "nombreuses","nombreux","non","nos","nôtre","nôtres","nous-mêmes","nul","o","o|","ô","oh","ohé",
            "olé","ollé","on","ont","onze","onzième","ore","ouf","ouias","oust","ouste","outre","p","paf","pan",
            "parmi","partant","particulier","particulière","particulièrement","passé","pendant","personne",
            "peuvent","peux","pff","pfft","pfut","pif","plein","plouf","plus","plusieurs","plutôt","pouah",
            "premier","première","premièrement","près","proche","psitt","puisque","q","qu","quant","quanta",
            "quant-à-soi","quarante","quatorze","quatre","quatre-vingt","quatrième","quatrièmement","quelconque",
            "quelque","quelques","quelqu'un","quiconque","quinze","quoi","quoique","r","revoici","revoilà","rien",
            "s","sacrebleu","sapristi","sauf","se","seize","selon","sept","septième","sera","seront","sienne","siennes",
            "siens","sinon","six","sixième","soi","soi-même","soit","soixante","stop","suis","suivant","surtout","t",
            "tac","tant","te","té","tel","telle","telles","tenant","tic","tien","tienne","tiennes","tiens","toc","toi",
            "toi-même","touchant","toujours","toute","toutes","treize","trente","trois","troisième","troisièmement",
            "tsoin","tsouin","u","un","une","unes","uns","v","va","vais","vas","vé","vers","via","vif","vifs","vingt",
            "vivat","vive","vives","vlan","voici","voilà","vos","vôtre","vôtres","vous-mêmes","w","x","y","z","zut",
            "droite","force","haut","nouveaux","parole","personnes","pièce","valeur","voie",
            ".", ",", "'", "?", "!", "-", "(", ")", "@", "/", ":",
            "_", ";", "+", "&", "%", "*", "=", "<", ">", "$", "[", "]", "{",
            "}", "~", "^", "#", "|", ":-" };

        public bool IsEmptyWord(string word)
        {
            return EmptyWordRoots.Contains(word.ToLower()) ? true : false;

        }
    }
}
