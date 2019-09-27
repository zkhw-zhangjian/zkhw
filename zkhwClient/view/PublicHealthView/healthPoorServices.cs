using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using zkhwClient.bean;
using zkhwClient.dao;
using zkhwClient.service;
using zkhwClient.utils;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class healthPoorServices : Form
    {
        static JavaScriptSerializer serializer = new JavaScriptSerializer();
        public string pCa = "";
        public string time1 = null;
        public string time2 = null;
        public string cun = "";
        bool isfirst = true;
        healthPoorServicesDao hpd = new healthPoorServicesDao();
        public healthPoorServices()
        {
            InitializeComponent();
        }
        private void SetControlWodth()
        {
            if (Common.m_nWindwMetricsY <= 900)
            {
                comboBox3.Width = 120;
                comboBox3.Left= comboBox2.Left + comboBox2.Width + 5;
                comboBox4.Width = 120;
                comboBox4.Left = comboBox3.Left + comboBox3.Width + 5;
                comboBox5.Width = 120;
                comboBox5.Left = comboBox4.Left + comboBox4.Width + 5;
            }
        }
        private void healthPoorServices_Load(object sender, EventArgs e)
        {
            SetControlWodth();
            isfirst = true;
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
             
            //string jsonjg = httpJk.getDeviceData("411326198905130031", "610929PDY700024", "2024561");
            //MessageBox.Show(jsonjg);
            //ispoor hw = serializer.Deserialize<ispoor>(jsonjg);
            //if (hw.Data == "1")
            //{
            //    String sql = @"update resident_base_info set is_poor = '1' where id_number='" + textBox3.Text + "'";
            //    DbHelperMySQL.ExecuteSql(sql);
            //}
            //MessageBox.Show(hw.Code + "--" + hw.Code + "--" + hw.Message);
            //return;
            time1 = this.dateTimePicker1.Text.ToString() + " 00:00:00";//开始时间
            time2 = this.dateTimePicker2.Text.ToString() + " 00:00:00";//结束时间
            string sql0 = "select b.id_number from zkhw_tj_jk a join resident_base_info b on a.id_number = b.id_number where a.createtime >= '"+ time1 + "' and a.createtime <= '"+time2+"' AND b.is_poor < 1";
            DataSet datapoor = DbHelperMySQL.Query(sql0);
            if (datapoor != null && datapoor.Tables.Count > 0)
            {
                DataTable dtpoor = datapoor.Tables[0];
                for (int i = 0; i < dtpoor.Rows.Count; i++)
                {
                    string jsonjg = httpJk.getDeviceData(dtpoor.Rows[i]["id_number"].ToString(), frmLogin.organCode, frmLogin.userCode);
                    ispoor hw = serializer.Deserialize<ispoor>(jsonjg);
                    if (hw.Data == "1")
                    {
                       String sql = @"update resident_base_info set is_poor = '1' where id_number='" + dtpoor.Rows[i]["id_number"].ToString() + "'";
                       DbHelperMySQL.ExecuteSql(sql);
                    }
                }
            }

            string sql1 = "select code as ID,name as Name from code_area_config where parent_code='-1';";
            DataSet datas = DbHelperMySQL.Query(sql1);
            if (datas != null && datas.Tables.Count > 0)
            {
                List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(datas.Tables[0]);
                Result.Bind(comboBox1, ts, "Name", "ID", "--请选择--");
            }
            cun = basicInfoSettings.xcuncode;
            querytcmHealthServices();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;
            if (pCa != "")
            {
                this.label2.Text = "";
            }
            else { this.label2.Text = "---姓名/身份证号/档案号---"; }
            cun = comboBox5.SelectedValue?.ToString();
            querytcmHealthServices();
        }
        
        private void querytcmHealthServices()
        {
            this.dataGridView1.DataSource = null;
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            DataTable dt=hpd.querytcmHealthServices(pCa, time1, time2,cun);
            if (dt!=null&&dt.Rows.Count < 1)
            {
                if(isfirst==false)
                {
                    MessageBox.Show("未查询出数据，请重新查询!");
                }
                isfirst = false;
                return;
            }
            this.dataGridView1.DataSource = dt;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular);
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[2].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[4].HeaderCell.Value = "性别";
            this.dataGridView1.Columns[5].HeaderCell.Value = "出生日期";
            this.dataGridView1.Columns[6].HeaderCell.Value = "随访日期";
            this.dataGridView1.Columns[7].HeaderCell.Value = "随访类型";
            this.dataGridView1.Columns[8].Visible = false;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dataGridView1.Rows[0].Selected = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = this.dataGridView1.SelectedRows[0].Index;
            string code = dataGridView1["archive_no", row].Value.ToString();
            string idnum = dataGridView1["id_number", row].Value.ToString();
            string doctor_id = dataGridView1["doctor_id", row].Value.ToString();
            DataTable dtcode = hpd.checkTcmHealthServicesByno(code, idnum);
            if (dtcode.Rows.Count > 0)
            {
                MessageBox.Show("此患者健康扶贫服务了,请重新选择!");
                return;
            }
            aUhealthPoorServices addtcm = new aUhealthPoorServices(1, dataGridView1["name", row].Value.ToString(), dataGridView1["archive_no", row].Value.ToString(),dataGridView1["id_number", row].Value.ToString(), dataGridView1["sex", row].Value.ToString(), dataGridView1["birthday", row].Value.ToString());
            addtcm.StartPosition = FormStartPosition.CenterScreen;
            addtcm.doctor_id = doctor_id;
            if (addtcm.ShowDialog() == DialogResult.OK)
            {
                querytcmHealthServices();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = this.dataGridView1.SelectedRows[0].Index;
            aUhealthPoorServices addtcm = new aUhealthPoorServices(0, dataGridView1["name", row].Value.ToString(), dataGridView1["archive_no", row].Value.ToString(), dataGridView1["id_number", row].Value.ToString(), dataGridView1["sex", row].Value.ToString(), dataGridView1["birthday", row].Value.ToString());
            if (addtcm.show)
            {
                addtcm.StartPosition = FormStartPosition.CenterScreen;
                addtcm.ShowDialog();
                querytcmHealthServices();
            }
            else
            {
                MessageBox.Show(addtcm.mag);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除       
                int row = this.dataGridView1.SelectedRows[0].Index;
                string archive_no = dataGridView1["archive_no", row].Value.ToString().Trim();
                string id_number = dataGridView1["id_number", row].Value.ToString().Trim();
                bool istrue = deletetcmHealthServices(archive_no, id_number);
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
        public bool deletetcmHealthServices(string archive_no, string id_number)
        {
            int rt = 0;
            string sql = $"delete from poor_follow_record where archive_no='{archive_no}' and id_number='{id_number}'";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label2.Visible = this.textBox1.Text.Length < 1;
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

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("添加", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(30, 7));

        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("修改", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(30, 7));

        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("删除", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(30, 7));

        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("查询", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 6));

        }
    }
}
