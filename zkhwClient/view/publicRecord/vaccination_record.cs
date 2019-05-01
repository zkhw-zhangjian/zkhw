using System;
using System.Windows.Forms;

namespace zkhwClient.view.publicRecord
{
    public partial class vaccination_record : Form
    {
        public string vaccination_name = "";
        public string vaccination_time = "";
        public string vaccination_organ = "";
        public vaccination_record()
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
            vaccination_name = this.textBox3.Text;
            vaccination_organ = this.textBox2.Text;
            vaccination_time = this.dateTimePicker1.Text.ToString();
            if (vaccination_name != "" && vaccination_organ != "" && vaccination_time != "") {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("信息不完整");
            }
        }
    }
}
