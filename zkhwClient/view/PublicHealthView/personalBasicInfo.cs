﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class personalBasicInfo : Form
    {
        service.personalBasicInfoService pBasicInfo = new service.personalBasicInfoService();
        areaConfigDao areadao = new areaConfigDao();
        public string pCa = "";
        public string code = "";
        public string time1 = null;
        public string time2 = null;
        public string shengcode = null;
        public string shicode = null;
        public string qxcode = null;
        public string xzcode = null;
        public string xcuncode = null;
        private float xMy;//定义当前窗体的宽度
        private float yMy;//定义当前窗体的高度


        public personalBasicInfo()
        {
            InitializeComponent();

            //xMy = this.Width;
            //yMy = this.Height;
            //Common.setTag(this);
        }

        private void SetControlWodth()
        {
            if(Common.m_nWindwMetricsY<=900)
            {
                comboBox4.Width = 120;
                comboBox5.Width = 120;
                comboBox5.Left = comboBox4.Left + comboBox4.Width + 5;
            } 
        }
        private void personalBasicInfo_Load(object sender, EventArgs e)
        {
            SetControlWodth();
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1); 
            //区域 
            Common.SetComboBoxInfo(comboBox1, areadao.shengInfo());

            xcuncode = basicInfoSettings.xcuncode;
            button5_Click(null,null);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;
            if (pCa != "")
            {
                this.label5.Text = "";
            }
            else { this.label5.Text = "---姓名/身份证号/档案号---"; }
            
            if (this.comboBox5.Text == "" || this.comboBox5.Text == "--请选择--" || comboBox5.SelectedValue == null)
            {
                xcuncode = "";
            }
            else
            {
                xcuncode = this.comboBox5.SelectedValue.ToString();
            }
            querypBasicInfo();
        }
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(String));
            dt.Columns.Add("name", typeof(String));
            dt.Columns.Add("archive_no", typeof(String));
            dt.Columns.Add("id_number", typeof(String));
            dt.Columns.Add("BiaoQian", typeof(String));
            dt.Columns.Add("create_name", typeof(String));
            dt.Columns.Add("create_time", typeof(String));
            dt.Columns.Add("doctor_name", typeof(String));
            return dt;
        }
        //个人基本建档记录历史表  关联传参调查询的方法
        private void querypBasicInfo()
        {
            time1 = this.dateTimePicker1.Text.ToString() +" 00:00:00";//开始时间
            time2 = this.dateTimePicker2.Text.ToString() + " 23:59:59";//结束时间
            this.dataGridView1.DataSource = null;
            LoadingHelper.myCaption = "正在查询...";
            LoadingHelper.myLabel = "正在查询数据...";
            LoadingHelper.ShowLoadingScreen();
            Thread.Sleep(50);
            try
            {
                DataTable dt = pBasicInfo.queryPersonalBasicInfo(pCa, time1, time2, xcuncode);
                //这里处理下对应的记录
                DataTable dtfinished = CreateDataTable();
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    DataRow dr = dtfinished.NewRow();
                    dr[0] = dt.Rows[i][0].ToString();
                    dr[1] = dt.Rows[i][1].ToString();
                    dr[2] = dt.Rows[i][2].ToString();
                    dr[3] = dt.Rows[i][3].ToString();
                     
                    #region 处理特殊标签
                    String _teshubiaoqian = "";
                    String _tmp= dt.Rows[i]["age"].ToString();
                    if (_tmp == "") _tmp = "0";
                    if(int.Parse(_tmp)>=65)
                    {
                        _teshubiaoqian = "老";
                    }
                    else
                    {
                        if (int.Parse(_tmp) <= 6)
                        {
                            _teshubiaoqian = "儿童";
                        }
                    }
                   

                    _tmp = dt.Rows[i]["is_gravida"].ToString();
                    if (_tmp == "") _tmp = "0";
                    if (int.Parse(_tmp) != 0)
                    {
                        _teshubiaoqian = _teshubiaoqian + " 孕";
                    }

                    _tmp = dt.Rows[i]["is_hypertension"].ToString();
                    if (_tmp == "") _tmp = "0";
                    if(int.Parse(_tmp) !=0)
                    {
                        _teshubiaoqian = _teshubiaoqian+" 高"; 
                    }

                    _tmp = dt.Rows[i]["is_diabetes"].ToString();
                    if (_tmp == "") _tmp = "0";
                    if (int.Parse(_tmp) != 0)
                    {
                        _teshubiaoqian = _teshubiaoqian + " 糖"; 
                    }

                    _tmp = dt.Rows[i]["is_psychosis"].ToString();
                    if (_tmp == "") _tmp = "0";
                    if (int.Parse(_tmp) != 0)
                    {
                        _teshubiaoqian = _teshubiaoqian + " 精";
                    }

                    _tmp = dt.Rows[i]["is_tuberculosis"].ToString();
                    if (_tmp == "") _tmp = "0";
                    if (int.Parse(_tmp) != 0)
                    {
                        _teshubiaoqian = _teshubiaoqian + " 结";
                    }

                    _tmp = dt.Rows[i]["is_poor"].ToString();
                    if (_tmp == "") _tmp = "0";
                    if (int.Parse(_tmp) != 0)
                    {
                        _teshubiaoqian = _teshubiaoqian + " 贫";
                    }
                    #endregion

                    dr[4] = _teshubiaoqian.Trim();
                    dr[5] = dt.Rows[i][4].ToString();
                    dr[6] = dt.Rows[i][5].ToString();
                    dr[7] = dt.Rows[i][6].ToString();
                    dtfinished.Rows.Add(dr);
                }
                this.dataGridView1.DataSource = dtfinished;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular);
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
                this.dataGridView1.Columns[2].HeaderCell.Value = "档案编号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "身份证号";
                this.dataGridView1.Columns[4].HeaderCell.Value = "重点人群标签";
                this.dataGridView1.Columns[5].HeaderCell.Value = "创建人";
                this.dataGridView1.Columns[6].HeaderCell.Value = "创建时间";
                this.dataGridView1.Columns[7].HeaderCell.Value = "责任医生";

                this.dataGridView1.ReadOnly = true;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                LoadingHelper.CloseForm();
            }
            catch (Exception ex)
            {
                LoadingHelper.CloseForm();
                MessageBox.Show("出错,请联系请联系管理员！" + ex.Message + "/r/n" + ex.StackTrace);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //添加 修改 高血压随访记录历史表 调此方法
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string name = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string archiveno = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            aUPersonalBasicInfo hm = new aUPersonalBasicInfo();
            hm.label47.Text = "添加个人基本信息表";
            hm.Text = "添加个人基本信息表";
            hm.textBox1.Text = name;
            hm.textBox2.Text = archiveno;
            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                button5_Click(null, null);
                MessageBox.Show("添加成功！");
            }
        }
        private void OpenWindow()
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            aUPersonalBasicInfoo hm = new aUPersonalBasicInfoo();
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            hm.id = id;
            hm.label47.Text = "修改个人基本信息表";
            hm.Text = "修改个人基本信息表";
            DataTable dt = pBasicInfo.queryPersonalBasicInfo0(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                hm.textBox1.Text = dt.Rows[0]["name"].ToString();
                hm.oldname= dt.Rows[0]["name"].ToString();

                hm.textBox2.Text = dt.Rows[0]["archive_no"].ToString();
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton1.Tag.ToString()) { hm.radioButton1.Checked = true; };
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton2.Tag.ToString()) { hm.radioButton2.Checked = true; };
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton3.Tag.ToString()) { hm.radioButton3.Checked = true; };
                if (dt.Rows[0]["sex"].ToString() == hm.radioButton25.Tag.ToString()) { hm.radioButton25.Checked = true; };
                hm.textBox12.Text = dt.Rows[0]["id_number"].ToString();
                if (dt.Rows[0]["birthday"].ToString() != "" && dt.Rows[0]["birthday"].ToString().IndexOf("-") > 0)
                {
                    hm.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["birthday"].ToString());
                }
                else {
                    hm.dateTimePicker1.Value = DateTime.Parse(hm.textBox12.Text.Substring(6,4)+ "-"+hm.textBox12.Text.Substring(10, 2) + "-" + hm.textBox12.Text.Substring(12, 2));
                }
                hm.textBox12.Text = dt.Rows[0]["id_number"].ToString();
                hm.oldidnumber= dt.Rows[0]["id_number"].ToString();

                hm.textBox14.Text = dt.Rows[0]["company"].ToString();
                if(dt.Rows[0]["residence_address"].ToString()=="")
                {
                    hm.textBox46.Text = dt.Rows[0]["county_name"].ToString() + dt.Rows[0]["towns_name"].ToString() + dt.Rows[0]["village_name"].ToString();
                }
                else
                {
                    hm.textBox46.Text = dt.Rows[0]["residence_address"].ToString();
                }
                
                hm.textBox44.Text = dt.Rows[0]["address"].ToString();

                hm.textBox16.Text = dt.Rows[0]["phone"].ToString();
                hm.textBox18.Text = dt.Rows[0]["link_name"].ToString();
                hm.textBox20.Text = dt.Rows[0]["link_phone"].ToString();
                if (dt.Rows[0]["resident_type"].ToString() == hm.radioButton4.Tag.ToString()) { hm.radioButton4.Checked = true; };
                if (dt.Rows[0]["resident_type"].ToString() == hm.radioButton5.Tag.ToString()) { hm.radioButton5.Checked = true; };
                string nation = dt.Rows[0]["nation"].ToString();
                if (nation != null && !"".Equals(nation))
                {
                    if (nation == hm.radioButton6.Tag.ToString())
                    {
                        hm.radioButton6.Checked = true;
                    }
                    else { hm.radioButton7.Checked = true; hm.comboBox1.Visible = true; hm.mzid = nation; };
                }
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton8.Tag.ToString()) { hm.radioButton8.Checked = true; };
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton9.Tag.ToString()) { hm.radioButton9.Checked = true; };
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton10.Tag.ToString()) { hm.radioButton10.Checked = true; };
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton11.Tag.ToString()) { hm.radioButton11.Checked = true; };
                if (dt.Rows[0]["blood_group"].ToString() == hm.radioButton12.Tag.ToString()) { hm.radioButton12.Checked = true; };

                if (dt.Rows[0]["blood_rh"].ToString() == hm.radioButton13.Tag.ToString()) { hm.radioButton13.Checked = true; };
                if (dt.Rows[0]["blood_rh"].ToString() == hm.radioButton14.Tag.ToString()) { hm.radioButton14.Checked = true; };
                if (dt.Rows[0]["blood_rh"].ToString() == hm.radioButton15.Tag.ToString()) { hm.radioButton15.Checked = true; };

                if (dt.Rows[0]["education"].ToString() == hm.radioButton22.Tag.ToString()) { hm.radioButton22.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton23.Tag.ToString()) { hm.radioButton23.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton24.Tag.ToString()) { hm.radioButton24.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton26.Tag.ToString()) { hm.radioButton26.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton27.Tag.ToString()) { hm.radioButton27.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton28.Tag.ToString()) { hm.radioButton28.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton29.Tag.ToString()) { hm.radioButton29.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton30.Tag.ToString()) { hm.radioButton30.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton31.Tag.ToString()) { hm.radioButton31.Checked = true; };
                if (dt.Rows[0]["education"].ToString() == hm.radioButton32.Tag.ToString()) { hm.radioButton32.Checked = true; };

                if (dt.Rows[0]["profession"].ToString() == hm.radioButton33.Tag.ToString()) { hm.radioButton33.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton34.Tag.ToString()) { hm.radioButton34.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton35.Tag.ToString()) { hm.radioButton35.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton36.Tag.ToString()) { hm.radioButton36.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton37.Tag.ToString()) { hm.radioButton37.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton38.Tag.ToString()) { hm.radioButton38.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton39.Tag.ToString()) { hm.radioButton39.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton40.Tag.ToString()) { hm.radioButton40.Checked = true; };
                if (dt.Rows[0]["profession"].ToString() == hm.radioButton41.Tag.ToString()) { hm.radioButton41.Checked = true; };

                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton42.Tag.ToString()) { hm.radioButton42.Checked = true; };
                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton43.Tag.ToString()) { hm.radioButton43.Checked = true; };
                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton44.Tag.ToString()) { hm.radioButton44.Checked = true; };
                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton45.Tag.ToString()) { hm.radioButton45.Checked = true; };
                if (dt.Rows[0]["marital_status"].ToString() == hm.radioButton46.Tag.ToString()) { hm.radioButton46.Checked = true; };

                if (dt.Rows[0]["is_heredity"].ToString() == hm.radioButton48.Tag.ToString())
                {
                    hm.radioButton48.Checked = true;
                }
                else if (dt.Rows[0]["is_heredity"].ToString() == hm.radioButton47.Tag.ToString())
                {
                    hm.radioButton47.Checked = true;
                    hm.textBox36.Text = dt.Rows[0]["heredity_name"].ToString();
                };

                foreach (Control ctr in hm.panel12.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        //if (dt.Rows[0]["pay_type"].ToString().IndexOf(ck.Text) > -1)
                        string[] ck2 = dt.Rows[0]["pay_type"].ToString().Split(',');
                        for (int i = 0; i < ck2.Length; i++)
                        {
                            if (ck2[i].ToString() == ck.Tag.ToString())
                            {
                                ck.Checked = true;
                            }
                            if (ck2[i].ToString() == "8")
                            {
                                hm.textBox38.Enabled = true;
                                hm.textBox38.Text = dt.Rows[0]["pay_other"].ToString();
                            }
                        }
                    }
                }

                foreach (Control ctr in hm.panel13.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        string[] ck2 = dt.Rows[0]["drug_allergy"].ToString().Split(',');
                        for (int i = 0; i < ck2.Length; i++)
                        {
                            if (ck2[i].ToString() == ck.Tag.ToString())
                            {
                                ck.Checked = true;
                            }
                            if (ck2[i].ToString() == "5")
                            {
                                hm.textBox39.Enabled = true;
                                hm.textBox39.Text = dt.Rows[0]["allergy_other"].ToString();
                            }
                        }
                    }
                }
                foreach (Control ctr in hm.panel14.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        string[] ck2 = dt.Rows[0]["exposure"].ToString().Split(',');
                        for (int i = 0; i < ck2.Length; i++)
                        {
                            if (ck2[i].ToString() == ck.Tag.ToString())
                            {
                                ck.Checked = true;
                            }
                        }
                    }
                }

                foreach (Control ctr in hm.panel20.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        string[] ck2 = dt.Rows[0]["is_deformity"].ToString().Split(',');
                        for (int i = 0; i < ck2.Length; i++)
                        {
                            if (ck2[i].ToString() == ck.Tag.ToString())
                            {
                                if (ck2[i].ToString() == "8")
                                {
                                    hm.textBox37.Text = dt.Rows[0]["deformity_name"].ToString();
                                }
                                ck.Checked = true;
                            }
                        }
                    }
                }

                if (dt.Rows[0]["kitchen"].ToString() == hm.radioButton18.Tag.ToString()) { hm.radioButton18.Checked = true; };
                if (dt.Rows[0]["kitchen"].ToString() == hm.radioButton19.Tag.ToString()) { hm.radioButton19.Checked = true; };
                if (dt.Rows[0]["kitchen"].ToString() == hm.radioButton20.Tag.ToString()) { hm.radioButton20.Checked = true; };
                if (dt.Rows[0]["kitchen"].ToString() == hm.radioButton21.Tag.ToString()) { hm.radioButton21.Checked = true; };

                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton70.Tag.ToString()) { hm.radioButton70.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton71.Tag.ToString()) { hm.radioButton71.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton72.Tag.ToString()) { hm.radioButton72.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton73.Tag.ToString()) { hm.radioButton73.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton74.Tag.ToString()) { hm.radioButton74.Checked = true; };
                if (dt.Rows[0]["fuel"].ToString() == hm.radioButton75.Tag.ToString()) { hm.radioButton75.Checked = true; };

                if (dt.Rows[0]["drink"].ToString() == hm.radioButton76.Tag.ToString()) { hm.radioButton76.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton77.Tag.ToString()) { hm.radioButton77.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton78.Tag.ToString()) { hm.radioButton78.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton79.Tag.ToString()) { hm.radioButton79.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton80.Tag.ToString()) { hm.radioButton80.Checked = true; };
                if (dt.Rows[0]["drink"].ToString() == hm.radioButton81.Tag.ToString()) { hm.radioButton81.Checked = true; };

                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton82.Tag.ToString()) { hm.radioButton82.Checked = true; };
                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton83.Tag.ToString()) { hm.radioButton83.Checked = true; };
                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton84.Tag.ToString()) { hm.radioButton84.Checked = true; };
                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton85.Tag.ToString()) { hm.radioButton85.Checked = true; };
                if (dt.Rows[0]["toilet"].ToString() == hm.radioButton86.Tag.ToString()) { hm.radioButton86.Checked = true; };

                if (dt.Rows[0]["poultry"].ToString() == hm.radioButton87.Tag.ToString()) { hm.radioButton87.Checked = true; };
                if (dt.Rows[0]["poultry"].ToString() == hm.radioButton88.Tag.ToString()) { hm.radioButton88.Checked = true; };
                if (dt.Rows[0]["poultry"].ToString() == hm.radioButton89.Tag.ToString()) { hm.radioButton89.Checked = true; };
                if (dt.Rows[0]["poultry"].ToString() == hm.radioButton90.Tag.ToString()) { hm.radioButton90.Checked = true; };

                //是否贫困户
                string _poor= dt.Rows[0]["is_poor"].ToString();
                if (_poor == "") _poor = "0";
                if(int.Parse(_poor)==0)
                {
                    hm.radpinkun0.Checked = true;
                }
                else
                {
                    hm.radpinkun1.Checked = true;
                }
                string _signing = dt.Rows[0]["is_signing"].ToString();
                if (_signing == "") _signing = "0";
                if (int.Parse(_signing) == 0)
                {
                    hm.radioButton17.Checked = true;
                }
                else
                {
                    hm.radioButton16.Checked = true;
                }

                string _isgravida = dt.Rows[0]["is_gravida"].ToString();
                if (_isgravida == "") _isgravida = "0";
                if (int.Parse(_isgravida) == 0)
                {
                    hm.radioButton50.Checked = true;
                }
                else
                {
                    hm.radioButton49.Checked = true;
                }

            }
            else { MessageBox.Show("未查询到数据"+id); } 

            if (hm.ShowDialog() == DialogResult.OK)
            {
                //刷新页面
                button5_Click(null, null);
                MessageBox.Show("修改成功！");

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenWindow();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
                bool istrue = pBasicInfo.deletePersonalBasicInfo(id);
                if (istrue)
                {
                    //刷新页面
                    button5_Click(null, null);
                    MessageBox.Show("删除成功！");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label5.Visible = this.textBox1.Text.Length < 1;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox2.DataSource = null;
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox1.SelectedValue == null) return;
            shengcode = this.comboBox1.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox2, areadao.shiInfo(shengcode)); 
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox2.SelectedValue == null) return;
            shicode = this.comboBox2.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox3, areadao.quxianInfo(shicode)); 
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox3.SelectedValue == null) return;
            qxcode = this.comboBox3.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox4, areadao.zhenInfo(qxcode)); 
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox5.DataSource = null;
            if (this.comboBox4.SelectedValue == null) return;
            xzcode = this.comboBox4.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox5, areadao.cunInfo(xzcode)); 
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBox5.SelectedValue == null) return;
            xcuncode = this.comboBox5.SelectedValue.ToString();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < e.RowCount; i++)
            {
                dataGridView1.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }

            for (int i = e.RowIndex + e.RowCount; i < this.dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells["BiaoQian"].Value.ToString() !="")
            {
                dataGridView1.Rows[e.RowIndex].Cells["BiaoQian"].Style.ForeColor = Color.Red;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenWindow();
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(77, 177, 81), Color.FromArgb(77, 177, 81));
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString("查询", new System.Drawing.Font("微软雅黑", 12, System.Drawing.FontStyle.Regular), new SolidBrush(Color.White), new PointF(20, 6));

        }
        
        private void button2_Paint(object sender, PaintEventArgs e)
        {
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, Color.FromArgb(81, 95, 154), Color.FromArgb(81, 95, 154));
            base.OnPaint(e);   
            string wenzi = "修改"; 
            Font font = new Font("微软雅黑", 14F);
            Brush bush = Brushes.White;
            ControlCircular.DrawFont(e, wenzi, font, bush);  
        }

        private void personalBasicInfo_Resize(object sender, EventArgs e)
        {
            //float newx = (this.Width) / xMy;
            //float newy = (this.Height) / yMy;
            //Common.setControls(newx, newy, this);
        }
    }
}
