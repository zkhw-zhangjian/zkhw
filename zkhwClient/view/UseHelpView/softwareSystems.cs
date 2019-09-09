﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;

namespace zkhwClient.view.UseHelpView
{
    public partial class softwareSystems : Form
    {
        XmlDocument xmlDoc = new XmlDocument();
        string path = @"config.xml";
        string[] sxa = null;
        public softwareSystems()
        {
            InitializeComponent();
        }

        private void OpenPdf(string url)
        {
            //定义一个ProcessStartInfo实例
            ProcessStartInfo info = new ProcessStartInfo();
            //设置启动进程的初始目录
            info.WorkingDirectory = Application.StartupPath;
            //设置启动进程的应用程序或文档名
            info.FileName = url;
            //设置启动进程的参数
            info.Arguments = "";
            //启动由包含进程启动信息的进程资源
            try
            {
                Process.Start(info);
            }
            catch
            {
                return;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\软件系统操作手册.docx");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\B超.docx");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (sxa.Length == 2&& "SH_LD_002".Equals(sxa[0]))
            {
                OpenPdf(Application.StartupPath + "\\pdf\\雷杜生化Chemray420.pdf");
            }
            else
            {
                OpenPdf(Application.StartupPath + "\\pdf\\生化血球配置.docx");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\尿分析仪.docx");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (sxa.Length == 2 && "XCG_LD_002".Equals(sxa[1]))
            {
                OpenPdf(Application.StartupPath + "\\pdf\\雷杜血球RT-7300.pdf");
            }
            else
            {
                OpenPdf(Application.StartupPath + "\\pdf\\生化血球配置.docx");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\身高体重仪.docx");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\心电图仪.docx");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\血压检测仪.docx");
        }

        private void softwareSystems_Load(object sender, EventArgs e)
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/shxqAgreement");
            string shxqAgreement = node.InnerText;//生化血球厂家协议
            sxa = shxqAgreement.Split(',');
        }
    }
}
