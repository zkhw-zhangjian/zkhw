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
    public partial class hypertensionPatientServices : Form
    {
        service.hypertensionPatientService hypertensionPatient = new service.hypertensionPatientService();
        areaConfigDao areadao = new areaConfigDao();
        public string pCa= "";
        public string time1 = null;
        public string time2 = null;
        string xcuncode = "";
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        public hypertensionPatientServices()
        {
            InitializeComponent();
        }
        private void SetControlWodth()
        {
            if (Common.m_nWindwMetricsY <= 900)
            {
                comboBox3.Width = 120;
                comboBox3.Left = comboBox2.Left + comboBox2.Width + 5;
                comboBox4.Width = 120;
                comboBox4.Left = comboBox3.Left + comboBox3.Width + 5;
                comboBox5.Width = 120;
                comboBox5.Left = comboBox4.Left + comboBox4.Width + 5;
            }
        }
        private void hypertensionPatientServices_Load(object sender, EventArgs e)
        {
            SetControlWodth();
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular);
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);

           

            //区域
            Common.SetComboBoxInfo(comboBox1, areadao.shengInfo());

            xcuncode = basicInfoSettings.xcuncode;
            queryhypertensionPatientServices();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;//patientName Cardcode aichive_no
            if (pCa != "")
            {
                this.label2.Text = "";
            }
            else { this.label2.Text = "---姓名/身份证号/档案号---"; }

            queryhypertensionPatientServices();
        }
        //高血压随访记录历史表  关联传参调查询的方法
        private void queryhypertensionPatientServices()
        {
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            this.dataGridView1.DataSource = null;
            DataTable dt = hypertensionPatient.queryHypertensionPatient(pCa, time1, time2, xcuncode);
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
            //方案一
            //aUHypertensionPatientServices hm = new aUHypertensionPatientServices();
            //hm.TopLevel = false;
            //hm.Dock = DockStyle.Fill;
            //hm.FormBorderStyle = FormBorderStyle.None;
            //this.panel2.Visible = false;//高血压随访历史记录表主页面  
            //this.panel3.Visible = true;  //添加页面
            //this.panel3.Size = this.panel2.Size;
            //hm.Size = this.panel3.Size;
            //this.panel3.Controls.Add(hm);
            //hm.Show();
            //方案二
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string archiveno = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string idnumber = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string id = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            DataTable dt = hypertensionPatient.queryHypertensionPatient0(id);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("此身份信息已参加过了!");
                return;
            }
            aUHypertensionPatientServices hm = new aUHypertensionPatientServices();
            hm.label47.Text = "添加高血压随访记录历史表";
            hm.Text = "添加高血压随访记录历史表";
            hm.textBox1.Text = name;
            hm.textBox2.Text = archiveno;
            hm.textBox49.Text = idnumber;
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                queryhypertensionPatientServices();
                MessageBox.Show("添加成功！");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            aUHypertensionPatientServices hm = new aUHypertensionPatientServices();
            string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            hm.id = id;
            hm.label47.Text = "修改高血压随访记录历史表";
            hm.Text = "修改高血压随访记录历史表";
            DataTable dt = hypertensionPatient.queryHypertensionPatient0(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                hm.textBox1.Text = dt.Rows[0]["name"].ToString();
                hm.textBox2.Text = dt.Rows[0]["aichive_no"].ToString();
                hm.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["visit_date"].ToString());
                if (dt.Rows[0]["visit_type"].ToString() == hm.radioButton1.Tag.ToString()) { hm.radioButton1.Checked = true; };
                if (dt.Rows[0]["visit_type"].ToString() == hm.radioButton2.Tag.ToString()) { hm.radioButton2.Checked = true; };
                if (dt.Rows[0]["visit_type"].ToString() == hm.radioButton3.Tag.ToString()) { hm.radioButton3.Checked = true; };
                foreach (Control ctr in hm.panel2.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (dt.Rows[0]["symptom"].ToString().IndexOf(ck.Tag.ToString())>-1)
                        {
                            ck.Checked = true;
                        }
                    }
                }
                hm.richTextBox1.Text = dt.Rows[0]["other_symptom"].ToString();

                hm.numericUpDown9.Value = Decimal.Parse(dt.Rows[0]["sbp"].ToString());
                hm.numericUpDown10.Value = Decimal.Parse(dt.Rows[0]["dbp"].ToString());
                hm.numericUpDown11.Value = Decimal.Parse(dt.Rows[0]["weight"].ToString());
                hm.numericUpDown12.Value = Decimal.Parse(dt.Rows[0]["target_weight"].ToString());
                hm.numericUpDown14.Value = Decimal.Parse(dt.Rows[0]["bmi"].ToString());
                hm.numericUpDown15.Value = Decimal.Parse(dt.Rows[0]["target_bmi"].ToString());
                hm.numericUpDown16.Value = Decimal.Parse(dt.Rows[0]["heart_rate"].ToString());
                hm.richTextBox3.Text = dt.Rows[0]["other_sign"].ToString();

                hm.numericUpDown1.Value = Decimal.Parse(dt.Rows[0]["smoken"].ToString());
                hm.numericUpDown2.Value = Decimal.Parse(dt.Rows[0]["target_somken"].ToString());
                hm.numericUpDown3.Value = Decimal.Parse(dt.Rows[0]["wine"].ToString());
                hm.numericUpDown4.Value = Decimal.Parse(dt.Rows[0]["target_wine"].ToString());
                hm.numericUpDown5.Value = Decimal.Parse(dt.Rows[0]["sport_week"].ToString());
                hm.numericUpDown6.Value = Decimal.Parse(dt.Rows[0]["sport_once"].ToString());
                hm.numericUpDown7.Value = Decimal.Parse(dt.Rows[0]["target_sport_week"].ToString());
                hm.numericUpDown8.Value = Decimal.Parse(dt.Rows[0]["target_sport_once"].ToString());
                if (dt.Rows[0]["salt_intake"].ToString() == hm.radioButton4.Tag.ToString()) { hm.radioButton4.Checked = true; };
                if (dt.Rows[0]["salt_intake"].ToString() == hm.radioButton5.Tag.ToString()) { hm.radioButton5.Checked = true; };
                if (dt.Rows[0]["salt_intake"].ToString() == hm.radioButton6.Tag.ToString()) { hm.radioButton6.Checked = true; };
                if (dt.Rows[0]["target_salt_intake"].ToString() == hm.radioButton7.Tag.ToString()) { hm.radioButton7.Checked = true; };
                if (dt.Rows[0]["target_salt_intake"].ToString() == hm.radioButton8.Tag.ToString()) { hm.radioButton8.Checked = true; };
                if (dt.Rows[0]["target_salt_intake"].ToString() == hm.radioButton9.Tag.ToString()) { hm.radioButton9.Checked = true; };
                if (dt.Rows[0]["mind_adjust"].ToString() == hm.radioButton10.Tag.ToString()) { hm.radioButton10.Checked = true; };
                if (dt.Rows[0]["mind_adjust"].ToString() == hm.radioButton11.Tag.ToString()) { hm.radioButton11.Checked = true; };
                if (dt.Rows[0]["mind_adjust"].ToString() == hm.radioButton12.Tag.ToString()) { hm.radioButton12.Checked = true; };
                if (dt.Rows[0]["doctor_obey"].ToString() == hm.radioButton13.Tag.ToString()) { hm.radioButton13.Checked = true; };
                if (dt.Rows[0]["doctor_obey"].ToString() == hm.radioButton14.Tag.ToString()) { hm.radioButton14.Checked = true; };
                if (dt.Rows[0]["doctor_obey"].ToString() == hm.radioButton15.Tag.ToString()) { hm.radioButton15.Checked = true; };

                hm.textBox3.Text = dt.Rows[0]["assist_examine"].ToString();
                if (dt.Rows[0]["drug_obey"].ToString() == hm.radioButton22.Tag.ToString()) { hm.radioButton22.Checked = true; };
                if (dt.Rows[0]["drug_obey"].ToString() == hm.radioButton23.Tag.ToString()) { hm.radioButton23.Checked = true; };
                if (dt.Rows[0]["drug_obey"].ToString() == hm.radioButton24.Tag.ToString()) { hm.radioButton24.Checked = true; };
                if (dt.Rows[0]["untoward_effect"].ToString() == hm.radioButton16.Tag.ToString()) { hm.radioButton16.Checked = true; };
                if (dt.Rows[0]["untoward_effect"].ToString() == hm.radioButton17.Tag.ToString()) { hm.radioButton17.Checked = true; };
                hm.textBox8.Text = dt.Rows[0]["untoward_effect_drug"].ToString();
                if (dt.Rows[0]["visit_class"].ToString() == hm.radioButton18.Tag.ToString()) { hm.radioButton18.Checked = true; };
                if (dt.Rows[0]["visit_class"].ToString() == hm.radioButton19.Tag.ToString()) { hm.radioButton19.Checked = true; };
                if (dt.Rows[0]["visit_class"].ToString() == hm.radioButton20.Tag.ToString()) { hm.radioButton20.Checked = true; };
                if (dt.Rows[0]["visit_class"].ToString() == hm.radioButton21.Tag.ToString()) { hm.radioButton21.Checked = true; };
                hm.richTextBox2.Text = dt.Rows[0]["advice"].ToString();

                hm.textBox5.Text = dt.Rows[0]["transfer_reason"].ToString();
                hm.textBox6.Text = dt.Rows[0]["transfer_organ"].ToString();
                hm.dateTimePicker2.Value = DateTime.Parse(dt.Rows[0]["next_visit_date"].ToString());
                hm.textBox7.Text = dt.Rows[0]["visit_doctor"].ToString();

            }
            else { MessageBox.Show("未查询出高血压服务数据，请先添加!"); return; }
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                queryhypertensionPatientServices();
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
                bool istrue = hypertensionPatient.deleteHypertensionPatient(id);
                if (istrue)
                {
                    //刷新页面
                    queryhypertensionPatientServices();
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

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);
            Font font = new Font("微软雅黑", 14F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, "添加", font, bush); 
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);
            Font font = new Font("微软雅黑", 14F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, "修改", font, bush); 
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);
            Font font = new Font("微软雅黑", 14F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, "删除", font, bush);
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);
            Font font = new Font("微软雅黑", 12F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, "查询", font, bush); 
        }
    }
}
