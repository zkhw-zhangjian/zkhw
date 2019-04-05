using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUolderHelthService : Form
    {

        service.personalBasicInfoService personalBasicInfoService = new service.personalBasicInfoService();
        public string id = "";
        public aUolderHelthService()
        {
            InitializeComponent();
        }
        private void aUHypertensionPatientServices_Load(object sender, EventArgs e)
        {
            this.label47.ForeColor = Color.SkyBlue;
            label47.Font = new Font("微软雅黑", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label47.Left = (this.panel11.Width - this.label47.Width) / 2;
            label47.BringToFront();

            this.label1.Text = "该表为自评表，根据下表中 5 个方面进行评估，将各方面判断评分汇总后，0 ～ 3 分者为可自理；4 ～ 8 分者为轻度依赖；9 ～ 18 分者为中度依赖； 19 分者为不能自理。";
            label1.Left = (this.panel11.Width - this.label1.Width) / 2;

            this.textBox16.Text += System.Environment.NewLine;
            this.textBox16.Text += "独立完成";
            this.textBox16.Text += System.Environment.NewLine;
            this.textBox16.Text += System.Environment.NewLine;
            this.textBox16.Text += System.Environment.NewLine;
            this.textBox16.Text += System.Environment.NewLine;
            this.textBox16.Text += "0";

            this.textBox17.Text += System.Environment.NewLine;
            this.textBox17.Text += "-";
            this.textBox17.Text += System.Environment.NewLine;
            this.textBox17.Text += System.Environment.NewLine;
            this.textBox17.Text += System.Environment.NewLine;
            this.textBox17.Text += System.Environment.NewLine;
            this.textBox17.Text += "0";

            this.textBox18.Text += System.Environment.NewLine;
            this.textBox18.Text += "需要协助，如";
            this.textBox18.Text += System.Environment.NewLine;
            this.textBox18.Text += "切碎、搅拌食";
            this.textBox18.Text += System.Environment.NewLine;
            this.textBox18.Text += "物等";
            this.textBox18.Text += System.Environment.NewLine;
            this.textBox18.Text += System.Environment.NewLine;
            this.textBox18.Text += "3";

            this.textBox19.Text += System.Environment.NewLine;
            this.textBox19.Text += "完全需要帮";
            this.textBox19.Text += System.Environment.NewLine;
            this.textBox19.Text += "助";
            this.textBox19.Text += System.Environment.NewLine;
            this.textBox19.Text += System.Environment.NewLine;
            this.textBox19.Text += System.Environment.NewLine;
            this.textBox19.Text += "3";
///////////////////////////////////////////////
            this.textBox22.Text += System.Environment.NewLine;
            this.textBox22.Text += "独立完成";
            this.textBox22.Text += System.Environment.NewLine;
            this.textBox22.Text += System.Environment.NewLine;
            this.textBox22.Text += System.Environment.NewLine;
            this.textBox22.Text += System.Environment.NewLine;
            this.textBox22.Text += "0";

            this.textBox23.Text += System.Environment.NewLine;
            this.textBox23.Text += "能独立地洗头、梳";
            this.textBox23.Text += System.Environment.NewLine;
            this.textBox23.Text += "头、洗脸、刷牙、剃";
            this.textBox23.Text += System.Environment.NewLine;
            this.textBox23.Text += "须等；洗澡需要协助";
            this.textBox23.Text += System.Environment.NewLine;
            this.textBox23.Text += System.Environment.NewLine;
            this.textBox23.Text += "1";

            this.textBox24.Text += System.Environment.NewLine;
            this.textBox24.Text += "在协助下和适当";
            this.textBox24.Text += System.Environment.NewLine;
            this.textBox24.Text += "的时间内能完，";
            this.textBox24.Text += System.Environment.NewLine;
            this.textBox24.Text += "成部分梳洗活动";
            this.textBox24.Text += System.Environment.NewLine;
            this.textBox24.Text += System.Environment.NewLine;
            this.textBox24.Text += "3";

            this.textBox25.Text += System.Environment.NewLine;
            this.textBox25.Text += "完全需要帮";
            this.textBox25.Text += System.Environment.NewLine;
            this.textBox25.Text += "助";
            this.textBox25.Text += System.Environment.NewLine;
            this.textBox25.Text += System.Environment.NewLine;
            this.textBox25.Text += System.Environment.NewLine;
            this.textBox25.Text += "7";
            ////////////////////////////////////////////////////////////////
            this.textBox28.Text += System.Environment.NewLine;
            this.textBox28.Text += "独立完成";
            this.textBox28.Text += System.Environment.NewLine;
            this.textBox28.Text += System.Environment.NewLine;
            this.textBox28.Text += System.Environment.NewLine;
            this.textBox28.Text += System.Environment.NewLine;
            this.textBox28.Text += "0";


        }




        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //bean.resident_base_infoBean resident_base_infoBean = new bean.resident_base_infoBean();

            //resident_base_infoBean.name = this.textBox1.Text.Replace(" ", "");
            //resident_base_infoBean.archive_no = this.textBox2.Text.Replace(" ", "");
            //resident_base_infoBean.pb_archive = this.textBox2.Text.Replace(" ", "");
            //if (this.radioButton1.Checked == true) { resident_base_infoBean.sex = this.radioButton1.Text; };
            //if (this.radioButton2.Checked == true) { resident_base_infoBean.sex = this.radioButton2.Text; };
            //if (this.radioButton3.Checked == true) { resident_base_infoBean.sex = this.radioButton3.Text; };
            //if (this.radioButton25.Checked == true) { resident_base_infoBean.sex = this.radioButton25.Text; };
            //resident_base_infoBean.birthday = this.dateTimePicker1.Text;
            //resident_base_infoBean.id_number = this.textBox12.Text.Replace(" ", "");
            //resident_base_infoBean.company = this.textBox14.Text.Replace(" ", "");
            //resident_base_infoBean.phone = this.textBox16.Text.Replace(" ", "");
            //resident_base_infoBean.link_name = this.textBox18.Text.Replace(" ", "");
            //resident_base_infoBean.link_phone = this.textBox20.Text.Replace(" ", "");
            //if (this.radioButton4.Checked == true) { resident_base_infoBean.resident_type = this.radioButton4.Text; };
            //if (this.radioButton5.Checked == true) { resident_base_infoBean.resident_type = this.radioButton5.Text; };

            //if (this.radioButton6.Checked == true) { resident_base_infoBean.nation = this.radioButton6.Text; };
            //if (this.radioButton7.Checked == true) { resident_base_infoBean.nation = this.radioButton7.Text; };

            //if (this.radioButton8.Checked == true) { resident_base_infoBean.blood_group = this.radioButton8.Text; };
            //if (this.radioButton9.Checked == true) { resident_base_infoBean.blood_group = this.radioButton9.Text; };
            //if (this.radioButton10.Checked == true) { resident_base_infoBean.blood_group = this.radioButton10.Text; };
            //if (this.radioButton11.Checked == true) { resident_base_infoBean.blood_group = this.radioButton11.Text; };
            //if (this.radioButton12.Checked == true) { resident_base_infoBean.blood_group = this.radioButton12.Text; };

            //if (this.radioButton13.Checked == true) { resident_base_infoBean.blood_rh = this.radioButton13.Text; };
            //if (this.radioButton14.Checked == true) { resident_base_infoBean.blood_rh = this.radioButton14.Text; };
            //if (this.radioButton15.Checked == true) { resident_base_infoBean.blood_rh = this.radioButton15.Text; };

            //if (this.radioButton22.Checked == true) { resident_base_infoBean.education = this.radioButton22.Text; };
            //if (this.radioButton23.Checked == true) { resident_base_infoBean.education = this.radioButton23.Text; };
            //if (this.radioButton24.Checked == true) { resident_base_infoBean.education = this.radioButton24.Text; };
            //if (this.radioButton26.Checked == true) { resident_base_infoBean.education = this.radioButton26.Text; };
            //if (this.radioButton27.Checked == true) { resident_base_infoBean.education = this.radioButton27.Text; };
            //if (this.radioButton28.Checked == true) { resident_base_infoBean.education = this.radioButton28.Text; };
            //if (this.radioButton29.Checked == true) { resident_base_infoBean.education = this.radioButton29.Text; };
            //if (this.radioButton30.Checked == true) { resident_base_infoBean.education = this.radioButton30.Text; };
            //if (this.radioButton31.Checked == true) { resident_base_infoBean.education = this.radioButton31.Text; };
            //if (this.radioButton32.Checked == true) { resident_base_infoBean.education = this.radioButton32.Text; };

            //if (this.radioButton33.Checked == true) { resident_base_infoBean.profession = this.radioButton33.Text; };
            //if (this.radioButton34.Checked == true) { resident_base_infoBean.profession = this.radioButton34.Text; };
            //if (this.radioButton35.Checked == true) { resident_base_infoBean.profession = this.radioButton35.Text; };
            //if (this.radioButton36.Checked == true) { resident_base_infoBean.profession = this.radioButton36.Text; };
            //if (this.radioButton37.Checked == true) { resident_base_infoBean.profession = this.radioButton37.Text; };
            //if (this.radioButton38.Checked == true) { resident_base_infoBean.profession = this.radioButton38.Text; };
            //if (this.radioButton39.Checked == true) { resident_base_infoBean.profession = this.radioButton39.Text; };
            //if (this.radioButton40.Checked == true) { resident_base_infoBean.profession = this.radioButton40.Text; };
            //if (this.radioButton41.Checked == true) { resident_base_infoBean.profession = this.radioButton41.Text; };

            //if (this.radioButton42.Checked == true) { resident_base_infoBean.marital_status = this.radioButton42.Text; };
            //if (this.radioButton43.Checked == true) { resident_base_infoBean.marital_status = this.radioButton43.Text; };
            //if (this.radioButton44.Checked == true) { resident_base_infoBean.marital_status = this.radioButton44.Text; };
            //if (this.radioButton45.Checked == true) { resident_base_infoBean.marital_status = this.radioButton45.Text; };
            //if (this.radioButton46.Checked == true) { resident_base_infoBean.marital_status = this.radioButton46.Text; };

            //if (this.radioButton47.Checked == true) { resident_base_infoBean.pay_type = this.radioButton47.Text; };
            //if (this.radioButton48.Checked == true) { resident_base_infoBean.pay_type = this.radioButton48.Text; };
            //if (this.radioButton49.Checked == true) { resident_base_infoBean.pay_type = this.radioButton49.Text; };
            //if (this.radioButton50.Checked == true) { resident_base_infoBean.pay_type = this.radioButton50.Text; };
            //if (this.radioButton51.Checked == true) { resident_base_infoBean.pay_type = this.radioButton51.Text; };
            //if (this.radioButton52.Checked == true) { resident_base_infoBean.pay_type = this.radioButton52.Text; };
            //if (this.radioButton53.Checked == true) { resident_base_infoBean.pay_type = this.radioButton53.Text; };
            //if (this.radioButton54.Checked == true) { resident_base_infoBean.pay_type = this.radioButton54.Text; };

            //if (this.radioButton55.Checked == true) { resident_base_infoBean.drug_allergy = this.radioButton55.Text; };
            //if (this.radioButton56.Checked == true) { resident_base_infoBean.drug_allergy = this.radioButton56.Text; };
            //if (this.radioButton57.Checked == true) { resident_base_infoBean.drug_allergy = this.radioButton57.Text; };
            //if (this.radioButton58.Checked == true) { resident_base_infoBean.drug_allergy = this.radioButton58.Text; };
            //if (this.radioButton59.Checked == true) { resident_base_infoBean.drug_allergy = this.radioButton59.Text; };

            //if (this.radioButton60.Checked == true) { resident_base_infoBean.exposure = this.radioButton60.Text; };
            //if (this.radioButton61.Checked == true) { resident_base_infoBean.exposure = this.radioButton61.Text; };
            //if (this.radioButton62.Checked == true) { resident_base_infoBean.exposure = this.radioButton62.Text; };
            //if (this.radioButton63.Checked == true) { resident_base_infoBean.exposure = this.radioButton63.Text; };



            //resident_base_infoBean.heredity_name = this.richTextBox4.Text;


            //foreach (Control ctr in this.panel20.Controls)
            //{
            //    //判断该控件是不是CheckBox
            //    if (ctr is CheckBox)
            //    {
            //        //将ctr转换成CheckBox并赋值给ck
            //        CheckBox ck = ctr as CheckBox;
            //        if (ck.Checked)
            //        {
            //            resident_base_infoBean.deformity_name += "," + ck.Text;
            //        }
            //    }
            //}
            //if (resident_base_infoBean.deformity_name != null && resident_base_infoBean.deformity_name != "")
            //{
            //    resident_base_infoBean.deformity_name = resident_base_infoBean.deformity_name.Substring(1);
            //}

            //if (this.radioButton18.Checked == true) { resident_base_infoBean.kitchen = this.radioButton18.Text; };
            //if (this.radioButton19.Checked == true) { resident_base_infoBean.kitchen = this.radioButton19.Text; };
            //if (this.radioButton20.Checked == true) { resident_base_infoBean.kitchen = this.radioButton20.Text; };
            //if (this.radioButton21.Checked == true) { resident_base_infoBean.kitchen = this.radioButton21.Text; };

            //if (this.radioButton70.Checked == true) { resident_base_infoBean.fuel = this.radioButton70.Text; };
            //if (this.radioButton71.Checked == true) { resident_base_infoBean.fuel = this.radioButton71.Text; };
            //if (this.radioButton72.Checked == true) { resident_base_infoBean.fuel = this.radioButton72.Text; };
            //if (this.radioButton73.Checked == true) { resident_base_infoBean.fuel = this.radioButton73.Text; };
            //if (this.radioButton74.Checked == true) { resident_base_infoBean.fuel = this.radioButton74.Text; };
            //if (this.radioButton75.Checked == true) { resident_base_infoBean.fuel = this.radioButton75.Text; };

            //if (this.radioButton76.Checked == true) { resident_base_infoBean.drink = this.radioButton76.Text; };
            //if (this.radioButton77.Checked == true) { resident_base_infoBean.drink = this.radioButton77.Text; };
            //if (this.radioButton78.Checked == true) { resident_base_infoBean.drink = this.radioButton78.Text; };
            //if (this.radioButton79.Checked == true) { resident_base_infoBean.drink = this.radioButton79.Text; };
            //if (this.radioButton80.Checked == true) { resident_base_infoBean.drink = this.radioButton80.Text; };
            //if (this.radioButton81.Checked == true) { resident_base_infoBean.drink = this.radioButton81.Text; };

            //if (this.radioButton82.Checked == true) { resident_base_infoBean.toilet = this.radioButton82.Text; };
            //if (this.radioButton83.Checked == true) { resident_base_infoBean.toilet = this.radioButton83.Text; };
            //if (this.radioButton84.Checked == true) { resident_base_infoBean.toilet = this.radioButton84.Text; };
            //if (this.radioButton85.Checked == true) { resident_base_infoBean.toilet = this.radioButton85.Text; };
            //if (this.radioButton86.Checked == true) { resident_base_infoBean.toilet = this.radioButton86.Text; };

            //if (this.radioButton87.Checked == true) { resident_base_infoBean.poultry = this.radioButton87.Text; };
            //if (this.radioButton88.Checked == true) { resident_base_infoBean.poultry = this.radioButton88.Text; };
            //if (this.radioButton89.Checked == true) { resident_base_infoBean.poultry = this.radioButton89.Text; };
            //if (this.radioButton90.Checked == true) { resident_base_infoBean.poultry = this.radioButton90.Text; };





            //////以下页面未用 数据库字段格式要求
            //resident_base_infoBean.synchro_time = DateTime.Now.ToString("yyyy-MM-dd");
            //resident_base_infoBean.create_time = DateTime.Now.ToString("yyyy-MM-dd");
            //resident_base_infoBean.update_time = DateTime.Now.ToString("yyyy-MM-dd");

            //resident_base_infoBean.is_hypertension = "0";
            //resident_base_infoBean.is_diabetes = "0";
            //resident_base_infoBean.is_psychosis = "0";
            //resident_base_infoBean.is_tuberculosis = "0";
            //resident_base_infoBean.is_heredity = "0";
            //resident_base_infoBean.is_deformity = "0";
            //resident_base_infoBean.is_poor = "0";
            //resident_base_infoBean.is_signing = "0";
            //resident_base_infoBean.is_synchro = "0";


            //bool isfalse = personalBasicInfoService.aUpersonalBasicInfo(resident_base_infoBean, id, goodsList, goodsList0, goodsList1, goodsList2, goodsList3);
            //if (isfalse)
            //{
            //    this.DialogResult = DialogResult.OK;
            //}
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox12.Text.Replace(" ", "").Length == 18 && id == "")
            {
                DataTable dt = personalBasicInfoService.query(this.textBox12.Text.Replace(" ", ""));
                if (dt.Rows.Count > 0)
                {
                    this.textBox12.Text = "";
                    MessageBox.Show("此身份证号已注册");
                }
            }
        }
    }
}
