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
        private OleDbDataAdapter oda = null;
        private DataSet myds_data = null;
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node;
        string path = @"config.xml";
        string shenghuapath= "";
        string xuechangguipath = "";
        public frmMain()
        {
            InitializeComponent();

        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            basicInfoSettings basicSet = new basicInfoSettings();
            basicSet.Show();

            this.timer1.Start();//时间控件定时器
            //this.timer2.Interval = Int32.Parse(Properties.Settings.Default.timeInterval);
            //this.timer2.Start();//定时获取生化和血球的数据

            this.timer3.Interval =Int32.Parse(Properties.Settings.Default.timer3Interval);
            this.timer3.Start();//1分钟定时刷新设备状态

            this.label1.Text = "一体化查体车  中科弘卫";
            this.label1.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));

            //获取首页右上角信息
            DataTable dts= bsdao.checkBasicsettingInfo();
            if (dts.Rows.Count>0) {
                label3.Text= dts.Rows[0]["organ_name"].ToString();
                label7.Text = dts.Rows[0]["input_name"].ToString();
                label9.Text = dts.Rows[0]["zeren_doctor"].ToString();
            }
            //默认主页面显示
            foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            {
                if (item.Text == "公共卫生")
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
                            pR = new personRegist();
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
            //socketTcp();
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
            Thread.Sleep(230);
            IntPtrFindWindow.showwindow(proAsNet.MainWindowHandle);
            //ftp                 
                //proFtp.StartInfo.FileName = @"C:\\Program Files\\iMAC FTP-JN120.05\\ftpservice.exe";
                //proFtp.StartInfo.CreateNoWindow = true;
                //proFtp.StartInfo.UseShellExecute = false;
            //proFtp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //proFtp.StartInfo.ErrorDialog = false;
                //proFtp.Start();
            //Thread.Sleep(1000);
            //IntPtrFindWindow.intptrwindows(proFtp.MainWindowHandle);
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
                //if (!proFtp.HasExited)
                //{
                //    proFtp.Kill();
                //}
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
                Environment.Exit(0);
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
                    PictureBox[] picb = new PictureBox[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        picb[i] = new PictureBox();
                        picb[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        picb[i].BorderStyle = BorderStyle.None;
                        if (i == 0)//默认首项选中
                        {
                            if (pR!=null) {
                                pR.btnClose_Click();
                                pR = null;
                            }
                            picb[i].BackColor = Color.Blue;
                            pR = new personRegist();
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
                            onSiteSigning pR = new onSiteSigning();
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
            if (!"人员登记".Equals(tag) && pR != null)
            {
                pR.btnClose_Click();
                pR = null;
            }
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
                personRegist pR = new personRegist();
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
            else if (tag == "老年人健康服务")
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
            else if (tag == "中医健康服务")
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
            else if (tag == "预防接种服务")
            {
                vaccinationServices pR = new vaccinationServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "健康教育服务")
            {
                healthEducationServices pR = new healthEducationServices();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "现场签约")
            {         //家医签约模块       
                onSiteSigning pR = new onSiteSigning();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "团队成员")
            {
                teamMembers pR = new teamMembers();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "签约统计")
            {
                signingStatistics pR = new signingStatistics();
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
            else if (tag == "体检设备说明书")
            {   //使用帮助模块 
                OpenPdf(Application.StartupPath+ "\\pdf\\仪器配置说明.docx");
            }
            else if (tag == "软件系统说明书")
            {   //使用帮助模块 
                softwareSystems pR = new softwareSystems();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "B超")
            {
                bUltrasound pR = new bUltrasound();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "生化")
            {
                biochemical pR = new biochemical();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "尿液")
            {
                urinaryFluid pR = new urinaryFluid();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "血常规")
            {
                bloodAnalysis pR = new bloodAnalysis();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "身高体重")
            {
                heightAndWeight pR = new heightAndWeight();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "心电图")
            {
                electrocarDiogram pR = new electrocarDiogram();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "血压")
            {
                bloodPressure pR = new bloodPressure();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();

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
                //if (!proFtp.HasExited)
                //{
                //    proFtp.Kill();
                //}
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
                //Environment.Exit(0);
            }
        }
        //定时任务获取生化和血球的数据
        private void timer2_Tick(object sender, EventArgs e)
        {
            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/shenghuaPath");
            shenghuapath = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/xuechangguiPath");
            xuechangguipath = node.InnerText;

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
                string sql = "select lop.patient_id,lop.send_time,lopr.* from LisOutput lop, LisOutputResult lopr where lop.sample_id=lopr.sample_id and lop.sample_id=(select top 1 l.sample_id from LisOutput l order by l.sample_id desc)";
                DataTable arr_dt = getShenghua(sql).Tables[0];
                if (arr_dt.Rows.Count > 0)
                {
                    shenghuaBean sh = new shenghuaBean();
                    sh.bar_code = arr_dt.Rows[0]["patient_id"].ToString();
                    DataTable dtjkinfo= jkdao.selectjkInfoBybarcode(sh.bar_code);
                    if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                    {
                        sh.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                        sh.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                    }
                    else {
                        return;
                    }
                    sh.createTime = Convert.ToDateTime(arr_dt.Rows[0]["send_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    for (int i = 0; i < arr_dt.Rows.Count; i++)
                    {
                        string item = arr_dt.Rows[i]["item"].ToString();
                        switch (item)
                        {
                            case "ALB": sh.ALB = arr_dt.Rows[i]["result"].ToString(); break;
                            case "ALP": sh.ALP = arr_dt.Rows[i]["result"].ToString(); break;
                            case "ALT": sh.ALT = arr_dt.Rows[i]["result"].ToString(); break;
                            case "AST": sh.AST = arr_dt.Rows[i]["result"].ToString(); break;
                            case "CHO": sh.CHO = arr_dt.Rows[i]["result"].ToString(); break;
                            case "Crea": sh.Crea = arr_dt.Rows[i]["result"].ToString(); break;
                            case "DBIL": sh.DBIL = arr_dt.Rows[i]["result"].ToString(); break;
                            case "GGT": sh.GGT = arr_dt.Rows[i]["result"].ToString(); break;
                            case "GLU": sh.GLU = arr_dt.Rows[i]["result"].ToString(); break;
                            case "HDL_C": sh.HDL_C = arr_dt.Rows[i]["result"].ToString(); break;
                            case "LDL_C": sh.LDL_C = arr_dt.Rows[i]["result"].ToString(); break;
                            case "TBIL": sh.TBIL = arr_dt.Rows[i]["result"].ToString(); break;
                            case "TG": sh.TG = arr_dt.Rows[i]["result"].ToString(); break;
                            case "TP": sh.TP = arr_dt.Rows[i]["result"].ToString(); break;
                            case "UA": sh.UA = arr_dt.Rows[i]["result"].ToString(); break;
                            case "UREA": sh.UREA = arr_dt.Rows[i]["result"].ToString(); break;
                            default: break;
                        }
                    }
                    bool istrue= tjdao.insertShenghuaInfo(sh);
                    if (istrue) {
                        tjdao.updateTJbgdcShenghua(sh.aichive_no,sh.bar_code,1);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C,sh.HDL_C);
                    }
                }
            }
            if (xuechangguipath == "" || !File.Exists(shenghuapath))
            {
                MessageBox.Show("未获取到血球中间库地址，请检查是否设置地址！");
                return;
            }
            else
            {
                bool bl = xuechangguipath.IndexOf("Lis_DB.mdb") > -1 ? true : false;
                if (bl == false) { MessageBox.Show("血球中间库地址不正确，请检查是否设置地址！"); return; }
                string sql1 = "select lop.patient_id,lop.send_time,lopr.* from LisOutput lop, LisOutputResult lopr where lop.sample_id=lopr.sample_id and lop.sample_id=(select top 1 l.sample_id from LisOutput l order by l.sample_id desc)";
                DataTable arr_dt1 = getXuechanggui(sql1).Tables[0];
                if (arr_dt1.Rows.Count > 0)
                {
                    xuechangguiBean xcg = new xuechangguiBean();
                    xcg.bar_code = arr_dt1.Rows[0]["patient_id"].ToString();
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
                    xcg.createTime = Convert.ToDateTime(arr_dt1.Rows[0]["send_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    for (int i = 0; i < arr_dt1.Rows.Count; i++)
                    {
                        string item = arr_dt1.Rows[i]["item"].ToString();
                        switch (item)
                        {
                            case "HCT": xcg.HCT = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "HGB": xcg.HGB = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "LYM#": xcg.LYM = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "LYM%": xcg.LYMP = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "MCH": xcg.MCH = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "MCHC": xcg.MCHC = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "MCV": xcg.MCV = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "MPV": xcg.MPV = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "MXD#": xcg.MXD = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "MXD%": xcg.MXDP = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "NEUT#": xcg.NEUT = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "NEUT%": xcg.NEUTP = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "PCT": xcg.PCT = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "PDW": xcg.PDW = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "PLT": xcg.PLT = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "RBC": xcg.RBC = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "RDW_CV": xcg.RDW_CV = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "RDW_SD": xcg.RDW_SD = arr_dt1.Rows[i]["result"].ToString(); break;
                            case "WBC": xcg.WBC = arr_dt1.Rows[i]["result"].ToString(); break;
                            default: break;
                        }
                    }
                    bool istrue = tjdao.insertXuechangguiInfo(xcg);
                    if (istrue)
                    {
                        tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, 1);
                        tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB,xcg.WBC,xcg.PLT);
                    }
                }
            }
        }
        /// <summary>
        /// 生化表
        /// </summary>
        public DataSet getShenghua(string strSQL)
        {
            string strcon = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + shenghuapath + "";
            myds_data = new DataSet();
            oda = new OleDbDataAdapter(strSQL, strcon);
            oda.Fill(myds_data);
            return myds_data;
        }
        /// <summary>
        /// 血球表
        /// </summary>
        public DataSet getXuechanggui(string strSQL)
        {
            string strcon = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + xuechangguipath + "";
            myds_data = new DataSet();
            oda = new OleDbDataAdapter(strSQL, strcon);
            oda.Fill(myds_data);
            return myds_data;
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
          DataTable dtDeviceType = tjdao.checkDevice();
          string sfz_online = dtDeviceType.Rows[0]["sfz_online"].ToString();
            if (sfz_online == "0" || "0".Equals(sfz_online))
            {
                this.button1.BackColor = Color.Red;
            }
            else {
                this.button1.BackColor = Color.MediumAquamarine;
            }
            string sxt_online = dtDeviceType.Rows[0]["sxt_online"].ToString();
            if (sxt_online == "0" || "0".Equals(sxt_online))
            {
                this.button2.BackColor = Color.Red;
            }
            else
            {
                this.button2.BackColor = Color.MediumAquamarine;
            }
            string dyj_online = dtDeviceType.Rows[0]["dyj_online"].ToString();
            if (dyj_online == "0" || "0".Equals(dyj_online))
            {
                this.button3.BackColor = Color.Red;
            }
            else
            {
                this.button3.BackColor = Color.MediumAquamarine;
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
                this.button5.BackColor = Color.Red;
            }
            else
            {
                this.button5.BackColor = Color.MediumAquamarine;
            }
            string ncg_online = dtDeviceType.Rows[0]["ncg_online"].ToString();
            if (ncg_online == "0" || "0".Equals(ncg_online))
            {
                this.button6.BackColor = Color.Red;
            }
            else
            {
                this.button6.BackColor = Color.MediumAquamarine;
            }
            string xdt_online = dtDeviceType.Rows[0]["xdt_online"].ToString();
            if (xdt_online == "0" || "0".Equals(xdt_online))
            {
                this.button7.BackColor = Color.Red;
            }
            else
            {
                this.button7.BackColor = Color.MediumAquamarine;
            }
            string sgtz_online = dtDeviceType.Rows[0]["sgtz_online"].ToString();
            if (sgtz_online == "0" || "0".Equals(sgtz_online))
            {
                this.button8.BackColor = Color.Red;
            }
            else
            {
                this.button8.BackColor = Color.MediumAquamarine;
            }
            string xy_online = dtDeviceType.Rows[0]["xy_online"].ToString();
            if (xy_online == "0" || "0".Equals(xy_online))
            {
                this.button9.BackColor = Color.Red;
            }
            else
            {
                this.button9.BackColor = Color.MediumAquamarine;
            }
            string bc_online = dtDeviceType.Rows[0]["bc_online"].ToString();
            if (bc_online == "0" || "0".Equals(bc_online))
            {
                this.button10.BackColor = Color.Red;
            }
            else
            {
                this.button10.BackColor = Color.MediumAquamarine;
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
            //尿机IP地址解析
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);//方法已过期，可以获取IPv4的地址
            IPAddress ip = localhost.AddressList[0];
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //IPAddress ip = IPAddress.Parse("192.168.2.103");
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
                    string sendHL7 = "MSH|^~\\&|||Rayto||1||ACK^R01|1|P|2.3.1||||S||UNICODE|||MSA|AA|1|||||";
                    string []sendArray= sendHL7.Split('|');
                    byte[] buffernew = buffer.Skip(0).Take(effective).ToArray();
                    string sHL7 = Encoding.Default.GetString(buffernew).Trim();
                    if (sHL7.IndexOf("CHEMRAY420") > 0)
                    {//解析生化协议报文数据
                        shenghuaBean sh = new shenghuaBean();
                        string[] sHL7Pids = Regex.Split(sHL7, "PID", RegexOptions.IgnoreCase);
                        if (sHL7Pids.Length == 0) { return; };
                        string[] MSHArray = sHL7Pids[0].Split('|');
                        sendArray[6] = MSHArray[6];
                        sendArray[9] = MSHArray[9];
                        sendArray[17] = "ASCII";
                        sendArray[22] = MSHArray[9];
                        string[] sHL7PArray = sHL7Pids[1].Split('|');
                        sh.bar_code = sHL7PArray[2];
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
                                case "Crea": sh.Crea = sHL7Array[5]; break;
                                case "DBIL": sh.DBIL = sHL7Array[5]; break;
                                case "GGT": sh.GGT = sHL7Array[5]; break;
                                case "GLU": sh.GLU = sHL7Array[5]; break;
                                case "HDL_C": sh.HDL_C = sHL7Array[5]; break;
                                case "LDL_C": sh.LDL_C = sHL7Array[5]; break;
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
                            tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, 1);
                            tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C);
                        }
                        //返回生化的确认数据报文
                        for (int j=0;j< sendArray.Length;j++) {
                            sendHL7new += "|"+sendArray[j];
                        }
                        byte[] sendBytes = Encoding.Unicode.GetBytes(sendHL7new.Substring(1));
                        send.Send(sendBytes);
                }else
                    {//解析血球协议报文数据
                    xuechangguiBean xcg = new xuechangguiBean();
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
                            case "LYMA": xcg.LYM = sHL7Array[5]; break;
                            case "LYMP": xcg.LYMP = sHL7Array[5]; break;
                            case "MCH": xcg.MCH = sHL7Array[5]; break;
                            case "MCHC": xcg.MCHC = sHL7Array[5]; break;
                            case "MCV": xcg.MCV = sHL7Array[5]; break;
                            case "MPV": xcg.MPV = sHL7Array[5]; break;
                            case "MXDA": xcg.MXD = sHL7Array[5]; break;
                            case "MXDP": xcg.MXDP = sHL7Array[5]; break;
                            case "NEUTA": xcg.NEUT = sHL7Array[5]; break;
                            case "NEUTP": xcg.NEUTP = sHL7Array[5]; break;
                            case "PCT": xcg.PCT = sHL7Array[5]; break;
                            case "PDW": xcg.PDW = sHL7Array[5]; break;
                            case "PLT": xcg.PLT = sHL7Array[5]; break;
                            case "RBC": xcg.RBC = sHL7Array[5]; break;
                            case "RDWCV": xcg.RDW_CV = sHL7Array[5]; break;
                            case "RDWSD": xcg.RDW_SD = sHL7Array[5]; break;
                            case "WBC": xcg.WBC = sHL7Array[5]; break;
                            case "MONA": xcg.MONO = sHL7Array[5]; break;
                            case "MONP": xcg.MONOP = sHL7Array[5]; break;
                            case "GRAN": xcg.GRAN = sHL7Array[5]; break;
                            case "GRANP": xcg.GRANP = sHL7Array[5]; break;
                            case "PLCR": xcg.PLCR = sHL7Array[5]; break;
                            default: break;
                        }
                    }
                    bool istrue = tjdao.insertXuechangguiInfo(xcg);
                    if (istrue)
                    {
                        tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, 1);
                        tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                    }
                    //返回血球的确认数据报文
                    for (int j = 0; j < sendArray.Length; j++)
                    {
                        sendHL7new += "|" + sendArray[j];
                    }
                    byte[] sendBytes = Encoding.Unicode.GetBytes(sendHL7new.Substring(1));
                    send.Send(sendBytes);
                }
           }
        }
    }
}
