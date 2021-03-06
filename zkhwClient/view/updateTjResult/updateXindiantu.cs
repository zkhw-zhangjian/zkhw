﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq; 
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.updateTjResult
{
    public partial class updateXindiantu : Form
    {
        public int rowIndex = 0;
        public delegate void TestFunDelegate(int _result, int _colIndex, int _rowIndex);
        public TestFunDelegate testFunDelegate;
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        bool flag = false;
        tjcheckDao tjdao = new tjcheckDao();

        public string xdtPath = "";
        public updateXindiantu()
        {
            InitializeComponent();
        }
        
        private void updateBichao_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = name;
            this.textBox3.Text = time;
            this.textBox9.Text = aichive_no;
            this.textBox4.Text = id_number;
            this.textBox2.Text = bar_code;
            DataTable dtbichao = tjdao.selectXindiantuInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                flag = true;
                this.textBox5.Text = dtbichao.Rows[0]["XdtResult"].ToString();
                this.textBox6.Text = dtbichao.Rows[0]["XdtDesc"].ToString();
                this.textBox17.Text = dtbichao.Rows[0]["Ventrate"].ToString();
                this.textBox7.Text = dtbichao.Rows[0]["PR"].ToString();
                this.textBox12.Text = dtbichao.Rows[0]["QRS"].ToString();
                this.textBox8.Text = dtbichao.Rows[0]["QT"].ToString();
                this.textBox10.Text = dtbichao.Rows[0]["QTc"].ToString();
                this.textBox11.Text = dtbichao.Rows[0]["P_R_T"].ToString();
                this.textBox13.Text = dtbichao.Rows[0]["DOB"].ToString();
                this.textBox14.Text = dtbichao.Rows[0]["Age"].ToString();
                this.textBox15.Text = dtbichao.Rows[0]["Gen"].ToString();
                this.textBox16.Text = dtbichao.Rows[0]["Dep"].ToString();
                string path= "xdtImg//"+ dtbichao.Rows[0]["imageUrl"].ToString();
                if (System.IO.File.Exists(path))
                {
                    xdtPath = path;
                    pictureBox1.Image = Image.FromFile(path, false); 
                }
                string XdtDesc= dtbichao.Rows[0]["XdtDesc"].ToString();
                if (XdtDesc.IndexOf("正常") > -1)
                {
                    this.textBox6.ForeColor = Color.Black;
                }
                else
                {
                    this.textBox6.ForeColor = Color.Red;
                }
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        
        
        
        private void button5_Click(object sender, EventArgs e)
        { 
            string XdtResult= this.textBox5.Text;
            string XdtDesc = this.textBox6.Text;
            string Ventrate = this.textBox17.Text;
            string PR = this.textBox7.Text;
            string QRS = this.textBox12.Text;
            string QT = this.textBox8.Text;
            string QTc = this.textBox10.Text;
            string P_R_T = this.textBox11.Text;
            string DOB = this.textBox13.Text;
            string Age = this.textBox14.Text;
            string Gen = this.textBox15.Text;
            string Dep = this.textBox16.Text;
            string barcode=this.textBox2.Text;
            bool istrue= tjdao.updateXindiantuInfo(aichive_no, bar_code, XdtResult, XdtDesc, Ventrate, PR, QRS, QT, QTc, P_R_T, DOB, Age, Gen, Dep);
            if (istrue)
            {
                if (XdtDesc.IndexOf("正常") > -1)
                {
                    DbHelperMySQL.ExecuteSql($"update physical_examination_record set cardiogram='1' where aichive_no='"+ aichive_no + "'and bar_code= '"+ barcode + "'");
                    string istruedgbc = "update zkhw_tj_bgdc set XinDian='1' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                    DbHelperMySQL.ExecuteSql(istruedgbc);
                    this.textBox6.ForeColor = Color.Black;
                    testFunDelegate(1, 6, rowIndex);
                }
                else
                {
                    DbHelperMySQL.ExecuteSql($"update physical_examination_record set cardiogram='2',cardiogram_memo='"+XdtDesc+"' where aichive_no='"+ aichive_no + "'and bar_code= '"+ barcode + "'");
                    string issqdgbc = "update zkhw_tj_bgdc set XinDian='3' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                    DbHelperMySQL.ExecuteSql(issqdgbc);
                    this.textBox6.ForeColor = Color.Red;
                    testFunDelegate(3, 6, rowIndex);
                }
                MessageBox.Show("数据保存成功!");
            }
            else {
                MessageBox.Show("数据保存失败!");
            } 
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("保存", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(25, 5));

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            //心电
            if (xdtPath != "")
            {
                frmPic frm = new frmPic();
                Image img = Image.FromFile(Application.StartupPath  + "\\"+xdtPath);
                frm.InPutBuffer(img);
                frm.ShowDialog();
            }
        } 
    }
}
