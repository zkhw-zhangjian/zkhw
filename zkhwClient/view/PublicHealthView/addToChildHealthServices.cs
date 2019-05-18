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
    public partial class addToChildHealthServices : Form
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
        public addToChildHealthServices(int ps, string names, string aichive_nos, string id_numbers)
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
            this.Text = (IS == 1 ? "0～6岁儿童健康检查添加" : "0～6岁儿童健康检查修改");
            InitializeComponent();

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
            List<children_health_record> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();

            foreach (children_health_record info in infolist)
            {
                DBSql sqls = new DBSql();
                sqls.sql = @"insert into children_health_record(id,name,archive_no,id_number,age,visit_date,weight,weight_evaluate,height,height_evaluate,weight_height,physical_assessment,head_circumference,complexion,complexion_other,skin,anterior_fontanelle_wide,anterior_fontanelle_high,anterior_fontanelle,neck_mass,eye,vision,ear,hearing,oral_cavity,teething_num,caries_num,breast,abdominal,umbilical_cord,extremity,gait,rickets_symptom,rickets_sign,anus,hemoglobin,other,outdoor_time,vitamind_name,vitamind_num,growth,sicken_stasus,pneumonia_num,diarrhea_num,trauma_num,sicken_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,guidance,guidance_other,next_visit_date,visit_doctor,create_user,create_name,create_time,create_org,create_org_name,upload_status) values(@id,@name,@archive_no,@id_number,@age,@visit_date,@weight,@weight_evaluate,@height,@height_evaluate,@weight_height,@physical_assessment,@head_circumference,@complexion,@complexion_other,@skin,@anterior_fontanelle_wide,@anterior_fontanelle_high,@anterior_fontanelle,@neck_mass,@eye,@vision,@ear,@hearing,@oral_cavity,@teething_num,@caries_num,@breast,@abdominal,@umbilical_cord,@extremity,@gait,@rickets_symptom,@rickets_sign,@anus,@hemoglobin,@other,@outdoor_time,@vitamind_name,@vitamind_num,@growth,@sicken_stasus,@pneumonia_num,@diarrhea_num,@trauma_num,@sicken_other,@transfer_treatment,@transfer_treatment_reason,@transfer_treatment_department,@guidance,@guidance_other,
@next_visit_date,@visit_doctor,@create_user,@create_name,@create_time,@create_org,@create_org_name,@upload_status);";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@archive_no", info.archive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@age", info.age),
                    new MySqlParameter("@weight", info.weight),
                    new MySqlParameter("@weight_evaluate", info.weight_evaluate),
                    new MySqlParameter("@height", info.height),
                    new MySqlParameter("@height_evaluate", info.height_evaluate),
                    new MySqlParameter("@weight_height", info.weight_height),
                    new MySqlParameter("@physical_assessment", info.physical_assessment),
                    new MySqlParameter("@head_circumference", info.head_circumference),
                    new MySqlParameter("@complexion", info.complexion),
                    new MySqlParameter("@complexion_other", info.complexion_other),
                    new MySqlParameter("@skin", info.skin),
                    new MySqlParameter("@anterior_fontanelle_wide", info.anterior_fontanelle_wide),
                    new MySqlParameter("@anterior_fontanelle_high", info.anterior_fontanelle_high),
                    new MySqlParameter("@anterior_fontanelle", info.anterior_fontanelle),
                    new MySqlParameter("@neck_mass", info.neck_mass),
                    new MySqlParameter("@eye", info.eye),
                    new MySqlParameter("@vision", info.vision),
                    new MySqlParameter("@ear", info.ear),
                    new MySqlParameter("@hearing", info.hearing),
                    new MySqlParameter("@oral_cavity", info.oral_cavity),
                    new MySqlParameter("@teething_num", info.teething_num),
                    new MySqlParameter("@caries_num", info.caries_num),
                    new MySqlParameter("@breast", info.breast),
                    new MySqlParameter("@abdominal", info.abdominal),
                    new MySqlParameter("@umbilical_cord", info.umbilical_cord),
                    new MySqlParameter("@extremity", info.extremity),
                    new MySqlParameter("@gait", info.gait),
                    new MySqlParameter("@rickets_symptom", info.rickets_symptom),
                    new MySqlParameter("@rickets_sign", info.rickets_sign),
                    new MySqlParameter("@anus", info.anus),
                    new MySqlParameter("@hemoglobin", info.hemoglobin),
                    new MySqlParameter("@other", info.other),
                    new MySqlParameter("@outdoor_time", info.outdoor_time),
                    new MySqlParameter("@vitamind_name", info.vitamind_name),
                    new MySqlParameter("@vitamind_num", info.vitamind_num),
                    new MySqlParameter("@growth", info.growth),
                    new MySqlParameter("@sicken_stasus", info.sicken_stasus),
                    new MySqlParameter("@pneumonia_num", info.pneumonia_num),
                    new MySqlParameter("@diarrhea_num", info.diarrhea_num),
                    new MySqlParameter("@trauma_num", info.trauma_num),
                    new MySqlParameter("@sicken_other", info.sicken_other),
                    new MySqlParameter("@transfer_treatment", info.transfer_treatment),
                    new MySqlParameter("@transfer_treatment_reason", info.transfer_treatment_reason),
                    new MySqlParameter("@transfer_treatment_department", info.transfer_treatment_department),
                    new MySqlParameter("@guidance", info.guidance),
                    new MySqlParameter("@guidance_other", info.guidance_other),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
                    new MySqlParameter("@visit_doctor", info.visit_doctor),
                    new MySqlParameter("@create_user", info.create_user),
                    new MySqlParameter("@create_name", info.create_name),
                    new MySqlParameter("@create_time", info.create_time),
                    new MySqlParameter("@create_org", info.create_org),
                    new MySqlParameter("@create_org_name", info.create_org_name),
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
            List<children_health_record> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();
            foreach (children_health_record info in infolist)
            {
                DBSql sqls = new DBSql();
                sqls.sql = @"update children_health_record set visit_date=@visit_date,weight=@weight,weight_evaluate=@weight_evaluate,height=@height,height_evaluate=@height_evaluate,weight_height=@weight_height,physical_assessment=@physical_assessment,head_circumference=@head_circumference,complexion=@complexion,complexion_other=@complexion_other,skin=@skin,anterior_fontanelle_wide=@anterior_fontanelle_wide,anterior_fontanelle_high=@anterior_fontanelle_high,anterior_fontanelle=@anterior_fontanelle,neck_mass=@neck_mass,eye=@eye,vision=@vision,ear=@ear,hearing=@hearing,oral_cavity=@oral_cavity,teething_num=@teething_num,caries_num=@caries_num,breast=@breast,abdominal=@abdominal,umbilical_cord=@umbilical_cord,extremity=@extremity,gait=@gait,rickets_symptom=@rickets_symptom,rickets_sign=@rickets_sign,anus=@anus,hemoglobin=@hemoglobin,other=@other,outdoor_time=@outdoor_time,vitamind_name=@vitamind_name,vitamind_num=@vitamind_num,growth=@growth,sicken_stasus=@sicken_stasus,pneumonia_num=@pneumonia_num,diarrhea_num=@diarrhea_num,trauma_num=@trauma_num,sicken_other=@sicken_other,transfer_treatment=@transfer_treatment,transfer_treatment_reason=@transfer_treatment_reason,transfer_treatment_department=@transfer_treatment_department,guidance=@guidance,guidance_other=@guidance_other,next_visit_date=@next_visit_date,visit_doctor=@visit_doctor,update_user=@update_user,update_name=@update_name,update_time=@update_time where name=@name and archive_no=@archive_no and id_number=@id_number and age=@age;";
                sqls.parameters = new MySqlParameter[] {
                     new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@archive_no", info.archive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@age", info.age),
                    new MySqlParameter("@weight", info.weight),
                    new MySqlParameter("@weight_evaluate", info.weight_evaluate),
                    new MySqlParameter("@height", info.height),
                    new MySqlParameter("@height_evaluate", info.height_evaluate),
                    new MySqlParameter("@weight_height", info.weight_height),
                    new MySqlParameter("@physical_assessment", info.physical_assessment),
                    new MySqlParameter("@head_circumference", info.head_circumference),
                    new MySqlParameter("@complexion", info.complexion),
                    new MySqlParameter("@complexion_other", info.complexion_other),
                    new MySqlParameter("@skin", info.skin),
                    new MySqlParameter("@anterior_fontanelle_wide", info.anterior_fontanelle_wide),
                    new MySqlParameter("@anterior_fontanelle_high", info.anterior_fontanelle_high),
                    new MySqlParameter("@anterior_fontanelle", info.anterior_fontanelle),
                    new MySqlParameter("@neck_mass", info.neck_mass),
                    new MySqlParameter("@eye", info.eye),
                    new MySqlParameter("@vision", info.vision),
                    new MySqlParameter("@ear", info.ear),
                    new MySqlParameter("@hearing", info.hearing),
                    new MySqlParameter("@oral_cavity", info.oral_cavity),
                    new MySqlParameter("@teething_num", info.teething_num),
                    new MySqlParameter("@caries_num", info.caries_num),
                    new MySqlParameter("@breast", info.breast),
                    new MySqlParameter("@abdominal", info.abdominal),
                    new MySqlParameter("@umbilical_cord", info.umbilical_cord),
                    new MySqlParameter("@extremity", info.extremity),
                    new MySqlParameter("@gait", info.gait),
                    new MySqlParameter("@rickets_symptom", info.rickets_symptom),
                    new MySqlParameter("@rickets_sign", info.rickets_sign),
                    new MySqlParameter("@anus", info.anus),
                    new MySqlParameter("@hemoglobin", info.hemoglobin),
                    new MySqlParameter("@other", info.other),
                    new MySqlParameter("@outdoor_time", info.outdoor_time),
                    new MySqlParameter("@vitamind_name", info.vitamind_name),
                    new MySqlParameter("@vitamind_num", info.vitamind_num),
                    new MySqlParameter("@growth", info.growth),
                    new MySqlParameter("@sicken_stasus", info.sicken_stasus),
                    new MySqlParameter("@pneumonia_num", info.pneumonia_num),
                    new MySqlParameter("@diarrhea_num", info.diarrhea_num),
                    new MySqlParameter("@trauma_num", info.trauma_num),
                    new MySqlParameter("@sicken_other", info.sicken_other),
                    new MySqlParameter("@transfer_treatment", info.transfer_treatment),
                    new MySqlParameter("@transfer_treatment_reason", info.transfer_treatment_reason),
                    new MySqlParameter("@transfer_treatment_department", info.transfer_treatment_department),
                    new MySqlParameter("@guidance", info.guidance),
                    new MySqlParameter("@guidance_other", info.guidance_other),
                    new MySqlParameter("@next_visit_date", info.next_visit_date),
                    new MySqlParameter("@visit_doctor", info.visit_doctor),
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
        private List<children_health_record> GetData()
        {
            List<children_health_record> infolist = new List<children_health_record>();
            if (月龄1.Checked)
            {
                children_health_record info = new children_health_record();
                info.name = Names;
                info.archive_no = aichive_no;
                info.id_number = id_number;
                info.age = "1";
                info.visit_date = 随访日期1.Value.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (Control item in 体重1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.weight_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.weight = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 身长1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.height_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.height = ((TextBox)item).Text;
                    }
                }
                info.head_circumference = 头围1.Text.Trim();
                foreach (Control item in 面色1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.complexion = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.complexion_other = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 皮肤1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.skin = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 前囟1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anterior_fontanelle = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "前囟宽1")
                        {
                            info.anterior_fontanelle_wide = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "前囟高1")
                        {
                            info.anterior_fontanelle_high = ((TextBox)item).Text;
                        }
                    }
                }
                foreach (Control item in 颈部包块1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.neck_mass = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 眼睛1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.eye = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 耳1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.ear = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 听力1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.hearing = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 口腔1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.oral_cavity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "出牙数1")
                        {
                            info.teething_num =string.IsNullOrWhiteSpace(((TextBox)item).Text)?0:Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "龋齿数1")
                        {
                            info.caries_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                    }
                }
                foreach (Control item in 胸部1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 腹部1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.abdominal = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 脐部1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.umbilical_cord = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 四肢1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.extremity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 步态1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.gait = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病症状1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_symptom = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病体征1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_sign = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 肛门1.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anus = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                info.hemoglobin = 血红蛋白值1.Text.Trim();
                info.outdoor_time = 户外活动时间1.Text.Trim();
                info.vitamind_name = 维生素D名称1.Text.Trim();
                info.vitamind_num = 维生素D数量1.Text.Trim();
                string growth = string.Empty;
                foreach (Control item in 发育评估1.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                }
                info.growth = growth.TrimEnd(',');
                string sicken_stasus = string.Empty;
                foreach (Control item in 患病情况1.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            sicken_stasus += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "肺炎次1")
                        {
                            info.pneumonia_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "腹泻次1")
                        {
                            info.diarrhea_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "外伤次1")
                        {
                            info.trauma_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "其他1")
                        {
                            info.sicken_other = ((TextBox)item).Text;
                        }
                    }
                }
                info.sicken_stasus = sicken_stasus.TrimEnd(',');
                foreach (Control item in 转诊1.Controls)
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
                        if (((TextBox)item).Name == "转诊原因1")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别1")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导1.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            guidance += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.guidance_other = ((RichTextBox)item).Text;
                    }
                }
                info.guidance = guidance.TrimEnd(',');
                info.next_visit_date = 下次随访日期1.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名1.Text.Trim();
                infolist.Add(info);
            }
            if (月龄3.Checked)
            {
                children_health_record info = new children_health_record();
                info.name = Names;
                info.archive_no = aichive_no;
                info.id_number = id_number;
                info.age = "3";
                info.visit_date = 随访日期3.Value.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (Control item in 体重3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.weight_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.weight = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 身长3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.height_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.height = ((TextBox)item).Text;
                    }
                }
                info.head_circumference = 头围3.Text.Trim();
                foreach (Control item in 面色3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.complexion = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.complexion_other = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 皮肤3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.skin = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 前囟3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anterior_fontanelle = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "前囟宽3")
                        {
                            info.anterior_fontanelle_wide = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "前囟高3")
                        {
                            info.anterior_fontanelle_high = ((TextBox)item).Text;
                        }
                    }
                }
                foreach (Control item in 颈部包块3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.neck_mass = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 眼睛3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.eye = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 耳3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.ear = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 听力3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.hearing = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 口腔3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.oral_cavity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "出牙数3")
                        {
                            info.teething_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "龋齿数3")
                        {
                            info.caries_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                    }
                }
                foreach (Control item in 胸部3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 腹部3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.abdominal = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 脐部3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.umbilical_cord = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 四肢3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.extremity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 步态3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.gait = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病症状3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_symptom = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病体征3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_sign = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 肛门3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anus = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                info.hemoglobin = 血红蛋白值3.Text.Trim();
                info.outdoor_time = 户外活动时间3.Text.Trim();
                info.vitamind_name = 维生素D名称3.Text.Trim();
                info.vitamind_num = 维生素D数量3.Text.Trim();
                string growth = string.Empty;
                foreach (Control item in 发育评估3.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                }
                info.growth = growth.TrimEnd(',');
                string sicken_stasus = string.Empty;
                foreach (Control item in 患病情况3.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "肺炎次3")
                        {
                            info.pneumonia_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "腹泻次3")
                        {
                            info.diarrhea_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "外伤次3")
                        {
                            info.trauma_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "其他3")
                        {
                            info.sicken_other = ((TextBox)item).Text;
                        }
                    }
                }
                info.sicken_stasus = sicken_stasus.TrimEnd(',');
                foreach (Control item in 转诊3.Controls)
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
                        if (((TextBox)item).Name == "转诊原因3")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别3")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导3.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            guidance += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.guidance_other = ((RichTextBox)item).Text;
                    }
                }
                info.guidance = guidance.TrimEnd(',');
                info.next_visit_date = 下次随访日期3.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名3.Text.Trim();
                infolist.Add(info);
            }
            if (月龄6.Checked)
            {
                children_health_record info = new children_health_record();
                info.name = Names;
                info.archive_no = aichive_no;
                info.id_number = id_number;
                info.age = "6";
                info.visit_date = 随访日期6.Value.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (Control item in 体重6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.weight_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.weight = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 身长6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.height_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.height = ((TextBox)item).Text;
                    }
                }
                info.head_circumference = 头围6.Text.Trim();
                foreach (Control item in 面色6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.complexion = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.complexion_other = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 皮肤6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.skin = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 前囟6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anterior_fontanelle = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "前囟宽6")
                        {
                            info.anterior_fontanelle_wide = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "前囟高6")
                        {
                            info.anterior_fontanelle_high = ((TextBox)item).Text;
                        }
                    }
                }
                foreach (Control item in 颈部包块6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.neck_mass = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 眼睛6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.eye = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 耳6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.ear = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 听力6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.hearing = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 口腔6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.oral_cavity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "出牙数6")
                        {
                            info.teething_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "龋齿数6")
                        {
                            info.caries_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                    }
                }
                foreach (Control item in 胸部6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 腹部6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.abdominal = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 脐部6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.umbilical_cord = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 四肢6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.extremity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 步态6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.gait = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病症状6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_symptom = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病体征6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_sign = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 肛门6.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anus = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                info.hemoglobin = 血红蛋白值6.Text.Trim();
                info.outdoor_time = 户外活动时间6.Text.Trim();
                info.vitamind_name = 维生素D名称6.Text.Trim();
                info.vitamind_num = 维生素D数量6.Text.Trim();
                string growth = string.Empty;
                foreach (Control item in 发育评估6.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                }
                info.growth = growth.TrimEnd(',');
                string sicken_stasus = string.Empty;
                foreach (Control item in 患病情况6.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "肺炎次6")
                        {
                            info.pneumonia_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "腹泻次6")
                        {
                            info.diarrhea_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "外伤次6")
                        {
                            info.trauma_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "其他6")
                        {
                            info.sicken_other = ((TextBox)item).Text;
                        }
                    }
                }
                info.sicken_stasus = sicken_stasus.TrimEnd(',');
                foreach (Control item in 转诊6.Controls)
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
                        if (((TextBox)item).Name == "转诊原因6")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别6")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导6.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            guidance += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.guidance_other = ((RichTextBox)item).Text;
                    }
                }
                info.guidance = guidance.TrimEnd(',');
                info.next_visit_date = 下次随访日期6.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名6.Text.Trim();
                infolist.Add(info);
            }
            if (月龄8.Checked)
            {
                children_health_record info = new children_health_record();
                info.name = Names;
                info.archive_no = aichive_no;
                info.id_number = id_number;
                info.age = "8";
                info.visit_date = 随访日期8.Value.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (Control item in 体重8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.weight_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.weight = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 身长8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.height_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.height = ((TextBox)item).Text;
                    }
                }
                info.head_circumference = 头围8.Text.Trim();
                foreach (Control item in 面色8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.complexion = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.complexion_other = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 皮肤8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.skin = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 前囟8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anterior_fontanelle = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "前囟宽8")
                        {
                            info.anterior_fontanelle_wide = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "前囟高8")
                        {
                            info.anterior_fontanelle_high = ((TextBox)item).Text;
                        }
                    }
                }
                foreach (Control item in 颈部包块8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.neck_mass = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 眼睛8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.eye = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 耳8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.ear = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 听力8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.hearing = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 口腔8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.oral_cavity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "出牙数8")
                        {
                            info.teething_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "龋齿数8")
                        {
                            info.caries_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                    }
                }
                foreach (Control item in 胸部8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 腹部8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.abdominal = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 脐部8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.umbilical_cord = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 四肢8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.extremity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 步态8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.gait = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病症状8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_symptom = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病体征8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_sign = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 肛门8.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anus = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                info.hemoglobin = 血红蛋白值8.Text.Trim();
                info.outdoor_time = 户外活动时间8.Text.Trim();
                info.vitamind_name = 维生素D名称8.Text.Trim();
                info.vitamind_num = 维生素D数量8.Text.Trim();
                string growth = string.Empty;
                foreach (Control item in 发育评估8.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                }
                info.growth = growth.TrimEnd(',');
                string sicken_stasus = string.Empty;
                foreach (Control item in 患病情况8.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "肺炎次8")
                        {
                            info.pneumonia_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "腹泻次8")
                        {
                            info.diarrhea_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "外伤次8")
                        {
                            info.trauma_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "其他8")
                        {
                            info.sicken_other = ((TextBox)item).Text;
                        }
                    }
                }
                info.sicken_stasus = sicken_stasus.TrimEnd(',');
                foreach (Control item in 转诊8.Controls)
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
                        if (((TextBox)item).Name == "转诊原因8")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别8")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导8.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            guidance += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.guidance_other = ((RichTextBox)item).Text;
                    }
                }
                info.guidance = guidance.TrimEnd(',');
                info.next_visit_date = 下次随访日期8.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名8.Text.Trim();
                infolist.Add(info);
            }
            if (月龄12.Checked)
            {
                children_health_record info = new children_health_record();
                info.name = Names;
                info.archive_no = aichive_no;
                info.id_number = id_number;
                info.age = "12";
                info.visit_date = 随访日期12.Value.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (Control item in 体重12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.weight_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.weight = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 身长12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.height_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.height = ((TextBox)item).Text;
                    }
                }
                info.head_circumference = 头围12.Text.Trim();
                foreach (Control item in 面色12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.complexion = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.complexion_other = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 皮肤12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.skin = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 前囟12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anterior_fontanelle = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "前囟宽12")
                        {
                            info.anterior_fontanelle_wide = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "前囟高12")
                        {
                            info.anterior_fontanelle_high = ((TextBox)item).Text;
                        }
                    }
                }
                foreach (Control item in 颈部包块12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.neck_mass = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 眼睛12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.eye = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 耳12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.ear = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 听力12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.hearing = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 口腔12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.oral_cavity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "出牙数12")
                        {
                            info.teething_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "龋齿数12")
                        {
                            info.caries_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                    }
                }
                foreach (Control item in 胸部12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 腹部12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.abdominal = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 脐部12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.umbilical_cord = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 四肢12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.extremity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 步态12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.gait = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病症状12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_symptom = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病体征12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_sign = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 肛门12.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anus = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                info.hemoglobin = 血红蛋白值12.Text.Trim();
                info.outdoor_time = 户外活动时间12.Text.Trim();
                info.vitamind_name = 维生素D名称12.Text.Trim();
                info.vitamind_num = 维生素D数量12.Text.Trim();
                string growth = string.Empty;
                foreach (Control item in 发育评估12.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                }
                info.growth = growth.TrimEnd(',');
                string sicken_stasus = string.Empty;
                foreach (Control item in 患病情况12.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "肺炎次12")
                        {
                            info.pneumonia_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "腹泻次12")
                        {
                            info.diarrhea_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "外伤次12")
                        {
                            info.trauma_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "其他12")
                        {
                            info.sicken_other = ((TextBox)item).Text;
                        }
                    }
                }
                info.sicken_stasus = sicken_stasus.TrimEnd(',');
                foreach (Control item in 转诊12.Controls)
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
                        if (((TextBox)item).Name == "转诊原因12")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别12")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导12.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            guidance += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.guidance_other = ((RichTextBox)item).Text;
                    }
                }
                info.guidance = guidance.TrimEnd(',');
                info.next_visit_date = 下次随访日期12.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名12.Text.Trim();
                infolist.Add(info);
            }
            if (月龄18.Checked)
            {
                children_health_record info = new children_health_record();
                info.name = Names;
                info.archive_no = aichive_no;
                info.id_number = id_number;
                info.age = "18";
                info.visit_date = 随访日期18.Value.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (Control item in 体重18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.weight_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.weight = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 身长18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.height_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.height = ((TextBox)item).Text;
                    }
                }
                info.head_circumference = 头围18.Text.Trim();
                foreach (Control item in 面色18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.complexion = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.complexion_other = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 皮肤18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.skin = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 前囟18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anterior_fontanelle = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "前囟宽18")
                        {
                            info.anterior_fontanelle_wide = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "前囟高18")
                        {
                            info.anterior_fontanelle_high = ((TextBox)item).Text;
                        }
                    }
                }
                foreach (Control item in 颈部包块18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.neck_mass = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 眼睛18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.eye = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 耳18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.ear = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 听力18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.hearing = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 口腔18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.oral_cavity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "出牙数18")
                        {
                            info.teething_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "龋齿数18")
                        {
                            info.caries_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                    }
                }
                foreach (Control item in 胸部18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 腹部18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.abdominal = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 脐部18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.umbilical_cord = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 四肢18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.extremity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 步态18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.gait = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病症状18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_symptom = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病体征18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_sign = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 肛门18.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anus = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                info.hemoglobin = 血红蛋白值18.Text.Trim();
                info.outdoor_time = 户外活动时间18.Text.Trim();
                info.vitamind_name = 维生素D名称18.Text.Trim();
                info.vitamind_num = 维生素D数量18.Text.Trim();
                string growth = string.Empty;
                foreach (Control item in 发育评估18.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                }
                info.growth = growth.TrimEnd(',');
                string sicken_stasus = string.Empty;
                foreach (Control item in 患病情况18.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "肺炎次18")
                        {
                            info.pneumonia_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "腹泻次18")
                        {
                            info.diarrhea_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "外伤次18")
                        {
                            info.trauma_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "其他18")
                        {
                            info.sicken_other = ((TextBox)item).Text;
                        }
                    }
                }
                info.sicken_stasus = sicken_stasus.TrimEnd(',');
                foreach (Control item in 转诊18.Controls)
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
                        if (((TextBox)item).Name == "转诊原因18")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别18")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导18.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            guidance += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.guidance_other = ((RichTextBox)item).Text;
                    }
                }
                info.guidance = guidance.TrimEnd(',');
                info.next_visit_date = 下次随访日期18.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名18.Text.Trim();
                infolist.Add(info);
            }
            if (月龄24.Checked)
            {
                children_health_record info = new children_health_record();
                info.name = Names;
                info.archive_no = aichive_no;
                info.id_number = id_number;
                info.age = "24";
                info.visit_date = 随访日期24.Value.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (Control item in 体重24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.weight_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.weight = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 身长24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.height_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.height = ((TextBox)item).Text;
                    }
                }
                info.head_circumference = 头围24.Text.Trim();
                foreach (Control item in 面色24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.complexion = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.complexion_other = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 皮肤24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.skin = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 前囟24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anterior_fontanelle = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "前囟宽24")
                        {
                            info.anterior_fontanelle_wide = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "前囟高24")
                        {
                            info.anterior_fontanelle_high = ((TextBox)item).Text;
                        }
                    }
                }
                foreach (Control item in 颈部包块24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.neck_mass = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 眼睛24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.eye = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 耳24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.ear = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 听力24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.hearing = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 口腔24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.oral_cavity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "出牙数24")
                        {
                            info.teething_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "龋齿数24")
                        {
                            info.caries_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                    }
                }
                foreach (Control item in 胸部24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 腹部24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.abdominal = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 脐部24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.umbilical_cord = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 四肢24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.extremity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 步态24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.gait = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病症状24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_symptom = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病体征24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_sign = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 肛门24.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anus = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                info.hemoglobin = 血红蛋白值24.Text.Trim();
                info.outdoor_time = 户外活动时间24.Text.Trim();
                info.vitamind_name = 维生素D名称24.Text.Trim();
                info.vitamind_num = 维生素D数量24.Text.Trim();
                string growth = string.Empty;
                foreach (Control item in 发育评估24.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                }
                info.growth = growth.TrimEnd(',');
                string sicken_stasus = string.Empty;
                foreach (Control item in 患病情况24.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "肺炎次24")
                        {
                            info.pneumonia_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "腹泻次24")
                        {
                            info.diarrhea_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "外伤次24")
                        {
                            info.trauma_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "其他24")
                        {
                            info.sicken_other = ((TextBox)item).Text;
                        }
                    }
                }
                info.sicken_stasus = sicken_stasus.TrimEnd(',');
                foreach (Control item in 转诊24.Controls)
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
                        if (((TextBox)item).Name == "转诊原因24")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别24")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导24.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            guidance += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.guidance_other = ((RichTextBox)item).Text;
                    }
                }
                info.guidance = guidance.TrimEnd(',');
                info.next_visit_date = 下次随访日期24.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名24.Text.Trim();
                infolist.Add(info);
            }
            if (月龄30.Checked)
            {
                children_health_record info = new children_health_record();
                info.name = Names;
                info.archive_no = aichive_no;
                info.id_number = id_number;
                info.age = "30";
                info.visit_date = 随访日期30.Value.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (Control item in 体重30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.weight_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.weight = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 身长30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.height_evaluate = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.height = ((TextBox)item).Text;
                    }
                }
                info.head_circumference = 头围30.Text.Trim();
                foreach (Control item in 面色30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.complexion = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.complexion_other = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 皮肤30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.skin = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 前囟30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anterior_fontanelle = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "前囟宽30")
                        {
                            info.anterior_fontanelle_wide = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "前囟高30")
                        {
                            info.anterior_fontanelle_high = ((TextBox)item).Text;
                        }
                    }
                }
                foreach (Control item in 颈部包块30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.neck_mass = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 眼睛30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.eye = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 耳30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.ear = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 听力30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.hearing = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 口腔30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.oral_cavity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "出牙数30")
                        {
                            info.teething_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "龋齿数30")
                        {
                            info.caries_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                    }
                }
                foreach (Control item in 胸部30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 腹部30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.abdominal = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 脐部30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.umbilical_cord = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 四肢30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.extremity = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 步态30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.gait = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病症状30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_symptom = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 可疑佝偻病体征30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.rickets_sign = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                foreach (Control item in 肛门30.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.anus = ((RadioButton)item).Tag.ToString();
                        }
                    }
                }
                info.hemoglobin = 血红蛋白值30.Text.Trim();
                info.outdoor_time = 户外活动时间30.Text.Trim();
                info.vitamind_name = 维生素D名称30.Text.Trim();
                info.vitamind_num = 维生素D数量30.Text.Trim();
                string growth = string.Empty;
                foreach (Control item in 发育评估30.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                }
                info.growth = growth.TrimEnd(',');
                string sicken_stasus = string.Empty;
                foreach (Control item in 患病情况30.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            growth += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is TextBox)
                    {
                        if (((TextBox)item).Name == "肺炎次30")
                        {
                            info.pneumonia_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "腹泻次30")
                        {
                            info.diarrhea_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "外伤次30")
                        {
                            info.trauma_num = string.IsNullOrWhiteSpace(((TextBox)item).Text) ? 0 : Convert.ToInt32(((TextBox)item).Text);
                        }
                        else if (((TextBox)item).Name == "其他30")
                        {
                            info.sicken_other = ((TextBox)item).Text;
                        }
                    }
                }
                info.sicken_stasus = sicken_stasus.TrimEnd(',');
                foreach (Control item in 转诊30.Controls)
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
                        if (((TextBox)item).Name == "转诊原因30")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别30")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导30.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            guidance += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.guidance_other = ((RichTextBox)item).Text;
                    }
                }
                info.guidance = guidance.TrimEnd(',');
                info.next_visit_date = 下次随访日期30.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名30.Text.Trim();
                infolist.Add(info);
            }
            return infolist;
        }
        /// <summary>
        /// 界面赋值
        /// </summary>
        private void SetData()
        {
            string sql = $@"select * from children_health_record where name='{Names}' and archive_no='{aichive_no}' and id_number='{id_number}'";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                List<children_health_record> ts = Result.ToDataList<children_health_record>(jb.Tables[0]);
                foreach (var dt in ts)
                {
                    Control yl = this.Controls.Find($"月龄{dt.age}", true)[0];
                    ((CheckBox)yl).Checked = true;
                    Control sfsj = Controls.Find($"随访日期{dt.age}", true)[0];
                    ((DateTimePicker)sfsj).Value = Convert.ToDateTime(dt.visit_date);
                    Control tz = Controls.Find($"体重{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)tz).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.weight_evaluate)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.weight;
                        }
                    }
                    Control sc = Controls.Find($"身长{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)sc).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.height_evaluate)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.height;
                        }
                    }
                    Control tw = Controls.Find($"头围{dt.age}", true)[0];
                    ((TextBox)tw).Text = dt.head_circumference.ToString();
                    Control ms = Controls.Find($"面色{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)ms).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.complexion)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            ((TextBox)item).Text = dt.complexion_other;
                        }
                    }
                    Control pf = Controls.Find($"皮肤{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)pf).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.skin)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control qx = Controls.Find($"前囟{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)qx).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.anterior_fontanelle)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            if (((TextBox)item).Name == $"前囟宽{dt.age}")
                            {
                                ((TextBox)item).Text = dt.anterior_fontanelle_wide;
                            }
                            else if (((TextBox)item).Name == $"前囟高{dt.age}")
                            {
                                ((TextBox)item).Text = dt.anterior_fontanelle_high;
                            }
                        }
                    }
                    Control jbbk = Controls.Find($"颈部包块{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)jbbk).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.neck_mass)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control yj = Controls.Find($"眼睛{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)yj).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.eye)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control e = Controls.Find($"耳{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)e).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.ear)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control tl = Controls.Find($"听力{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)tl).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.hearing)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control kq = Controls.Find($"口腔{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)kq).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.oral_cavity)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                        else if (item is TextBox)
                        {
                            if (((TextBox)item).Name == $"出牙数{dt.age}")
                            {
                                ((TextBox)item).Text = dt.teething_num.ToString();
                            }
                            else if (((TextBox)item).Name == $"龋齿数{dt.age}")
                            {
                                ((TextBox)item).Text = dt.caries_num.ToString();
                            }
                        }
                    }
                    Control xb = Controls.Find($"胸部{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)xb).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.breast)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control fb = Controls.Find($"腹部{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)fb).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.abdominal)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control jib = Controls.Find($"脐部{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)jib).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.umbilical_cord)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control sz = Controls.Find($"四肢{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)sz).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.extremity)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control bc = Controls.Find($"步态{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)bc).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.gait)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control zz = Controls.Find($"可疑佝偻病症状{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)zz).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.rickets_symptom)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control tzs = Controls.Find($"可疑佝偻病体征{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)tzs).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.rickets_sign)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control gm = Controls.Find($"肛门{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)gm).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.anus)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control xhdbz = Controls.Find($"血红蛋白值{dt.age}", true)[0];
                    ((TextBox)xhdbz).Text = dt.hemoglobin.ToString();
                    Control hwhdsj = Controls.Find($"户外活动时间{dt.age}", true)[0];
                    ((TextBox)hwhdsj).Text = dt.outdoor_time.ToString();
                    Control wssmc = Controls.Find($"维生素D名称{dt.age}", true)[0];
                    ((TextBox)wssmc).Text = dt.vitamind_name.ToString();
                    Control wsssl = Controls.Find($"维生素D数量{dt.age}", true)[0];
                    ((TextBox)wsssl).Text = dt.vitamind_num.ToString();
                    if (!string.IsNullOrWhiteSpace(dt.growth))
                    {
                        Control fypg = Controls.Find($"发育评估{dt.age}", true)[0];
                        foreach (Control item in ((GroupBox)fypg).Controls)
                        {
                            if (item is CheckBox)
                            {
                                if (dt.growth.IndexOf(",") >= 0)
                                {
                                    string[] sys = dt.growth.Split(',');
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
                                    if (((CheckBox)item).Tag.ToString() == dt.growth)
                                    {
                                        ((CheckBox)item).Checked = true;
                                    }
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(dt.sicken_stasus))
                    {
                        Control fypg = Controls.Find($"患病情况{dt.age}", true)[0];
                        foreach (Control item in ((GroupBox)fypg).Controls)
                        {
                            if (item is CheckBox)
                            {
                                if (dt.sicken_stasus.IndexOf(",") >= 0)
                                {
                                    string[] sys = dt.sicken_stasus.Split(',');
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
                                    if (((CheckBox)item).Tag.ToString() == dt.sicken_stasus)
                                    {
                                        ((CheckBox)item).Checked = true;
                                    }
                                }
                            }
                            else if (item is TextBox)
                            {
                                if (((TextBox)item).Name == $"肺炎次{dt.age}")
                                {
                                    ((TextBox)item).Text = dt.pneumonia_num.ToString();
                                }
                                else if (((TextBox)item).Name == $"腹泻次{dt.age}")
                                {
                                    ((TextBox)item).Text = dt.diarrhea_num.ToString();
                                }
                                else if (((TextBox)item).Name == $"外伤次{dt.age}")
                                {
                                    ((TextBox)item).Text = dt.trauma_num.ToString();
                                }
                                else if (((TextBox)item).Name == $"其他{dt.age}")
                                {
                                    ((TextBox)item).Text = dt.sicken_other;
                                }
                            }
                        }
                    }
                    Control zwz = Controls.Find($"转诊{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)zwz).Controls)
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
                            if (((TextBox)item).Name == $"转诊原因{dt.age}")
                            {
                                ((TextBox)item).Text = dt.transfer_treatment_reason;
                            }
                            else if (((TextBox)item).Name == $"转诊机构和科别{dt.age}")
                            {
                                ((TextBox)item).Text = dt.transfer_treatment_department;
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(dt.sicken_stasus))
                    {
                        Control fypg = Controls.Find($"指导{dt.age}", true)[0];
                        foreach (Control item in ((GroupBox)fypg).Controls)
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
                    }
                    Control xcsf = Controls.Find($"下次随访日期{dt.age}", true)[0];
                    ((DateTimePicker)xcsf).Value = Convert.ToDateTime(dt.next_visit_date);
                    Control xy1 = Controls.Find($"随访医生签名{dt.age}", true)[0];
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
            DataSet data = DbHelperMySQL.Query($@"select * from children_health_record where name='{Names}' and archive_no='{aichive_no}' and id_number='{id_number}'");
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
    /// 0-6岁儿童健康检查记录表
    /// </summary>
    public class children_health_record
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
        /// 月龄
        /// </summary>
        public string age { get; set; }
        /// <summary>
        /// 随访日期
        /// </summary>
        public string visit_date { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public string weight { get; set; }
        /// <summary>
        /// 体重评价
        /// </summary>
        public string weight_evaluate { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public string height { get; set; }
        /// <summary>
        /// 身高评价
        /// </summary>
        public string height_evaluate { get; set; }
        /// <summary>
        /// 身高体重评估
        /// </summary>
        public string weight_height { get; set; }
        /// <summary>
        /// 体格发育评价
        /// </summary>
        public string physical_assessment { get; set; }
        /// <summary>
        /// 头围
        /// </summary>
        public string head_circumference { get; set; }
        /// <summary>
        /// 面色
        /// </summary>
        public string complexion { get; set; }
        /// <summary>
        /// 面色其他
        /// </summary>
        public string complexion_other { get; set; }
        /// <summary>
        /// 皮肤
        /// </summary>
        public string skin { get; set; }
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
        /// 颈部包块
        /// </summary>
        public string neck_mass { get; set; }
        /// <summary>
        /// 眼睛是否异常
        /// </summary>
        public string eye { get; set; }
        /// <summary>
        /// 视力
        /// </summary>
        public string vision { get; set; }
        /// <summary>
        /// 耳外观
        /// </summary>
        public string ear { get; set; }
        /// <summary>
        /// 听力
        /// </summary>
        public string hearing { get; set; }
        /// <summary>
        /// 口腔
        /// </summary>
        public string oral_cavity { get; set; }
        /// <summary>
        /// 出牙数
        /// </summary>
        public int? teething_num { get; set; }
        /// <summary>
        /// 龋齿数
        /// </summary>
        public int? caries_num { get; set; }
        /// <summary>
        /// 胸部
        /// </summary>
        public string breast { get; set; }
        /// <summary>
        /// 腹部
        /// </summary>
        public string abdominal { get; set; }
        /// <summary>
        /// 脐部
        /// </summary>
        public string umbilical_cord { get; set; }
        /// <summary>
        /// 四肢
        /// </summary>
        public string extremity { get; set; }
        /// <summary>
        /// 步态
        /// </summary>
        public string gait { get; set; }
        /// <summary>
        /// 可疑佝偻病症状
        /// </summary>
        public string rickets_symptom { get; set; }
        /// <summary>
        /// 可疑佝偻病体征
        /// </summary>
        public string rickets_sign { get; set; }
        /// <summary>
        /// 肛门
        /// </summary>
        public string anus { get; set; }
        /// <summary>
        /// 血红蛋白值
        /// </summary>
        public string hemoglobin { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string other { get; set; }
        /// <summary>
        /// 户外活动时间
        /// </summary>
        public string outdoor_time { get; set; }
        /// <summary>
        /// 维生素D名称
        /// </summary>
        public string vitamind_name { get; set; }
        /// <summary>
        /// 维生素D数量
        /// </summary>
        public string vitamind_num { get; set; }
        /// <summary>
        /// 发育评估
        /// </summary>
        public string growth { get; set; }
        /// <summary>
        /// 患病情况
        /// </summary>
        public string sicken_stasus { get; set; }
        /// <summary>
        /// 肺炎次数
        /// </summary>
        public int? pneumonia_num { get; set; }
        /// <summary>
        /// 腹泻次数
        /// </summary>
        public int? diarrhea_num { get; set; }
        /// <summary>
        /// 外伤次数
        /// </summary>
        public int? trauma_num { get; set; }
        /// <summary>
        /// 患病其他
        /// </summary>
        public string sicken_other { get; set; }
        /// <summary>
        /// 是否转诊
        /// </summary>
        public string transfer_treatment { get; set; }
        /// <summary>
        /// 转诊原因
        /// </summary>
        public string transfer_treatment_reason { get; set; }
        /// <summary>
        /// 转诊机构和科室
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
        /// 下次随访日期
        /// </summary>
        public string next_visit_date { get; set; }
        /// <summary>
        /// 随方医生签名
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
