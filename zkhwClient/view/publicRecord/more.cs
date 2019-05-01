using System;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class more : Form
    {
        public string other_symptom = "";
        public more()
        {
            InitializeComponent();
        }
        private void more_Load(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in this.panel1.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        other_symptom += "," + ck.Text;
                    }
                }
            }
            if (other_symptom != null && other_symptom != "")
            {
                other_symptom = other_symptom.Substring(1);
            }
            this.DialogResult = DialogResult.OK;
        }


    }
}
