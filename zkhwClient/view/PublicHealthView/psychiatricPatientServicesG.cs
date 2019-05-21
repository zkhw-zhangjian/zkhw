using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.service;
using zkhwClient.view.PublicHealthView;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class psychiatricPatientServicesG : Form
    {
        psychiatricPatientService psychiatricPatient = new psychiatricPatientService();
        areaConfigDao areadao = new areaConfigDao();
        public string pCa = "";
        public string time1 = null;
        public string time2 = null;
        string xcuncode = "";
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        public psychiatricPatientServicesG()
        {
            InitializeComponent();
        }
        private void hypertensionPatientServices_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);

            this.label4.Text = "严重精神病障碍患者信息";
            this.label4.ForeColor = Color.SkyBlue;
            label4.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label4.Left = (this.panel1.Width - this.label4.Width) / 2;
            label4.BringToFront();

            //区域
            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 

            xcuncode = basicInfoSettings.xcuncode;
            queryPsychosis_info();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;//patientName Cardcode aichive_no
            if (pCa != "")
            {
                this.label2.Text = "";
            }
            else { this.label2.Text = "---姓名/身份证号/档案号---"; }
            //time1 = this.dateTimePicker1.Text.ToString();//开始时间
            //time2 = this.dateTimePicker2.Text.ToString();//结束时间
            queryPsychosis_info();
        }
        private void queryPsychosis_info()
        {
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            this.dataGridView1.DataSource = null;
            DataTable dt = psychiatricPatient.queryPsychosis_info(pCa, time1, time2, xcuncode);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[1].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "随访日期";
            this.dataGridView1.Columns[4].HeaderCell.Value = "监护人";
            this.dataGridView1.Columns[5].HeaderCell.Value = "随访医生";
            this.dataGridView1.Columns[6].Visible = false;

            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //添加 修改 
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string name = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string aichive_no = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string id_number = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                DataTable dtcf = psychiatricPatient.queryPsychosis_info(id);
                if (dtcf.Rows.Count>0) { MessageBox.Show("此患者已添加过神障碍患者个人补充信息,请重新操作!");return; }
                if (aichive_no != null && !"".Equals(aichive_no))
                {
                    aUpsychiatricPatientServices hm = new aUpsychiatricPatientServices();
                    hm.id = id;//祖
                    hm.textBox37.Text = name;
                    hm.textBox39.Text = aichive_no;
                    hm.textBox41.Text = id_number;
                    if (hm.ShowDialog() == DialogResult.OK)
                    {
                        //刷新页面
                        queryPsychosis_info();
                        MessageBox.Show("添加成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string name = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string aichive_no = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string id_number = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                if (aichive_no != null && !"".Equals(aichive_no))
                {
                    aUpsychiatricPatientServices hm = new aUpsychiatricPatientServices();
                    hm.id = id;//祖
                    hm.textBox37.Text = name;
                    hm.textBox39.Text = aichive_no;
                    hm.textBox41.Text = id_number;
                    if (hm.ShowDialog() == DialogResult.OK)
                    {
                        //刷新页面
                        queryPsychosis_info();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择一行！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();

            DialogResult rr = MessageBox.Show("确认删除补充信息？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
                bool istrue = psychiatricPatient.deletePsychosis_info(id);
                if (istrue)
                {
                    //刷新页面
                    queryPsychosis_info();
                    MessageBox.Show("删除成功！");
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label2.Visible = this.textBox1.Text.Length < 1;
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
    }
}
