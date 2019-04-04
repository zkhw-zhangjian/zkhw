using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace zkhwClient.view.setting
{
    public partial class parameterSetting : Form
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node;
        string path = @"config.xml";

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/shenghuaPath");
            node.InnerText = this.textBox1.Text;
            xmlDoc.Save(path);

            node = xmlDoc.SelectSingleNode("config/xuechangguiPath");
            node.InnerText = this.textBox2.Text;
            xmlDoc.Save(path);

            node = xmlDoc.SelectSingleNode("config/chejiahao");
            node.InnerText = this.textBox3.Text;
            xmlDoc.Save(path);

            node = xmlDoc.SelectSingleNode("config/chepaihao");
            node.InnerText = this.textBox4.Text;
            xmlDoc.Save(path);
            MessageBox.Show("保存成功！");
        }
    }
}
