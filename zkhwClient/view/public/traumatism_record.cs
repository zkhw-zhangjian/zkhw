using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class traumatism_record : Form
    {

        public string traumatism_name = "";
        public string traumatism_time = "";
        public string traumatism_code = "1";
        public traumatism_record()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void follow_medicine_record_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            traumatism_name = this.textBox1.Text;
            if (traumatism_name == "") { MessageBox.Show("外伤名称不能为空"); return; }
            traumatism_time = this.dateTimePicker1.Text;
            traumatism_code = "2";
            this.DialogResult = DialogResult.OK;

        }
    }
}
