/*
 * 
 * 2019-7-15初始化不删除 resident_base_info（个人信息基本表）
 * 判断如果有就更新否则添加
 * 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using zkhwClient.bean;
using zkhwClient.dao;

namespace zkhwClient.view.setting
{
    public partial class basicInfoInit : Form
    {
        areaConfigDao areadao = new areaConfigDao();
        loginLogDao logindao = new loginLogDao();
        public static string xcuncode = null;
        public static string xzName = null;
        public static string xcName = null;
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        ltdOrganization organ = null;
        resident_base_infoBean resident = null;
        UserInfo user = null;
        public basicInfoInit()
        {
            InitializeComponent();
        }
        private void basicInfoInit_Load(object sender, EventArgs e)
        { 
            Common.SetComboBoxInfo(comboBox1, areadao.shengInfo());
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox2.DataSource = null;
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (comboBox1.SelectedValue == null) return;
            shengcode = this.comboBox1.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox2, areadao.shiInfo(shengcode));  
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (comboBox2.SelectedValue == null) return;
            shicode = this.comboBox2.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox3, areadao.quxianInfo(shicode));  
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (comboBox3.SelectedValue == null) return;
            qxcode = this.comboBox3.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox4, areadao.zhenInfo(qxcode)); 
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox4.SelectedValue == null) return;
            xzcode = this.comboBox4.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox5, areadao.cunInfo(xzcode)); 
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {       
            //xcuncode = this.comboBox5.SelectedValue.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedValue == null || comboBox4.Text== "--请选择--")
            {
                MessageBox.Show("区域选择不完整！");
                return;
            }   
            xzcode = this.comboBox4.SelectedValue.ToString(); 
            if (xzcode != null && !"".Equals(xzcode))
            {
                logindao.deleteLtdOrganization(xzcode);
                logindao.deleteUsersBycode(xzcode);
                //logindao.deleteResidentsBycode(xzcode);
                logindao.deleteThresholdValue();

                #region  同步阈值信息 
                DataTable dtThresholdValue = logindao.checkThresholdValue();
                if (dtThresholdValue.Rows.Count > 0)
                {
                    List<thresholdValueBean> listTv = new List<thresholdValueBean>();
                    for (int i = 0; i < dtThresholdValue.Rows.Count; i++)
                    {
                        thresholdValueBean tv = new thresholdValueBean();
                        tv.id = dtThresholdValue.Rows[i]["id"].ToString();
                        tv.class_type = dtThresholdValue.Rows[i]["class_type"].ToString();
                        tv.type = dtThresholdValue.Rows[i]["type"].ToString();
                        string warning_min = dtThresholdValue.Rows[i]["warning_min"].ToString();
                        tv.warning_min = warning_min == null || "".Equals(warning_min) ? "0" : warning_min;
                        string warning_max = dtThresholdValue.Rows[i]["warning_max"].ToString();
                        tv.warning_max = warning_max == null || "".Equals(warning_max) ? "0" : warning_max;
                        string threshold_min = dtThresholdValue.Rows[i]["threshold_min"].ToString();
                        tv.threshold_min = threshold_min == null || "".Equals(threshold_min) ? "0" : threshold_min;
                        string threshold_max = dtThresholdValue.Rows[i]["threshold_max"].ToString();
                        tv.threshold_max = threshold_max == null || "".Equals(threshold_max) ? "0" : threshold_max;
                        tv.create_user = dtThresholdValue.Rows[i]["create_user"].ToString();
                        tv.create_name = dtThresholdValue.Rows[i]["create_name"].ToString();
                        string create_time = dtThresholdValue.Rows[i]["create_time"].ToString();
                        tv.create_time = create_time == null || "".Equals(create_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(create_time).ToString("yyyy-MM-dd HH:mm:ss");
                        tv.update_user = dtThresholdValue.Rows[i]["update_user"].ToString();
                        tv.update_name = dtThresholdValue.Rows[i]["update_name"].ToString();
                        string update_time = dtThresholdValue.Rows[i]["update_time"].ToString();
                        tv.update_time = update_time == null || "".Equals(update_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(update_time).ToString("yyyy-MM-dd HH:mm:ss");
                        tv.chinaName= dtThresholdValue.Rows[i]["chinaName"].ToString();
                        tv.CheckMethod = dtThresholdValue.Rows[i]["checkMethod"].ToString();
                        tv.unit = dtThresholdValue.Rows[i]["unit"].ToString();
                        tv.deviceModel = dtThresholdValue.Rows[i]["deviceModel"].ToString();
                        listTv.Add(tv);
                    }
                    if (listTv.Count > 0)
                    {
                        bool istrue = logindao.addThresholdValues(listTv);
                        if (istrue)
                        {
                            this.progressBar1.Value = 5;
                        }
                    }
                }
                #endregion

                #region 同步云平台机构信息
                DataTable dtOrganization = logindao.checkLtdOrganizationBycode(xzcode);
                if (dtOrganization.Rows.Count>0)
                {
                    List<ltdOrganization> listOragn = new List<ltdOrganization>();
                    for (int i = 0; i < dtOrganization.Rows.Count; i++)
                    {
                        organ = new ltdOrganization();
                        organ.id = dtOrganization.Rows[i]["id"].ToString();
                        organ.pub_orgcode = dtOrganization.Rows[i]["pub_orgcode"].ToString();
                        organ.organ_code = dtOrganization.Rows[i]["organ_code"].ToString();
                        organ.organ_name = dtOrganization.Rows[i]["organ_name"].ToString();
                        organ.organ_short_name = dtOrganization.Rows[i]["organ_short_name"].ToString();
                        organ.organ_level = dtOrganization.Rows[i]["organ_level"].ToString();
                        organ.organ_parent_code = dtOrganization.Rows[i]["organ_parent_code"].ToString();
                        organ.create_user_code = dtOrganization.Rows[i]["create_user_code"].ToString();
                        organ.create_time = dtOrganization.Rows[i]["create_time"].ToString();
                        string create_time = dtOrganization.Rows[i]["create_time"].ToString();
                        organ.create_time = create_time == "" || "".Equals(create_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(create_time).ToString("yyyy-MM-dd HH:mm:ss");
                        organ.update_user_code = dtOrganization.Rows[i]["update_user_code"].ToString();
                        string update_time = dtOrganization.Rows[i]["update_time"].ToString();
                        organ.update_time = update_time == "" || "".Equals(update_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(update_time).ToString("yyyy-MM-dd HH:mm:ss");
                        organ.is_delete = dtOrganization.Rows[i]["is_delete"].ToString();
                        organ.remark = dtOrganization.Rows[i]["remark"].ToString();
                        organ.province_code = dtOrganization.Rows[i]["province_code"].ToString();
                        organ.province_name = dtOrganization.Rows[i]["province_name"].ToString();
                        organ.city_code = dtOrganization.Rows[i]["city_code"].ToString();
                        organ.city_name = dtOrganization.Rows[i]["city_name"].ToString();
                        organ.county_code = dtOrganization.Rows[i]["county_code"].ToString();
                        organ.county_name = dtOrganization.Rows[i]["county_name"].ToString();
                        organ.towns_code = dtOrganization.Rows[i]["towns_code"].ToString();
                        organ.towns_name = dtOrganization.Rows[i]["towns_name"].ToString();
                        organ.village_code = dtOrganization.Rows[i]["village_code"].ToString();
                        organ.village_name = dtOrganization.Rows[i]["village_name"].ToString();
                        organ.address = dtOrganization.Rows[i]["address"].ToString();
                        organ.lng = dtOrganization.Rows[i]["lng"].ToString();
                        organ.lat = dtOrganization.Rows[i]["lat"].ToString();
                        listOragn.Add(organ);
                        addUsersBycode(organ.organ_code);
                    }
                    if (listOragn.Count > 0)
                    {
                        bool istrue = logindao.addLtdOrganization(listOragn);
                        if (istrue)
                        {
                            this.progressBar1.Value = 10;
                        }
                    }
                }
                #endregion

                #region 同步云平台家医团队信息
                //    DataTable dtTeams = logindao.checkTeanInfoBycode(xzcode);
                //    if (dtTeams.Rows.Count > 0)
                //    {
                //        List<teamInfoBean> listed = new List<teamInfoBean>();
                //        for (int i = 0; i < dtTeams.Rows.Count; i++)
                //        {
                //            teamInfoBean teaninfo = new teamInfoBean();
                //            teaninfo.id = dtUsers.Rows[i]["id"].ToString();
                //            teaninfo.team_no = dtUsers.Rows[i]["team_no"].ToString();
                //            teaninfo.team_name = dtUsers.Rows[i]["team_name"].ToString();
                //            teaninfo.team_lead_doctor = dtUsers.Rows[i]["team_lead_doctor"].ToString();
                //            teaninfo.team_lead_name = dtUsers.Rows[i]["team_lead_name"].ToString();
                //            teaninfo.team_lead_phone = dtUsers.Rows[i]["team_lead_phone"].ToString();
                //            teaninfo.team_type = dtUsers.Rows[i]["team_type"].ToString();
                //            teaninfo.member_num = dtUsers.Rows[i]["member_num"].ToString();
                //            teaninfo.status = dtUsers.Rows[i]["status"].ToString();
                //            teaninfo.remark = dtUsers.Rows[i]["remark"].ToString();
                //            teaninfo.org_code = dtUsers.Rows[i]["org_code"].ToString();
                //            teaninfo.province_code = dtUsers.Rows[i]["province_code"].ToString();
                //            teaninfo.province_name = dtUsers.Rows[i]["province_name"].ToString();
                //            teaninfo.city_code = dtUsers.Rows[i]["city_code"].ToString();
                //            teaninfo.city_name = dtUsers.Rows[i]["city_name"].ToString();
                //            teaninfo.county_code = dtUsers.Rows[i]["county_code"].ToString();
                //            teaninfo.county_name = dtUsers.Rows[i]["county_name"].ToString();
                //            teaninfo.towns_code = dtUsers.Rows[i]["towns_code"].ToString();
                //            teaninfo.towns_name = dtUsers.Rows[i]["towns_name"].ToString();
                //            teaninfo.village_code = dtUsers.Rows[i]["village_code"].ToString();
                //            teaninfo.village_name = dtUsers.Rows[i]["village_name"].ToString();
                //            teaninfo.create_user = dtUsers.Rows[i]["create_user"].ToString();
                //            teaninfo.create_name = dtUsers.Rows[i]["create_name"].ToString();
                //            teaninfo.create_time = dtUsers.Rows[i]["create_time"].ToString();
                //            teaninfo.update_user = dtUsers.Rows[i]["update_user"].ToString();
                //            teaninfo.update_name = dtUsers.Rows[i]["update_name"].ToString();
                //            teaninfo.update_time = dtUsers.Rows[i]["update_time"].ToString();
                //            listed.Add(teaninfo);
                //            DataTable dtTeanDoctor = logindao.checkTeanDoctorBycode(teaninfo.team_no);
                //        if (dtTeanDoctor.Rows.Count > 0) {
                //            List<teamDoctorBean> listtd = new List<teamDoctorBean>();
                //            for (int j = 0; j < dtTeanDoctor.Rows.Count; j++)
                //            {
                //                teamDoctorBean teamdoctor = new teamDoctorBean();
                //                teamdoctor.id = dtTeanDoctor.Rows[j]["id"].ToString();
                //                teamdoctor.team_no = dtTeanDoctor.Rows[j]["team_no"].ToString();
                //                teamdoctor.team_name = dtTeanDoctor.Rows[j]["team_name"].ToString();
                //                teamdoctor.doctor_no = dtTeanDoctor.Rows[j]["doctor_no"].ToString();
                //                teamdoctor.doctor_name = dtTeanDoctor.Rows[j]["doctor_name"].ToString();
                //                teamdoctor.position = dtTeanDoctor.Rows[j]["position"].ToString();
                //                teamdoctor.create_user = dtTeanDoctor.Rows[j]["create_user"].ToString();
                //                teamdoctor.create_name = dtTeanDoctor.Rows[j]["create_name"].ToString();
                //                teamdoctor.create_time = dtTeanDoctor.Rows[j]["create_time"].ToString();
                //                teamdoctor.update_user = dtTeanDoctor.Rows[j]["update_user"].ToString();
                //                teamdoctor.update_name = dtTeanDoctor.Rows[j]["update_name"].ToString();
                //                teamdoctor.update_time= dtTeanDoctor.Rows[j]["update_time"].ToString();
                //                listtd.Add(teamdoctor);
                //            }
                //            logindao.addTeanDoctors(listtd);
                //        }
                //    }
                //            logindao.addTeanInfos(listed);
                //            this.progressBar1.Value = 40;
                //}
                #endregion

                #region 同步个人档案信息
                DataTable dtresident = logindao.checkResidentBycode(xzcode);
                if (dtresident.Rows.Count > 0)
                {
                    List<resident_base_infoBean> listresident = new List<resident_base_infoBean>();
                    int insert_num = 0;
                    for (int i = 0; i < dtresident.Rows.Count; i++)
                    {
                        resident = new resident_base_infoBean();
                        resident.id = dtresident.Rows[i]["id"].ToString();
                        resident.archive_no = dtresident.Rows[i]["archive_no"].ToString();
                        resident.pb_archive = dtresident.Rows[i]["pb_archive"].ToString();
                        resident.name = dtresident.Rows[i]["name"].ToString();
                        resident.sex = dtresident.Rows[i]["sex"].ToString();
                        resident.birthday = dtresident.Rows[i]["birthday"].ToString();
                        string age = dtresident.Rows[i]["age"].ToString();
                        resident.age = age == null || "".Equals(age) ? "0" : age;
                        resident.id_number = dtresident.Rows[i]["id_number"].ToString();
                        resident.CardPic = dtresident.Rows[i]["card_pic"].ToString();
                        resident.company = dtresident.Rows[i]["company"].ToString();
                        resident.phone = dtresident.Rows[i]["phone"].ToString();
                        resident.link_name = dtresident.Rows[i]["link_name"].ToString();
                        resident.link_phone = dtresident.Rows[i]["link_phone"].ToString();
                        resident.resident_type = dtresident.Rows[i]["resident_type"].ToString();
                        resident.register_address = dtresident.Rows[i]["register_address"].ToString();
                        resident.residence_address = dtresident.Rows[i]["residence_address"].ToString();
                        resident.nation = dtresident.Rows[i]["nation"].ToString();
                        resident.blood_group = dtresident.Rows[i]["blood_group"].ToString();
                        resident.blood_rh = dtresident.Rows[i]["blood_rh"].ToString();
                        resident.education = dtresident.Rows[i]["education"].ToString();
                        resident.profession = dtresident.Rows[i]["profession"].ToString();
                        resident.marital_status = dtresident.Rows[i]["marital_status"].ToString();
                        resident.pay_type = dtresident.Rows[i]["pay_type"].ToString();
                        resident.pay_other = dtresident.Rows[i]["pay_other"].ToString();
                        resident.drug_allergy = dtresident.Rows[i]["drug_allergy"].ToString();
                        resident.allergy_other = dtresident.Rows[i]["allergy_other"].ToString();
                        resident.exposure = dtresident.Rows[i]["exposure"].ToString();
                        resident.disease_other = dtresident.Rows[i]["disease_other"].ToString();
                        string is_hypertension = dtresident.Rows[i]["is_hypertension"].ToString();
                        resident.is_hypertension = is_hypertension == null || "".Equals(is_hypertension) ? "0" : is_hypertension;
                        string is_diabetes = dtresident.Rows[i]["is_diabetes"].ToString();
                        resident.is_diabetes = is_diabetes == null || "".Equals(is_diabetes) ? "0" : is_diabetes;
                        string is_psychosis = dtresident.Rows[i]["is_psychosis"].ToString();
                        resident.is_psychosis = is_psychosis == null || "".Equals(is_psychosis) ? "0" : is_psychosis;
                        string is_tuberculosis = dtresident.Rows[i]["is_tuberculosis"].ToString();
                        resident.is_tuberculosis = is_tuberculosis == null || "".Equals(is_tuberculosis) ? "0" : is_tuberculosis;
                        string is_heredity = dtresident.Rows[i]["is_heredity"].ToString();
                        resident.is_heredity = is_heredity == null || "".Equals(is_heredity) ? "0" : is_heredity;
                        resident.heredity_name = dtresident.Rows[i]["heredity_name"].ToString();
                        resident.is_deformity = dtresident.Rows[i]["is_deformity"].ToString();
                        resident.deformity_name = dtresident.Rows[i]["deformity_name"].ToString();
                        resident.is_poor = dtresident.Rows[i]["is_poor"].ToString();
                        resident.kitchen = dtresident.Rows[i]["kitchen"].ToString();
                        resident.fuel = dtresident.Rows[i]["fuel"].ToString();
                        resident.other_fuel = dtresident.Rows[i]["other_fuel"].ToString();
                        resident.drink = dtresident.Rows[i]["drink"].ToString();
                        resident.other_drink = dtresident.Rows[i]["other_drink"].ToString();
                        resident.toilet = dtresident.Rows[i]["toilet"].ToString();
                        resident.poultry = dtresident.Rows[i]["poultry"].ToString();
                        resident.medical_code = dtresident.Rows[i]["medical_code"].ToString();
                        resident.photo_code = dtresident.Rows[i]["photo_code"].ToString();
                        resident.aichive_org = dtresident.Rows[i]["aichive_org"].ToString();
                        resident.doctor_id = dtresident.Rows[i]["doctor_id"].ToString();
                        resident.doctor_name = dtresident.Rows[i]["doctor_name"].ToString();
                        string is_signing = dtresident.Rows[i]["is_signing"].ToString();
                        resident.is_signing = is_signing == null || "".Equals(is_signing) ? "0" : is_signing;
                        string is_synchro = dtresident.Rows[i]["is_synchro"].ToString();
                        resident.is_synchro = is_synchro == null || "".Equals(is_synchro) ? "0" : is_synchro;
                        resident.synchro_result = dtresident.Rows[i]["synchro_result"].ToString();
                        resident.synchro_time = dtresident.Rows[i]["synchro_time"].ToString();
                        resident.province_code = dtresident.Rows[i]["province_code"].ToString();
                        resident.province_name = dtresident.Rows[i]["province_name"].ToString();
                        resident.city_code = dtresident.Rows[i]["city_code"].ToString();
                        resident.city_name = dtresident.Rows[i]["city_name"].ToString();
                        resident.county_code = dtresident.Rows[i]["county_code"].ToString();
                        resident.county_name = dtresident.Rows[i]["county_name"].ToString();
                        resident.towns_code = dtresident.Rows[i]["towns_code"].ToString();
                        resident.towns_name = dtresident.Rows[i]["towns_name"].ToString();
                        resident.village_code = dtresident.Rows[i]["village_code"].ToString();
                        resident.village_name = dtresident.Rows[i]["village_name"].ToString();
                        resident.status = dtresident.Rows[i]["status"].ToString();
                        resident.remark = dtresident.Rows[i]["remark"].ToString();
                        resident.create_user = dtresident.Rows[i]["create_user"].ToString();
                        resident.create_name = dtresident.Rows[i]["create_name"].ToString();
                        string create_time = dtresident.Rows[i]["create_time"].ToString();
                        resident.create_time = create_time == null || "".Equals(create_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(create_time).ToString("yyyy-MM-dd HH:mm:ss");
                        resident.create_org = dtresident.Rows[i]["create_org"].ToString();
                        resident.create_org_name = dtresident.Rows[i]["create_org_name"].ToString();
                        resident.update_user = dtresident.Rows[i]["update_user"].ToString();
                        resident.update_name = dtresident.Rows[i]["update_name"].ToString();
                        string update_time = dtresident.Rows[i]["update_time"].ToString();
                        resident.update_time = update_time == null || "".Equals(update_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(update_time).ToString("yyyy-MM-dd HH:mm:ss");
                        listresident.Add(resident);
                        insert_num += 1;
                        if (insert_num % 1000 == 0 || (i == dtresident.Rows.Count - 1 && listresident.Count > 0))
                        {
                            bool bl = logindao.DownResidents(listresident);
                            if (bl)
                            {
                                listresident.Clear();
                                insert_num = 0;
                            }
                        }
                    }
                }
                #endregion

                this.progressBar1.Value = 100;
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("数据同步已完成");
            }
            else
            {
                MessageBox.Show("区域选择不完整！");
            }
        }
        //根据区域编号，添加用户信息
        private void addUsersBycode(string organcode)
        {
            //同步云平台用户信息
            DataTable dtUsers = logindao.checkUsersBycode(organcode);
            if (dtUsers.Rows.Count > 0)
            {
                List<UserInfo> listed = new List<UserInfo>();
                for (int i = 0; i < dtUsers.Rows.Count; i++)
                {
                    user = new UserInfo();
                    user.userName = dtUsers.Rows[i]["login_name"].ToString();
                    user.password = dtUsers.Rows[i]["login_pass"].ToString();
                    user.user_code = dtUsers.Rows[i]["user_code"].ToString();
                    user.user_name = dtUsers.Rows[i]["user_name"].ToString();
                    user.pub_usercode = dtUsers.Rows[i]["pub_usercode"].ToString();
                    string sex = dtUsers.Rows[i]["sex"].ToString();
                    user.sex = sex == null || "".Equals(sex) ? "0" : sex;
                    user.job_num = dtUsers.Rows[i]["job_num"].ToString();
                    user.tele_phone = dtUsers.Rows[i]["tele_phone"].ToString();
                    user.mail = dtUsers.Rows[i]["mail"].ToString();
                    user.birthday = dtUsers.Rows[i]["birthday"].ToString();
                    user.organ_code = dtUsers.Rows[i]["organ_code"].ToString();
                    user.parent_organ = dtUsers.Rows[i]["parent_organ"].ToString();
                    user.depart_code = dtUsers.Rows[i]["depart_code"].ToString();
                    user.user_type_code = dtUsers.Rows[i]["user_type_code"].ToString();
                    string data_level = dtUsers.Rows[i]["data_level"].ToString();
                    user.data_level = data_level == null || "".Equals(data_level) ? "0" : data_level;
                    string status = dtUsers.Rows[i]["status"].ToString();
                    user.status = status == null || "".Equals(status) ? "0" : status;
                    string is_delete = dtUsers.Rows[i]["is_delete"].ToString();
                    user.is_delete = is_delete == null || "".Equals(is_delete) ? "0" : is_delete;
                    string create_time = dtUsers.Rows[i]["create_time"].ToString();
                    user.create_time = create_time == "" || "".Equals(create_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(create_time).ToString("yyyy-MM-dd HH:mm:ss");
                    string update_time = dtUsers.Rows[i]["update_time"].ToString();
                    user.update_time = update_time == "" || "".Equals(update_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(update_time).ToString("yyyy-MM-dd HH:mm:ss");
                    user.update_user_code = dtUsers.Rows[i]["update_user_code"].ToString();
                    listed.Add(user);
                }
                bool aa = logindao.addUsers(listed);
                if (aa)
                {
                    this.progressBar1.Value = 20;
                }
            }
        }
 }
}
