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
                    this.textBox42.Text = dt.Rows[0]["guardian_relation"].ToString();
                    this.textBox2.Text = dt.Rows[0]["guardian_phone"].ToString();
                    this.textBox5.Text = dt.Rows[0]["guardian_address"].ToString();
                    this.textBox8.Text = dt.Rows[0]["neighborhood_committee_linkman"].ToString();
                    this.textBox11.Text = dt.Rows[0]["neighborhood_committee_linktel"].ToString();
                    if (this.radioButton1.Tag.ToString() == dt.Rows[0]["resident_type"].ToString()) { this.radioButton1.Checked = true; };
                    if (this.radioButton2.Tag.ToString() == dt.Rows[0]["resident_type"].ToString()) { this.radioButton2.Checked = true; };

                    if (this.radioButton3.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton3.Checked = true; };
                    if (this.radioButton4.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton4.Checked = true; };
                    if (this.radioButton5.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton5.Checked = true; };
                    if (this.radioButton6.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton6.Checked = true; };
                    if (this.radioButton7.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton7.Checked = true; };
                    if (this.radioButton8.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton8.Checked = true; };
                    if (this.radioButton9.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton9.Checked = true; };
                    if (this.radioButton10.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton10.Checked = true; };
                    if (this.radioButton11.Tag.ToString() == dt.Rows[0]["employment_status"].ToString()) { this.radioButton11.Checked = true; };

                    if (this.radioButton12.Tag.ToString() == dt.Rows[0]["agree_manage"].ToString()) { this.radioButton12.Checked = true; };
                    if (this.radioButton13.Tag.ToString() == dt.Rows[0]["agree_manage"].ToString()) { this.radioButton13.Checked = true; };

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
                            if (dt.Rows[0]["symptom"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }
                    if (this.radioButton14.Tag.ToString()==dt.Rows[0]["isolation"].ToString()) { this.radioButton14.Checked = true; };
                    if (this.radioButton15.Tag.ToString() == dt.Rows[0]["isolation"].ToString()) { this.radioButton15.Checked = true; };
                    if (this.radioButton16.Tag.ToString() == dt.Rows[0]["isolation"].ToString()) { this.radioButton16.Checked = true; };

                    if (this.radioButton17.Tag.ToString() == dt.Rows[0]["outpatient"].ToString()) { this.radioButton17.Checked = true; };
                    if (this.radioButton18.Tag.ToString() == dt.Rows[0]["outpatient"].ToString()) { this.radioButton18.Checked = true; };
                    if (this.radioButton19.Tag.ToString() == dt.Rows[0]["outpatient"].ToString()) { this.radioButton19.Checked = true; };

                    this.dateTimePicker4.Value = DateTime.Parse(dt.Rows[0]["first_medicine_date"].ToString());
                    this.numericUpDown1.Value =  Int32.Parse(dt.Rows[0]["hospitalized_num"].ToString());
                    this.textBox26.Text = dt.Rows[0]["diagnosis"].ToString();
                    this.textBox28.Text = dt.Rows[0]["diagnosis_hospital"].ToString();
                    this.dateTimePicker5.Value = DateTime.Parse(dt.Rows[0]["diagnosis_date"].ToString());
                    if (this.radioButton20.Tag.ToString() == dt.Rows[0]["recently_treatment_effect"].ToString()) { this.radioButton20.Checked = true; };
                    if (this.radioButton21.Tag.ToString() == dt.Rows[0]["recently_treatment_effect"].ToString()) { this.radioButton21.Checked = true; };
                    if (this.radioButton22.Tag.ToString() == dt.Rows[0]["recently_treatment_effect"].ToString()) { this.radioButton22.Checked = true; };
                    if (this.radioButton27.Tag.ToString() == dt.Rows[0]["recently_treatment_effect"].ToString()) { this.radioButton27.Checked = true; };
                    foreach (Control ctr in this.panel10.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["dangerous_act"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }
                    
                    if (this.checkBox13.Checked)
                    {
                        this.numericUpDown2.Text = dt.Rows[0]["slight_trouble_num"].ToString();
                    }
                    if (this.checkBox14.Checked)
                    {
                        this.numericUpDown3.Text = dt.Rows[0]["cause_trouble_num"].ToString();
                    }
                    if (this.checkBox15.Checked)
                    {
                        this.numericUpDown4.Text = dt.Rows[0]["cause_accident_num"].ToString();
                    }
                    if (this.checkBox16.Checked)
                    {
                        this.numericUpDown5.Text = dt.Rows[0]["harm_other_num"].ToString();
                    }
                    if (this.checkBox17.Checked)
                    {
                        this.numericUpDown6.Text = dt.Rows[0]["autolesion_num"].ToString();
                    }
                    if (this.checkBox18.Checked)
                    {
                        this.numericUpDown7.Text = dt.Rows[0]["attempted_suicide_num"].ToString();
                    }

                    if (this.radioButton25.Tag.ToString() == dt.Rows[0]["economics"].ToString()) { this.radioButton25.Checked = true; };
                    if (this.radioButton26.Tag.ToString() == dt.Rows[0]["economics"].ToString()) { this.radioButton26.Checked = true; };

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
            psychosis_infoBean.guardian_relation = this.textBox42.Text.Replace(" ", "");
            psychosis_infoBean.guardian_phone = this.textBox2.Text.Replace(" ", "");
            psychosis_infoBean.guardian_address = this.textBox5.Text.Replace(" ", "");
            psychosis_infoBean.neighborhood_committee_linkman = this.textBox8.Text.Replace(" ", "");
            psychosis_infoBean.neighborhood_committee_linktel = this.textBox11.Text.Replace(" ", "");
            if (this.radioButton1.Checked == true) { psychosis_infoBean.resident_type = this.radioButton1.Tag.ToString(); };
            if (this.radioButton2.Checked == true) { psychosis_infoBean.resident_type = this.radioButton2.Tag.ToString(); };

            if (this.radioButton3.Checked == true) { psychosis_infoBean.employment_status = this.radioButton3.Tag.ToString(); };
            if (this.radioButton4.Checked == true) { psychosis_infoBean.employment_status = this.radioButton4.Tag.ToString(); };
            if (this.radioButton5.Checked == true) { psychosis_infoBean.employment_status = this.radioButton5.Tag.ToString(); };
            if (this.radioButton6.Checked == true) { psychosis_infoBean.employment_status = this.radioButton6.Tag.ToString(); };
            if (this.radioButton7.Checked == true) { psychosis_infoBean.employment_status = this.radioButton7.Tag.ToString(); };
            if (this.radioButton8.Checked == true) { psychosis_infoBean.employment_status = this.radioButton8.Tag.ToString(); };
            if (this.radioButton9.Checked == true) { psychosis_infoBean.employment_status = this.radioButton9.Tag.ToString(); };
            if (this.radioButton10.Checked == true) { psychosis_infoBean.employment_status = this.radioButton10.Tag.ToString(); };
            if (this.radioButton11.Checked == true) { psychosis_infoBean.employment_status = this.radioButton11.Tag.ToString(); };

            if (this.radioButton12.Checked == true) { psychosis_infoBean.agree_manage = this.radioButton12.Tag.ToString(); };
            if (this.radioButton13.Checked == true) { psychosis_infoBean.agree_manage = this.radioButton13.Tag.ToString(); };

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
                        psychosis_infoBean.symptom += "," + ck.Tag.ToString();
                    }
                }
            }
            if (psychosis_infoBean.symptom != null && psychosis_infoBean.symptom != "")
            {
                psychosis_infoBean.symptom = psychosis_infoBean.symptom.Substring(1);
            }

            if (this.radioButton14.Checked == true) { psychosis_infoBean.isolation = this.radioButton14.Tag.ToString(); };
            if (this.radioButton15.Checked == true) { psychosis_infoBean.isolation = this.radioButton15.Tag.ToString(); };
            if (this.radioButton16.Checked == true) { psychosis_infoBean.isolation = this.radioButton16.Tag.ToString(); };

            if (this.radioButton17.Checked == true) { psychosis_infoBean.outpatient = this.radioButton17.Tag.ToString(); };
            if (this.radioButton18.Checked == true) { psychosis_infoBean.outpatient = this.radioButton18.Tag.ToString(); };
            if (this.radioButton19.Checked == true) { psychosis_infoBean.outpatient = this.radioButton19.Tag.ToString(); };        
           
            psychosis_infoBean.first_medicine_date = this.dateTimePicker4.Text.ToString();
            psychosis_infoBean.hospitalized_num = this.numericUpDown1.Value.ToString();
            psychosis_infoBean.diagnosis = this.textBox26.Text.Replace(" ", "");
            psychosis_infoBean.diagnosis_hospital = this.textBox28.Text.Replace(" ", "");
            psychosis_infoBean.diagnosis_date = this.dateTimePicker5.Text.ToString();
            if (this.radioButton20.Checked == true) { psychosis_infoBean.recently_treatment_effect = this.radioButton20.Tag.ToString(); };
            if (this.radioButton21.Checked == true) { psychosis_infoBean.recently_treatment_effect = this.radioButton21.Tag.ToString(); };
            if (this.radioButton22.Checked == true) { psychosis_infoBean.recently_treatment_effect = this.radioButton22.Tag.ToString(); };
            if (this.radioButton27.Checked == true) { psychosis_infoBean.recently_treatment_effect = this.radioButton27.Tag.ToString(); };

            if (!this.checkBox19.Checked)
            {
                foreach (Control ctr in this.panel10.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (ck.Checked)
                        {
                            psychosis_infoBean.dangerous_act += "," + ck.Tag.ToString();
                        }
                    }
                }
                if (psychosis_infoBean.dangerous_act != null && psychosis_infoBean.dangerous_act != "")
                {
                    psychosis_infoBean.dangerous_act = psychosis_infoBean.dangerous_act.Substring(1);
                }
                if (this.checkBox13.Checked)
                {
                    psychosis_infoBean.slight_trouble_num = this.numericUpDown2.Text;
                }
                if (this.checkBox14.Checked)
                {
                    psychosis_infoBean.cause_trouble_num = this.numericUpDown3.Text;
                }
                if (this.checkBox15.Checked)
                {
                    psychosis_infoBean.cause_accident_num = this.numericUpDown4.Text;
                }
                if (this.checkBox16.Checked)
                {
                    psychosis_infoBean.harm_other_num = this.numericUpDown5.Text;
                }
                if (this.checkBox17.Checked)
                {
                    psychosis_infoBean.autolesion_num = this.numericUpDown6.Text;
                }
                if (this.checkBox18.Checked)
                {
                    psychosis_infoBean.attempted_suicide_num = this.numericUpDown7.Text;
                }
            }
            else {
                psychosis_infoBean.dangerous_act = "0";
            }
            if (this.radioButton25.Checked == true) { psychosis_infoBean.economics = this.radioButton25.Tag.ToString(); };
            if (this.radioButton26.Checked == true) { psychosis_infoBean.economics = this.radioButton26.Tag.ToString(); };

            psychosis_infoBean.specialist_suggestion = this.textBox34.Text.Replace(" ", "");
            psychosis_infoBean.record_date = this.dateTimePicker2.Text.ToString();
            psychosis_infoBean.record_doctor = this.textBox7.Text.Replace(" ", "");

            psychosis_infoBean.upload_status = "0";

            psychosis_infoBean.create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            psychosis_infoBean.update_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            psychosis_infoBean.upload_time = DateTime.Now.ToString("yyyy-MM-dd");

            bool isfalse = psychiatricPatient.aUpsychosis_info(psychosis_infoBean, id);
            if (isfalse)
            {
                this.DialogResult = DialogResult.OK;
            }
           
        }
    }
}
