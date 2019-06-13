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
        public DataTable dttv=null;
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
                    double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
                    double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
                    if (wbcdouble > wbctmax || wbcdouble < wbctmin)
                    {
                        this.textBox5.ForeColor = Color.Red;
                    }
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
                    double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
                    double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
                    if (rbcdouble > rbctmax || rbcdouble < rbctmin)
                    {
                        this.textBox6.ForeColor = Color.Red;
                    }
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
                    double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
                    double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
                    if (pctdouble > pcttmax || pctdouble < pcttmin)
                    {
                        this.textBox8.ForeColor = Color.Red;
                    }
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
                    double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
                    double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
                    if (pltdouble > plttmax || pltdouble < plttmin)
                    {
                        this.textBox7.ForeColor = Color.Red;
                    }
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
                    double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
                    double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
                    if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
                    {
                        this.textBox11.ForeColor = Color.Red;
                    }
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
                    double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
                    double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
                    if (hctdouble > hcttmax || hctdouble < hcttmin)
                    {
                        this.textBox10.ForeColor = Color.Red;
                    }
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
                    double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
                    double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
                    if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
                    {
                        this.textBox13.ForeColor = Color.Red;
                    }
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
                    double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
                    double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
                    if (mchdouble > mchtmax || mchdouble < mchtmin)
                    {
                        this.textBox12.ForeColor = Color.Red;
                    }
                }
                this.textBox12.Text = mch;

                string mchc = dtbichao.Rows[0]["MCHC"].ToString();
                if (mchc != "" && mchc != "*")
                {
                    double mchcdouble = double.Parse(mchc);
                    DataRow[] drmchc = dttv.Select("type='MCHC'");
                    double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
                    double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
                    if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
                    {
                        this.textBox15.ForeColor = Color.Blue;
                    }
                    double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
                    double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
                    if (mchcdouble > mchctmax || mchcdouble < mchctmin)
                    {
                        this.textBox15.ForeColor = Color.Red;
                    }
                }
                this.textBox15.Text = mchc;

                string rdwcv = dtbichao.Rows[0]["RDWCV"].ToString();
                if (rdwcv != "" && rdwcv != "*")
                {
                    double rdwcvdouble = double.Parse(rdwcv);
                    DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
                    double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
                    double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
                    if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
                    {
                        this.textBox14.ForeColor = Color.Blue;
                    }
                    double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
                    double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
                    if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
                    {
                        this.textBox14.ForeColor = Color.Red;
                    }
                }
                this.textBox14.Text = rdwcv;

                string rdwsd = dtbichao.Rows[0]["RDWSD"].ToString();
                if (rdwsd != "" && rdwsd != "*")
                {
                    double rdwsddouble = double.Parse(rdwsd);
                    DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
                    double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
                    double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
                    if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
                    {
                        this.textBox17.ForeColor = Color.Blue;
                    }
                    double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
                    double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
                    if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
                    {
                        this.textBox17.ForeColor = Color.Red;
                    }
                }
                this.textBox17.Text = rdwsd;

                this.textBox16.Text = dtbichao.Rows[0]["MONO"].ToString();
                this.textBox19.Text = dtbichao.Rows[0]["MONOP"].ToString();
                this.textBox18.Text = dtbichao.Rows[0]["GRAN"].ToString();
                this.textBox21.Text = dtbichao.Rows[0]["GRANP"].ToString();

                string neut = dtbichao.Rows[0]["NEUT"].ToString();
                if (neut != "" && neut != "*")
                {
                    double neutdouble = double.Parse(neut);
                    DataRow[] drneut = dttv.Select("type='NEUT'");
                    double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                    double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                    if (neutdouble > neutwmax || neutdouble < neutwmin)
                    {
                        this.textBox20.ForeColor = Color.Blue;
                    }
                    double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                    double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                    if (neutdouble > neuttmax || neutdouble < neuttmin)
                    {
                        this.textBox20.ForeColor = Color.Red;
                    }
                }
                this.textBox20.Text = neut;

                string neutp = dtbichao.Rows[0]["NEUTP"].ToString();
                if (neutp != "" && neutp != "*")
                {
                    double neutpdouble = double.Parse(neutp);
                    DataRow[] drneutp = dttv.Select("type='NEUTP'");
                    double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
                    double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
                    if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
                    {
                        this.textBox23.ForeColor = Color.Blue;
                    }
                    double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
                    double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
                    if (neutpdouble > neutptmax || neutpdouble < neutptmin)
                    {
                        this.textBox23.ForeColor = Color.Red;
                    }
                }
                this.textBox23.Text = neutp;

                this.textBox22.Text = dtbichao.Rows[0]["EO"].ToString();
                this.textBox25.Text = dtbichao.Rows[0]["EOP"].ToString();
                this.textBox24.Text = dtbichao.Rows[0]["BASO"].ToString();
                this.textBox29.Text = dtbichao.Rows[0]["BASOP"].ToString();

                string lym = dtbichao.Rows[0]["LYM"].ToString();
                if (lym != "" && lym != "*")
                {
                    double lymdouble = double.Parse(lym);
                    DataRow[] drlym = dttv.Select("type='LYM'");
                    double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
                    double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
                    if (lymdouble > lymwmax || lymdouble < lymwmin)
                    {
                        this.textBox28.ForeColor = Color.Blue;
                    }
                    double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
                    double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
                    if (lymdouble > lymtmax || lymdouble < lymtmin)
                    {
                        this.textBox28.ForeColor = Color.Red;
                    }
                }
                this.textBox28.Text = lym;

                string lymp = dtbichao.Rows[0]["LYMP"].ToString();
                if (lymp != "" && lymp != "*")
                {
                    double lympdouble = double.Parse(lymp);
                    DataRow[] drlymp = dttv.Select("type='LYMP'");
                    double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
                    double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
                    if (lympdouble > lympwmax || lympdouble < lympwmin)
                    {
                        this.textBox31.ForeColor = Color.Blue;
                    }
                    double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
                    double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
                    if (lympdouble > lymptmax || lympdouble < lymptmin)
                    {
                        this.textBox31.ForeColor = Color.Red;
                    }
                }
                this.textBox31.Text = lymp;

                string mpv = dtbichao.Rows[0]["MPV"].ToString();
                if (mpv != "" && mpv != "*")
                {
                    double mpvdouble = double.Parse(mpv);
                    DataRow[] drmpv = dttv.Select("type='MPV'");
                    double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
                    double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
                    if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
                    {
                        this.textBox30.ForeColor = Color.Blue;
                    }
                    double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
                    double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
                    if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
                    {
                        this.textBox30.ForeColor = Color.Red;
                    }
                }
                this.textBox30.Text = mpv;

                string pdw = dtbichao.Rows[0]["PDW"].ToString();
                if (pdw != "" && pdw != "*")
                {
                    double pdwdouble = double.Parse(pdw);
                    DataRow[] drpdw = dttv.Select("type='PDW'");
                    double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
                    double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
                    if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
                    {
                        this.textBox33.ForeColor = Color.Blue;
                    }
                    double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
                    double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
                    if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
                    {
                        this.textBox33.ForeColor = Color.Red;
                    }
                }
                this.textBox33.Text = pdw;

                string mxd = dtbichao.Rows[0]["MXD"].ToString();
                if (mxd != "" && mxd != "*")
                {
                    double mxddouble = double.Parse(mxd);
                    DataRow[] drmxd = dttv.Select("type='MXD'");
                    double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
                    double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
                    if (mxddouble > mxdwmax || mxddouble < mxdwmin)
                    {
                        this.textBox32.ForeColor = Color.Blue;
                    }
                    double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
                    double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
                    if (mxddouble > mxdtmax || mxddouble < mxdtmin)
                    {
                        this.textBox32.ForeColor = Color.Red;
                    }
                }
                this.textBox32.Text = mxd;

                string mxdp = dtbichao.Rows[0]["MXDP"].ToString();
                if (mxdp != "" && mxdp != "*")
                {
                    double mxdpdouble = double.Parse(mxdp);
                    DataRow[] drmxdp = dttv.Select("type='MXDP'");
                    double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
                    double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
                    if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
                    {
                        this.textBox35.ForeColor = Color.Blue;
                    }
                    double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
                    double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
                    if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
                    {
                        this.textBox35.ForeColor = Color.Red;
                    }
                }
                this.textBox35.Text = mxdp;
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
            //if (flag) {
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
                    tjdao.updateTJbgdcXuechanggui(aichive_no, bar_code, 1);
                    tjdao.updatePEXcgInfo(aichive_no, bar_code, HGB, WBC, PLT);
                    MessageBox.Show("数据保存成功!");
                }
                else {
                    MessageBox.Show("数据保存失败!");
                }
            //}
        }

    }
}
