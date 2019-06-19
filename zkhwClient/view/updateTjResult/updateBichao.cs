using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        private List<string> _picPath = new List<string>();
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
                this.textBox5.Text = dtbichao.Rows[0]["FubuBC"].ToString();
                this.textBox6.Text = dtbichao.Rows[0]["FubuResult"].ToString();
                this.textBox7.Text = dtbichao.Rows[0]["FubuDesc"].ToString();
                this.textBox11.Text = dtbichao.Rows[0]["QitaBC"].ToString();
                this.textBox10.Text = dtbichao.Rows[0]["QitaResult"].ToString();
                this.textBox8.Text = dtbichao.Rows[0]["QitaDesc"].ToString();
                string tmp = "";
                tmp= dtbichao.Rows[0]["BuPic01"].ToString();
                if(tmp !="")
                {
                    _picPath.Add(tmp);
                }
                tmp = "";
                tmp = dtbichao.Rows[0]["BuPic02"].ToString();
                if (tmp != "")
                {
                    _picPath.Add(tmp);
                }
                tmp = "";
                tmp = dtbichao.Rows[0]["BuPic03"].ToString();
                if (tmp != "")
                {
                    _picPath.Add(tmp);
                }
                tmp = "";
                tmp = dtbichao.Rows[0]["BuPic04"].ToString();
                if (tmp != "")
                {
                    _picPath.Add(tmp);
                }
                if(_picPath.Count<=1)
                {
                    btnPre.Visible = false;
                    btnNext.Visible = false;
                }
                DisplayPic(0);
            }
            else { 
                MessageBox.Show("未查询到数据!");
                btnPre.Visible = false;
                btnNext.Visible = false;
            }
        }

        private void DisplayPic(int index)
        {
            if (_picPath.Count > 0)
            {
                string t = _picPath[index].ToString();
                if(t !="")
                {
                    string path = "bcImg//"+t;
                    if (System.IO.File.Exists(path))
                    {
                        pictureBox1.Image = Image.FromFile(path, false);
                    }
                } 
            }
        }
        private void button5_Click(object sender, EventArgs e)
        { 
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
        }
        private int _indexPic = 0;
        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_indexPic<_picPath.Count-1)
            {
                _indexPic = _indexPic + 1;
                DisplayPic(_indexPic);
            }
            else
            {
                MessageBox.Show("已经是最后一张了！");
            }
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if(_indexPic>0)
            {
                _indexPic = _indexPic - 1;
                DisplayPic(_indexPic);
            }
            else
            {
                MessageBox.Show("到头了！");
            }
        }
    }
}
