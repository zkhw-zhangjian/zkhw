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

namespace zkhwClient
{
    public partial class frmMain : Form
    {
        personRegist pR = null;
        Process proHttp = new Process();
        basicSettingDao bsdao = new basicSettingDao();
        tjcheckDao thdao = new tjcheckDao();
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
            //监听有没有B超的文件生成

            //验证监听文件是否存在
            //string watchPath = string.Empty;

            ////是否启动监听AOUP
            //if (System.IO.File.Exists(watchPath))
            //{
            //    //开启监控
            //    FileWatcher.WatcheDirForAoup();

            //}
            //else
            //{
            //    MessageBox.Show(watchPath + "\nB超监听开启失败，系统不能正常运行！\n请创建该文件后重新运行应用程序！", "提示");
            //    return;
            //}

            basicInfoSettings basicSet = new basicInfoSettings();
            basicSet.Show();
            //http
            proHttp.StartInfo.FileName = Application.StartupPath+"\\http\\httpCeshi.exe";
            proHttp.StartInfo.UseShellExecute = false;
            proHttp.Start();

            this.timer1.Start();//时间控件定时器
            this.timer2.Interval =Int32.Parse(Properties.Settings.Default.timeInterval);
            this.timer2.Start();//定时获取生化和血球的数据

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
                proHttp.Kill() ;
                service.loginLogService llse = new service.loginLogService();
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "退出系统！";
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
        [DllImport("user32 ")]
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
                            pR.btnClose_Click();
                            pR = null;
                            
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
            else if (tag == "软件系统")
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
            proHttp.Kill();
            Process p = Process.GetCurrentProcess();
            if (p != null)
            {
                p.Kill();
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
            if (shenghuapath == "")
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
                    bool istrue= thdao.insertShenghuaInfo(sh);
                    if (istrue) {
                        thdao.updateTJbgdcShenghua(sh.aichive_no,sh.bar_code,1);
                    }
                }
            }
            if (xuechangguipath == "")
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
                    bool istrue = thdao.insertXuechangguiInfo(xcg);
                    if (istrue)
                    {
                        thdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, 1);
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
    }
}
