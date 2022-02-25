# Conceptual Browser
Extract Formal Concepts from any Text

The solution is divided into 3 projects:
- ConceptualBrowser.Business
    - Consists of the algorithms to extract the formal concepts
- ConceptualBrowser.Form UI
    - A Windows Form Application to view all the extracted formal concepts from the text and the sentences and keywords that form the concept.
- ConceptualBrowser.API
    - A simple web api, that provides the summary of a given text; which is the most optimal concept extracted from the given text.

To build the ConceptualBrowser.Business:
 - Right Click on ConceptualBrowser.Business in the Solution Explorer
 - Add > Referrence
 - Browse to ConceptualBrowser.FormUI\Assets
 - Select NTextCat.dll


How to use the API:
 - The API expects a single JSON Escaped string.
 - The text can be JSON escaped using: https://www.freeformatter.com/json-escape.html
 - Sample Text to be used with API
    - "Ukraine wants peace and is ready for talks with Russia, including on neutral status regarding the North Atlantic Treaty Organisation (Nato), Ukrainian presidential advisor Mykhailo Podolyak told Reuters on Friday.\r\n\r\n\"If talks are possible, they should be held. If in Moscow they say they want to hold talks, including on neutral status, we are not afraid of this,\" he said via a text message. \"We can talk about that as well.\"\r\n\r\n\"Our readiness for dialogue is part of our persistent pursuit of peace.\"\r\n\r\nUkraine currently is not part of Nato or the European Union, though it wants to join both, anathema to its former overlord Moscow.\r\n\r\nThe statement from Podolyak comes after the Kremlin said Russian President Vladimir Putin is ready to send a delegation to Minsk for negotiations with the representatives of Ukraine.\r\n\r\nThe development followed a phone call between Putin and Chinese President Xi Jinping, who conveyed China's support to Moscow in its efforts to resolve the Ukraine crisis via dialogue.\r\n\r\nAccording to a statement on the Chinese foreign ministry's website, Xi called for all sides to abandon a Cold War mentality, respect the legitimate security concerns of all countries, and form a balanced, effective and sustainable European security mechanism through negotiation.\r\n\r\nXi said China respects the sovereignty and territorial integrity of all countries.\r\n\r\nMeanwhile, the Kremlin said Xi respected Russia's actions and was ready for close coordination and mutual support at the United Nations, where both are veto-holding permanent members of the Security Council.\r\n\r\nAccording to the Kremlin, Putin told Xi that Russia's military operation in Ukraine was necessary to protect people against \u201Cgenocide\u201D, an accusation that the West calls baseless propaganda.\r\n\r\nPutin also communicated to Xi that the US and North Atlantic Treaty Organisation had long ignored Russia's legitimate security concerns, repeatedly reneged on their commitments, and continued to expand military deployment eastward, challenging Russia's strategic bottom line, according to the statement on Chinese foreign ministry's website.\r\n\r\nRussia is willing to conduct high-level negotiations with Ukraine, the statement quoted Putin as saying.\r\n\r\nHowever, Russian Foreign Minister Sergei Lavrov made the talks conditional to Ukraine's military laying down its arms.\r\n\r\nHe said that Moscow did not want \"neo-Nazis\" to govern Ukraine.\r\n\r\nRussia launched its operation on land, air and sea on Thursday following a declaration of war by President Vladimir Putin.\r\n\r\nChina has refused to call Russia's action in Ukraine an \"invasion\" or criticise Moscow.\r\n"

