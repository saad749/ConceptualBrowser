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
using ConceptualBrowser.Business.Entities;

namespace ConceptualBrowser.FormUI
{
    public partial class ConceptualBrowserForm : Form
    {
        public ConceptualBrowserForm()
        {
            InitializeComponent();
        }

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            treeViewBrowser.Nodes.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if(!String.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                try
                {
                    string text = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
                    txtText.Text = text;

                    ConceptExtraction ce = new ConceptExtraction();
                    List<OptimalConceptTreeItem> optimals = ce.Extract(text);
                    //treeViewBrowser.Nodes.Add(optimals[0].OptimalConcept.ConceptName);
                    FillNode(optimals, null);
                    //AddListToTree(optimals);
                }
                catch (IOException)
                {
                    MessageBox.Show("Input Exception");
                }

            }
        }

        private void FillNode(List<OptimalConceptTreeItem> optimals, TreeNode node)
        {
            int parentID = node != null ? (int)node.Tag : 0;

            TreeNodeCollection nodesCollection = node != null ? node.Nodes : treeViewBrowser.Nodes;

            foreach (OptimalConceptTreeItem optimal in optimals.Where(i => i.ParentId == parentID))
            {
                var newNode = nodesCollection.Add(optimal.OptimalConcept.ConceptName);//, optimal.Name);
                newNode.Tag = optimal.Id;

                FillNode(optimals, newNode);
            }
        }

    }
}
