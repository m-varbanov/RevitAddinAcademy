using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;

namespace RevitAddinAcademy
{
    public partial class FrmToDoList : Form
    {
        public FrmToDoList()
        {
            InitializeComponent();
        }

      

        private void btnButton1_Click(object sender, EventArgs e)
        {
            TaskDialog.Show("Test", "I pressed Button 1");
        }

        private void btnButton2_Click(object sender, EventArgs e)
        {
            tbxTextBox.Text = "This is Button 2 text";
        }

        private void btnButton3_Click(object sender, EventArgs e)
        {
            lbxText.Items.Add("This is Button 3 text");
        }

        private void lbxText_DoubleClick(object sender, EventArgs e)
        {
            tbxTextBox.Text = "I double clicked an item";
        }
    }
}
