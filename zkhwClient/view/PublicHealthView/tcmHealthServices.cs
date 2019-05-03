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
    public partial class tcmHealthServices : Form
    {

        service.tcmHealthService tcmHealthService = new service.tcmHealthService();
        //高血压随访记录历史表  关联传参调查询的方法
        //public string name = "";
        //public string id_number = "";
        //public string aichive_no = "";
        public string pCa = "";
        public string time1 = null;
        public string time2 = null;
        public tcmHealthServices()
        {
            InitializeComponent();
        }
        private void tcmHealthServices_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            this.label4.Text = "中医体质辨识记录表";
            this.label4.ForeColor = Color.SkyBlue;
            label4.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label4.Left = (this.panel1.Width - this.label4.Width) / 2;
            label4.BringToFront();
            querytcmHealthServices();
            #region 区域数据绑定
            string sql1 = "select code as ID,name as Name from code_area_config where parent_code='-1';";
            DataSet datas = DbHelperMySQL.Query(sql1);
            if (datas != null && datas.Tables.Count > 0)
            {
                List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(datas.Tables[0]);
                Result.Bind(comboBox1, ts, "Name", "ID", "--请选择--");
            }
            #endregion
        }
        private void button5_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;//patientName Cardcode aichive_no
            if (pCa != "")
            {
                this.label2.Text = "";
            }
            else { this.label2.Text = "---姓名/身份证号/档案号---"; }
            //time1 = this.dateTimePicker1.Text.ToString();//开始时间
            //time2 = this.dateTimePicker2.Text.ToString();//结束时间
            querytcmHealthServices();

        }
        private void querytcmHealthServices()
        {
            this.dataGridView1.DataSource = null;
            //DataTable dt = tcmHealthService.querytcmHealthServices(pCa, time1, time2);
            this.dataGridView1.DataSource = GetData();
            //this.dataGridView1.Columns[0].Visible = false;
            //this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
            //this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            //this.dataGridView1.Columns[3].HeaderCell.Value = "档案编号";
            //this.dataGridView1.Columns[4].HeaderCell.Value = "测试日期";
            //this.dataGridView1.Columns[5].HeaderCell.Value = "测试医生";
            //this.dataGridView1.Columns[6].HeaderCell.Value = "数据状态";

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
            int row = dataGridView1.CurrentRow.Index;
            addtcmHealthServices addtcm = new addtcmHealthServices(1, dataGridView1["姓名", row].Value.ToString(), dataGridView1["编码", row].Value.ToString(), dataGridView1["身份证号", row].Value.ToString());
            addtcm.StartPosition = FormStartPosition.CenterScreen;
            addtcm.ShowDialog();
            //aUdiabetesPatientServices hm = new aUdiabetesPatientServices();
            //hm.label51.Text = "添加糖尿病患者服务";
            //hm.Text = "添加糖尿病患者服务";
            //if (hm.ShowDialog() == DialogResult.OK)
            //{
            //    //刷新页面
            //    querytcmHealthServices();
            //    MessageBox.Show("添加成功！");

            //}
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            addtcmHealthServices addtcm = new addtcmHealthServices(0, dataGridView1["姓名", row].Value.ToString(), dataGridView1["编码", row].Value.ToString(), dataGridView1["身份证号", row].Value.ToString());
            addtcm.StartPosition = FormStartPosition.CenterScreen;
            addtcm.ShowDialog();
            //if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            //aUdiabetesPatientServices dp = new aUdiabetesPatientServices();
            //string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            //dp.id = id;
            //dp.label51.Text = "修改糖尿病患者服务";
            //dp.Text = "修改糖尿病患者服务";

            //DataTable dt = diabetesPatient.queryDiabetesPatient0(id);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    dp.textBox1.Text = dt.Rows[0]["name"].ToString();
            //    dp.textBox2.Text = dt.Rows[0]["aichive_no"].ToString();
            //    dp.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["visit_date"].ToString());
            //    if (dt.Rows[0]["visit_type"].ToString() == dp.radioButton1.Text) { dp.radioButton1.Checked = true; };
            //    if (dt.Rows[0]["visit_type"].ToString() == dp.radioButton2.Text) { dp.radioButton2.Checked = true; };
            //    if (dt.Rows[0]["visit_type"].ToString() == dp.radioButton3.Text) { dp.radioButton3.Checked = true; };
            //    foreach (Control ctr in dp.panel2.Controls)
            //    {
            //        //判断该控件是不是CheckBox
            //        if (ctr is CheckBox)
            //        {
            //            //将ctr转换成CheckBox并赋值给ck
            //            CheckBox ck = ctr as CheckBox;
            //            if (dt.Rows[0]["symptom"].ToString().IndexOf(ck.Text) > -1)
            //            {
            //                ck.Checked = true;
            //            }
            //        }
            //    }
            //    dp.richTextBox1.Text = dt.Rows[0]["symptom_other"].ToString();

            //    dp.numericUpDown9.Value = Decimal.Parse(dt.Rows[0]["blood_pressure_high"].ToString());
            //    dp.numericUpDown10.Value = Decimal.Parse(dt.Rows[0]["blood_pressure_low"].ToString());
            //    dp.numericUpDown11.Value = Decimal.Parse(dt.Rows[0]["weight_now"].ToString());
            //    dp.numericUpDown12.Value = Decimal.Parse(dt.Rows[0]["weight_next"].ToString());
            //    dp.numericUpDown14.Value = Decimal.Parse(dt.Rows[0]["bmi_now"].ToString());
            //    dp.numericUpDown15.Value = Decimal.Parse(dt.Rows[0]["bmi_next"].ToString());
            //    foreach (Control ctr in dp.panel11.Controls)
            //    {
            //        //判断该控件是不是CheckBox
            //        if (ctr is CheckBox)
            //        {
            //            //将ctr转换成CheckBox并赋值给ck
            //            CheckBox ck = ctr as CheckBox;
            //            if (dt.Rows[0]["dorsal_artery"].ToString().IndexOf(ck.Text) > -1)
            //            {
            //                ck.Checked = true;
            //            }
            //        }
            //    }
            //    dp.richTextBox3.Text = dt.Rows[0]["other"].ToString();

            //    dp.numericUpDown1.Value = Decimal.Parse(dt.Rows[0]["smoke_now"].ToString());
            //    dp.numericUpDown2.Value = Decimal.Parse(dt.Rows[0]["smoke_next"].ToString());
            //    dp.numericUpDown3.Value = Decimal.Parse(dt.Rows[0]["drink_now"].ToString());
            //    dp.numericUpDown4.Value = Decimal.Parse(dt.Rows[0]["drink_next"].ToString());
            //    dp.numericUpDown5.Value = Decimal.Parse(dt.Rows[0]["sports_num_now"].ToString());
            //    dp.numericUpDown6.Value = Decimal.Parse(dt.Rows[0]["sports_time_now"].ToString());
            //    dp.numericUpDown7.Value = Decimal.Parse(dt.Rows[0]["sports_num_next"].ToString());
            //    dp.numericUpDown8.Value = Decimal.Parse(dt.Rows[0]["sports_time_next"].ToString());
            //    dp.numericUpDown13.Value = Decimal.Parse(dt.Rows[0]["staple_food_now"].ToString());
            //    dp.numericUpDown16.Value = Decimal.Parse(dt.Rows[0]["staple_food_next"].ToString());
            //    if (dt.Rows[0]["psychological_recovery"].ToString() == dp.radioButton10.Text) { dp.radioButton10.Checked = true; };
            //    if (dt.Rows[0]["psychological_recovery"].ToString() == dp.radioButton11.Text) { dp.radioButton11.Checked = true; };
            //    if (dt.Rows[0]["psychological_recovery"].ToString() == dp.radioButton12.Text) { dp.radioButton12.Checked = true; };
            //    if (dt.Rows[0]["medical_compliance"].ToString() == dp.radioButton13.Text) { dp.radioButton13.Checked = true; };
            //    if (dt.Rows[0]["medical_compliance"].ToString() == dp.radioButton14.Text) { dp.radioButton14.Checked = true; };
            //    if (dt.Rows[0]["medical_compliance"].ToString() == dp.radioButton15.Text) { dp.radioButton15.Checked = true; };

            //    dp.numericUpDown17.Value = Decimal.Parse(dt.Rows[0]["blood_glucose"].ToString());
            //    dp.numericUpDown18.Value = Decimal.Parse(dt.Rows[0]["glycosylated_hemoglobin"].ToString());
            //    if (dt.Rows[0]["compliance"].ToString() == dp.radioButton22.Text) { dp.radioButton22.Checked = true; };
            //    if (dt.Rows[0]["compliance"].ToString() == dp.radioButton23.Text) { dp.radioButton23.Checked = true; };
            //    if (dt.Rows[0]["compliance"].ToString() == dp.radioButton24.Text) { dp.radioButton24.Checked = true; };
            //    if (dt.Rows[0]["untoward_effect"].ToString() == dp.radioButton16.Text) { dp.radioButton16.Checked = true; };
            //    if (dt.Rows[0]["untoward_effect"].ToString() == dp.radioButton17.Text) { dp.radioButton17.Checked = true; };
            //    if (dt.Rows[0]["reactive_hypoglycemia"].ToString() == dp.radioButton4.Text) { dp.radioButton4.Checked = true; };
            //    if (dt.Rows[0]["reactive_hypoglycemia"].ToString() == dp.radioButton5.Text) { dp.radioButton5.Checked = true; };
            //    if (dt.Rows[0]["reactive_hypoglycemia"].ToString() == dp.radioButton6.Text) { dp.radioButton6.Checked = true; };

            //    if (dt.Rows[0]["follow_type"].ToString() == dp.radioButton18.Text) { dp.radioButton18.Checked = true; };
            //    if (dt.Rows[0]["follow_type"].ToString() == dp.radioButton19.Text) { dp.radioButton19.Checked = true; };
            //    if (dt.Rows[0]["follow_type"].ToString() == dp.radioButton20.Text) { dp.radioButton20.Checked = true; };
            //    if (dt.Rows[0]["follow_type"].ToString() == dp.radioButton21.Text) { dp.radioButton21.Checked = true; };
            //    dp.richTextBox2.Text = dt.Rows[0]["advice"].ToString();

            //    dp.textBox5.Text = dt.Rows[0]["transfer_treatment_reason"].ToString();
            //    dp.textBox6.Text = dt.Rows[0]["transfer_treatment_department"].ToString();
            //    dp.dateTimePicker2.Value = DateTime.Parse(dt.Rows[0]["next_visit_date"].ToString());
            //    dp.textBox7.Text = dt.Rows[0]["visit_doctor"].ToString();

            //}
            //else { }



            //if (dp.ShowDialog() == DialogResult.OK)
            //{
            //    //刷新页面
            //    querytcmHealthServices();
            //    MessageBox.Show("修改成功！");

            //}
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
             // bool istrue = tcmHealthService.deletetcmHealthServices(id);
                int row = dataGridView1.CurrentRow.Index;
                string Name = dataGridView1["姓名", row].Value.ToString().Trim();
                string aichive_no = dataGridView1["编码", row].Value.ToString().Trim();
                string id_number = dataGridView1["身份证号", row].Value.ToString().Trim();
                bool istrue = deletetcmHealthServices(Name, aichive_no, id_number);
                if (istrue)
                {
                    //刷新页面
                    querytcmHealthServices();
                    MessageBox.Show("删除成功！");
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label2.Visible = this.textBox1.Text.Length < 1;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            string timesta = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string timeend = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string sheng = comboBox1.SelectedValue?.ToString();
            string shi = comboBox2.SelectedValue?.ToString();
            string xian = comboBox3.SelectedValue?.ToString();
            string cun = comboBox4.SelectedValue?.ToString();
            string zu = comboBox5.SelectedValue?.ToString();
            string juming = textBox1.Text;
            var pairs = new Dictionary<string, string>();
            pairs.Add("timesta", timesta);
            pairs.Add("timeend", timeend);
            pairs.Add("juming", juming);
            pairs.Add("sheng", sheng);
            pairs.Add("xian", xian);
            pairs.Add("cun", cun);
            pairs.Add("zu", zu);
            pairs.Add("shi", shi);
            string sql = $@"select 
DATE_FORMAT(base.create_time,'%Y%m%d') 登记时间,
concat(base.province_name,base.city_name,base.county_name,base.towns_name,base.village_name) 区域,
base.archive_no 编码,
base.name 姓名,
(case base.sex when '1'then '男' when '2' then '女' when '9' then '未说明的性别' when '0' then '未知的性别' ELSE ''
END)性别,
base.id_number 身份证号,
base.upload_status 是否同步
from resident_base_info base
where base.village_code='{basicInfoSettings.xcuncode}' and base.create_time>='{basicInfoSettings.createtime}'";//base.village_code='{basicInfoSettings.xcuncode}' and base.create_time>='{basicInfoSettings.createtime}'
            if (pairs != null && pairs.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(pairs["timesta"]) && !string.IsNullOrWhiteSpace(pairs["timeend"]))
                {
                    sql += $" and date_format(base.create_time,'%Y-%m-%d') between '{pairs["timesta"]}' and '{pairs["timeend"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["juming"]))
                {
                    sql += $" or base.name like '%{pairs["juming"]}%' or base.bar_code like '%{pairs["juming"]}%' or base.id_number like '%{pairs["juming"]}%'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["sheng"]))
                {
                    sql += $" and base.province_code='{pairs["sheng"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["shi"]))
                {
                    sql += $" and base.city_code='{pairs["shi"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["xian"]))
                {
                    sql += $" and base.county_code='{pairs["xian"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["cun"]))
                {
                    sql += $" and base.towns_code='{pairs["cun"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["zu"]))
                {
                    sql += $" and base.village_code='{pairs["zu"]}'";
                }
            }
            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable dt = dataSet.Tables[0];
            return dt;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="aichive_no">编号</param>
        /// <param name="id_number">身份证id</param>
        /// <returns></returns>
        public bool deletetcmHealthServices(string name, string aichive_no, string id_number)
        {
            int rt = 0;
            string sql = $"delete from elderly_tcm_record where name='{name}' and aichive_no='{aichive_no}' and id_number='{id_number}'";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }

        #region 下拉框绑定
        /// <summary>
        /// 绑定下拉选项
        /// </summary>
        /// <param name="combo">获取值</param>
        /// <param name="box">绑定值</param>
        private void comboBoxBin(ComboBox combo, ComboBox box)
        {
            string id = combo.SelectedValue?.ToString();
            if (!string.IsNullOrWhiteSpace(id))
            {
                string sql1 = $"select code as ID,name as Name from code_area_config where parent_code='{id}'";
                DataSet datas = DbHelperMySQL.Query(sql1);
                if (datas != null && datas.Tables.Count > 0)
                {
                    List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(datas.Tables[0]);
                    Result.Bind(box, ts, "Name", "ID", "--请选择--");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBin(comboBox1, comboBox2);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBin(comboBox2, comboBox3);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBin(comboBox3, comboBox4);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBin(comboBox4, comboBox5);
        }
        #endregion
    }
}
