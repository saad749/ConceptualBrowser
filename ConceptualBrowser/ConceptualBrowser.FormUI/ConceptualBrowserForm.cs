using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
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
using ConceptualBrowser.Business.Common.TextAnalysis;
using ConceptualBrowser.Business.Entities;
using Iso639;
using Newtonsoft.Json;

namespace ConceptualBrowser.FormUI
{
    public partial class ConceptualBrowserForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Language Language { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OptimalConceptTreeItem> OptimalTree { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileText { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Encoding Encoding { get; set; } = Encoding.Default;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Stopwatch Stopwatch { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double CoveragePercentage { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float FontSize { get; set; } = 10.0F;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NumericPrecision { get; set; } = -4;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ExtractionLanguageCode { get; set; }

        /// <summary>
        /// Index of the last sentence in the text (used for highlighting test/special sentence)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int LastSentenceIndex { get; set; } = -1;

        public ConceptualBrowserForm()
        {
            InitializeComponent();
            cmbCoveragePercentage.SelectedIndex = 19;

            var items = new[] {
                new { Text = "Auto-Detect", Value = "Auto-Detect" },
                new { Text = "Numeric (CSV)", Value = Stemmers.NumericCode },
                new { Text = "Arabic", Value = "ara" },
                new { Text = "Armenian", Value = "hye" },
                new { Text = "Bulgarian", Value = "bul" },
                new { Text = "Basque", Value = "eus" },
                new { Text = "Catalan", Value = "cat" },
                new { Text = "Czech", Value = "ces" },
                new { Text = "Danish", Value = "dan" },
                new { Text = "Dutch", Value = "nld" },
                new { Text = "English", Value = "eng" },
                new { Text = "Finnish", Value = "fin" },
                new { Text = "French", Value = "fra" },
                new { Text = "German", Value = "deu" },
                new { Text = "Greek", Value = "ell" },
                new { Text = "Hebrew", Value = "heb" },
                new { Text = "Hindi", Value = "hin" },
                new { Text = "Hungarian", Value = "hun" },
                new { Text = "Indonesian", Value = "ind" },
                new { Text = "Irish", Value = "gle" },
                new { Text = "Italian", Value = "ita" },
                new { Text = "Lithuanian", Value = "lit" },
                new { Text = "Malay", Value = "msa" },
                new { Text = "Nepali", Value = "nep" },
                new { Text = "Norwegian", Value = "nor" },
                new { Text = "Polish", Value = "pol" },
                new { Text = "Portuguese", Value = "por" },
                new { Text = "Romanian", Value = "ron" },
                new { Text = "Russian", Value = "rus" },
                new { Text = "Serbian", Value = "srp" },
                new { Text = "Slovakian", Value = "slk" },
                new { Text = "Spanish", Value = "spa" },
                new { Text = "Swedish", Value = "swe" },
                new { Text = "Tamil", Value = "tam" },
                new { Text = "Turkish", Value = "tur" },
                new { Text = "Urdu", Value = "urd" },
                new { Text = "Vietnamese", Value = "vie" },
                new { Text = "Yiddish", Value = "yid" },
                new { Text = "None", Value = "none" }
            };

            cmbLanguage.DisplayMember = "Text";
            cmbLanguage.ValueMember = "Value";
            cmbLanguage.DataSource = items;
            cmbLanguage.SelectedIndex = 0;
            cmbFont.SelectedIndex = 1;
            // nudPrecision default is set in Designer to -4 (4 decimal places)

            if (unicodeToolStripMenuItem.Checked)
                Encoding = Encoding.Unicode;
            else if (aNSIToolStripMenuItem.Checked)
                Encoding = Encoding.ASCII;
            else if (uTF8ToolStripMenuItem.Checked)
                Encoding = Encoding.UTF8;

            Directory.CreateDirectory("Output");
        }

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                fileToolStripMenuItem.Enabled = true;
            else
            {
                string fileName = openFileDialog.FileName;
                if (!String.IsNullOrWhiteSpace(fileName)) 
                {
                    ProcessText(ReadFile(fileName));
                }
            }
        }

        private void FillNode(List<OptimalConceptTreeItem> optimals, TreeNode node)
        {
            // BUGFIX: Add null check to prevent crash when optimals is null
            if (optimals == null || optimals.Count == 0)
            {
                return;
            }

            int parentID = (int?)node?.Tag ?? 0;

            TreeNodeCollection nodesCollection = node?.Nodes ?? treeViewBrowser.Nodes;

            foreach (OptimalConceptTreeItem optimal in optimals.Where(i => i.ParentId == parentID))
            {
                TreeNode newNode = nodesCollection.Add(optimal.OptimalConcept.ConceptName + " (" + optimal.OptimalConcept.Gain + ")");//, optimal.Name);
                newNode.Tag = optimal.Id;

                // Highlight concepts that contain the last (test/special) sentence
                if (LastSentenceIndex >= 0 &&
                    optimal.OptimalConcept.Sentences != null &&
                    optimal.OptimalConcept.Sentences.Any(s => s.SentenceIndex == LastSentenceIndex))
                {
                    newNode.ForeColor = Color.Green;
                    newNode.NodeFont = new Font(treeViewBrowser.Font, FontStyle.Bold);
                }

                FillNode(optimals, newNode);
            }
        }

        private string DetectLanguage(string text)
        {
            int sampleStringLength = text.Length > 1000 ? 1000 : text.Length;
            string textSample = text.Substring(0, sampleStringLength);
            var detectedLanguage = LanguageDetection.DetectLanguage(textSample).Result;
            return detectedLanguage;
        }

        /// <summary>
        /// Calculates the index of the last sentence in the text for special highlighting.
        /// </summary>
        private void CalculateLastSentenceIndex()
        {
            if (string.IsNullOrEmpty(FileText))
            {
                LastSentenceIndex = -1;
                return;
            }

            ITextAnalyzer textAnalyzer;
            if (ExtractionLanguageCode == Stemmers.NumericCode)
            {
                textAnalyzer = new NumericAnalyzer(NumericPrecision);
            }
            else
            {
                textAnalyzer = new TextAnalyzer(ExtractionLanguageCode ?? Language.Part3);
            }

            var sentences = textAnalyzer.GetSentences(FileText);
            LastSentenceIndex = sentences.Count - 1;
        }

        private void treeViewBrowser_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                treeViewBrowser.SelectedNode = e.Node;
                OptimalConceptTreeItem optimal = OptimalTree.Find(t => t.Id == (int)e.Node.Tag);
                List<int> coveringSentenceNumbers = optimal.OptimalConcept.Sentences.Select(n => n.SentenceIndex).ToList();
                List<string> keywords = optimal.OptimalConcept.Keywords.Select(k => k.Keyword).ToList();

                // Use the correct analyzer based on extraction language code
                ITextAnalyzer textAnalyzer;
                if (ExtractionLanguageCode == Stemmers.NumericCode)
                {
                    textAnalyzer = new NumericAnalyzer(NumericPrecision);
                }
                else
                {
                    textAnalyzer = new TextAnalyzer(ExtractionLanguageCode ?? Language.Part3);
                }
                List<string> sentences = textAnalyzer.GetSentences(FileText);
                txtText.Text = "";
                txtSummary.Text = "";
                txtKeywords.Text = string.Join(Environment.NewLine, keywords.ToArray());

                for (int i = 0; i < sentences.Count; i++)
                {
                    // Determine color: Green for last (test) sentence, DarkBlue for covered, Black for others
                    bool isLastSentence = (i == LastSentenceIndex);
                    Color sentenceColor = isLastSentence ? Color.Green : (coveringSentenceNumbers.Contains(i) ? Color.DarkBlue : Color.Black);

                    if (coveringSentenceNumbers.Contains(i))
                    {
                        AppendText(txtText, i + ": ", sentenceColor, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold));
                        AppendText(txtText, sentences[i].Trim() + "." + Environment.NewLine, sentenceColor, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold));
                        AppendText(txtSummary, i + ": ", sentenceColor, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold));
                        AppendText(txtSummary, sentences[i].Trim() + Environment.NewLine, sentenceColor, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Regular));
                    }
                    else
                    {
                        AppendText(txtText, i + ": ", sentenceColor, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold));
                        AppendText(txtText, sentences[i].Trim() + "." + Environment.NewLine, sentenceColor, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Regular));
                    }
                }

                AppendText(txtSummary, Environment.NewLine +
                                            "Gain: " + optimal.OptimalConcept.Gain.ToString() + Environment.NewLine +
                                            "Category: " + optimal.OptimalConcept.Category + "(P:" + optimal.OptimalConcept.PositiveCount + "/N:" + optimal.OptimalConcept.NegativeCount + ")"  + Environment.NewLine +
                                            "Keywords: " + optimal.OptimalConcept.Keywords.Count.ToString() + Environment.NewLine +
                                            "Sentences: " + optimal.OptimalConcept.Sentences.Count.ToString() + Environment.NewLine,
                                            Color.DarkGreen, new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold));

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
            ((ToolStripMenuItem)sender).Checked = true;
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

            string defaultFileName = $"exported_concepts_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.txt";
            string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            string filePath = ShowSaveFileDialog(defaultFileName, filter, "txt");

            if (filePath == null)
                return;

            string[] lines = OptimalTree.Select(c => c.OptimalConcept.ConceptName).ToArray();
            File.WriteAllLines(filePath, lines);
            OpenFileInNotepad(filePath);
        }

        private void bgwExtraction_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            var backgroundWorker = sender as BackgroundWorker;
            ConceptExtraction ce = new ConceptExtraction();
            var optimals = ce.Extract(FileText, ExtractionLanguageCode, CoveragePercentage, backgroundWorker, NumericPrecision);

            // BUGFIX: Handle case where concept extraction returns no results
            if (optimals == null || optimals.Count == 0)
            {
                OptimalTree = new List<OptimalConceptTreeItem>();
                return; // Early return to prevent crash
            }

            OptimalTree = CreateTree(optimals.OrderByDescending(o => o.Gain).ToList());

            // BUGFIX: Check if OptimalTree has any items before accessing
            if (OptimalTree == null || OptimalTree.Count == 0)
            {
                return; // Early return to prevent crash
            }

            var MostOptimalConcept = OptimalTree.FirstOrDefault().OptimalConcept;

            File.WriteAllLines("Output\\mySentences.txt", MostOptimalConcept.Sentences.Select(x => x.OriginalSentence).ToArray());

            Stopwatch.Stop();

            var elapsedMilliseconds = Stopwatch.ElapsedMilliseconds;


        }

        private void bgwExtraction_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbMain.Value = e.ProgressPercentage > 100 ? 100 : e.ProgressPercentage;
        }

        private void bgwExtraction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Calculate the last sentence index for highlighting
            CalculateLastSentenceIndex();

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
            if (OptimalTree is null)
            {
                MessageBox.Show("Please Open a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string defaultFileName = $"exported_concepts_simple_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json";
            string filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            string filePath = ShowSaveFileDialog(defaultFileName, filter, "json");

            if (filePath == null)
                return;

            var concepts = OptimalTree.Select(c => new
            {
                c.OptimalConcept.ConceptName,
                Sentences = c.OptimalConcept.Sentences.Select(s => new
                {
                    s.OriginalSentence
                }).ToList(),
                Keywords = c.OptimalConcept.Keywords.Select(k => new
                {
                    k.Keyword
                }).ToList()
            }).ToList();
            var json = JsonConvert.SerializeObject(concepts, Formatting.Indented);
            File.WriteAllText(filePath, json);
            OpenFileInNotepad(filePath);
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

            string defaultFileName = $"exported_concepts_detailed_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json";
            string filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            string filePath = ShowSaveFileDialog(defaultFileName, filter, "json");

            if (filePath == null)
                return;

            var concepts = OptimalTree.Select(c => c.OptimalConcept).ToList();
            var detailed = new { Concepts = concepts, Text = txtText.Text };
            var json = JsonConvert.SerializeObject(detailed, Formatting.Indented);
            File.WriteAllText(filePath, json, Encoding);
            OpenFileInNotepad(filePath);
        }

        private void TsmiImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                fileToolStripMenuItem.Enabled = true;
            else
            {
                fileToolStripMenuItem.Enabled = false;
                treeViewBrowser.Nodes.Clear();
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

                        Language = Language.FromPart3(cmbLanguage.SelectedIndex == 0 ? DetectLanguage(FileText) : cmbLanguage.SelectedValue.ToString());
                        tssLanguage.Text = "Language: " + Language.Name;

                        if (Language.Part3 == "ara")
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

                        OptimalTree = CreateTree(conceptsDTO.Concepts);

                        var optimals = OptimalTree.Select(x => x.OptimalConcept);
                        List<Sentence> sentences = new List<Sentence>();
                        foreach (var optimal in optimals)
                        {
                            Sentence sentence = optimal.Sentences[0];
                            sentences.Add(sentence);
                        }

                        // Set extraction language code and calculate last sentence index for highlighting
                        ExtractionLanguageCode = Language.Part3;
                        CalculateLastSentenceIndex();

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
        }

        internal class ConceptsDTO
        {
            public string Text { get; set; }
            public List<OptimalConcept> Concepts { get; set; }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Shows a SaveFileDialog with the specified parameters and returns the selected file path.
        /// </summary>
        /// <param name="defaultFileName">Default file name to show in the dialog</param>
        /// <param name="filter">File type filter (e.g., "JSON files (*.json)|*.json")</param>
        /// <param name="defaultExt">Default file extension without the dot (e.g., "json")</param>
        /// <returns>The selected file path, or null if the user cancelled</returns>
        private string ShowSaveFileDialog(string defaultFileName, string filter, string defaultExt)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = defaultFileName;
                saveFileDialog.Filter = filter;
                saveFileDialog.DefaultExt = defaultExt;
                // Use full path to Output folder relative to current working directory
                saveFileDialog.InitialDirectory = Path.GetFullPath("Output");
                saveFileDialog.AddExtension = true;
                saveFileDialog.OverwritePrompt = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
                return null;
            }
        }

        /// <summary>
        /// Safely opens a file in Notepad. Does nothing if Notepad is not available.
        /// </summary>
        /// <param name="filePath">The path to the file to open</param>
        private void OpenFileInNotepad(string filePath)
        {
            try
            {
                string notepadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "notepad.exe");

                if (!File.Exists(notepadPath))
                    return;

                Process.Start(new ProcessStartInfo
                {
                    FileName = notepadPath,
                    Arguments = $"\"{filePath}\"",
                    UseShellExecute = false
                });
            }
            catch
            {
                // Silently fail if Notepad cannot be started
            }
        }

        private void TsmiBinaryRelation_Click(object sender, EventArgs e)
        {
            if (OptimalTree is null || OptimalTree.Count == 0)
            {
                MessageBox.Show("Please process a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string defaultFileName = $"binary_relation_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.csv";
            string filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            string filePath = ShowSaveFileDialog(defaultFileName, filter, "csv");

            if (filePath == null)
                return;

            string csvContent = GenerateBinaryRelationMatrix();
            File.WriteAllText(filePath, csvContent, Encoding.UTF8);
            OpenFileInNotepad(filePath);
        }

        /// <summary>
        /// Generates a binary relation matrix CSV from the extracted concepts.
        /// Rows are sentences, columns are keywords, cells indicate keyword presence in sentence.
        /// </summary>
        private string GenerateBinaryRelationMatrix()
        {
            // Collect all unique keywords and sentences from all concepts
            var allKeywords = OptimalTree
                .SelectMany(t => t.OptimalConcept.Keywords)
                .GroupBy(k => k.Keyword)
                .Select(g => g.First())
                .OrderBy(k => k.Keyword)
                .ToList();

            var allSentences = OptimalTree
                .SelectMany(t => t.OptimalConcept.Sentences)
                .GroupBy(s => s.SentenceIndex)
                .Select(g => g.First())
                .OrderBy(s => s.SentenceIndex)
                .ToList();

            // Build the matrix
            var sb = new StringBuilder();

            // Header row: empty cell + keyword names
            sb.Append(","); // Empty cell for row headers
            foreach (var keyword in allKeywords)
            {
                sb.Append($"\"{keyword.Keyword}\",");
            }
            sb.AppendLine();

            // Data rows: sentence index + binary values
            foreach (var sentence in allSentences)
            {
                sb.Append($"{sentence.SentenceIndex},");

                foreach (var keyword in allKeywords)
                {
                    // Check if this sentence contains this keyword (using index-based lookup)
                    bool hasKeyword = sentence.KeywordIndexes.Contains(keyword.KeywordIndex);
                    sb.Append(hasKeyword ? "1," : "0,");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public List<OptimalConceptTreeItem> CreateTree(List<OptimalConcept> optimals)
        {
            List<OptimalConceptTreeItem> treeItems = new List<OptimalConceptTreeItem>();

            int i = 1;
            int? parentId = null;
            // If a tree/heap structure is required
            foreach (OptimalConcept optimal in optimals)
            {
                if (i != 0)
                {
                    parentId = ((i + 1) - 1) / 2;
                }
                treeItems.Add(new OptimalConceptTreeItem()
                {
                    Id = i,
                    ParentId = parentId,
                    OptimalConcept = optimal
                });
                i++;
            }

            //normal list
            //foreach (OptimalConcept optimal in optimals)
            //{
            //    treeItems.Add(new OptimalConceptTreeItem()
            //    {
            //        Id = i,
            //        ParentId = 0,
            //        OptimalConcept = optimal
            //    });
            //    i++;
            //}

            return treeItems;
        }

        private void OpenTextBoxMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new TextForm())
            {
                DialogResult result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    ProcessText(form.UserText);
                }
            }
        }

        private void openNumericFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.Title = "Open Numeric CSV File";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string fileName = openFileDialog.FileName;
            if (String.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            try
            {
                // Get precision from numeric up/down control
                int precision = (int)nudPrecision.Value;

                // Create preprocessor and convert CSV to text
                var preprocessor = new NumericPreprocessor(precision);
                preprocessor.HasHeaderRow = true; // Assume first row has headers

                string csvContent = File.ReadAllText(fileName, Encoding);
                var stats = preprocessor.GetStats(csvContent);

                // Convert to text format for FCA processing
                string convertedText = preprocessor.ConvertCsvToText(csvContent);

                if (string.IsNullOrWhiteSpace(convertedText))
                {
                    MessageBox.Show("Could not convert numeric data. Please check the CSV format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Show stats to user
                tssLanguage.Text = $"Numeric Data: {stats.TotalRows} rows, {stats.AttributeCount} attrs, +{stats.PositiveCount}/-{stats.NegativeCount}";

                // Process the converted text using "none" language (no text stemming)
                ProcessNumericText(convertedText);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing numeric file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessNumericText(string text)
        {
            fileToolStripMenuItem.Enabled = false;
            treeViewBrowser.Nodes.Clear();

            try
            {
                FileText = text;
                txtText.Text = FileText;
                txtSummary.Text = "";
                txtKeywords.Text = "";

                // Use "none" for numeric data - the NumericPreprocessor already did the stemming
                Language = Language.FromPart3("none");

                CoveragePercentage = Convert.ToDouble(cmbCoveragePercentage.SelectedItem) / 100;
                tssCoveragePercentage.Text = "Coverage Percentage: " + CoveragePercentage * 100;

                pbMain.Maximum = 100;
                pbMain.Minimum = 0;
                pbMain.Value = 5;

                // Numeric data is left-to-right
                txtKeywords.SelectionAlignment = HorizontalAlignment.Left;
                txtText.SelectionAlignment = HorizontalAlignment.Left;
                txtKeywords.RightToLeft = RightToLeft.No;
                txtText.RightToLeft = RightToLeft.No;
                txtSummary.RightToLeft = RightToLeft.No;

                bgwExtraction.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing numeric data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fileToolStripMenuItem.Enabled = true;
            }
        }

        private void ProcessText(string text)
        {
            fileToolStripMenuItem.Enabled = false;
            treeViewBrowser.Nodes.Clear();

            try
            {
                // Check if Numeric mode is selected
                string selectedLanguage = cmbLanguage.SelectedValue?.ToString();
                bool isNumericMode = selectedLanguage == Stemmers.NumericCode;

                // Set precision for numeric processing
                NumericPrecision = (int)nudPrecision.Value;

                if (isNumericMode)
                {
                    // For numeric mode, pass raw CSV to be processed by NumericAnalyzer
                    FileText = text;
                    txtText.Text = FileText;

                    // Get stats for display (using a temporary preprocessor just for stats)
                    var statsPreprocessor = new NumericPreprocessor(NumericPrecision);
                    statsPreprocessor.HasHeaderRow = true;
                    var stats = statsPreprocessor.GetStats(text);

                    tssLanguage.Text = $"Numeric: {stats.TotalRows} rows, {stats.AttributeCount} attrs, +{stats.PositiveCount}/-{stats.NegativeCount}";

                    // Use "numeric" language code so BinaryRelation uses NumericAnalyzer
                    ExtractionLanguageCode = Stemmers.NumericCode;
                    Language = Language.FromPart3("none"); // For display purposes only
                }
                else
                {
                    FileText = text;
                    txtText.Text = FileText;

                    // Detect or use selected language
                    string langCode = cmbLanguage.SelectedIndex == 0 ? DetectLanguage(FileText) : selectedLanguage;
                    ExtractionLanguageCode = langCode;
                    Language = Language.FromPart3(langCode);
                    tssLanguage.Text = "Language: " + Language.Name;
                }

                txtSummary.Text = "";
                txtKeywords.Text = "";

                CoveragePercentage = Convert.ToDouble(cmbCoveragePercentage.SelectedItem) / 100;
                tssCoveragePercentage.Text = "Coverage Percentage: " + CoveragePercentage * 100;

                pbMain.Maximum = 100;
                pbMain.Minimum = 0;
                pbMain.Value = 5;

                if (!isNumericMode && (Language.Part3 == "ara" || Language.Part3 == "urd" || Language.Part3 == "heb" || Language.Part3 == "yid" || Language.Part3 == "fas"))
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
                    + "Detected Language is " + Language + Environment.NewLine
                    + ex.Message);
            }

        }
    }
}
