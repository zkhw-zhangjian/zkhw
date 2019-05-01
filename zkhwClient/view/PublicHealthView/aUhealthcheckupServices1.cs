using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUhealthcheckupServices1 : Form
    {
        public string id = "";
        healthCheckupDao hcd = new healthCheckupDao();
        public aUhealthcheckupServices1()
        {
            InitializeComponent();
        }
        private void aUdiabetesPatientServices_Load(object sender, EventArgs e)
        {
            this.label51.Text = "健康体检表第一页(共四页)";
            this.label51.ForeColor = Color.SkyBlue;
            label51.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label51.Left = (this.panel1.Width - this.label51.Width) / 2;
            label51.BringToFront();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();
            per.name = this.textBox1.Text.Replace(" ", "");
            per.aichive_no = this.textBox2.Text.Replace(" ", "");
            per.bar_code = this.textBox118.Text;
            per.check_date = this.dateTimePicker1.Value.ToString();
            per.doctor_name = this.textBox51.Text.Replace(" ", "");

            foreach (Control ctr in this.groupBox3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.symptom += "," + ck.Text;
                    }
                }
            }
            if (per.symptom != null && per.symptom != "")
            {
                per.symptom = per.symptom.Substring(1);
            }
            per.symptom_other = this.textBox11.Text;

            per.base_temperature = this.textBox52.Text.Replace(" ", "");
            per.base_heartbeat = this.textBox66.Text.Replace(" ", "");
            per.base_respiratory = this.textBox53.Text.Replace(" ", "");
            if (this.textBox14.Text.Replace(" ", "") != "" && !"".Equals(this.textBox14.Text.Replace(" ", "")))
            {
                per.base_blood_pressure_left_high = this.textBox14.Text.Replace(" ", "");
            }
            else {
                per.base_blood_pressure_left_high = "0";
            }
            if (this.textBox58.Text.Replace(" ", "") != "" && !"".Equals(this.textBox58.Text.Replace(" ", "")))
            {
                per.base_blood_pressure_left_low = this.textBox58.Text.Replace(" ", "");
            }
            else
            {
                per.base_blood_pressure_left_low = "0";
            }
            if (this.textBox63.Text.Replace(" ", "") != "" && !"".Equals(this.textBox63.Text.Replace(" ", "")))
            {
                per.base_blood_pressure_right_high = this.textBox63.Text.Replace(" ", "");
            }
            else
            {
                per.base_blood_pressure_right_high = "0";
            }
            if (this.textBox61.Text.Replace(" ", "") != "" && !"".Equals(this.textBox61.Text.Replace(" ", "")))
            {
                per.base_blood_pressure_right_low = this.textBox61.Text.Replace(" ", "");
            }
            else
            {
                per.base_blood_pressure_right_low = "0";
            }
            per.base_height= this.textBox56.Text.Replace(" ", "");
            per.base_weight= this.textBox45.Text.Replace(" ", "");
            per.base_waist= this.textBox42.Text.Replace(" ", "");
            per.base_bmi= this.textBox67.Text.Replace(" ", "");
            
            if (this.radioButton1.Checked == true) { per.base_health_estimate = this.radioButton1.Tag.ToString(); };
            if (this.radioButton2.Checked == true) { per.base_health_estimate = this.radioButton2.Tag.ToString(); };
            if (this.radioButton3.Checked == true) { per.base_health_estimate = this.radioButton3.Tag.ToString(); };
            if (this.radioButton4.Checked == true) { per.base_health_estimate = this.radioButton4.Tag.ToString(); };
            if (this.radioButton5.Checked == true) { per.base_health_estimate = this.radioButton5.Tag.ToString(); };

            if (this.radioButton8.Checked == true) { per.base_selfcare_estimate = this.radioButton8.Tag.ToString(); };
            if (this.radioButton9.Checked == true) { per.base_selfcare_estimate = this.radioButton9.Tag.ToString(); };
            if (this.radioButton16.Checked == true) { per.base_selfcare_estimate = this.radioButton16.Tag.ToString(); };
            if (this.radioButton7.Checked == true) { per.base_selfcare_estimate = this.radioButton7.Tag.ToString(); };

            if (this.radioButton17.Checked == true) { per.base_cognition_estimate = this.radioButton17.Tag.ToString(); };
            if (this.radioButton18.Checked == true) {
                per.base_cognition_estimate = this.radioButton18.Tag.ToString();
                per.base_cognition_score= this.textBox72.Text.Replace(" ", "");
            };

            if (this.radioButton6.Checked == true) { per.base_feeling_estimate = this.radioButton6.Tag.ToString(); };
            if (this.radioButton10.Checked == true) {
                per.base_feeling_estimate = this.radioButton10.Tag.ToString();
                per.base_feeling_score = this.textBox20.Text.Replace(" ", "");
            };

            if (this.radioButton24.Checked == true) { per.lifeway_exercise_frequency = this.radioButton24.Tag.ToString(); };
            if (this.radioButton25.Checked == true) { per.lifeway_exercise_frequency = this.radioButton25.Tag.ToString(); };
            if (this.radioButton26.Checked == true) { per.lifeway_exercise_frequency = this.radioButton26.Tag.ToString(); };
            if (this.radioButton23.Checked == true) { per.lifeway_exercise_frequency = this.radioButton23.Tag.ToString(); };
           
            per.lifeway_exercise_time = this.textBox80.Text.Replace(" ", "");
            per.lifeway_exercise_year = this.textBox75.Text.Replace(" ", "");
            per.lifeway_exercise_type = this.textBox77.Text.Replace(" ", "");

            foreach (Control ctr in this.panel11.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.lifeway_diet += "," + ck.Text;
                    }
                }
            }
            if (per.lifeway_diet != null && per.lifeway_diet != "")
            {
                per.lifeway_diet = per.lifeway_diet.Substring(1);
            }

            if (this.radioButton35.Checked == true) { per.lifeway_smoke_status = this.radioButton35.Tag.ToString(); };
            if (this.radioButton36.Checked == true) { per.lifeway_smoke_status = this.radioButton36.Tag.ToString(); };
            if (this.radioButton37.Checked == true) { per.lifeway_smoke_status = this.radioButton37.Tag.ToString(); };

            per.lifeway_smoke_number = this.textBox29.Text.Replace(" ", "");
            per.lifeway_smoke_startage = this.textBox39.Text.Replace(" ", "");
            per.lifeway_smoke_endage = this.textBox49.Text.Replace(" ", "");

            if (this.radioButton12.Checked == true) { per.lifeway_drink_status = this.radioButton12.Tag.ToString(); };
            if (this.radioButton13.Checked == true) { per.lifeway_drink_status = this.radioButton13.Tag.ToString(); };
            if (this.radioButton14.Checked == true) { per.lifeway_drink_status = this.radioButton14.Tag.ToString(); };
            if (this.radioButton11.Checked == true) { per.lifeway_drink_status = this.radioButton11.Tag.ToString(); };

            per.lifeway_drink_number = this.textBox25.Text.Replace(" ", "");
            if (this.radioButton19.Checked == true) { per.lifeway_drink_stop = this.radioButton19.Tag.ToString(); };
            if (this.radioButton20.Checked == true)
            {
                per.lifeway_drink_stop = this.radioButton20.Tag.ToString();
                per.lifeway_drink_stopage = this.textBox76.Text.Replace(" ", "");
            };
            per.lifeway_drink_startage = this.textBox85.Text.Replace(" ", "");
            if (this.radioButton15.Checked == true) { per.lifeway_drink_oneyear = this.radioButton15.Tag.ToString(); };
            if (this.radioButton21.Checked == true) { per.lifeway_drink_oneyear = this.radioButton21.Tag.ToString(); };

            foreach (Control ctr in this.panel13.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.lifeway_drink_type += "," + ck.Text;
                    }
                }
            }

            if (per.lifeway_drink_type != null && per.lifeway_drink_type != "")
            {
                per.lifeway_drink_type = per.lifeway_drink_type.Substring(1);
            }
            per.lifeway_drink_other = this.textBox117.Text.Replace(" ", "");

            if (this.radioButton40.Checked == true) { per.lifeway_occupational_disease = this.radioButton40.Tag.ToString(); };
            if (this.radioButton41.Checked == true)
            {
                per.lifeway_occupational_disease = this.radioButton41.Tag.ToString();
                per.lifeway_job = this.textBox92.Text.Replace(" ", "");
                per.lifeway_job_period = this.textBox95.Text.Replace(" ", "");
                per.lifeway_hazardous_dust = this.textBox98.Text.Replace(" ", "");
                if (this.radioButton42.Checked == true)
                {
                    per.lifeway_dust_preventive = this.radioButton42.Tag.ToString();
                }
                else if(this.radioButton43.Checked == true)
                {
                    per.lifeway_dust_preventive = this.textBox100.Text.Replace(" ", ""); ;
                }

                per.lifeway_hazardous_radiation = this.textBox103.Text.Replace(" ", "");
                if (this.radioButton44.Checked == true)
                {
                    per.lifeway_radiation_preventive = this.radioButton44.Tag.ToString();
                }
                else if (this.radioButton45.Checked == true)
                {
                    per.lifeway_radiation_preventive = this.textBox101.Text.Replace(" ", ""); ;
                }

                per.lifeway_hazardous_physical = this.textBox115.Text.Replace(" ", "");
                if (this.radioButton50.Checked == true)
                {
                    per.lifeway_physical_preventive = this.radioButton50.Tag.ToString();
                }
                else if (this.radioButton51.Checked == true)
                {
                    per.lifeway_physical_preventive = this.textBox113.Text.Replace(" ", ""); ;
                }

                per.lifeway_hazardous_chemical = this.textBox107.Text.Replace(" ", "");
                if (this.radioButton46.Checked == true)
                {
                    per.lifeway_chemical_preventive = this.radioButton46.Tag.ToString();
                }
                else if (this.radioButton47.Checked == true)
                {
                    per.lifeway_chemical_preventive = this.textBox105.Text.Replace(" ", ""); ;
                }

                per.lifeway_hazardous_other = this.textBox111.Text.Replace(" ", "");
                if (this.radioButton48.Checked == true)
                {
                    per.lifeway_other_preventive = this.radioButton48.Tag.ToString();
                }
                else if (this.radioButton49.Checked == true)
                {
                    per.lifeway_other_preventive = this.textBox109.Text.Replace(" ", ""); ;
                }
            };

            //以下页面未用 数据库字段格式要求
            bool isfalse = hcd.addPhysicalExaminationRecord1(per);
            if (isfalse)
            {
                //this.DialogResult = DialogResult.OK;
                this.Close();
                aUhealthcheckupServices2 auhc2 = new aUhealthcheckupServices2();
                auhc2.textBox95.Text = per.aichive_no;
                auhc2.textBox96.Text = per.bar_code;
                auhc2.textBox99.Text = this.textBox119.Text;
                auhc2.Show();
            }
            else {
                MessageBox.Show("保存不成功!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            more0 hm = new more0();
            foreach (Control ctr in hm.panel1.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    //if (this.richTextBox2.Text.IndexOf(ck.Text) > -1)
                    //{
                    //    ck.Checked = true;
                    //}
                }
            }
            if (hm.ShowDialog() == DialogResult.OK)
            {
                
            }
        }
    }
}
