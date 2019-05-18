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
    public partial class addEndMaternalHealthServices : Form
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
        public addEndMaternalHealthServices(int ps, string names, string aichive_nos, string id_numbers)
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
            List<gravida_after_record> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();
            DBSql sqls = null;
            foreach (gravida_after_record info in infolist)
            {
                sqls = new DBSql();
                sqls.sql = @"insert into gravida_after_record(id,name,aichive_no,id_number,order_num,visit_date,childbirth,discharge_date,temperature,general_health_status,general_psychology_status,blood_pressure_high,blood_pressure_low,breast,breast_error,lyma,lyma_error,womb,womb_error,wound,wound_error,other,`condition`,error_info,guidance,guidance_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,next_visit_date,visit_doctor,create_user,create_name,create_time,upload_status) values(@id,@name,@aichive_no,@id_number,@order_num,@visit_date,@childbirth,@discharge_date,@temperature,@general_health_status,@general_psychology_status,@blood_pressure_high,@blood_pressure_low,@breast,@breast_error,@lyma,@lyma_error,@womb,@womb_error,@wound,@wound_error,@other,@condition,@error_info,@guidance,@guidance_other,@transfer_treatment,@transfer_treatment_reason,@transfer_treatment_department,@next_visit_date,@visit_doctor,@create_user,@create_name,@create_time,@upload_status);";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@order_num", info.order_num),
                    new MySqlParameter("@childbirth", info.childbirth),
                    new MySqlParameter("@discharge_date", info.discharge_date),
                    new MySqlParameter("@temperature", info.temperature),
                    new MySqlParameter("@general_health_status", info.general_health_status),
                    new MySqlParameter("@general_psychology_status", info.general_psychology_status),
                    new MySqlParameter("@blood_pressure_high", info.blood_pressure_high),
                    new MySqlParameter("@blood_pressure_low", info.blood_pressure_low),
                    new MySqlParameter("@breast", info.breast),
                    new MySqlParameter("@breast_error", info.breast_error),
                    new MySqlParameter("@lyma", info.lyma),
                    new MySqlParameter("@lyma_error", info.lyma_error),
                    new MySqlParameter("@womb", info.womb),
                    new MySqlParameter("@womb_error", info.womb_error),
                    new MySqlParameter("@wound", info.wound),
                    new MySqlParameter("@wound_error", info.wound_error),
                    new MySqlParameter("@other", info.other),
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
            List<gravida_after_record> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();
            foreach (gravida_after_record info in infolist)
            {
                DBSql sqls = new DBSql();
                sqls.sql = @"update gravida_after_record set visit_date=@visit_date,childbirth=@childbirth,discharge_date=@discharge_date,temperature=@temperature,general_health_status=@general_health_status,general_psychology_status=@general_psychology_status,blood_pressure_high=@blood_pressure_high,blood_pressure_low=@blood_pressure_low,breast=@breast,breast_error=@breast_error,lyma=@lyma,lyma_error=@lyma_error,womb=@womb,womb_error=@womb_error,wound=@wound,wound_error=@wound_error,other=@other,`condition`=@condition,error_info=@error_info,guidance=@guidance,guidance_other=@guidance_other,transfer_treatment=@transfer_treatment,transfer_treatment_reason=@transfer_treatment_reason,transfer_treatment_department=@transfer_treatment_department,next_visit_date=@next_visit_date,visit_doctor=@visit_doctor,update_user=@update_user,update_name=@update_name,update_time=@update_time where name=@name and aichive_no=@aichive_no and id_number=@id_number and order_num=@order_num;";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@order_num", info.order_num),
                    new MySqlParameter("@childbirth", info.childbirth),
                    new MySqlParameter("@discharge_date", info.discharge_date),
                    new MySqlParameter("@temperature", info.temperature),
                    new MySqlParameter("@general_health_status", info.general_health_status),
                    new MySqlParameter("@general_psychology_status", info.general_psychology_status),
                    new MySqlParameter("@blood_pressure_high", info.blood_pressure_high),
                    new MySqlParameter("@blood_pressure_low", info.blood_pressure_low),
                    new MySqlParameter("@breast", info.breast),
                    new MySqlParameter("@breast_error", info.breast_error),
                    new MySqlParameter("@lyma", info.lyma),
                    new MySqlParameter("@lyma_error", info.lyma_error),
                    new MySqlParameter("@womb", info.womb),
                    new MySqlParameter("@womb_error", info.womb_error),
                    new MySqlParameter("@wound", info.wound),
                    new MySqlParameter("@wound_error", info.wound_error),
                    new MySqlParameter("@other", info.other),
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
        private List<gravida_after_record> GetData()
        {
            List<gravida_after_record> infolist = new List<gravida_after_record>();
            if (产后7天.Checked)
            {
                gravida_after_record info = new gravida_after_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.order_num = "7";
                info.visit_date = 随访日期7.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.childbirth = 分娩日期7.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.discharge_date = 出院日期7.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.temperature = 体温7.Text.Trim();
                info.general_health_status = 一般健康情况7.Text.Trim();
                info.general_psychology_status = 一般心理状况7.Text.Trim();
                info.blood_pressure_high =string.IsNullOrWhiteSpace(血压高7.Text.Trim()) ?0: Convert.ToInt32(血压高7.Text.Trim());
                info.blood_pressure_low = string.IsNullOrWhiteSpace(血压低7.Text.Trim()) ? 0 : Convert.ToInt32(血压低7.Text.Trim()); 
                foreach (Control item in 乳房7.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.breast_error = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 恶露7.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.lyma = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.lyma_error = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 子宫7.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.womb = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.womb_error = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 伤口7.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.wound = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.wound_error = ((TextBox)item).Text;
                    }
                }
                info.other = 其他7.Text.Trim();
                foreach (Control item in 分类7.Controls)
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
                foreach (Control item in 指导7.Controls)
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
                foreach (Control item in 转诊7.Controls)
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
                        if (((TextBox)item).Name == "转诊原因7")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别7")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                info.next_visit_date = 下次随访日期7.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名7.Text.Trim();
                infolist.Add(info);
            }
            if (产后42天.Checked)
            {
                gravida_after_record info = new gravida_after_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.order_num = "42";
                info.visit_date = 随访日期42.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.childbirth = 分娩日期42.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.discharge_date = 出院日期42.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.temperature = 体温42.Text.Trim();
                info.general_health_status = 一般健康情况42.Text.Trim();
                info.general_psychology_status = 一般心理状况42.Text.Trim();
                info.blood_pressure_high = string.IsNullOrWhiteSpace(血压高42.Text.Trim()) ? 0 : Convert.ToInt32(血压高42.Text.Trim());
                info.blood_pressure_low = string.IsNullOrWhiteSpace(血压低42.Text.Trim()) ? 0 : Convert.ToInt32(血压低42.Text.Trim());
                foreach (Control item in 乳房42.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.breast = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.breast_error = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 恶露42.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.lyma = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.lyma_error = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 子宫42.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.womb = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.womb_error = ((TextBox)item).Text;
                    }
                }
                foreach (Control item in 伤口42.Controls)
                {
                    if (item is RadioButton)
                    {
                        if (((RadioButton)item).Checked)
                        {
                            info.wound = ((RadioButton)item).Tag.ToString();
                        }
                    }
                    else if (item is TextBox)
                    {
                        info.wound_error = ((TextBox)item).Text;
                    }
                }
                info.other = 其他42.Text.Trim();
                foreach (Control item in 分类42.Controls)
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
                foreach (Control item in 指导42.Controls)
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
                foreach (Control item in 转诊42.Controls)
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
                        if (((TextBox)item).Name == "转诊原因42")
                        {
                            info.transfer_treatment_reason = ((TextBox)item).Text;
                        }
                        else if (((TextBox)item).Name == "转诊机构和科别42")
                        {
                            info.transfer_treatment_department = ((TextBox)item).Text;
                        }
                    }
                }
                info.next_visit_date = 下次随访日期42.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名42.Text.Trim();
                infolist.Add(info);
            }
           
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
    /// 产后随访记录表
    /// </summary>
    public class gravida_after_record
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
        /// 分娩日期
        /// </summary>
        public string childbirth { get; set; }
        /// <summary>
        /// 出院日期
        /// </summary>
        public string discharge_date { get; set; }
        /// <summary>
        /// 体温
        /// </summary>
        public string temperature { get; set; }
        /// <summary>
        /// 一般健康状况
        /// </summary>
        public string general_health_status { get; set; }
        /// <summary>
        /// 一般心理状况
        /// </summary>
        public string general_psychology_status { get; set; }
        /// <summary>
        /// 血压高
        /// </summary>
        public int? blood_pressure_high { get; set; }
        /// <summary>
        /// 血压低
        /// </summary>
        public int? blood_pressure_low { get; set; }
        /// <summary>
        /// 乳房是否异常
        /// </summary>
        public string breast { get; set; }
        /// <summary>
        /// 乳房异常信息
        /// </summary>
        public string breast_error { get; set; }
        /// <summary>
        /// 恶露
        /// </summary>
        public string lyma { get; set; }
        /// <summary>
        /// 恶露异常信息
        /// </summary>
        public string lyma_error { get; set; }
        /// <summary>
        /// 子宫
        /// </summary>
        public string womb { get; set; }
        /// <summary>
        /// 子宫异常信息
        /// </summary>
        public string womb_error { get; set; }
        /// <summary>
        /// 伤口
        /// </summary>
        public string wound { get; set; }
        /// <summary>
        /// 伤口异常
        /// </summary>
        public string wound_error { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string other { get; set; }
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
