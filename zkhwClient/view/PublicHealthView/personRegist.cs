using AForge.Video.DirectShow;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
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
using zkhwClient.utils;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class personRegist : Form
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
        public personRegist()
        {
            InitializeComponent();
        }

        private void personRegist_Load(object sender, EventArgs e)
        {
            this.label1.Text = "居民健康档案登记";
            this.label1.ForeColor = Color.SkyBlue;
            label1.Font = new Font("微软雅黑", 20F, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label1.Left = (this.panel1.Width - this.label1.Width) / 2;
            label1.BringToFront();

            this.label13.Text = "登记记录";
            this.label13.ForeColor = Color.SkyBlue;
            label13.Font = new Font("微软雅黑", 20F, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label13.Left = (this.panel4.Width - this.label13.Width) / 2;
            label13.BringToFront();

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
        //读取身份证
        private void button3_Click(object sender, EventArgs e)
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
                textBox2.Text = Encoding.GetEncoding("GB2312").GetString(people).Replace("\0", "").Trim();
                //label11.Text = "安全模块号：" + System.Text.Encoding.GetEncoding("GB2312").GetString(samid).Replace("\0", "").Trim();
                //textBox8.Text = Encoding.GetEncoding("GB2312").GetString(validtermOfStart).Replace("\0", "").Trim() + "-" + Encoding.GetEncoding("GB2312").GetString(validtermOfEnd).Replace("\0", "").Trim();
                richTextBox1.Text=richTextBox1.Text.Replace("?","号");
                //把身份证图片名称zp.bpm 修改为对应的名称
                string pName = Application.StartupPath + "\\zp.bmp";
                FileInfo inf = new FileInfo(pName);
                if (textBox1.Text != null && !"".Equals(textBox1.Text)&& textBox8.Text != null && !"".Equals(textBox8.Text))
                {
                    if (textBox3.Text != null && !"".Equals(textBox3.Text)) {
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
                            string sexflag= dt.Rows[0][1].ToString();
                            if (sexflag == "1")
                            {
                                this.comboBox1.Text = "男";
                            } else if (sexflag == "2") {
                                this.comboBox1.Text = "女";
                            }
                            textBox8.Text = dt.Rows[0][2].ToString();
                            textBox3.Text = dt.Rows[0][3].ToString();
                            pictureBox1.ImageLocation = Application.StartupPath + "\\cardImg\\" + dt.Rows[0][4].ToString();
                            textBox5.Text = dt.Rows[0][5].ToString();
                        };
                        this.label41.Text = "读卡成功！";
                        checkPerson();//判断居民一周内是否做过体检
                    }
                    else {
                        if (File.Exists(Application.StartupPath + "\\cardImg\\" + textBox1.Text + textBox8.Text + ".jpg"))
                        {
                            File.Delete(Application.StartupPath + "\\cardImg\\" + textBox1.Text + textBox8.Text + ".jpg");
                        }
                        inf.MoveTo(Application.StartupPath + "\\cardImg\\" + textBox1.Text + textBox8.Text + ".jpg");

                        pictureBox1.ImageLocation = Application.StartupPath + "\\cardImg\\" + textBox1.Text + textBox8.Text + ".jpg";

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
                            pictureBox1.ImageLocation = Application.StartupPath + "\\cardImg\\" + dt.Rows[0][4].ToString();
                            richTextBox1.Text = dt.Rows[0][5].ToString();
                            textBox2.Text = dt.Rows[0][6].ToString();
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
                    pictureBox1.ImageLocation = Application.StartupPath + "\\cardImg\\123.jpg";
                }
                jkjcheckdao.updateShDevice(1, -1, -1, -1, -1, -1, -1, -1, -1, -1);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                loginLogBean lb = new loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "身份证读卡失败！";
                lb.type = "3";
                logservice.addCheckLog(lb);
            }
        }

        //摄像头拍照
        private void button4_Click(object sender, EventArgs e)
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
                        else {
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
        //关闭摄像头
        public void btnClose_Click()
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            this.Close();
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
            if(_teshubiaoqian !="")
            {
                label45.Text = _teshubiaoqian;
            }
        }
        //打印条形码
        private void button1_Click(object sender, EventArgs e)
        {
            label45.Text = "";
            string address = richTextBox1.Text;
            string sexcomboBox = this.comboBox1.Text;
            string sex = "1";
            if ("男".Equals(sexcomboBox))
            {
                sex = "1";
            }
            else if ("女".Equals(sexcomboBox)) {
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
            string signdate = textBox4.Text;
            string number = textBox3.Text;
            string name = textBox1.Text;
            string people = textBox2.Text;
            if (number==null|| number == "" || "".Equals(number)) {
                MessageBox.Show("身份证号不能为空,如未带身份证，请手动填写身份证号!");
                return;
            }
            DisplayPersonTeShuInfo(number);
            string time1 = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")+" 00:00:00";
            DataTable dttjjk = grjddao.selectTjjk(number, time1);
            if (dttjjk != null && dttjjk.Rows.Count > 0)
            {
                MessageBox.Show("此居民身份证号在7天内已经登记过,不能再次登记,如需补打条码,请点击补打条码按钮!");
                return;
            }
            if (name != null && !"".Equals(name) && birthday != null && !"".Equals(birthday))
            {
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
                grjdxx.create_org_name= basicInfoSettings.organ_name;
                grjdxx.doctor_name = basicInfoSettings.zeren_doctor;
                grjdxx.create_archives_name = basicInfoSettings.input_name;
                grjdxx.residence_address = basicInfoSettings.allareaname;
                grjdxx.province_name = basicInfoSettings.shengName;
                grjdxx.province_code = basicInfoSettings.shengcode;
                grjdxx.city_code = basicInfoSettings.shicode;
                grjdxx.city_name = basicInfoSettings.shiName;
                grjdxx.county_code = basicInfoSettings.qxcode;
                grjdxx.county_name = basicInfoSettings.qxName;
                grjdxx.towns_code = basicInfoSettings.xzcode;
                grjdxx.towns_name = basicInfoSettings.xzName;
                grjdxx.village_code = basicInfoSettings.xcuncode;
                grjdxx.village_name = basicInfoSettings.xcName;
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
            carcode = carcode.Substring(carcode.Length-4, 4);
            node = xmlDoc.SelectSingleNode("config/barnumCode");
            string barnumCode = node.InnerText;
            if (carcode == null || carcode.Length != 4) { MessageBox.Show("车编号不正确，请确认系统设置中的车编号！"); return; };

            string nameCode = textBox1.Text + " " + Regex.Replace(textBox3.Text, "(\\d{6})\\d{10}(\\d{2})", "$1**********$2");
            if (nameCode.IndexOf('*') < 0) { 
                nameCode = textBox1.Text + " " + textBox3.Text.Substring(0, 6) + "**********" + textBox3.Text.Substring(16, 2);
            }
            OnPrintSampleBarcode(carcode + barnumCode, Int32.Parse(this.numericUpDown1.Value.ToString()), nameCode);
         
            node = xmlDoc.SelectSingleNode("config/barnumCode");
            int fnum= Int32.Parse(barnumCode) + 1;
            if (fnum==100000){
                fnum = 10001;
            }
            node.InnerText = fnum.ToString();
            xmlDoc.Save(path);

            registrationRecordCheck();//右侧统计信息
        }
        //打印条码
        public void OnPrintSampleBarcode(string barcode, int pageCount, string nameCode)
        {
            bool addjkbool = false;
            DataTable dt = null;
            if (grjdxx != null)
            {
                string cardcode= grjdxx.Cardcode;
                if (!"".Equals(cardcode)) {
                    dt = grjddao.judgeRepeat(textBox3.Text);
                }
                else {
                    dt = grjddao.judgeRepeatBync(textBox1.Text, textBox8.Text);
                }
                if (dt.Rows.Count < 1)
                {
                    if (!"".Equals(cardcode))
                    {
                        //grjdxx.archive_no = basicInfoSettings.xcuncode + "0" + grjdxx.Cardcode.Substring(14);
                        grjdxx.archive_no = cardcode;
                    }
                    //else
                    //{
                    //    grjdxx.archive_no = basicInfoSettings.xcuncode + barcode.Substring(5, 4);
                    //}
                    grjdxx.doctor_id = basicInfoSettings.zeren_doctorId;
                    grjddao.addgrjdInfo(grjdxx);//添加个人信息档案
                    registrationRecordCheck();//右侧统计
                }
                else {
                    grjdxx.archive_no = dt.Rows[0]["archive_no"].ToString();
                    grjdxx.doctor_id= dt.Rows[0]["doctor_id"].ToString();
                    grjddao.updateGrjdInfo(grjdxx.archive_no, grjdxx.photo_code);
                    grjddao.updategejdInfo(grjdxx);
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
                MessageBox.Show("打印机设备连接不正确,请重新连接或重启!");
                jkjcheckdao.updateShDevice(-1, -1, 0, -1, -1, -1, -1, -1, -1, -1);
                MessageBox.Show(e.Message+"---"+e.StackTrace);
            }
        }
        //右侧查询按钮
        private void button2_Click(object sender, EventArgs e)
        {
            string name = this.textBox7.Text;
            if (name != null && !"".Equals(name))
            {
                DataTable dtRegistration = grjddao.registrationRecordInfo(name);
                if (dtRegistration.Rows.Count > 0)
                {
                    //DataView dv = dtRegistration.DefaultView;//虚拟视图
                    //dv.Sort = "measureCode,meterNo,devtime asc";
                    //DataTable dts = dv.ToTable(true);
                    this.dataGridView1.DataSource = dtRegistration;
                    this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
                    this.dataGridView1.Columns[1].HeaderCell.Value = "性别";
                    this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
                    this.dataGridView1.Columns[3].HeaderCell.Value = "电子档案号";
                    this.dataGridView1.Columns[4].HeaderCell.Value = "条码号";
                    this.dataGridView1.Columns[0].Width = 70;
                    this.dataGridView1.Columns[1].Width = 55;
                    this.dataGridView1.Columns[2].Width = 120;
                    this.dataGridView1.Columns[3].Width = 150;
                    this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                    this.dataGridView1.AllowUserToAddRows = false;
                    int rows = this.dataGridView1.Rows.Count - 1 < 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                    for (int count = 0; count <= rows; count++)
                    {
                        this.dataGridView1.Rows[count].HeaderCell.Value = String.Format("{0}", count + 1);
                    }
                }
                else {
                    MessageBox.Show("未查询出数据!");
                }
            }
            else {
                MessageBox.Show("搜索框不能为空!");
            }
        }


        private void button5_Click(object sender, EventArgs e)
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
                int fnum = Int32.Parse(this.numericUpDown1.Value.ToString());
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
                else {
                    MessageBox.Show("此身份证号没有登记过，请检查!");
                }
            }
            else
            {
                MessageBox.Show("身份证号不正确,请检查!");
            }
        }
        private void checkPerson()
        {   
            string time1 = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            string time2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string idnumber = this.textBox3.Text;
            DataTable dttjjk = grjddao.selectTjjk(idnumber,time1,time2);
            if (dttjjk != null && dttjjk.Rows.Count > 0)
            {
                MessageBox.Show("此居民在一周内已经登记体检过一次,如需继续上次体检,请点击补打条码按钮!");
            }
        }
        /// <summary>
        /// 双击截取身份证的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox3_DoubleClick(object sender, EventArgs e)
        {
            if (textBox3.Text == "") return;
            if (textBox3.Text.Length < 18) return;
            //开始验证
            string _sYear = textBox3.Text.Substring(6, 4);
            string _sMonth = textBox3.Text.Substring(10, 2);
            string _sDay = textBox3.Text.Substring(12, 2);
            textBox8.Text = _sYear + "-" + _sMonth + "-" + _sDay;
            int _xb = int.Parse(textBox3.Text.Substring(16, 1));
            if(_xb%2==0)
            {
                comboBox1.Text = "女";
            }
            else
            {
                comboBox1.Text = "男";
            }
           string cardCode6 = textBox3.Text.Trim().Substring(0,6);
           DataTable dtArea = area.selectAreaBycode(cardCode6);
            if (dtArea!=null&&dtArea.Rows.Count>0) {
                string fullName = dtArea.Rows[0]["full_name"].ToString().Replace("中国", "").Replace(",", "");
                this.richTextBox1.Text = fullName;
            }
        }

        //体检人数统计
        public void registrationRecordCheck()
        {
            DataTable dt16num= grjddao.residentNum(basicInfoSettings.xcuncode);
            if (dt16num != null&& dt16num.Rows.Count>0) {
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
            DataTable dtjkifo =jkinfodao.selectjktjInfo(basicInfoSettings.xcuncode, time);
            if (dtjkifo != null && dtjkifo.Rows.Count > 0)
            { 
                for (int i=0;i< dtjkifo.Rows.Count;i++) {
                   string sex = dtjkifo.Rows[i]["sex"].ToString();
                   int age =Int32.Parse(dtjkifo.Rows[i]["age"].ToString());
                    if (age >= 40 && age <= 64 && "1".Equals(sex)) {
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
                    if (age >= 75  && "2".Equals(sex))
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

        #region 打印条码个数
        private void ReadPrintBarCodeNumber()
        { 
            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/barNum");
            string _printBarCodeNumber = node.InnerText; 
            if (_printBarCodeNumber == "") _printBarCodeNumber = "4";
            numericUpDown1.Value = int.Parse(_printBarCodeNumber);
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //if (numericUpDown1.Value == 0) numericUpDown1.Value = 4;
            //WritePrintBarCodeNumber();
        }

        private void WritePrintBarCodeNumber()
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/barNum");
            node.InnerText = numericUpDown1.Value.ToString();
            xmlDoc.Save(path); 
        }
        #endregion
    }
}
