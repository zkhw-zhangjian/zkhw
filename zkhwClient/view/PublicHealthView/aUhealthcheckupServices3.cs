using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.bean;
using zkhwClient.dao;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUhealthcheckupServices3 : Form
    {
        healthCheckupDao hcd = new healthCheckupDao();
        public string id = "";
        DataTable goodsList = new DataTable();//用药记录清单表 follow_medicine_record
        public aUhealthcheckupServices3()
        {
            InitializeComponent();
        }
        private void aUdiabetesPatientServices_Load(object sender, EventArgs e)
        {
            this.label51.Text = "健康体检表第三页(共四页)";
            this.label51.BackColor = Color.SkyBlue;
            label51.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label51.Left = (this.panel1.Width - this.label51.Width) / 2;
            label51.BringToFront();
            id = this.textBox108.Text;
            //查询赋值
            if (id != "")
            {
                DataTable dt = hcd.queryhealthCheckup(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //this.textBox106.Text = dt.Rows[0]["aichive_no"].ToString();
                    //this.textBox105.Text = dt.Rows[0]["bar_code"].ToString();
                    //this.textBox108.Text = dt.Rows[0]["id"].ToString();

                    this.textBox77.Text = dt.Rows[0]["microalbuminuria"].ToString();

                    if (this.radioButton48.Tag.ToString() == dt.Rows[0]["fob"].ToString()) { this.radioButton48.Checked = true; };
                    if (this.radioButton49.Tag.ToString() == dt.Rows[0]["fob"].ToString()) { this.radioButton49.Checked = true; this.textBox75.BackColor = Color.Salmon; };
                    this.textBox90.Text = dt.Rows[0]["glycosylated_hemoglobin"].ToString();

                    if (this.radioButton46.Tag.ToString() == dt.Rows[0]["hb"].ToString()) { this.radioButton46.Checked = true; };
                    if (this.radioButton47.Tag.ToString() == dt.Rows[0]["hb"].ToString()) { this.radioButton47.Checked = true; this.textBox63.BackColor = Color.Salmon; };
                    this.textBox3.Text = dt.Rows[0]["sgft"].ToString();
                    string sgft = dt.Rows[0]["sgft"].ToString();
                    if (sgft != null && !"".Equals(sgft))
                    {
                        if (Convert.ToDouble(sgft) > 40) { this.textBox4.BackColor = Color.Salmon; }
                    }
                    this.textBox6.Text = dt.Rows[0]["ast"].ToString();
                    string ast = dt.Rows[0]["ast"].ToString();
                    if (ast != null && !"".Equals(ast))
                    {
                        if (Convert.ToDouble(ast) > 40) { this.textBox8.BackColor = Color.Salmon; }
                    }
                    this.textBox10.Text = dt.Rows[0]["albumin"].ToString();
                    string albumin = dt.Rows[0]["albumin"].ToString();
                    if (albumin != null && !"".Equals(albumin))
                    {
                        if (Convert.ToDouble(albumin) > 54 || Convert.ToDouble(albumin) < 34) { this.textBox11.BackColor = Color.Salmon; }
                    }
                    this.textBox65.Text = dt.Rows[0]["total_bilirubin"].ToString();
                    string total_bilirubin = dt.Rows[0]["total_bilirubin"].ToString();
                    if (total_bilirubin != null && !"".Equals(total_bilirubin))
                    {
                        if (Convert.ToDouble(total_bilirubin) > 20 || Convert.ToDouble(total_bilirubin) < 2) { this.textBox66.BackColor = Color.Salmon; }
                    }
                    this.textBox68.Text = dt.Rows[0]["conjugated_bilirubin"].ToString();
                    string conjugated_bilirubin = dt.Rows[0]["conjugated_bilirubin"].ToString();
                    if (conjugated_bilirubin != null && !"".Equals(conjugated_bilirubin))
                    {
                        if (Convert.ToDouble(conjugated_bilirubin) > 6.8 || Convert.ToDouble(conjugated_bilirubin) < 1.7) { this.textBox69.BackColor = Color.Salmon; }
                    }
                    this.textBox73.Text = dt.Rows[0]["scr"].ToString();
                    string scr = dt.Rows[0]["scr"].ToString();
                    if (scr != null && !"".Equals(scr))
                    {
                        if (Convert.ToDouble(scr) > 115 || Convert.ToDouble(scr) < 44) { this.textBox74.BackColor = Color.Salmon; }
                    }
                    this.textBox81.Text = dt.Rows[0]["blood_urea"].ToString();
                    string blood_urea = dt.Rows[0]["blood_urea"].ToString();
                    if (blood_urea != null && !"".Equals(blood_urea))
                    {
                        if (Convert.ToDouble(blood_urea) > 8.2 || Convert.ToDouble(blood_urea) < 1.7) { this.textBox82.BackColor = Color.Salmon; }
                    }
                    this.textBox84.Text = dt.Rows[0]["blood_k"].ToString();
                    this.textBox87.Text = dt.Rows[0]["blood_na"].ToString();
                    this.textBox13.Text = dt.Rows[0]["tc"].ToString();
                    string tc = dt.Rows[0]["tc"].ToString();
                    if (tc != null && !"".Equals(tc))
                    {
                        if (Convert.ToDouble(tc) > 5.2) { this.textBox14.BackColor = Color.Salmon; }
                    }
                    this.textBox16.Text = dt.Rows[0]["tg"].ToString();
                    string tg = dt.Rows[0]["tg"].ToString();
                    if (tg != null && !"".Equals(tg))
                    {
                        if (Convert.ToDouble(tg) > 1.7) { this.textBox17.BackColor = Color.Salmon; }
                    }
                    this.textBox19.Text = dt.Rows[0]["ldl"].ToString();
                    string ldl = dt.Rows[0]["ldl"].ToString();
                    if (ldl != null && !"".Equals(ldl))
                    {
                        if (Convert.ToDouble(ldl) > 3.9 || Convert.ToDouble(ldl) < 1.5) { this.textBox20.BackColor = Color.Salmon; }
                    }
                    this.textBox22.Text = dt.Rows[0]["hdl"].ToString();
                    string hdl = dt.Rows[0]["hdl"].ToString();
                    if (hdl != null && !"".Equals(hdl))
                    {
                        if (Convert.ToDouble(hdl) > 1.9 || Convert.ToDouble(hdl) < 0.9) { this.textBox23.BackColor = Color.Salmon; }
                    }
                    if (this.radioButton1.Tag.ToString() == dt.Rows[0]["chest_x"].ToString()) { this.radioButton1.Checked = true; };
                    if (this.radioButton2.Tag.ToString() == dt.Rows[0]["chest_x"].ToString())
                    {
                        this.radioButton2.Checked = true;
                        this.textBox25.Text = dt.Rows[0]["chestx_memo"].ToString();
                    };

                    if (this.radioButton3.Tag.ToString() == dt.Rows[0]["ultrasound_abdomen"].ToString()) { this.radioButton3.Checked = true; };
                    if (this.radioButton4.Tag.ToString() == dt.Rows[0]["ultrasound_abdomen"].ToString())
                    {
                        this.radioButton4.Checked = true;
                        this.textBox27.Text = dt.Rows[0]["ultrasound_memo"].ToString();
                        this.textBox28.BackColor = Color.Salmon;

                    };

                    if (this.radioButton5.Tag.ToString() == dt.Rows[0]["other_b"].ToString()) { this.radioButton5.Checked = true; };
                    if (this.radioButton6.Tag.ToString() == dt.Rows[0]["other_b"].ToString())
                    {
                        this.radioButton6.Checked = true;
                        this.textBox29.Text = dt.Rows[0]["otherb_memo"].ToString();
                    };

                    if (this.radioButton7.Tag.ToString() == dt.Rows[0]["cervical_smear"].ToString()) { this.radioButton7.Checked = true; };
                    if (this.radioButton8.Tag.ToString() == dt.Rows[0]["cervical_smear"].ToString())
                    {
                        this.radioButton8.Checked = true;
                        this.textBox31.Text = dt.Rows[0]["cervical_smear_memo"].ToString();
                    };
                    this.textBox34.Text = dt.Rows[0]["other"].ToString();

                    foreach (Control ctr in this.panel7.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["cerebrovascular_disease"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                                if ("23456".IndexOf(ck.Tag.ToString()) > -1) {
                                    this.textBox37.BackColor = Color.Salmon;
                                }
                            }
                        }
                    }
                    this.textBox36.Text = dt.Rows[0]["cerebrovascular_disease_other"].ToString();

                    foreach (Control ctr in this.panel8.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["kidney_disease"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                                if ("23456".IndexOf(ck.Tag.ToString()) > -1)
                                {
                                    this.textBox39.BackColor = Color.Salmon;
                                }
                            }
                        }
                    }
                    this.textBox38.Text = dt.Rows[0]["kidney_disease_other"].ToString();

                    foreach (Control ctr in this.panel9.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["heart_disease"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                                if ("234567".IndexOf(ck.Tag.ToString()) > -1)
                                {
                                    this.textBox40.BackColor = Color.Salmon;
                                }
                            }
                        }
                    }
                    this.textBox41.Text = dt.Rows[0]["heart_disease_other"].ToString();

                    foreach (Control ctr in this.panel10.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["vascular_disease"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                                if ("234".IndexOf(ck.Tag.ToString()) > -1)
                                {
                                    this.textBox43.BackColor = Color.Salmon;
                                }
                            }
                        }
                    }
                    this.textBox42.Text = dt.Rows[0]["vascular_disease_other"].ToString();

                    foreach (Control ctr in this.panel11.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["ocular_diseases"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                                if ("2345".IndexOf(ck.Tag.ToString()) > -1)
                                {
                                    this.textBox45.BackColor = Color.Salmon;
                                }
                            }
                        }
                    }
                    this.textBox44.Text = dt.Rows[0]["ocular_diseases_other"].ToString();

                    if (this.radioButton37.Tag.ToString() == dt.Rows[0]["nervous_system_disease"].ToString()) { this.radioButton37.Checked = true; };
                    if (this.radioButton38.Tag.ToString() == dt.Rows[0]["nervous_system_disease"].ToString())
                    {
                        this.radioButton38.Checked = true;
                        this.textBox46.Text = dt.Rows[0]["nervous_disease_memo"].ToString();
                        this.textBox47.BackColor = Color.Salmon;
                    };

                    if (this.radioButton39.Tag.ToString() == dt.Rows[0]["other_disease"].ToString()) { this.radioButton39.Checked = true; };
                    if (this.radioButton40.Tag.ToString() == dt.Rows[0]["other_disease"].ToString())
                    {
                        this.radioButton40.Checked = true;
                        this.textBox48.Text = dt.Rows[0]["other_disease_memo"].ToString();
                        this.textBox49.BackColor = Color.Salmon;
                    };
                }
                //加载子表
                DataTable dtZ = hcd.queryHospitalizedRecord(id);
                int j = 0;
                int k = 0;
                if (dtZ != null && dtZ.Rows.Count > 0)
                {
                    for (int i = 0; i < dtZ.Rows.Count; i++)
                    {
                        if (dtZ.Rows[i]["hospitalized_type"].ToString() == "1")
                        {
                            if (j == 0)
                            {
                                this.checkBox29.Checked = true;
                                string in_hospital_time = dtZ.Rows[i]["in_hospital_time"].ToString();
                                string leave_hospital_time = dtZ.Rows[i]["leave_hospital_time"].ToString();
                                if (in_hospital_time != "")
                                {
                                    this.dateTimePicker1.Value = DateTime.Parse(dtZ.Rows[i]["in_hospital_time"].ToString());
                                }
                                if (leave_hospital_time != "")
                                {
                                    this.dateTimePicker2.Value = DateTime.Parse(dtZ.Rows[i]["leave_hospital_time"].ToString());
                                }
                                this.textBox60.Text = dtZ.Rows[i]["reason"].ToString();
                                this.textBox62.Text = dtZ.Rows[i]["hospital_organ"].ToString();
                                this.textBox89.Text = dtZ.Rows[i]["case_code"].ToString();
                            }
                            else
                            {
                                this.checkBox30.Checked = true;
                                this.dateTimePicker4.Value = DateTime.Parse(dtZ.Rows[i]["in_hospital_time"].ToString());
                                this.dateTimePicker3.Value = DateTime.Parse(dtZ.Rows[i]["leave_hospital_time"].ToString());
                                this.textBox61.Text = dtZ.Rows[i]["reason"].ToString();
                                this.textBox70.Text = dtZ.Rows[i]["hospital_organ"].ToString();
                                this.textBox92.Text = dtZ.Rows[i]["case_code"].ToString();
                            }
                            j++;
                        }
                        else
                        {
                            if (k == 0)
                            {
                                this.checkBox32.Checked = true;
                                this.dateTimePicker8.Value = DateTime.Parse(dtZ.Rows[i]["in_hospital_time"].ToString());
                                this.dateTimePicker7.Value = DateTime.Parse(dtZ.Rows[i]["leave_hospital_time"].ToString());
                                this.textBox98.Text = dtZ.Rows[i]["reason"].ToString();
                                this.textBox96.Text = dtZ.Rows[i]["hospital_organ"].ToString();
                                this.textBox94.Text = dtZ.Rows[i]["case_code"].ToString();
                            }
                            else
                            {
                                this.checkBox31.Checked = true;
                                this.dateTimePicker6.Value = DateTime.Parse(dtZ.Rows[i]["in_hospital_time"].ToString());
                                this.dateTimePicker5.Value = DateTime.Parse(dtZ.Rows[i]["leave_hospital_time"].ToString());
                                this.textBox97.Text = dtZ.Rows[i]["reason"].ToString();
                                this.textBox95.Text = dtZ.Rows[i]["hospital_organ"].ToString();
                                this.textBox93.Text = dtZ.Rows[i]["case_code"].ToString();
                            }
                            k++;
                        }
                    }



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

            per.microalbuminuria = this.textBox77.Text;

            if (this.radioButton48.Checked == true) { per.fob = this.radioButton48.Tag.ToString(); };
            if (this.radioButton49.Checked == true) { per.fob = this.radioButton49.Tag.ToString(); };
            per.glycosylated_hemoglobin = this.textBox90.Text;

            if (this.radioButton46.Checked == true) { per.hb = this.radioButton46.Tag.ToString(); };
            if (this.radioButton47.Checked == true) { per.hb = this.radioButton47.Tag.ToString(); };

            per.sgft = this.textBox3.Text;
            per.ast = this.textBox6.Text;
            per.albumin = this.textBox10.Text;
            per.total_bilirubin = this.textBox65.Text;
            per.conjugated_bilirubin = this.textBox68.Text;
            per.scr = this.textBox73.Text;
            per.blood_urea = this.textBox81.Text;
            per.blood_k = this.textBox84.Text;
            per.blood_na = this.textBox87.Text;
            per.tc = this.textBox13.Text;
            per.tg = this.textBox16.Text;
            per.ldl = this.textBox19.Text;
            per.hdl = this.textBox22.Text;

            if (this.radioButton1.Checked == true) { per.chest_x = this.radioButton1.Tag.ToString(); };
            if (this.radioButton2.Checked == true)
            {
                per.chest_x = this.radioButton2.Tag.ToString();
                per.chestx_memo = this.textBox25.Text;
            };

            if (this.radioButton3.Checked == true) { per.ultrasound_abdomen = this.radioButton3.Tag.ToString(); };
            if (this.radioButton4.Checked == true)
            {
                per.ultrasound_abdomen = this.radioButton4.Tag.ToString();
                per.ultrasound_memo = this.textBox27.Text;
            };

            if (this.radioButton5.Checked == true) { per.other_b = this.radioButton5.Tag.ToString(); };
            if (this.radioButton6.Checked == true)
            {
                per.other_b = this.radioButton6.Tag.ToString();
                per.otherb_memo = this.textBox29.Text;
            };

            if (this.radioButton7.Checked == true) { per.cervical_smear = this.radioButton7.Tag.ToString(); };
            if (this.radioButton8.Checked == true)
            {
                per.cervical_smear = this.radioButton8.Tag.ToString();
                per.cervical_smear_memo = this.textBox31.Text;
            };
            per.other = this.textBox34.Text;

            foreach (Control ctr in this.panel7.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.cerebrovascular_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.cerebrovascular_disease != null && per.cerebrovascular_disease != "")
            {
                per.cerebrovascular_disease = per.cerebrovascular_disease.Substring(1);
                if (this.checkBox6.Checked)
                {
                    per.cerebrovascular_disease_other = this.textBox36.Text;
                }
            }

            foreach (Control ctr in this.panel8.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.kidney_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.kidney_disease != null && per.kidney_disease != "")
            {
                per.kidney_disease = per.kidney_disease.Substring(1);
                if (this.checkBox12.Checked)
                {
                    per.kidney_disease_other = this.textBox38.Text;
                }
            }

            foreach (Control ctr in this.panel9.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.heart_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.heart_disease != null && per.heart_disease != "")
            {
                per.heart_disease = per.heart_disease.Substring(1);
                if (this.checkBox19.Checked)
                {
                    per.heart_disease_other = this.textBox41.Text;
                }
            }

            foreach (Control ctr in this.panel10.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.vascular_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.vascular_disease != null && per.vascular_disease != "")
            {
                per.vascular_disease = per.vascular_disease.Substring(1);
                if (this.checkBox23.Checked)
                {
                    per.vascular_disease_other = this.textBox42.Text;
                }
            }

            foreach (Control ctr in this.panel11.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.ocular_diseases += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.ocular_diseases != null && per.ocular_diseases != "")
            {
                per.ocular_diseases = per.ocular_diseases.Substring(1);
                if (this.checkBox28.Checked)
                {
                    per.ocular_diseases_other = this.textBox44.Text;
                }
            }

            if (this.radioButton37.Checked == true) { per.nervous_system_disease = this.radioButton37.Tag.ToString(); };
            if (this.radioButton38.Checked == true)
            {
                per.nervous_system_disease = this.radioButton38.Tag.ToString();
                per.nervous_disease_memo = this.textBox46.Text;
            };

            if (this.radioButton39.Checked == true) { per.other_disease = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true)
            {
                per.other_disease = this.radioButton40.Tag.ToString();
                per.other_disease_memo = this.textBox48.Text;
            };

            string intime1 = this.dateTimePicker1.Text;
            string outtime1 = this.dateTimePicker2.Text;
            //string nowtime=DateTime.Now.ToString("yyyy-MM-dd");

            string intime2 = this.dateTimePicker4.Text;
            string outtime2 = this.dateTimePicker3.Text;

            string intime11 = this.dateTimePicker8.Text;
            string outtime11 = this.dateTimePicker7.Text;

            string intime22 = this.dateTimePicker6.Text;
            string outtime22 = this.dateTimePicker5.Text;

            per.aichive_no = this.textBox106.Text;
            per.bar_code = this.textBox105.Text;
            per.id_number = this.textBox107.Text;
            per.id = this.textBox108.Text;
            bool isfalse = hcd.addPhysicalExaminationRecord3(per);

            if (isfalse)
            {
                hospitalizedRecord hr = null;

                if (this.checkBox29.Checked)
                {
                    string text60 = this.textBox60.Text;
                    string text62 = this.textBox62.Text;
                    if (text60 == "" || text62 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "1";
                    hr.in_hospital_time = intime1;
                    hr.leave_hospital_time = outtime1;
                    hr.reason = this.textBox60.Text;
                    hr.hospital_organ = this.textBox62.Text;
                    hr.case_code = this.textBox89.Text;
                    hcd.addHospitalizedRecord(hr, "1");
                }

                if (this.checkBox30.Checked)
                {
                    string text61 = this.textBox61.Text;
                    string text70 = this.textBox70.Text;
                    if (text61 == "" || text70 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "1";
                    hr.in_hospital_time = intime2;
                    hr.leave_hospital_time = outtime2;
                    hr.reason = this.textBox61.Text;
                    hr.hospital_organ = this.textBox70.Text;
                    hr.case_code = this.textBox92.Text;
                    hcd.addHospitalizedRecord(hr, "0");
                }
                if (this.checkBox32.Checked)
                {
                    string text98 = this.textBox98.Text;
                    string text96 = this.textBox96.Text;
                    if (text98 == "" || text96 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "2";
                    hr.in_hospital_time = intime11;
                    hr.leave_hospital_time = outtime11;
                    hr.reason = this.textBox98.Text;
                    hr.hospital_organ = this.textBox96.Text;
                    hr.case_code = this.textBox94.Text;
                    hcd.addHospitalizedRecord(hr, "2");
                }
                if (this.checkBox31.Checked)
                {
                    string text97 = this.textBox97.Text;
                    string text95 = this.textBox95.Text;
                    if (text97 == "" || text95 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.exam_id = per.id;
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.hospitalized_type = "2";
                    hr.in_hospital_time = intime22;
                    hr.leave_hospital_time = outtime22;
                    hr.reason = this.textBox97.Text;
                    hr.hospital_organ = this.textBox95.Text;
                    hr.case_code = this.textBox93.Text;
                    hcd.addHospitalizedRecord(hr, "0");
                }
                this.Close();
                aUhealthcheckupServices4 auhc4 = new aUhealthcheckupServices4();
                auhc4.textBox1.Text = this.textBox106.Text;
                auhc4.textBox2.Text = this.textBox105.Text;
                auhc4.textBox3.Text = this.textBox107.Text;
                auhc4.id = per.id;//祖
                auhc4.textBox4.Text = per.id;
                auhc4.Show();
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
                this.checkBox5.Checked = false;
                this.checkBox6.Checked = false;
                this.textBox36.Text = "";
            }
        }

        private void checkBox7_Click(object sender, EventArgs e)
        {
            if (this.checkBox7.Checked)
            {
                this.checkBox8.Checked = false;
                this.checkBox9.Checked = false;
                this.checkBox10.Checked = false;
                this.checkBox11.Checked = false;
                this.checkBox12.Checked = false;
                this.textBox38.Text = "";
            }
        }

        private void checkBox13_Click(object sender, EventArgs e)
        {
            if (this.checkBox13.Checked)
            {
                this.checkBox14.Checked = false;
                this.checkBox15.Checked = false;
                this.checkBox16.Checked = false;
                this.checkBox17.Checked = false;
                this.checkBox18.Checked = false;
                this.checkBox19.Checked = false;
                this.textBox41.Text = "";
            }
        }

        private void checkBox20_Click(object sender, EventArgs e)
        {
            if (this.checkBox20.Checked)
            {
                this.checkBox21.Checked = false;
                this.checkBox22.Checked = false;
                this.checkBox23.Checked = false;
                this.textBox42.Text = "";
            }
        }

        private void checkBox24_Click(object sender, EventArgs e)
        {
            if (this.checkBox24.Checked)
            {
                this.checkBox25.Checked = false;
                this.checkBox26.Checked = false;
                this.checkBox27.Checked = false;
                this.checkBox28.Checked = false;
                this.textBox44.Text = "";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.textBox25.Enabled = true;
            }
            else
            {
                this.textBox25.Enabled = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton4.Checked)
            {
                this.textBox27.Enabled = true;
            }
            else
            {
                this.textBox27.Enabled = false;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton6.Checked)
            {
                this.textBox29.Enabled = true;
            }
            else
            {
                this.textBox29.Enabled = false;
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton8.Checked)
            {
                this.textBox31.Enabled = true;
            }
            else
            {
                this.textBox31.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox6.Checked)
            {
                this.checkBox1.Checked = false;
                this.textBox36.Enabled = true;
            }
            else
            {
                this.textBox36.Enabled = false;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox12.Checked)
            {
                this.checkBox7.Checked = false;
                this.textBox38.Enabled = true;
            }
            else
            {
                this.textBox38.Enabled = false;
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox19.Checked)
            {
                this.checkBox13.Checked = false;
                this.textBox41.Enabled = true;
            }
            else
            {
                this.textBox41.Enabled = false;
            }
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox23.Checked)
            {
                this.checkBox20.Checked = false;
                this.textBox42.Enabled = true;
            }
            else
            {
                this.textBox42.Enabled = false;
            }
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox28.Checked)
            {
                this.checkBox24.Checked = false;
                this.textBox44.Enabled = true;
            }
            else
            {
                this.textBox44.Enabled = false;
            }
        }

        private void radioButton40_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton40.Checked)
            {
                this.textBox48.Enabled = true;
            }
            else
            {
                this.textBox48.Enabled = false;
            }
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox29.Checked)
            {
                this.dateTimePicker1.Enabled = true;
                this.dateTimePicker2.Enabled = true;
                this.textBox60.Enabled = true;
                this.textBox62.Enabled = true;
                this.textBox89.Enabled = true;
            }
            else
            {
                this.dateTimePicker1.Enabled = false;
                this.dateTimePicker2.Enabled = false;
                this.textBox60.Enabled = false;
                this.textBox62.Enabled = false;
                this.textBox89.Enabled = false;
            }
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox30.Checked)
            {
                this.dateTimePicker4.Enabled = true;
                this.dateTimePicker3.Enabled = true;
                this.textBox61.Enabled = true;
                this.textBox70.Enabled = true;
                this.textBox92.Enabled = true;
            }
            else
            {
                this.dateTimePicker4.Enabled = false;
                this.dateTimePicker3.Enabled = false;
                this.textBox61.Enabled = false;
                this.textBox70.Enabled = false;
                this.textBox92.Enabled = false;
            }
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox32.Checked)
            {
                this.dateTimePicker8.Enabled = true;
                this.dateTimePicker7.Enabled = true;
                this.textBox98.Enabled = true;
                this.textBox96.Enabled = true;
                this.textBox94.Enabled = true;
            }
            else
            {
                this.dateTimePicker8.Enabled = false;
                this.dateTimePicker7.Enabled = false;
                this.textBox98.Enabled = false;
                this.textBox96.Enabled = false;
                this.textBox94.Enabled = false;
            }
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox31.Checked)
            {
                this.dateTimePicker6.Enabled = true;
                this.dateTimePicker5.Enabled = true;
                this.textBox97.Enabled = true;
                this.textBox95.Enabled = true;
                this.textBox93.Enabled = true;
            }
            else
            {
                this.dateTimePicker6.Enabled = false;
                this.dateTimePicker5.Enabled = false;
                this.textBox97.Enabled = false;
                this.textBox95.Enabled = false;
                this.textBox93.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();

            per.microalbuminuria = this.textBox77.Text;

            if (this.radioButton48.Checked == true) { per.fob = this.radioButton48.Tag.ToString(); };
            if (this.radioButton49.Checked == true) { per.fob = this.radioButton49.Tag.ToString(); };
            per.glycosylated_hemoglobin = this.textBox90.Text;

            if (this.radioButton46.Checked == true) { per.hb = this.radioButton46.Tag.ToString(); };
            if (this.radioButton47.Checked == true) { per.hb = this.radioButton47.Tag.ToString(); };

            per.sgft = this.textBox3.Text;
            per.ast = this.textBox6.Text;
            per.albumin = this.textBox10.Text;
            per.total_bilirubin = this.textBox65.Text;
            per.conjugated_bilirubin = this.textBox68.Text;
            per.scr = this.textBox73.Text;
            per.blood_urea = this.textBox81.Text;
            per.blood_k = this.textBox84.Text;
            per.blood_na = this.textBox87.Text;
            per.tc = this.textBox13.Text;
            per.tg = this.textBox16.Text;
            per.ldl = this.textBox19.Text;
            per.hdl = this.textBox22.Text;

            if (this.radioButton1.Checked == true) { per.chest_x = this.radioButton1.Tag.ToString(); };
            if (this.radioButton2.Checked == true)
            {
                per.chest_x = this.radioButton2.Tag.ToString();
                per.chestx_memo = this.textBox25.Text;
            };

            if (this.radioButton3.Checked == true) { per.ultrasound_abdomen = this.radioButton3.Tag.ToString(); };
            if (this.radioButton4.Checked == true)
            {
                per.ultrasound_abdomen = this.radioButton4.Tag.ToString();
                per.ultrasound_memo = this.textBox27.Text;
            };

            if (this.radioButton5.Checked == true) { per.other_b = this.radioButton5.Tag.ToString(); };
            if (this.radioButton6.Checked == true)
            {
                per.other_b = this.radioButton6.Tag.ToString();
                per.otherb_memo = this.textBox29.Text;
            };

            if (this.radioButton7.Checked == true) { per.cervical_smear = this.radioButton7.Tag.ToString(); };
            if (this.radioButton8.Checked == true)
            {
                per.cervical_smear = this.radioButton8.Tag.ToString();
                per.cervical_smear_memo = this.textBox31.Text;
            };
            per.other = this.textBox34.Text;

            foreach (Control ctr in this.panel7.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.cerebrovascular_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.cerebrovascular_disease != null && per.cerebrovascular_disease != "")
            {
                per.cerebrovascular_disease = per.cerebrovascular_disease.Substring(1);
                if (this.checkBox6.Checked)
                {
                    per.cerebrovascular_disease_other = this.textBox36.Text;
                }
            }

            foreach (Control ctr in this.panel8.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.kidney_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.kidney_disease != null && per.kidney_disease != "")
            {
                per.kidney_disease = per.kidney_disease.Substring(1);
                if (this.checkBox12.Checked)
                {
                    per.kidney_disease_other = this.textBox38.Text;
                }
            }

            foreach (Control ctr in this.panel9.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.heart_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.heart_disease != null && per.heart_disease != "")
            {
                per.heart_disease = per.heart_disease.Substring(1);
                if (this.checkBox19.Checked)
                {
                    per.heart_disease_other = this.textBox41.Text;
                }
            }

            foreach (Control ctr in this.panel10.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.vascular_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.vascular_disease != null && per.vascular_disease != "")
            {
                per.vascular_disease = per.vascular_disease.Substring(1);
                if (this.checkBox23.Checked)
                {
                    per.vascular_disease_other = this.textBox42.Text;
                }
            }

            foreach (Control ctr in this.panel11.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.ocular_diseases += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.ocular_diseases != null && per.ocular_diseases != "")
            {
                per.ocular_diseases = per.ocular_diseases.Substring(1);
                if (this.checkBox28.Checked)
                {
                    per.ocular_diseases_other = this.textBox44.Text;
                }
            }

            if (this.radioButton37.Checked == true) { per.nervous_system_disease = this.radioButton37.Tag.ToString(); };
            if (this.radioButton38.Checked == true)
            {
                per.nervous_system_disease = this.radioButton38.Tag.ToString();
                per.nervous_disease_memo = this.textBox46.Text;
            };

            if (this.radioButton39.Checked == true) { per.other_disease = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true)
            {
                per.other_disease = this.radioButton40.Tag.ToString();
                per.other_disease_memo = this.textBox48.Text;
            };

            string intime1 = this.dateTimePicker1.Text;
            string outtime1 = this.dateTimePicker2.Text;
            //string nowtime=DateTime.Now.ToString("yyyy-MM-dd");

            string intime2 = this.dateTimePicker4.Text;
            string outtime2 = this.dateTimePicker3.Text;

            string intime11 = this.dateTimePicker8.Text;
            string outtime11 = this.dateTimePicker7.Text;

            string intime22 = this.dateTimePicker6.Text;
            string outtime22 = this.dateTimePicker5.Text;

            per.aichive_no = this.textBox106.Text;
            per.bar_code = this.textBox105.Text;
            per.id_number = this.textBox107.Text;
            per.id = this.textBox108.Text;
            bool isfalse = hcd.addPhysicalExaminationRecord3(per);

            if (isfalse)
            {
                hospitalizedRecord hr = null;

                if (this.checkBox29.Checked)
                {
                    string text60 = this.textBox60.Text;
                    string text62 = this.textBox62.Text;
                    if (text60 == "" || text62 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "1";
                    hr.in_hospital_time = intime1;
                    hr.leave_hospital_time = outtime1;
                    hr.reason = this.textBox60.Text;
                    hr.hospital_organ = this.textBox62.Text;
                    hr.case_code = this.textBox89.Text;
                    hcd.addHospitalizedRecord(hr, "1");
                }

                if (this.checkBox30.Checked)
                {
                    string text61 = this.textBox61.Text;
                    string text70 = this.textBox70.Text;
                    if (text61 == "" || text70 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "1";
                    hr.in_hospital_time = intime2;
                    hr.leave_hospital_time = outtime2;
                    hr.reason = this.textBox61.Text;
                    hr.hospital_organ = this.textBox70.Text;
                    hr.case_code = this.textBox92.Text;
                    hcd.addHospitalizedRecord(hr, "0");
                }
                if (this.checkBox32.Checked)
                {
                    string text98 = this.textBox98.Text;
                    string text96 = this.textBox96.Text;
                    if (text98 == "" || text96 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "2";
                    hr.in_hospital_time = intime11;
                    hr.leave_hospital_time = outtime11;
                    hr.reason = this.textBox98.Text;
                    hr.hospital_organ = this.textBox96.Text;
                    hr.case_code = this.textBox94.Text;
                    hcd.addHospitalizedRecord(hr, "2");
                }
                if (this.checkBox31.Checked)
                {
                    string text97 = this.textBox97.Text;
                    string text95 = this.textBox95.Text;
                    if (text97 == "" || text95 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.exam_id = per.id;
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.hospitalized_type = "2";
                    hr.in_hospital_time = intime22;
                    hr.leave_hospital_time = outtime22;
                    hr.reason = this.textBox97.Text;
                    hr.hospital_organ = this.textBox95.Text;
                    hr.case_code = this.textBox93.Text;
                    hcd.addHospitalizedRecord(hr, "0");
                }
                this.Close();
                aUhealthcheckupServices2 auhc2 = new aUhealthcheckupServices2();
                auhc2.textBox95.Text = this.textBox106.Text;
                auhc2.textBox96.Text = this.textBox105.Text;
                auhc2.textBox99.Text = this.textBox107.Text;
                auhc2.id = per.id;//祖
                auhc2.textBox100.Text = per.id;
                auhc2.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();

            per.microalbuminuria = this.textBox77.Text;

            if (this.radioButton48.Checked == true) { per.fob = this.radioButton48.Tag.ToString(); };
            if (this.radioButton49.Checked == true) { per.fob = this.radioButton49.Tag.ToString(); };
            per.glycosylated_hemoglobin = this.textBox90.Text;

            if (this.radioButton46.Checked == true) { per.hb = this.radioButton46.Tag.ToString(); };
            if (this.radioButton47.Checked == true) { per.hb = this.radioButton47.Tag.ToString(); };

            per.sgft = this.textBox3.Text;
            per.ast = this.textBox6.Text;
            per.albumin = this.textBox10.Text;
            per.total_bilirubin = this.textBox65.Text;
            per.conjugated_bilirubin = this.textBox68.Text;
            per.scr = this.textBox73.Text;
            per.blood_urea = this.textBox81.Text;
            per.blood_k = this.textBox84.Text;
            per.blood_na = this.textBox87.Text;
            per.tc = this.textBox13.Text;
            per.tg = this.textBox16.Text;
            per.ldl = this.textBox19.Text;
            per.hdl = this.textBox22.Text;

            if (this.radioButton1.Checked == true) { per.chest_x = this.radioButton1.Tag.ToString(); };
            if (this.radioButton2.Checked == true)
            {
                per.chest_x = this.radioButton2.Tag.ToString();
                per.chestx_memo = this.textBox25.Text;
            };

            if (this.radioButton3.Checked == true) { per.ultrasound_abdomen = this.radioButton3.Tag.ToString(); };
            if (this.radioButton4.Checked == true)
            {
                per.ultrasound_abdomen = this.radioButton4.Tag.ToString();
                per.ultrasound_memo = this.textBox27.Text;
            };

            if (this.radioButton5.Checked == true) { per.other_b = this.radioButton5.Tag.ToString(); };
            if (this.radioButton6.Checked == true)
            {
                per.other_b = this.radioButton6.Tag.ToString();
                per.otherb_memo = this.textBox29.Text;
            };

            if (this.radioButton7.Checked == true) { per.cervical_smear = this.radioButton7.Tag.ToString(); };
            if (this.radioButton8.Checked == true)
            {
                per.cervical_smear = this.radioButton8.Tag.ToString();
                per.cervical_smear_memo = this.textBox31.Text;
            };
            per.other = this.textBox34.Text;

            foreach (Control ctr in this.panel7.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.cerebrovascular_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.cerebrovascular_disease != null && per.cerebrovascular_disease != "")
            {
                per.cerebrovascular_disease = per.cerebrovascular_disease.Substring(1);
                if (this.checkBox6.Checked)
                {
                    per.cerebrovascular_disease_other = this.textBox36.Text;
                }
            }

            foreach (Control ctr in this.panel8.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.kidney_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.kidney_disease != null && per.kidney_disease != "")
            {
                per.kidney_disease = per.kidney_disease.Substring(1);
                if (this.checkBox12.Checked)
                {
                    per.kidney_disease_other = this.textBox38.Text;
                }
            }

            foreach (Control ctr in this.panel9.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.heart_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.heart_disease != null && per.heart_disease != "")
            {
                per.heart_disease = per.heart_disease.Substring(1);
                if (this.checkBox19.Checked)
                {
                    per.heart_disease_other = this.textBox41.Text;
                }
            }

            foreach (Control ctr in this.panel10.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.vascular_disease += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.vascular_disease != null && per.vascular_disease != "")
            {
                per.vascular_disease = per.vascular_disease.Substring(1);
                if (this.checkBox23.Checked)
                {
                    per.vascular_disease_other = this.textBox42.Text;
                }
            }

            foreach (Control ctr in this.panel11.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.ocular_diseases += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.ocular_diseases != null && per.ocular_diseases != "")
            {
                per.ocular_diseases = per.ocular_diseases.Substring(1);
                if (this.checkBox28.Checked)
                {
                    per.ocular_diseases_other = this.textBox44.Text;
                }
            }

            if (this.radioButton37.Checked == true) { per.nervous_system_disease = this.radioButton37.Tag.ToString(); };
            if (this.radioButton38.Checked == true)
            {
                per.nervous_system_disease = this.radioButton38.Tag.ToString();
                per.nervous_disease_memo = this.textBox46.Text;
            };

            if (this.radioButton39.Checked == true) { per.other_disease = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true)
            {
                per.other_disease = this.radioButton40.Tag.ToString();
                per.other_disease_memo = this.textBox48.Text;
            };

            string intime1 = this.dateTimePicker1.Text;
            string outtime1 = this.dateTimePicker2.Text;
            //string nowtime=DateTime.Now.ToString("yyyy-MM-dd");

            string intime2 = this.dateTimePicker4.Text;
            string outtime2 = this.dateTimePicker3.Text;

            string intime11 = this.dateTimePicker8.Text;
            string outtime11 = this.dateTimePicker7.Text;

            string intime22 = this.dateTimePicker6.Text;
            string outtime22 = this.dateTimePicker5.Text;

            per.aichive_no = this.textBox106.Text;
            per.bar_code = this.textBox105.Text;
            per.id_number = this.textBox107.Text;
            per.id = this.textBox108.Text;
            bool isfalse = hcd.addPhysicalExaminationRecord3(per);

            if (isfalse)
            {
                hospitalizedRecord hr = null;

                if (this.checkBox29.Checked)
                {
                    string text60 = this.textBox60.Text;
                    string text62 = this.textBox62.Text;
                    if (text60 == "" || text62 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "1";
                    hr.in_hospital_time = intime1;
                    hr.leave_hospital_time = outtime1;
                    hr.reason = this.textBox60.Text;
                    hr.hospital_organ = this.textBox62.Text;
                    hr.case_code = this.textBox89.Text;
                    hcd.addHospitalizedRecord(hr, "1");
                }

                if (this.checkBox30.Checked)
                {
                    string text61 = this.textBox61.Text;
                    string text70 = this.textBox70.Text;
                    if (text61 == "" || text70 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "1";
                    hr.in_hospital_time = intime2;
                    hr.leave_hospital_time = outtime2;
                    hr.reason = this.textBox61.Text;
                    hr.hospital_organ = this.textBox70.Text;
                    hr.case_code = this.textBox92.Text;
                    hcd.addHospitalizedRecord(hr, "0");
                }
                if (this.checkBox32.Checked)
                {
                    string text98 = this.textBox98.Text;
                    string text96 = this.textBox96.Text;
                    if (text98 == "" || text96 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id = per.id;
                    hr.hospitalized_type = "2";
                    hr.in_hospital_time = intime11;
                    hr.leave_hospital_time = outtime11;
                    hr.reason = this.textBox98.Text;
                    hr.hospital_organ = this.textBox96.Text;
                    hr.case_code = this.textBox94.Text;
                    hcd.addHospitalizedRecord(hr, "2");
                }
                if (this.checkBox31.Checked)
                {
                    string text97 = this.textBox97.Text;
                    string text95 = this.textBox95.Text;
                    if (text97 == "" || text95 == "") { MessageBox.Show("住院史信息填写不完整!"); return; }
                    hr = new hospitalizedRecord();
                    hr.exam_id = per.id;
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.hospitalized_type = "2";
                    hr.in_hospital_time = intime22;
                    hr.leave_hospital_time = outtime22;
                    hr.reason = this.textBox97.Text;
                    hr.hospital_organ = this.textBox95.Text;
                    hr.case_code = this.textBox93.Text;
                    hcd.addHospitalizedRecord(hr, "0");
                }
                this.Close();
                aUhealthcheckupServices1 auhc1 = new aUhealthcheckupServices1();
                auhc1.textBox2.Text = this.textBox106.Text;
                auhc1.textBox118.Text = this.textBox105.Text;
                auhc1.textBox120.Text = per.id;
                auhc1.id = per.id;//祖
                auhc1.textBox119.Text = this.textBox107.Text;
                auhc1.Show();
            }
        }

        private void radioButton38_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton38.Checked)
            {
                this.textBox46.Enabled = true;
            }
            else
            {
                this.textBox46.Enabled = false;
            }
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked) {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox5_Click(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked)
            {
                this.checkBox1.Checked = false;
            }
        }

        private void checkBox8_Click(object sender, EventArgs e)
        {
            if (this.checkBox8.Checked)
            {
                this.checkBox7.Checked = false;
            }
        }

        private void checkBox9_Click(object sender, EventArgs e)
        {
            if (this.checkBox9.Checked)
            {
                this.checkBox7.Checked = false;
            }
        }

        private void checkBox10_Click(object sender, EventArgs e)
        {
            if (this.checkBox10.Checked)
            {
                this.checkBox7.Checked = false;
            }
        }

        private void checkBox11_Click(object sender, EventArgs e)
        {
            if (this.checkBox11.Checked)
            {
                this.checkBox7.Checked = false;
            }
        }

        private void checkBox14_Click(object sender, EventArgs e)
        {
            if (this.checkBox14.Checked)
            {
                this.checkBox13.Checked = false;
            }
        }

        private void checkBox15_Click(object sender, EventArgs e)
        {
            if (this.checkBox15.Checked)
            {
                this.checkBox13.Checked = false;
            }
        }

        private void checkBox16_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.checkBox13.Checked = false;
            }
        }

        private void checkBox17_Click(object sender, EventArgs e)
        {
            if (this.checkBox17.Checked)
            {
                this.checkBox13.Checked = false;
            }
        }

        private void checkBox18_Click(object sender, EventArgs e)
        {
            if (this.checkBox18.Checked)
            {
                this.checkBox13.Checked = false;
            }
        }

        private void checkBox21_Click(object sender, EventArgs e)
        {
            if (this.checkBox21.Checked)
            {
                this.checkBox20.Checked = false;
            }
        }

        private void checkBox22_Click(object sender, EventArgs e)
        {
            if (this.checkBox22.Checked)
            {
                this.checkBox20.Checked = false;
            }
        }

        private void checkBox25_Click(object sender, EventArgs e)
        {
            if (this.checkBox25.Checked)
            {
                this.checkBox24.Checked = false;
            }
        }

        private void checkBox26_Click(object sender, EventArgs e)
        {
            if (this.checkBox26.Checked)
            {
                this.checkBox24.Checked = false;
            }
        }

        private void checkBox27_Click(object sender, EventArgs e)
        {
            if (this.checkBox27.Checked)
            {
                this.checkBox24.Checked = false;
            }
        }
    }
}
