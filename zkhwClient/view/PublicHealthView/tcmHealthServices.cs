using System;
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
        private void tcmHealthServices_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            this.label4.Text = "中医体质辨识记录表";
            this.label4.ForeColor = Color.SkyBlue;
            label4.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label4.Left = (this.panel1.Width - this.label4.Width) / 2;
            label4.BringToFront();
            
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
            cun = comboBox5.SelectedValue?.ToString();
            querytcmHealthServices(); 
        }
        private void querytcmHealthServices()
        {
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
                sql = @"SELECT bb.name,bb.archive_no,bb.id_number,aa.test_date,aa.test_doctor,aa.id,(case aa.upload_status when '1' then '是' ELSE '否' END) cstatus,aa.exam_id ,bb.bar_code
                     FROM (select b.name, b.archive_no, b.id_number,z.bar_code from resident_base_info b inner join zkhw_tj_jk z on b.id_number=z.id_number 
                    where 1=1 and z.createtime>='" + time1 + "' and b.age >= '65' ";
                if (cun != null && !"".Equals(cun)) { sql += " AND b.village_code='" + cun + "'"; }
                if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
                sql += ") bb LEFT JOIN(select id,aichive_no,test_date,test_doctor,upload_status,exam_id from elderly_tcm_record where test_date >= '" + time1 + "' and test_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no";
            //}
            DataSet dataSet = DbHelperMySQL.Query(sql);
            if (dataSet.Tables.Count < 1) { MessageBox.Show("未查询出数据，请重新查询!"); return; }
            DataTable dt = dataSet.Tables[0];
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[1].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "填表日期";
            this.dataGridView1.Columns[4].HeaderCell.Value = "签字医生";
            this.dataGridView1.Columns[5].Visible = false;
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
            
            //判断有没有exam_id
            if(examid=="")
            {
                healthCheckupDao hd = new healthCheckupDao();  //获取exam_id
                examid = hd.GetExaminationRecord(code, idnum, barcode); 
            }
            DataTable dtcode = null;
            if (examid=="")
            {
                dtcode = tcmHealthService.checkTcmHealthServicesByno1(code, idnum);
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
            addtcmHealthServices addtcm = new addtcmHealthServices(1, dataGridView1["name", row].Value.ToString(), dataGridView1["archive_no", row].Value.ToString(), dataGridView1["id_number", row].Value.ToString());
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
                dtcode = tcmHealthService.checkTcmHealthServicesByno1(code, idnum);
            }
            else
            {
                dtcode = tcmHealthService.checkTcmHealthServicesByExamID(examid);
            }
            if (dtcode.Rows.Count > 0)
            {
                addtcmHealthServices addtcm = new addtcmHealthServices(0, dataGridView1["name", row].Value.ToString(), dataGridView1["archive_no", row].Value.ToString(), dataGridView1["id_number", row].Value.ToString());
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
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            int row = dataGridView1.CurrentRow.Index;
            string _archiveno = dataGridView1["archive_no", row].Value.ToString();
            string _idnumber = dataGridView1["id_number", row].Value.ToString();
            string sql = "";
            if (_archiveno != "")
            {
                sql = string.Format("select * from elderly_tcm_record Where aichive_no='{0}'", _archiveno);
            }
            else
            {
                sql = string.Format("select * from elderly_tcm_record Where id_number='{0}'", _idnumber);
            }
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

                string _id = data.Rows[0]["id"].ToString();
                sqllist.Add($@"delete from elderly_tcm_record where id={Ifnull(data.Rows[0]["id"])};");
                sqllist.Add($@"insert into elderly_tcm_record (id,name,archive_no,id_number,test_date,answer_result,qixuzhi_score,qixuzhi_result,yangxuzhi_score,yangxuzhi_result,yinxuzhi_score,yinxuzhi_result,tanshizhi_score,tanshizhi_result,shirezhi_score,shirezhi_result,xueyuzhi_score,xueyuzhi_result,qiyuzhi_score,qiyuzhi_result,tebingzhi_sorce,tebingzhi_result,pinghezhi_sorce,pinghezhi_result,test_doctor,tcm_guidance,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
                            values({Ifnull(data.Rows[0]["id"])},{Ifnull(data.Rows[0]["name"])},{Ifnull(data.Rows[0]["aichive_no"])},{Ifnull(data.Rows[0]["id_number"])},{Ifnull(data.Rows[0]["test_date"])},{Ifnull(data.Rows[0]["answer_result"])},{Ifnull(data.Rows[0]["qixuzhi_score"])},{Ifnull(data.Rows[0]["qixuzhi_result"])},{Ifnull(data.Rows[0]["yangxuzhi_score"])},
                            {Ifnull(data.Rows[0]["yangxuzhi_result"])},{Ifnull(data.Rows[0]["yinxuzhi_score"])},{Ifnull(data.Rows[0]["yinxuzhi_result"])},{Ifnull(data.Rows[0]["tanshizhi_score"])},{Ifnull(data.Rows[0]["tanshizhi_result"])},{Ifnull(data.Rows[0]["shirezhi_score"])},{Ifnull(data.Rows[0]["shirezhi_result"])},{Ifnull(data.Rows[0]["xueyuzhi_score"])},{Ifnull(data.Rows[0]["xueyuzhi_result"])},{Ifnull(data.Rows[0]["qiyuzhi_score"])},{Ifnull(data.Rows[0]["qiyuzhi_result"])},{Ifnull(data.Rows[0]["tebingzhi_sorce"])},{Ifnull(data.Rows[0]["tebingzhi_result"])},{Ifnull(data.Rows[0]["pinghezhi_sorce"])},{Ifnull(data.Rows[0]["pinghezhi_result"])},{Ifnull(data.Rows[0]["test_doctor"])},{Ifnull(data.Rows[0]["tcm_guidance"])},
                            {Ifnull(data.Rows[0]["create_user"])},{Ifnull(data.Rows[0]["create_name"])},{Ifnull(data.Rows[0]["create_org"])},{Ifnull(data.Rows[0]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[0]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[0]["upload_status"])}
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
         
    }
}
