using AForge.Video.DirectShow;
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Xml;
using zkhwClient.bean;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class personRegist : Form
    {
        string str = Application.StartupPath;//项目路径
        int iRetUSB = 0, iRetCOM = 0;
        bool isClose = false;
        FilterInfoCollection videoDevices = null;

        public BarTender.Application btApp;
        public BarTender.Format btFormat;

        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node;
        string path = @"config.xml";
        string shenghuapath = "";
        string xuechangguipath = "";
        string xindiantupath = "";
        string bichaopath = "";
        string carcode = null;
        private OleDbDataAdapter oda = null;
        private DataSet myds_data = null;

        DataTable dtshenfen = new DataTable();
        grjdDao grjddao = new grjdDao();
        jkInfoDao jkinfodao = new jkInfoDao();
        grjdxxBean grjdxx = null;
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
                }
                else
                {
                    this.label42.Text = "初始化失败！";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            }
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
                    MessageBox.Show("初始化失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读卡错误,请重试！");
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
                byte[] address = new byte[30];
                length = 70;
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
                textBox9.Text = Encoding.GetEncoding("GB2312").GetString(sex).Replace("\0", "").Trim();
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
                if (textBox3.Text != null && !"".Equals(textBox3.Text) && textBox3.Text.Length == 18)
                {
                    if (File.Exists(Application.StartupPath + "\\cardImg\\" + textBox3.Text + ".jpg"))
                    {
                        File.Delete(Application.StartupPath + "\\cardImg\\" + textBox3.Text + ".jpg");
                    }
                    inf.MoveTo(Application.StartupPath + "\\cardImg\\" + textBox3.Text + ".jpg");
                    
                    pictureBox1.ImageLocation = Application.StartupPath + "\\cardImg\\" + textBox3.Text + ".jpg";
                    if (File.Exists(pName))
                    {
                        File.Delete(pName);
                    }
                    DataTable dt = grjddao.judgeRepeat(textBox3.Text);
                    if (dt.Rows.Count > 0)
                    {
                        textBox1.Text = dt.Rows[0][0].ToString();
                        textBox9.Text = dt.Rows[0][1].ToString();
                        textBox8.Text = dt.Rows[0][2].ToString();
                        textBox3.Text = dt.Rows[0][3].ToString();
                        //richTextBox1.Text = dt.Rows[0][4].ToString();
                        //textBox2.Text = dt.Rows[0][5].ToString();
                        //textBox4.Text = dt.Rows[0][6].ToString();
                        pictureBox1.ImageLocation = Application.StartupPath + "\\cardImg\\" + dt.Rows[0][4].ToString();
                        textBox5.Text = dt.Rows[0][5].ToString();
                    };
                    this.label41.Text = "读卡成功！";
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("摄像头异常：" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        //关闭摄像头
        public void btnClose_Click()
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            this.Close();
        }
        //打印条形码
        private void button1_Click(object sender, EventArgs e)
        {
            string address = richTextBox1.Text;
            string sex = textBox9.Text;
            string birthday = textBox8.Text;
            string signdate = textBox4.Text;
            string number = textBox3.Text;
            string name = textBox1.Text;
            string people = textBox2.Text;

            if (number != null && !"".Equals(number) && name != null && !"".Equals(name) && birthday != null && !"".Equals(birthday))
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
            }
            else
            {
                MessageBox.Show("居民信息填写不完整！");
                return;
            }
            if (pictureBox1.Image == null || pictureBox2.Image == null)
            {
                MessageBox.Show("没有身份证照片或摄像头拍摄照片,请重试!");
                return;
            }

            //Random rand = new Random();
            //int randnum = rand.Next(10000, 99999);
            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/chejiahao");
            carcode = node.InnerText;
            carcode = carcode.Substring(carcode.Length-4, 4);
            node = xmlDoc.SelectSingleNode("config/barnumCode");
            string barnumCode = node.InnerText;
            if (carcode == null || carcode.Length != 4) { MessageBox.Show("车编号不正确，请确认系统设置中的车编号！"); return; };

            string nameCode = textBox1.Text + " " + Regex.Replace(textBox3.Text, "(\\d{4})\\d{4}(\\d{4})", "$1****$2");

            if (textBox1.Text == "" || textBox3.Text == "")
            {
                OnPrintSampleBarcode(carcode + barnumCode, Int32.Parse(this.numericUpDown1.Value.ToString()), "测试 2202****1410186221");
            }
            else
            {
                OnPrintSampleBarcode(carcode + barnumCode, Int32.Parse(this.numericUpDown1.Value.ToString()), nameCode);
            }
            node = xmlDoc.SelectSingleNode("config/barnumCode");
            node.InnerText = (Int32.Parse(barnumCode)+1).ToString();
            xmlDoc.Save(path);
        }
        //打印条码
        public void OnPrintSampleBarcode(string barcode, int pageCount, string nameCode)
        {
            bool addjkbool = false;
            if (grjdxx != null)
            {
                DataTable dt = grjddao.judgeRepeat(textBox3.Text);
                if (dt.Rows.Count < 1)
                {
                    grjdxx.archive_no = basicInfoSettings.xcuncode + "0"+grjdxx.Cardcode.Substring(14);
                    bool istrue = grjddao.addgrjdInfo(grjdxx);
                }
                jkBean jk = new jkBean();
                string archive_no= grjdxx.archive_no;
                if (archive_no==null|| archive_no=="") {
                    archive_no = textBox5.Text;
                }
                jk.aichive_no = archive_no;
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
                    grjddao.addBgdcInfo(grjdxx, barcode, archive_no);
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
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show("打印机设备连接不正确,请重新连接或重启!");
                //throw e;
            }
        }
        //右侧查询按钮
        private void button2_Click(object sender, EventArgs e)
        {
            string name = this.textBox7.Text;
            if (name != null && !"".Equals(name))
            {
                DataTable dtRegistration = grjddao.registrationRecordInfo(name);
                //DataView dv = dtRegistration.DefaultView;//虚拟视图
                //dv.Sort = "measureCode,meterNo,devtime asc";
                //DataTable dts = dv.ToTable(true);
                this.dataGridView1.DataSource = dtRegistration;
                this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
                this.dataGridView1.Columns[1].HeaderCell.Value = "性别";
                this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "电子档案号";
                this.dataGridView1.Columns[4].HeaderCell.Value = "条码号";
                //this.dataGridView1.Columns[7].Visible = false;
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
                MessageBox.Show("搜索框不能为空!");
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("是否需要补打条形码？");
        }

        //体检人数统计
        public void registrationRecordCheck()
        {
            DataTable dt16num= grjddao.residentNum(basicInfoSettings.xcuncode);
            if (dt16num != null&& dt16num.Rows.Count>0) {
                label16.Text = dt16num.Rows[0][0].ToString();//计划体检人数
            }

            DataTable dt19num = grjddao.jkAllNum(basicInfoSettings.xcuncode);
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
            DataTable dtjkifo =jkinfodao.selectjktjInfo(basicInfoSettings.xcuncode, basicInfoSettings.createtime);
            if (dtjkifo != null && dtjkifo.Rows.Count > 0)
            { 
                for (int i=0;i< dtjkifo.Rows.Count;i++) {
                   string sex = dtjkifo.Rows[i]["sex"].ToString();
                   int age =Int32.Parse(dtjkifo.Rows[i]["age"].ToString());
                    if (age >= 40 && age <= 64 && "男".Equals(sex)) {
                        num22 += 1;
                    }
                    if (age >= 40 && age <= 64 && "女".Equals(sex))
                    {
                        num24 += 1;
                    }
                    if (age >= 65 && age <= 70 && "男".Equals(sex))
                    {
                        num27 += 1;
                    }
                    if (age >= 65 && age <= 70 && "女".Equals(sex))
                    {
                        num29 += 1;
                    }
                    if (age >= 71 && age <= 75 && "男".Equals(sex))
                    {
                        num32 += 1;
                    }
                    if (age >= 71 && age <= 75 && "女".Equals(sex))
                    {
                        num34 += 1;
                    }
                    if (age >= 75 && "男".Equals(sex))
                    {
                        num37 += 1;
                    }
                    if (age >= 75  && "女".Equals(sex))
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
    }
}
