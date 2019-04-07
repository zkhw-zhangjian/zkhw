using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using zkhwClient.view.setting;

namespace zkhwClient
{
    public partial class frmLogin : Form
    {
        public static string name = null;
        public static string passw = null;
        public static string dataPassw = "";
        public static List<string> listpower = null;
        public static string organCode = null;
        service.loginLogService lls = new service.loginLogService();
        service.UserService us = new service.UserService();

        public frmLogin()
        {
            InitializeComponent();
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
            System.Environment.Exit(0);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            //用户登录 获取用户的账号和密码并判断          
            DataTable ret = service.UserService.UserExists(comboBox1.Text, txtPassword.Text);
            if (ret.Rows.Count == 1)
            {  //获取当前登录用户的机构
                organCode = ret.Rows[0]["organ_code"].ToString();

                name = this.comboBox1.Text;
                passw = this.txtPassword.Text;

                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "登录系统！";
                if (lb.name != "admin" && lb.name != "" && lb.name != null)
                {
                    lls.addCheckLog(lb);
                }
                this.Hide();
                frmMain1 main = new frmMain1();
                main.Show();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void frmLogin_Load_1(object sender, EventArgs e)
        {
            this.label3.Text = "中科弘卫一体化查体系统";//标题
            this.label3.ForeColor = Color.Blue;
            label3.Font = new Font("微软雅黑", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label3.BringToFront();
            this.label4.Text = "24小时服务电话：4008150101";//标题
            string str = Application.StartupPath;//项目路径   
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/login1.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/tuichu.png");
            this.pictureBox1.Image = Image.FromFile(@str + "/images/logo.png");
            DataTable dd = us.listUser();
            this.comboBox1.DataSource = dd;//绑定数据源
            this.comboBox1.DisplayMember = "username";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "username";//操作时获取的值 

            //监听有没有B超的文件生成


            //是否启动监听AOUP

            //开启监控
            //FileWatcher.WatcheDirForAoup();




        }
    }
}
