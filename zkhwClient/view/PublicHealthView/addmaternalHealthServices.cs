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
    public partial class addmaternalHealthServices : Form
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
        public addmaternalHealthServices(int ps, string names, string aichive_nos, string id_numbers)
        {
            InitializeComponent();
            this.Text = (IS == 1 ? "第1次产前检查添加" : "第1次产前检查修改");
            Names = names;
            aichive_no = aichive_nos;
            id_number = id_numbers;
            IS = ps;
            if (IS == 0)
            {
                if (GetUpdate())
                {
                    SetData();
                    return;
                }
                else
                {
                    show = false;
                    mag = "没有修改数据！";
                    return;
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
            gravida_info info = GetData();

            string sql = @"insert into gravida_info(id,name,aichive_no,id_number,visit_date,gestational_weeks,gravida_age,husband_name,husband_age,husband_phone,pregnant_num,natural_labour_num,cesarean_num,last_menstruation_date,due_date,past_illness,past_illness_other,family_history,family_history_other,habits_customs,habits_customs_other,isoperation,operation_name,natural_abortion_num,abactio_num,fetaldeath_num,stillbirth_num,neonatal_death_num,birth_defect_num,height,weight,bmi,blood_pressure_high,blood_pressure_low,heart,heart_other,lungs,lungs_other,vulva,vulva_other,vagina,vagina_other,cervix,cervix_other,corpus,corpus_other,accessories,accessories_other,hemoglobin,leukocyte,platelet,blood_other,urine_protein,glycosuria,urine_acetone_bodies,bld,urine_other,blood_sugar,blood_group,blood_rh,sgft,ast,albumin,total_bilirubin,conjugated_bilirubin,scr,blood_urea,vaginal_fluid,vaginal_fluid_other,vaginal_cleaning,hb,hbsab,hbeag,hbeab,hbcab,syphilis,hiv,b_ultrasonic,other,general_assessment,assessment_error,health_guidance,health_guidance_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,next_visit_date,visit_doctor,create_user,create_name,create_time,upload_status
) values(@id,@name,@aichive_no,@id_number,@visit_date,@gestational_weeks,@gravida_age,@husband_name,@husband_age,@husband_phone,@pregnant_num,@natural_labour_num,@cesarean_num,@last_menstruation_date,@due_date,@past_illness,@past_illness_other,@family_history,@family_history_other,@habits_customs,@habits_customs_other,@isoperation,@operation_name,@natural_abortion_num,@abactio_num,@fetaldeath_num,@stillbirth_num,@neonatal_death_num,@birth_defect_num,@height,@weight,@bmi,@blood_pressure_high,@blood_pressure_low,@heart,@heart_other,@lungs,@lungs_other,@vulva,@vulva_other,@vagina,@vagina_other,@cervix,@cervix_other,@corpus,@corpus_other,@accessories,@accessories_other,@hemoglobin,@leukocyte,@platelet,@blood_other,@urine_protein,@glycosuria,@urine_acetone_bodies,@bld,@urine_other,@blood_sugar,@blood_group,@blood_rh,@sgft,@ast,@albumin,@total_bilirubin,@conjugated_bilirubin,@scr,@blood_urea,@vaginal_fluid,@vaginal_fluid_other,@vaginal_cleaning,@hb,@hbsab,@hbeag,@hbeab,@hbcab,@syphilis,@hiv,@b_ultrasonic,@other,@general_assessment,@assessment_error,@health_guidance,@health_guidance_other,@transfer_treatment,@transfer_treatment_reason,@transfer_treatment_department,
@next_visit_date,@visit_doctor,@create_user,@create_name,@create_time,@upload_status);";
            MySqlParameter[] mySqls = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@gestational_weeks", info.gestational_weeks),
                    new MySqlParameter("@gravida_age", info.gravida_age),
                    new MySqlParameter("@husband_name", info.husband_name),
                    new MySqlParameter("@husband_age", info.husband_age),
                    new MySqlParameter("@husband_phone", info.husband_phone),
                    new MySqlParameter("@pregnant_num", info.pregnant_num),
                    new MySqlParameter("@natural_labour_num", info.natural_labour_num),
                    new MySqlParameter("@cesarean_num", info.cesarean_num),
                    new MySqlParameter("@last_menstruation_date", info.last_menstruation_date),
                    new MySqlParameter("@due_date", info.due_date),
                    new MySqlParameter("@past_illness", info.past_illness),
                    new MySqlParameter("@past_illness_other", info.past_illness_other),
                    new MySqlParameter("@family_history", info.family_history),
                    new MySqlParameter("@family_history_other", info.family_history_other),
                    new MySqlParameter("@habits_customs", info.habits_customs),
                    new MySqlParameter("@habits_customs_other", info.habits_customs_other),
                    new MySqlParameter("@isoperation", info.isoperation),
                    new MySqlParameter("@operation_name", info.operation_name),
                    new MySqlParameter("@natural_abortion_num", info.natural_abortion_num),
                    new MySqlParameter("@abactio_num", info.abactio_num),
                    new MySqlParameter("@fetaldeath_num", info.fetaldeath_num),
                    new MySqlParameter("@stillbirth_num", info.stillbirth_num),
                    new MySqlParameter("@neonatal_death_num", info.neonatal_death_num),
                    new MySqlParameter("@birth_defect_num", info.birth_defect_num),
                    new MySqlParameter("@height", info.height),
                    new MySqlParameter("@weight", info.weight),
                    new MySqlParameter("@bmi", info.bmi),
                    new MySqlParameter("@blood_pressure_high", info.blood_pressure_high),
                    new MySqlParameter("@blood_pressure_low", info.blood_pressure_low),
                    new MySqlParameter("@heart", info.heart),
                    new MySqlParameter("@heart_other", info.heart_other),
                    new MySqlParameter("@lungs", info.lungs),
                    new MySqlParameter("@lungs_other", info.lungs_other),
                    new MySqlParameter("@vulva", info.vulva),
                    new MySqlParameter("@vulva_other", info.vulva_other),
                    new MySqlParameter("@vagina", info.vagina),
                    new MySqlParameter("@vagina_other", info.vagina_other),
                    new MySqlParameter("@cervix", info.cervix),
                    new MySqlParameter("@cervix_other", info.cervix_other),
                    new MySqlParameter("@corpus", info.corpus),
                    new MySqlParameter("@corpus_other", info.corpus_other),
                    new MySqlParameter("@accessories", info.accessories),
                    new MySqlParameter("@accessories_other", info.accessories_other),
                    new MySqlParameter("@hemoglobin", info.hemoglobin),
                    new MySqlParameter("@leukocyte", info.leukocyte),
                    new MySqlParameter("@platelet", info.platelet),
                    new MySqlParameter("@blood_other", info.blood_other),
                    new MySqlParameter("@urine_protein", info.urine_protein),
                    new MySqlParameter("@glycosuria", info.glycosuria),
                    new MySqlParameter("@urine_acetone_bodies", info.urine_acetone_bodies),
                    new MySqlParameter("@bld", info.bld),
                    new MySqlParameter("@urine_other", info.urine_other),
                    new MySqlParameter("@blood_sugar", info.blood_sugar),
                    new MySqlParameter("@blood_group", info.blood_group),
                    new MySqlParameter("@blood_rh", info.blood_rh),
                    new MySqlParameter("@sgft", info.sgft),
                    new MySqlParameter("@ast", info.ast),
                    new MySqlParameter("@albumin", info.albumin),
                    new MySqlParameter("@total_bilirubin", info.total_bilirubin),
                    new MySqlParameter("@conjugated_bilirubin", info.conjugated_bilirubin),
                    new MySqlParameter("@scr", info.scr),
                    new MySqlParameter("@blood_urea", info.blood_urea),
                    new MySqlParameter("@vaginal_fluid", info.vaginal_fluid),
                    new MySqlParameter("@vaginal_fluid_other", info.vaginal_fluid_other),
                    new MySqlParameter("@vaginal_cleaning", info.vaginal_cleaning),
                    new MySqlParameter("@hb", info.hb),
                    new MySqlParameter("@hbsab", info.hbsab),
                    new MySqlParameter("@hbeag", info.hbeag),
                    new MySqlParameter("@hbeab", info.hbeab),
                    new MySqlParameter("@hbcab", info.hbcab),
                    new MySqlParameter("@syphilis", info.syphilis),
                    new MySqlParameter("@hiv", info.hiv),
                    new MySqlParameter("@b_ultrasonic", info.b_ultrasonic),
                    new MySqlParameter("@other", info.other),
                    new MySqlParameter("@general_assessment", info.general_assessment),
                    new MySqlParameter("@assessment_error", info.assessment_error),
                    new MySqlParameter("@health_guidance", info.health_guidance),
                    new MySqlParameter("@health_guidance_other", info.health_guidance_other),
                    new MySqlParameter("@transfer_treatment", info.transfer_treatment),
                    new MySqlParameter("@transfer_treatment_reason", info.transfer_treatment_reason),
                    new MySqlParameter("@transfer_treatment_department", info.transfer_treatment_department),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
                    new MySqlParameter("@visit_doctor", info.visit_doctor),
                    new MySqlParameter("@create_user", info.create_user),
                    new MySqlParameter("@create_name", info.create_name),
                    new MySqlParameter("@create_time", info.create_time),
                    new MySqlParameter("@upload_status", info.upload_status),
            };
            return DbHelperMySQL.ExecuteSql(sql, mySqls);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        private int Update()
        {
            gravida_info info = GetData();
            string sql = @"update gravida_info set visit_date=@visit_date,
gestational_weeks=@gestational_weeks,gravida_age=@gravida_age,husband_name=@husband_name,husband_age=@husband_age,husband_phone=@husband_phone,pregnant_num=@pregnant_num,
natural_labour_num=@natural_labour_num,cesarean_num=@cesarean_num,last_menstruation_date=@last_menstruation_date,due_date=@due_date,past_illness=@past_illness,
past_illness_other=@past_illness_other,family_history=@family_history,family_history_other=@family_history_other,habits_customs=@habits_customs,
habits_customs_other=@habits_customs_other,isoperation=@isoperation,operation_name=@operation_name,natural_abortion_num=@natural_abortion_num,
abactio_num=@abactio_num,fetaldeath_num=@fetaldeath_num,stillbirth_num=@stillbirth_num,neonatal_death_num=@neonatal_death_num,
birth_defect_num=@birth_defect_num,height=@height,weight=@weight,bmi=@bmi,blood_pressure_high=@blood_pressure_high,
blood_pressure_low=@blood_pressure_low,heart=@heart,heart_other=@heart_other,lungs=@lungs,lungs_other=@lungs_other,
vulva=@vulva,vulva_other=@vulva_other,vagina=@vagina,vagina_other=@vagina_other,cervix=@cervix,cervix_other=@cervix_other,
corpus=@corpus,corpus_other=@corpus_other,accessories=@accessories,accessories_other=@accessories_other,hemoglobin=@hemoglobin,
leukocyte=@leukocyte,platelet=@platelet,blood_other=@blood_other,urine_protein=@urine_protein,glycosuria=@glycosuria,
urine_acetone_bodies=@urine_acetone_bodies,bld=@bld,urine_other=@urine_other,blood_sugar=@blood_sugar,blood_group=@blood_group,
blood_rh=@blood_rh,sgft=@sgft,ast=@ast,albumin=@albumin,total_bilirubin=@total_bilirubin,conjugated_bilirubin=@conjugated_bilirubin,
scr=@scr,blood_urea=@blood_urea,vaginal_fluid=@vaginal_fluid,vaginal_fluid_other=@vaginal_fluid_other,vaginal_cleaning=@vaginal_cleaning,
hb=@hb,hbsab=@hbsab,hbeag=@hbeag,hbeab=@hbeab,hbcab=@hbcab,syphilis=@syphilis,hiv=@hiv,b_ultrasonic=@b_ultrasonic,other=@other,
general_assessment=@general_assessment,assessment_error=@assessment_error,health_guidance=@health_guidance,health_guidance_other=@health_guidance_other,
transfer_treatment=@transfer_treatment,transfer_treatment_reason=@transfer_treatment_reason,transfer_treatment_department=@transfer_treatment_department,
next_visit_date=@next_visit_date,visit_doctor=@visit_doctor,update_user=@update_user,update_name=@update_name,update_time=@update_time where name=@name and aichive_no=@aichive_no and id_number=@id_number;";
            MySqlParameter[] mySqls = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@gestational_weeks", info.gestational_weeks),
                    new MySqlParameter("@gravida_age", info.gravida_age),
                    new MySqlParameter("@husband_name", info.husband_name),
                    new MySqlParameter("@husband_age", info.husband_age),
                    new MySqlParameter("@husband_phone", info.husband_phone),
                    new MySqlParameter("@pregnant_num", info.pregnant_num),
                    new MySqlParameter("@natural_labour_num", info.natural_labour_num),
                    new MySqlParameter("@cesarean_num", info.cesarean_num),
                    new MySqlParameter("@last_menstruation_date", info.last_menstruation_date),
                    new MySqlParameter("@due_date", info.due_date),
                    new MySqlParameter("@past_illness", info.past_illness),
                    new MySqlParameter("@past_illness_other", info.past_illness_other),
                    new MySqlParameter("@family_history", info.family_history),
                    new MySqlParameter("@family_history_other", info.family_history_other),
                    new MySqlParameter("@habits_customs", info.habits_customs),
                    new MySqlParameter("@habits_customs_other", info.habits_customs_other),
                    new MySqlParameter("@isoperation", info.isoperation),
                    new MySqlParameter("@operation_name", info.operation_name),
                    new MySqlParameter("@natural_abortion_num", info.natural_abortion_num),
                    new MySqlParameter("@abactio_num", info.abactio_num),
                    new MySqlParameter("@fetaldeath_num", info.fetaldeath_num),
                    new MySqlParameter("@stillbirth_num", info.stillbirth_num),
                    new MySqlParameter("@neonatal_death_num", info.neonatal_death_num),
                    new MySqlParameter("@birth_defect_num", info.birth_defect_num),
                    new MySqlParameter("@height", info.height),
                    new MySqlParameter("@weight", info.weight),
                    new MySqlParameter("@bmi", info.bmi),
                    new MySqlParameter("@blood_pressure_high", info.blood_pressure_high),
                    new MySqlParameter("@blood_pressure_low", info.blood_pressure_low),
                    new MySqlParameter("@heart", info.heart),
                    new MySqlParameter("@heart_other", info.heart_other),
                    new MySqlParameter("@lungs", info.lungs),
                    new MySqlParameter("@lungs_other", info.lungs_other),
                    new MySqlParameter("@vulva", info.vulva),
                    new MySqlParameter("@vulva_other", info.vulva_other),
                    new MySqlParameter("@vagina", info.vagina),
                    new MySqlParameter("@vagina_other", info.vagina_other),
                    new MySqlParameter("@cervix", info.cervix),
                    new MySqlParameter("@cervix_other", info.cervix_other),
                    new MySqlParameter("@corpus", info.corpus),
                    new MySqlParameter("@corpus_other", info.corpus_other),
                    new MySqlParameter("@accessories", info.accessories),
                    new MySqlParameter("@accessories_other", info.accessories_other),
                    new MySqlParameter("@hemoglobin", info.hemoglobin),
                    new MySqlParameter("@leukocyte", info.leukocyte),
                    new MySqlParameter("@platelet", info.platelet),
                    new MySqlParameter("@blood_other", info.blood_other),
                    new MySqlParameter("@urine_protein", info.urine_protein),
                    new MySqlParameter("@glycosuria", info.glycosuria),
                    new MySqlParameter("@urine_acetone_bodies", info.urine_acetone_bodies),
                    new MySqlParameter("@bld", info.bld),
                    new MySqlParameter("@urine_other", info.urine_other),
                    new MySqlParameter("@blood_sugar", info.blood_sugar),
                    new MySqlParameter("@blood_group", info.blood_group),
                    new MySqlParameter("@blood_rh", info.blood_rh),
                    new MySqlParameter("@sgft", info.sgft),
                    new MySqlParameter("@ast", info.ast),
                    new MySqlParameter("@albumin", info.albumin),
                    new MySqlParameter("@total_bilirubin", info.total_bilirubin),
                    new MySqlParameter("@conjugated_bilirubin", info.conjugated_bilirubin),
                    new MySqlParameter("@scr", info.scr),
                    new MySqlParameter("@blood_urea", info.blood_urea),
                    new MySqlParameter("@vaginal_fluid", info.vaginal_fluid),
                    new MySqlParameter("@vaginal_fluid_other", info.vaginal_fluid_other),
                    new MySqlParameter("@vaginal_cleaning", info.vaginal_cleaning),
                    new MySqlParameter("@hb", info.hb),
                    new MySqlParameter("@hbsab", info.hbsab),
                    new MySqlParameter("@hbeag", info.hbeag),
                    new MySqlParameter("@hbeab", info.hbeab),
                    new MySqlParameter("@hbcab", info.hbcab),
                    new MySqlParameter("@syphilis", info.syphilis),
                    new MySqlParameter("@hiv", info.hiv),
                    new MySqlParameter("@b_ultrasonic", info.b_ultrasonic),
                    new MySqlParameter("@other", info.other),
                    new MySqlParameter("@general_assessment", info.general_assessment),
                    new MySqlParameter("@assessment_error", info.assessment_error),
                    new MySqlParameter("@health_guidance", info.health_guidance),
                    new MySqlParameter("@health_guidance_other", info.health_guidance_other),
                    new MySqlParameter("@transfer_treatment", info.transfer_treatment),
                    new MySqlParameter("@transfer_treatment_reason", info.transfer_treatment_reason),
                    new MySqlParameter("@transfer_treatment_department", info.transfer_treatment_department),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
                    new MySqlParameter("@visit_doctor", info.visit_doctor),
                    new MySqlParameter("@update_user", info.update_user),
                    new MySqlParameter("@update_name", info.update_name),
                    new MySqlParameter("@update_time", info.update_time),
            };
            return DbHelperMySQL.ExecuteSql(sql, mySqls);
        }
        /// <summary>
        /// 获取界面数据
        /// </summary>
        /// <returns></returns>
        private gravida_info GetData()
        {
            gravida_info info = new gravida_info();
            info.name = Names;
            info.aichive_no = aichive_no;
            info.id_number = id_number;
            info.visit_date = 填表日期.Value.ToString("yyyy-MM-dd HH:mm:ss");
            info.gestational_weeks = string.IsNullOrWhiteSpace(孕周.Text) ? 0 : Convert.ToInt32(孕周.Text);
            info.gravida_age = string.IsNullOrWhiteSpace(孕妇年龄.Text) ? 0 : Convert.ToInt32(孕妇年龄.Text); 
            info.husband_name = 丈夫姓名.Text;
            info.husband_age = string.IsNullOrWhiteSpace(丈夫年龄.Text) ? 0 : Convert.ToInt32(丈夫年龄.Text);
            info.husband_phone = 丈夫电话.Text;
            info.pregnant_num = string.IsNullOrWhiteSpace(孕次.Text) ? 0 : Convert.ToInt32(孕次.Text); 
            info.natural_labour_num = string.IsNullOrWhiteSpace(阴道分娩次数.Text) ? 0 : Convert.ToInt32(阴道分娩次数.Text); 
            info.cesarean_num = string.IsNullOrWhiteSpace(剖腹产次数.Text) ? 0 : Convert.ToInt32(剖腹产次数.Text); 
            info.last_menstruation_date = 末次月经.Text;
            info.due_date = 预产期.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string past_illness = string.Empty;
            foreach (Control item in 既往史.Controls)
            {
                if (item is CheckBox)
                {
                    if (((CheckBox)item).Checked)
                    {
                        past_illness += ((CheckBox)item).Tag.ToString() + ",";
                    }
                }
                else if (item is RichTextBox)
                {
                    info.past_illness_other = ((RichTextBox)item).Text;
                }
            }
            info.past_illness = past_illness.TrimEnd(',');
            string family_history = string.Empty;
            foreach (Control item in 家族史.Controls)
            {
                if (item is CheckBox)
                {
                    if (((CheckBox)item).Checked)
                    {
                        family_history += ((CheckBox)item).Tag.ToString() + ",";
                    }
                }
                else if (item is RichTextBox)
                {
                    info.family_history_other = ((RichTextBox)item).Text;
                }
            }
            info.family_history = family_history.TrimEnd(',');
            string habits_customs = string.Empty;
            foreach (Control item in 个人史.Controls)
            {
                if (item is CheckBox)
                {
                    if (((CheckBox)item).Checked)
                    {
                        habits_customs += ((CheckBox)item).Tag.ToString() + ",";
                    }
                }
                else if (item is RichTextBox)
                {
                    info.habits_customs_other = ((RichTextBox)item).Text;
                }
            }
            info.habits_customs = habits_customs.TrimEnd(',');
            foreach (Control item in 妇产科手术史.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.isoperation = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.operation_name = ((TextBox)item).Text;
                }
            }
            info.natural_abortion_num = string.IsNullOrWhiteSpace(自然流产次数.Text) ? 0 : Convert.ToInt32(自然流产次数.Text); 
            info.abactio_num = string.IsNullOrWhiteSpace(人工流产次数.Text) ? 0 : Convert.ToInt32(人工流产次数.Text); 
            info.fetaldeath_num = string.IsNullOrWhiteSpace(死胎次数.Text) ? 0 : Convert.ToInt32(死胎次数.Text);
            info.stillbirth_num = string.IsNullOrWhiteSpace(死产次数.Text) ? 0 : Convert.ToInt32(死产次数.Text); 
            info.neonatal_death_num = string.IsNullOrWhiteSpace(新生儿死亡次数.Text) ? 0 : Convert.ToInt32(新生儿死亡次数.Text); 
            info.birth_defect_num = string.IsNullOrWhiteSpace(出生缺陷儿次数.Text) ? 0 : Convert.ToInt32(出生缺陷儿次数.Text);
            info.height = 身高.Text;
            info.weight = 体重.Text;
            info.bmi = 体质指数.Text;
            info.blood_pressure_high = string.IsNullOrWhiteSpace(血压高.Text) ? 0 : Convert.ToInt32(血压高.Text);
            info.blood_pressure_low = string.IsNullOrWhiteSpace(血压低.Text) ? 0 : Convert.ToInt32(血压低.Text); 
            foreach (Control item in 心脏.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.heart = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.heart_other = ((TextBox)item).Text;
                }
            }
            foreach (Control item in 肺部.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.lungs = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.lungs_other = ((TextBox)item).Text;
                }
            }
            foreach (Control item in 外阴.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.vulva = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.vulva_other = ((TextBox)item).Text;
                }
            }
            foreach (Control item in 阴道.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.vagina = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.vagina_other = ((TextBox)item).Text;
                }
            }
            foreach (Control item in 宫颈.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.cervix = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.cervix_other = ((TextBox)item).Text;
                }
            }
            foreach (Control item in 子宫.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.corpus = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.corpus_other = ((TextBox)item).Text;
                }
            }
            foreach (Control item in 附件.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.accessories = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.accessories_other = ((TextBox)item).Text;
                }
            }
            info.hemoglobin = string.IsNullOrWhiteSpace(血红蛋白.Text) ? 0 : Convert.ToInt32(血红蛋白.Text); 
            info.leukocyte = 白细胞计数.Text;
            info.platelet = 血小板计数.Text;
            info.blood_other = 血液中其他.Text;
            info.urine_protein = 尿蛋白.Text;
            info.glycosuria = 尿糖.Text;
            info.urine_acetone_bodies = 尿酮体.Text;
            info.bld = 尿潜血.Text;
            info.urine_other = 尿常规其他.Text;
            info.blood_sugar = 血糖.Text;
            info.blood_group = 血型.Text;
            info.blood_rh = string.IsNullOrWhiteSpace(RH.Text) ? 0 : Convert.ToInt32(RH.Text);
            info.sgft = 血清谷丙转氨酶.Text;
            info.ast = 血清谷草转氨酶.Text;
            info.albumin = 白蛋白.Text;
            info.total_bilirubin = 总胆红素.Text;
            info.conjugated_bilirubin = 结合胆红素.Text;
            info.scr = 血清肌酐.Text;
            info.blood_urea = 血尿素.Text;
            string vaginal_fluid = string.Empty;
            foreach (Control item in 阴道分泌物.Controls)
            {
                if (item is CheckBox)
                {
                    if (((CheckBox)item).Checked)
                    {
                        vaginal_fluid += ((CheckBox)item).Tag.ToString() + ",";
                    }
                }
                else if (item is RichTextBox)
                {
                    info.vaginal_fluid_other = ((RichTextBox)item).Text;
                }
                else if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.vaginal_cleaning = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            info.vaginal_fluid = vaginal_fluid.TrimEnd(',');
            info.hb = 乙型肝炎表面抗原.Text;
            info.hbsab = 乙型肝炎表面抗体.Text;
            info.hbeag = 乙型肝炎e抗原.Text;
            info.hbeab = 乙型肝炎e抗体.Text;
            info.hbcab = 乙型肝炎核心抗体.Text;
            foreach (Control item in 梅毒血清学试验.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.syphilis = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            foreach (Control item in HIV抗体检测.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.hiv = ((RadioButton)item).Tag.ToString();
                    }
                }
            }
            info.b_ultrasonic = B超.Text;
            info.other = 其他检测.Text;
            foreach (Control item in 总体评估.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.general_assessment = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    info.assessment_error = ((TextBox)item).Text;
                }
            }
            string health_guidance = string.Empty;
            foreach (Control item in 保健指导.Controls)
            {
                if (item is CheckBox)
                {
                    if (((CheckBox)item).Checked)
                    {
                        health_guidance += ((CheckBox)item).Tag.ToString() + ",";
                    }
                }
                else if (item is RichTextBox)
                {
                    info.health_guidance_other = ((RichTextBox)item).Text;
                }
            }
            info.health_guidance = health_guidance.TrimEnd(',');
            foreach (Control item in 转诊.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        info.transfer_treatment = ((RadioButton)item).Tag.ToString();
                    }
                }
                else if (item is TextBox)
                {
                    if (((TextBox)item).Name == "转诊原因")
                    {
                        info.transfer_treatment_reason = ((TextBox)item).Text;
                    }
                    else if (((TextBox)item).Name == "转诊机构和科别")
                    {
                        info.transfer_treatment_department = ((TextBox)item).Text;
                    }
                }
            }
            info.next_visit_date = 下次访问日期.Value.ToString("yyyy-MM-dd HH:mm:ss");
            info.visit_doctor = 随访医生签名.Text;
            return info;
        }
        /// <summary>
        /// 界面赋值
        /// </summary>
        private void SetData()
        {
            string sql = $@"select * from gravida_info where name='{Names}' and aichive_no='{aichive_no}' and id_number='{id_number}'";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                List<gravida_info> ts = Result.ToDataList<gravida_info>(jb.Tables[0]);
                foreach (var dt in ts)
                {
                    填表日期.Value = Convert.ToDateTime(dt.visit_date);
                    孕周.Text = dt.gestational_weeks.ToString();
                    孕妇年龄.Text = dt.gravida_age.ToString();
                    丈夫姓名.Text = dt.husband_name;
                    丈夫年龄.Text = dt.husband_age.ToString();
                    丈夫电话.Text = dt.husband_phone;
                    孕次.Text = dt.pregnant_num.ToString();
                    阴道分娩次数.Text = dt.natural_labour_num.ToString();
                    剖腹产次数.Text = dt.cesarean_num.ToString();
                    末次月经.Text = dt.last_menstruation_date;
                    预产期.Value = Convert.ToDateTime(dt.due_date);
                    foreach (Control item in 既往史.Controls)
                    {
                        if (item is CheckBox)
                        {
                            if (dt.past_illness.IndexOf(",") >= 0)
                            {
                                string[] sys = dt.past_illness.Split(',');
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
                                if (((CheckBox)item).Tag.ToString() == dt.past_illness)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                        else if (item is RichTextBox)
                        {
                            ((RichTextBox)item).Text = dt.past_illness_other;
                        }
                    }
                    foreach (Control item in 家族史.Controls)
                    {
                        if (item is CheckBox)
                        {
                            if (dt.family_history.IndexOf(",") >= 0)
                            {
                                string[] sys = dt.family_history.Split(',');
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
                                if (((CheckBox)item).Tag.ToString() == dt.family_history)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                        else if (item is RichTextBox)
                        {
                            ((RichTextBox)item).Text = dt.family_history_other;
                        }
                    }
                    foreach (Control item in 个人史.Controls)
                    {
                        if (item is CheckBox)
                        {
                            if (dt.habits_customs.IndexOf(",") >= 0)
                            {
                                string[] sys = dt.habits_customs.Split(',');
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
                                if (((CheckBox)item).Tag.ToString() == dt.habits_customs)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                        else if (item is RichTextBox)
                        {
                            ((RichTextBox)item).Text = dt.habits_customs_other;
                        }
                    }
                    foreach (Control item in 妇产科手术史.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.isoperation)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.operation_name;
                        }
                    }
                    自然流产次数.Text = dt.natural_abortion_num.ToString();
                    人工流产次数.Text = dt.abactio_num.ToString();
                    死胎次数.Text = dt.fetaldeath_num.ToString();
                    死产次数.Text = dt.stillbirth_num.ToString();
                    新生儿死亡次数.Text = dt.neonatal_death_num.ToString();
                    出生缺陷儿次数.Text = dt.birth_defect_num.ToString();
                    身高.Text = dt.height;
                    体重.Text = dt.weight;
                    体质指数.Text = dt.bmi;
                    血压高.Text = dt.blood_pressure_high.ToString();
                    血压低.Text = dt.blood_pressure_low.ToString();
                    foreach (Control item in 心脏.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.heart)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.heart_other;
                        }
                    }
                    foreach (Control item in 肺部.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.lungs)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.lungs_other;
                        }
                    }
                    foreach (Control item in 外阴.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.vulva)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.vulva_other;
                        }
                    }
                    foreach (Control item in 阴道.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.vagina)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.vagina_other;
                        }
                    }
                    foreach (Control item in 宫颈.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.cervix)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.cervix_other;
                        }
                    }
                    foreach (Control item in 子宫.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.corpus)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.corpus_other;
                        }
                    }
                    foreach (Control item in 附件.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.accessories)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.accessories_other;
                        }
                    }
                    血红蛋白.Text = dt.hemoglobin.ToString();
                    白细胞计数.Text = dt.leukocyte;
                    血小板计数.Text = dt.platelet;
                    血液中其他.Text = dt.blood_other;
                    尿蛋白.Text = dt.urine_protein;
                    尿糖.Text = dt.glycosuria;
                    尿酮体.Text = dt.urine_acetone_bodies;
                    尿潜血.Text = dt.bld;
                    尿常规其他.Text = dt.urine_other;
                    血糖.Text = dt.blood_sugar;
                    血型.Text = dt.blood_group;
                    RH.Text = dt.blood_rh.ToString();
                    血清谷丙转氨酶.Text = dt.sgft;
                    血清谷草转氨酶.Text = dt.ast;
                    白蛋白.Text = dt.albumin;
                    总胆红素.Text = dt.total_bilirubin;
                    结合胆红素.Text = dt.conjugated_bilirubin;
                    血清肌酐.Text = dt.scr;
                    血尿素.Text = dt.blood_urea;
                    foreach (Control item in 阴道分泌物.Controls)
                    {
                        if (item is CheckBox)
                        {
                            if (dt.vaginal_fluid.IndexOf(",") >= 0)
                            {
                                string[] sys = dt.vaginal_fluid.Split(',');
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
                                if (((CheckBox)item).Tag.ToString() == dt.vaginal_fluid)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                        else if (item is RichTextBox)
                        {
                            ((RichTextBox)item).Text = dt.vaginal_fluid_other;
                        }
                        else if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.vaginal_cleaning)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    乙型肝炎表面抗原.Text = dt.hb;
                    乙型肝炎表面抗体.Text = dt.hbsab;
                    乙型肝炎e抗原.Text = dt.hbeag;
                    乙型肝炎e抗体.Text = dt.hbeab;
                    乙型肝炎核心抗体.Text = dt.hbcab;
                    foreach (Control item in 梅毒血清学试验.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.syphilis)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    foreach (Control item in HIV抗体检测.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.hiv)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    B超.Text = dt.b_ultrasonic;
                    其他检测.Text = dt.other;
                    foreach (Control item in 总体评估.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.general_assessment)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.assessment_error;
                        }
                    }
                    foreach (Control item in 保健指导.Controls)
                    {
                        if (item is CheckBox)
                        {
                            if (dt.health_guidance.IndexOf(",") >= 0)
                            {
                                string[] sys = dt.health_guidance.Split(',');
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
                                if (((CheckBox)item).Tag.ToString() == dt.health_guidance)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                        else if (item is RichTextBox)
                        {
                            ((RichTextBox)item).Text = dt.health_guidance_other;
                        }
                    }
                    foreach (Control item in 转诊.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.transfer_treatment)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            if (((TextBox)item).Name == "转诊原因")
                            {
                                ((TextBox)item).Text = dt.transfer_treatment_reason;
                            }
                            else if (((TextBox)item).Name == "转诊机构和科别")
                            {
                                ((TextBox)item).Text = dt.transfer_treatment_department;
                            }
                        }
                    }
                    下次访问日期.Value = Convert.ToDateTime(dt.next_visit_date);
                    随访医生签名.Text = dt.visit_doctor;
                }
            }
        }
        /// <summary>
        /// 判断是否有修改数据
        /// </summary>
        /// <returns></returns>
        private bool GetUpdate()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from gravida_info where name='{Names}' and aichive_no='{aichive_no}' and id_number='{id_number}'");
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
    /// 孕产妇第一次检查记录表
    /// </summary>
    public class gravida_info
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
        public string id_number { get; set; }
        /// <summary>
        /// 填表日期
        /// </summary>
        public string visit_date { get; set; }
        /// <summary>
        /// 孕周
        /// </summary>
        public int? gestational_weeks { get; set; }
        /// <summary>
        /// 孕妇年龄
        /// </summary>
        public int? gravida_age { get; set; }
        /// <summary>
        /// 丈夫姓名
        /// </summary>
        public string husband_name { get; set; }
        /// <summary>
        /// 丈夫年龄
        /// </summary>
        public int? husband_age { get; set; }
        /// <summary>
        /// 丈夫电话
        /// </summary>
        public string husband_phone { get; set; }
        /// <summary>
        /// 孕次
        /// </summary>
        public int? pregnant_num { get; set; }
        /// <summary>
        /// 阴道分娩次数
        /// </summary>
        public int? natural_labour_num { get; set; }
        /// <summary>
        /// 剖腹产次数
        /// </summary>
        public int? cesarean_num { get; set; }
        /// <summary>
        /// 末次月经日期
        /// </summary>
        public string last_menstruation_date { get; set; }
        /// <summary>
        /// 预产期
        /// </summary>
        public string due_date { get; set; }
        /// <summary>
        /// 既往史
        /// </summary>
        public string past_illness { get; set; }
        /// <summary>
        /// 既往史其他
        /// </summary>
        public string past_illness_other { get; set; }
        /// <summary>
        /// 家族史
        /// </summary>
        public string family_history { get; set; }
        /// <summary>
        /// 家族史其他
        /// </summary>
        public string family_history_other { get; set; }
        /// <summary>
        /// 个人史
        /// </summary>
        public string habits_customs { get; set; }
        /// <summary>
        /// 个人史其他
        /// </summary>
        public string habits_customs_other { get; set; }
        /// <summary>
        /// 妇产科手术史
        /// </summary>
        public string isoperation { get; set; }
        /// <summary>
        /// 手术名称
        /// </summary>
        public string operation_name { get; set; }
        /// <summary>
        /// 自然流产次数
        /// </summary>
        public int? natural_abortion_num { get; set; }
        /// <summary>
        /// 人工流产次数
        /// </summary>
        public int? abactio_num { get; set; }
        /// <summary>
        /// 死胎次数
        /// </summary>
        public int? fetaldeath_num { get; set; }
        /// <summary>
        /// 死产次数
        /// </summary>
        public int? stillbirth_num { get; set; }
        /// <summary>
        /// 新生儿死亡次数
        /// </summary>
        public int? neonatal_death_num { get; set; }
        /// <summary>
        /// 出生缺陷儿次数
        /// </summary>
        public int? birth_defect_num { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public string height { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public string weight { get; set; }
        /// <summary>
        /// 体质指数bmi
        /// </summary>
        public string bmi { get; set; }
        /// <summary>
        /// 血压高
        /// </summary>
        public int? blood_pressure_high { get; set; }
        /// <summary>
        /// 血压低
        /// </summary>
        public int? blood_pressure_low { get; set; }
        /// <summary>
        /// 心脏
        /// </summary>
        public string heart { get; set; }
        /// <summary>
        /// 心脏其他
        /// </summary>
        public string heart_other { get; set; }
        /// <summary>
        /// 肺部
        /// </summary>
        public string lungs { get; set; }
        /// <summary>
        /// 肺部其他
        /// </summary>
        public string lungs_other { get; set; }
        /// <summary>
        /// 外阴
        /// </summary>
        public string vulva { get; set; }
        /// <summary>
        /// 外阴其他
        /// </summary>
        public string vulva_other { get; set; }
        /// <summary>
        /// 阴道
        /// </summary>
        public string vagina { get; set; }
        /// <summary>
        /// 阴道其他
        /// </summary>
        public string vagina_other { get; set; }
        /// <summary>
        /// 宫颈
        /// </summary>
        public string cervix { get; set; }
        /// <summary>
        /// 宫颈其他
        /// </summary>
        public string cervix_other { get; set; }
        /// <summary>
        /// 子宫
        /// </summary>
        public string corpus { get; set; }
        /// <summary>
        /// 子宫其他
        /// </summary>
        public string corpus_other { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string accessories { get; set; }
        /// <summary>
        /// 附件其他
        /// </summary>
        public string accessories_other { get; set; }
        /// <summary>
        /// 血红蛋白
        /// </summary>
        public int? hemoglobin { get; set; }
        /// <summary>
        /// 白细胞计数
        /// </summary>
        public string leukocyte { get; set; }
        /// <summary>
        /// 血小板计数
        /// </summary>
        public string platelet { get; set; }
        /// <summary>
        /// 血液中其他
        /// </summary>
        public string blood_other { get; set; }
        /// <summary>
        /// 尿蛋白
        /// </summary>
        public string urine_protein { get; set; }
        /// <summary>
        /// 尿糖
        /// </summary>
        public string glycosuria { get; set; }
        /// <summary>
        /// 尿酮体
        /// </summary>
        public string urine_acetone_bodies { get; set; }
        /// <summary>
        /// 尿潜血
        /// </summary>
        public string bld { get; set; }
        /// <summary>
        /// 尿常规其他
        /// </summary>
        public string urine_other { get; set; }
        /// <summary>
        /// 血糖
        /// </summary>
        public string blood_sugar { get; set; }
        /// <summary>
        /// 血型
        /// </summary>
        public string blood_group { get; set; }
        /// <summary>
        /// RH
        /// </summary>
        public int? blood_rh { get; set; }
        /// <summary>
        /// 血清谷丙转氨酶
        /// </summary>
        public string sgft { get; set; }
        /// <summary>
        /// 血清谷草转氨酶
        /// </summary>
        public string ast { get; set; }
        /// <summary>
        /// 白蛋白
        /// </summary>
        public string albumin { get; set; }
        /// <summary>
        /// 总胆红素
        /// </summary>
        public string total_bilirubin { get; set; }
        /// <summary>
        /// 结合胆红素
        /// </summary>
        public string conjugated_bilirubin { get; set; }
        /// <summary>
        /// 血清肌酐
        /// </summary>
        public string scr { get; set; }
        /// <summary>
        /// 血尿素
        /// </summary>
        public string blood_urea { get; set; }
        /// <summary>
        /// 阴道分泌物
        /// </summary>
        public string vaginal_fluid { get; set; }
        /// <summary>
        /// 阴道分泌物其他
        /// </summary>
        public string vaginal_fluid_other { get; set; }
        /// <summary>
        /// 阴道清洁度
        /// </summary>
        public string vaginal_cleaning { get; set; }
        /// <summary>
        /// 乙型肝炎表面抗原
        /// </summary>
        public string hb { get; set; }
        /// <summary>
        /// 乙型肝炎表面抗体
        /// </summary>
        public string hbsab { get; set; }
        /// <summary>
        /// 乙型肝炎e抗原
        /// </summary>
        public string hbeag { get; set; }
        /// <summary>
        /// 乙型肝炎e抗体
        /// </summary>
        public string hbeab { get; set; }
        /// <summary>
        /// 乙型肝炎核心抗体
        /// </summary>
        public string hbcab { get; set; }
        /// <summary>
        /// 梅毒血清学实验
        /// </summary>
        public string syphilis { get; set; }
        /// <summary>
        /// HIV抗体检测
        /// </summary>
        public string hiv { get; set; }
        /// <summary>
        /// B超
        /// </summary>
        public string b_ultrasonic { get; set; }
        /// <summary>
        /// 其他检测
        /// </summary>
        public string other { get; set; }
        /// <summary>
        /// 总体评估
        /// </summary>
        public string general_assessment { get; set; }
        /// <summary>
        /// 评估异常
        /// </summary>
        public string assessment_error { get; set; }
        /// <summary>
        /// 保健指导
        /// </summary>
        public string health_guidance { get; set; }
        /// <summary>
        /// 保健指导其他
        /// </summary>
        public string health_guidance_other { get; set; }
        /// <summary>
        /// 转移治疗
        /// </summary>
        public string transfer_treatment { get; set; }
        /// <summary>
        /// 转诊原因
        /// </summary>
        public string transfer_treatment_reason { get; set; }
        /// <summary>
        /// 转诊机构和科别
        /// </summary>
        public string transfer_treatment_department { get; set; }
        /// <summary>
        /// 下次访问日期
        /// </summary>
        public string next_visit_date { get; set; }
        /// <summary>
        /// 访问医生
        /// </summary>
        public string visit_doctor { get; set; }
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
        public string create_org { get; set; }= frmLogin.organCode;
        /// <summary>
        /// 创建组织名
        /// </summary>
        public string create_org_name { get; set; }= frmLogin.organName;
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
