using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using zkhwClient.dao;

namespace zkhwClient.view.setting
{
    public partial class basicInfoSettings : Form
    {
        public delegate void SetFunDelegate(string a,string b,string c);
        public SetFunDelegate setFunDelegate;
        areaConfigDao areadao = new areaConfigDao();
        basicSettingDao bsdao = new basicSettingDao();
        UserDao userdao = new UserDao();
        public static string shengName = null;
        public static string shiName = null;
        public static string qxName = null;
        public static string xzName = null;
        public static string xcName = null;
        public static string xcuncode = null;
        public static string xzcode = null;
        public static string qxcode = null;
        public static string shicode = null;
        public static string shengcode = null;
        public static string zeren_doctor = null;
        public static string zeren_doctorId = null;
        public static string organ_name = null;
        public static string input_name = null;
        public static string createtime = null;
        public static string allareaname = null;
        public static string bc = null;
        public static string xcg = null;
        public static string sh = null;
        public static string sgtz = null;
        public static string ncg = null;
        public static string xdt = null;
        public static string xy = null;
        public static string wx = null;
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node;
        string path = @"config.xml";

        string issave = "0";
        public basicInfoSettings()
        {
            InitializeComponent();
        }
        private void basicInfoSettings_Load(object sender, EventArgs e)
        {
            issave = "0";
            Common.SetComboBoxInfo(comboBox1, ltdorganizationDao.GetShengInfo());
            showCombobox();

            DataTable dtbasic= bsdao.checkBasicsettingInfo();
            if (dtbasic.Rows.Count > 0)
            {
                zeren_doctor = dtbasic.Rows[0]["zeren_doctor"].ToString();
                zeren_doctorId = dtbasic.Rows[0]["update_user"].ToString();
                organ_name = dtbasic.Rows[0]["organ_name"].ToString();
                input_name = dtbasic.Rows[0]["input_name"].ToString();
                createtime = dtbasic.Rows[0]["create_time"].ToString();
                xcuncode = dtbasic.Rows[0]["cun_code"].ToString();
                shengcode = dtbasic.Rows[0]["sheng_code"].ToString();

                Common.SetComboBoxInfo(comboBox2, ltdorganizationDao.GetCityInfo(shengcode));                

                shicode = dtbasic.Rows[0]["shi_code"].ToString();

                Common.SetComboBoxInfo(comboBox3, ltdorganizationDao.GetCountyInfo(shicode));

                qxcode = dtbasic.Rows[0]["qx_code"].ToString();
                Common.SetComboBoxInfo(comboBox4, areadao.zhenInfo(qxcode));

                //this.comboBox4.DataSource = areadao.zhenInfo(qxcode);//绑定数据源
                //this.comboBox4.DisplayMember = "name";//显示给用户的数据集表项
                //this.comboBox4.ValueMember = "code";//操作时获取的值

                xzcode = dtbasic.Rows[0]["xz_code"].ToString();
                Common.SetComboBoxInfo(comboBox5, areadao.cunInfo(xzcode));

                //this.comboBox5.DataSource = areadao.cunInfo(xzcode);//绑定数据源
                //this.comboBox5.DisplayMember = "name";//显示给用户的数据集表项
                //this.comboBox5.ValueMember = "code";//操作时获取的值

                allareaname = dtbasic.Rows[0]["allFullName"].ToString();
                bc = dtbasic.Rows[0]["bc"].ToString();
                xcg = dtbasic.Rows[0]["xcg"].ToString();
                sh = dtbasic.Rows[0]["sh"].ToString();
                sgtz = dtbasic.Rows[0]["sgtz"].ToString();
                ncg = dtbasic.Rows[0]["ncg"].ToString();
                xdt = dtbasic.Rows[0]["xdt"].ToString();
                xy = dtbasic.Rows[0]["xy"].ToString();
                wx = dtbasic.Rows[0]["wx"].ToString();
                string czy = dtbasic.Rows[0]["operation"].ToString();
                string carname = dtbasic.Rows[0]["car_name"].ToString();
                string other = dtbasic.Rows[0]["other"].ToString();
                if (xzcode != null && !"".Equals(xzcode))
                {
                    xzName = bsdao.selectAreaBycode(xzcode).Rows[0][0].ToString();
                }
                if (xcuncode != null && !"".Equals(xcuncode))
                {
                    xcName = bsdao.selectAreaBycode(xcuncode).Rows[0][0].ToString();
                }
                shengName = bsdao.selectAreaBycode(shengcode).Rows[0][0].ToString();
                shiName = bsdao.selectAreaBycode(shicode).Rows[0][0].ToString();
                qxName = bsdao.selectAreaBycode(qxcode).Rows[0][0].ToString();
                //因为名称有可能对应不上那么就用code对应
                Common.SetComboBoxSelectIndex(comboBox1, shengcode);
                Common.SetComboBoxSelectIndex(comboBox2, shicode);
                Common.SetComboBoxSelectIndex(comboBox3, qxcode);
                Common.SetComboBoxSelectIndex(comboBox4, xzcode);
                Common.SetComboBoxSelectIndex(comboBox5, xcuncode); 

                this.textBox1.Text = organ_name;
                this.comboBox6.Text = input_name;
                this.comboBox7.Text = zeren_doctor;

                this.comboBox8.Text = bc;
                this.comboBox9.Text = xcg;
                this.comboBox10.Text = sh;
                this.comboBox11.Text = sgtz;
                this.comboBox12.Text = ncg;
                this.comboBox13.Text = xdt;
                this.comboBox14.Text = xy;
                this.comboBox16.Text = czy;
                this.comboBox17.Text = carname;

                this.textBox2.Text = other;
                this.textBox3.Text = wx;

                issave = "1";
            }
             this.comboBox7.DropDownStyle = ComboBoxStyle.DropDownList; 
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox2.DataSource = null;
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox1.SelectedValue == null) return;
            shengcode = this.comboBox1.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox2, ltdorganizationDao.GetCityInfo(shengcode));
            this.comboBox7.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
            if (this.comboBox2.SelectedValue == null) return;
            shicode = this.comboBox2.SelectedValue.ToString();
            Common.SetComboBoxInfo(comboBox3, ltdorganizationDao.GetCountyInfo(shicode)); 
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBox5.Text == "" || this.comboBox5.Text == "--请选择--" || this.comboBox5.SelectedValue==null)
            {
                MessageBox.Show("【区域设置】信息不完整!"); return;
            }
            try
            {
                xcuncode = this.comboBox5.SelectedValue.ToString();
            }
            catch
            {
                MessageBox.Show("出错!"); return;
            }
            if (xcuncode == null || "".Equals(xcuncode)) {
                MessageBox.Show("【区域设置】信息不完整!"); return;
            } else if (this.comboBox5.Text== "--请选择--") {
                DataTable dtxzs = areadao.cunInfo(xzcode);
                this.comboBox5.Text = dtxzs.Rows[0]["name"].ToString();
                xcuncode = dtxzs.Rows[0]["code"].ToString();
            } 
            shengName = this.comboBox1.Text;
            shiName = this.comboBox2.Text;
            qxName = this.comboBox3.Text;
            xzName = this.comboBox4.Text;
            xcName = this.comboBox5.Text;
            allareaname = this.comboBox1.Text + this.comboBox2.Text + this.comboBox3.Text + xzName + xcName;
            string organ_code = null;
            organ_name = textBox1.Text;
            if (organ_name=="" || organ_name.Length<1) {
                MessageBox.Show("建档单位不能为空!");
                return;
            }
            input_name = this.comboBox6.Text;

            zeren_doctor = this.comboBox7.Text;
            //if (shengName == "陕西")
            //{
                zeren_doctorId = this.comboBox7.SelectedValue.ToString();
            //}
            bc = this.comboBox8.Text;
            xcg = this.comboBox9.Text;
            sh = this.comboBox10.Text;
            sgtz = this.comboBox11.Text;
            ncg = this.comboBox12.Text;
            xdt = this.comboBox13.Text;
            xy = this.comboBox14.Text;
            wx = textBox3.Text;
            string other = textBox2.Text;
            string captain = ""; //this.comboBox15.SelectedValue.ToString();
            string members = "";// textBox4.Text;
            string operation = this.comboBox16.Text;
            string car_name = this.comboBox17.Text;
            string create_user = null;
            string create_name = null;

            if (xcuncode != null && !"".Equals(xcuncode))
            {
               bool bl= bsdao.addBasicSetting(shengcode, shicode, qxcode, xzcode, xcuncode, organ_code, organ_name, input_name, zeren_doctor, bc, xcg, sh, sgtz, ncg, xdt, xy, wx, other, captain, members, operation, car_name, create_user, create_name, zeren_doctorId, allareaname);
                if (bl) {
                    //createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ////重置条码起始位置
                    //xmlDoc.Load(path);
                    //node = xmlDoc.SelectSingleNode("config/barnumCode");
                    //node.InnerText = "10001";
                    //xmlDoc.Save(path);
                    //处理别的窗体的信息 

                    if (setFunDelegate == null)
                    {
                        frmMain frm =(frmMain)this.Parent.Parent;
                        frm.SetJianDangInfo(textBox1.Text.Trim(), comboBox6.Text, comboBox7.Text);
                    }
                    else
                    {  
                        setFunDelegate(textBox1.Text.Trim(), comboBox6.Text, comboBox7.Text);
                    }
                    issave = "1";
                    MessageBox.Show("数据保存成功！");
                }
            }
            else{
                MessageBox.Show("区域选择不完整！");
            }
        }
        
        private void showCombobox() {
            DataTable dtuserlist= userdao.listUserbyOrganCode(frmLogin.organCode);
            this.comboBox6.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox6.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox6.ValueMember = "uname";//操作时获取的值

            this.comboBox7.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox7.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox7.ValueMember = "ucode";//操作时获取的值

            this.comboBox8.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox8.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox8.ValueMember = "uname";//操作时获取的值

            this.comboBox8.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox8.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox8.ValueMember = "uname";//操作时获取的值

            this.comboBox9.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox9.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox9.ValueMember = "uname";//操作时获取的值

            this.comboBox10.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox10.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox10.ValueMember = "uname";//操作时获取的值

            this.comboBox11.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox11.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox11.ValueMember = "uname";//操作时获取的值

            this.comboBox12.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox12.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox12.ValueMember = "uname";//操作时获取的值

            this.comboBox13.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox13.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox13.ValueMember = "uname";//操作时获取的值

            this.comboBox14.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox14.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox14.ValueMember = "uname";//操作时获取的值

            this.comboBox16.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox16.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox16.ValueMember = "uname";//操作时获取的值

            this.comboBox17.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox17.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox17.ValueMember = "uname";//操作时获取的值
            //家医团队信息
            //DataTable dtTeamDz = bsdao.checkTeamInfoBycode(frmLogin.organCode);
            //if (dtTeamDz.Rows.Count>0) {
            //    this.comboBox15.DataSource = dtTeamDz.Copy();//绑定数据源
            //    this.comboBox15.DisplayMember = "team_lead_name";//显示给用户的数据集表项
            //    this.comboBox15.ValueMember = "team_no";//操作时获取的值
            //}
        }

        //数据初始化
        private void button2_Click(object sender, EventArgs e)
        {
            if (IsInternetAvailable())
            {
                basicInfoInit baseinfo = new basicInfoInit();
                baseinfo.Show();
            }
            else
            {
                MessageBox.Show("电脑未连接外网,请检查网络!");
            }
        }

        //判断是否连接 外网
        private bool IsInternetAvailable()
        {
            try
            {
                Dns.GetHostEntry("www.baidu.com"); //using System.Net;
                return true;
            }
            catch (SocketException ex)
            {
                return false;
            }
        }

        private void comboBox15_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //家医团队成员信息
            string teamNo = this.comboBox15.SelectedValue.ToString();
            DataTable dtTeamCy = bsdao.checkTeamInfoBycode(teamNo);
            string names = "";
            if (dtTeamCy.Rows.Count > 0)
            {  
                for (int i=0;i< dtTeamCy.Rows.Count;i++) {
                    names += "," + dtTeamCy.Rows[i]["doctor_name"].ToString();
                }
            }
            this.textBox4.Text = names.Substring(1);
        }

        private void basicInfoSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (xcuncode == null || xcuncode == "")
            //{
            //    e.Cancel = true;
            //}
            //else
            //{
            //    if(issave=="0")
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }
 

       
    }
}
