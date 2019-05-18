using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    public partial class addchildrenCMHealthServices : Form
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
        public addchildrenCMHealthServices(int ps, string names, string aichive_nos, string id_numbers)
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
            this.Text = (IS == 1 ? "儿童中医药健康管理服务记录添加" : "儿童中医药健康管理服务记录修改");
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
            List<children_tcm_record> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();

            foreach (children_tcm_record info in infolist)
            {
                DBSql sqls = new DBSql();
                sqls.sql = @"insert into children_tcm_record(id,name,aichive_no,id_number,age,visit_date,tcm_info,tcm_other,next_visit_date,visit_doctor,create_user,create_name,create_time,upload_status
) values(@id,@name,@aichive_no,@id_number,@age,@visit_date,@tcm_info,@tcm_other,@next_visit_date,@visit_doctor,@create_user,@create_name,@create_time,@upload_status);";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@age", info.age),
                    new MySqlParameter("@tcm_info", info.tcm_info),
                    new MySqlParameter("@tcm_other", info.tcm_other),
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
            List<children_tcm_record> infolist = GetData();
            List<DBSql> hb = new List<DBSql>();
            foreach (children_tcm_record info in infolist)
            {
                DBSql sqls = new DBSql();
                sqls.sql = @"update children_tcm_record set visit_date=@visit_date,tcm_info=@tcm_info,tcm_other=@tcm_other,next_visit_date=@next_visit_date,visit_doctor=@visit_doctor,update_user=@update_user,update_name=@update_name,update_time=@update_time where name=@name and aichive_no=@aichive_no and id_number=@id_number and age=@age;";
                sqls.parameters = new MySqlParameter[] {
                    new MySqlParameter("@id",info.id),
                    new MySqlParameter("@name", info.name),
                    new MySqlParameter("@aichive_no", info.aichive_no),
                    new MySqlParameter("@id_number", info.id_number),
                    new MySqlParameter("@visit_date", info.visit_date),
                    new MySqlParameter("@age", info.age),
                    new MySqlParameter("@tcm_info", info.tcm_info),
                    new MySqlParameter("@tcm_other", info.tcm_other),
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
        private List<children_tcm_record> GetData()
        {
            List<children_tcm_record> infolist = new List<children_tcm_record>();
            if (月龄6.Checked)
            {
                children_tcm_record info = new children_tcm_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.age = "6";
                info.visit_date = 随访日期6.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string tcm_info = string.Empty;
                foreach (Control item in 中医药健康管理服务6.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            tcm_info += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.tcm_other = ((RichTextBox)item).Text;
                    }
                }
                info.tcm_info = tcm_info.TrimEnd(',');
                info.next_visit_date = 下次随访日期6.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名6.Text.Trim();
                infolist.Add(info);
            }
            if (月龄12.Checked)
            {
                children_tcm_record info = new children_tcm_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.age = "12";
                info.visit_date = 随访日期12.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string tcm_info = string.Empty;
                foreach (Control item in 中医药健康管理服务12.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            tcm_info += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.tcm_other = ((RichTextBox)item).Text;
                    }
                }
                info.tcm_info = tcm_info.TrimEnd(',');
                info.next_visit_date = 下次随访日期12.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名12.Text.Trim();
                infolist.Add(info);
            }
            if (月龄18.Checked)
            {
                children_tcm_record info = new children_tcm_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.age = "18";
                info.visit_date = 随访日期18.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string tcm_info = string.Empty;
                foreach (Control item in 中医药健康管理服务18.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            tcm_info += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.tcm_other = ((RichTextBox)item).Text;
                    }
                }
                info.tcm_info = tcm_info.TrimEnd(',');
                info.next_visit_date = 下次随访日期18.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名18.Text.Trim();
                infolist.Add(info);
            }
            if (月龄24.Checked)
            {
                children_tcm_record info = new children_tcm_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.age = "24";
                info.visit_date = 随访日期24.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string tcm_info = string.Empty;
                foreach (Control item in 中医药健康管理服务24.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            tcm_info += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.tcm_other = ((RichTextBox)item).Text;
                    }
                }
                info.tcm_info = tcm_info.TrimEnd(',');
                info.next_visit_date = 下次随访日期24.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名24.Text.Trim();
                infolist.Add(info);
            }
            if (月龄30.Checked)
            {
                children_tcm_record info = new children_tcm_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.age = "30";
                info.visit_date = 随访日期30.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string tcm_info = string.Empty;
                foreach (Control item in 中医药健康管理服务30.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            tcm_info += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.tcm_other = ((RichTextBox)item).Text;
                    }
                }
                info.tcm_info = tcm_info.TrimEnd(',');
                info.next_visit_date = 下次随访日期30.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名30.Text.Trim();
                infolist.Add(info);
            }
            if (月龄36.Checked)
            {
                children_tcm_record info = new children_tcm_record();
                info.name = Names;
                info.aichive_no = aichive_no;
                info.id_number = id_number;
                info.age = "36";
                info.visit_date = 随访日期36.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string tcm_info = string.Empty;
                foreach (Control item in 中医药健康管理服务36.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            tcm_info += ((CheckBox)item).Tag.ToString() + ",";
                        }
                    }
                    else if (item is RichTextBox)
                    {
                        info.tcm_other = ((RichTextBox)item).Text;
                    }
                }
                info.tcm_info = tcm_info.TrimEnd(',');
                info.next_visit_date = 下次随访日期36.Value.ToString("yyyy-MM-dd HH:mm:ss");
                info.visit_doctor = 随访医生签名36.Text.Trim();
                infolist.Add(info);
            }
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
    /// 儿童中医药健康
    /// </summary>
    public class children_tcm_record
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
        /// 月龄
        /// </summary>
        public string age { get; set; }
        /// <summary>
        /// 随访日期
        /// </summary>
        public string visit_date { get; set; }
        /// <summary>
        /// 中医药健康管理服务
        /// </summary>
        public string tcm_info { get; set; }
        /// <summary>
        /// 其他服务
        /// </summary>
        public string tcm_other { get; set; }
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
