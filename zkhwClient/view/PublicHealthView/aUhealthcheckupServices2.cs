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
    public partial class aUhealthcheckupServices2 : Form
    {
        service.diabetesPatientService diabetesPatient = new service.diabetesPatientService();
        service.hypertensionPatientService hypertensionPatient = new service.hypertensionPatientService();
        public string id = "";
        DataTable goodsList = new DataTable();//用药记录清单表 follow_medicine_record
        public aUhealthcheckupServices2()
        {
            InitializeComponent();
        }
        private void aUdiabetesPatientServices_Load(object sender, EventArgs e)
        {
            this.label51.Text = "健康体检表第二页(共四页)";
            this.label51.ForeColor = Color.SkyBlue;
            label51.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label51.Left = (this.panel1.Width - this.label51.Width) / 2;
            label51.BringToFront();

            DataTable dt = hypertensionPatient.queryFollow_medicine_record(id);
            goodsList = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drtmp = goodsList.NewRow();
                drtmp["id"] = dt.Rows[i]["id"].ToString();
                drtmp["follow_id"] = dt.Rows[i]["follow_id"].ToString();
                drtmp["drug_name"] = dt.Rows[i]["drug_name"].ToString();
                drtmp["num"] = dt.Rows[i]["num"].ToString();
                drtmp["dosage"] = dt.Rows[i]["dosage"].ToString();
                goodsList.Rows.Add(drtmp);
            }

            goodsListBind();//加载用药记录清单表 follow_medicine_record
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();
            per.aichive_no = this.textBox95.Text;
            per.bar_code = this.textBox96.Text;

            if (this.radioButton55.Checked == true) { per.organ_lips = this.radioButton55.Tag.ToString(); };
            if (this.radioButton56.Checked == true) { per.organ_lips = this.radioButton56.Tag.ToString(); };
            if (this.radioButton57.Checked == true) { per.organ_lips = this.radioButton57.Tag.ToString(); };
            if (this.radioButton53.Checked == true) { per.organ_lips = this.radioButton53.Tag.ToString(); };
            if (this.radioButton54.Checked == true) { per.organ_lips = this.radioButton54.Tag.ToString(); };


            //foreach (Control ctr in this.panel2.Controls)
            //{
            //    //判断该控件是不是CheckBox
            //    if (ctr is CheckBox)
            //    {
            //        //将ctr转换成CheckBox并赋值给ck
            //        CheckBox ck = ctr as CheckBox;
            //        if (ck.Checked)
            //        {
            //            diabetes_follow_recordBean.symptom += "," + ck.Text;
            //        }
            //    }
            //}
            //if (diabetes_follow_recordBean.symptom != null && diabetes_follow_recordBean.symptom != "")
            //{
            //    diabetes_follow_recordBean.symptom = diabetes_follow_recordBean.symptom.Substring(1);
            //}
           // diabetes_follow_recordBean.symptom_other = this.richTextBox1.Text;

            //diabetes_follow_recordBean.blood_pressure_high = this.numericUpDown9.Value.ToString();
            //diabetes_follow_recordBean.blood_pressure_low = this.numericUpDown10.Value.ToString();
            //diabetes_follow_recordBean.weight_now = this.numericUpDown11.Value.ToString();
            //diabetes_follow_recordBean.weight_next = this.numericUpDown12.Value.ToString();
            //diabetes_follow_recordBean.bmi_now = this.numericUpDown14.Value.ToString();
            //diabetes_follow_recordBean.bmi_next = this.numericUpDown15.Value.ToString();
            //foreach (Control ctr in this.panel11.Controls)
            //{
            //    //判断该控件是不是CheckBox
            //    if (ctr is CheckBox)
            //    {
            //        //将ctr转换成CheckBox并赋值给ck
            //        CheckBox ck = ctr as CheckBox;
            //        if (ck.Checked)
            //        {
            //            diabetes_follow_recordBean.dorsal_artery += "," + ck.Text;
            //        }
            //    }
            //}

            //diabetes_follow_recordBean.other = this.richTextBox3.Text;

            //diabetes_follow_recordBean.smoke_now = this.numericUpDown1.Value.ToString();
            //diabetes_follow_recordBean.smoke_next = this.numericUpDown2.Value.ToString();
            //diabetes_follow_recordBean.drink_now = this.numericUpDown3.Value.ToString();
            //diabetes_follow_recordBean.drink_next = this.numericUpDown4.Value.ToString();
            //diabetes_follow_recordBean.sports_num_now = this.numericUpDown5.Value.ToString();
            //diabetes_follow_recordBean.sports_time_now = this.numericUpDown6.Value.ToString();
            //diabetes_follow_recordBean.sports_num_next = this.numericUpDown7.Value.ToString();
            //diabetes_follow_recordBean.sports_time_next = this.numericUpDown8.Value.ToString();
            //diabetes_follow_recordBean.staple_food_now = this.numericUpDown13.Value.ToString();
            //diabetes_follow_recordBean.staple_food_next = this.numericUpDown16.Value.ToString();

            //diabetes_follow_recordBean.blood_glucose = this.numericUpDown17.Value.ToString();
            //diabetes_follow_recordBean.glycosylated_hemoglobin = this.numericUpDown18.Value.ToString();
            //if (this.radioButton22.Checked == true) { diabetes_follow_recordBean.compliance = this.radioButton22.Text; };
            //if (this.radioButton23.Checked == true) { diabetes_follow_recordBean.compliance = this.radioButton23.Text; };
            //if (this.radioButton24.Checked == true) { diabetes_follow_recordBean.compliance = this.radioButton24.Text; };


            bool isfalse = diabetesPatient.aUdiabetesPatient(null, id, goodsList);
            if (isfalse)
            {
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
            aUhealthcheckupServices3 auhc3 = new aUhealthcheckupServices3();
            auhc3.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            more hm = new more();
            foreach (Control ctr in hm.panel1.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    //if (this.richTextBox1.Text.IndexOf(ck.Text) > -1)
                    //{
                    //    ck.Checked = true;
                    //}
                }
            }
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //this.richTextBox1.Text = hm.other_symptom;
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

            //this.dataGridView1.DataSource = goodsList;
            //this.dataGridView1.Columns[0].Visible = false;//id
            //this.dataGridView1.Columns[1].Visible = false;//follow_id
            //this.dataGridView1.Columns[2].HeaderCell.Value = "药物名称";
            //this.dataGridView1.Columns[3].HeaderCell.Value = "每日几次";
            //this.dataGridView1.Columns[4].HeaderCell.Value = "每次用量";

            //this.dataGridView1.ReadOnly = true;
            //this.dataGridView1.AllowUserToAddRows = false;
            //this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            //this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            //if (this.dataGridView1.SelectedRows.Count > 0)
            //{
            //    this.dataGridView1.SelectedRows[0].Selected = false;
            //}
            //if (goodsList != null && goodsList.Rows.Count > 0)
            //{
            //    this.dataGridView1.Rows[goodsList.Rows.Count - 1].Selected = true;
            //}
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (goodsList == null) { return; }
            if (goodsList.Rows.Count > 0)
            {
                //goodsList.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                //goodsListBind();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            more0 hm = new more0();
            foreach (Control ctr in hm.panel1.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    //if (this.richTextBox2.Text.IndexOf(ck.Text) > -1)
                    //{
                    //    ck.Checked = true;
                    //}
                }
            }
            if (hm.ShowDialog() == DialogResult.OK)
            {
                
            }
        }
    }
}
