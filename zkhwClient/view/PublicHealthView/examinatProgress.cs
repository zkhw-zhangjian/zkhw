using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;
using zkhwClient.view.updateTjResult;

namespace zkhwClient.view.PublicHealthView
{
    public partial class examinatProgress : Form
    {
        public string time1 = null;
        public string time2 = null;
        DataTable dt=null;
        areaConfigDao areadao = new areaConfigDao();
        grjdDao grjddao = new grjdDao();
        jkInfoDao jkdao = new jkInfoDao();
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        string xcuncode = null;
        string jmxx = null;
        string str = Application.StartupPath;//项目路径
        public examinatProgress()
        {
            InitializeComponent();
        }
        private void examinatProgress_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-4);
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/check.png");

            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 
            registrationRecordCheck();//体检人数统计
        }
        public void queryExaminatProgress()
        {
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            jmxx = this.textBox1.Text;
            string ytj = "1";
            if (time1 != null && !"".Equals(time1) && time2 != null && !"".Equals(time2))
            {
                dt = jkdao.querytjjd(time1, time2, xcuncode, jmxx);
            }
            else { MessageBox.Show("时间段不能为空!"); return; };

            if (dt != null && dt.Rows.Count > 0)
            {
                if (this.radioButton1.Checked)
                {
                    DataRow[] dr = dt.Select("BChao='" + ytj + "' and XinDian='" + ytj + "' and XueChangGui='" + ytj + "' and NiaoChangGui='" + ytj + "' and Shengaotizhong='" + ytj + "' and XueYa='" + ytj + "' and ShengHua='" + ytj + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        for (int i = dr.Length - 1; i >= 0; i--) {
                            dt.Rows.Remove(dr[i]);
                        }
                    }
                }
                else if (this.radioButton2.Checked)
                {
                    DataRow[] dr = dt.Select("BChao='"+ ytj + "' and XinDian='" + ytj + "' and XueChangGui='" + ytj + "' and NiaoChangGui='" + ytj + "' and Shengaotizhong='" + ytj + "' and XueYa='" + ytj + "' and ShengHua='" + ytj + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        DataTable tmp = dr[0].Table.Clone();  // 复制DataRow的表结构  
                        foreach (DataRow row in dr)
                        {
                            tmp.Rows.Add(row.ItemArray);  // 将DataRow添加到DataTable中  
                        }
                        dt = tmp;
                    }
                }
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].HeaderCell.Value = "体检时间";
                this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
                this.dataGridView1.Columns[2].HeaderCell.Value = "档案编号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "身份证号";
                this.dataGridView1.Columns[4].HeaderCell.Value = "条码号";
                this.dataGridView1.Columns[5].HeaderCell.Value = "B超";
                this.dataGridView1.Columns[6].HeaderCell.Value = "心电图";
                this.dataGridView1.Columns[7].HeaderCell.Value = "生化";
                this.dataGridView1.Columns[8].HeaderCell.Value = "血常规";
                this.dataGridView1.Columns[9].HeaderCell.Value = "尿常规";
                this.dataGridView1.Columns[10].HeaderCell.Value = "血压";
                this.dataGridView1.Columns[11].HeaderCell.Value = "身高体重";
                this.dataGridView1.Columns[0].Width = 120;
                this.dataGridView1.Columns[1].Width = 150;
                this.dataGridView1.Columns[2].Width = 190;
                this.dataGridView1.Columns[3].Width = 190;
                this.dataGridView1.Columns[4].Width = 150;
                this.dataGridView1.Columns[5].Width = 125;
                this.dataGridView1.Columns[6].Width = 125;
                this.dataGridView1.Columns[7].Width = 125;
                this.dataGridView1.Columns[8].Width = 125;
                this.dataGridView1.Columns[9].Width = 125;
                this.dataGridView1.Columns[10].Width = 125;                
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                int rows = this.dataGridView1.Rows.Count - 1 <= 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                for (int x = 0; x <= rows; x++)
                {
                    this.dataGridView1.Rows[x].HeaderCell.Value = String.Format("{0}", x + 1);

                    if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Red;
                    }
                    else {
                        this.dataGridView1.Rows[x].Cells[5].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "--";
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            queryExaminatProgress();
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

        //体检人数统计
        public void registrationRecordCheck()
        {
            DataTable dt16num = grjddao.residentNum(basicInfoSettings.xcuncode);
            if (dt16num != null && dt16num.Rows.Count > 0)
            {
                label9.Text = dt16num.Rows[0][0].ToString();//计划体检人数
            }

            DataTable dt19num = grjddao.jkAllNum(basicInfoSettings.xcuncode);
            if (dt19num != null && dt19num.Rows.Count > 0)
            {
                label11.Text = dt19num.Rows[0][0].ToString();//登记人数
            }
            label13.Text = "0";//未完成人数
            label15.Text = "0";//未到人数
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string str1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string str2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            string str3 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            string str4 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            int columnIndex=e.ColumnIndex;
            if (columnIndex == 5) {
                updateBichao ubichao = new updateBichao();
                ubichao.time = str0;
                ubichao.name = str1;
                ubichao.aichive_no = str2;
                ubichao.id_number = str3;
                ubichao.bar_code = str4;
                ubichao.Show();
            } else if (columnIndex == 6) {
                updateXindiantu uxindt = new updateXindiantu();
                uxindt.time = str0;
                uxindt.name = str1;
                uxindt.aichive_no = str2;
                uxindt.id_number = str3;
                uxindt.bar_code = str4;
                uxindt.Show();
            }
            else if (columnIndex == 7)
            {
                updateShenghua uxindt = new updateShenghua();
                uxindt.time = str0;
                uxindt.name = str1;
                uxindt.aichive_no = str2;
                uxindt.id_number = str3;
                uxindt.bar_code = str4;
                uxindt.Show();
            }
            else if (columnIndex == 8)
            {

            }
            else if (columnIndex == 9)
            {

            }
            else if (columnIndex == 10)
            {

            }
            else if (columnIndex == 11)
            {

            }
        }
    }
}
