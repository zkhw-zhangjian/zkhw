﻿using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUhealthcheckupServices1 : Form
    {
        private string _laonianrenshenghuozili = "n";
        private string _barCode = "";
        public string id = "";
        healthCheckupDao hcd = new healthCheckupDao();
        public aUhealthcheckupServices1()
        {
            InitializeComponent();
        }
        private void SetelderlySelfcareEstimate(int score)
        {
            if (score >= 0 && score <= 3)
            {
                this.radioButton8.Checked = true;
            }
            else if (score >= 4 && score <= 8)
            {
                this.radioButton9.Checked = true;
            }
            else if (score >= 9 && score <= 18)
            {
                this.radioButton16.Checked = true;
            }
            else if (score >= 19)
            {
                this.radioButton7.Checked = true;
            }
        }
        private void aUdiabetesPatientServices_Load(object sender, EventArgs e)
        {
            this.label51.Text = "健康体检表第一页(共四页)";
            this.label51.BackColor = Color.SkyBlue;
            label51.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label51.Left = (this.panel1.Width - this.label51.Width) / 2;
            label51.BringToFront();

            //DataTable dtese= hcd.queryelderlySelfcareEstimate(this.textBox2.Text); queryelderlySelfcareEstimateForExamID
            DataTable dtese = hcd.queryelderlySelfcareEstimateForExamID(id); 
            if (dtese.Rows.Count>0) {
                _laonianrenshenghuozili = "y";
                int score = Int32.Parse( dtese.Rows[0]["total_score"].ToString());
                SetelderlySelfcareEstimate(score);
            } 
            else
            {
                _laonianrenshenghuozili = "n";
            }
            //查询赋值
            if (id != "")
            {
                DataTable dt = hcd.queryhealthCheckupAndAge(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region 年龄小于65老年人信息哪里不能用
                    string age = dt.Rows[0]["age"].ToString();
                    if (age == "") age = "0";
                    if(int.Parse(age)<65)
                    {
                        panel2.Enabled = false;
                        panel3.Enabled = false;
                        panel4.Enabled = false;
                        panel6.Enabled = false;
                    }
                    #endregion
                    this.textBox1.Text = dt.Rows[0]["name"].ToString(); 
                    this.textBox2.Text = dt.Rows[0]["aichive_no"].ToString();
                    this.textBox118.Text = dt.Rows[0]["bar_code"].ToString();
                    this.textBox119.Text = dt.Rows[0]["id_number"].ToString();
                    this.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["check_date"].ToString());
                    this.textBox51.Text = dt.Rows[0]["doctor_name"].ToString();
                    this.textBox120.Text = dt.Rows[0]["id"].ToString();
                    this.checkBox21.Checked = false;

                    _barCode= dt.Rows[0]["bar_code"].ToString();

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
                        this.textBox5.BackColor = Color.Salmon;
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
                        if (symptom!="1") { this.textBox5.BackColor = Color.Salmon; }
                    }
                          
                    this.textBox11.Text = dt.Rows[0]["symptom_other"].ToString();

                    string temperature = dt.Rows[0]["base_temperature"].ToString();
                    this.numericUpDown1.Text = dt.Rows[0]["base_temperature"].ToString();
                    if (temperature != null && !"".Equals(temperature))
                    {
                        double temdouble = Convert.ToDouble(temperature);
                        if (temdouble<36||temdouble > 37.5)
                        {
                            this.textBox12.BackColor = Color.Salmon;
                        }
                    }
                    string base_heartbeat = dt.Rows[0]["base_heartbeat"].ToString();
                    this.textBox66.Text = dt.Rows[0]["base_heartbeat"].ToString();
                    if (base_heartbeat != null && !"".Equals(base_heartbeat))
                    {
                        double heartbeatdouble = Convert.ToDouble(base_heartbeat);
                        if (heartbeatdouble > 100 || heartbeatdouble < 60)
                        {
                            this.textBox13.BackColor = Color.Salmon;
                        }
                    }
                    this.textBox53.Text = dt.Rows[0]["base_respiratory"].ToString().Trim();
                    string respiratory = dt.Rows[0]["base_respiratory"].ToString().Trim();
                    if (respiratory != null && !"".Equals(respiratory))
                    {
                        double respiratorydouble = Convert.ToDouble(respiratory);
                        if (respiratorydouble > 20 || respiratorydouble < 12)
                        {
                            this.textBox53.BackColor = Color.Salmon;
                        }
                    }
                    this.textBox14.Text = dt.Rows[0]["base_blood_pressure_left_high"].ToString();
                    this.textBox58.Text = dt.Rows[0]["base_blood_pressure_left_low"].ToString();
                    this.textBox63.Text = dt.Rows[0]["base_blood_pressure_right_high"].ToString();    
                    this.textBox61.Text = dt.Rows[0]["base_blood_pressure_right_low"].ToString();
                    if (!string.IsNullOrWhiteSpace(dt.Rows[0]["base_blood_pressure_left_high"].ToString()) || !string.IsNullOrWhiteSpace(dt.Rows[0]["base_blood_pressure_left_low"].ToString()))
                    {
                        if (Convert.ToInt32(dt.Rows[0]["base_blood_pressure_left_high"]) > 140 || Convert.ToInt32(dt.Rows[0]["base_blood_pressure_left_low"]) > 90)
                        {
                            this.textBox17.BackColor = Color.Salmon;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(dt.Rows[0]["base_blood_pressure_right_high"].ToString()) || !string.IsNullOrWhiteSpace(dt.Rows[0]["base_blood_pressure_right_low"].ToString()))
                    {
                        if (Convert.ToInt32(dt.Rows[0]["base_blood_pressure_right_high"]) > 140 || Convert.ToInt32(dt.Rows[0]["base_blood_pressure_right_low"]) > 90)
                        {
                            this.textBox64.BackColor = Color.Salmon;
                        }
                    }

                    this.textBox56.Text = dt.Rows[0]["base_height"].ToString();
                    this.textBox45.Text = dt.Rows[0]["base_weight"].ToString();

                    this.textBox42.Text = dt.Rows[0]["base_waist"].ToString();
                    string base_waist = dt.Rows[0]["base_waist"].ToString();
                    if (base_waist != null && !"".Equals(base_waist))
                    {
                        string sexw = this.textBox119.Text.Substring(16, 1);
                        int waistint = Int32.Parse(base_waist);
                        if (sexw == "1" && waistint > 95)
                        {
                            this.textBox43.BackColor = Color.Salmon;
                        }
                        else if (sexw == "2" && waistint > 85)
                        {
                            this.textBox43.BackColor = Color.Salmon;
                        }
                    }
                    this.textBox67.Text = dt.Rows[0]["base_bmi"].ToString();
                    string base_bmi = dt.Rows[0]["base_bmi"].ToString();
                    if (base_bmi != null && !"".Equals(base_bmi))
                    {
                        double bmidouble = Convert.ToDouble(base_bmi);
                        if (bmidouble > 24)
                        {
                            this.textBox68.BackColor = Color.Salmon;
                        }
                    }
                    if (this.radioButton1.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton1.Checked = true; };
                    if (this.radioButton2.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton2.Checked = true; };
                    if (this.radioButton3.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton3.Checked = true; this.textBox70.BackColor = Color.Salmon; };
                    if (this.radioButton4.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton4.Checked = true; this.textBox70.BackColor = Color.Salmon; };
                    if (this.radioButton5.Tag.ToString() == dt.Rows[0]["base_health_estimate"].ToString()) { this.radioButton5.Checked = true; this.textBox70.BackColor = Color.Salmon; };

                    if (this.radioButton8.Tag.ToString() == dt.Rows[0]["base_selfcare_estimate"].ToString()) { this.radioButton8.Checked = true; };
                    if (this.radioButton9.Tag.ToString() == dt.Rows[0]["base_selfcare_estimate"].ToString()) { this.radioButton9.Checked = true; this.textBox71.BackColor = Color.Salmon; };
                    if (this.radioButton16.Tag.ToString() == dt.Rows[0]["base_selfcare_estimate"].ToString()) {this.radioButton16.Checked = true; this.textBox71.BackColor = Color.Salmon; };
                    if (this.radioButton7.Tag.ToString() == dt.Rows[0]["base_selfcare_estimate"].ToString()) { this.radioButton7.Checked = true; this.textBox71.BackColor = Color.Salmon; };

                    if (this.radioButton17.Tag.ToString() == dt.Rows[0]["base_cognition_estimate"].ToString()) { this.radioButton17.Checked = true; };
                    if (this.radioButton18.Tag.ToString() == dt.Rows[0]["base_cognition_estimate"].ToString())
                    {
                        this.textBox72.Text = dt.Rows[0]["base_cognition_score"].ToString();
                        this.radioButton18.Checked = true;
                        this.textBox69.BackColor = Color.Salmon;
                    };

                    if (this.radioButton6.Tag.ToString() == dt.Rows[0]["base_feeling_estimate"].ToString()) { this.radioButton6.Checked = true; };
                    if (this.radioButton10.Tag.ToString() == dt.Rows[0]["base_feeling_estimate"].ToString())
                    {
                        this.radioButton10.Checked = true;
                        this.textBox20.Text = dt.Rows[0]["base_feeling_score"].ToString();
                        this.textBox21.BackColor = Color.Salmon;
                    };

                    if (this.radioButton24.Tag.ToString() == dt.Rows[0]["lifeway_exercise_frequency"].ToString()) { this.radioButton24.Checked = true; };
                    if (this.radioButton25.Tag.ToString() == dt.Rows[0]["lifeway_exercise_frequency"].ToString()) { this.radioButton25.Checked = true; };
                    if (this.radioButton26.Tag.ToString() == dt.Rows[0]["lifeway_exercise_frequency"].ToString()) {  this.radioButton26.Checked = true; this.textBox78.BackColor = Color.Salmon; };
                    if (this.radioButton23.Tag.ToString() == dt.Rows[0]["lifeway_exercise_frequency"].ToString()) {  this.radioButton23.Checked = true; this.textBox78.BackColor = Color.Salmon; };

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
                                if ("456".IndexOf(ck.Tag.ToString())>-1) {
                                    this.textBox38.BackColor = Color.Salmon;
                                }
                            }
                        }
                    }

                    if (this.radioButton35.Tag.ToString() == dt.Rows[0]["lifeway_smoke_status"].ToString()) { this.radioButton35.Checked = true; };
                    if (this.radioButton36.Tag.ToString() == dt.Rows[0]["lifeway_smoke_status"].ToString()) { this.radioButton36.Checked = true; };
                    if (this.radioButton37.Tag.ToString() == dt.Rows[0]["lifeway_smoke_status"].ToString()) { this.radioButton37.Checked = true; this.textBox27.BackColor = Color.Salmon; };

                    this.textBox29.Text = dt.Rows[0]["lifeway_smoke_number"].ToString();
                    this.textBox39.Text = dt.Rows[0]["lifeway_smoke_startage"].ToString();
                    this.textBox48.Text = dt.Rows[0]["lifeway_smoke_endage"].ToString();

                    if (this.radioButton12.Tag.ToString() == dt.Rows[0]["lifeway_drink_status"].ToString()) { this.radioButton12.Checked = true; this.radioButton19.Checked = false; dt.Rows[0]["lifeway_drink_stop"] = "0"; };
                    if (this.radioButton13.Tag.ToString() == dt.Rows[0]["lifeway_drink_status"].ToString()) { this.radioButton13.Checked = true; };
                    if (this.radioButton14.Tag.ToString() == dt.Rows[0]["lifeway_drink_status"].ToString()) { this.radioButton14.Checked = true; this.textBox22.BackColor = Color.Salmon; };
                    if (this.radioButton11.Tag.ToString() == dt.Rows[0]["lifeway_drink_status"].ToString()) { this.radioButton11.Checked = true; this.textBox22.BackColor = Color.Salmon; };

                    this.textBox25.Text = dt.Rows[0]["lifeway_drink_number"].ToString();
                    if (this.radioButton19.Tag.ToString() == dt.Rows[0]["lifeway_drink_stop"].ToString()) { this.radioButton19.Checked = true; this.textBox73.BackColor = Color.Salmon; };
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
                        this.textBox89.BackColor = Color.Salmon;
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
        private bean.physical_examination_recordBean GetControlValues()
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
                textBox5.Focus();
                MessageBox.Show("症状不能为空");
                return null;
            }
            per.symptom_other = this.textBox11.Text.Trim();

            per.base_temperature = this.numericUpDown1.Text;
            if (this.numericUpDown1.Text != "")
            {
                if (Common.IsNumeric(per.base_temperature) == false)
                {
                    MessageBox.Show("体温应填写数字!");
                    numericUpDown1.Focus();
                    return null; 
                }
            }
            else
            {
                per.base_temperature = "";
            }
            string tmp = this.textBox66.Text.Replace(" ", "").Trim(); 
            if (tmp != "")
            {
                if(Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("脉率应填写数字!");
                    textBox66.Focus();
                    return null;
                } 
                else
                {
                    per.base_heartbeat = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_heartbeat = "";
            }

            tmp = this.textBox53.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("呼吸应填写数字!");
                    textBox53.Focus();
                    return null;
                }
                else
                {
                    per.base_respiratory = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_respiratory = "";
            } 

            tmp = this.textBox14.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("左侧高压应填写数字!");
                    textBox14.Focus();
                    return null;
                }
                else
                {
                    per.base_blood_pressure_left_high = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_blood_pressure_left_high = "0";
            } 

            tmp = this.textBox58.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("左侧低压应填写数字!");
                    textBox58.Focus();
                    return null;
                }
                else
                {
                    per.base_blood_pressure_left_low = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_blood_pressure_left_low = "0";
            } 

            tmp = this.textBox63.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("右侧高压应填写数字!");
                    textBox63.Focus();
                    return null;
                }
                else
                {
                    per.base_blood_pressure_right_high = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_blood_pressure_right_high = "0";
            } 

            tmp = this.textBox61.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("右侧低压应填写数字!");
                    textBox61.Focus();
                    return null;
                }
                else
                {
                    per.base_blood_pressure_right_low = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_blood_pressure_right_low = "0";
            } 

            tmp = this.textBox56.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("身高应填写数字!");
                    textBox56.Focus();
                    return null;
                }
                else
                {
                    per.base_height = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_height = "";
            } 
            tmp = this.textBox45.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("体重应填写数字!");
                    textBox45.Focus();
                    return null;
                }
                else
                {
                    per.base_weight = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_weight = "";
            } 

            tmp = this.textBox42.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("腰围应填写数字!");
                    textBox42.Focus();
                    return null;
                }
                else
                {
                    per.base_waist = float.Parse(tmp).ToString();
                }
            }
            else
            {
                MessageBox.Show("腰围不能为空!");
                textBox42.Focus();
                return null;
            } 

            tmp = this.textBox67.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("体质BMI应填写数字!");
                    textBox67.Focus();
                    return null;
                }
                else
                {
                    per.base_bmi = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.base_bmi = "";
            }

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
                if (!Result.Validate(per.base_cognition_score.Trim(), @"^(-?\d+)(\.\d+)?$"))
                {
                    MessageBox.Show("老年人认知功能-简易智力状态检查分数应填写数字!");
                    textBox69.Focus();
                    return null;
                }
            };

            if (this.radioButton6.Checked == true) { per.base_feeling_estimate = this.radioButton6.Tag.ToString(); };
            if (this.radioButton10.Checked == true)
            {
                per.base_feeling_estimate = this.radioButton10.Tag.ToString();
                per.base_feeling_score = this.textBox20.Text.Replace(" ", "");
                if (!Result.Validate(per.base_feeling_score.Trim(), @"^(-?\d+)(\.\d+)?$"))
                {
                    MessageBox.Show("老年人情感状态-抑郁评分检查分数应填写数字!");
                    textBox21.Focus();
                    return null;
                }
            };

            if (this.radioButton24.Checked == true) { per.lifeway_exercise_frequency = this.radioButton24.Tag.ToString(); };
            if (this.radioButton25.Checked == true) { per.lifeway_exercise_frequency = this.radioButton25.Tag.ToString(); };
            if (this.radioButton26.Checked == true) { per.lifeway_exercise_frequency = this.radioButton26.Tag.ToString(); };
            if (this.radioButton23.Checked == true) { per.lifeway_exercise_frequency = this.radioButton23.Tag.ToString(); };

            
            tmp = this.textBox80.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("每次锻炼时间填写数字!");
                    textBox80.Focus();
                    return null;
                }
                else
                {
                    per.lifeway_exercise_time = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.lifeway_exercise_time = "";
            }
             
            tmp = this.textBox75.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("坚持锻炼时间填写数字!");
                    textBox75.Focus();
                    return null;
                }
                else
                {
                    per.lifeway_exercise_year = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.lifeway_exercise_year = "";
            }

            per.lifeway_exercise_type = this.textBox77.Text.Replace(" ", "").Trim();

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
                MessageBox.Show("饮食习惯不能为空!");
                textBox38.Focus();
                return null;
            }

            if (this.radioButton35.Checked == true) { per.lifeway_smoke_status = this.radioButton35.Tag.ToString(); };
            if (this.radioButton36.Checked == true) { per.lifeway_smoke_status = this.radioButton36.Tag.ToString(); };
            if (this.radioButton37.Checked == true) { per.lifeway_smoke_status = this.radioButton37.Tag.ToString(); };
             
            tmp = this.textBox29.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("日吸烟量填写数字!");
                    textBox29.Focus();
                    return null;
                }
                else
                {
                    per.lifeway_smoke_number = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.lifeway_smoke_number = "";
            }
             
            tmp = this.textBox39.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("开始吸烟年龄填写数字!");
                    textBox39.Focus();
                    return null;
                }
                else
                {
                    per.lifeway_smoke_startage = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.lifeway_smoke_startage = "";
            }
             
            tmp = this.textBox48.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("戒烟年龄填写数字!");
                    textBox48.Focus();
                    return null;
                }
                else
                {
                    per.lifeway_smoke_endage = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.lifeway_smoke_endage = "";
            }


            if (this.radioButton12.Checked == true) { per.lifeway_drink_status = this.radioButton12.Tag.ToString(); };
            if (this.radioButton13.Checked == true) { per.lifeway_drink_status = this.radioButton13.Tag.ToString(); };
            if (this.radioButton14.Checked == true) { per.lifeway_drink_status = this.radioButton14.Tag.ToString(); };
            if (this.radioButton11.Checked == true) { per.lifeway_drink_status = this.radioButton11.Tag.ToString(); };
             
            tmp = this.textBox25.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("日饮酒量填写数字!");
                    textBox25.Focus();
                    return null;
                }
                else
                {
                    per.lifeway_drink_number = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.lifeway_drink_number = "";
            }

            if (this.radioButton19.Checked == true) { per.lifeway_drink_stop = this.radioButton19.Tag.ToString(); };
            if (this.radioButton20.Checked == true)
            {
                per.lifeway_drink_stop = this.radioButton20.Tag.ToString();
                 
                tmp = this.textBox76.Text.Replace(" ", "").Trim();
                if (tmp != "")
                {
                    if (Common.IsNumeric(tmp) == false)
                    {
                        MessageBox.Show("戒酒年龄填写数字!");
                        textBox76.Focus();
                        return null;
                    }
                    else
                    {
                        per.lifeway_drink_stopage = float.Parse(tmp).ToString();
                    }
                }
                else
                {
                    per.lifeway_drink_stopage = "";
                }
            }; 
            tmp = this.textBox85.Text.Replace(" ", "").Trim();
            if (tmp != "")
            {
                if (Common.IsNumeric(tmp) == false)
                {
                    MessageBox.Show("开始饮酒年龄填写数字!");
                    textBox85.Focus();
                    return null;
                }
                else
                {
                    per.lifeway_drink_startage = float.Parse(tmp).ToString();
                }
            }
            else
            {
                per.lifeway_drink_startage = "";
            }

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
            return per;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = GetControlValues();
            if (per == null) return;
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
            bean.physical_examination_recordBean per = GetControlValues();
            if (per == null) return;
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
            bean.physical_examination_recordBean per = GetControlValues();
            if (per == null) return;
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

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox21.Checked) {
                this.textBox11.Text = "";
                foreach (Control ctr in this.groupBox3.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (ck.Checked && !"1".Equals(ck.Tag))
                        {
                            ck.Checked = false;
                        }

                    }
                }
            }
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox20.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox19.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox18.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox17.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox15.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox13.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox14.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox23.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox22.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox9.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox8.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox7.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox6.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox24.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox25.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox26.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox27.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox28.Checked)
            {
                this.checkBox21.Checked = false;
            }
        }

        private void textBox70_DoubleClick(object sender, EventArgs e)
        {
            this.textBox70.BackColor = Color.FromArgb(240, 240, 240);
            foreach (Control ctr in this.panel2.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is RadioButton)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    RadioButton ck = ctr as RadioButton;
                    if (ck.Checked)
                    {
                        ck.Checked = false;
                    }
                }
            }
        }

        private void textBox71_DoubleClick(object sender, EventArgs e)
        {
            this.textBox71.BackColor = Color.FromArgb(240, 240, 240);
            foreach (Control ctr in this.panel3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is RadioButton)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    RadioButton ck = ctr as RadioButton;
                    if (ck.Checked)
                    {
                        ck.Checked = false;
                    }
                }
            }
        }

        private void textBox69_DoubleClick(object sender, EventArgs e)
        {
            this.textBox69.BackColor = Color.FromArgb(240, 240, 240);
            foreach (Control ctr in this.panel4.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is RadioButton)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    RadioButton ck = ctr as RadioButton;
                    if (ck.Checked)
                    {
                        ck.Checked = false;
                    }
                }
            }
        }

        private void textBox21_DoubleClick(object sender, EventArgs e)
        {
            this.textBox21.BackColor = Color.FromArgb(240, 240, 240);
            foreach (Control ctr in this.panel6.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is RadioButton)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    RadioButton ck = ctr as RadioButton;
                    if (ck.Checked)
                    {
                        ck.Checked = false;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //1:判断是不是已经参加过评估了
            string archive_no = textBox2.Text.Trim();
            if (archive_no == "") return; 
            if (_laonianrenshenghuozili=="n")
            {
                //添加
                aUolderHelthService hm = new aUolderHelthService();
                hm.label47.Text = "老年人生活自理能力评估表";
                hm.Text = "老年人生活自理能力评估表";
                hm.textBox1.Text = textBox1.Text.Trim();
                hm.textBox2.Text = archive_no;
                hm.textBox12.Text = textBox119.Text.Trim();
                int _xb = int.Parse(textBox119.Text.Substring(16, 1));
                if (_xb % 2 == 0)
                {
                    hm.sex  = "女";
                }
                else
                {
                    hm.sex  = "男";
                } 
                hm.flag = 0;
                hm._barCode = _barCode;
                hm._examid = id;
                if (hm.ShowDialog() == DialogResult.OK)
                {
                    //成功了自动给分数处理
                    int score = (int)hm.numericUpDown6.Value;
                    SetelderlySelfcareEstimate(score); 
                }
            }
            else
            {
                service.olderHelthServices olderHelthS = new service.olderHelthServices();
                DataTable dt = olderHelthS.queryOlderHelthServiceForExamID(id);
                //修改
                aUolderHelthService hm = new aUolderHelthService();
                string name = textBox1.Text.Trim(); 
                string idnumber = textBox119.Text.Trim();
                int _xb = int.Parse(idnumber.Substring(16, 1));
                if (_xb % 2 == 0)
                {
                    hm.sex = "女";
                }
                else
                {
                    hm.sex = "男";
                }
                hm._barCode = _barCode;
                hm._examid = id;
                hm.label47.Text = "老年人生活自理能力评估表";
                hm.Text = "老年人生活自理能力评估表";
                hm.flag = 1; 
                hm.archiveno = archive_no; 
                hm.textBox1.Text = name;
                hm.textBox2.Text = archive_no;
                hm.textBox12.Text = idnumber;
                string[] ck2 = dt.Rows[0]["answer_result"].ToString().Split(',');
                hm.numericUpDown1.Value = Decimal.Parse(ck2[0]);
                hm.numericUpDown2.Value = Decimal.Parse(ck2[1]);
                hm.numericUpDown3.Value = Decimal.Parse(ck2[2]);
                hm.numericUpDown4.Value = Decimal.Parse(ck2[3]);
                hm.numericUpDown5.Value = Decimal.Parse(ck2[4]);
                hm.numericUpDown6.Value = Decimal.Parse(dt.Rows[0]["total_score"].ToString());
                if (hm.ShowDialog() == DialogResult.OK)
                {
                    //成功了自动给分数处理
                    int score = (int)hm.numericUpDown6.Value;
                    SetelderlySelfcareEstimate(score);
                }
            }
        }

        private void textBox66_KeyPress(object sender, KeyPressEventArgs e)
        {
            Common.txtBox_KeyPress(sender, e);
        }
    }
}
