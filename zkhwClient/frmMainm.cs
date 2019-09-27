using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using zkhwClient.dao;
using zkhwClient.PublicHealth;
using zkhwClient.utils;
using zkhwClient.view;
using zkhwClient.view.HomeDoctorSigningView;
using zkhwClient.view.PublicHealthView;
using zkhwClient.view.setting;
using zkhwClient.view.updateTjResult;
using zkhwClient.view.UseHelpView;

namespace zkhwClient
{
    public partial class frmMainm : Form
    {
   
        personRegistt pR = null;
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
        string shenghuapath = "";
        string xuechangguipath = "";
        string shlasttime = "";
        string xcglasttime = "";
        DataTable dttv = null;
        public SerialPort port = new SerialPort();
        Byte[] totalByteRead = new Byte[0];

        private float xMy;//定义当前窗体的宽度
        private float yMy;//定义当前窗体的高度

        public frmMainm()
        {
            InitializeComponent();

            //xMy = this.Width;
            //yMy = this.Height;
            //Common.setTag(this);
        }

        private void startlabel()
        {
            label26.Text = "    老年人生活        \r\n    自理能力评估";
            label30.Text = "    2型糖尿病\r\n    患者健康管理";
            label27.Text = "    老年人中医药        \r\n    健康管理";
            label31.Text = "    严重精神障碍        \r\n    患者管理";
            pangw.Dock = DockStyle.Fill; 
            panel17.Controls.Clear();
            panel17.Controls.Add(pangw);
        }

        public void SetJianDangInfo(string a, string b, string c)
        {
            label14.Text = "      " + a; //建档单位
            label18.Text = "      " + b;//建档人
            label19.Text = "      " + c;//责任医生
        }
        private void frmMainm_Load(object sender, EventArgs e)
        {
              startlabel();
            //初始化界面
            basicInfoSettings basicSet = new basicInfoSettings();
            basicSet.setFunDelegate = SetJianDangInfo;
            basicSet.Show();

            //读取配置文件
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/shxqAgreement");
            string shxqAgreement = node.InnerText;//生化血球厂家协议
            Common._deviceModel = shxqAgreement;

            dttv = grjddao.checkThresholdValues(Common._deviceModel, "");//获取阈值信息
            this.timer1.Start();//时间控件定时器
            //定时器
            this.timer3.Interval = Int32.Parse(Properties.Settings.Default.timer3Interval);
            this.timer3.Start();//1分钟定时刷新设备状态

            node = xmlDoc.SelectSingleNode("config/shlasttime");
            node.InnerText = DateTime.Now.ToString("yyyy-MM-dd") + " 06:00:00";
            xmlDoc.Save(path);
            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/xcglasttime");
            node.InnerText = DateTime.Now.ToString("yyyy-MM-dd") + " 06:00:00";
            xmlDoc.Save(path);

            DataTable dts = bsdao.checkBasicsettingInfo();
            if (dts.Rows.Count > 0)
            {
                string a = dts.Rows[0]["organ_name"].ToString();
                string b = dts.Rows[0]["input_name"].ToString();
                string c = dts.Rows[0]["zeren_doctor"].ToString();
                SetJianDangInfo(a, b, c);
            }
            else
            {
                SetJianDangInfo("", "", "");
            }

            #region 调用那个程序
            node = xmlDoc.SelectSingleNode("config/com");
            string comnum = node.InnerText;
            CallMethod(shxqAgreement, comnum);

            #endregion

            pR = new personRegistt();
            OpenWinToMain();

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
        private void CallMethod(string shxqAgreement, string comnum)
        {
            string[] sxa = shxqAgreement.Split(',');
            if (sxa.Length < 2) { return; }
            if (sxa[0].ToString().Trim() == "SH_YNH_001" || sxa[1].ToString().Trim() == "XCG_YNH_001")
            {
                this.timer2.Interval = Int32.Parse(Properties.Settings.Default.timeInterval);
                this.timer2.Start();//定时获取生化和血球的数据-英诺华
            }

            if (sxa[0].ToString().Trim() == "SH_KBE_003" || sxa[1].ToString().Trim() == "XCG_KBE_003")
            {
                socketTcpKbe();//库贝尔
                bool bl = initPort(comnum);
                if (bl)
                {
                    port.DataReceived += new SerialDataReceivedEventHandler(this.mySerialPort_DataReceived);
                }
                else
                {
                    MessageBox.Show("打开串口异常,请检查，并重启软件！");
                }
            }
            if (sxa[0].ToString().Trim() == "SH_LD_002" || sxa[1].ToString().Trim() == "XCG_LD_002")
            {
                socketTcp();//雷杜
            }
            if (sxa[0].ToString().Trim() == "SH_MR_004" || sxa[1].ToString().Trim() == "XCG_MR_004")
            {
                socketTcpMr();//迈瑞
            }
            if (sxa[0].ToString().Trim() == "SH_DR_005" || sxa[1].ToString().Trim() == "XCG_DR_005")
            {
                socketTcpDr();//迪瑞
            }
        }
       
        private void OpenWinToMain()
        {
            pR.TopLevel = false;
            pR.Dock = DockStyle.Fill;
            pR.FormBorderStyle = FormBorderStyle.None;
            panel1.Controls.Clear();
            panel1.Controls.Add(pR);
            pR.Show();
        }
        #region 底部处理 
        private void label1_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string stag = lbl.Tag.ToString();
            switch(stag)
            {
                case "0":
                    checkBichao checkBc = new checkBichao();
                    checkBc.Show();
                    break;
                case "1":
                    checkShenghua checkSh = new checkShenghua();
                    checkSh.Show();
                    break;
                case "2":
                    checkNiaocg checkNcg = new checkNiaocg();
                    checkNcg.Show();
                    break;
                case "3":
                    checkXuecg checkXcg = new checkXuecg();
                    checkXcg.Show();
                    break;
                case "4":
                    checkSgtz checkSgTz = new checkSgtz();
                    checkSgTz.Show();
                    break;
                case "5":
                    checkXindt checkXdt = new checkXindt();
                    checkXdt.Show();
                    break;
                case "6":
                    checkXueya checkXy = new checkXueya();
                    checkXy.Show();
                    break; 
            }
        }
        private void label1_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = Color.FromArgb(11, 26, 83);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = Color.FromArgb(37, 55, 129);
        }
        #endregion

        #region 顶部色块处理
        private void label2_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = Color.FromArgb(118, 118, 118);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string stag = lbl.Tag.ToString();
            if (stag == "0")
            {
                lbl.BackColor = Color.FromArgb(67, 67, 67);
            }
        }
        private void setlabeltagfortop()
        {
            label2.Tag = 0;
            label13.Tag = 0;
            label12.Tag = 0;
            label15.Tag = 0;
            label16.Tag = 0;
            label17.Tag = 0;

            label2.BackColor = Color.FromArgb(67, 67, 67);
            label12.BackColor = Color.FromArgb(67, 67, 67);
        }
        private void label2_Click(object sender, EventArgs e)
        {
            setlabeltagfortop();
            Label lbl = (Label)sender;
            lbl.BackColor = Color.FromArgb(118, 118, 118);
            if (lbl.Text == "公共卫生" || lbl.Text == "设置")
            {
                lbl.Tag = 1;
                if (lbl.Text == "公共卫生")
                {
                    pansetup.Visible = false;
                    pangw.Visible = true;
                    setlabeltaggongweiforleft(1);
                    pangw.Dock = DockStyle.Fill;
                    panel17.Controls.Clear();
                    panel17.Controls.Add(pangw);
                    if (pR != null)
                    {
                        pR.btnClose_Click();
                        pR = null;
                    }
                    pR = new personRegistt();
                    OpenWinToMain();
                }
                else if (lbl.Text == "设置")
                {
                    pansetup.Visible = true;
                    pangw.Visible = false;
                    setlabeltagshezhiforleft(1);
                    pansetup.Dock = DockStyle.Fill;
                    panel17.Controls.Clear();
                    panel17.Controls.Add(pansetup);

                    basicInfoSettings frm = new basicInfoSettings();
                    if (pR != null)
                    {
                        pR.btnClose_Click();
                        pR = null;
                    }
                    frm.TopLevel = false;
                    frm.Dock = DockStyle.Fill;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    panel1.Controls.Clear();
                    panel1.Controls.Add(frm);
                    frm.Show();
                }
            }
        }
        #endregion

        #region 左侧设置
        private Color leftselectColor= Color.FromArgb(49, 151, 052);
        private Color leftColor= Color.FromArgb(77, 177, 81);
        private void setlabeltagshezhiforleft(int itype)
        {
            if(itype==0)
            {
                label49.Tag = "0,0";
                label49.BackColor = leftColor;
            }
            else
            {
                label49.Tag = "1,0";
                label49.BackColor = leftselectColor;
            } 
            label48.Tag = "0,1";
            label48.BackColor = leftColor;

            label47.Tag = "0,2";
            label47.BackColor = leftColor;

            label46.Tag = "0,3";
            label46.BackColor = leftColor;

            label45.Tag = "0,4";
            label45.BackColor = leftColor;

            label43.Tag = "0,5";
            label43.BackColor = leftColor;
        }
        private void label49_Click(object sender, EventArgs e)
        {  
            setlabeltagshezhiforleft(0);
            Label lbl = (Label)sender;
            string a = lbl.Tag.ToString();
            string[] b = a.Split(',');
            lbl.Tag = "1," + b[1].ToString();
            lbl.BackColor = leftselectColor;
            string c = b[1].ToString();
            Form frm = null;
            switch (c)
            {
                case "0":
                    frm = new basicInfoSettings();
                    break;
                case "1":
                    frm = new deviceManagementt();
                    break;
                case "2":
                    frm = new systemlog();
                    break;
                case "3":
                    frm = new parameterSetting();
                    break;
                case "4":
                    break;
                case "5":
                    frm = new frmThresholdSetting();
                    break;
            }
            if (c=="4")
            {
                frmEmpower frm1 = new frmEmpower();
                frm1._EditType = 0;
                frm1.ShowDialog();
            }
            else
            {
                if (pR != null)
                {
                    pR.btnClose_Click();
                    pR = null;
                }
                frm.TopLevel = false;
                frm.Dock = DockStyle.Fill;
                frm.FormBorderStyle = FormBorderStyle.None;
                panel1.Controls.Clear();
                panel1.Controls.Add(frm);
                frm.Show();
            }
        }

        private void label49_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = leftselectColor;
        }

        private void label49_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string a = lbl.Tag.ToString();
            string[] b = a.Split(',');
            if (b[0].ToString() == "0")
            {
                lbl.BackColor = leftColor;
            }
        }
        #endregion

        #region 左侧公卫
        private void label20_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = leftselectColor;
        }

        private void label20_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string a = lbl.Tag.ToString();
            string[] b = a.Split(',');
            if (b[0].ToString() == "0")
            {
                lbl.BackColor = leftColor;
            }
        }
        private void setlabeltaggongweiforleft(int itype)
        {
            if(itype==0)
            {
                label20.Tag = "0,0";
                label20.BackColor = leftColor;
            }
            else
            {
                label20.Tag = "1,0";
                label20.BackColor = leftselectColor;
            }

            label21.Tag = "0,1";
            label21.BackColor = leftColor;

            label23.Tag = "0,2";
            label23.BackColor = leftColor;

            label24.Tag = "0,3";
            label24.BackColor = leftColor;

            label25.Tag = "0,4";
            label25.BackColor = leftColor;

            label26.Tag = "0,5";
            label26.BackColor = leftColor;

            label27.Tag = "0,6";
            label27.BackColor = leftColor;

            label28.Tag = "0,7";
            label28.BackColor = leftColor;

            label29.Tag = "0,8";
            label29.BackColor = leftColor;

            label30.Tag = "0,9";
            label30.BackColor = leftColor;

            label31.Tag = "0,10";
            label31.BackColor = leftColor;

            label32.Tag = "0,11";
            label32.BackColor = leftColor;

            label33.Tag = "0,12";
            label33.BackColor = leftColor;

            label22.Tag = "0,13";
            label22.BackColor = leftColor;

            label34.Tag = "0,14";
            label34.BackColor = leftColor;
        }
         
        private void label20_Click(object sender, EventArgs e)
        { 
            setlabeltaggongweiforleft(0);
            Label lbl = (Label)sender;
            string a = lbl.Tag.ToString();
            string[] b = a.Split(',');
            lbl.Tag = "1," + b[1].ToString();
            lbl.BackColor = leftselectColor;
            string c = b[1].ToString(); 
            if(c=="0")
            {
                if (pR != null)
                {
                    pR.btnClose_Click();
                    pR = null;
                }
                pR = new personRegistt();
                OpenWinToMain();
            }
            else
            {
                Form frm = null;
                switch (c)
                {
                    case "1":
                        frm = new examinatProgress();
                        break;
                    case "2":
                        frm = new examinatReport();
                        break;
                    case "3":
                        frm = new personalBasicInfo();
                        break;
                    case "4":
                        frm = new healthCheckup();
                        break;
                    case "5":
                        frm = new olderHelthService();
                        break;
                    case "6":
                        frm = new tcmHealthServices();
                        break;
                    case "7":
                        frm = new healthPoorServices();
                        break;
                    case "8":
                        frm = new hypertensionPatientServices();
                        break;
                    case "9":
                        frm = new diabetesPatientServices();
                        break;
                    case "10":
                        frm = new psychiatricPatientServices();
                        break;
                    case "11":
                        frm = new tuberculosisPatientServices();
                        break;
                    case "12":
                        frm = new childHealthServices();
                        break;
                    case "13":
                        frm = new maternalHealthServices();
                        break;
                    case "14":
                        frm = new childrenCMHealthServices();
                        break;
                }
                if(frm !=null)
                {
                    if (pR != null)
                    {
                        pR.btnClose_Click();
                        pR = null;
                    }
                    frm.TopLevel = false;
                    frm.Dock = DockStyle.Fill;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    panel1.Controls.Clear();
                    panel1.Controls.Add(frm);
                    frm.Show();
                }
            } 
        }
        #endregion

        #region  挂机
        [DllImport("user32")]
        public static extern bool LockWorkStation();//这个是调用windows的系统锁定 

        private void label16_Click(object sender, EventArgs e)
        {
            LockWorkStation();
        }
        #endregion

        private void label15_Click(object sender, EventArgs e)
        {
            softwareSystems softs = new softwareSystems();
            softs.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbldt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void frmMainm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Environment.Exit(0);
            }
            catch
            { }
        }
        #region 英洛华
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
                if (getShenghua(sql1) == null || getShenghua(sql1).Tables.Count < 1) { return; }
                DataTable arr_dt1 = getShenghua(sql1).Tables[0];
                if (arr_dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < arr_dt1.Rows.Count; j++)
                    {
                        string sql2 = "select lop.patient_id,lop.send_time,lopr.* from LisOutput lop, LisOutputResult lopr where lop.sample_id=lopr.sample_id and lop.sample_id='" + arr_dt1.Rows[j]["sample_id"].ToString() + "'";
                        DataTable arr_dt2 = getShenghua(sql2).Tables[0];
                        if (arr_dt2.Rows.Count > 0)
                        {
                            shenghuaBean sh = new shenghuaBean();
                            string[] a = Common._deviceModel.Split(',');
                            sh.deviceModel = a[0].ToString().Trim();
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
                            bool istrue = tjdao.insertShenghuaInfo(sh);
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
                                    if (drhdlc.Length == 0)
                                    {
                                        drhdlc = dttv.Select("type='HDL'");
                                    }
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
                                    if (drldlc.Length == 0)
                                    {
                                        drldlc = dttv.Select("type='LDL'");
                                    }
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
                            else
                            {
                                bool istree = tjdao.updateShenghuaInfo(sh);
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
                                    if (drhdlc.Length == 0)
                                    {
                                        drhdlc = dttv.Select("type='HDL'");
                                    }
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
                                    if (drldlc.Length == 0)
                                    {
                                        drldlc = dttv.Select("type='LDL'");
                                    }
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
                            string[] a = Common._deviceModel.Split(',');
                            xcg.deviceModel = a[1].ToString().Trim();
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
            catch
            {
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
        #endregion

        private void socketTcp()
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
                string[] sendArray = sendHL7.Split('|');
                byte[] buffernew = buffer.Skip(0).Take(effective).ToArray();
                string sHL7 = Encoding.Default.GetString(buffernew).Trim();
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\n" + sHL7);
                }
                if (sHL7.IndexOf("CHEMRAY420") > 0)
                {//解析生化协议报文数据                   
                    shenghuaBean sh = new shenghuaBean();
                    string[] a = Common._deviceModel.Split(',');
                    sh.deviceModel = a[0].ToString().Trim();
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
                        int flag = Common.JudgeValueForSh(dttv, sh);
                        tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                        #endregion
                    }
                    else
                    {
                        bool istree = tjdao.updateShenghuaInfo(sh);
                        if (!istree) return;
                        #region 生化
                        int flag = Common.JudgeValueForSh(dttv, sh);
                        tjdao.updateTJbgdcShenghua(sh.aichive_no, sh.bar_code, flag);
                        tjdao.updatePEShInfo(sh.aichive_no, sh.bar_code, sh.CHO, sh.TG, sh.LDL_C, sh.HDL_C, sh.GLU, sh.ALT, sh.AST, sh.ALB, sh.TBIL, sh.DBIL, sh.Crea, sh.UREA);
                        #endregion
                    }
                    //返回生化的确认数据报文
                    for (int j = 0; j < sendArray.Length; j++)
                    {
                        sendHL7new += "|" + sendArray[j];
                    }
                    send.Send(AckKbe(sendHL7new.Substring(1)));
                }
                else
                {//解析血球协议报文数据
                    try
                    {
                        xuechangguiBean xcg = new xuechangguiBean();
                        string[] a = Common._deviceModel.Split(',');
                        xcg.deviceModel = a[1].ToString().Trim();
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
                            int flag = Common.JudgeValueForXCG(dttv, xcg);
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
                            int flag = Common.JudgeValueForXCG(dttv, xcg);
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
            }
        }

        private void frmMainm_FormClosing(object sender, FormClosingEventArgs e)
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
                catch (Exception tt)
                {
                    MessageBox.Show("Err:" + tt.Message);
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
            try
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
                    string sendHL7 = "MSH|^~\\&|||ICUBIO|740|20190821110721||ACK^R01|1|P|2.3.1||||0||ASCII|||MSA|AA|1|Message accepted|||0|";
                    string[] sendArray = sendHL7.Split('|');
                    byte[] buffernew = buffer.Skip(0).Take(effective).ToArray();
                    string sHL7 = Encoding.Default.GetString(buffernew).Trim();

                    if (sHL7.IndexOf("ICUBIO") > 0)
                    {//解析生化协议报文数据                   
                        shenghuaBean sh = new shenghuaBean();
                        string[] a = Common._deviceModel.Split(',');
                        sh.deviceModel = a[0].ToString().Trim();
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
                            if (sHL7Array[5] == "" || "".Equals(sHL7Array[5]))
                            {
                                continue;
                            }
                            switch (sHL7Array[4])
                            {
                                case "ALB": sh.ALB = sHL7Array[5].Substring(0, sHL7Array[5].IndexOf('.') + 3); break;
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
                        sh.createTime = DateTime.ParseExact(sHL7PArray[38], "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss");
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
                                if (drhdlc.Length == 0)
                                {
                                    drhdlc = dttv.Select("type='HDL'");
                                }
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
                                if (drldlc.Length == 0)
                                {
                                    drldlc = dttv.Select("type='LDL'");
                                }
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
                                if (drhdlc.Length == 0)
                                {
                                    drhdlc = dttv.Select("type='HDL'");
                                }
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
                                if (drldlc.Length == 0)
                                {
                                    drldlc = dttv.Select("type='LDL'");
                                }
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
            catch (Exception ex)
            {
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
                    string beginText = text.Substring(0, 16);
                    string endText = text.Substring(text.Length - 18, 18);
                    if (beginText == "3C73616D706C653E" && endText == "3C2F73616D706C653E")
                    {
                        text = Encoding.ASCII.GetString(totalByteRead);
                        isCRC = true;
                    }
                    string endText1 = text.Substring(text.Length - 22, 22);
                    if (beginText == "3C73616D706C653E" && endText1 == "3C2F73616D706C653E0D0A")
                    {
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
                string[] a = Common._deviceModel.Split(',');
                xcg.deviceModel = a[1].ToString().Trim();
                var doc = new XmlDocument();
                doc.LoadXml(xmlStr);
                var rowNoteList = doc.SelectNodes("/sample/smpinfo/p");
                var fieldNodeID = rowNoteList[0].ChildNodes;
                string barcode = fieldNodeID[1].InnerText;
                string[] barcodes = barcode.Split('/');
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
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "异常报文解析：" + ee.Message + "--" + ee.StackTrace);
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
                if (totalByteRead.Length < 100) { continue; }
                string sHL7 = Encoding.Default.GetString(totalByteRead).Trim();
                string sendHL7new = "";
                string sendHL7 = "MSH|^~\\&|LIS||||20361231235956||ACK^R01|1|P|2.3.1||||||UNICODEMSA|AA|1";
                string[] sendArray = sendHL7.Split('|');
                string sendHL7sh = "MSH|^~\\&|||||20120508094823||ACK^R01|1|P|2.3.1||||2||ASCII|||MSA|AA|1|Message accepted|||0|";
                string[] sendArraysh = sendHL7sh.Split('|');
                if (sHL7.IndexOf("ASCII") > 0)
                {//解析生化协议报文数据
                    if (sHL7.Substring(0, 3) != "MSH" || sHL7.Substring(sHL7.Length - 1, 1) != "|")
                    {
                        continue;
                    }
                    shenghuaBean sh = new shenghuaBean();
                    string[] a = Common._deviceModel.Split(',');
                    sh.deviceModel = a[0].ToString().Trim();
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
                            if (drhdlc.Length == 0)
                            {
                                drhdlc = dttv.Select("type='HDL'");
                            }
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
                            if (drldlc.Length == 0)
                            {
                                drldlc = dttv.Select("type='LDL'");
                            }
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
                            if (drhdlc.Length == 0)
                            {
                                drhdlc = dttv.Select("type='HDL'");
                            }
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
                            if (drldlc.Length == 0)
                            {
                                drldlc = dttv.Select("type='LDL'");
                            }
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
                else if (sHL7.IndexOf("UNICODE") > 1)
                {//解析血球协议报文数据
                    try
                    {
                        if (sHL7.Substring(0, 3) != "MSH" || sHL7.Substring(sHL7.Length - 1, 1) != "F")
                        {
                            continue;
                        }
                        xuechangguiBean xcg = new xuechangguiBean();
                        string[] a = Common._deviceModel.Split(',');
                        xcg.deviceModel = a[1].ToString().Trim();
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
                    string[] a = Common._deviceModel.Split(',');
                    sh.deviceModel = a[0].ToString().Trim();
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
                            if (drhdlc.Length == 0)
                            {
                                drhdlc = dttv.Select("type='HDL'");
                            }
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
                            if (drldlc.Length == 0)
                            {
                                drldlc = dttv.Select("type='LDL'");
                            }
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
                            if (drhdlc.Length == 0)
                            {
                                drhdlc = dttv.Select("type='HDL'");
                            }
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
                            if (drldlc.Length == 0)
                            {
                                drldlc = dttv.Select("type='LDL'");
                            }
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
                        string[] a = Common._deviceModel.Split(',');
                        xcg.deviceModel = a[1].ToString().Trim();
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
                    if (IP == "192.168.1.80")
                    {
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

        private void timer3_Tick(object sender, EventArgs e)
        {
            GetAllLocalMachines();//cmd获取局域网内的所有设备IP和MAC信息
            DataTable dtDeviceType = tjdao.checkDevice();
            if (dtDeviceType == null || dtDeviceType.Rows.Count < 1)
            {
                return;
            }
            Color _color= Color.FromArgb(37, 55, 129);
            string sfz_online = dtDeviceType.Rows[0]["sfz_online"].ToString();
            if (sfz_online == "0" || "0".Equals(sfz_online))
            {
                label10.BackColor = Color.Red;
            }
            else
            {
                label10.BackColor = _color;
            }
            string sxt_online = dtDeviceType.Rows[0]["sxt_online"].ToString();
            if (sxt_online == "0" || "0".Equals(sxt_online))
            {
                label9.BackColor = Color.Red;
            }
            else
            {
                label9.BackColor = _color;
            }
            string dyj_online = dtDeviceType.Rows[0]["dyj_online"].ToString();
            if (dyj_online == "0" || "0".Equals(dyj_online))
            {
                label11.BackColor = Color.Red;
            }
            else
            {
                label11.BackColor = _color;
            }
            string xcg_online = dtDeviceType.Rows[0]["xcg_online"].ToString();
            if (xcg_online == "0" || "0".Equals(xcg_online))
            {
                label5.BackColor = Color.Red;
            }
            else
            {
                label5.BackColor = _color;
            }
            string sh_online = dtDeviceType.Rows[0]["sh_online"].ToString();
            if (sh_online == "0" || "0".Equals(sh_online))
            {
                label3.BackColor = Color.Red;
            }
            else
            {
                label3.BackColor = _color;
            }
            string ncg_online = dtDeviceType.Rows[0]["ncg_online"].ToString();
            if (ncg_online == "0" || "0".Equals(ncg_online))
            {
                label4.BackColor = Color.Red;
            }
            else
            {
                label4.BackColor = _color;
            }
            string xdt_online = dtDeviceType.Rows[0]["xdt_online"].ToString();
            if (xdt_online == "0" || "0".Equals(xdt_online))
            {
                label7.BackColor = Color.Red;
            }
            else
            {
                label7.BackColor = _color;
            }
            string sgtz_online = dtDeviceType.Rows[0]["sgtz_online"].ToString();
            if (sgtz_online == "0" || "0".Equals(sgtz_online))
            {
                label6.BackColor = Color.Red;
            }
            else
            {
                label6.BackColor = _color;
            }
            string xy_online = dtDeviceType.Rows[0]["xy_online"].ToString();
            if (xy_online == "0" || "0".Equals(xy_online))
            {
                label8.BackColor = Color.Red;
            }
            else
            {
                label8.BackColor = _color;
            }
            string bc_online = dtDeviceType.Rows[0]["bc_online"].ToString();
            if (bc_online == "0" || "0".Equals(bc_online))
            {
                label1.BackColor = Color.Red;
            }
            else
            {
                label1.BackColor = _color;
            }
        }

        private void label17_Click(object sender, EventArgs e)
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

        private void frmMainm_Resize(object sender, EventArgs e)
        {
            //float newx = (this.Width) / xMy;
            //float newy = (this.Height) / yMy;
            //Common.setControls(newx, newy, this);
        }

        private void label13_Click(object sender, EventArgs e)
        {
            HomeDoctorLogin softs = new HomeDoctorLogin();
            softs.Show();
        }
    }
}
