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
    public partial class addtuberculosisPatientServices : Form
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
        /// 首次随访添加
        /// </summary>
        /// <param name="names">姓名</param>
        /// <param name="aichive_nos">档案编号</param>
        /// <param name="id_numbers">身份证号</param>
        public addtuberculosisPatientServices(int ps, string names, string aichive_nos, string id_numbers)
        {
            Names = names;
            aichive_no = aichive_nos;
            id_number = id_numbers;
            IS = ps;
            if (GetRecord())
            {
                show = false;
                mag = "已有随访记录不可新增！";
            }
            this.Text = (IS == 1 ? "首次随访添加" : "首次随访修改");
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
                }
            }
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            this.Close();
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

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        private int Insert()
        {
            tuberculosis_info info = GetData();
            info.name = Names;
            info.aichive_no = aichive_no;
            info.Cardcode = id_number;
            info.id_number = id_number;
            info.create_user = frmLogin.userCode;
            info.create_name = frmLogin.name;
            info.create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            info.create_org = frmLogin.organCode;
            info.create_org_name = frmLogin.organName;
            string issql = @"insert into tuberculosis_info(id,name,archive_no,id_number,Cardcode,visit_date,visit_type,patient_type,sputum_bacterium_type,drug_fast_type,symptom,symptom_other,chemotherapy_plan,`usage`,drugs_type,supervisor_type,supervisor_other,single_room,ventilation,smoke_now,smoke_next,drink_now,drink_next,get_medicine_address,get_medicine_date,medicine_record,medicine_leave,treatment_course,erratically,untoward_effect,further_consultation,insist,habits_customs,intimate_contact,next_visit_date,estimate_doctor,create_user,create_name,create_time,create_org,create_org_name,upload_status) values(@id,@name,@archive_no,@id_number,@Cardcode,@visit_date,@visit_type,@patient_type,@sputum_bacterium_type,@drug_fast_type,@symptom,@symptom_other,@chemotherapy_plan,@usage,@drugs_type,@supervisor_type,@supervisor_other,@single_room,@ventilation,@smoke_now,@smoke_next,@drink_now,@drink_next,@get_medicine_address,@get_medicine_date,@medicine_record,@medicine_leave,@treatment_course,@erratically,@untoward_effect,@further_consultation,@insist,@habits_customs,@intimate_contact,@next_visit_date,@estimate_doctor,@create_user,@create_name,@create_time,@create_org,@create_org_name,@upload_status)";
            MySqlParameter[] args = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@archive_no", info.aichive_no),
                    new MySqlParameter("@Cardcode", info.Cardcode),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@visit_type", info.visit_type),
                    new MySqlParameter("@patient_type", info.patient_type),
                    new MySqlParameter("@sputum_bacterium_type", info.sputum_bacterium_type),
                    new MySqlParameter("@drug_fast_type", info.drug_fast_type),
                    new MySqlParameter("@symptom", info.symptom),
                    new MySqlParameter("@symptom_other", info.symptom_other),
                    new MySqlParameter("@chemotherapy_plan", info.chemotherapy_plan),
                    new MySqlParameter("@usage", info.usage),
                    new MySqlParameter("@drugs_type", info.drugs_type),
                    new MySqlParameter("@supervisor_type",info.supervisor_type),
                    new MySqlParameter("@supervisor_other", info.supervisor_other),
                    new MySqlParameter("@single_room", info.single_room),
                    new MySqlParameter("@ventilation",info.ventilation),
                    new MySqlParameter("@smoke_now", info.smoke_now),
                    new MySqlParameter("@smoke_next", info.smoke_next),
                    new MySqlParameter("@drink_now", info.drink_now),
                    new MySqlParameter("@drink_next",info.drink_next),
                    new MySqlParameter("@get_medicine_address", info.get_medicine_address),
                    new MySqlParameter("@get_medicine_date", info.get_medicine_date),
                    new MySqlParameter("@medicine_record", info.medicine_record),
                    new MySqlParameter("@medicine_leave", info.medicine_leave),
                    new MySqlParameter("@treatment_course", info.treatment_course),
                    new MySqlParameter("@erratically", info.erratically),
                    new MySqlParameter("@untoward_effect", info.untoward_effect),
                    new MySqlParameter("@further_consultation", info.further_consultation),
                    new MySqlParameter("@insist", info.insist),
                    new MySqlParameter("@habits_customs", info.habits_customs),
                    new MySqlParameter("@intimate_contact", info.intimate_contact),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
                    new MySqlParameter("@estimate_doctor", info.estimate_doctor),
                    new MySqlParameter("@create_user", info.create_user),
                    new MySqlParameter("@create_name", info.create_name),
                    new MySqlParameter("@create_time", info.create_time),
                    new MySqlParameter("@create_org", info.create_org),
                    new MySqlParameter("@create_org_name", info.create_org_name),
                    new MySqlParameter("@upload_status", "0"),
                    };
            return DbHelperMySQL.ExecuteSql(issql, args);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        private int Update()
        {
            tuberculosis_info info = GetData();
            info.name = Names;
            info.aichive_no = aichive_no;
            info.Cardcode = id_number;
            info.update_user= frmLogin.userCode;
            info.update_name=frmLogin.name;
            info.update_time= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string issql = @"update tuberculosis_info set visit_date=@visit_date,visit_type=@visit_type,patient_type=@patient_type,sputum_bacterium_type=@sputum_bacterium_type,drug_fast_type=@drug_fast_type,symptom=@symptom,symptom_other=@symptom_other,chemotherapy_plan=@chemotherapy_plan,`usage`=@usage,drugs_type=@drugs_type,supervisor_type=@supervisor_type,supervisor_other=@supervisor_other,single_room=@single_room,ventilation=@ventilation,smoke_now=@smoke_now,smoke_next=@smoke_next,drink_now=@drink_now,drink_next=@drink_next,get_medicine_address=@get_medicine_address,get_medicine_date=@get_medicine_date,medicine_record=@medicine_record,medicine_leave=@medicine_leave,treatment_course=@treatment_course,erratically=@erratically,untoward_effect=@untoward_effect,further_consultation=@further_consultation,insist=@insist,habits_customs=@habits_customs,intimate_contact=@intimate_contact,next_visit_date=@next_visit_date,estimate_doctor=@estimate_doctor,update_user=@update_user,update_name=@update_name,update_time=@update_time where name=@name and archive_no=@archive_no and Cardcode=@Cardcode";
            MySqlParameter[] args = new MySqlParameter[] {
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@archive_no", info.aichive_no),
                    new MySqlParameter("@Cardcode", info.Cardcode),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@visit_type", info.visit_type),
                    new MySqlParameter("@patient_type", info.patient_type),
                    new MySqlParameter("@sputum_bacterium_type",info.sputum_bacterium_type),
                    new MySqlParameter("@drug_fast_type", info.drug_fast_type),
                    new MySqlParameter("@symptom",info.symptom),
                    new MySqlParameter("@symptom_other", info.symptom_other),
                    new MySqlParameter("@chemotherapy_plan", info.chemotherapy_plan),
                    new MySqlParameter("@usage",info.usage),
                    new MySqlParameter("@drugs_type", info.drugs_type),
                    new MySqlParameter("@supervisor_type", info.supervisor_type),
                    new MySqlParameter("@supervisor_other", info.supervisor_other),
                    new MySqlParameter("@single_room",info.single_room),
                    new MySqlParameter("@ventilation", info.ventilation),
                    new MySqlParameter("@smoke_now",info.smoke_now),
                    new MySqlParameter("@smoke_next", info.smoke_next),
                    new MySqlParameter("@drink_now",info.drink_now),
                    new MySqlParameter("@drink_next", info.drink_next),
                    new MySqlParameter("@get_medicine_address", info.get_medicine_address),
                    new MySqlParameter("@get_medicine_date", info.get_medicine_date),
                    new MySqlParameter("@medicine_record", info.medicine_record),
                    new MySqlParameter("@medicine_leave", info.medicine_leave),
                    new MySqlParameter("@treatment_course", info.treatment_course),
                    new MySqlParameter("@erratically", info.erratically),
                    new MySqlParameter("@untoward_effect", info.untoward_effect),
                    new MySqlParameter("@further_consultation", info.further_consultation),
                    new MySqlParameter("@insist", info.insist),
                    new MySqlParameter("@habits_customs", info.habits_customs),
                    new MySqlParameter("@intimate_contact", info.intimate_contact),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
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
        private tuberculosis_info GetData()
        {
            tuberculosis_info info = new tuberculosis_info();
            info.visit_date = 随访时间.Value.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (Control item in 患者类型.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.patient_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 耐药情况.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.drug_fast_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 随访方式.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.visit_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 痰菌情况.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.sputum_bacterium_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            string symptom = string.Empty;
            foreach (Control item in 症状及体征.Controls)
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
            info.chemotherapy_plan = 化疗方案.Text;
            foreach (Control item in 用法.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.usage = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 药品剂型.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.drugs_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 督导人员选择.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.supervisor_type = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 单独的居室.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.single_room = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 通风情况.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.ventilation = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            info.smoke_now = string.IsNullOrWhiteSpace(吸烟1.Text) ? 0 : Convert.ToInt32(吸烟1.Text);
            info.smoke_next = string.IsNullOrWhiteSpace(吸烟2.Text) ? 0 : Convert.ToInt32(吸烟2.Text);
            info.drink_now = string.IsNullOrWhiteSpace(饮酒1.Text) ? 0 : Convert.ToInt32(饮酒1.Text);
            info.drink_next = string.IsNullOrWhiteSpace(饮酒2.Text) ? 0 : Convert.ToInt32(饮酒2.Text);
            info.get_medicine_address = 取药地点.Text;
            info.get_medicine_date = 取药时间.Value.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (Control item in 服药记录卡的填写.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.medicine_record = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 服药方法及药品存放.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.medicine_leave = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 肺结核治疗疗程.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.treatment_course = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 不规律服药危害.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.erratically = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 服药后不良反应及处理.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.untoward_effect = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 治疗期间复诊查痰.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.further_consultation = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 外出期间如何坚持服药.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.insist = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 生活习惯及注意事项.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.habits_customs = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in 密切接触者检查.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.intimate_contact = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            info.next_visit_date = 下次随访时间.Value.ToString("yyyy-MM-dd HH:mm:ss");
            return info;
        }
        /// <summary>
        /// 界面赋值
        /// </summary>
        private void SetData()
        {
            string sql = $@"select * from tuberculosis_info where name='{Names}' and archive_no='{aichive_no}' and Cardcode='{id_number}' order by create_time desc LIMIT 1";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                List<tuberculosis_info> ts = Result.ToDataList<tuberculosis_info>(jb.Tables[0]);
                foreach (var dt in ts)
                {
                    随访时间.Value = Convert.ToDateTime(dt.visit_date);
                    foreach (Control item in 患者类型.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.patient_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 耐药情况.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.drug_fast_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 随访方式.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.visit_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 痰菌情况.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.sputum_bacterium_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 症状及体征.Controls)
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
                    化疗方案.Text = dt.chemotherapy_plan;
                    foreach (Control item in 用法.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.usage)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 药品剂型.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.drugs_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 督导人员选择.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.supervisor_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 单独的居室.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.single_room)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 通风情况.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.ventilation)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    吸烟1.Text = dt.smoke_now.ToString();
                    吸烟2.Text = dt.smoke_next.ToString();
                    饮酒1.Text = dt.drink_now.ToString();
                    饮酒2.Text = dt.drink_next.ToString();
                    取药地点.Text = dt.get_medicine_address;
                    取药时间.Value = Convert.ToDateTime(dt.get_medicine_date);
                    foreach (Control item in 服药记录卡的填写.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.medicine_record)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 服药方法及药品存放.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.medicine_leave)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 肺结核治疗疗程.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.treatment_course)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 不规律服药危害.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.erratically)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 服药后不良反应及处理.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.untoward_effect)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 治疗期间复诊查痰.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.further_consultation)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 外出期间如何坚持服药.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.insist)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 生活习惯及注意事项.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.habits_customs)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in 密切接触者检查.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.intimate_contact)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    下次随访时间.Value = Convert.ToDateTime(dt.next_visit_date);
                }
            }
        }
        /// <summary>
        /// 判断是否有随访记录
        /// </summary>
        /// <returns></returns>
        private bool GetRecord()
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
        /// <summary>
        /// 判断是否有修改数据
        /// </summary>
        /// <returns></returns>
        private bool GetUpdate()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from tuberculosis_info where name='{Names}' and archive_no='{aichive_no}' and Cardcode='{id_number}'");
            if (data != null && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    /// <summary>
    /// 肺结核第一次访问记录表
    /// </summary>
    public class tuberculosis_info
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
        /// 访问类型
        /// </summary>
        public string visit_type { get; set; }
        /// <summary>
        /// 患者类型
        /// </summary>
        public string patient_type { get; set; }
        /// <summary>
        /// 痰菌类型
        /// </summary>
        public string sputum_bacterium_type { get; set; }
        /// <summary>
        /// 耐药类型
        /// </summary>
        public string drug_fast_type { get; set; }
        /// <summary>
        /// 症状及体征
        /// </summary>
        public string symptom { get; set; }
        /// <summary>
        /// 症状其他
        /// </summary>
        public string symptom_other { get; set; }
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
        /// 督导人员选择
        /// </summary>
        public string supervisor_type { get; set; }
        /// <summary>
        /// 督导其他
        /// </summary>
        public string supervisor_other { get; set; }
        /// <summary>
        /// 单独居室
        /// </summary>
        public string single_room { get; set; }
        /// <summary>
        /// 通风情况
        /// </summary>
        public string ventilation { get; set; }
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
        /// 取药地址
        /// </summary>
        public string get_medicine_address { get; set; }
        /// <summary>
        /// 取药日期
        /// </summary>
        public string get_medicine_date { get; set; }
        /// <summary>
        /// 服药记录卡的填写
        /// </summary>
        public string medicine_record { get; set; }
        /// <summary>
        /// 服药方法及药品存放
        /// </summary>
        public string medicine_leave { get; set; }
        /// <summary>
        /// 肺结核治疗疗程
        /// </summary>
        public string treatment_course { get; set; }
        /// <summary>
        /// 不规律服药危害
        /// </summary>
        public string erratically { get; set; }
        /// <summary>
        /// 服药后的不良反应及处理
        /// </summary>
        public string untoward_effect { get; set; }
        /// <summary>
        /// 治疗期间复诊查痰
        /// </summary>
        public string further_consultation { get; set; }
        /// <summary>
        /// 外出期间如何坚持服药
        /// </summary>
        public string insist { get; set; }
        /// <summary>
        /// 生活习惯及注意事项
        /// </summary>
        public string habits_customs { get; set; }
        /// <summary>
        /// 密切接触者检查
        /// </summary>
        public string intimate_contact { get; set; }
        /// <summary>
        /// 下次随访日期
        /// </summary>
        public string next_visit_date { get; set; }
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
        public string create_org_name { get; set; } = frmLogin.organName;
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
