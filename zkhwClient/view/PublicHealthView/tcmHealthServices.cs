﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class tcmHealthServices : Form
    {
        service.tcmHealthService tcmHealthService = new service.tcmHealthService();
        //高血压随访记录历史表  关联传参调查询的方法
        //public string name = "";
        //public string id_number = "";
        //public string aichive_no = "";
        public string pCa = "";
        public string time1 = null;
        public string time2 = null;
        public string cun = "";
        public tcmHealthServices()
        {
            InitializeComponent();
        }
        private void SetControlWodth()
        {
            if (Common.m_nWindwMetricsY <= 900)
            { 
                comboBox4.Width = 120;
                comboBox4.Left = comboBox3.Left + comboBox3.Width + 5;
                comboBox5.Width = 120;
                comboBox5.Left = comboBox4.Left + comboBox4.Width + 5;
            }
        }

        private void tcmHealthServices_Load(object sender, EventArgs e)
        {
            SetControlWodth();
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
             
            
            #region 区域数据绑定
            string sql1 = "select code as ID,name as Name from code_area_config where parent_code='-1';";
            DataSet datas = DbHelperMySQL.Query(sql1);
            if (datas != null && datas.Tables.Count > 0)
            {
                List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(datas.Tables[0]);
                Result.Bind(comboBox1, ts, "Name", "ID", "--请选择--");
            }
            cun = basicInfoSettings.xcuncode;
            querytcmHealthServices();
            #endregion
        }
        private void button5_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;//patientName Cardcode aichive_no
            if (pCa != "")
            {
                this.label2.Text = "";
            }
            else { this.label2.Text = "---姓名/身份证号/档案号---"; }
             
            
            querytcmHealthServices(); 
        }
        private void querytcmHealthServices()
        {
            if (this.comboBox5.Text == "" || this.comboBox5.Text == "--请选择--" || comboBox5.SelectedValue == null)
            {
                cun = "";
            }
            else
            {
                cun = this.comboBox5.SelectedValue.ToString();
            }
            this.dataGridView1.DataSource = null;
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间

            DateTime currentdt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime st = DateTime.Parse(time1);
            string sql = "";
            //if (currentdt != st)
            //{
            //    sql = "SELECT bb.name,bb.archive_no,bb.id_number,aa.test_date,aa.test_doctor,aa.id,(case aa.upload_status when '1' then '是' ELSE '否' END) cstatus FROM (select b.name, b.archive_no, b.id_number from resident_base_info b where 1=1 and age >= '65'";
            //    if (cun != null && !"".Equals(cun)) { sql += " AND b.village_code='" + cun + "'"; }
            //    if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
            //    sql += ") bb INNER JOIN(select id,aichive_no,test_date,test_doctor,upload_status from elderly_tcm_record where test_date >= '" + time1 + "' and test_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no";
            //}
            //else
            //{
                sql = @"SELECT bb.name,bb.archive_no,bb.id_number,aa.test_date,aa.test_doctor,bb.bar_code,(case aa.upload_status when '1' then '是' ELSE '否' END) cstatus,aa.exam_id ,aa.id
                     FROM (select b.name, b.archive_no, b.id_number,z.bar_code from resident_base_info b inner join zkhw_tj_jk z on b.id_number=z.id_number 
                    where 1=1 and (DATE_FORMAT( z.createtime,'%Y-%m-%d')>='" + time1 + "' and DATE_FORMAT( z.createtime,'%Y-%m-%d')<='"+ time2+ "' ) and b.age >= '65' ";
                if (cun != null && !"".Equals(cun)) { sql += " AND b.village_code='" + cun + "'"; }
                if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
                sql += ") bb LEFT JOIN(select e.id,e.aichive_no,e.test_date,e.test_doctor,e.upload_status,e.exam_id,p.bar_code from elderly_tcm_record e  inner join physical_examination_record p on e.exam_id = p.id where DATE_FORMAT( test_date,'%Y-%m-%d') >= '" + time1 + "' and DATE_FORMAT( test_date,'%Y-%m-%d') <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no and bb.bar_code=aa.bar_code ";
            //}
            DataSet dataSet = DbHelperMySQL.Query(sql);
            if (dataSet.Tables.Count < 1) { MessageBox.Show("未查询出数据，请重新查询!"); return; }
            DataTable dt = dataSet.Tables[0];
            this.dataGridView1.DataSource = dt;

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular);
            this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";  
            this.dataGridView1.Columns[1].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "问询日期";
            this.dataGridView1.Columns[4].HeaderCell.Value = "问询医生";
            this.dataGridView1.Columns[5].HeaderCell.Value = "条码号";
            //this.dataGridView1.Columns[5].Visible = false;
            this.dataGridView1.Columns[6].HeaderCell.Value = "是否上传";
            this.dataGridView1.Columns[7].Visible = false;
            this.dataGridView1.Columns[8].Visible = false;

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
        private bool IsHaveExamID(string _archiveno,string _idnumber,string _barcode)
        {
            bool ret = false;
            return ret;
        }
        //添加 修改 高血压随访记录历史表 调此方法
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            string code = dataGridView1["archive_no", row].Value.ToString();
            string idnum=dataGridView1["id_number", row].Value.ToString();
            string examid= dataGridView1["exam_id", row].Value.ToString();
            string barcode= dataGridView1["bar_code", row].Value.ToString();
             
            DataTable dtcode = null;
            if (examid=="")
            {
                dtcode = tcmHealthService.checkTcmHealthServicesByno1(code, idnum, barcode);
                if (examid == "")
                {
                    healthCheckupDao hd = new healthCheckupDao();  //获取exam_id
                    examid = hd.GetExaminationRecord(code, idnum, barcode);
                }
            }
            else
            {
                dtcode = tcmHealthService.checkTcmHealthServicesByExamID(examid);
            }
             
            if (dtcode.Rows.Count>0)
            {

                string _testdate = dtcode.Rows[0]["test_date"].ToString();
                string _strDisplay = string.Format("此患者已参加过中医体质服务了,日期为{0} !", _testdate);
                MessageBox.Show(_strDisplay); 
                return;
            }
            addtcmHealthServices addtcm = new addtcmHealthServices(1, dataGridView1["name", row].Value.ToString(), dataGridView1["archive_no", row].Value.ToString(), dataGridView1["id_number", row].Value.ToString(), examid);
            addtcm.bar_code = barcode;
            addtcm.exam_id = examid;
            addtcm.StartPosition = FormStartPosition.CenterScreen;
            if (addtcm.ShowDialog()==DialogResult.OK) {
                querytcmHealthServices();
            } 
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            string code = dataGridView1["archive_no", row].Value.ToString();
            string idnum = dataGridView1["id_number", row].Value.ToString();
            string examid = dataGridView1["exam_id", row].Value.ToString();
            string barcode = dataGridView1["bar_code", row].Value.ToString();
            DataTable dtcode = null;
            if (examid == "")
            {
                dtcode = tcmHealthService.checkTcmHealthServicesByno1(code, idnum, barcode);
            }
            else
            {
                dtcode = tcmHealthService.checkTcmHealthServicesByExamID(examid);
            }
            if (dtcode.Rows.Count > 0)
            {
                addtcmHealthServices addtcm = new addtcmHealthServices(0, dataGridView1["name", row].Value.ToString(), dataGridView1["archive_no", row].Value.ToString(), dataGridView1["id_number", row].Value.ToString(), examid);
                addtcm.bar_code = barcode;
                addtcm.exam_id = examid;
                addtcm.StartPosition = FormStartPosition.CenterScreen;
                addtcm.ShowDialog();
                querytcmHealthServices();
            }
            else
            {
                MessageBox.Show("没有修改数据！"); 
            } 
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
             // bool istrue = tcmHealthService.deletetcmHealthServices(id);
                int row = dataGridView1.CurrentRow.Index;
                string Name = dataGridView1["name", row].Value.ToString().Trim();
                string aichive_no = dataGridView1["archive_no", row].Value.ToString().Trim();
                string id_number = dataGridView1["id_number", row].Value.ToString().Trim();
                bool istrue = deletetcmHealthServices(Name, aichive_no, id_number);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label2.Visible = this.textBox1.Text.Length < 1;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="aichive_no">编号</param>
        /// <param name="id_number">身份证id</param>
        /// <returns></returns>
        public bool deletetcmHealthServices(string name, string aichive_no, string id_number)
        {
            int rt = 0;
            string sql = $"delete from elderly_tcm_record where name='{name}' and aichive_no='{aichive_no}' and id_number='{id_number}'";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }

        #region 下拉框绑定
        /// <summary>
        /// 绑定下拉选项
        /// </summary>
        /// <param name="combo">获取值</param>
        /// <param name="box">绑定值</param>
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
            else
            {

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
        #endregion

        #region 上传 2019-6-12

        private void GetResidentBaseInfo(string _idnumber,out string str1,out string str2)
        {
            str1 = "";
            str2 = "";
            string sql = string.Format("select * from resident_base_info where id_number='{0}'", _idnumber);
            DataSet info = DbHelperMySQL.Query(sql);
            DataTable data = info.Tables[0]; 
            if(data.Rows.Count>0)
            {
                str1 = string.Format("Delete From resident_info_temp where id='{0}'", _idnumber);
                str2 = $@"insert into resident_info_temp (id,archive_no,pb_archive,name,sex,birthday,id_number,card_pic,company,phone,link_name,link_phone,resident_type,register_address,residence_address,nation,blood_group,blood_rh,education,profession,marital_status,pay_type,pay_other,drug_allergy,allergy_other,exposure,disease_other,is_hypertension,is_diabetes,is_psychosis,is_tuberculosis,is_heredity,heredity_name,is_deformity,deformity_name,is_poor,kitchen,fuel,other_fuel,drink,other_drink,toilet,poultry,medical_code,photo_code,aichive_org,doctor_name,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,status,remark,create_user,create_name,create_time,create_org,create_org_name
                                ) values({Ifnull(data.Rows[0]["id"])},{Ifnull(data.Rows[0]["archive_no"])},{Ifnull(data.Rows[0]["pb_archive"])},{Ifnull(data.Rows[0]["name"])},{Ifnull(data.Rows[0]["sex"])},{Ifnull(data.Rows[0]["birthday"])},{Ifnull(_idnumber)},{Ifnull(data.Rows[0]["card_pic"])},{Ifnull(data.Rows[0]["company"])},{Ifnull(data.Rows[0]["phone"])},{Ifnull(data.Rows[0]["link_name"])},{Ifnull(data.Rows[0]["link_phone"])},{Ifnull(data.Rows[0]["resident_type"])},{Ifnull(data.Rows[0]["address"])},{Ifnull(data.Rows[0]["residence_address"])},{Ifnull(data.Rows[0]["nation"])},{Ifnull(data.Rows[0]["blood_group"])},{Ifnull(data.Rows[0]["blood_rh"])},{Ifnull(data.Rows[0]["education"])},{Ifnull(data.Rows[0]["profession"])},
                            {Ifnull(data.Rows[0]["marital_status"])},{Ifnull(data.Rows[0]["pay_type"])},{Ifnull(data.Rows[0]["pay_other"])},{Ifnull(data.Rows[0]["drug_allergy"])},{Ifnull(data.Rows[0]["allergy_other"])},{Ifnull(data.Rows[0]["exposure"])},{Ifnull(data.Rows[0]["disease_other"])},{Ifnull(data.Rows[0]["is_hypertension"])},{Ifnull(data.Rows[0]["is_diabetes"])},{Ifnull(data.Rows[0]["is_psychosis"])},{Ifnull(data.Rows[0]["is_tuberculosis"])},{Ifnull(data.Rows[0]["is_heredity"])},{Ifnull(data.Rows[0]["heredity_name"])},{Ifnull(data.Rows[0]["is_deformity"])},{Ifnull(data.Rows[0]["deformity_name"])},{Ifnull(data.Rows[0]["is_poor"])},{Ifnull(data.Rows[0]["kitchen"])},{Ifnull(data.Rows[0]["fuel"])},{Ifnull(data.Rows[0]["other_fuel"])},
                            {Ifnull(data.Rows[0]["drink"])},{Ifnull(data.Rows[0]["other_drink"])},{Ifnull(data.Rows[0]["toilet"])},{Ifnull(data.Rows[0]["poultry"])},{Ifnull(data.Rows[0]["medical_code"])},{Ifnull(data.Rows[0]["photo_code"])},{Ifnull(data.Rows[0]["aichive_org"])},{Ifnull(data.Rows[0]["doctor_name"])},{Ifnull(data.Rows[0]["province_code"])},{Ifnull(data.Rows[0]["province_name"])},{Ifnull(data.Rows[0]["city_code"])},{Ifnull(data.Rows[0]["city_name"])},{Ifnull(data.Rows[0]["county_code"])},{Ifnull(data.Rows[0]["county_name"])},{Ifnull(data.Rows[0]["towns_code"])},{Ifnull(data.Rows[0]["towns_name"])},
                            {Ifnull(data.Rows[0]["village_code"])},{Ifnull(data.Rows[0]["village_name"])},{Ifnull(data.Rows[0]["status"])},{Ifnull(data.Rows[0]["remark"])},{Ifnull(data.Rows[0]["create_user"])},{Ifnull(data.Rows[0]["create_name"])},{Ifnull(Convert.ToDateTime(data.Rows[0]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[0]["create_org"])},{Ifnull(data.Rows[0]["create_org_name"])});";
            }
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            string _archiveno = dataGridView1["archive_no", row].Value.ToString();
            string _idnumber = dataGridView1["id_number", row].Value.ToString();
            string _idd = dataGridView1["id", row].Value.ToString();
            string sql =  string.Format("select * from elderly_tcm_record Where id='{0}'", _idd);
            //if (_archiveno != "")
            //{
            //    sql = string.Format("select * from elderly_tcm_record Where aichive_no='{0}'", _archiveno);
            //}
            //else
            //{
            //    sql = string.Format("select * from elderly_tcm_record Where id_number='{0}'", _idnumber);
            //}
            DataSet estimate = DbHelperMySQL.Query(sql);
            if (estimate != null && estimate.Tables[0].Rows.Count > 0)
            {
                //判断是否上传
                DataTable data = estimate.Tables[0];
                string _uploadstatus = data.Rows[0]["upload_status"].ToString();
                if (_uploadstatus == "1")
                {
                    string _testdate = data.Rows[0]["test_date"].ToString();
                    string _strDisplay = string.Format("已经上传,日期为{0} !", _testdate);
                    MessageBox.Show(_strDisplay);
                    return;
                }

                string _answerresult= data.Rows[0]["answer_result"].ToString();
                if (_answerresult == "") { MessageBox.Show("还未参加体质服务不能上传！"); return; }
                List<string> sqllist = new List<string>();
                //这里上传对应的档案信息
                string str1 = "";
                string str2 = "";
                GetResidentBaseInfo(_idnumber, out str1, out str2);
                if(str1!="")
                {
                    sqllist.Add(str1);
                    sqllist.Add(str2);
                }
                string _id = data.Rows[0]["id"].ToString();
                sqllist.Add($@"delete from elderly_tcm_record where id={Ifnull(data.Rows[0]["id"])};");
                sqllist.Add($@"insert into elderly_tcm_record (id,name,archive_no,id_number,test_date,answer_result,qixuzhi_score,qixuzhi_result,yangxuzhi_score,yangxuzhi_result,yinxuzhi_score,yinxuzhi_result,tanshizhi_score,tanshizhi_result,shirezhi_score,shirezhi_result,xueyuzhi_score,xueyuzhi_result,qiyuzhi_score,qiyuzhi_result,tebingzhi_sorce,tebingzhi_result,pinghezhi_sorce,pinghezhi_result,test_doctor,tcm_guidance,create_user,create_name,create_org,create_org_name,create_time,upload_status,exam_id) 
                            values({Ifnull(data.Rows[0]["id"])},{Ifnull(data.Rows[0]["name"])},{Ifnull(data.Rows[0]["aichive_no"])},{Ifnull(data.Rows[0]["id_number"])},{Ifnull(data.Rows[0]["test_date"])},{Ifnull(data.Rows[0]["answer_result"])},{Ifnull(data.Rows[0]["qixuzhi_score"])},{Ifnull(data.Rows[0]["qixuzhi_result"])},{Ifnull(data.Rows[0]["yangxuzhi_score"])},
                            {Ifnull(data.Rows[0]["yangxuzhi_result"])},{Ifnull(data.Rows[0]["yinxuzhi_score"])},{Ifnull(data.Rows[0]["yinxuzhi_result"])},{Ifnull(data.Rows[0]["tanshizhi_score"])},{Ifnull(data.Rows[0]["tanshizhi_result"])},{Ifnull(data.Rows[0]["shirezhi_score"])},{Ifnull(data.Rows[0]["shirezhi_result"])},{Ifnull(data.Rows[0]["xueyuzhi_score"])},{Ifnull(data.Rows[0]["xueyuzhi_result"])},{Ifnull(data.Rows[0]["qiyuzhi_score"])},{Ifnull(data.Rows[0]["qiyuzhi_result"])},{Ifnull(data.Rows[0]["tebingzhi_sorce"])},{Ifnull(data.Rows[0]["tebingzhi_result"])},{Ifnull(data.Rows[0]["pinghezhi_sorce"])},{Ifnull(data.Rows[0]["pinghezhi_result"])},{Ifnull(data.Rows[0]["test_doctor"])},{Ifnull(data.Rows[0]["tcm_guidance"])},
                            {Ifnull(data.Rows[0]["create_user"])},{Ifnull(data.Rows[0]["create_name"])},{Ifnull(data.Rows[0]["create_org"])},{Ifnull(data.Rows[0]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[0]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[0]["upload_status"])},{Ifnull(data.Rows[0]["exam_id"])}
                            );");
                int ret = DbHelperMySQL.ExecuteSqlTranYpt(sqllist);
                if (ret > 0)
                {
                    MessageBox.Show("上传成功！");
                    //更新本地表中的状态为 1
                    sql = string.Format("update elderly_tcm_record set  upload_status=1 where  id='{0}'", _id);
                    ret = 0;
                    ret = DbHelperMySQL.ExecuteSql(sql); 
                    dataGridView1.SelectedRows[0].Cells[6].Value = "是";
                    dataGridView1.Refresh();
                }
                else
                {
                    MessageBox.Show("上传失败！");
                }
            }
            else
            {
                MessageBox.Show("无信息不能上传！");
            }
        }

        private string Ifnull(object dataRow)
        {
            if (Convert.IsDBNull(dataRow))
            {
                return "NULL";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(dataRow.ToString()))
                {
                    return "'" + dataRow.ToString() + "'";
                }
                else
                {
                    return "NULL";
                }
            }
        }

        #endregion

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < e.RowCount; i++)
            {
                dataGridView1.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }

            for (int i = e.RowIndex + e.RowCount; i < this.dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);
            Font font = new Font("微软雅黑", 12F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, "查询", font, bush);            
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);
            Font font = new Font("微软雅黑", 14F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, "添加", font, bush); 
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);
            Font font = new Font("微软雅黑", 14F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, "修改", font, bush); 
        }

        private void btnUpload_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);
            Font font = new Font("微软雅黑", 14F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, "数据上传", font, bush); 
        }
    }
}
