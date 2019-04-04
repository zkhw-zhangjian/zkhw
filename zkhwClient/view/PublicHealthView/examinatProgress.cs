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
    public partial class examinatProgress : Form
    {
        public string time1 = null;
        public string time2 = null;
        DataTable dt;

        public examinatProgress()
        {
            InitializeComponent();
        }

        private void examinatProgress_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/check.png");
            queryExaminatProgress();
        }
        private void queryExaminatProgress()
        {
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间

            dt = null;//traderecord.queryTradeRecord(time1, time2, userinfoName, tradeRecordCode, tradeMeans);
            if (dt != null)
            {
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].HeaderCell.Value = "操作员";
                this.dataGridView1.Columns[2].HeaderCell.Value = "流水号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "类别";
                this.dataGridView1.Columns[4].HeaderCell.Value = "名称";
                this.dataGridView1.Columns[5].HeaderCell.Value = "条码";
                this.dataGridView1.Columns[6].HeaderCell.Value = "进价";
                this.dataGridView1.Columns[7].HeaderCell.Value = "售价";
                this.dataGridView1.Columns[8].HeaderCell.Value = "数量";
                this.dataGridView1.Columns[9].HeaderCell.Value = "计量单位";
                this.dataGridView1.Columns[10].HeaderCell.Value = "交易金额";
                this.dataGridView1.Columns[11].HeaderCell.Value = "利润";
                this.dataGridView1.Columns[12].HeaderCell.Value = "交易时间";
                this.dataGridView1.Columns[13].HeaderCell.Value = "交易方式";

                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                this.dataGridView1.Columns[0].Visible = false;

                this.dataGridView1.ReadOnly = true;
                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            queryExaminatProgress();
        }
    }
}
