using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class healthCheckup : Form
    {
        healthCheckupDao hcd = new healthCheckupDao();
        areaConfigDao areadao = new areaConfigDao();
        string str = Application.StartupPath;//项目路径
        public string time1 = null;
        public string time2 = null;
        public string pCa = "";
        string xcuncode = "";
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        string TarStr = "yyyy-MM-dd";
        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
        public healthCheckup()
        {
            InitializeComponent();
        }

        private void healthCheckup_Load(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);

            //区域
            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 
            xcuncode = basicInfoSettings.xcuncode;
            queryOlderHelthService();
        }

        private void queryOlderHelthService()
        {
            //展示
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            this.dataGridView1.DataSource = null;
            DataTable dt = hcd.queryhealthCheckup(pCa, time1, time2,xcuncode);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[1].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "条码号";
            this.dataGridView1.Columns[4].HeaderCell.Value = "检查日期";
            this.dataGridView1.Columns[5].HeaderCell.Value = "责任医生";
            this.dataGridView1.Columns[6].Visible = false;

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
            queryOlderHelthService();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label5.Visible = this.textBox1.Text.Length < 1;
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string name = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string aichive_no = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string id_number = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string bar_code = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string check_date = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                string doctor_name = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                if (id==null||"".Equals(id)) { MessageBox.Show("未查询到此人的健康体检信息,请调整时间间隔，再点击查询！！"); return; }
                DataTable dtup= hcd.queryhealthCheckup(id);
                if (dtup.Rows.Count>0) { MessageBox.Show(name+"已有健康体检基本信息，请点击修改按钮!");return; }
                if (aichive_no != null && !"".Equals(aichive_no))
                {
                    aUhealthcheckupServices1 auhcs = new aUhealthcheckupServices1();
                    auhcs.textBox1.Text = name;
                    auhcs.textBox118.Text = bar_code;
                    auhcs.textBox119.Text = id_number;
                    auhcs.textBox120.Text = id;
                    auhcs.textBox2.Text = aichive_no;
                    if (check_date != "")
                    {
                        auhcs.dateTimePicker1.Value = DateTime.ParseExact(check_date, TarStr, format);
                    }
                    auhcs.textBox51.Text = doctor_name;
                    auhcs.id = id;
                    auhcs.Show();
                    //aUhealthcheckupServices3 auhcs = new aUhealthcheckupServices3();
                    //auhcs.id = id;
                    //auhcs.textBox107.Text = id_number;
                    //auhcs.textBox106.Text = aichive_no;
                    //auhcs.textBox105.Text = bar_code;
                    //auhcs.textBox108.Text = id;
                    //auhcs.Show();
                }
            }
            else {
                MessageBox.Show("请选择一行！");
            }
        }
        //删除数据
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
                bool istrue = hcd.deletePhysical_examination_record(id);
                if (istrue)
                {
                    hcd.deleteVaccination_record(id);
                    hcd.deleteTake_medicine_record(id);
                    hcd.deleteHospitalized_record(id);
                    //刷新页面
                    queryOlderHelthService();
                    MessageBox.Show("删除成功！");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string name = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string aichive_no = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string id_number = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string bar_code = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string check_date = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                string doctor_name = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                if (id == null || "".Equals(id)) { MessageBox.Show("未查询到此人的健康体检信息,请调整时间间隔，再点击查询！"); return; }
                if (aichive_no != null && !"".Equals(aichive_no))
                {
                    aUhealthcheckupServices1 auhcs = new aUhealthcheckupServices1();
                    auhcs.textBox1.Text = name;
                    auhcs.textBox118.Text = bar_code;
                    auhcs.textBox119.Text = id_number;
                    auhcs.textBox120.Text = id;
                    auhcs.textBox2.Text = aichive_no;
                    if (check_date != "")
                    {
                        auhcs.dateTimePicker1.Value = DateTime.ParseExact(check_date, TarStr, format);
                    }
                    auhcs.textBox51.Text = doctor_name;

                    auhcs.id = id;//祖
                    auhcs.Show();
                    //aUhealthcheckupServices3 auhcs = new aUhealthcheckupServices3();
                    //auhcs.textBox107.Text = id_number;
                    //auhcs.textBox106.Text = aichive_no;
                    //auhcs.textBox105.Text = bar_code;
                    //auhcs.textBox108.Text = id;
                    //auhcs.Show();
                }
            }
            else
            {
                MessageBox.Show("请选择一行！");
            }
        }
    }
}
