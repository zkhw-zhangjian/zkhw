using System;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class resident_diseases : Form
    {

        public string disease_name = "";
        public string disease_date = "";
        public string disease_type = "1";
        public string _displaydt = "0";
        public resident_diseases()
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

            if(_displaydt=="1")
            {
                label1.Visible = false;
                comboBox1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(_displaydt=="0")
            { 
                disease_name = this.comboBox1.Text;
                if (this.comboBox1.Text == "高血压")
                {
                    disease_type = "2";
                }
                else if (this.comboBox1.Text == "糖尿病")
                {
                    disease_type = "3";
                }
                else if (this.comboBox1.Text == "冠心病")
                {
                    disease_type = "4";
                }
                else if (this.comboBox1.Text == "慢性阻塞性肺疾病")
                {
                    disease_type = "5";
                }
                else if (this.comboBox1.Text == "恶性肿瘤")
                {
                    disease_type = "6";
                }
                else if (this.comboBox1.Text == "脑卒中")
                {
                    disease_type = "7";
                }
                else if (this.comboBox1.Text == "严重精神障碍")
                {
                    disease_type = "8";
                }
                else if (this.comboBox1.Text == "结核病")
                {
                    disease_type = "9";
                }
                else if (this.comboBox1.Text == "肝炎")
                {
                    disease_type = "10";
                }
                else if (this.comboBox1.Text == "其它法定传染病")
                {
                    disease_type = "11";
                }
                else if (this.comboBox1.Text == "职业病")
                {
                    disease_type = "12";
                }
                else if (this.comboBox1.Text == "职业病")
                {
                    disease_type = "13";
                }
                else
                {

                }

            }

            disease_date = this.dateTimePicker1.Text;
            this.DialogResult = DialogResult.OK;

        }
    }
}
