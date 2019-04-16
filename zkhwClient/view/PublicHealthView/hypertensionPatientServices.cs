using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

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
        private void hypertensionPatientServices_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);

            this.label4.Text = "高血压随访记录历史表";
            this.label4.ForeColor = Color.SkyBlue;
            label4.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label4.Left = (this.panel1.Width - this.label4.Width) / 2;
            label4.BringToFront();

            //区域
            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 

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
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            queryhypertensionPatientServices();
        }
        //高血压随访记录历史表  关联传参调查询的方法
        private void queryhypertensionPatientServices()
        {
            this.dataGridView1.DataSource = null;
            DataTable dt = hypertensionPatient.queryHypertensionPatient(pCa, time1, time2, xcuncode);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[2].HeaderCell.Value = "年龄";
            this.dataGridView1.Columns[3].HeaderCell.Value = "建档医生";
            this.dataGridView1.Columns[4].HeaderCell.Value = "随访日期";
            this.dataGridView1.Columns[5].HeaderCell.Value = "下次随访日期";
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
            aUHypertensionPatientServices hm = new aUHypertensionPatientServices();
            hm.label47.Text = "添加高血压随访记录历史表";
            hm.Text = "添加高血压随访记录历史表";
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
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            hm.id = id;
            hm.label47.Text = "修改高血压随访记录历史表";
            hm.Text = "修改高血压随访记录历史表";
            DataTable dt = hypertensionPatient.queryHypertensionPatient0(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                hm.textBox1.Text = dt.Rows[0]["patientName"].ToString();
                hm.textBox2.Text = dt.Rows[0]["aichive_no"].ToString();
                hm.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["visit_date"].ToString());
                if (dt.Rows[0]["visit_type"].ToString() == hm.radioButton1.Text) { hm.radioButton1.Checked = true; };
                if (dt.Rows[0]["visit_type"].ToString() == hm.radioButton2.Text) { hm.radioButton2.Checked = true; };
                if (dt.Rows[0]["visit_type"].ToString() == hm.radioButton3.Text) { hm.radioButton3.Checked = true; };
                foreach (Control ctr in hm.panel2.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (dt.Rows[0]["symptom"].ToString().IndexOf(ck.Text) > -1)
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
                if (dt.Rows[0]["salt_intake"].ToString() == hm.radioButton4.Text) { hm.radioButton4.Checked = true; };
                if (dt.Rows[0]["salt_intake"].ToString() == hm.radioButton5.Text) { hm.radioButton5.Checked = true; };
                if (dt.Rows[0]["salt_intake"].ToString() == hm.radioButton6.Text) { hm.radioButton6.Checked = true; };
                if (dt.Rows[0]["target_salt_intake"].ToString() == hm.radioButton7.Text) { hm.radioButton7.Checked = true; };
                if (dt.Rows[0]["target_salt_intake"].ToString() == hm.radioButton8.Text) { hm.radioButton8.Checked = true; };
                if (dt.Rows[0]["target_salt_intake"].ToString() == hm.radioButton9.Text) { hm.radioButton9.Checked = true; };
                if (dt.Rows[0]["mind_adjust"].ToString() == hm.radioButton10.Text) { hm.radioButton10.Checked = true; };
                if (dt.Rows[0]["mind_adjust"].ToString() == hm.radioButton11.Text) { hm.radioButton11.Checked = true; };
                if (dt.Rows[0]["mind_adjust"].ToString() == hm.radioButton12.Text) { hm.radioButton12.Checked = true; };
                if (dt.Rows[0]["doctor_obey"].ToString() == hm.radioButton13.Text) { hm.radioButton13.Checked = true; };
                if (dt.Rows[0]["doctor_obey"].ToString() == hm.radioButton14.Text) { hm.radioButton14.Checked = true; };
                if (dt.Rows[0]["doctor_obey"].ToString() == hm.radioButton15.Text) { hm.radioButton15.Checked = true; };

                hm.textBox3.Text = dt.Rows[0]["assist_examine"].ToString();
                if (dt.Rows[0]["drug_obey"].ToString() == hm.radioButton22.Text) { hm.radioButton22.Checked = true; };
                if (dt.Rows[0]["drug_obey"].ToString() == hm.radioButton23.Text) { hm.radioButton23.Checked = true; };
                if (dt.Rows[0]["drug_obey"].ToString() == hm.radioButton24.Text) { hm.radioButton24.Checked = true; };
                if (dt.Rows[0]["untoward_effect"].ToString() == hm.radioButton16.Text) { hm.radioButton16.Checked = true; };
                if (dt.Rows[0]["untoward_effect"].ToString() == hm.radioButton17.Text) { hm.radioButton17.Checked = true; };
                hm.textBox8.Text = dt.Rows[0]["untoward_effect_drug"].ToString();
                if (dt.Rows[0]["visit_class"].ToString() == hm.radioButton18.Text) { hm.radioButton18.Checked = true; };
                if (dt.Rows[0]["visit_class"].ToString() == hm.radioButton19.Text) { hm.radioButton19.Checked = true; };
                if (dt.Rows[0]["visit_class"].ToString() == hm.radioButton20.Text) { hm.radioButton20.Checked = true; };
                if (dt.Rows[0]["visit_class"].ToString() == hm.radioButton21.Text) { hm.radioButton21.Checked = true; };
                hm.richTextBox2.Text = dt.Rows[0]["advice"].ToString();

                hm.textBox5.Text = dt.Rows[0]["transfer_reason"].ToString();
                hm.textBox6.Text = dt.Rows[0]["transfer_organ"].ToString();
                hm.dateTimePicker2.Value = DateTime.Parse(dt.Rows[0]["next_visit_date"].ToString());
                hm.textBox7.Text = dt.Rows[0]["visit_doctor"].ToString();

            }
            else { }



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
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

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
            shengcode = this.comboBox1.SelectedValue.ToString();
            this.comboBox2.DataSource = areadao.shiInfo(shengcode);//绑定数据源
            this.comboBox2.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox2.ValueMember = "code";//操作时获取的值 
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            shicode = this.comboBox2.SelectedValue.ToString();
            this.comboBox3.DataSource = areadao.quxianInfo(shicode);//绑定数据源
            this.comboBox3.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox3.ValueMember = "code";//操作时获取的值 
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            qxcode = this.comboBox3.SelectedValue.ToString();
            this.comboBox4.DataSource = areadao.zhenInfo(qxcode);//绑定数据源
            this.comboBox4.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox4.ValueMember = "code";//操作时获取的值 
            this.comboBox5.DataSource = null;
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            xzcode = this.comboBox4.SelectedValue.ToString();
            this.comboBox5.DataSource = areadao.cunInfo(xzcode);//绑定数据源
            this.comboBox5.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox5.ValueMember = "code";//操作时获取的值
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            xcuncode = this.comboBox5.SelectedValue.ToString();
        }
    }
}
