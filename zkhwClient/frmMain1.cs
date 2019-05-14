using zkhwClient.dao;
using zkhwClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zkhwClient.view.PublicHealthView;
using zkhwClient.PublicHealth;
using zkhwClient.view.HomeDoctorSigningView;
using zkhwClient.view.UseHelpView;
using zkhwClient.view.setting;
using zkhwClient.view;

namespace zkhwClient
{
    public partial class frmMain1 : Form
    {

        public frmMain1()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            //注册窗体关闭事件。
            this.FormClosing += new FormClosingEventHandler(MainForm_Closing);
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
                    Label[] Lab = new Label[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        Lab[i] = new Label();
                        if (i == 0)//默认首项选中
                        {
                            Lab[i].ForeColor = Color.LawnGreen;
                            personRegist pR = new personRegist();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        Lab[i].Font = new Font("宋体", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        Lab[i].Size = new Size(260, 40);//大   小
                        Lab[i].Click += new EventHandler(Lab_DouClick);
                        Lab[i].Tag = item.DropDownItems[i].Text;
                        Lab[i].Text = item.DropDownItems[i].Text;
                        this.flowLayoutPanel1.Controls.Add(Lab[i]);
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
        //点击关闭按钮时触发此函数。
        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            Application.Exit();//退出程序
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
                    Label[] Lab = new Label[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        Lab[i] = new Label();
                        if (i == 0)//默认首项选中
                        {
                            Lab[i].ForeColor = Color.LawnGreen;
                            personRegist pR = new personRegist();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }

                        Lab[i].Font = new Font("宋体", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        Lab[i].Size = new Size(260, 40);//大   小
                        Lab[i].Click += new EventHandler(Lab_DouClick);
                        Lab[i].Tag = item.DropDownItems[i].Text;
                        Lab[i].Text = item.DropDownItems[i].Text;

                        //TextBox rt = new TextBox();
                        //rt.Width = 200;
                        //rt.Height = 40;
                        //rt.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        //rt.Enabled = false;
                        //rt.Text = item.DropDownItems[i].Text;
                        //rt.Parent = Lab[i];//指定父级
                        this.flowLayoutPanel1.Controls.Add(Lab[i]);
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
                    Label[] Lab = new Label[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        Lab[i] = new Label();
                        if (i == 0)//默认首项选中
                        {
                            Lab[i].ForeColor = Color.LawnGreen;
                            onSiteSigning pR = new onSiteSigning();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        Lab[i].Font = new Font("宋体", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        Lab[i].Size = new Size(260, 40);//大   小
                        Lab[i].Click += new EventHandler(Lab_DouClick);
                        Lab[i].Tag = item.DropDownItems[i].Text;
                        Lab[i].Text = item.DropDownItems[i].Text;
                        this.flowLayoutPanel1.Controls.Add(Lab[i]);
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
        private void Lab_DouClick(object sender, EventArgs e)
        {
            Label pic = (Label)sender;
            //pic.BackColor = Color.Blue;
            string tag = pic.Tag.ToString();
            ////选中打标
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i].Tag.ToString() == tag)
                {
                    this.flowLayoutPanel1.Controls[i].ForeColor = Color.LawnGreen;
                }
                else
                {
                    this.flowLayoutPanel1.Controls[i].ForeColor = this.flowLayoutPanel1.ForeColor;
                }
            }

            if (tag == "人员登记")
            {               //公共卫生模块
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
                childrenCMHealthServices pR = new childrenCMHealthServices();
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
            {          //使用帮助模块 
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
            else if (tag == "血液分析")
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
            else if (tag == "血液计")
            {
                bloodMeter pR = new bloodMeter();
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
            else if (tag == "血糖")
            {
                bloodSugar pR = new bloodSugar();
                pR.TopLevel = false;
                pR.Dock = DockStyle.Fill;
                pR.FormBorderStyle = FormBorderStyle.None;
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(pR);
                pR.Show();
            }
            else if (tag == "体温枪")
            {
                bodyTemperatureGun pR = new bodyTemperatureGun();
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
                    Label[] Lab = new Label[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        Lab[i] = new Label();
                        if (i == 0)//默认首项选中
                        {
                            Lab[i].ForeColor = Color.LawnGreen;
                            usageStatistics pR = new usageStatistics();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        Lab[i].Font = new Font("宋体", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));

                        Lab[i].Size = new Size(260, 40);//大   小
                        Lab[i].Click += new EventHandler(Lab_DouClick);
                        Lab[i].Tag = item.DropDownItems[i].Text;
                        Lab[i].Text = item.DropDownItems[i].Text;
                        this.flowLayoutPanel1.Controls.Add(Lab[i]);
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
                    Label[] Lab = new Label[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        Lab[i] = new Label();
                        if (i == 0)//默认首项选中
                        {

                            Lab[i].ForeColor = Color.LawnGreen;
                            basicInfoSettings pR = new basicInfoSettings();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        Lab[i].Font = new Font("宋体", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        Lab[i].Size = new Size(260, 40);//大   小
                        Lab[i].Click += new EventHandler(Lab_DouClick);
                        Lab[i].Tag = item.DropDownItems[i].Text;
                        Lab[i].Text = item.DropDownItems[i].Text;
                        this.flowLayoutPanel1.Controls.Add(Lab[i]);
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
                    Label[] Lab = new Label[item.DropDownItems.Count];
                    for (int i = 0; i < item.DropDownItems.Count; i++)
                    {
                        Lab[i] = new Label();

                        if (i == 0)//默认首项选中
                        {
                            Lab[i].ForeColor = Color.LawnGreen;
                            softwareSystems pR = new softwareSystems();
                            pR.TopLevel = false;
                            pR.Dock = DockStyle.Fill;
                            pR.FormBorderStyle = FormBorderStyle.None;
                            this.panel1.Controls.Clear();
                            this.panel1.Controls.Add(pR);
                            pR.Show();
                        }
                        Lab[i].Font = new Font("宋体", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        Lab[i].Size = new Size(260, 40);//大   小
                        Lab[i].Click += new EventHandler(Lab_DouClick);
                        Lab[i].Tag = item.DropDownItems[i].Text;
                        Lab[i].Text = item.DropDownItems[i].Text;

                        //TextBox rt = new TextBox();
                        //rt.Width = 200;
                        //rt.Height = 40;
                        //rt.Font = new Font("微软雅黑", 13F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                        //rt.Enabled = false;
                        //rt.Text = item.DropDownItems[i].Text;
                        //rt.Parent = Lab[i];//指定父级
                        this.flowLayoutPanel1.Controls.Add(Lab[i]);
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
    }
}
