using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.bean;
using zkhwClient.dao;

namespace zkhwClient.view.setting
{
    public partial class frmThresholdSetting : Form
    {
        grjdDao grjddao = new grjdDao();
        DataTable dtSh = null;
        DataTable dtXCG = null;
        public frmThresholdSetting()
        {
            InitializeComponent();
        }

        private void frmThresholdSetting_Load(object sender, EventArgs e)
        {
            GetThreshValuesForSheHua();
        }
        private void GetThreshValuesForSheHua()
        {
            dtSh = grjddao.checkThresholdValues(Common._deviceModel, "生化");
            dataGridView1.DataSource = dtSh;

            dataGridView1.Columns[0].DefaultCellStyle.SelectionBackColor = Control.DefaultBackColor;
            dataGridView1.Columns[0].DefaultCellStyle.SelectionForeColor = Color.Black;
        }
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridViewRowsAdded(dataGridView1, e); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveItem(dataGridView1); 
        }
        private void dataGridViewRowsAdded(DataGridView dg,DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < e.RowCount; i++)
            {
                dg.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }

            for (int i = e.RowIndex + e.RowCount; i < dg.Rows.Count; i++)
            {
                dg.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }
        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridViewRowsAdded(dataGridView2, e); 
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex==1)
            {
                GetThreshValuesForXeChangGui();
            }
        }

        private void GetThreshValuesForXeChangGui()
        { 
            dtXCG = grjddao.checkThresholdValues(Common._deviceModel, "血常规");
            dataGridView2.DataSource = dtXCG;
            dataGridView2.Columns[0].DefaultCellStyle.SelectionBackColor = Control.DefaultBackColor;
            dataGridView2.Columns[0].DefaultCellStyle.SelectionForeColor = Color.Black;
        }
        private void SaveItem(DataGridView dg)
        {
            List<string> _sqllst = new List<string>();
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                #region 整理
                thresholdValueBean obj = new thresholdValueBean();
                obj.id = dg.Rows[i].Cells[8].Value.ToString();
                if (dg.Rows[i].Cells[1] == null)
                {
                    obj.chinaName = "";
                }
                else
                {
                    obj.chinaName = dg.Rows[i].Cells[1].Value.ToString();
                }
                if (dg.Rows[i].Cells[2] == null)
                {
                    obj.CheckMethod = "";
                }
                else
                {
                    obj.CheckMethod = dg.Rows[i].Cells[2].Value.ToString();
                }

                if (dg.Rows[i].Cells[3] == null)
                {
                    obj.warning_min = "0";
                }
                else
                {
                    obj.warning_min = dg.Rows[i].Cells[3].Value.ToString();
                }

                if (dg.Rows[i].Cells[4] == null)
                {
                    obj.warning_max = "0";
                }
                else
                {
                    obj.warning_max = dg.Rows[i].Cells[4].Value.ToString();
                }

                if (dg.Rows[i].Cells[5] == null)
                {
                    obj.threshold_min = "0";
                }
                else
                {
                    obj.threshold_min = dg.Rows[i].Cells[5].Value.ToString();
                }

                if (dg.Rows[i].Cells[6] == null)
                {
                    obj.threshold_max = "0";
                }
                else
                {
                    obj.threshold_max = dg.Rows[i].Cells[6].Value.ToString();
                }

                if (dg.Rows[i].Cells[7] == null)
                {
                    obj.unit = "";
                }
                else
                {
                    obj.unit = dg.Rows[i].Cells[7].Value.ToString();
                }
                #endregion
                string sql = string.Format(@"update threshold_value set chinaName='{0}',CheckMethod='{1}',
                warning_min={2},warning_max={3},threshold_min={4},threshold_max={5},unit='{6}' where id='{7}' ", obj.chinaName,
                obj.CheckMethod, obj.warning_min, obj.warning_max, obj.threshold_min, obj.threshold_max, obj.unit, obj.id);
                _sqllst.Add(sql);
            }
            int ret = DbHelperMySQL.ExecuteSqlTran(_sqllst);
            if (ret > 0)
            {
                MessageBox.Show("成功！");
            }
            else
            {
                MessageBox.Show("失败！");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveItem(dataGridView2);
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if(e.ColumnIndex==0)
            {
                e.CellStyle.BackColor= Control.DefaultBackColor;
            }
        } 
    }
}
