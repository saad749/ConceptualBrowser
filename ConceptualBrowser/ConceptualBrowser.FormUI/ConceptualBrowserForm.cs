using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConceptualBrowser.Business;
using ConceptualBrowser.Business.Common;
using ConceptualBrowser.Business.Common.Stemmer;
using ConceptualBrowser.Business.Entities;

namespace ConceptualBrowser.FormUI
{
    public partial class ConceptualBrowserForm : Form
    {
        public string Langauge { get; set; }
        public List<OptimalConceptTreeItem> OptimalTree { get; set; }
        public string FileText { get; set; }
        public Encoding Encoding { get; set; } = Encoding.Default;


        public ConceptualBrowserForm()
        {
            InitializeComponent();
            cmbLanguage.SelectedIndex = 0;
            this.tsAddToStopWords.Click += new EventHandler(AddToStopWords);
        }

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            treeViewBrowser.Nodes.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string fileName = openFileDialog.FileName;

            if (!String.IsNullOrWhiteSpace(fileName))
            {
                try
                {
                    FileText = ReadFile(fileName);
                    txtText.Text = FileText;

                    //Also Take Language by User Input
                    Langauge = cmbLanguage.SelectedIndex == 0 ? DetectLanguage(FileText) : cmbLanguage.SelectedItem.ToString();
                    tssLanguage.Text = "Language: " + Langauge;
                    ConceptExtraction ce = new ConceptExtraction();
                    OptimalTree = ce.Extract(FileText, Langauge);


                    FillNode(OptimalTree, null);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Input Exception" + Environment.NewLine + ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show("The language of text is not supported" + Environment.NewLine
                        + "Try choosing a different Encoding or choose a supported language" + Environment.NewLine
                        + "Detected Language is " + Langauge + Environment.NewLine
                        + ex.Message);
                }

            }
        }

        private void FillNode(List<OptimalConceptTreeItem> optimals, TreeNode node)
        {
            int parentID = (int?) node?.Tag ?? 0;

            TreeNodeCollection nodesCollection = node?.Nodes ?? treeViewBrowser.Nodes;

            foreach (OptimalConceptTreeItem optimal in optimals.Where(i => i.ParentId == parentID))
            {
                TreeNode newNode = nodesCollection.Add(optimal.OptimalConcept.ConceptName);//, optimal.Name);
                newNode.Tag = optimal.Id;
                newNode.ContextMenuStrip = ctxMenuTreeView;


                FillNode(optimals, newNode);
            }
        }

        private string DetectLanguage(string text)
        {
            int sampleStringLength = text.Length > 1000 ? 1000 : text.Length;
            string textSample = text.Substring(0, sampleStringLength);
            LanguageDetection detection = new LanguageDetection();

            return detection.DetectLanguage(textSample);
        }

        private void AddToStopWords(object sender, EventArgs e)
        {
            EmptyWords emptyWords = new EmptyWords(Langauge);

            string text = treeViewBrowser.SelectedNode.Text;
            emptyWords.AppendEmptyWordsToFile(text);

        }

        private void treeViewBrowser_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                treeViewBrowser.SelectedNode = e.Node;
                OptimalConceptTreeItem optimal = OptimalTree.Find(t => t.Id == (int)e.Node.Tag);
                List<int> coveringSentenceNumbers = optimal.OptimalConcept.Sentences.Select(n => n.SentenceIndex).ToList();
                //List<int> coveringSentenceNumbers = optimal.OptimalConcept.Keywords.SelectMany(n => n.Sentences).Select(s => s.CoveredByConceptNumber).ToList();
                //List<int> distinctSentences = coveringSentenceNumbers.
                List<int> distinctSentence = coveringSentenceNumbers
                                                      .GroupBy(p => p)
                                                      .Select(g => g.First())
                                                      .ToList();

                ITextAnalyzer textAnalyzer = new TextAnalyzer();
                List<String> sentences = textAnalyzer.GetSentences(FileText);
                //List<String> sentences = FileText.Split(new char[] { '.', ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                txtText.Text = String.Empty;

                for (int i = 0; i < sentences.Count; i++)
                {
                    if (coveringSentenceNumbers.Contains(i))
                    {
                        AppendText(txtText, i + ": " + sentences[i].Trim() + Environment.NewLine, Color.DarkBlue, new Font(FontFamily.GenericSansSerif, 11.0F, FontStyle.Regular));
                    }
                    //else if(sentences.Count < 100)
                    //{
                    //    AppendText(txtText, sentences[i], Color.Black, new Font(FontFamily.GenericSansSerif, 10.0F, FontStyle.Regular));
                    //}
                }
                
            }
        }

        private string ReadFile(string path)
        {
            return File.ReadAllText(path, Encoding);

            //using (var reader = new StreamReader(path))
            //{
            //    // Make sure you read from the file or it won't be able
            //    // to guess the encoding
            //    var file = reader.ReadToEnd();
            //    Console.WriteLine(reader.CurrentEncoding);
            //}
        }

        public static void AppendText(RichTextBox box, string text, Color color, Font font)
        {
            int length = box.TextLength;  // at end of text
            box.AppendText(text);
            box.SelectionStart = length;
            box.SelectionLength = text.Length;
            box.SelectionColor = color;
            box.SelectionFont = font;

            //box.SelectionStart = box.TextLength;
            //box.SelectionLength = 0;

            //box.SelectionColor = color;
            //box.SelectionFont = font;
            //box.AppendText(text);
            //box.SelectionColor = box.ForeColor;
            //box.SelectionFont = new Font(FontFamily.GenericSansSerif, 10.0F, FontStyle.Regular);
        }

        private void unicodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Encoding = Encoding.Unicode;
            ((ToolStripMenuItem) sender).Checked = true;
            aNSIToolStripMenuItem.Checked = false;
            uTF8ToolStripMenuItem.Checked = false;
        }

        private void aNSIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Encoding = Encoding.Default;
            ((ToolStripMenuItem)sender).Checked = true;
            unicodeToolStripMenuItem.Checked = false;
            uTF8ToolStripMenuItem.Checked = false;
        }

        private void uTF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Encoding = Encoding.Default;
            ((ToolStripMenuItem)sender).Checked = true;
            unicodeToolStripMenuItem.Checked = false;
            aNSIToolStripMenuItem.Checked = false;
        }

        private void optimalConceptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] lines = OptimalTree.Select(c => c.OptimalConcept.ConceptName).ToArray();
            string fileName = "exported_concepts.txt";
            File.WriteAllLines(fileName, lines);
            Process.Start(fileName);
        }
    }
}
