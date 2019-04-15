using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
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
        UserInfo user = null;
        public basicInfoInit()
        {
            InitializeComponent();
        }
        private void basicInfoInit_Load(object sender, EventArgs e)
        {
            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            shengcode = this.comboBox1.SelectedValue.ToString();
            this.comboBox2.DataSource = areadao.shiInfo(shengcode);//绑定数据源
            this.comboBox2.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox2.ValueMember = "code";//操作时获取的值 
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            shicode = this.comboBox2.SelectedValue.ToString();
            this.comboBox3.DataSource = areadao.quxianInfo(shicode);//绑定数据源
            this.comboBox3.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox3.ValueMember = "code";//操作时获取的值 
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            qxcode = this.comboBox3.SelectedValue.ToString();
            this.comboBox4.DataSource = areadao.zhenInfo(qxcode);//绑定数据源
            this.comboBox4.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox4.ValueMember = "code";//操作时获取的值 
            this.comboBox5.DataSource = null;
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            xzcode = this.comboBox4.SelectedValue.ToString();
            //this.comboBox5.DataSource = areadao.cunInfo(xzcode);//绑定数据源
            //this.comboBox5.DisplayMember = "name";//显示给用户的数据集表项
            //this.comboBox5.ValueMember = "code";//操作时获取的值 
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {       
            //xcuncode = this.comboBox5.SelectedValue.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {

            if (xzcode != null && !"".Equals(xzcode))
            {
                DataTable dtOrganization = logindao.checkLtdOrganizationBycode(xzcode);
                if (dtOrganization.Rows.Count>0) {
                    string organcode = dtOrganization.Rows[0]["organ_code"].ToString();
                    organ = new ltdOrganization();
                    organ.id = dtOrganization.Rows[0]["id"].ToString();
                    organ.pub_orgcode = dtOrganization.Rows[0]["pub_orgcode"].ToString();
                    organ.organ_code = organcode;
                    organ.organ_name = dtOrganization.Rows[0]["organ_name"].ToString();
                    organ.organ_short_name = dtOrganization.Rows[0]["organ_short_name"].ToString();
                    organ.organ_level = dtOrganization.Rows[0]["organ_level"].ToString();
                    organ.organ_parent_code = dtOrganization.Rows[0]["organ_parent_code"].ToString();
                    organ.create_user_code = dtOrganization.Rows[0]["create_user_code"].ToString();
                    organ.create_time = dtOrganization.Rows[0]["create_time"].ToString();
                    organ.update_user_code = dtOrganization.Rows[0]["update_user_code"].ToString();
                    organ.update_time = dtOrganization.Rows[0]["update_time"].ToString();
                    organ.is_delete = dtOrganization.Rows[0]["is_delete"].ToString();
                    organ.remark = dtOrganization.Rows[0]["remark"].ToString();
                    organ.province_code = dtOrganization.Rows[0]["province_code"].ToString();
                    organ.province_name = dtOrganization.Rows[0]["province_name"].ToString();
                    organ.city_code = dtOrganization.Rows[0]["city_code"].ToString();
                    organ.city_name = dtOrganization.Rows[0]["city_name"].ToString();
                    organ.county_code = dtOrganization.Rows[0]["county_code"].ToString();
                    organ.county_name = dtOrganization.Rows[0]["county_name"].ToString();
                    organ.towns_code = dtOrganization.Rows[0]["towns_code"].ToString();
                    organ.towns_name = dtOrganization.Rows[0]["towns_name"].ToString();
                    organ.village_code = dtOrganization.Rows[0]["village_code"].ToString();
                    organ.village_name = dtOrganization.Rows[0]["village_name"].ToString();
                    organ.address = dtOrganization.Rows[0]["address"].ToString();
                    organ.lng = dtOrganization.Rows[0]["lng"].ToString();
                    organ.lat = dtOrganization.Rows[0]["lat"].ToString();

                    bool istrue = logindao.addLtdOrganization(organ);
                    DataTable dtUsers = logindao.checkUsersBycode(xzcode);
                    List<UserInfo> listed = new List<UserInfo>();
                    if (dtUsers.Rows.Count > 0)
                    {
                        for(int i=0;i< dtUsers.Rows.Count;i++)
                        {
                            user = new UserInfo();
                            user.userName = dtUsers.Rows[i]["lat"].ToString();
                            user.password = dtUsers.Rows[i]["lat"].ToString();
                            user.user_code = dtUsers.Rows[i]["user_code"].ToString();
                            user.user_name = dtUsers.Rows[i]["user_name"].ToString();
                            user.pub_usercode = dtUsers.Rows[i]["pub_usercode"].ToString();
                            user.sex = dtUsers.Rows[i]["sex"].ToString();
                            user.job_num = dtUsers.Rows[i]["job_num"].ToString();
                            user.tele_phone = dtUsers.Rows[i]["tele_phone"].ToString();
                            user.mail = dtUsers.Rows[i]["mail"].ToString();
                            user.birthday = dtUsers.Rows[i]["birthday"].ToString();
                            user.organ_code = dtUsers.Rows[i]["organ_code"].ToString();
                            user.parent_organ = dtUsers.Rows[i]["parent_organ"].ToString();
                            user.depart_code = dtUsers.Rows[i]["depart_code"].ToString();
                            user.user_type_code = dtUsers.Rows[i]["user_type_code"].ToString();
                            user.data_level = dtUsers.Rows[i]["data_level"].ToString();
                            user.status = dtUsers.Rows[i]["status"].ToString();
                            user.is_delete = dtUsers.Rows[i]["is_delete"].ToString();
                            user.create_time = dtUsers.Rows[i]["create_time"].ToString();
                            user.create_user_code = dtUsers.Rows[i]["create_user_code"].ToString();
                            user.update_time = dtUsers.Rows[i]["update_time"].ToString();
                            user.update_user_code = dtUsers.Rows[i]["update_user_code"].ToString();
                            listed.Add(user);
                        }
                        logindao.addUsers(listed);
                    }
                }
            }
            else{
                MessageBox.Show("区域选择不完整！");
            }
        }
    }
}
