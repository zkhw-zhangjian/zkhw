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

            if (this.checkBox1.Checked) {
                per.organ_tooth = "1";
            }
            if (this.checkBox2.Checked)
            {
                per.organ_hypodontia = "2";
            }
            if (this.checkBox3.Checked)
            {
                per.organ_caries = "3";
            }
            if (this.checkBox4.Checked)
            {
                per.organ_denture = "4";
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
            };

            if (this.radioButton34.Checked == true) { per.examination_breast = this.radioButton34.Tag.ToString(); };
            if (this.radioButton35.Checked == true) { per.examination_breast = this.radioButton35.Tag.ToString(); };
            if (this.radioButton33.Checked == true) { per.examination_breast = this.radioButton33.Tag.ToString(); };
            if (this.radioButton32.Checked == true) { per.examination_breast = this.radioButton32.Tag.ToString(); };
            if (this.radioButton31.Checked == true)
            {
                per.examination_breast = this.radioButton31.Tag.ToString();
                per.examination_breast_other = this.textBox30.Text;
            };

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
                auhc3.textBox107.Text = this.textBox99.Text;
                auhc3.Show();
            }
            else
            {
                MessageBox.Show("保存不成功!");
            }
        }
    }
}
