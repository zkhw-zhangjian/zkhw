using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.updateTjResult
{
    public partial class updateNiaochanggui : Form
    {
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        bool flag = false;
        tjcheckDao tjdao = new tjcheckDao();
        public updateNiaochanggui()
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
            DataTable dtbichao = tjdao.selectNiaochangguiInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                flag = true; 
                this.textBox5.Text = dtbichao.Rows[0]["WBC"].ToString();
                this.textBox6.Text = dtbichao.Rows[0]["LEU"].ToString();
                this.textBox8.Text = dtbichao.Rows[0]["NIT"].ToString();
                this.textBox7.Text = dtbichao.Rows[0]["URO"].ToString();
                this.textBox11.Text = dtbichao.Rows[0]["PRO"].ToString();
                this.textBox10.Text = dtbichao.Rows[0]["PH"].ToString();
                this.textBox13.Text = dtbichao.Rows[0]["BLD"].ToString();
                this.textBox12.Text = dtbichao.Rows[0]["SG"].ToString();
                this.textBox15.Text = dtbichao.Rows[0]["KET"].ToString();
                this.textBox14.Text = dtbichao.Rows[0]["BIL"].ToString();
                this.textBox17.Text = dtbichao.Rows[0]["GLU"].ToString();
                this.textBox16.Text = dtbichao.Rows[0]["Vc"].ToString();
                this.textBox19.Text = dtbichao.Rows[0]["MA"].ToString();
                this.textBox18.Text = dtbichao.Rows[0]["ACR"].ToString();
                this.textBox21.Text = dtbichao.Rows[0]["Ca"].ToString();
                this.textBox20.Text = dtbichao.Rows[0]["CR"].ToString();
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (flag) {
                string WBC = this.textBox5.Text;
                string LEU = this.textBox6.Text;
                string NIT = this.textBox8.Text;
                string URO = this.textBox7.Text;
                string PRO = this.textBox11.Text;
                string PH = this.textBox10.Text;
                string BLD = this.textBox13.Text;
                string SG = this.textBox12.Text;
                string KET = this.textBox15.Text;
                string BIL = this.textBox14.Text;
                string GLU = this.textBox17.Text;
                string Vc = this.textBox16.Text;
                string MA = this.textBox19.Text;
                string ACR = this.textBox18.Text;
                string Ca = this.textBox21.Text;
                string CR = this.textBox20.Text;
                bool istrue= tjdao.updateNiaochangguiInfo(aichive_no, bar_code, WBC, LEU, NIT, URO, PRO, PH, BLD, SG, KET, BIL, GLU, Vc, MA, ACR, Ca, CR);
                if (istrue)
                {
                    MessageBox.Show("数据保存成功!");
                }
                else {
                    MessageBox.Show("数据保存失败!");
                }
            }
        }

    }
}
