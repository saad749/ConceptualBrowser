using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public ConceptualBrowserForm()
        {
            InitializeComponent();
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


                    Langauge = DetectLanguage(FileText);
                    tssLanguage.Text = "Language: " + Langauge;


                    ConceptExtraction ce = new ConceptExtraction();
                    OptimalTree = ce.Extract(FileText, Langauge);


                    FillNode(OptimalTree, null);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Input Exception" + Environment.NewLine + ex.Message);
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
                List<int> coveringSentenceNumbers = optimal.OptimalConcept.Nodes.Select(n => n.Number).ToList();

                List<String> sentences = FileText.Split(new char[] { '.', ',', ';' }).ToList();
                txtText.Text = String.Empty;

                for (int i = 0; i < sentences.Count; i++)
                {
                    if (coveringSentenceNumbers.Contains(i))
                    {
                        AppendText(txtText, sentences[i], Color.DarkBlue, new Font(FontFamily.GenericSansSerif, 12.0F, FontStyle.Bold));
                    }
                    else
                    {
                        AppendText(txtText, sentences[i], Color.Black, new Font(FontFamily.GenericSansSerif, 10.0F, FontStyle.Regular));
                    }
                }
                
            }
        }

        private string ReadFile(string path)
        {
            return File.ReadAllText(path, Encoding.Default);

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
    }
}
