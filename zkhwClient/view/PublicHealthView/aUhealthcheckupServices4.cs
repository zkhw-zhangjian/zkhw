using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.publicRecord;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUhealthcheckupServices4 : Form
    {
        healthCheckupDao hcd = new healthCheckupDao();
        DataTable goodsList = new DataTable();//用药记录 take_medicine_record
        DataTable goodsListym = new DataTable();//疫苗记录 take_medicine_record
        healthCheckupDao hcdao = new healthCheckupDao();
        public aUhealthcheckupServices4()
        {
            InitializeComponent();
        }
        private void aUdiabetesPatientServices_Load(object sender, EventArgs e)
        {
            this.label51.Text = "健康体检表第四页(共四页)";
            this.label51.ForeColor = Color.SkyBlue;
            label51.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label51.Left = (this.panel1.Width - this.label51.Width) / 2;
            label51.BringToFront();

            DataTable dt = hcdao.queryTake_medicine_record(this.textBox1.Text);
            goodsList = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drtmp = goodsList.NewRow();
                drtmp["drug_name"] = dt.Rows[i]["drug_name"].ToString();
                drtmp["drug_usage"] = dt.Rows[i]["drug_usage"].ToString();
                drtmp["drug_use"] = dt.Rows[i]["drug_use"].ToString();
                drtmp["drug_time"] = dt.Rows[i]["drug_time"].ToString();
                drtmp["drug_type"] = dt.Rows[i]["drug_type"].ToString();
                goodsList.Rows.Add(drtmp);
            }
            goodsListBind();//加载用药记录清单表

            DataTable dtym = hcdao.queryVaccination_record(this.textBox1.Text);
            goodsListym = dtym.Clone();
            for (int i = 0; i < dtym.Rows.Count; i++)
            {
                DataRow drtmp = goodsListym.NewRow();
                drtmp["vaccination_name"] = dt.Rows[i]["vaccination_name"].ToString();
                drtmp["vaccination_time"] = dt.Rows[i]["vaccination_time"].ToString();
                drtmp["vaccination_organ_name"] = dt.Rows[i]["vaccination_organ_name"].ToString();
                goodsListym.Rows.Add(drtmp);
            }
            goodsListBindym();//加载用药记录清单表
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();
            per.aichive_no = this.textBox1.Text;
            per.bar_code = this.textBox2.Text;
            per.id_number = this.textBox3.Text;
            if (this.radioButton39.Checked == true) { per.health_evaluation = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true) {
                per.health_evaluation = this.radioButton40.Tag.ToString();
                per.abnormal1 = this.textBox48.Text;
                per.abnormal2 = this.textBox29.Text;
                per.abnormal3 = this.textBox31.Text;
                per.abnormal4 = this.textBox33.Text;
            };

            foreach (Control ctr in this.panel2.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.health_guidance += "," + ck.Text;
                    }
                }
            }
            if (per.health_guidance != null && per.health_guidance != "")
            {
                per.health_guidance = per.health_guidance.Substring(1);
            }

            foreach (Control ctr in this.panel3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.danger_controlling += "," + ck.Text;
                    }
                }
            }
            if (per.danger_controlling != null && per.danger_controlling != "")
            {
                per.danger_controlling = per.danger_controlling.Substring(1);
            }
            if (checkBox8.Checked) {
                per.target_weight= this.textBox37.Text;
            }
            if (checkBox9.Checked)
            {
                per.target_weight = this.textBox39.Text;
            }
            if (checkBox10.Checked)
            {
                per.target_weight = this.textBox40.Text;
            }
            bool isfalse = hcd.addPhysicalExaminationRecord4(per, goodsList, goodsListym);
            if (isfalse)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
        //将用药记录 goodsList 绑定到页面 dataGridView1展示出来
        private void goodsListBind()
        {
            this.dataGridView1.DataSource = goodsList;
            this.dataGridView1.Columns[0].HeaderCell.Value = "药物名称";
            this.dataGridView1.Columns[1].HeaderCell.Value = "药物用法";
            this.dataGridView1.Columns[2].HeaderCell.Value = "药物用量";
            this.dataGridView1.Columns[3].HeaderCell.Value = "用药时间";
            this.dataGridView1.Columns[4].HeaderCell.Value = "服药依从性";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                this.dataGridView1.SelectedRows[0].Selected = false;
            }
            if (goodsList != null && goodsList.Rows.Count > 0)
            {
                this.dataGridView1.Rows[goodsList.Rows.Count - 1].Selected = true;
            }
        }
        //将疫苗记录 goodsList 绑定到页面 dataGridView1展示出来
        private void goodsListBindym()
        {
            this.dataGridView2.DataSource = goodsListym;
            this.dataGridView2.Columns[0].HeaderCell.Value = "疫苗名称";
            this.dataGridView2.Columns[1].HeaderCell.Value = "接种时间";
            this.dataGridView2.Columns[2].HeaderCell.Value = "接种机构";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView2.SelectedRows.Count > 0)
            {
                this.dataGridView2.SelectedRows[0].Selected = false;
            }
            if (goodsListym != null && goodsListym.Rows.Count > 0)
            {
                this.dataGridView2.Rows[goodsListym.Rows.Count - 1].Selected = true;
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            take_medicine_record tmr = new take_medicine_record();
            if (tmr.ShowDialog() == DialogResult.OK)
            {
                DataRow[] drr = goodsList.Select("drug_name = '" + tmr.drug_name.ToString() + "'");
                if (drr.Length > 0)
                {
                    MessageBox.Show("用药记录已存在！");
                    return;
                }
                DataRow drtmp = goodsList.NewRow();
                drtmp["drug_name"] = tmr.drug_name;
                drtmp["drug_usage"] = tmr.drug_usage;
                drtmp["drug_use"] = tmr.drug_use;
                drtmp["drug_time"] = tmr.drug_time;
                drtmp["drug_type"] = tmr.drug_type;
                goodsList.Rows.Add(drtmp);
            }
            goodsListBind();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (goodsList == null) { return; }
            if (goodsList.Rows.Count > 0)
            {
                goodsList.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                goodsListBind();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            vaccination_record vr = new vaccination_record();
            if (vr.ShowDialog() == DialogResult.OK)
            {
                DataRow[] drr = goodsListym.Select("drug_name = '" + vr.vaccination_name + "'");
                if (drr.Length > 0)
                {
                    MessageBox.Show("疫苗记录已存在！");
                    return;
                }
                DataRow drtmp = goodsListym.NewRow();
                drtmp["vaccination_name"] = vr.vaccination_name;
                drtmp["vaccination_time"] = vr.vaccination_time;
                drtmp["vaccination_organ_name"] = vr.vaccination_organ;
                goodsListym.Rows.Add(drtmp);
            }
            goodsListBind();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (goodsListym == null) { return; }
            if (goodsListym.Rows.Count > 0)
            {
                goodsListym.Rows.RemoveAt(this.dataGridView2.SelectedRows[0].Index);
                goodsListBindym();
            }
        }
    }
}
