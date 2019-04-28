using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using zkhwClient.dao;
using zkhwClient.utils;
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
        XmlDocument xmlDoc = new XmlDocument();
        string path = @"config.xml";
        XmlNode node;
        string shenghuapath = "";
        string xuechangguipath = "";
        tjcheckDao tjdao = new tjcheckDao();
        jkInfoDao jkdao = new jkInfoDao();
        private OleDbDataAdapter oda = null;
        private DataSet myds_data = null;
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
            //passw = this.txtPassword.Text;
            //string md5passw= Md5.HashString(passw);
            //用户登录 获取用户的账号和密码并判断          
            DataTable ret = service.UserService.UserExists(comboBox1.Text, txtPassword.Text);
            if (ret.Rows.Count == 1)
            {  //获取当前登录用户的机构
                organCode = ret.Rows[0]["organ_code"].ToString();
                name = this.comboBox1.Text;
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "登录系统！";
                lb.type = "1";
                if (lb.name != "admin" && lb.name != "" && lb.name != null)
                {
                    lls.addCheckLog(lb);
                }
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

            this.pictureBox1.Image = Image.FromFile(@str + "/images/logo.png");
            DataTable dd = us.listUser();
            this.comboBox1.DataSource = dd;//绑定数据源
            this.comboBox1.DisplayMember = "username";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "username";//操作时获取的值 

            //监听心电图和B超
            //FSWControl.WatcherStrat(@"E:\Examine\xdt","*.xml",true,true);
            //FSWControl.WatcherStratBchao(@"E:\Examine\bc", "*.xml", true, true);
            ////开启监控血常规和生化
            //xmlDoc.Load(path);
            //node = xmlDoc.SelectSingleNode("config/shenghuaPath");
            //shenghuapath = node.InnerText;
            //node = xmlDoc.SelectSingleNode("config/xuechangguiPath");
            //xuechangguipath = node.InnerText;
            //if (xuechangguipath == "" || !File.Exists(shenghuapath))
            //{
            //    MessageBox.Show("未获取到血球中间库地址，请检查是否设置地址并重新启动软件！");
            //    return;
            //}
            //else {
            //    FSWControl.WatcherStratxcg(@"F:\血球", "Lis_DB.mdb", true, true);
            //}
            //FileWatcher.WatcheDirForBChao();
        }
    }
}
