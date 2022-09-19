using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAddinAcademy
{
    public partial class FrmWallsFromLines : Form
    {
        public FrmWallsFromLines(List<string> wallTypes, List<string> lineStyles)
        {
            InitializeComponent();

            foreach(string wallType in wallTypes)
            {
                //by using "this" we specify this specific instance of the form 
                this.cmbWallTypes.Items.Add(wallType);

            }

            foreach(string lineStyle in lineStyles)
            {
                this.cmbLineStyles.Items.Add(lineStyle);
            }


            //we are prefilling the box to force the user to choose an item
            this.cmbWallTypes.SelectedIndex = 0;
            this.cmbLineStyles.SelectedIndex = 0;



        }

        private void FrmWallsFromLines_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
