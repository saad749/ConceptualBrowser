using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConceptualBrowser.FormUI
{
    public partial class TextForm : Form
    {
        public string UserText { get; set; }
        public TextForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            UserText = txtUserInput.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();

            return;
        }
    }
}
