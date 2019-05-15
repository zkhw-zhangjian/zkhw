using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.bean;

namespace zkhwClient.view
{
    public partial class aUpsychiatricPatientServicesS1 : Form
    {
        service.psychiatricPatientServiceS psychiatricPatientS = new service.psychiatricPatientServiceS();
        public string id = ""; 
        public string archive_no = "";
        DataTable goodsList = new DataTable();//用药记录清单表 follow_medicine_record
        public aUpsychiatricPatientServicesS1()
        {
            InitializeComponent();
        }
        private void aUpsychiatricPatientServicesS1_Load(object sender, EventArgs e)
        {
            DataTable dt0 = psychiatricPatientS.queryPsychosis_follow_record0(archive_no);
            if (dt0 != null && dt0.Rows.Count > 0)
            {
                id = dt0.Rows[0]["id"].ToString();
            }
            //查询赋值
            if (id != "")
            {
                DataTable dt = psychiatricPatientS.queryPsychosis_follow_record(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (this.radioButton54.Text == dt.Rows[0]["transfer_treatment"].ToString()) { this.radioButton54.Checked = true; };
                    if (this.radioButton55.Text == dt.Rows[0]["transfer_treatment"].ToString()) { this.radioButton55.Checked = true; };

                    this.textBox26.Text = dt.Rows[0]["transfer_treatment_reason"].ToString();

                    if (this.radioButton25.Text == dt.Rows[0]["treatment_effect"].ToString()) { this.radioButton25.Checked = true; };
                    if (this.radioButton8.Text == dt.Rows[0]["treatment_effect"].ToString()) { this.radioButton8.Checked = true; };
                    if (this.radioButton9.Text == dt.Rows[0]["treatment_effect"].ToString()) { this.radioButton9.Checked = true; };


                    foreach (Control ctr in this.panel5.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["rehabilitation_measure"].ToString().IndexOf(ck.Text.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }
                    string next_visit_date=dt.Rows[0]["next_visit_date"].ToString();
                    if (next_visit_date == null || "".Equals(next_visit_date))
                    {
                        this.dateTimePicker1.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        this.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["next_visit_date"].ToString());
                    }
                    this.textBox6.Text = dt.Rows[0]["visit_doctor"].ToString();

                    if (this.radioButton28.Text == dt.Rows[0]["next_visit_classify"].ToString()) {  this.radioButton28.Checked = true; };
                    if (this.radioButton29.Text == dt.Rows[0]["next_visit_classify"].ToString()) { this.radioButton29.Checked = true; };
                }

                DataTable dt1 = psychiatricPatientS.queryFollow_medicine_record(id);
                goodsList = dt1.Clone();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow drtmp = goodsList.NewRow();
                    drtmp["id"] = dt1.Rows[i]["id"].ToString();
                    drtmp["follow_id"] = dt1.Rows[i]["follow_id"].ToString();
                    drtmp["drug_name"] = dt1.Rows[i]["drug_name"].ToString();
                    drtmp["num"] = dt1.Rows[i]["num"].ToString();
                    drtmp["dosage"] = dt1.Rows[i]["dosage"].ToString();
                    goodsList.Rows.Add(drtmp);
                }

                goodsListBind();//加载用药记录清单表 follow_medicine_record

            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            psychosis_follow_recordBean psychosis_follow_recordbean = new psychosis_follow_recordBean();
            if (this.radioButton54.Checked == true) { psychosis_follow_recordbean.transfer_treatment = this.radioButton54.Text; };
            if (this.radioButton55.Checked == true) { psychosis_follow_recordbean.transfer_treatment = this.radioButton55.Text; };

            psychosis_follow_recordbean.transfer_treatment_reason = this.textBox26.Text.Replace(" ", "");

            if (this.radioButton25.Checked == true) { psychosis_follow_recordbean.treatment_effect = this.radioButton25.Text; };
            if (this.radioButton8.Checked == true) { psychosis_follow_recordbean.treatment_effect = this.radioButton8.Text; };
            if (this.radioButton9.Checked == true) { psychosis_follow_recordbean.treatment_effect = this.radioButton9.Text; };


            foreach (Control ctr in this.panel5.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        psychosis_follow_recordbean.rehabilitation_measure += "," + ck.Text;
                    }
                }
            }
            if (psychosis_follow_recordbean.rehabilitation_measure != null && psychosis_follow_recordbean.rehabilitation_measure != "")
            {
                psychosis_follow_recordbean.rehabilitation_measure = psychosis_follow_recordbean.rehabilitation_measure.Substring(1);
            }
            psychosis_follow_recordbean.next_visit_date = this.dateTimePicker1.Text;
            psychosis_follow_recordbean.visit_doctor = this.textBox6.Text.Replace(" ", "");

            if (this.radioButton28.Checked == true) { psychosis_follow_recordbean.next_visit_classify = this.radioButton28.Text; };
            if (this.radioButton29.Checked == true) { psychosis_follow_recordbean.next_visit_classify = this.radioButton29.Text; };


            bool isfalse = psychiatricPatientS.aUPsychosis_follow_record(psychosis_follow_recordbean,id,goodsList);
            if (isfalse)
            {
                this.DialogResult = DialogResult.OK;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            follow_medicine_record hm = new follow_medicine_record();
            if (hm.ShowDialog() == DialogResult.OK)
            {
                DataRow[] drr = goodsList.Select("drug_name = '" + hm.drug_name.ToString() + "'");
                if (drr.Length > 0)
                {
                    MessageBox.Show("用药记录已存在！");
                    return;
                }
                DataRow drtmp = goodsList.NewRow();
                drtmp["id"] = 0;
                drtmp["follow_id"] = id;
                drtmp["drug_name"] = hm.drug_name.ToString();
                drtmp["num"] = hm.num.ToString();
                drtmp["dosage"] = hm.dosage.ToString();
                goodsList.Rows.Add(drtmp);
            }
            goodsListBind();
        }
        //将用药记录 goodsList 绑定到页面 dataGridView1展示出来
        private void goodsListBind()
        {

            this.dataGridView1.DataSource = goodsList;
            this.dataGridView1.Columns[0].Visible = false;//id
            this.dataGridView1.Columns[1].Visible = false;//follow_id
            this.dataGridView1.Columns[2].HeaderCell.Value = "药物名称";
            this.dataGridView1.Columns[3].HeaderCell.Value = "每日几次";
            this.dataGridView1.Columns[4].HeaderCell.Value = "每次用量";

            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                this.dataGridView1.SelectedRows[0].Selected = false;
            }
            if (goodsList != null && goodsList.Rows.Count > 0)
            {
                this.dataGridView1.Rows[goodsList.Rows.Count - 1].Selected = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (goodsList == null) { return; }
            if (goodsList.Rows.Count > 0)
            {
                goodsList.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                goodsListBind();
            }
        }
    }
}
