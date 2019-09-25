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
    public partial class aUPersonalBasicInfoo : Form
    {

        service.personalBasicInfoService personalBasicInfoService = new service.personalBasicInfoService();
        public string id = "";
        public string mzid = "";
        DataTable goodsList = new DataTable();//既往史疾病清单表 resident_diseases
        DataTable goodsList0 = new DataTable();//既往史手术清单表 operation_record
        DataTable goodsList1 = new DataTable();//既往史外伤清单表 traumatism_record
        DataTable goodsList2 = new DataTable();//既往史输血清单表 metachysis_record
        DataTable goodsList3 = new DataTable();//家族史表 family_record
        public string oldname = "";
        public string oldidnumber = "";

        private float xMy;//定义当前窗体的宽度
        private float yMy;//定义当前窗体的高度
        public aUPersonalBasicInfoo()
        {
            InitializeComponent();

            //xMy = this.Width;
            //yMy = this.Height;
            //Common.setTag(this);
        }
        private void aUHypertensionPatientServices_Load(object sender, EventArgs e)
        {
            //this.label47.ForeColor = Color.SkyBlue;
            //label47.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            //label47.Left = (this.panel1.Width - this.label47.Width) / 2;
            //label47.BringToFront();

            DataTable dtno =Common.GetNationDataTable(1);
            this.comboBox1.DataSource = dtno;//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "id";//操作时获取的值
            if (mzid!="") {
                this.comboBox1.SelectedValue = mzid;
            }
            //既往史疾病清单表 
            DataTable dt = personalBasicInfoService.queryResident_diseases(id);
            goodsList = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drtmp = goodsList.NewRow();
                drtmp["id"] = dt.Rows[i]["id"].ToString();
                drtmp["resident_base_info_id"] = dt.Rows[i]["resident_base_info_id"].ToString();
                drtmp["disease_name"] = dt.Rows[i]["disease_name"].ToString();
                drtmp["disease_date"] = dt.Rows[i]["disease_date"].ToString();
                drtmp["disease_type"] = dt.Rows[i]["disease_type"].ToString();
                goodsList.Rows.Add(drtmp);
            }
            goodsListBind();
            ///////////////////////////////////
            //既往史手术清单表 
            DataTable dt0 = personalBasicInfoService.queryOperation_record(id);
            goodsList0 = dt0.Clone();
            for (int i = 0; i < dt0.Rows.Count; i++)
            {
                DataRow drtmp = goodsList0.NewRow();
                drtmp["id"] = dt0.Rows[i]["id"].ToString();
                drtmp["resident_base_info_id"] = dt0.Rows[i]["resident_base_info_id"].ToString();
                drtmp["operation_name"] = dt0.Rows[i]["operation_name"].ToString();
                drtmp["operation_time"] = dt0.Rows[i]["operation_time"].ToString(); 
                drtmp["operation_code"] = dt0.Rows[i]["operation_code"].ToString(); 
                goodsList0.Rows.Add(drtmp);
            }
            goodsList0Bind();
            ///////////////////////////////////
            //既往史外伤清单表 traumatism_record 
            DataTable dt1 = personalBasicInfoService.queryTraumatism_record(id);
            goodsList1 = dt1.Clone();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DataRow drtmp = goodsList1.NewRow();
                drtmp["id"] = dt1.Rows[i]["id"].ToString();
                drtmp["resident_base_info_id"] = dt1.Rows[i]["resident_base_info_id"].ToString();
                drtmp["traumatism_name"] = dt1.Rows[i]["traumatism_name"].ToString();
                drtmp["traumatism_time"] = dt1.Rows[i]["traumatism_time"].ToString();
                drtmp["traumatism_code"] = dt1.Rows[i]["traumatism_code"].ToString();
                goodsList1.Rows.Add(drtmp);
            }
            goodsList1Bind();
            ///////////////////////////////////
            //既往史输血清单表 metachysis_record
            DataTable dt2 = personalBasicInfoService.queryMetachysis_record(id);
            goodsList2 = dt2.Clone();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                DataRow drtmp = goodsList2.NewRow();
                drtmp["id"] = dt2.Rows[i]["id"].ToString();
                drtmp["resident_base_info_id"] = dt2.Rows[i]["resident_base_info_id"].ToString();
                drtmp["metachysis_reasonn"] = dt2.Rows[i]["metachysis_reasonn"].ToString();
                drtmp["metachysis_time"] = dt2.Rows[i]["metachysis_time"].ToString();
                drtmp["metachysis_code"] = dt2.Rows[i]["metachysis_code"].ToString();
                goodsList2.Rows.Add(drtmp);
            }
            goodsList2Bind();
            ///////////////////////////////////
            //家族史表 family_record 
            DataTable dt3 = personalBasicInfoService.queryFamily_record(id);
            goodsList3 = dt3.Clone();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                DataRow drtmp = goodsList3.NewRow();
                drtmp["id"] = dt3.Rows[i]["id"].ToString();
                drtmp["resident_base_info_id"] = dt3.Rows[i]["resident_base_info_id"].ToString();
                drtmp["relation"] = dt3.Rows[i]["relation"].ToString();
                drtmp["disease_name"] = dt3.Rows[i]["disease_name"].ToString(); 
                drtmp["disease_type"] = dt3.Rows[i]["disease_type"].ToString();
                goodsList3.Rows.Add(drtmp);
            }
            goodsList3Bind();
        }

        //既往史疾病清单表 resident_diseases////////////////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            resident_diseases hm = new resident_diseases();
            hm._displaydt = "0";
            if (hm.ShowDialog() == DialogResult.OK)
            {
                DataRow[] drr = goodsList.Select("disease_name = '" + hm.disease_name.ToString() + "'");
                if (drr.Length > 0)
                {
                    MessageBox.Show("疾病记录已存在！");
                    return;
                }
                DataRow drtmp = goodsList.NewRow();
                drtmp["id"] = 0;
                drtmp["resident_base_info_id"] = id;
                drtmp["disease_name"] = hm.disease_name.ToString();
                drtmp["disease_date"] = hm.disease_date.ToString(); 
                drtmp["disease_type"] = hm.disease_type.ToString();
                goodsList.Rows.Add(drtmp);
            }
            goodsListBind();
        }
        private void goodsListBind()
        {

            this.dataGridView1.DataSource = goodsList;
            this.dataGridView1.Columns[0].Visible = false;//id
            this.dataGridView1.Columns[1].Visible = false;//resident_base_info_id
            this.dataGridView1.Columns[2].HeaderCell.Value = "疾病名称";
            this.dataGridView1.Columns[3].HeaderCell.Value = "确认日期";
            this.dataGridView1.Columns[4].Visible = false;//disease_type


            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                this.dataGridView1.SelectedRows[0].Selected = false;
            }
            if (goodsList != null && goodsList.Rows.Count > 0)
            {
                this.dataGridView1.Rows[goodsList.Rows.Count - 1].Selected = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (goodsList == null) { return; }
            if (goodsList.Rows.Count > 0)
            {
                goodsList.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                goodsListBind();
            }
        }
     //既往史手术清单表  operation_record
        private void button3_Click(object sender, EventArgs e)
        {
            if (goodsList0.Rows.Count >= 2)
            {
                MessageBox.Show("不能超过2条数据!"); return;
            }
            operation_record hm = new operation_record();
            if (hm.ShowDialog() == DialogResult.OK)
            {
                DataRow drtmp = goodsList0.NewRow();
                drtmp["id"] = 0;
                drtmp["resident_base_info_id"] = id;
                drtmp["operation_name"] = hm.operation_name.ToString();
                drtmp["operation_time"] = hm.operation_time.ToString();
                drtmp["operation_code"] = hm.operation_code.ToString();
                goodsList0.Rows.Add(drtmp);
            }
            goodsList0Bind();
        }
        private void goodsList0Bind()
        {

            this.dataGridView2.DataSource = goodsList0;
            this.dataGridView2.Columns[0].Visible = false;//id
            this.dataGridView2.Columns[1].Visible = false;//resident_base_info_id
            this.dataGridView2.Columns[2].HeaderCell.Value = "手术名称";
            this.dataGridView2.Columns[3].HeaderCell.Value = "手术时间";
            this.dataGridView2.Columns[4].Visible = false;//operation_code


            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView2.SelectedRows.Count > 0)
            {
                this.dataGridView2.SelectedRows[0].Selected = false;
            }
            if (goodsList0 != null && goodsList0.Rows.Count > 0)
            {
                this.dataGridView2.Rows[goodsList0.Rows.Count - 1].Selected = true;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (goodsList0 == null) { return; }
            if (goodsList0.Rows.Count > 0)
            {
                goodsList0.Rows.RemoveAt(this.dataGridView2.SelectedRows[0].Index);
                goodsList0Bind();
            }
        }
      //既往史外伤清单表 traumatism_record
        private void button7_Click(object sender, EventArgs e)
        {
            if (goodsList1.Rows.Count >= 2)
            {
                MessageBox.Show("不能超过2条数据!"); return;
            }
            traumatism_record hm = new traumatism_record();
            if (hm.ShowDialog() == DialogResult.OK)
            {
                DataRow drtmp = goodsList1.NewRow();
                drtmp["id"] = 0;
                drtmp["resident_base_info_id"] = id;
                drtmp["traumatism_name"] = hm.traumatism_name.ToString();
                drtmp["traumatism_time"] = hm.traumatism_time.ToString();
                drtmp["traumatism_code"] = hm.traumatism_code.ToString();
                goodsList1.Rows.Add(drtmp);
            }
            goodsList1Bind();
        }
        private void goodsList1Bind()
        {

            this.dataGridView3.DataSource = goodsList1;
            this.dataGridView3.Columns[0].Visible = false;//id
            this.dataGridView3.Columns[1].Visible = false;//resident_base_info_id
            this.dataGridView3.Columns[2].HeaderCell.Value = "外伤名称";
            this.dataGridView3.Columns[3].HeaderCell.Value = "外伤时间";
            this.dataGridView3.Columns[4].Visible = false;//traumatism_code


            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView3.SelectedRows.Count > 0)
            {
                this.dataGridView3.SelectedRows[0].Selected = false;
            }
            if (goodsList1 != null && goodsList1.Rows.Count > 0)
            {
                this.dataGridView3.Rows[goodsList1.Rows.Count - 1].Selected = true;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (goodsList1 == null) { return; }
            if (goodsList1.Rows.Count > 0)
            {
                goodsList1.Rows.RemoveAt(this.dataGridView3.SelectedRows[0].Index);
                goodsList1Bind();
            }
        }
        //既往史输血清单表 metachysis_record
        private void button9_Click(object sender, EventArgs e)
        {
            if (goodsList2.Rows.Count >= 2)
            {
                MessageBox.Show("不能超过2条数据!"); return;
            }
            metachysis_record hm = new metachysis_record();
            if (hm.ShowDialog() == DialogResult.OK)
            {
                DataRow drtmp = goodsList2.NewRow();
                drtmp["id"] = 0;
                drtmp["resident_base_info_id"] = id;
                drtmp["metachysis_reasonn"] = hm.metachysis_reasonn.ToString();
                drtmp["metachysis_time"] = hm.metachysis_time.ToString();
                drtmp["metachysis_code"] = hm.metachysis_code.ToString();
                goodsList2.Rows.Add(drtmp);
                
            }
            goodsList2Bind();
        }
        private void goodsList2Bind()
        {
            this.dataGridView4.DataSource = goodsList2;
            this.dataGridView4.Columns[0].Visible = false;//id
            this.dataGridView4.Columns[1].Visible = false;//resident_base_info_id
            this.dataGridView4.Columns[2].HeaderCell.Value = "输血原因";
            this.dataGridView4.Columns[3].HeaderCell.Value = "输血时间";
            this.dataGridView4.Columns[4].Visible = false;//metachysis_code


            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView4.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView4.SelectedRows.Count > 0)
            {
                this.dataGridView4.SelectedRows[0].Selected = false;
            }
            if (goodsList2 != null && goodsList2.Rows.Count > 0)
            {
                this.dataGridView4.Rows[goodsList2.Rows.Count - 1].Selected = true;
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (goodsList2 == null) { return; }
            if (goodsList2.Rows.Count > 0)
            {
                goodsList2.Rows.RemoveAt(this.dataGridView4.SelectedRows[0].Index);
                goodsList2Bind();
            }
        }
        //家族史表 family_record
        private void button11_Click(object sender, EventArgs e)
        {
            family_record hm = new family_record();
            if (hm.ShowDialog() == DialogResult.OK)
            {
                DataRow[] drr = goodsList3.Select("relation = '" + hm.relation.ToString() + "'");
                if (drr.Length > 0)
                {
                    MessageBox.Show("关系已存在！");
                    return;
                }
                DataRow drtmp = goodsList3.NewRow();
                drtmp["id"] = 0;
                drtmp["resident_base_info_id"] = id;
                drtmp["relation"] = hm.relation.ToString();
                drtmp["disease_name"] = hm.disease_name.ToString();
                drtmp["disease_type"] = hm.disease_type.ToString();

                goodsList3.Rows.Add(drtmp);
            }
            goodsList3Bind();
        }
        private void goodsList3Bind()
        {

            this.dataGridView6.DataSource = goodsList3;
            this.dataGridView6.Columns[0].Visible = false;//id
            this.dataGridView6.Columns[1].Visible = false;//resident_base_info_id
            this.dataGridView6.Columns[2].HeaderCell.Value = "关系";
            this.dataGridView6.Columns[3].HeaderCell.Value = "疾病名称";
            this.dataGridView6.Columns[4].Visible = false;//disease_type


            this.dataGridView6.AllowUserToAddRows = false;
            this.dataGridView6.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView6.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView6.SelectedRows.Count > 0)
            {
                this.dataGridView6.SelectedRows[0].Selected = false;
            }
            if (goodsList3 != null && goodsList3.Rows.Count > 0)
            {
                this.dataGridView6.Rows[goodsList3.Rows.Count - 1].Selected = true;
            }
            for (int i = 0; i < this.dataGridView6.Rows.Count; i++) {
                string ftype = this.dataGridView6.Rows[i].Cells[2].Value.ToString();
                if (ftype == "1")
                {
                    this.dataGridView6.Rows[i].Cells[2].Value = "父亲";
                }
                else if(ftype == "2")
                {
                    this.dataGridView6.Rows[i].Cells[2].Value = "母亲";
                }
                else if (ftype == "3")
                {
                    this.dataGridView6.Rows[i].Cells[2].Value = "兄弟姐妹";
                }
                else if (ftype == "4")
                {
                    this.dataGridView6.Rows[i].Cells[2].Value = "子女";
                }
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (goodsList3 == null) { return; }
            if (goodsList3.Rows.Count > 0)
            {
                goodsList3.Rows.RemoveAt(this.dataGridView6.SelectedRows[0].Index);
                goodsList3Bind();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bean.resident_base_infoBean resident_base_infoBean = new bean.resident_base_infoBean();

            resident_base_infoBean.name = this.textBox1.Text.Replace(" ", "");
            resident_base_infoBean.archive_no = this.textBox2.Text.Replace(" ", "");
            resident_base_infoBean.pb_archive = this.textBox2.Text.Replace(" ", "");
            if (this.radioButton1.Checked == true) { resident_base_infoBean.sex = this.radioButton1.Tag.ToString(); };
            if (this.radioButton2.Checked == true) { resident_base_infoBean.sex = this.radioButton2.Tag.ToString(); };
            if (this.radioButton3.Checked == true) { resident_base_infoBean.sex = this.radioButton3.Tag.ToString(); };
            if (this.radioButton25.Checked == true) { resident_base_infoBean.sex = this.radioButton25.Tag.ToString(); };
            resident_base_infoBean.birthday = this.dateTimePicker1.Text;
            resident_base_infoBean.id_number = this.textBox12.Text.Replace(" ", "");
            if (resident_base_infoBean.id_number=="" || resident_base_infoBean.id_number.Length!=18) {
                MessageBox.Show("身份证号码不正确!");
                return;
            }
            else
            {
                //判断下身份证号不能重复
                DataTable dt = personalBasicInfoService.query(resident_base_infoBean.id_number);
                if(dt.Rows.Count>0)
                {
                    if(dt.Rows[0]["archive_no"].ToString()!= resident_base_infoBean.archive_no)
                    {
                        MessageBox.Show("身份证号码已经使用!");
                        return;
                    }
                }
            }
            resident_base_infoBean.address= this.textBox44.Text.Replace(" ", "");
            resident_base_infoBean.residence_address = this.textBox46.Text.Replace(" ", "");

            resident_base_infoBean.company = this.textBox14.Text.Replace(" ", "");
            resident_base_infoBean.phone = this.textBox16.Text.Replace(" ", "");
            resident_base_infoBean.link_name = this.textBox18.Text.Replace(" ", "");
            resident_base_infoBean.link_phone = this.textBox20.Text.Replace(" ", "");
            if (this.radioButton4.Checked == true) { resident_base_infoBean.resident_type = this.radioButton4.Tag.ToString(); };
            if (this.radioButton5.Checked == true) { resident_base_infoBean.resident_type = this.radioButton5.Tag.ToString(); };

            if (this.radioButton6.Checked == true) { resident_base_infoBean.nation = this.radioButton6.Tag.ToString(); };
            if (this.radioButton7.Checked == true) { if(mzid!="") resident_base_infoBean.nation = mzid; };

            if (this.radioButton8.Checked == true) { resident_base_infoBean.blood_group = this.radioButton8.Tag.ToString(); };
            if (this.radioButton9.Checked == true) { resident_base_infoBean.blood_group = this.radioButton9.Tag.ToString(); };
            if (this.radioButton10.Checked == true) { resident_base_infoBean.blood_group = this.radioButton10.Tag.ToString(); };
            if (this.radioButton11.Checked == true) { resident_base_infoBean.blood_group = this.radioButton11.Tag.ToString(); };
            if (this.radioButton12.Checked == true) { resident_base_infoBean.blood_group = this.radioButton12.Tag.ToString(); };

            if (this.radioButton13.Checked == true) { resident_base_infoBean.blood_rh = this.radioButton13.Tag.ToString(); };
            if (this.radioButton14.Checked == true) { resident_base_infoBean.blood_rh = this.radioButton14.Tag.ToString(); };
            if (this.radioButton15.Checked == true) { resident_base_infoBean.blood_rh = this.radioButton15.Tag.ToString(); };

            if (this.radioButton22.Checked == true) { resident_base_infoBean.education = this.radioButton22.Tag.ToString(); };
            if (this.radioButton23.Checked == true) { resident_base_infoBean.education = this.radioButton23.Tag.ToString(); };
            if (this.radioButton24.Checked == true) { resident_base_infoBean.education = this.radioButton24.Tag.ToString(); };
            if (this.radioButton26.Checked == true) { resident_base_infoBean.education = this.radioButton26.Tag.ToString(); };
            if (this.radioButton27.Checked == true) { resident_base_infoBean.education = this.radioButton27.Tag.ToString(); };
            if (this.radioButton28.Checked == true) { resident_base_infoBean.education = this.radioButton28.Tag.ToString(); };
            if (this.radioButton29.Checked == true) { resident_base_infoBean.education = this.radioButton29.Tag.ToString(); };
            if (this.radioButton30.Checked == true) { resident_base_infoBean.education = this.radioButton30.Tag.ToString(); };
            if (this.radioButton31.Checked == true) { resident_base_infoBean.education = this.radioButton31.Tag.ToString(); };
            if (this.radioButton32.Checked == true) { resident_base_infoBean.education = this.radioButton32.Tag.ToString(); };

            if (this.radioButton33.Checked == true) { resident_base_infoBean.profession = this.radioButton33.Tag.ToString(); };
            if (this.radioButton34.Checked == true) { resident_base_infoBean.profession = this.radioButton34.Tag.ToString(); };
            if (this.radioButton35.Checked == true) { resident_base_infoBean.profession = this.radioButton35.Tag.ToString(); };
            if (this.radioButton36.Checked == true) { resident_base_infoBean.profession = this.radioButton36.Tag.ToString(); };
            if (this.radioButton37.Checked == true) { resident_base_infoBean.profession = this.radioButton37.Tag.ToString(); };
            if (this.radioButton38.Checked == true) { resident_base_infoBean.profession = this.radioButton38.Tag.ToString(); };
            if (this.radioButton39.Checked == true) { resident_base_infoBean.profession = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true) { resident_base_infoBean.profession = this.radioButton40.Tag.ToString(); };
            if (this.radioButton41.Checked == true) { resident_base_infoBean.profession = this.radioButton41.Tag.ToString(); };

            if (this.radioButton42.Checked == true) { resident_base_infoBean.marital_status = this.radioButton42.Tag.ToString(); };
            if (this.radioButton43.Checked == true) { resident_base_infoBean.marital_status = this.radioButton43.Tag.ToString(); };
            if (this.radioButton44.Checked == true) { resident_base_infoBean.marital_status = this.radioButton44.Tag.ToString(); };
            if (this.radioButton45.Checked == true) { resident_base_infoBean.marital_status = this.radioButton45.Tag.ToString(); };
            if (this.radioButton46.Checked == true) { resident_base_infoBean.marital_status = this.radioButton46.Tag.ToString(); };

            foreach (Control ctr in this.panel12.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        resident_base_infoBean.pay_type += "," + ck.Tag.ToString(); ;
                    }
                }
            }
            if (resident_base_infoBean.pay_type != null && resident_base_infoBean.pay_type != "")
            {
                string tmp1 = resident_base_infoBean.pay_type.Substring(1);
                string[] b = tmp1.Split(',');
                int[] a = new int[b.Length];
                string str = "";
                if (b.Length > 1)
                {
                    for (int i = 0; i < b.Length; i++)
                    {
                        a[i] = int.Parse(b[i].ToString());
                    }
                    Array.Sort(a);
                    str = string.Join(",", a);
                }
                else
                {
                    str = b[0];
                }
                resident_base_infoBean.pay_type = str; 
            }
            if (this.checkBox8.Checked) {
                resident_base_infoBean.pay_other = this.textBox38.Text;
            }
            foreach (Control ctr in this.panel13.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        resident_base_infoBean.drug_allergy += "," + ck.Tag.ToString(); ;
                    }
                }
            }
            if (resident_base_infoBean.drug_allergy != null && resident_base_infoBean.drug_allergy != "")
            {
                string tmp1 = resident_base_infoBean.drug_allergy.Substring(1);
                string[] b = tmp1.Split(',');
                int[] a = new int[b.Length];
                string str = "";
                if (b.Length > 1)
                {
                    for (int i = 0; i < b.Length; i++)
                    {
                        a[i] = int.Parse(b[i].ToString());
                    }
                    Array.Sort(a);
                    str = string.Join(",", a);
                }
                else
                {
                    str = b[0];
                }
                resident_base_infoBean.drug_allergy = str; 
            }
            if (this.checkBox13.Checked)
            {
                resident_base_infoBean.allergy_other = this.textBox39.Text;
            }
            foreach (Control ctr in this.panel14.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        resident_base_infoBean.exposure += "," + ck.Tag.ToString();
                    }
                }
            }
            if (resident_base_infoBean.exposure != null && resident_base_infoBean.exposure != "")
            {
                string tmp1 = resident_base_infoBean.exposure.Substring(1);
                string[] b = tmp1.Split(',');
                int[] a = new int[b.Length];
                string str = "";
                if (b.Length > 1)
                {
                    for (int i = 0; i < b.Length; i++)
                    {
                        a[i] = int.Parse(b[i].ToString());
                    }
                    Array.Sort(a);
                    str = string.Join(",", a);
                }
                else
                {
                    str = b[0];
                }
                resident_base_infoBean.exposure = str;
            }
            if (this.radioButton48.Checked == true) { resident_base_infoBean.is_heredity = this.radioButton48.Tag.ToString(); };
            if (this.radioButton47.Checked == true) { resident_base_infoBean.is_heredity = this.radioButton47.Tag.ToString();
                resident_base_infoBean.heredity_name = this.textBox36.Text;
            };
            
                foreach (Control ctr in this.panel20.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (ck.Checked)
                        {
                            resident_base_infoBean.is_deformity += "," + ck.Tag.ToString();
                        }
                    }
                }
                if (resident_base_infoBean.is_deformity != null && resident_base_infoBean.is_deformity != "")
                { 
                    string tmp1 = resident_base_infoBean.is_deformity.Substring(1);
                    string[] b = tmp1.Split(',');
                    int[] a = new int[b.Length];
                    string str = "";
                    if (b.Length > 1)
                    {
                        for (int i = 0; i < b.Length; i++)
                        {
                            a[i] = int.Parse(b[i].ToString());
                        }
                        Array.Sort(a);
                        str = string.Join(",", a);
                    }
                    else
                    {
                        str = b[0];
                    }
                    resident_base_infoBean.is_deformity = str;

                if (this.checkBox33.Checked)resident_base_infoBean.deformity_name = this.textBox37.Text;
                }
            

            if (this.radioButton18.Checked == true) { resident_base_infoBean.kitchen = this.radioButton18.Tag.ToString(); };
            if (this.radioButton19.Checked == true) { resident_base_infoBean.kitchen = this.radioButton19.Tag.ToString(); };
            if (this.radioButton20.Checked == true) { resident_base_infoBean.kitchen = this.radioButton20.Tag.ToString(); };
            if (this.radioButton21.Checked == true) { resident_base_infoBean.kitchen = this.radioButton21.Tag.ToString(); };

            if (this.radioButton70.Checked == true) { resident_base_infoBean.fuel = this.radioButton70.Tag.ToString(); };
            if (this.radioButton71.Checked == true) { resident_base_infoBean.fuel = this.radioButton71.Tag.ToString(); };
            if (this.radioButton72.Checked == true) { resident_base_infoBean.fuel = this.radioButton72.Tag.ToString(); };
            if (this.radioButton73.Checked == true) { resident_base_infoBean.fuel = this.radioButton73.Tag.ToString(); };
            if (this.radioButton74.Checked == true) { resident_base_infoBean.fuel = this.radioButton74.Tag.ToString(); };
            if (this.radioButton75.Checked == true) { resident_base_infoBean.fuel = this.radioButton75.Tag.ToString(); };

            if (this.radioButton76.Checked == true) { resident_base_infoBean.drink = this.radioButton76.Tag.ToString(); };
            if (this.radioButton77.Checked == true) { resident_base_infoBean.drink = this.radioButton77.Tag.ToString(); };
            if (this.radioButton78.Checked == true) { resident_base_infoBean.drink = this.radioButton78.Tag.ToString(); };
            if (this.radioButton79.Checked == true) { resident_base_infoBean.drink = this.radioButton79.Tag.ToString(); };
            if (this.radioButton80.Checked == true) { resident_base_infoBean.drink = this.radioButton80.Tag.ToString(); };
            if (this.radioButton81.Checked == true) { resident_base_infoBean.drink = this.radioButton81.Tag.ToString(); };

            if (this.radioButton82.Checked == true) { resident_base_infoBean.toilet = this.radioButton82.Tag.ToString(); };
            if (this.radioButton83.Checked == true) { resident_base_infoBean.toilet = this.radioButton83.Tag.ToString(); };
            if (this.radioButton84.Checked == true) { resident_base_infoBean.toilet = this.radioButton84.Tag.ToString(); };
            if (this.radioButton85.Checked == true) { resident_base_infoBean.toilet = this.radioButton85.Tag.ToString(); };
            if (this.radioButton86.Checked == true) { resident_base_infoBean.toilet = this.radioButton86.Tag.ToString(); };

            if (this.radioButton87.Checked == true) { resident_base_infoBean.poultry = this.radioButton87.Tag.ToString(); };
            if (this.radioButton88.Checked == true) { resident_base_infoBean.poultry = this.radioButton88.Tag.ToString(); };
            if (this.radioButton89.Checked == true) { resident_base_infoBean.poultry = this.radioButton89.Tag.ToString(); };
            if (this.radioButton90.Checked == true) { resident_base_infoBean.poultry = this.radioButton90.Tag.ToString(); };

            ////以下页面未用 数据库字段格式要求
            resident_base_infoBean.create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            resident_base_infoBean.update_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            resident_base_infoBean.update_name = frmLogin.name;
            resident_base_infoBean.create_name = frmLogin.name;
            resident_base_infoBean.create_user = frmLogin.userCode;
            resident_base_infoBean.is_hypertension = "0";   //是否高血压
            resident_base_infoBean.is_diabetes = "0";       //是否糖尿病    
            resident_base_infoBean.is_psychosis = "0";      //是否精神病 
            resident_base_infoBean.is_tuberculosis = "0";   //是否结核病
             
            ////resident_base_infoBean.is_deformity = "0";
            #region  高 糖 精 结 新的判断方法 
            for (int i=0;i< goodsList.Rows.Count;i++)
            {
                string tmp = goodsList.Rows[i]["disease_type"].ToString();
                switch (tmp)
                {
                    case "2":
                        resident_base_infoBean.is_hypertension = "1";
                        break;
                    case "3":
                        resident_base_infoBean.is_diabetes = "1";
                        break;
                    case "8":
                        resident_base_infoBean.is_psychosis = "1";
                        break;
                    case "9":
                        resident_base_infoBean.is_tuberculosis = "1";
                        break;
                }
            }
            #endregion 
            //贫困户处理
            if (radpinkun0.Checked==true)
            {
                resident_base_infoBean.is_poor = "0";
            }
            if(radpinkun1.Checked == true)
            {
                resident_base_infoBean.is_poor = "1";
            }

            if(radioButton17.Checked==true)
            {
                resident_base_infoBean.is_signing = "0";
            }
            if(radioButton16.Checked==true)
            {
                resident_base_infoBean.is_signing = "1";
            }
            int intbian = 0;
            if(oldname != resident_base_infoBean.name)
            {
                intbian = 1;  //姓名改变
            }
            if (oldidnumber != resident_base_infoBean.id_number)
            {
                intbian = 2;   //身份证号改变
            }
            if (oldname != resident_base_infoBean.name && oldidnumber != resident_base_infoBean.id_number)
            {
                intbian = 3;   //两个都改变了
            }
            
            bool isfalse = personalBasicInfoService.aUpersonalBasicInfo(resident_base_infoBean, id, goodsList, goodsList0, goodsList1, goodsList2, goodsList3, intbian);
            if (isfalse)
            {
                this.DialogResult = DialogResult.OK;
            }
            else {
                MessageBox.Show("保存失败");
            }
        }

        private void radioButton7_Click(object sender, EventArgs e)
        {
            if (this.radioButton7.Checked) {
                this.comboBox1.Visible = true;
            }
        }

        private void radioButton6_Click(object sender, EventArgs e)
        {
            if (this.radioButton6.Checked)
            {
                this.comboBox1.Visible = false;
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mzid = this.comboBox1.SelectedValue.ToString();
        }

        private void checkBox33_Click(object sender, EventArgs e)
        {
            if (this.checkBox33.Checked)
            {
                this.checkBox26.Checked = false;
            }
        }

        private void checkBox26_Click(object sender, EventArgs e)
        {
            if (this.checkBox26.Checked) {
                this.checkBox27.Checked = false;
                this.checkBox28.Checked = false;
                this.checkBox29.Checked = false;
                this.checkBox30.Checked = false;
                this.checkBox31.Checked = false;
                this.checkBox32.Checked = false;
                this.checkBox33.Checked = false;
                this.textBox37.Text = "";
            }
        }

        private void checkBox27_Click(object sender, EventArgs e)
        {
            if (this.checkBox27.Checked)
            {
                this.checkBox26.Checked = false;
            }
        }

        private void checkBox28_Click(object sender, EventArgs e)
        {
            if (this.checkBox28.Checked)
            {
                this.checkBox26.Checked = false;
            }
        }

        private void checkBox29_Click(object sender, EventArgs e)
        {
            if (this.checkBox29.Checked)
            {
                this.checkBox26.Checked = false;
            }
        }

        private void checkBox30_Click(object sender, EventArgs e)
        {
            if (this.checkBox30.Checked)
            {
                this.checkBox26.Checked = false;
            }
        }

        private void checkBox31_Click(object sender, EventArgs e)
        {
            if (this.checkBox31.Checked)
            {
                this.checkBox26.Checked = false;
            }
        }

        private void checkBox32_Click(object sender, EventArgs e)
        {
            if (this.checkBox32.Checked)
            {
                this.checkBox26.Checked = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox8.Checked)
            {
                this.textBox38.Enabled = true;
            }
            else {
                this.textBox38.Enabled = false;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox13.Checked)
            {
                this.textBox39.Enabled = true;
            }
            else
            {
                this.textBox39.Enabled = false;
            }
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            //ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            //base.OnPaint(e);

            //Graphics g = e.Graphics;
            //g.DrawString("保存", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 5));
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            //ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(170, 171, 171), Color.FromArgb(170, 171, 171));
            //base.OnPaint(e);

            //Graphics g = e.Graphics;
            //g.DrawString("取消", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 5));
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            //ControlCircular.Draw(e.ClipRectangle, e.Graphics, 5, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            //base.OnPaint(e);

            //Graphics g = e.Graphics;
            //g.DrawString("+", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Bold), new SolidBrush(Color.White), new PointF(1, 1));

        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            //ControlCircular.Draw(e.ClipRectangle, e.Graphics, 5, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            //base.OnPaint(e);

            //Graphics g = e.Graphics;
            //g.DrawString("-", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Bold), new SolidBrush(Color.White), new PointF(2, 1));

        }

        private void aUPersonalBasicInfoo_Resize(object sender, EventArgs e)
        {
            //float newx = (this.Width) / xMy;
            //float newy = (this.Height) / yMy;
            //Common.setControls(newx, newy, this);
        }

        private void radioButton47_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton47.Checked == true)
            {
                textBox36.Enabled = true;
            }
            else
            {
                textBox36.Text = "";
                textBox36.Enabled = false;
            }
        }
    }
}
