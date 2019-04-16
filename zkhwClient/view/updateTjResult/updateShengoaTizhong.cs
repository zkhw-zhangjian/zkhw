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
                this.textBox5.Text = dtbichao.Rows[0]["Height"].ToString();
                this.textBox6.Text = dtbichao.Rows[0]["Weight"].ToString();
                this.textBox7.Text = dtbichao.Rows[0]["BMI"].ToString();
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (flag) {
                string Height = this.textBox5.Text;
                string Weight =  this.textBox6.Text;
                string BMI = this.textBox7.Text;
                bool istrue= tjdao.updateSgtzInfo(aichive_no, bar_code, Height, Weight, BMI);
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
