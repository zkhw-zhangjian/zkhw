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

namespace zkhwClient
{
    public partial class frmMain : Form
    {
        personRegist pR = null;
        Process proHttp = new Process();
        public frmMain()
        {
            InitializeComponent();

        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            //proHttp.StartInfo.FileName = "C:\\Users\\5050\\Desktop\\healthCheck\\httpCeshi\\bin\\Debug\\httpCeshi.exe";
            //proHttp.StartInfo.UseShellExecute = false;
            //proHttp.Start();

            this.timer1.Start();//时间控件定时器

            this.label1.Text = "一体化查体车  中科弘卫";
            this.label1.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));

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
                proHttp.Close();
                service.loginLogService llse = new service.loginLogService();
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "退出系统！";
                if (lb.name != "admin" && lb.name != "" && lb.name != null)
                {
                    llse.addCheckLog(lb);
                }
                System.Environment.Exit(0);
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
            //proHttp.Kill();
            Process p = Process.GetCurrentProcess();
            if (p != null)
            {
                p.Kill();
            }
        }
    }
}
