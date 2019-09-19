using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.service;

namespace zkhwClient.view.setting
{
    public partial class systemlog : Form
    {
        DataTable dt = null;
        int flag = 1;
        loginLogService logservice = new loginLogService();
        public systemlog()
        {
            InitializeComponent();
        }

        private void systemlog_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string time1 = this.dateTimePicker1.Text.ToString();
            string time2 = this.dateTimePicker2.Text.ToString();
            if (this.radioButton1.Checked) {
                flag = 1;
            } else if (this.radioButton2.Checked) {
                flag = 2;
            }
            else if (this.radioButton3.Checked)
            {
                flag = 3;
            }
            DateTime t1 = Convert.ToDateTime(time1);
            DateTime t2 = Convert.ToDateTime(time2);
            if (DateTime.Compare(t1, t2) < 0)
            {
                dt = logservice.checkLog(time1, time2,flag.ToString());
            }
           
            if (dt != null && dt.Rows.Count > 0)
            {
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].HeaderCell.Value = "时间";
                this.dataGridView1.Columns[1].HeaderCell.Value = "类型";
                this.dataGridView1.Columns[2].HeaderCell.Value = "用户";
                this.dataGridView1.Columns[3].HeaderCell.Value = "描述";
                this.dataGridView1.Columns[0].Width = 200;
                this.dataGridView1.Columns[1].Width = 200;
                this.dataGridView1.Columns[2].Width = 200;
                this.dataGridView1.Columns[3].Width = 300;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                int rows = this.dataGridView1.Rows.Count - 1 <= 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                for (int x = 0; x <= rows; x++)
                {
                    this.dataGridView1.Rows[x].HeaderCell.Value = String.Format("{0}", x + 1);

                    if (this.dataGridView1.Rows[x].Cells[1].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[1].Value = "系统日志";
                    }
                    else if (this.dataGridView1.Rows[x].Cells[1].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[1].Value = "操作日志";
                    }
                    else if (this.dataGridView1.Rows[x].Cells[1].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[1].Value = "错误日志";
                    }
                }
                }
            else
            {
                this.dataGridView1.DataSource = null;
                   MessageBox.Show("未查询出数据!");
            }
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("查询", new System.Drawing.Font("微软雅黑", 9, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 7));

        }
    }
}
