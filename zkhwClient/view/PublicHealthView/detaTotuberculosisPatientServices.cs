using MySql.Data.MySqlClient;
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
    public partial class detaTotuberculosisPatientServices : Form
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        /// 档案编号
        /// </summary>
        public string aichive_no { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string id_number { get; set; }

        /// <summary>
        /// 非首次随访添加
        /// </summary>
        /// <param name="names">姓名</param>
        /// <param name="aichive_nos">档案编号</param>
        /// <param name="id_numbers">身份证号</param>
        public detaTotuberculosisPatientServices(string names, string aichive_nos, string id_numbers)
        {
            Names = names;
            aichive_no = aichive_nos;
            id_number = id_numbers;
            if (GetRecord())
            {
                MessageBox.Show("没有添加第一次随访记录不可新增！");
                return;
            }
            InitializeComponent();
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 确定_Click(object sender, EventArgs e)
        {
            if (Insert()>0)
            {
                MessageBox.Show("成功！");
            }
            else
            {
                MessageBox.Show("失败！");
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        private int Insert()
        {
            tuberculosis_follow_record info = GetData();
            info.name = Names;
            info.aichive_no = aichive_no;
            info.Cardcode = id_number;
            string issql = @"insert into tuberculosis_follow_record(id,name,aichive_no,Cardcode,visit_date,month_order,supervisor_type,visit_type,symptom,symptom_other,smoke_now,smoke_next,drink_now,drink_next,chemotherapy_plan,usage,drugs_type,miss,untoward_effect,untoward_effect_info,complication,complication_info,transfer_treatment_department,transfer_treatment_reason,twoweek_visit_result,handling_suggestion,next_visit_date,visit_doctor,stop_date,stop_reason,must_visit_num,actual_visit_num,must_medicine_num,actual_medicine_num,medicine_rate,estimate_doctor,create_user,create_name,create_time,upload_status) values(@id,@name,@aichive_no,@Cardcode,@visit_date,@month_order,@supervisor_type,@visit_type,@symptom,@symptom_other,@smoke_now,@smoke_next,@drink_now,@drink_next,@chemotherapy_plan,@usage,@drugs_type,@miss,@untoward_effect,@untoward_effect_info,@complication,@complication_info,@transfer_treatment_department,@transfer_treatment_reason,@twoweek_visit_result,@handling_suggestion,@next_visit_date,@visit_doctor,@stop_date,@stop_reason,@must_visit_num,@actual_visit_num,@must_medicine_num,@actual_medicine_num,@medicine_rate,@estimate_doctor,@create_user,@create_name,@create_time,@upload_status)";
            MySqlParameter[] args = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@Cardcode", info.Cardcode),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@month_order", info.month_order),
                    new MySqlParameter("@supervisor_type",info.supervisor_type),
                    new MySqlParameter("@visit_type", info.visit_type),
                    new MySqlParameter("@symptom", info.symptom),
                    new MySqlParameter("@symptom_other", info.symptom_other),
                    new MySqlParameter("@smoke_now", info.smoke_now),
                    new MySqlParameter("@smoke_next", info.smoke_next),
                    new MySqlParameter("@drink_now", info.drink_now),
                    new MySqlParameter("@drink_next",info.drink_next),
                    new MySqlParameter("@chemotherapy_plan", info.chemotherapy_plan),
                    new MySqlParameter("@usage", info.usage),
                    new MySqlParameter("@drugs_type", info.drugs_type),
                    new MySqlParameter("@miss", info.miss),
                    new MySqlParameter("@untoward_effect", info.untoward_effect),
                    new MySqlParameter("@untoward_effect_info", info.untoward_effect_info),
                    new MySqlParameter("@complication", info.complication),
                    new MySqlParameter("@complication_info",info.complication_info),
                    new MySqlParameter("@transfer_treatment_department", info.transfer_treatment_department),
                    new MySqlParameter("@transfer_treatment_reason", info.transfer_treatment_reason),
                    new MySqlParameter("@twoweek_visit_result", info.twoweek_visit_result),
                    new MySqlParameter("@handling_suggestion", info.handling_suggestion),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
                    new MySqlParameter("@visit_doctor", info.visit_doctor),
                    new MySqlParameter("@stop_date", info.stop_date),
                    new MySqlParameter("@stop_reason", info.stop_reason),
                    new MySqlParameter("@must_visit_num", info.must_visit_num),
                    new MySqlParameter("@actual_visit_num", info.actual_visit_num),
                    new MySqlParameter("@must_medicine_num", info.must_medicine_num),
                    new MySqlParameter("@actual_medicine_num", info.actual_medicine_num),
                    new MySqlParameter("@medicine_rate", info.medicine_rate),
                    new MySqlParameter("@estimate_doctor", info.estimate_doctor),
                    new MySqlParameter("@create_user", info.create_user),
                    new MySqlParameter("@create_name", info.create_name),
                    new MySqlParameter("@create_time", info.create_time),
                    new MySqlParameter("@upload_status", info.upload_status),
};
            return DbHelperMySQL.ExecuteSql(issql, args);
        }

        /// <summary>
        /// 获取界面数据
        /// </summary>
        /// <returns></returns>
        private tuberculosis_follow_record GetData()
        {
            tuberculosis_follow_record info = new tuberculosis_follow_record();

            return info;
        }

        /// <summary>
        /// 判断是否有第一次随访记录
        /// </summary>
        /// <returns></returns>
        private bool GetRecord()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from tuberculosis_info where name='{Names}' and aichive_no='{aichive_no}' and Cardcode='{id_number}'");
            if (data != null && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }


}
