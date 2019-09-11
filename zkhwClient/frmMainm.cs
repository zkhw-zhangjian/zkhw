using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient
{
    public partial class frmMainm : Form
    {
        public frmMainm()
        {
            InitializeComponent();
        }

        private void startlabel()
        {
            label31.Text = "严重精神病        \r\n障碍患者服务      ";
            label26.Text = "老年人生活        \r\n自理能力评估     ";

            pangw.Dock = DockStyle.Fill;
            panel17.Controls.Clear();
            panel17.Controls.Add(pangw);
        }
        private void frmMainm_Load(object sender, EventArgs e)
        {
            startlabel();
        }
        #region 底部处理 
        private void label1_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = Color.FromArgb(11, 26, 83);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = Color.FromArgb(37, 55, 129);
        }
        #endregion

        #region 顶部色块处理
        private void label2_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = Color.FromArgb(118, 118, 118);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string stag = lbl.Tag.ToString();
            if (stag == "0")
            {
                lbl.BackColor = Color.FromArgb(67, 67, 67);
            }
        }
        private void setlabeltagfortop()
        {
            label2.Tag = 0;
            label13.Tag = 0;
            label12.Tag = 0;
            label15.Tag = 0;
            label16.Tag = 0;
            label17.Tag = 0;

            label2.BackColor = Color.FromArgb(67, 67, 67);
            label12.BackColor = Color.FromArgb(67, 67, 67);
        }
        private void label2_Click(object sender, EventArgs e)
        {
            setlabeltagfortop();
            Label lbl = (Label)sender;
            lbl.BackColor = Color.FromArgb(118, 118, 118);
            if (lbl.Text == "公共卫生" || lbl.Text == "设置")
            {
                lbl.Tag = 1;
                if (lbl.Text == "公共卫生")
                {
                    setlabeltaggongweiforleft(1);
                    pangw.Dock = DockStyle.Fill;
                    panel17.Controls.Clear();
                    panel17.Controls.Add(pangw);
                }
                else if (lbl.Text == "设置")
                {
                    setlabeltagshezhiforleft(1);
                    pansetup.Dock = DockStyle.Fill;
                    panel17.Controls.Clear();
                    panel17.Controls.Add(pansetup);
                }
            }
        }
        #endregion

        #region 左侧设置
        private Color leftselectColor= Color.FromArgb(65, 120, 19);
        private Color leftColor= Color.FromArgb(103, 168, 50);
        private void setlabeltagshezhiforleft(int itype)
        {
            if(itype==0)
            {
                label49.Tag = "0,0";
                label49.BackColor = leftColor;
            }
            else
            {
                label49.Tag = "1,0";
                label49.BackColor = leftselectColor;
            } 
            label48.Tag = "0,1";
            label48.BackColor = leftColor;

            label47.Tag = "0,2";
            label47.BackColor = leftColor;

            label46.Tag = "0,3";
            label46.BackColor = leftColor;

            label45.Tag = "0,4";
            label45.BackColor = leftColor;

            label43.Tag = "0,5";
            label43.BackColor = leftColor;
        }
        private void label49_Click(object sender, EventArgs e)
        {  
            setlabeltagshezhiforleft(0);
            Label lbl = (Label)sender;
            string a = lbl.Tag.ToString();
            string[] b = a.Split(',');
            lbl.Tag = "1," + b[1].ToString();
            lbl.BackColor = leftselectColor;
        }

        private void label49_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = leftselectColor;
        }

        private void label49_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string a = lbl.Tag.ToString();
            string[] b = a.Split(',');
            if (b[0].ToString() == "0")
            {
                lbl.BackColor = leftColor;
            }
        }
        #endregion

        #region 左侧公卫
        private void label20_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = leftselectColor;
        }

        private void label20_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string a = lbl.Tag.ToString();
            string[] b = a.Split(',');
            if (b[0].ToString() == "0")
            {
                lbl.BackColor = leftColor;
            }
        }
        private void setlabeltaggongweiforleft(int itype)
        {
            if(itype==0)
            {
                label20.Tag = "0,0";
                label20.BackColor = leftColor;
            }
            else
            {
                label20.Tag = "1,0";
                label20.BackColor = leftselectColor;
            }

            label21.Tag = "0,1";
            label21.BackColor = leftColor;

            label23.Tag = "0,2";
            label23.BackColor = leftColor;

            label24.Tag = "0,3";
            label24.BackColor = leftColor;

            label25.Tag = "0,4";
            label25.BackColor = leftColor;

            label26.Tag = "0,5";
            label26.BackColor = leftColor;

            label27.Tag = "0,6";
            label27.BackColor = leftColor;

            label28.Tag = "0,7";
            label28.BackColor = leftColor;

            label29.Tag = "0,8";
            label29.BackColor = leftColor;

            label30.Tag = "0,9";
            label30.BackColor = leftColor;

            label31.Tag = "0,10";
            label31.BackColor = leftColor;

            label32.Tag = "0,11";
            label32.BackColor = leftColor;

            label33.Tag = "0,12";
            label33.BackColor = leftColor;

            label22.Tag = "0,13";
            label22.BackColor = leftColor;

            label34.Tag = "0,14";
            label34.BackColor = leftColor;
        }
        private void label20_Click(object sender, EventArgs e)
        { 
            setlabeltaggongweiforleft(0);
            Label lbl = (Label)sender;
            string a = lbl.Tag.ToString();
            string[] b = a.Split(',');
            lbl.Tag = "1," + b[1].ToString();
            lbl.BackColor = leftselectColor;
        }
        #endregion
    }
}
