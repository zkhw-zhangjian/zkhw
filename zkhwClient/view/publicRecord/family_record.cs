using System;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class family_record : Form
    {

        public string relation = "";
        public string disease_name = ""; 
            public string disease_type = ""; 
        public family_record()
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
            relation = this.comboBox1.Text;
            if (relation == "父亲") {
                relation = "1";
            } else if (relation == "母亲") {
                relation = "2";
            }
            else if (relation == "兄弟姐妹")
            {
                relation = "3";
            }
            else if (relation == "子女")
            {
                relation = "4";
            }
            foreach (Control ctr in this.panel1.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        disease_name += "," + ck.Text;
                        disease_type += "," + ck.Tag.ToString();
                    }
                }
            }
            if (disease_name != null && disease_name != "")
            {
                disease_name = disease_name.Substring(1);
                disease_type = disease_type.Substring(1);
            }
            this.DialogResult = DialogResult.OK;

        }
    }
}
