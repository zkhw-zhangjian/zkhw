using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.bean;
using zkhwClient.dao;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUhealthPoorServices : Form
    {
        public int IS = 0;
        public string aichive_no = "";
        public string id = "";
        public string id_number = "";
        public string doctor_id = "";
        public bool show = true;
        public string mag = "";
        healthPoorServicesDao hpsd = new healthPoorServicesDao();
        public aUhealthPoorServices(int s,string name,string no, string id,string sex,string birthday)
        {
            InitializeComponent();

            aichive_no = no.Trim();
            id_number = id.Trim();
            this.textBox1.Text = name;
            this.textBox2.Text = aichive_no;
            this.textBox12.Text = id_number;
            this.dateTimePicker1.Text = birthday;
            if (sex == "男") {
                this.radioButton1.Checked = true;
            } else if (sex == "女") {
                this.radioButton2.Checked = true;
            }
            this.Text += "  "+(IS == 1 ? "新增" : "修改");
            if (IS == 1) { id = "";}
            if (GetUpdate())
            {
                GetData();
                return;
            }
            else
            {
                show = false;
                mag = "没有修改数据！";
                return;
            }
        }

        private void aUhealthPoorServices_Load(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            healthPoor hp = new healthPoor();
            hp.archive_no = this.textBox2.Text;
            hp.id_number = this.textBox12.Text;
            if (IS==1) {
                if (this.radioButton1.Checked) {
                    hp.sex = "1";
                } else if (this.radioButton2.Checked) {
                    hp.sex = "2";
                }
                hp.birthday = this.dateTimePicker1.Text;
                hp.visit_doctor = doctor_id;
            }
            
            hp.visit_date = this.dateTimePicker2.Text;
            
            if (this.radioButton3.Checked)
            {
                hp.visit_type = "1";
            }
            else if (this.radioButton4.Checked)
            {
                hp.visit_type = "2";
            }
            else if (this.radioButton5.Checked)
            {
                hp.visit_type = "3";
            }
            hp.work_info = this.richTextBox1.Text;
            hp.advice = this.richTextBox2.Text;
            bool bl= hpsd.aUelderly_selfcare_estimate(hp,id);
            if (bl) {
                MessageBox.Show("保存成功！");
                this.DialogResult = DialogResult.OK;
            }
        }
        /// <summary>
        /// 判断是否有修改数据
        /// </summary>
        /// <returns></returns>
        private bool GetUpdate()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from poor_follow_record where archive_no='{aichive_no}' and id_number='{id_number}'");
            if (data != null && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void GetData()
        {
            string sql = $@"select * from poor_follow_record where archive_no='{aichive_no}' and id_number='{id_number}' order by create_time desc LIMIT 1";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                DataTable dt= jb.Tables[0];
                id=dt.Rows[0]["id"].ToString();
                //this.textBox1.Text = dt.Rows[0]["name"].ToString();
                //this.textBox2.Text = dt.Rows[0]["archive_no"].ToString();
                //this.textBox12.Text = dt.Rows[0]["id_number"].ToString();
                //string sex = dt.Rows[0]["sex"].ToString();
                //if (sex == "1")
                //{
                //    this.radioButton1.Checked = true;
                //}
                //else if(sex == "2")
                //{
                //    this.radioButton2.Checked = true;
                //}
                //this.dateTimePicker1.Text = dt.Rows[0]["birthday"].ToString();
                this.dateTimePicker2.Text = dt.Rows[0]["visit_date"].ToString();
                string type = dt.Rows[0]["visit_type"].ToString();
                if (type == "1")
                {
                    this.radioButton3.Checked = true;
                }
                else if (type == "2")
                {
                    this.radioButton4.Checked = true;
                }
                else if (type == "3")
                {
                    this.radioButton5.Checked = true;
                }
                this.richTextBox1.Text = dt.Rows[0]["work_info"].ToString();
                this.richTextBox2.Text = dt.Rows[0]["advice"].ToString();
            }
        }
  }
}
