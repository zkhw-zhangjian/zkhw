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
    public partial class aUhealthcheckTooth : Form
    {
        public string hypodontia1 = "";
        public string hypodontia2 = "";
        public string hypodontia3 = "";
        public string hypodontia4 = "";

        public string caries1 = "";
        public string caries2 = "";
        public string caries3 = "";
        public string caries4 = "";

        public string denture1 = "";
        public string denture2 = "";
        public string denture3 = "";
        public string denture4 = "";

        public aUhealthcheckTooth()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            hypodontia1 = this.textBox9.Text;
            hypodontia2 = this.textBox1.Text;
            hypodontia3 = this.textBox2.Text;
            hypodontia4 = this.textBox3.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            caries1 = this.textBox7.Text;
            caries2 = this.textBox6.Text;
            caries3 = this.textBox5.Text;
            caries4 = this.textBox4.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            denture1 = this.textBox12.Text;
            denture2 = this.textBox11.Text;
            denture3 = this.textBox10.Text;
            denture4 = this.textBox8.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
