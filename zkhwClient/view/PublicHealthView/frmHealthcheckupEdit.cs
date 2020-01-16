using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view.PublicHealthView
{
    public partial class frmHealthcheckupEdit : Form
    {
        public string id_number = "";
        public string name = "";
        public string barcode = "";
        public frmHealthcheckupEdit()
        {
            InitializeComponent();
        }

        private void frmHealthcheckupEdit_Load(object sender, EventArgs e)
        {
            label47.Text = name+"的健康体检表";
            label47.Left = (this.Width - label47.Width) / 2;
            if (Common.m_nWindwMetricsY <=900)
            {
                this.Height = 641;
            }
        }
    }
}
