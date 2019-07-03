using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Xml;
using zkhwClient.dao;
using zkhwClient.utils;
using zkhwClient.view.setting;

namespace zkhwClient
{
    public partial class frmLogin : Form
    {
        public static string loginname = null;
        public static string name = null;
        public static string passw = null;
        public static string dataPassw = "";
        public static List<string> listpower = null;
        public static string organCode = null;
        public static string organName = null;
        public static string userCode = null;
        service.loginLogService lls = new service.loginLogService();
        service.UserService us = new service.UserService();
        UserDao udao = new UserDao();
        XmlDocument xmlDoc = new XmlDocument();
        tjcheckDao tjdao = new tjcheckDao();
        jkInfoDao jkdao = new jkInfoDao();
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
            LoginSys();
        }
        private void LoginSys()
        {
            loginname = this.comboBox1.Text;
            if (loginname=="admin") {
                this.button3.Visible = true;
                DataTable sumret = service.UserService.sumUser();
                if (sumret!=null&&sumret.Rows.Count>=1) {
                    DialogResult rr = MessageBox.Show("未初始化数据，是否继续?", "操作提示", MessageBoxButtons.YesNo);
                    int tt = (int)rr;
                    if (tt == 7)
                    {
                        return;
                    }
                }
            }
            passw = this.txtPassword.Text;
            string md5passw = Md5.HashString(passw);
            //用户登录 获取用户的账号和密码并判断          
            //DataTable ret = service.UserService.UserExists(comboBox1.Text, txtPassword.Text);
            DataTable ret = service.UserService.UserExists(comboBox1.Text, md5passw);
            if (ret.Rows.Count > 0)
            {  //获取当前登录用户的机构
                userCode = ret.Rows[0]["user_code"].ToString();
                if (!"admin".Equals(this.comboBox1.Text))
                {
                    organCode = ret.Rows[0]["organ_code"].ToString();

                    if (udao.checkOrganNameBycode(organCode) == null || udao.checkOrganNameBycode(organCode).Rows.Count <= 0)
                    { }
                    else {
                        organName = udao.checkOrganNameBycode(organCode).Rows[0]["organ_name"].ToString();
                    } 
                    
                }
                name = ret.Rows[0]["user_name"].ToString();
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "登录系统！";
                lb.type = "1";
                if (lb.name != "admin" && lb.name != "" && lb.name != null)
                {
                    lls.addCheckLog(lb);
                }
                /************/
                //string fpath = Application.StartupPath + "\\sysstem.ini";
                //sysstem.UpdateInfo(fpath);
                /*****end******/
                this.Hide();
                frmMain main = new frmMain();
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
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/csh.png");
            this.pictureBox1.Image = Image.FromFile(@str + "/images/logo.png");
            DataTable dd = us.listUser();
            this.comboBox1.DataSource = dd;//绑定数据源
            this.comboBox1.DisplayMember = "username";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "username";//操作时获取的值
            //删除文件夹
            DeleteDir1(@"E:\Examine\xdt");
            DeleteDir1(@"E:\Examine\bc");
            //监听心电图和B超
            FSWControl.WatcherStrat(@"E:\Examine\xdt", "*.xml", true, true);
            FSWControl.WatcherStratBchao(@"E:\Examine\bc", "*.xml", true, true);
        }
       
        /// 删除文件夹及其内容
        /// </summary>
        /// <param name="dir"></param>
        public void DeleteDir1(string dir)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(dir);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                //去除文件的只读属性
                System.IO.File.SetAttributes(dir, System.IO.FileAttributes.Normal);
                //判断文件夹是否还存在

                if (Directory.Exists(dir))
                {
                    System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(dir);
                    Empty1(directory);
                }
            }
            catch// 异常处理
            {

            }
        }
        public void Empty1(System.IO.DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                LoginSys();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IsInternetAvailable())
            {
                basicInfoInit baseinfo = new basicInfoInit();
                if (baseinfo.ShowDialog() == DialogResult.OK) {
                    this.comboBox1.DataSource = null;
                    DataTable dd = us.listUser();
                    this.comboBox1.DataSource = dd;//绑定数据源
                    this.comboBox1.DisplayMember = "username";//显示给用户的数据集表项
                    this.comboBox1.ValueMember = "username";//操作时获取的值
                }
            }
            else
            {
                MessageBox.Show("电脑未连接外网,请检查网络!");
            }
        }
        //判断是否连接 外网
        private bool IsInternetAvailable()
        {
            try
            {
                Dns.GetHostEntry("www.baidu.com"); //using System.Net;
                return true;
            }
            catch (SocketException ex)
            {
                return false;
            }
        }
    }
}
