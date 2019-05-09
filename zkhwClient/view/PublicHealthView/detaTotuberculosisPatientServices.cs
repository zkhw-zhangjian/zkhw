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
    public partial class detaTotuberculosisPatientServices : Form
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
        public detaTotuberculosisPatientServices(string names, string aichive_nos, string id_numbers)
        {
            Names = names;
            aichive_no = aichive_nos;
            id_number = id_numbers;
            if (GetRecord())
            {
                MessageBox.Show("没有详细信息！");
                return;
            }
            InitializeComponent();
            SetData();
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
                int a = 0;
                foreach (var dt in ts)
                {
                    a++;
                    Control sfsj = Controls.Find($"随访时间{a}", true)[0];
                    ((DateTimePicker)sfsj).Value = Convert.ToDateTime(dt.visit_date);
                    Control zlyx = Controls.Find($"治疗月序{a}", true)[0];
                    ((TextBox)sfsj).Text = dt.month_order;
                    Control ddry = Controls.Find($"督导人员{a}", true)[0];
                    foreach (Control item in ((GroupBox)ddry).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.supervisor_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control sffs = Controls.Find($"随访方式{a}", true)[0];
                    foreach (Control item in ((GroupBox)sffs).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.visit_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control zzjtz = Controls.Find($"症状及体征{a}", true)[0];
                    foreach (Control item in ((GroupBox)zzjtz).Controls)
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
                    Control xy1 = Controls.Find($"吸烟{GetZM(a)}1", true)[0];
                    ((TextBox)xy1).Text = dt.smoke_now.ToString();
                    Control xy2 = Controls.Find($"吸烟{GetZM(a)}2", true)[0];
                    ((TextBox)xy2).Text = dt.smoke_next.ToString();
                    Control yj1 = Controls.Find($"饮酒{GetZM(a)}1", true)[0];
                    ((TextBox)yj1).Text = dt.drink_now.ToString();
                    Control yj2 = Controls.Find($"饮酒{GetZM(a)}2", true)[0];
                    ((TextBox)yj2).Text = dt.drink_next.ToString();
                    Control hlfyu = Controls.Find($"化疗方案{a}", true)[0];
                    ((TextBox)hlfyu).Text = dt.chemotherapy_plan;
                    Control yf = Controls.Find($"用法{a}", true)[0];
                    foreach (Control item in ((GroupBox)yf).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.usage)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control ypjx = Controls.Find($"药品剂型{a}", true)[0];
                    foreach (Control item in ((GroupBox)ypjx).Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Tag.ToString() == dt.drugs_type)
                            {
                                ((RadioButton)item).Checked = true;
                            }
                        }
                    }
                    Control lfycs = Controls.Find($"漏服药次数{a}", true)[0];
                    ((TextBox)lfycs).Text = dt.miss;
                    Control ywblfy = Controls.Find($"药物不良反应{a}", true)[0];
                    foreach (Control item in ((GroupBox)ywblfy).Controls)
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
                            if (((TextBox)item).Name == $"药物不良反应有{a}")
                            {
                                ((TextBox)item).Text = dt.untoward_effect_info;
                            }
                        }
                    }
                    Control zzkb = Controls.Find($"转诊科别{a}", true)[0];
                    ((TextBox)zzkb).Text = dt.transfer_treatment_department;
                    Control zzyy = Controls.Find($"转诊原因{a}", true)[0];
                    ((TextBox)zzyy).Text = dt.transfer_treatment_reason;
                    Control zzjg = Controls.Find($"转诊结果{a}", true)[0];
                    ((TextBox)zzjg).Text = dt.twoweek_visit_result;
                    Control clyj = Controls.Find($"处理意见{a}", true)[0];
                    ((TextBox)clyj).Text = dt.handling_suggestion;
                    Control xcfwsj = Controls.Find($"下次随访时间{a}", true)[0];
                    ((TextBox)xcfwsj).Text = dt.next_visit_date;
                    Control sfysqm = Controls.Find($"随访医生签名{a}", true)[0];
                    ((TextBox)sfysqm).Text = dt.visit_doctor;
                    Control tzzlyy = Controls.Find($"停止治疗原因{a}", true)[0];
                    foreach (Control item in ((GroupBox)tzzlyy).Controls)
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
                                if (((CheckBox)item).Tag.ToString() == dt.symptom)
                                {
                                    ((CheckBox)item).Checked = true;
                                }
                            }
                        }
                    }
                    Control cxtzzlsj = Controls.Find($"出现停止治疗时间{a}", true)[0];
                    ((DateTimePicker)cxtzzlsj).Value = Convert.ToDateTime(dt.stop_date);
                    Control yfshzcs = Controls.Find($"应访视患者次数{a}", true)[0];
                    ((TextBox)yfshzcs).Text = dt.must_visit_num;
                    Control sjfscs = Controls.Find($"实际访视次数{a}", true)[0];
                    ((TextBox)sjfscs).Text = dt.actual_visit_num;
                    Control yfycs = Controls.Find($"应服药次数{a}", true)[0];
                    ((TextBox)yfycs).Text = dt.must_medicine_num;
                    Control sjfycs = Controls.Find($"实际服药次数{a}", true)[0];
                    ((TextBox)sjfycs).Text = dt.actual_medicine_num;
                    Control fyl = Controls.Find($"服药率{a}", true)[0];
                    ((TextBox)fyl).Text = dt.medicine_rate;
                }
            }
        }

        private string GetZM(int rot)
        {
            switch (rot)
            {
                case 1:
                    return "a";
                case 2:
                    return "b";
                case 3:
                    return "c";
                case 4:
                    return "d";
                default:
                    return null;
            }
        }

        /// <summary>
        /// 判断是否有第一次随访记录
        /// </summary>
        /// <returns></returns>
        private bool GetRecord()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from tuberculosis_follow_record where name='{Names}' and aichive_no='{aichive_no}' and Cardcode='{id_number}'");
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


}
