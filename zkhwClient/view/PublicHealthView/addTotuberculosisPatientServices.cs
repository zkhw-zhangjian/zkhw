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
    public partial class addTotuberculosisPatientServices : Form
    {
        /// <summary>
        /// 状态(1:新增 0:修改)
        /// </summary>
        public int IS { get; set; }
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
        public bool show { get; set; } = true;
        public string mag { get; set; }
        /// <summary>
        /// 非首次随访添加
        /// </summary>
        /// <param name="names">姓名</param>
        /// <param name="aichive_nos">档案编号</param>
        /// <param name="id_numbers">身份证号</param>
        public addTotuberculosisPatientServices(int ps, string names, string aichive_nos, string id_numbers)
        {
            Names = names;
            aichive_no = aichive_nos;
            id_number = id_numbers;
            IS = ps;
            if (GetRecord())
            {
                show = false;
                mag = "没有添加第一次随访记录不可新增！";
                return;
            }
            this.Text = (IS == 1 ? "非首次随访添加" : "非首次随访修改");
            InitializeComponent();
            if (IS == 0)
            {
                if (GetUpdate())
                {
                    SetData();
                }
                else
                {
                    show = false;
                    mag = "没有修改数据！";
                    return;
                }
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        private int Insert()
        {
            if (GetCount() >= 4)
            {
                MessageBox.Show("已添加四次，无法再添加");
                return 0;
            }
            tuberculosis_follow_record info = GetData();
            info.name = Names;
            info.aichive_no = aichive_no;
            info.Cardcode = id_number;
            info.id_number = id_number;
            string issql = @"insert into tuberculosis_follow_record(id,name,aichive_no,Cardcode,id_number,visit_date,month_order,supervisor_type,visit_type,symptom,symptom_other,smoke_now,smoke_next,drink_now,drink_next,chemotherapy_plan,`usage`,drugs_type,miss,untoward_effect,untoward_effect_info,complication,complication_info,transfer_treatment_department,transfer_treatment_reason,twoweek_visit_result,handling_suggestion,next_visit_date,visit_doctor,stop_date,stop_reason,must_visit_num,actual_visit_num,must_medicine_num,actual_medicine_num,medicine_rate,estimate_doctor,create_user,create_name,create_time,create_org,create_org_name,upload_status) values(@id,@name,@aichive_no,@Cardcode,@id_number,@visit_date,@month_order,@supervisor_type,@visit_type,@symptom,@symptom_other,@smoke_now,@smoke_next,@drink_now,@drink_next,@chemotherapy_plan,@usage,@drugs_type,@miss,@untoward_effect,@untoward_effect_info,@complication,@complication_info,@transfer_treatment_department,@transfer_treatment_reason,@twoweek_visit_result,@handling_suggestion,@next_visit_date,@visit_doctor,@stop_date,@stop_reason,@must_visit_num,@actual_visit_num,@must_medicine_num,@actual_medicine_num,@medicine_rate,@estimate_doctor,@create_user,@create_name,@create_time,@create_org,@create_org_name,@upload_status)";
            MySqlParameter[] args = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@Cardcode", info.Cardcode),
                    new MySqlParameter("@id_number",info.id_number ),
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
                    new MySqlParameter("@create_org", info.create_org),
                    new MySqlParameter("@create_org_name", info.create_org_name),
                    new MySqlParameter("@create_time", info.create_time),
                    new MySqlParameter("@upload_status", info.upload_status),
};
            return DbHelperMySQL.ExecuteSql(issql, args);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        private int Update()
        {
            tuberculosis_follow_record info = GetData();
            info.name = Names;
            info.aichive_no = aichive_no;
            info.Cardcode = id_number;
            string issql = @"update tuberculosis_follow_record set visit_date=@visit_date,month_order=@month_order,supervisor_type=@supervisor_type,visit_type=@visit_type,symptom=@symptom,symptom_other=@symptom_other,smoke_now=@smoke_now,smoke_next=@smoke_next,drink_now=@drink_now,drink_next=@drink_next,chemotherapy_plan=@chemotherapy_plan,`usage`=@usage,drugs_type=@drugs_type,miss=@miss,untoward_effect=@untoward_effect,untoward_effect_info=@untoward_effect_info,complication=@complication,complication_info=@complication_info,transfer_treatment_department=@transfer_treatment_department,transfer_treatment_reason=@transfer_treatment_reason,twoweek_visit_result=@twoweek_visit_result,handling_suggestion=@handling_suggestion,next_visit_date=@next_visit_date,visit_doctor=@visit_doctor,stop_date=@stop_date,stop_reason=@stop_reason,must_visit_num=@must_visit_num,actual_visit_num=@actual_visit_num,must_medicine_num=@must_medicine_num,actual_medicine_num=@actual_medicine_num,medicine_rate=@medicine_rate,estimate_doctor=@estimate_doctor,update_user=@update_user,update_name=@update_name,update_time=@update_time where id=(select a.id from( select id from tuberculosis_follow_record where `name`=@name and aichive_no=@aichive_no and Cardcode=@Cardcode order by create_time desc LIMIT 1)a)";
            MySqlParameter[] args = new MySqlParameter[] {
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@Cardcode", info.Cardcode),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@month_order", info.month_order),
                    new MySqlParameter("@supervisor_type", info.supervisor_type),
                    new MySqlParameter("@visit_type", info.visit_type),
                    new MySqlParameter("@symptom",info.symptom),
                    new MySqlParameter("@symptom_other", info.symptom_other),
                    new MySqlParameter("@smoke_now",info.smoke_now),
                    new MySqlParameter("@smoke_next", info.smoke_next),
                    new MySqlParameter("@drink_now",info.drink_now),
                    new MySqlParameter("@drink_next", info.drink_next),
                    new MySqlParameter("@chemotherapy_plan", info.chemotherapy_plan),
                    new MySqlParameter("@usage",info.usage),
                    new MySqlParameter("@drugs_type", info.drugs_type),
                    new MySqlParameter("@miss",info.miss),
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
                    new MySqlParameter("@update_user", info.update_user),
                    new MySqlParameter("@update_name", info.update_name),
                    new MySqlParameter("@update_time", info.update_time),
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
            info.visit_date = 随访时间1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            info.month_order = string.IsNullOrWhiteSpace(治疗月序1.Text) ? 0 : Convert.ToInt32(治疗月序1.Text);
            foreach (Control item in 督导人员1.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.supervisor_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 随访方式1.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.visit_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            string symptom = string.Empty;
            foreach (Control item in 症状及体征1.Controls)
            {
                if (item is CheckBox)
                {
                    if (((CheckBox)item).Checked)
                    {
                        symptom += ((CheckBox)item).Tag.ToString() + ",";
                    }
                }
                else if (item is RichTextBox)
                {
                    info.symptom_other = ((RichTextBox)item).Text;
                }
            }
            info.symptom = symptom.TrimEnd(',');
            info.smoke_now = string.IsNullOrWhiteSpace(吸烟a1.Text) ? 0 : Convert.ToInt32(吸烟a1.Text);
            info.smoke_next = string.IsNullOrWhiteSpace(吸烟a2.Text) ? 0 : Convert.ToInt32(吸烟a2.Text);
            info.drink_now = string.IsNullOrWhiteSpace(饮酒a1.Text) ? 0 : Convert.ToInt32(饮酒a1.Text);
            info.drink_next = string.IsNullOrWhiteSpace(饮酒a2.Text) ? 0 : Convert.ToInt32(饮酒a2.Text);
            info.chemotherapy_plan = 化疗方案1.Text;
            foreach (Control item in 用法1.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.usage = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 药品剂型1.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.drugs_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            info.miss = Convert.ToInt32(string.IsNullOrWhiteSpace(漏服药次数1.Text) ? "0" : 漏服药次数1.Text);
            foreach (Control item in 药物不良反应1.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.untoward_effect = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    if (((TextBox)item).Name == "药物不良反应有1")
                    {
                        info.untoward_effect_info = ((TextBox)item).Text.ToString();
                    }

                }
            }
            info.transfer_treatment_department = 转诊科别1.Text;
            info.transfer_treatment_reason = 转诊原因1.Text;
            info.twoweek_visit_result = 转诊结果1.Text;
            info.handling_suggestion = 处理意见1.Text;
            info.next_visit_date = 下次随访时间1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            info.visit_doctor = 随访医生签名1.Text;
            string stop_reason = string.Empty;
            foreach (Control item in 停止治疗原因1.Controls)
            {
                if (item is CheckBox)
                {
                    if (((CheckBox)item).Checked)
                    {
                        stop_reason += ((CheckBox)item).Tag.ToString() + ",";
                    }
                }
            }
            info.stop_reason = stop_reason.TrimEnd(',');
            info.stop_date = 出现停止治疗时间1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            info.must_visit_num = string.IsNullOrWhiteSpace(应访视患者次数1.Text) ? 0 : Convert.ToInt32(应访视患者次数1.Text);
            info.actual_visit_num = string.IsNullOrWhiteSpace(实际访视次数1.Text) ? 0 : Convert.ToInt32(实际访视次数1.Text);
            info.must_medicine_num = string.IsNullOrWhiteSpace(应服药次数1.Text) ? 0 : Convert.ToInt32(应服药次数1.Text);
            info.actual_medicine_num = string.IsNullOrWhiteSpace(实际服药次数1.Text) ? 0 : Convert.ToInt32(实际服药次数1.Text);
            info.medicine_rate = 服药率1.Text;
            return info;
        }

        /// <summary>
        /// 界面赋值
        /// </summary>
        private void SetData()
        {
            string sql = $@"select * from tuberculosis_follow_record where name='{Names}' and aichive_no='{aichive_no}' and Cardcode='{id_number}' order by create_time desc LIMIT 1";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                List<tuberculosis_follow_record> ts = Result.ToDataList<tuberculosis_follow_record>(jb.Tables[0]);
                foreach (var dt in ts)
                {
                    随访时间1.Value = Convert.ToDateTime(dt.visit_date);
                    治疗月序1.Text = dt.month_order.ToString();
                    foreach (Control item in 督导人员1.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.supervisor_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 随访方式1.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.visit_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 症状及体征1.Controls)
                    {
                        if (item is CheckBox)
                        {
                            if (dt.symptom.IndexOf(",") >= 0)
                            {
                                string[] sys = dt.symptom.Split(',');
                                if (item is CheckBox)
                                {
                                    if (sys.Contains(((CheckBox)item).Tag.ToString()))
                                    {
                                        ((CheckBox)item).Checked = true;
                                    }
                                }
                            }
                            else
                            {
                                if (((CheckBox)item).Tag.ToString() == dt.symptom)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                        else if (item is RichTextBox)
                        {
                            ((RichTextBox)item).Text = dt.symptom_other;
                        }
                    }
                    吸烟a1.Text = dt.smoke_now.ToString();
                    吸烟a2.Text = dt.smoke_next.ToString();
                    饮酒a1.Text = dt.drink_now.ToString();
                    饮酒a2.Text = dt.drink_next.ToString();
                    化疗方案1.Text = dt.chemotherapy_plan;
                    foreach (Control item in 用法1.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.usage)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 药品剂型1.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.drugs_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    漏服药次数1.Text = dt.miss.ToString();
                    foreach (Control item in 药物不良反应1.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.untoward_effect)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            if (((TextBox)item).Name == "药物不良反应有1")
                            {
                                ((TextBox)item).Text = dt.untoward_effect_info;
                            }
                        }
                    }
                    转诊科别1.Text = dt.transfer_treatment_department;
                    转诊原因1.Text = dt.transfer_treatment_reason;
                    转诊结果1.Text = dt.twoweek_visit_result;
                    处理意见1.Text = dt.handling_suggestion;
                    下次随访时间1.Value = Convert.ToDateTime(dt.next_visit_date);
                    随访医生签名1.Text = dt.visit_doctor;
                    if (!string.IsNullOrWhiteSpace(dt.stop_reason))
                    {
                        foreach (Control item in 停止治疗原因1.Controls)
                        {
                            if (item is CheckBox)
                            {
                                if (dt.stop_reason.IndexOf(",") >= 0)
                                {
                                    string[] sys = dt.stop_reason.Split(',');
                                    if (item is CheckBox)
                                    {
                                        if (sys.Contains(((CheckBox)item).Tag.ToString()))
                                        {
                                            ((CheckBox)item).Checked = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (((CheckBox)item).Tag.ToString() == dt.stop_reason)
                                    {
                                        ((CheckBox)item).Checked = true;
                                    }
                                }
                            }
                        }
                    }

                    出现停止治疗时间1.Value = Convert.ToDateTime(dt.stop_date);
                    应访视患者次数1.Text = dt.must_visit_num.ToString();
                    实际访视次数1.Text = dt.actual_visit_num.ToString();
                    应服药次数1.Text = dt.must_medicine_num.ToString();
                    实际服药次数1.Text = dt.actual_medicine_num.ToString();
                    服药率1.Text = dt.medicine_rate;
                }
            }
        }
        /// <summary>
        /// 判断是否有第一次随访记录
        /// </summary>
        /// <returns></returns>
        private bool GetRecord()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from tuberculosis_info where name='{Names}' and archive_no='{aichive_no}' and Cardcode='{id_number}'");
            if (data != null && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 判断是否有第一次随访记录
        /// </summary>
        /// <returns></returns>
        private int GetCount()
        {
            DataSet data = DbHelperMySQL.Query($@"select COUNT(id) co from tuberculosis_follow_record where name='{Names}' and aichive_no='{aichive_no}' and Cardcode='{id_number}'");
            if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(data.Tables[0].Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 判断是否有修改数据
        /// </summary>
        /// <returns></returns>
        private bool GetUpdate()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from tuberculosis_follow_record where name='{Names}' and aichive_no='{aichive_no}' and Cardcode='{id_number}'");
            if (data != null && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void 确定_Click(object sender, EventArgs e)
        {
            if ((IS == 1 ? Insert() : Update()) > 0)
            {
                MessageBox.Show("成功！");
            }
            else
            {
                MessageBox.Show("失败！");
            }
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            this.Close();
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

        public string id_number { get; set; }
        /// <summary>
        /// 访问日期
        /// </summary>
        public string visit_date { get; set; }
        /// <summary>
        /// 治疗月序
        /// </summary>
        public int? month_order { get; set; }
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
        public string chemotherapy_plan { get; set; }
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
        public int miss { get; set; }
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
        public int? must_visit_num { get; set; }
        /// <summary>
        /// 实际随访次数
        /// </summary>
        public int? actual_visit_num { get; set; }
        /// <summary>
        /// 应服药次数
        /// </summary>
        public int? must_medicine_num { get; set; }
        /// <summary>
        /// 实际服药次数
        /// </summary>
        public int? actual_medicine_num { get; set; }
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
        public string create_user { get; set; } = frmLogin.userCode;
        /// <summary>
        /// 创建用户名
        /// </summary>
        public string create_name { get; set; } = frmLogin.name;
        /// <summary>
        /// 创建组织
        /// </summary>
        public string create_org { get; set; } = frmLogin.organCode;
        /// <summary>
        /// 创建组织名
        /// </summary>
        public string create_org_name { get; set; } =frmLogin.organName;
        /// <summary>
        /// 创建时间
        /// </summary>
        public string create_time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 修改用户
        /// </summary>
        public string update_user { get; set; } = frmLogin.userCode;
        /// <summary>
        /// 修改用户名
        /// </summary>
        public string update_name { get; set; } = frmLogin.name;
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
