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

            DataTable dtese= hcd.queryelderlySelfcareEstimate(this.textBox2.Text);
            if (dtese.Rows.Count>0) {
                int score = Int32.Parse( dtese.Rows[0]["total_score"].ToString());
                if (score >= 0 && score <= 3) {
                    this.radioButton8.Checked = true;
                }else if (score >= 4 && score <= 8)
                {
                    this.radioButton9.Checked = true;
                }else if (score >= 9 && score <= 18)
                {
                    this.radioButton16.Checked = true;
                }else if (score >=19)
                {
                    this.radioButton7.Checked = true;
                }
            }
            //查询赋值
            if (id != "")
            {
                DataTable dt = hcd.queryhealthCheckup(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.textBox1.Text = dt.Rows[0]["name"].ToString(); 
                    this.textBox2.Text = dt.Rows[0]["aichive_no"].ToString();
                    this.textBox118.Text = dt.Rows[0]["bar_code"].ToString();
                    this.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["check_date"].ToString());
                    this.textBox51.Text = dt.Rows[0]["doctor_name"].ToString();
                    this.textBox120.Text = dt.Rows[0]["id"].ToString();
                    this.checkBox21.Checked = false;
                    
                    //将ctr转换成CheckBox并赋值给ck
                    string symptom=dt.Rows[0]["symptom"].ToString();
                    if (symptom != "" && symptom.IndexOf(',') > -1)
                    {
                        string[] sym = symptom.Split(',');
                        for (int i = 0; i < sym.Length; i++)
                        {
                            foreach (Control ctr in this.groupBox3.Controls)
                            {
                                CheckBox ck = ctr as CheckBox;
                                //判断该控件是不是CheckBox
                                if (ck is CheckBox)
                                {
                                    if (ck.Tag.ToString()==sym[i]) {
                                        ck.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (symptom != ""&& symptom.Length<=2)
                    {
                        foreach (Control ctr in this.groupBox3.Controls)
                        {
                            CheckBox ck = ctr as CheckBox;
                            //判断该控件是不是CheckBox
                            if (ck is CheckBox)
                            {
                                if (ck.Tag.ToString() == symptom)
                                {
                                    ck.Checked = true;
                                }
                            }
                        }
                    }
                          
                    this.textBox11.Text = dt.Rows[0]["symptom_other"].ToString();

                    this.numericUpDown1.Text = dt.Rows[0]["base_temperature"].ToString();

                    this.textBox66.Text = dt.Rows[0]["base_heartbeat"].ToString();
                    this.textBox53.Text = dt.Rows[0]["base_respiratory"].ToString();
                    this.textBox14.Text = dt.Rows[0]["base_blood_pressure_left_high"].ToString();
                    this.textBox58.Text = dt.Rows[0]["base_blood_pressure_left_low"].ToString();
                    this.textBox63.Text = dt.Rows[0]["base_blood_pressure_right_high"].ToString();    
                    this.textBox61.Text = dt.Rows[0]["base_blood_pressure_right_low"].ToString();

                    this.textBox56.Text = dt.Rows[0]["base_height"].ToString();
                    this.textBox45.Text = dt.Rows[0]["base_weight"].ToString();

                    this.textBox42.Text = dt.Rows[0]["base_waist"].ToString();
                    this.textBox67.Text = dt.Rows[0]["base_bmi"].ToString();

                    if (this.radioButton1.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton1.Checked = true; };
                    if (this.radioButton2.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton2.Checked = true; };
                    if (this.radioButton3.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton3.Checked = true; };
                    if (this.radioButton4.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton4.Checked = true; };
                    if (this.radioButton5.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton5.Checked = true; };

                    if (this.radioButton8.Tag.ToString() == dt.Rows[0]["base_selfcare_estimate"].ToString()) { this.radioButton8.Checked = true; };
                    if (this.radioButton9.Tag.ToString() == dt.Rows[0]["base_selfcare_estimate"].ToString()) { this.radioButton9.Checked = true; };
                    if (this.radioButton16.Tag.ToString() == dt.Rows[0]["base_selfcare_estimate"].ToString()) {this.radioButton16.Checked = true; };
                    if (this.radioButton7.Tag.ToString() == dt.Rows[0]["base_selfcare_estimate"].ToString()) { this.radioButton7.Checked = true; };

                    if (this.radioButton17.Tag.ToString() == dt.Rows[0]["base_cognition_estimate"].ToString()) { this.radioButton17.Checked = true; };
                    if (this.radioButton18.Tag.ToString() == dt.Rows[0]["base_cognition_estimate"].ToString())
                    {
                        this.textBox72.Text = dt.Rows[0]["base_cognition_score"].ToString();
                        this.radioButton18.Checked = true;
                    };

                    if (this.radioButton6.Tag.ToString() == dt.Rows[0]["base_feeling_estimate"].ToString()) { this.radioButton6.Checked = true; };
                    if (this.radioButton10.Tag.ToString() == dt.Rows[0]["base_feeling_estimate"].ToString())
                    {
                        this.radioButton10.Checked = true;
                        this.textBox20.Text = dt.Rows[0]["base_feeling_score"].ToString();
                    };

                    if (this.radioButton24.Tag.ToString() == dt.Rows[0]["lifeway_exercise_frequency"].ToString()) { this.radioButton24.Checked = true; };
                    if (this.radioButton25.Tag.ToString() == dt.Rows[0]["lifeway_exercise_frequency"].ToString()) { this.radioButton25.Checked = true; };
                    if (this.radioButton26.Tag.ToString() == dt.Rows[0]["lifeway_exercise_frequency"].ToString()) {  this.radioButton26.Checked = true; };
                    if (this.radioButton23.Tag.ToString() == dt.Rows[0]["lifeway_exercise_frequency"].ToString()) {  this.radioButton23.Checked = true; };

                    this.textBox80.Text = dt.Rows[0]["lifeway_exercise_time"].ToString();
                    this.textBox75.Text = dt.Rows[0]["lifeway_exercise_year"].ToString();
                    this.textBox77.Text = dt.Rows[0]["lifeway_exercise_type"].ToString();

                    foreach (Control ctr in this.panel11.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["lifeway_diet"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }

                    if (this.radioButton35.Tag.ToString() == dt.Rows[0]["lifeway_smoke_status"].ToString()) { this.radioButton35.Checked = true; };
                    if (this.radioButton36.Tag.ToString() == dt.Rows[0]["lifeway_smoke_status"].ToString()) { this.radioButton36.Checked = true; };
                    if (this.radioButton37.Tag.ToString() == dt.Rows[0]["lifeway_smoke_status"].ToString()) { this.radioButton37.Checked = true; };

                    this.textBox29.Text = dt.Rows[0]["lifeway_smoke_number"].ToString();
                    this.textBox39.Text = dt.Rows[0]["lifeway_smoke_startage"].ToString();
                    this.textBox48.Text = dt.Rows[0]["lifeway_smoke_endage"].ToString();

                    if (this.radioButton12.Tag.ToString() == dt.Rows[0]["lifeway_drink_status"].ToString()) { this.radioButton12.Checked = true; };
                    if (this.radioButton13.Tag.ToString() == dt.Rows[0]["lifeway_drink_status"].ToString()) { this.radioButton13.Checked = true; };
                    if (this.radioButton14.Tag.ToString() == dt.Rows[0]["lifeway_drink_status"].ToString()) { this.radioButton14.Checked = true; };
                    if (this.radioButton11.Tag.ToString() == dt.Rows[0]["lifeway_drink_status"].ToString()) { this.radioButton11.Checked = true; };

                    this.textBox25.Text = dt.Rows[0]["lifeway_drink_number"].ToString();
                    if (this.radioButton19.Tag.ToString() == dt.Rows[0]["lifeway_drink_stop"].ToString()) { this.radioButton19.Checked = true; };
                    if (this.radioButton20.Tag.ToString() == dt.Rows[0]["lifeway_drink_stop"].ToString())
                    {

                        this.radioButton20.Checked = true;
                        this.textBox76.Text = dt.Rows[0]["lifeway_drink_stopage"].ToString();
                    };
                    this.textBox85.Text = dt.Rows[0]["lifeway_drink_startage"].ToString();
                    if (this.radioButton15.Tag.ToString() == dt.Rows[0]["lifeway_drink_oneyear"].ToString()) { this.radioButton15.Checked = true; };
                    if (this.radioButton21.Tag.ToString() == dt.Rows[0]["lifeway_drink_oneyear"].ToString()) { this.radioButton21.Checked = true; };

                    foreach (Control ctr in this.panel13.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["lifeway_drink_type"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }
                    this.textBox117.Text = dt.Rows[0]["lifeway_drink_other"].ToString();

                    if (this.radioButton40.Tag.ToString() == dt.Rows[0]["lifeway_occupational_disease"].ToString()) { this.radioButton40.Checked = true; };
                    if (this.radioButton41.Tag.ToString() == dt.Rows[0]["lifeway_occupational_disease"].ToString())
                    {
                        this.radioButton41.Checked = true;
                        this.textBox92.Text = dt.Rows[0]["lifeway_job"].ToString();
                        this.textBox95.Text = dt.Rows[0]["lifeway_job_period"].ToString();
                        this.textBox98.Text = dt.Rows[0]["lifeway_hazardous_dust"].ToString();
                        if (this.radioButton42.Tag.ToString() == dt.Rows[0]["lifeway_dust_preventive"].ToString())
                        {
                            this.radioButton42.Checked = true;
                        }else if ("2" == dt.Rows[0]["lifeway_dust_preventive"].ToString())
                        {
                            this.radioButton43.Checked = true;
                        }

                        this.textBox103.Text = dt.Rows[0]["lifeway_hazardous_radiation"].ToString();
                        if (this.radioButton44.Tag.ToString() == dt.Rows[0]["lifeway_radiation_preventive"].ToString())
                        {
                            this.radioButton44.Checked = true;
                        }
                        else if ("2"== dt.Rows[0]["lifeway_radiation_preventive"].ToString())
                        {
                            this.radioButton45.Checked = true;
                        }

                        this.textBox115.Text = dt.Rows[0]["lifeway_hazardous_physical"].ToString();
                        if (this.radioButton50.Tag.ToString() == dt.Rows[0]["lifeway_physical_preventive"].ToString())
                        {
                                this.radioButton50.Checked = true;
                        }
                        else if ("2" == dt.Rows[0]["lifeway_physical_preventive"].ToString())
                        {
                            this.radioButton51.Checked = true;
                        }

                        this.textBox107.Text = dt.Rows[0]["lifeway_hazardous_chemical"].ToString();
                        if (this.radioButton46.Tag.ToString() == dt.Rows[0]["lifeway_chemical_preventive"].ToString())
                        {
                                this.radioButton46.Checked = true;
                        }
                        else if ("2" == dt.Rows[0]["lifeway_chemical_preventive"].ToString())
                        {
                            this.radioButton47.Checked = true;
                        }

                        this.textBox111.Text = dt.Rows[0]["lifeway_hazardous_other"].ToString();
                        if (this.radioButton48.Tag.ToString() == dt.Rows[0]["lifeway_other_preventive"].ToString())
                        {
                                this.radioButton48.Checked = true;
                        }
                        else if ("2" == dt.Rows[0]["lifeway_other_preventive"].ToString())
                        {
                            this.radioButton49.Checked = true;
                        }
                    };
                }
            }
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
            per.id= this.textBox120.Text;
            foreach (Control ctr in this.groupBox3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.symptom += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.symptom != null && per.symptom != "")
            {
                per.symptom = per.symptom.Substring(1);
            }
            else {
                MessageBox.Show("症状不能为空");
                return;
            }
            per.symptom_other = this.textBox11.Text;

            per.base_temperature = this.numericUpDown1.Text;
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
            if (per.base_waist == "")
            {
                MessageBox.Show("腰围不能为空!");return;
            }
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
                        per.lifeway_diet += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.lifeway_diet != null && per.lifeway_diet != "")
            {
                per.lifeway_diet = per.lifeway_diet.Substring(1);
            }
            else {
                MessageBox.Show("饮食习惯不能为空!");return;
            }

            if (this.radioButton35.Checked == true) { per.lifeway_smoke_status = this.radioButton35.Tag.ToString(); };
            if (this.radioButton36.Checked == true) { per.lifeway_smoke_status = this.radioButton36.Tag.ToString(); };
            if (this.radioButton37.Checked == true) { per.lifeway_smoke_status = this.radioButton37.Tag.ToString(); };

            per.lifeway_smoke_number = this.textBox29.Text.Replace(" ", "");
            per.lifeway_smoke_startage = this.textBox39.Text.Replace(" ", "");
            per.lifeway_smoke_endage = this.textBox48.Text.Replace(" ", "");

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
                        per.lifeway_drink_type += "," + ck.Tag.ToString();
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
                    //per.lifeway_dust_preventive = this.textBox100.Text.Replace(" ", "");
                    per.lifeway_dust_preventive = this.radioButton43.Tag.ToString();
                }

                per.lifeway_hazardous_radiation = this.textBox103.Text.Replace(" ", "");
                if (this.radioButton44.Checked == true)
                {
                    per.lifeway_radiation_preventive = this.radioButton44.Tag.ToString();
                }
                else if (this.radioButton45.Checked == true)
                {
                    //per.lifeway_radiation_preventive = this.textBox101.Text.Replace(" ", "");
                    per.lifeway_radiation_preventive = this.radioButton45.Tag.ToString();
                }

                per.lifeway_hazardous_physical = this.textBox115.Text.Replace(" ", "");
                if (this.radioButton50.Checked == true)
                {
                    per.lifeway_physical_preventive = this.radioButton50.Tag.ToString();
                }
                else if (this.radioButton51.Checked == true)
                {
                    //per.lifeway_physical_preventive = this.textBox113.Text.Replace(" ", "");
                    per.lifeway_physical_preventive = this.radioButton51.Tag.ToString();
                }

                per.lifeway_hazardous_chemical = this.textBox107.Text.Replace(" ", "");
                if (this.radioButton46.Checked == true)
                {
                    per.lifeway_chemical_preventive = this.radioButton46.Tag.ToString();
                }
                else if (this.radioButton47.Checked == true)
                {
                    //per.lifeway_chemical_preventive = this.textBox105.Text.Replace(" ", "");
                    per.lifeway_chemical_preventive = this.radioButton47.Tag.ToString();
                }

                per.lifeway_hazardous_other = this.textBox111.Text.Replace(" ", "");
                if (this.radioButton48.Checked == true)
                {
                    per.lifeway_other_preventive = this.radioButton48.Tag.ToString();
                }
                else if (this.radioButton49.Checked == true)
                {
                    //per.lifeway_other_preventive = this.textBox109.Text.Replace(" ", "");
                    per.lifeway_other_preventive = this.radioButton49.Tag.ToString();
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
                auhc2.textBox100.Text = per.id;
                auhc2.id = id;//祖
                auhc2.textBox99.Text = this.textBox119.Text;
                auhc2.Show();
            }
            else {
                MessageBox.Show("保存不成功!");
            }
        }

        private void checkBox28_Click(object sender, EventArgs e)
        {
            if (this.checkBox28.Checked)
            {
                this.textBox11.Enabled = true;
            }
            else {
                this.textBox11.Enabled = false;
            }
        }

        private void radioButton17_Click(object sender, EventArgs e)
        {
            if (this.radioButton17.Checked) {
                this.textBox72.Enabled = false;
            }
        }

        private void radioButton18_Click(object sender, EventArgs e)
        {
            if (this.radioButton18.Checked)
            {
                this.textBox72.Enabled = true;
            }
        }

        private void radioButton6_Click(object sender, EventArgs e)
        {
            if (this.radioButton16.Checked)
            {
                this.textBox20.Enabled = false;
            }
        }

        private void radioButton10_Click(object sender, EventArgs e)
        {
            if (this.radioButton10.Checked)
            {
                this.textBox20.Enabled = true;
            }
        }

        private void radioButton23_Click(object sender, EventArgs e)
        {
            if (this.radioButton23.Checked)
            {
                this.textBox80.Enabled = false;
                this.textBox75.Enabled = false;
                this.textBox77.Enabled = false;
            }
        }

        private void radioButton24_Click(object sender, EventArgs e)
        {
            if (this.radioButton24.Checked)
            {
                this.textBox80.Enabled = true;
                this.textBox75.Enabled = true;
                this.textBox77.Enabled = true;
            }
        }

        private void radioButton25_Click(object sender, EventArgs e)
        {
            if (this.radioButton25.Checked)
            {
                this.textBox80.Enabled = true;
                this.textBox75.Enabled = true;
                this.textBox77.Enabled = true;
            }
        }

        private void radioButton26_Click(object sender, EventArgs e)
        {
            if (this.radioButton26.Checked)
            {
                this.textBox80.Enabled = true;
                this.textBox75.Enabled = true;
                this.textBox77.Enabled = true;
            }
        }

        private void radioButton35_Click(object sender, EventArgs e)
        {
            if (this.radioButton35.Checked)
            {
                this.textBox29.Enabled = false;
                this.textBox39.Enabled = false;
                this.textBox48.Enabled = false;
            }
        }

        private void radioButton36_Click(object sender, EventArgs e)
        {
            if (this.radioButton36.Checked)
            {
                this.textBox29.Enabled = false;
                this.textBox39.Enabled = true;
                this.textBox48.Enabled = true;
            }
        }

        private void radioButton37_Click(object sender, EventArgs e)
        {
            if (this.radioButton37.Checked)
            {
                this.textBox29.Enabled = true;
                this.textBox39.Enabled = true;
                this.textBox48.Enabled = false;
            }
        }

        private void radioButton12_Click(object sender, EventArgs e)
        {
            if (this.radioButton12.Checked)
            {
                this.textBox25.Enabled = false;
                this.textBox85.Enabled = false;
                this.panel8.Enabled = false;
                this.panel9.Enabled = false;
            }
        }

        private void radioButton13_Click(object sender, EventArgs e)
        {
            if (this.radioButton13.Checked)
            {
                this.textBox25.Enabled = true;
                this.textBox85.Enabled = true;
                this.panel8.Enabled = true;
                this.panel9.Enabled = true;
            }
        }

        private void radioButton14_Click(object sender, EventArgs e)
        {
            if (this.radioButton14.Checked)
            {
                this.textBox25.Enabled = true;
                this.textBox85.Enabled = true;
                this.panel8.Enabled = true;
                this.panel9.Enabled = true;
            }
        }

        private void radioButton11_Click(object sender, EventArgs e)
        {
            if (this.radioButton11.Checked)
            {
                this.textBox25.Enabled = true;
                this.textBox85.Enabled = true;
                this.panel8.Enabled = true;
                this.panel9.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();
            per.name = this.textBox1.Text.Replace(" ", "");
            per.aichive_no = this.textBox2.Text.Replace(" ", "");
            per.bar_code = this.textBox118.Text;
            per.check_date = this.dateTimePicker1.Value.ToString();
            per.doctor_name = this.textBox51.Text.Replace(" ", "");
            per.id = this.textBox120.Text;
            foreach (Control ctr in this.groupBox3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.symptom += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.symptom != null && per.symptom != "")
            {
                per.symptom = per.symptom.Substring(1);
            }
            else
            {
                MessageBox.Show("症状不能为空");
                return;
            }
            per.symptom_other = this.textBox11.Text;

            per.base_temperature = this.numericUpDown1.Text;
            per.base_heartbeat = this.textBox66.Text.Replace(" ", "");
            per.base_respiratory = this.textBox53.Text.Replace(" ", "");
            if (this.textBox14.Text.Replace(" ", "") != "" && !"".Equals(this.textBox14.Text.Replace(" ", "")))
            {
                per.base_blood_pressure_left_high = this.textBox14.Text.Replace(" ", "");
            }
            else
            {
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
            per.base_height = this.textBox56.Text.Replace(" ", "");
            per.base_weight = this.textBox45.Text.Replace(" ", "");
            per.base_waist = this.textBox42.Text.Replace(" ", "");
            if (per.base_waist == "")
            {
                MessageBox.Show("腰围不能为空!"); return;
            }
            per.base_bmi = this.textBox67.Text.Replace(" ", "");

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
            if (this.radioButton18.Checked == true)
            {
                per.base_cognition_estimate = this.radioButton18.Tag.ToString();
                per.base_cognition_score = this.textBox72.Text.Replace(" ", "");
            };

            if (this.radioButton6.Checked == true) { per.base_feeling_estimate = this.radioButton6.Tag.ToString(); };
            if (this.radioButton10.Checked == true)
            {
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
                        per.lifeway_diet += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.lifeway_diet != null && per.lifeway_diet != "")
            {
                per.lifeway_diet = per.lifeway_diet.Substring(1);
            }
            else
            {
                MessageBox.Show("饮食习惯不能为空!"); return;
            }

            if (this.radioButton35.Checked == true) { per.lifeway_smoke_status = this.radioButton35.Tag.ToString(); };
            if (this.radioButton36.Checked == true) { per.lifeway_smoke_status = this.radioButton36.Tag.ToString(); };
            if (this.radioButton37.Checked == true) { per.lifeway_smoke_status = this.radioButton37.Tag.ToString(); };

            per.lifeway_smoke_number = this.textBox29.Text.Replace(" ", "");
            per.lifeway_smoke_startage = this.textBox39.Text.Replace(" ", "");
            per.lifeway_smoke_endage = this.textBox48.Text.Replace(" ", "");

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
                        per.lifeway_drink_type += "," + ck.Tag.ToString();
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
                else if (this.radioButton43.Checked == true)
                {
                    //per.lifeway_dust_preventive = this.textBox100.Text.Replace(" ", "");
                    per.lifeway_dust_preventive = this.radioButton43.Tag.ToString();
                }

                per.lifeway_hazardous_radiation = this.textBox103.Text.Replace(" ", "");
                if (this.radioButton44.Checked == true)
                {
                    per.lifeway_radiation_preventive = this.radioButton44.Tag.ToString();
                }
                else if (this.radioButton45.Checked == true)
                {
                    //per.lifeway_radiation_preventive = this.textBox101.Text.Replace(" ", "");
                    per.lifeway_radiation_preventive = this.radioButton45.Tag.ToString();
                }

                per.lifeway_hazardous_physical = this.textBox115.Text.Replace(" ", "");
                if (this.radioButton50.Checked == true)
                {
                    per.lifeway_physical_preventive = this.radioButton50.Tag.ToString();
                }
                else if (this.radioButton51.Checked == true)
                {
                    //per.lifeway_physical_preventive = this.textBox113.Text.Replace(" ", "");
                    per.lifeway_physical_preventive = this.radioButton51.Tag.ToString();
                }

                per.lifeway_hazardous_chemical = this.textBox107.Text.Replace(" ", "");
                if (this.radioButton46.Checked == true)
                {
                    per.lifeway_chemical_preventive = this.radioButton46.Tag.ToString();
                }
                else if (this.radioButton47.Checked == true)
                {
                    //per.lifeway_chemical_preventive = this.textBox105.Text.Replace(" ", "");
                    per.lifeway_chemical_preventive = this.radioButton47.Tag.ToString();
                }

                per.lifeway_hazardous_other = this.textBox111.Text.Replace(" ", "");
                if (this.radioButton48.Checked == true)
                {
                    per.lifeway_other_preventive = this.radioButton48.Tag.ToString();
                }
                else if (this.radioButton49.Checked == true)
                {
                    //per.lifeway_other_preventive = this.textBox109.Text.Replace(" ", "");
                    per.lifeway_other_preventive = this.radioButton49.Tag.ToString();
                }
            };

            //以下页面未用 数据库字段格式要求
            bool isfalse = hcd.addPhysicalExaminationRecord1(per);
            if (isfalse)
            {
                //this.DialogResult = DialogResult.OK;
                this.Close();
                aUhealthcheckupServices4 auhc4 = new aUhealthcheckupServices4();
                auhc4.textBox1.Text = per.aichive_no;
                auhc4.textBox2.Text = per.bar_code;
                auhc4.textBox3.Text = this.textBox119.Text;
                auhc4.id = per.id;//祖
                auhc4.textBox4.Text = per.id;
                auhc4.Show();
            }
            else
            {
                MessageBox.Show("保存不成功!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();
            per.name = this.textBox1.Text.Replace(" ", "");
            per.aichive_no = this.textBox2.Text.Replace(" ", "");
            per.bar_code = this.textBox118.Text;
            per.check_date = this.dateTimePicker1.Value.ToString();
            per.doctor_name = this.textBox51.Text.Replace(" ", "");
            per.id = this.textBox120.Text;
            foreach (Control ctr in this.groupBox3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.symptom += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.symptom != null && per.symptom != "")
            {
                per.symptom = per.symptom.Substring(1);
            }
            else
            {
                MessageBox.Show("症状不能为空");
                return;
            }
            per.symptom_other = this.textBox11.Text;

            per.base_temperature = this.numericUpDown1.Text;
            per.base_heartbeat = this.textBox66.Text.Replace(" ", "");
            per.base_respiratory = this.textBox53.Text.Replace(" ", "");
            if (this.textBox14.Text.Replace(" ", "") != "" && !"".Equals(this.textBox14.Text.Replace(" ", "")))
            {
                per.base_blood_pressure_left_high = this.textBox14.Text.Replace(" ", "");
            }
            else
            {
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
            per.base_height = this.textBox56.Text.Replace(" ", "");
            per.base_weight = this.textBox45.Text.Replace(" ", "");
            per.base_waist = this.textBox42.Text.Replace(" ", "");
            if (per.base_waist == "")
            {
                MessageBox.Show("腰围不能为空!"); return;
            }
            per.base_bmi = this.textBox67.Text.Replace(" ", "");

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
            if (this.radioButton18.Checked == true)
            {
                per.base_cognition_estimate = this.radioButton18.Tag.ToString();
                per.base_cognition_score = this.textBox72.Text.Replace(" ", "");
            };

            if (this.radioButton6.Checked == true) { per.base_feeling_estimate = this.radioButton6.Tag.ToString(); };
            if (this.radioButton10.Checked == true)
            {
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
                        per.lifeway_diet += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.lifeway_diet != null && per.lifeway_diet != "")
            {
                per.lifeway_diet = per.lifeway_diet.Substring(1);
            }
            else
            {
                MessageBox.Show("饮食习惯不能为空!"); return;
            }

            if (this.radioButton35.Checked == true) { per.lifeway_smoke_status = this.radioButton35.Tag.ToString(); };
            if (this.radioButton36.Checked == true) { per.lifeway_smoke_status = this.radioButton36.Tag.ToString(); };
            if (this.radioButton37.Checked == true) { per.lifeway_smoke_status = this.radioButton37.Tag.ToString(); };

            per.lifeway_smoke_number = this.textBox29.Text.Replace(" ", "");
            per.lifeway_smoke_startage = this.textBox39.Text.Replace(" ", "");
            per.lifeway_smoke_endage = this.textBox48.Text.Replace(" ", "");

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
                        per.lifeway_drink_type += "," + ck.Tag.ToString();
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
                else if (this.radioButton43.Checked == true)
                {
                    //per.lifeway_dust_preventive = this.textBox100.Text.Replace(" ", "");
                    per.lifeway_dust_preventive = this.radioButton43.Tag.ToString();
                }

                per.lifeway_hazardous_radiation = this.textBox103.Text.Replace(" ", "");
                if (this.radioButton44.Checked == true)
                {
                    per.lifeway_radiation_preventive = this.radioButton44.Tag.ToString();
                }
                else if (this.radioButton45.Checked == true)
                {
                    //per.lifeway_radiation_preventive = this.textBox101.Text.Replace(" ", "");
                    per.lifeway_radiation_preventive = this.radioButton45.Tag.ToString();
                }

                per.lifeway_hazardous_physical = this.textBox115.Text.Replace(" ", "");
                if (this.radioButton50.Checked == true)
                {
                    per.lifeway_physical_preventive = this.radioButton50.Tag.ToString();
                }
                else if (this.radioButton51.Checked == true)
                {
                    //per.lifeway_physical_preventive = this.textBox113.Text.Replace(" ", "");
                    per.lifeway_physical_preventive = this.radioButton51.Tag.ToString();
                }

                per.lifeway_hazardous_chemical = this.textBox107.Text.Replace(" ", "");
                if (this.radioButton46.Checked == true)
                {
                    per.lifeway_chemical_preventive = this.radioButton46.Tag.ToString();
                }
                else if (this.radioButton47.Checked == true)
                {
                    //per.lifeway_chemical_preventive = this.textBox105.Text.Replace(" ", "");
                    per.lifeway_chemical_preventive = this.radioButton47.Tag.ToString();
                }

                per.lifeway_hazardous_other = this.textBox111.Text.Replace(" ", "");
                if (this.radioButton48.Checked == true)
                {
                    per.lifeway_other_preventive = this.radioButton48.Tag.ToString();
                }
                else if (this.radioButton49.Checked == true)
                {
                    //per.lifeway_other_preventive = this.textBox109.Text.Replace(" ", "");
                    per.lifeway_other_preventive = this.radioButton49.Tag.ToString();
                }
            };

            //以下页面未用 数据库字段格式要求
            bool isfalse = hcd.addPhysicalExaminationRecord1(per);
            if (isfalse)
            {
                //this.DialogResult = DialogResult.OK;
                this.Close();
                aUhealthcheckupServices3 auhc3 = new aUhealthcheckupServices3();
                auhc3.textBox106.Text = per.aichive_no;
                auhc3.textBox105.Text = per.bar_code;
                auhc3.textBox108.Text = per.id;
                auhc3.id = per.id;//祖
                auhc3.textBox107.Text = this.textBox119.Text;
                auhc3.Show();
            }
            else
            {
                MessageBox.Show("保存不成功!");
            }
        }
    }
}
