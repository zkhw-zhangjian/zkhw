using System;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class operation_record : Form
    {

        public string operation_name = "";
        public string operation_time = "";
        public string operation_code = "1";
        public operation_record()
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
            operation_name = this.textBox1.Text;
            if (operation_name == "") { MessageBox.Show("手术名称不能为空");return; }
            operation_time = this.dateTimePicker1.Text;
            operation_code = "2";
            this.DialogResult = DialogResult.OK;

        }
    }
}
