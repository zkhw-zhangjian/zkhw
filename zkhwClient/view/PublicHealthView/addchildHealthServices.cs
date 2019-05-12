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
    public partial class addchildHealthServices : Form
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
        public addchildHealthServices(int ps, string names, string aichive_nos, string id_numbers)
        {
            Names = names;
            aichive_no = aichive_nos;
            id_number = id_numbers;
            IS = ps;
            if (IS == 0)
            {
                if (GetUpdate())
                {
                    InitializeComponent();
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
            InitializeComponent();
            this.Text = (IS == 1 ? "产后访视添加" : "产后访视修改");
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
            List<neonatus_info> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();
            DBSql sqls = null;
            foreach (neonatus_info info in infolist)
            {
                sqls = new DBSql();
                sqls.sql = @"insert into neonatus_info(id,name,archive_no,id_number,sex,birthday,home_address,father_name,father_profession,father_phone,father_birthday,mother_name,mother_profession,mother_phone,mother_birthday,gestational_weeks,sicken_stasus,sicken_other,midwife_org,birth_situation,birth_other,asphyxia_neonatorum,asphyxia_time,deformity,deformity_other,hearing,disease,disease_other,birth_weight,weight,birth_height,feeding_patterns,milk_num,milk_intake,vomit,shit,defecation_num,temperature,heart_rate,breathing_rate,complexion,complexion_other,aurigo,aurigo_other,anterior_fontanelle_wide,anterior_fontanelle_high,anterior_fontanelle,anterior_fontanelle_other,eye,extremity_mobility,ear,neck_mass,nose,skin,skin_other,oral_cavity,anus,heart_lung,breast,abdominal_touch,spine,aedea,umbilical_cord,umbilical_cord_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,guidance,guidance_other,visit_date,next_visit_date,next_visit_address,visit_doctor,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,create_user,create_name,create_time,upload_status) values(@id,@name,@archive_no,@id_number,@sex,@birthday,@home_address,@father_name,@father_profession,@father_phone,@father_birthday,@mother_name,@mother_profession,@mother_phone,@mother_birthday,@gestational_weeks,@sicken_stasus,@sicken_other,@midwife_org,@birth_situation,@birth_other,@asphyxia_neonatorum,@asphyxia_time,@deformity,@deformity_other,@hearing,@disease,@disease_other,@birth_weight,@weight,@birth_height,@feeding_patterns,@milk_num,@milk_intake,@vomit,@shit,@defecation_num,@temperature,@heart_rate,@breathing_rate,@complexion,@complexion_other,@aurigo,@aurigo_other,@anterior_fontanelle_wide,@anterior_fontanelle_high,@anterior_fontanelle,@anterior_fontanelle_other,@eye,@extremity_mobility,@ear,@neck_mass,@nose,@skin,@skin_other,@oral_cavity,@anus,@heart_lung,@breast,@abdominal_touch,@spine,@aedea,@umbilical_cord,@umbilical_cord_other,@transfer_treatment,@transfer_treatment_reason,@transfer_treatment_department,@guidance,@guidance_other,@visit_date,@next_visit_date,@next_visit_address,@visit_doctor,@province_code,@province_name,@city_code,@city_name,@county_code,@county_name,@towns_code,@towns_name,@village_code,@village_name,@create_user,@create_name,@create_time,@upload_status);";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@archive_no", info.archive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@sex", info.sex),
                    new MySqlParameter("@birthday", info.birthday),
                    new MySqlParameter("@home_address", info.home_address),
                    new MySqlParameter("@temperature", info.temperature),
                    new MySqlParameter("@father_name", info.father_name),
                    new MySqlParameter("@father_profession", info.father_profession),
                    new MySqlParameter("@father_phone", info.father_phone),
                    new MySqlParameter("@father_birthday", info.father_birthday),
                    new MySqlParameter("@breast", info.breast),
                    new MySqlParameter("@mother_name", info.mother_name),
                    new MySqlParameter("@mother_profession", info.mother_profession),
                    new MySqlParameter("@mother_phone", info.mother_phone),
                    new MySqlParameter("@mother_birthday", info.mother_birthday),
                    new MySqlParameter("@gestational_weeks", info.gestational_weeks),
                    new MySqlParameter("@sicken_stasus", info.sicken_stasus),
                    new MySqlParameter("@sicken_other", info.sicken_other),
                    new MySqlParameter("@midwife_org", info.midwife_org),
                    new MySqlParameter("@birth_situation", info.birth_situation),
                    new MySqlParameter("@birth_other", info.birth_other),
                    new MySqlParameter("@guidance", info.guidance),
                    new MySqlParameter("@guidance_other", info.guidance_other),
                    new MySqlParameter("@transfer_treatment", info.transfer_treatment),
                    new MySqlParameter("@transfer_treatment_reason", info.transfer_treatment_reason),
                    new MySqlParameter("@transfer_treatment_department", info.transfer_treatment_department),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
                    new MySqlParameter("@visit_doctor", info.visit_doctor),
                    new MySqlParameter("@asphyxia_neonatorum", info.asphyxia_neonatorum),
                    new MySqlParameter("@asphyxia_time", info.asphyxia_time),
                    new MySqlParameter("@deformity", info.deformity),
                    new MySqlParameter("@deformity_other", info.deformity_other),
                    new MySqlParameter("@hearing", info.hearing),
                    new MySqlParameter("@disease", info.disease),
                    new MySqlParameter("@disease_other", info.disease_other),
                    new MySqlParameter("@birth_weight", info.birth_weight),
                    new MySqlParameter("@weight", info.weight),
                    new MySqlParameter("@birth_height", info.birth_height),
                    new MySqlParameter("@feeding_patterns", info.feeding_patterns),
                    new MySqlParameter("@milk_num", info.milk_num),
                    new MySqlParameter("@milk_intake", info.milk_intake),
                    new MySqlParameter("@vomit", info.vomit),
                    new MySqlParameter("@shit", info.shit),
                    new MySqlParameter("@defecation_num", info.defecation_num),
                    new MySqlParameter("@heart_rate", info.heart_rate),
                    new MySqlParameter("@breathing_rate", info.breathing_rate),
                    new MySqlParameter("@complexion", info.complexion),
                    new MySqlParameter("@complexion_other", info.complexion_other),
                    new MySqlParameter("@aurigo", info.aurigo),
                    new MySqlParameter("@aurigo_other", info.aurigo_other),
                    new MySqlParameter("@anterior_fontanelle_wide", info.anterior_fontanelle_wide),
                    new MySqlParameter("@anterior_fontanelle_high", info.anterior_fontanelle_high),
                    new MySqlParameter("@anterior_fontanelle", info.anterior_fontanelle),
                    new MySqlParameter("@anterior_fontanelle_other", info.anterior_fontanelle_other),
                    new MySqlParameter("@eye", info.eye),
                    new MySqlParameter("@extremity_mobility", info.extremity_mobility),
                    new MySqlParameter("@ear", info.ear),
                    new MySqlParameter("@neck_mass", info.neck_mass),
                    new MySqlParameter("@nose", info.nose),
                    new MySqlParameter("@skin", info.skin),
                    new MySqlParameter("@skin_other", info.skin_other),
                    new MySqlParameter("@oral_cavity", info.oral_cavity),
                    new MySqlParameter("@anus", info.anus),
                    new MySqlParameter("@heart_lung", info.heart_lung),
                    new MySqlParameter("@abdominal_touch", info.abdominal_touch),
                    new MySqlParameter("@spine", info.spine),
                    new MySqlParameter("@aedea", info.aedea),
                    new MySqlParameter("@umbilical_cord", info.umbilical_cord),
                    new MySqlParameter("@umbilical_cord_other", info.umbilical_cord_other),
                    new MySqlParameter("@next_visit_address", info.next_visit_address),
                    new MySqlParameter("@province_code", info.province_code),
                    new MySqlParameter("@province_name", info.province_name),
                    new MySqlParameter("@city_code", info.city_code),
                    new MySqlParameter("@city_name", info.city_name),
                    new MySqlParameter("@county_code", info.county_code),
                    new MySqlParameter("@county_name", info.county_name),
                    new MySqlParameter("@towns_code", info.towns_code),
                    new MySqlParameter("@towns_name", info.towns_name),
                    new MySqlParameter("@village_code", info.village_code),
                    new MySqlParameter("@village_name", info.village_name),
                    new MySqlParameter("@create_user", info.create_user),
                    new MySqlParameter("@create_name", info.create_name),
                    new MySqlParameter("@create_time", info.create_time),
                    new MySqlParameter("@upload_status", info.upload_status),
                    };
                hb.Add(sqls);
            }
            return DbHelperMySQL.ExecuteSqlTran(hb);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        private int Update()
        {
            List<neonatus_info> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();
            foreach (neonatus_info info in infolist)
            {
                DBSql sqls = new DBSql();
                sqls.sql = @"update neonatus_info set sex=@sex,birthday=@birthday,home_address=@home_address,father_name=@father_name,father_profession=@father_profession,father_phone=@father_phone,father_birthday=@father_birthday,mother_name=@mother_name,mother_profession=@mother_profession,mother_phone=@mother_phone,mother_birthday=@mother_birthday,gestational_weeks=@gestational_weeks,sicken_stasus=@sicken_stasus,sicken_other=@sicken_other,midwife_org=@midwife_org,birth_situation=@birth_situation,birth_other=@birth_other,asphyxia_neonatorum=@asphyxia_neonatorum,asphyxia_time=@asphyxia_time,deformity=@deformity,deformity_other=@deformity_other,hearing=@hearing,disease=@disease,disease_other=@disease_other,birth_weight=@birth_weight,weight=@weight,birth_height=@birth_height,feeding_patterns=@feeding_patterns,milk_num=@milk_num,milk_intake=@milk_intake,vomit=@vomit,shit=@shit,defecation_num=@defecation_num,temperature=@temperature,heart_rate=@heart_rate,breathing_rate=@breathing_rate,complexion=@complexion,complexion_other=@complexion_other,aurigo=@aurigo,aurigo_other=@aurigo_other,anterior_fontanelle_wide=@anterior_fontanelle_wide,anterior_fontanelle_high=@anterior_fontanelle_high,anterior_fontanelle=@anterior_fontanelle,anterior_fontanelle_other=@anterior_fontanelle_other,eye=@eye,extremity_mobility=@extremity_mobility,ear=@ear,neck_mass=@neck_mass,nose=@nose,skin=@skin,skin_other=@skin_other,oral_cavity=@oral_cavity,anus=@anus,heart_lung=@heart_lung,breast=@breast,abdominal_touch=@abdominal_touch,spine=@spine,aedea=@aedea,umbilical_cord=@umbilical_cord,umbilical_cord_other=@umbilical_cord_other,transfer_treatment=@transfer_treatment,transfer_treatment_reason=@transfer_treatment_reason,transfer_treatment_department=@transfer_treatment_department,guidance=@guidance,guidance_other=@guidance_other,visit_date=@visit_date,next_visit_date=@next_visit_date,next_visit_address=@next_visit_address,visit_doctor=@visit_doctor,province_code=@province_code,province_name=@province_name,city_code=@city_code,city_name=@city_name,county_code=@county_code,county_name=@county_name,towns_code=@towns_code,towns_name=@towns_name,village_code=@village_code,village_name=@village_name,update_user=@update_user,update_name=@update_name,update_time=@update_time where name=@name and aichive_no=@aichive_no and id_number=@id_number and order_num=@order_num;";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@archive_no", info.archive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@sex", info.sex),
                    new MySqlParameter("@birthday", info.birthday),
                    new MySqlParameter("@home_address", info.home_address),
                    new MySqlParameter("@temperature", info.temperature),
                    new MySqlParameter("@father_name", info.father_name),
                    new MySqlParameter("@father_profession", info.father_profession),
                    new MySqlParameter("@father_phone", info.father_phone),
                    new MySqlParameter("@father_birthday", info.father_birthday),
                    new MySqlParameter("@breast", info.breast),
                    new MySqlParameter("@mother_name", info.mother_name),
                    new MySqlParameter("@mother_profession", info.mother_profession),
                    new MySqlParameter("@mother_phone", info.mother_phone),
                    new MySqlParameter("@mother_birthday", info.mother_birthday),
                    new MySqlParameter("@gestational_weeks", info.gestational_weeks),
                    new MySqlParameter("@sicken_stasus", info.sicken_stasus),
                    new MySqlParameter("@sicken_other", info.sicken_other),
                    new MySqlParameter("@midwife_org", info.midwife_org),
                    new MySqlParameter("@birth_situation", info.birth_situation),
                    new MySqlParameter("@birth_other", info.birth_other),
                    new MySqlParameter("@guidance", info.guidance),
                    new MySqlParameter("@guidance_other", info.guidance_other),
                    new MySqlParameter("@transfer_treatment", info.transfer_treatment),
                    new MySqlParameter("@transfer_treatment_reason", info.transfer_treatment_reason),
                    new MySqlParameter("@transfer_treatment_department", info.transfer_treatment_department),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
                    new MySqlParameter("@visit_doctor", info.visit_doctor),
                    new MySqlParameter("@asphyxia_neonatorum", info.asphyxia_neonatorum),
                    new MySqlParameter("@asphyxia_time", info.asphyxia_time),
                    new MySqlParameter("@deformity", info.deformity),
                    new MySqlParameter("@deformity_other", info.deformity_other),
                    new MySqlParameter("@hearing", info.hearing),
                    new MySqlParameter("@disease", info.disease),
                    new MySqlParameter("@disease_other", info.disease_other),
                    new MySqlParameter("@birth_weight", info.birth_weight),
                    new MySqlParameter("@weight", info.weight),
                    new MySqlParameter("@birth_height", info.birth_height),
                    new MySqlParameter("@feeding_patterns", info.feeding_patterns),
                    new MySqlParameter("@milk_num", info.milk_num),
                    new MySqlParameter("@milk_intake", info.milk_intake),
                    new MySqlParameter("@vomit", info.vomit),
                    new MySqlParameter("@shit", info.shit),
                    new MySqlParameter("@defecation_num", info.defecation_num),
                    new MySqlParameter("@heart_rate", info.heart_rate),
                    new MySqlParameter("@breathing_rate", info.breathing_rate),
                    new MySqlParameter("@complexion", info.complexion),
                    new MySqlParameter("@complexion_other", info.complexion_other),
                    new MySqlParameter("@aurigo", info.aurigo),
                    new MySqlParameter("@aurigo_other", info.aurigo_other),
                    new MySqlParameter("@anterior_fontanelle_wide", info.anterior_fontanelle_wide),
                    new MySqlParameter("@anterior_fontanelle_high", info.anterior_fontanelle_high),
                    new MySqlParameter("@anterior_fontanelle", info.anterior_fontanelle),
                    new MySqlParameter("@anterior_fontanelle_other", info.anterior_fontanelle_other),
                    new MySqlParameter("@eye", info.eye),
                    new MySqlParameter("@extremity_mobility", info.extremity_mobility),
                    new MySqlParameter("@ear", info.ear),
                    new MySqlParameter("@neck_mass", info.neck_mass),
                    new MySqlParameter("@nose", info.nose),
                    new MySqlParameter("@skin", info.skin),
                    new MySqlParameter("@skin_other", info.skin_other),
                    new MySqlParameter("@oral_cavity", info.oral_cavity),
                    new MySqlParameter("@anus", info.anus),
                    new MySqlParameter("@heart_lung", info.heart_lung),
                    new MySqlParameter("@abdominal_touch", info.abdominal_touch),
                    new MySqlParameter("@spine", info.spine),
                    new MySqlParameter("@aedea", info.aedea),
                    new MySqlParameter("@umbilical_cord", info.umbilical_cord),
                    new MySqlParameter("@umbilical_cord_other", info.umbilical_cord_other),
                    new MySqlParameter("@next_visit_address", info.next_visit_address),
                    new MySqlParameter("@province_code", info.province_code),
                    new MySqlParameter("@province_name", info.province_name),
                    new MySqlParameter("@city_code", info.city_code),
                    new MySqlParameter("@city_name", info.city_name),
                    new MySqlParameter("@county_code", info.county_code),
                    new MySqlParameter("@county_name", info.county_name),
                    new MySqlParameter("@towns_code", info.towns_code),
                    new MySqlParameter("@towns_name", info.towns_name),
                    new MySqlParameter("@village_code", info.village_code),
                    new MySqlParameter("@village_name", info.village_name),
                    new MySqlParameter("@update_user", info.update_user),
                    new MySqlParameter("@update_name", info.update_name),
                    new MySqlParameter("@update_time", info.update_time),
                    };
                hb.Add(sqls);
            }
            return DbHelperMySQL.ExecuteSqlTran(hb);
        }
        /// <summary>
        /// 获取界面数据
        /// </summary>
        /// <returns></returns>
        private List<neonatus_info> GetData()
        {
            List<neonatus_info> infolist = new List<neonatus_info>();
            //if (产后7天.Checked)
            //{
            //    neonatus_info info = new neonatus_info();
            //    info.name = Names;
            //    info.aichive_no = aichive_no;
            //    info.id_number = id_number;
            //    info.order_num = "7";
            //    info.visit_date = 随访日期7.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.childbirth = 分娩日期7.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.discharge_date = 出院日期7.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.temperature = 体温7.Text.Trim();
            //    info.general_health_status = 一般健康情况7.Text.Trim();
            //    info.general_psychology_status = 一般心理状况7.Text.Trim();
            //    info.blood_pressure_high = Convert.ToInt32(血压高7.Text.Trim());
            //    info.blood_pressure_low = Convert.ToInt32(血压低7.Text.Trim());
            //    foreach (Control item in 乳房7.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.breast = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.breast_error = ((TextBox)item).Text;
            //        }
            //    }
            //    foreach (Control item in 恶露7.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.lyma = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.lyma_error = ((TextBox)item).Text;
            //        }
            //    }
            //    foreach (Control item in 子宫7.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.womb = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.womb_error = ((TextBox)item).Text;
            //        }
            //    }
            //    foreach (Control item in 伤口7.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.wound = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.wound_error = ((TextBox)item).Text;
            //        }
            //    }
            //    info.other = 其他7.Text.Trim();
            //    foreach (Control item in 分类7.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.condition = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.error_info = ((TextBox)item).Text;
            //        }
            //    }
            //    string guidance = string.Empty;
            //    foreach (Control item in 指导7.Controls)
            //    {
            //        if (item is CheckBox)
            //        {
            //            if (((CheckBox)item).Checked)
            //            {
            //                guidance += ((CheckBox)item).Tag.ToString() + ",";
            //            }
            //        }
            //        else if (item is RichTextBox)
            //        {
            //            info.guidance_other = ((RichTextBox)item).Text;
            //        }
            //    }
            //    info.guidance = guidance.TrimEnd(',');
            //    foreach (Control item in 转诊7.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.transfer_treatment = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            if (((TextBox)item).Name == "转诊原因7")
            //            {
            //                info.transfer_treatment_reason = ((TextBox)item).Text;
            //            }
            //            else if (((TextBox)item).Name == "转诊机构和科别7")
            //            {
            //                info.transfer_treatment_department = ((TextBox)item).Text;
            //            }
            //        }
            //    }
            //    info.next_visit_date = 下次随访日期7.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.visit_doctor = 随访医生签名7.Text.Trim();
            //    infolist.Add(info);
            //}
            //if (产后42天.Checked)
            //{
            //    gravida_after_record info = new gravida_after_record();
            //    info.name = Names;
            //    info.aichive_no = aichive_no;
            //    info.id_number = id_number;
            //    info.order_num = "42";
            //    info.visit_date = 随访日期42.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.childbirth = 分娩日期42.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.discharge_date = 出院日期42.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.temperature = 体温42.Text.Trim();
            //    info.general_health_status = 一般健康情况42.Text.Trim();
            //    info.general_psychology_status = 一般心理状况42.Text.Trim();
            //    info.blood_pressure_high = Convert.ToInt32(血压高42.Text.Trim());
            //    info.blood_pressure_low = Convert.ToInt32(血压低42.Text.Trim());
            //    foreach (Control item in 乳房42.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.breast = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.breast_error = ((TextBox)item).Text;
            //        }
            //    }
            //    foreach (Control item in 恶露42.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.lyma = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.lyma_error = ((TextBox)item).Text;
            //        }
            //    }
            //    foreach (Control item in 子宫42.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.womb = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.womb_error = ((TextBox)item).Text;
            //        }
            //    }
            //    foreach (Control item in 伤口42.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.wound = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.wound_error = ((TextBox)item).Text;
            //        }
            //    }
            //    info.other = 其他42.Text.Trim();
            //    foreach (Control item in 分类42.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.condition = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            info.error_info = ((TextBox)item).Text;
            //        }
            //    }
            //    string guidance = string.Empty;
            //    foreach (Control item in 指导42.Controls)
            //    {
            //        if (item is CheckBox)
            //        {
            //            if (((CheckBox)item).Checked)
            //            {
            //                guidance += ((CheckBox)item).Tag.ToString() + ",";
            //            }
            //        }
            //        else if (item is RichTextBox)
            //        {
            //            info.guidance_other = ((RichTextBox)item).Text;
            //        }
            //    }
            //    info.guidance = guidance.TrimEnd(',');
            //    foreach (Control item in 转诊42.Controls)
            //    {
            //        if (item is RadioButton)
            //        {
            //            if (((RadioButton)item).Checked)
            //            {
            //                info.transfer_treatment = ((RadioButton)item).Tag.ToString();
            //            }
            //        }
            //        else if (item is TextBox)
            //        {
            //            if (((TextBox)item).Name == "转诊原因42")
            //            {
            //                info.transfer_treatment_reason = ((TextBox)item).Text;
            //            }
            //            else if (((TextBox)item).Name == "转诊机构和科别42")
            //            {
            //                info.transfer_treatment_department = ((TextBox)item).Text;
            //            }
            //        }
            //    }
            //    info.next_visit_date = 下次随访日期42.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.visit_doctor = 随访医生签名42.Text.Trim();
            //    infolist.Add(info);
            //}

            return infolist;
        }
        /// <summary>
        /// 界面赋值
        /// </summary>
        private void SetData()
        {
            string sql = $@"select * from gravida_after_record where name='{Names}' and aichive_no='{aichive_no}' and id_number='{id_number}'";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                List<gravida_after_record> ts = Result.ToDataList<gravida_after_record>(jb.Tables[0]);
                foreach (var dt in ts)
                {
                    Control yl = this.Controls.Find($"产后{dt.order_num}天", true)[0];
                    ((CheckBox)yl).Checked = true;
                    Control sfsj = Controls.Find($"随访日期{dt.order_num}", true)[0];
                    ((DateTimePicker)sfsj).Value = Convert.ToDateTime(dt.visit_date);
                    Control yz = Controls.Find($"分娩日期{dt.order_num}", true)[0];
                    ((DateTimePicker)yz).Value = Convert.ToDateTime(dt.childbirth);
                    Control yfzszz = Controls.Find($"出院日期{dt.order_num}", true)[0];
                    ((DateTimePicker)yfzszz).Value = Convert.ToDateTime(dt.discharge_date);
                    Control tz = Controls.Find($"体温{dt.order_num}", true)[0];
                    ((TextBox)tz).Text = dt.temperature.ToString();
                    Control gg = Controls.Find($"一般健康情况{dt.order_num}", true)[0];
                    ((TextBox)gg).Text = dt.general_health_status.ToString();
                    Control fw = Controls.Find($"一般心理状况{dt.order_num}", true)[0];
                    ((TextBox)fw).Text = dt.general_psychology_status.ToString();
                    Control gxy = Controls.Find($"血压高{dt.order_num}", true)[0];
                    ((TextBox)gxy).Text = dt.blood_pressure_high.ToString();
                    Control dxy = Controls.Find($"血压低{dt.order_num}", true)[0];
                    ((TextBox)dxy).Text = dt.blood_pressure_low.ToString();
                    Control rf = Controls.Find($"乳房{dt.order_num}", true)[0];
                    foreach (Control item in ((GroupBox)rf).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.breast)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.breast_error;
                        }
                    }
                    Control el = Controls.Find($"恶露{dt.order_num}", true)[0];
                    foreach (Control item in ((GroupBox)el).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.lyma)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.lyma_error;
                        }
                    }
                    Control zg = Controls.Find($"子宫{dt.order_num}", true)[0];
                    foreach (Control item in ((GroupBox)zg).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.womb)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.womb_error;
                        }
                    }
                    Control sk = Controls.Find($"伤口{dt.order_num}", true)[0];
                    foreach (Control item in ((GroupBox)sk).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.wound)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.wound_error;
                        }
                    }
                    Control qtfzjc = Controls.Find($"其他{dt.order_num}", true)[0];
                    ((TextBox)qtfzjc).Text = dt.other.ToString();
                    Control fl = Controls.Find($"分类{dt.order_num}", true)[0];
                    foreach (Control item in ((GroupBox)fl).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.condition)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.error_info;
                        }
                    }
                    Control zzjtz = Controls.Find($"指导{dt.order_num}", true)[0];
                    foreach (Control item in ((GroupBox)zzjtz).Controls)
                    {
                        if (item is CheckBox)
                        {
                            if (dt.guidance.IndexOf(",") >= 0)
                            {
                                string[] sys = dt.guidance.Split(',');
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
                                if (((CheckBox)item).Tag.ToString() == dt.guidance)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                        else if (item is RichTextBox)
                        {
                            ((RichTextBox)item).Text = dt.guidance_other;
                        }
                    }
                    Control zz = Controls.Find($"转诊{dt.order_num}", true)[0];
                    foreach (Control item in ((GroupBox)zz).Controls)
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
                            if (((TextBox)item).Name == $"转诊原因{dt.order_num}")
                            {
                                ((TextBox)item).Text = dt.transfer_treatment_reason;
                            }
                            else if (((TextBox)item).Name == $"转诊机构和科别{dt.order_num}")
                            {
                                ((TextBox)item).Text = dt.transfer_treatment_department;
                            }
                        }
                    }
                    Control xcsf = Controls.Find($"下次随访日期{dt.order_num}", true)[0];
                    ((DateTimePicker)xcsf).Value = Convert.ToDateTime(dt.next_visit_date);
                    Control xy1 = Controls.Find($"随访医生签名{dt.order_num}", true)[0];
                    ((TextBox)xy1).Text = dt.visit_doctor.ToString();
                }
            }
        }
        /// <summary>
        /// 判断是否有修改数据
        /// </summary>
        /// <returns></returns>
        private bool GetUpdate()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from gravida_after_record where name='{Names}' and aichive_no='{aichive_no}' and id_number='{id_number}'");
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
    /// 新生儿记录表
    /// </summary>
    public class neonatus_info
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
        public string archive_no { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string id_number { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string birthday { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string home_address { get; set; }
        /// <summary>
        /// 父亲姓名
        /// </summary>
        public string father_name { get; set; }
        /// <summary>
        /// 父亲职业
        /// </summary>
        public string father_profession { get; set; }
        /// <summary>
        /// 父亲电话
        /// </summary>
        public string father_phone { get; set; }
        /// <summary>
        /// 父亲出生日期
        /// </summary>
        public string father_birthday { get; set; }
        /// <summary>
        /// 母亲名字
        /// </summary>
        public string mother_name { get; set; }
        /// <summary>
        /// 母亲职业
        /// </summary>
        public string mother_profession { get; set; }
        /// <summary>
        /// 母亲电话
        /// </summary>
        public string mother_phone { get; set; }
        /// <summary>
        /// 母亲出生日期
        /// </summary>
        public string mother_birthday { get; set; }
        /// <summary>
        /// 孕周
        /// </summary>
        public int? gestational_weeks { get; set; }
        /// <summary>
        /// 妊娠期间患病情况
        /// </summary>
        public string sicken_stasus { get; set; }
        /// <summary>
        /// 患病其他
        /// </summary>
        public string sicken_other { get; set; }
        /// <summary>
        /// 呕吐
        /// </summary>
        public string midwife_org { get; set; }
        /// <summary>
        /// 出生情况
        /// </summary>
        public string birth_situation { get; set; }
        /// <summary>
        /// 出生情况其他
        /// </summary>
        public string birth_other { get; set; }
        /// <summary>
        /// 新生儿窒息
        /// </summary>
        public string asphyxia_neonatorum { get; set; }
        /// <summary>
        /// 窒息时间
        /// </summary>
        public string asphyxia_time { get; set; }
        /// <summary>
        /// 畸形
        /// </summary>
        public string deformity { get; set; }
        /// <summary>
        /// 畸形其他
        /// </summary>
        public string deformity_other { get; set; }
        /// <summary>
        /// 新生儿听力
        /// </summary>
        public string hearing { get; set; }
        /// <summary>
        /// 新生儿疾病
        /// </summary>
        public string disease { get; set; }
        /// <summary>
        /// 其他疾病
        /// </summary>
        public string disease_other { get; set; }
        /// <summary>
        /// 出生体重
        /// </summary>
        public string birth_weight { get; set; }
        /// <summary>
        /// 目前体重
        /// </summary>
        public string weight { get; set; }
        /// <summary>
        /// 出生身高
        /// </summary>
        public string birth_height { get; set; }
        /// <summary>
        /// 喂养方式
        /// </summary>
        public string feeding_patterns { get; set; }
        /// <summary>
        /// 吃奶次数
        /// </summary>
        public int? milk_num { get; set; }
        /// <summary>
        /// 吃奶量
        /// </summary>
        public int? milk_intake { get; set; }
        /// <summary>
        /// 呕吐
        /// </summary>
        public string vomit { get; set; }
        /// <summary>
        /// 大便
        /// </summary>
        public string shit { get; set; }
        /// <summary>
        /// 大便次数
        /// </summary>
        public string defecation_num { get; set; }
        /// <summary>
        /// 体温
        /// </summary>
        public string temperature { get; set; }
        /// <summary>
        /// 心率
        /// </summary>
        public int? heart_rate { get; set; }
        /// <summary>
        /// 呼吸评率
        /// </summary>
        public int? breathing_rate { get; set; }
        /// <summary>
        /// 面色
        /// </summary>
        public string complexion { get; set; }
        /// <summary>
        /// 面色其他
        /// </summary>
        public string complexion_other { get; set; }
        /// <summary>
        /// 黄疸部位
        /// </summary>
        public string aurigo { get; set; }
        /// <summary>
        /// 黄疸其他部位
        /// </summary>
        public string aurigo_other { get; set; }
        /// <summary>
        /// 前囱宽
        /// </summary>
        public string anterior_fontanelle_wide { get; set; }
        /// <summary>
        /// 前囱高
        /// </summary>
        public string anterior_fontanelle_high { get; set; }
        /// <summary>
        /// 前囱状态
        /// </summary>
        public string anterior_fontanelle { get; set; }
        /// <summary>
        /// 前囱其他
        /// </summary>
        public string anterior_fontanelle_other { get; set; }
        /// <summary>
        /// 眼睛是否异常
        /// </summary>
        public string eye { get; set; }
        /// <summary>
        /// 四肢活动度
        /// </summary>
        public string extremity_mobility { get; set; }
        /// <summary>
        /// 耳外观
        /// </summary>
        public string ear { get; set; }
        /// <summary>
        /// 颈部包块
        /// </summary>
        public string neck_mass { get; set; }
        /// <summary>
        /// 鼻子
        /// </summary>
        public string nose { get; set; }
        /// <summary>
        /// 皮肤
        /// </summary>
        public string skin { get; set; }
        /// <summary>
        /// 皮肤其他
        /// </summary>
        public string skin_other { get; set; }
        /// <summary>
        /// 口腔
        /// </summary>
        public string oral_cavity { get; set; }
        /// <summary>
        /// 肛门
        /// </summary>
        public string anus { get; set; }
        /// <summary>
        /// 心肺听诊
        /// </summary>
        public string heart_lung { get; set; }
        /// <summary>
        /// 胸部
        /// </summary>
        public string breast { get; set; }
        /// <summary>
        /// 腹部触诊
        /// </summary>
        public string abdominal_touch { get; set; }
        /// <summary>
        /// 脊柱
        /// </summary>
        public string spine { get; set; }
        /// <summary>
        /// 外生殖器
        /// </summary>
        public string aedea { get; set; }
        /// <summary>
        /// 脐带
        /// </summary>
        public string umbilical_cord { get; set; }
        /// <summary>
        /// 脐带其他
        /// </summary>
        public string umbilical_cord_other { get; set; }
        /// <summary>
        /// 有无转诊
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
        /// 指导
        /// </summary>
        public string guidance { get; set; }
        /// <summary>
        /// 指导其他
        /// </summary>
        public string guidance_other { get; set; }
        /// <summary>
        /// 访问日期
        /// </summary>
        public string visit_date { get; set; }
        /// <summary>
        /// 下次访问日期
        /// </summary>
        public string next_visit_date { get; set; }
        /// <summary>
        /// 下次访问地址
        /// </summary>
        public string next_visit_address { get; set; }
        /// <summary>
        /// 随访医生签名
        /// </summary>
        public string visit_doctor { get; set; }
        /// <summary>
        /// 省编码
        /// </summary>
        public string province_code { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string province_name { get; set; }
        /// <summary>
        /// 市编码
        /// </summary>
        public string city_code { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        public string city_name { get; set; }
        /// <summary>
        /// 县编码
        /// </summary>
        public string county_code { get; set; }
        /// <summary>
        /// 县名称
        /// </summary>
        public string county_name { get; set; }
        /// <summary>
        /// 镇编码
        /// </summary>
        public string towns_code { get; set; }
        /// <summary>
        /// 镇名称
        /// </summary>
        public string towns_name { get; set; }
        /// <summary>
        /// 村代码
        /// </summary>
        public string village_code { get; set; }
        /// <summary>
        /// 村名称
        /// </summary>
        public string village_name { get; set; }
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
