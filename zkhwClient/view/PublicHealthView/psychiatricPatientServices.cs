using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.view.PublicHealthView;

namespace zkhwClient.view
{
    public partial class psychiatricPatientServices : Form
    {
        public psychiatricPatientServices()
        {
            InitializeComponent();
        }

        private void tabControl1_Selected(object sender, EventArgs e)
        {
            int page = this.tabControl1.SelectedIndex;
            if (page == 0)
            {
                psychiatricPatientServicesG pR = new psychiatricPatientServicesG();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();


            }
            else
            {
                psychiatricPatientServicesS pR = new psychiatricPatientServicesS();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();

            }
        }

        private void psychiatricPatientServices_Load(object sender, EventArgs e)
        {
            tabControl1_Selected(null,null);
        }
    }
}
