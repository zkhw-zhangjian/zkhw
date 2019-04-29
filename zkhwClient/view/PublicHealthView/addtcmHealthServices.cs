using MySql.Data.MySqlClient;
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
    public partial class addtcmHealthServices : Form
    {
        private string YS { get; set; } = basicInfoSettings.zeren_doctor;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 档案编号
        /// </summary>
        public string aichive_no { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string id_number { get; set; }
        public addtcmHealthServices()
        {
            InitializeComponent();
            姓名.Text = Name;
        }

        private void 确定_Click(object sender, EventArgs e)
        {

        }

        private void 计算_Click(object sender, EventArgs e)
        {
            string[] bh = GetFen().Split('|');
            List<int> qxlist = new List<int>();
            List<int> yxlist = new List<int>();
            List<int> yixlist = new List<int>();
            List<int> tslist = new List<int>();
            List<int> srlist = new List<int>();
            List<int> xylist = new List<int>();
            List<int> qylist = new List<int>();
            List<int> tylist = new List<int>();
            List<int> hplist = new List<int>();
            for (int i = 0; i < bh.Length; i++)
            {
                string[] zi = bh[i].Split(':');
                if (zi[0] == "2" || zi[0] == "3" || zi[0] == "4" || zi[0] == "14")
                {
                    qxlist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "11" || zi[0] == "12" || zi[0] == "13" || zi[0] == "29")
                {
                    yxlist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "10" || zi[0] == "21" || zi[0] == "26" || zi[0] == "31")
                {
                    yixlist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "9" || zi[0] == "16" || zi[0] == "28" || zi[0] == "32")
                {
                    tslist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "23" || zi[0] == "25" || zi[0] == "27" || zi[0] == "30")
                {
                    srlist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "19" || zi[0] == "22" || zi[0] == "24" || zi[0] == "33")
                {
                    xylist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "5" || zi[0] == "6" || zi[0] == "7" || zi[0] == "8")
                {
                    qylist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "15" || zi[0] == "17" || zi[0] == "18" || zi[0] == "20")
                {
                    tylist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "1" || zi[0] == "2" || zi[0] == "4" || zi[0] == "5" || zi[0] == "13")
                {
                    if (zi[0] != "1")
                    {
                        hplist.Add(Convert.ToInt32(FX(zi[1])));
                    }
                    else
                    {
                        hplist.Add(Convert.ToInt32(zi[1]));
                    }
                }
            }
            #region 气虚质体质
            a1.Text = "1．得分 " + qxlist.Sum();
            if (qxlist.Sum() >= 11)
            {
                a2.Checked = true;
            }
            else if (qxlist.Sum() >= 9 && qxlist.Sum() <= 10)
            {
                a3.Checked = true;
            }
            #endregion
            #region 阳虚质体质
            b1.Text = "1．得分 " + yxlist.Sum();
            if (yxlist.Sum() >= 11)
            {
                b2.Checked = true;
            }
            else if (yxlist.Sum() >= 9 && yxlist.Sum() <= 10)
            {
                b3.Checked = true;
            }
            #endregion
            #region 阴虚质体质
            c1.Text = "1．得分 " + yixlist.Sum();
            if (yixlist.Sum() >= 11)
            {
                c2.Checked = true;
            }
            else if (yixlist.Sum() >= 9 && yixlist.Sum() <= 10)
            {
                c3.Checked = true;
            }
            #endregion
            #region 痰湿质体质
            d1.Text = "1．得分 " + tslist.Sum();
            if (tslist.Sum() >= 11)
            {
                d2.Checked = true;
            }
            else if (tslist.Sum() >= 9 && tslist.Sum() <= 10)
            {
                d3.Checked = true;
            }
            #endregion
            #region 湿热质体质
            e1.Text = "1．得分 " + srlist.Sum();
            if (srlist.Sum() >= 11)
            {
                e2.Checked = true;
            }
            else if (srlist.Sum() >= 9 && srlist.Sum() <= 10)
            {
                e3.Checked = true;
            }
            #endregion
            #region 血瘀质体质
            f1.Text = "1．得分 " + xylist.Sum();
            if (xylist.Sum() >= 11)
            {
                f2.Checked = true;
            }
            else if (xylist.Sum() >= 9 && xylist.Sum() <= 10)
            {
                f3.Checked = true;
            }
            #endregion
            #region 气郁质体质
            g1.Text = "1．得分 " + qylist.Sum();
            if (qylist.Sum() >= 11)
            {
                g2.Checked = true;
            }
            else if (qylist.Sum() >= 9 && qylist.Sum() <= 10)
            {
                g3.Checked = true;
            }
            #endregion
            #region 特禀质体质
            h1.Text = "1．得分 " + tylist.Sum();
            if (tylist.Sum() >= 11)
            {
                h2.Checked = true;
            }
            else if (tylist.Sum() >= 9 && tylist.Sum() <= 10)
            {
                h3.Checked = true;
            }
            #endregion
            #region 平和质体质
            i1.Text = "1．得分 " + hplist.Sum();
            if (hplist.Sum() >= 17 && qxlist.Sum() <= 8 && yxlist.Sum() <= 8 && yixlist.Sum() <= 8 && tslist.Sum() <= 8 && srlist.Sum() <= 8 && xylist.Sum() <= 8 && qylist.Sum() <= 8 && tylist.Sum() <= 8)
            {
                i2.Checked = true;
            }
            else if (hplist.Sum() >= 17 && qxlist.Sum() <= 10 && yxlist.Sum() <= 10 && yixlist.Sum() <= 10 && tslist.Sum() <= 10 && srlist.Sum() <= 10 && xylist.Sum() <= 10 && qylist.Sum() <= 10 && tylist.Sum() <= 10)
            {
                i3.Checked = true;
            }
            #endregion
        }

        private string GetFen()
        {
            string res = string.Empty;
            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if (ctrl is GroupBox)
                {
                    foreach (Control item in ctrl.Controls)
                    {
                        if (item is RadioButton)
                        {
                            if (((RadioButton)item).Checked)
                            {
                                res += ((RadioButton)item).Tag.ToString().Replace(".", ":") + "|";
                            }
                        }
                    }
                }
            }
            return res.TrimEnd('|');
        }

        private Dictionary<string, int> TZ()
        {
            string[] bh = GetFen().Split('|');
            List<int> qxlist = new List<int>();
            List<int> yxlist = new List<int>();
            List<int> yixlist = new List<int>();
            List<int> tslist = new List<int>();
            List<int> srlist = new List<int>();
            List<int> xylist = new List<int>();
            List<int> qylist = new List<int>();
            List<int> tylist = new List<int>();
            List<int> hplist = new List<int>();
            for (int i = 0; i < bh.Length; i++)
            {
                string[] zi = bh[i].Split(':');
                if (zi[0] == "2" || zi[0] == "3" || zi[0] == "4" || zi[0] == "14")
                {
                    qxlist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "11" || zi[0] == "12" || zi[0] == "13" || zi[0] == "29")
                {
                    yxlist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "10" || zi[0] == "21" || zi[0] == "26" || zi[0] == "31")
                {
                    yixlist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "9" || zi[0] == "16" || zi[0] == "28" || zi[0] == "32")
                {
                    tslist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "23" || zi[0] == "25" || zi[0] == "27" || zi[0] == "30")
                {
                    srlist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "19" || zi[0] == "22" || zi[0] == "24" || zi[0] == "33")
                {
                    xylist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "5" || zi[0] == "6" || zi[0] == "7" || zi[0] == "8")
                {
                    qylist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "15" || zi[0] == "17" || zi[0] == "18" || zi[0] == "20")
                {
                    tylist.Add(Convert.ToInt32(zi[1]));
                }
                if (zi[0] == "1" || zi[0] == "2" || zi[0] == "4" || zi[0] == "5" || zi[0] == "13")
                {
                    if (zi[0] != "1")
                    {
                        hplist.Add(Convert.ToInt32(FX(zi[1])));
                    }
                    else
                    {
                        hplist.Add(Convert.ToInt32(zi[1]));
                    }
                }
            }
            var vs = new Dictionary<string, int>();
            vs.Add("气虚质体质", qxlist.Sum());
            vs.Add("阳虚质体质", yxlist.Sum());
            vs.Add("阴虚质体质", yixlist.Sum());
            vs.Add("痰湿质体质", tslist.Sum());
            vs.Add("湿热质体质", srlist.Sum());
            vs.Add("血瘀质体质", xylist.Sum());
            vs.Add("气郁质体质", qylist.Sum());
            vs.Add("特禀质体质", tylist.Sum());
            vs.Add("平和质体质", hplist.Sum());
            return vs;
        }

        private Dictionary<string, string> BJ()
        {
            var vs = new Dictionary<string, string>();
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                if (ctrl is GroupBox)
                {
                    if (((GroupBox)ctrl).Name.IndexOf("保健") > 0)
                    {
                        string res = string.Empty;
                        foreach (Control item in ctrl.Controls)
                        {
                            if (item is CheckBox)
                            {
                                if (((CheckBox)item).Checked)
                                {
                                    res += ((CheckBox)item).Tag.ToString() + ",";
                                }
                            }
                        }
                        vs.Add(((GroupBox)ctrl).Name.Replace("保健", ""), res.TrimEnd(','));
                    }
                }
            }
            return vs;

        }

        private int Insert()
        {
            string res = GetFen();
            DateTime time = DateTime.Now;
            string issql = @"insert into elderly_tcm_record(id,name,aichive_no,id_number,test_date,answer_result,qixuzhi_score,qixuzhi_result,yangxuzhi_score,yangxuzhi_resultyinxuzhi_score,yinxuzhi_result,tanshizhi_score,tanshizhi_result,shirezhi_score,shirezhi_result,xueyuzhi_score,xueyuzhi_result,qiyuzhi_score,qiyuzhi_result,tebingzhi_sorce,tebingzhi_result,pinghezhi_sorce,pinghezhi_result,test_doctor,create_user,create_name,create_org,create_org_name,create_time,update_user,update_name,update_time,upload_status,upload_time,upload_result) values(@id,@name,@aichive_no,@id_number,@test_date,@answer_result,@qixuzhi_score,@qixuzhi_result,@yangxuzhi_score,@yangxuzhi_resultyinxuzhi_score,@yinxuzhi_result,@tanshizhi_score,@tanshizhi_result,@shirezhi_score,@shirezhi_result,@xueyuzhi_score,@xueyuzhi_result,@qiyuzhi_score,@qiyuzhi_result,@tebingzhi_sorce,@tebingzhi_result,@pinghezhi_sorce,@pinghezhi_result,@test_doctor,@create_user,@create_name,@create_org,@create_org_name,@create_time,@update_user,@update_name,@update_time,@upload_status,@upload_time,@upload_result)";
            MySqlParameter[] args = new MySqlParameter[] {
                    new MySqlParameter("@id",Result.GetNewId()),
                    new MySqlParameter("@name", Name),
                    new MySqlParameter("@aichive_no", aichive_no),
                    new MySqlParameter("@id_number", id_number),
                    new MySqlParameter("@test_date", time),
                    new MySqlParameter("@answer_result", res),
                    new MySqlParameter("@qixuzhi_score", res),
                    new MySqlParameter("@answer_result", res),
                    new MySqlParameter("@answer_result", res),
                    new MySqlParameter("@answer_result", res),
                    new MySqlParameter("@answer_result", res),
            };
            return DbHelperMySQL.ExecuteSql(issql, args);
        }

        private string FX(string zi)
        {
            switch (zi)
            {
                case "1":
                    return "5";
                case "2":
                    return "4";
                case "3":
                    return "3";
                case "4":
                    return "2";
                case "5":
                    return "1";
                default:
                    break;
            }
            return null;
        }
    }
}
