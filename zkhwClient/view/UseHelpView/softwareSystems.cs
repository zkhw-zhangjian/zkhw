using System;
using System.Diagnostics;
using System.Drawing;
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
            OpenPdf(Application.StartupPath + "\\pdf\\系统软件说明书.pdf");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\B超_安盛_C5.pdf");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (sxa.Length == 2&& "SH_LD_002".Equals(sxa[0]))
            {
                OpenPdf(Application.StartupPath + "\\pdf\\生化_雷杜_Chemray420.pdf");
            }
            else
            {
                OpenPdf(Application.StartupPath + "\\pdf\\生化_英诺华_DS-301.pdf");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (sxa.Length == 2 && "XCG_LD_002".Equals(sxa[1]))
            {
                OpenPdf(Application.StartupPath + "\\pdf\\血常规_雷杜_RT-7300.pdf");
            }
            else
            {
                OpenPdf(Application.StartupPath + "\\pdf\\血常规_英诺华_HB-7021.pdf");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\尿常规_优利特_URIT-300.pdf");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\身高体重_悦骑_SG-1000.pdf");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\心电图_中旗_iMac300.pdf");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenPdf(Application.StartupPath + "\\pdf\\血压_悦骑_ABP-1000.pdf");
        }

        private void softwareSystems_Load(object sender, EventArgs e)
        {
            xmlDoc.Load(path);
            XmlNode node;
            node = xmlDoc.SelectSingleNode("config/shxqAgreement");
            string shxqAgreement = node.InnerText;//生化血球厂家协议
            sxa = shxqAgreement.Split(',');
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 8, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("系统软件说明书", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(30, 7));

        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 8, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("生化设备", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(45, 7));
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 8, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("B超设备", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(50, 7));
        }

        private void button6_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 8, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("血常规设备", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(40, 7));
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 8, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("尿常规设备", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(40, 7));
        }

        private void button8_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 8, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("心电图设备", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(40, 7));

        }

        private void button7_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 8, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("身高体重设备", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(40, 7));

        }

        private void button9_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 8, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("血压计设备", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(40, 7));

        }
    }
}
