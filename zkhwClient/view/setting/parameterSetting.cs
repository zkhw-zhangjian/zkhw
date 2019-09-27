using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml; 
using zkhwClient.dao;

namespace zkhwClient.view.setting
{
    public partial class parameterSetting : Form
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node;
        string path = @"config.xml";
        string str = Application.StartupPath;//项目路径   
        bean.ConfigInfo _bChaoOBJ = null;
        string _BChao = "安盛B超";
        public parameterSetting()
        {
            InitializeComponent();
        }

        private void GetDeviceModel()
        {
            basicSettingDao dDao = new basicSettingDao();
            //生化
            DataTable dtsh = dDao.GetZkhwDictionaries("ZWSH");
            comboBox1.DataSource = dtsh; 
            comboBox1.DisplayMember = "ITEMNAME"; 
            comboBox1.ValueMember = "DICTCODE"; 
            //血球
            DataTable dtxq = dDao.GetZkhwDictionaries("ZWXCG");
            comboBox3.DataSource = dtxq;
            comboBox3.DisplayMember = "ITEMNAME";
            comboBox3.ValueMember = "DICTCODE";
        }

        private void LabelVisibleSH(bool a)
        {
            label3.Visible = a;
            label1.Visible = a;
            textBox1.Visible = a; 
        }
        private void LabelVisibleXCG(bool a)
        {
            label5.Visible = a;
            label2.Visible = a;
            textBox2.Visible = a;
        }
        private void CommVisible(bool a)
        {
            label14.Visible = a;
            comboBox2.Visible = a;
        }
        private void GetCommPort()
        { 
            string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBox2.Items.Clear();
            if (ArryPort.Length > 0)
            {
                for (int i = 0; i < ArryPort.Length; i++)
                {
                    this.comboBox2.Items.Add(ArryPort[i]);
                }
            } 
        }

        private void parameterSetting_Load(object sender, EventArgs e)
        {
            LabelVisibleSH(false);
            LabelVisibleXCG(false);

            GetDeviceModel();      //得到对应的设备型号
           
            GetCommPort();
            CommVisible(false);

            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/shenghuaPath");
            this.textBox1.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/xuechangguiPath");
            this.textBox2.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/chejiahao");
            this.textBox3.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/chepaihao");
            this.textBox4.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/barNum");
            this.textBox5.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/bcJudge");
            this.textBox10.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/chebiaoshi");
            this.textBox9.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/shxqAgreement");  //生化在前，血球在后 
            string shxqAgreement = node.InnerText;

            node = xmlDoc.SelectSingleNode("config/com");
            comboBox2.Text = node.InnerText; 

            GetBChaoPanDuan(); 
            this.textBox6.Text = @str + "/up/result/";
            this.textBox7.Text = @str + "/xdtImg/";
            this.textBox8.Text = @str + "/bcImg/";

            string[] a = shxqAgreement.Split(',');
            if(a.Length<2)
            { 
                return;
            }
            for(int i=0;i< comboBox1.Items.Count;i++)
            {
                DataRowView dv = comboBox1.Items[i] as DataRowView;
                string sh = dv.Row["DICTCODE"].ToString();
                if(sh==a[0].ToString().Trim())
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBox3.Items.Count; i++)
            {
                DataRowView dv = comboBox3.Items[i] as DataRowView;
                string sh = dv.Row["DICTCODE"].ToString();
                if (sh == a[1].ToString().Trim())
                {
                    comboBox3.SelectedIndex = i;
                    break;
                }
            }
        }

        private void GetBChaoPanDuan()
        {
            string s =string.Format( "Where Name='{0}'", _BChao);
            ConfigInfoManage cdal = new ConfigInfoManage();
            _bChaoOBJ = cdal.GetObj(s);
            if(_bChaoOBJ !=null)
            {
                textBox10.Text = _bChaoOBJ.Content;
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox9.Text.Trim() == "")
            {
                MessageBox.Show("车标识不能为空！");
                return;
            }
            if(textBox9.Text.Trim().Length !=3)
            {
                MessageBox.Show("车标识长度不能大于小于3！");
                return;
            }
            xmlDoc.Load(path);
            XmlNode node;

            
            string codexcg = "";
            try
            {
                DataRowView dv = comboBox3.Items[comboBox3.SelectedIndex] as DataRowView;
                codexcg = dv.Row["DICTCODE"].ToString();
            }
            catch
            {
                MessageBox.Show("血球设备选择有误！");
                return;
            }
            if(codexcg == "XCG_KBE_003")  //特殊处理
            {
                if (this.comboBox2.Text != "")
                {
                    node = xmlDoc.SelectSingleNode("config/com");
                    node.InnerText = this.comboBox2.Text;
                    xmlDoc.Save(path);
                }
                else
                {
                    MessageBox.Show("库贝尔血球串口号不能为空!"); return;
                }
            }
            string codesh = "";
            try
            {
                DataRowView dv = comboBox1.Items[comboBox1.SelectedIndex] as DataRowView;
                codesh = dv.Row["DICTCODE"].ToString();
            }
            catch
            {
                MessageBox.Show("生化设备选择有误！");
                return;
            }

            if (codesh == "SH_YNH_001")
            {
                node = xmlDoc.SelectSingleNode("config/shenghuaPath");
                node.InnerText = this.textBox1.Text;
                xmlDoc.Save(path); 
            }

            if (codexcg == "XCG_YNH_001")
            {  
                node = xmlDoc.SelectSingleNode("config/xuechangguiPath");
                node.InnerText = this.textBox2.Text;
                xmlDoc.Save(path);
            }

            node = xmlDoc.SelectSingleNode("config/shxqAgreement");
            string tmp = codesh + "," + codexcg;
            node.InnerText = tmp;
            xmlDoc.Save(path);

            
            node = xmlDoc.SelectSingleNode("config/chejiahao");
            node.InnerText = this.textBox3.Text;
            xmlDoc.Save(path);

            node = xmlDoc.SelectSingleNode("config/chepaihao");
            node.InnerText = this.textBox4.Text;
            xmlDoc.Save(path);

            if (this.textBox5.Text == "" || !Result.Validate(this.textBox5.Text.Trim(), @"^[1-9]\d*$"))
            {
                MessageBox.Show("条码默认打印数量应填写正整数!");
                return;
            }
            node = xmlDoc.SelectSingleNode("config/barNum");
            node.InnerText = this.textBox5.Text;
            xmlDoc.Save(path); 

            node = xmlDoc.SelectSingleNode("config/chebiaoshi");
            node.InnerText = this.textBox9.Text;
            xmlDoc.Save(path);

            Common._deviceModel = codesh + "," + codexcg;
            //保存数据库
            SetBChaoPanDuanInfo();
            MessageBox.Show("保存成功！");
        }
        private void SetBChaoPanDuanInfo()
        {
            int type = 1;
            if (_bChaoOBJ == null)
            {
                _bChaoOBJ = new bean.ConfigInfo();
                type = 0;
            }
            _bChaoOBJ.Name =  _BChao ;
            _bChaoOBJ.Content = textBox10.Text;
            ConfigInfoManage cdal = new ConfigInfoManage();
           if(type==0)
            {
                cdal.Insert(_bChaoOBJ);
            }
           else
            {
                cdal.Update(_bChaoOBJ);
            }
        }
        private void OpenPdf(string url)
        {
            //定义一个ProcessStartInfo实例
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            //设置启动进程的初始目录
            info.WorkingDirectory = str;
            //设置启动进程的应用程序或文档名
            info.FileName = url;
            //设置启动进程的参数
            info.Arguments = "";
            //启动由包含进程启动信息的进程资源
            try
            {
                System.Diagnostics.Process.Start(info);
            }
            catch (System.ComponentModel.Win32Exception we)
            {
                MessageBox.Show(this, we.Message);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenPdf(this.textBox6.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenPdf(this.textBox7.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenPdf(this.textBox8.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = "";
            try
            {
                DataRowView dv = comboBox1.Items[comboBox1.SelectedIndex] as DataRowView;
                code = dv.Row["DICTCODE"].ToString();
            }
            catch
            {
                code = comboBox1.Text;
            }
            LabelVisibleSH(false);
            switch(code)
            {
                case "SH_YNH_001":
                    LabelVisibleSH(true);
                    break;
            } 
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = "";
            try
            {
                DataRowView dv = comboBox3.Items[comboBox3.SelectedIndex] as DataRowView;
                code = dv.Row["DICTCODE"].ToString();
            }
            catch
            {
                code = comboBox3.Text;
            }
            LabelVisibleXCG(false);
            CommVisible(false);
            switch (code)
            {
                case "XCG_YNH_001":
                    LabelVisibleXCG(true);
                    break;
                case "XCG_KBE_003":
                    CommVisible(true);
                    break;
            }
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("打开目录", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(10, 5));
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("保存", new System.Drawing.Font("微软雅黑", 13, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(30, 6));

        }
    }
}
