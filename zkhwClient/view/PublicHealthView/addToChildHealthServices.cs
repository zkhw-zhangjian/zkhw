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
                sqls.sql = @"insert into children_health_record(id,name,archive_no,id_number,age,visit_date,weight,weight_evaluate,height,height_evaluate,weight_height,physical_assessment,head_circumference,complexion,complexion_other,skin,anterior_fontanelle_wide,anterior_fontanelle_high,anterior_fontanelle,neck_mass,eye,vision,ear,hearing,oral_cavity,teething_num,caries_num,breast,abdominal,umbilical_cord,extremity,gait,rickets_symptom,rickets_sign,anus,hemoglobin,other,outdoor_time,vitamind_name,vitamind_num,growth,sicken_stasus,pneumonia_num,diarrhea_num,trauma_num,sicken_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,guidance,guidance_other,next_visit_date,visit_doctor,create_user,create_name,create_time,upload_status) values(@id,@name,@archive_no,@id_number,@age,@visit_date,@weight,@weight_evaluate,@height,@height_evaluate,@weight_height,@physical_assessment,@head_circumference,@complexion,@complexion_other,@skin,@anterior_fontanelle_wide,@anterior_fontanelle_high,@anterior_fontanelle,@neck_mass,@eye,@vision,@ear,@hearing,@oral_cavity,@teething_num,@caries_num,@breast,@abdominal,@umbilical_cord,@extremity,@gait,@rickets_symptom,@rickets_sign,@anus,@hemoglobin,@other,@outdoor_time,@vitamind_name,@vitamind_num,@growth,@sicken_stasus,@pneumonia_num,@diarrhea_num,@trauma_num,@sicken_other,@transfer_treatment,@transfer_treatment_reason,@transfer_treatment_department,@guidance,@guidance_other,
@next_visit_date,@visit_doctor,@create_user,@create_name,@create_time,@upload_status);";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@archive_no", info.archive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@age", info.age),
                    new MySqlParameter("@visit_date", info.visit_date),
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
                    new MySqlParameter("@visit_date", info.visit_date),
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
            //if (月龄6.Checked)
            //{
            //    children_tcm_record info = new children_tcm_record();
            //    info.name = Names;
            //    info.aichive_no = aichive_no;
            //    info.id_number = id_number;
            //    info.age = "6";
            //    info.visit_date = 随访日期6.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    string tcm_info = string.Empty;
            //    foreach (Control item in 中医药健康管理服务6.Controls)
            //    {
            //        if (item is CheckBox)
            //        {
            //            if (((CheckBox)item).Checked)
            //            {
            //                tcm_info += ((CheckBox)item).Tag.ToString() + ",";
            //            }
            //        }
            //        else if (item is RichTextBox)
            //        {
            //            info.tcm_other = ((RichTextBox)item).Text;
            //        }
            //    }
            //    info.tcm_info = tcm_info.TrimEnd(',');
            //    info.next_visit_date = 下次随访日期6.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.visit_doctor = 随访医生签名6.Text.Trim();
            //    infolist.Add(info);
            //}
            //if (月龄12.Checked)
            //{
            //    children_tcm_record info = new children_tcm_record();
            //    info.name = Names;
            //    info.aichive_no = aichive_no;
            //    info.id_number = id_number;
            //    info.age = "12";
            //    info.visit_date = 随访日期12.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    string tcm_info = string.Empty;
            //    foreach (Control item in 中医药健康管理服务12.Controls)
            //    {
            //        if (item is CheckBox)
            //        {
            //            if (((CheckBox)item).Checked)
            //            {
            //                tcm_info += ((CheckBox)item).Tag.ToString() + ",";
            //            }
            //        }
            //        else if (item is RichTextBox)
            //        {
            //            info.tcm_other = ((RichTextBox)item).Text;
            //        }
            //    }
            //    info.tcm_info = tcm_info.TrimEnd(',');
            //    info.next_visit_date = 下次随访日期12.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.visit_doctor = 随访医生签名12.Text.Trim();
            //    infolist.Add(info);
            //}
            //if (月龄18.Checked)
            //{
            //    children_tcm_record info = new children_tcm_record();
            //    info.name = Names;
            //    info.aichive_no = aichive_no;
            //    info.id_number = id_number;
            //    info.age = "18";
            //    info.visit_date = 随访日期18.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    string tcm_info = string.Empty;
            //    foreach (Control item in 中医药健康管理服务18.Controls)
            //    {
            //        if (item is CheckBox)
            //        {
            //            if (((CheckBox)item).Checked)
            //            {
            //                tcm_info += ((CheckBox)item).Tag.ToString() + ",";
            //            }
            //        }
            //        else if (item is RichTextBox)
            //        {
            //            info.tcm_other = ((RichTextBox)item).Text;
            //        }
            //    }
            //    info.tcm_info = tcm_info.TrimEnd(',');
            //    info.next_visit_date = 下次随访日期18.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.visit_doctor = 随访医生签名18.Text.Trim();
            //    infolist.Add(info);
            //}
            //if (月龄24.Checked)
            //{
            //    children_tcm_record info = new children_tcm_record();
            //    info.name = Names;
            //    info.aichive_no = aichive_no;
            //    info.id_number = id_number;
            //    info.age = "24";
            //    info.visit_date = 随访日期24.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    string tcm_info = string.Empty;
            //    foreach (Control item in 中医药健康管理服务24.Controls)
            //    {
            //        if (item is CheckBox)
            //        {
            //            if (((CheckBox)item).Checked)
            //            {
            //                tcm_info += ((CheckBox)item).Tag.ToString() + ",";
            //            }
            //        }
            //        else if (item is RichTextBox)
            //        {
            //            info.tcm_other = ((RichTextBox)item).Text;
            //        }
            //    }
            //    info.tcm_info = tcm_info.TrimEnd(',');
            //    info.next_visit_date = 下次随访日期24.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.visit_doctor = 随访医生签名24.Text.Trim();
            //    infolist.Add(info);
            //}
            //if (月龄30.Checked)
            //{
            //    children_tcm_record info = new children_tcm_record();
            //    info.name = Names;
            //    info.aichive_no = aichive_no;
            //    info.id_number = id_number;
            //    info.age = "30";
            //    info.visit_date = 随访日期30.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    string tcm_info = string.Empty;
            //    foreach (Control item in 中医药健康管理服务30.Controls)
            //    {
            //        if (item is CheckBox)
            //        {
            //            if (((CheckBox)item).Checked)
            //            {
            //                tcm_info += ((CheckBox)item).Tag.ToString() + ",";
            //            }
            //        }
            //        else if (item is RichTextBox)
            //        {
            //            info.tcm_other = ((RichTextBox)item).Text;
            //        }
            //    }
            //    info.tcm_info = tcm_info.TrimEnd(',');
            //    info.next_visit_date = 下次随访日期30.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.visit_doctor = 随访医生签名30.Text.Trim();
            //    infolist.Add(info);
            //}
            //if (月龄36.Checked)
            //{
            //    children_tcm_record info = new children_tcm_record();
            //    info.name = Names;
            //    info.aichive_no = aichive_no;
            //    info.id_number = id_number;
            //    info.age = "36";
            //    info.visit_date = 随访日期36.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    string tcm_info = string.Empty;
            //    foreach (Control item in 中医药健康管理服务36.Controls)
            //    {
            //        if (item is CheckBox)
            //        {
            //            if (((CheckBox)item).Checked)
            //            {
            //                tcm_info += ((CheckBox)item).Tag.ToString() + ",";
            //            }
            //        }
            //        else if (item is RichTextBox)
            //        {
            //            info.tcm_other = ((RichTextBox)item).Text;
            //        }
            //    }
            //    info.tcm_info = tcm_info.TrimEnd(',');
            //    info.next_visit_date = 下次随访日期36.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //    info.visit_doctor = 随访医生签名36.Text.Trim();
            //    infolist.Add(info);
            //}
            return infolist;
        }
        /// <summary>
        /// 界面赋值
        /// </summary>
        private void SetData()
        {
            string sql = $@"select * from children_tcm_record where name='{Names}' and aichive_no='{aichive_no}' and id_number='{id_number}'";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                List<children_tcm_record> ts = Result.ToDataList<children_tcm_record>(jb.Tables[0]);
                foreach (var dt in ts)
                {
                    Control yl = this.Controls.Find($"月龄{dt.age}", true)[0];
                    ((CheckBox)yl).Checked = true;
                    Control sfsj = Controls.Find($"随访日期{dt.age}", true)[0];
                    ((DateTimePicker)sfsj).Value = Convert.ToDateTime(dt.visit_date);
                    Control zzjtz = Controls.Find($"中医药健康管理服务{dt.age}", true)[0];
                    foreach (Control item in ((GroupBox)zzjtz).Controls)
                    {
                        if (item is CheckBox)
                        {
                            if (dt.tcm_info.IndexOf(",") >= 0)
                            {
                                string[] sys = dt.tcm_info.Split(',');
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
                                if (((CheckBox)item).Tag.ToString() == dt.tcm_info)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                        else if (item is RichTextBox)
                        {
                            ((RichTextBox)item).Text = dt.tcm_other;
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
            DataSet data = DbHelperMySQL.Query($@"select * from children_tcm_record where name='{Names}' and aichive_no='{aichive_no}' and id_number='{id_number}'");
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
        public string teething_num { get; set; }
        /// <summary>
        /// 龋齿数
        /// </summary>
        public string caries_num { get; set; }
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
        public string pneumonia_num { get; set; }
        /// <summary>
        /// 腹泻次数
        /// </summary>
        public string diarrhea_num { get; set; }
        /// <summary>
        /// 外伤次数
        /// </summary>
        public string trauma_num { get; set; }
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
