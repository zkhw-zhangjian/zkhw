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
    public partial class maternalHealthServices : Form
    {
        public maternalHealthServices()
        {
            InitializeComponent();
            Bin();
        }

        private void 查询_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.textBox1.Text))
            {
                this.label2.Text = "";
            }
            else { this.label2.Text = "---姓名/身份证号/档案号---"; }
            querytcmHealthServices();
        }

        private void 第1次产前检查添加_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            addmaternalHealthServices addtcm = new addmaternalHealthServices(1, dataGridView1["姓名", row].Value.ToString().Trim(), dataGridView1["编码", row].Value.ToString().Trim(), dataGridView1["身份证号", row].Value.ToString().Trim());
            if (addtcm.show)
            {
                addtcm.StartPosition = FormStartPosition.CenterScreen;
                addtcm.ShowDialog();
            }
            else
            {
                MessageBox.Show(addtcm.mag);
            }
        }

        private void 第1次产前检查修改_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            addmaternalHealthServices addtcm = new addmaternalHealthServices(0, dataGridView1["姓名", row].Value.ToString().Trim(), dataGridView1["编码", row].Value.ToString().Trim(), dataGridView1["身份证号", row].Value.ToString().Trim());
            if (addtcm.show)
            {
                addtcm.StartPosition = FormStartPosition.CenterScreen;
                addtcm.ShowDialog();
            }
            else
            {
                MessageBox.Show(addtcm.mag);
            }
        }

        private void 第1次产前检查删除_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
                int row = dataGridView1.CurrentRow.Index;
                string Name = dataGridView1["姓名", row].Value.ToString().Trim();
                string aichive_no = dataGridView1["编码", row].Value.ToString().Trim();
                string id_number = dataGridView1["身份证号", row].Value.ToString().Trim();
                bool istrue = deletematernalHealthServices(Name, aichive_no, id_number);
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

        private void 第25次产前随访添加_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            addStartMaternalHealthServices addtcm = new addStartMaternalHealthServices(1, dataGridView1["姓名", row].Value.ToString().Trim(), dataGridView1["编码", row].Value.ToString().Trim(), dataGridView1["身份证号", row].Value.ToString().Trim());
            if (addtcm.show)
            {
                addtcm.StartPosition = FormStartPosition.CenterScreen;
                addtcm.ShowDialog();
            }
            else
            {
                MessageBox.Show(addtcm.mag);
            }
        }

        private void 第25次产前随访修改_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            addStartMaternalHealthServices addtcm = new addStartMaternalHealthServices(0, dataGridView1["姓名", row].Value.ToString().Trim(), dataGridView1["编码", row].Value.ToString().Trim(), dataGridView1["身份证号", row].Value.ToString().Trim());
            if (addtcm.show)
            {
                addtcm.StartPosition = FormStartPosition.CenterScreen;
                addtcm.ShowDialog();
            }
            else
            {
                MessageBox.Show(addtcm.mag);
            }
        }

        private void 第25次产前随访删除_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            deleteStartMaternalHealthServices addtcm = new deleteStartMaternalHealthServices(dataGridView1["姓名", row].Value.ToString().Trim(), dataGridView1["编码", row].Value.ToString().Trim(), dataGridView1["身份证号", row].Value.ToString().Trim());
            addtcm.StartPosition = FormStartPosition.CenterScreen;
            addtcm.ShowDialog();
        }

        private void 产后访视添加_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            addEndMaternalHealthServices addtcm = new addEndMaternalHealthServices(1, dataGridView1["姓名", row].Value.ToString().Trim(), dataGridView1["编码", row].Value.ToString().Trim(), dataGridView1["身份证号", row].Value.ToString().Trim());
            if (addtcm.show)
            {
                addtcm.StartPosition = FormStartPosition.CenterScreen;
                addtcm.ShowDialog();
            }
            else
            {
                MessageBox.Show(addtcm.mag);
            }
        }

        private void 产后访视修改_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            addEndMaternalHealthServices addtcm = new addEndMaternalHealthServices(0, dataGridView1["姓名", row].Value.ToString().Trim(), dataGridView1["编码", row].Value.ToString().Trim(), dataGridView1["身份证号", row].Value.ToString().Trim());
            if (addtcm.show)
            {
                addtcm.StartPosition = FormStartPosition.CenterScreen;
                addtcm.ShowDialog();
            }
            else
            {
                MessageBox.Show(addtcm.mag);
            }
        }

        private void 产后访视删除_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            deleteEndMaternalHealthServices addtcm = new deleteEndMaternalHealthServices(dataGridView1["姓名", row].Value.ToString().Trim(), dataGridView1["编码", row].Value.ToString().Trim(), dataGridView1["身份证号", row].Value.ToString().Trim());
            addtcm.StartPosition = FormStartPosition.CenterScreen;
            addtcm.ShowDialog();
        }

        private void 关闭_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 列表数据绑定
        /// </summary>
        private void querytcmHealthServices()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = GetData();
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
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
where base.village_code='{basicInfoSettings.xcuncode}' and base.create_time>='{Convert.ToDateTime(basicInfoSettings.createtime).ToString("yyyy-MM-dd")}'";//base.village_code='{basicInfoSettings.xcuncode}' and base.create_time>='{basicInfoSettings.createtime}'
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
        /// 初始数据
        /// </summary>
        private void Bin()
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
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
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="aichive_no">编号</param>
        /// <param name="id_number">身份证id</param>
        /// <returns></returns>
        public bool deletematernalHealthServices(string name, string aichive_no, string id_number)
        {
            int rt = 0;
            string sql = $"delete from gravida_info where name='{name}' and aichive_no='{aichive_no}' and id_number='{id_number}'";
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
