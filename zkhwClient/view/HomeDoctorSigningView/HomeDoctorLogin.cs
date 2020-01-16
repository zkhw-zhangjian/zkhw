using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.view.setting;

namespace zkhwClient.view.HomeDoctorSigningView
{
    public partial class HomeDoctorLogin : Form
    {
        public String zwAddress = "";
        public String zwaddresstest = "";
        public String zwpass = "";
        public HomeDoctorLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string loginname = this.textBox1.Text;
            if (loginname != null && !"".Equals(loginname))
            {
                //string url = "http://113.142.73.218:8080/qianyue/login.do?username=" + loginname + "&password=FE173FFCD3AF4298A196CDCD63CDA465&type=gw";
                //string url = "http://113.142.73.218:8080/qianyue/login.do?username=hj&password=FE173FFCD3AF4298A196CDCD63CDA465&type=gw";
                if (zwaddresstest == "1") {
                    loginname = "hj";
                    zwpass = "FE173FFCD3AF4298A196CDCD63CDA465";
                }
               
                string url = String.Format("{0}?username={1}&password={2}&type=gw",zwAddress, loginname, zwpass);
                FormJY f2 = new FormJY();
                f2.url = url;
                f2.ShowDialog();
            }
        }

        private void HomeDoctorLogin_Load(object sender, EventArgs e)
        {
            this.textBox1.Text=frmLogin.loginname;
            zwpass = frmLogin.passw;
            button1_Click(sender,e);
        }
    }
}
