using System;
using System.Windows.Forms;

namespace zkhwClient.view.publicRecord
{
    public partial class take_medicine_record : Form
    {
        public string drug_name = "";
        public string drug_usage = "";
        public string drug_use = "";
        public string drug_time = "";
        public string drug_type = "";
        public take_medicine_record()
        {
            InitializeComponent();
        }

        private void take_medicine_record_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            drug_name = this.textBox3.Text;
            drug_usage = this.textBox1.Text;
            drug_use = this.textBox2.Text;
            drug_time = this.textBox4.Text;
            drug_type = this.comboBox1.Text;
            if (drug_name != "" && drug_usage != "" && drug_use != "") {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("信息不完整");
            }
        }
    }
}
