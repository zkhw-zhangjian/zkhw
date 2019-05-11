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
    public partial class addStartMaternalHealthServices : Form
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
        public addStartMaternalHealthServices(int ps, string names, string aichive_nos, string id_numbers)
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
            this.Text = (IS == 1 ? "第2～5次产前随访添加" : "第2～5次产前随访修改");
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
            List<gravida_follow_record> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();
            DBSql sqls = null;
            foreach (gravida_follow_record info in infolist)
            {
                sqls = new DBSql();
                sqls.sql = @"insert into gravida_follow_record(id,name,aichive_no,id_number,order_num,visit_date,gestational_weeks,symptom,weight,fundus_height,abdomen_circumference,fetus_position,fetal_heart_rate,blood_pressure_high,blood_pressure_low,hemoglobin,urine_protein,check_other,condition,error_info,guidance,guidance_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,next_visit_date,visit_doctor,create_user,create_name,create_time,upload_status) values(@id,@name,@aichive_no,@id_number,@order_num,@visit_date,@gestational_weeks,@symptom,@weight,@fundus_height,@abdomen_circumference,@fetus_position,@fetal_heart_rate,@blood_pressure_high,@blood_pressure_low,@hemoglobin,@urine_protein,@check_other,@condition,@error_info,@guidance,@guidance_other,@transfer_treatment,@transfer_treatment_reason,@transfer_treatment_department,@next_visit_date,@visit_doctor,@create_user,@create_name,@create_time,@upload_status);";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@order_num", info.order_num),
                    new MySqlParameter("@gestational_weeks", info.gestational_weeks),
                    new MySqlParameter("@symptom", info.symptom),
                    new MySqlParameter("@weight", info.weight),
                    new MySqlParameter("@fundus_height", info.fundus_height),
                    new MySqlParameter("@abdomen_circumference", info.abdomen_circumference),
                    new MySqlParameter("@fetus_position", info.fetus_position),
                    new MySqlParameter("@fetal_heart_rate", info.fetal_heart_rate),
                    new MySqlParameter("@blood_pressure_high", info.blood_pressure_high),
                    new MySqlParameter("@blood_pressure_low", info.blood_pressure_low),
                    new MySqlParameter("@hemoglobin", info.hemoglobin),
                    new MySqlParameter("@urine_protein", info.urine_protein),
                    new MySqlParameter("@check_other", info.check_other),
                    new MySqlParameter("@condition", info.condition),
                    new MySqlParameter("@error_info", info.error_info),
                    new MySqlParameter("@guidance", info.guidance),
                    new MySqlParameter("@guidance_other", info.guidance_other),
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
            List<gravida_follow_record> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();
            foreach (gravida_follow_record info in infolist)
            {
                DBSql sqls = new DBSql();
                sqls.sql = @"update gravida_follow_record set visit_date=@visit_date,gestational_weeks=@gestational_weeks,symptom=@symptom,
weight=@weight,fundus_height=@fundus_height,abdomen_circumference=@abdomen_circumference,fetus_position=@fetus_position,
fetal_heart_rate=@fetal_heart_rate,blood_pressure_high=@blood_pressure_high,blood_pressure_low=@blood_pressure_low,
hemoglobin=@hemoglobin,urine_protein=@urine_protein,check_other=@check_other,condition=@condition,error_info=@error_info,
guidance=@guidance,guidance_other=@guidance_other,transfer_treatment=@transfer_treatment,transfer_treatment_reason=@transfer_treatment_reason,transfer_treatment_department=@transfer_treatment_department,next_visit_date=@next_visit_date,visit_doctor=@visit_doctor,update_user=@update_user,update_name=@update_name,update_time=@update_time where name=@name and aichive_no=@aichive_no and id_number=@id_number and order_num=@order_num;";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@order_num", info.order_num),
                    new MySqlParameter("@gestational_weeks", info.gestational_weeks),
                    new MySqlParameter("@symptom", info.symptom),
                    new MySqlParameter("@weight", info.weight),
                    new MySqlParameter("@fundus_height", info.fundus_height),
                    new MySqlParameter("@abdomen_circumference", info.abdomen_circumference),
                    new MySqlParameter("@fetus_position", info.fetus_position),
                    new MySqlParameter("@fetal_heart_rate", info.fetal_heart_rate),
                    new MySqlParameter("@blood_pressure_high", info.blood_pressure_high),
                    new MySqlParameter("@blood_pressure_low", info.blood_pressure_low),
                    new MySqlParameter("@hemoglobin", info.hemoglobin),
                    new MySqlParameter("@urine_protein", info.urine_protein),
                    new MySqlParameter("@check_other", info.check_other),
                    new MySqlParameter("@condition", info.condition),
                    new MySqlParameter("@error_info", info.error_info),
                    new MySqlParameter("@guidance", info.guidance),
                    new MySqlParameter("@guidance_other", info.guidance_other),
                    new MySqlParameter("@transfer_treatment", info.transfer_treatment),
                    new MySqlParameter("@transfer_treatment_reason", info.transfer_treatment_reason),
                    new MySqlParameter("@transfer_treatment_department", info.transfer_treatment_department),
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
        private List<gravida_follow_record> GetData()
        {
            List<gravida_follow_record> infolist = new List<gravida_follow_record>();
            if (第2次.Checked)
            {
                gravida_follow_record info = new gravida_follow_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.order_num = "2";
                info.visit_date = 随访日期2.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.gestational_weeks = Convert.ToInt32(孕周2.Text.Trim());
                info.symptom = 孕妇自述症状2.Text.Trim();
                info.weight = 体重2.Text.Trim();
                info.fundus_height = 宫高2.Text.Trim();
                info.abdomen_circumference = 腹围2.Text.Trim();
                info.fetus_position = 胎儿的位置2.Text.Trim();
                info.fetal_heart_rate = 胎心率2.Text.Trim();
                info.blood_pressure_high = Convert.ToInt32(血压高2.Text.Trim());
                info.blood_pressure_low = Convert.ToInt32(血压低2.Text.Trim());
                info.hemoglobin = 血红蛋白2.Text.Trim();
                info.urine_protein = 尿蛋白2.Text.Trim();
                info.condition = 其他辅助检查2.Text.Trim();
                foreach (Control item in 分类2.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.condition = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.error_info = ((TextBox)item).Text;
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导2.Controls)
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
                foreach (Control item in 转诊2.Controls)
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
                        if (((TextBox)item).Name == "转诊原因2")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别2")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                info.next_visit_date = 下次随访日期2.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名2.Text.Trim();
                infolist.Add(info);
            }
            if (第3次.Checked)
            {
                gravida_follow_record info = new gravida_follow_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.order_num = "3";
                info.visit_date = 随访日期3.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.gestational_weeks = Convert.ToInt32(孕周3.Text.Trim());
                info.symptom = 孕妇自述症状3.Text.Trim();
                info.weight = 体重3.Text.Trim();
                info.fundus_height = 宫高3.Text.Trim();
                info.abdomen_circumference = 腹围3.Text.Trim();
                info.fetus_position = 胎儿的位置3.Text.Trim();
                info.fetal_heart_rate = 胎心率3.Text.Trim();
                info.blood_pressure_high = Convert.ToInt32(血压高3.Text.Trim());
                info.blood_pressure_low = Convert.ToInt32(血压低3.Text.Trim());
                info.hemoglobin = 血红蛋白3.Text.Trim();
                info.urine_protein = 尿蛋白3.Text.Trim();
                info.condition = 其他辅助检查3.Text.Trim();
                foreach (Control item in 分类3.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.condition = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.error_info = ((TextBox)item).Text;
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
                info.next_visit_date = 下次随访日期3.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名3.Text.Trim();
                infolist.Add(info);
            }
            if (第4次.Checked)
            {
                gravida_follow_record info = new gravida_follow_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.order_num = "4";
                info.visit_date = 随访日期4.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.gestational_weeks = Convert.ToInt32(孕周4.Text.Trim());
                info.symptom = 孕妇自述症状4.Text.Trim();
                info.weight = 体重4.Text.Trim();
                info.fundus_height = 宫高4.Text.Trim();
                info.abdomen_circumference = 腹围4.Text.Trim();
                info.fetus_position = 胎儿的位置4.Text.Trim();
                info.fetal_heart_rate = 胎心率4.Text.Trim();
                info.blood_pressure_high = Convert.ToInt32(血压高4.Text.Trim());
                info.blood_pressure_low = Convert.ToInt32(血压低4.Text.Trim());
                info.hemoglobin = 血红蛋白4.Text.Trim();
                info.urine_protein = 尿蛋白4.Text.Trim();
                info.condition = 其他辅助检查4.Text.Trim();
                foreach (Control item in 分类4.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.condition = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.error_info = ((TextBox)item).Text;
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导4.Controls)
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
                foreach (Control item in 转诊4.Controls)
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
                        if (((TextBox)item).Name == "转诊原因4")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别4")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                info.next_visit_date = 下次随访日期4.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名4.Text.Trim();
                infolist.Add(info);
            }
            if (第5次.Checked)
            {
                gravida_follow_record info = new gravida_follow_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.order_num = "5";
                info.visit_date = 随访日期5.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.gestational_weeks = Convert.ToInt32(孕周5.Text.Trim());
                info.symptom = 孕妇自述症状5.Text.Trim();
                info.weight = 体重5.Text.Trim();
                info.fundus_height = 宫高5.Text.Trim();
                info.abdomen_circumference = 腹围5.Text.Trim();
                info.fetus_position = 胎儿的位置5.Text.Trim();
                info.fetal_heart_rate = 胎心率5.Text.Trim();
                info.blood_pressure_high = Convert.ToInt32(血压高5.Text.Trim());
                info.blood_pressure_low = Convert.ToInt32(血压低5.Text.Trim());
                info.hemoglobin = 血红蛋白5.Text.Trim();
                info.urine_protein = 尿蛋白5.Text.Trim();
                info.condition = 其他辅助检查5.Text.Trim();
                foreach (Control item in 分类5.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.condition = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.error_info = ((TextBox)item).Text;
                    }
                }
                string guidance = string.Empty;
                foreach (Control item in 指导5.Controls)
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
                foreach (Control item in 转诊5.Controls)
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
                        if (((TextBox)item).Name == "转诊原因5")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别5")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                info.next_visit_date = 下次随访日期5.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名5.Text.Trim();
                infolist.Add(info);
            }

            return infolist;
        }
        /// <summary>
        /// 界面赋值
        /// </summary>
        private void SetData()
        {
            string sql = $@"select * from gravida_follow_record where name='{Names}' and aichive_no='{aichive_no}' and id_number='{id_number}'";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                List<gravida_follow_record> ts = Result.ToDataList<gravida_follow_record>(jb.Tables[0]);
                foreach (var dt in ts)
                {
                    Control yl = this.Controls.Find($"第{dt.order_num}次", true)[0];
                    ((CheckBox)yl).Checked = true;
                    Control sfsj = Controls.Find($"随访日期{dt.order_num}", true)[0];
                    ((DateTimePicker)sfsj).Value = Convert.ToDateTime(dt.visit_date);
                    Control yz = Controls.Find($"孕周{dt.order_num}", true)[0];
                    ((TextBox)yz).Text = dt.gestational_weeks.ToString();
                    Control yfzszz = Controls.Find($"孕妇自述症状{dt.order_num}", true)[0];
                    ((TextBox)yfzszz).Text = dt.symptom.ToString();
                    Control tz = Controls.Find($"体重{dt.order_num}", true)[0];
                    ((TextBox)tz).Text = dt.weight.ToString();
                    Control gg = Controls.Find($"宫高{dt.order_num}", true)[0];
                    ((TextBox)gg).Text = dt.fundus_height.ToString();
                    Control fw = Controls.Find($"腹围{dt.order_num}", true)[0];
                    ((TextBox)fw).Text = dt.abdomen_circumference.ToString();
                    Control trwz = Controls.Find($"胎儿的位置{dt.order_num}", true)[0];
                    ((TextBox)trwz).Text = dt.fetus_position.ToString();
                    Control txl = Controls.Find($"胎心率{dt.order_num}", true)[0];
                    ((TextBox)txl).Text = dt.fetal_heart_rate.ToString();
                    Control gxy = Controls.Find($"血压高{dt.order_num}", true)[0];
                    ((TextBox)gxy).Text = dt.blood_pressure_high.ToString();
                    Control dxy = Controls.Find($"血压低{dt.order_num}", true)[0];
                    ((TextBox)dxy).Text = dt.blood_pressure_low.ToString();
                    Control xhdb = Controls.Find($"血红蛋白{dt.order_num}", true)[0];
                    ((TextBox)xhdb).Text = dt.hemoglobin.ToString();
                    Control ldb = Controls.Find($"尿蛋白{dt.order_num}", true)[0];
                    ((TextBox)ldb).Text = dt.urine_protein.ToString();
                    Control qtfzjc = Controls.Find($"其他辅助检查{dt.order_num}", true)[0];
                    ((TextBox)qtfzjc).Text = dt.condition.ToString();
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
            DataSet data = DbHelperMySQL.Query($@"select * from gravida_follow_record where name='{Names}' and aichive_no='{aichive_no}' and id_number='{id_number}'");
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
    /// 产前随访记录表
    /// </summary>
    public class gravida_follow_record
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
        /// 访问日期
        /// </summary>
        public string visit_date { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string order_num { get; set; }
        /// <summary>
        /// 孕周
        /// </summary>
        public int? gestational_weeks { get; set; }
        /// <summary>
        /// 孕妇自述症状
        /// </summary>
        public string symptom { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public string weight { get; set; }
        /// <summary>
        /// 宫高
        /// </summary>
        public string fundus_height { get; set; }
        /// <summary>
        /// 腹围
        /// </summary>
        public string abdomen_circumference { get; set; }
        /// <summary>
        /// 胎儿的位置
        /// </summary>
        public string fetus_position { get; set; }
        /// <summary>
        /// 胎心率
        /// </summary>
        public string fetal_heart_rate { get; set; }
        /// <summary>
        /// 血压高
        /// </summary>
        public int? blood_pressure_high { get; set; }
        /// <summary>
        /// 血压低
        /// </summary>
        public int? blood_pressure_low { get; set; }
        /// <summary>
        /// 血红蛋白
        /// </summary>
        public string hemoglobin { get; set; }
        /// <summary>
        /// 尿蛋白
        /// </summary>
        public string urine_protein { get; set; }
        /// <summary>
        /// 其他辅助检查
        /// </summary>
        public string check_other { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string condition { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string error_info { get; set; }
        /// <summary>
        /// 指导
        /// </summary>
        public string guidance { get; set; }
        /// <summary>
        /// 指导其他
        /// </summary>
        public string guidance_other { get; set; }
        
        /// <summary>
        /// 转移治疗
        /// </summary>
        public string transfer_treatment { get; set; }
        /// <summary>
        /// 转移治疗原因
        /// </summary>
        public string transfer_treatment_reason { get; set; }
        /// <summary>
        /// 转移治疗机构及科室
        /// </summary>
        public string transfer_treatment_department { get; set; }
        /// <summary>
        /// 下次访问日期
        /// </summary>
        public string next_visit_date { get; set; }
        /// <summary>
        /// 随访医生签名
        /// </summary>
        public string visit_doctor { get; set; }
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
