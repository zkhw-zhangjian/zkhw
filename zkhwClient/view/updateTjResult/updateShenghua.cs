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

        private string _currentdevno = "SH_YNH_001";
        private bool _haveData = false;
        public updateShenghua()
        {
            InitializeComponent();
        }

        #region 判断
        private int GetJudgeResultForALT(double altdouble)
        {
            int _result = 1;
            DataRow[] dralt = dttv.Select("type='ALT'");
            double altwmin = double.Parse(dralt[0]["warning_min"].ToString());
            double altwmax = double.Parse(dralt[0]["warning_max"].ToString());
            if (altdouble > altwmax || altdouble < altwmin)
            {
                this.textBox5.ForeColor = Color.Blue;
                _result = 2;
            }
            double alttmin = double.Parse(dralt[0]["threshold_min"].ToString());
            double alttmax = double.Parse(dralt[0]["threshold_max"].ToString());
            if (altdouble > alttmax || altdouble < alttmin)
            {
                this.textBox5.ForeColor = Color.Red;
                _result = 3;
            }
            if (_result == 1)
            {
                this.textBox5.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForAST(double astdouble)
        {
            int _result = 1;
            DataRow[] drast = dttv.Select("type='AST'");
            double astwmin = double.Parse(drast[0]["warning_min"].ToString());
            double astwmax = double.Parse(drast[0]["warning_max"].ToString());
            if (astdouble > astwmax || astdouble < astwmin)
            {
                this.textBox6.ForeColor = Color.Blue;
                _result = 2;
            }
            double asttmin = double.Parse(drast[0]["threshold_min"].ToString());
            double asttmax = double.Parse(drast[0]["threshold_max"].ToString());
            if (astdouble > asttmax || astdouble < asttmin)
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

        private int GetJudgeResultForTBIL(double tbildouble)
        {
            int _result = 1;
            DataRow[] drtbil = dttv.Select("type='TBIL'");
            double tbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
            double tbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
            if (tbildouble > tbilwmax || tbildouble < tbilwmin)
            {
                this.textBox8.ForeColor = Color.Blue;
                _result = 2;
            }
            double tbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
            double tbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
            if (tbildouble > tbiltmax || tbildouble < tbiltmin)
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

        private int GetJudgeResultForDBIL(double dbildouble)
        {
            int _result = 1;
            DataRow[] drtbil = dttv.Select("type='DBIL'");
            double dbilwmin = double.Parse(drtbil[0]["warning_min"].ToString());
            double dbilwmax = double.Parse(drtbil[0]["warning_max"].ToString());
            if (dbildouble > dbilwmax || dbildouble < dbilwmin)
            {
                this.textBox7.ForeColor = Color.Blue;
                _result = 2;
            }
            double dbiltmin = double.Parse(drtbil[0]["threshold_min"].ToString());
            double dbiltmax = double.Parse(drtbil[0]["threshold_max"].ToString());
            if (dbildouble > dbiltmax || dbildouble < dbiltmin)
            {
                this.textBox7.ForeColor = Color.Red;
                _result = 3;
            }
            if(_result==1)
            {
                this.textBox7.ForeColor = Color.Black;
            }
            return _result;
        }


        private int GetJudgeResultForCREA(double creadouble)
        {
            int _result = 1;
            DataRow[] drcrea = dttv.Select("type='CREA'");
            double creawmin = double.Parse(drcrea[0]["warning_min"].ToString());
            double creawmax = double.Parse(drcrea[0]["warning_max"].ToString());
            if (creadouble > creawmax || creadouble < creawmin)
            {
                this.textBox11.ForeColor = Color.Blue;
                _result = 2;
            }
            double creatmin = double.Parse(drcrea[0]["threshold_min"].ToString());
            double creatmax = double.Parse(drcrea[0]["threshold_max"].ToString());
            if (creadouble > creatmax || creadouble < creatmin)
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

        private int GetJudgeResultForUREA(double ureadouble)
        {
            int _result = 1;
            DataRow[] drurea = dttv.Select("type='UREA'");
            double ureawmin = double.Parse(drurea[0]["warning_min"].ToString());
            double ureawmax = double.Parse(drurea[0]["warning_max"].ToString());
            if (ureadouble > ureawmax || ureadouble < ureawmin)
            {
                this.textBox10.ForeColor = Color.Blue;
                _result = 2;
            }
            double ureatmin = double.Parse(drurea[0]["threshold_min"].ToString());
            double ureatmax = double.Parse(drurea[0]["threshold_max"].ToString());
            if (ureadouble > ureatmax || ureadouble < ureatmin)
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

        private int GetJudgeResultForGLU(double gludouble)
        {
            int _result = 1;
            DataRow[] drglu = dttv.Select("type='GLU'");
            double gluwmin = double.Parse(drglu[0]["warning_min"].ToString());
            double gluwmax = double.Parse(drglu[0]["warning_max"].ToString());
            if (gludouble > gluwmax || gludouble < gluwmin)
            {
                this.textBox13.ForeColor = Color.Blue;
                _result = 2;
            }
            double glutmin = double.Parse(drglu[0]["threshold_min"].ToString());
            double glutmax = double.Parse(drglu[0]["threshold_max"].ToString());
            if (gludouble > glutmax || gludouble < glutmin)
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

        private int GetJudgeResultForTG(double tgdouble)
        {
            int _result = 1;
            DataRow[] drtg = dttv.Select("type='TG'");
            double tgwmin = double.Parse(drtg[0]["warning_min"].ToString());
            double tgwmax = double.Parse(drtg[0]["warning_max"].ToString());
            if (tgdouble > tgwmax || tgdouble < tgwmin)
            {
                this.textBox12.ForeColor = Color.Blue;
                _result = 2;
            }
            double tgtmin = double.Parse(drtg[0]["threshold_min"].ToString());
            double tgtmax = double.Parse(drtg[0]["threshold_max"].ToString());
            if (tgdouble > tgtmax || tgdouble < tgtmin)
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

        private int GetJudgeResultForCHO(double chodouble)
        {
            int _result = 1;
            DataRow[] drcho = dttv.Select("type='CHO'");
            double chowmin = double.Parse(drcho[0]["warning_min"].ToString());
            double chowmax = double.Parse(drcho[0]["warning_max"].ToString());
            if (chodouble > chowmax || chodouble < chowmin)
            {
                this.textBox15.ForeColor = Color.Blue;
                _result = 2;
            }
            double chotmin = double.Parse(drcho[0]["threshold_min"].ToString());
            double chotmax = double.Parse(drcho[0]["threshold_max"].ToString());
            if (chodouble > chotmax || chodouble < chotmin)
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

        private int GetJudgeResultForHDLC(double hdlcdouble)
        {
            int _result = 1;
            DataRow[] drhdlc = dttv.Select("type='HDLC'");
            if(drhdlc.Length==0)
            {
                drhdlc = dttv.Select("type='HDL'");
            }
            double hdlcwmin = double.Parse(drhdlc[0]["warning_min"].ToString());
            double hdlcwmax = double.Parse(drhdlc[0]["warning_max"].ToString());
            if (hdlcdouble > hdlcwmax || hdlcdouble < hdlcwmin)
            {
                this.textBox14.ForeColor = Color.Blue;
                _result = 2;
            }
            double hdlctmin = double.Parse(drhdlc[0]["threshold_min"].ToString());
            double hdlctmax = double.Parse(drhdlc[0]["threshold_max"].ToString());
            if (hdlcdouble > hdlctmax || hdlcdouble < hdlctmin)
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

        private int GetJudgeResultForLDLC(double ldlcdouble)
        {
            int _result = 1;
            DataRow[] drldlc = dttv.Select("type='LDLC'");
            if(drldlc.Length==0)
            {
                drldlc = dttv.Select("type='LDL'");
            }
            double ldlcwmin = double.Parse(drldlc[0]["warning_min"].ToString());
            double ldlcwmax = double.Parse(drldlc[0]["warning_max"].ToString());
            if (ldlcdouble > ldlcwmax || ldlcdouble < ldlcwmin)
            {
                this.textBox17.ForeColor = Color.Blue;
                _result = 2;
            }
            double ldlctmin = double.Parse(drldlc[0]["threshold_min"].ToString());
            double ldlctmax = double.Parse(drldlc[0]["threshold_max"].ToString());
            if (ldlcdouble > ldlctmax || ldlcdouble < ldlctmin)
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

        #endregion     
        
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
                _haveData = true;
                //这里处理下  dttv
                _currentdevno = "SH_YNH_001";
                if(dtbichao.Rows[0]["deviceModel"]!=null)
                {
                    _currentdevno = dtbichao.Rows[0]["deviceModel"].ToString();
                    if (_currentdevno == "")
                    {
                        _currentdevno = "SH_YNH_001";
                    }
                } 
                grjdDao grjddao = new grjdDao();
                dttv = grjddao.checkThresholdValues(_currentdevno, "生化");

                string alt = dtbichao.Rows[0]["ALT"].ToString();
                if (alt != "" && alt != "*")
                {
                    double altdouble = 0;
                    bool a = double.TryParse(alt, out altdouble);
                    if (a == true)
                    {
                        GetJudgeResultForALT(altdouble);
                    } 
                }
                this.textBox5.Text = alt;

                string ast = dtbichao.Rows[0]["AST"].ToString();
                if (ast != "" && ast != "*")
                {
                    double astdouble = 0;
                    bool a = double.TryParse(ast, out astdouble);
                    if (a == true)
                    {
                        GetJudgeResultForAST(astdouble);
                    } 
                }
                this.textBox6.Text = ast;

                string tbil = dtbichao.Rows[0]["TBIL"].ToString();
                if (tbil != "" && tbil != "*")
                {
                    double tbildouble = 0;
                    bool a = double.TryParse(tbil, out tbildouble);
                    if (a == true)
                    {
                        GetJudgeResultForTBIL(tbildouble);
                    } 
                }
                this.textBox8.Text = tbil;

                string dbil= dtbichao.Rows[0]["DBIL"].ToString();
                //if (dbil != "" && dbil != "*")
                //{
                //    double dbildouble = double.Parse(dbil);
                //    GetJudgeResultForDBIL(dbildouble);
                //}
                this.textBox7.Text = dbil; 

                string crea = dtbichao.Rows[0]["CREA"].ToString();
                if (crea != "" && crea != "*")
                {
                    double creadouble = 0;
                    bool a = double.TryParse(crea, out creadouble);
                    if (a == true)
                    {
                        GetJudgeResultForCREA(creadouble);
                    } 
                }
                this.textBox11.Text = crea;
                string urea = dtbichao.Rows[0]["UREA"].ToString();
                if (urea != "" && urea != "*")
                {
                    double ureadouble = 0;
                    bool a = double.TryParse(urea, out ureadouble);
                    if (a == true)
                    {
                        GetJudgeResultForUREA(ureadouble);
                    } 
                }
                this.textBox10.Text = urea;

                string glu = dtbichao.Rows[0]["GLU"].ToString();
                if (glu != "" && glu != "*")
                {
                    double gludouble = 0;
                    bool a = double.TryParse(glu, out gludouble);
                    if (a == true)
                    {
                        GetJudgeResultForGLU(gludouble);
                    } 
                }
                this.textBox13.Text = glu;

                string tg = dtbichao.Rows[0]["TG"].ToString();
                if (tg != "" && tg != "*")
                {
                    double tgdouble = 0;
                    bool a = double.TryParse(tg, out tgdouble);
                    if (a == true)
                    {
                        GetJudgeResultForTG(tgdouble);
                    } 
                }
                this.textBox12.Text = tg;

                string cho = dtbichao.Rows[0]["CHO"].ToString();
                if (cho != "" && cho != "*")
                {
                    double chodouble = 0;
                    bool a = double.TryParse(cho, out chodouble);
                    if (a == true)
                    {
                        GetJudgeResultForCHO(chodouble);
                    } 
                }
                this.textBox15.Text = cho;

                string hdlc = dtbichao.Rows[0]["HDLC"].ToString();
                if (hdlc != "" && hdlc != "*")
                {
                    double hdlcdouble = 0;
                    bool a = double.TryParse(hdlc, out hdlcdouble);
                    if (a == true)
                    {
                        GetJudgeResultForHDLC(hdlcdouble);
                    } 
                }
                this.textBox14.Text = hdlc;

                string ldlc = dtbichao.Rows[0]["LDLC"].ToString();
                if (ldlc != "" && ldlc != "*")
                {
                    double ldlcdouble = 0;
                    bool a = double.TryParse(ldlc, out ldlcdouble);
                    if (a == true)
                    {
                        GetJudgeResultForLDLC(ldlcdouble);
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
                _currentdevno = "SH_YNH_001";
                string[] a = Common._deviceModel.Split(',');
                _currentdevno = a[0].ToString();
                grjdDao grjddao = new grjdDao();
                dttv = grjddao.checkThresholdValues(_currentdevno, "生化");
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        { 
            string ALT= this.textBox5.Text;
            string AST = this.textBox6.Text;
            string TBIL = this.textBox8.Text;
            string DBIL = this.textBox7.Text;
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
            string shren = setting.basicInfoSettings.sh;
             
            if(_haveData==false)
            {
                string[] deviceno = Common._deviceModel.Split(',');
                _currentdevno = deviceno[0].ToString().Trim();
                if (_currentdevno == "") _currentdevno = "SH_YNH_001";
            }
            bool istrue= tjdao.updateShenghuaInfo(aichive_no, id_number,bar_code, ALT, AST, TBIL, DBIL, CREA, UREA, GLU, TG, CHO, HDLC, LDLC, ALB, UA, HCY, AFP, CEA, Ka, Na, TP, ALP, GGT, CHE, TBA, APOA1, APOB, CK, CKMB, LDHL, HBDH, aAMY, shren, _currentdevno);
            if (istrue)
            {
                #region 判断
                int r = 1;
                int r0 = 1;
                if (ALT != "" && ALT != "*")
                {
                    double b = 0;
                    bool a = double.TryParse(ALT, out b);
                    if(a==true)
                    {
                        r0 = GetJudgeResultForALT(b);
                    } 
                }
                int r1 = 1;
                if (AST != "" && AST != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(AST, out b);
                    if (a == true)
                    {
                        r1 = GetJudgeResultForAST(b);
                    }
                }

                int r2 = 1;
                if (TBIL != "" && TBIL != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(TBIL, out b);
                    if (a == true)
                    {
                        r2 = GetJudgeResultForTBIL(b);
                    }
                }

                int r3 = 1;
                //if (DBIL != "" && DBIL != "*")
                //{
                //    r3 = GetJudgeResultForDBIL(double.Parse(DBIL));
                //}

                int r4 = 1;
                if (CREA != "" && CREA != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(CREA, out b);
                    if (a == true)
                    {
                        r4 = GetJudgeResultForCREA(b);
                    }
                }

                int r5 = 1;
                if (UREA != "" && UREA != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(UREA, out b);
                    if (a == true)
                    {
                        r5 = GetJudgeResultForUREA(b);
                    }
                }
                int r6 = 1;
                if (GLU != "" && GLU != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(GLU, out b);
                    if (a == true)
                    {
                        r6 = GetJudgeResultForGLU(b);
                    }
                }

                int r7 = 1;
                if (TG != "" && TG != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(TG, out b);
                    if (a == true)
                    {
                        r7 = GetJudgeResultForTG(b);
                    }
                }
                int r8 = 1;
                if (CHO != "" && CHO != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(CHO, out b);
                    if (a == true)
                    {
                        r8 = GetJudgeResultForCHO(b);
                    }
                }
                int r9 = 1;
                if (HDLC != "" && HDLC != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(HDLC, out b);
                    if (a == true)
                    {
                        r9 = GetJudgeResultForHDLC(b);
                    }
                }
                int r10 = 1;
                if (LDLC != "" && LDLC != "*")
                { 
                    double b = 0;
                    bool a = double.TryParse(LDLC, out b);
                    if (a == true)
                    {
                        r10 = GetJudgeResultForLDLC(b);
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
                #endregion
                tjdao.updateTJbgdcShenghua(aichive_no, bar_code, r);
                tjdao.updatePEShInfo(aichive_no, bar_code, CHO, TG, LDLC, HDLC, GLU, ALT, AST, ALB, TBIL, DBIL, CREA, UREA);
                testFunDelegate(r, 7, rowIndex);
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
