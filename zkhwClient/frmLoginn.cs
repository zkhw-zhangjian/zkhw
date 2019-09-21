using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using zkhwClient.dao;
using zkhwClient.utils;
using zkhwClient.view.setting;

namespace zkhwClient
{
    public partial class frmLoginn : Form
    {
        public static string loginname = null;
        public static string name = null;
        public static string passw = null;
        public static string dataPassw = "";
        public static List<string> listpower = null;
        public static string organCode = null;
        public static string organName = null;
        public static string userCode = null;
        public static string user_Name = null;
        service.loginLogService lls = new service.loginLogService();
        service.UserService us = new service.UserService();
        UserDao udao = new UserDao();
        XmlDocument xmlDoc = new XmlDocument();
        tjcheckDao tjdao = new tjcheckDao();
        jkInfoDao jkdao = new jkInfoDao();

        private float xMy;//定义当前窗体的宽度
        private float yMy;//定义当前窗体的高度

        public frmLoginn()
        {
            InitializeComponent();

            xMy = this.Width;
            yMy = this.Height;
            Common.setTag(this);
        }
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
        private void frmLoginn_Load(object sender, EventArgs e)
        {
            if (IsInternetAvailable())
            {
                this.button2.BackColor = Color.FromArgb(77, 177, 81);
            }
            else
            {
                this.button2.BackColor = Color.FromArgb(170, 171, 171);
            }
            DataTable dd = us.listUserForLogin();
            this.comboBox1.DataSource = dd;//绑定数据源
            this.comboBox1.DisplayMember = "displayname";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "username";//操作时获取的值
            //删除文件夹
            DeleteDir1(@"E:\Examine\xdt");
            DeleteDir1(@"E:\Examine\bc");
            //监听心电图和B超
            FSWControl.WatcherStrat(@"E:\Examine\xdt", "*.xml", true, true);
            FSWControl.WatcherStratBchao(@"E:\Examine\bc", "*.xml", true, true);
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                LoginSys();
            }
        }
        private void LoginSys()
        {
            loginname = this.comboBox1.Text;
            if (loginname == "admin")
            {
                DataTable sumret = service.UserService.sumUser();
                if (sumret != null && sumret.Rows.Count >= 1)
                {
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
            //特殊处理下用户名
            string username = "";
            try
            {
                DataRowView dv = comboBox1.Items[comboBox1.SelectedIndex] as DataRowView;
                username = dv.Row["username"].ToString();
            }
            catch
            {
                username = comboBox1.Text;
            }
            //用户登录 获取用户的账号和密码并判断          
            //DataTable ret = service.UserService.UserExists(comboBox1.Text, txtPassword.Text);
            DataTable ret = service.UserService.UserExists(username, md5passw);
            if (ret.Rows.Count > 0)
            {  //获取当前登录用户的机构
                userCode = ret.Rows[0]["user_code"].ToString();
                if (!"admin".Equals(username))
                {
                    organCode = ret.Rows[0]["organ_code"].ToString();

                    if (udao.checkOrganNameBycode(organCode) == null || udao.checkOrganNameBycode(organCode).Rows.Count <= 0)
                    { }
                    else
                    {
                        organName = udao.checkOrganNameBycode(organCode).Rows[0]["organ_name"].ToString();
                    }

                }
                loginname = ret.Rows[0]["username"].ToString();
                name = ret.Rows[0]["user_name"].ToString();
                user_Name = name;
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
                string fpath = Application.StartupPath + "\\sysstem.ini";
                sysstem.UpdateInfo(fpath);

                string spath = Application.StartupPath + "/log.txt";
                FileInfo f = new FileInfo(spath);
                double fm = f.Length / 1024.0 / 1024.0;
                if (fm >= 1)   //文件大于1M就删除掉
                {
                    File.WriteAllText(spath, string.Empty);
                }
                /*****end******/
                this.Hide();
                //frmMain main = new frmMain(); 
                frmMainm main = new frmMainm();
                main.Show();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginSys();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (IsInternetAvailable())
            {
                basicInfoInit baseinfo = new basicInfoInit();
                if (baseinfo.ShowDialog() == DialogResult.OK)
                {
                    this.comboBox1.DataSource = null;
                    DataTable dd = us.listUserForLogin();
                    this.comboBox1.DataSource = dd;//绑定数据源
                    this.comboBox1.DisplayMember = "displayname";//显示给用户的数据集表项
                    this.comboBox1.ValueMember = "username";//操作时获取的值
                    //DataTable dd = us.listUser();
                    //this.comboBox1.DataSource = dd;//绑定数据源
                    //this.comboBox1.DisplayMember = "username";//显示给用户的数据集表项
                    //this.comboBox1.ValueMember = "username";//操作时获取的值
                }
            }
            else
            {
                MessageBox.Show("电脑未连接外网,请检查网络!");
            }
        }

        private void frmLoginn_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / xMy;
            float newy = (this.Height) / yMy;
            Common.setControls(newx, newy, this);
        }

        //判断是否连接 外网
        private bool IsInternetAvailable()
        {
            try
            {
                Ping ping = new Ping();
                PingReply pr = ping.Send("baidu.com");
                if (pr.Status == IPStatus.Success)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
