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
    public partial class updateXuechanggui : Form
    {
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        bool flag = false;
        tjcheckDao tjdao = new tjcheckDao();
        DataTable dttv=null;
        public updateXuechanggui()
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
            DataTable dtbichao = tjdao.selectXuechangguiInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                flag = true;
                string wbc = dtbichao.Rows[0]["WBC"].ToString();
                if (wbc != "" && wbc != "*")
                {
                    double wbcdouble = double.Parse(wbc);
                    DataRow[] drwbc = dttv.Select("type='WBC'");
                    double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
                    double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
                    if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
                    {
                        this.textBox5.ForeColor = Color.Blue;
                    }
                    //double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                    //double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                    //if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                    //{
                    //    this.textBox5.ForeColor = Color.Red;
                    //}
                }
               
                this.textBox5.Text = wbc;
                string rbc = dtbichao.Rows[0]["RBC"].ToString();
                if (rbc != "" && rbc != "*")
                {
                    double rbcdouble = double.Parse(rbc);
                    DataRow[] drrbc = dttv.Select("type='RBC'");
                    double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
                    double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
                    if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
                    {
                        this.textBox6.ForeColor = Color.Blue;
                    }
                    //double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                    //double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                    //if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                    //{
                    //    this.textBox6.ForeColor = Color.Red;
                    //}
                }
                
                this.textBox6.Text = rbc;
                string pct = dtbichao.Rows[0]["PCT"].ToString();
                if (pct != "" && pct != "*")
                {
                    double pctdouble = double.Parse(pct);
                    DataRow[] drpct = dttv.Select("type='PCT'");
                    double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
                    double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
                    if (pctdouble > pctwmax || pctdouble < pctwmin)
                    {
                        this.textBox8.ForeColor = Color.Blue;
                    }
                    //double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                    //double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                    //if (pctdouble > pcttmax || pctdouble < pcttmin)
                    //{
                    //    this.textBox8.ForeColor = Color.Red;
                    //}
                }
                this.textBox8.Text = pct;
                string plt = dtbichao.Rows[0]["PLT"].ToString();
                if (plt != "" && plt != "*")
                {
                    double pltdouble = double.Parse(plt);
                    DataRow[] drplt = dttv.Select("type='PLT'");
                    double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
                    double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
                    if (pltdouble > pltwmax || pltdouble < pltwmin)
                    {
                        this.textBox7.ForeColor = Color.Blue;
                    }
                    //double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                    //double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                    //if (pltdouble > plttmax || pltdouble < plttmin)
                    //{
                    //    this.textBox7.ForeColor = Color.Red;
                    //}
                }
                this.textBox7.Text = plt;
                string hgb = dtbichao.Rows[0]["HGB"].ToString();
                if (hgb != "" && hgb != "*")
                {
                    double hgbdouble = double.Parse(hgb);
                    DataRow[] drhgb = dttv.Select("type='HGB'");
                    double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
                    double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
                    if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
                    {
                        this.textBox11.ForeColor = Color.Blue;
                    }
                    //double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                    //double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                    //if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                    //{
                    //    this.textBox11.ForeColor = Color.Red;
                    //}
                }
                this.textBox11.Text = hgb;
                string hct = dtbichao.Rows[0]["HCT"].ToString();
                if (hct != "" && hct != "*")
                {
                    double hctdouble = double.Parse(hct);
                    DataRow[] drhct = dttv.Select("type='HCT'");
                    double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
                    double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
                    if (hctdouble > hctwmax || hctdouble < hctwmin)
                    {
                        this.textBox10.ForeColor = Color.Blue;
                    }
                    //double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                    //double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                    //if (hctdouble > hcttmax || hctdouble < hcttmin)
                    //{
                    //    this.textBox10.ForeColor = Color.Red;
                    //}
                }
                this.textBox10.Text = hct;
                string mcv = dtbichao.Rows[0]["MCV"].ToString();
                if (mcv != "" && mcv != "*")
                {
                    double mcvdouble = double.Parse(mcv);
                    DataRow[] drmcv = dttv.Select("type='MCV'");
                    double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
                    double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
                    if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
                    {
                        this.textBox13.ForeColor = Color.Blue;
                    }
                    //double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                    //double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                    //if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                    //{
                    //    this.textBox13.ForeColor = Color.Red;
                    //}
                }
                this.textBox13.Text = mcv;
                string mch = dtbichao.Rows[0]["MCH"].ToString();
                if (mch != "" && mch != "*")
                {
                    double mchdouble = double.Parse(mch);
                    DataRow[] drmch = dttv.Select("type='MCH'");
                    double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
                    double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
                    if (mchdouble > mchwmax || mchdouble < mchwmin)
                    {
                        this.textBox12.ForeColor = Color.Blue;
                    }
                    //double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                    //double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                    //if (mchdouble > mchtmax || mchdouble < mchtmin)
                    //{
                    //    this.textBox12.ForeColor = Color.Red;
                    //}
                }
                this.textBox12.Text = mch;

                this.textBox15.Text = dtbichao.Rows[0]["MCHC"].ToString();
                this.textBox14.Text = dtbichao.Rows[0]["RDWCV"].ToString();
                this.textBox17.Text = dtbichao.Rows[0]["RDWSD"].ToString();
                this.textBox16.Text = dtbichao.Rows[0]["MONO"].ToString();
                this.textBox19.Text = dtbichao.Rows[0]["MONOP"].ToString();
                this.textBox18.Text = dtbichao.Rows[0]["GRAN"].ToString();
                this.textBox21.Text = dtbichao.Rows[0]["GRANP"].ToString();
                this.textBox20.Text = dtbichao.Rows[0]["NEUT"].ToString();
                this.textBox23.Text = dtbichao.Rows[0]["NEUTP"].ToString();
                this.textBox22.Text = dtbichao.Rows[0]["EO"].ToString();
                this.textBox25.Text = dtbichao.Rows[0]["EOP"].ToString();
                this.textBox24.Text = dtbichao.Rows[0]["BASO"].ToString();
                this.textBox29.Text = dtbichao.Rows[0]["BASOP"].ToString();
                this.textBox28.Text = dtbichao.Rows[0]["LYM"].ToString();
                this.textBox31.Text = dtbichao.Rows[0]["LYMP"].ToString();
                this.textBox30.Text = dtbichao.Rows[0]["MPV"].ToString();
                this.textBox33.Text = dtbichao.Rows[0]["PDW"].ToString();
                this.textBox32.Text = dtbichao.Rows[0]["MXD"].ToString();
                this.textBox35.Text = dtbichao.Rows[0]["MXDP"].ToString();
                this.textBox34.Text = dtbichao.Rows[0]["PLCR"].ToString();
                this.textBox36.Text = dtbichao.Rows[0]["OTHERS"].ToString();
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (flag) {
                string WBC= this.textBox5.Text;
                string RBC =  this.textBox6.Text;
                string PCT = this.textBox8.Text;
                string PLT = this.textBox7.Text;
                string HGB = this.textBox11.Text;
                string HCT = this.textBox10.Text;
                string MCV = this.textBox13.Text;
                string MCH = this.textBox12.Text;
                string MCHC = this.textBox15.Text;
                string RDWCV = this.textBox14.Text;
                string RDWSD = this.textBox17.Text;
                string MONO = this.textBox16.Text;
                string MONOP = this.textBox19.Text;
                string GRAN = this.textBox18.Text;
                string GRANP = this.textBox21.Text;
                string NEUT = this.textBox20.Text;
                string NEUTP = this.textBox23.Text;
                string EO = this.textBox22.Text;
                string EOP = this.textBox25.Text;
                string BASO = this.textBox24.Text;
                string BASOP = this.textBox29.Text;
                string LYM = this.textBox28.Text ;
                string LYMP = this.textBox31.Text ;
                string MPV = this.textBox30.Text;
                string PDW = this.textBox33.Text;
                string MXD = this.textBox32.Text;
                string MXDP = this.textBox35.Text;
                string PLCR = this.textBox34.Text;
                string OTHERS = this.textBox36.Text;
                bool istrue= tjdao.updateXuechangguiInfo(aichive_no, bar_code, WBC, RBC, PCT, PLT, HGB, HCT, MCV, MCH, MCHC, RDWCV, RDWSD, MONO, MONOP, GRAN, GRANP, NEUT, NEUTP, EO, EOP, BASO, BASOP, LYM, LYMP, MPV, PDW, MXD, MXDP, PLCR, OTHERS);
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
