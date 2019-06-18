﻿namespace ConceptualBrowser.FormUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConceptualBrowserForm));
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optimalConceptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optimalConceptsWithObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.ctxMenuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsAddToStopWords = new System.Windows.Forms.ToolStripMenuItem();
            this.txtText = new System.Windows.Forms.RichTextBox();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.txtSummary = new System.Windows.Forms.RichTextBox();
            this.lblCoveragePercentage = new System.Windows.Forms.Label();
            this.cmbCoveragePercentage = new System.Windows.Forms.ComboBox();
            this.bgwExtraction = new System.ComponentModel.BackgroundWorker();
            this.menuStripMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.ctxMenuTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.encodingToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1008, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenuItem,
            this.exportMenuItem,
            this.exitMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Name = "openFileMenuItem";
            this.openFileMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openFileMenuItem.Text = "&Open File";
            this.openFileMenuItem.Click += new System.EventHandler(this.openFileMenuItem_Click);
            // 
            // exportMenuItem
            // 
            this.exportMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optimalConceptsToolStripMenuItem,
            this.optimalConceptsWithObjectsToolStripMenuItem});
            this.exportMenuItem.Name = "exportMenuItem";
            this.exportMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exportMenuItem.Text = "E&xport";
            // 
            // optimalConceptsToolStripMenuItem
            // 
            this.optimalConceptsToolStripMenuItem.Name = "optimalConceptsToolStripMenuItem";
            this.optimalConceptsToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.optimalConceptsToolStripMenuItem.Text = "Optimal &Concepts";
            this.optimalConceptsToolStripMenuItem.Click += new System.EventHandler(this.optimalConceptsToolStripMenuItem_Click);
            // 
            // optimalConceptsWithObjectsToolStripMenuItem
            // 
            this.optimalConceptsWithObjectsToolStripMenuItem.Name = "optimalConceptsWithObjectsToolStripMenuItem";
            this.optimalConceptsWithObjectsToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.optimalConceptsWithObjectsToolStripMenuItem.Text = "Optimal Concepts with Objects";
            this.optimalConceptsWithObjectsToolStripMenuItem.Click += new System.EventHandler(this.optimalConceptsWithObjectsToolStripMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exitMenuItem.Text = "&Exit";
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
            this.unicodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.unicodeToolStripMenuItem.Text = "Unicode";
            this.unicodeToolStripMenuItem.Click += new System.EventHandler(this.unicodeToolStripMenuItem_Click);
            // 
            // aNSIToolStripMenuItem
            // 
            this.aNSIToolStripMenuItem.Name = "aNSIToolStripMenuItem";
            this.aNSIToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aNSIToolStripMenuItem.Text = "ANSI";
            this.aNSIToolStripMenuItem.Click += new System.EventHandler(this.aNSIToolStripMenuItem_Click);
            // 
            // uTF8ToolStripMenuItem
            // 
            this.uTF8ToolStripMenuItem.Checked = true;
            this.uTF8ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uTF8ToolStripMenuItem.Name = "uTF8ToolStripMenuItem";
            this.uTF8ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.uTF8ToolStripMenuItem.Text = "UTF-8";
            this.uTF8ToolStripMenuItem.Click += new System.EventHandler(this.uTF8ToolStripMenuItem_Click);
            // 
            // treeViewBrowser
            // 
            this.treeViewBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewBrowser.Location = new System.Drawing.Point(0, 55);
            this.treeViewBrowser.Name = "treeViewBrowser";
            this.treeViewBrowser.Size = new System.Drawing.Size(285, 601);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 659);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tssLanguage
            // 
            this.tssLanguage.Name = "tssLanguage";
            this.tssLanguage.Size = new System.Drawing.Size(151, 17);
            this.tssLanguage.Text = "Language: Not Yet Selected";
            // 
            // tssPerformance
            // 
            this.tssPerformance.Name = "tssPerformance";
            this.tssPerformance.Size = new System.Drawing.Size(94, 17);
            this.tssPerformance.Text = "Time Taken: N/A";
            // 
            // tssCoveragePercentage
            // 
            this.tssCoveragePercentage.Name = "tssCoveragePercentage";
            this.tssCoveragePercentage.Size = new System.Drawing.Size(147, 17);
            this.tssCoveragePercentage.Text = "Coverage Percentage: N/A";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(99, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // pbMain
            // 
            this.pbMain.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(500, 16);
            this.pbMain.Step = 1;
            // 
            // ctxMenuTreeView
            // 
            this.ctxMenuTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAddToStopWords});
            this.ctxMenuTreeView.Name = "ctxMenuTreeView";
            this.ctxMenuTreeView.Size = new System.Drawing.Size(214, 26);
            // 
            // tsAddToStopWords
            // 
            this.tsAddToStopWords.Name = "tsAddToStopWords";
            this.tsAddToStopWords.Size = new System.Drawing.Size(213, 22);
            this.tsAddToStopWords.Text = "Add to Stop/Empty Words";
            // 
            // txtText
            // 
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.Location = new System.Drawing.Point(291, 356);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(717, 300);
            this.txtText.TabIndex = 4;
            this.txtText.Text = "";
            // 
            // labelLanguage
            // 
            this.labelLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLanguage.Location = new System.Drawing.Point(806, 30);
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
            this.cmbLanguage.Location = new System.Drawing.Point(875, 27);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(121, 21);
            this.cmbLanguage.TabIndex = 6;
            // 
            // txtSummary
            // 
            this.txtSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSummary.Location = new System.Drawing.Point(291, 56);
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.Size = new System.Drawing.Size(717, 294);
            this.txtSummary.TabIndex = 7;
            this.txtSummary.Text = "";
            // 
            // lblCoveragePercentage
            // 
            this.lblCoveragePercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCoveragePercentage.AutoSize = true;
            this.lblCoveragePercentage.Location = new System.Drawing.Point(588, 30);
            this.lblCoveragePercentage.Name = "lblCoveragePercentage";
            this.lblCoveragePercentage.Size = new System.Drawing.Size(53, 13);
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
            this.cmbCoveragePercentage.Location = new System.Drawing.Point(660, 28);
            this.cmbCoveragePercentage.Name = "cmbCoveragePercentage";
            this.cmbCoveragePercentage.Size = new System.Drawing.Size(121, 21);
            this.cmbCoveragePercentage.TabIndex = 9;
            // 
            // bgwExtraction
            // 
            this.bgwExtraction.WorkerReportsProgress = true;
            this.bgwExtraction.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwExtraction_DoWork);
            this.bgwExtraction.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwExtraction_ProgressChanged);
            this.bgwExtraction.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwExtraction_RunWorkerCompleted);
            // 
            // ConceptualBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 681);
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
            this.Name = "ConceptualBrowserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conceptual Browser";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ctxMenuTreeView.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip ctxMenuTreeView;
        private System.Windows.Forms.ToolStripMenuItem tsAddToStopWords;
        private System.Windows.Forms.RichTextBox txtText;
        private System.Windows.Forms.ToolStripMenuItem encodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unicodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aNSIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uTF8ToolStripMenuItem;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.ToolStripMenuItem optimalConceptsToolStripMenuItem;
        private System.Windows.Forms.RichTextBox txtSummary;
        private System.Windows.Forms.ToolStripStatusLabel tssPerformance;
        private System.Windows.Forms.Label lblCoveragePercentage;
        private System.Windows.Forms.ComboBox cmbCoveragePercentage;
        private System.Windows.Forms.ToolStripStatusLabel tssCoveragePercentage;
        private System.ComponentModel.BackgroundWorker bgwExtraction;
        private System.Windows.Forms.ToolStripProgressBar pbMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem optimalConceptsWithObjectsToolStripMenuItem;
    }
}

