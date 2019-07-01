using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class aUolderHelthService : Form
    {

        service.olderHelthServices olderHelthServices = new service.olderHelthServices();
        public string archiveno = "";
        public string sex = "";
        public int flag = 0;
        public string _examid = "";
        public string _barCode = ""; 
        public aUolderHelthService()
        {
            InitializeComponent();
        }
        private void aUolderHelthService_Load(object sender, EventArgs e)
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
            this.textBox19.Text += "5";
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

            this.textBox29.Text += System.Environment.NewLine;
            this.textBox29.Text += "-";
            this.textBox29.Text += System.Environment.NewLine;
            this.textBox29.Text += System.Environment.NewLine;
            this.textBox29.Text += System.Environment.NewLine;
            this.textBox29.Text += System.Environment.NewLine;
            this.textBox29.Text += "0";

            this.textBox30.Text += System.Environment.NewLine;
            this.textBox30.Text += "需要协助，在";
            this.textBox30.Text += System.Environment.NewLine;
            this.textBox30.Text += "适当的时间内";
            this.textBox30.Text += System.Environment.NewLine;
            this.textBox30.Text += "完成部分穿衣";
            this.textBox30.Text += System.Environment.NewLine;
            this.textBox30.Text += System.Environment.NewLine;
            this.textBox30.Text += "3";

            this.textBox31.Text += System.Environment.NewLine;
            this.textBox31.Text += "完全需要帮";
            this.textBox31.Text += System.Environment.NewLine;
            this.textBox31.Text += "助";
            this.textBox31.Text += System.Environment.NewLine;
            this.textBox31.Text += System.Environment.NewLine;
            this.textBox31.Text += System.Environment.NewLine;
            this.textBox31.Text += "5";
            ////////////////////////////////////////////////////////////////

            this.textBox34.Text += System.Environment.NewLine;
            this.textBox34.Text += "不需协助，";
            this.textBox34.Text += System.Environment.NewLine;
            this.textBox34.Text += "可自控";
            this.textBox34.Text += System.Environment.NewLine;
            this.textBox34.Text += System.Environment.NewLine;
            this.textBox34.Text += System.Environment.NewLine;
            this.textBox34.Text += "0";

            this.textBox35.Text += System.Environment.NewLine;
            this.textBox35.Text += "偶尔失禁，但";
            this.textBox35.Text += System.Environment.NewLine;
            this.textBox35.Text += "基本上能如";
            this.textBox35.Text += System.Environment.NewLine;
            this.textBox35.Text += "厕或使用便";
            this.textBox35.Text += System.Environment.NewLine;
            this.textBox35.Text += "具";
            this.textBox35.Text += System.Environment.NewLine;
            this.textBox35.Text += "1";

            this.textBox36.Text += System.Environment.NewLine;
            this.textBox36.Text += "经常失禁，在";
            this.textBox36.Text += System.Environment.NewLine;
            this.textBox36.Text += "很多提示和协";
            this.textBox36.Text += System.Environment.NewLine;
            this.textBox36.Text += "助下尚能如厕";
            this.textBox36.Text += System.Environment.NewLine;
            this.textBox36.Text += "或使用便具";
            this.textBox36.Text += System.Environment.NewLine;
            this.textBox36.Text += "5";

            this.textBox37.Text += System.Environment.NewLine;
            this.textBox37.Text += "完全失禁，";
            this.textBox37.Text += System.Environment.NewLine;
            this.textBox37.Text += "完全需要帮";
            this.textBox37.Text += System.Environment.NewLine;
            this.textBox37.Text += "助";
            this.textBox37.Text += System.Environment.NewLine;
            this.textBox37.Text += System.Environment.NewLine;
            this.textBox37.Text += "10";
            ////////////////////////////////////////////////////////////////
            this.textBox40.Text += System.Environment.NewLine;
            this.textBox40.Text += "独立完成所，";
            this.textBox40.Text += System.Environment.NewLine;
            this.textBox40.Text += "有活动";
            this.textBox40.Text += System.Environment.NewLine;
            this.textBox40.Text += System.Environment.NewLine;
            this.textBox40.Text += System.Environment.NewLine;
            this.textBox40.Text += "0";

            this.textBox41.Text += System.Environment.NewLine;
            this.textBox41.Text += "借助较小的外力";
            this.textBox41.Text += System.Environment.NewLine;
            this.textBox41.Text += "或辅助装置能完";
            this.textBox41.Text += System.Environment.NewLine;
            this.textBox41.Text += "成站立、行走、";
            this.textBox41.Text += System.Environment.NewLine;
            this.textBox41.Text += "上下楼梯等";
            this.textBox41.Text += System.Environment.NewLine;
            this.textBox41.Text += "1";

            this.textBox42.Text += System.Environment.NewLine;
            this.textBox42.Text += "借助较大的外";
            this.textBox42.Text += System.Environment.NewLine;
            this.textBox42.Text += "力才能完成站";
            this.textBox42.Text += System.Environment.NewLine;
            this.textBox42.Text += "立、行走，不";
            this.textBox42.Text += System.Environment.NewLine;
            this.textBox42.Text += "能上下楼梯";
            this.textBox42.Text += System.Environment.NewLine;
            this.textBox42.Text += "5";

            this.textBox43.Text += System.Environment.NewLine;
            this.textBox43.Text += "卧床不起，";
            this.textBox43.Text += System.Environment.NewLine;
            this.textBox43.Text += "活动完全需";
            this.textBox43.Text += System.Environment.NewLine;
            this.textBox43.Text += "要帮助";
            this.textBox43.Text += System.Environment.NewLine;
            this.textBox43.Text += System.Environment.NewLine;
            this.textBox43.Text += "10";
            ////////////////////////////////////////////////////////////////


        }




        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string _stag = "1";
            bean.elderly_selfcare_estimateBean elderly_selfcare_estimateBean = new bean.elderly_selfcare_estimateBean();
            elderly_selfcare_estimateBean.sex = sex;
            elderly_selfcare_estimateBean.name = this.textBox1.Text.Replace(" ", "");
            elderly_selfcare_estimateBean.aichive_no = this.textBox2.Text.Replace(" ", "");
            elderly_selfcare_estimateBean.id_number = this.textBox12.Text.Replace(" ", "");

            elderly_selfcare_estimateBean.answer_result += "," + this.numericUpDown1.Value.ToString();
            elderly_selfcare_estimateBean.answer_result += "," + this.numericUpDown2.Value.ToString();
            elderly_selfcare_estimateBean.answer_result += "," + this.numericUpDown3.Value.ToString();
            elderly_selfcare_estimateBean.answer_result += "," + this.numericUpDown4.Value.ToString();
            elderly_selfcare_estimateBean.answer_result += "," + this.numericUpDown5.Value.ToString();
            elderly_selfcare_estimateBean.answer_result = elderly_selfcare_estimateBean.answer_result.Substring(1);
            elderly_selfcare_estimateBean.total_score = this.numericUpDown6.Value.ToString();

            if (this.numericUpDown6.Value >=0 && this.numericUpDown6.Value <= 3) {
                elderly_selfcare_estimateBean.judgement_result = "可自理";
                _stag = "1";
            }
            else if (this.numericUpDown6.Value >= 4 && this.numericUpDown6.Value <= 8)
            {
                elderly_selfcare_estimateBean.judgement_result = "轻度依赖";
                _stag = "2";
            }
            else if (this.numericUpDown6.Value >= 9 && this.numericUpDown6.Value <= 18) {
                elderly_selfcare_estimateBean.judgement_result = "中度依赖";
                _stag = "3";
            } else if (this.numericUpDown6.Value >= 19) {
                elderly_selfcare_estimateBean.judgement_result = "不能自理";
                _stag = "4";
            }
            else { } 
            //////以下页面未用 数据库字段格式要求
            elderly_selfcare_estimateBean.upload_time = DateTime.Now.ToString("yyyy-MM-dd");
            elderly_selfcare_estimateBean.create_time = DateTime.Now.ToString("yyyy-MM-dd");
            elderly_selfcare_estimateBean.update_time = DateTime.Now.ToString("yyyy-MM-dd"); 
            elderly_selfcare_estimateBean.upload_status = "0"; 
            elderly_selfcare_estimateBean.test_date = DateTime.Now.ToString("yyyy-MM-dd");
            elderly_selfcare_estimateBean.create_name = frmLogin.name;
            elderly_selfcare_estimateBean.test_doctor= basicInfoSettings.zeren_doctor;
            if(_examid=="")
            {
                healthCheckupDao hcd = new healthCheckupDao();
                _examid = hcd.GetExaminationRecord(elderly_selfcare_estimateBean.aichive_no, elderly_selfcare_estimateBean.id_number, _barCode);
            }
            elderly_selfcare_estimateBean.exam_id = _examid;
            string _id = "";
            if(flag==1)
            {
                _id = _examid;
            }
            bool isfalse = olderHelthServices.aUelderly_selfcare_estimateForExamID(elderly_selfcare_estimateBean, _id);
            //bool isfalse = olderHelthServices.aUelderly_selfcare_estimate(elderly_selfcare_estimateBean, archiveno);
            if (isfalse)
            {
                //这里就要更新对应的 zkhw_tj_bgdc-->lnrzlnlpg、physical_examination_record-->base_selfcare_estimate
                string id_number = textBox12.Text;
                string aichive_no = textBox2.Text;
                tjcheckDao tjdao = new tjcheckDao(); 
                //用事务更新
                tjdao.UpdateOldestimateTran("1", _barCode, id_number, _examid, _stag); 
                this.DialogResult = DialogResult.OK;
            }
        }
        private void countTotal()
        {
            this.numericUpDown6.Value = 0;
            this.numericUpDown6.Value += this.numericUpDown1.Value;
            this.numericUpDown6.Value += this.numericUpDown2.Value;
            this.numericUpDown6.Value += this.numericUpDown3.Value;
            this.numericUpDown6.Value += this.numericUpDown4.Value;
            this.numericUpDown6.Value += this.numericUpDown5.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            countTotal();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            countTotal();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            countTotal();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            countTotal();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            countTotal();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked) {
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
                this.numericUpDown1.Value = 0;
            }
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
            {
                this.checkBox1.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
                this.numericUpDown1.Value = 0;
            }
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked)
            {
                this.checkBox1.Checked = false;
                this.checkBox2.Checked = false;
                this.checkBox4.Checked = false;
                this.numericUpDown1.Value = 3;
            }
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {
                this.checkBox1.Checked = false;
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.numericUpDown1.Value = 5;
            }
        }

        private void checkBox5_Click(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked)
            {
                this.checkBox6.Checked = false;
                this.checkBox7.Checked = false;
                this.checkBox8.Checked = false;
                this.numericUpDown2.Value = 0;
            }
        }

        private void checkBox6_Click(object sender, EventArgs e)
        {
            if (this.checkBox6.Checked)
            {
                this.checkBox5.Checked = false;
                this.checkBox7.Checked = false;
                this.checkBox8.Checked = false;
                this.numericUpDown2.Value = 1;
            }
        }

        private void checkBox7_Click(object sender, EventArgs e)
        {
            if (this.checkBox7.Checked)
            {
                this.checkBox5.Checked = false;
                this.checkBox6.Checked = false;
                this.checkBox8.Checked = false;
                this.numericUpDown2.Value = 3;
            }
        }

        private void checkBox8_Click(object sender, EventArgs e)
        {
            if (this.checkBox8.Checked)
            {
                this.checkBox5.Checked = false;
                this.checkBox6.Checked = false;
                this.checkBox7.Checked = false;
                this.numericUpDown2.Value = 7;
            }
        }

        private void checkBox9_Click(object sender, EventArgs e)
        {
            if (this.checkBox9.Checked)
            {
                this.checkBox10.Checked = false;
                this.checkBox11.Checked = false;
                this.checkBox12.Checked = false;
                this.numericUpDown3.Value = 0;
            }
        }

        private void checkBox10_Click(object sender, EventArgs e)
        {
            if (this.checkBox10.Checked)
            {
                this.checkBox9.Checked = false;
                this.checkBox11.Checked = false;
                this.checkBox12.Checked = false;
                this.numericUpDown3.Value = 0;
            }
        }

        private void checkBox11_Click(object sender, EventArgs e)
        {
            if (this.checkBox11.Checked)
            {
                this.checkBox9.Checked = false;
                this.checkBox10.Checked = false;
                this.checkBox12.Checked = false;
                this.numericUpDown3.Value = 3;
            }
        }

        private void checkBox12_Click(object sender, EventArgs e)
        {
            if (this.checkBox12.Checked)
            {
                this.checkBox9.Checked = false;
                this.checkBox10.Checked = false;
                this.checkBox11.Checked = false;
                this.numericUpDown3.Value = 5;
            }
        }

        private void checkBox13_Click(object sender, EventArgs e)
        {
            if (this.checkBox13.Checked)
            {
                this.checkBox14.Checked = false;
                this.checkBox15.Checked = false;
                this.checkBox16.Checked = false;
                this.numericUpDown4.Value = 0;
            }
        }

        private void checkBox14_Click(object sender, EventArgs e)
        {
            if (this.checkBox14.Checked)
            {
                this.checkBox13.Checked = false;
                this.checkBox15.Checked = false;
                this.checkBox16.Checked = false;
                this.numericUpDown4.Value = 1;
            }
        }

        private void checkBox15_Click(object sender, EventArgs e)
        {
            if (this.checkBox15.Checked)
            {
                this.checkBox13.Checked = false;
                this.checkBox14.Checked = false;
                this.checkBox16.Checked = false;
                this.numericUpDown4.Value = 5;
            }
        }

        private void checkBox16_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.checkBox13.Checked = false;
                this.checkBox14.Checked = false;
                this.checkBox15.Checked = false;
                this.numericUpDown4.Value = 10;
            }
        }

        private void checkBox17_Click(object sender, EventArgs e)
        {
            if (this.checkBox17.Checked)
            {
                this.checkBox18.Checked = false;
                this.checkBox19.Checked = false;
                this.checkBox20.Checked = false;
                this.numericUpDown5.Value = 0;
            }
        }

        private void checkBox18_Click(object sender, EventArgs e)
        {
            if (this.checkBox18.Checked)
            {
                this.checkBox17.Checked = false;
                this.checkBox19.Checked = false;
                this.checkBox20.Checked = false;
                this.numericUpDown5.Value = 1;
            }
        }

        private void checkBox19_Click(object sender, EventArgs e)
        {
            if (this.checkBox19.Checked)
            {
                this.checkBox17.Checked = false;
                this.checkBox18.Checked = false;
                this.checkBox20.Checked = false;
                this.numericUpDown5.Value = 5;
            }
        }

        private void checkBox20_Click(object sender, EventArgs e)
        {
            if (this.checkBox20.Checked)
            {
                this.checkBox17.Checked = false;
                this.checkBox18.Checked = false;
                this.checkBox19.Checked = false;
                this.numericUpDown5.Value = 10;
            }
        }
    }
}
