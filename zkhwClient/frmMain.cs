using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using zkhwClient.view.PublicHealthView;
using zkhwClient.PublicHealth;
using zkhwClient.view.HomeDoctorSigningView;
using zkhwClient.view.UseHelpView;
using zkhwClient.view.setting;
using System.Diagnostics;
using zkhwClient.dao;
using System.Data;
using System.Xml;
using System.Threading;
using System.Data.OleDb;
using System.IO;
using zkhwClient.view.updateTjResult;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using zkhwClient.utils;
using zkhwClient.view;
using System.IO.Ports;

namespace zkhwClient
{
    public partial class frmMain : Form
    {
        personRegist pR = null;
        Process proHttp = new Process();
        //Process proFtp = new Process();
        Process proAsNet = new Process();
        basicSettingDao bsdao = new basicSettingDao();
        tjcheckDao tjdao = new tjcheckDao();
        jkInfoDao jkdao = new jkInfoDao();
        grjdDao grjddao = new grjdDao();
        private OleDbDataAdapter oda = null;
        private DataSet myds_data = null;
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node;
        string path = @"config.xml";
        string shenghuapath= "";
        string xuechangguipath = "";
        string shlasttime = "";
        string xcglasttime = "";
        DataTable dttv = null;
        public SerialPort port = new SerialPort();
        Byte[] totalByteRead = new Byte[0];

        public frmMain()
        {
            InitializeComponent();
        } 

        public void SetJianDangInfo(string a,string b,string c)
        {
            label3.Text = a; //建档单位
            label7.Text = b;//建档人
            label9.Text = c;//责任医生
        }
        private void frmMain_Load(object sender, EventArgs e)
        { 
            basicInfoSettings basicSet = new basicInfoSettings();
            basicSet.setFunDelegate = SetJianDangInfo;
            basicSet.Show();
            //basicSet.Hide();

            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/shxqAgreement");
            string shxqAgreement = node.InnerText;//生化血球厂家协议
            
            dttv = grjddao.checkThresholdValues();//获取阈值信息
            this.timer1.Start();//时间控件定时器
            if (shxqAgreement == "英诺华")
            {
                this.timer2.Interval = Int32.Parse(Properties.Settings.Default.timeInterval);
                this.timer2.Start();//定时获取生化和血球的数据-英诺华
            }
            this.timer3.Interval = Int32.Parse(Properties.Settings.Default.timer3Interval);
            this.timer3.Start();//1分钟定时刷新设备状态

            this.label1.Text = "一体化查体车  中科弘卫";
            this.label1.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
           
            node = xmlDoc.SelectSingleNode("config/shlasttime");
            node.InnerText = DateTime.Now.ToString("yyyy-MM-dd") + " 06:00:00";
            xmlDoc.Save(path);
            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/xcglasttime");
            node.InnerText = DateTime.Now.ToString("yyyy-MM-dd") + " 06:00:00";
            xmlDoc.Save(path);
            //获取首页右上角信息
            DataTable dts= bsdao.checkBasicsettingInfo();
            if (dts.Rows.Count>0) {
                label3.Text= dts.Rows[0]["organ_name"].ToString();
                label7.Text = dts.Rows[0]["input_name"].ToString();
                label9.Text = dts.Rows[0]["zeren_doctor"].ToString();
            } 
            else
            {
                label3.Text = "";
                label7.Text = "";
                label9.Text = "";
            }
            //默认主页面显示
            foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            {
                if (item.Text == "公共卫生")
                {
                    item.Checked = true;
                    item.BackColor = Color.CadetBlue;
                    this.flowLayoutPanel1.Controls.Clear();
                    //PictureBox[] picb = new PictureBox[item.DropDownItems.Count];
                    int numcount = 0;
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        string stag = item.DropDownItems[i].Tag.ToString();
                        if (stag == "") stag = "0";
                        if (int.Parse(stag) == 1)
                        {
                            numcount = numcount + 1;
                        }
                    }
                    PictureBox[] picb = new PictureBox[numcount];
                    int _index = 0;
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        string stag = item.DropDownItems[i].Tag.ToString();
                        if (stag == "") stag = "0";
                        if (int.Parse(stag) == 0) continue;

                        picb[_index] = new PictureBox();
                        picb[_index].SizeMode = PictureBoxSizeMode.StretchImage;
                        picb[_index].BorderStyle = BorderStyle.None;
                        if (_index == 0)//默认首项选中
                        {
                            picb[_index].BackColor = Color.Blue;
                            pR = new personRegist();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        picb[_index].Size = new Size(216, 40);//大   小
                        picb[_index].Click += new EventHandler(picb_DouClick);
                        picb[_index].Tag = item.DropDownItems[i].Text;

                        TextBox rt = new TextBox();
                        rt.Width = 200;
                        rt.Height = 35;
                        rt.Font = new Font("微软雅黑", 13F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                        rt.Enabled = false;
                        rt.Text = item.DropDownItems[i].Text;
                        rt.Parent = picb[_index];//指定父级
                        this.flowLayoutPanel1.Controls.Add(picb[_index]);
                        item.DropDownItems[i].Visible = false; //看不见

                        _index = _index + 1;
                    };
                }
                else if (item.Text == "挂机" || item.Text == "系统退出" || item.Text == "系统设置")
                {
                    item.Checked = false;
                    item.BackColor = Color.SkyBlue;

                }//保留系统功能菜单下拉选
                else
                {
                    item.Checked = false;
                    item.BackColor = Color.SkyBlue;
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        item.DropDownItems[i].Visible = false; //看不见
                    };
                }//屏蔽其它功能菜单下拉选
            }
            if (shxqAgreement == "库贝尔")
            {
                node = xmlDoc.SelectSingleNode("config/com");
                string comnum = node.InnerText;
                socketTcpKbe();//库贝尔
                bool bl= initPort(comnum);
                if (bl)
                {
                    port.DataReceived += new SerialDataReceivedEventHandler(this.mySerialPort_DataReceived);
                }
                else {
                    MessageBox.Show("打开串口异常,请检查，并重启软件！");
                }
            }
            if (shxqAgreement == "雷杜")
            {
                socketTcp();//雷杜
            }
            if (shxqAgreement == "迈瑞")
            {
                socketTcpMr();//迈瑞
            }
            if (shxqAgreement == "迪瑞")
            {
                socketTcpDr();//迪瑞
            }
            //http
            proHttp.StartInfo.FileName = Application.StartupPath + "\\http\\httpCeshi.exe";
            proHttp.StartInfo.CreateNoWindow = true;
            proHttp.StartInfo.UseShellExecute = false;
            proHttp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proHttp.StartInfo.ErrorDialog = false;
            proHttp.StartInfo.UseShellExecute = false;
            proHttp.Start();
            //AsNetWork  B超
            proAsNet.StartInfo.FileName = Application.StartupPath + "\\AsNetWork\\ASNetWks.exe";
            proAsNet.StartInfo.WorkingDirectory = Application.StartupPath + "\\AsNetWork";
            proAsNet.StartInfo.CreateNoWindow = true;
            proAsNet.StartInfo.ErrorDialog = false;
            proAsNet.StartInfo.UseShellExecute = true;
            proAsNet.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; 
            proAsNet.Start();
            Thread.Sleep(300);
            IntPtrFindWindow.showwindow(proAsNet.MainWindowHandle);
            ////ftp                 
            ////proFtp.StartInfo.FileName = @"C:\\Program Files\\iMAC FTP-JN120.05\\ftpservice.exe";
            ////proFtp.StartInfo.CreateNoWindow = true;
            ////proFtp.StartInfo.UseShellExecute = false;
            ////proFtp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ////proFtp.StartInfo.ErrorDialog = false;
            ////proFtp.Start();
            ////Thread.Sleep(1000);
            ////IntPtrFindWindow.intptrwindows(proFtp.MainWindowHandle);
        }

        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userManage um = new userManage();
            um.ShowDialog();
        }

        private void 密码修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            passWordUpdate pwu = new passWordUpdate();
            pwu.ShowDialog();
        }

        private void 系统退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否确认退出？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (!proHttp.HasExited)
                {
                    proHttp.Kill();
                }
                if (!proAsNet.HasExited)
                {
                    proAsNet.Kill();
                }
                ////if (!proFtp.HasExited)
                ////{
                ////    proFtp.Kill();
                ////}
                service.loginLogService llse = new service.loginLogService();
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "退出系统！";
                lb.type = "1";
                if (lb.name != "admin" && lb.name != "" && lb.name != null)
                {
                    llse.addCheckLog(lb);
                }
                Process p = Process.GetCurrentProcess();
                if (p != null)
                {
                    p.Kill();
                }
                try
                {
                    Environment.Exit(0);
                }
                catch
                { } 
            }
            
        }
        //挂机
        [DllImport("user32")]
        public static extern bool LockWorkStation();//这个是调用windows的系统锁定 
        private void 挂机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LockWorkStation();
        }

        private void 公共卫生ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            {
                if (item.Text == "公共卫生")
                { 
                    item.Checked = true;
                    item.BackColor = Color.CadetBlue;
                    this.flowLayoutPanel1.Controls.Clear();
                    //PictureBox[] picb = new PictureBox[item.DropDownItems.Count];

                    int numcount = 0;
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        string stag = item.DropDownItems[i].Tag.ToString();
                        if (stag == "") stag = "0";
                        if (int.Parse(stag) == 1)
                        {
                            numcount = numcount + 1;
                        }
                    }
                    PictureBox[] picb = new PictureBox[numcount];
                    int _index = 0;
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        string stag = item.DropDownItems[i].Tag.ToString();
                        if (stag == "") stag = "0";
                        if (int.Parse(stag) == 0) continue;

                        picb[_index] = new PictureBox();
                        picb[_index].SizeMode = PictureBoxSizeMode.StretchImage;
                        picb[_index].BorderStyle = BorderStyle.None;
                        if (_index == 0)//默认首项选中
                        {
                            if (pR!=null) {
                                pR.btnClose_Click();
                                pR = null;
                            }
                            picb[_index].BackColor = Color.Blue;
                            pR = new personRegist();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        picb[_index].Size = new Size(216, 40);//大   小
                        picb[_index].Click += new EventHandler(picb_DouClick);
                        picb[_index].Tag = item.DropDownItems[i].Text;

                        TextBox rt = new TextBox();
                        rt.Width = 200;
                        rt.Height = 40;
                        rt.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        rt.Enabled = false;
                        rt.Text = item.DropDownItems[i].Text;
                        rt.Parent = picb[_index];//指定父级
                        this.flowLayoutPanel1.Controls.Add(picb[i]);
                        item.DropDownItems[i].Visible = false; //看不见

                        _index = _index + 1;
                    };
                }
                else
                {
                    item.Checked = false;
                    item.BackColor = Color.SkyBlue;
                }

            }
        }
        private void 家医ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            {
                if (item.Text == "家医签约")
                {
                    item.Checked = true;
                    item.BackColor = Color.CadetBlue;
                    this.flowLayoutPanel1.Controls.Clear();
                    PictureBox[] picb = new PictureBox[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        picb[i] = new PictureBox();
                        picb[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        picb[i].BorderStyle = BorderStyle.None;
                        if (i == 0)//默认首项选中
                        {
                            picb[i].BackColor = Color.Blue;
                            HomeDoctorLogin pR = new HomeDoctorLogin();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        picb[i].Size = new Size(216, 40);//大   小
                        picb[i].Click += new EventHandler(picb_DouClick);
                        picb[i].Tag = item.DropDownItems[i].Text;

                        TextBox rt = new TextBox();
                        rt.Width = 200;
                        rt.Height = 40;
                        rt.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        rt.Enabled = false;
                        rt.Text = item.DropDownItems[i].Text;
                        rt.Parent = picb[i];//指定父级
                        this.flowLayoutPanel1.Controls.Add(picb[i]);
                        item.DropDownItems[i].Visible = false; //看不见
                    };
                }
                else
                {
                    item.Checked = false;
                    item.BackColor = Color.SkyBlue;
                }

            }
        }
        private void picb_DouClick(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            pic.BackColor = Color.Blue;
            string tag = pic.Tag.ToString();
            //if (!"人员登记".Equals(tag) && pR != null)
            //{
            //    pR.btnClose_Click();
            //    pR = null;
            //}
            //选中打标
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i].Tag.ToString() == tag)
                {
                    this.flowLayoutPanel1.Controls[i].BackColor = Color.Blue;
                }
                else
                {
                    this.flowLayoutPanel1.Controls[i].BackColor = this.flowLayoutPanel1.BackColor;
                }
            }

            if (tag == "人员登记")
            {    //公共卫生模块
                if (pR != null)
                {
                    pR.btnClose_Click();
                    pR = null;
                }
                pR = new personRegist();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "体检进度")
            {
                examinatProgress pR = new examinatProgress();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
                pR.queryExaminatProgress();
            }
            else if (tag == "体检报告")
            {
                examinatReport pR = new examinatReport();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "个人基本信息建档")
            {
                personalBasicInfo pR = new personalBasicInfo();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "健康体检表")
            {
                healthCheckup pR = new healthCheckup();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "老年人生活自理能力评估")
            {
                olderHelthService pR = new olderHelthService();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "高血压患者服务")
            {
                hypertensionPatientServices pR = new hypertensionPatientServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "健康扶贫服务")
            {
                healthPoorServices pR = new healthPoorServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "个人档案管理")
            {
                fileManagement pR = new fileManagement();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }      
            else if (tag == "2型糖尿病患者服务")
            {
                diabetesPatientServices pR = new diabetesPatientServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "严重精神病障碍患者服务")
            {
                psychiatricPatientServices pR = new psychiatricPatientServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "肺结核患者服务")
            {
                tuberculosisPatientServices pR = new tuberculosisPatientServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "老年人中医体质辨识")
            {
                tcmHealthServices pR = new tcmHealthServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "孕产妇健康服务")
            {
                maternalHealthServices pR = new maternalHealthServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "0—6岁儿童健康服务")
            {
                childHealthServices pR = new childHealthServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "儿童中医健康服务")
            {
                childrenCMHealthServices pR = new childrenCMHealthServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            //else if (tag == "健康教育服务")
            //{
            //    healthEducationServices pR = new healthEducationServices();
            //    pR.TopLevel = false;
            //    pR.Dock = DockStyle.Fill;
            //    pR.FormBorderStyle = FormBorderStyle.None;
            //    this.panel1.Controls.Clear();
            //    this.panel1.Controls.Add(pR);
            //    pR.Show();
            //}
            //else if (tag == "现场签约")
            //{         //家医签约模块       
            //    onSiteSigning pR = new onSiteSigning();
            //    pR.TopLevel = false;
            //    pR.Dock = DockStyle.Fill;
            //    pR.FormBorderStyle = FormBorderStyle.None;
            //    this.panel1.Controls.Clear();
            //    this.panel1.Controls.Add(pR);
            //    pR.Show();
            //}
            //else if (tag == "团队成员")
            //{
            //    teamMembers pR = new teamMembers();
            //    pR.TopLevel = false;
            //    pR.Dock = DockStyle.Fill;
            //    pR.FormBorderStyle = FormBorderStyle.None;
            //    this.panel1.Controls.Clear();
            //    this.panel1.Controls.Add(pR);
            //    pR.Show();
            //}
            //else if (tag == "签约统计")
            //{
            //    signingStatistics pR = new signingStatistics();
            //    pR.TopLevel = false;
            //    pR.Dock = DockStyle.Fill;
            //    pR.FormBorderStyle = FormBorderStyle.None;
            //    this.panel1.Controls.Clear();
            //    this.panel1.Controls.Add(pR);
            //    pR.Show();
            //}
            else if (tag == "家医签约登录")
            {
                HomeDoctorLogin pR = new HomeDoctorLogin();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "使用情况统计")
            {     //数据分析模块模块  
                usageStatistics pR = new usageStatistics(); 
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();

            }
            else if (tag == "基本信息设置")
            {     //设置模块 
                basicInfoSettings pR = new basicInfoSettings(); 
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "设备管理")
            {
                deviceManagement pR = new deviceManagement();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "系统日志")
            {
                systemlog pR = new systemlog();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "参数设置")
            {
                parameterSetting pR = new parameterSetting();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if(tag== "软件授权")
            {
                //弹出窗体
                frmEmpower frm = new frmEmpower();
                frm._EditType = 0;
                frm.ShowDialog();
            }
            else if (tag == "生化血常规范围")
            {
                frmThresholdSetting frm = new frmThresholdSetting();
                frm.TopLevel = false;
                frm.Dock = DockStyle.Fill;
                frm.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(frm);
                frm.Show();
            }
            else if (tag == "体检设备说明书")
            {   //使用帮助模块 
                OpenPdf(Application.StartupPath+ "\\pdf\\仪器配置说明.docx");
            }
            else if (tag == "软件系统说明书")
            {   //使用帮助模块 
                //softwareSystems pR = new softwareSystems();
                //pR.TopLevel = false;
                //pR.Dock = DockStyle.Fill;
                //pR.FormBorderStyle = FormBorderStyle.None;
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(pR);
                //pR.Show();
                OpenPdf(Application.StartupPath + "\\pdf\\软件系统.pdf");
            }
            else if (tag == "B超")
            {
                OpenPdf(Application.StartupPath + "\\pdf\\B超.pdf");
                //bUltrasound pR = new bUltrasound();
                //pR.TopLevel = false;
                //pR.Dock = DockStyle.Fill;
                //pR.FormBorderStyle = FormBorderStyle.None;
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(pR);
                //pR.Show();
            }
            else if (tag == "生化")
            {
                OpenPdf(Application.StartupPath + "\\pdf\\生化.pdf");
                //biochemical pR = new biochemical();
                //pR.TopLevel = false;
                //pR.Dock = DockStyle.Fill;
                //pR.FormBorderStyle = FormBorderStyle.None;
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(pR);
                //pR.Show();
            }
            else if (tag == "尿常规")
            {
                OpenPdf(Application.StartupPath + "\\pdf\\尿常规.pdf");
                //urinaryFluid pR = new urinaryFluid();
                //pR.TopLevel = false;
                //pR.Dock = DockStyle.Fill;
                //pR.FormBorderStyle = FormBorderStyle.None;
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(pR);
                //pR.Show();
            }
            else if (tag == "血常规")
            {
                OpenPdf(Application.StartupPath + "\\pdf\\血常规.pdf");
                //bloodAnalysis pR = new bloodAnalysis();
                //pR.TopLevel = false;
                //pR.Dock = DockStyle.Fill;
                //pR.FormBorderStyle = FormBorderStyle.None;
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(pR);
                //pR.Show();
            }
            else if (tag == "身高体重")
            {
                OpenPdf(Application.StartupPath + "\\pdf\\身高体重.pdf");
                //heightAndWeight pR = new heightAndWeight();
                //pR.TopLevel = false;
                //pR.Dock = DockStyle.Fill;
                //pR.FormBorderStyle = FormBorderStyle.None;
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(pR);
                //pR.Show();
            }
            else if (tag == "心电图")
            {
                OpenPdf(Application.StartupPath + "\\pdf\\心电图.pdf");
                //electrocarDiogram pR = new electrocarDiogram();
                //pR.TopLevel = false;
                //pR.Dock = DockStyle.Fill;
                //pR.FormBorderStyle = FormBorderStyle.None;
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(pR);
                //pR.Show();
            }
            else if (tag == "血压")
            {
                OpenPdf(Application.StartupPath + "\\pdf\\血压.pdf");
                //bloodPressure pR = new bloodPressure();
                //pR.TopLevel = false;
                //pR.Dock = DockStyle.Fill;
                //pR.FormBorderStyle = FormBorderStyle.None;
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(pR);
                //pR.Show();

            }
            else
            {
                this.panel1.Controls.Clear();
            }
        }

        private void 数据分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            {
                if (item.Text == "数据分析")
                {
                    item.Checked = true;
                    item.BackColor = Color.CadetBlue;
                    this.flowLayoutPanel1.Controls.Clear();
                    PictureBox[] picb = new PictureBox[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        picb[i] = new PictureBox();
                        picb[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        picb[i].BorderStyle = BorderStyle.None;
                        if (i == 0)//默认首项选中
                        {
                            picb[i].BackColor = Color.Blue;
                            usageStatistics pR = new usageStatistics();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        picb[i].Size = new Size(216, 40);//大   小
                        picb[i].Click += new EventHandler(picb_DouClick);
                        picb[i].Tag = item.DropDownItems[i].Text;

                        TextBox rt = new TextBox();
                        rt.Width = 200;
                        rt.Height = 40;
                        rt.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        rt.Enabled = false;
                        rt.Text = item.DropDownItems[i].Text;
                        rt.Parent = picb[i];//指定父级
                        this.flowLayoutPanel1.Controls.Add(picb[i]);
                        item.DropDownItems[i].Visible = false; //看不见
                    };
                }
                else
                {
                    item.Checked = false;
                    item.BackColor = Color.SkyBlue;
                }

            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            {
                if (item.Text == "设置")
                {
                    item.Checked = true;
                    item.BackColor = Color.CadetBlue;
                    this.flowLayoutPanel1.Controls.Clear();
                    PictureBox[] picb = new PictureBox[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    { 
                        picb[i] = new PictureBox();
                        picb[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        picb[i].BorderStyle = BorderStyle.None;
                        if (i == 0)//默认首项选中
                        {
                            picb[i].BackColor = Color.Blue;
                            basicInfoSettings pR = new basicInfoSettings();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        picb[i].Size = new Size(216, 40);//大   小
                        picb[i].Click += new EventHandler(picb_DouClick);
                        picb[i].Tag = item.DropDownItems[i].Text;

                        TextBox rt = new TextBox();
                        rt.Width = 200;
                        rt.Height = 40;
                        rt.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        rt.Enabled = false;
                        rt.Text = item.DropDownItems[i].Text;
                        rt.Parent = picb[i];//指定父级
                        this.flowLayoutPanel1.Controls.Add(picb[i]);
                        item.DropDownItems[i].Visible = false; //看不见
                    };
                }
                else
                {
                    item.Checked = false;
                    item.BackColor = Color.SkyBlue;
                }

            }
        }

        private void 使用帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            {
                if (item.Text == "使用帮助")
                {
                    item.Checked = true;
                    item.BackColor = Color.CadetBlue;
                    this.flowLayoutPanel1.Controls.Clear();
                    PictureBox[] picb = new PictureBox[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        picb[i] = new PictureBox();
                        picb[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        picb[i].BorderStyle = BorderStyle.None;
                        if (i == 0)//默认首项选中
                        {
                            picb[i].BackColor = Color.Blue;
                            softwareSystems pR = new softwareSystems();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        picb[i].Size = new Size(216, 40);//大   小
                        picb[i].Click += new EventHandler(picb_DouClick);
                        picb[i].Tag = item.DropDownItems[i].Text;

                        TextBox rt = new TextBox();
                        rt.Width = 200;
                        rt.Height = 40;
                        rt.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        rt.Enabled = false;
                        rt.Text = item.DropDownItems[i].Text;
                        rt.Parent = picb[i];//指定父级
                        this.flowLayoutPanel1.Controls.Add(picb[i]);
                        item.DropDownItems[i].Visible = false; //看不见
                    };
                }
                else
                {
                    item.Checked = false;
                    item.BackColor = Color.SkyBlue;
                }

            }
        }
        //定时器 刷新页面时间控件
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label5.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            try
            {
                Environment.Exit(0);
            }
            catch
            { }
        }
        //定时任务获取生化和血球的数据
        private void timer2_Tick(object sender, EventArgs e)
        {
            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/shenghuaPath");
            shenghuapath = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/xuechangguiPath");
            xuechangguipath = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/shlasttime");
            shlasttime = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/xcglasttime");
            xcglasttime = node.InnerText;

            Thread demoThread = new Thread(new ThreadStart(shAndxcg));
            demoThread.IsBackground = true;
            demoThread.Start();//启动线程 
        }
        private void shAndxcg()
        {
            if (shenghuapath == "" || !File.Exists(shenghuapath))
            {
                MessageBox.Show("未获取到生化中间库地址，请检查是否设置地址！");
                return;
            }
            else
            {
                bool bl = shenghuapath.IndexOf("Lis_DB.mdb") > -1 ? true : false;
                if (bl == false) { MessageBox.Show("生化中间库地址不正确，请检查是否设置地址！"); return; }

                string sql1 = "select sample_id,patient_id,send_time from LisOutput where send_time > cdate('" + shlasttime + "') order by send_time asc";
                if (getShenghua(sql1)==null || getShenghua(sql1).Tables.Count<1) { return; }
                DataTable arr_dt1 = getShenghua(sql1).Tables[0];
                if (arr_dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < arr_dt1.Rows.Count; j++)
                    {
                        string sql2 = "select lop.patient_id,lop.send_time,lopr.* from LisOutput lop, LisOutputResult lopr where lop.sample_id=lopr.sample_id and lop.sample_id='"+ arr_dt1.Rows[j]["sample_id"].ToString() + "'";
                        DataTable arr_dt2 = getShenghua(sql2).Tables[0];
                        if (arr_dt2.Rows.Count > 0)
                        {
                            shenghuaBean sh = new shenghuaBean();
                            sh.ZrysSH = basicInfoSettings.sh;
                            sh.bar_code = arr_dt1.Rows[j]["patient_id"].ToString();
                            sh.createTime = Convert.ToDateTime(arr_dt1.Rows[j]["send_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(sh.bar_code);
                            if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                            {
                                sh.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                                sh.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                            }
                            else
                            {
                                continue;
                            }
                            for (int i = 0; i < arr_dt2.Rows.Count; i++)
                            {
                                string item = arr_dt2.Rows[i]["item"].ToString();
                                switch (item)
                                {
                                    case "ALB": sh.ALB = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "ALP": sh.ALP = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "ALT": sh.ALT = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "AST": sh.AST = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "CHO": sh.CHO = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "Crea": sh.Crea = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "DBIL": sh.DBIL = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "GGT": sh.GGT = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "GLU": sh.GLU = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "HDL_C": sh.HDL_C = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "LDL_C": sh.LDL_C = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "TBIL": sh.TBIL = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "TG": sh.TG = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "TP": sh.TP = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "UA": sh.UA = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "UREA": sh.UREA = arr_dt2.Rows[i]["result"].ToString(); break;
                                    default: break;
                                }
                            }
                            if (sh.ALT == "N/A") { sh.ALT = "0"; };
                            if (sh.AST == "N/A") { sh.AST = "0"; };
                            if (sh.TBIL == "N/A") { sh.TBIL = "0"; };
                            if (sh.Crea == "N/A") { sh.Crea = "0"; };
                            if (sh.UREA == "N/A") { sh.UREA = "0"; };
                            if (sh.GLU == "N/A") { sh.GLU = "0"; };
                            if (sh.TG == "N/A") { sh.TG = "0"; };
                            if (sh.CHO == "N/A") { sh.CHO = "0"; };
                            if (sh.HDL_C == "N/A") { sh.HDL_C = "0"; };
                            if (sh.LDL_C == "N/A") { sh.LDL_C = "0"; };
                            bool istrue= tjdao.insertShenghuaInfo(sh);
                            if (istrue)
                            {
                                int flag = 1;
                                string alt = sh.ALT;
                                if (alt != null && alt != "*")
                                {
                                    double altdouble = double.Parse(alt);
                                    DataRow[] dralt = dttv.Select("type='ALT'");
                                    double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                                    double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                                    if (altdouble > altwmax || altdouble < altwmin)
                                    {
                                        flag = 2;
                                    }
                                    double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                                    double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                                    if (altdouble > alttmax || altdouble < alttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                               
                                    string ast = sh.AST;
                                    if (ast != null && ast != "*")
                                    {
                                        double astdouble = double.Parse(ast);
                                        DataRow[] drast = dttv.Select("type='AST'");
                                        double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                                        double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                                        if (astdouble > astwmax || astdouble < astwmin)
                                        {
                                            flag = 2;
                                        }
                                        double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                                        double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                                        if (astdouble > asttmax || astdouble < asttmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                                
                                    string tbil = sh.TBIL;
                                    if (tbil != null && tbil != "*")
                                    {
                                        double tbildouble = double.Parse(tbil);
                                        DataRow[] drtbil = dttv.Select("type='TBIL'");
                                        double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                                        double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                                        if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                                        {
                                            flag = 2;
                                        }
                                        double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                                        double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                                        if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                                        {
                                            flag = 3;
                                        }
                                    }  
                               
                                    string crea = sh.Crea;
                                    if (crea != null && crea != "*")
                                    {
                                        double creadouble = double.Parse(crea);
                                        DataRow[] drcrea = dttv.Select("type='CREA'");
                                        double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                                        double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                                        if (creadouble > creawmax || creadouble < creawmin)
                                        {
                                            flag = 2;
                                        }
                                        double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                                        double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                                        if (creadouble > creatmax || creadouble < creatmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                                
                                    string urea = sh.UREA;
                                    if (urea != null && urea != "*")
                                    {
                                        double ureadouble = double.Parse(urea);
                                        DataRow[] drurea = dttv.Select("type='UREA'");
                                        double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                                        double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                                        if (ureadouble > ureawmax || ureadouble < ureawmin)
                                        {
                                            flag = 2;
                                        }
                                        double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                                        double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                                        if (ureadouble > ureatmax || ureadouble < ureatmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string glu = sh.GLU;
                                    if (glu != null && glu != "*")
                                    {
                                        double gludouble = double.Parse(glu);
                                        DataRow[] drglu = dttv.Select("type='GLU'");
                                        double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                                        double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                                        if (gludouble > gluwmax || gludouble < gluwmin)
                                        {
                                            flag = 2;
                                        }
                                        double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                                        double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                                        if (gludouble > glutmax || gludouble < glutmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                              
                                    string tg = sh.TG;
                                    if (tg != null && tg != "*")
                                    {
                                        double tgdouble = double.Parse(tg);
                                        DataRow[] drtg = dttv.Select("type='TG'");
                                        double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                                        double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                                        if (tgdouble > tgwmax || tgdouble < tgwmin)
                                        {
                                            flag = 2;
                                        }
                                        double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                                        double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                                        if (tgdouble > tgtmax || tgdouble < tgtmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string cho = sh.CHO;
                                    if (cho != null && cho != "*")
                                    {
                                        double chodouble = double.Parse(cho);
                                        DataRow[] drcho = dttv.Select("type='CHO'");
                                        double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                                        double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                                        if (chodouble > chowmax || chodouble < chowmin)
                                        {
                                            flag = 2;
                                        }
                                        double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                                        double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                                        if (chodouble > chotmax || chodouble < chotmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string hdlc = sh.HDL_C;
                                    if (hdlc != null && hdlc != "*")
                                    {
                                        double hdlcdouble = double.Parse(hdlc);
                                        DataRow[] drhdlc = dttv.Select("type='HDLC'");
                                        double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                                        double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                                        if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                                        {
                                            flag = 2;
                                        }
                                        double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                                        double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                                        if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                              
                                    string ldlc = sh.LDL_C;
                                    if (ldlc != null && ldlc != "*")
                                    {
                                        double ldlcdouble = double.Parse(ldlc);
                                        DataRow[] drldlc = dttv.Select("type='LDLC'");
                                        double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                                        double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                                        if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                                        {
                                            flag = 2;
                                        }
                                        double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                                        double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                                        if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                                
                                tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                                tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                                //xmlDoc.Load(path);
                                //XmlNode node;
                                //node = xmlDoc.SelectSingleNode("config/shlasttime");
                                //node.InnerText = sh.createTime;
                                //xmlDoc.Save(path);
                            }
                            else {
                                bool istree= tjdao.updateShenghuaInfo(sh);
                                if (!istree) return;
                                int flag = 1;
                                string alt = sh.ALT;
                                if (alt != null && alt != "*")
                                {
                                    double altdouble = double.Parse(alt);
                                    DataRow[] dralt = dttv.Select("type='ALT'");
                                    double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                                    double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                                    if (altdouble > altwmax || altdouble < altwmin)
                                    {
                                        flag = 2;
                                    }
                                    double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                                    double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                                    if (altdouble > alttmax || altdouble < alttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                               
                                    string ast = sh.AST;
                                    if (ast != null && ast != "*")
                                    {
                                        double astdouble = double.Parse(ast);
                                        DataRow[] drast = dttv.Select("type='AST'");
                                        double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                                        double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                                        if (astdouble > astwmax || astdouble < astwmin)
                                        {
                                            flag = 2;
                                        }
                                        double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                                        double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                                        if (astdouble > asttmax || astdouble < asttmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string tbil = sh.TBIL;
                                    if (tbil != null && tbil != "*")
                                    {
                                        double tbildouble = double.Parse(tbil);
                                        DataRow[] drtbil = dttv.Select("type='TBIL'");
                                        double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                                        double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                                        if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                                        {
                                            flag = 2;
                                        }
                                        double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                                        double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                                        if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string crea = sh.Crea;
                                    if (crea != null && crea != "*")
                                    {
                                        double creadouble = double.Parse(crea);
                                        DataRow[] drcrea = dttv.Select("type='CREA'");
                                        double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                                        double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                                        if (creadouble > creawmax || creadouble < creawmin)
                                        {
                                            flag = 2;
                                        }
                                        double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                                        double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                                        if (creadouble > creatmax || creadouble < creatmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                              
                                    string urea = sh.UREA;
                                    if (urea != null && urea != "*")
                                    {
                                        double ureadouble = double.Parse(urea);
                                        DataRow[] drurea = dttv.Select("type='UREA'");
                                        double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                                        double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                                        if (ureadouble > ureawmax || ureadouble < ureawmin)
                                        {
                                            flag = 2;
                                        }
                                        double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                                        double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                                        if (ureadouble > ureatmax || ureadouble < ureatmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string glu = sh.GLU;
                                    if (glu != null && glu != "*")
                                    {
                                        double gludouble = double.Parse(glu);
                                        DataRow[] drglu = dttv.Select("type='GLU'");
                                        double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                                        double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                                        if (gludouble > gluwmax || gludouble < gluwmin)
                                        {
                                            flag = 2;
                                        }
                                        double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                                        double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                                        if (gludouble > glutmax || gludouble < glutmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                                
                                    string tg = sh.TG;
                                    if (tg != null && tg != "*")
                                    {
                                        double tgdouble = double.Parse(tg);
                                        DataRow[] drtg = dttv.Select("type='TG'");
                                        double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                                        double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                                        if (tgdouble > tgwmax || tgdouble < tgwmin)
                                        {
                                            flag = 2;
                                        }
                                        double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                                        double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                                        if (tgdouble > tgtmax || tgdouble < tgtmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string cho = sh.CHO;
                                    if (cho != null && cho != "*")
                                    {
                                        double chodouble = double.Parse(cho);
                                        DataRow[] drcho = dttv.Select("type='CHO'");
                                        double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                                        double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                                        if (chodouble > chowmax || chodouble < chowmin)
                                        {
                                            flag = 2;
                                        }
                                        double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                                        double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                                        if (chodouble > chotmax || chodouble < chotmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string hdlc = sh.HDL_C;
                                    if (hdlc != null && hdlc != "*")
                                    {
                                        double hdlcdouble = double.Parse(hdlc);
                                        DataRow[] drhdlc = dttv.Select("type='HDLC'");
                                        double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                                        double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                                        if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                                        {
                                            flag = 2;
                                        }
                                        double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                                        double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                                        if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                               
                                    string ldlc = sh.LDL_C;
                                    if (ldlc != null && ldlc != "*")
                                    {
                                        double ldlcdouble = double.Parse(ldlc);
                                        DataRow[] drldlc = dttv.Select("type='LDLC'");
                                        double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                                        double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                                        if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                                        {
                                            flag = 2;
                                        }
                                        double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                                        double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                                        if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                                        {
                                            flag = 3;
                                        }
                                    }
                                
                                tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                                tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                            }
                        }                  
                    }
                }
            }
            if (xuechangguipath == "" || !File.Exists(xuechangguipath))
            {
                MessageBox.Show("未获取到血球中间库地址，请检查是否设置地址！");
                return;
            }
            else
            {
                bool bl = xuechangguipath.IndexOf("Lis_DB.mdb") > -1 ? true : false;
                if (bl == false) { MessageBox.Show("血球中间库地址不正确，请检查是否设置地址！"); return; }
                string sql1 = "select sample_id,patient_id,send_time from LisOutput where send_time > cdate('" + xcglasttime + "') order by send_time asc";
                if (getXuechanggui(sql1) == null || getXuechanggui(sql1).Tables.Count < 1) { return; }
                DataTable arr_dt1 = getXuechanggui(sql1).Tables[0];
                if (arr_dt1 != null && arr_dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < arr_dt1.Rows.Count; j++)
                    {
                        string sql2 = "select lop.patient_id,lop.send_time,lopr.* from LisOutput lop, LisOutputResult lopr where lop.sample_id=lopr.sample_id and lop.sample_id='" + arr_dt1.Rows[j]["sample_id"].ToString() + "'";
                        DataTable arr_dt2 = getXuechanggui(sql2).Tables[0];
                        if (arr_dt2.Rows.Count > 0)
                        {
                            xuechangguiBean xcg = new xuechangguiBean();
                            xcg.ZrysXCG = basicInfoSettings.xcg;
                            xcg.bar_code = arr_dt1.Rows[j]["patient_id"].ToString();
                            DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(xcg.bar_code);
                            if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                            {
                                xcg.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                                xcg.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                            }
                            else
                            {
                                continue;
                            }

                            DateTime newtime = Convert.ToDateTime(arr_dt1.Rows[j]["send_time"].ToString());
                            DateTime oldtime = Convert.ToDateTime(xcglasttime);
                            if (newtime <= oldtime)
                            {
                                continue;
                            }
                            xcg.createTime = newtime.ToString("yyyy-MM-dd HH:mm:ss");
                            for (int i = 0; i < arr_dt2.Rows.Count; i++)
                            {
                                string item = arr_dt2.Rows[i]["item"].ToString();
                                switch (item)
                                {
                                    case "HCT": xcg.HCT = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "HGB": xcg.HGB = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "LYM#": xcg.LYM = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "LYM%": xcg.LYMP = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "MCH": xcg.MCH = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "MCHC": xcg.MCHC = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "MCV": xcg.MCV = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "MPV": xcg.MPV = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "MXD#": xcg.MXD = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "MXD%": xcg.MXDP = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "NEUT#": xcg.NEUT = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "NEUT%": xcg.NEUTP = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "PCT": xcg.PCT = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "PDW": xcg.PDW = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "PLT": xcg.PLT = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "RBC": xcg.RBC = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "RDW_CV": xcg.RDW_CV = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "RDW_SD": xcg.RDW_SD = arr_dt2.Rows[i]["result"].ToString(); break;
                                    case "WBC": xcg.WBC = arr_dt2.Rows[i]["result"].ToString(); break;
                                    default: break;
                                }
                            }
                            bool istrue = tjdao.insertXuechangguiInfo(xcg);
                            if (istrue)
                            {
                                //xmlDoc.Load(path);
                                //XmlNode node;
                                //node = xmlDoc.SelectSingleNode("config/xcglasttime");
                                //node.InnerText = xcg.createTime;
                                //xmlDoc.Save(path);
                                int flag = 1;
                                string wbc = xcg.WBC;
                                if (wbc != null && wbc != "*")
                                {
                                    double wbcdouble = double.Parse(wbc);
                                    DataRow[] drwbc = dttv.Select("type='WBC'");
                                    double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                                    double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                                    if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                                    {
                                        flag = 2;
                                    }
                                    double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                                    double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                                    if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string rbc = xcg.RBC;
                                if (rbc != null && rbc != "*")
                                {
                                    double rbcdouble = double.Parse(rbc);
                                    DataRow[] drrbc = dttv.Select("type='RBC'");
                                    double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                                    double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                                    if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                                    {
                                        flag = 2;
                                    }
                                    double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                                    double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                                    if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string pct = xcg.PCT;
                                if (pct != null && pct != "*")
                                {
                                    double pctdouble = double.Parse(pct);
                                    DataRow[] drpct = dttv.Select("type='PCT'");
                                    double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                                    double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                                    if (pctdouble > pctwmax || pctdouble < pctwmin)
                                    {
                                        flag = 2;
                                    }
                                    double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                                    double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                                    if (pctdouble > pcttmax || pctdouble < pcttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string plt = xcg.PLT;
                                if (plt != null && plt != "*")
                                {
                                    double pltdouble = double.Parse(plt);
                                    DataRow[] drplt = dttv.Select("type='PLT'");
                                    double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                                    double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                                    if (pltdouble > pltwmax || pltdouble < pltwmin)
                                    {
                                        flag = 2;
                                    }
                                    double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                                    double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                                    if (pltdouble > plttmax || pltdouble < plttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string hgb = xcg.HGB;
                                if (hgb != null && hgb != "*")
                                {
                                    double hgbdouble = double.Parse(hgb);
                                    DataRow[] drhgb = dttv.Select("type='HGB'");
                                    double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                                    double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                                    if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                                    {
                                        flag = 2;
                                    }
                                    double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                                    double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                                    if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string hct = xcg.HCT;
                                if (hct != null && hct != "*")
                                {
                                    double hctdouble = double.Parse(hct);
                                    DataRow[] drhct = dttv.Select("type='HCT'");
                                    double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                                    double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                                    if (hctdouble > hctwmax || hctdouble < hctwmin)
                                    {
                                        flag = 2;
                                    }
                                    double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                                    double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                                    if (hctdouble > hcttmax || hctdouble < hcttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mcv = xcg.MCV;
                                if (mcv != null && mcv != "*")
                                {
                                    double mcvdouble = double.Parse(mcv);
                                    DataRow[] drmcv = dttv.Select("type='MCV'");
                                    double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                                    double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                                    if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                                    double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                                    if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mch = xcg.MCH;
                                if (mch != null && mch != "*")
                                {
                                    double mchdouble = double.Parse(mch);
                                    DataRow[] drmch = dttv.Select("type='MCH'");
                                    double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                                    double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                                    if (mchdouble > mchwmax || mchdouble < mchwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                                    double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                                    if (mchdouble > mchtmax || mchdouble < mchtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mchc = xcg.MCHC;
                                if (mchc != null && mchc != "*")
                                {
                                    double mchcdouble = double.Parse(mchc);
                                    DataRow[] drmchc = dttv.Select("type='MCHC'");
                                    double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                                    double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                                    if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                                    double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                                    if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string rdwcv = xcg.RDW_CV;
                                if (rdwcv != null && rdwcv != "*")
                                {
                                    double rdwcvdouble = double.Parse(rdwcv);
                                    DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                                    double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                                    double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                                    if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                                    {
                                        flag = 2;
                                    }
                                    double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                                    double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                                    if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string rdwsd = xcg.RDW_SD;
                                if (rdwsd != null && rdwsd != "*")
                                {
                                    double rdwsddouble = double.Parse(rdwsd);
                                    DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                                    double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                                    double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                                    if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                                    {
                                        flag = 2;
                                    }
                                    double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                                    double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                                    if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string neut = xcg.NEUT;
                                if (neut != null && neut != "*")
                                {
                                    double neutdouble = double.Parse(neut);
                                    DataRow[] drneut = dttv.Select("type='NEUT'");
                                    double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                                    double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                                    if (neutdouble > neutwmax || neutdouble < neutwmin)
                                    {
                                        flag = 2;
                                    }
                                    double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                                    double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                                    if (neutdouble > neuttmax || neutdouble < neuttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string neutp = xcg.NEUTP;
                                if (neutp != null && neutp != "*")
                                {
                                    double neutpdouble = double.Parse(neutp);
                                    DataRow[] drneutp = dttv.Select("type='NEUTP'");
                                    double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                                    double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                                    if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                                    {
                                        flag = 2;
                                    }
                                    double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                                    double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                                    if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string lym = xcg.LYM;
                                if (lym != null && lym != "*")
                                {
                                    double lymdouble = double.Parse(lym);
                                    DataRow[] drlym = dttv.Select("type='LYM'");
                                    double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                                    double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                                    if (lymdouble > lymwmax || lymdouble < lymwmin)
                                    {
                                        flag = 2;
                                    }
                                    double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                                    double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                                    if (lymdouble > lymtmax || lymdouble < lymtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string lymp = xcg.LYMP;
                                if (lymp != null && lymp != "*")
                                {
                                    double lympdouble = double.Parse(lymp);
                                    DataRow[] drlymp = dttv.Select("type='LYMP'");
                                    double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                                    double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                                    if (lympdouble > lympwmax || lympdouble < lympwmin)
                                    {
                                        flag = 2;
                                    }
                                    double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                                    double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                                    if (lympdouble > lymptmax || lympdouble < lymptmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mpv = xcg.MPV;
                                if (mpv != null && mpv != "*")
                                {
                                    double mpvdouble = double.Parse(mpv);
                                    DataRow[] drmpv = dttv.Select("type='MPV'");
                                    double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                                    double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                                    if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                                    double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                                    if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string pdw = xcg.PDW;
                                if (pdw != null && pdw != "*")
                                {
                                    double pdwdouble = double.Parse(pdw);
                                    DataRow[] drpdw = dttv.Select("type='PDW'");
                                    double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                                    double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                                    if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                                    {
                                        flag = 2;
                                    }
                                    double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                                    double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                                    if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mxd = xcg.MXD;
                                if (mxd != null && mxd != "*")
                                {
                                    double mxddouble = double.Parse(mxd);
                                    DataRow[] drmxd = dttv.Select("type='MXD'");
                                    double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                                    double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                                    if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                                    double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                                    if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mxdp = xcg.MXDP;
                                if (mxdp != null && mxdp != "*")
                                {
                                    double mxdpdouble = double.Parse(mxdp);
                                    DataRow[] drmxdp = dttv.Select("type='MXDP'");
                                    double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                                    double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                                    if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                                    double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                                    if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                                tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                            }
                            else
                            {
                                bool istrue1 = tjdao.updateXuechangguiInfo(xcg);
                                if (!istrue1)
                                {
                                    return;
                                }
                                int flag = 1;
                                string wbc = xcg.WBC;
                                if (wbc != null && wbc != "*")
                                {
                                    double wbcdouble = double.Parse(wbc);
                                    DataRow[] drwbc = dttv.Select("type='WBC'");
                                    double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                                    double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                                    if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                                    {
                                        flag = 2;
                                    }
                                    double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                                    double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                                    if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string rbc = xcg.RBC;
                                if (rbc != null && rbc != "*")
                                {
                                    double rbcdouble = double.Parse(rbc);
                                    DataRow[] drrbc = dttv.Select("type='RBC'");
                                    double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                                    double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                                    if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                                    {
                                        flag = 2;
                                    }
                                    double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                                    double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                                    if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string pct = xcg.PCT;
                                if (pct != null && pct != "*")
                                {
                                    double pctdouble = double.Parse(pct);
                                    DataRow[] drpct = dttv.Select("type='PCT'");
                                    double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                                    double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                                    if (pctdouble > pctwmax || pctdouble < pctwmin)
                                    {
                                        flag = 2;
                                    }
                                    double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                                    double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                                    if (pctdouble > pcttmax || pctdouble < pcttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string plt = xcg.PLT;
                                if (plt != null && plt != "*")
                                {
                                    double pltdouble = double.Parse(plt);
                                    DataRow[] drplt = dttv.Select("type='PLT'");
                                    double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                                    double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                                    if (pltdouble > pltwmax || pltdouble < pltwmin)
                                    {
                                        flag = 2;
                                    }
                                    double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                                    double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                                    if (pltdouble > plttmax || pltdouble < plttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string hgb = xcg.HGB;
                                if (hgb != null && hgb != "*")
                                {
                                    double hgbdouble = double.Parse(hgb);
                                    DataRow[] drhgb = dttv.Select("type='HGB'");
                                    double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                                    double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                                    if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                                    {
                                        flag = 2;
                                    }
                                    double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                                    double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                                    if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string hct = xcg.HCT;
                                if (hct != null && hct != "*")
                                {
                                    double hctdouble = double.Parse(hct);
                                    DataRow[] drhct = dttv.Select("type='HCT'");
                                    double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                                    double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                                    if (hctdouble > hctwmax || hctdouble < hctwmin)
                                    {
                                        flag = 2;
                                    }
                                    double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                                    double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                                    if (hctdouble > hcttmax || hctdouble < hcttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mcv = xcg.MCV;
                                if (mcv != null && mcv != "*")
                                {
                                    double mcvdouble = double.Parse(mcv);
                                    DataRow[] drmcv = dttv.Select("type='MCV'");
                                    double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                                    double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                                    if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                                    double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                                    if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mch = xcg.MCH;
                                if (mch != null && mch != "*")
                                {
                                    double mchdouble = double.Parse(mch);
                                    DataRow[] drmch = dttv.Select("type='MCH'");
                                    double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                                    double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                                    if (mchdouble > mchwmax || mchdouble < mchwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                                    double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                                    if (mchdouble > mchtmax || mchdouble < mchtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mchc = xcg.MCHC;
                                if (mchc != null && mchc != "*")
                                {
                                    double mchcdouble = double.Parse(mchc);
                                    DataRow[] drmchc = dttv.Select("type='MCHC'");
                                    double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                                    double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                                    if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                                    double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                                    if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string rdwcv = xcg.RDW_CV;
                                if (rdwcv != null && rdwcv != "*")
                                {
                                    double rdwcvdouble = double.Parse(rdwcv);
                                    DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                                    double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                                    double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                                    if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                                    {
                                        flag = 2;
                                    }
                                    double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                                    double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                                    if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string rdwsd = xcg.RDW_SD;
                                if (rdwsd != null && rdwsd != "*")
                                {
                                    double rdwsddouble = double.Parse(rdwsd);
                                    DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                                    double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                                    double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                                    if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                                    {
                                        flag = 2;
                                    }
                                    double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                                    double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                                    if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string neut = xcg.NEUT;
                                if (neut != null && neut != "*")
                                {
                                    double neutdouble = double.Parse(neut);
                                    DataRow[] drneut = dttv.Select("type='NEUT'");
                                    double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                                    double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                                    if (neutdouble > neutwmax || neutdouble < neutwmin)
                                    {
                                        flag = 2;
                                    }
                                    double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                                    double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                                    if (neutdouble > neuttmax || neutdouble < neuttmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string neutp = xcg.NEUTP;
                                if (neutp != null && neutp != "*")
                                {
                                    double neutpdouble = double.Parse(neutp);
                                    DataRow[] drneutp = dttv.Select("type='NEUTP'");
                                    double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                                    double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                                    if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                                    {
                                        flag = 2;
                                    }
                                    double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                                    double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                                    if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string lym = xcg.LYM;
                                if (lym != null && lym != "*")
                                {
                                    double lymdouble = double.Parse(lym);
                                    DataRow[] drlym = dttv.Select("type='LYM'");
                                    double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                                    double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                                    if (lymdouble > lymwmax || lymdouble < lymwmin)
                                    {
                                        flag = 2;
                                    }
                                    double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                                    double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                                    if (lymdouble > lymtmax || lymdouble < lymtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string lymp = xcg.LYMP;
                                if (lymp != null && lymp != "*")
                                {
                                    double lympdouble = double.Parse(lymp);
                                    DataRow[] drlymp = dttv.Select("type='LYMP'");
                                    double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                                    double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                                    if (lympdouble > lympwmax || lympdouble < lympwmin)
                                    {
                                        flag = 2;
                                    }
                                    double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                                    double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                                    if (lympdouble > lymptmax || lympdouble < lymptmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mpv = xcg.MPV;
                                if (mpv != null && mpv != "*")
                                {
                                    double mpvdouble = double.Parse(mpv);
                                    DataRow[] drmpv = dttv.Select("type='MPV'");
                                    double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                                    double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                                    if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                                    double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                                    if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string pdw = xcg.PDW;
                                if (pdw != null && pdw != "*")
                                {
                                    double pdwdouble = double.Parse(pdw);
                                    DataRow[] drpdw = dttv.Select("type='PDW'");
                                    double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                                    double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                                    if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                                    {
                                        flag = 2;
                                    }
                                    double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                                    double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                                    if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mxd = xcg.MXD;
                                if (mxd != null && mxd != "*")
                                {
                                    double mxddouble = double.Parse(mxd);
                                    DataRow[] drmxd = dttv.Select("type='MXD'");
                                    double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                                    double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                                    if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                                    double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                                    if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                string mxdp = xcg.MXDP;
                                if (mxdp != null && mxdp != "*")
                                {
                                    double mxdpdouble = double.Parse(mxdp);
                                    DataRow[] drmxdp = dttv.Select("type='MXDP'");
                                    double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                                    double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                                    if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                                    {
                                        flag = 2;
                                    }
                                    double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                                    double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                                    if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                                    {
                                        flag = 3;
                                    }
                                }
                                tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                                tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 生化表
        /// </summary>
        public DataSet getShenghua(string strSQL)
        {
            try
            {
                string strcon = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + shenghuapath + "";
                myds_data = new DataSet();
                oda = new OleDbDataAdapter(strSQL, strcon);
                oda.Fill(myds_data);
                return myds_data;
            }
            catch {
                return null;
            }
        }
        /// <summary>
        /// 血球表
        /// </summary>
        public DataSet getXuechanggui(string strSQL)
        {
            try
            {
            string strcon = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + xuechangguipath + "";
            myds_data = new DataSet();
            oda = new OleDbDataAdapter(strSQL, strcon);
            oda.Fill(myds_data);
            return myds_data;
            }
            catch
            {
                return null;
            }
        }
        //首页点击B超按钮
        private void button1_Click(object sender, EventArgs e)
        {
            checkBichao checkBc = new checkBichao();
            checkBc.Show();
        }
        //首页点击生化按钮
        private void button2_Click(object sender, EventArgs e)
        {
            checkShenghua checkSh = new checkShenghua();
            checkSh.Show();
        }
        //首页点击尿常规按钮
        private void button3_Click(object sender, EventArgs e)
        {
            checkNiaocg checkNcg = new checkNiaocg();
            checkNcg.Show();
        }
        //首页点击血常规按钮
        private void button4_Click(object sender, EventArgs e)
        {
            checkXuecg checkXcg = new checkXuecg();
            checkXcg.Show();
        }
        //首页点击身高体重按钮
        private void button5_Click(object sender, EventArgs e)
        {
            checkSgtz checkSgTz = new checkSgtz();
            checkSgTz.Show();
        }
        //首页点击心电图按钮
        private void button6_Click(object sender, EventArgs e)
        {
            checkXindt checkXdt = new checkXindt();
            checkXdt.Show();
        }
        //首页点击血压按钮
        private void button7_Click(object sender, EventArgs e)
        {
            checkXueya checkXy = new checkXueya();
            checkXy.Show();
        }

        //首页底部设备状态更新
        private void timer3_Tick(object sender, EventArgs e)
        {
            GetAllLocalMachines();//cmd获取局域网内的所有设备IP和MAC信息
            DataTable dtDeviceType = tjdao.checkDevice();
            if (dtDeviceType==null|| dtDeviceType.Rows.Count<1) {
                return;
            }
          string sfz_online = dtDeviceType.Rows[0]["sfz_online"].ToString();
            if (sfz_online == "0" || "0".Equals(sfz_online))
            {
                this.button9.BackColor = Color.Red;
            }
            else {
                this.button9.BackColor = Color.MediumAquamarine;
            }
            string sxt_online = dtDeviceType.Rows[0]["sxt_online"].ToString();
            if (sxt_online == "0" || "0".Equals(sxt_online))
            {
                this.button8.BackColor = Color.Red;
            }
            else
            {
                this.button8.BackColor = Color.MediumAquamarine;
            }
            string dyj_online = dtDeviceType.Rows[0]["dyj_online"].ToString();
            if (dyj_online == "0" || "0".Equals(dyj_online))
            {
                this.button10.BackColor = Color.Red;
            }
            else
            {
                this.button10.BackColor = Color.MediumAquamarine;
            }
            string xcg_online = dtDeviceType.Rows[0]["xcg_online"].ToString();
            if (xcg_online == "0" || "0".Equals(xcg_online))
            {
                this.button4.BackColor = Color.Red;
            }
            else
            {
                this.button4.BackColor = Color.MediumAquamarine;
            }
            string sh_online = dtDeviceType.Rows[0]["sh_online"].ToString();
            if (sh_online == "0" || "0".Equals(sh_online))
            {
                this.button2.BackColor = Color.Red;
            }
            else
            {
                this.button2.BackColor = Color.MediumAquamarine;
            }
            string ncg_online = dtDeviceType.Rows[0]["ncg_online"].ToString();
            if (ncg_online == "0" || "0".Equals(ncg_online))
            {
                this.button3.BackColor = Color.Red;
            }
            else
            {
                this.button3.BackColor = Color.MediumAquamarine;
            }
            string xdt_online = dtDeviceType.Rows[0]["xdt_online"].ToString();
            if (xdt_online == "0" || "0".Equals(xdt_online))
            {
                this.button6.BackColor = Color.Red;
            }
            else
            {
                this.button6.BackColor = Color.MediumAquamarine;
            }
            string sgtz_online = dtDeviceType.Rows[0]["sgtz_online"].ToString();
            if (sgtz_online == "0" || "0".Equals(sgtz_online))
            {
                this.button5.BackColor = Color.Red;
            }
            else
            {
                this.button5.BackColor = Color.MediumAquamarine;
            }
            string xy_online = dtDeviceType.Rows[0]["xy_online"].ToString();
            if (xy_online == "0" || "0".Equals(xy_online))
            {
                this.button7.BackColor = Color.Red;
            }
            else
            {
                this.button7.BackColor = Color.MediumAquamarine;
            }
            string bc_online = dtDeviceType.Rows[0]["bc_online"].ToString();
            if (bc_online == "0" || "0".Equals(bc_online))
            {
                this.button1.BackColor = Color.Red;
            }
            else
            {
                this.button1.BackColor = Color.MediumAquamarine;
            }
        }
        private void GetAllLocalMachines()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine("arp -a");
            p.StandardInput.WriteLine("exit");
            //ArrayList list = new ArrayList();
            StreamReader reader = p.StandardOutput;
            string IPHead = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString().Substring(0, 3);
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                line = line.Trim();
                if (line.StartsWith(IPHead) && line.Length > 40)
                {
                    string IP = line.Substring(0, 15).Trim();
                    //身高82 血压80 心电81 尿机83 B超84
                    if (IP=="192.168.1.80") {
                        String sql = "update zkhw_state_device set xy_online='1',xy_state='1' where ID='1'";
                        DbHelperMySQL.ExecuteSql(sql);
                    }
                    if (IP == "192.168.1.83")
                    {
                        String sql = "update zkhw_state_device set ncg_online='1',ncg_state='1' where ID='1'";
                        DbHelperMySQL.ExecuteSql(sql);
                    }
                    if (IP == "192.168.1.82")
                    {
                        String sql = "update zkhw_state_device set sgtz_online='1',sgtz_state='1' where ID='1'";
                        DbHelperMySQL.ExecuteSql(sql);
                    }
                    if (IP == "192.168.1.81")
                    {
                        String sql = "update zkhw_state_device set xdt_online='1',xdt_state='1' where ID='1'";
                        DbHelperMySQL.ExecuteSql(sql);
                    }
                    if (IP == "192.168.1.84")
                    {
                        String sql = "update zkhw_state_device set bc_online='1',bc_state='1' where ID='1'";
                        DbHelperMySQL.ExecuteSql(sql);
                    }
                }                
            }
        }
        private void OpenPdf(string url)
        {
            //定义一个ProcessStartInfo实例
            ProcessStartInfo info = new ProcessStartInfo();
            //设置启动进程的初始目录
            info.WorkingDirectory = Application.StartupPath;
            //设置启动进程的应用程序或文档名
            info.FileName = url;
            //设置启动进程的参数
            info.Arguments = "";
            //启动由包含进程启动信息的进程资源
            try
            {
                Process.Start(info);
            }
            catch
            {
                return;
            }
        }
        private void socketTcp() {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);//方法已过期，可以获取IPv4的地址
            IPAddress ip = localhost.AddressList[0];
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(ip, 9001);
            //socket绑定监听地址
            serverSocket.Bind(point);
            //设置同时连接个数
            serverSocket.Listen(10);

            //利用线程后台执行监听,否则程序会假死
            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start(serverSocket);
        }
        /// <summary>
        /// 监听连接
        /// </summary>
        private void Listen(object o)
        {
            var serverSocket = o as Socket;
            while (true)
            {
                //等待连接并且创建一个负责通讯的socket
                var send = serverSocket.Accept();
                //获取链接的IP地址
                //var sendIpoint = send.RemoteEndPoint.ToString();
                //开启一个新线程不停接收消息
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(send);
            }
        }
        

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        private void Recive(object o)
        {
            var send = o as Socket;
            while (true)
            {
                //获取发送过来的消息容器
                byte[] buffer = new byte[1024 * 2];
                var effective = 0;
                try
                {
                    effective = send.Receive(buffer);
                }
                catch { break; }
                //有效字节为0则跳过
                if (effective == 0)
                {
                    break;
                } 
                string sendHL7new = "";
                string sendHL7 = "MSH|^~\\&|||Rayto||1||ACK^R01|1|P|2.3.1||||S||UNICODE|||MSA|AA|1||||0|";
                string []sendArray= sendHL7.Split('|');
                byte[] buffernew = buffer.Skip(0).Take(effective).ToArray();
                string sHL7 = Encoding.Default.GetString(buffernew).Trim();
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\n" + sHL7);
                }
                if (sHL7.IndexOf("CHEMRAY420") > 0)
                {//解析生化协议报文数据                   
                    shenghuaBean sh = new shenghuaBean();
                    sh.ZrysSH = basicInfoSettings.sh;
                    string[] sHL7Pids = Regex.Split(sHL7, "PID", RegexOptions.IgnoreCase);
                    if (sHL7Pids.Length == 0) { return; };
                    string[] MSHArray = sHL7Pids[0].Split('|');
                    sendArray[6] = MSHArray[6];
                    sendArray[9] = MSHArray[9];
                    sendArray[17] = "ASCII";
                    sendArray[22] = MSHArray[9];
                    string[] sHL7PArray = sHL7Pids[1].Split('|');
                    sh.bar_code = sHL7PArray[34];  
                    DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(sh.bar_code);
                    if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                    { 
                        sh.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                        sh.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                    }
                    else
                    {
                         
                        return;
                    }
                    //把HL7分成段
                    string[] sHL7Lines = Regex.Split(sHL7, "OBX", RegexOptions.IgnoreCase);
                    if (sHL7Lines.Length == 0) { return; };
                    for (int i = 1; i < sHL7Lines.Length; i++)
                    {
                        string[] sHL7Array = sHL7Lines[i].Split('|');
                        switch (sHL7Array[4])
                        {
                            case "ALB": sh.ALB = sHL7Array[5]; break;
                            case "ALP": sh.ALP = sHL7Array[5]; break;
                            case "ALT": sh.ALT = sHL7Array[5]; break;
                            case "AST": sh.AST = sHL7Array[5]; break;
                            case "CHO": sh.CHO = sHL7Array[5]; break;
                            case "CRE": sh.Crea = sHL7Array[5]; break;
                            //case "CREA": sh.Crea = sHL7Array[5]; break;
                            case "DBIL": sh.DBIL = sHL7Array[5]; break;
                            case "GGT": sh.GGT = sHL7Array[5]; break;
                            case "GLU": sh.GLU = sHL7Array[5]; break;
                            case "HDL": sh.HDL_C = sHL7Array[5]; break;
                            case "LDL": sh.LDL_C = sHL7Array[5]; break;
                            case "TBIL": sh.TBIL = sHL7Array[5]; break;
                            case "TG": sh.TG = sHL7Array[5]; break;
                            case "TP": sh.TP = sHL7Array[5]; break;
                            case "UA": sh.UA = sHL7Array[5]; break;
                            case "UREA": sh.UREA = sHL7Array[5]; break;
                            default: break;
                        }
                    }
                    sh.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    bool istrue = tjdao.insertShenghuaInfo(sh);
                    if (istrue)
                    {
                        #region 生化
                        int flag =Common.JudgeValueForSh(dttv,sh);
                        tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C,sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                        #endregion
                    }
                    else
                    {
                        bool istree = tjdao.updateShenghuaInfo(sh);
                        if (!istree) return;
                        #region 生化
                        int flag = Common.JudgeValueForSh(dttv,sh);
                        tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                        #endregion
                    }
                    //返回生化的确认数据报文
                    for (int j = 0; j < sendArray.Length; j++) {
                        sendHL7new += "|" + sendArray[j];
                    }
                    send.Send(AckKbe(sendHL7new.Substring(1)));
                } else
                {//解析血球协议报文数据
                    try
                    {
                        xuechangguiBean xcg = new xuechangguiBean();
                        xcg.ZrysXCG = basicInfoSettings.xcg;
                        string[] sHL7Pids = Regex.Split(sHL7, "PID", RegexOptions.IgnoreCase);
                        if (sHL7Pids.Length == 0) { return; };
                        string[] MSHArray = sHL7Pids[0].Split('|');
                        sendArray[6] = MSHArray[6];
                        sendArray[9] = MSHArray[9];
                        sendArray[22] = MSHArray[9];
                        string[] sHL7PArray = sHL7Pids[1].Split('|');
                        xcg.bar_code = sHL7PArray[2]; 
                        DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(xcg.bar_code);
                        if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                        {
                            xcg.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                            xcg.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                        }
                        else
                        {
                            return;
                        }
                        xcg.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //把HL7分成段
                        string[] sHL7Lines = Regex.Split(sHL7, "OBX", RegexOptions.IgnoreCase);
                        if (sHL7Lines.Length == 0) { return; };
                        for (int i = 1; i < sHL7Lines.Length; i++)
                        {
                            string[] sHL7Array = sHL7Lines[i].Split('|');
                            switch (sHL7Array[3])
                            {
                                case "HCT": xcg.HCT = sHL7Array[5]; break;
                                case "HGB": xcg.HGB = sHL7Array[5]; break;
                                case "LYM#": xcg.LYM = sHL7Array[5]; break;
                                case "LYM%": xcg.LYMP = sHL7Array[5]; break;
                                case "MCH": xcg.MCH = sHL7Array[5]; break;
                                case "MCHC": xcg.MCHC = sHL7Array[5]; break;
                                case "MCV": xcg.MCV = sHL7Array[5]; break;
                                case "MPV": xcg.MPV = sHL7Array[5]; break;
                                case "MID#": xcg.MXD = sHL7Array[5]; break;
                                case "MID%": xcg.MXDP = sHL7Array[5]; break;
                                case "NEUT#": xcg.NEUT = sHL7Array[5]; break;
                                case "NEUT%": xcg.NEUTP = sHL7Array[5]; break;
                                case "PCT": xcg.PCT = sHL7Array[5]; break;
                                case "PDW": xcg.PDW = sHL7Array[5]; break;
                                case "PLT": xcg.PLT = sHL7Array[5]; break;
                                case "RBC": xcg.RBC = sHL7Array[5]; break;
                                case "RDW-CV": xcg.RDW_CV = sHL7Array[5]; break;
                                case "RDW-SD": xcg.RDW_SD = sHL7Array[5]; break;
                                case "WBC": xcg.WBC = sHL7Array[5]; break;
                                case "MON#": xcg.MONO = sHL7Array[5]; break;
                                case "MON%": xcg.MONOP = sHL7Array[5]; break;
                                case "GRA#": xcg.GRAN = sHL7Array[5]; break;
                                case "GRA%": xcg.GRANP = sHL7Array[5]; break;
                                case "P-LCR": xcg.PLCR = sHL7Array[5]; break;
                                default: break;
                            }
                        }
                        bool istrue = tjdao.insertXuechangguiInfo(xcg);
                        if (istrue)
                        {
                            int flag = Common.JudgeValueForXCG(dttv,xcg); 
                            tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                            tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                        }
                        else
                        {
                            bool istrue1 = tjdao.updateXuechangguiInfo(xcg);
                            if (!istrue1)
                            {
                                return;
                            }
                            int flag = Common.JudgeValueForXCG(dttv,xcg); 
                            tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                            tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                        }
                        //返回血球的确认数据报文
                        for (int j = 0; j < sendArray.Length; j++)
                        {
                            sendHL7new += "|" + sendArray[j];
                        }
                            send.Send(AckKbe(sendHL7new.Substring(1)));
                    }
                    catch (Exception ex){
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\n" +ex.Message+"\n"+ex.StackTrace);
                        }
                    }
               }
           }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否确认退出？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (!proHttp.HasExited)
                {
                    proHttp.Kill();
                }
                if (!proAsNet.HasExited)
                {
                    proAsNet.Kill();
                }
                ////if (!proFtp.HasExited)
                ////{
                ////    proFtp.Kill();
                ////}
                service.loginLogService llse = new service.loginLogService();
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "退出系统！";
                lb.type = "1";
                if (lb.name != "admin" && lb.name != "" && lb.name != null)
                {
                    llse.addCheckLog(lb);
                }
                Process p = Process.GetCurrentProcess();
                if (p != null)
                {
                    p.Kill();
                }

                try
                {
                    try
                    {
                        /************************/
                        string fpath = Application.StartupPath + "\\sysstem.ini";
                        sysstem.UpdateInfo(fpath);
                        /************************/
                    }
                    catch (Exception tt1)
                    {
                        MessageBox.Show("Err1:" + tt1.Message);
                    }

                    Environment.Exit(0);
                }
                catch(Exception tt)
                {
                    MessageBox.Show("Err:"+tt.Message);
                    Environment.Exit(0);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void socketTcpKbe()
        {
            try
            {
                string hostName = Dns.GetHostName();   //获取本机名
                IPHostEntry localhost = Dns.GetHostByName(hostName);//方法已过期，可以获取IPv4的地址
                IPAddress ip = localhost.AddressList[0];
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint point = new IPEndPoint(ip, 9001);  
                //socket绑定监听地址
                serverSocket.Bind(point);
                //设置同时连接个数
                serverSocket.Listen(10);

                //利用线程后台执行监听,否则程序会假死
                Thread thread = new Thread(ListenKbe);
                thread.IsBackground = true;
                thread.Start(serverSocket);
            }
            catch (Exception dd)
            {
                MessageBox.Show(dd.Message);
            }
        }
        /// <summary>
        /// 监听连接
        /// </summary>
        private void ListenKbe(object o)
        {
            var serverSocket = o as Socket;
            while (true)
            {
                //等待连接并且创建一个负责通讯的socket
                var send = serverSocket.Accept();
                //获取链接的IP地址
                //var sendIpoint = send.RemoteEndPoint.ToString();
                //开启一个新线程不停接收消息
                Thread thread = new Thread(ReciveKbe);
                thread.IsBackground = true;
                thread.Start(send);
            }
        }
        private byte[] AckKbe(string str)
        {
            string[] astr = Regex.Split(str, "MSA", RegexOptions.IgnoreCase);

            string a = astr[0];
            string b = "MSA" + astr[1];
            int num = a.Length + b.Length + 5;
            byte[] c = new byte[num];
            byte[] a1 = Encoding.ASCII.GetBytes(a);
            Array.Copy(a1, 0, c, 1, a1.Length);
            byte[] b1 = Encoding.ASCII.GetBytes(b);
            Array.Copy(b1, 0, c, a1.Length + 2, b1.Length);
            //特殊处理的几个值
            c[0] = 0x0B;
            c[a1.Length + 1] = 0x0D;
            c[num - 3] = 0x0D;
            c[num - 2] = 0x1C;
            c[num - 1] = 0x0D;
            return c;
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        private void ReciveKbe(object o)
        {
            try {
                var send = o as Socket;
                while (true)
                {
                    //获取发送过来的消息容器
                    byte[] buffer = new byte[1024 * 2];
                    var effective = 0;
                    try
                    {
                        effective = send.Receive(buffer);
                    }
                    catch { break; }
                    //有效字节为0则跳过
                    if (effective == 0)
                    {
                        break;
                    }
                    string sendHL7new = "";
                    string sendHL7 = "MSH|^~\\&|||ICUBIO|740|20190821110721||ACK^R01|1|P|2.3.1||||0||ASCII|||MSA|AA|1|Message accepted|||0|";
                    string[] sendArray = sendHL7.Split('|');
                    byte[] buffernew = buffer.Skip(0).Take(effective).ToArray();
                    string sHL7 = Encoding.Default.GetString(buffernew).Trim();

                    if (sHL7.IndexOf("ICUBIO") > 0)
                    {//解析生化协议报文数据                   
                        shenghuaBean sh = new shenghuaBean();
                        sh.ZrysSH = basicInfoSettings.sh;
                        string[] sHL7Pids = Regex.Split(sHL7, "PID", RegexOptions.IgnoreCase);
                        if (sHL7Pids.Length == 0) { return; };
                        string[] MSHArray = sHL7Pids[0].Split('|');
                        sendArray[6] = MSHArray[6];
                        sendArray[9] = MSHArray[9];
                        sendArray[22] = MSHArray[9];
                        string[] sHL7PArray = sHL7Pids[1].Split('|');
                        sh.bar_code = sHL7PArray[33];
                        DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(sh.bar_code);
                        if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                        {
                            sh.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                            sh.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                        }
                        else
                        {
                            return;
                        }
                        //把HL7分成段
                        string[] sHL7Lines = Regex.Split(sHL7, "OBX", RegexOptions.IgnoreCase);
                        if (sHL7Lines.Length == 0) { return; };
                        for (int i = 1; i < sHL7Lines.Length; i++)
                        {
                            string[] sHL7Array = sHL7Lines[i].Split('|');
                            if (sHL7Array[5]==""||"".Equals(sHL7Array[5])) {
                                continue;
                            }
                            switch (sHL7Array[4])
                            {
                                case "ALB": sh.ALB = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.')+3); break;
                                case "ALP": sh.ALP = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "ALT": sh.ALT = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.')); break;
                                case "AST": sh.AST = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.')); break;
                                case "CHO": sh.CHO = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "CREA": sh.Crea = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 2); break;
                                case "D-BIL": sh.DBIL = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "GGT": sh.GGT = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "GLU": sh.GLU = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "HDL": sh.HDL_C = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "LDL": sh.LDL_C = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "T-BIL": sh.TBIL = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 2); break;
                                case "TG": sh.TG = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "TP": sh.TP = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "UA": sh.UA = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                case "UREA": sh.UREA = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
                                default: break;
                            }
                        }
                        sh.createTime = DateTime.ParseExact(sHL7PArray[38],"yyyyMMddHHmmss",System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss");
                        bool istrue = tjdao.insertShenghuaInfo(sh);
                        if (istrue)
                        {
                            #region 生化
                            int flag = 1;
                            string alt = sh.ALT;
                            if (alt != null && alt != "*")
                            {
                                double altdouble = double.Parse(alt);
                                DataRow[] dralt = dttv.Select("type='ALT'");
                                double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                                double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                                if (altdouble > altwmax || altdouble < altwmin)
                                {
                                    flag = 2;
                                }
                                double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                                double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                                if (altdouble > alttmax || altdouble < alttmin)
                                {
                                    flag = 3;
                                }
                            }

                            string ast = sh.AST;
                            if (ast != null && ast != "*")
                            {
                                double astdouble = double.Parse(ast);
                                DataRow[] drast = dttv.Select("type='AST'");
                                double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                                double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                                if (astdouble > astwmax || astdouble < astwmin)
                                {
                                    flag = 2;
                                }
                                double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                                double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                                if (astdouble > asttmax || astdouble < asttmin)
                                {
                                    flag = 3;
                                }
                            }

                            string tbil = sh.TBIL;
                            if (tbil != null && tbil != "*")
                            {
                                double tbildouble = double.Parse(tbil);
                                DataRow[] drtbil = dttv.Select("type='TBIL'");
                                double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                                double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                                if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                                {
                                    flag = 2;
                                }
                                double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                                double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                                if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                                {
                                    flag = 3;
                                }
                            }

                            string dbil = sh.DBIL;
                            if (dbil != null && dbil != "*")
                            {
                                double dbildouble = double.Parse(dbil);
                                DataRow[] drtbil = dttv.Select("type='DBIL'");
                                double dbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                                double dbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                                if (dbildouble > dbilwmax || dbildouble < dbilwmin)
                                {
                                    flag = 2;
                                }
                                double dbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                                double dbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                                if (dbildouble > dbiltmax || dbildouble < dbiltmin)
                                {
                                    flag = 3;
                                }
                            }

                            string crea = sh.Crea;
                            if (crea != null && crea != "*")
                            {
                                double creadouble = double.Parse(crea);
                                DataRow[] drcrea = dttv.Select("type='CREA'");
                                double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                                double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                                if (creadouble > creawmax || creadouble < creawmin)
                                {
                                    flag = 2;
                                }
                                double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                                double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                                if (creadouble > creatmax || creadouble < creatmin)
                                {
                                    flag = 3;
                                }
                            }

                            string urea = sh.UREA;
                            if (urea != null && urea != "*")
                            {
                                double ureadouble = double.Parse(urea);
                                DataRow[] drurea = dttv.Select("type='UREA'");
                                double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                                double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                                if (ureadouble > ureawmax || ureadouble < ureawmin)
                                {
                                    flag = 2;
                                }
                                double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                                double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                                if (ureadouble > ureatmax || ureadouble < ureatmin)
                                {
                                    flag = 3;
                                }
                            }

                            string glu = sh.GLU;
                            if (glu != null && glu != "*")
                            {
                                double gludouble = double.Parse(glu);
                                DataRow[] drglu = dttv.Select("type='GLU'");
                                double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                                double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                                if (gludouble > gluwmax || gludouble < gluwmin)
                                {
                                    flag = 2;
                                }
                                double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                                double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                                if (gludouble > glutmax || gludouble < glutmin)
                                {
                                    flag = 3;
                                }
                            }

                            string tg = sh.TG;
                            if (tg != null && tg != "*")
                            {
                                double tgdouble = double.Parse(tg);
                                DataRow[] drtg = dttv.Select("type='TG'");
                                double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                                double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                                if (tgdouble > tgwmax || tgdouble < tgwmin)
                                {
                                    flag = 2;
                                }
                                double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                                double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                                if (tgdouble > tgtmax || tgdouble < tgtmin)
                                {
                                    flag = 3;
                                }
                            }

                            string cho = sh.CHO;
                            if (cho != null && cho != "*")
                            {
                                double chodouble = double.Parse(cho);
                                DataRow[] drcho = dttv.Select("type='CHO'");
                                double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                                double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                                if (chodouble > chowmax || chodouble < chowmin)
                                {
                                    flag = 2;
                                }
                                double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                                double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                                if (chodouble > chotmax || chodouble < chotmin)
                                {
                                    flag = 3;
                                }
                            }

                            string hdlc = sh.HDL_C;
                            if (hdlc != null && hdlc != "*")
                            {
                                double hdlcdouble = double.Parse(hdlc);
                                DataRow[] drhdlc = dttv.Select("type='HDLC'");
                                double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                                double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                                if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                                {
                                    flag = 2;
                                }
                                double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                                double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                                if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                                {
                                    flag = 3;
                                }
                            }

                            string ldlc = sh.LDL_C;
                            if (ldlc != null && ldlc != "*")
                            {
                                double ldlcdouble = double.Parse(ldlc);
                                DataRow[] drldlc = dttv.Select("type='LDLC'");
                                double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                                double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                                if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                                {
                                    flag = 2;
                                }
                                double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                                double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                                if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                                {
                                    flag = 3;
                                }
                            }

                            tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                            tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                            #endregion
                        }
                        else
                        {
                            bool istree = tjdao.updateShenghuaInfo(sh);
                            if (!istree) return;
                            #region 生化
                            int flag = 1;
                            string alt = sh.ALT;
                            if (alt != null && alt != "*")
                            {
                                double altdouble = double.Parse(alt);
                                DataRow[] dralt = dttv.Select("type='ALT'");
                                double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                                double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                                if (altdouble > altwmax || altdouble < altwmin)
                                {
                                    flag = 2;
                                }
                                double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                                double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                                if (altdouble > alttmax || altdouble < alttmin)
                                {
                                    flag = 3;
                                }
                            }

                            string ast = sh.AST;
                            if (ast != null && ast != "*")
                            {
                                double astdouble = double.Parse(ast);
                                DataRow[] drast = dttv.Select("type='AST'");
                                double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                                double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                                if (astdouble > astwmax || astdouble < astwmin)
                                {
                                    flag = 2;
                                }
                                double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                                double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                                if (astdouble > asttmax || astdouble < asttmin)
                                {
                                    flag = 3;
                                }
                            }

                            string tbil = sh.TBIL;
                            if (tbil != null && tbil != "*")
                            {
                                double tbildouble = double.Parse(tbil);
                                DataRow[] drtbil = dttv.Select("type='TBIL'");
                                double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                                double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                                if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                                {
                                    flag = 2;
                                }
                                double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                                double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                                if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                                {
                                    flag = 3;
                                }
                            }
                            string dbil = sh.DBIL;
                            if (dbil != null && dbil != "*")
                            {
                                double dbildouble = double.Parse(dbil);
                                DataRow[] drtbil = dttv.Select("type='DBIL'");
                                double dbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                                double dbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                                if (dbildouble > dbilwmax || dbildouble < dbilwmin)
                                {
                                    flag = 2;
                                }
                                double dbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                                double dbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                                if (dbildouble > dbiltmax || dbildouble < dbiltmin)
                                {
                                    flag = 3;
                                }
                            }
                            string crea = sh.Crea;
                            if (crea != null && crea != "*")
                            {
                                double creadouble = double.Parse(crea);
                                DataRow[] drcrea = dttv.Select("type='CREA'");
                                double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                                double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                                if (creadouble > creawmax || creadouble < creawmin)
                                {
                                    flag = 2;
                                }
                                double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                                double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                                if (creadouble > creatmax || creadouble < creatmin)
                                {
                                    flag = 3;
                                }
                            }

                            string urea = sh.UREA;
                            if (urea != null && urea != "*")
                            {
                                double ureadouble = double.Parse(urea);
                                DataRow[] drurea = dttv.Select("type='UREA'");
                                double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                                double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                                if (ureadouble > ureawmax || ureadouble < ureawmin)
                                {
                                    flag = 2;
                                }
                                double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                                double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                                if (ureadouble > ureatmax || ureadouble < ureatmin)
                                {
                                    flag = 3;
                                }
                            }

                            string glu = sh.GLU;
                            if (glu != null && glu != "*")
                            {
                                double gludouble = double.Parse(glu);
                                DataRow[] drglu = dttv.Select("type='GLU'");
                                double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                                double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                                if (gludouble > gluwmax || gludouble < gluwmin)
                                {
                                    flag = 2;
                                }
                                double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                                double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                                if (gludouble > glutmax || gludouble < glutmin)
                                {
                                    flag = 3;
                                }
                            }

                            string tg = sh.TG;
                            if (tg != null && tg != "*")
                            {
                                double tgdouble = double.Parse(tg);
                                DataRow[] drtg = dttv.Select("type='TG'");
                                double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                                double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                                if (tgdouble > tgwmax || tgdouble < tgwmin)
                                {
                                    flag = 2;
                                }
                                double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                                double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                                if (tgdouble > tgtmax || tgdouble < tgtmin)
                                {
                                    flag = 3;
                                }
                            }

                            string cho = sh.CHO;
                            if (cho != null && cho != "*")
                            {
                                double chodouble = double.Parse(cho);
                                DataRow[] drcho = dttv.Select("type='CHO'");
                                double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                                double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                                if (chodouble > chowmax || chodouble < chowmin)
                                {
                                    flag = 2;
                                }
                                double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                                double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                                if (chodouble > chotmax || chodouble < chotmin)
                                {
                                    flag = 3;
                                }
                            }

                            string hdlc = sh.HDL_C;
                            if (hdlc != null && hdlc != "*")
                            {
                                double hdlcdouble = double.Parse(hdlc);
                                DataRow[] drhdlc = dttv.Select("type='HDLC'");
                                double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                                double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                                if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                                {
                                    flag = 2;
                                }
                                double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                                double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                                if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                                {
                                    flag = 3;
                                }
                            }

                            string ldlc = sh.LDL_C;
                            if (ldlc != null && ldlc != "*")
                            {
                                double ldlcdouble = double.Parse(ldlc);
                                DataRow[] drldlc = dttv.Select("type='LDLC'");
                                double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                                double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                                if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                                {
                                    flag = 2;
                                }
                                double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                                double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                                if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                                {
                                    flag = 3;
                                }
                            }
                            tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                            tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                            #endregion
                        }
                        //返回生化的确认数据报文
                        for (int j = 0; j < sendArray.Length; j++)
                        {
                            sendHL7new += "|" + sendArray[j];
                        }
                        //byte[] sendBytes = Encoding.Unicode.GetBytes(sendHL7new.Substring(1));
                        byte[] sendBytes = AckKbe(sendHL7new.Substring(1));
                        send.Send(sendBytes);
                    }
                }
            }
            catch (Exception ex) {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\n" + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }
        private void mySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            bool isCRC = false;
            Thread.Sleep(12000);
            try
            {
                SerialPort sp = (SerialPort)sender;
                string text = string.Empty;
                Byte[] byteRead = new Byte[sp.BytesToRead];
                if (byteRead.Length == 0)
                {
                    return;
                }
                sp.Read(byteRead, 0, byteRead.Length);
                //sp.DiscardInBuffer();
                //sp.DiscardOutBuffer();
                totalByteRead = totalByteRead.Concat(byteRead).ToArray();
                text = ToHexString(totalByteRead);
                if (totalByteRead.Length > 1000)
                {
                    string beginText = text.Substring(0,16);
                    string endText = text.Substring(text.Length-18, 18);
                    if (beginText == "3C73616D706C653E" && endText == "3C2F73616D706C653E") {
                        text = Encoding.ASCII.GetString(totalByteRead);
                        isCRC = true;
                    }
                    string endText1 = text.Substring(text.Length - 22, 22);
                    if (beginText == "3C73616D706C653E" && endText1 == "3C2F73616D706C653E0D0A") {
                        text = Encoding.ASCII.GetString(totalByteRead);
                        isCRC = true;
                    }
                }
                
                if (isCRC)
                {
                    //using (StreamWriter sw = new StreamWriter(Application.StartupPath + "/log.txt", true))
                    //{
                    //    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "接收报文2：" + text );
                    //}
                    Thread multiAdd = new Thread(parsingTextData);
                    multiAdd.IsBackground = true;
                    multiAdd.Start(text);
                    totalByteRead = new Byte[0];
                }
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + "/log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "异常报文1：" + ToHexString(totalByteRead));
                }
            }
        }

        public void parsingTextData(object parameter)
        {
            xuechangguiBean xcg = new xuechangguiBean();
            try
            {
                string xmlStr = @parameter.ToString();
                xcg.ZrysXCG = basicInfoSettings.xcg;
                var doc = new XmlDocument();
                doc.LoadXml(xmlStr);
                var rowNoteList = doc.SelectNodes("/sample/smpinfo/p");
                var fieldNodeID = rowNoteList[0].ChildNodes;
                string barcode = fieldNodeID[1].InnerText;
                string []barcodes = barcode.Split('/');
                xcg.bar_code = barcodes[0].Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Trim();
                var fieldNodeTime = rowNoteList[2].ChildNodes;
                string timeNow = fieldNodeTime[1].InnerText;
                timeNow = timeNow.Replace("T", " ") + ":00";
                DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(xcg.bar_code);
                if (dtjkinfo.Rows.Count > 0)
                {
                    xcg.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                    xcg.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                }
                else
                {
                    return;
                }
                xcg.createTime = timeNow; //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var smpresultsList = doc.SelectNodes("/sample/smpresults/p");
                foreach (XmlNode rowNode in smpresultsList)
                {
                    var fieldNodeList = rowNode.ChildNodes;
                    string type = fieldNodeList[0].InnerText;
                    switch (type)
                    {
                        case "HCT": xcg.HCT = fieldNodeList[1].InnerText; break;
                        case "HGB": xcg.HGB = fieldNodeList[1].InnerText; break;
                        case "LYMHC": xcg.LYM = fieldNodeList[1].InnerText; break;
                        case "LYMHR": xcg.LYMP = fieldNodeList[1].InnerText; break;
                        case "MCH": xcg.MCH = fieldNodeList[1].InnerText; break;
                        case "MCHC": xcg.MCHC = fieldNodeList[1].InnerText; break;
                        case "MCV": xcg.MCV = fieldNodeList[1].InnerText; break;
                        case "MPV": xcg.MPV = fieldNodeList[1].InnerText; break;
                        case "MIDC": xcg.MXD = fieldNodeList[1].InnerText; break;
                        case "MIDR": xcg.MXDP = fieldNodeList[1].InnerText; break;
                        case "NEUTC": xcg.NEUT = fieldNodeList[1].InnerText; break;
                        case "NEUTR": xcg.NEUTP = fieldNodeList[1].InnerText; break;
                        case "PCT": xcg.PCT = fieldNodeList[1].InnerText; break;
                        case "PDW": xcg.PDW = fieldNodeList[1].InnerText; break;
                        case "PLT": xcg.PLT = fieldNodeList[1].InnerText; break;
                        case "RBC": xcg.RBC = fieldNodeList[1].InnerText; break;
                        case "RDW-CV": xcg.RDW_CV = fieldNodeList[1].InnerText; break;
                        case "RDW-SD": xcg.RDW_SD = fieldNodeList[1].InnerText; break;
                        case "WBC": xcg.WBC = fieldNodeList[1].InnerText; break;
                        case "MONC": xcg.MONO = fieldNodeList[1].InnerText; break;
                        case "MONP": xcg.MONOP = fieldNodeList[1].InnerText; break;
                        case "GRAC": xcg.GRAN = fieldNodeList[1].InnerText; break;
                        case "GRAP": xcg.GRANP = fieldNodeList[1].InnerText; break;
                        case "P-LCR": xcg.PLCR = fieldNodeList[1].InnerText; break;
                        default: break;
                    }
                }
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + "/log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "异常报文解析：" + ee.Message+"--"+ee.StackTrace);
                }
                return;
            }
                bool istrue = tjdao.insertXuechangguiInfo(xcg);
                if (istrue)
                {
                    int flag = 1;
                    string wbc = xcg.WBC;
                    if (wbc != null && wbc != "")
                    {
                        double wbcdouble = double.Parse(wbc);
                        DataRow[] drwbc = dttv.Select("type='WBC'");
                        double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                        double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                        if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                        {
                            flag = 2;
                        }
                        double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                        double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                        if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                        {
                            flag = 3;
                        }
                    }
                    string rbc = xcg.RBC;
                    if (rbc != null && rbc != "")
                    {
                        double rbcdouble = double.Parse(rbc);
                        DataRow[] drrbc = dttv.Select("type='RBC'");
                        double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                        double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                        if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                        {
                            flag = 2;
                        }
                        double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                        double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                        if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                        {
                            flag = 3;
                        }
                    }
                    string pct = xcg.PCT;
                    if (pct != null && pct != "")
                    {
                        double pctdouble = double.Parse(pct);
                        DataRow[] drpct = dttv.Select("type='PCT'");
                        double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                        double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                        if (pctdouble > pctwmax || pctdouble < pctwmin)
                        {
                            flag = 2;
                        }
                        double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                        double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                        if (pctdouble > pcttmax || pctdouble < pcttmin)
                        {
                            flag = 3;
                        }
                    }
                    string plt = xcg.PLT;
                    if (plt != null && plt != "")
                    {
                        double pltdouble = double.Parse(plt);
                        DataRow[] drplt = dttv.Select("type='PLT'");
                        double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                        double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                        if (pltdouble > pltwmax || pltdouble < pltwmin)
                        {
                            flag = 2;
                        }
                        double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                        double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                        if (pltdouble > plttmax || pltdouble < plttmin)
                        {
                            flag = 3;
                        }
                    }
                    string hgb = xcg.HGB;
                    if (hgb != null && hgb != "")
                    {
                        double hgbdouble = double.Parse(hgb);
                        DataRow[] drhgb = dttv.Select("type='HGB'");
                        double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                        double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                        if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                        {
                            flag = 2;
                        }
                        double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                        double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                        if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                        {
                            flag = 3;
                        }
                    }
                    string hct = xcg.HCT;
                    if (hct != null && hct != "")
                    {
                        double hctdouble = double.Parse(hct);
                        DataRow[] drhct = dttv.Select("type='HCT'");
                        double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                        double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                        if (hctdouble > hctwmax || hctdouble < hctwmin)
                        {
                            flag = 2;
                        }
                        double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                        double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                        if (hctdouble > hcttmax || hctdouble < hcttmin)
                        {
                            flag = 3;
                        }
                    }
                    string mcv = xcg.MCV;
                    if (mcv != null && mcv != "")
                    {
                        double mcvdouble = double.Parse(mcv);
                        DataRow[] drmcv = dttv.Select("type='MCV'");
                        double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                        double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                        if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                        {
                            flag = 2;
                        }
                        double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                        double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                        if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                        {
                            flag = 3;
                        }
                    }
                    string mch = xcg.MCH;
                    if (mch != null && mch != "")
                    {
                        double mchdouble = double.Parse(mch);
                        DataRow[] drmch = dttv.Select("type='MCH'");
                        double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                        double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                        if (mchdouble > mchwmax || mchdouble < mchwmin)
                        {
                            flag = 2;
                        }
                        double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                        double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                        if (mchdouble > mchtmax || mchdouble < mchtmin)
                        {
                            flag = 3;
                        }
                    }
                    string mchc = xcg.MCHC;
                    if (mchc != null && mchc != "")
                    {
                        double mchcdouble = double.Parse(mchc);
                        DataRow[] drmchc = dttv.Select("type='MCHC'");
                        double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                        double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                        if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                        {
                            flag = 2;
                        }
                        double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                        double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                        if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                        {
                            flag = 3;
                        }
                    }
                    string rdwcv = xcg.RDW_CV;
                    if (rdwcv != null && rdwcv != "")
                    {
                        double rdwcvdouble = double.Parse(rdwcv);
                        DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                        double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                        double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                        if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                        {
                            flag = 2;
                        }
                        double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                        double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                        if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                        {
                            flag = 3;
                        }
                    }
                    string rdwsd = xcg.RDW_SD;
                    if (rdwsd != null && rdwsd != "")
                    {
                        double rdwsddouble = double.Parse(rdwsd);
                        DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                        double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                        double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                        if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                        {
                            flag = 2;
                        }
                        double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                        double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                        if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                        {
                            flag = 3;
                        }
                    }
                    string neut = xcg.NEUT;
                    if (neut != null && neut != "")
                    {
                        double neutdouble = double.Parse(neut);
                        DataRow[] drneut = dttv.Select("type='NEUT'");
                        double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                        double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                        if (neutdouble > neutwmax || neutdouble < neutwmin)
                        {
                            flag = 2;
                        }
                        double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                        double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                        if (neutdouble > neuttmax || neutdouble < neuttmin)
                        {
                            flag = 3;
                        }
                    }
                    string neutp = xcg.NEUTP;
                    if (neutp != null && neutp != "")
                    {
                        double neutpdouble = double.Parse(neutp);
                        DataRow[] drneutp = dttv.Select("type='NEUTP'");
                        double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                        double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                        if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                        {
                            flag = 2;
                        }
                        double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                        double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                        if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                        {
                            flag = 3;
                        }
                    }
                    string lym = xcg.LYM;
                    if (lym != null && lym != "")
                    {
                        double lymdouble = double.Parse(lym);
                        DataRow[] drlym = dttv.Select("type='LYM'");
                        double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                        double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                        if (lymdouble > lymwmax || lymdouble < lymwmin)
                        {
                            flag = 2;
                        }
                        double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                        double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                        if (lymdouble > lymtmax || lymdouble < lymtmin)
                        {
                            flag = 3;
                        }
                    }
                    string lymp = xcg.LYMP;
                    if (lymp != null && lymp != "")
                    {
                        double lympdouble = double.Parse(lymp);
                        DataRow[] drlymp = dttv.Select("type='LYMP'");
                        double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                        double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                        if (lympdouble > lympwmax || lympdouble < lympwmin)
                        {
                            flag = 2;
                        }
                        double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                        double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                        if (lympdouble > lymptmax || lympdouble < lymptmin)
                        {
                            flag = 3;
                        }
                    }
                    string mpv = xcg.MPV;
                    if (mpv != null && mpv != "")
                    {
                        double mpvdouble = double.Parse(mpv);
                        DataRow[] drmpv = dttv.Select("type='MPV'");
                        double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                        double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                        if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                        {
                            flag = 2;
                        }
                        double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                        double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                        if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                        {
                            flag = 3;
                        }
                    }
                    string pdw = xcg.PDW;
                    if (pdw != null && pdw != "")
                    {
                        double pdwdouble = double.Parse(pdw);
                        DataRow[] drpdw = dttv.Select("type='PDW'");
                        double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                        double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                        if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                        {
                            flag = 2;
                        }
                        double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                        double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                        if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                        {
                            flag = 3;
                        }
                    }
                    string mxd = xcg.MXD;
                    if (mxd != null && mxd != "")
                    {
                        double mxddouble = double.Parse(mxd);
                        DataRow[] drmxd = dttv.Select("type='MXD'");
                        double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                        double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                        if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                        {
                            flag = 2;
                        }
                        double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                        double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                        if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                        {
                            flag = 3;
                        }
                    }
                    string mxdp = xcg.MXDP;
                    if (mxdp != null && mxdp != "")
                    {
                        double mxdpdouble = double.Parse(mxdp);
                        DataRow[] drmxdp = dttv.Select("type='MXDP'");
                        double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                        double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                        if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                        {
                            flag = 2;
                        }
                        double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                        double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                        if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                        {
                            flag = 3;
                        }
                    }
                    tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                    tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                }
                else
                {
                    bool istrue1 = tjdao.updateXuechangguiInfo(xcg);
                    if (!istrue1)
                    {
                        return;
                    }
                    int flag = 1;
                    string wbc = xcg.WBC;
                    if (wbc != null && wbc != "")
                    {
                        double wbcdouble = double.Parse(wbc);
                        DataRow[] drwbc = dttv.Select("type='WBC'");
                        double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                        double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                        if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                        {
                            flag = 2;
                        }
                        double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                        double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                        if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                        {
                            flag = 3;
                        }
                    }
                    string rbc = xcg.RBC;
                    if (rbc != null && rbc != "")
                    {
                        double rbcdouble = double.Parse(rbc);
                        DataRow[] drrbc = dttv.Select("type='RBC'");
                        double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                        double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                        if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                        {
                            flag = 2;
                        }
                        double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                        double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                        if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                        {
                            flag = 3;
                        }
                    }
                    string pct = xcg.PCT;
                    if (pct != null && pct != "")
                    {
                        double pctdouble = double.Parse(pct);
                        DataRow[] drpct = dttv.Select("type='PCT'");
                        double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                        double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                        if (pctdouble > pctwmax || pctdouble < pctwmin)
                        {
                            flag = 2;
                        }
                        double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                        double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                        if (pctdouble > pcttmax || pctdouble < pcttmin)
                        {
                            flag = 3;
                        }
                    }
                    string plt = xcg.PLT;
                    if (plt != null && plt != "")
                    {
                        double pltdouble = double.Parse(plt);
                        DataRow[] drplt = dttv.Select("type='PLT'");
                        double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                        double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                        if (pltdouble > pltwmax || pltdouble < pltwmin)
                        {
                            flag = 2;
                        }
                        double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                        double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                        if (pltdouble > plttmax || pltdouble < plttmin)
                        {
                            flag = 3;
                        }
                    }
                    string hgb = xcg.HGB;
                    if (hgb != null && hgb != "")
                    {
                        double hgbdouble = double.Parse(hgb);
                        DataRow[] drhgb = dttv.Select("type='HGB'");
                        double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                        double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                        if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                        {
                            flag = 2;
                        }
                        double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                        double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                        if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                        {
                            flag = 3;
                        }
                    }
                    string hct = xcg.HCT;
                    if (hct != null && hct != "")
                    {
                        double hctdouble = double.Parse(hct);
                        DataRow[] drhct = dttv.Select("type='HCT'");
                        double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                        double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                        if (hctdouble > hctwmax || hctdouble < hctwmin)
                        {
                            flag = 2;
                        }
                        double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                        double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                        if (hctdouble > hcttmax || hctdouble < hcttmin)
                        {
                            flag = 3;
                        }
                    }
                    string mcv = xcg.MCV;
                    if (mcv != null && mcv != "")
                    {
                        double mcvdouble = double.Parse(mcv);
                        DataRow[] drmcv = dttv.Select("type='MCV'");
                        double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                        double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                        if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                        {
                            flag = 2;
                        }
                        double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                        double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                        if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                        {
                            flag = 3;
                        }
                    }
                    string mch = xcg.MCH;
                    if (mch != null && mch != "")
                    {
                        double mchdouble = double.Parse(mch);
                        DataRow[] drmch = dttv.Select("type='MCH'");
                        double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                        double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                        if (mchdouble > mchwmax || mchdouble < mchwmin)
                        {
                            flag = 2;
                        }
                        double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                        double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                        if (mchdouble > mchtmax || mchdouble < mchtmin)
                        {
                            flag = 3;
                        }
                    }
                    string mchc = xcg.MCHC;
                    if (mchc != null && mchc != "")
                    {
                        double mchcdouble = double.Parse(mchc);
                        DataRow[] drmchc = dttv.Select("type='MCHC'");
                        double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                        double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                        if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                        {
                            flag = 2;
                        }
                        double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                        double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                        if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                        {
                            flag = 3;
                        }
                    }
                    string rdwcv = xcg.RDW_CV;
                    if (rdwcv != null && rdwcv != "")
                    {
                        double rdwcvdouble = double.Parse(rdwcv);
                        DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                        double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                        double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                        if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                        {
                            flag = 2;
                        }
                        double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                        double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                        if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                        {
                            flag = 3;
                        }
                    }
                    string rdwsd = xcg.RDW_SD;
                    if (rdwsd != null && rdwsd != "")
                    {
                        double rdwsddouble = double.Parse(rdwsd);
                        DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                        double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                        double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                        if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                        {
                            flag = 2;
                        }
                        double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                        double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                        if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                        {
                            flag = 3;
                        }
                    }
                    string neut = xcg.NEUT;
                    if (neut != null && neut != "")
                    {
                        double neutdouble = double.Parse(neut);
                        DataRow[] drneut = dttv.Select("type='NEUT'");
                        double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                        double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                        if (neutdouble > neutwmax || neutdouble < neutwmin)
                        {
                            flag = 2;
                        }
                        double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                        double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                        if (neutdouble > neuttmax || neutdouble < neuttmin)
                        {
                            flag = 3;
                        }
                    }
                    string neutp = xcg.NEUTP;
                    if (neutp != null && neutp != "")
                    {
                        double neutpdouble = double.Parse(neutp);
                        DataRow[] drneutp = dttv.Select("type='NEUTP'");
                        double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                        double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                        if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                        {
                            flag = 2;
                        }
                        double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                        double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                        if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                        {
                            flag = 3;
                        }
                    }
                    string lym = xcg.LYM;
                    if (lym != null && lym != "")
                    {
                        double lymdouble = double.Parse(lym);
                        DataRow[] drlym = dttv.Select("type='LYM'");
                        double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                        double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                        if (lymdouble > lymwmax || lymdouble < lymwmin)
                        {
                            flag = 2;
                        }
                        double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                        double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                        if (lymdouble > lymtmax || lymdouble < lymtmin)
                        {
                            flag = 3;
                        }
                    }
                    string lymp = xcg.LYMP;
                    if (lymp != null && lymp != "")
                    {
                        double lympdouble = double.Parse(lymp);
                        DataRow[] drlymp = dttv.Select("type='LYMP'");
                        double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                        double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                        if (lympdouble > lympwmax || lympdouble < lympwmin)
                        {
                            flag = 2;
                        }
                        double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                        double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                        if (lympdouble > lymptmax || lympdouble < lymptmin)
                        {
                            flag = 3;
                        }
                    }
                    string mpv = xcg.MPV;
                    if (mpv != null && mpv != "")
                    {
                        double mpvdouble = double.Parse(mpv);
                        DataRow[] drmpv = dttv.Select("type='MPV'");
                        double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                        double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                        if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                        {
                            flag = 2;
                        }
                        double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                        double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                        if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                        {
                            flag = 3;
                        }
                    }
                    string pdw = xcg.PDW;
                    if (pdw != null && pdw != "")
                    {
                        double pdwdouble = double.Parse(pdw);
                        DataRow[] drpdw = dttv.Select("type='PDW'");
                        double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                        double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                        if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                        {
                            flag = 2;
                        }
                        double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                        double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                        if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                        {
                            flag = 3;
                        }
                    }
                    string mxd = xcg.MXD;
                    if (mxd != null && mxd != "")
                    {
                        double mxddouble = double.Parse(mxd);
                        DataRow[] drmxd = dttv.Select("type='MXD'");
                        double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                        double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                        if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                        {
                            flag = 2;
                        }
                        double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                        double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                        if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                        {
                            flag = 3;
                        }
                    }
                    string mxdp = xcg.MXDP;
                    if (mxdp != null && mxdp != "")
                    {
                        double mxdpdouble = double.Parse(mxdp);
                        DataRow[] drmxdp = dttv.Select("type='MXDP'");
                        double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                        double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                        if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                        {
                            flag = 2;
                        }
                        double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                        double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                        if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                        {
                            flag = 3;
                        }
                    }
                    tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                    tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                }
         }

        private bool initPort(string com)
        {
            try
            {
                if (!port.IsOpen)
                {
                    string portName = com;
                    int baudRate = 115200;
                    port.PortName = portName;
                    port.BaudRate = baudRate;
                    port.DtrEnable = true;
                    port.ReceivedBytesThreshold = 1;
                    port.Open();
                    return true;
                }
                else
                {
                    MessageBox.Show("库贝尔血球串口打开失败,请联系运维人员!");
                    return false;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("库贝尔血球串口打开失败,请联系运维人员!");
                return false;
            }
        }
        private string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        private void socketTcpMr()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);//方法已过期，可以获取IPv4的地址
            IPAddress ip = localhost.AddressList[0];
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(ip, 9001);
            //socket绑定监听地址
            serverSocket.Bind(point);
            //设置同时连接个数
            serverSocket.Listen(10);

            //利用线程后台执行监听,否则程序会假死
            Thread thread = new Thread(ListenMr);
            thread.IsBackground = true;
            thread.Start(serverSocket);
        }
        /// <summary>
        /// 监听连接
        /// </summary>
        private void ListenMr(object o)
        {
            var serverSocket = o as Socket;
            while (true)
            {
                //等待连接并且创建一个负责通讯的socket
                var send = serverSocket.Accept();
                //获取链接的IP地址
                //var sendIpoint = send.RemoteEndPoint.ToString();
                //开启一个新线程不停接收消息
                Thread thread = new Thread(ReciveMr);
                thread.IsBackground = true;
                thread.Start(send);
            }
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        private void ReciveMr(object o)
        {
            var send = o as Socket;
            totalByteRead = new Byte[0];
            while (true)
            {
                //获取发送过来的消息容器
                byte[] buffer = new byte[1024 * 5];
                var effective = 0;
                try
                {
                    effective = send.Receive(buffer);
                }
                catch { break; }
                //有效字节为0则跳过
                if (effective == 0)
                {
                    break;
                }
                byte[] buffernew = buffer.Skip(0).Take(effective).ToArray();
                totalByteRead = totalByteRead.Concat(buffernew).ToArray();
                if (totalByteRead.Length<100) { continue; }            
                string sHL7 = Encoding.Default.GetString(totalByteRead).Trim();
                string sendHL7new = "";
                string sendHL7 = "MSH|^~\\&|LIS||||20361231235956||ACK^R01|1|P|2.3.1||||||UNICODEMSA|AA|1";
                string[] sendArray = sendHL7.Split('|');
                string sendHL7sh = "MSH|^~\\&|||||20120508094823||ACK^R01|1|P|2.3.1||||2||ASCII|||MSA|AA|1|Message accepted|||0|";
                string[] sendArraysh = sendHL7sh.Split('|');
                if (sHL7.IndexOf("ASCII") > 0)
                {//解析生化协议报文数据
                    if (sHL7.Substring(0, 3) != "MSH" || sHL7.Substring(sHL7.Length-1, 1)!="|")
                    {
                        continue;
                    }
                    shenghuaBean sh = new shenghuaBean();
                    sh.ZrysSH = basicInfoSettings.sh;
                    string[] sHL7Pids = Regex.Split(sHL7, "PID", RegexOptions.IgnoreCase);
                    if (sHL7Pids.Length == 0) { return; };
                    string[] MSHArray = sHL7Pids[0].Split('|');
                    sendArraysh[6] = MSHArray[6];
                    sendArraysh[9] = MSHArray[9];
                    sendArraysh[22] = MSHArray[9];
                    string[] sHL7PArray = sHL7Pids[1].Split('|');
                    sh.bar_code = sHL7PArray[33];
                    DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(sh.bar_code);
                    if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                    {
                        sh.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                        sh.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                    }
                    else
                    {
                        return;
                    }
                    //把HL7分成段
                    string[] sHL7Lines = Regex.Split(sHL7, "OBX", RegexOptions.IgnoreCase);
                    if (sHL7Lines.Length == 0) { return; };
                    for (int i = 1; i < sHL7Lines.Length; i++)
                    {
                        string[] sHL7Array = sHL7Lines[i].Split('|');
                        switch (sHL7Array[4])
                        {
                            case "ALB": sh.ALB = sHL7Array[5]; break;
                            case "ALP": sh.ALP = sHL7Array[5]; break;
                            case "ALT": sh.ALT = sHL7Array[5]; break;
                            case "AST": sh.AST = sHL7Array[5]; break;
                            case "TC": sh.CHO = sHL7Array[5]; break;
                            case "CREA-S": sh.Crea = sHL7Array[5]; break;
                            case "D-Bil-V": sh.DBIL = sHL7Array[5]; break;
                            case "Glu-G": sh.GLU = sHL7Array[5]; break;
                            case "HDL-C": sh.HDL_C = sHL7Array[5]; break;
                            case "LDL-C": sh.LDL_C = sHL7Array[5]; break;
                            case "T-Bil-V": sh.TBIL = sHL7Array[5]; break;
                            case "TG": sh.TG = sHL7Array[5]; break;
                            case "TP": sh.TP = sHL7Array[5]; break;
                            case "UA": sh.UA = sHL7Array[5]; break;
                            case "UREA": sh.UREA = sHL7Array[5]; break;
                            default: break;
                        }
                    }
                    sh.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    bool istrue = tjdao.insertShenghuaInfo(sh);
                    if (istrue)
                    {
                        #region 生化
                        int flag = 1;
                        string alt = sh.ALT;
                        if (alt != null && alt != "*")
                        {
                            double altdouble = double.Parse(alt);
                            DataRow[] dralt = dttv.Select("type='ALT'");
                            double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                            double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                            if (altdouble > altwmax || altdouble < altwmin)
                            {
                                flag = 2;
                            }
                            double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                            double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                            if (altdouble > alttmax || altdouble < alttmin)
                            {
                                flag = 3;
                            }
                        }

                        string ast = sh.AST;
                        if (ast != null && ast != "*")
                        {
                            double astdouble = double.Parse(ast);
                            DataRow[] drast = dttv.Select("type='AST'");
                            double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                            double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                            if (astdouble > astwmax || astdouble < astwmin)
                            {
                                flag = 2;
                            }
                            double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                            double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                            if (astdouble > asttmax || astdouble < asttmin)
                            {
                                flag = 3;
                            }
                        }

                        string tbil = sh.TBIL;
                        if (tbil != null && tbil != "*")
                        {
                            double tbildouble = double.Parse(tbil);
                            DataRow[] drtbil = dttv.Select("type='TBIL'");
                            double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                            double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                            if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                            {
                                flag = 2;
                            }
                            double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                            double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                            if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                            {
                                flag = 3;
                            }
                        }

                        string dbil = sh.DBIL;
                        if (dbil != null && dbil != "*")
                        {
                            double dbildouble = double.Parse(dbil);
                            DataRow[] drtbil = dttv.Select("type='DBIL'");
                            double dbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                            double dbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                            if (dbildouble > dbilwmax || dbildouble < dbilwmin)
                            {
                                flag = 2;
                            }
                            double dbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                            double dbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                            if (dbildouble > dbiltmax || dbildouble < dbiltmin)
                            {
                                flag = 3;
                            }
                        }

                        string crea = sh.Crea;
                        if (crea != null && crea != "*")
                        {
                            double creadouble = double.Parse(crea);
                            DataRow[] drcrea = dttv.Select("type='CREA'");
                            double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                            double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                            if (creadouble > creawmax || creadouble < creawmin)
                            {
                                flag = 2;
                            }
                            double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                            double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                            if (creadouble > creatmax || creadouble < creatmin)
                            {
                                flag = 3;
                            }
                        }

                        string urea = sh.UREA;
                        if (urea != null && urea != "*")
                        {
                            double ureadouble = double.Parse(urea);
                            DataRow[] drurea = dttv.Select("type='UREA'");
                            double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                            double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                            if (ureadouble > ureawmax || ureadouble < ureawmin)
                            {
                                flag = 2;
                            }
                            double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                            double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                            if (ureadouble > ureatmax || ureadouble < ureatmin)
                            {
                                flag = 3;
                            }
                        }

                        string glu = sh.GLU;
                        if (glu != null && glu != "*")
                        {
                            double gludouble = double.Parse(glu);
                            DataRow[] drglu = dttv.Select("type='GLU'");
                            double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                            double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                            if (gludouble > gluwmax || gludouble < gluwmin)
                            {
                                flag = 2;
                            }
                            double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                            double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                            if (gludouble > glutmax || gludouble < glutmin)
                            {
                                flag = 3;
                            }
                        }

                        string tg = sh.TG;
                        if (tg != null && tg != "*")
                        {
                            double tgdouble = double.Parse(tg);
                            DataRow[] drtg = dttv.Select("type='TG'");
                            double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                            double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                            if (tgdouble > tgwmax || tgdouble < tgwmin)
                            {
                                flag = 2;
                            }
                            double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                            double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                            if (tgdouble > tgtmax || tgdouble < tgtmin)
                            {
                                flag = 3;
                            }
                        }

                        string cho = sh.CHO;
                        if (cho != null && cho != "*")
                        {
                            double chodouble = double.Parse(cho);
                            DataRow[] drcho = dttv.Select("type='CHO'");
                            double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                            double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                            if (chodouble > chowmax || chodouble < chowmin)
                            {
                                flag = 2;
                            }
                            double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                            double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                            if (chodouble > chotmax || chodouble < chotmin)
                            {
                                flag = 3;
                            }
                        }

                        string hdlc = sh.HDL_C;
                        if (hdlc != null && hdlc != "*")
                        {
                            double hdlcdouble = double.Parse(hdlc);
                            DataRow[] drhdlc = dttv.Select("type='HDLC'");
                            double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                            double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                            if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                            {
                                flag = 2;
                            }
                            double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                            double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                            if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                            {
                                flag = 3;
                            }
                        }

                        string ldlc = sh.LDL_C;
                        if (ldlc != null && ldlc != "*")
                        {
                            double ldlcdouble = double.Parse(ldlc);
                            DataRow[] drldlc = dttv.Select("type='LDLC'");
                            double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                            double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                            if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                            {
                                flag = 2;
                            }
                            double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                            double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                            if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                            {
                                flag = 3;
                            }
                        }

                        tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                        #endregion
                    }
                    else
                    {
                        bool istree = tjdao.updateShenghuaInfo(sh);
                        if (!istree) return;
                        #region 生化
                        int flag = 1;
                        string alt = sh.ALT;
                        if (alt != null && alt != "*")
                        {
                            double altdouble = double.Parse(alt);
                            DataRow[] dralt = dttv.Select("type='ALT'");
                            double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                            double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                            if (altdouble > altwmax || altdouble < altwmin)
                            {
                                flag = 2;
                            }
                            double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                            double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                            if (altdouble > alttmax || altdouble < alttmin)
                            {
                                flag = 3;
                            }
                        }

                        string ast = sh.AST;
                        if (ast != null && ast != "*")
                        {
                            double astdouble = double.Parse(ast);
                            DataRow[] drast = dttv.Select("type='AST'");
                            double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                            double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                            if (astdouble > astwmax || astdouble < astwmin)
                            {
                                flag = 2;
                            }
                            double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                            double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                            if (astdouble > asttmax || astdouble < asttmin)
                            {
                                flag = 3;
                            }
                        }

                        string tbil = sh.TBIL;
                        if (tbil != null && tbil != "*")
                        {
                            double tbildouble = double.Parse(tbil);
                            DataRow[] drtbil = dttv.Select("type='TBIL'");
                            double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                            double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                            if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                            {
                                flag = 2;
                            }
                            double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                            double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                            if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                            {
                                flag = 3;
                            }
                        }
                        string dbil = sh.DBIL;
                        if (dbil != null && dbil != "*")
                        {
                            double dbildouble = double.Parse(dbil);
                            DataRow[] drtbil = dttv.Select("type='DBIL'");
                            double dbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                            double dbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                            if (dbildouble > dbilwmax || dbildouble < dbilwmin)
                            {
                                flag = 2;
                            }
                            double dbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                            double dbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                            if (dbildouble > dbiltmax || dbildouble < dbiltmin)
                            {
                                flag = 3;
                            }
                        }
                        string crea = sh.Crea;
                        if (crea != null && crea != "*")
                        {
                            double creadouble = double.Parse(crea);
                            DataRow[] drcrea = dttv.Select("type='CREA'");
                            double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                            double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                            if (creadouble > creawmax || creadouble < creawmin)
                            {
                                flag = 2;
                            }
                            double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                            double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                            if (creadouble > creatmax || creadouble < creatmin)
                            {
                                flag = 3;
                            }
                        }

                        string urea = sh.UREA;
                        if (urea != null && urea != "*")
                        {
                            double ureadouble = double.Parse(urea);
                            DataRow[] drurea = dttv.Select("type='UREA'");
                            double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                            double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                            if (ureadouble > ureawmax || ureadouble < ureawmin)
                            {
                                flag = 2;
                            }
                            double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                            double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                            if (ureadouble > ureatmax || ureadouble < ureatmin)
                            {
                                flag = 3;
                            }
                        }

                        string glu = sh.GLU;
                        if (glu != null && glu != "*")
                        {
                            double gludouble = double.Parse(glu);
                            DataRow[] drglu = dttv.Select("type='GLU'");
                            double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                            double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                            if (gludouble > gluwmax || gludouble < gluwmin)
                            {
                                flag = 2;
                            }
                            double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                            double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                            if (gludouble > glutmax || gludouble < glutmin)
                            {
                                flag = 3;
                            }
                        }

                        string tg = sh.TG;
                        if (tg != null && tg != "*")
                        {
                            double tgdouble = double.Parse(tg);
                            DataRow[] drtg = dttv.Select("type='TG'");
                            double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                            double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                            if (tgdouble > tgwmax || tgdouble < tgwmin)
                            {
                                flag = 2;
                            }
                            double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                            double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                            if (tgdouble > tgtmax || tgdouble < tgtmin)
                            {
                                flag = 3;
                            }
                        }

                        string cho = sh.CHO;
                        if (cho != null && cho != "*")
                        {
                            double chodouble = double.Parse(cho);
                            DataRow[] drcho = dttv.Select("type='CHO'");
                            double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                            double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                            if (chodouble > chowmax || chodouble < chowmin)
                            {
                                flag = 2;
                            }
                            double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                            double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                            if (chodouble > chotmax || chodouble < chotmin)
                            {
                                flag = 3;
                            }
                        }

                        string hdlc = sh.HDL_C;
                        if (hdlc != null && hdlc != "*")
                        {
                            double hdlcdouble = double.Parse(hdlc);
                            DataRow[] drhdlc = dttv.Select("type='HDLC'");
                            double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                            double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                            if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                            {
                                flag = 2;
                            }
                            double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                            double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                            if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                            {
                                flag = 3;
                            }
                        }

                        string ldlc = sh.LDL_C;
                        if (ldlc != null && ldlc != "*")
                        {
                            double ldlcdouble = double.Parse(ldlc);
                            DataRow[] drldlc = dttv.Select("type='LDLC'");
                            double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                            double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                            if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                            {
                                flag = 2;
                            }
                            double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                            double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                            if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                            {
                                flag = 3;
                            }
                        }
                        tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                        #endregion
                    }
                    //返回生化的确认数据报文
                    for (int j = 0; j < sendArraysh.Length; j++)
                    {
                        sendHL7sh += "|" + sendArraysh[j];
                    }
                    send.Send(AckKbe(sendHL7sh));
                }
                else if(sHL7.IndexOf("UNICODE") >1)
                {//解析血球协议报文数据
                    try
                    {
                        if (sHL7.Substring(0, 3) != "MSH" || sHL7.Substring(sHL7.Length - 1, 1) != "F")
                        {
                            continue;
                        }
                        xuechangguiBean xcg = new xuechangguiBean();
                        xcg.ZrysXCG = basicInfoSettings.xcg;
                        string[] sHL7Pids = Regex.Split(sHL7, "PID", RegexOptions.IgnoreCase);
                        if (sHL7Pids.Length == 0) { return; };
                        string[] MSHArray = sHL7Pids[0].Split('|');
                        sendArray[6] = MSHArray[6];
                        sendArray[9] = MSHArray[9];
                        sendArray[19] = MSHArray[9];
                        string[] sHL7PArray = sHL7Pids[1].Split('|');
                        xcg.bar_code = sHL7PArray[14];
                        DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(xcg.bar_code);
                        if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                        {
                            xcg.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                            xcg.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                        }
                        else
                        {
                            return;
                        }
                        xcg.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //把HL7分成段
                        string[] sHL7Lines = Regex.Split(sHL7, "OBX", RegexOptions.IgnoreCase);
                        if (sHL7Lines.Length == 0) { return; };
                        for (int i = 1; i < sHL7Lines.Length; i++)
                        {
                            string[] sHL7Array = sHL7Lines[i].Split('|');
                            if (sHL7Array[2].IndexOf("NM") == -1)
                            {
                                continue;
                            }
                            if (sHL7Array[3].IndexOf("WBC^LN") > -1)
                            {
                                xcg.WBC = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("LYM#") > -1)
                            {
                                xcg.LYM = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("LYM%") > -1)
                            {
                                xcg.LYMP = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("RBC^LN") > -1)
                            {
                                xcg.RBC = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("HGB^LN") > -1)
                            {
                                xcg.HGB = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("PCT") > -1)
                            {
                                xcg.PCT = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("PLT^LN") > -1)
                            {
                                xcg.PLT = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("HCT") > -1)
                            {
                                xcg.HCT = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MCV") > -1)
                            {
                                xcg.MCV = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MCH^LN") > -1)
                            {
                                xcg.MCH = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MCHC^LN") > -1)
                            {
                                xcg.MCHC = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("RDW-CV") > -1)
                            {
                                xcg.RDW_CV = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("RDW-SD") > -1)
                            {
                                xcg.RDW_SD = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("NEU#") > -1)
                            {
                                xcg.NEUT = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("NEU%") > -1)
                            {
                                xcg.NEUTP = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MPV") > -1)
                            {
                                xcg.MPV = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("PDW^LN") > -1)
                            {
                                xcg.PDW = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MID#") > -1)
                            {
                                xcg.MXD = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MID%") > -1)
                            {
                                xcg.MXDP = sHL7Array[5]; continue;
                            }
                            else {
                                continue;
                            }
                        }
                        bool istrue = tjdao.insertXuechangguiInfo(xcg);
                        if (istrue)
                        {
                            int flag = 1;
                            string wbc = xcg.WBC;
                            if (wbc != null && wbc != "*")
                            {
                                double wbcdouble = double.Parse(wbc);
                                DataRow[] drwbc = dttv.Select("type='WBC'");
                                double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                                double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                                if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                                {
                                    flag = 2;
                                }
                                double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                                double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                                if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rbc = xcg.RBC;
                            if (rbc != null && rbc != "*")
                            {
                                double rbcdouble = double.Parse(rbc);
                                DataRow[] drrbc = dttv.Select("type='RBC'");
                                double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                                double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                                if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                                {
                                    flag = 2;
                                }
                                double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                                double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                                if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string pct = xcg.PCT;
                            if (pct != null && pct != "*")
                            {
                                double pctdouble = double.Parse(pct);
                                DataRow[] drpct = dttv.Select("type='PCT'");
                                double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                                double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                                if (pctdouble > pctwmax || pctdouble < pctwmin)
                                {
                                    flag = 2;
                                }
                                double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                                double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                                if (pctdouble > pcttmax || pctdouble < pcttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string plt = xcg.PLT;
                            if (plt != null && plt != "*")
                            {
                                double pltdouble = double.Parse(plt);
                                DataRow[] drplt = dttv.Select("type='PLT'");
                                double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                                double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                                if (pltdouble > pltwmax || pltdouble < pltwmin)
                                {
                                    flag = 2;
                                }
                                double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                                double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                                if (pltdouble > plttmax || pltdouble < plttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string hgb = xcg.HGB;
                            if (hgb != null && hgb != "*")
                            {
                                double hgbdouble = double.Parse(hgb);
                                DataRow[] drhgb = dttv.Select("type='HGB'");
                                double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                                double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                                if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                                {
                                    flag = 2;
                                }
                                double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                                double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                                if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string hct = xcg.HCT;
                            if (hct != null && hct != "*")
                            {
                                double hctdouble = double.Parse(hct);
                                DataRow[] drhct = dttv.Select("type='HCT'");
                                double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                                double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                                if (hctdouble > hctwmax || hctdouble < hctwmin)
                                {
                                    flag = 2;
                                }
                                double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                                double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                                if (hctdouble > hcttmax || hctdouble < hcttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mcv = xcg.MCV;
                            if (mcv != null && mcv != "*")
                            {
                                double mcvdouble = double.Parse(mcv);
                                DataRow[] drmcv = dttv.Select("type='MCV'");
                                double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                                double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                                if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                                {
                                    flag = 2;
                                }
                                double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                                double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                                if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mch = xcg.MCH;
                            if (mch != null && mch != "*")
                            {
                                double mchdouble = double.Parse(mch);
                                DataRow[] drmch = dttv.Select("type='MCH'");
                                double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                                double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                                if (mchdouble > mchwmax || mchdouble < mchwmin)
                                {
                                    flag = 2;
                                }
                                double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                                double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                                if (mchdouble > mchtmax || mchdouble < mchtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mchc = xcg.MCHC;
                            if (mchc != null && mchc != "*")
                            {
                                double mchcdouble = double.Parse(mchc);
                                DataRow[] drmchc = dttv.Select("type='MCHC'");
                                double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                                double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                                if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                                {
                                    flag = 2;
                                }
                                double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                                double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                                if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rdwcv = xcg.RDW_CV;
                            if (rdwcv != null && rdwcv != "*")
                            {
                                double rdwcvdouble = double.Parse(rdwcv);
                                DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                                double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                                double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                                if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                                {
                                    flag = 2;
                                }
                                double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                                double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                                if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rdwsd = xcg.RDW_SD;
                            if (rdwsd != null && rdwsd != "*")
                            {
                                double rdwsddouble = double.Parse(rdwsd);
                                DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                                double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                                double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                                if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                                {
                                    flag = 2;
                                }
                                double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                                double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                                if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string neut = xcg.NEUT;
                            if (neut != null && neut != "*")
                            {
                                double neutdouble = double.Parse(neut);
                                DataRow[] drneut = dttv.Select("type='NEUT'");
                                double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                                double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                                if (neutdouble > neutwmax || neutdouble < neutwmin)
                                {
                                    flag = 2;
                                }
                                double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                                double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                                if (neutdouble > neuttmax || neutdouble < neuttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string neutp = xcg.NEUTP;
                            if (neutp != null && neutp != "*")
                            {
                                double neutpdouble = double.Parse(neutp);
                                DataRow[] drneutp = dttv.Select("type='NEUTP'");
                                double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                                double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                                if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                                {
                                    flag = 2;
                                }
                                double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                                double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                                if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                                {
                                    flag = 3;
                                }
                            }
                            string lym = xcg.LYM;
                            if (lym != null && lym != "*")
                            {
                                double lymdouble = double.Parse(lym);
                                DataRow[] drlym = dttv.Select("type='LYM'");
                                double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                                double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                                if (lymdouble > lymwmax || lymdouble < lymwmin)
                                {
                                    flag = 2;
                                }
                                double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                                double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                                if (lymdouble > lymtmax || lymdouble < lymtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string lymp = xcg.LYMP;
                            if (lymp != null && lymp != "*")
                            {
                                double lympdouble = double.Parse(lymp);
                                DataRow[] drlymp = dttv.Select("type='LYMP'");
                                double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                                double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                                if (lympdouble > lympwmax || lympdouble < lympwmin)
                                {
                                    flag = 2;
                                }
                                double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                                double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                                if (lympdouble > lymptmax || lympdouble < lymptmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mpv = xcg.MPV;
                            if (mpv != null && mpv != "*")
                            {
                                double mpvdouble = double.Parse(mpv);
                                DataRow[] drmpv = dttv.Select("type='MPV'");
                                double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                                double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                                if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                                {
                                    flag = 2;
                                }
                                double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                                double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                                if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string pdw = xcg.PDW;
                            if (pdw != null && pdw != "*")
                            {
                                double pdwdouble = double.Parse(pdw);
                                DataRow[] drpdw = dttv.Select("type='PDW'");
                                double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                                double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                                if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                                {
                                    flag = 2;
                                }
                                double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                                double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                                if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mxd = xcg.MXD;
                            if (mxd != null && mxd != "*")
                            {
                                double mxddouble = double.Parse(mxd);
                                DataRow[] drmxd = dttv.Select("type='MXD'");
                                double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                                double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                                if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                                {
                                    flag = 2;
                                }
                                double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                                double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                                if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mxdp = xcg.MXDP;
                            if (mxdp != null && mxdp != "*")
                            {
                                double mxdpdouble = double.Parse(mxdp);
                                DataRow[] drmxdp = dttv.Select("type='MXDP'");
                                double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                                double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                                if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                                {
                                    flag = 2;
                                }
                                double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                                double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                                if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                                {
                                    flag = 3;
                                }
                            }
                            tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                            tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                        }
                        else
                        {
                            bool istrue1 = tjdao.updateXuechangguiInfo(xcg);
                            if (!istrue1)
                            {
                                return;
                            }
                            int flag = 1;
                            string wbc = xcg.WBC;
                            if (wbc != null && wbc != "*")
                            {
                                double wbcdouble = double.Parse(wbc);
                                DataRow[] drwbc = dttv.Select("type='WBC'");
                                double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                                double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                                if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                                {
                                    flag = 2;
                                }
                                double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                                double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                                if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rbc = xcg.RBC;
                            if (rbc != null && rbc != "*")
                            {
                                double rbcdouble = double.Parse(rbc);
                                DataRow[] drrbc = dttv.Select("type='RBC'");
                                double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                                double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                                if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                                {
                                    flag = 2;
                                }
                                double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                                double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                                if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string pct = xcg.PCT;
                            if (pct != null && pct != "*")
                            {
                                double pctdouble = double.Parse(pct);
                                DataRow[] drpct = dttv.Select("type='PCT'");
                                double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                                double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                                if (pctdouble > pctwmax || pctdouble < pctwmin)
                                {
                                    flag = 2;
                                }
                                double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                                double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                                if (pctdouble > pcttmax || pctdouble < pcttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string plt = xcg.PLT;
                            if (plt != null && plt != "*")
                            {
                                double pltdouble = double.Parse(plt);
                                DataRow[] drplt = dttv.Select("type='PLT'");
                                double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                                double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                                if (pltdouble > pltwmax || pltdouble < pltwmin)
                                {
                                    flag = 2;
                                }
                                double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                                double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                                if (pltdouble > plttmax || pltdouble < plttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string hgb = xcg.HGB;
                            if (hgb != null && hgb != "*")
                            {
                                double hgbdouble = double.Parse(hgb);
                                DataRow[] drhgb = dttv.Select("type='HGB'");
                                double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                                double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                                if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                                {
                                    flag = 2;
                                }
                                double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                                double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                                if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string hct = xcg.HCT;
                            if (hct != null && hct != "*")
                            {
                                double hctdouble = double.Parse(hct);
                                DataRow[] drhct = dttv.Select("type='HCT'");
                                double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                                double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                                if (hctdouble > hctwmax || hctdouble < hctwmin)
                                {
                                    flag = 2;
                                }
                                double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                                double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                                if (hctdouble > hcttmax || hctdouble < hcttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mcv = xcg.MCV;
                            if (mcv != null && mcv != "*")
                            {
                                double mcvdouble = double.Parse(mcv);
                                DataRow[] drmcv = dttv.Select("type='MCV'");
                                double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                                double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                                if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                                {
                                    flag = 2;
                                }
                                double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                                double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                                if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mch = xcg.MCH;
                            if (mch != null && mch != "*")
                            {
                                double mchdouble = double.Parse(mch);
                                DataRow[] drmch = dttv.Select("type='MCH'");
                                double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                                double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                                if (mchdouble > mchwmax || mchdouble < mchwmin)
                                {
                                    flag = 2;
                                }
                                double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                                double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                                if (mchdouble > mchtmax || mchdouble < mchtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mchc = xcg.MCHC;
                            if (mchc != null && mchc != "*")
                            {
                                double mchcdouble = double.Parse(mchc);
                                DataRow[] drmchc = dttv.Select("type='MCHC'");
                                double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                                double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                                if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                                {
                                    flag = 2;
                                }
                                double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                                double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                                if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rdwcv = xcg.RDW_CV;
                            if (rdwcv != null && rdwcv != "*")
                            {
                                double rdwcvdouble = double.Parse(rdwcv);
                                DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                                double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                                double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                                if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                                {
                                    flag = 2;
                                }
                                double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                                double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                                if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rdwsd = xcg.RDW_SD;
                            if (rdwsd != null && rdwsd != "*")
                            {
                                double rdwsddouble = double.Parse(rdwsd);
                                DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                                double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                                double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                                if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                                {
                                    flag = 2;
                                }
                                double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                                double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                                if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string neut = xcg.NEUT;
                            if (neut != null && neut != "*")
                            {
                                double neutdouble = double.Parse(neut);
                                DataRow[] drneut = dttv.Select("type='NEUT'");
                                double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                                double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                                if (neutdouble > neutwmax || neutdouble < neutwmin)
                                {
                                    flag = 2;
                                }
                                double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                                double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                                if (neutdouble > neuttmax || neutdouble < neuttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string neutp = xcg.NEUTP;
                            if (neutp != null && neutp != "*")
                            {
                                double neutpdouble = double.Parse(neutp);
                                DataRow[] drneutp = dttv.Select("type='NEUTP'");
                                double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                                double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                                if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                                {
                                    flag = 2;
                                }
                                double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                                double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                                if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                                {
                                    flag = 3;
                                }
                            }
                            string lym = xcg.LYM;
                            if (lym != null && lym != "*")
                            {
                                double lymdouble = double.Parse(lym);
                                DataRow[] drlym = dttv.Select("type='LYM'");
                                double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                                double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                                if (lymdouble > lymwmax || lymdouble < lymwmin)
                                {
                                    flag = 2;
                                }
                                double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                                double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                                if (lymdouble > lymtmax || lymdouble < lymtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string lymp = xcg.LYMP;
                            if (lymp != null && lymp != "*")
                            {
                                double lympdouble = double.Parse(lymp);
                                DataRow[] drlymp = dttv.Select("type='LYMP'");
                                double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                                double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                                if (lympdouble > lympwmax || lympdouble < lympwmin)
                                {
                                    flag = 2;
                                }
                                double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                                double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                                if (lympdouble > lymptmax || lympdouble < lymptmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mpv = xcg.MPV;
                            if (mpv != null && mpv != "*")
                            {
                                double mpvdouble = double.Parse(mpv);
                                DataRow[] drmpv = dttv.Select("type='MPV'");
                                double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                                double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                                if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                                {
                                    flag = 2;
                                }
                                double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                                double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                                if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string pdw = xcg.PDW;
                            if (pdw != null && pdw != "*")
                            {
                                double pdwdouble = double.Parse(pdw);
                                DataRow[] drpdw = dttv.Select("type='PDW'");
                                double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                                double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                                if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                                {
                                    flag = 2;
                                }
                                double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                                double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                                if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mxd = xcg.MXD;
                            if (mxd != null && mxd != "*")
                            {
                                double mxddouble = double.Parse(mxd);
                                DataRow[] drmxd = dttv.Select("type='MXD'");
                                double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                                double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                                if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                                {
                                    flag = 2;
                                }
                                double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                                double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                                if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mxdp = xcg.MXDP;
                            if (mxdp != null && mxdp != "*")
                            {
                                double mxdpdouble = double.Parse(mxdp);
                                DataRow[] drmxdp = dttv.Select("type='MXDP'");
                                double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                                double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                                if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                                {
                                    flag = 2;
                                }
                                double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                                double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                                if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                                {
                                    flag = 3;
                                }
                            }
                            tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                            tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                        }
                        //返回血球的确认数据报文
                        for (int j = 0; j < sendArray.Length; j++)
                        {
                            sendHL7new += "|" + sendArray[j];
                        }
                        send.Send(AckKbe(sendHL7new.Substring(1)));
                    }
                    catch (Exception ex)
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                }
                totalByteRead = new Byte[0];
            }
        }
        private void socketTcpDr()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);//方法已过期，可以获取IPv4的地址
            IPAddress ip = localhost.AddressList[0];
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(ip, 9001);
            //socket绑定监听地址
            serverSocket.Bind(point);
            //设置同时连接个数
            serverSocket.Listen(10);
            
            //利用线程后台执行监听,否则程序会假死
            Thread thread = new Thread(ListenDr);
            thread.IsBackground = true;
            thread.Start(serverSocket);
        }
        /// <summary>
        /// 监听连接
        /// </summary>
        private void ListenDr(object o)
        {
            var serverSocket = o as Socket;
            while (true)
            {
                //等待连接并且创建一个负责通讯的socket
                var send = serverSocket.Accept();
                //获取链接的IP地址
                //var sendIpoint = send.RemoteEndPoint.ToString();
                //开启一个新线程不停接收消息
                Thread thread = new Thread(ReciveDr);
                thread.IsBackground = true;
                thread.Start(send);
            }
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        private void ReciveDr(object o)
        {
            var send = o as Socket;
            totalByteRead = new Byte[0];
            while (true)
            {
                //获取发送过来的消息容器
                byte[] buffer = new byte[1024 * 5];
                var effective = 0;
                try
                {
                    effective = send.Receive(buffer);
                }
                catch { break; }
                //有效字节为0则跳过
                if (effective == 0)
                {
                    break;
                }
                byte[] buffernew = buffer.Skip(0).Take(effective).ToArray();
                totalByteRead = totalByteRead.Concat(buffernew).ToArray();
                if (totalByteRead.Length < 100) { continue; }
                string sHL7 = Encoding.Default.GetString(totalByteRead).Trim();
                string sendHL7new = "";
                string sendHL7 = "MSH|^~\\&|LIS||||20361231235956||ACK^R01|1|P|2.3.1||||||UTF8MSA|AA|1||||0";
                string[] sendArray = sendHL7.Split('|');
                string sendHL7sh = "MSH|^~\\&|Analyzer ID|CS6400|LIS ID||20120508094823||ACK^R01|1|P|2.3.1||||0||UNICODE|||MSA|AA|1||||0|";
                string[] sendArraysh = sendHL7sh.Split('|');
                if (sHL7.IndexOf("UTF-8") > 0)
                {//解析生化协议报文数据
                    if (sHL7.Substring(0, 3) != "MSH" || sHL7.Substring(sHL7.Length - 1, 1) != "|")
                    {
                        continue;
                    }
                    shenghuaBean sh = new shenghuaBean();
                    sh.ZrysSH = basicInfoSettings.sh;
                    string[] sHL7Pids = Regex.Split(sHL7, "PID", RegexOptions.IgnoreCase);
                    if (sHL7Pids.Length == 0) { return; };
                    string[] MSHArray = sHL7Pids[0].Split('|');
                    sendArraysh[6] = MSHArray[6];
                    sendArraysh[9] = MSHArray[9];
                    sendArraysh[22] = MSHArray[9];
                    string[] sHL7PArray = sHL7Pids[1].Split('|');
                    sh.bar_code = sHL7PArray[33];
                    DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(sh.bar_code);
                    if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                    {
                        sh.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                        sh.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                    }
                    else
                    {
                        return;
                    }
                    //把HL7分成段
                    string[] sHL7Lines = Regex.Split(sHL7, "OBX", RegexOptions.IgnoreCase);
                    if (sHL7Lines.Length == 0) { return; };
                    for (int i = 1; i < sHL7Lines.Length; i++)
                    {
                        string[] sHL7Array = sHL7Lines[i].Split('|');
                        switch (sHL7Array[3])
                        {
                            case "ALB^1": sh.ALB = sHL7Array[4]; break;
                            case "ALP^1": sh.ALP = sHL7Array[4]; break;
                            case "ALT^1": sh.ALT = sHL7Array[4]; break;
                            case "AST^1": sh.AST = sHL7Array[4]; break;
                            case "TC^1": sh.CHO = sHL7Array[4]; break;
                            case "CRE^1": sh.Crea = sHL7Array[4]; break;
                            case "DBIL^1": sh.DBIL = sHL7Array[4]; break;
                            case "GLU^1": sh.GLU = sHL7Array[4]; break;
                            case "HDL-C^1": sh.HDL_C = sHL7Array[4]; break;
                            case "LDL-C^1": sh.LDL_C = sHL7Array[4]; break;
                            case "TBIL^1": sh.TBIL = sHL7Array[4]; break;
                            case "TG^1": sh.TG = sHL7Array[4]; break;
                            case "TP^1": sh.TP = sHL7Array[4]; break;
                            case "UA^1": sh.UA = sHL7Array[4]; break;
                            case "UREA^1": sh.UREA = sHL7Array[4]; break;
                            default: break;
                        }
                    }
                    sh.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    bool istrue = tjdao.insertShenghuaInfo(sh);
                    if (istrue)
                    {
                        #region 生化
                        int flag = 1;
                        string alt = sh.ALT;
                        if (alt != null && alt != "*")
                        {
                            double altdouble = double.Parse(alt);
                            DataRow[] dralt = dttv.Select("type='ALT'");
                            double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                            double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                            if (altdouble > altwmax || altdouble < altwmin)
                            {
                                flag = 2;
                            }
                            double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                            double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                            if (altdouble > alttmax || altdouble < alttmin)
                            {
                                flag = 3;
                            }
                        }

                        string ast = sh.AST;
                        if (ast != null && ast != "*")
                        {
                            double astdouble = double.Parse(ast);
                            DataRow[] drast = dttv.Select("type='AST'");
                            double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                            double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                            if (astdouble > astwmax || astdouble < astwmin)
                            {
                                flag = 2;
                            }
                            double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                            double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                            if (astdouble > asttmax || astdouble < asttmin)
                            {
                                flag = 3;
                            }
                        }

                        string tbil = sh.TBIL;
                        if (tbil != null && tbil != "*")
                        {
                            double tbildouble = double.Parse(tbil);
                            DataRow[] drtbil = dttv.Select("type='TBIL'");
                            double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                            double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                            if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                            {
                                flag = 2;
                            }
                            double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                            double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                            if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                            {
                                flag = 3;
                            }
                        }

                        string dbil = sh.DBIL;
                        if (dbil != null && dbil != "*")
                        {
                            double dbildouble = double.Parse(dbil);
                            DataRow[] drtbil = dttv.Select("type='DBIL'");
                            double dbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                            double dbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                            if (dbildouble > dbilwmax || dbildouble < dbilwmin)
                            {
                                flag = 2;
                            }
                            double dbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                            double dbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                            if (dbildouble > dbiltmax || dbildouble < dbiltmin)
                            {
                                flag = 3;
                            }
                        }

                        string crea = sh.Crea;
                        if (crea != null && crea != "*")
                        {
                            double creadouble = double.Parse(crea);
                            DataRow[] drcrea = dttv.Select("type='CREA'");
                            double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                            double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                            if (creadouble > creawmax || creadouble < creawmin)
                            {
                                flag = 2;
                            }
                            double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                            double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                            if (creadouble > creatmax || creadouble < creatmin)
                            {
                                flag = 3;
                            }
                        }

                        string urea = sh.UREA;
                        if (urea != null && urea != "*")
                        {
                            double ureadouble = double.Parse(urea);
                            DataRow[] drurea = dttv.Select("type='UREA'");
                            double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                            double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                            if (ureadouble > ureawmax || ureadouble < ureawmin)
                            {
                                flag = 2;
                            }
                            double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                            double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                            if (ureadouble > ureatmax || ureadouble < ureatmin)
                            {
                                flag = 3;
                            }
                        }

                        string glu = sh.GLU;
                        if (glu != null && glu != "*")
                        {
                            double gludouble = double.Parse(glu);
                            DataRow[] drglu = dttv.Select("type='GLU'");
                            double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                            double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                            if (gludouble > gluwmax || gludouble < gluwmin)
                            {
                                flag = 2;
                            }
                            double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                            double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                            if (gludouble > glutmax || gludouble < glutmin)
                            {
                                flag = 3;
                            }
                        }

                        string tg = sh.TG;
                        if (tg != null && tg != "*")
                        {
                            double tgdouble = double.Parse(tg);
                            DataRow[] drtg = dttv.Select("type='TG'");
                            double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                            double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                            if (tgdouble > tgwmax || tgdouble < tgwmin)
                            {
                                flag = 2;
                            }
                            double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                            double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                            if (tgdouble > tgtmax || tgdouble < tgtmin)
                            {
                                flag = 3;
                            }
                        }

                        string cho = sh.CHO;
                        if (cho != null && cho != "*")
                        {
                            double chodouble = double.Parse(cho);
                            DataRow[] drcho = dttv.Select("type='CHO'");
                            double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                            double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                            if (chodouble > chowmax || chodouble < chowmin)
                            {
                                flag = 2;
                            }
                            double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                            double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                            if (chodouble > chotmax || chodouble < chotmin)
                            {
                                flag = 3;
                            }
                        }

                        string hdlc = sh.HDL_C;
                        if (hdlc != null && hdlc != "*")
                        {
                            double hdlcdouble = double.Parse(hdlc);
                            DataRow[] drhdlc = dttv.Select("type='HDLC'");
                            double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                            double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                            if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                            {
                                flag = 2;
                            }
                            double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                            double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                            if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                            {
                                flag = 3;
                            }
                        }

                        string ldlc = sh.LDL_C;
                        if (ldlc != null && ldlc != "*")
                        {
                            double ldlcdouble = double.Parse(ldlc);
                            DataRow[] drldlc = dttv.Select("type='LDLC'");
                            double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                            double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                            if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                            {
                                flag = 2;
                            }
                            double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                            double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                            if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                            {
                                flag = 3;
                            }
                        }

                        tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                        #endregion
                    }
                    else
                    {
                        bool istree = tjdao.updateShenghuaInfo(sh);
                        if (!istree) return;
                        #region 生化
                        int flag = 1;
                        string alt = sh.ALT;
                        if (alt != null && alt != "*")
                        {
                            double altdouble = double.Parse(alt);
                            DataRow[] dralt = dttv.Select("type='ALT'");
                            double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                            double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                            if (altdouble > altwmax || altdouble < altwmin)
                            {
                                flag = 2;
                            }
                            double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                            double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                            if (altdouble > alttmax || altdouble < alttmin)
                            {
                                flag = 3;
                            }
                        }

                        string ast = sh.AST;
                        if (ast != null && ast != "*")
                        {
                            double astdouble = double.Parse(ast);
                            DataRow[] drast = dttv.Select("type='AST'");
                            double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                            double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                            if (astdouble > astwmax || astdouble < astwmin)
                            {
                                flag = 2;
                            }
                            double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                            double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                            if (astdouble > asttmax || astdouble < asttmin)
                            {
                                flag = 3;
                            }
                        }

                        string tbil = sh.TBIL;
                        if (tbil != null && tbil != "*")
                        {
                            double tbildouble = double.Parse(tbil);
                            DataRow[] drtbil = dttv.Select("type='TBIL'");
                            double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                            double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                            if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                            {
                                flag = 2;
                            }
                            double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                            double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                            if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                            {
                                flag = 3;
                            }
                        }
                        string dbil = sh.DBIL;
                        if (dbil != null && dbil != "*")
                        {
                            double dbildouble = double.Parse(dbil);
                            DataRow[] drtbil = dttv.Select("type='DBIL'");
                            double dbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                            double dbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                            if (dbildouble > dbilwmax || dbildouble < dbilwmin)
                            {
                                flag = 2;
                            }
                            double dbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                            double dbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                            if (dbildouble > dbiltmax || dbildouble < dbiltmin)
                            {
                                flag = 3;
                            }
                        }
                        string crea = sh.Crea;
                        if (crea != null && crea != "*")
                        {
                            double creadouble = double.Parse(crea);
                            DataRow[] drcrea = dttv.Select("type='CREA'");
                            double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                            double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                            if (creadouble > creawmax || creadouble < creawmin)
                            {
                                flag = 2;
                            }
                            double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                            double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                            if (creadouble > creatmax || creadouble < creatmin)
                            {
                                flag = 3;
                            }
                        }

                        string urea = sh.UREA;
                        if (urea != null && urea != "*")
                        {
                            double ureadouble = double.Parse(urea);
                            DataRow[] drurea = dttv.Select("type='UREA'");
                            double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                            double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                            if (ureadouble > ureawmax || ureadouble < ureawmin)
                            {
                                flag = 2;
                            }
                            double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                            double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                            if (ureadouble > ureatmax || ureadouble < ureatmin)
                            {
                                flag = 3;
                            }
                        }

                        string glu = sh.GLU;
                        if (glu != null && glu != "*")
                        {
                            double gludouble = double.Parse(glu);
                            DataRow[] drglu = dttv.Select("type='GLU'");
                            double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                            double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                            if (gludouble > gluwmax || gludouble < gluwmin)
                            {
                                flag = 2;
                            }
                            double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                            double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                            if (gludouble > glutmax || gludouble < glutmin)
                            {
                                flag = 3;
                            }
                        }

                        string tg = sh.TG;
                        if (tg != null && tg != "*")
                        {
                            double tgdouble = double.Parse(tg);
                            DataRow[] drtg = dttv.Select("type='TG'");
                            double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                            double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                            if (tgdouble > tgwmax || tgdouble < tgwmin)
                            {
                                flag = 2;
                            }
                            double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                            double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                            if (tgdouble > tgtmax || tgdouble < tgtmin)
                            {
                                flag = 3;
                            }
                        }

                        string cho = sh.CHO;
                        if (cho != null && cho != "*")
                        {
                            double chodouble = double.Parse(cho);
                            DataRow[] drcho = dttv.Select("type='CHO'");
                            double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                            double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                            if (chodouble > chowmax || chodouble < chowmin)
                            {
                                flag = 2;
                            }
                            double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                            double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                            if (chodouble > chotmax || chodouble < chotmin)
                            {
                                flag = 3;
                            }
                        }

                        string hdlc = sh.HDL_C;
                        if (hdlc != null && hdlc != "*")
                        {
                            double hdlcdouble = double.Parse(hdlc);
                            DataRow[] drhdlc = dttv.Select("type='HDLC'");
                            double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                            double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                            if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                            {
                                flag = 2;
                            }
                            double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                            double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                            if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                            {
                                flag = 3;
                            }
                        }

                        string ldlc = sh.LDL_C;
                        if (ldlc != null && ldlc != "*")
                        {
                            double ldlcdouble = double.Parse(ldlc);
                            DataRow[] drldlc = dttv.Select("type='LDLC'");
                            double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                            double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                            if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                            {
                                flag = 2;
                            }
                            double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                            double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                            if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                            {
                                flag = 3;
                            }
                        }
                        tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                        #endregion
                    }
                    //返回生化的确认数据报文
                    for (int j = 0; j < sendArraysh.Length; j++)
                    {
                        sendHL7sh += "|" + sendArraysh[j];
                    }
                    send.Send(AckKbe(sendHL7sh.Substring(1)));
                }
                else if (sHL7.IndexOf("UNICODE") > 1)
                {//解析血球协议报文数据
                    try
                    {
                        if (sHL7.Substring(0, 3) != "MSH" || sHL7.Substring(sHL7.Length - 1, 1) != "|")
                        {
                            continue;
                        }
                        xuechangguiBean xcg = new xuechangguiBean();
                        xcg.ZrysXCG = basicInfoSettings.xcg;
                        string[] sHL7Pids = Regex.Split(sHL7, "PID", RegexOptions.IgnoreCase);
                        if (sHL7Pids.Length == 0) { return; };
                        string[] MSHArray = sHL7Pids[0].Split('|');
                        sendArray[6] = MSHArray[6];
                        sendArray[9] = MSHArray[9];
                        sendArray[19] = MSHArray[9];
                        string[] sHL7PArray = sHL7Pids[1].Split('|');
                        xcg.bar_code = sHL7PArray[14];
                        DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(xcg.bar_code);
                        if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                        {
                            xcg.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                            xcg.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                        }
                        else
                        {
                            return;
                        }
                        xcg.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //把HL7分成段
                        string[] sHL7Lines = Regex.Split(sHL7, "OBX", RegexOptions.IgnoreCase);
                        if (sHL7Lines.Length == 0) { return; };
                        for (int i = 1; i < sHL7Lines.Length; i++)
                        {
                            string[] sHL7Array = sHL7Lines[i].Split('|');
                            if (sHL7Array[2].IndexOf("NM") == -1)
                            {
                                continue;
                            }
                            if (sHL7Array[3].IndexOf("WBC") > -1)
                            {
                                xcg.WBC = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("LYM_c") > -1)
                            {
                                xcg.LYM = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("LYM_p") > -1)
                            {
                                xcg.LYMP = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("RBC") > -1)
                            {
                                xcg.RBC = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("HGB") > -1)
                            {
                                xcg.HGB = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("PCT") > -1)
                            {
                                xcg.PCT = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("PLT") > -1)
                            {
                                xcg.PLT = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("HCT") > -1)
                            {
                                xcg.HCT = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MCV") > -1)
                            {
                                xcg.MCV = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MCH") > -1)
                            {
                                xcg.MCH = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MCHC") > -1)
                            {
                                xcg.MCHC = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("RDW_CV") > -1)
                            {
                                xcg.RDW_CV = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("RDW_SD") > -1)
                            {
                                xcg.RDW_SD = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("NEU_c") > -1)
                            {
                                xcg.NEUT = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("NEU_p") > -1)
                            {
                                xcg.NEUTP = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MPV") > -1)
                            {
                                xcg.MPV = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("PDW") > -1)
                            {
                                xcg.PDW = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MXD_c") > -1)
                            {
                                xcg.MXD = sHL7Array[5]; continue;
                            }
                            else if (sHL7Array[3].IndexOf("MXD_p") > -1)
                            {
                                xcg.MXDP = sHL7Array[5]; continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        bool istrue = tjdao.insertXuechangguiInfo(xcg);
                        if (istrue)
                        {
                            int flag = 1;
                            string wbc = xcg.WBC;
                            if (wbc != null && wbc != "*")
                            {
                                double wbcdouble = double.Parse(wbc);
                                DataRow[] drwbc = dttv.Select("type='WBC'");
                                double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                                double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                                if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                                {
                                    flag = 2;
                                }
                                double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                                double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                                if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rbc = xcg.RBC;
                            if (rbc != null && rbc != "*")
                            {
                                double rbcdouble = double.Parse(rbc);
                                DataRow[] drrbc = dttv.Select("type='RBC'");
                                double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                                double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                                if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                                {
                                    flag = 2;
                                }
                                double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                                double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                                if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string pct = xcg.PCT;
                            if (pct != null && pct != "*")
                            {
                                double pctdouble = double.Parse(pct);
                                DataRow[] drpct = dttv.Select("type='PCT'");
                                double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                                double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                                if (pctdouble > pctwmax || pctdouble < pctwmin)
                                {
                                    flag = 2;
                                }
                                double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                                double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                                if (pctdouble > pcttmax || pctdouble < pcttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string plt = xcg.PLT;
                            if (plt != null && plt != "*")
                            {
                                double pltdouble = double.Parse(plt);
                                DataRow[] drplt = dttv.Select("type='PLT'");
                                double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                                double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                                if (pltdouble > pltwmax || pltdouble < pltwmin)
                                {
                                    flag = 2;
                                }
                                double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                                double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                                if (pltdouble > plttmax || pltdouble < plttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string hgb = xcg.HGB;
                            if (hgb != null && hgb != "*")
                            {
                                double hgbdouble = double.Parse(hgb);
                                DataRow[] drhgb = dttv.Select("type='HGB'");
                                double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                                double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                                if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                                {
                                    flag = 2;
                                }
                                double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                                double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                                if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string hct = xcg.HCT;
                            if (hct != null && hct != "*")
                            {
                                double hctdouble = double.Parse(hct);
                                DataRow[] drhct = dttv.Select("type='HCT'");
                                double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                                double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                                if (hctdouble > hctwmax || hctdouble < hctwmin)
                                {
                                    flag = 2;
                                }
                                double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                                double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                                if (hctdouble > hcttmax || hctdouble < hcttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mcv = xcg.MCV;
                            if (mcv != null && mcv != "*")
                            {
                                double mcvdouble = double.Parse(mcv);
                                DataRow[] drmcv = dttv.Select("type='MCV'");
                                double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                                double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                                if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                                {
                                    flag = 2;
                                }
                                double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                                double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                                if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mch = xcg.MCH;
                            if (mch != null && mch != "*")
                            {
                                double mchdouble = double.Parse(mch);
                                DataRow[] drmch = dttv.Select("type='MCH'");
                                double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                                double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                                if (mchdouble > mchwmax || mchdouble < mchwmin)
                                {
                                    flag = 2;
                                }
                                double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                                double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                                if (mchdouble > mchtmax || mchdouble < mchtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mchc = xcg.MCHC;
                            if (mchc != null && mchc != "*")
                            {
                                double mchcdouble = double.Parse(mchc);
                                DataRow[] drmchc = dttv.Select("type='MCHC'");
                                double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                                double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                                if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                                {
                                    flag = 2;
                                }
                                double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                                double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                                if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rdwcv = xcg.RDW_CV;
                            if (rdwcv != null && rdwcv != "*")
                            {
                                double rdwcvdouble = double.Parse(rdwcv);
                                DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                                double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                                double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                                if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                                {
                                    flag = 2;
                                }
                                double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                                double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                                if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rdwsd = xcg.RDW_SD;
                            if (rdwsd != null && rdwsd != "*")
                            {
                                double rdwsddouble = double.Parse(rdwsd);
                                DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                                double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                                double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                                if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                                {
                                    flag = 2;
                                }
                                double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                                double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                                if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string neut = xcg.NEUT;
                            if (neut != null && neut != "*")
                            {
                                double neutdouble = double.Parse(neut);
                                DataRow[] drneut = dttv.Select("type='NEUT'");
                                double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                                double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                                if (neutdouble > neutwmax || neutdouble < neutwmin)
                                {
                                    flag = 2;
                                }
                                double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                                double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                                if (neutdouble > neuttmax || neutdouble < neuttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string neutp = xcg.NEUTP;
                            if (neutp != null && neutp != "*")
                            {
                                double neutpdouble = double.Parse(neutp);
                                DataRow[] drneutp = dttv.Select("type='NEUTP'");
                                double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                                double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                                if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                                {
                                    flag = 2;
                                }
                                double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                                double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                                if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                                {
                                    flag = 3;
                                }
                            }
                            string lym = xcg.LYM;
                            if (lym != null && lym != "*")
                            {
                                double lymdouble = double.Parse(lym);
                                DataRow[] drlym = dttv.Select("type='LYM'");
                                double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                                double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                                if (lymdouble > lymwmax || lymdouble < lymwmin)
                                {
                                    flag = 2;
                                }
                                double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                                double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                                if (lymdouble > lymtmax || lymdouble < lymtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string lymp = xcg.LYMP;
                            if (lymp != null && lymp != "*")
                            {
                                double lympdouble = double.Parse(lymp);
                                DataRow[] drlymp = dttv.Select("type='LYMP'");
                                double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                                double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                                if (lympdouble > lympwmax || lympdouble < lympwmin)
                                {
                                    flag = 2;
                                }
                                double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                                double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                                if (lympdouble > lymptmax || lympdouble < lymptmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mpv = xcg.MPV;
                            if (mpv != null && mpv != "*")
                            {
                                double mpvdouble = double.Parse(mpv);
                                DataRow[] drmpv = dttv.Select("type='MPV'");
                                double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                                double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                                if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                                {
                                    flag = 2;
                                }
                                double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                                double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                                if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string pdw = xcg.PDW;
                            if (pdw != null && pdw != "*")
                            {
                                double pdwdouble = double.Parse(pdw);
                                DataRow[] drpdw = dttv.Select("type='PDW'");
                                double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                                double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                                if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                                {
                                    flag = 2;
                                }
                                double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                                double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                                if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mxd = xcg.MXD;
                            if (mxd != null && mxd != "*")
                            {
                                double mxddouble = double.Parse(mxd);
                                DataRow[] drmxd = dttv.Select("type='MXD'");
                                double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                                double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                                if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                                {
                                    flag = 2;
                                }
                                double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                                double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                                if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mxdp = xcg.MXDP;
                            if (mxdp != null && mxdp != "*")
                            {
                                double mxdpdouble = double.Parse(mxdp);
                                DataRow[] drmxdp = dttv.Select("type='MXDP'");
                                double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                                double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                                if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                                {
                                    flag = 2;
                                }
                                double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                                double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                                if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                                {
                                    flag = 3;
                                }
                            }
                            tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                            tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                        }
                        else
                        {
                            bool istrue1 = tjdao.updateXuechangguiInfo(xcg);
                            if (!istrue1)
                            {
                                return;
                            }
                            int flag = 1;
                            string wbc = xcg.WBC;
                            if (wbc != null && wbc != "*")
                            {
                                double wbcdouble = double.Parse(wbc);
                                DataRow[] drwbc = dttv.Select("type='WBC'");
                                double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                                double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                                if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                                {
                                    flag = 2;
                                }
                                double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                                double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                                if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rbc = xcg.RBC;
                            if (rbc != null && rbc != "*")
                            {
                                double rbcdouble = double.Parse(rbc);
                                DataRow[] drrbc = dttv.Select("type='RBC'");
                                double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                                double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                                if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                                {
                                    flag = 2;
                                }
                                double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                                double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                                if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string pct = xcg.PCT;
                            if (pct != null && pct != "*")
                            {
                                double pctdouble = double.Parse(pct);
                                DataRow[] drpct = dttv.Select("type='PCT'");
                                double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                                double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                                if (pctdouble > pctwmax || pctdouble < pctwmin)
                                {
                                    flag = 2;
                                }
                                double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                                double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                                if (pctdouble > pcttmax || pctdouble < pcttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string plt = xcg.PLT;
                            if (plt != null && plt != "*")
                            {
                                double pltdouble = double.Parse(plt);
                                DataRow[] drplt = dttv.Select("type='PLT'");
                                double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                                double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                                if (pltdouble > pltwmax || pltdouble < pltwmin)
                                {
                                    flag = 2;
                                }
                                double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                                double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                                if (pltdouble > plttmax || pltdouble < plttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string hgb = xcg.HGB;
                            if (hgb != null && hgb != "*")
                            {
                                double hgbdouble = double.Parse(hgb);
                                DataRow[] drhgb = dttv.Select("type='HGB'");
                                double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                                double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                                if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                                {
                                    flag = 2;
                                }
                                double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                                double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                                if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string hct = xcg.HCT;
                            if (hct != null && hct != "*")
                            {
                                double hctdouble = double.Parse(hct);
                                DataRow[] drhct = dttv.Select("type='HCT'");
                                double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                                double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                                if (hctdouble > hctwmax || hctdouble < hctwmin)
                                {
                                    flag = 2;
                                }
                                double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                                double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                                if (hctdouble > hcttmax || hctdouble < hcttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mcv = xcg.MCV;
                            if (mcv != null && mcv != "*")
                            {
                                double mcvdouble = double.Parse(mcv);
                                DataRow[] drmcv = dttv.Select("type='MCV'");
                                double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                                double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                                if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                                {
                                    flag = 2;
                                }
                                double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                                double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                                if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mch = xcg.MCH;
                            if (mch != null && mch != "*")
                            {
                                double mchdouble = double.Parse(mch);
                                DataRow[] drmch = dttv.Select("type='MCH'");
                                double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                                double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                                if (mchdouble > mchwmax || mchdouble < mchwmin)
                                {
                                    flag = 2;
                                }
                                double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                                double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                                if (mchdouble > mchtmax || mchdouble < mchtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mchc = xcg.MCHC;
                            if (mchc != null && mchc != "*")
                            {
                                double mchcdouble = double.Parse(mchc);
                                DataRow[] drmchc = dttv.Select("type='MCHC'");
                                double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                                double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                                if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                                {
                                    flag = 2;
                                }
                                double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                                double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                                if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rdwcv = xcg.RDW_CV;
                            if (rdwcv != null && rdwcv != "*")
                            {
                                double rdwcvdouble = double.Parse(rdwcv);
                                DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                                double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                                double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                                if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                                {
                                    flag = 2;
                                }
                                double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                                double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                                if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string rdwsd = xcg.RDW_SD;
                            if (rdwsd != null && rdwsd != "*")
                            {
                                double rdwsddouble = double.Parse(rdwsd);
                                DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                                double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                                double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                                if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                                {
                                    flag = 2;
                                }
                                double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                                double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                                if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string neut = xcg.NEUT;
                            if (neut != null && neut != "*")
                            {
                                double neutdouble = double.Parse(neut);
                                DataRow[] drneut = dttv.Select("type='NEUT'");
                                double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                                double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                                if (neutdouble > neutwmax || neutdouble < neutwmin)
                                {
                                    flag = 2;
                                }
                                double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                                double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                                if (neutdouble > neuttmax || neutdouble < neuttmin)
                                {
                                    flag = 3;
                                }
                            }
                            string neutp = xcg.NEUTP;
                            if (neutp != null && neutp != "*")
                            {
                                double neutpdouble = double.Parse(neutp);
                                DataRow[] drneutp = dttv.Select("type='NEUTP'");
                                double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                                double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                                if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                                {
                                    flag = 2;
                                }
                                double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                                double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                                if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                                {
                                    flag = 3;
                                }
                            }
                            string lym = xcg.LYM;
                            if (lym != null && lym != "*")
                            {
                                double lymdouble = double.Parse(lym);
                                DataRow[] drlym = dttv.Select("type='LYM'");
                                double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                                double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                                if (lymdouble > lymwmax || lymdouble < lymwmin)
                                {
                                    flag = 2;
                                }
                                double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                                double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                                if (lymdouble > lymtmax || lymdouble < lymtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string lymp = xcg.LYMP;
                            if (lymp != null && lymp != "*")
                            {
                                double lympdouble = double.Parse(lymp);
                                DataRow[] drlymp = dttv.Select("type='LYMP'");
                                double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                                double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                                if (lympdouble > lympwmax || lympdouble < lympwmin)
                                {
                                    flag = 2;
                                }
                                double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                                double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                                if (lympdouble > lymptmax || lympdouble < lymptmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mpv = xcg.MPV;
                            if (mpv != null && mpv != "*")
                            {
                                double mpvdouble = double.Parse(mpv);
                                DataRow[] drmpv = dttv.Select("type='MPV'");
                                double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                                double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                                if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                                {
                                    flag = 2;
                                }
                                double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                                double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                                if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string pdw = xcg.PDW;
                            if (pdw != null && pdw != "*")
                            {
                                double pdwdouble = double.Parse(pdw);
                                DataRow[] drpdw = dttv.Select("type='PDW'");
                                double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                                double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                                if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                                {
                                    flag = 2;
                                }
                                double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                                double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                                if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mxd = xcg.MXD;
                            if (mxd != null && mxd != "*")
                            {
                                double mxddouble = double.Parse(mxd);
                                DataRow[] drmxd = dttv.Select("type='MXD'");
                                double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                                double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                                if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                                {
                                    flag = 2;
                                }
                                double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                                double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                                if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                                {
                                    flag = 3;
                                }
                            }
                            string mxdp = xcg.MXDP;
                            if (mxdp != null && mxdp != "*")
                            {
                                double mxdpdouble = double.Parse(mxdp);
                                DataRow[] drmxdp = dttv.Select("type='MXDP'");
                                double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                                double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                                if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                                {
                                    flag = 2;
                                }
                                double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                                double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                                if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                                {
                                    flag = 3;
                                }
                            }
                            tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, flag);
                            tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                        }
                        //返回血球的确认数据报文
                        for (int j = 0; j < sendArray.Length; j++)
                        {
                            sendHL7new += "|" + sendArray[j];
                        }
                        send.Send(AckKbe(sendHL7new.Substring(1)));
                    }
                    catch (Exception ex)
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                }
                totalByteRead = new Byte[0];
            }
        }
    }
}
