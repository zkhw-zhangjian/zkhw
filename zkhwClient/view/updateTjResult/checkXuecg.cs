﻿using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.updateTjResult
{
    public partial class checkXuecg : Form
    {
        tjcheckDao tjdao = new tjcheckDao();
        string str = Application.StartupPath;//项目路径
        public checkXuecg()
        {
            InitializeComponent();
        }
        
        private void checkXuecg_Load(object sender, EventArgs e)
        { 
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular);
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;


            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1); 

            DataTable dtbichao = tjdao.checkXcgInfo(dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss"), null, basicInfoSettings.xcuncode);
            //DataTable dtbichao = tjdao.checkXcgInfo(basicInfoSettings.createtime, null, basicInfoSettings.xcuncode);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                this.dataGridView1.DataSource = dtbichao;
                this.dataGridView1.Columns[0].HeaderCell.Value = "体检时间";
                this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
                this.dataGridView1.Columns[2].HeaderCell.Value = "档案编号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "身份证号";
                this.dataGridView1.Columns[4].HeaderCell.Value = "条码号";
                this.dataGridView1.Columns[5].HeaderCell.Value = "是否检验";
                this.dataGridView1.Columns[0].Width = 120;
                this.dataGridView1.Columns[1].Width = 120;
                this.dataGridView1.Columns[2].Width = 160;
                this.dataGridView1.Columns[3].Width = 160;
                this.dataGridView1.Columns[4].Width = 110;
                this.dataGridView1.Columns[5].Width = 100;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                int rows = this.dataGridView1.Rows.Count - 1 <= 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                for (int x = 0; x <= rows; x++)
                {
                    this.dataGridView1.Rows[x].HeaderCell.Value = String.Format("{0}", x + 1);
                }
            }
            else {
                this.dataGridView1.DataSource = null;
                //MessageBox.Show("未查询到数据!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string time1 = this.dateTimePicker1.Text.ToString();
            string time2 = this.dateTimePicker2.Text.ToString();
            DataTable dtbichao = tjdao.checkXcgInfo(time1, time2, null);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                if (this.radioButton3.Checked)
                {
                    DataRow[] dr = dtbichao.Select("type='完成'");
                    if (dr != null && dr.Length > 0)
                    {
                        for (int i = dr.Length - 1; i >= 0; i--)
                        {
                            dtbichao.Rows.Remove(dr[i]);
                        }
                    }
                }
                else if (this.radioButton1.Checked)
                {
                    DataRow[] dr = dtbichao.Select("type='未完成'");
                    if (dr != null && dr.Length > 0)
                    {
                        for (int i = dr.Length - 1; i >= 0; i--)
                        {
                            dtbichao.Rows.Remove(dr[i]);
                        }
                    }
                }
                if (dtbichao.Rows.Count < 1)
                {
                    this.dataGridView1.DataSource = null; MessageBox.Show("未查询出数据!"); return;
                }
                this.dataGridView1.DataSource = dtbichao;
                this.dataGridView1.Columns[0].HeaderCell.Value = "体检时间";
                this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
                this.dataGridView1.Columns[2].HeaderCell.Value = "档案编号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "身份证号";
                this.dataGridView1.Columns[4].HeaderCell.Value = "条码号";
                this.dataGridView1.Columns[5].HeaderCell.Value = "是否检验";
                this.dataGridView1.Columns[0].Width = 120;
                this.dataGridView1.Columns[1].Width = 120;
                this.dataGridView1.Columns[2].Width = 160;
                this.dataGridView1.Columns[3].Width = 160;
                this.dataGridView1.Columns[4].Width = 110;
                this.dataGridView1.Columns[5].Width = 100;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                int rows = this.dataGridView1.Rows.Count - 1 <= 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                for (int x = 0; x <= rows; x++)
                {
                    this.dataGridView1.Rows[x].HeaderCell.Value = String.Format("{0}", x + 1);
                }
            }
            else{
                this.dataGridView1.DataSource = null;
                MessageBox.Show("未查询到数据!");
            }
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            Color color = Color.FromArgb(77, 177, 81);
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, color, color);
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("查询", new Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 5));

        }
    }
}
