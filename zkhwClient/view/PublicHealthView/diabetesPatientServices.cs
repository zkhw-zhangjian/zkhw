using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class diabetesPatientServices : Form
    {
        service.diabetesPatientService diabetesPatient = new service.diabetesPatientService();
        public string pCa = "";
        public string time1 = null;
        public string time2 = null;
        string xcuncode = "";
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        areaConfigDao areadao = new areaConfigDao();
        public diabetesPatientServices()
        {
            InitializeComponent();
        }
        private void diabetesPatientServices_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            this.label4.Text = "糖尿病患者服务";
            this.label4.ForeColor = Color.SkyBlue;
            label4.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label4.Left = (this.panel1.Width - this.label4.Width) / 2;
            label4.BringToFront();

            //区域
            Common.SetComboBoxInfo(comboBox1, areadao.shengInfo());
            xcuncode = basicInfoSettings.xcuncode;
            querydiabetesPatientServices();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;//patientName Cardcode aichive_no
            if (pCa != "")
            {
                this.label2.Text = "";
            }
            else { this.label2.Text = "---姓名/身份证号/档案号---"; }
            
            querydiabetesPatientServices();
        }
        private void querydiabetesPatientServices()
        {
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            this.dataGridView1.DataSource = null;
            DataTable dt = diabetesPatient.querydiabetesPatient(pCa, time1, time2,xcuncode);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[1].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "随访类型";
            this.dataGridView1.Columns[4].HeaderCell.Value = "随访日期";
            this.dataGridView1.Columns[5].HeaderCell.Value = "随访医生";
            this.dataGridView1.Columns[6].Visible = false;

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
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string archiveno = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string idnumber = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string id = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            DataTable dt = diabetesPatient.queryDiabetesPatient0(id);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("此身份信息已参加过了!");
                return;
            }
            aUdiabetesPatientServices hm = new aUdiabetesPatientServices();
            hm.label51.Text = "添加糖尿病患者服务";
            hm.Text = "添加糖尿病患者服务";
            hm.textBox1.Text = name;
            hm.textBox2.Text = archiveno;
            hm.textBox51.Text = idnumber;
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                querydiabetesPatientServices();
                MessageBox.Show("添加成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            aUdiabetesPatientServices dp = new aUdiabetesPatientServices();
            string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            dp.id = id;
            dp.label51.Text = "修改糖尿病患者服务";
            dp.Text = "修改糖尿病患者服务";

            DataTable dt = diabetesPatient.queryDiabetesPatient0(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                dp.textBox1.Text = dt.Rows[0]["name"].ToString();
                dp.textBox2.Text = dt.Rows[0]["aichive_no"].ToString();
                dp.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["visit_date"].ToString());
                if (dt.Rows[0]["visit_type"].ToString() == dp.radioButton1.Tag.ToString()) { dp.radioButton1.Checked = true; };
                if (dt.Rows[0]["visit_type"].ToString() == dp.radioButton2.Tag.ToString()) { dp.radioButton2.Checked = true; };
                if (dt.Rows[0]["visit_type"].ToString() == dp.radioButton3.Tag.ToString()) { dp.radioButton3.Checked = true; };
                foreach (Control ctr in dp.panel2.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (dt.Rows[0]["symptom"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                        {
                            ck.Checked = true;
                        }
                    }
                }
                dp.richTextBox1.Text = dt.Rows[0]["symptom_other"].ToString();

                dp.numericUpDown9.Value = Decimal.Parse(dt.Rows[0]["blood_pressure_high"].ToString());
                dp.numericUpDown10.Value = Decimal.Parse(dt.Rows[0]["blood_pressure_low"].ToString());
                dp.numericUpDown11.Value = Decimal.Parse(dt.Rows[0]["weight_now"].ToString());
                dp.numericUpDown12.Value = Decimal.Parse(dt.Rows[0]["weight_next"].ToString());
                dp.numericUpDown14.Value = Decimal.Parse(dt.Rows[0]["bmi_now"].ToString());
                dp.numericUpDown15.Value = Decimal.Parse(dt.Rows[0]["bmi_next"].ToString());
                foreach (Control ctr in dp.panel11.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (dt.Rows[0]["dorsal_artery"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                        {
                            ck.Checked = true;
                        }
                    }
                }
                dp.richTextBox3.Text = dt.Rows[0]["other"].ToString();

                dp.numericUpDown1.Value = Decimal.Parse(dt.Rows[0]["smoke_now"].ToString());
                dp.numericUpDown2.Value = Decimal.Parse(dt.Rows[0]["smoke_next"].ToString());
                dp.numericUpDown3.Value = Decimal.Parse(dt.Rows[0]["drink_now"].ToString());
                dp.numericUpDown4.Value = Decimal.Parse(dt.Rows[0]["drink_next"].ToString());
                dp.numericUpDown5.Value = Decimal.Parse(dt.Rows[0]["sports_num_now"].ToString());
                dp.numericUpDown6.Value = Decimal.Parse(dt.Rows[0]["sports_time_now"].ToString());
                dp.numericUpDown7.Value = Decimal.Parse(dt.Rows[0]["sports_num_next"].ToString());
                dp.numericUpDown8.Value = Decimal.Parse(dt.Rows[0]["sports_time_next"].ToString());
                dp.numericUpDown13.Value = Decimal.Parse(dt.Rows[0]["staple_food_now"].ToString());
                dp.numericUpDown16.Value = Decimal.Parse(dt.Rows[0]["staple_food_next"].ToString());
                if (dt.Rows[0]["psychological_recovery"].ToString() == dp.radioButton10.Tag.ToString()) { dp.radioButton10.Checked = true; };
                if (dt.Rows[0]["psychological_recovery"].ToString() == dp.radioButton11.Tag.ToString()) { dp.radioButton11.Checked = true; };
                if (dt.Rows[0]["psychological_recovery"].ToString() == dp.radioButton12.Tag.ToString()) { dp.radioButton12.Checked = true; };
                if (dt.Rows[0]["medical_compliance"].ToString() == dp.radioButton13.Tag.ToString()) { dp.radioButton13.Checked = true; };
                if (dt.Rows[0]["medical_compliance"].ToString() == dp.radioButton14.Tag.ToString()) { dp.radioButton14.Checked = true; };
                if (dt.Rows[0]["medical_compliance"].ToString() == dp.radioButton15.Tag.ToString()) { dp.radioButton15.Checked = true; };

                dp.numericUpDown17.Value = Decimal.Parse(dt.Rows[0]["blood_glucose"].ToString());
                dp.numericUpDown18.Value = Decimal.Parse(dt.Rows[0]["glycosylated_hemoglobin"].ToString());
                dp.dateTimePicker3.Value= DateTime.Parse(dt.Rows[0]["check_date"].ToString());
                if (dt.Rows[0]["compliance"].ToString() == dp.radioButton22.Tag.ToString()) { dp.radioButton22.Checked = true; };
                if (dt.Rows[0]["compliance"].ToString() == dp.radioButton23.Tag.ToString()) { dp.radioButton23.Checked = true; };
                if (dt.Rows[0]["compliance"].ToString() == dp.radioButton24.Tag.ToString()) { dp.radioButton24.Checked = true; };
                if (dt.Rows[0]["untoward_effect"].ToString() == dp.radioButton16.Tag.ToString()) { dp.radioButton16.Checked = true; };
                if (dt.Rows[0]["untoward_effect"].ToString() == dp.radioButton17.Tag.ToString()) { dp.radioButton17.Checked = true; };
                if (dt.Rows[0]["reactive_hypoglycemia"].ToString() == dp.radioButton4.Tag.ToString()) { dp.radioButton4.Checked = true; };
                if (dt.Rows[0]["reactive_hypoglycemia"].ToString() == dp.radioButton5.Tag.ToString()) { dp.radioButton5.Checked = true; };
                if (dt.Rows[0]["reactive_hypoglycemia"].ToString() == dp.radioButton6.Tag.ToString()) { dp.radioButton6.Checked = true; };

                if (dt.Rows[0]["follow_type"].ToString() == dp.radioButton18.Tag.ToString()) { dp.radioButton18.Checked = true; };
                if (dt.Rows[0]["follow_type"].ToString() == dp.radioButton19.Tag.ToString()) { dp.radioButton19.Checked = true; };
                if (dt.Rows[0]["follow_type"].ToString() == dp.radioButton20.Tag.ToString()) { dp.radioButton20.Checked = true; };
                if (dt.Rows[0]["follow_type"].ToString() == dp.radioButton21.Tag.ToString()) { dp.radioButton21.Checked = true; };
                dp.richTextBox2.Text = dt.Rows[0]["advice"].ToString();

                dp.textBox5.Text = dt.Rows[0]["transfer_treatment_reason"].ToString();
                dp.textBox6.Text = dt.Rows[0]["transfer_treatment_department"].ToString();
                dp.dateTimePicker2.Value = DateTime.Parse(dt.Rows[0]["next_visit_date"].ToString());
                dp.textBox7.Text = dt.Rows[0]["visit_doctor"].ToString();

            }
            else { MessageBox.Show("没有此人的糖尿病服务信息,请先添加糖尿病服务信息!"); return; }
            if (dp.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                querydiabetesPatientServices();
                MessageBox.Show("修改成功！");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();

            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
                bool istrue = diabetesPatient.deleteDiabetesPatient(id);
                if (istrue)
                {
                    //刷新页面
                    querydiabetesPatientServices();
                    MessageBox.Show("删除成功！");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label2.Visible = this.textBox1.Text.Length < 1;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox2.DataSource = null;
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox1.SelectedValue == null) return;
            shengcode = this.comboBox1.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox2, areadao.shiInfo(shengcode)); 
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox2.SelectedValue == null) return;
            shicode = this.comboBox2.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox3, areadao.quxianInfo(shicode));
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox3.SelectedValue == null) return;
            qxcode = this.comboBox3.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox4, areadao.zhenInfo(qxcode));
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox5.DataSource = null;
            if (this.comboBox4.SelectedValue == null) return;
            xzcode = this.comboBox4.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox5, areadao.cunInfo(xzcode));
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBox5.SelectedValue == null) return;
            xcuncode = this.comboBox5.SelectedValue.ToString(); 
        }
    }
}
