using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.publicRecord;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUhealthcheckupServices4 : Form
    {
        public string id = "";
        healthCheckupDao hcd = new healthCheckupDao();
        DataTable goodsList = new DataTable();//用药记录 take_medicine_record
        DataTable goodsListym = new DataTable();//疫苗记录 take_medicine_record
        healthCheckupDao hcdao = new healthCheckupDao();
        public aUhealthcheckupServices4()
        {
            InitializeComponent();
        }
        private void aUdiabetesPatientServices_Load(object sender, EventArgs e)
        {
            this.label51.Text = "健康体检表第四页(共四页)";
            this.label51.ForeColor = Color.SkyBlue;
            label51.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label51.Left = (this.panel1.Width - this.label51.Width) / 2;
            label51.BringToFront();

            DataTable dt = hcdao.queryTake_medicine_record(this.textBox4.Text);
            goodsList = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drtmp = goodsList.NewRow();
                drtmp["drug_name"] = dt.Rows[i]["drug_name"].ToString();
                drtmp["drug_usage"] = dt.Rows[i]["drug_usage"].ToString();
                drtmp["drug_use"] = dt.Rows[i]["drug_use"].ToString();
                drtmp["drug_time"] = dt.Rows[i]["drug_time"].ToString();
                drtmp["drug_type"] = dt.Rows[i]["drug_type"].ToString();
                goodsList.Rows.Add(drtmp);
            }
            goodsListBind();//加载用药记录清单表

            DataTable dtym = hcdao.queryVaccination_record(this.textBox4.Text);
            goodsListym = dtym.Clone();
            for (int i = 0; i < dtym.Rows.Count; i++)
            {
                DataRow drtmp = goodsListym.NewRow();
                drtmp["vaccination_name"] = dtym.Rows[i]["vaccination_name"].ToString();
                drtmp["vaccination_time"] = dtym.Rows[i]["vaccination_time"].ToString();
                drtmp["vaccination_organ_name"] = dtym.Rows[i]["vaccination_organ_name"].ToString();
                goodsListym.Rows.Add(drtmp);
            }
            goodsListBindym();//加载用药记录清单表

            //查询赋值
            if (id != "")
            {
                DataTable dtz = hcd.queryhealthCheckup(id);
                if (dtz != null && dtz.Rows.Count > 0)
                {
                    this.textBox1.Text = dtz.Rows[0]["aichive_no"].ToString();
                    this.textBox2.Text = dtz.Rows[0]["bar_code"].ToString();
                    this.textBox3.Text = dtz.Rows[0]["id_number"].ToString();
                    textBox6.Text= dtz.Rows[0]["name"].ToString();
                    if (this.radioButton39.Tag.ToString() == dtz.Rows[0]["health_evaluation"].ToString())
                    {
                        this.radioButton39.Checked = true;
                    }
                    else if (this.radioButton40.Tag.ToString() == dtz.Rows[0]["health_evaluation"].ToString())
                    {
                        this.radioButton40.Checked = true;
                        this.textBox48.Text = dtz.Rows[0]["abnormal1"].ToString();
                        this.textBox29.Text = dtz.Rows[0]["abnormal2"].ToString();
                        this.textBox31.Text = dtz.Rows[0]["abnormal3"].ToString();
                        this.textBox33.Text = dtz.Rows[0]["abnormal4"].ToString();
                    }
                    else
                    {//异常1
                        string temperature = dtz.Rows[0]["base_temperature"].ToString();
                        if (temperature != null && !"".Equals(temperature))
                        {
                            double temdouble = Convert.ToDouble(temperature);
                            if (temdouble >= 37)
                            {
                                this.textBox48.Text += "体温升高 ";
                            }
                        }

                        string base_bmi = dtz.Rows[0]["base_bmi"].ToString();
                        string base_height = dtz.Rows[0]["base_height"].ToString();
                        if (base_bmi != null && !"".Equals(base_bmi))
                        {
                            double bmidouble = Convert.ToDouble(base_bmi);
                            if (bmidouble > 28)
                            {
                                this.textBox48.Text += "肥胖 ";
                                this.checkBox8.Checked = true;
                                if (base_height != null && !"".Equals(base_height))
                                {
                                    double heightdouble = Convert.ToDouble(base_height);
                                    this.textBox37.Text = (24 * (heightdouble / 100) * (heightdouble / 100)).ToString();
                                }
                            }
                            else if (bmidouble >= 24&&bmidouble < 28)
                            {
                                this.textBox48.Text += "体重超标 ";
                                this.checkBox8.Checked = true;
                                if (base_height != null && !"".Equals(base_height))
                                {
                                    double heightdouble = Convert.ToDouble(base_height);
                                    this.textBox37.Text = (24 * (heightdouble / 100) * (heightdouble / 100)).ToString();
                                }
                            }
                        }
                        string base_waist = dtz.Rows[0]["base_waist"].ToString();
                        if (base_waist != null && !"".Equals(base_waist))
                        {
                            string sexw= this.textBox3.Text.Substring(16,1);
                            int waistint = Int32.Parse(base_waist);
                            if (sexw=="1"&& waistint >95)
                            {
                                this.textBox48.Text += "中心型肥胖 ";
                            }else if (sexw == "2" && waistint > 85)
                            {
                                this.textBox48.Text += "中心型肥胖 ";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(dtz.Rows[0]["base_blood_pressure_right_high"].ToString()) || !string.IsNullOrWhiteSpace(dtz.Rows[0]["base_blood_pressure_right_low"].ToString()))
                        {
                            if (Convert.ToInt32(dtz.Rows[0]["base_blood_pressure_right_high"]) > 140 || Convert.ToInt32(dtz.Rows[0]["base_blood_pressure_right_low"]) > 90)
                            {
                                this.textBox48.Text += "血压偏高 ";
                                this.checkBox1.Checked = true;
                                this.checkBox2.Checked = true;
                            }
                            else if (Convert.ToInt32(dtz.Rows[0]["base_blood_pressure_right_high"]) < 90 || Convert.ToInt32(dtz.Rows[0]["base_blood_pressure_right_low"]) < 60)
                            {
                                this.textBox48.Text += "血压偏低 ";
                            }
                        }
                        string estimate = dtz.Rows[0]["base_selfcare_estimate"].ToString();
                        if (estimate != null && !"".Equals(estimate))
                        {
                            int estimateint = Int32.Parse(estimate);
                            if (estimateint == 3|| estimateint == 4)
                            {
                                this.textBox48.Text += "生活不能自理 ";
                            }
                        }
                        string frequency = dtz.Rows[0]["lifeway_exercise_frequency"].ToString();
                        if (frequency != null && !"".Equals(frequency))
                        {
                            int frequencyint = Int32.Parse(frequency);
                            if (frequencyint == 3 || frequencyint == 4)
                            {
                                this.textBox48.Text += "缺乏规律锻炼 ";
                                this.checkBox7.Checked = true;
                            }
                        }
                        string smoke = dtz.Rows[0]["lifeway_smoke_status"].ToString();
                        if (smoke != null && !"".Equals(smoke))
                        {
                            int smokeint = Int32.Parse(smoke);
                            if (smokeint == 3)
                            {
                                this.textBox48.Text += "吸烟 ";
                                this.checkBox4.Checked = true;
                            }
                        }
                        string drink = dtz.Rows[0]["lifeway_drink_status"].ToString();
                        if (drink != null && !"".Equals(drink))
                        {
                            int drinkint = Int32.Parse(drink);
                            if (drinkint == 3|| drinkint == 4)
                            {
                                this.textBox48.Text += "过量饮酒 ";
                                this.checkBox5.Checked = true;
                            }
                        }
                        //异常2
                        string organ_lips = dtz.Rows[0]["organ_lips"].ToString();
                        if (organ_lips != null && !"".Equals(organ_lips))
                        {
                            int organdouble = Convert.ToInt32(organ_lips);
                            if (organdouble != 1)
                            {
                                this.textBox29.Text += "口唇异常 ";
                            }
                        }

                        string organ_tooth = dtz.Rows[0]["organ_tooth"].ToString();
                        if (organ_tooth != null && !"".Equals(organ_tooth))
                        {
                            int toothdouble = Convert.ToInt32(organ_tooth);
                            if (toothdouble > 0)
                            {
                                this.textBox29.Text += "齿列异常 ";
                            }
                        }

                        string organ_guttur = dtz.Rows[0]["organ_guttur"].ToString();
                        if (organ_guttur != null && !"".Equals(organ_guttur))
                        {
                            int gutturdouble = Convert.ToInt32(organ_guttur);
                            if (gutturdouble > 1)
                            {
                                this.textBox29.Text += "咽部异常 ";
                            }
                        }
                        string organ_vision_left = dtz.Rows[0]["organ_vision_left"].ToString();
                        if (organ_vision_left != null && !"".Equals(organ_vision_left))
                        {
                            Double visionleft = Convert.ToDouble(organ_vision_left);
                            if (visionleft < 4.0)
                            {
                                this.textBox29.Text += "左视力异常 ";
                            }
                        }
                        string organ_vision_right = dtz.Rows[0]["organ_vision_right"].ToString();
                        if (organ_vision_right != null && !"".Equals(organ_vision_right))
                        {
                            Double visionright = Convert.ToDouble(organ_vision_right);
                            if (visionright < 4.0)
                            {
                                this.textBox29.Text += "右视力异常 ";
                            }
                        }
                        string organ_hearing = dtz.Rows[0]["organ_hearing"].ToString();
                        if (organ_hearing != null && !"".Equals(organ_hearing))
                        {
                            int hearingdouble = Convert.ToInt32(organ_hearing);
                            if (hearingdouble > 1)
                            {
                                this.textBox29.Text += "听力异常 ";
                            }
                        }
                        string organ_movement = dtz.Rows[0]["organ_movement"].ToString();
                        if (organ_movement != null && !"".Equals(organ_movement))
                        {
                            int movementdouble = Convert.ToInt32(organ_movement);
                            if (movementdouble > 1)
                            {
                                this.textBox29.Text += "运动无法完成 ";
                            }
                        }
                        string examination_skin = dtz.Rows[0]["examination_skin"].ToString();
                        if (examination_skin != null && !"".Equals(examination_skin))
                        {
                            int skindouble = Convert.ToInt32(examination_skin);
                            if (skindouble > 1)
                            {
                                this.textBox29.Text += "皮肤异常 ";
                            }
                        }
                        string examination_sclera = dtz.Rows[0]["examination_sclera"].ToString();
                        if (examination_sclera != null && !"".Equals(examination_sclera))
                        {
                            int scleradouble = Convert.ToInt32(examination_sclera);
                            if (scleradouble > 1)
                            {
                                this.textBox29.Text += "巩膜异常 ";
                            }
                        }
                        string examination_lymph = dtz.Rows[0]["examination_lymph"].ToString();
                        if (examination_lymph != null && !"".Equals(examination_lymph))
                        {
                            int lymphdouble = Convert.ToInt32(examination_lymph);
                            if (lymphdouble > 1)
                            {
                                this.textBox29.Text += "有淋巴结 ";
                            }
                        }
                        string examination_barrel_chest = dtz.Rows[0]["examination_barrel_chest"].ToString();
                        if (examination_barrel_chest != null && !"".Equals(examination_barrel_chest))
                        {
                            int barrel_chestdouble = Convert.ToInt32(examination_barrel_chest);
                            if (barrel_chestdouble > 1)
                            {
                                this.textBox29.Text += "是桶状胸 ";
                            }
                        }
                        string examination_breath_sounds = dtz.Rows[0]["examination_breath_sounds"].ToString();
                        if (examination_breath_sounds != null && !"".Equals(examination_breath_sounds))
                        {
                            int breath_soundsdouble = Convert.ToInt32(examination_breath_sounds);
                            if (breath_soundsdouble > 1)
                            {
                                this.textBox29.Text += "呼吸音异常 ";
                            }
                        }
                        string examination_rale = dtz.Rows[0]["examination_rale"].ToString();
                        if (examination_rale != null && !"".Equals(examination_rale))
                        {
                            int raledouble = Convert.ToInt32(examination_rale);
                            if (raledouble > 1)
                            {
                                this.textBox29.Text += "罗音异常 ";
                            }
                        }
                        string examination_heart_rate = dtz.Rows[0]["examination_heart_rate"].ToString();
                        if (examination_heart_rate != null && !"".Equals(examination_heart_rate))
                        {
                            int hraledouble = Convert.ToInt32(examination_heart_rate);
                            if (hraledouble > 100)
                            {
                                this.textBox29.Text += "心率偏快 ";
                            }
                            else if (hraledouble < 60)
                            {
                                this.textBox29.Text += "心率偏慢 ";
                            }
                        }
                        string examination_heart_rhythm = dtz.Rows[0]["examination_heart_rhythm"].ToString();
                        if (examination_heart_rhythm != null && !"".Equals(examination_heart_rhythm))
                        {
                            int rhythmdouble = Convert.ToInt32(examination_heart_rhythm);
                            if (rhythmdouble > 1)
                            {
                                this.textBox29.Text += "心律不齐 ";
                            }
                        }
                        string examination_heart_noise = dtz.Rows[0]["examination_heart_noise"].ToString();
                        if (examination_heart_noise != null && !"".Equals(examination_heart_noise))
                        {
                            int noisedouble = Convert.ToInt32(examination_heart_noise);
                            if (noisedouble > 1)
                            {
                                this.textBox29.Text += "心脏有杂音 ";
                            }
                        }
                        string examination_abdomen_tenderness = dtz.Rows[0]["examination_abdomen_tenderness"].ToString();
                        if (examination_abdomen_tenderness != null && !"".Equals(examination_abdomen_tenderness))
                        {
                            int tendernessdouble = Convert.ToInt32(examination_abdomen_tenderness);
                            if (tendernessdouble > 1)
                            {
                                this.textBox29.Text += "腹部有压痛 ";
                            }
                        }
                        string examination_abdomen_mass = dtz.Rows[0]["examination_abdomen_mass"].ToString();
                        if (examination_abdomen_mass != null && !"".Equals(examination_abdomen_mass))
                        {
                            int massdouble = Convert.ToInt32(examination_abdomen_mass);
                            if (massdouble > 1)
                            {
                                this.textBox29.Text += "腹部有包块 ";
                            }
                        }
                        string examination_abdomen_hepatomegaly = dtz.Rows[0]["examination_abdomen_hepatomegaly"].ToString();
                        if (examination_abdomen_hepatomegaly != null && !"".Equals(examination_abdomen_hepatomegaly))
                        {
                            int hepatomegalydouble = Convert.ToInt32(examination_abdomen_hepatomegaly);
                            if (hepatomegalydouble > 1)
                            {
                                this.textBox29.Text += "腹部肝大 ";
                            }
                        }
                        string examination_abdomen_splenomegaly = dtz.Rows[0]["examination_abdomen_splenomegaly"].ToString();
                        if (examination_abdomen_splenomegaly != null && !"".Equals(examination_abdomen_splenomegaly))
                        {
                            int splenomegalydouble = Convert.ToInt32(examination_abdomen_splenomegaly);
                            if (splenomegalydouble > 1)
                            {
                                this.textBox29.Text += "腹部脾大 ";
                            }
                        }
                        string examination_abdomen_shiftingdullness = dtz.Rows[0]["examination_abdomen_shiftingdullness"].ToString();
                        if (examination_abdomen_shiftingdullness != null && !"".Equals(examination_abdomen_shiftingdullness))
                        {
                            int shiftingdullnessdouble = Convert.ToInt32(examination_abdomen_shiftingdullness);
                            if (shiftingdullnessdouble > 1)
                            {
                                this.textBox29.Text += "腹部有移动性浊音 ";
                            }
                        }
                        string examination_lowerextremity_edema = dtz.Rows[0]["examination_lowerextremity_edema"].ToString();
                        if (examination_lowerextremity_edema != null && !"".Equals(examination_lowerextremity_edema))
                        {
                            int edemadouble = Convert.ToInt32(examination_lowerextremity_edema);
                            if (edemadouble > 1)
                            {
                                this.textBox29.Text += "下肢有水肿 ";
                            }
                        }
                        //异常3
                        bool xcgflag = false;
                        string blood_hemoglobin = dtz.Rows[0]["blood_hemoglobin"].ToString();
                        if (blood_hemoglobin != null && !"".Equals(blood_hemoglobin))
                        {
                            if (Convert.ToDouble(blood_hemoglobin) < 110 || Convert.ToDouble(blood_hemoglobin) > 160)
                            {
                                xcgflag = true;
                            }
                        }
                        string blood_leukocyte = dtz.Rows[0]["blood_leukocyte"].ToString();
                        if (blood_leukocyte != null && !"".Equals(blood_leukocyte))
                        {
                            if (Convert.ToDouble(blood_leukocyte) >10)
                            {
                                xcgflag = true;
                            }
                        }
                        string blood_platelet = dtz.Rows[0]["blood_platelet"].ToString();
                        if (blood_platelet != null && !"".Equals(blood_platelet))
                        {
                            if (Convert.ToDouble(blood_platelet) > 300 || Convert.ToDouble(blood_platelet)<100)
                            {
                                xcgflag = true;
                            }
                        }
                        if(xcgflag) this.textBox31.Text += "血常规有异常 ";

                        bool ncgflag = false;
                        string urine_protein = dtz.Rows[0]["urine_protein"].ToString();
                        if (urine_protein != null && !"".Equals(urine_protein)&& urine_protein != "-")
                        {
                             ncgflag = true;
                        }
                        string glycosuria = dtz.Rows[0]["glycosuria"].ToString();
                        if (glycosuria != null && !"".Equals(glycosuria) && glycosuria != "-")
                        {
                            ncgflag = true;
                        }
                        string urine_acetone_bodies = dtz.Rows[0]["urine_acetone_bodies"].ToString();
                        if (urine_acetone_bodies != null && !"".Equals(urine_acetone_bodies) && urine_acetone_bodies != "-")
                        {
                            ncgflag = true;
                        }
                        string bld = dtz.Rows[0]["bld"].ToString();
                        if (bld != null && !"".Equals(bld) && bld != "-")
                        {
                            ncgflag = true;
                        }
                        if (ncgflag) this.textBox31.Text += "尿常规有异常 ";

                        string blood_glucose_mmol = dtz.Rows[0]["blood_glucose_mmol"].ToString();
                        if (blood_glucose_mmol != null && !"".Equals(blood_glucose_mmol))
                        {
                            if (Convert.ToDouble(blood_glucose_mmol) >7) { this.textBox31.Text += "空腹血糖值偏高 "; this.checkBox1.Checked = true; }
                        }
                        string cardiogram = dtz.Rows[0]["cardiogram"].ToString();
                        if (cardiogram=="2")
                        {
                            this.textBox31.Text += "心电图异常 "; 
                        }
                        
                        bool shflag = false;
                        string sgft = dtz.Rows[0]["sgft"].ToString();
                        if (sgft != null && !"".Equals(sgft))
                        {
                            if (Convert.ToDouble(sgft) > 40) { shflag = true; }
                        }
                        string ast = dtz.Rows[0]["ast"].ToString();
                        if (ast != null && !"".Equals(ast))
                        {
                            if (Convert.ToDouble(ast) > 40) { shflag = true; }
                        }
                        string albumin = dtz.Rows[0]["albumin"].ToString();
                        if (albumin != null && !"".Equals(albumin))
                        {
                            if (Convert.ToDouble(albumin) > 54|| Convert.ToDouble(albumin)<34) { shflag = true; }
                        }
                        string total_bilirubin = dtz.Rows[0]["total_bilirubin"].ToString();
                        if (total_bilirubin != null && !"".Equals(total_bilirubin))
                        {
                            if (Convert.ToDouble(total_bilirubin) > 20 || Convert.ToDouble(total_bilirubin) < 2) { shflag = true; }
                        }
                        string conjugated_bilirubin = dtz.Rows[0]["conjugated_bilirubin"].ToString();
                        if (conjugated_bilirubin != null && !"".Equals(conjugated_bilirubin))
                        {
                            if (Convert.ToDouble(conjugated_bilirubin) > 6.8 || Convert.ToDouble(conjugated_bilirubin) < 1.7) { shflag = true; }
                        }
                        string scr = dtz.Rows[0]["scr"].ToString();
                        if (scr != null && !"".Equals(scr))
                        {
                            if (Convert.ToDouble(scr) > 115 || Convert.ToDouble(scr) < 44) { shflag = true; }
                        }
                        string blood_urea = dtz.Rows[0]["blood_urea"].ToString();
                        if (blood_urea != null && !"".Equals(blood_urea))
                        {
                            if (Convert.ToDouble(blood_urea) > 8.2 || Convert.ToDouble(blood_urea) < 1.7) { shflag = true; }
                        }
                        string tc = dtz.Rows[0]["tc"].ToString();
                        if (tc != null && !"".Equals(tc))
                        {
                            if (Convert.ToDouble(tc) > 5.2) { shflag = true; }
                        }
                        string tg = dtz.Rows[0]["tg"].ToString();
                        if (tg != null && !"".Equals(tg))
                        {
                            if (Convert.ToDouble(tg) > 1.7) { shflag = true; }
                        }
                        string ldl = dtz.Rows[0]["ldl"].ToString();
                        if (ldl != null && !"".Equals(ldl))
                        {
                            if (Convert.ToDouble(ldl) > 3.9|| Convert.ToDouble(ldl)<1.5) { shflag = true; }
                        }
                        string hdl = dtz.Rows[0]["hdl"].ToString();
                        if (hdl != null && !"".Equals(hdl))
                        {
                            if (Convert.ToDouble(hdl) > 1.9 || Convert.ToDouble(hdl) < 0.9) { shflag = true; }
                        }
                        if (shflag) this.textBox31.Text += "生化有异常 ";
                        if (dtz.Rows[0]["ultrasound_abdomen"].ToString() == "2")
                        {
                            this.textBox31.Text +="B超异常 ";
                        }
                        if (xcgflag||ncgflag||shflag|| cardiogram == "2"|| dtz.Rows[0]["ultrasound_abdomen"].ToString() == "2") {
                            this.checkBox2.Checked = true;
                        }
                        //异常4
                        string cerebrovascular = dtz.Rows[0]["cerebrovascular_disease"].ToString();
                        if (cerebrovascular != null && !"".Equals(cerebrovascular))
                        {
                            if (cerebrovascular.IndexOf("2")>-1|| cerebrovascular.IndexOf("3") > -1|| cerebrovascular.IndexOf("4") > -1|| cerebrovascular.IndexOf("5") > -1|| cerebrovascular.IndexOf("6") > -1)
                            {
                                this.textBox33.Text += "脑血管疾病 ";
                            }
                        }
                        string kidney_disease = dtz.Rows[0]["kidney_disease"].ToString();
                        if (kidney_disease != null && !"".Equals(kidney_disease))
                        {
                            if (kidney_disease.IndexOf("2") > -1 || kidney_disease.IndexOf("3") > -1 || kidney_disease.IndexOf("4") > -1 || kidney_disease.IndexOf("5") > -1 || kidney_disease.IndexOf("6") > -1)
                            {
                                this.textBox33.Text += "肾脏疾病 ";
                            }
                        }
                        string heart_disease = dtz.Rows[0]["heart_disease"].ToString();
                        if (heart_disease != null && !"".Equals(heart_disease))
                        {
                            if (heart_disease.IndexOf("2") > -1 || heart_disease.IndexOf("3") > -1 || heart_disease.IndexOf("4") > -1 || heart_disease.IndexOf("5") > -1 || heart_disease.IndexOf("6") > -1 || heart_disease.IndexOf("7") > -1)
                            {
                                this.textBox33.Text += "心脏疾病 ";
                            }
                        }
                        string vascular_disease = dtz.Rows[0]["vascular_disease"].ToString();
                        if (vascular_disease != null && !"".Equals(vascular_disease))
                        {
                            if (vascular_disease.IndexOf("2") > -1 || vascular_disease.IndexOf("3") > -1 || vascular_disease.IndexOf("4") > -1)
                            {
                                this.textBox33.Text += "血管疾病 ";
                            }
                        }
                        string ocular_diseases = dtz.Rows[0]["ocular_diseases"].ToString();
                        if (ocular_diseases != null && !"".Equals(ocular_diseases))
                        {
                            if (ocular_diseases.IndexOf("2") > -1 || ocular_diseases.IndexOf("3") > -1 || ocular_diseases.IndexOf("4") > -1 || ocular_diseases.IndexOf("5") > -1)
                            {
                                this.textBox33.Text += "眼部疾病 ";
                            }
                        }
                        string nervous = dtz.Rows[0]["nervous_system_disease"].ToString();
                        if (nervous != null && !"".Equals(nervous))
                        {
                            int nervousint = Int32.Parse(nervous);
                            if (nervousint == 2)
                            {
                                this.textBox33.Text += "神经系统疾病 ";
                            }
                        }
                        string other_disease = dtz.Rows[0]["other_disease"].ToString();
                        if (other_disease != null && !"".Equals(other_disease))
                        {
                            int otherint = Int32.Parse(other_disease);
                            if (otherint == 2)
                            {
                                this.textBox33.Text += "其他系统疾病 ";
                            }
                        }
                    }

                    foreach (Control ctr in this.panel2.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dtz.Rows[0]["health_guidance"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }

                    foreach (Control ctr in this.panel3.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            
                            if (dtz.Rows[0]["danger_controlling"].ToString().IndexOf(ck.Tag.ToString()) > -1)
                            {
                               ck.Checked = true;
                            }
                        }
                    }
                    if (this.checkBox8.Checked)
                    {
                        this.textBox37.Text = dtz.Rows[0]["target_weight"].ToString();
                    }
                    if (this.checkBox9.Checked)
                    {
                        this.textBox39.Text = dtz.Rows[0]["advise_bacterin"].ToString();
                    }
                    if (this.checkBox10.Checked)
                    {
                       this.textBox40.Text = dtz.Rows[0]["danger_controlling_other"].ToString();                     
                    }
                    this.richTextBox1.Text = dtz.Rows[0]["healthAdvice"].ToString();
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
           
            if (this.radioButton39.Checked == true) { per.health_evaluation = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true) {
                per.health_evaluation = this.radioButton40.Tag.ToString();
                per.abnormal1 = this.textBox48.Text;
                per.abnormal2 = this.textBox29.Text;
                per.abnormal3 = this.textBox31.Text;
                per.abnormal4 = this.textBox33.Text;
            };

            foreach (Control ctr in this.panel2.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.health_guidance += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.health_guidance != null && per.health_guidance != "")
            {
                per.health_guidance = per.health_guidance.Substring(1);
            }

            foreach (Control ctr in this.panel3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.danger_controlling += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.danger_controlling != null && per.danger_controlling != "")
            {
                per.danger_controlling = per.danger_controlling.Substring(1);
            }
            if (checkBox8.Checked) {
                per.target_weight= this.textBox37.Text;
            }
            if (checkBox9.Checked)
            {
                per.advise_bacterin = this.textBox39.Text;
            }
            if (checkBox10.Checked)
            {
                per.danger_controlling_other = this.textBox40.Text;
            }
            per.healthAdvice=this.richTextBox1.Text;
            if (per.healthAdvice==null||"".Equals(per.healthAdvice)) {
                MessageBox.Show("健康建议不能为空!");return;
            }
            per.aichive_no = this.textBox1.Text;
            per.bar_code = this.textBox2.Text;
            per.id_number = this.textBox3.Text;
            per.id = this.textBox4.Text;
            per.name = textBox6.Text;
            bool isfalse = hcd.addPhysicalExaminationRecord4(per, goodsList, goodsListym);
            if (isfalse)
            {
                //更新
                tjcheckDao tjdao = new tjcheckDao();
                tjdao.updateTJbgdcjktjb(per.id_number, per.bar_code, "1");

                MessageBox.Show("数据保存成功!");
                if (per.id_number!=""&& per.id_number.Length==18) {
                  string yl = per.id_number.Substring(6, 4) + "-" + per.id_number.Substring(10, 2) + "-" + per.id_number.Substring(12, 2);
                  int year1 = DateTime.Parse(yl).Year;
                  int year2 = DateTime.Now.Year;
                    if (year2-year1>=65) {
                        //MessageBox.Show("此患者已满65周岁,还需要填写老年人生活自理能力评估表，和老年人中医体质表!");
                        DialogResult rr = MessageBox.Show("是否要做老年人中医体质辨识？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        int tt = (int)rr;
                        if(tt==1)
                        { 
                            //MessageBox.Show("老年人中医体质辨识"+ per.id);
                            int ret = 0;
                            service.tcmHealthService tcmHealthService = new service.tcmHealthService();
                            DataTable dtcode = tcmHealthService.checkTcmHealthServicesByExamID(per.id);
                            if (dtcode.Rows.Count > 0)
                            {
                                ret = 0;
                            }
                            else
                            {
                                ret = 1;
                            }
                            addtcmHealthServices addtcm = new addtcmHealthServices(ret, per.name, per.aichive_no, per.id_number);
                            addtcm.bar_code = per.bar_code;
                            addtcm.exam_id = per.id;
                            addtcm.StartPosition = FormStartPosition.CenterScreen;
                            addtcm.ShowDialog();
                        }
                    }
                }
            }
        }
        //将用药记录 goodsList 绑定到页面 dataGridView1展示出来
        private void goodsListBind()
        {
            this.dataGridView1.DataSource = goodsList;
            this.dataGridView1.Columns[0].HeaderCell.Value = "药物名称";
            this.dataGridView1.Columns[1].HeaderCell.Value = "药物用法";
            this.dataGridView1.Columns[2].HeaderCell.Value = "药物用量";
            this.dataGridView1.Columns[3].HeaderCell.Value = "用药时间";
            this.dataGridView1.Columns[4].HeaderCell.Value = "服药依从性";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                this.dataGridView1.SelectedRows[0].Selected = false;
            }
            if (goodsList != null && goodsList.Rows.Count > 0)
            {
                this.dataGridView1.Rows[goodsList.Rows.Count - 1].Selected = true;
            }
        }
        //将疫苗记录 goodsList 绑定到页面 dataGridView1展示出来
        private void goodsListBindym()
        {
            this.dataGridView2.DataSource = goodsListym;
            this.dataGridView2.Columns[0].HeaderCell.Value = "疫苗名称";
            this.dataGridView2.Columns[1].HeaderCell.Value = "接种时间";
            this.dataGridView2.Columns[2].HeaderCell.Value = "接种机构";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (this.dataGridView2.SelectedRows.Count > 0)
            {
                this.dataGridView2.SelectedRows[0].Selected = false;
            }
            if (goodsListym != null && goodsListym.Rows.Count > 0)
            {
                this.dataGridView2.Rows[goodsListym.Rows.Count - 1].Selected = true;
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            take_medicine_record tmr = new take_medicine_record();
            if (tmr.ShowDialog() == DialogResult.OK)
            {
                DataRow[] drr = goodsList.Select("drug_name = '" + tmr.drug_name.ToString() + "'");
                if (drr.Length > 0)
                {
                    MessageBox.Show("用药记录已存在！");
                    return;
                }
                DataRow drtmp = goodsList.NewRow();
                drtmp["drug_name"] = tmr.drug_name;
                drtmp["drug_usage"] = tmr.drug_usage;
                drtmp["drug_use"] = tmr.drug_use;
                drtmp["drug_time"] = tmr.drug_time;
                drtmp["drug_type"] = tmr.drug_type;
                goodsList.Rows.Add(drtmp);
            }
            goodsListBind();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (goodsList == null) { return; }
            if (goodsList.Rows.Count > 0)
            {
                goodsList.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                goodsListBind();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            vaccination_record vr = new vaccination_record();
            if (vr.ShowDialog() == DialogResult.OK)
            {
                DataRow[] drr = goodsListym.Select("vaccination_name = '" + vr.vaccination_name + "'");
                if (drr.Length > 0)
                {
                    MessageBox.Show("疫苗记录已存在！");
                    return;
                }
                DataRow drtmp = goodsListym.NewRow();
                drtmp["vaccination_name"] = vr.vaccination_name;
                drtmp["vaccination_time"] = vr.vaccination_time;
                drtmp["vaccination_organ_name"] = vr.vaccination_organ;
                goodsListym.Rows.Add(drtmp);
            }
            goodsListBind();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (goodsListym == null) { return; }
            if (goodsListym.Rows.Count > 0)
            {
                goodsListym.Rows.RemoveAt(this.dataGridView2.SelectedRows[0].Index);
                goodsListBindym();
            }
        }

        private void radioButton40_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton40.Checked)
            {
                this.textBox48.Enabled = true;
                this.textBox29.Enabled = true;
                this.textBox31.Enabled = true;
                this.textBox33.Enabled = true;
            }
            else {
                this.textBox48.Enabled = false;
                this.textBox29.Enabled = false;
                this.textBox31.Enabled = false;
                this.textBox33.Enabled = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();

            if (this.radioButton39.Checked == true) { per.health_evaluation = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true)
            {
                per.health_evaluation = this.radioButton40.Tag.ToString();
                per.abnormal1 = this.textBox48.Text;
                per.abnormal2 = this.textBox29.Text;
                per.abnormal3 = this.textBox31.Text;
                per.abnormal4 = this.textBox33.Text;
            };

            foreach (Control ctr in this.panel2.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.health_guidance += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.health_guidance != null && per.health_guidance != "")
            {
                per.health_guidance = per.health_guidance.Substring(1);
            }

            foreach (Control ctr in this.panel3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.danger_controlling += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.danger_controlling != null && per.danger_controlling != "")
            {
                per.danger_controlling = per.danger_controlling.Substring(1);
            }
            if (checkBox8.Checked)
            {
                per.target_weight = this.textBox37.Text;
            }
            if (checkBox9.Checked)
            {
                per.advise_bacterin = this.textBox39.Text;
            }
            if (checkBox10.Checked)
            {
                per.danger_controlling_other = this.textBox40.Text;
            }
            per.healthAdvice = this.richTextBox1.Text;
            if (per.healthAdvice == null || "".Equals(per.healthAdvice))
            {
                MessageBox.Show("健康建议不能为空!"); return;
            }
            per.aichive_no = this.textBox1.Text;
            per.bar_code = this.textBox2.Text;
            per.id_number = this.textBox3.Text;
            per.id = this.textBox4.Text;
            bool isfalse = hcd.addPhysicalExaminationRecord4(per, goodsList, goodsListym);
            this.Close();
            aUhealthcheckupServices3 auhc3 = new aUhealthcheckupServices3();
            auhc3.textBox106.Text = this.textBox1.Text;
            auhc3.textBox105.Text = this.textBox2.Text;
            auhc3.textBox107.Text = this.textBox3.Text;
            auhc3.id = per.id;//祖
            auhc3.textBox108.Text = per.id;
            auhc3.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();

            if (this.radioButton39.Checked == true) { per.health_evaluation = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true)
            {
                per.health_evaluation = this.radioButton40.Tag.ToString();
                per.abnormal1 = this.textBox48.Text;
                per.abnormal2 = this.textBox29.Text;
                per.abnormal3 = this.textBox31.Text;
                per.abnormal4 = this.textBox33.Text;
            };

            foreach (Control ctr in this.panel2.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.health_guidance += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.health_guidance != null && per.health_guidance != "")
            {
                per.health_guidance = per.health_guidance.Substring(1);
            }

            foreach (Control ctr in this.panel3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.danger_controlling += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.danger_controlling != null && per.danger_controlling != "")
            {
                per.danger_controlling = per.danger_controlling.Substring(1);
            }
            if (checkBox8.Checked)
            {
                per.target_weight = this.textBox37.Text;
            }
            if (checkBox9.Checked)
            {
                per.advise_bacterin = this.textBox39.Text;
            }
            if (checkBox10.Checked)
            {
                per.danger_controlling_other = this.textBox40.Text;
            }
            per.healthAdvice = this.richTextBox1.Text;
            if (per.healthAdvice == null || "".Equals(per.healthAdvice))
            {
                MessageBox.Show("健康建议不能为空!"); return;
            }
            per.aichive_no = this.textBox1.Text;
            per.bar_code = this.textBox2.Text;
            per.id_number = this.textBox3.Text;
            per.id = this.textBox4.Text;
            bool isfalse = hcd.addPhysicalExaminationRecord4(per, goodsList, goodsListym);
            this.Close();
            aUhealthcheckupServices1 auhc1 = new aUhealthcheckupServices1();
            auhc1.textBox2.Text = this.textBox1.Text;
            auhc1.textBox118.Text = this.textBox2.Text;
            auhc1.textBox120.Text = per.id;
            auhc1.id = per.id;//祖
            auhc1.textBox119.Text = this.textBox3.Text;
            auhc1.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            bean.physical_examination_recordBean per = new bean.physical_examination_recordBean();

            if (this.radioButton39.Checked == true) { per.health_evaluation = this.radioButton39.Tag.ToString(); };
            if (this.radioButton40.Checked == true)
            {
                per.health_evaluation = this.radioButton40.Tag.ToString();
                per.abnormal1 = this.textBox48.Text;
                per.abnormal2 = this.textBox29.Text;
                per.abnormal3 = this.textBox31.Text;
                per.abnormal4 = this.textBox33.Text;
            };

            foreach (Control ctr in this.panel2.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.health_guidance += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.health_guidance != null && per.health_guidance != "")
            {
                per.health_guidance = per.health_guidance.Substring(1);
            }

            foreach (Control ctr in this.panel3.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        per.danger_controlling += "," + ck.Tag.ToString();
                    }
                }
            }
            if (per.danger_controlling != null && per.danger_controlling != "")
            {
                per.danger_controlling = per.danger_controlling.Substring(1);
            }
            if (checkBox8.Checked)
            {
                per.target_weight = this.textBox37.Text;
            }
            if (checkBox9.Checked)
            {
                per.advise_bacterin = this.textBox39.Text;
            }
            if (checkBox10.Checked)
            {
                per.danger_controlling_other = this.textBox40.Text;
            }
            per.healthAdvice = this.richTextBox1.Text.Trim().Replace(" ","");
            if (per.healthAdvice == null || "".Equals(per.healthAdvice))
            {
                MessageBox.Show("健康建议不能为空!"); return;
            }
            per.aichive_no = this.textBox1.Text;
            per.bar_code = this.textBox2.Text;
            per.id_number = this.textBox3.Text;
            per.id = this.textBox4.Text;
            bool isfalse = hcd.addPhysicalExaminationRecord4(per, goodsList, goodsListym);
            this.Close();
            aUhealthcheckupServices2 auhc2 = new aUhealthcheckupServices2();
            auhc2.textBox95.Text = this.textBox1.Text;
            auhc2.textBox96.Text = this.textBox2.Text;
            auhc2.textBox100.Text = per.id;
            auhc2.id = id;//祖
            auhc2.textBox99.Text = this.textBox3.Text;
            auhc2.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string jkjy = this.comboBox1.Text;
            if (jkjy.IndexOf("--") == -1)
            {
                this.richTextBox1.Text += jkjy;
            }
        }
    }
}
