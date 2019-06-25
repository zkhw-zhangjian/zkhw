using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.PublicHealthView
{
    public partial class fileManagement : Form
    {
        public fileManagement()
        {
            InitializeComponent();
        }

        private void fileManagement_Load(object sender, EventArgs e)
        {
            this.label4.Text = "个人档案管理";
            this.label4.ForeColor = Color.SkyBlue;
            label4.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label4.Left = (this.panel1.Width - this.label4.Width) / 2;
            label4.BringToFront();

            #region 报告查询 区域数据绑定
            string sql1 = "select code as ID,name as Name from code_area_config where parent_code='-1';";
            DataSet datas = DbHelperMySQL.Query(sql1);
            if (datas != null && datas.Tables.Count > 0)
            {
                List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(datas.Tables[0]);
                Result.Bind(comboBox1, ts, "Name", "ID", "--请选择--");
            }
            #endregion

            SetPageControl();
        }

        private void SetPageControl()
        {
            pagerControl1.PageIndex = 1;
            pagerControl1.PageSize = 20; 
            pagerControl1.DrawControl(100);
        }
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

        private void queryData()
        {
            string timesta = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string timeend = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string sheng = comboBox1.SelectedValue?.ToString();
            string shi = comboBox2.SelectedValue?.ToString();
            string xian = comboBox3.SelectedValue?.ToString();
            string cun = comboBox4.SelectedValue?.ToString();
            string zu = comboBox5.SelectedValue?.ToString();
            string juming = textBox1.Text;
            string sWhere = "";
            #region 条件整理
            if(timesta !="")
            {
                string tmp =string.Format(" date_format(r.create_time, '%Y-%m-%d')>='{0}'", timesta);
                sWhere = tmp;
            }
            if (timeend != "")
            {
                string tmp = string.Format(" date_format(r.create_time, '%Y-%m-%d')<='{0}'", timeend);
                if(sWhere =="")
                {
                    sWhere = tmp;
                }
                else
                {
                    sWhere = sWhere+" And "+ tmp;
                }
            }
            if (!string.IsNullOrWhiteSpace(sheng))
            {
                string tmp = string.Format(" r.province_code='{0}'", sheng);
                if (sWhere == "")
                {
                    sWhere = tmp;
                }
                else
                {
                    sWhere = sWhere + " And " + tmp;
                }
            }
            if (!string.IsNullOrWhiteSpace(shi))
            {
                string tmp = string.Format(" r.city_code='{0}'", shi);
                if (sWhere == "")
                {
                    sWhere = tmp;
                }
                else
                {
                    sWhere = sWhere + " And " + tmp;
                }
            }
            if (!string.IsNullOrWhiteSpace(xian))
            {
                string tmp = string.Format(" r.county_code='{0}'", xian);
                if (sWhere == "")
                {
                    sWhere = tmp;
                }
                else
                {
                    sWhere = sWhere + " And " + tmp;
                }
            }
            if (!string.IsNullOrWhiteSpace(cun))
            {
                string tmp = string.Format(" r.towns_code='{0}'", cun);
                if (sWhere == "")
                {
                    sWhere = tmp;
                }
                else
                {
                    sWhere = sWhere + " And " + tmp;
                }
            }
            if (!string.IsNullOrWhiteSpace(zu))
            {
                string tmp = string.Format(" r.village_code='{0}'", zu);
                if (sWhere == "")
                {
                    sWhere = tmp;
                }
                else
                {
                    sWhere = sWhere + " And " + tmp;
                }
            }
            if (!string.IsNullOrWhiteSpace(juming))
            {
                string tmp = string.Format(" (r.name like '%{0}%' or r.id_number like '%{0}%' or r.archive_no like '%{0}%')", juming);
                if (sWhere == "")
                {
                    sWhere = tmp;
                }
                else
                {
                    sWhere = sWhere + " And " + tmp;
                }
            }
            #endregion
            if(sWhere !="")
            {
                sWhere = " Where "+sWhere;
            }
            jkInfoDao jk = new jkInfoDao();
            DataTable dt = jk.queryGeRenArchivesInfo(sWhere);
            dataGridView1.DataSource = dt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            queryData();
        }
    }
}
