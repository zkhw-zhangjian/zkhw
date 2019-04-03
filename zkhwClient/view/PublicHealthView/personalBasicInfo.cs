using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view.PublicHealthView
{
    public partial class personalBasicInfo : Form
    {

        service.personalBasicInfoService pBasicInfo = new service.personalBasicInfoService();
        //public string name = "";
        //public string id_number = "";
        //public string aichive_no = "";
        public string pCa = "";
        public string time1 = null;
        public string time2 = null;
        public personalBasicInfo()
        {
            InitializeComponent();
        }

        private void personalBasicInfo_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);

            this.label4.Text = "个人基本信息建档";
            this.label4.ForeColor = Color.SkyBlue;
            label4.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label4.Left = (this.panel1.Width - this.label4.Width) / 2;
            label4.BringToFront();

            button5_Click(null,null);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;
            if (pCa != "")
            {
                this.label5.Text = "";
            }
            else { this.label5.Text = "---姓名/身份证号/档案号---"; }
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            querypBasicInfo();
        }
        //高血压随访记录历史表  关联传参调查询的方法
        private void querypBasicInfo()
        {
            this.dataGridView1.DataSource = null;
            DataTable dt = pBasicInfo.queryPersonalBasicInfo(pCa, time1, time2);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "创建人";
            this.dataGridView1.Columns[4].HeaderCell.Value = "创建时间";
            this.dataGridView1.Columns[5].HeaderCell.Value = "责任医生";
            this.dataGridView1.Columns[6].HeaderCell.Value = "数据状态";

            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //添加 修改 高血压随访记录历史表 调此方法
        private void button1_Click(object sender, EventArgs e)
        {
            aUPersonalBasicInfo hm = new aUPersonalBasicInfo();
            hm.label47.Text = "添加个人基本信息表";
            hm.Text = "添加个人基本信息表";
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                button5_Click(null, null);
                MessageBox.Show("添加成功！");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            aUPersonalBasicInfo hm = new aUPersonalBasicInfo();
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            hm.id = id;
            hm.label47.Text = "修改个人基本信息表";
            hm.Text = "修改个人基本信息表";
            DataTable dt = pBasicInfo.queryPersonalBasicInfo0(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                hm.textBox1.Text = dt.Rows[0]["name"].ToString();
                hm.textBox2.Text = dt.Rows[0]["archive_no"].ToString();
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton1.Text) {  hm.radioButton1.Checked = true; };
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton2.Text) { hm.radioButton2.Checked = true; };
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton3.Text) { hm.radioButton3.Checked = true; };
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton25.Text) { hm.radioButton25.Checked = true; };
                hm.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["birthday"].ToString());
                hm.textBox12.Text = dt.Rows[0]["id_number"].ToString();
                hm.textBox14.Text = dt.Rows[0]["company"].ToString();
                hm.textBox16.Text = dt.Rows[0]["phone"].ToString();
                hm.textBox18.Text = dt.Rows[0]["link_name"].ToString();
                hm.textBox20.Text = dt.Rows[0]["link_phone"].ToString();
                if (dt.Rows[0]["resident_type"].ToString() == hm.radioButton4.Text) { hm.radioButton4.Checked = true; };
                if (dt.Rows[0]["resident_type"].ToString() == hm.radioButton5.Text) { hm.radioButton5.Checked = true; };

                if (dt.Rows[0]["nation"].ToString() == hm.radioButton6.Text) { hm.radioButton6.Checked = true; };
                if (dt.Rows[0]["nation"].ToString() == hm.radioButton7.Text) { hm.radioButton7.Checked = true; };

                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton8.Text) { hm.radioButton8.Checked = true; };
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton9.Text) { hm.radioButton9.Checked = true; };
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton10.Text) { hm.radioButton10.Checked = true; };
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton11.Text) { hm.radioButton11.Checked = true; };
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton12.Text) { hm.radioButton12.Checked = true; };

                if (dt.Rows[0]["blood_rh"].ToString() == hm.radioButton13.Text) { hm.radioButton13.Checked = true; };
                if (dt.Rows[0]["blood_rh"].ToString() == hm.radioButton14.Text) { hm.radioButton14.Checked = true; };
                if (dt.Rows[0]["blood_rh"].ToString() == hm.radioButton15.Text) { hm.radioButton15.Checked = true; };

                if (dt.Rows[0]["education"].ToString() == hm.radioButton22.Text) { hm.radioButton22.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton23.Text) { hm.radioButton23.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton24.Text) { hm.radioButton24.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton26.Text) { hm.radioButton26.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton27.Text) { hm.radioButton27.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton28.Text) { hm.radioButton28.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton29.Text) { hm.radioButton29.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton30.Text) { hm.radioButton30.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton31.Text) { hm.radioButton31.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton32.Text) { hm.radioButton32.Checked = true; };

                if (dt.Rows[0]["profession"].ToString() == hm.radioButton33.Text) { hm.radioButton33.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton34.Text) { hm.radioButton34.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton35.Text) { hm.radioButton35.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton36.Text) { hm.radioButton36.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton37.Text) { hm.radioButton37.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton38.Text) { hm.radioButton38.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton39.Text) { hm.radioButton39.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton40.Text) { hm.radioButton40.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton41.Text) { hm.radioButton41.Checked = true; };

                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton42.Text) { hm.radioButton42.Checked = true; };
                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton43.Text) { hm.radioButton43.Checked = true; };
                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton44.Text) { hm.radioButton44.Checked = true; };
                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton45.Text) { hm.radioButton45.Checked = true; };
                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton46.Text) { hm.radioButton46.Checked = true; };

                if (dt.Rows[0]["pay_type"].ToString() == hm.radioButton47.Text) { hm.radioButton47.Checked = true; };
                if (dt.Rows[0]["pay_type"].ToString() == hm.radioButton48.Text) { hm.radioButton48.Checked = true; };
                if (dt.Rows[0]["pay_type"].ToString() == hm.radioButton49.Text) { hm.radioButton49.Checked = true; };
                if (dt.Rows[0]["pay_type"].ToString() == hm.radioButton50.Text) { hm.radioButton50.Checked = true; };
                if (dt.Rows[0]["pay_type"].ToString() == hm.radioButton51.Text) { hm.radioButton51.Checked = true; };
                if (dt.Rows[0]["pay_type"].ToString() == hm.radioButton52.Text) { hm.radioButton52.Checked = true; };
                if (dt.Rows[0]["pay_type"].ToString() == hm.radioButton53.Text) { hm.radioButton53.Checked = true; };
                if (dt.Rows[0]["pay_type"].ToString() == hm.radioButton54.Text) { hm.radioButton54.Checked = true; };

                if (dt.Rows[0]["drug_allergy"].ToString() == hm.radioButton55.Text) { hm.radioButton55.Checked = true; };
                if (dt.Rows[0]["drug_allergy"].ToString() == hm.radioButton56.Text) { hm.radioButton56.Checked = true; };
                if (dt.Rows[0]["drug_allergy"].ToString() == hm.radioButton57.Text) { hm.radioButton57.Checked = true; };
                if (dt.Rows[0]["drug_allergy"].ToString() == hm.radioButton58.Text) { hm.radioButton58.Checked = true; };
                if (dt.Rows[0]["drug_allergy"].ToString() == hm.radioButton59.Text) { hm.radioButton59.Checked = true; };

                if (dt.Rows[0]["exposure"].ToString() == hm.radioButton60.Text) { hm.radioButton60.Checked = true; };
                if (dt.Rows[0]["exposure"].ToString() == hm.radioButton61.Text) { hm.radioButton61.Checked = true; };
                if (dt.Rows[0]["exposure"].ToString() == hm.radioButton62.Text) { hm.radioButton62.Checked = true; };
                if (dt.Rows[0]["exposure"].ToString() == hm.radioButton63.Text) { hm.radioButton63.Checked = true; };



                hm.richTextBox4.Text = dt.Rows[0]["heredity_name"].ToString();
                foreach (Control ctr in hm.panel20.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (dt.Rows[0]["deformity_name"].ToString().IndexOf(ck.Text) > -1)
                        {
                            ck.Checked = true;
                        }
                    }
                }

                if (dt.Rows[0]["kitchen"].ToString() == hm.radioButton18.Text) { hm.radioButton18.Checked = true; };
                if (dt.Rows[0]["kitchen"].ToString() == hm.radioButton19.Text) { hm.radioButton19.Checked = true; };
                if (dt.Rows[0]["kitchen"].ToString() == hm.radioButton20.Text) { hm.radioButton20.Checked = true; };
                if (dt.Rows[0]["kitchen"].ToString() == hm.radioButton21.Text) { hm.radioButton21.Checked = true; };

                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton70.Text) { hm.radioButton70.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton71.Text) { hm.radioButton71.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton72.Text) { hm.radioButton72.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton73.Text) { hm.radioButton73.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton74.Text) { hm.radioButton74.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton75.Text) { hm.radioButton75.Checked = true; };

                if (dt.Rows[0]["drink"].ToString() == hm.radioButton76.Text) { hm.radioButton76.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton77.Text) { hm.radioButton77.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton78.Text) { hm.radioButton78.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton79.Text) { hm.radioButton79.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton80.Text) { hm.radioButton80.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton81.Text) { hm.radioButton81.Checked = true; };

                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton82.Text) { hm.radioButton82.Checked = true; };
                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton83.Text) { hm.radioButton83.Checked = true; };
                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton84.Text) { hm.radioButton84.Checked = true; };
                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton85.Text) { hm.radioButton85.Checked = true; };
                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton86.Text) { hm.radioButton86.Checked = true; };

                if (dt.Rows[0]["poultry"].ToString() == hm.radioButton87.Text) { hm.radioButton87.Checked = true; };
                if (dt.Rows[0]["poultry"].ToString() == hm.radioButton88.Text) { hm.radioButton88.Checked = true; };
                if (dt.Rows[0]["poultry"].ToString() == hm.radioButton89.Text) { hm.radioButton89.Checked = true; };
                if (dt.Rows[0]["poultry"].ToString() == hm.radioButton90.Text) { hm.radioButton90.Checked = true; };
            }
            else { }



            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面

                button5_Click(null, null);
                MessageBox.Show("修改成功！");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
                bool istrue = pBasicInfo.deletePersonalBasicInfo(id);
                if (istrue)
                {
                    //刷新页面
                    button5_Click(null, null);
                    MessageBox.Show("删除成功！");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label5.Visible = this.textBox1.Text.Length < 1;
        }
    }
}
