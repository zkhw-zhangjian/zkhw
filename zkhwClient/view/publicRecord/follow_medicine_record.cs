using System;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class follow_medicine_record : Form
    {

        public string drug_name = "";
        public string num = "";
        public string dosage = "";
        public follow_medicine_record()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void follow_medicine_record_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            drug_name = this.comboBox1.Text;
            num = this.numericUpDown1.Value.ToString();
            dosage = this.textBox1.Text.ToString();
            if (drug_name != "" && num != "" && dosage != "")
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("信息不完整");
            }
        }
    }
}
