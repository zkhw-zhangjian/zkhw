using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zkhwClient
{
    public partial class addUser : Form
    {
        public addUser()
        {
            InitializeComponent();
        }

        private void addUser_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/insert.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
        }
        //屏蔽双击
        protected override void DefWndProc(ref Message m)
        {
            byte msg = 0x00A3;
            if (m.Msg == Convert.ToInt32(msg))
            {
                return;
            }
            base.DefWndProc(ref m);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = this.textBox1.Text;
            string pwd = this.textBox2.Text;
            string pwd1 = this.textBox3.Text;

            string lasttime = DateTime.Now.ToString("yyyy-MM-dd");
            string loginnumber = "1";
            string depaid = "1";
            string name = "1";
            string type = "1";


            if (name != null && !"".Equals(name) && pwd.Equals(pwd1))
            {
                bean.UserInfo ui = new bean.UserInfo();
                ui.UserName = userName;
                ui.Password = pwd;
                ui.Lasttime = lasttime;
                ui.Loginnumber = loginnumber;
                ui.Depaid = depaid;
                ui.Name = name;
                ui.Type = type;


                service.UserService us = new service.UserService();
                bool istrue = us.addUser(ui);
                if (istrue)
                {
                    this.Hide();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("用户名已重复，请重新输入！");
                }
            }
            else
            {
                MessageBox.Show("用户名或密码不能为空或两次密码输入不一致！");
            }
        }
    }
}
