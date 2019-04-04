using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using zkhwClient.dao;

namespace zkhwClient.view.setting
{
    public partial class basicInfoSettings : Form
    {
        areaConfigDao areadao = new areaConfigDao();
        basicSettingDao bsdao = new basicSettingDao();
        UserDao userdao = new UserDao();
        public static string xcuncode = null;
        public static string xzName = null;
        public static string xcName = null;
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        public static string zeren_doctor = null;
        public static string organ_name = null;
        public static string input_name = null;
        public static string createtime = null;
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node;
        string path = @"config.xml";
        public basicInfoSettings()
        {
            InitializeComponent();
        }
        private void basicInfoSettings_Load(object sender, EventArgs e)
        {
            DataTable dtbasic= bsdao.checkBasicsettingInfo();
            if (dtbasic.Rows.Count > 0) {
                zeren_doctor = dtbasic.Rows[0]["zeren_doctor"].ToString();
                organ_name = dtbasic.Rows[0]["organ_name"].ToString();
                input_name = dtbasic.Rows[0]["input_name"].ToString();
                createtime = dtbasic.Rows[0]["create_time"].ToString();
                xcuncode = dtbasic.Rows[0]["cun_code"].ToString();
                xzcode = dtbasic.Rows[0]["xz_code"].ToString();
                if (xzcode != null && !"".Equals(xzcode))
                {
                    xzName = bsdao.selectAreaBycode(xzcode).Rows[0][0].ToString();
                }
                if (xcuncode != null && !"".Equals(xcuncode)) {
                   xcName = bsdao.selectAreaBycode(xcuncode).Rows[0][0].ToString();
                }
            }
            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 
            showCombobox();
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
            this.comboBox5.DataSource = areadao.cunInfo(xzcode);//绑定数据源
            this.comboBox5.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox5.ValueMember = "code";//操作时获取的值 
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {       
            xcuncode = this.comboBox5.SelectedValue.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            xzName = this.comboBox4.Text;
            xcName = this.comboBox5.Text;
            string organ_code = null;
            organ_name = textBox1.Text;
            input_name = this.comboBox6.SelectedValue.ToString();
            zeren_doctor = this.comboBox7.SelectedValue.ToString();
            string bc = this.comboBox8.SelectedValue.ToString();
            string xcg = this.comboBox9.SelectedValue.ToString();
            string sh = this.comboBox10.SelectedValue.ToString();
            string sgtz = this.comboBox11.SelectedValue.ToString();
            string ncg = this.comboBox12.SelectedValue.ToString();
            string xdt = this.comboBox13.SelectedValue.ToString();
            string xy = this.comboBox14.SelectedValue.ToString();
            string wx = textBox3.Text;
            string other = textBox2.Text;
            string captain = "";//this.comboBox15.SelectedValue.ToString();
            string members = textBox4.Text;
            string operation = this.comboBox16.SelectedValue.ToString();
            string car_name = this.comboBox17.SelectedValue.ToString();
            string create_user = null;
            string create_name = null;

            if (xcuncode != null && !"".Equals(xcuncode))
            {
               bool bl= bsdao.addBasicSetting(shengcode, shicode, qxcode, xzcode, xcuncode, organ_code, organ_name, input_name, zeren_doctor, bc, xcg, sh, sgtz, ncg, xdt, xy, wx, other, captain, members, operation, car_name, create_user, create_name);
                if (bl) {
                    createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //重置条码起始位置
                    xmlDoc.Load(path);
                    node = xmlDoc.SelectSingleNode("config/barnumCode");
                    node.InnerText = "10001";
                    xmlDoc.Save(path);
                    MessageBox.Show("数据保存成功！");
                }
            }
            else{
                MessageBox.Show("区域选择不完整！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("云平台同步当前机构的居民档案信息！");
        }

        private void showCombobox() {
            DataTable dtuserlist= userdao.listUserbyOrganCode(frmLogin.organCode);
            this.comboBox6.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox6.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox6.ValueMember = "uname";//操作时获取的值

            this.comboBox7.DataSource = dtuserlist.Copy();//绑定数据源
            this.comboBox7.DisplayMember = "uname";//显示给用户的数据集表项
            this.comboBox7.ValueMember = "uname";//操作时获取的值

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
        }
    }
}
