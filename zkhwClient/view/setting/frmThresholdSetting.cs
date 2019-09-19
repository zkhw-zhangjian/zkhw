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
            dtSh = grjddao.checkThresholdValues1(Common._deviceModel, "生化");
            dataGridView1.DataSource = dtSh;
           
            dataGridView1.Columns[0].DefaultCellStyle.SelectionBackColor = Control.DefaultBackColor;
            dataGridView1.Columns[0].DefaultCellStyle.SelectionForeColor = Color.Black;
        }
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridViewRowsAdded(dataGridView1, e); 
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
       

         

        private void GetThreshValuesForXeChangGui()
        { 
            dtXCG = grjddao.checkThresholdValues1(Common._deviceModel, "血常规");
            dataGridView1.DataSource = dtXCG;
            dataGridView1.Columns[0].DefaultCellStyle.SelectionBackColor = Control.DefaultBackColor;
            dataGridView1.Columns[0].DefaultCellStyle.SelectionForeColor = Color.Black;
        }
         

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if(e.ColumnIndex==0)
            {
                //e.CellStyle.BackColor= Control.DefaultBackColor;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                GetThreshValuesForSheHua();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                GetThreshValuesForXeChangGui();
            }
        }
    }
}
