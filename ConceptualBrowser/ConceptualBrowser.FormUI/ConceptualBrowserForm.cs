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
using Newtonsoft.Json;

namespace ConceptualBrowser.FormUI
{
    public partial class ConceptualBrowserForm : Form
    {
        public string Langauge { get; set; }
        public List<OptimalConceptTreeItem> OptimalTree { get; set; }
        public string FileText { get; set; }
        public Encoding Encoding { get; set; } = Encoding.Default;
        public Stopwatch Stopwatch { get; set; }
        public double CoveragePercentage { get; set; }
        public float FontSize { get; set; } = 10.0F;

        public ConceptualBrowserForm()
        {
            InitializeComponent();
            
            this.tsAddToStopWords.Click += new EventHandler(AddToStopWords);
            cmbCoveragePercentage.SelectedIndex = 19;

            var items = new[] {
                new { Text = "Auto-Detect", Value = "Auto-Detect" },
                new { Text = "English", Value = "eng" },
                new { Text = "French", Value = "fra" },
                new { Text = "Spanish", Value = "spa" },
                new { Text = "Czech", Value = "ces" },
                new { Text = "Danish", Value = "dan" },
                new { Text = "Dutch", Value = "nld" },
                new { Text = "Finnish", Value = "fin" },
                new { Text = "German", Value = "deu" },
                new { Text = "Hungarian", Value = "hun" },
                new { Text = "Norwegian", Value = "nor" },
                new { Text = "Italian", Value = "ita" },
                new { Text = "Portuguese", Value = "por" },
                new { Text = "Romanian", Value = "ron" },
                new { Text = "Russian", Value = "rus" },
                new { Text = "Arabic", Value = "arb" },
                new { Text = "None", Value = "none" }
            };

            cmbLanguage.DisplayMember = "Text";
            cmbLanguage.ValueMember = "Value";
            cmbLanguage.DataSource = items;
            cmbLanguage.SelectedIndex = 15;
            cmbFont.SelectedIndex = 1;

            if (unicodeToolStripMenuItem.Checked)
                Encoding = Encoding.Unicode;
            else if (aNSIToolStripMenuItem.Checked)
                Encoding = Encoding.ASCII;
            else if (uTF8ToolStripMenuItem.Checked)
                Encoding = Encoding.UTF8;
        }

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            fileToolStripMenuItem.Enabled = false;
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
                    txtSummary.Text = "";
                    txtKeywords.Text = "";

                    //Also Take Language by User Input
                    Langauge = cmbLanguage.SelectedIndex == 0 ? DetectLanguage(FileText) : cmbLanguage.SelectedValue.ToString();
                    tssLanguage.Text = "Language: " + Langauge;

                    CoveragePercentage = Convert.ToDouble(cmbCoveragePercentage.SelectedItem) / 100;
                    tssCoveragePercentage.Text = "Coverage Percentage: " + CoveragePercentage * 100;

                    pbMain.Maximum = 100;
                    pbMain.Minimum = 0;
                    pbMain.Value = 5;

                    if (Langauge == "arb")
                    {
                        txtKeywords.SelectionAlignment = HorizontalAlignment.Right;
                        txtText.SelectionAlignment = HorizontalAlignment.Right;
                        txtKeywords.RightToLeft = RightToLeft.Yes;
                        txtText.RightToLeft = RightToLeft.Yes;
                        txtSummary.RightToLeft = RightToLeft.Yes;
                    }
                    else
                    {
                        txtKeywords.SelectionAlignment = HorizontalAlignment.Left;
                        txtText.SelectionAlignment = HorizontalAlignment.Left;
                        txtKeywords.RightToLeft = RightToLeft.No;
                        txtText.RightToLeft = RightToLeft.No;
                        txtSummary.RightToLeft = RightToLeft.No;
                    }


                    bgwExtraction.RunWorkerAsync();
                    
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
                List<string> keywords = optimal.OptimalConcept.Keywords.Select(k => k.Keyword).ToList();

                ITextAnalyzer textAnalyzer = new TextAnalyzer();
                List<string> sentences = textAnalyzer.GetSentences(FileText);
                txtText.Text = "";
                txtSummary.Text = "";
                txtKeywords.Text = string.Join(Environment.NewLine, keywords.ToArray());

                for (int i = 0; i < sentences.Count; i++)
                {
                    if (coveringSentenceNumbers.Contains(i))
                    {
                        AppendText(txtText, sentences[i] + ".", Color.DarkBlue, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold));
                        AppendText(txtSummary, i + ": ", Color.DarkBlue, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold));
                        AppendText(txtSummary, sentences[i].Trim() + Environment.NewLine, Color.DarkBlue, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Regular));
                    }
                    else
                    {
                        AppendText(txtText, sentences[i] + ".", Color.Black, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Regular));
                    }
                }
                
            }
        }

        private string ReadFile(string path)
        {
            return File.ReadAllText(path, Encoding);
        }

        public static void AppendText(RichTextBox box, string text, Color color, Font font)
        {
            int length = box.TextLength;  // at end of text
            box.AppendText(text);
            box.SelectionStart = length;
            box.SelectionLength = text.Length;
            box.SelectionColor = color;
            box.SelectionFont = font;
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
            Encoding = Encoding.UTF8;
            ((ToolStripMenuItem)sender).Checked = true;
            unicodeToolStripMenuItem.Checked = false;
            aNSIToolStripMenuItem.Checked = false;
        }

        private void tsmiOptimalConcepts_Click(object sender, EventArgs e)
        {
            if (OptimalTree is null)
            {
                MessageBox.Show("Please Open a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] lines = OptimalTree.Select(c => c.OptimalConcept.ConceptName).ToArray();
            string fileName = $"exported_concepts_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.txt";
            File.WriteAllLines(fileName, lines);
            Process.Start(fileName);
        }

        private void bgwExtraction_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            var backgroundWorker = sender as BackgroundWorker;
            ConceptExtraction ce = new ConceptExtraction();
            OptimalTree = ce.Extract(FileText, Langauge, CoveragePercentage, backgroundWorker);

            var optimals = OptimalTree.Select(x => x.OptimalConcept);
            List<Sentence> sentences = new List<Sentence>();
            foreach (var optimal in optimals)
            {
                Sentence sentence = optimal.Sentences[0];
                sentences.Add(sentence);
            }

            File.WriteAllLines("mySentences.txt", sentences.Select(x => x.KeywordString).ToArray());

            Stopwatch.Stop();
            

            
        }

        private void bgwExtraction_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbMain.Value = e.ProgressPercentage > 100 ? 100 : e.ProgressPercentage;
        }

        private void bgwExtraction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FillNode(OptimalTree, null);
            pbMain.Value = 100;
            var timeTaken = "";
            var ms = Stopwatch.ElapsedMilliseconds;

            if (ms > 60000)
                timeTaken = (ms / (double)60000) + " minutes";
            else if (ms > 1000)
                timeTaken = (ms / (double)1000) + " seconds";
            else
                timeTaken = ms + " ms";

            tssPerformance.Text = "Time Taken: " + timeTaken;
            //MessageBox.Show("Extraction Completed!", "Success!");
            fileToolStripMenuItem.Enabled = true;
        }

        private void tsmiOptimalConceptsSimple_Click(object sender, EventArgs e)
        {
            if(OptimalTree is null)
            {
                MessageBox.Show("Please Open a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var concepts = OptimalTree.Select(c => new {
                c.OptimalConcept.ConceptName,
                Sentences = c.OptimalConcept.Sentences.Select(s => new {
                    s.OriginalSentence
                }).ToList(),
                Keywords = c.OptimalConcept.Keywords.Select(k => new {
                    k.Keyword
                }).ToList()
            }).ToList();
            var json = JsonConvert.SerializeObject(concepts, Formatting.Indented);
            string fileName = $"exported_concepts_simple_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json";
            File.WriteAllText(fileName, json);
            Process.Start(fileName);
        }

        private void CmbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmbFont.SelectedItem.ToString()))
                FontSize = (float)Convert.ToDouble(cmbFont.SelectedItem.ToString());

            txtSummary.SelectAll();
            txtSummary.SelectionFont = new Font(txtSummary.Font.FontFamily, FontSize, txtSummary.Font.Style);

            //txtText.SelectAll();
            //txtText.SelectionFont = new Font(txtSummary.Font.FontFamily, FontSize, txtSummary.Font.Style);

            treeViewBrowser.Font = new Font(treeViewBrowser.Font.FontFamily, FontSize, treeViewBrowser.Font.Style);

            txtKeywords.SelectAll();
            txtKeywords.SelectionFont = new Font(txtKeywords.Font.FontFamily, FontSize, txtKeywords.Font.Style);
        }

        private void TsmiOptimalConceptsDetailed_Click(object sender, EventArgs e)
        {
            if (OptimalTree is null)
            {
                MessageBox.Show("Please Open a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var concepts = OptimalTree.Select(c => c.OptimalConcept).ToList();
            var detailed = new { Concepts = concepts, Text = txtText.Text};
            var json = JsonConvert.SerializeObject(detailed, Formatting.Indented);
            string fileName = $"exported_concepts_detailed_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json";
            File.WriteAllText(fileName, json, Encoding);
            Process.Start(fileName);
        }

        private void TsmiImport_Click(object sender, EventArgs e)
        {
            fileToolStripMenuItem.Enabled = false;
            treeViewBrowser.Nodes.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string fileName = openFileDialog.FileName;

            if (!String.IsNullOrWhiteSpace(fileName))
            {
                try
                {
                    FileText = ReadFile(fileName);
                    var conceptsDTO = JsonConvert.DeserializeObject<ConceptsDTO>(FileText);
                    FileText = conceptsDTO.Text;
                    txtText.Text = FileText;

                    txtSummary.Text = "";
                    txtKeywords.Text = "";

                    Langauge = cmbLanguage.SelectedIndex == 0 ? DetectLanguage(FileText) : cmbLanguage.SelectedValue.ToString();
                    tssLanguage.Text = "Language: " + Langauge;

                    if (Langauge == "arb")
                    {
                        txtKeywords.SelectionAlignment = HorizontalAlignment.Right;
                        txtText.SelectionAlignment = HorizontalAlignment.Right;
                        txtKeywords.RightToLeft = RightToLeft.Yes;
                        txtText.RightToLeft = RightToLeft.Yes;
                        txtSummary.RightToLeft = RightToLeft.Yes;
                    }
                    else
                    {
                        txtKeywords.SelectionAlignment = HorizontalAlignment.Left;
                        txtText.SelectionAlignment = HorizontalAlignment.Left;
                        txtKeywords.RightToLeft = RightToLeft.No;
                        txtText.RightToLeft = RightToLeft.No;
                        txtSummary.RightToLeft = RightToLeft.No;
                    }

                    ConceptExtraction ce = new ConceptExtraction();
                    OptimalTree = ce.CreateTree(conceptsDTO.Concepts);

                    var optimals = OptimalTree.Select(x => x.OptimalConcept);
                    List<Sentence> sentences = new List<Sentence>();
                    foreach (var optimal in optimals)
                    {
                        Sentence sentence = optimal.Sentences[0];
                        sentences.Add(sentence);
                    }

                    FillNode(OptimalTree, null);
                    //MessageBox.Show("Extraction Completed!", "Success!");
                    fileToolStripMenuItem.Enabled = true;

                    File.WriteAllLines("mySentences.txt", sentences.Select(x => x.KeywordString).ToArray());

                }
                catch (IOException ex)
                {
                    MessageBox.Show("Input Exception" + Environment.NewLine + ex.Message);
                }
            }
        }

        internal class ConceptsDTO
        {
            public string Text { get; set; }
            public List<OptimalConcept> Concepts { get; set; }
        }
    }
}
