using System;
using System.Data;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.updateTjResult
{
    public partial class updateBichao : Form
    {
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        bool flag = false;
        tjcheckDao tjdao = new tjcheckDao();
        public updateBichao()
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
            DataTable dtbichao = tjdao.selectBichaoInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                flag = true;
                this.textBox5.Text = dtbichao.Rows[0]["FubuBC"].ToString();
                this.textBox6.Text = dtbichao.Rows[0]["FubuResult"].ToString();
                this.textBox7.Text = dtbichao.Rows[0]["FubuDesc"].ToString();
                this.textBox11.Text = dtbichao.Rows[0]["QitaBC"].ToString();
                this.textBox10.Text = dtbichao.Rows[0]["QitaResult"].ToString();
                this.textBox8.Text = dtbichao.Rows[0]["QitaDesc"].ToString();
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //if (flag) {
                string FubuBC = this.textBox5.Text;
                string FubuResult =this.textBox6.Text;
                string FubuDesc = this.textBox7.Text;
                string QitaBC = this.textBox11.Text;
                string QitaResult = this.textBox10.Text;
                string QitaDesc = this.textBox8.Text;
                string barcode = this.textBox2.Text;
                bool istrue= tjdao.updateBichaoInfo(aichive_no, bar_code, FubuBC, FubuResult, FubuDesc, QitaBC, QitaResult, QitaDesc);
                if (istrue)
                {
                    if (FubuResult.IndexOf("未见异常") > -1 || FubuResult.IndexOf("未见明显异常") > -1)
                    {
                        DbHelperMySQL.ExecuteSql($"update physical_examination_record set ultrasound_abdomen='1' where aichive_no='"+ aichive_no + "' and bar_code='" + barcode + "'");
                        string issqdgbc = "update zkhw_tj_bgdc set BChao='1' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                        DbHelperMySQL.ExecuteSql(issqdgbc);
                    }
                    else
                    {
                        DbHelperMySQL.ExecuteSql($"update physical_examination_record set ultrasound_abdomen='2',ultrasound_memo='"+ FubuResult + "' where aichive_no='"+ aichive_no + "' and bar_code='" + barcode + "'");
                        string issqdgbc = "update zkhw_tj_bgdc set BChao='3' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                        DbHelperMySQL.ExecuteSql(issqdgbc);
                    }
                    MessageBox.Show("数据保存成功!");
                }
                else {
                    MessageBox.Show("数据保存失败!");
                }
            //}
        }

    }
}
