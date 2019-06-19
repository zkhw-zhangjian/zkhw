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
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        bool flag = false;
        tjcheckDao tjdao = new tjcheckDao();
        public DataTable dttv = null;
        public updateShengoaTizhong()
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
            DataTable dtbichao = tjdao.selectSgtzInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                flag = true;
                string height = dtbichao.Rows[0]["Height"].ToString();
                if (height != "")
                {
                    double heightdouble = double.Parse(height);
                    DataRow[] drheight = dttv.Select("type='HEIGHT'");
                    double heightwmin = double.Parse(drheight[0]["warning_min"].ToString());
                    double heightwmax = double.Parse(drheight[0]["warning_max"].ToString());
                    if (heightdouble > heightwmax || heightdouble < heightwmin)
                    {
                        this.textBox5.ForeColor = Color.Blue;
                    }
                    double heighttmin = double.Parse(drheight[0]["threshold_min"].ToString());
                    double heighttmax = double.Parse(drheight[0]["threshold_max"].ToString());
                    if (heightdouble > heighttmax || heightdouble < heighttmin)
                    {
                        this.textBox5.ForeColor = Color.Red;
                    }
                }
                this.textBox5.Text = height;
                string weight = dtbichao.Rows[0]["Weight"].ToString();
                if (weight != "")
                {
                    double weightdouble = double.Parse(weight);
                    DataRow[] drweightdouble = dttv.Select("type='WEIGHT'");
                    double weightwmin = double.Parse(drweightdouble[0]["warning_min"].ToString());
                    double weightwmax = double.Parse(drweightdouble[0]["warning_max"].ToString());
                    if (weightdouble > weightwmax || weightdouble < weightwmin)
                    {
                        this.textBox6.ForeColor = Color.Blue;
                    }
                    double weighttmin = double.Parse(drweightdouble[0]["threshold_min"].ToString());
                    double weighttmax = double.Parse(drweightdouble[0]["threshold_max"].ToString());
                    if (weightdouble > weighttmax || weightdouble < weighttmin)
                    {
                        this.textBox6.ForeColor = Color.Red;
                    }
                }
                this.textBox6.Text = weight;

                string _bmi= dtbichao.Rows[0]["BMI"].ToString();
                if(_bmi !="")
                {
                    double _dbBmi = double.Parse(_bmi);
                    if (_dbBmi >= 24 && _dbBmi <= 28)
                    {
                        this.textBox7.ForeColor = Color.Blue;
                    }
                    else if (_dbBmi > 28)
                    {
                        this.textBox7.ForeColor = Color.Red;
                    }

                }
                this.textBox7.Text = _bmi;
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //if (flag) {
                string Height = this.textBox5.Text;
                string Weight =  this.textBox6.Text;
                string BMI = this.textBox7.Text;
                bool istrue= tjdao.updateSgtzInfo(aichive_no, bar_code, Height, Weight, BMI);
                if (istrue)
                {
                    tjdao.updateTJbgdcSgtz(aichive_no, bar_code);
                    tjdao.updatePESgtzInfo(aichive_no, bar_code, Height, Weight, BMI);
                    MessageBox.Show("数据保存成功!");
                }
                else {
                    MessageBox.Show("数据保存失败!");
                }
            //}
        }

    }
}
