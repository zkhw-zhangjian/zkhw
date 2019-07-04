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
            if(_EditType == 0)
            {
                txtNote.Visible = false; 

                panel1.Top = (groupBox1.Height - panel1.Height) / 2;
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
    }
}
