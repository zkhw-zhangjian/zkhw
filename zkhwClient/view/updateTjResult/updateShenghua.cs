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
    public partial class updateShenghua : Form
    {
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        bool flag = false;
        tjcheckDao tjdao = new tjcheckDao();
        public updateShenghua()
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
            DataTable dtbichao = tjdao.selectShenghuaInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                flag = true;
                this.textBox5.Text = dtbichao.Rows[0]["ALT"].ToString();
                this.textBox6.Text = dtbichao.Rows[0]["AST"].ToString();
                this.textBox8.Text = dtbichao.Rows[0]["TBIL"].ToString();
                this.textBox7.Text = dtbichao.Rows[0]["DBIL"].ToString();
                this.textBox11.Text = dtbichao.Rows[0]["CREA"].ToString();
                this.textBox10.Text = dtbichao.Rows[0]["UREA"].ToString();
                this.textBox13.Text = dtbichao.Rows[0]["GLU"].ToString();
                this.textBox12.Text = dtbichao.Rows[0]["TG"].ToString();
                this.textBox15.Text = dtbichao.Rows[0]["CHO"].ToString();
                this.textBox14.Text = dtbichao.Rows[0]["HDLC"].ToString();
                this.textBox17.Text = dtbichao.Rows[0]["LDLC"].ToString();
                this.textBox16.Text = dtbichao.Rows[0]["ALB"].ToString();
                this.textBox19.Text = dtbichao.Rows[0]["UA"].ToString();
                this.textBox18.Text = dtbichao.Rows[0]["HCY"].ToString();
                this.textBox21.Text = dtbichao.Rows[0]["AFP"].ToString();
                this.textBox20.Text = dtbichao.Rows[0]["CEA"].ToString();
                this.textBox23.Text = dtbichao.Rows[0]["Ka"].ToString();
                this.textBox22.Text = dtbichao.Rows[0]["Na"].ToString();
                this.textBox25.Text = dtbichao.Rows[0]["TP"].ToString();
                this.textBox24.Text = dtbichao.Rows[0]["ALP"].ToString();
                this.textBox27.Text = dtbichao.Rows[0]["GGT"].ToString();
                this.textBox26.Text = dtbichao.Rows[0]["CHE"].ToString();
                this.textBox29.Text = dtbichao.Rows[0]["TBA"].ToString();
                this.textBox28.Text = dtbichao.Rows[0]["APOA1"].ToString();
                this.textBox31.Text = dtbichao.Rows[0]["APOB"].ToString();
                this.textBox30.Text = dtbichao.Rows[0]["CK"].ToString();
                this.textBox33.Text = dtbichao.Rows[0]["CKMB"].ToString();
                this.textBox32.Text = dtbichao.Rows[0]["LDHL"].ToString();
                this.textBox35.Text = dtbichao.Rows[0]["HBDH"].ToString();
                this.textBox34.Text = dtbichao.Rows[0]["aAMY"].ToString();
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (flag) {
                string ALT= this.textBox5.Text;
                string AST = this.textBox6.Text;
                string TBIL = this.textBox7.Text;
                string DBIL = this.textBox8.Text;
                string CREA = this.textBox11.Text;
                string UREA = this.textBox10.Text;
                string GLU = this.textBox13.Text;
                string TG = this.textBox12.Text;
                string CHO = this.textBox15.Text;
                string HDLC = this.textBox14.Text;
                string LDLC = this.textBox17.Text;
                string ALB = this.textBox16.Text;
                string UA = this.textBox19.Text;
                string HCY = this.textBox18.Text;
                string AFP = this.textBox21.Text;
                string CEA = this.textBox20.Text;
                string Ka = this.textBox23.Text;
                string Na = this.textBox22.Text;
                string TP = this.textBox25.Text;
                string ALP = this.textBox24.Text;
                string GGT = this.textBox27.Text;
                string CHE = this.textBox26.Text;
                string TBA = this.textBox29.Text;
                string APOA1 = this.textBox28.Text;
                string APOB = this.textBox31.Text;
                string CK = this.textBox30.Text;
                string CKMB = this.textBox33.Text;
                string LDHL = this.textBox32.Text;
                string HBDH = this.textBox35.Text;
                string aAMY = this.textBox34.Text;
                bool istrue= tjdao.updateShenghuaInfo(aichive_no, bar_code, ALT, AST, TBIL, DBIL, CREA, UREA, GLU, TG, CHO, HDLC, LDLC, ALB, UA, HCY, AFP, CEA, Ka, Na, TP, ALP, GGT, CHE, TBA, APOA1, APOB, CK, CKMB, LDHL, HBDH, aAMY);
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
