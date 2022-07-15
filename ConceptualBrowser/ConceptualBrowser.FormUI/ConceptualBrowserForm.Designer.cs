namespace ConceptualBrowser.FormUI
{
    partial class ConceptualBrowserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConceptualBrowserForm));
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImport = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOptimalConcepts = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOptimalConceptsSimple = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOptimalConceptsDetailed = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBinaryRelation = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unicodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aNSIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uTF8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewBrowser = new System.Windows.Forms.TreeView();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tssLanguage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssPerformance = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssCoveragePercentage = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbMain = new System.Windows.Forms.ToolStripProgressBar();
            this.txtText = new System.Windows.Forms.RichTextBox();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.txtSummary = new System.Windows.Forms.RichTextBox();
            this.lblCoveragePercentage = new System.Windows.Forms.Label();
            this.cmbCoveragePercentage = new System.Windows.Forms.ComboBox();
            this.bgwExtraction = new System.ComponentModel.BackgroundWorker();
            this.txtKeywords = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFont = new System.Windows.Forms.ComboBox();
            this.lblFont = new System.Windows.Forms.Label();
            this.OpenTextBoxMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.encodingToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(1176, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenuItem,
            this.OpenTextBoxMenuItem,
            this.tsmiImport,
            this.exportMenuItem,
            this.exitMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Name = "openFileMenuItem";
            this.openFileMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openFileMenuItem.Text = "&Open File";
            this.openFileMenuItem.Click += new System.EventHandler(this.openFileMenuItem_Click);
            // 
            // tsmiImport
            // 
            this.tsmiImport.Name = "tsmiImport";
            this.tsmiImport.Size = new System.Drawing.Size(180, 22);
            this.tsmiImport.Text = "&Import";
            this.tsmiImport.Click += new System.EventHandler(this.TsmiImport_Click);
            // 
            // exportMenuItem
            // 
            this.exportMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOptimalConcepts,
            this.tsmiOptimalConceptsSimple,
            this.tsmiOptimalConceptsDetailed,
            this.tsmiBinaryRelation});
            this.exportMenuItem.Name = "exportMenuItem";
            this.exportMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportMenuItem.Text = "E&xport";
            // 
            // tsmiOptimalConcepts
            // 
            this.tsmiOptimalConcepts.Name = "tsmiOptimalConcepts";
            this.tsmiOptimalConcepts.Size = new System.Drawing.Size(224, 22);
            this.tsmiOptimalConcepts.Text = "Optimal &Concepts Only";
            this.tsmiOptimalConcepts.Click += new System.EventHandler(this.tsmiOptimalConcepts_Click);
            // 
            // tsmiOptimalConceptsSimple
            // 
            this.tsmiOptimalConceptsSimple.Name = "tsmiOptimalConceptsSimple";
            this.tsmiOptimalConceptsSimple.Size = new System.Drawing.Size(224, 22);
            this.tsmiOptimalConceptsSimple.Text = "Optimal Concepts - Simple";
            this.tsmiOptimalConceptsSimple.Click += new System.EventHandler(this.tsmiOptimalConceptsSimple_Click);
            // 
            // tsmiOptimalConceptsDetailed
            // 
            this.tsmiOptimalConceptsDetailed.Name = "tsmiOptimalConceptsDetailed";
            this.tsmiOptimalConceptsDetailed.Size = new System.Drawing.Size(224, 22);
            this.tsmiOptimalConceptsDetailed.Text = "Optimal Concepts - Detailed";
            this.tsmiOptimalConceptsDetailed.Click += new System.EventHandler(this.TsmiOptimalConceptsDetailed_Click);
            // 
            // tsmiBinaryRelation
            // 
            this.tsmiBinaryRelation.Name = "tsmiBinaryRelation";
            this.tsmiBinaryRelation.Size = new System.Drawing.Size(224, 22);
            this.tsmiBinaryRelation.Text = "Context Matrix";
            this.tsmiBinaryRelation.Click += new System.EventHandler(this.TsmiBinaryRelation_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitMenuItem.Text = "&Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // encodingToolStripMenuItem
            // 
            this.encodingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unicodeToolStripMenuItem,
            this.aNSIToolStripMenuItem,
            this.uTF8ToolStripMenuItem});
            this.encodingToolStripMenuItem.Name = "encodingToolStripMenuItem";
            this.encodingToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.encodingToolStripMenuItem.Text = "Encoding";
            // 
            // unicodeToolStripMenuItem
            // 
            this.unicodeToolStripMenuItem.Name = "unicodeToolStripMenuItem";
            this.unicodeToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.unicodeToolStripMenuItem.Text = "Unicode";
            this.unicodeToolStripMenuItem.Click += new System.EventHandler(this.unicodeToolStripMenuItem_Click);
            // 
            // aNSIToolStripMenuItem
            // 
            this.aNSIToolStripMenuItem.Name = "aNSIToolStripMenuItem";
            this.aNSIToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.aNSIToolStripMenuItem.Text = "ANSI";
            this.aNSIToolStripMenuItem.Click += new System.EventHandler(this.aNSIToolStripMenuItem_Click);
            // 
            // uTF8ToolStripMenuItem
            // 
            this.uTF8ToolStripMenuItem.Checked = true;
            this.uTF8ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uTF8ToolStripMenuItem.Name = "uTF8ToolStripMenuItem";
            this.uTF8ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.uTF8ToolStripMenuItem.Text = "UTF-8";
            this.uTF8ToolStripMenuItem.Click += new System.EventHandler(this.uTF8ToolStripMenuItem_Click);
            // 
            // treeViewBrowser
            // 
            this.treeViewBrowser.Location = new System.Drawing.Point(14, 90);
            this.treeViewBrowser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.treeViewBrowser.Name = "treeViewBrowser";
            this.treeViewBrowser.Size = new System.Drawing.Size(318, 313);
            this.treeViewBrowser.TabIndex = 1;
            this.treeViewBrowser.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewBrowser_NodeMouseClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssLanguage,
            this.tssPerformance,
            this.tssCoveragePercentage,
            this.toolStripStatusLabel1,
            this.pbMain});
            this.statusStrip.Location = new System.Drawing.Point(0, 762);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip.Size = new System.Drawing.Size(1176, 24);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tssLanguage
            // 
            this.tssLanguage.Name = "tssLanguage";
            this.tssLanguage.Size = new System.Drawing.Size(151, 19);
            this.tssLanguage.Text = "Language: Not Yet Selected";
            // 
            // tssPerformance
            // 
            this.tssPerformance.Name = "tssPerformance";
            this.tssPerformance.Size = new System.Drawing.Size(94, 19);
            this.tssPerformance.Text = "Time Taken: N/A";
            // 
            // tssCoveragePercentage
            // 
            this.tssCoveragePercentage.Name = "tssCoveragePercentage";
            this.tssCoveragePercentage.Size = new System.Drawing.Size(147, 19);
            this.tssCoveragePercentage.Text = "Coverage Percentage: N/A";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(182, 19);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // pbMain
            // 
            this.pbMain.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(583, 18);
            this.pbMain.Step = 1;
            // 
            // txtText
            // 
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.Location = new System.Drawing.Point(340, 435);
            this.txtText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtText.Name = "txtText";
            this.txtText.ReadOnly = true;
            this.txtText.Size = new System.Drawing.Size(836, 321);
            this.txtText.TabIndex = 4;
            this.txtText.Text = "";
            // 
            // labelLanguage
            // 
            this.labelLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelLanguage.Location = new System.Drawing.Point(974, 35);
            this.labelLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(55, 13);
            this.labelLanguage.TabIndex = 5;
            this.labelLanguage.Text = "Langauge";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Items.AddRange(new object[] {
            "Auto-Detect",
            "eng",
            "fra",
            "spa",
            "ces",
            "dan",
            "nld",
            "fin",
            "deu",
            "hun",
            "nor",
            "ita",
            "por",
            "ron",
            "rus",
            "arb",
            "none"});
            this.cmbLanguage.Location = new System.Drawing.Point(1045, 31);
            this.cmbLanguage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(116, 23);
            this.cmbLanguage.TabIndex = 6;
            // 
            // txtSummary
            // 
            this.txtSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSummary.Location = new System.Drawing.Point(340, 90);
            this.txtSummary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.Size = new System.Drawing.Size(836, 313);
            this.txtSummary.TabIndex = 7;
            this.txtSummary.Text = "";
            // 
            // lblCoveragePercentage
            // 
            this.lblCoveragePercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCoveragePercentage.AutoSize = true;
            this.lblCoveragePercentage.Location = new System.Drawing.Point(824, 35);
            this.lblCoveragePercentage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCoveragePercentage.Name = "lblCoveragePercentage";
            this.lblCoveragePercentage.Size = new System.Drawing.Size(57, 15);
            this.lblCoveragePercentage.TabIndex = 8;
            this.lblCoveragePercentage.Text = "Coverage";
            // 
            // cmbCoveragePercentage
            // 
            this.cmbCoveragePercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCoveragePercentage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCoveragePercentage.FormattingEnabled = true;
            this.cmbCoveragePercentage.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "60",
            "65",
            "70",
            "75",
            "80",
            "85",
            "90",
            "95",
            "100"});
            this.cmbCoveragePercentage.Location = new System.Drawing.Point(892, 31);
            this.cmbCoveragePercentage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbCoveragePercentage.Name = "cmbCoveragePercentage";
            this.cmbCoveragePercentage.Size = new System.Drawing.Size(74, 23);
            this.cmbCoveragePercentage.TabIndex = 9;
            // 
            // bgwExtraction
            // 
            this.bgwExtraction.WorkerReportsProgress = true;
            this.bgwExtraction.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwExtraction_DoWork);
            this.bgwExtraction.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwExtraction_ProgressChanged);
            this.bgwExtraction.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwExtraction_RunWorkerCompleted);
            // 
            // txtKeywords
            // 
            this.txtKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtKeywords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtKeywords.Location = new System.Drawing.Point(14, 435);
            this.txtKeywords.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtKeywords.Name = "txtKeywords";
            this.txtKeywords.ReadOnly = true;
            this.txtKeywords.Size = new System.Drawing.Size(318, 321);
            this.txtKeywords.TabIndex = 10;
            this.txtKeywords.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Concept Tree";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Summary";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 417);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "Keywords";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(336, 417);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Text";
            // 
            // cmbFont
            // 
            this.cmbFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFont.FormattingEnabled = true;
            this.cmbFont.Items.AddRange(new object[] {
            "8",
            "10",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "32",
            "48"});
            this.cmbFont.Location = new System.Drawing.Point(747, 31);
            this.cmbFont.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbFont.Name = "cmbFont";
            this.cmbFont.Size = new System.Drawing.Size(69, 23);
            this.cmbFont.TabIndex = 16;
            this.cmbFont.SelectedIndexChanged += new System.EventHandler(this.CmbFont_SelectedIndexChanged);
            // 
            // lblFont
            // 
            this.lblFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFont.AutoSize = true;
            this.lblFont.Location = new System.Drawing.Point(707, 35);
            this.lblFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(31, 15);
            this.lblFont.TabIndex = 15;
            this.lblFont.Text = "Font";
            // 
            // OpenTextBoxMenuItem
            // 
            this.OpenTextBoxMenuItem.Name = "OpenTextBoxMenuItem";
            this.OpenTextBoxMenuItem.Size = new System.Drawing.Size(180, 22);
            this.OpenTextBoxMenuItem.Text = "Open Textbox";
            this.OpenTextBoxMenuItem.Click += new System.EventHandler(this.OpenTextBoxMenuItem_Click);
            // 
            // ConceptualBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 786);
            this.Controls.Add(this.cmbFont);
            this.Controls.Add(this.lblFont);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKeywords);
            this.Controls.Add(this.cmbCoveragePercentage);
            this.Controls.Add(this.lblCoveragePercentage);
            this.Controls.Add(this.txtSummary);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.labelLanguage);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.treeViewBrowser);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ConceptualBrowserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conceptual Browser";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.TreeView treeViewBrowser;
        private System.Windows.Forms.ToolStripMenuItem exportMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tssLanguage;
        private System.Windows.Forms.RichTextBox txtText;
        private System.Windows.Forms.ToolStripMenuItem encodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unicodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aNSIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uTF8ToolStripMenuItem;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptimalConcepts;
        private System.Windows.Forms.RichTextBox txtSummary;
        private System.Windows.Forms.ToolStripStatusLabel tssPerformance;
        private System.Windows.Forms.Label lblCoveragePercentage;
        private System.Windows.Forms.ComboBox cmbCoveragePercentage;
        private System.Windows.Forms.ToolStripStatusLabel tssCoveragePercentage;
        private System.ComponentModel.BackgroundWorker bgwExtraction;
        private System.Windows.Forms.ToolStripProgressBar pbMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptimalConceptsSimple;
        private System.Windows.Forms.RichTextBox txtKeywords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbFont;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptimalConceptsDetailed;
        private System.Windows.Forms.ToolStripMenuItem tsmiImport;
        private System.Windows.Forms.ToolStripMenuItem tsmiBinaryRelation;
        private System.Windows.Forms.ToolStripMenuItem OpenTextBoxMenuItem;
    }
}

