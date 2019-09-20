using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view.setting
{
    public partial class frmEmpower : Form
    {
        public int _EditType = 0;
        public frmEmpower()
        {
            InitializeComponent();
        }

        private void frmEmpower_Load(object sender, EventArgs e)
        { 
            if (_EditType == 0)
            {
                txtNote.Visible = false; 

                panel1.Top = txtNote.Top;
            }
            else
            {
                txtNote.Visible = true;
                btnCancel.Visible = true; 
            }
            GetLastInfo();
        }
        private void GetLastInfo()
        {
            string fpath = Application.StartupPath + "\\sysstem.ini";
            string b=sysstem.GetINIInfo(fpath, 1);
            DateTime a = DateTime.Parse(sysstem.defaultDt);
            if(b.Length>=4)
            {
                string c = b.Substring(4);
                bool flag = sysstem.IsDate(c, out a);
                txtLast.Text = a.ToString("yyyy-MM-dd");
            }
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(txtShouQuanMa.Text.Trim()=="")
            {
                MessageBox.Show("授权码不能为空！");
                return;
            }
            //判断下授权码
            bool flag=sysstem.EmpowerValid(txtShouQuanMa.Text.Trim());
            if(flag==false)
            {
                MessageBox.Show("不是有效的授权码！");
                return;
            }
            string fpath = Application.StartupPath + "\\sysstem.ini";
            bool flag1=sysstem.EmpowerInfo(fpath, txtShouQuanMa.Text.Trim());
            if(flag1==true)
            {
                GetLastInfo();
                MessageBox.Show("授权成功！"); 
                this.DialogResult = DialogResult.OK; 
            }
            else
            {
                MessageBox.Show("授权失败！");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(170, 171, 171), Color.FromArgb(170, 171, 171));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("暂不授权", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(15, 5));

        }

        private void btnOK_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("确定授权", new System.Drawing.Font("微软雅黑", 11, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(15, 5));
        }
    }
}
