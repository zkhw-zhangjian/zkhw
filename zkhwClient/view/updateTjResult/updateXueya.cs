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
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        bool flag = false;
        tjcheckDao tjdao = new tjcheckDao();
        public DataTable dttv = null;
        public updateXueya()
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
            DataTable dtbichao = tjdao.selectXueyaInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                flag = true;
                string sbp=dtbichao.Rows[0]["SBP"].ToString();
                if (sbp != "")
                {
                    double sbpdouble = double.Parse(sbp);
                    DataRow[] drsbp = dttv.Select("type='SBP'");
                    double sbpwmin = double.Parse(drsbp[0]["warning_min"].ToString());
                    double sbpwmax = double.Parse(drsbp[0]["warning_max"].ToString());
                    if (sbpdouble > sbpwmax || sbpdouble < sbpwmin)
                    {
                        this.textBox5.ForeColor = Color.Blue;
                    }
                    double sbptmin = double.Parse(drsbp[0]["threshold_min"].ToString());
                    double sbptmax = double.Parse(drsbp[0]["threshold_max"].ToString());
                    if (sbpdouble > sbptmax || sbpdouble < sbptmin)
                    {
                        this.textBox5.ForeColor = Color.Red;
                    }
                }
                this.textBox5.Text = sbp;
                string dbp = dtbichao.Rows[0]["DBP"].ToString();
                if (dbp != "")
                {
                    double dbpdouble = double.Parse(dbp);
                    DataRow[] drdbp = dttv.Select("type='DBP'");
                    double dbpwmin = double.Parse(drdbp[0]["warning_min"].ToString());
                    double dbpwmax = double.Parse(drdbp[0]["warning_max"].ToString());
                    if (dbpdouble > dbpwmax || dbpdouble < dbpwmin)
                    {
                        this.textBox6.ForeColor = Color.Blue;
                    }
                    double dbptmin = double.Parse(drdbp[0]["threshold_min"].ToString());
                    double dbptmax = double.Parse(drdbp[0]["threshold_max"].ToString());
                    if (dbpdouble > dbptmax || dbpdouble < dbptmin)
                    {
                        this.textBox6.ForeColor = Color.Red;
                    }
                }
                this.textBox6.Text = dbp;
                this.textBox7.Text = dtbichao.Rows[0]["Pulse"].ToString();
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //if (flag) {
                string SBP = this.textBox5.Text;
                string DBP =  this.textBox6.Text;
                string Pulse = this.textBox7.Text;
                bool istrue= tjdao.updateXueyaInfo(aichive_no, id_number, bar_code, DBP, SBP, Pulse);
                if (istrue)
                {   
                    MessageBox.Show("数据保存成功!");
                }
                else {
                MessageBox.Show("数据保存失败!");
            }
            //}
        }
    }
}
