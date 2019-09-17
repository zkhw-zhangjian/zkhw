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
    public partial class updateShengoaTizhong : Form
    {
        public int rowIndex = 0;
        public delegate void TestFunDelegate(int _result, int _colIndex, int _rowIndex);
        public TestFunDelegate testFunDelegate;
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        private bool isfrist = false; 
        tjcheckDao tjdao = new tjcheckDao();
        public DataTable dttv = null;
        public updateShengoaTizhong()
        {
            InitializeComponent();
        }

        #region 判断结果
        private int GetJudgeResultForHeight(double _height)
        {
            int _result = 1;
            DataRow[] drheight = dttv.Select("type='HEIGHT'");
            double heightwmin = double.Parse(drheight[0]["warning_min"].ToString());
            double heightwmax = double.Parse(drheight[0]["warning_max"].ToString());
            if (_height > heightwmax || _height < heightwmin)
            {
                _result = 2;
                this.textBox5.ForeColor = Color.Blue;
            }
            double heighttmin = double.Parse(drheight[0]["threshold_min"].ToString());
            double heighttmax = double.Parse(drheight[0]["threshold_max"].ToString());
            if (_result > heighttmax || _result < heighttmin)
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
        private int GetJudgeResultForWeight(double _weight)
        {
            int _result = 1;
            DataRow[] drweightdouble = dttv.Select("type='WEIGHT'");
            double weightwmin = double.Parse(drweightdouble[0]["warning_min"].ToString());
            double weightwmax = double.Parse(drweightdouble[0]["warning_max"].ToString());
            if (_weight > weightwmax || _weight < weightwmin)
            {
                _result = 2;
                this.textBox6.ForeColor = Color.Blue;
            }
            double weighttmin = double.Parse(drweightdouble[0]["threshold_min"].ToString());
            double weighttmax = double.Parse(drweightdouble[0]["threshold_max"].ToString());
            if (_weight > weighttmax || _weight < weighttmin)
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
        private int GetJudgeResultForBMI(double _bmi)
        {
            int _result = 1; 
            if (_bmi >= 24 && _bmi <= 28)
            {
                _result = 2;
                this.textBox7.ForeColor = Color.Blue;
            }
            else if (_bmi > 28)
            {
                _result = 3;
                this.textBox7.ForeColor = Color.Red;
            }  
            if(_result==1)
            {
                this.textBox7.ForeColor = Color.Black;
            }
            return _result;
        }
        #endregion
        private void updateBichao_Load(object sender, EventArgs e)
        {
            isfrist = true;
            this.textBox1.Text = name;
            this.textBox3.Text = time;
            this.textBox9.Text = aichive_no;
            this.textBox4.Text = id_number;
            this.textBox2.Text = bar_code;
            DataTable dtbichao = tjdao.selectSgtzInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                #region 身高
                string height = dtbichao.Rows[0]["Height"].ToString();
                if (height != "")
                {
                    double heightdouble = 0;
                    bool a = double.TryParse(height, out heightdouble);
                    if (a == true)
                    {
                        int ret = GetJudgeResultForHeight(heightdouble);
                    } 
                }
                this.textBox5.Text = height;
                #endregion

                #region 重量
                string weight = dtbichao.Rows[0]["Weight"].ToString();
                if (weight != "")
                {
                    double weightdouble = 0;
                    bool a = double.TryParse(weight, out weightdouble);
                    if (a == true)
                    {
                        int ret = GetJudgeResultForWeight(weightdouble);
                    } 
                }
                this.textBox6.Text = weight;
                #endregion

                #region BMI
                string _bmi= dtbichao.Rows[0]["BMI"].ToString();
                if(_bmi !="")
                {
                    double _dbBmi = 0;
                    bool a = double.TryParse(_bmi, out _dbBmi);
                    if (a == true)
                    {
                        int ret = GetJudgeResultForBMI(_dbBmi);
                    } 
                }
                this.textBox7.Text = _bmi;
                #endregion
            }
            else { 
                MessageBox.Show("未查询到数据!");
            }
            isfrist = false;
        }
        private void button5_Click(object sender, EventArgs e)
        {   
            string Height = this.textBox5.Text;
            string Weight =  this.textBox6.Text;
            string BMI = this.textBox7.Text; 
            bool istrue= tjdao.updateSgtzInfo(aichive_no, bar_code, Height, Weight, BMI);
            if (istrue)
            {
                //2019-6-19更新 zkhw_tj_bgdc 要根据输入的数值进行判断
                int r0 = 1;
                int r1 = 1;
                int r2 = 1;
                if (Height != "")
                {
                    double a = 0; 
                    bool b = double.TryParse(Height, out a);
                    if (b == true)
                    {
                        r0 = GetJudgeResultForHeight(a);
                    } 
                }
                if (Weight != "")
                {
                    double a = 0; 
                    bool b = double.TryParse(Weight, out a);
                    if (b == true)
                    {
                        r1 = GetJudgeResultForWeight(a);
                    } 
                }
                if (BMI != "")
                {
                    double a = 0;
                    bool b = double.TryParse(BMI, out a);
                    if (b == true)
                    {
                        r2 = GetJudgeResultForBMI(a);
                    } 
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
                tjdao.updateTJbgdcSgtz(aichive_no, bar_code, r.ToString());
                tjdao.updatePESgtzInfo(aichive_no, bar_code, Height, Weight, BMI);
                testFunDelegate(r, 11, rowIndex);
                MessageBox.Show("数据保存成功!");
            }
            else {
                MessageBox.Show("数据保存失败!");
            } 
        }



        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //这里计算体面积
            GetBMIText();
        }

        private void GetBMIText()
        {
            if (isfrist == true) return;
            if (textBox5.Text == "" || textBox6.Text == "") return;
            double _height = double.Parse(textBox5.Text);
            double _weight = double.Parse(textBox6.Text);
            textBox7.Text = JiSuanBMI(_height, _weight);
            int ret = GetJudgeResultForBMI(double.Parse(textBox7.Text));
        }

        private string JiSuanBMI(double _height,double _weight)
        { 
            double a = _height / 100.0;
            double b = Math.Pow(a, 2);
            double c = _weight / b;
            return c.ToString("#0.0");
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

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            GetBMIText();
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("保存", new System.Drawing.Font("微软雅黑", 10, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 5));
        }
    }
}
