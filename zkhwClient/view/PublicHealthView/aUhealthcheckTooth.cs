using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUhealthcheckTooth : Form
    {
        public string hypodontia1 = "";
        public string hypodontia2 = "";
        public string hypodontia3 = "";
        public string hypodontia4 = "";

        public string caries1 = "";
        public string caries2 = "";
        public string caries3 = "";
        public string caries4 = "";

        public string denture1 = "";
        public string denture2 = "";
        public string denture3 = "";
        public string denture4 = "";

        public aUhealthcheckTooth()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            hypodontia1 = this.textBox9.Text;
            hypodontia2 = this.textBox1.Text;
            hypodontia3 = this.textBox2.Text;
            hypodontia4 = this.textBox3.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            caries1 = this.textBox7.Text;
            caries2 = this.textBox6.Text;
            caries3 = this.textBox5.Text;
            caries4 = this.textBox4.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            denture1 = this.textBox12.Text;
            denture2 = this.textBox11.Text;
            denture3 = this.textBox10.Text;
            denture4 = this.textBox8.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void aUhealthcheckTooth_Load(object sender, EventArgs e)
        {
            this.textBox9.Text = hypodontia1;
            this.textBox1.Text = hypodontia2;
            this.textBox2.Text = hypodontia3;
            this.textBox3.Text = hypodontia4;

            this.textBox7.Text = caries1;
            this.textBox6.Text = caries2;
            this.textBox5.Text = caries3;
            this.textBox4.Text = caries4;

            this.textBox12.Text = denture1;
            this.textBox11.Text = denture2;
            this.textBox10.Text = denture3;
            this.textBox8.Text = denture4;
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            Common.txtBox_KeyPress(sender, e);
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("保存", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(18, 4));
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(255, 0, 0), Color.FromArgb(255, 0, 0));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("关闭", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(18, 4));
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("保存", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(18, 4));
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(255, 0, 0), Color.FromArgb(255, 0, 0));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("关闭", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(18, 4));
        }

        private void button6_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("保存", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(18, 4));
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(255, 0, 0), Color.FromArgb(255, 0, 0));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("关闭", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(18, 4));
        }
    }
}
