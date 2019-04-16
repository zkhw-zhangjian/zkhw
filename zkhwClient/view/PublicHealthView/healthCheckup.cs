using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.PublicHealthView
{
    public partial class healthCheckup : Form
    {
        healthCheckupDao hcd = new healthCheckupDao();
        string str = Application.StartupPath;//项目路径
        public string time1 = null;
        public string time2 = null;
        public string pCa = "";
        public healthCheckup()
        {
            InitializeComponent();
        }

        private void healthCheckup_Load(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            queryOlderHelthService(0);
        }

        private void queryOlderHelthService(int flag)
        {
            //展示
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            if (flag!=0) {
                pCa = this.textBox1.Text;
            }
            this.dataGridView1.DataSource = null;
            DataTable dt = hcd.queryhealthCheckup(pCa, time1, time2);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[1].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[2].HeaderCell.Value = "条码号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[4].HeaderCell.Value = "检查日期";
            this.dataGridView1.Columns[5].HeaderCell.Value = "责任医生";

            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            queryOlderHelthService(1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label5.Visible = this.textBox1.Text.Length < 1;
        }
    }
}
