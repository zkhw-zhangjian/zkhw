using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.updateTjResult
{
    public partial class updateNiaochanggui : Form
    {
        public int rowIndex = 0;
        public delegate void TestFunDelegate(int _result, int _colIndex, int _rowIndex);
        public TestFunDelegate testFunDelegate;
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = ""; 
        tjcheckDao tjdao = new tjcheckDao();
        public DataTable dttv = null;
        private bool _isHaveData = false;
        public updateNiaochanggui()
        {
            InitializeComponent();
        }

        #region 判断
        private void GetMinMax(string _sType,out double _warningmin, out double _warningmax, out double _thresholdmin, out double _thresholdmax,out bool _ishave)
        {
            _warningmin = 0;
            _warningmax = 0;
            _thresholdmin = 0;
            _thresholdmax = 0;
            _ishave = false;
            DataRow[] dr = dttv.Select("class_type='尿常规' and type='"+ _sType + "'");
            //以前的方法
            if (dr == null || dr.Length == 0)
            {
                _ishave = false;
            }
            else
            {
                _ishave = true;
                _warningmin = double.Parse(dr[0]["warning_min"].ToString());
                _warningmax = double.Parse(dr[0]["warning_max"].ToString());
                _thresholdmin = double.Parse(dr[0]["threshold_min"].ToString());
                _thresholdmax = double.Parse(dr[0]["threshold_max"].ToString()); 
            }
        }

        private int GetJudgeResultForWBC(string wbc)
        {
            int _result = 1;
            if (!"-".Equals(wbc))
            {
                if ("+-".Equals(wbc) || "+1".Equals(wbc) || "+2".Equals(wbc) || "+3".Equals(wbc))
                {
                    this.textBox5.ForeColor = Color.Blue;
                    _result = 2;
                }
                else
                {
                    this.textBox5.ForeColor = Color.Red;
                    _result = 3;
                }
            }
            if(_result==1)
            {
                this.textBox5.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForNIT(string nit)
        {
            int _result = 1;
            if (!"-".Equals(nit))
            {
                if ("+".Equals(nit))
                {
                    this.textBox8.ForeColor = Color.Blue;
                    _result = 2;
                }
                else
                {
                    _result = 3;
                    this.textBox8.ForeColor = Color.Red;
                }
            }
            if (_result == 1)
            {
                this.textBox8.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForURO(string uro)
        {
            int _result = 1;
            if (!"Normal".Equals(uro))
            {
                if ("+1".Equals(uro) || "+2".Equals(uro) || "+3".Equals(uro))
                {
                    _result = 2;
                    this.textBox7.ForeColor = Color.Blue;
                }
                else
                {
                    _result = 3;
                    this.textBox7.ForeColor = Color.Red;
                }
            }
            if (_result == 1)
            {
                this.textBox7.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForPRO(string pro)
        {
            int _result = 1;
            if (!"-".Equals(pro))
            {
                if ("+-".Equals(pro) || "+1".Equals(pro) || "+2".Equals(pro) || "+3".Equals(pro) || "+4".Equals(pro))
                {
                    _result = 2;
                    this.textBox11.ForeColor = Color.Blue;
                }
                else
                {
                    _result = 3;
                    this.textBox11.ForeColor = Color.Red;
                }
            }
            if (_result == 1)
            {
                this.textBox11.ForeColor = Color.Black;
            }
            return _result;
        }
        private int GetJudgeResultForPH(double phdouble)
        {
            int _result = 1; 
            double _warningmin = 0;
            double _warningmax = 0;
            double _thresholdmin = 0;
            double _thresholdmax = 0;
            bool _ishave = false;
            GetMinMax("PH", out _warningmin, out _warningmax, out _thresholdmin, out _thresholdmax, out _ishave);

            if (_ishave == true)
            {
                if (phdouble < _thresholdmin || phdouble > _thresholdmax)
                {
                    _result = 3;
                    this.textBox10.ForeColor = Color.Red;
                }
                else if (phdouble >= _warningmin && phdouble <= _warningmax)
                { }
                else
                {
                    _result = 2;
                    this.textBox10.ForeColor = Color.Blue;
                }
            }
            if (_result == 1)
            {
                this.textBox10.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForBLD(string bld)
        {
            int _result = 1;
            if (!"-".Equals(bld))
            {
                if ("+-".Equals(bld) || "+1".Equals(bld) || "+2".Equals(bld) || "+3".Equals(bld))
                {
                    _result = 2;
                    this.textBox13.ForeColor = Color.Blue;
                }
                else
                {
                    _result = 3;
                    this.textBox13.ForeColor = Color.Red;
                }
            }
            if (_result == 1)
            {
                this.textBox13.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForSG(double sgdouble)
        {
            int _result = 1;
            double _warningmin = 0;
            double _warningmax = 0;
            double _thresholdmin = 0;
            double _thresholdmax = 0;
            bool _ishave = false;
            GetMinMax("SG", out _warningmin, out _warningmax, out _thresholdmin, out _thresholdmax, out _ishave);
            if (_ishave == true)
            {
                if (sgdouble > _thresholdmax || sgdouble < _thresholdmin)
                {
                    _result = 3;
                    this.textBox12.ForeColor = Color.Red;
                }
                else if (sgdouble >= _warningmin && sgdouble <= _warningmax)
                { }
                else
                {
                    _result = 2;
                    this.textBox12.ForeColor = Color.Blue;
                }
            }
            if (_result == 1)
            {
                this.textBox12.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForKET(string ket)
        {
            int _result = 1;
            if (!"-".Equals(ket))
            {
                if ("+-".Equals(ket) || "+1".Equals(ket) || "+2".Equals(ket) || "+3".Equals(ket))
                {
                    this.textBox15.ForeColor = Color.Blue;
                    _result = 2;
                }
                else
                {
                    this.textBox15.ForeColor = Color.Red;
                    _result = 3;
                }
            } 
            if (_result == 1)
            {
                this.textBox15.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForBIL(string bil)
        {
            int _result = 1;
            if (!"-".Equals(bil))
            {
                if ("+1".Equals(bil) || "+2".Equals(bil) || "+3".Equals(bil))
                {
                    this.textBox14.ForeColor = Color.Blue;
                    _result = 2;
                }
                else
                {
                    this.textBox14.ForeColor = Color.Red;
                    _result = 3;
                }
            }
            if (_result == 1)
            {
                this.textBox14.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForGLU(string glu)
        {
            int _result = 1;
            if (!"-".Equals(glu))
            {
                if ("+-".Equals(glu) || "+1".Equals(glu) || "+2".Equals(glu) || "+3".Equals(glu) || "+4".Equals(glu))
                {
                    this.textBox17.ForeColor = Color.Blue;
                    _result = 2;
                }
                else
                {
                    this.textBox17.ForeColor = Color.Red;
                    _result = 3;
                }
            }
            if (_result == 1)
            {
                this.textBox17.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForVC(string vc)
        {
            int _result = 1;
            if (!"-".Equals(vc))
            {
                if ("+-".Equals(vc) || "+1".Equals(vc) || "+2".Equals(vc) || "+3".Equals(vc))
                {
                    this.textBox16.ForeColor = Color.Blue;
                    _result = 2;
                }
                else
                {
                    this.textBox16.ForeColor = Color.Red;
                    _result = 3;
                }
            }
            if (_result == 1)
            {
                this.textBox16.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForMA(double madouble)
        {
            int _result = 1;
            double _warningmin = 0;
            double _warningmax = 0;
            double _thresholdmin = 0;
            double _thresholdmax = 0;
            bool _ishave = false;
            GetMinMax("MA", out _warningmin, out _warningmax, out _thresholdmin, out _thresholdmax, out _ishave);
            if (_ishave == true)
            {
                if (madouble > _thresholdmax || madouble < _thresholdmin)
                {
                    this.textBox19.ForeColor = Color.Red;
                    _result = 3;
                }
                else if (madouble >= _warningmin && madouble <= _warningmax)
                { }
                else
                {
                    this.textBox19.ForeColor = Color.Blue;
                    _result = 2;
                }
            }
            if (_result == 1)
            {
                this.textBox19.ForeColor = Color.Black;
            }
            return _result;
        }
        
        #endregion
        private void updateBichao_Load(object sender, EventArgs e)
        {
            grjdDao grjddao = new grjdDao();
            dttv = grjddao.checkThresholdValues("", "");

            this.textBox1.Text = name;
            this.textBox3.Text = time;
            this.textBox9.Text = aichive_no;
            this.textBox4.Text = id_number;
            this.textBox2.Text = bar_code;
            _isHaveData = false;
            DataTable dtbichao = tjdao.selectNiaochangguiInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                _isHaveData = true;
                string wbc= dtbichao.Rows[0]["WBC"].ToString();
                int _result = GetJudgeResultForWBC(wbc); 
                this.textBox5.Text = wbc;

                this.textBox6.Text = dtbichao.Rows[0]["LEU"].ToString();

                string nit = dtbichao.Rows[0]["NIT"].ToString(); 
                _result = GetJudgeResultForNIT(nit);  
                this.textBox8.Text = nit;

                string uro = dtbichao.Rows[0]["URO"].ToString();
                _result = GetJudgeResultForURO(uro);  
                this.textBox7.Text = uro;

                string pro = dtbichao.Rows[0]["PRO"].ToString();
                _result=GetJudgeResultForPRO(pro);
                this.textBox11.Text = pro;

                string ph = dtbichao.Rows[0]["PH"].ToString();
                if (ph != "")
                { 
                    double phdouble = 0;
                    bool a = double.TryParse(ph, out phdouble);
                    if (a==true)
                    {
                        _result = GetJudgeResultForPH(phdouble);
                    } 
                }
                this.textBox10.Text = ph;

                string bld = dtbichao.Rows[0]["BLD"].ToString();
                _result=GetJudgeResultForBLD(bld); 
                this.textBox13.Text = bld;

                string sg=dtbichao.Rows[0]["SG"].ToString();
                if (sg != "")
                {
                    double sgdouble = 0;
                    bool a = double.TryParse(sg, out sgdouble);
                    if (a == true)
                    {
                        GetJudgeResultForSG(sgdouble);
                    } 
                }
                this.textBox12.Text = sg;

                string ket = dtbichao.Rows[0]["KET"].ToString();
                GetJudgeResultForKET(ket);
                this.textBox15.Text = ket;

                string bil = dtbichao.Rows[0]["BIL"].ToString();
                GetJudgeResultForBIL(bil);
                this.textBox14.Text = bil;

                string glu = dtbichao.Rows[0]["GLU"].ToString();
                GetJudgeResultForGLU(glu);
                this.textBox17.Text = glu;

                string vc = dtbichao.Rows[0]["Vc"].ToString();
                GetJudgeResultForVC(vc); 
                this.textBox16.Text = dtbichao.Rows[0]["Vc"].ToString();

                string strma=dtbichao.Rows[0]["MA"].ToString();
                if (strma != "")
                {
                    double madouble = 0;
                    bool a = double.TryParse(strma, out madouble);
                    if (a == true)
                    {
                        GetJudgeResultForMA(madouble);
                    } 
                } 
                this.textBox19.Text = dtbichao.Rows[0]["MA"].ToString();

                this.textBox18.Text = dtbichao.Rows[0]["ACR"].ToString();
                this.textBox21.Text = dtbichao.Rows[0]["Ca"].ToString();
                this.textBox20.Text = dtbichao.Rows[0]["CR"].ToString();
            }
            else { 
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        { 
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
            bool istrue = false;
            if (_isHaveData)
            {
                istrue = tjdao.updateNiaochangguiInfo(aichive_no, bar_code, WBC, LEU, NIT, URO, PRO, PH, BLD, SG, KET, BIL, GLU, Vc, MA, ACR, Ca, CR);
            }
            else {
                istrue = tjdao.insertNiaojiInfo(aichive_no, id_number, bar_code, WBC, KET, NIT, URO, BIL, GLU, PRO, SG, PH, BLD, Vc, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "11G",basicInfoSettings.ncg);
            }
            if (istrue)
            {
                int r = 1;
                #region bgdc表中显示什么颜色
                int r0 = GetJudgeResultForWBC(WBC);
                int r1 = GetJudgeResultForNIT(NIT);
                int r2 = GetJudgeResultForURO(URO);
                int r3 = GetJudgeResultForPRO(PRO);
                int r4 = 1;
                if (PH != "")
                {
                    double b = 0;
                    bool a = double.TryParse(PH, out b);
                    if (a == true)
                    {
                        r4 = GetJudgeResultForPH(b);
                    } 
                } 
                int r5 = GetJudgeResultForBLD(BLD);
                int r6 = 1;
                if(SG !="")
                { 
                    double b = 0;
                    bool a = double.TryParse(SG, out b);
                    if (a == true)
                    {
                        r6 = GetJudgeResultForSG(b);
                    }
                } 
                int r7 = GetJudgeResultForKET(KET);
                int r8 = GetJudgeResultForBIL(BIL);
                int r9 = GetJudgeResultForGLU(GLU);
                int r10 = GetJudgeResultForVC(Vc);
                int r11 = 1;
                if(MA !="")
                { 
                    double b = 0;
                    bool a = double.TryParse(MA, out b);
                    if (a == true)
                    {
                        r11 = GetJudgeResultForMA(b);
                    }
                }

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
                #endregion
                tjdao.updateTJbgdcNiaochanggui(aichive_no, bar_code, r);
                tjdao.updatePENcgInfo(aichive_no, bar_code, PRO, GLU, KET, BLD);
                testFunDelegate(r, 9, rowIndex);
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
            g.DrawString("保存", new System.Drawing.Font("微软雅黑", 10, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(24, 5));
        }
    }
}
