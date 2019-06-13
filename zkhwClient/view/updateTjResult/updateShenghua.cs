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
        public DataTable dttv = null;
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
                string alt = dtbichao.Rows[0]["ALT"].ToString();
                if (alt != "" && alt != "*")
                {
                    double altdouble = double.Parse(alt);
                    DataRow[] dralt = dttv.Select("type='ALT'");
                    double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
                    double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
                    if (altdouble > altwmax || altdouble < altwmin)
                    {
                        this.textBox5.ForeColor = Color.Blue;
                    }
                    double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
                    double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
                    if (altdouble > alttmax || altdouble < alttmin)
                    {
                        this.textBox5.ForeColor = Color.Red;
                    }
                }
                this.textBox5.Text = alt;
                string ast = dtbichao.Rows[0]["AST"].ToString();
                if (ast != "" && ast != "*")
                {
                    double astdouble = double.Parse(ast);
                    DataRow[] drast = dttv.Select("type='AST'");
                    double astwmin = double.Parse(drast[0]["warning_min"].ToString());
                    double astwmax = double.Parse(drast[0]["warning_max"].ToString());
                    if (astdouble > astwmax || astdouble < astwmin)
                    {
                        this.textBox6.ForeColor = Color.Blue;
                    }
                    double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
                    double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
                    if (astdouble > asttmax || astdouble < asttmin)
                    {
                        this.textBox6.ForeColor = Color.Red;
                    }
                }
                this.textBox6.Text = ast;
                string tbil = dtbichao.Rows[0]["TBIL"].ToString();
                if (tbil != "" && tbil != "*")
                {
                    double tbildouble = double.Parse(tbil);
                    DataRow[] drtbil = dttv.Select("type='TBIL'");
                    double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
                    double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
                    if (tbildouble > tbilwmax || tbildouble < tbilwmin)
                    {
                        this.textBox8.ForeColor = Color.Blue;
                    }
                    double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
                    double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
                    if (tbildouble > tbiltmax || tbildouble < tbiltmin)
                    {
                        this.textBox8.ForeColor = Color.Red;
                    }
                }
                this.textBox8.Text = tbil;
                this.textBox7.Text = dtbichao.Rows[0]["DBIL"].ToString();
                string crea = dtbichao.Rows[0]["CREA"].ToString();
                if (crea != "" && crea != "*")
                {
                    double creadouble = double.Parse(crea);
                    DataRow[] drcrea = dttv.Select("type='CREA'");
                    double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
                    double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
                    if (creadouble > creawmax || creadouble < creawmin)
                    {
                        this.textBox11.ForeColor = Color.Blue;
                    }
                    double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
                    double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
                    if (creadouble > creatmax || creadouble < creatmin)
                    {
                        this.textBox11.ForeColor = Color.Red;
                    }
                }
                this.textBox11.Text = crea;
                string urea = dtbichao.Rows[0]["UREA"].ToString();
                if (urea != "" && urea != "*")
                {
                    double ureadouble = double.Parse(urea);
                    DataRow[] drurea = dttv.Select("type='UREA'");
                    double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
                    double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
                    if (ureadouble > ureawmax || ureadouble < ureawmin)
                    {
                        this.textBox10.ForeColor = Color.Blue;
                    }
                    double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
                    double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
                    if (ureadouble > ureatmax || ureadouble < ureatmin)
                    {
                        this.textBox10.ForeColor = Color.Red;
                    }
                }
                this.textBox10.Text = urea;
                string glu = dtbichao.Rows[0]["GLU"].ToString();
                if (glu != "" && glu != "*")
                {
                    double gludouble = double.Parse(glu);
                    DataRow[] drglu = dttv.Select("type='GLU'");
                    double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
                    double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
                    if (gludouble > gluwmax || gludouble < gluwmin)
                    {
                        this.textBox13.ForeColor = Color.Blue;
                    }
                    double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
                    double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
                    if (gludouble > glutmax || gludouble < glutmin)
                    {
                        this.textBox13.ForeColor = Color.Red;
                    }
                }
                this.textBox13.Text = glu;
                string tg = dtbichao.Rows[0]["TG"].ToString();
                if (tg != "" && tg != "*")
                {
                    double tgdouble = double.Parse(tg);
                    DataRow[] drtg = dttv.Select("type='TG'");
                    double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
                    double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
                    if (tgdouble > tgwmax || tgdouble < tgwmin)
                    {
                        this.textBox12.ForeColor = Color.Blue;
                    }
                    double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
                    double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
                    if (tgdouble > tgtmax || tgdouble < tgtmin)
                    {
                        this.textBox12.ForeColor = Color.Red;
                    }
                }
                this.textBox12.Text = tg;
                string cho = dtbichao.Rows[0]["CHO"].ToString();
                if (cho != "" && cho != "*")
                {
                    double chodouble = double.Parse(cho);
                    DataRow[] drcho = dttv.Select("type='CHO'");
                    double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
                    double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
                    if (chodouble > chowmax || chodouble < chowmin)
                    {
                        this.textBox15.ForeColor = Color.Blue;
                    }
                    double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
                    double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
                    if (chodouble > chotmax || chodouble < chotmin)
                    {
                        this.textBox15.ForeColor = Color.Red;
                    }
                }
                this.textBox15.Text = cho;
                string hdlc = dtbichao.Rows[0]["HDLC"].ToString();
                if (hdlc != "" && hdlc != "*")
                {
                    double hdlcdouble = double.Parse(hdlc);
                    DataRow[] drhdlc = dttv.Select("type='HDLC'");
                    double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
                    double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
                    if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
                    {
                        this.textBox14.ForeColor = Color.Blue;
                    }
                    double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
                    double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
                    if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
                    {
                        this.textBox14.ForeColor = Color.Red;
                    }
                }
                this.textBox14.Text = hdlc;
                string ldlc = dtbichao.Rows[0]["LDLC"].ToString();
                if (ldlc != "" && ldlc != "*")
                {
                    double ldlcdouble = double.Parse(ldlc);
                    DataRow[] drldlc = dttv.Select("type='LDLC'");
                    double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
                    double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
                    if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
                    {
                        this.textBox17.ForeColor = Color.Blue;
                    }
                    double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
                    double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
                    if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
                    {
                        this.textBox17.ForeColor = Color.Red;
                    }
                }
                this.textBox17.Text = ldlc;

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
            //if (flag) {
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
                bool istrue= tjdao.updateShenghuaInfo(aichive_no, id_number,bar_code, ALT, AST, TBIL, DBIL, CREA, UREA, GLU, TG, CHO, HDLC, LDLC, ALB, UA, HCY, AFP, CEA, Ka, Na, TP, ALP, GGT, CHE, TBA, APOA1, APOB, CK, CKMB, LDHL, HBDH, aAMY);
                if (istrue)
                {
                    tjdao.updateTJbgdcShenghua(aichive_no, bar_code, 1);
                    tjdao.updatePEShInfo(aichive_no, bar_code, CHO, TG, LDLC, HDLC, GLU, ALT, AST, ALB, TBIL, DBIL, CREA, UREA);
                    MessageBox.Show("数据保存成功!");
                }
                else {
                    MessageBox.Show("数据保存失败!");
                }
            //}
        }

    }
}
