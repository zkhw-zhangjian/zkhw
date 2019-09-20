using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Xml;
using zkhwClient.bean;
using zkhwClient.dao;
using zkhwClient.service;
using zkhwClient.view.setting;

namespace zkhwClient
{
    public partial class personRegistt : Form
    {
        static JavaScriptSerializer serializer = new JavaScriptSerializer();
        string str = Application.StartupPath;//项目路径
        int iRetUSB = 0, iRetCOM = 0;
        bool isClose = false;
        FilterInfoCollection videoDevices = null;

        public BarTender.Application btApp;
        public BarTender.Format btFormat;

        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node;
        string path = @"config.xml";
        string xindiantupath = "";
        string bichaopath = "";
        string carcode = null;
        DataTable dtshenfen = new DataTable();
        grjdDao grjddao = new grjdDao();
        jkInfoDao jkinfodao = new jkInfoDao();
        tjcheckDao jkjcheckdao = new tjcheckDao();
        grjdxxBean grjdxx = null;
        loginLogService logservice = new loginLogService();
        areaConfigDao area = new areaConfigDao();
        DataTable dtno = null;
        basicSettingDao bsdao = new basicSettingDao();
        areaConfigDao areadao = new areaConfigDao();
        public string xcuncode = null;
        public string xzcode = null;
        public string qxcode = null;
        public string shicode = null;
        public string shengcode = null;
        public string shengName = null;
        public string shiName = null;
        public string qxName = null;
        public string xzName = null;
        public string xcName = null;

        private float xMy;//定义当前窗体的宽度
        private float yMy;//定义当前窗体的高度

        public personRegistt()
        {
            InitializeComponent();

            //xMy = this.Width;
            //yMy = this.Height;
            //Common.setTag(this);
        }
      
        
        private void BindNation()
        {
            dtno = Common.GetNationDataTable(0);
            this.comboBox2.DataSource = dtno;//绑定数据源
            this.comboBox2.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox2.ValueMember = "id";//操作时获取的值
        }
        public void btnClose_Click()
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            this.Close();
        }
        private void personRegistt_Load(object sender, EventArgs e)
        {
            label45.Text = "";
            BindNation();
            Common.SetComboBoxInfo(comboBox7, ltdorganizationDao.GetShengInfo());//区域
            DataTable dtbasic = bsdao.checkBasicsettingInfo();
            if (dtbasic.Rows.Count > 0)
            {
                xcuncode = dtbasic.Rows[0]["cun_code"].ToString();
                shengcode = dtbasic.Rows[0]["sheng_code"].ToString();
                Common.SetComboBoxInfo(comboBox6, ltdorganizationDao.GetCityInfo(shengcode));
                shicode = dtbasic.Rows[0]["shi_code"].ToString();
                Common.SetComboBoxInfo(comboBox3, ltdorganizationDao.GetCountyInfo(shicode));
                qxcode = dtbasic.Rows[0]["qx_code"].ToString();
                Common.SetComboBoxInfo(comboBox4, areadao.zhenInfo(qxcode));
                xzcode = dtbasic.Rows[0]["xz_code"].ToString();
                Common.SetComboBoxInfo(comboBox5, areadao.cunInfo(xzcode));
                //因为名称有可能对应不上那么就用code对应
                Common.SetComboBoxSelectIndex(comboBox7, shengcode);
                Common.SetComboBoxSelectIndex(comboBox6, shicode);
                Common.SetComboBoxSelectIndex(comboBox3, qxcode);
                Common.SetComboBoxSelectIndex(comboBox4, xzcode);
                Common.SetComboBoxSelectIndex(comboBox5, xcuncode);
            }
            label14.Text = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日";
            registrationRecordCheck();//右侧统计信息
                                      //身份证读卡初始化
            try
            {
                int iPort;
                for (iPort = 1001; iPort <= 1016; iPort++)
                {
                    iRetUSB = CVRSDK.CVR_InitComm(iPort);
                    if (iRetUSB == 1)
                    {
                        break;
                    }
                }
                if (iRetUSB != 1)
                {
                    for (iPort = 1; iPort <= 4; iPort++)
                    {
                        iRetCOM = CVRSDK.CVR_InitComm(iPort);
                        if (iRetCOM == 1)
                        {
                            break;
                        }
                    }
                }

                if ((iRetCOM == 1) || (iRetUSB == 1))
                {
                    this.label42.Text = "初始化成功！";
                    jkjcheckdao.updateShDevice(1, -1, -1, -1, -1, -1, -1, -1, -1, -1);
                }
                else
                {
                    this.label42.Text = "初始化失败！";
                    loginLogBean lb = new loginLogBean();
                    lb.name = frmLogin.name;
                    lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    lb.eventInfo = "身份证读卡初始化失败！";
                    lb.type = "3";
                    logservice.addCheckLog(lb);
                    jkjcheckdao.updateShDevice(0, -1, -1, -1, -1, -1, -1, -1, -1, -1);
                    this.label42.Visible = true;
                }
            }
            catch (Exception ex)
            {
                loginLogBean lb = new loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "身份证读卡初始化失败！";
                lb.type = "3";
                logservice.addCheckLog(lb);
                jkjcheckdao.updateShDevice(0, -1, -1, -1, -1, -1, -1, -1, -1, -1);
            }

            //摄像头初始化
            try
            {
                // 枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                {
                    throw new ApplicationException();
                }
                //foreach (FilterInfo device in videoDevices)
                //{
                //    tscbxCameras.Items.Add(device.Name);
                //}
                //tscbxCameras.SelectedIndex = 0;

                VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.DesiredFrameSize = new System.Drawing.Size(320, 240);
                videoSource.DesiredFrameRate = 1;
                videoSourcePlayer1.VideoSource = videoSource;
                videoSourcePlayer1.Start();
            }
            catch (ApplicationException)
            {
                videoDevices = null;
                loginLogBean lb = new loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "摄像头初始化失败！";
                lb.type = "3";
                logservice.addCheckLog(lb);
                jkjcheckdao.updateShDevice(-1, 0, -1, -1, -1, -1, -1, -1, -1, -1);
            }
            //读取默认的打印条数
            ReadPrintBarCodeNumber(); 
        }
        private void ReadPrintBarCodeNumber()
        {
            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/barNum");
            string _printBarCodeNumber = node.InnerText;
            if (_printBarCodeNumber == "") _printBarCodeNumber = "4";
            txtNum.Text = _printBarCodeNumber;
        }
        public void registrationRecordCheck()
        {
            DataTable dt16num = grjddao.residentNum(basicInfoSettings.xcuncode);
            if (dt16num != null && dt16num.Rows.Count > 0)
            {
                label16.Text = dt16num.Rows[0][0].ToString();//计划体检人数
            }
            string time = Common.GetCreateTime(basicInfoSettings.createtime);
            DataTable dt19num = grjddao.jkAllNum(basicInfoSettings.xcuncode, time);
            if (dt19num != null && dt19num.Rows.Count > 0)
            {
                label19.Text = dt19num.Rows[0][0].ToString();//登记人数
            }

            int num22 = 0;
            int num24 = 0;
            int num27 = 0;
            int num29 = 0;
            int num32 = 0;
            int num34 = 0;
            int num37 = 0;
            int num39 = 0;
            DataTable dtjkifo = jkinfodao.selectjktjInfo(basicInfoSettings.xcuncode, time);
            if (dtjkifo != null && dtjkifo.Rows.Count > 0)
            {
                for (int i = 0; i < dtjkifo.Rows.Count; i++)
                {
                    string sex = dtjkifo.Rows[i]["sex"].ToString();
                    int age = Int32.Parse(dtjkifo.Rows[i]["age"].ToString());
                    if (age >= 40 && age <= 64 && "1".Equals(sex))
                    {
                        num22 += 1;
                    }
                    if (age >= 40 && age <= 64 && "2".Equals(sex))
                    {
                        num24 += 1;
                    }
                    if (age >= 65 && age <= 70 && "1".Equals(sex))
                    {
                        num27 += 1;
                    }
                    if (age >= 65 && age <= 70 && "2".Equals(sex))
                    {
                        num29 += 1;
                    }
                    if (age >= 71 && age <= 75 && "1".Equals(sex))
                    {
                        num32 += 1;
                    }
                    if (age >= 71 && age <= 75 && "2".Equals(sex))
                    {
                        num34 += 1;
                    }
                    if (age >= 75 && "1".Equals(sex))
                    {
                        num37 += 1;
                    }
                    if (age >= 75 && "2".Equals(sex))
                    {
                        num39 += 1;
                    }
                }
            }
            label22.Text = num22.ToString(); //40 - 64岁 男
            label24.Text = num24.ToString(); //40 - 64岁 女

            label27.Text = num27.ToString(); //65 - 70岁 男
            label29.Text = num29.ToString(); //65 - 70岁 女

            label32.Text = num32.ToString(); //71 - 75岁 男
            label34.Text = num34.ToString(); //71 - 75岁 女

            label37.Text = num37.ToString(); //75岁以上 男
            label39.Text = num39.ToString(); //75岁以上 女
        }
        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            //ControlCircular.Draw(e.ClipRectangle, e.Graphics, 18, false, Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255));
            //base.OnPaint(e);
        }
        private Single GetBtnFontSize(Button con)
        {
            float newx = (this.Width) / xMy;
            float newy = (this.Height) / yMy; 
            string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
            //根据窗体缩放的比例确定控件的值
            con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
            con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
            con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
            con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
            Single currentSize = (System.Convert.ToSingle(mytag[4]) * newy);//字体大小
            return currentSize;
        }
        private void button1_Paint(object sender, PaintEventArgs e)
        {
            Button bt = (Button)sender;
            //Single _size = GetBtnFontSize(bt);

            Single _size = 10;
            string stag = bt.AccessibleName; 
            string wenzi = "";
            int starti = 20;
            Color color = Color.FromArgb(81, 95, 154);
            switch (stag)
            {
                case "0":    //读卡
                    color = Color.FromArgb(77, 177, 81);
                    wenzi = "读  卡";
                    break;
                case "1":    //拍照 
                    wenzi = "拍  照";
                    break;
                case "2":    //打印 
                    wenzi = "打  印";
                    break;
                case "3":    //补打条码
                    wenzi = "补打条码";
                    starti = 10;
                    break;
            }
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, color, color);
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString(wenzi, new Font("微软雅黑", _size, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(starti, 5));
        }

        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b') 
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9')) 
                {
                    e.Handled = true;
                }
            }
        }

        private void label52_Click(object sender, EventArgs e)
        {
            int num = 0;
            if(txtNum.Text!="")
            {
                num = int.Parse(txtNum.Text.Trim());
            }
            num = num + 1;
            txtNum.Text = num.ToString();
        }

        private void label53_Click(object sender, EventArgs e)
        {
            int num = 0;
            if (txtNum.Text != "")
            {
                num = int.Parse(txtNum.Text.Trim());
            }
            num = num - 1;
            if (num <= 1) num = 1;
            txtNum.Text = num.ToString();
        }

        #region 区域

        private void comboBox7_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox6.DataSource = null;
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox7.SelectedValue == null) return;
            shengcode = this.comboBox7.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox6, ltdorganizationDao.GetCityInfo(shengcode));
        }

        private void comboBox6_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox6.SelectedValue == null) return;
            shicode = this.comboBox6.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox3, ltdorganizationDao.GetCountyInfo(shicode));
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox3.SelectedValue == null) return;
            qxcode = this.comboBox3.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox4, areadao.zhenInfo(qxcode));
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox5.DataSource = null;
            if (this.comboBox4.SelectedValue == null) return;
            xzcode = this.comboBox4.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox5, areadao.cunInfo(xzcode));
        }
        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBox5.SelectedValue == null) return;
            xcuncode = this.comboBox5.SelectedValue.ToString();
        }

        #endregion
        public void FillData()
        {
            try
            {
                byte[] name = new byte[30];
                int length = 30;
                CVRSDK.GetPeopleName(ref name[0], ref length);
                byte[] number = new byte[30];
                length = 36;
                CVRSDK.GetPeopleIDCode(ref number[0], ref length);
                byte[] people = new byte[30];
                length = 3;
                CVRSDK.GetPeopleNation(ref people[0], ref length);
                byte[] validtermOfStart = new byte[30];
                length = 16;
                CVRSDK.GetStartDate(ref validtermOfStart[0], ref length);
                byte[] birthday = new byte[30];
                length = 16;
                CVRSDK.GetPeopleBirthday(ref birthday[0], ref length);
                byte[] address = new byte[50];
                length = 100;//40 80
                CVRSDK.GetPeopleAddress(ref address[0], ref length);
                byte[] validtermOfEnd = new byte[30];
                length = 16;
                CVRSDK.GetEndDate(ref validtermOfEnd[0], ref length);
                byte[] signdate = new byte[30];
                length = 30;
                CVRSDK.GetDepartment(ref signdate[0], ref length);
                byte[] sex = new byte[30];
                length = 3;
                CVRSDK.GetPeopleSex(ref sex[0], ref length);

                //byte[] samid = new byte[32];
                //CVRSDK.CVR_GetSAMID(ref samid[0]);
                richTextBox1.Text = Encoding.GetEncoding("GB2312").GetString(address).Replace("\0", "").Trim();
                //textBox9.Text = Encoding.GetEncoding("GB2312").GetString(sex).Replace("\0", "").Trim();
                this.comboBox1.Text = Encoding.GetEncoding("GB2312").GetString(sex).Replace("\0", "").Trim();
                textBox8.Text = Encoding.GetEncoding("GB2312").GetString(birthday).Replace("\0", "").Trim();
                textBox4.Text = Encoding.GetEncoding("GB2312").GetString(signdate).Replace("\0", "").Trim();
                textBox3.Text = Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim();
                textBox1.Text = Encoding.GetEncoding("GB2312").GetString(name).Replace("\0", "").Trim();
                //textBox2.Text = Encoding.GetEncoding("GB2312").GetString(people).Replace("\0", "").Trim();
                string tmp = Encoding.GetEncoding("GB2312").GetString(people).Replace("\0", "").Trim();
                if (tmp.IndexOf("族") < 0)
                {
                    tmp = tmp + "族";
                }
                comboBox2.Text = tmp;
                //label11.Text = "安全模块号：" + System.Text.Encoding.GetEncoding("GB2312").GetString(samid).Replace("\0", "").Trim();
                //textBox8.Text = Encoding.GetEncoding("GB2312").GetString(validtermOfStart).Replace("\0", "").Trim() + "-" + Encoding.GetEncoding("GB2312").GetString(validtermOfEnd).Replace("\0", "").Trim();
                richTextBox1.Text = richTextBox1.Text.Replace("?", "号");
                //把身份证图片名称zp.bpm 修改为对应的名称
                string pName = Application.StartupPath + "\\zp.bmp";
                FileInfo inf = new FileInfo(pName);
                if (textBox1.Text != null && !"".Equals(textBox1.Text) && textBox8.Text != null && !"".Equals(textBox8.Text))
                {
                    if (textBox3.Text != null && !"".Equals(textBox3.Text))
                    {
                        if (File.Exists(Application.StartupPath + "\\cardImg\\" + textBox3.Text + ".jpg"))
                        {
                            File.Delete(Application.StartupPath + "\\cardImg\\" + textBox3.Text + ".jpg");
                        }
                        inf.MoveTo(Application.StartupPath + "\\cardImg\\" + textBox3.Text + ".jpg");

                        pictureBox1.ImageLocation = Application.StartupPath + "\\cardImg\\" + textBox3.Text + ".jpg";

                        DataTable dt = grjddao.judgeRepeat(textBox3.Text);
                        if (dt.Rows.Count > 0)
                        {
                            textBox1.Text = dt.Rows[0][0].ToString();
                            //textBox9.Text = dt.Rows[0][1].ToString();
                            string sexflag = dt.Rows[0][1].ToString();
                            if (sexflag == "1")
                            {
                                this.comboBox1.Text = "男";
                            }
                            else if (sexflag == "2")
                            {
                                this.comboBox1.Text = "女";
                            }
                            textBox8.Text = dt.Rows[0][2].ToString();
                            textBox3.Text = dt.Rows[0][3].ToString();
                            pictureBox4.ImageLocation = Application.StartupPath + "\\cardImg\\" + dt.Rows[0][4].ToString();
                            textBox5.Text = dt.Rows[0][5].ToString();
                        };
                        this.label41.Text = "读卡成功！";
                        checkPerson();//判断居民一周内是否做过体检
                    }
                    else
                    {
                        if (File.Exists(Application.StartupPath + "\\cardImg\\" + textBox1.Text + textBox8.Text + ".jpg"))
                        {
                            File.Delete(Application.StartupPath + "\\cardImg\\" + textBox1.Text + textBox8.Text + ".jpg");
                        }
                        inf.MoveTo(Application.StartupPath + "\\cardImg\\" + textBox1.Text + textBox8.Text + ".jpg");

                        pictureBox4.ImageLocation = Application.StartupPath + "\\cardImg\\" + textBox1.Text + textBox8.Text + ".jpg";

                        DataTable dt = grjddao.judgeRepeatBync(textBox1.Text, textBox8.Text);
                        if (dt.Rows.Count > 0)
                        {
                            textBox1.Text = dt.Rows[0][0].ToString();
                            //textBox9.Text = dt.Rows[0][1].ToString();
                            string sexflag = dt.Rows[0][1].ToString();
                            if (sexflag == "1")
                            {
                                this.comboBox1.Text = "男";
                            }
                            else if (sexflag == "2")
                            {
                                this.comboBox1.Text = "女";
                            }
                            textBox8.Text = dt.Rows[0][2].ToString();
                            textBox3.Text = dt.Rows[0][3].ToString();
                            pictureBox4.ImageLocation = Application.StartupPath + "\\cardImg\\" + dt.Rows[0][4].ToString();
                            richTextBox1.Text = dt.Rows[0][5].ToString();
                            tmp = dt.Rows[0][6].ToString();
                            if (tmp == "") tmp = "1";
                            DataRow[] drw = dtno.Select("id='" + tmp + "'");
                            if (drw != null)
                            {
                                comboBox2.Text = drw[0]["Name"].ToString();
                            }

                            textBox5.Text = dt.Rows[0][7].ToString();
                        };
                    }
                    if (File.Exists(pName))
                    {
                        File.Delete(pName);
                    }
                }
                else
                {
                    inf.MoveTo(Application.StartupPath + "\\cardImg\\123.jpg");
                    if (File.Exists(pName))
                    {
                        File.Delete(pName);
                    }
                    pictureBox4.ImageLocation = Application.StartupPath + "\\cardImg\\123.jpg";
                }
                jkjcheckdao.updateShDevice(1, -1, -1, -1, -1, -1, -1, -1, -1, -1);
            }
            catch (Exception ex)
            {
                loginLogBean lb = new loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "身份证读卡失败！";
                lb.type = "3";
                logservice.addCheckLog(lb);
            }
        }
        private void checkPerson()
        {
            string time1 = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            string time2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string idnumber = this.textBox3.Text;
            DataTable dttjjk = grjddao.selectTjjk(idnumber, time1, time2);
            if (dttjjk != null && dttjjk.Rows.Count > 0)
            {
                MessageBox.Show("此居民在一周内已经登记体检过一次,如需继续上次体检,请点击补打条码按钮!");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox6.Text = "";
            try
            {
                if ((iRetCOM == 1) || (iRetUSB == 1))
                {

                    int authenticate = CVRSDK.CVR_Authenticate();
                    if (authenticate == 1)
                    {
                        int readContent = CVRSDK.CVR_Read_Content(4);
                        if (readContent == 1)
                        {
                            FillData();
                        }
                        else
                        {
                            this.label41.Text = "读卡操作失败！";
                        }
                    }
                    else
                    {
                        MessageBox.Show("未放卡或卡片放置不正确");
                    }
                }
                else
                {
                    //MessageBox.Show("初始化失败！");
                    loginLogBean lb = new loginLogBean();
                    lb.name = frmLogin.name;
                    lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    lb.eventInfo = "身份证读卡初始化失败！";
                    lb.type = "3";
                    logservice.addCheckLog(lb);
                }
            }
            catch (Exception ex)
            {
                loginLogBean lb = new loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "身份证读卡失败！";
                lb.type = "3";
                logservice.addCheckLog(lb);
                jkjcheckdao.updateShDevice(0, -1, -1, -1, -1, -1, -1, -1, -1, -1);
                //MessageBox.Show("读卡错误,请重试！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoSourcePlayer1.IsRunning)
                {
                    BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                                    videoSourcePlayer1.GetCurrentVideoFrame().GetHbitmap(),
                                    IntPtr.Zero,
                                     Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());
                    PngBitmapEncoder pE = new PngBitmapEncoder();
                    pE.Frames.Add(BitmapFrame.Create(bitmapSource));

                    if (textBox1.Text != null && !"".Equals(textBox1.Text) && textBox8.Text != null && !"".Equals(textBox8.Text))
                    {
                        if (textBox3.Text != null && !"".Equals(textBox3.Text))
                        {
                            if (File.Exists(Application.StartupPath + "\\photoImg\\" + textBox3.Text + ".jpg"))
                            {
                                File.Delete(Application.StartupPath + "\\photoImg\\" + textBox3.Text + ".jpg");
                            }
                            string picName = Application.StartupPath + "\\photoImg" + "\\" + textBox3.Text + ".jpg";
                            if (File.Exists(picName))
                            {
                                File.Delete(picName);
                            }
                            using (Stream stream = File.Create(picName))
                            {
                                pE.Save(stream);
                                this.pictureBox2.ImageLocation = picName;
                            }
                        }
                        else
                        {
                            if (File.Exists(Application.StartupPath + "\\photoImg\\" + textBox1.Text + textBox8.Text + ".jpg"))
                            {
                                File.Delete(Application.StartupPath + "\\photoImg\\" + textBox1.Text + textBox8.Text + ".jpg");
                            }
                            string picName = Application.StartupPath + "\\photoImg" + "\\" + textBox1.Text + textBox8.Text + ".jpg";
                            if (File.Exists(picName))
                            {
                                File.Delete(picName);
                            }
                            using (Stream stream = File.Create(picName))
                            {
                                pE.Save(stream);
                                this.pictureBox2.ImageLocation = picName;
                            }
                        }
                    }
                    else
                    {
                        string picName = Application.StartupPath + "\\photoImg" + "\\123.jpg";
                        if (File.Exists(picName))
                        {
                            File.Delete(picName);
                        }
                        using (Stream stream = File.Create(picName))
                        {
                            pE.Save(stream);
                            this.pictureBox2.ImageLocation = picName;
                        }
                    }
                }
                jkjcheckdao.updateShDevice(-1, -1, 1, -1, -1, -1, -1, -1, -1, -1);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("摄像头异常：" + ex.Message + "\n" + ex.StackTrace);
                loginLogBean lb = new loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "摄像头拍照失败！";
                lb.type = "3";
                logservice.addCheckLog(lb);
                jkjcheckdao.updateShDevice(-1, 0, -1, -1, -1, -1, -1, -1, -1, -1);
            }
        }
        private bool HaveRepeat(string barcode)
        {
            bool flag = false;
            string s = DateTime.Now.ToString("yyyy-MM-dd");
            string sql = string.Format("select count(ID) from zkhw_tj_jk where bar_code='{0}' and createtime>='{1}'", barcode, s);
            object obj = DbHelperMySQL.GetSingle(sql);
            if (obj != null)
            {
                string tmp = obj.ToString();
                if (tmp == "") tmp = "0";
                if (int.Parse(tmp) > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }

        private string CreateBarCode(string a, string b)
        {
            string finalcode = "";
            string t = b.PadLeft(6, '0');
            if (int.Parse(t) == 1000000)
            {
                t = "000001";
            }
            finalcode = a + t;
            if (HaveRepeat(finalcode) == true)
            {
                int c = int.Parse(t) + 1;
                t = c.ToString().PadLeft(6, '0');
                finalcode = a + t;
            }
            return finalcode;
        }
        private void DisplayPersonTeShuInfo(string id_number)
        {
            label45.Text = "";
            service.personalBasicInfoService pBasicInfo = new service.personalBasicInfoService();
            DataTable dt = pBasicInfo.query(id_number);
            if (dt == null || dt.Rows.Count <= 0) return;
            string tmp = dt.Rows[0]["is_hypertension"].ToString();
            if (tmp == "") tmp = "0";
            int is_hypertension = int.Parse(tmp);

            tmp = dt.Rows[0]["is_diabetes"].ToString();
            if (tmp == "") tmp = "0";
            int is_diabetes = int.Parse(tmp);

            tmp = dt.Rows[0]["is_psychosis"].ToString();
            if (tmp == "") tmp = "0";
            int is_psychosis = int.Parse(tmp);

            tmp = dt.Rows[0]["is_tuberculosis"].ToString();
            if (tmp == "") tmp = "0";
            int is_tuberculosis = int.Parse(tmp);

            string _teshubiaoqian = "";
            if (is_hypertension != 0)
            {
                _teshubiaoqian = _teshubiaoqian + " 高";
            }
            if (is_diabetes != 0)
            {
                _teshubiaoqian = _teshubiaoqian + " 糖";
            }
            if (is_psychosis != 0)
            {
                _teshubiaoqian = _teshubiaoqian + " 精";
            }
            if (is_tuberculosis != 0)
            {
                _teshubiaoqian = _teshubiaoqian + " 结";
            }
            if (_teshubiaoqian != "")
            {
                label45.Text = _teshubiaoqian;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label45.Text = "";
            string address = richTextBox1.Text;
            string sexcomboBox = this.comboBox1.Text;
            string sex = "1";
            if ("男".Equals(sexcomboBox))
            {
                sex = "1";
            }
            else if ("女".Equals(sexcomboBox))
            {
                sex = "2";
            }
            else if ("未说明的性别".Equals(sexcomboBox))
            {
                sex = "9";
            }
            else if ("未知的性别".Equals(sexcomboBox))
            {
                sex = "0";
            }
            string birthday = textBox8.Text;
            if (birthday.IndexOf("-")<0) {
                MessageBox.Show("出生日期格式不正确,请重新输入!例:1990-01-01");return;
            }
            string signdate = textBox4.Text;
            string number = textBox3.Text;
            string name = textBox1.Text;
            string people = "1";
            string tmp = comboBox2.Text;
            if (tmp == "") tmp = "汉族";
            DataRow[] drw = dtno.Select("name='" + tmp + "'");
            if (drw != null)
            {
                people = drw[0]["id"].ToString();
            }

            if (number == null || number == "" || "".Equals(number))
            {
                MessageBox.Show("身份证号不能为空,如未带身份证，请手动填写身份证号!");
                return;
            }
            if (number.Length != 18)
            {
                MessageBox.Show("身份证号有误,请输入正确的号码!");
                return;
            }
            DisplayPersonTeShuInfo(number);
            string time1 = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " 00:00:00";
            DataTable dttjjk = grjddao.selectTjjk(number, time1);
            if (dttjjk != null && dttjjk.Rows.Count > 0)
            {
                MessageBox.Show("此居民身份证号在7天内已经登记过,不能再次登记,如需补打条码,请点击补打条码按钮!");
                return;
            }
            if (name != null && !"".Equals(name) && birthday != null && !"".Equals(birthday))
            {
                shengName = bsdao.selectAreaBycode(shengcode).Rows[0][0].ToString();
                shiName = bsdao.selectAreaBycode(shicode).Rows[0][0].ToString();
                qxName = bsdao.selectAreaBycode(qxcode).Rows[0][0].ToString();
                xzName = bsdao.selectAreaBycode(xzcode).Rows[0][0].ToString();
                xcName = bsdao.selectAreaBycode(xcuncode).Rows[0][0].ToString();
                grjdxx = new grjdxxBean();
                grjdxx.name = name;
                grjdxx.Sex = sex;
                grjdxx.Nation = people;
                grjdxx.Birthday = birthday;
                grjdxx.Zhuzhi = address;
                grjdxx.Cardcode = number;
                grjdxx.IssuingAgencies = signdate;
                grjdxx.CardPic = textBox3.Text + ".jpg";
                grjdxx.photo_code = grjdxx.Cardcode + ".jpg";
                grjdxx.age = DateTime.Now.Year - Int32.Parse(grjdxx.Birthday.Substring(0, 4));
                grjdxx.aichive_org = basicInfoSettings.organ_name;
                grjdxx.create_org = frmLogin.organCode;
                grjdxx.create_org_name = basicInfoSettings.organ_name;
                grjdxx.doctor_name = basicInfoSettings.zeren_doctor;
                grjdxx.create_archives_name = basicInfoSettings.input_name;
                grjdxx.residence_address = basicInfoSettings.allareaname;
                grjdxx.province_name = shengName;
                grjdxx.province_code = shengcode;
                grjdxx.city_code = shicode;
                grjdxx.city_name = shiName;
                grjdxx.county_code = qxcode;
                grjdxx.county_name = qxName;
                grjdxx.towns_code = xzcode;
                grjdxx.towns_name = xzName;
                grjdxx.village_code = xcuncode;
                grjdxx.village_name = xcName;
                grjdxx.create_name = frmLogin.user_Name;
            }
            else
            {
                MessageBox.Show("居民信息填写不完整！");
                return;
            }
            //if (pictureBox2.Image == null)
            //{
            //    MessageBox.Show("没有摄像头拍摄照片,请重试!");
            //    return;
            //}

            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/chejiahao");
            carcode = node.InnerText;
            carcode = carcode.Substring(carcode.Length - 4, 4);

            node = xmlDoc.SelectSingleNode("config/barnumCode");  //获取最后一次的编号
            string barnumCode = node.InnerText;

            node = xmlDoc.SelectSingleNode("config/chebiaoshi");  //获取车标识
            string chebiaoshi = node.InnerText;
            if (chebiaoshi == null || chebiaoshi.Length != 3) { MessageBox.Show("车标识不正确，请确认系统设置中的车标识！"); return; };
            string finalBarCode = CreateBarCode(chebiaoshi, barnumCode);

            if (carcode == null || carcode.Length != 4) { MessageBox.Show("车编号不正确，请确认系统设置中的车编号！"); return; };

            string nameCode = textBox1.Text + " " + Regex.Replace(textBox3.Text, "(\\d{6})\\d{10}(\\d{2})", "$1**********$2");
            if (nameCode.IndexOf('*') < 0)
            {
                nameCode = textBox1.Text + " " + textBox3.Text.Substring(0, 6) + "**********" + textBox3.Text.Substring(16, 2);
            }
            OnPrintSampleBarcode(finalBarCode, Int32.Parse(txtNum.Text), nameCode);

            node = xmlDoc.SelectSingleNode("config/barnumCode");
            int fnum = Int32.Parse(barnumCode) + 1;
            if (fnum == 1000000)
            {
                fnum = 1;
            }
            node.InnerText = fnum.ToString();
            xmlDoc.Save(path);

            registrationRecordCheck();//右侧统计信息
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                label45.Text = "";
                string idnumber = this.textBox3.Text;
                if (idnumber != null && idnumber.Length == 18)
                {
                    DisplayPersonTeShuInfo(idnumber);

                    string nameCodenew = textBox1.Text + " " + Regex.Replace(textBox3.Text, "(\\d{6})\\d{10}(\\d{2})", "$1**********$2");
                    if (nameCodenew.IndexOf('*') < 0)
                    {
                        nameCodenew = textBox1.Text + " " + textBox3.Text.Substring(0, 6) + "**********" + textBox3.Text.Substring(16, 2);
                    }
                    string codenew = "";
                    int fnum = Int32.Parse(txtNum.Text);
                    DataTable dttjjk = grjddao.selectTjjk(idnumber);
                    if (dttjjk != null && dttjjk.Rows.Count > 0)
                    {
                        codenew = dttjjk.Rows[0]["bar_code"].ToString();

                        //调用Bartender 
                        btApp = new BarTender.Application();
                        //获取打印模板,指定打印机 
                        btFormat = btApp.Formats.Open(@str + "\\cs1.btw", false, "");
                        // 同样标签的份数 
                        btFormat.PrintSetup.IdenticalCopiesOfLabel = fnum;
                        // 序列标签数 
                        btFormat.PrintSetup.NumberSerializedLabels = 1;
                        //设置参数 code
                        btFormat.SetNamedSubStringValue("code", codenew);
                        btFormat.SetNamedSubStringValue("nameCode", nameCodenew);
                        //打印开始 第2个参数是 是否显示打印机属性的。可以设置打印机路径 
                        btFormat.PrintOut(false, false);
                        //关闭摸板文件，并且关闭文件流 
                        btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        //打印完毕 
                        btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);

                    }
                    else
                    {
                        MessageBox.Show("此身份证号没有登记过，请检查!");
                    }
                }
                else
                {
                    MessageBox.Show("身份证号不正确,请检查!");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "---" + ee.StackTrace);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string name = this.textBox7.Text;
            if (name != null && !"".Equals(name))
            {
                DataTable dtRegistration = grjddao.registrationRecordInfo(name);
                if (dtRegistration.Rows.Count > 0)
                { 
                    this.dataGridView1.DataSource = dtRegistration;
                    //this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
                    //this.dataGridView1.Columns[1].HeaderCell.Value = "性别";
                    //this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
                    //this.dataGridView1.Columns[3].HeaderCell.Value = "电子档案号";
                    //this.dataGridView1.Columns[4].HeaderCell.Value = "条码号";
                    //this.dataGridView1.Columns[0].Width = 70;
                    //this.dataGridView1.Columns[1].Width = 55;
                    //this.dataGridView1.Columns[2].Width = 120;
                    //this.dataGridView1.Columns[3].Width = 150;
                    //this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                    //this.dataGridView1.AllowUserToAddRows = false;
                    //int rows = this.dataGridView1.Rows.Count - 1 < 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                    //for (int count = 0; count <= rows; count++)
                    //{
                    //    this.dataGridView1.Rows[count].HeaderCell.Value = String.Format("{0}", count + 1);
                    //}
                }
                else
                {
                    MessageBox.Show("未查询出数据!");
                }
            }
            else
            {
                MessageBox.Show("搜索框不能为空!");
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < e.RowCount; i++)
            {
                dataGridView1.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }

            for (int i = e.RowIndex + e.RowCount; i < this.dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void textBox3_DoubleClick(object sender, EventArgs e)
        {
            string number = textBox3.Text.Trim();
            if (number == "") return;
            if (number.Length < 18) return;
            if (number.Length != 18)
            {
                MessageBox.Show("身份证号有误,请输入正确的号码!");
                return;
            }
            //开始验证
            string _sYear = number.Substring(6, 4);
            string _sMonth = number.Substring(10, 2);
            string _sDay = number.Substring(12, 2);
            textBox8.Text = _sYear + "-" + _sMonth + "-" + _sDay;
            try
            {
                int _xb = int.Parse(number.Substring(16, 1));
                if (_xb % 2 == 0)
                {
                    comboBox1.Text = "女";
                }
                else
                {
                    comboBox1.Text = "男";
                }
            }
            catch
            {
                comboBox1.Text = "未说明的性别";
            }
            this.richTextBox1.Text = basicInfoSettings.shengName + basicInfoSettings.shiName + basicInfoSettings.qxName;
            //string cardCode6 = textBox3.Text.Trim().Substring(0,6);
            //DataTable dtArea = area.GetAreaByCode(cardCode6);

            // if (dtArea!=null&&dtArea.Rows.Count>0) {
            //     string fullName = dtArea.Rows[0]["detail"].ToString().Replace(",", "");
            //     this.richTextBox1.Text = fullName;
            // }
        }

        private void personRegistt_Resize(object sender, EventArgs e)
        {
            //float newx = (this.Width) / xMy;
            //float newy = (this.Height) / yMy;
            //Common.setControls(newx, newy, this);
        }

        public void OnPrintSampleBarcode(string barcode, int pageCount, string nameCode)
        {
            bool addjkbool = false;
            DataTable dt = null;
            if (grjdxx != null)
            {
                string cardcode = grjdxx.Cardcode;
                if (!"".Equals(cardcode))
                {
                    dt = grjddao.judgeRepeat(textBox3.Text);
                }
                else
                {
                    dt = grjddao.judgeRepeatBync(textBox1.Text, textBox8.Text);
                }
                if (dt.Rows.Count < 1)
                {
                    if (!"".Equals(cardcode))
                    {
                        grjdxx.archive_no = cardcode;
                    }
                    grjdxx.doctor_id = basicInfoSettings.zeren_doctorId;
                    grjddao.addgrjdInfo(grjdxx);//添加个人信息档案
                    registrationRecordCheck();//右侧统计
                }
                else
                {
                    grjdxx.archive_no = dt.Rows[0]["archive_no"].ToString();
                    grjddao.updategejdInfonew(grjdxx);
                    //grjdxx.archive_no = dt.Rows[0]["archive_no"].ToString();
                    //grjdxx.doctor_id= dt.Rows[0]["doctor_id"].ToString();
                    //grjddao.updateGrjdInfo(grjdxx.archive_no, grjdxx.photo_code);
                    //grjddao.updategejdInfo(grjdxx);
                }
                grjddao.addPhysicalExaminationInfo(grjdxx, barcode);//添加健康体检表信息 
                jkBean jk = new jkBean();
                jk.aichive_no = grjdxx.archive_no;
                jk.id_number = grjdxx.Cardcode;
                jk.bar_code = barcode;
                jk.Pic1 = grjdxx.CardPic;
                jk.Pic2 = grjdxx.Cardcode + ".jpg";
                jk.village_code = basicInfoSettings.xcuncode;
                jk.address = grjdxx.Zhuzhi;
                jk.name = grjdxx.name;
                jk.sex = grjdxx.Sex;
                jk.age = grjdxx.age;
                jk.JddwName = basicInfoSettings.organ_name;
                jk.JdrName = basicInfoSettings.input_name;
                jk.ZrysName = basicInfoSettings.zeren_doctor;
                jk.XzjdName = basicInfoSettings.xzName;
                jk.CjwhName = basicInfoSettings.xcName;
                addjkbool = grjddao.addJkInfo(jk);
                textBox5.Text = jk.aichive_no;
                textBox6.Text = barcode;
                if (addjkbool)
                {   //体检信息统计表
                    grjddao.addBgdcInfo(grjdxx, barcode, basicInfoSettings.xcuncode);
                }
            }
            try
            {
                if (addjkbool)
                {
                    //调用Bartender 
                    btApp = new BarTender.Application();
                    //获取打印模板,指定打印机
                    btFormat = btApp.Formats.Open(@str + "\\cs1.btw", false, "");
                    // 同样标签的份数 
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = pageCount;
                    // 序列标签数 
                    btFormat.PrintSetup.NumberSerializedLabels = 1;
                    //设置参数 code
                    btFormat.SetNamedSubStringValue("code", barcode);
                    btFormat.SetNamedSubStringValue("nameCode", nameCode);
                    //打印开始 第2个参数是 是否显示打印机属性的。可以设置打印机路径 
                    btFormat.PrintOut(false, false);
                    //关闭摸板文件，并且关闭文件流 
                    btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                    //打印完毕 
                    btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                    jkjcheckdao.updateShDevice(-1, -1, 1, -1, -1, -1, -1, -1, -1, -1);
                }
            }
            catch (Exception e)
            {
                loginLogBean lb = new loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "打印机设备连接不正确！";
                lb.type = "3";
                logservice.addCheckLog(lb);
                MessageBox.Show(e.Message + "---" + e.StackTrace);
                MessageBox.Show("打印机设备连接不正确,请重新连接或重启!");
                jkjcheckdao.updateShDevice(-1, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            }
            Common.SetComboBoxInfo(comboBox7, ltdorganizationDao.GetShengInfo());//默认区域
            DataTable dtbasic = bsdao.checkBasicsettingInfo();
            if (dtbasic.Rows.Count > 0)
            {
                xcuncode = dtbasic.Rows[0]["cun_code"].ToString();
                shengcode = dtbasic.Rows[0]["sheng_code"].ToString();
                Common.SetComboBoxInfo(comboBox6, ltdorganizationDao.GetCityInfo(shengcode));
                shicode = dtbasic.Rows[0]["shi_code"].ToString();
                Common.SetComboBoxInfo(comboBox3, ltdorganizationDao.GetCountyInfo(shicode));
                qxcode = dtbasic.Rows[0]["qx_code"].ToString();
                Common.SetComboBoxInfo(comboBox4, areadao.zhenInfo(qxcode));
                xzcode = dtbasic.Rows[0]["xz_code"].ToString();
                Common.SetComboBoxInfo(comboBox5, areadao.cunInfo(xzcode));
                //因为名称有可能对应不上那么就用code对应
                Common.SetComboBoxSelectIndex(comboBox7, shengcode);
                Common.SetComboBoxSelectIndex(comboBox6, shicode);
                Common.SetComboBoxSelectIndex(comboBox3, qxcode);
                Common.SetComboBoxSelectIndex(comboBox4, xzcode);
                Common.SetComboBoxSelectIndex(comboBox5, xcuncode);
            }
        }

       
    }
}
