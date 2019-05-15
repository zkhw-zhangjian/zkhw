using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class aUpsychiatricPatientServicesS : Form
    {

        service.psychiatricPatientServiceS psychiatricPatientS = new service.psychiatricPatientServiceS();
        public string id = "";
        public aUpsychiatricPatientServicesS()
        {
            InitializeComponent();
        }
        private void aUpsychiatricPatientServicesS_Load(object sender, EventArgs e)
        {
            //查询赋值
            if (id != "")
            {
                DataTable dt = psychiatricPatientS.queryPsychosis_follow_record(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.dateTimePicker5.Value = DateTime.Parse(dt.Rows[0]["visit_date"].ToString());

                    if (this.radioButton1.Text == dt.Rows[0]["visit_type"].ToString()) { this.radioButton1.Checked = true; };
                    if (this.radioButton2.Text == dt.Rows[0]["visit_type"].ToString()) { this.radioButton2.Checked = true; };
                    if (this.radioButton11.Text == dt.Rows[0]["visit_type"].ToString()) { this.radioButton11.Checked = true; };

                    if (this.radioButton3.Text == dt.Rows[0]["miss_reason"].ToString()) { this.radioButton3.Checked = true; };
                    if (this.radioButton4.Text == dt.Rows[0]["miss_reason"].ToString()) { this.radioButton4.Checked = true; };
                    if (this.radioButton5.Text == dt.Rows[0]["miss_reason"].ToString()) { this.radioButton5.Checked = true; };
                    if (this.radioButton6.Text == dt.Rows[0]["miss_reason"].ToString()) { this.radioButton6.Checked = true; };

                    this.dateTimePicker3.Value = DateTime.Parse(dt.Rows[0]["die_date"].ToString());

                    if (this.radioButton14.Text == dt.Rows[0]["die_reason"].ToString()) { this.radioButton14.Checked = true; };
                    if (this.radioButton15.Text == dt.Rows[0]["die_reason"].ToString()) { this.radioButton15.Checked = true; };
                    if (this.radioButton16.Text == dt.Rows[0]["die_reason"].ToString()) { this.radioButton16.Checked = true; };
                    if (this.radioButton17.Text == dt.Rows[0]["die_reason"].ToString()) { this.radioButton17.Checked = true; };
                    if (this.radioButton18.Text == dt.Rows[0]["die_reason"].ToString()) { this.radioButton18.Checked = true; };
                    if (this.radioButton19.Text == dt.Rows[0]["die_reason"].ToString()) { this.radioButton19.Checked = true; };

                    this.textBox26.Text = dt.Rows[0]["physical_disease"].ToString();

                    if (this.radioButton20.Text == dt.Rows[0]["fatalness"].ToString()) { this.radioButton20.Checked = true; };
                    if (this.radioButton21.Text == dt.Rows[0]["fatalness"].ToString()) { this.radioButton21.Checked = true; };
                    if (this.radioButton22.Text == dt.Rows[0]["fatalness"].ToString()) { this.radioButton22.Checked = true; };
                    if (this.radioButton23.Text == dt.Rows[0]["fatalness"].ToString()) { this.radioButton23.Checked = true; };
                    if (this.radioButton24.Text == dt.Rows[0]["fatalness"].ToString()) { this.radioButton24.Checked = true; };
                    if (this.radioButton26.Text == dt.Rows[0]["fatalness"].ToString()) { this.radioButton26.Checked = true; };

                    foreach (Control ctr in this.panel5.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            if (dt.Rows[0]["symptom"].ToString().IndexOf(ck.Text.ToString()) > -1)
                            {
                                ck.Checked = true;
                            }
                        }
                    }


                    if (this.radioButton25.Text == dt.Rows[0]["insight"].ToString()) { this.radioButton25.Checked = true; };
                    if (this.radioButton8.Text == dt.Rows[0]["insight"].ToString()) { this.radioButton8.Checked = true; };
                    if (this.radioButton9.Text == dt.Rows[0]["insight"].ToString()) { this.radioButton9.Checked = true; };

                    if (this.radioButton10.Text == dt.Rows[0]["sleep_status"].ToString()) { this.radioButton10.Checked = true; };
                    if (this.radioButton12.Text == dt.Rows[0]["sleep_status"].ToString()) {  this.radioButton12.Checked = true; };

                    if (this.radioButton13.Text == dt.Rows[0]["dietary_status"].ToString()) { this.radioButton13.Checked = true; };
                    if (this.radioButton27.Text == dt.Rows[0]["dietary_status"].ToString()) {  this.radioButton27.Checked = true; };

                    if (this.radioButton28.Text == dt.Rows[0]["self_help"].ToString()) { this.radioButton28.Checked = true; };
                    if (this.radioButton29.Text == dt.Rows[0]["self_help"].ToString()) { this.radioButton29.Checked = true; };

                    if (this.radioButton30.Text == dt.Rows[0]["housework"].ToString()) { this.radioButton30.Checked = true; };
                    if (this.radioButton31.Text == dt.Rows[0]["housework"].ToString()) { this.radioButton31.Checked = true; };

                    if (this.radioButton32.Text == dt.Rows[0]["work"].ToString()) { this.radioButton32.Checked = true; };
                    if (this.radioButton33.Text == dt.Rows[0]["work"].ToString()) { this.radioButton33.Checked = true; };

                    if (this.radioButton34.Text == dt.Rows[0]["learning_ability"].ToString()) {  this.radioButton34.Checked = true; };
                    if (this.radioButton35.Text == dt.Rows[0]["learning_ability"].ToString()) {this.radioButton35.Checked = true; };

                    if (this.radioButton36.Text == dt.Rows[0]["interpersonal"].ToString()) { this.radioButton36.Checked = true; };
                    if (this.radioButton37.Text == dt.Rows[0]["interpersonal"].ToString()) { this.radioButton37.Checked = true; };

                    if (this.radioButton38.Text == dt.Rows[0]["dangerous_act"].ToString()) { this.radioButton38.Checked = true; };
                    if (this.radioButton39.Text == dt.Rows[0]["dangerous_act"].ToString()) { this.radioButton39.Checked = true; };

                    this.numericUpDown1.Value = Int32.Parse(dt.Rows[0]["slight_trouble_num"].ToString());
                    this.numericUpDown2.Value = Int32.Parse(dt.Rows[0]["cause_trouble_num"].ToString());
                    this.numericUpDown3.Value = Int32.Parse(dt.Rows[0]["cause_accident_num"].ToString());
                    this.numericUpDown4.Value = Int32.Parse(dt.Rows[0]["autolesion_num"].ToString());
                    this.numericUpDown5.Value = Int32.Parse(dt.Rows[0]["attempted_suicide_num"].ToString());
                    this.numericUpDown6.Value = Int32.Parse(dt.Rows[0]["harm_other_num"].ToString());

                    if (this.radioButton42.Text == dt.Rows[0]["isolation"].ToString()) { this.radioButton42.Checked = true; };
                    if (this.radioButton43.Text == dt.Rows[0]["isolation"].ToString()) { this.radioButton43.Checked = true; };
                    if (this.radioButton44.Text == dt.Rows[0]["isolation"].ToString()) { this.radioButton44.Checked = true; };

                    if (this.radioButton45.Text == dt.Rows[0]["hospitalized_status"].ToString()) { this.radioButton45.Checked = true; };
                    if (this.radioButton46.Text == dt.Rows[0]["hospitalized_status"].ToString()) { this.radioButton46.Checked = true; };
                    if (this.radioButton47.Text == dt.Rows[0]["hospitalized_status"].ToString()) { this.radioButton47.Checked = true; };

                    this.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["out_hospital_date"].ToString());

                    if (this.radioButton48.Text == dt.Rows[0]["laboratory_examination"].ToString()) { this.radioButton48.Checked = true; };
                    if (this.radioButton49.Text == dt.Rows[0]["laboratory_examination"].ToString()) { this.radioButton49.Checked = true; };

                    if (this.radioButton50.Text == dt.Rows[0]["compliance"].ToString()) { this.radioButton50.Checked = true; };
                    if (this.radioButton51.Text == dt.Rows[0]["compliance"].ToString()) { this.radioButton51.Checked = true; };
                    if (this.radioButton52.Text == dt.Rows[0]["compliance"].ToString()) { this.radioButton52.Checked = true; };
                    if (this.radioButton53.Text == dt.Rows[0]["compliance"].ToString()) { this.radioButton53.Checked = true; };

                    if (this.radioButton54.Text == dt.Rows[0]["untoward_effect"].ToString()) { this.radioButton54.Checked = true; };
                    if (this.radioButton55.Text == dt.Rows[0]["untoward_effect"].ToString()) { this.radioButton55.Checked = true; };

                    this.textBox7.Text = dt.Rows[0]["untoward_effect_info"].ToString();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bean.psychosis_follow_recordBean psychosis_follow_recordBean = new bean.psychosis_follow_recordBean();

            psychosis_follow_recordBean.name = this.textBox37.Text.Replace(" ", "");
            psychosis_follow_recordBean.archive_no = this.textBox39.Text.Replace(" ", "");
            psychosis_follow_recordBean.Cardcode = this.textBox41.Text.Replace(" ", "");
            psychosis_follow_recordBean.visit_date = this.dateTimePicker5.Text.ToString();

            if (this.radioButton1.Checked == true) { psychosis_follow_recordBean.visit_type = this.radioButton1.Text; };
            if (this.radioButton2.Checked == true) { psychosis_follow_recordBean.visit_type = this.radioButton2.Text; };
            if (this.radioButton11.Checked == true) { psychosis_follow_recordBean.visit_type = this.radioButton11.Text; };

            if (this.radioButton3.Checked == true) { psychosis_follow_recordBean.miss_reason = this.radioButton3.Text; };
            if (this.radioButton4.Checked == true) { psychosis_follow_recordBean.miss_reason = this.radioButton4.Text; };
            if (this.radioButton5.Checked == true) { psychosis_follow_recordBean.miss_reason = this.radioButton5.Text; };
            if (this.radioButton6.Checked == true) { psychosis_follow_recordBean.miss_reason = this.radioButton6.Text; };

            psychosis_follow_recordBean.die_date = this.dateTimePicker3.Text.ToString();

            if (this.radioButton14.Checked == true) { psychosis_follow_recordBean.die_reason = this.radioButton14.Text; };
            if (this.radioButton15.Checked == true) { psychosis_follow_recordBean.die_reason = this.radioButton15.Text; };
            if (this.radioButton16.Checked == true) { psychosis_follow_recordBean.die_reason = this.radioButton16.Text; };
            if (this.radioButton17.Checked == true) { psychosis_follow_recordBean.die_reason = this.radioButton17.Text; };
            if (this.radioButton18.Checked == true) { psychosis_follow_recordBean.die_reason = this.radioButton18.Text; };
            if (this.radioButton19.Checked == true) { psychosis_follow_recordBean.die_reason = this.radioButton19.Text; };

            psychosis_follow_recordBean.physical_disease = this.textBox26.Text.Replace(" ", "");

            if (this.radioButton20.Checked == true) { psychosis_follow_recordBean.fatalness = this.radioButton20.Text; };
            if (this.radioButton21.Checked == true) { psychosis_follow_recordBean.fatalness = this.radioButton21.Text; };
            if (this.radioButton22.Checked == true) { psychosis_follow_recordBean.fatalness = this.radioButton22.Text; };
            if (this.radioButton23.Checked == true) { psychosis_follow_recordBean.fatalness = this.radioButton23.Text; };
            if (this.radioButton24.Checked == true) { psychosis_follow_recordBean.fatalness = this.radioButton24.Text; };
            if (this.radioButton26.Checked == true) { psychosis_follow_recordBean.fatalness = this.radioButton26.Text; };

            foreach (Control ctr in this.panel5.Controls)
            {
                //判断该控件是不是CheckBox
                if (ctr is CheckBox)
                {
                    //将ctr转换成CheckBox并赋值给ck
                    CheckBox ck = ctr as CheckBox;
                    if (ck.Checked)
                    {
                        psychosis_follow_recordBean.symptom += "," + ck.Text;
                    }
                }
            }
            if (psychosis_follow_recordBean.symptom != null && psychosis_follow_recordBean.symptom != "")
            {
                psychosis_follow_recordBean.symptom = psychosis_follow_recordBean.symptom.Substring(1);
            }

            if (this.radioButton25.Checked == true) { psychosis_follow_recordBean.insight = this.radioButton25.Text; };
            if (this.radioButton8.Checked == true) { psychosis_follow_recordBean.insight = this.radioButton8.Text; };
            if (this.radioButton9.Checked == true) { psychosis_follow_recordBean.insight = this.radioButton9.Text; };

            if (this.radioButton10.Checked == true) { psychosis_follow_recordBean.sleep_status = this.radioButton10.Text; };
            if (this.radioButton12.Checked == true) { psychosis_follow_recordBean.sleep_status = this.radioButton12.Text; };

            if (this.radioButton13.Checked == true) { psychosis_follow_recordBean.dietary_status = this.radioButton13.Text; };
            if (this.radioButton27.Checked == true) { psychosis_follow_recordBean.dietary_status = this.radioButton27.Text; };

            if (this.radioButton28.Checked == true) { psychosis_follow_recordBean.self_help = this.radioButton28.Text; };
            if (this.radioButton29.Checked == true) { psychosis_follow_recordBean.self_help = this.radioButton29.Text; };

            if (this.radioButton30.Checked == true) { psychosis_follow_recordBean.housework = this.radioButton30.Text; };
            if (this.radioButton31.Checked == true) { psychosis_follow_recordBean.housework = this.radioButton31.Text; };

            if (this.radioButton32.Checked == true) { psychosis_follow_recordBean.work = this.radioButton32.Text; };
            if (this.radioButton33.Checked == true) { psychosis_follow_recordBean.work = this.radioButton33.Text; };

            if (this.radioButton34.Checked == true) { psychosis_follow_recordBean.learning_ability = this.radioButton34.Text; };
            if (this.radioButton35.Checked == true) { psychosis_follow_recordBean.learning_ability = this.radioButton35.Text; };

            if (this.radioButton36.Checked == true) { psychosis_follow_recordBean.interpersonal = this.radioButton36.Text; };
            if (this.radioButton37.Checked == true) { psychosis_follow_recordBean.interpersonal = this.radioButton37.Text; };

            if (this.radioButton38.Checked == true) { psychosis_follow_recordBean.dangerous_act = this.radioButton38.Text; };
            if (this.radioButton39.Checked == true) { psychosis_follow_recordBean.dangerous_act = this.radioButton39.Text; };

            psychosis_follow_recordBean.slight_trouble_num = this.numericUpDown1.Value.ToString();
            psychosis_follow_recordBean.cause_trouble_num = this.numericUpDown2.Value.ToString();
            psychosis_follow_recordBean.cause_accident_num = this.numericUpDown3.Value.ToString();
            psychosis_follow_recordBean.autolesion_num = this.numericUpDown4.Value.ToString();
            psychosis_follow_recordBean.attempted_suicide_num = this.numericUpDown5.Value.ToString();
            psychosis_follow_recordBean.harm_other_num = this.numericUpDown6.Value.ToString();

            if (this.radioButton42.Checked == true) { psychosis_follow_recordBean.isolation = this.radioButton42.Text; };
            if (this.radioButton43.Checked == true) { psychosis_follow_recordBean.isolation = this.radioButton43.Text; };
            if (this.radioButton44.Checked == true) { psychosis_follow_recordBean.isolation = this.radioButton44.Text; };

            if (this.radioButton45.Checked == true) { psychosis_follow_recordBean.hospitalized_status = this.radioButton45.Text; };
            if (this.radioButton46.Checked == true) { psychosis_follow_recordBean.hospitalized_status = this.radioButton46.Text; };
            if (this.radioButton47.Checked == true) { psychosis_follow_recordBean.hospitalized_status = this.radioButton47.Text; };

            psychosis_follow_recordBean.out_hospital_date = this.dateTimePicker1.Text.ToString();

            if (this.radioButton48.Checked == true) { psychosis_follow_recordBean.laboratory_examination = this.radioButton48.Text; };
            if (this.radioButton49.Checked == true) { psychosis_follow_recordBean.laboratory_examination = this.radioButton49.Text; };

            if (this.radioButton50.Checked == true) { psychosis_follow_recordBean.compliance = this.radioButton50.Text; };
            if (this.radioButton51.Checked == true) { psychosis_follow_recordBean.compliance = this.radioButton51.Text; };
            if (this.radioButton52.Checked == true) { psychosis_follow_recordBean.compliance = this.radioButton52.Text; };
            if (this.radioButton53.Checked == true) { psychosis_follow_recordBean.compliance = this.radioButton53.Text; };

            if (this.radioButton54.Checked == true) { psychosis_follow_recordBean.untoward_effect = this.radioButton54.Text; };
            if (this.radioButton55.Checked == true) { psychosis_follow_recordBean.untoward_effect = this.radioButton55.Text; };
            
            psychosis_follow_recordBean.untoward_effect_info = this.textBox7.Text.Replace(" ", "");


            ////以下页面未用 数据库字段格式要求

            psychosis_follow_recordBean.upload_status = "0";
            psychosis_follow_recordBean.create_time = DateTime.Now.ToString("yyyy-MM-dd");
            psychosis_follow_recordBean.update_time = DateTime.Now.ToString("yyyy-MM-dd");
            psychosis_follow_recordBean.upload_time = DateTime.Now.ToString("yyyy-MM-dd");


            bool isfalse = psychiatricPatientS.aUPsychosis_follow_record(psychosis_follow_recordBean, id);
            if (isfalse)
            {
                this.Close();
                aUpsychiatricPatientServicesS1 auhc1 = new aUpsychiatricPatientServicesS1();
                auhc1.id = id;//祖
                auhc1.archive_no = this.textBox39.Text.Replace(" ", "");
                if (auhc1.ShowDialog()==DialogResult.OK) {
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("数据保存成功!");
                }
            }
            else
            {
                MessageBox.Show("保存不成功!");
            }
        }
    }
}
