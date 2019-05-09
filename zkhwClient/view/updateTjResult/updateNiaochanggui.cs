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
        public DataTable dttv = null;
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
                string wbc= dtbichao.Rows[0]["WBC"].ToString();
                if (!"-".Equals(wbc)) {
                    if ("+-".Equals(wbc) || "+1".Equals(wbc) || "+2".Equals(wbc) || "+3".Equals(wbc))
                    {
                        this.textBox5.ForeColor = Color.Blue;
                    }
                    else {
                        this.textBox5.ForeColor = Color.Red;
                    }
                }
                this.textBox5.Text = wbc;

                this.textBox6.Text = dtbichao.Rows[0]["LEU"].ToString();

                string nit = dtbichao.Rows[0]["NIT"].ToString();
                if (!"-".Equals(nit))
                {
                    if ("+".Equals(nit))
                    {
                        this.textBox8.ForeColor = Color.Blue;
                    }
                    else
                    {
                        this.textBox8.ForeColor = Color.Red;
                    }
                }
                this.textBox8.Text = nit;

                string uro = dtbichao.Rows[0]["URO"].ToString();
                if (!"Normal".Equals(uro))
                {
                    if ("+1".Equals(uro)|| "+2".Equals(uro) || "+3".Equals(uro))
                    {
                        this.textBox7.ForeColor = Color.Blue;
                    }
                    else
                    {
                        this.textBox7.ForeColor = Color.Red;
                    }
                }
                this.textBox7.Text = uro;

                string pro = dtbichao.Rows[0]["PRO"].ToString();
                if (!"-".Equals(pro))
                {
                    if ("+-".Equals(pro) || "+1".Equals(pro) || "+2".Equals(pro) || "+3".Equals(pro) || "+4".Equals(pro))
                    {
                        this.textBox11.ForeColor = Color.Blue;
                    }
                    else
                    {
                        this.textBox11.ForeColor = Color.Red;
                    }
                }
                this.textBox11.Text = pro;

                string ph = dtbichao.Rows[0]["PH"].ToString();
                if (ph != "")
                {
                    double phdouble = double.Parse(ph);
                    if (phdouble==5.0 || phdouble == 5.5 || phdouble == 6.0 || phdouble == 6.5 || phdouble == 7.0 || phdouble == 7.5 || phdouble == 8.0 || phdouble == 8.5 || phdouble == 9.0)
                    {
                        if (phdouble > 8.0 && phdouble <= 9.0)
                        {
                            this.textBox10.ForeColor = Color.Blue;
                        }else if(phdouble > 9.0 || phdouble < 5.0)
                        {
                            this.textBox10.ForeColor = Color.Red;
                        }
                    }
                }
                this.textBox10.Text = ph;

                string bld = dtbichao.Rows[0]["BLD"].ToString();
                if (!"-".Equals(bld))
                {
                    if ("+-".Equals(bld) || "+1".Equals(bld) || "+2".Equals(bld) || "+3".Equals(bld) )
                    {
                        this.textBox13.ForeColor = Color.Blue;
                    }
                    else
                    {
                        this.textBox13.ForeColor = Color.Red;
                    }
                }
                this.textBox13.Text = bld;

                string sg=dtbichao.Rows[0]["SG"].ToString();
                if (sg != "")
                {
                    double sgdouble = double.Parse(sg);
                    if (sgdouble == 1.005 || sgdouble == 1.01 || sgdouble == 1.015 || sgdouble == 1.02 || sgdouble == 1.025 || sgdouble == 1.03)
                    {
                        if (sgdouble > 1.025 || sgdouble < 1.015)
                        {
                            this.textBox12.ForeColor = Color.Blue;
                        }
                        else if (sgdouble < 1.005 || sgdouble > 1.03)
                        {
                            this.textBox12.ForeColor = Color.Red;
                        }
                    }
                }   
                this.textBox12.Text = sg;

                string ket = dtbichao.Rows[0]["KET"].ToString();
                if (!"-".Equals(ket))
                {
                    if ("+-".Equals(ket) || "+1".Equals(ket) || "+2".Equals(ket) || "+3".Equals(ket))
                    {
                        this.textBox15.ForeColor = Color.Blue;
                    }
                    else
                    {
                        this.textBox15.ForeColor = Color.Red;
                    }
                }
                this.textBox15.Text = ket;

                string bil = dtbichao.Rows[0]["BIL"].ToString();
                if (!"-".Equals(bil))
                {
                    if ("+1".Equals(bil) || "+2".Equals(bil) || "+3".Equals(bil))
                    {
                        this.textBox14.ForeColor = Color.Blue;
                    }
                    else
                    {
                        this.textBox14.ForeColor = Color.Red;
                    }
                }
                this.textBox14.Text = bil;

                string glu = dtbichao.Rows[0]["GLU"].ToString();
                    if (!"-".Equals(glu))
                    {
                        if ("+-".Equals(glu) || "+1".Equals(glu) || "+2".Equals(glu) || "+3".Equals(glu) || "+4".Equals(glu))
                        {
                            this.textBox17.ForeColor = Color.Blue;
                        }
                        else
                        {
                            this.textBox17.ForeColor = Color.Red;
                        }
                    }
                    this.textBox17.Text = glu;

                    string vc = dtbichao.Rows[0]["Vc"].ToString();
                if (!"-".Equals(vc))
                {
                    if ("+-".Equals(vc) || "+1".Equals(vc) || "+2".Equals(vc) || "+3".Equals(vc))
                    {
                        this.textBox16.ForeColor = Color.Blue;
                    }
                    else
                    {
                        this.textBox16.ForeColor = Color.Red;
                    }
                }
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
