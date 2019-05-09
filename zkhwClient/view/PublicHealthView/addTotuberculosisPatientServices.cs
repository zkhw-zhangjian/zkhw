﻿using MySql.Data.MySqlClient;
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
    public partial class addTotuberculosisPatientServices : Form
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
        public addTotuberculosisPatientServices(string names, string aichive_nos, string id_numbers)
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
    /// <summary>
    /// 肺结核第一次访问记录表
    /// </summary>
    public class tuberculosis_follow_record
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; } = Result.GetNewId();
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 电子档案编码
        /// </summary>
        public string aichive_no { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string Cardcode { get; set; }
        /// <summary>
        /// 访问日期
        /// </summary>
        public string visit_date { get; set; }
        /// <summary>
        /// 治疗月序
        /// </summary>
        public string month_order { get; set; }
        /// <summary>
        /// 督导人员选择
        /// </summary>
        public string supervisor_type { get; set; }
        /// <summary>
        /// 访问方式
        /// </summary>
        public string visit_type { get; set; }
        /// <summary>
        /// 症状及体征
        /// </summary>
        public string symptom { get; set; }
        /// <summary>
        /// 症状其他
        /// </summary>
        public string symptom_other { get; set; }
        /// <summary>
        /// 现在每天吸烟量
        /// </summary>
        public int smoke_now { get; set; }
        /// <summary>
        /// 下次随访每天吸烟目标
        /// </summary>
        public int smoke_next { get; set; }
        /// <summary>
        /// 现在每天饮酒量
        /// </summary>
        public int drink_now { get; set; }
        /// <summary>
        /// 下次随访每天饮酒目标量
        /// </summary>
        public int drink_next { get; set; }
        /// <summary>
        /// 化疗方案
        /// </summary>
        public int chemotherapy_plan { get; set; }
        /// <summary>
        /// 用法
        /// </summary>
        public string usage { get; set; }
        /// <summary>
        /// 药品剂型
        /// </summary>
        public string drugs_type { get; set; }
        /// <summary>
        /// 漏服药次数
        /// </summary>
        public string miss { get; set; }
        /// <summary>
        /// 服药后的不良反应及处理
        /// </summary>
        public string untoward_effect { get; set; }
        /// <summary>
        /// 不良反应症状
        /// </summary>
        public string untoward_effect_info { get; set; }
        /// <summary>
        /// 并发症或合并症
        /// </summary>
        public string complication { get; set; }
        /// <summary>
        /// 并发症信息
        /// </summary>
        public string complication_info { get; set; }
        /// <summary>
        /// 转诊机构和科别
        /// </summary>
        public string transfer_treatment_department { get; set; }
        /// <summary>
        /// 转诊原因
        /// </summary>
        public string transfer_treatment_reason { get; set; }
        /// <summary>
        /// 转诊两周后随访结果
        /// </summary>
        public string twoweek_visit_result { get; set; }
        /// <summary>
        /// 处理意见
        /// </summary>
        public string handling_suggestion { get; set; }
        /// <summary>
        /// 下次随访日期
        /// </summary>
        public string next_visit_date { get; set; }
        /// <summary>
        /// 随访医生
        /// </summary>
        public string visit_doctor { get; set; }
        /// <summary>
        /// 停止治疗日期
        /// </summary>
        public string stop_date { get; set; }
        /// <summary>
        /// 停止治疗原因
        /// </summary>
        public string stop_reason { get; set; }
        /// <summary>
        /// 应该随访次数
        /// </summary>
        public string must_visit_num { get; set; }
        /// <summary>
        /// 实际随访次数
        /// </summary>
        public string actual_visit_num { get; set; }
        /// <summary>
        /// 应服药次数
        /// </summary>
        public string must_medicine_num { get; set; }
        /// <summary>
        /// 实际服药次数
        /// </summary>
        public string actual_medicine_num { get; set; }
        /// <summary>
        /// 服药率
        /// </summary>
        public string medicine_rate { get; set; }
        /// <summary>
        /// 评估医生
        /// </summary>
        public string estimate_doctor { get; set; } = basicInfoSettings.zeren_doctor;
        /// <summary>
        /// 创建用户
        /// </summary>
        public string create_user { get; set; } = basicInfoSettings.zeren_doctor;
        /// <summary>
        /// 创建用户名
        /// </summary>
        public string create_name { get; set; } = basicInfoSettings.zeren_doctor;
        /// <summary>
        /// 创建组织
        /// </summary>
        public string create_org { get; set; }
        /// <summary>
        /// 创建组织名
        /// </summary>
        public string create_org_name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string create_time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 修改用户
        /// </summary>
        public string update_user { get; set; } = basicInfoSettings.zeren_doctor;
        /// <summary>
        /// 修改用户名
        /// </summary>
        public string update_name { get; set; } = basicInfoSettings.zeren_doctor;
        /// <summary>
        /// 修改时间
        /// </summary>
        public string update_time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 上传状态
        /// </summary>
        public int upload_status { get; set; } = 0;
        /// <summary>
        /// 上传时间
        /// </summary>
        public string upload_time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 上传结果
        /// </summary>
        public string upload_result { get; set; }
    }
}
