using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.PublicHealthView;
using zkhwClient.view.setting;

namespace zkhwClient.PublicHealth
{
    public partial class olderHelthService : Form
    {
        service.olderHelthServices olderHelthS = new service.olderHelthServices();
        areaConfigDao areadao = new areaConfigDao();
        public string pCa = "";
        public string time1 = null;
        public string time2 = null;
        string xcuncode = "";
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        public olderHelthService()
        {
            InitializeComponent();
        }

        private void examinatProgress_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            string str = Application.StartupPath;//项目路径
            //区域
            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 
            //统计
            DataTable dt0 = olderHelthS.queryOlderHelthService0();
            if (dt0 != null && dt0.Rows.Count > 0)
            {
                this.label10.Text = dt0.Rows[0]["label10"].ToString();
                this.label11.Text = dt0.Rows[0]["label11"].ToString();
                this.label13.Text = dt0.Rows[0]["label13"].ToString();
                this.label15.Text = dt0.Rows[0]["label15"].ToString();
                this.label17.Text = dt0.Rows[0]["label17"].ToString();
                this.label18.Text = dt0.Rows[0]["label18"].ToString();
                this.label21.Text = dt0.Rows[0]["label21"].ToString();
            }
            queryOlderHelthService();
        }
        private void queryOlderHelthService()
        {
            this.dataGridView1.DataSource = null;
            //展示
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间

            DataTable dt = olderHelthS.queryOlderHelthService(pCa, time1, time2, xcuncode);
            if (dt.Rows.Count<1) { MessageBox.Show("未查询出数据!");return; }
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[1].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "总分";
            this.dataGridView1.Columns[4].HeaderCell.Value = "评判结果";
            this.dataGridView1.Columns[5].HeaderCell.Value = "测试日期";
            this.dataGridView1.Columns[6].HeaderCell.Value = "测试医生";

            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;
            if (pCa != "")
            {
                this.label5.Text = "";
            }
            else { this.label5.Text = "---姓名/身份证号/档案号---"; }
            queryOlderHelthService();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label5.Visible = this.textBox1.Text.Length < 1;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
                bool istrue = olderHelthS.deleteOlderHelthService(id);
                if (istrue)
                {
                    //刷新页面
                    queryOlderHelthService();
                    MessageBox.Show("删除成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1) { return; }
            string name=dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string archiveno = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string idnumber = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            aUolderHelthService hm = new aUolderHelthService();
            hm.label47.Text = "添加老年人生活自理能力评估表";
            hm.Text = "添加老年人生活自理能力评估表";
            hm.textBox1.Text = name;
            hm.textBox2.Text = archiveno;
            hm.textBox12.Text = idnumber;
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                queryOlderHelthService();
                MessageBox.Show("添加成功！");

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            aUolderHelthService hm = new aUolderHelthService();
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            hm.id = id;
            hm.label47.Text = "修改老年人生活自理能力评估表";
            hm.Text = "修改老年人生活自理能力评估表";
            DataTable dt = olderHelthS.queryOlderHelthService(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                hm.textBox1.Text = dt.Rows[0]["name"].ToString();
                hm.textBox2.Text = dt.Rows[0]["aichive_no"].ToString();
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton1.Text) { hm.radioButton1.Checked = true; };
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton2.Text) { hm.radioButton2.Checked = true; };
                hm.textBox12.Text = dt.Rows[0]["id_number"].ToString();
                string[] ck2 = dt.Rows[0]["answer_result"].ToString().Split(',');
                hm.numericUpDown1.Value = Decimal.Parse(ck2[0]);
                hm.numericUpDown2.Value = Decimal.Parse(ck2[1]);
                hm.numericUpDown3.Value = Decimal.Parse(ck2[2]);
                hm.numericUpDown4.Value = Decimal.Parse(ck2[3]);
                hm.numericUpDown5.Value = Decimal.Parse(ck2[4]);
                hm.numericUpDown6.Value = Decimal.Parse(dt.Rows[0]["total_score"].ToString());
            }


            if (hm.ShowDialog() == DialogResult.OK)
            {
                    //刷新页面
                 queryOlderHelthService();
                MessageBox.Show("修改成功！");

            }
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
