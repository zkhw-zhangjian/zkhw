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
            per.aichive_no = this.textBox106.Text;
            per.bar_code = this.textBox105.Text;
            per.id_number = this.textBox107.Text;
            per.id = this.textBox108.Text;

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
            if (this.radioButton2.Checked == true) { per.chest_x = this.radioButton2.Tag.ToString();
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
            string nowtime=DateTime.Now.ToString("yyyy-MM-dd");
            
            string intime2 = this.dateTimePicker4.Text;
            string outtime2 = this.dateTimePicker3.Text;
            
            string intime11 = this.dateTimePicker8.Text;
            string outtime11 = this.dateTimePicker7.Text;
           
            string intime22 = this.dateTimePicker6.Text;
            string outtime22 = this.dateTimePicker5.Text;
           
            bool isfalse = hcd.addPhysicalExaminationRecord3(per);

            if (isfalse)
            {
                hospitalizedRecord  hr= null;
                if (!nowtime.Equals(intime1) && !nowtime.Equals(outtime1))
                {
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.exam_id= per.id;
                    hr.hospitalized_type = "1";
                    hr.in_hospital_time = intime1;
                    hr.leave_hospital_time = outtime1;
                    hr.reason = this.textBox60.Text;
                    hr.hospital_organ= this.textBox62.Text;
                    hr.case_code= this.textBox89.Text;
                    hcd.addHospitalizedRecord(hr);
                }
                if (!nowtime.Equals(intime2) && !nowtime.Equals(outtime2))
                {
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
                    hcd.addHospitalizedRecord(hr);
                }
                if (!nowtime.Equals(intime11) && !nowtime.Equals(outtime11))
                {
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.hospitalized_type = "2";
                    hr.in_hospital_time = intime11;
                    hr.leave_hospital_time = outtime11;
                    hr.reason = this.textBox98.Text;
                    hr.hospital_organ = this.textBox96.Text;
                    hr.case_code = this.textBox94.Text;
                    hcd.addHospitalizedRecord(hr);
                }
                if (!nowtime.Equals(intime22) && !nowtime.Equals(outtime22))
                {
                    hr = new hospitalizedRecord();
                    hr.archive_no = this.textBox106.Text;
                    hr.id_number = this.textBox107.Text;
                    hr.hospitalized_type = "2";
                    hr.in_hospital_time = intime22;
                    hr.leave_hospital_time = outtime22;
                    hr.reason = this.textBox97.Text;
                    hr.hospital_organ = this.textBox95.Text;
                    hr.case_code = this.textBox93.Text;
                    hcd.addHospitalizedRecord(hr);
                }
                this.Close();
                aUhealthcheckupServices4 auhc4 = new aUhealthcheckupServices4();
                auhc4.textBox1.Text = per.aichive_no;
                auhc4.textBox2.Text = per.bar_code;
                auhc4.textBox3.Text = per.id_number;
                auhc4.textBox4.Text = per.id;
                auhc4.Show();
            }
        }
    }
}
