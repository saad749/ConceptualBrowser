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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if(!String.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                try
                {
                    string text = File.ReadAllText(openFileDialog.FileName);
                    txtText.Text = text;

                    ConceptExtraction ce = new ConceptExtraction();
                    List<OptimalConcept> optimals = ce.Extract(text);
                    AddListToTree(optimals);
                }
                catch (IOException)
                {
                    MessageBox.Show("Input Exception");
                }

            }
        }

        private void AddListToTree(List<OptimalConcept> optimals)
        {
            TreeNode node = new TreeNode(optimals[0].ConceptName);
            List<TreeNode[]> childrens = new List<TreeNode[]>();
            int childrensAdded = 0;
            for( int i = 1; i < optimals.Count; i+=2)
            {
                TreeNode child1 = new TreeNode(optimals[i].ConceptName);
                TreeNode child2 = new TreeNode(optimals[i + 1].ConceptName);
                childrens.Add(new TreeNode[] { child1, child2 });
            }

            //for (int i = 0; i < childrens.Count; i++)
            //{
            //    childrens[i][0].
            //}
        }
    }
}
