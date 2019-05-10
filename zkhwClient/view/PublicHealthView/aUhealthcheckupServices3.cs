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

            //查询赋值
            if (id != "")
            {
                DataTable dt = hcd.queryhealthCheckup(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.textBox106.Text = dt.Rows[0]["aichive_no"].ToString();
                    this.textBox105.Text = dt.Rows[0]["bar_code"].ToString();
                    this.textBox108.Text = dt.Rows[0]["id"].ToString();

                    this.textBox77.Text = dt.Rows[0]["microalbuminuria"].ToString();

                    if (this.radioButton48.Tag.ToString() == dt.Rows[0]["fob"].ToString()) { this.radioButton48.Checked = true; };
                    if (this.radioButton49.Tag.ToString() == dt.Rows[0]["fob"].ToString()) {this.radioButton49.Checked = true; };
                    this.textBox90.Text = dt.Rows[0]["glycosylated_hemoglobin"].ToString();

                    if (this.radioButton46.Tag.ToString() == dt.Rows[0]["hb"].ToString()) { this.radioButton46.Checked = true; };
                    if (this.radioButton47.Tag.ToString() == dt.Rows[0]["hb"].ToString()) { this.radioButton47.Checked = true; };

                    this.textBox3.Text = dt.Rows[0]["sgft"].ToString();
                    this.textBox6.Text = dt.Rows[0]["ast"].ToString();
                    this.textBox10.Text = dt.Rows[0]["albumin"].ToString();
                    this.textBox65.Text = dt.Rows[0]["total_bilirubin"].ToString();
                    this.textBox68.Text = dt.Rows[0]["conjugated_bilirubin"].ToString();
                    this.textBox73.Text = dt.Rows[0]["scr"].ToString();
                    this.textBox81.Text = dt.Rows[0]["blood_urea"].ToString();
                    this.textBox84.Text = dt.Rows[0]["blood_k"].ToString();
                    this.textBox87.Text = dt.Rows[0]["blood_na"].ToString();
                    this.textBox13.Text = dt.Rows[0]["tc"].ToString();
                    this.textBox16.Text = dt.Rows[0]["tg"].ToString();
                    this.textBox19.Text = dt.Rows[0]["ldl"].ToString();
                    this.textBox22.Text = dt.Rows[0]["hdl"].ToString();

                    if (this.radioButton1.Tag.ToString() == dt.Rows[0]["chest_x"].ToString()) { this.radioButton1.Checked = true; };
                    if (this.radioButton2.Tag.ToString() == dt.Rows[0]["chest_x"].ToString())
                    {
                        this.radioButton2.Checked = true;
                        this.textBox25.Text = dt.Rows[0]["chestx_memo"].ToString();
                    };

                    if (this.radioButton3.Tag.ToString() == dt.Rows[0]["ultrasound_abdomen"].ToString()) {this.radioButton3.Checked = true; };
                    if (this.radioButton4.Tag.ToString() == dt.Rows[0]["ultrasound_abdomen"].ToString())
                    {
                        this.radioButton4.Checked = true;
                        this.textBox27.Text = dt.Rows[0]["ultrasound_memo"].ToString();
                    };

                    if (this.radioButton5.Tag.ToString() == dt.Rows[0]["other_b"].ToString()) { this.radioButton5.Checked = true; };
                    if (this.radioButton6.Tag.ToString() == dt.Rows[0]["other_b"].ToString())
                    {
                        this.radioButton6.Checked = true;
                        this.textBox29.Text = dt.Rows[0]["otherb_memo"].ToString();
                    };

                    if (this.radioButton7.Tag.ToString() == dt.Rows[0]["cervical_smear"].ToString()) { this.radioButton7.Checked = true; };
                    if (this.radioButton7.Tag.ToString() == dt.Rows[0]["cervical_smear"].ToString())
                    {
                        this.radioButton8.Checked = true;
                        this.textBox31.Text = dt.Rows[0]["cervical_smear_memo"].ToString();
                    };
                    this.textBox34.Text = dt.Rows[0]["other"].ToString();

                    if (this.radioButton9.Tag.ToString() == dt.Rows[0]["cerebrovascular_disease"].ToString()) {this.radioButton9.Checked = true; };
                    if (this.radioButton10.Tag.ToString() == dt.Rows[0]["cerebrovascular_disease"].ToString()) { this.radioButton10.Checked = true; };
                    if (this.radioButton11.Tag.ToString() == dt.Rows[0]["cerebrovascular_disease"].ToString()) { this.radioButton11.Checked = true; };
                    if (this.radioButton12.Tag.ToString() == dt.Rows[0]["cerebrovascular_disease"].ToString()) { this.radioButton12.Checked = true; };
                    if (this.radioButton13.Tag.ToString() == dt.Rows[0]["cerebrovascular_disease"].ToString()) { this.radioButton13.Checked = true; };
                    if (this.radioButton14.Tag.ToString() == dt.Rows[0]["cerebrovascular_disease"].ToString())
                    {
                        this.radioButton14.Checked = true;
                        this.textBox36.Text = dt.Rows[0]["cerebrovascular_disease_other"].ToString();
                    };

                    if (this.radioButton19.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) {this.radioButton19.Checked = true; };
                    if (this.radioButton20.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) {  this.radioButton20.Checked = true; };
                    if (this.radioButton18.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) { this.radioButton18.Checked = true; };
                    if (this.radioButton17.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) { this.radioButton17.Checked = true; };
                    if (this.radioButton16.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) { this.radioButton16.Checked = true; };
                    if (this.radioButton15.Text == dt.Rows[0]["kidney_disease"].ToString())
                    {
                        this.radioButton15.Checked = true;
                        this.textBox38.Text = dt.Rows[0]["kidney_disease_other"].ToString();
                    };

                    if (this.radioButton19.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) { this.radioButton19.Checked = true; };
                    if (this.radioButton20.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) { this.radioButton20.Checked = true; };
                    if (this.radioButton18.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) { this.radioButton18.Checked = true; };
                    if (this.radioButton17.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) { this.radioButton17.Checked = true; };
                    if (this.radioButton16.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString()) { this.radioButton16.Checked = true; };
                    if (this.radioButton15.Tag.ToString() == dt.Rows[0]["kidney_disease"].ToString())
                    {
                        this.radioButton15.Checked = true;
                        this.textBox38.Text = dt.Rows[0]["kidney_disease_other"].ToString();
                    };

                    if (this.radioButton25.Tag.ToString() == dt.Rows[0]["heart_disease"].ToString()) { this.radioButton25.Checked = true; };
                    if (this.radioButton26.Tag.ToString() == dt.Rows[0]["heart_disease"].ToString()) { this.radioButton26.Checked = true; };
                    if (this.radioButton24.Tag.ToString() == dt.Rows[0]["heart_disease"].ToString()) { this.radioButton24.Checked = true; };
                    if (this.radioButton23.Tag.ToString() == dt.Rows[0]["heart_disease"].ToString()) { this.radioButton23.Checked = true; };
                    if (this.radioButton27.Tag.ToString() == dt.Rows[0]["heart_disease"].ToString()) { this.radioButton27.Checked = true; };
                    if (this.radioButton22.Tag.ToString() == dt.Rows[0]["heart_disease"].ToString()) { this.radioButton22.Checked = true; };
                    if (this.radioButton21.Tag.ToString() == dt.Rows[0]["heart_disease"].ToString())
                    {
                        this.radioButton21.Checked = true;
                        this.textBox41.Text = dt.Rows[0]["heart_disease_other"].ToString();
                    };

                    if (this.radioButton32.Tag.ToString() == dt.Rows[0]["vascular_disease"].ToString()) { this.radioButton32.Checked = true; };
                    if (this.radioButton33.Tag.ToString() == dt.Rows[0]["vascular_disease"].ToString()) { this.radioButton33.Checked = true; };
                    if (this.radioButton31.Tag.ToString() == dt.Rows[0]["vascular_disease"].ToString()) { this.radioButton31.Checked = true; };
                    if (this.radioButton28.Tag.ToString() == dt.Rows[0]["vascular_disease"].ToString())
                    {
                        this.radioButton28.Checked = true;
                        this.textBox42.Text = dt.Rows[0]["vascular_disease_other"].ToString();
                    };

                    if (this.radioButton34.Tag.ToString() == dt.Rows[0]["ocular_diseases"].ToString()) { this.radioButton34.Checked = true; };
                    if (this.radioButton35.Tag.ToString() == dt.Rows[0]["ocular_diseases"].ToString()) { this.radioButton35.Checked = true; };
                    if (this.radioButton30.Tag.ToString() == dt.Rows[0]["ocular_diseases"].ToString()) { this.radioButton30.Checked = true; };
                    if (this.radioButton36.Tag.ToString() == dt.Rows[0]["ocular_diseases"].ToString()) { this.radioButton36.Checked = true; };
                    if (this.radioButton29.Tag.ToString() == dt.Rows[0]["ocular_diseases"].ToString())
                    {
                        this.radioButton29.Checked = true;
                        this.textBox44.Text = dt.Rows[0]["ocular_diseases_other"].ToString();
                    };

                    if (this.radioButton37.Tag.ToString() == dt.Rows[0]["nervous_system_disease"].ToString()) { this.radioButton37.Checked = true; };
                    if (this.radioButton38.Tag.ToString() == dt.Rows[0]["nervous_system_disease"].ToString())
                    {
                        this.radioButton38.Checked = true;
                        this.textBox46.Text = dt.Rows[0]["nervous_disease_memo"].ToString();
                    };

                    if (this.radioButton39.Tag.ToString() == dt.Rows[0]["other_disease"].ToString()) {this.radioButton39.Checked = true; };
                    if (this.radioButton40.Tag.ToString() == dt.Rows[0]["other_disease"].ToString())
                    {
                        this.radioButton40.Checked = true;
                        this.textBox48.Text = dt.Rows[0]["other_disease_memo"].ToString();
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
                        if (dt.Rows[i]["hospitalized_type"].ToString() == "1")
                        {

                            if (j == 0)
                            {
                                this.dateTimePicker1.Value = DateTime.Parse(dt.Rows[i]["in_hospital_time"].ToString());
                                this.dateTimePicker2.Value = DateTime.Parse(dt.Rows[i]["leave_hospital_time"].ToString());
                                this.textBox60.Text = dt.Rows[i]["reason"].ToString();
                                this.textBox62.Text = dt.Rows[i]["hospital_organ"].ToString();
                                this.textBox89.Text = dt.Rows[i]["case_code"].ToString();
                            }
                            else
                            {
                                this.dateTimePicker4.Value = DateTime.Parse(dt.Rows[i]["in_hospital_time"].ToString());
                                this.dateTimePicker3.Value = DateTime.Parse(dt.Rows[i]["leave_hospital_time"].ToString());
                                this.textBox61.Text = dt.Rows[i]["reason"].ToString();
                                this.textBox70.Text = dt.Rows[i]["hospital_organ"].ToString();
                                this.textBox92.Text = dt.Rows[i]["case_code"].ToString();
                            }
                            j++;
                        }
                        else {
                            if (k == 0)
                            {
                                this.dateTimePicker8.Value = DateTime.Parse(dt.Rows[i]["in_hospital_time"].ToString());
                                this.dateTimePicker7.Value = DateTime.Parse(dt.Rows[i]["leave_hospital_time"].ToString());
                                this.textBox98.Text = dt.Rows[i]["reason"].ToString();
                                this.textBox96.Text = dt.Rows[i]["hospital_organ"].ToString();
                                this.textBox94.Text = dt.Rows[i]["case_code"].ToString();
                            }
                            else
                            {
                                this.dateTimePicker6.Value = DateTime.Parse(dt.Rows[i]["in_hospital_time"].ToString());
                                this.dateTimePicker5.Value = DateTime.Parse(dt.Rows[i]["leave_hospital_time"].ToString());
                                this.textBox97.Text = dt.Rows[i]["reason"].ToString();
                                this.textBox95.Text = dt.Rows[i]["hospital_organ"].ToString();
                                this.textBox93.Text = dt.Rows[i]["case_code"].ToString();
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
            per.aichive_no = this.textBox106.Text;
            per.bar_code = this.textBox105.Text;
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
                per.cervical_smear = this.radioButton7.Tag.ToString();
                per.cervical_smear_memo = this.textBox31.Text;
            };
            per.other = this.textBox34.Text;

            if (this.radioButton9.Checked == true) { per.cerebrovascular_disease = this.radioButton9.Tag.ToString(); };
            if (this.radioButton10.Checked == true) { per.cerebrovascular_disease = this.radioButton10.Tag.ToString(); };
            if (this.radioButton11.Checked == true) { per.cerebrovascular_disease = this.radioButton11.Tag.ToString(); };
            if (this.radioButton12.Checked == true) { per.cerebrovascular_disease = this.radioButton12.Tag.ToString(); };
            if (this.radioButton13.Checked == true) { per.cerebrovascular_disease = this.radioButton13.Tag.ToString(); };
            if (this.radioButton14.Checked == true)
            {
                per.cerebrovascular_disease = this.radioButton14.Tag.ToString();
                per.cerebrovascular_disease_other = this.textBox36.Text;
            };

            if (this.radioButton19.Checked == true) { per.kidney_disease = this.radioButton19.Tag.ToString(); };
            if (this.radioButton20.Checked == true) { per.kidney_disease = this.radioButton20.Tag.ToString(); };
            if (this.radioButton18.Checked == true) { per.kidney_disease = this.radioButton18.Tag.ToString(); };
            if (this.radioButton17.Checked == true) { per.kidney_disease = this.radioButton17.Tag.ToString(); };
            if (this.radioButton16.Checked == true) { per.kidney_disease = this.radioButton16.Tag.ToString(); };
            if (this.radioButton15.Checked == true)
            {
                per.kidney_disease = this.radioButton15.Text;
                per.kidney_disease_other = this.textBox38.Text;
            };

            if (this.radioButton19.Checked == true) { per.kidney_disease = this.radioButton19.Tag.ToString(); };
            if (this.radioButton20.Checked == true) { per.kidney_disease = this.radioButton20.Tag.ToString(); };
            if (this.radioButton18.Checked == true) { per.kidney_disease = this.radioButton18.Tag.ToString(); };
            if (this.radioButton17.Checked == true) { per.kidney_disease = this.radioButton17.Tag.ToString(); };
            if (this.radioButton16.Checked == true) { per.kidney_disease = this.radioButton16.Tag.ToString(); };
            if (this.radioButton15.Checked == true)
            {
                per.kidney_disease = this.radioButton15.Tag.ToString();
                per.kidney_disease_other = this.textBox38.Text;
            };

            if (this.radioButton25.Checked == true) { per.heart_disease = this.radioButton25.Tag.ToString(); };
            if (this.radioButton26.Checked == true) { per.heart_disease = this.radioButton26.Tag.ToString(); };
            if (this.radioButton24.Checked == true) { per.heart_disease = this.radioButton24.Tag.ToString(); };
            if (this.radioButton23.Checked == true) { per.heart_disease = this.radioButton23.Tag.ToString(); };
            if (this.radioButton27.Checked == true) { per.heart_disease = this.radioButton27.Tag.ToString(); };
            if (this.radioButton22.Checked == true) { per.heart_disease = this.radioButton22.Tag.ToString(); };
            if (this.radioButton21.Checked == true)
            {
                per.heart_disease = this.radioButton21.Tag.ToString();
                per.heart_disease_other = this.textBox41.Text;
            };

            if (this.radioButton32.Checked == true) { per.vascular_disease = this.radioButton32.Tag.ToString(); };
            if (this.radioButton33.Checked == true) { per.vascular_disease = this.radioButton33.Tag.ToString(); };
            if (this.radioButton31.Checked == true) { per.vascular_disease = this.radioButton31.Tag.ToString(); };
            if (this.radioButton28.Checked == true)
            {
                per.vascular_disease = this.radioButton28.Tag.ToString();
                per.vascular_disease_other = this.textBox42.Text;
            };

            if (this.radioButton34.Checked == true) { per.ocular_diseases = this.radioButton34.Tag.ToString(); };
            if (this.radioButton35.Checked == true) { per.ocular_diseases = this.radioButton35.Tag.ToString(); };
            if (this.radioButton30.Checked == true) { per.ocular_diseases = this.radioButton30.Tag.ToString(); };
            if (this.radioButton36.Checked == true) { per.ocular_diseases = this.radioButton36.Tag.ToString(); };
            if (this.radioButton29.Checked == true)
            {
                per.ocular_diseases = this.radioButton29.Tag.ToString();
                per.ocular_diseases_other = this.textBox44.Text;
            };

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
                auhc4.textBox1.Text = this.textBox106.Text;
                auhc4.textBox2.Text = this.textBox105.Text;
                auhc4.textBox3.Text = this.textBox107.Text;
                auhc4.id = id;//祖
                auhc4.textBox4.Text = per.id;
                auhc4.Show();
            }
        }
    }
}
