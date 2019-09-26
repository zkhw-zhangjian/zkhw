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
        public int rowIndex = 0;
        public delegate void TestFunDelegate(int _result, int _colIndex, int _rowIndex);
        public TestFunDelegate testFunDelegate;

        private bool _isHaveData = false;
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = ""; 
        tjcheckDao tjdao = new tjcheckDao();
        public DataTable dttv=null;

        private string _currentdevno = "XCG_YNH_001"; 
        public updateXuechanggui()
        {
            InitializeComponent();
        }

        #region 判断
        private int GetJudgeResultForWBC(double wbcdouble)
        {
            int _result = 1;
            DataRow[] drwbc = dttv.Select("type='WBC'");
            double wbcwmin = double.Parse(drwbc[0]["warning_min"].ToString());
            double wbcwmax = double.Parse(drwbc[0]["warning_max"].ToString());
            if (wbcdouble > wbcwmax || wbcdouble < wbcwmin)
            {
                this.textBox5.ForeColor = Color.Blue;
                _result = 2;
            }
            double wbctmin = double.Parse(drwbc[0]["threshold_min"].ToString());
            double wbctmax = double.Parse(drwbc[0]["threshold_max"].ToString());
            if (wbcdouble > wbctmax || wbcdouble < wbctmin)
            {
                this.textBox5.ForeColor = Color.Red;
                _result = 3;
            }
            if(_result==1)
            {
                this.textBox5.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForRBC(double rbcdouble)
        {
            int _result = 1;
            DataRow[] drrbc = dttv.Select("type='RBC'");
            double rbcwmin = double.Parse(drrbc[0]["warning_min"].ToString());
            double rbcwmax = double.Parse(drrbc[0]["warning_max"].ToString());
            if (rbcdouble > rbcwmax || rbcdouble < rbcwmin)
            {
                this.textBox6.ForeColor = Color.Blue;
                _result = 2;
            }
            double rbctmin = double.Parse(drrbc[0]["threshold_min"].ToString());
            double rbctmax = double.Parse(drrbc[0]["threshold_max"].ToString());
            if (rbcdouble > rbctmax || rbcdouble < rbctmin)
            {
                this.textBox6.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox6.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForPCT(double pctdouble)
        {
            int _result = 1;
            DataRow[] drpct = dttv.Select("type='PCT'");
            double pctwmin = double.Parse(drpct[0]["warning_min"].ToString());
            double pctwmax = double.Parse(drpct[0]["warning_max"].ToString());
            if (pctdouble > pctwmax || pctdouble < pctwmin)
            {
                this.textBox8.ForeColor = Color.Blue;
                _result = 2;
            }
            double pcttmin = double.Parse(drpct[0]["threshold_min"].ToString());
            double pcttmax = double.Parse(drpct[0]["threshold_max"].ToString());
            if (pctdouble > pcttmax || pctdouble < pcttmin)
            {
                this.textBox8.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox8.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForPLT(double pltdouble)
        {
            int _result = 1;
            DataRow[] drplt = dttv.Select("type='PLT'");
            double pltwmin = double.Parse(drplt[0]["warning_min"].ToString());
            double pltwmax = double.Parse(drplt[0]["warning_max"].ToString());
            if (pltdouble > pltwmax || pltdouble < pltwmin)
            {
                this.textBox7.ForeColor = Color.Blue;
                _result = 2;
            }
            double plttmin = double.Parse(drplt[0]["threshold_min"].ToString());
            double plttmax = double.Parse(drplt[0]["threshold_max"].ToString());
            if (pltdouble > plttmax || pltdouble < plttmin)
            {
                this.textBox7.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox7.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForHGB(double hgbdouble)
        {
            int _result = 1;
            DataRow[] drhgb = dttv.Select("type='HGB'");
            double hgbwmin = double.Parse(drhgb[0]["warning_min"].ToString());
            double hgbwmax = double.Parse(drhgb[0]["warning_max"].ToString());
            if (hgbdouble > hgbwmax || hgbdouble < hgbwmin)
            {
                this.textBox11.ForeColor = Color.Blue;
                _result = 2;
            }
            double hgbtmin = double.Parse(drhgb[0]["threshold_min"].ToString());
            double hgbtmax = double.Parse(drhgb[0]["threshold_max"].ToString());
            if (hgbdouble > hgbtmax || hgbdouble < hgbtmin)
            {
                this.textBox11.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox11.ForeColor = Color.Black;
            }
            return _result;
        }
        private int GetJudgeResultForHCT(double hctdouble)
        {
            int _result = 1;
            DataRow[] drhct = dttv.Select("type='HCT'");
            double hctwmin = double.Parse(drhct[0]["warning_min"].ToString());
            double hctwmax = double.Parse(drhct[0]["warning_max"].ToString());
            if (hctdouble > hctwmax || hctdouble < hctwmin)
            {
                this.textBox10.ForeColor = Color.Blue;
                _result = 2;
            }
            double hcttmin = double.Parse(drhct[0]["threshold_min"].ToString());
            double hcttmax = double.Parse(drhct[0]["threshold_max"].ToString());
            if (hctdouble > hcttmax || hctdouble < hcttmin)
            {
                this.textBox10.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox10.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForMCV(double mcvdouble)
        {
            int _result = 1;
            DataRow[] drmcv = dttv.Select("type='MCV'");
            double mcvwmin = double.Parse(drmcv[0]["warning_min"].ToString());
            double mcvwmax = double.Parse(drmcv[0]["warning_max"].ToString());
            if (mcvdouble > mcvwmax || mcvdouble < mcvwmin)
            {
                this.textBox13.ForeColor = Color.Blue;
                _result = 2;
            }
            double mcvtmin = double.Parse(drmcv[0]["threshold_min"].ToString());
            double mcvtmax = double.Parse(drmcv[0]["threshold_max"].ToString());
            if (mcvdouble > mcvtmax || mcvdouble < mcvtmin)
            {
                this.textBox13.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox13.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForMCH(double mchdouble)
        {
            int _result = 1;
            DataRow[] drmch = dttv.Select("type='MCH'");
            double mchwmin = double.Parse(drmch[0]["warning_min"].ToString());
            double mchwmax = double.Parse(drmch[0]["warning_max"].ToString());
            if (mchdouble > mchwmax || mchdouble < mchwmin)
            {
                this.textBox12.ForeColor = Color.Blue;
                _result = 2;
            }
            double mchtmin = double.Parse(drmch[0]["threshold_min"].ToString());
            double mchtmax = double.Parse(drmch[0]["threshold_max"].ToString());
            if (mchdouble > mchtmax || mchdouble < mchtmin)
            {
                this.textBox12.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox12.ForeColor = Color.Black;
            }
            return _result;
        }


        private int GetJudgeResultForMCHC(double mchcdouble)
        {
            int _result = 1;
            DataRow[] drmchc = dttv.Select("type='MCHC'");
            double mchcwmin = double.Parse(drmchc[0]["warning_min"].ToString());
            double mchcwmax = double.Parse(drmchc[0]["warning_max"].ToString());
            if (mchcdouble > mchcwmax || mchcdouble < mchcwmin)
            {
                this.textBox15.ForeColor = Color.Blue;
                _result = 2;
            }
            double mchctmin = double.Parse(drmchc[0]["threshold_min"].ToString());
            double mchctmax = double.Parse(drmchc[0]["threshold_max"].ToString());
            if (mchcdouble > mchctmax || mchcdouble < mchctmin)
            {
                this.textBox15.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox15.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForRDWCV(double rdwcvdouble)
        {
            int _result = 1;
            DataRow[] drrdwcv = dttv.Select("type='RDWCV'");
            double rdwcvwmin = double.Parse(drrdwcv[0]["warning_min"].ToString());
            double rdwcvwmax = double.Parse(drrdwcv[0]["warning_max"].ToString());
            if (rdwcvdouble > rdwcvwmax || rdwcvdouble < rdwcvwmin)
            {
                this.textBox14.ForeColor = Color.Blue;
                _result = 2;
            }
            double rdwcvtmin = double.Parse(drrdwcv[0]["threshold_min"].ToString());
            double rdwcvtmax = double.Parse(drrdwcv[0]["threshold_max"].ToString());
            if (rdwcvdouble > rdwcvtmax || rdwcvdouble < rdwcvtmin)
            {
                this.textBox14.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox14.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForRDWSD(double rdwsddouble)
        {
            int _result = 1;
            DataRow[] drrdwsd = dttv.Select("type='RDWSD'");
            double rdwsdwmin = double.Parse(drrdwsd[0]["warning_min"].ToString());
            double rdwsdwmax = double.Parse(drrdwsd[0]["warning_max"].ToString());
            if (rdwsddouble > rdwsdwmax || rdwsddouble < rdwsdwmin)
            {
                this.textBox17.ForeColor = Color.Blue;
                _result = 2;
            }
            double rdwsdtmin = double.Parse(drrdwsd[0]["threshold_min"].ToString());
            double rdwsdtmax = double.Parse(drrdwsd[0]["threshold_max"].ToString());
            if (rdwsddouble > rdwsdtmax || rdwsddouble < rdwsdtmin)
            {
                this.textBox17.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox17.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForNEUT(double neutdouble)
        {
            int _result = 1;
            DataRow[] drneut = dttv.Select("type='NEUT'");
            if(drneut.Length>0)
            {
                double neutwmin = double.Parse(drneut[0]["warning_min"].ToString());
                double neutwmax = double.Parse(drneut[0]["warning_max"].ToString());
                if (neutdouble > neutwmax || neutdouble < neutwmin)
                {
                    this.textBox20.ForeColor = Color.Blue;
                    _result = 2;
                }
                double neuttmin = double.Parse(drneut[0]["threshold_min"].ToString());
                double neuttmax = double.Parse(drneut[0]["threshold_max"].ToString());
                if (neutdouble > neuttmax || neutdouble < neuttmin)
                {
                    this.textBox20.ForeColor = Color.Red;
                    _result = 3;
                }
                if (_result == 1)
                {
                    this.textBox20.ForeColor = Color.Black;
                }
            } 
            return _result;
        }

        private int GetJudgeResultForNEUTP(double neutpdouble)
        {
            int _result = 1;
            DataRow[] drneutp = dttv.Select("type='NEUTP'");
            double neutpwmin = double.Parse(drneutp[0]["warning_min"].ToString());
            double neutpwmax = double.Parse(drneutp[0]["warning_max"].ToString());
            if (neutpdouble > neutpwmax || neutpdouble < neutpwmin)
            {
                this.textBox23.ForeColor = Color.Blue;
                _result = 2;
            }
            double neutptmin = double.Parse(drneutp[0]["threshold_min"].ToString());
            double neutptmax = double.Parse(drneutp[0]["threshold_max"].ToString());
            if (neutpdouble > neutptmax || neutpdouble < neutptmin)
            {
                this.textBox23.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox23.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForLYM(double lymdouble)
        {
            int _result = 1; 
            DataRow[] drlym = dttv.Select("type='LYM'");
            double lymwmin = double.Parse(drlym[0]["warning_min"].ToString());
            double lymwmax = double.Parse(drlym[0]["warning_max"].ToString());
            if (lymdouble > lymwmax || lymdouble < lymwmin)
            {
                this.textBox28.ForeColor = Color.Blue;
                _result = 2;
            }
            double lymtmin = double.Parse(drlym[0]["threshold_min"].ToString());
            double lymtmax = double.Parse(drlym[0]["threshold_max"].ToString());
            if (lymdouble > lymtmax || lymdouble < lymtmin)
            {
                this.textBox28.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox28.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForLYMP(double lympdouble)
        {
            int _result = 1; 
            DataRow[] drlymp = dttv.Select("type='LYMP'");
            double lympwmin = double.Parse(drlymp[0]["warning_min"].ToString());
            double lympwmax = double.Parse(drlymp[0]["warning_max"].ToString());
            if (lympdouble > lympwmax || lympdouble < lympwmin)
            {
                this.textBox31.ForeColor = Color.Blue;
                _result = 2;
            }
            double lymptmin = double.Parse(drlymp[0]["threshold_min"].ToString());
            double lymptmax = double.Parse(drlymp[0]["threshold_max"].ToString());
            if (lympdouble > lymptmax || lympdouble < lymptmin)
            {
                this.textBox31.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox31.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForMPV(double mpvdouble)
        {
            int _result = 1; 
            DataRow[] drmpv = dttv.Select("type='MPV'");
            double mpvwmin = double.Parse(drmpv[0]["warning_min"].ToString());
            double mpvwmax = double.Parse(drmpv[0]["warning_max"].ToString());
            if (mpvdouble > mpvwmax || mpvdouble < mpvwmin)
            {
                this.textBox30.ForeColor = Color.Blue;
                _result = 2;
            }
            double mpvtmin = double.Parse(drmpv[0]["threshold_min"].ToString());
            double mpvtmax = double.Parse(drmpv[0]["threshold_max"].ToString());
            if (mpvdouble > mpvtmax || mpvdouble < mpvtmin)
            {
                this.textBox30.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox30.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForPDW(double pdwdouble)
        {
            int _result = 1; 
            DataRow[] drpdw = dttv.Select("type='PDW'");
            double pdwwmin = double.Parse(drpdw[0]["warning_min"].ToString());
            double pdwwmax = double.Parse(drpdw[0]["warning_max"].ToString());
            if (pdwdouble > pdwwmax || pdwdouble < pdwwmin)
            {
                this.textBox33.ForeColor = Color.Blue;
                _result = 2;
            }
            double pdwtmin = double.Parse(drpdw[0]["threshold_min"].ToString());
            double pdwtmax = double.Parse(drpdw[0]["threshold_max"].ToString());
            if (pdwdouble > pdwtmax || pdwdouble < pdwtmin)
            {
                this.textBox33.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox33.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForMXD(double mxddouble)
        {
            int _result = 1; 
            DataRow[] drmxd = dttv.Select("type='MXD'");
            double mxdwmin = double.Parse(drmxd[0]["warning_min"].ToString());
            double mxdwmax = double.Parse(drmxd[0]["warning_max"].ToString());
            if (mxddouble > mxdwmax || mxddouble < mxdwmin)
            {
                this.textBox32.ForeColor = Color.Blue;
                _result = 2;
            }
            double mxdtmin = double.Parse(drmxd[0]["threshold_min"].ToString());
            double mxdtmax = double.Parse(drmxd[0]["threshold_max"].ToString());
            if (mxddouble > mxdtmax || mxddouble < mxdtmin)
            {
                this.textBox32.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox32.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForMXDP(double mxdpdouble)
        {
            int _result = 1; 
            DataRow[] drmxdp = dttv.Select("type='MXDP'");
            double mxdpwmin = double.Parse(drmxdp[0]["warning_min"].ToString());
            double mxdpwmax = double.Parse(drmxdp[0]["warning_max"].ToString());
            if (mxdpdouble > mxdpwmax || mxdpdouble < mxdpwmin)
            {
                this.textBox35.ForeColor = Color.Blue;
                _result = 2;
            }
            double mxdptmin = double.Parse(drmxdp[0]["threshold_min"].ToString());
            double mxdptmax = double.Parse(drmxdp[0]["threshold_max"].ToString());
            if (mxdpdouble > mxdptmax || mxdpdouble < mxdptmin)
            {
                this.textBox35.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox35.ForeColor = Color.Black;
            }
            return _result;
        }

        #endregion 
        private void updateBichao_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = name;
            this.textBox3.Text = time;
            this.textBox9.Text = aichive_no;
            this.textBox4.Text = id_number;
            this.textBox2.Text = bar_code;
            DataTable dtbichao = tjdao.selectXuechangguiInfo(aichive_no, bar_code);
            _isHaveData = false;
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                _isHaveData = true;
                _currentdevno = "XCG_YNH_001";
                if (dtbichao.Rows[0]["deviceModel"] != null)
                {
                    _currentdevno = dtbichao.Rows[0]["deviceModel"].ToString();
                    if (_currentdevno == "")
                    {
                        _currentdevno = "XCG_YNH_001";
                    }
                }
                grjdDao grjddao = new grjdDao();
                dttv = grjddao.checkThresholdValues(_currentdevno, "血常规");


                string wbc = dtbichao.Rows[0]["WBC"].ToString();
                if (wbc != "" && wbc != "*")
                {
                    double wbcdouble = 0;
                    bool a = double.TryParse(wbc, out wbcdouble);
                    if(a==true)
                    {
                        GetJudgeResultForWBC(wbcdouble);
                    } 
                }
                this.textBox5.Text = wbc;

                string rbc = dtbichao.Rows[0]["RBC"].ToString();
                if (rbc != "" && rbc != "*")
                {
                    double rbcdouble = 0;
                    bool a = double.TryParse(rbc, out rbcdouble);
                    if(a==true)
                    {
                        GetJudgeResultForRBC(rbcdouble);
                    } 
                }               
                this.textBox6.Text = rbc;

                string pct = dtbichao.Rows[0]["PCT"].ToString();
                if (pct != "" && pct != "*")
                {
                    double pctdouble = 0;
                    bool a = double.TryParse(pct, out pctdouble);
                    if (a == true)
                    {
                        GetJudgeResultForPCT(pctdouble);
                    } 
                }
                this.textBox8.Text = pct;

                string plt = dtbichao.Rows[0]["PLT"].ToString();
                if (plt != "" && plt != "*")
                {
                    double pltdouble = 0;
                    bool a = double.TryParse(plt, out pltdouble);
                    if (a == true)
                    {
                        GetJudgeResultForPLT(pltdouble);
                    } 
                }
                this.textBox7.Text = plt;

                string hgb = dtbichao.Rows[0]["HGB"].ToString();
                if (hgb != "" && hgb != "*")
                {
                    double hgbdouble = 0;
                    bool a = double.TryParse(hgb, out hgbdouble);
                    if (a == true)
                    {
                        GetJudgeResultForHGB(hgbdouble);
                    } 
                }
                this.textBox11.Text = hgb;

                string hct = dtbichao.Rows[0]["HCT"].ToString();
                if (hct != "" && hct != "*")
                {
                    double hctdouble = 0;
                    bool a = double.TryParse(hct, out hctdouble);
                    if (a == true)
                    {
                        GetJudgeResultForHCT(hctdouble);
                    } 
                }
                this.textBox10.Text = hct;

                string mcv = dtbichao.Rows[0]["MCV"].ToString();
                if (mcv != "" && mcv != "*")
                {
                    double mcvdouble =0;
                    bool a = double.TryParse(mcv, out mcvdouble);
                    if (a == true)
                    {
                        GetJudgeResultForMCV(mcvdouble);
                    } 
                }
                this.textBox13.Text = mcv;

                string mch = dtbichao.Rows[0]["MCH"].ToString();
                if (mch != "" && mch != "*")
                {
                    double mchdouble = 0;
                    bool a = double.TryParse(mch, out mchdouble);
                    if (a == true)
                    {
                        GetJudgeResultForMCH(mchdouble);
                    }  
                }
                this.textBox12.Text = mch;

                string mchc = dtbichao.Rows[0]["MCHC"].ToString();
                if (mchc != "" && mchc != "*")
                {
                    double mchcdouble = 0;
                    bool a = double.TryParse(mchc, out mchcdouble);
                    if (a == true)
                    {
                        GetJudgeResultForMCHC(mchcdouble);
                    } 
                }
                this.textBox15.Text = mchc;

                string rdwcv = dtbichao.Rows[0]["RDWCV"].ToString();
                if (rdwcv != "" && rdwcv != "*")
                {
                    double rdwcvdouble = 0; 
                    bool a = double.TryParse(rdwcv, out rdwcvdouble);
                    if (a == true)
                    {
                        GetJudgeResultForRDWCV(rdwcvdouble);
                    }
                }
                this.textBox14.Text = rdwcv;

                string rdwsd = dtbichao.Rows[0]["RDWSD"].ToString();
                if (rdwsd != "" && rdwsd != "*")
                {
                    double rdwsddouble = 0;
                    bool a = double.TryParse(rdwsd, out rdwsddouble);
                    if (a == true)
                    {
                        GetJudgeResultForRDWSD(rdwsddouble);
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
                    GetJudgeResultForNEUT(neutdouble); 
                }
                this.textBox20.Text = neut;

                string neutp = dtbichao.Rows[0]["NEUTP"].ToString();
                if (neutp != "" && neutp != "*")
                {
                    double neutpdouble = double.Parse(neutp);
                    GetJudgeResultForNEUTP(neutpdouble); 
                }
                this.textBox23.Text = neutp;

                this.textBox22.Text = dtbichao.Rows[0]["EO"].ToString();
                this.textBox25.Text = dtbichao.Rows[0]["EOP"].ToString();
                this.textBox24.Text = dtbichao.Rows[0]["BASO"].ToString();
                this.textBox29.Text = dtbichao.Rows[0]["BASOP"].ToString();

                string lym = dtbichao.Rows[0]["LYM"].ToString();
                if (lym != "" && lym != "*")
                {
                    double lymdouble = 0;
                    bool a = double.TryParse(lym, out lymdouble);
                    if (a == true)
                    {
                        GetJudgeResultForLYM(lymdouble);
                    } 
                }
                this.textBox28.Text = lym;

                string lymp = dtbichao.Rows[0]["LYMP"].ToString();
                if (lymp != "" && lymp != "*")
                {
                    double lympdouble = 0;
                    bool a = double.TryParse(lymp, out lympdouble);
                    if (a == true)
                    {
                        GetJudgeResultForLYMP(lympdouble);
                    } 
                }
                this.textBox31.Text = lymp;

                string mpv = dtbichao.Rows[0]["MPV"].ToString();
                if (mpv != "" && mpv != "*")
                {
                    double mpvdouble =0;
                    bool a = double.TryParse(mpv, out mpvdouble);
                    if (a == true)
                    {
                        GetJudgeResultForMPV(mpvdouble);
                    } 
                }
                this.textBox30.Text = mpv;

                string pdw = dtbichao.Rows[0]["PDW"].ToString();
                if (pdw != "" && pdw != "*")
                {
                    double pdwdouble = 0;
                    bool a = double.TryParse(pdw, out pdwdouble);
                    if (a == true)
                    {
                        GetJudgeResultForPDW(pdwdouble);
                    } 
                }
                this.textBox33.Text = pdw;

                string mxd = dtbichao.Rows[0]["MXD"].ToString();
                if (mxd != "" && mxd != "*")
                {
                    double mxddouble = 0;
                    bool a = double.TryParse(mxd, out mxddouble);
                    if (a == true)
                    {
                        GetJudgeResultForMXD(mxddouble);
                    } 
                }
                this.textBox32.Text = mxd;

                string mxdp = dtbichao.Rows[0]["MXDP"].ToString();
                if (mxdp != "" && mxdp != "*")
                {
                    double mxdpdouble = 0;
                    bool a = double.TryParse(mxdp, out mxdpdouble);
                    if (a == true)
                    {
                        GetJudgeResultForMXDP(mxdpdouble);
                    } 
                }
                this.textBox35.Text = mxdp;
                this.textBox34.Text = dtbichao.Rows[0]["PLCR"].ToString();
                this.textBox36.Text = dtbichao.Rows[0]["OTHERS"].ToString();
            }
            else {
                _currentdevno = "XCG_YNH_001";
                string[] a = Common._deviceModel.Split(',');
                _currentdevno = a[1].ToString();
                grjdDao grjddao = new grjdDao();
                dttv = grjddao.checkThresholdValues(_currentdevno, "血常规");

                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        { 
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
            //1：判断有没有，有就更新 否则就插入 DataTable selectXuechangguiInfo
            bool istrue = false;
           
            if (_isHaveData==true)
            { 
                istrue = tjdao.updateXuechangguiInfo(aichive_no, bar_code, WBC, RBC, PCT, PLT, HGB, HCT, MCV, MCH, MCHC, RDWCV, RDWSD, MONO, MONOP, GRAN, GRANP, NEUT, NEUTP, EO, EOP, BASO, BASOP, LYM, LYMP, MPV, PDW, MXD, MXDP, PLCR, OTHERS, _currentdevno);
            }
            else
            {
                string[] deviceno = Common._deviceModel.Split(',');
                xuechangguiBean obj = new xuechangguiBean();
                obj.deviceModel = deviceno[1].ToString().Trim();
                obj.aichive_no = aichive_no;
                obj.bar_code = bar_code;
                obj.id_number = textBox4.Text;
                obj.HCT = HCT;
                obj.HGB = HGB;
                obj.LYM = LYM;
                obj.LYMP = LYMP;
                obj.MCH = MCH;
                obj.MCHC = MCHC;
                obj.MCV = MCV;
                obj.MPV = MPV;
                obj.MXD = MXD;
                obj.MXDP = MXDP;
                obj.NEUT = NEUT;
                obj.NEUTP= NEUTP;
                obj.PCT = PCT;
                obj.PDW = PDW;
                obj.PLT = PLT;
                obj.RBC = RBC;
                obj.RDWCV = RDWCV;
                obj.RDWSD = RDWSD;
                obj.WBC = WBC;
                obj.MONO = MONO;
                obj.MONOP = MONOP;
                obj.GRAN = GRAN;
                obj.GRANP = GRANP;
                obj.PLCR = PLCR;
                obj.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                obj.ZrysXCG = "";
                //obj.timeCodeUnique = obj.bar_code + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                istrue = tjdao.insertXuechangguiInfo(obj);
            }
            
            if (istrue)
            {
                #region 处理下数据判断
                
                int r0 = 1;
                if (WBC != "" && WBC != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(WBC, out a);
                    if(b==true)
                    {
                        r0 = GetJudgeResultForWBC(a);
                    }
                    
                } 
                int r1 = 1;
                if (RBC != "" && RBC != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(RBC, out a);
                    if (b == true)
                    {
                        r1 = GetJudgeResultForRBC(a);
                    } 
                } 
                int r2 = 1;
                if (PCT != "" && PCT != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(PCT, out a);
                    if (b == true)
                    {
                        r2 = GetJudgeResultForPCT(a);
                    } 
                } 
                int r3 = 1;
                if (PLT != "" && PLT != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(PLT, out a);
                    if (b == true)
                    {
                        r3 = GetJudgeResultForPLT(a);
                    } 
                } 
                int r4 = 1;
                if (HGB != "" && HGB != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(HGB, out a);
                    if (b == true)
                    {
                        r4 = GetJudgeResultForHGB(a);
                    } 
                }
                int r5 = 1;
                if (HCT != "" && HCT != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(HCT, out a);
                    if (b == true)
                    {
                        r5 = GetJudgeResultForHCT(a);
                    } 
                }
                int r6 = 1;
                if (MCV != "" && MCV != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(MCV, out a);
                    if (b == true)
                    {
                        r6 = GetJudgeResultForMCV(a);
                    } 
                }
                int r7 = 1;
                if (MCH != "" && MCH != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(MCH, out a);
                    if (b == true)
                    {
                        r7 = GetJudgeResultForMCH(a);
                    } 
                }
                int r8 = 1;
                if (MCHC != "" && MCHC != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(MCHC, out a);
                    if (b == true)
                    {
                        r8 = GetJudgeResultForMCHC(a);
                    } 
                }
                int r9 = 1;
                if (RDWCV != "" && RDWCV != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(RDWCV, out a);
                    if (b == true)
                    {
                        r9 = GetJudgeResultForRDWCV(a);
                    } 
                }
                int r10 = 1;
                if (RDWSD != "" && RDWSD != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(RDWSD, out a);
                    if (b == true)
                    {
                        r10 = GetJudgeResultForRDWSD(a);
                    } 
                }
                int r11 = 1;
                if (NEUT != "" && NEUT != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(NEUT, out a);
                    if (b == true)
                    {
                        r11 = GetJudgeResultForNEUT(a);
                    } 
                }
                int r12 = 1;
                if (NEUTP != "" && NEUTP != "*")
                {
                    double a =0;
                    bool b = double.TryParse(NEUTP, out a);
                    if (b == true)
                    {
                        r12 = GetJudgeResultForNEUTP(a);
                    } 
                }
                int r13 = 1;
                if (LYM != "" && LYM != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(LYM, out a);
                    if (b == true)
                    {
                        r13 = GetJudgeResultForLYM(a);
                    } 
                }
                int r14 = 1;
                if (LYMP != "" && LYMP != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(LYMP, out a);
                    if (b == true)
                    {
                        r14 = GetJudgeResultForLYMP(a);
                    } 
                }
                int r15 = 1;
                if (MPV != "" && MPV != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(MPV, out a);
                    if (b == true)
                    {
                        r15 = GetJudgeResultForMPV(a);
                    } 
                }
                int r16 = 1;
                if (PDW != "" && PDW != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(PDW, out a);
                    if (b == true)
                    {
                        r16 = GetJudgeResultForPDW(a);
                    } 
                }
                int r17 = 1;
                if (MXD != "" && MXD != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(MXD, out a);
                    if (b == true)
                    {
                        r17 = GetJudgeResultForMXD(a);
                    } 
                }
                int r18 = 1;
                if (MXDP != "" && MXDP != "*")
                {
                    double a = 0;
                    bool b = double.TryParse(MXDP, out a);
                    if (b == true)
                    {
                        r18 = GetJudgeResultForMXDP(a);
                    } 
                }
                int r = 1;
                r = r0;
                if(r<r1)
                {
                    r = r1;
                }
                if (r < r2)
                {
                    r = r2;
                }
                if (r < r3)
                {
                    r = r3;
                }
                if (r < r4)
                {
                    r = r4;
                }
                if (r < r5)
                {
                    r = r5;
                }
                if (r < r6)
                {
                    r = r6;
                }
                if (r < r7)
                {
                    r = r7;
                }
                if (r < r8)
                {
                    r = r8;
                }
                if (r < r9)
                {
                    r = r9;
                }
                if (r < r10)
                {
                    r = r10;
                }
                if (r < r11)
                {
                    r = r11;
                }
                if (r < r12)
                {
                    r = r12;
                }
                if (r < r13)
                {
                    r = r13;
                }
                if (r < r14)
                {
                    r = r14;
                }
                if (r < r15)
                {
                    r = r15;
                }
                if (r < r16)
                {
                    r = r16;
                }
                if (r < r17)
                {
                    r = r17;
                }
                if (r < r18)
                {
                    r = r18;
                }
                #endregion
                tjdao.updateTJbgdcXuechanggui(aichive_no, bar_code, r);
                tjdao.updatePEXcgInfo(aichive_no, bar_code, HGB, WBC, PLT);
                testFunDelegate(r, 8, rowIndex);
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
            g.DrawString("保存", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 5));

        }
    }
}
