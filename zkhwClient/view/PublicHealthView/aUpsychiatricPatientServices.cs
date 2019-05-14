using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class aUpsychiatricPatientServices : Form
    {

        service.psychiatricPatientService psychiatricPatient = new service.psychiatricPatientService();
        public string id = "";
        public aUpsychiatricPatientServices()
        {
            InitializeComponent();
        }
        private void aUHypertensionPatientServices_Load(object sender, EventArgs e)
        {
            //查询赋值
            if (id != "")
            {
                DataTable dt = psychiatricPatient.queryPsychosis_info(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.textBox1.Text = dt.Rows[0]["guardian_name"].ToString();
                    this.textBox2.Text = dt.Rows[0]["guardian_phone"].ToString();
                    this.textBox5.Text = dt.Rows[0]["guardian_address"].ToString();
                    this.textBox8.Text = dt.Rows[0]["neighborhood_committee_linkman"].ToString();
                    this.textBox11.Text = dt.Rows[0]["neighborhood_committee_linktel"].ToString();
                    if (this.radioButton1.Text == dt.Rows[0]["resident_type"].ToString()) { this.radioButton1.Checked = true; };
                    if (this.radioButton2.Text == dt.Rows[0]["resident_type"].ToString()) { this.radioButton2.Checked = true; };

                    if (this.radioButton3.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton3.Checked = true; };
                    if (this.radioButton4.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton4.Checked = true; };
                    if (this.radioButton5.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton5.Checked = true; };
                    if (this.radioButton6.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton6.Checked = true; };
                    if (this.radioButton7.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton7.Checked = true; };
                    if (this.radioButton8.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton8.Checked = true; };
                    if (this.radioButton9.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton9.Checked = true; };
                    if (this.radioButton10.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton10.Checked = true; };
                    if (this.radioButton11.Text == dt.Rows[0]["employment_status"].ToString()) { this.radioButton11.Checked = true; };

                    if (this.radioButton12.Text == dt.Rows[0]["agree_manage"].ToString()) { this.radioButton12.Checked = true; };
                    if (this.radioButton13.Text == dt.Rows[0]["agree_manage"].ToString()) { this.radioButton13.Checked = true; };

                    this.textBox17.Text = dt.Rows[0]["agree_name"].ToString();
                    this.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["agree_date"].ToString());
                    this.dateTimePicker3.Value= DateTime.Parse(dt.Rows[0]["first_morbidity_date"].ToString());

                    foreach (Control ctr in this.panel5.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["symptom"].ToString().IndexOf(ck.Text.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }
                    foreach (Control ctr in this.panel6.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["isolation"].ToString().IndexOf(ck.Text.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }
                    foreach (Control ctr in this.panel7.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["outpatient"].ToString().IndexOf(ck.Text.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }

                    this.dateTimePicker4.Value = DateTime.Parse(dt.Rows[0]["first_medicine_date"].ToString());
                    this.numericUpDown1.Value =  Int32.Parse(dt.Rows[0]["hospitalized_num"].ToString());
                    this.textBox26.Text = dt.Rows[0]["diagnosis"].ToString();
                    this.textBox28.Text = dt.Rows[0]["diagnosis_hospital"].ToString();
                    this.dateTimePicker5.Value = DateTime.Parse(dt.Rows[0]["diagnosis_date"].ToString());
                    if (this.radioButton20.Text == dt.Rows[0]["recently_treatment_effect"].ToString()) { this.radioButton20.Checked = true; };
                    if (this.radioButton21.Text == dt.Rows[0]["recently_treatment_effect"].ToString()) { this.radioButton21.Checked = true; };
                    if (this.radioButton22.Text == dt.Rows[0]["recently_treatment_effect"].ToString()) { this.radioButton22.Checked = true; };

                    if (this.radioButton23.Text == dt.Rows[0]["dangerous_act"].ToString()) { this.radioButton23.Checked = true; };
                    if (this.radioButton24.Text == dt.Rows[0]["dangerous_act"].ToString()) { this.radioButton24.Checked = true; };

                    if (this.radioButton25.Text == dt.Rows[0]["economics"].ToString()) { this.radioButton25.Checked = true; };
                    if (this.radioButton26.Text == dt.Rows[0]["economics"].ToString()) { this.radioButton26.Checked = true; };

                    this.textBox34.Text = dt.Rows[0]["specialist_suggestion"].ToString();
                    this.dateTimePicker2.Value = DateTime.Parse(dt.Rows[0]["record_date"].ToString());
                    this.textBox7.Text = dt.Rows[0]["record_doctor"].ToString();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bean.psychosis_infoBean psychosis_infoBean = new bean.psychosis_infoBean();

            psychosis_infoBean.name = this.textBox37.Text.Replace(" ", "");
            psychosis_infoBean.archive_no = this.textBox39.Text.Replace(" ", "");
            psychosis_infoBean.id_number = this.textBox41.Text.Replace(" ", "");

            psychosis_infoBean.guardian_name = this.textBox1.Text.Replace(" ", ""); 
            psychosis_infoBean.guardian_phone = this.textBox2.Text.Replace(" ", "");
            psychosis_infoBean.guardian_address = this.textBox5.Text.Replace(" ", "");
            psychosis_infoBean.neighborhood_committee_linkman = this.textBox8.Text.Replace(" ", "");
            psychosis_infoBean.neighborhood_committee_linktel = this.textBox11.Text.Replace(" ", "");
            if (this.radioButton1.Checked == true) { psychosis_infoBean.resident_type = this.radioButton1.Text; };
            if (this.radioButton2.Checked == true) { psychosis_infoBean.resident_type = this.radioButton2.Text; };

            if (this.radioButton3.Checked == true) { psychosis_infoBean.employment_status = this.radioButton3.Text; };
            if (this.radioButton4.Checked == true) { psychosis_infoBean.employment_status = this.radioButton4.Text; };
            if (this.radioButton5.Checked == true) { psychosis_infoBean.employment_status = this.radioButton5.Text; };
            if (this.radioButton6.Checked == true) { psychosis_infoBean.employment_status = this.radioButton6.Text; };
            if (this.radioButton7.Checked == true) { psychosis_infoBean.employment_status = this.radioButton7.Text; };
            if (this.radioButton8.Checked == true) { psychosis_infoBean.employment_status = this.radioButton8.Text; };
            if (this.radioButton9.Checked == true) { psychosis_infoBean.employment_status = this.radioButton9.Text; };
            if (this.radioButton10.Checked == true) { psychosis_infoBean.employment_status = this.radioButton10.Text; };
            if (this.radioButton11.Checked == true) { psychosis_infoBean.employment_status = this.radioButton11.Text; };

            if (this.radioButton12.Checked == true) { psychosis_infoBean.agree_manage = this.radioButton12.Text; };
            if (this.radioButton13.Checked == true) { psychosis_infoBean.agree_manage = this.radioButton13.Text; };

            psychosis_infoBean.agree_name = this.textBox17.Text.Replace(" ", "");
            psychosis_infoBean.agree_date = this.dateTimePicker1.Text.ToString();
            psychosis_infoBean.first_morbidity_date = this.dateTimePicker3.Text.ToString();

            foreach (Control ctr in this.panel5.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        psychosis_infoBean.symptom += "," + ck.Text;
                    }
                }
            }
            if (psychosis_infoBean.symptom != null && psychosis_infoBean.symptom != "")
            {
                psychosis_infoBean.symptom = psychosis_infoBean.symptom.Substring(1);
            }


            foreach (Control ctr in this.panel6.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        psychosis_infoBean.isolation += "," + ck.Text;
                    }
                }
            }
            if (psychosis_infoBean.isolation != null && psychosis_infoBean.isolation != "")
            {
                psychosis_infoBean.isolation = psychosis_infoBean.isolation.Substring(1);
            }


            foreach (Control ctr in this.panel7.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        psychosis_infoBean.outpatient += "," + ck.Text;
                    }
                }
            }
            if (psychosis_infoBean.outpatient != null && psychosis_infoBean.outpatient != "")
            {
                psychosis_infoBean.outpatient = psychosis_infoBean.outpatient.Substring(1);
            }

            psychosis_infoBean.first_medicine_date = this.dateTimePicker4.Text.ToString();
            psychosis_infoBean.hospitalized_num = this.numericUpDown1.Value.ToString();
            psychosis_infoBean.diagnosis = this.textBox26.Text.Replace(" ", "");
            psychosis_infoBean.diagnosis_hospital = this.textBox28.Text.Replace(" ", "");
            psychosis_infoBean.diagnosis_date = this.dateTimePicker5.Text.ToString();
            if (this.radioButton20.Checked == true) { psychosis_infoBean.recently_treatment_effect = this.radioButton20.Text; };
            if (this.radioButton21.Checked == true) { psychosis_infoBean.recently_treatment_effect = this.radioButton21.Text; };
            if (this.radioButton22.Checked == true) { psychosis_infoBean.recently_treatment_effect = this.radioButton22.Text; };

            if (this.radioButton23.Checked == true) { psychosis_infoBean.dangerous_act = this.radioButton23.Text; };
            if (this.radioButton24.Checked == true) { psychosis_infoBean.dangerous_act = this.radioButton24.Text; };

            if (this.radioButton25.Checked == true) { psychosis_infoBean.economics = this.radioButton25.Text; };
            if (this.radioButton26.Checked == true) { psychosis_infoBean.economics = this.radioButton26.Text; };

            psychosis_infoBean.specialist_suggestion = this.textBox34.Text.Replace(" ", "");
            psychosis_infoBean.record_date = this.dateTimePicker2.Text.ToString();
            psychosis_infoBean.record_doctor = this.textBox7.Text.Replace(" ", "");




            ////以下页面未用 数据库字段格式要求
            //,,,,,,
            psychosis_infoBean.slight_trouble_num = "0";
            psychosis_infoBean.cause_trouble_num = "0";
            psychosis_infoBean.cause_accident_num = "0";
            psychosis_infoBean.harm_other_num = "0";
            psychosis_infoBean.autolesion_num = "0";
            psychosis_infoBean.attempted_suicide_num = "0";

            psychosis_infoBean.upload_status = "0";

            psychosis_infoBean.create_time = DateTime.Now.ToString("yyyy-MM-dd");
            psychosis_infoBean.update_time = DateTime.Now.ToString("yyyy-MM-dd");
            psychosis_infoBean.upload_time = DateTime.Now.ToString("yyyy-MM-dd");


            bool isfalse = psychiatricPatient.aUpsychosis_info(psychosis_infoBean, id);
            if (isfalse)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
