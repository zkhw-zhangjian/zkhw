using System;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class metachysis_record : Form
    {

        public string metachysis_reasonn = "";
        public string metachysis_time = ""; 
            public string metachysis_code = "1"; 
        public metachysis_record()
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
            metachysis_reasonn = this.textBox1.Text;
            if (metachysis_reasonn == "") { MessageBox.Show("输血原因不能为空"); return; }
            metachysis_code = "2";
            metachysis_time = this.dateTimePicker1.Text;
            this.DialogResult = DialogResult.OK;

        }
    }
}
