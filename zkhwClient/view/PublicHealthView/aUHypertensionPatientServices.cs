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
    public partial class aUHypertensionPatientServices : Form
    {
        service.hypertensionPatientService hypertensionPatient = new service.hypertensionPatientService();
        public string id = "";
        DataTable goodsList = new DataTable();//用药记录清单表 follow_medicine_record
        public aUHypertensionPatientServices()
        {
            InitializeComponent();
        }
        private void aUHypertensionPatientServices_Load(object sender, EventArgs e)
        {
            this.label47.ForeColor = Color.SkyBlue;
            label47.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label47.Left = (this.panel1.Width - this.label47.Width) / 2;
            label47.BringToFront();

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
            if (goodsList != null && goodsList.Rows.Count > 0) {
                this.dataGridView1.Rows[goodsList.Rows.Count - 1].Selected = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bean.fuv_hypertensionBean fuv_hypertensionBean = new bean.fuv_hypertensionBean();

            fuv_hypertensionBean.name = this.textBox1.Text.Replace(" ", "");
            fuv_hypertensionBean.aichive_no = this.textBox2.Text.Replace(" ", "");
            fuv_hypertensionBean.visit_date = this.dateTimePicker1.Text.ToString();
            if (this.radioButton1.Checked == true) { fuv_hypertensionBean.visit_type = this.radioButton1.Tag.ToString(); };
            if (this.radioButton2.Checked == true) { fuv_hypertensionBean.visit_type = this.radioButton2.Tag.ToString(); };
            if (this.radioButton3.Checked == true) { fuv_hypertensionBean.visit_type = this.radioButton3.Tag.ToString(); };
            foreach (Control ctr in this.panel2.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        fuv_hypertensionBean.symptom += "," + ck.Tag.ToString();
                    }
                }
            }
            if (fuv_hypertensionBean.symptom != null && fuv_hypertensionBean.symptom != "")
            {
                fuv_hypertensionBean.symptom = fuv_hypertensionBean.symptom.Substring(1);
            }
            fuv_hypertensionBean.other_symptom = this.richTextBox1.Text;

            fuv_hypertensionBean.sbp = this.numericUpDown9.Value.ToString();
            fuv_hypertensionBean.dbp = this.numericUpDown10.Value.ToString();
            fuv_hypertensionBean.weight = this.numericUpDown11.Value.ToString();
            fuv_hypertensionBean.target_weight = this.numericUpDown12.Value.ToString();
            fuv_hypertensionBean.bmi = this.numericUpDown14.Value.ToString();
            fuv_hypertensionBean.target_bmi = this.numericUpDown15.Value.ToString();
            fuv_hypertensionBean.heart_rate = this.numericUpDown16.Value.ToString();
            fuv_hypertensionBean.other_sign = this.richTextBox3.Text;

            fuv_hypertensionBean.smoken = this.numericUpDown1.Value.ToString();
            fuv_hypertensionBean.target_somken = this.numericUpDown2.Value.ToString();
            fuv_hypertensionBean.wine = this.numericUpDown3.Value.ToString();
            fuv_hypertensionBean.target_wine = this.numericUpDown4.Value.ToString();
            fuv_hypertensionBean.sport_week = this.numericUpDown5.Value.ToString();
            fuv_hypertensionBean.sport_once = this.numericUpDown6.Value.ToString();
            fuv_hypertensionBean.target_sport_week = this.numericUpDown7.Value.ToString();
            fuv_hypertensionBean.target_sport_once = this.numericUpDown8.Value.ToString();
            if (this.radioButton4.Checked == true) { fuv_hypertensionBean.salt_intake = this.radioButton4.Tag.ToString(); };
            if (this.radioButton5.Checked == true) { fuv_hypertensionBean.salt_intake = this.radioButton5.Tag.ToString(); };
            if (this.radioButton6.Checked == true) { fuv_hypertensionBean.salt_intake = this.radioButton6.Tag.ToString(); };
            if (this.radioButton7.Checked == true) { fuv_hypertensionBean.target_salt_intake = this.radioButton7.Tag.ToString(); };
            if (this.radioButton8.Checked == true) { fuv_hypertensionBean.target_salt_intake = this.radioButton8.Tag.ToString(); };
            if (this.radioButton9.Checked == true) { fuv_hypertensionBean.target_salt_intake = this.radioButton9.Tag.ToString(); };
            if (this.radioButton10.Checked == true) { fuv_hypertensionBean.mind_adjust = this.radioButton10.Tag.ToString(); };
            if (this.radioButton11.Checked == true) { fuv_hypertensionBean.mind_adjust = this.radioButton11.Tag.ToString(); };
            if (this.radioButton12.Checked == true) { fuv_hypertensionBean.mind_adjust = this.radioButton12.Tag.ToString(); };
            if (this.radioButton13.Checked == true) { fuv_hypertensionBean.doctor_obey = this.radioButton13.Tag.ToString(); };
            if (this.radioButton14.Checked == true) { fuv_hypertensionBean.doctor_obey = this.radioButton14.Tag.ToString(); };
            if (this.radioButton15.Checked == true) { fuv_hypertensionBean.doctor_obey = this.radioButton15.Tag.ToString(); };

            fuv_hypertensionBean.assist_examine = this.textBox3.Text.Replace(" ", "");
            if (this.radioButton22.Checked == true) { fuv_hypertensionBean.drug_obey = this.radioButton22.Tag.ToString(); };
            if (this.radioButton23.Checked == true) { fuv_hypertensionBean.drug_obey = this.radioButton23.Tag.ToString(); };
            if (this.radioButton24.Checked == true) { fuv_hypertensionBean.drug_obey = this.radioButton24.Tag.ToString(); };

            if (this.radioButton16.Checked == true) { fuv_hypertensionBean.untoward_effect = this.radioButton16.Tag.ToString(); };
            if (this.radioButton17.Checked == true) { fuv_hypertensionBean.untoward_effect = this.radioButton17.Tag.ToString(); };
            fuv_hypertensionBean.untoward_effect_drug = this.textBox8.Text.Replace(" ", "");

            if (this.radioButton18.Checked == true) { fuv_hypertensionBean.visit_class = this.radioButton18.Tag.ToString(); };
            if (this.radioButton19.Checked == true) { fuv_hypertensionBean.visit_class = this.radioButton19.Tag.ToString(); };
            if (this.radioButton20.Checked == true) { fuv_hypertensionBean.visit_class = this.radioButton20.Tag.ToString(); };
            if (this.radioButton21.Checked == true) { fuv_hypertensionBean.visit_class = this.radioButton21.Tag.ToString(); };
            if (fuv_hypertensionBean.visit_class == "") { MessageBox.Show("随访分类不能为空！"); return; };
            fuv_hypertensionBean.advice = this.richTextBox2.Text;

            fuv_hypertensionBean.transfer_reason = this.textBox5.Text.Replace(" ", "");
            fuv_hypertensionBean.transfer_organ = this.textBox6.Text.Replace(" ", "");
            fuv_hypertensionBean.next_visit_date = this.dateTimePicker2.Text.ToString();
            fuv_hypertensionBean.visit_doctor = this.textBox7.Text.Replace(" ", "");


            //以下页面未用 数据库字段格式要求
            fuv_hypertensionBean.create_time = DateTime.Now.ToString("yyyy-MM-dd");
            fuv_hypertensionBean.update_time = DateTime.Now.ToString("yyyy-MM-dd");

            bool isfalse = hypertensionPatient.aUfuv_hypertension(fuv_hypertensionBean, id, goodsList);
            if (isfalse)
            {
                this.DialogResult = DialogResult.OK;
            }
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
                    if (this.richTextBox1.Text.IndexOf(ck.Text) > -1)
                    {
                        ck.Checked = true;
                    }
                }
            }
            if (hm.ShowDialog() == DialogResult.OK)
            {
                this.richTextBox1.Text = hm.other_symptom;
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
                    if (this.richTextBox2.Text.IndexOf(ck.Text) > -1)
                    {
                        ck.Checked = true;
                    }
                }
            }
            if (hm.ShowDialog() == DialogResult.OK)
            {
                this.richTextBox2.Text = hm.advice;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (goodsList == null) { return; }
            if (goodsList.Rows.Count > 0)
            {
                goodsList.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                goodsListBind();
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

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
                this.checkBox5.Checked = false;
                this.checkBox6.Checked = false;
                this.checkBox7.Checked = false;
                this.checkBox8.Checked = false;
                this.checkBox9.Checked = false;
            }
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox5_Click(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox6_Click(object sender, EventArgs e)
        {
            if (this.checkBox6.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox7_Click(object sender, EventArgs e)
        {
            if (this.checkBox7.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox8_Click(object sender, EventArgs e)
        {
            if (this.checkBox8.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox9_Click(object sender, EventArgs e)
        {
            if (this.checkBox9.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void radioButton16_Click(object sender, EventArgs e)
        {
            this.textBox8.Enabled = false;
        }

        private void radioButton17_Click(object sender, EventArgs e)
        {
            this.textBox8.Enabled = true;
        }
    }
}
