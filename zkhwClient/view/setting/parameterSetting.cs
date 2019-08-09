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

        private void parameterSetting_Load(object sender, EventArgs e)
        {
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
            this.richTextBox1.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/chebiaoshi");
            this.textBox9.Text = node.InnerText;
            node = xmlDoc.SelectSingleNode("config/shxqAgreement");
            this.comboBox1.Text = node.InnerText;
            if (this.comboBox1.Text == "库贝尔")
            {
                this.label14.Visible = true;
                this.comboBox2.Visible = true;
                string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
                this.comboBox2.Items.Clear();
                if (ArryPort.Length > 0)
                {
                    for (int i = 0; i < ArryPort.Length; i++)
                    {
                        this.comboBox2.Items.Add(ArryPort[i]);
                    }   
                }
                node = xmlDoc.SelectSingleNode("config/com");
                this.comboBox2.Text = node.InnerText;
            }
            else if (this.comboBox1.Text == "英诺华")
            {
                this.label1.Visible = true;
                this.label2.Visible = true;
                this.label3.Visible = true;
                this.label5.Visible = true;
                this.textBox1.Visible = true;
                this.textBox2.Visible = true;
            }
            GetBChaoPanDuan();

            this.textBox6.Text = @str + "/up/result/";
            this.textBox7.Text = @str + "/xdtImg/";
            this.textBox8.Text = @str + "/bcImg/";
        }

        private void GetBChaoPanDuan()
        {
            string s =string.Format( "Where Name='{0}'", _BChao);
            ConfigInfoManage cdal = new ConfigInfoManage();
            _bChaoOBJ = cdal.GetObj(s);
            if(_bChaoOBJ !=null)
            {
                richTextBox1.Text = _bChaoOBJ.Content;
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

            if (this.comboBox1.Text == "库贝尔")
            {
                if (this.comboBox1.Text == "库贝尔" && this.comboBox2.Text != "")
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
            
            node = xmlDoc.SelectSingleNode("config/shxqAgreement");
            node.InnerText = this.comboBox1.Text;
            xmlDoc.Save(path);

            if (this.comboBox1.Text== "英诺华") {
            node = xmlDoc.SelectSingleNode("config/shenghuaPath");
            node.InnerText = this.textBox1.Text;
            xmlDoc.Save(path);

            node = xmlDoc.SelectSingleNode("config/xuechangguiPath");
            node.InnerText = this.textBox2.Text;
            xmlDoc.Save(path);
             }
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
            _bChaoOBJ.Content = richTextBox1.Text;
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
            string shxq = this.comboBox1.Text;
            if (shxq == "库贝尔")
            {
                this.label14.Visible = true;
                this.comboBox2.Visible = true;
                string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
                this.comboBox2.Items.Clear();
                if (ArryPort.Length > 0)
                {
                    for (int i = 0; i < ArryPort.Length; i++)
                    {
                        this.comboBox2.Items.Add(ArryPort[i]);
                    }
                }
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.label3.Visible = false;
                this.label5.Visible = false;
                this.textBox1.Visible = false;
                this.textBox2.Visible = false;
            }
            else if (shxq == "英诺华")
            {
                this.label14.Visible = false;
                this.comboBox2.Visible = false;
                this.label1.Visible = true;
                this.label2.Visible = true;
                this.label3.Visible = true;
                this.label5.Visible = true;
                this.textBox1.Visible = true;
                this.textBox2.Visible = true;
            }
            else
            {
                this.label14.Visible = false;
                this.comboBox2.Visible = false;
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.label3.Visible = false;
                this.label5.Visible = false;
                this.textBox1.Visible = false;
                this.textBox2.Visible = false;
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
    }
}
