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
    public partial class updateXueya : Form
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
        public updateXueya()
        {
            InitializeComponent();
        }
        #region 判断
        private int GetJudgeResultForSBP(double _sbpdouble)
        {
            int _result = 1;
            DataRow[] drsbp = dttv.Select("type='SBP'");
            double sbpwmin = double.Parse(drsbp[0]["warning_min"].ToString());
            double sbpwmax = double.Parse(drsbp[0]["warning_max"].ToString());
            if (_sbpdouble > sbpwmax || _sbpdouble < sbpwmin)
            {
                _result = 2;
                this.textBox5.ForeColor = Color.Blue;
            }
            double sbptmin = double.Parse(drsbp[0]["threshold_min"].ToString());
            double sbptmax = double.Parse(drsbp[0]["threshold_max"].ToString());
            if (_sbpdouble > sbptmax || _sbpdouble < sbptmin)
            {
                _result = 3;
                this.textBox5.ForeColor = Color.Red;
            } 
            if (_result == 1)
            {
                this.textBox5.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForDBP(double _dbpdouble)
        {
            int _result = 1;
            DataRow[] drsbp = dttv.Select("type='DBP'");
            double sbpwmin = double.Parse(drsbp[0]["warning_min"].ToString());
            double sbpwmax = double.Parse(drsbp[0]["warning_max"].ToString());
            if (_dbpdouble > sbpwmax || _dbpdouble < sbpwmin)
            {
                _result = 2;
                this.textBox6.ForeColor = Color.Blue;
            }
            double sbptmin = double.Parse(drsbp[0]["threshold_min"].ToString());
            double sbptmax = double.Parse(drsbp[0]["threshold_max"].ToString());
            if (_dbpdouble > sbptmax || _dbpdouble < sbptmin)
            {
                _result = 3;
                this.textBox6.ForeColor = Color.Red;
            } 
            if(_result==1)
            {
                this.textBox6.ForeColor = Color.Black;
            }
            return _result;
        }

        private int GetJudgeResultForPulse(double _dbpulse)
        {
            int _result = 1;
            if (_dbpulse > 100 || _dbpulse < 60)
            {
                _result = 3;
                this.textBox7.ForeColor = Color.Red;
            }
            if (_result == 1)
            {
                this.textBox7.ForeColor = Color.Black;
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
            DataTable dtbichao = tjdao.selectXueyaInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            { 
                string sbp=dtbichao.Rows[0]["SBP"].ToString();
                if (sbp != "")
                {
                    double sbpdouble = double.Parse(sbp);
                    int _result = GetJudgeResultForSBP(sbpdouble); 
                }
                this.textBox5.Text = sbp;
                string dbp = dtbichao.Rows[0]["DBP"].ToString();
                if (dbp != "")
                {
                    double dbpdouble = double.Parse(dbp);
                    int _result = GetJudgeResultForDBP(dbpdouble);
                }
                this.textBox6.Text = dbp;
                
                string _pulsestr= dtbichao.Rows[0]["Pulse"].ToString();
                if(_pulsestr !="")
                {
                    double _dbpulse = double.Parse(_pulsestr);
                    int _result = GetJudgeResultForPulse(_dbpulse);               
                }
                this.textBox7.Text = _pulsestr;
            }
            else { 
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        { 
            string SBP = this.textBox5.Text;
            string DBP =  this.textBox6.Text;
            string Pulse = this.textBox7.Text;
            bool istrue= tjdao.updateXueyaInfo(aichive_no, id_number, bar_code, DBP, SBP, Pulse);
            if (istrue)
            {
                string xueya = "1";
                #region 处理需要什么颜色
                int r0 = 1;
                int r1 = 1;
                int r2 = 1;
                if (SBP != "")
                {
                    double a = double.Parse(SBP);
                    r0 = GetJudgeResultForSBP(a);
                }
                if (DBP != "")
                {
                    double a = double.Parse(DBP);
                    r1 = GetJudgeResultForDBP(a);
                }
                if (Pulse != "")
                {
                    double a = double.Parse(Pulse);
                    r2 = GetJudgeResultForPulse(a);
                }
                int r = r0;
                if (r < r1)
                {
                    r = r1;
                }
                if (r < r2)
                {
                    r = r2;
                }
                xueya = r.ToString();
                #endregion
                tjdao.updateTJbgdcXueya(aichive_no, bar_code, xueya);
                string base_respiratory = (Int32.Parse(Pulse) / 4).ToString();
                tjdao.updatePEXyInfo(aichive_no, bar_code, Pulse, SBP, DBP, (Int32.Parse(SBP) - 2).ToString(), (Int32.Parse(DBP) -3).ToString(),base_respiratory);
                testFunDelegate(r, 10, rowIndex);
                MessageBox.Show("数据保存成功!");
            }
            else {
            MessageBox.Show("数据保存失败!");
            } 
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                TextBox b = (TextBox)sender;
                int kc = (int)e.KeyChar;
                if ((kc < 48 || kc > 57) && kc != 8 && kc != 46)
                    e.Handled = true;
                if (kc == 46)                       //小数点
                {
                    if (b.Text.Length <= 0)
                        e.Handled = true;           //小数点不能在第一位
                    else
                    {
                        float f;
                        float oldf;
                        bool b1 = false, b2 = false;
                        b1 = float.TryParse(b.Text, out oldf);
                        b2 = float.TryParse(b.Text + e.KeyChar.ToString(), out f);
                        if (b2 == false)
                        {
                            if (b1 == true)
                                e.Handled = true;
                            else
                                e.Handled = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
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
