using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.view.PublicHealthView;

namespace zkhwClient.PublicHealth
{
    public partial class olderHelthService : Form
    {
        service.olderHelthServices olderHelthS = new service.olderHelthServices();

        public string pCa = "";
        public string time1 = null;
        public string time2 = null;

        public olderHelthService()
        {
            InitializeComponent();
        }

        private void examinatProgress_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            string str = Application.StartupPath;//项目路径
            queryOlderHelthService();
        }
        private void queryOlderHelthService()
        {
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

            //展示
            this.dataGridView1.DataSource = null;
            DataTable dt = olderHelthS.queryOlderHelthService(pCa, time1, time2);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
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
                this.label2.Text = "";
            }
            else { this.label2.Text = "---姓名/身份证号/档案号---"; }
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
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

            aUolderHelthService hm = new aUolderHelthService();
            hm.label47.Text = "添加老年人生活自理能力评估表";
            hm.Text = "添加老年人生活自理能力评估表";
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                queryOlderHelthService();
                MessageBox.Show("添加成功！");

            }
        }
    }
}
