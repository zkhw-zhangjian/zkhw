using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.setting
{
    public partial class deviceManagementt : Form
    {
        string str = Application.StartupPath;//项目路径
        tjcheckDao tjdao = new tjcheckDao();
        public deviceManagementt()
        {
            InitializeComponent();
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("刷新", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 5));

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(170, 171, 171), Color.FromArgb(170, 171, 171));
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.DrawString("使用说明书", new Font("微软雅黑", 9, FontStyle.Regular), new SolidBrush(Color.White), new PointF(6, 4));
        }

        private void deviceManagementt_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Image.FromFile(@str + "/images/sh.png");
            this.pictureBox2.Image = Image.FromFile(@str + "/images/ncg.png");
            this.pictureBox3.Image = Image.FromFile(@str + "/images/xcg.png");
            this.pictureBox4.Image = Image.FromFile(@str + "/images/bc.png");
            this.pictureBox5.Image = Image.FromFile(@str + "/images/xdt.png");
            this.pictureBox6.Image = Image.FromFile(@str + "/images/sgtz.png");
            this.pictureBox7.Image = Image.FromFile(@str + "/images/xyj.png");
            this.pictureBox8.Image = Image.FromFile(@str + "/images/sfz.png");
            this.pictureBox9.Image = Image.FromFile(@str + "/images/sxt.png");
            this.pictureBox10.Image = Image.FromFile(@str + "/images/dyj.png");

            DataTable dtDeviceType = tjdao.checkDevice();
            showStatus(dtDeviceType);
        }
        private void showStatus(DataTable dtDeviceType)
        {
            Color zhagnchang = Color.Black;
            Color buzhangchang = Color.Red;
            string sfz_online = dtDeviceType.Rows[0]["sfz_online"].ToString();
            if (sfz_online == "0" || "0".Equals(sfz_online))
            {
                this.label24.ForeColor = buzhangchang;
                this.label25.ForeColor = buzhangchang;
                this.label26.ForeColor = buzhangchang;
            }
            else
            {
                this.label24.ForeColor = zhagnchang;
                this.label25.ForeColor = zhagnchang;
                this.label26.ForeColor = zhagnchang;
            }
            string sxt_online = dtDeviceType.Rows[0]["sxt_online"].ToString();
            if (sxt_online == "0" || "0".Equals(sxt_online))
            {
                this.label59.ForeColor = buzhangchang;
                this.label61.ForeColor = buzhangchang;
                this.label60.ForeColor = buzhangchang;
            }
            else
            {
                this.label59.ForeColor = zhagnchang;
                this.label61.ForeColor = zhagnchang;
                this.label60.ForeColor = zhagnchang;
            }
            string dyj_online = dtDeviceType.Rows[0]["dyj_online"].ToString();
            if (dyj_online == "0" || "0".Equals(dyj_online))
            {
                this.label56.ForeColor = buzhangchang;
                this.label55.ForeColor = buzhangchang;
                this.label54.ForeColor = buzhangchang;
            }
            else
            {
                this.label56.ForeColor = zhagnchang;
                this.label55.ForeColor = zhagnchang;
                this.label54.ForeColor = zhagnchang;
            }
            string xcg_online = dtDeviceType.Rows[0]["xcg_online"].ToString();
            if (xcg_online == "0" || "0".Equals(xcg_online))
            {
                this.label14.ForeColor = buzhangchang;
                this.label15.ForeColor = buzhangchang;
                this.label16.ForeColor = buzhangchang;
            }
            else
            {
                this.label14.ForeColor = zhagnchang;
                this.label15.ForeColor = zhagnchang;
                this.label16.ForeColor = zhagnchang;
            }
            string sh_online = dtDeviceType.Rows[0]["sh_online"].ToString();
            if (sh_online == "0" || "0".Equals(sh_online))
            {
                this.label1.ForeColor = buzhangchang;
                this.label2.ForeColor = buzhangchang;
                this.label3.ForeColor = buzhangchang;
            }
            else
            {
                this.label1.ForeColor = zhagnchang;
                this.label2.ForeColor = zhagnchang;
                this.label3.ForeColor = zhagnchang;
            }
            string ncg_online = dtDeviceType.Rows[0]["ncg_online"].ToString();
            if (ncg_online == "0" || "0".Equals(ncg_online))
            {
                this.label6.ForeColor = buzhangchang;
                this.label7.ForeColor = buzhangchang;
                this.label8.ForeColor = buzhangchang;
            }
            else
            {
                this.label6.ForeColor = zhagnchang;
                this.label7.ForeColor = zhagnchang;
                this.label8.ForeColor = zhagnchang;
            }
            string xdt_online = dtDeviceType.Rows[0]["xdt_online"].ToString();
            if (xdt_online == "0" || "0".Equals(xdt_online))
            {
                this.label39.ForeColor = buzhangchang;
                this.label40.ForeColor = buzhangchang;
                this.label41.ForeColor = buzhangchang;
            }
            else
            {
                this.label39.ForeColor = zhagnchang;
                this.label40.ForeColor = zhagnchang;
                this.label41.ForeColor = zhagnchang;
            }
            string sgtz_online = dtDeviceType.Rows[0]["sgtz_online"].ToString();
            if (sgtz_online == "0" || "0".Equals(sgtz_online))
            {
                this.label34.ForeColor = buzhangchang;
                this.label35.ForeColor = buzhangchang;
                this.label36.ForeColor = buzhangchang;
            }
            else
            {
                this.label34.ForeColor = zhagnchang;
                this.label35.ForeColor = zhagnchang;
                this.label36.ForeColor = zhagnchang;
            }
            string xy_online = dtDeviceType.Rows[0]["xy_online"].ToString();
            if (xy_online == "0" || "0".Equals(xy_online))
            {
                this.label29.ForeColor = buzhangchang;
                this.label30.ForeColor = buzhangchang;
                this.label31.ForeColor = buzhangchang;
            }
            else
            {
                this.label29.ForeColor = zhagnchang;
                this.label30.ForeColor = zhagnchang;
                this.label31.ForeColor = zhagnchang;
            }
            string bc_online = dtDeviceType.Rows[0]["bc_online"].ToString();
            if (bc_online == "0" || "0".Equals(bc_online))
            {
                this.label19.ForeColor = buzhangchang;
                this.label20.ForeColor = buzhangchang;
                this.label21.ForeColor = buzhangchang;
            }
            else
            {
                this.label19.ForeColor = zhagnchang;
                this.label20.ForeColor = zhagnchang;
                this.label21.ForeColor = zhagnchang;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dtDeviceType = tjdao.checkDevice();
            showStatus(dtDeviceType);
        }
    }
}
