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
    public partial class deviceManagement : Form
    {
        string str = Application.StartupPath;//项目路径
        tjcheckDao tjdao = new tjcheckDao();
        public deviceManagement()
        {
            InitializeComponent();
        }

        private void deviceManagement_Load(object sender, EventArgs e)
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
        //生化说明书
        private void label4_Click(object sender, EventArgs e)
        {

        }
        //尿常规说明书
        private void label5_Click(object sender, EventArgs e)
        {

        }
        //血常规说明书
        private void label9_Click(object sender, EventArgs e)
        {

        }
        //B超说明书
        private void label13_Click(object sender, EventArgs e)
        {

        }
        //心电图说明书
        private void label17_Click(object sender, EventArgs e)
        {

        }
        //身高体重说明书
        private void label21_Click(object sender, EventArgs e)
        {

        }
        //血压检测说明书
        private void label25_Click(object sender, EventArgs e)
        {

        }
        //身份证读卡说明书
        private void label29_Click(object sender, EventArgs e)
        {

        }
        //摄像头说明书
        private void label33_Click(object sender, EventArgs e)
        {

        }
        //打印机说明书
        private void label37_Click(object sender, EventArgs e)
        {

        }
        //全部刷新
        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dtDeviceType = tjdao.checkDevice();
            showStatus(dtDeviceType);
        }

        private void showStatus(DataTable dtDeviceType) {
            string sfz_online = dtDeviceType.Rows[0]["sfz_online"].ToString();
            if (sfz_online == "0" || "0".Equals(sfz_online))
            {
                this.label30.BackColor = Color.Red;
                this.label31.BackColor = Color.Red;
                this.label32.BackColor = Color.Red;
            }
            else
            {
                this.label30.BackColor = Color.Lime;
                this.label31.BackColor = Color.Lime;
                this.label32.BackColor = Color.Lime;
            }
            string sxt_online = dtDeviceType.Rows[0]["sxt_online"].ToString();
            if (sxt_online == "0" || "0".Equals(sxt_online))
            {
                this.label34.BackColor = Color.Red;
                this.label35.BackColor = Color.Red;
                this.label36.BackColor = Color.Red;
            }
            else
            {
                this.label34.BackColor = Color.Lime;
                this.label35.BackColor = Color.Lime;
                this.label36.BackColor = Color.Lime;
            }
            string dyj_online = dtDeviceType.Rows[0]["dyj_online"].ToString();
            if (dyj_online == "0" || "0".Equals(dyj_online))
            {
                this.label38.BackColor = Color.Red;
                this.label39.BackColor = Color.Red;
                this.label40.BackColor = Color.Red;
            }
            else
            {
                this.label38.BackColor = Color.Lime;
                this.label39.BackColor = Color.Lime;
                this.label40.BackColor = Color.Lime;
            }
            string xcg_online = dtDeviceType.Rows[0]["xcg_online"].ToString();
            if (xcg_online == "0" || "0".Equals(xcg_online))
            {
                this.label10.BackColor = Color.Red;
                this.label11.BackColor = Color.Red;
                this.label12.BackColor = Color.Red;
            }
            else
            {
                this.label10.BackColor = Color.Lime;
                this.label11.BackColor = Color.Lime;
                this.label12.BackColor = Color.Lime;
            }
            string sh_online = dtDeviceType.Rows[0]["sh_online"].ToString();
            if (sh_online == "0" || "0".Equals(sh_online))
            {
                this.label1.BackColor = Color.Red;
                this.label2.BackColor = Color.Red;
                this.label3.BackColor = Color.Red;
            }
            else
            {
                this.label1.BackColor = Color.Lime;
                this.label2.BackColor = Color.Lime;
                this.label3.BackColor = Color.Lime;
            }
            string ncg_online = dtDeviceType.Rows[0]["ncg_online"].ToString();
            if (ncg_online == "0" || "0".Equals(ncg_online))
            {
                this.label6.BackColor = Color.Red;
                this.label7.BackColor = Color.Red;
                this.label8.BackColor = Color.Red;
            }
            else
            {
                this.label6.BackColor = Color.Lime;
                this.label7.BackColor = Color.Lime;
                this.label8.BackColor = Color.Lime;
            }
            string xdt_online = dtDeviceType.Rows[0]["xdt_online"].ToString();
            if (xdt_online == "0" || "0".Equals(xdt_online))
            {
                this.label18.BackColor = Color.Red;
                this.label19.BackColor = Color.Red;
                this.label20.BackColor = Color.Red;
            }
            else
            {
                this.label18.BackColor = Color.Lime;
                this.label19.BackColor = Color.Lime;
                this.label20.BackColor = Color.Lime;
            }
            string sgtz_online = dtDeviceType.Rows[0]["sgtz_online"].ToString();
            if (sgtz_online == "0" || "0".Equals(sgtz_online))
            {
                this.label22.BackColor = Color.Red;
                this.label23.BackColor = Color.Red;
                this.label24.BackColor = Color.Red;
            }
            else
            {
                this.label22.BackColor = Color.Lime;
                this.label23.BackColor = Color.Lime;
                this.label24.BackColor = Color.Lime;
            }
            string xy_online = dtDeviceType.Rows[0]["xy_online"].ToString();
            if (xy_online == "0" || "0".Equals(xy_online))
            {
                this.label22.BackColor = Color.Red;
                this.label23.BackColor = Color.Red;
                this.label24.BackColor = Color.Red;
            }
            else
            {
                this.label22.BackColor = Color.Lime;
                this.label23.BackColor = Color.Lime;
                this.label24.BackColor = Color.Lime;
            }
            string bc_online = dtDeviceType.Rows[0]["bc_online"].ToString();
            if (bc_online == "0" || "0".Equals(bc_online))
            {
                this.label14.BackColor = Color.Red;
                this.label15.BackColor = Color.Red;
                this.label16.BackColor = Color.Red;
            }
            else
            {
                this.label14.BackColor = Color.Lime;
                this.label15.BackColor = Color.Lime;
                this.label16.BackColor = Color.Lime;
            }
        }
    }
}
