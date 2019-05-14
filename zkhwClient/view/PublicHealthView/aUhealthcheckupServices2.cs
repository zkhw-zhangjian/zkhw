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
    public partial class aUhealthcheckupServices2 : Form
    {
        healthCheckupDao hcd = new healthCheckupDao();
        public string id = "";
        DataTable goodsList = new DataTable();//用药记录清单表 follow_medicine_record
        public string hypodontia1 = "";
        public string hypodontia2 = "";
        public string hypodontia3 = "";
        public string hypodontia4 = "";

        public string caries1 = "";
        public string caries2 = "";
        public string caries3 = "";
        public string caries4 = "";

        public string denture1 = "";
        public string denture2 = "";
        public string denture3 = "";
        public string denture4 = "";
        public aUhealthcheckupServices2()
        {
            InitializeComponent();
        }
        private void aUdiabetesPatientServices_Load(object sender, EventArgs e)
        {
            this.label51.Text = "健康体检表第二页(共四页)";
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
                    this.textBox96.Text = dt.Rows[0]["aichive_no"].ToString();
                    this.textBox95.Text = dt.Rows[0]["bar_code"].ToString();
                    this.textBox100.Text = dt.Rows[0]["id"].ToString();

                    if (this.radioButton55.Tag.ToString() == dt.Rows[0]["organ_lips"].ToString()) {  this.radioButton55.Checked = true; };
                    if (this.radioButton56.Tag.ToString() == dt.Rows[0]["organ_lips"].ToString()) {  this.radioButton56.Checked = true; };
                    if (this.radioButton57.Tag.ToString() == dt.Rows[0]["organ_lips"].ToString()) { this.radioButton57.Checked = true; };
                    if (this.radioButton54.Tag.ToString() == dt.Rows[0]["organ_lips"].ToString()) {this.radioButton54.Checked = true; };
                    if (this.radioButton53.Tag.ToString() == dt.Rows[0]["organ_lips"].ToString()) { this.radioButton53.Checked = true; };

                    foreach (Control ctr in this.panel25.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["organ_tooth"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                                if ("2".Equals(ck.Tag.ToString())) {
                                    hypodontia1 = dt.Rows[0]["organ_hypodontia_topleft"].ToString();
                                    hypodontia3 = dt.Rows[0]["organ_hypodontia_topright"].ToString();
                                    hypodontia2 = dt.Rows[0]["organ_hypodontia_bottomleft"].ToString();
                                    hypodontia4 = dt.Rows[0]["organ_hypodontia_bottomright"].ToString();
                                }
                                if ("3".Equals(ck.Tag.ToString()))
                                {
                                    caries1 = dt.Rows[0]["organ_caries_topleft"].ToString();
                                    caries3 = dt.Rows[0]["organ_caries_topright"].ToString();
                                    caries2 = dt.Rows[0]["organ_caries_bottomleft"].ToString();
                                    caries4 = dt.Rows[0]["organ_caries_bottomright"].ToString();
                                }
                                if ("4".Equals(ck.Tag.ToString()))
                                {
                                    denture1 = dt.Rows[0]["organ_denture_topleft"].ToString();
                                    denture3 = dt.Rows[0]["organ_denture_topright"].ToString();
                                    denture2 = dt.Rows[0]["organ_denture_bottomleft"].ToString();
                                    denture4 = dt.Rows[0]["organ_denture_bottomright"].ToString();
                                }
                            }
                        }
                    }
                 
                    if (this.radioButton87.Tag.ToString() == dt.Rows[0]["organ_guttur"].ToString()) { this.radioButton87.Checked = true; };
                    if (this.radioButton88.Tag.ToString() == dt.Rows[0]["organ_guttur"].ToString()) { this.radioButton88.Checked = true; };
                    if (this.radioButton89.Tag.ToString() == dt.Rows[0]["organ_guttur"].ToString()) {  this.radioButton89.Checked = true; };

                    this.textBox9.Text = dt.Rows[0]["organ_vision_left"].ToString();
                    this.textBox8.Text = dt.Rows[0]["organ_vision_right"].ToString();
                    this.textBox4.Text = dt.Rows[0]["organ_correctedvision_left"].ToString();
                    this.textBox51.Text = dt.Rows[0]["organ_correctedvision_right"].ToString();

                    if (this.radioButton52.Tag.ToString() == dt.Rows[0]["organ_hearing"].ToString()) {  this.radioButton52.Checked = true; };
                    if (this.radioButton58.Tag.ToString() == dt.Rows[0]["organ_hearing"].ToString()) { this.radioButton58.Checked = true; };

                    if (this.radioButton59.Tag.ToString() == dt.Rows[0]["organ_movement"].ToString()) { this.radioButton59.Checked = true; };
                    if (this.radioButton60.Tag.ToString() == dt.Rows[0]["organ_movement"].ToString()) { this.radioButton60.Checked = true; };

                    if (this.radioButton6.Tag.ToString() == dt.Rows[0]["examination_eye"].ToString()) {this.radioButton6.Checked = true; };
                    if (this.radioButton10.Tag.ToString() == dt.Rows[0]["examination_eye"].ToString())
                    {
                        this.radioButton10.Checked = true;
                        this.textBox13.Text = dt.Rows[0]["examination_eye_other"].ToString();
                    };
                    if (this.radioButton63.Tag.ToString() == dt.Rows[0]["examination_skin"].ToString()) { this.radioButton63.Checked = true; };
                    if (this.radioButton64.Tag.ToString() == dt.Rows[0]["examination_skin"].ToString()) { this.radioButton64.Checked = true; };
                    if (this.radioButton65.Tag.ToString() == dt.Rows[0]["examination_skin"].ToString()) { this.radioButton65.Checked = true; };
                    if (this.radioButton62.Tag.ToString() == dt.Rows[0]["examination_skin"].ToString()) { this.radioButton62.Checked = true; };
                    if (this.radioButton61.Tag.ToString() == dt.Rows[0]["examination_skin"].ToString()) { this.radioButton61.Checked = true; };
                    if (this.radioButton66.Tag.ToString() == dt.Rows[0]["examination_skin"].ToString()) { this.radioButton66.Checked = true; };
                    if (this.radioButton67.Tag.ToString() == dt.Rows[0]["examination_skin"].ToString())
                    {
                       this.radioButton67.Checked = true;
                       this.textBox14.Text = dt.Rows[0]["examination_skin_other"].ToString();
                    };

                    if (this.radioButton72.Tag.ToString() == dt.Rows[0]["examination_sclera"].ToString()) { this.radioButton72.Checked = true; };
                    if (this.radioButton74.Tag.ToString() == dt.Rows[0]["examination_sclera"].ToString()) { this.radioButton74.Checked = true; };
                    if (this.radioButton71.Tag.ToString() == dt.Rows[0]["examination_sclera"].ToString()) { this.radioButton71.Checked = true; };
                    if (this.radioButton68.Tag.ToString() == dt.Rows[0]["examination_sclera"].ToString())
                    {
                        this.radioButton68.Checked = true;
                        this.textBox15.Text = dt.Rows[0]["examination_sclera_other"].ToString();
                    };

                    if (this.radioButton73.Tag.ToString() == dt.Rows[0]["examination_lymph"].ToString()) { this.radioButton73.Checked = true; };
                    if (this.radioButton75.Tag.ToString() == dt.Rows[0]["examination_lymph"].ToString()) { this.radioButton75.Checked = true; };
                    if (this.radioButton70.Tag.ToString() == dt.Rows[0]["examination_lymph"].ToString()) {this.radioButton70.Checked = true; };
                    if (this.radioButton69.Tag.ToString() == dt.Rows[0]["examination_lymph"].ToString())
                    {
                        this.radioButton69.Checked = true;
                        this.textBox16.Text = dt.Rows[0]["examination_lymph_other"].ToString();
                    };

                    if (this.radioButton1.Tag.ToString() == dt.Rows[0]["examination_barrel_chest"].ToString()) { this.radioButton1.Checked = true; };
                    if (this.radioButton2.Tag.ToString() == dt.Rows[0]["examination_barrel_chest"].ToString()) { this.radioButton2.Checked = true; };

                    if (this.radioButton3.Tag.ToString() == dt.Rows[0]["examination_breath_sounds"].ToString()) { this.radioButton3.Checked = true; };
                    if (this.radioButton4.Tag.ToString() == dt.Rows[0]["examination_breath_sounds"].ToString())
                    {
                        this.radioButton4.Checked = true;
                        this.textBox21.Text = dt.Rows[0]["examination_breath_other"].ToString();
                    };

                    if (this.radioButton8.Tag.ToString() == dt.Rows[0]["examination_rale"].ToString()) { this.radioButton8.Checked = true; };
                    if (this.radioButton9.Tag.ToString() == dt.Rows[0]["examination_rale"].ToString()) { this.radioButton9.Checked = true; };
                    if (this.radioButton7.Tag.ToString() == dt.Rows[0]["examination_rale"].ToString()) { this.radioButton7.Checked = true; };
                    if (this.radioButton5.Tag.ToString() == dt.Rows[0]["examination_rale"].ToString())
                    {
                        this.radioButton5.Checked = true;
                        this.textBox42.Text = dt.Rows[0]["examination_rale_other"].ToString();
                    };

                    this.textBox47.Text = dt.Rows[0]["examination_heart_rate"].ToString();
                    if (this.radioButton16.Tag.ToString() == dt.Rows[0]["examination_heart_rhythm"].ToString()) { this.radioButton16.Checked = true; };
                    if (this.radioButton17.Tag.ToString() == dt.Rows[0]["examination_heart_rhythm"].ToString()) { this.radioButton17.Checked = true; };
                    if (this.radioButton18.Tag.ToString() == dt.Rows[0]["examination_heart_rhythm"].ToString()) { this.radioButton18.Checked = true; };

                    if (this.radioButton77.Tag.ToString() == dt.Rows[0]["examination_heart_noise"].ToString()) { this.radioButton77.Checked = true; };
                    if (this.radioButton78.Tag.ToString() == dt.Rows[0]["examination_heart_noise"].ToString())
                    {
                        this.radioButton78.Checked = true;
                        this.textBox54.Text = dt.Rows[0]["examination_noise_other"].ToString();
                    };

                    if (this.radioButton80.Tag.ToString() == dt.Rows[0]["examination_abdomen_tenderness"].ToString()) { this.radioButton80.Checked = true; };
                    if (this.radioButton81.Tag.ToString() == dt.Rows[0]["examination_abdomen_tenderness"].ToString())
                    {
                        this.radioButton81.Checked = true;
                        this.textBox59.Text = dt.Rows[0]["examination_tenderness_memo"].ToString();
                    };

                    if (this.radioButton76.Tag.ToString() == dt.Rows[0]["examination_abdomen_mass"].ToString()) {this.radioButton76.Checked = true; };
                    if (this.radioButton79.Tag.ToString() == dt.Rows[0]["examination_abdomen_mass"].ToString())
                    {
                        this.radioButton79.Checked = true;
                        this.textBox56.Text = dt.Rows[0]["examination_mass_memo"].ToString();
                    };

                    if (this.radioButton82.Tag.ToString() == dt.Rows[0]["examination_abdomen_hepatomegaly"].ToString()) { this.radioButton82.Checked = true; };
                    if (this.radioButton83.Tag.ToString() == dt.Rows[0]["examination_abdomen_hepatomegaly"].ToString())
                    {
                        this.radioButton83.Checked = true;
                        this.textBox56.Text = dt.Rows[0]["examination_hepatomegaly_memo"].ToString();
                    };

                    if (this.radioButton11.Tag.ToString() == dt.Rows[0]["examination_abdomen_splenomegaly"].ToString()) { this.radioButton11.Checked = true; };
                    if (this.radioButton12.Tag.ToString() == dt.Rows[0]["examination_abdomen_splenomegaly"].ToString())
                    {
                        this.radioButton12.Checked = true;
                        this.textBox56.Text = dt.Rows[0]["examination_splenomegaly_memo"].ToString();
                    };

                    if (this.radioButton13.Tag.ToString() == dt.Rows[0]["examination_abdomen_shiftingdullness"].ToString()) { this.radioButton13.Checked = true; };
                    if (this.radioButton14.Tag.ToString() == dt.Rows[0]["examination_abdomen_shiftingdullness"].ToString())
                    {
                        this.radioButton14.Checked = true;
                        this.textBox23.Text = dt.Rows[0]["examination_shiftingdullness_memo"].ToString();
                    };

                    if (this.radioButton20.Tag.ToString() == dt.Rows[0]["examination_lowerextremity_edema"].ToString()) { this.radioButton20.Checked = true; };
                    if (this.radioButton21.Tag.ToString() == dt.Rows[0]["examination_lowerextremity_edema"].ToString()) { this.radioButton21.Checked = true; };
                    if (this.radioButton19.Tag.ToString() == dt.Rows[0]["examination_lowerextremity_edema"].ToString()) { this.radioButton19.Checked = true; };
                    if (this.radioButton15.Tag.ToString() == dt.Rows[0]["examination_lowerextremity_edema"].ToString()) { this.radioButton15.Checked = true; };

                    if (this.radioButton24.Tag.ToString() == dt.Rows[0]["examination_dorsal_artery"].ToString()) { this.radioButton24.Checked = true; };
                    if (this.radioButton24.Tag.ToString() == dt.Rows[0]["examination_dorsal_artery"].ToString()) { this.radioButton25.Checked = true; };
                    if (this.radioButton23.Tag.ToString() == dt.Rows[0]["examination_dorsal_artery"].ToString()) { this.radioButton23.Checked = true; };
                    if (this.radioButton22.Tag.ToString() == dt.Rows[0]["examination_dorsal_artery"].ToString()) { this.radioButton22.Checked = true; };

                    if (this.radioButton28.Tag.ToString() == dt.Rows[0]["examination_anus"].ToString()) { this.radioButton28.Checked = true; };
                    if (this.radioButton28.Tag.ToString() == dt.Rows[0]["examination_anus"].ToString()) { this.radioButton29.Checked = true; };
                    if (this.radioButton27.Tag.ToString() == dt.Rows[0]["examination_anus"].ToString()) { this.radioButton27.Checked = true; };
                    if (this.radioButton26.Tag.ToString() == dt.Rows[0]["examination_anus"].ToString()) { this.radioButton26.Checked = true; };
                    if (this.radioButton30.Tag.ToString() == dt.Rows[0]["examination_anus"].ToString())
                    {
                        this.radioButton30.Checked = true;
                        this.textBox28.Text = dt.Rows[0]["examination_anus_other"].ToString();
                    };

                    foreach (Control ctr in this.panel10.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["examination_breast"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }
                    this.textBox30.Text = dt.Rows[0]["examination_breast_other"].ToString();

                    if (this.radioButton38.Tag.ToString() == dt.Rows[0]["examination_woman_vulva"].ToString()) { this.radioButton38.Checked = true; };
                    if (this.radioButton39.Tag.ToString() == dt.Rows[0]["examination_woman_vulva"].ToString())
                    {
                        this.radioButton39.Checked = true;
                        this.textBox33.Text = dt.Rows[0]["examination_vulva_memo"].ToString();
                    };

                    if (this.radioButton36.Tag.ToString() == dt.Rows[0]["examination_woman_vagina"].ToString()) { this.radioButton36.Checked = true; };
                    if (this.radioButton37.Tag.ToString() == dt.Rows[0]["examination_woman_vagina"].ToString())
                    {
                        this.radioButton37.Checked = true;
                        this.textBox34.Text = dt.Rows[0]["examination_vagina_memo"].ToString();
                    };

                    if (this.radioButton40.Tag.ToString() == dt.Rows[0]["examination_woman_cervix"].ToString()) { this.radioButton40.Checked = true; };
                    if (this.radioButton41.Tag.ToString() == dt.Rows[0]["examination_woman_cervix"].ToString())
                    {
                        this.radioButton41.Checked = true;
                        this.textBox36.Text = dt.Rows[0]["examination_cervix_memo"].ToString();
                    };

                    if (this.radioButton42.Tag.ToString() == dt.Rows[0]["examination_woman_corpus"].ToString()) { this.radioButton42.Checked = true; };
                    if (this.radioButton43.Tag.ToString() == dt.Rows[0]["examination_woman_corpus"].ToString())
                    {
                        this.radioButton43.Checked = true;
                        this.textBox38.Text = dt.Rows[0]["examination_corpus_memo"].ToString();
                    };

                    if (this.radioButton44.Tag.ToString() == dt.Rows[0]["examination_woman_accessories"].ToString()) { this.radioButton44.Checked = true; };
                    if (this.radioButton45.Tag.ToString() == dt.Rows[0]["examination_woman_accessories"].ToString())
                    {
                        this.radioButton45.Checked = true;
                        this.textBox40.Text = dt.Rows[0]["examination_accessories_memo"].ToString();
                    };
                    this.textBox50.Text = dt.Rows[0]["examination_other"].ToString();

                    this.textBox77.Text = dt.Rows[0]["blood_hemoglobin"].ToString();
                    this.textBox82.Text = dt.Rows[0]["blood_leukocyte"].ToString();
                    this.textBox85.Text = dt.Rows[0]["blood_platelet"].ToString();
                    this.textBox88.Text = dt.Rows[0]["blood_other"].ToString();
                    this.textBox72.Text = dt.Rows[0]["urine_protein"].ToString();
                    this.textBox71.Text = dt.Rows[0]["glycosuria"].ToString();
                    this.textBox68.Text = dt.Rows[0]["urine_acetone_bodies"].ToString();
                    this.textBox67.Text = dt.Rows[0]["bld"].ToString();
                    this.textBox89.Text = dt.Rows[0]["urine_other"].ToString();
                    this.textBox90.Text = dt.Rows[0]["blood_glucose_mmol"].ToString();
                    this.textBox92.Text = dt.Rows[0]["blood_glucose_mg"].ToString();

                    if (this.radioButton46.Tag.ToString() == dt.Rows[0]["cardiogram"].ToString()) { this.radioButton46.Checked = true; };
                    if (this.radioButton47.Tag.ToString() == dt.Rows[0]["cardiogram"].ToString())
                    {
                        this.radioButton47.Checked = true;
                        this.textBox94.Text = dt.Rows[0]["cardiogram_memo"].ToString();
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
            per.aichive_no = this.textBox96.Text;
            per.bar_code = this.textBox95.Text;
            per.id = this.textBox100.Text;

            if (this.radioButton55.Checked == true) { per.organ_lips = this.radioButton55.Tag.ToString(); };
            if (this.radioButton56.Checked == true) { per.organ_lips = this.radioButton56.Tag.ToString(); };
            if (this.radioButton57.Checked == true) { per.organ_lips = this.radioButton57.Tag.ToString(); };
            if (this.radioButton54.Checked == true) { per.organ_lips = this.radioButton54.Tag.ToString(); };
            if (this.radioButton53.Checked == true) { per.organ_lips = this.radioButton53.Tag.ToString(); };

            foreach (Control ctr in this.panel25.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.organ_tooth += "," + ck.Tag.ToString();
                    }
                    if ("checkBox2".Equals(ck.Name))
                    {
                        per.organ_hypodontia_topleft = hypodontia1;
                        per.organ_hypodontia_bottomleft = hypodontia2;
                        per.organ_hypodontia_topright = hypodontia3;
                        per.organ_hypodontia_bottomright = hypodontia4;
                        //per.organ_hypodontia = "1";
                    }
                    if ("checkBox3".Equals(ck.Name))
                    {
                        per.organ_caries_topleft = caries1;
                        per.organ_caries_bottomleft = caries2;
                        per.organ_caries_topright = caries3;
                        per.organ_caries_bottomright = caries4;
                        //per.organ_caries = "1";
                    }
                    if ("checkBox4".Equals(ck.Name))
                    {
                        per.organ_denture_topleft = denture1;
                        per.organ_denture_bottomleft = denture2;
                        per.organ_denture_topright = denture3;
                        per.organ_denture_bottomleft = denture4;
                        //per.organ_denture = "1";
                    }
                }
            }
            if (per.organ_tooth != null && per.organ_tooth != "")
            {
                per.organ_tooth = per.organ_tooth.Substring(1);
            }
            
            if (this.radioButton87.Checked == true) { per.organ_guttur = this.radioButton87.Tag.ToString(); };
            if (this.radioButton88.Checked == true) { per.organ_guttur = this.radioButton88.Tag.ToString(); };
            if (this.radioButton89.Checked == true) { per.organ_guttur = this.radioButton89.Tag.ToString(); };

            per.organ_vision_left = this.textBox9.Text;
            per.organ_vision_right = this.textBox8.Text;
            per.organ_correctedvision_left = this.textBox4.Text;
            per.organ_correctedvision_right = this.textBox51.Text;

            if (this.radioButton52.Checked == true) { per.organ_hearing = this.radioButton52.Tag.ToString(); };
            if (this.radioButton58.Checked == true) { per.organ_hearing = this.radioButton58.Tag.ToString(); };

            if (this.radioButton59.Checked == true) { per.organ_movement = this.radioButton59.Tag.ToString(); };
            if (this.radioButton60.Checked == true) { per.organ_movement = this.radioButton60.Tag.ToString(); };

            if (this.radioButton6.Checked == true) { per.examination_eye = this.radioButton6.Tag.ToString(); };
            if (this.radioButton10.Checked == true) { per.examination_eye = this.radioButton10.Tag.ToString();
            per.examination_eye_other = this.textBox13.Text;
            };
            if (this.radioButton63.Checked == true) { per.examination_skin = this.radioButton63.Tag.ToString(); };
            if (this.radioButton64.Checked == true) { per.examination_skin = this.radioButton64.Tag.ToString(); };
            if (this.radioButton65.Checked == true) { per.examination_skin = this.radioButton65.Tag.ToString(); };
            if (this.radioButton62.Checked == true) { per.examination_skin = this.radioButton62.Tag.ToString(); };
            if (this.radioButton61.Checked == true) { per.examination_skin = this.radioButton61.Tag.ToString(); };
            if (this.radioButton66.Checked == true) { per.examination_skin = this.radioButton66.Tag.ToString(); };
            if (this.radioButton67.Checked == true) { per.examination_skin = this.radioButton67.Tag.ToString();
                per.examination_skin_other = this.textBox14.Text;
            };

            if (this.radioButton72.Checked == true) { per.examination_sclera = this.radioButton72.Tag.ToString(); };
            if (this.radioButton74.Checked == true) { per.examination_sclera = this.radioButton74.Tag.ToString(); };
            if (this.radioButton71.Checked == true) { per.examination_sclera = this.radioButton71.Tag.ToString(); };
            if (this.radioButton68.Checked == true)
            {
                per.examination_sclera = this.radioButton68.Tag.ToString();
                per.examination_sclera_other = this.textBox15.Text;
            };

            if (this.radioButton73.Checked == true) { per.examination_lymph = this.radioButton73.Tag.ToString(); };
            if (this.radioButton75.Checked == true) { per.examination_lymph = this.radioButton75.Tag.ToString(); };
            if (this.radioButton70.Checked == true) { per.examination_lymph = this.radioButton70.Tag.ToString(); };
            if (this.radioButton69.Checked == true)
            {
                per.examination_lymph = this.radioButton69.Tag.ToString();
                per.examination_lymph_other = this.textBox16.Text;
            };

            if (this.radioButton1.Checked == true) { per.examination_barrel_chest = this.radioButton1.Tag.ToString(); };
            if (this.radioButton2.Checked == true) { per.examination_barrel_chest = this.radioButton2.Tag.ToString(); };

            if (this.radioButton3.Checked == true) { per.examination_breath_sounds = this.radioButton3.Tag.ToString(); };
            if (this.radioButton4.Checked == true) { per.examination_breath_sounds = this.radioButton4.Tag.ToString();
                per.examination_breath_other = this.textBox21.Text;
            };

            if (this.radioButton8.Checked == true) { per.examination_rale = this.radioButton8.Tag.ToString(); };
            if (this.radioButton9.Checked == true) { per.examination_rale = this.radioButton9.Tag.ToString(); };
            if (this.radioButton7.Checked == true) { per.examination_rale = this.radioButton7.Tag.ToString(); };
            if (this.radioButton5.Checked == true)
            {
                per.examination_rale = this.radioButton5.Tag.ToString();
                per.examination_rale_other = this.textBox42.Text;
            };

            per.examination_heart_rate = this.textBox47.Text;
            if (this.radioButton16.Checked == true) { per.examination_heart_rhythm = this.radioButton16.Tag.ToString(); };
            if (this.radioButton17.Checked == true) { per.examination_heart_rhythm = this.radioButton17.Tag.ToString(); };
            if (this.radioButton18.Checked == true) { per.examination_heart_rhythm = this.radioButton18.Tag.ToString(); };

            if (this.radioButton77.Checked == true) { per.examination_heart_noise = this.radioButton77.Tag.ToString(); };
            if (this.radioButton78.Checked == true) { per.examination_heart_noise = this.radioButton78.Tag.ToString();
                per.examination_noise_other = this.textBox54.Text;
            };

            if (this.radioButton80.Checked == true) { per.examination_abdomen_tenderness = this.radioButton80.Tag.ToString(); };
            if (this.radioButton81.Checked == true)
            {
                per.examination_abdomen_tenderness = this.radioButton81.Tag.ToString();
                per.examination_tenderness_memo = this.textBox59.Text;
            };

            if (this.radioButton76.Checked == true) { per.examination_abdomen_mass = this.radioButton76.Tag.ToString(); };
            if (this.radioButton79.Checked == true)
            {
                per.examination_abdomen_mass = this.radioButton79.Tag.ToString();
                per.examination_mass_memo = this.textBox56.Text;
            };

            if (this.radioButton82.Checked == true) { per.examination_abdomen_hepatomegaly = this.radioButton82.Tag.ToString(); };
            if (this.radioButton83.Checked == true)
            {
                per.examination_abdomen_hepatomegaly = this.radioButton83.Tag.ToString();
                per.examination_hepatomegaly_memo = this.textBox56.Text;
            };

            if (this.radioButton11.Checked == true) { per.examination_abdomen_splenomegaly = this.radioButton11.Tag.ToString(); };
            if (this.radioButton12.Checked == true)
            {
                per.examination_abdomen_splenomegaly = this.radioButton12.Tag.ToString();
                per.examination_splenomegaly_memo = this.textBox56.Text;
            };

            if (this.radioButton13.Checked == true) { per.examination_abdomen_shiftingdullness = this.radioButton13.Tag.ToString(); };
            if (this.radioButton14.Checked == true)
            {
                per.examination_abdomen_shiftingdullness = this.radioButton14.Tag.ToString();
                per.examination_shiftingdullness_memo = this.textBox23.Text;
            };

            if (this.radioButton20.Checked == true) { per.examination_lowerextremity_edema = this.radioButton20.Tag.ToString(); };
            if (this.radioButton21.Checked == true) { per.examination_lowerextremity_edema = this.radioButton21.Tag.ToString(); };
            if (this.radioButton19.Checked == true) { per.examination_lowerextremity_edema = this.radioButton19.Tag.ToString(); };
            if (this.radioButton15.Checked == true) { per.examination_lowerextremity_edema = this.radioButton15.Tag.ToString(); };

            if (this.radioButton24.Checked == true) { per.examination_dorsal_artery = this.radioButton24.Tag.ToString(); };
            if (this.radioButton25.Checked == true) { per.examination_dorsal_artery = this.radioButton24.Tag.ToString(); };
            if (this.radioButton23.Checked == true) { per.examination_dorsal_artery = this.radioButton23.Tag.ToString(); };
            if (this.radioButton22.Checked == true) { per.examination_dorsal_artery = this.radioButton22.Tag.ToString(); };

            if (this.radioButton28.Checked == true) { per.examination_anus = this.radioButton28.Tag.ToString(); };
            if (this.radioButton29.Checked == true) { per.examination_anus = this.radioButton28.Tag.ToString(); };
            if (this.radioButton27.Checked == true) { per.examination_anus = this.radioButton27.Tag.ToString(); };
            if (this.radioButton26.Checked == true) { per.examination_anus = this.radioButton26.Tag.ToString(); };
            if (this.radioButton30.Checked == true)
            {
                per.examination_anus = this.radioButton30.Tag.ToString();
                per.examination_anus_other = this.textBox28.Text;
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
                        per.examination_breast += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.examination_breast != null && per.examination_breast != "")
            {
                per.examination_breast = per.examination_breast.Substring(1);
                if (this.checkBox9.Checked)
                {
                    per.examination_breast_other = this.textBox30.Text;
                }
            }
            

            if (this.radioButton38.Checked == true) { per.examination_woman_vulva = this.radioButton38.Tag.ToString(); };
            if (this.radioButton39.Checked == true)
            {
                per.examination_woman_vulva = this.radioButton39.Tag.ToString();
                per.examination_vulva_memo = this.textBox33.Text;
            };

            if (this.radioButton36.Checked == true) { per.examination_woman_vagina = this.radioButton36.Tag.ToString(); };
            if (this.radioButton37.Checked == true)
            {
                per.examination_woman_vagina = this.radioButton37.Tag.ToString();
                per.examination_vagina_memo = this.textBox34.Text;
            };

            if (this.radioButton40.Checked == true) { per.examination_woman_cervix = this.radioButton40.Tag.ToString(); };
            if (this.radioButton41.Checked == true)
            {
                per.examination_woman_cervix = this.radioButton41.Tag.ToString();
                per.examination_cervix_memo = this.textBox36.Text;
            };

            if (this.radioButton42.Checked == true) { per.examination_woman_corpus = this.radioButton42.Tag.ToString(); };
            if (this.radioButton43.Checked == true)
            {
                per.examination_woman_corpus = this.radioButton43.Tag.ToString();
                per.examination_corpus_memo = this.textBox38.Text;
            };

            if (this.radioButton44.Checked == true) { per.examination_woman_accessories = this.radioButton44.Tag.ToString(); };
            if (this.radioButton45.Checked == true)
            {
                per.examination_woman_accessories = this.radioButton45.Tag.ToString();
                per.examination_accessories_memo = this.textBox40.Text;
            };
            per.examination_other = this.textBox50.Text;

            per.blood_hemoglobin = this.textBox77.Text;
            per.blood_leukocyte = this.textBox82.Text;
            per.blood_platelet = this.textBox85.Text;
            per.blood_other = this.textBox88.Text;
            per.urine_protein = this.textBox72.Text;
            per.glycosuria = this.textBox71.Text;
            per.urine_acetone_bodies = this.textBox68.Text;
            per.bld = this.textBox67.Text;
            per.urine_other = this.textBox89.Text;
            per.blood_glucose_mmol = this.textBox90.Text;
            per.blood_glucose_mg = this.textBox92.Text;

            if (this.radioButton46.Checked == true) { per.cardiogram = this.radioButton46.Tag.ToString(); };
            if (this.radioButton47.Checked == true)
            {
                per.cardiogram = this.radioButton47.Tag.ToString();
                per.cardiogram_memo = this.textBox94.Text;
            };
            //以下页面未用 数据库字段格式要求
            bool isfalse = hcd.addPhysicalExaminationRecord2(per);
            if (isfalse)
            {
                this.Close();
                aUhealthcheckupServices3 auhc3 = new aUhealthcheckupServices3();
                auhc3.textBox106.Text = per.aichive_no;
                auhc3.textBox105.Text = per.bar_code;
                auhc3.textBox108.Text = per.id;
                auhc3.id = id;//祖
                auhc3.textBox107.Text = this.textBox99.Text;
                auhc3.Show();
            }
            else
            {
                MessageBox.Show("保存不成功!");
            }
        }

        private void checkBox5_Click(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked) {
                this.checkBox6.Checked = false;
                this.checkBox7.Checked = false;
                this.checkBox8.Checked = false;
                this.checkBox9.Checked = false;
                this.textBox30.Text = "";
            }
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked) {
                this.checkBox1.Checked = false;
                aUhealthcheckTooth auhtt = new aUhealthcheckTooth();
                auhtt.tabControl1.TabPages.Remove(auhtt.tabPage2);
                auhtt.tabControl1.TabPages.Remove(auhtt.tabPage3);
                auhtt.hypodontia1 = hypodontia1;
                auhtt.hypodontia2 = hypodontia2;
                auhtt.hypodontia3 = hypodontia3;
                auhtt.hypodontia4 = hypodontia4;
                if (auhtt.ShowDialog() == DialogResult.OK) {
                    hypodontia1 = auhtt.hypodontia1;
                    hypodontia2 = auhtt.hypodontia2;
                    hypodontia3 = auhtt.hypodontia3;
                    hypodontia4 = auhtt.hypodontia4;
                }
            }
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked)
            {
                this.checkBox1.Checked = false;
                aUhealthcheckTooth auhtt = new aUhealthcheckTooth();
                auhtt.tabControl1.TabPages.Remove(auhtt.tabPage1);
                auhtt.tabControl1.TabPages.Remove(auhtt.tabPage3);
                auhtt.caries1 = caries1;
                auhtt.caries2 = caries2;
                auhtt.caries3 = caries3;
                auhtt.caries4 = caries4;
                if (auhtt.ShowDialog() == DialogResult.OK)
                {
                    caries1 = auhtt.caries1;
                    caries2 = auhtt.caries2;
                    caries3 = auhtt.caries3;
                    caries4 = auhtt.caries4;
                }
            }
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {
                this.checkBox1.Checked = false;
                aUhealthcheckTooth auhtt = new aUhealthcheckTooth();
                auhtt.tabControl1.TabPages.Remove(auhtt.tabPage1);
                auhtt.tabControl1.TabPages.Remove(auhtt.tabPage2);
                auhtt.denture1 = denture1;
                auhtt.denture2 = denture2;
                auhtt.denture3 = denture3;
                auhtt.denture4 = denture4;
                if (auhtt.ShowDialog() == DialogResult.OK)
                {
                    denture1 = auhtt.denture1;
                    denture2 = auhtt.denture2;
                    denture3 = auhtt.denture3;
                    denture4 = auhtt.denture4;
                }
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
            }
        }

        private void radioButton6_Click(object sender, EventArgs e)
        {
            if (this.radioButton6.Checked) {
                this.textBox13.Enabled = false;
            }
        }

        private void radioButton10_Click(object sender, EventArgs e)
        {
            if (this.radioButton10.Checked)
            {
                this.textBox13.Enabled = true;
            }
        }

        private void radioButton67_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton67.Checked)
            {
                this.textBox14.Enabled = true;
            }
            else {
                this.textBox14.Enabled = false;
            }
        }

        private void radioButton68_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton68.Checked)
            {
                this.textBox15.Enabled = true;
            }
            else
            {
                this.textBox15.Enabled = false;
            }
        }

        private void radioButton69_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton69.Checked)
            {
                this.textBox16.Enabled = true;
            }
            else
            {
                this.textBox16.Enabled = false;
            }
        }

        private void radioButton78_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton78.Checked)
            {
                this.textBox54.Enabled = true;
            }
            else
            {
                this.textBox54.Enabled = false;
            }
        }

        private void radioButton81_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton81.Checked)
            {
                this.textBox59.Enabled = true;
            }
            else
            {
                this.textBox59.Enabled = false;
            }
        }

        private void radioButton79_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton79.Checked)
            {
                this.textBox56.Enabled = true;
            }
            else
            {
                this.textBox56.Enabled = false;
            }
        }

        private void radioButton83_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton83.Checked)
            {
                this.textBox61.Enabled = true;
            }
            else
            {
                this.textBox61.Enabled = false;
            }
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton12.Checked)
            {
                this.textBox7.Enabled = true;
            }
            else
            {
                this.textBox7.Enabled = false;
            }
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton14.Checked)
            {
                this.textBox23.Enabled = true;
            }
            else
            {
                this.textBox23.Enabled = false;
            }
        }

        private void radioButton30_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton30.Checked)
            {
                this.textBox28.Enabled = true;
            }
            else
            {
                this.textBox28.Enabled = false;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox9.Checked)
            {
                this.textBox30.Enabled = true;
            }
            else
            {
                this.textBox30.Enabled = false;
            }
        }

        private void radioButton39_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton39.Checked)
            {
                this.textBox33.Enabled = true;
            }
            else
            {
                this.textBox33.Enabled = false;
            }
        }

        private void radioButton37_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton37.Checked)
            {
                this.textBox34.Enabled = true;
            }
            else
            {
                this.textBox34.Enabled = false;
            }
        }

        private void radioButton41_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton41.Checked)
            {
                this.textBox36.Enabled = true;
            }
            else
            {
                this.textBox36.Enabled = false;
            }
        }

        private void radioButton43_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton43.Checked)
            {
                this.textBox38.Enabled = true;
            }
            else
            {
                this.textBox38.Enabled = false;
            }
        }

        private void radioButton45_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton45.Checked)
            {
                this.textBox40.Enabled = true;
            }
            else
            {
                this.textBox40.Enabled = false;
            }
        }

        private void radioButton47_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton47.Checked)
            {
                this.textBox94.Enabled = true;
            }
            else
            {
                this.textBox94.Enabled = false;
            }
        }
    }
}
