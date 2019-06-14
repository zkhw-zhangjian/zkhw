using MySql.Data.MySqlClient;
using System;
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
    public partial class addtcmHealthServices : Form
    {
        private string YS { get; set; } = basicInfoSettings.zeren_doctor;

        /// <summary>
        /// 状态(1:新增 0:修改)
        /// </summary>
        public int IS { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Names { get; set; }
        /// <summary>
        /// 档案编号
        /// </summary>
        public string aichive_no { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string id_number { get; set; }
        public bool show { get; set; } = true;
        public string mag { get; set; }
        public addtcmHealthServices(int s, string name, string no, string id)
        {
            InitializeComponent();
            IS = s;
            Names = name.Trim();
            aichive_no = no.Trim();
            id_number = id.Trim();
            姓名.Text = Names;
            this.Text = (IS == 1 ? "新增" : "修改");
            if (GetUpdate())
            {
                GetData();
                return;
            }
            else
            {
                show = false;
                mag = "没有修改数据！";
                return;
            }
        }

        private void 确定_Click(object sender, EventArgs e)
        {
            if ((IS == 1 ? Insert() : Update()) > 0)
            {
                MessageBox.Show("成功！");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("失败！");
            }
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 计算_Click(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            try
            {
                Clears();
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
                if (qxlist.Sum() > hplist.Sum() && qxlist.Sum() > tylist.Sum() && qxlist.Sum() > qylist.Sum() && qxlist.Sum() > xylist.Sum() && qxlist.Sum() > srlist.Sum() && qxlist.Sum() > tslist.Sum() && qxlist.Sum() > yixlist.Sum() && qxlist.Sum() > yxlist.Sum())
                {
                    #region 气虚质体质
                    a1.Text = "1．得分 " + qxlist.Sum();
                    if (qxlist.Sum() >= 11)
                    {
                        a2.Checked = true;
                        JingYong("1");
                    }
                    else if (qxlist.Sum() >= 9 && qxlist.Sum() <= 10)
                    {
                        a3.Checked = true;
                        JingYong("1");
                    }
                    #endregion
                }
                else if (yxlist.Sum() > hplist.Sum() && yxlist.Sum() > tylist.Sum() && yxlist.Sum() > qylist.Sum() && yxlist.Sum() > xylist.Sum() && yxlist.Sum() > srlist.Sum() && yxlist.Sum() > tslist.Sum() && yxlist.Sum() > yixlist.Sum() && yxlist.Sum() > qxlist.Sum())
                {
                    #region 阳虚质体质
                    b1.Text = "1．得分 " + yxlist.Sum();
                    if (yxlist.Sum() >= 11)
                    {
                        b2.Checked = true;
                        JingYong("2");
                    }
                    else if (yxlist.Sum() >= 9 && yxlist.Sum() <= 10)
                    {
                        b3.Checked = true;
                        JingYong("2");
                    }
                    #endregion
                }
                else if (yixlist.Sum() > hplist.Sum() && yixlist.Sum() > tylist.Sum() && yixlist.Sum() > qylist.Sum() && yixlist.Sum() > xylist.Sum() && yixlist.Sum() > srlist.Sum() && yixlist.Sum() > tslist.Sum() && yixlist.Sum() > yxlist.Sum() && yixlist.Sum() > qxlist.Sum())
                {
                    #region 阴虚质体质
                    c1.Text = "1．得分 " + yixlist.Sum();
                    if (yixlist.Sum() >= 11)
                    {
                        c2.Checked = true;
                        JingYong("3");
                    }
                    else if (yixlist.Sum() >= 9 && yixlist.Sum() <= 10)
                    {
                        c3.Checked = true;
                        JingYong("3");
                    }
                    #endregion
                }
                else if (tslist.Sum() > hplist.Sum() && tslist.Sum() > tylist.Sum() && tslist.Sum() > qylist.Sum() && tslist.Sum() > xylist.Sum() && tslist.Sum() > srlist.Sum() && tslist.Sum() > yixlist.Sum() && tslist.Sum() > yxlist.Sum() && tslist.Sum() > qxlist.Sum())
                {
                    #region 痰湿质体质
                    d1.Text = "1．得分 " + tslist.Sum();
                    if (tslist.Sum() >= 11)
                    {
                        d2.Checked = true;
                        JingYong("4");
                    }
                    else if (tslist.Sum() >= 9 && tslist.Sum() <= 10)
                    {
                        d3.Checked = true;
                        JingYong("4");
                    }
                    #endregion
                }
                else if (srlist.Sum() > hplist.Sum() && srlist.Sum() > tylist.Sum() && srlist.Sum() > qylist.Sum() && srlist.Sum() > xylist.Sum() && srlist.Sum() > tslist.Sum() && srlist.Sum() > yixlist.Sum() && srlist.Sum() > yxlist.Sum() && srlist.Sum() > qxlist.Sum())
                {
                    #region 湿热质体质
                    e1.Text = "1．得分 " + srlist.Sum();
                    if (srlist.Sum() >= 11)
                    {
                        e2.Checked = true;
                        JingYong("5");
                    }
                    else if (srlist.Sum() >= 9 && srlist.Sum() <= 10)
                    {
                        e3.Checked = true;
                        JingYong("5");
                    }
                    #endregion
                }
                else if (xylist.Sum() > hplist.Sum() && xylist.Sum() > tylist.Sum() && xylist.Sum() > qylist.Sum() && xylist.Sum() > srlist.Sum() && xylist.Sum() > tslist.Sum() && xylist.Sum() > yixlist.Sum() && xylist.Sum() > yxlist.Sum() && xylist.Sum() > qxlist.Sum())
                {
                    #region 血瘀质体质
                    f1.Text = "1．得分 " + xylist.Sum();
                    if (xylist.Sum() >= 11)
                    {
                        f2.Checked = true;
                        JingYong("6");
                    }
                    else if (xylist.Sum() >= 9 && xylist.Sum() <= 10)
                    {
                        f3.Checked = true;
                        JingYong("6");
                    }
                    #endregion
                }
                else if (qylist.Sum() >= hplist.Sum() && qylist.Sum() > tylist.Sum() && qylist.Sum() > xylist.Sum() && qylist.Sum() > srlist.Sum() && qylist.Sum() > tslist.Sum() && qylist.Sum() > yixlist.Sum() && qylist.Sum() > yxlist.Sum() && qylist.Sum() > qxlist.Sum())
                {
                    #region 气郁质体质
                    g1.Text = "1．得分 " + qylist.Sum();
                    if (qylist.Sum() >= 11)
                    {
                        g2.Checked = true;
                        JingYong("7");
                    }
                    else if (qylist.Sum() >= 9 && qylist.Sum() <= 10)
                    {
                        g3.Checked = true;
                        JingYong("7");
                    }
                    #endregion
                }
                else if (tylist.Sum() > hplist.Sum() && tylist.Sum() > qylist.Sum() && tylist.Sum() > xylist.Sum() && tylist.Sum() > srlist.Sum() && tylist.Sum() > tslist.Sum() && tylist.Sum() > yixlist.Sum() && tylist.Sum() > yxlist.Sum() && tylist.Sum() > qxlist.Sum())
                {
                    #region 特禀质体质
                    h1.Text = "1．得分 " + tylist.Sum();
                    if (tylist.Sum() >= 11)
                    {
                        h2.Checked = true;
                        JingYong("8");
                    }
                    else if (tylist.Sum() >= 9 && tylist.Sum() <= 10)
                    {
                        h3.Checked = true;
                        JingYong("8");
                    }
                    #endregion
                }
                else if (hplist.Sum() >= tylist.Sum() && hplist.Sum() >= qylist.Sum() && hplist.Sum() >= xylist.Sum() && hplist.Sum() >= srlist.Sum() && hplist.Sum() >= tslist.Sum() && hplist.Sum() >= yixlist.Sum() && hplist.Sum() >= yxlist.Sum() && hplist.Sum() >= qxlist.Sum())
                {
                //    #region 平和质体质
                //    i1.Text = "1．得分 " + hplist.Sum();
                //    if (hplist.Sum() >= 17 && qxlist.Sum() <= 8 && yxlist.Sum() <= 8 && yixlist.Sum() <= 8 && tslist.Sum() <= 8 && srlist.Sum() <= 8 && xylist.Sum() <= 8 && qylist.Sum() <= 8 && tylist.Sum() <= 8)
                //    {
                //        i2.Checked = true;
                //        JingYong("9");
                //    }
                //    else if (hplist.Sum() >= 17 && qxlist.Sum() <= 10 && yxlist.Sum() <= 10 && yixlist.Sum() <= 10 && tslist.Sum() <= 10 && srlist.Sum() <= 10 && xylist.Sum() <= 10 && qylist.Sum() <= 10 && tylist.Sum() <= 10)
                //    {
                //        i3.Checked = true;
                //        JingYong("9");
                //    }
                //    #endregion
                //}
                //else
                //{
                    #region 平和质体质
                    i1.Text = "1．得分 " + hplist.Sum();
                    i2.Checked = true;
                    JingYong("9");
                    #endregion
                }
            }
            catch (Exception es)
            {
                MessageBox.Show("请联系管理员！");
            }
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

        private void JingYong(string s)
        {
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                if (ctrl is GroupBox)
                {
                    if (((GroupBox)ctrl).Name != "保健" + s)
                    {
                        ((GroupBox)ctrl).Enabled = false;
                        foreach (Control item in ctrl.Controls)
                        {
                            if (item is CheckBox)
                            {
                                ((CheckBox)item).Checked = false;
                            }
                        }
                    }
                    else if (((GroupBox)ctrl).Name == "保健" + s)
                    {
                        ((GroupBox)ctrl).Enabled = true;
                    }
                }
            }
        }

        private void Clears()
        {
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                if (ctrl is GroupBox)
                {
                    foreach (Control item in ctrl.Controls)
                    {
                        if (item is RadioButton)
                        {
                            ((RadioButton)item).Checked = false;
                        }
                    }
                }
            }
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
            //vs.Add("气虚质体质", qxlist.Sum());
            //vs.Add("阳虚质体质", yxlist.Sum());
            //vs.Add("阴虚质体质", yixlist.Sum());
            //vs.Add("痰湿质体质", tslist.Sum());
            //vs.Add("湿热质体质", srlist.Sum());
            //vs.Add("血瘀质体质", xylist.Sum());
            //vs.Add("气郁质体质", qylist.Sum());
            //vs.Add("特禀质体质", tylist.Sum());
            //vs.Add("平和质体质", hplist.Sum());

            if (qxlist.Sum() > hplist.Sum() && qxlist.Sum() > tylist.Sum() && qxlist.Sum() > qylist.Sum() && qxlist.Sum() > xylist.Sum() && qxlist.Sum() > srlist.Sum() && qxlist.Sum() > tslist.Sum() && qxlist.Sum() > yixlist.Sum() && qxlist.Sum() > yxlist.Sum())
            {
                vs.Add("气虚质体质", qxlist.Sum());
                vs.Add("阳虚质体质", 0);
                vs.Add("阴虚质体质", 0);
                vs.Add("痰湿质体质", 0);
                vs.Add("湿热质体质", 0);
                vs.Add("血瘀质体质", 0);
                vs.Add("气郁质体质", 0);
                vs.Add("特禀质体质", 0);
                vs.Add("平和质体质", 0);
            }
            else if (yxlist.Sum() > hplist.Sum() && yxlist.Sum() > tylist.Sum() && yxlist.Sum() > qylist.Sum() && yxlist.Sum() > xylist.Sum() && yxlist.Sum() > srlist.Sum() && yxlist.Sum() > tslist.Sum() && yxlist.Sum() > yixlist.Sum() && yxlist.Sum() > qxlist.Sum())
            {
                vs.Add("气虚质体质", 0);
                vs.Add("阳虚质体质", yxlist.Sum());
                vs.Add("阴虚质体质", 0);
                vs.Add("痰湿质体质", 0);
                vs.Add("湿热质体质", 0);
                vs.Add("血瘀质体质", 0);
                vs.Add("气郁质体质", 0);
                vs.Add("特禀质体质", 0);
                vs.Add("平和质体质", 0);
            }
            else if (yixlist.Sum() > hplist.Sum() && yixlist.Sum() > tylist.Sum() && yixlist.Sum() > qylist.Sum() && yixlist.Sum() > xylist.Sum() && yixlist.Sum() > srlist.Sum() && yixlist.Sum() > tslist.Sum() && yixlist.Sum() > yxlist.Sum() && yixlist.Sum() > qxlist.Sum())
            {
                vs.Add("气虚质体质", 0);
                vs.Add("阳虚质体质", 0);
                vs.Add("阴虚质体质", yixlist.Sum());
                vs.Add("痰湿质体质", 0);
                vs.Add("湿热质体质", 0);
                vs.Add("血瘀质体质", 0);
                vs.Add("气郁质体质", 0);
                vs.Add("特禀质体质", 0);
                vs.Add("平和质体质", 0);
            }
            else if (tslist.Sum() > hplist.Sum() && tslist.Sum() > tylist.Sum() && tslist.Sum() > qylist.Sum() && tslist.Sum() > xylist.Sum() && tslist.Sum() > srlist.Sum() && tslist.Sum() > yixlist.Sum() && tslist.Sum() > yxlist.Sum() && tslist.Sum() > qxlist.Sum())
            {
                vs.Add("气虚质体质", 0);
                vs.Add("阳虚质体质", 0);
                vs.Add("阴虚质体质", 0);
                vs.Add("痰湿质体质", tslist.Sum());
                vs.Add("湿热质体质", 0);
                vs.Add("血瘀质体质", 0);
                vs.Add("气郁质体质", 0);
                vs.Add("特禀质体质", 0);
                vs.Add("平和质体质", 0);
            }
            else if (srlist.Sum() > hplist.Sum() && srlist.Sum() > tylist.Sum() && srlist.Sum() > qylist.Sum() && srlist.Sum() > xylist.Sum() && srlist.Sum() > tslist.Sum() && srlist.Sum() > yixlist.Sum() && srlist.Sum() > yxlist.Sum() && srlist.Sum() > qxlist.Sum())
            {
                vs.Add("气虚质体质", 0);
                vs.Add("阳虚质体质", 0);
                vs.Add("阴虚质体质", 0);
                vs.Add("痰湿质体质", 0);
                vs.Add("湿热质体质", srlist.Sum());
                vs.Add("血瘀质体质", 0);
                vs.Add("气郁质体质", 0);
                vs.Add("特禀质体质", 0);
                vs.Add("平和质体质", 0);
            }
            else if (xylist.Sum() > hplist.Sum() && xylist.Sum() > tylist.Sum() && xylist.Sum() > qylist.Sum() && xylist.Sum() > srlist.Sum() && xylist.Sum() > tslist.Sum() && xylist.Sum() > yixlist.Sum() && xylist.Sum() > yxlist.Sum() && xylist.Sum() > qxlist.Sum())
            {
                vs.Add("气虚质体质", 0);
                vs.Add("阳虚质体质", 0);
                vs.Add("阴虚质体质", 0);
                vs.Add("痰湿质体质", 0);
                vs.Add("湿热质体质", 0);
                vs.Add("血瘀质体质", xylist.Sum());
                vs.Add("气郁质体质", 0);
                vs.Add("特禀质体质", 0);
                vs.Add("平和质体质", 0);
            }
            else if (qylist.Sum() > hplist.Sum() && qylist.Sum() > tylist.Sum() && qylist.Sum() > xylist.Sum() && qylist.Sum() > srlist.Sum() && qylist.Sum() > tslist.Sum() && qylist.Sum() > yixlist.Sum() && qylist.Sum() > yxlist.Sum() && qylist.Sum() > qxlist.Sum())
            {
                vs.Add("气虚质体质", 0);
                vs.Add("阳虚质体质", 0);
                vs.Add("阴虚质体质", 0);
                vs.Add("痰湿质体质", 0);
                vs.Add("湿热质体质", 0);
                vs.Add("血瘀质体质", 0);
                vs.Add("气郁质体质", qylist.Sum());
                vs.Add("特禀质体质", 0);
                vs.Add("平和质体质", 0);
            }
            else if (tylist.Sum() > hplist.Sum() && tylist.Sum() > qylist.Sum() && tylist.Sum() > xylist.Sum() && tylist.Sum() > srlist.Sum() && tylist.Sum() > tslist.Sum() && tylist.Sum() > yixlist.Sum() && tylist.Sum() > yxlist.Sum() && tylist.Sum() > qxlist.Sum())
            {
                vs.Add("气虚质体质", 0);
                vs.Add("阳虚质体质", 0);
                vs.Add("阴虚质体质", 0);
                vs.Add("痰湿质体质", 0);
                vs.Add("湿热质体质", 0);
                vs.Add("血瘀质体质", 0);
                vs.Add("气郁质体质", 0);
                vs.Add("特禀质体质", tylist.Sum());
                vs.Add("平和质体质", 0);
            }
            else if (hplist.Sum() > tylist.Sum() && hplist.Sum() > qylist.Sum() && hplist.Sum() > xylist.Sum() && hplist.Sum() > srlist.Sum() && hplist.Sum() > tslist.Sum() && hplist.Sum() > yixlist.Sum() && hplist.Sum() > yxlist.Sum() && hplist.Sum() > qxlist.Sum())
            {
                vs.Add("气虚质体质", 0);
                vs.Add("阳虚质体质", 0);
                vs.Add("阴虚质体质", 0);
                vs.Add("痰湿质体质", 0);
                vs.Add("湿热质体质", 0);
                vs.Add("血瘀质体质", 0);
                vs.Add("气郁质体质", 0);
                vs.Add("特禀质体质", 0);
                vs.Add("平和质体质", hplist.Sum());
            }
            return vs;
        }

        private string BJ()
        {
            string res = string.Empty;
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                if (ctrl is GroupBox)
                {
                    if (((GroupBox)ctrl).Name.IndexOf("保健") >= 0)
                    {

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
                    }
                }
            }
            return res.TrimEnd(',');

        }

        private int Insert()
        {
            string res = GetFen();
            DateTime time = DateTime.Now;
            var tz = TZ();
            string bj = BJ();
            string issql = @"insert into elderly_tcm_record(id,name,aichive_no,id_number,test_date,answer_result,qixuzhi_score,qixuzhi_result,yangxuzhi_score,yangxuzhi_result,yinxuzhi_score,yinxuzhi_result,tanshizhi_score,tanshizhi_result,shirezhi_score,shirezhi_result,xueyuzhi_score,xueyuzhi_result,qiyuzhi_score,qiyuzhi_result,tebingzhi_sorce,tebingzhi_result,pinghezhi_sorce,pinghezhi_result,tcm_guidance,test_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) values(@id,@name,@aichive_no,@id_number,@test_date,@answer_result,@qixuzhi_score,@qixuzhi_result,@yangxuzhi_score,@yangxuzhi_result,@yinxuzhi_score,@yinxuzhi_result,@tanshizhi_score,@tanshizhi_result,@shirezhi_score,@shirezhi_result,@xueyuzhi_score,@xueyuzhi_result,@qiyuzhi_score,@qiyuzhi_result,@tebingzhi_sorce,@tebingzhi_result,@pinghezhi_sorce,@pinghezhi_result,@tcm_guidance,@test_doctor,@create_user,@create_name,@create_org,@create_org_name,@create_time,@upload_status)";
            MySqlParameter[] args = new MySqlParameter[] {
                    new MySqlParameter("@id",Result.GetNewId()),
                    new MySqlParameter("@name", Names),
                    new MySqlParameter("@aichive_no", aichive_no),
                    new MySqlParameter("@id_number", id_number),
                    new MySqlParameter("@test_date", time.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@answer_result", res),
                    new MySqlParameter("@qixuzhi_score", tz["气虚质体质"]),
                    new MySqlParameter("@qixuzhi_result", tz["气虚质体质"]>=11?1:0),
                    new MySqlParameter("@yangxuzhi_score", tz["阳虚质体质"]),
                    new MySqlParameter("@yangxuzhi_result", tz["阳虚质体质"]>=11?1:0),
                    new MySqlParameter("@yinxuzhi_score", tz["阴虚质体质"]),
                    new MySqlParameter("@yinxuzhi_result", tz["阴虚质体质"]>=11?1:0),
                    new MySqlParameter("@tanshizhi_score", tz["痰湿质体质"]),
                    new MySqlParameter("@tanshizhi_result", tz["痰湿质体质"]>=11?1:0),
                    new MySqlParameter("@shirezhi_score", tz["湿热质体质"]),
                    new MySqlParameter("@shirezhi_result", tz["湿热质体质"]>=11?1:0),
                    new MySqlParameter("@xueyuzhi_score", tz["血瘀质体质"]),
                    new MySqlParameter("@xueyuzhi_result",tz["血瘀质体质"]>=11?1:0),
                    new MySqlParameter("@qiyuzhi_score", tz["气郁质体质"]),
                    new MySqlParameter("@qiyuzhi_result", tz["气郁质体质"]>=11?1:0),
                    new MySqlParameter("@tebingzhi_sorce", tz["特禀质体质"]),
                    new MySqlParameter("@tebingzhi_result",tz["特禀质体质"]>=11?1:0),
                    new MySqlParameter("@pinghezhi_sorce", tz["平和质体质"]),
                    //new MySqlParameter("@pinghezhi_result", (tz["平和质体质"]>=17&&tz["气虚质体质"]<=8&&tz["阳虚质体质"]<=8&&tz["阴虚质体质"]<=8&&tz["痰湿质体质"]<=8&&tz["湿热质体质"]<=8&&tz["血瘀质体质"]<=8&&tz["气郁质体质"]<=8&&tz["特禀质体质"]<=8)?1:0),
                    new MySqlParameter("@pinghezhi_result", (tz["平和质体质"]>=1)?1:0),
                    new MySqlParameter("@tcm_guidance", bj),
                    new MySqlParameter("@test_doctor", YS),
                    new MySqlParameter("@create_user", frmLogin.userCode),
                    new MySqlParameter("@create_name", frmLogin.name),
                    new MySqlParameter("@create_org", frmLogin.organCode),
                    new MySqlParameter("@create_org_name", frmLogin.organName),
                    new MySqlParameter("@create_time", time),
                    new MySqlParameter("@upload_status","0")
                    };
            return DbHelperMySQL.ExecuteSql(issql, args);
        }

        private int Update()
        {
            string res = GetFen();
            DateTime time = DateTime.Now;
            var tz = TZ();
            string bj = BJ();
            string issql = @"update elderly_tcm_record set test_date=@test_date,answer_result=@answer_result,qixuzhi_score=@qixuzhi_score,qixuzhi_result=@qixuzhi_result,yangxuzhi_score=@yangxuzhi_score,yangxuzhi_result=@yangxuzhi_result,yinxuzhi_score=@yinxuzhi_score,yinxuzhi_result=@yinxuzhi_result,tanshizhi_score=@tanshizhi_score,tanshizhi_result=@tanshizhi_result,shirezhi_score=@shirezhi_score,shirezhi_result=@shirezhi_result,xueyuzhi_score=@xueyuzhi_score,xueyuzhi_result=@xueyuzhi_result,qiyuzhi_score=@qiyuzhi_score,qiyuzhi_result=@qiyuzhi_result,tebingzhi_sorce=@tebingzhi_sorce,tebingzhi_result=@tebingzhi_result,pinghezhi_sorce=@pinghezhi_sorce,pinghezhi_result=@pinghezhi_result,tcm_guidance=@tcm_guidance,update_user=@update_user,update_name=@update_name,update_time=@update_time where name=@name and aichive_no=@aichive_no and id_number=@id_number";
            MySqlParameter[] args = new MySqlParameter[] {
                    new MySqlParameter("@name", Names),
                    new MySqlParameter("@aichive_no", aichive_no),
                    new MySqlParameter("@id_number", id_number),
                    new MySqlParameter("@test_date", time.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@answer_result", res),
                    new MySqlParameter("@qixuzhi_score", tz["气虚质体质"]),
                    new MySqlParameter("@qixuzhi_result", tz["气虚质体质"]>=11?1:0),
                    new MySqlParameter("@yangxuzhi_score", tz["阳虚质体质"]),
                    new MySqlParameter("@yangxuzhi_result", tz["阳虚质体质"]>=11?1:0),
                    new MySqlParameter("@yinxuzhi_score", tz["阴虚质体质"]),
                    new MySqlParameter("@yinxuzhi_result", tz["阴虚质体质"]>=11?1:0),
                    new MySqlParameter("@tanshizhi_score", tz["痰湿质体质"]),
                    new MySqlParameter("@tanshizhi_result", tz["痰湿质体质"]>=11?1:0),
                    new MySqlParameter("@shirezhi_score", tz["湿热质体质"]),
                    new MySqlParameter("@shirezhi_result", tz["湿热质体质"]>=11?1:0),
                    new MySqlParameter("@xueyuzhi_score", tz["血瘀质体质"]),
                    new MySqlParameter("@xueyuzhi_result",tz["血瘀质体质"]>=11?1:0),
                    new MySqlParameter("@qiyuzhi_score", tz["气郁质体质"]),
                    new MySqlParameter("@qiyuzhi_result", tz["气郁质体质"]>=11?1:0),
                    new MySqlParameter("@tebingzhi_sorce", tz["特禀质体质"]),
                    new MySqlParameter("@tebingzhi_result",tz["特禀质体质"]>=11?1:0),
                    new MySqlParameter("@pinghezhi_sorce", tz["平和质体质"]),
                    //new MySqlParameter("@pinghezhi_result", (tz["平和质体质"]>=17&&tz["气虚质体质"]<=8&&tz["阳虚质体质"]<=8&&tz["阴虚质体质"]<=8&&tz["痰湿质体质"]<=8&&tz["湿热质体质"]<=8&&tz["血瘀质体质"]<=8&&tz["气郁质体质"]<=8&&tz["特禀质体质"]<=8)?1:0),
                    new MySqlParameter("@pinghezhi_result", (tz["平和质体质"]>=1)?1:0),
                    new MySqlParameter("@tcm_guidance", bj),
                    new MySqlParameter("@update_user", frmLogin.userCode),
                    new MySqlParameter("@update_name", frmLogin.name),
                    new MySqlParameter("@update_time", time)
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

        private void GetData()
        {
            string sql = $@"select * from elderly_tcm_record where id_number='{id_number}' order by create_time desc LIMIT 1";
            DataSet jb = DbHelperMySQL.Query(sql);
            if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
            {
                DataTable da = jb.Tables[0];
                for (int j = 0; j < da.Rows.Count; j++)
                {
                    string[] zz = da.Rows[j]["answer_result"].ToString().Split('|');
                    List<string> vs = new List<string>();
                    for (int i = 0; i < zz.Length; i++)
                    {
                        vs.Add(zz[i].Replace(":", "."));
                    }
                    foreach (Control ctrl in tableLayoutPanel1.Controls)
                    {
                        if (ctrl is GroupBox)
                        {
                            foreach (Control item in ctrl.Controls)
                            {
                                if (item is RadioButton)
                                {
                                    if (!string.IsNullOrWhiteSpace(vs.Where(m => m == ((RadioButton)item).Tag.ToString()).SingleOrDefault()))
                                    {
                                        ((RadioButton)item).Checked = true;
                                    }
                                }
                            }
                        }
                    }
                    string[] tcm = da.Rows[j]["tcm_guidance"].ToString().Split(',');
                    List<string> vstcm = new List<string>();
                    for (int i = 0; i < tcm.Length; i++)
                    {
                        vstcm.Add(tcm[i]);
                    }
                    if (da.Rows[j]["qixuzhi_result"].ToString() == "1")
                    {
                        a1.Text = "1．得分 " + da.Rows[j]["qixuzhi_score"].ToString();
                        a2.Checked = true;
                        JingYong("1");
                        foreach (Control ctrl in 保健1.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                    else if (da.Rows[j]["yangxuzhi_result"].ToString() == "1")
                    {
                        b1.Text = "1．得分 " + da.Rows[j]["yangxuzhi_score"].ToString();
                        b2.Checked = true;
                        JingYong("2");
                        foreach (Control ctrl in 保健2.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                    else if (da.Rows[j]["yinxuzhi_result"].ToString() == "1")
                    {
                        c1.Text = "1．得分 " + da.Rows[j]["yinxuzhi_score"].ToString();
                        c2.Checked = true;
                        JingYong("3");
                        foreach (Control ctrl in 保健3.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                    else if (da.Rows[j]["tanshizhi_result"].ToString() == "1")
                    {
                        d1.Text = "1．得分 " + da.Rows[j]["tanshizhi_score"].ToString();
                        d2.Checked = true;
                        JingYong("4");
                        foreach (Control ctrl in 保健4.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                    else if (da.Rows[j]["shirezhi_result"].ToString() == "1")
                    {
                        e1.Text = "1．得分 " + da.Rows[j]["shirezhi_score"].ToString();
                        e2.Checked = true;
                        JingYong("5");
                        foreach (Control ctrl in 保健5.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                    else if (da.Rows[j]["xueyuzhi_result"].ToString() == "1")
                    {
                        f1.Text = "1．得分 " + da.Rows[j]["xueyuzhi_score"].ToString();
                        f2.Checked = true;
                        JingYong("6");
                        foreach (Control ctrl in 保健6.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                    else if (da.Rows[j]["qiyuzhi_result"].ToString() == "1")
                    {
                        g1.Text = "1．得分 " + da.Rows[j]["qiyuzhi_score"].ToString();
                        g2.Checked = true;
                        JingYong("7");
                        foreach (Control ctrl in 保健7.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                    else if (da.Rows[j]["tebingzhi_result"].ToString() == "1")
                    {
                        h1.Text = "1．得分 " + da.Rows[j]["tebingzhi_score"].ToString();
                        h2.Checked = true;
                        JingYong("8");
                        foreach (Control ctrl in 保健8.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                    else if (da.Rows[j]["pinghezhi_result"].ToString() == "1")
                    {
                        i1.Text = "1．得分 " + da.Rows[j]["pinghezhi_sorce"].ToString();
                        i2.Checked = true;
                        JingYong("9");
                        foreach (Control ctrl in 保健9.Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                string tag = ((CheckBox)ctrl).Tag.ToString();
                                if (vstcm.Contains(tag))
                                {
                                    ((CheckBox)ctrl).Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 判断是否有修改数据
        /// </summary>
        /// <returns></returns>
        private bool GetUpdate()
        {
            DataSet data = DbHelperMySQL.Query($@"select * from elderly_tcm_record where id_number='{id_number}'");
            if (data != null && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void addtcmHealthServices_Load(object sender, EventArgs e)
        {
            healthCheckupDao hcd = new healthCheckupDao();
            DataTable dt = hcd.queryhealthCheckupByid(id_number);
            if (dt!=null&&dt.Rows.Count>0) {
               string baseWaist= dt.Rows[0]["base_waist"].ToString();
               string baseBmi = dt.Rows[0]["base_bmi"].ToString();
                if (baseBmi != null && !"".Equals(baseBmi))
                {
                    double baseBmidouble = Convert.ToDouble(baseBmi);
                    if (baseBmidouble < 24)
                    {
                        this.radioButton45.Checked = true;
                    }
                    else if (baseBmidouble>=24&& baseBmidouble <25)
                    {
                        this.radioButton44.Checked = true;
                    }
                    else if (baseBmidouble >= 25 && baseBmidouble < 26)
                    {
                        this.radioButton43.Checked = true;
                    }
                    else if (baseBmidouble >= 26 && baseBmidouble <= 27)
                    {
                        this.radioButton42.Checked = true;
                    }
                    else if (baseBmidouble >= 28)
                    {
                        this.radioButton41.Checked = true;
                    }
                }
                if (baseWaist != null && !"".Equals(baseWaist))
                {
                    double baseWaistdouble = Convert.ToDouble(baseWaist);
                    if (baseWaistdouble < 80)
                    {
                        this.radioButton140.Checked = true;
                    } else if (baseWaistdouble >=80 && baseWaistdouble <= 85)
                    {
                        this.radioButton139.Checked = true;
                    }
                    else if (baseWaistdouble >= 86 && baseWaistdouble <= 90)
                    {
                        this.radioButton138.Checked = true;
                    }
                    else if (baseWaistdouble >= 91 && baseWaistdouble <= 105)
                    {
                        this.radioButton137.Checked = true;
                    }
                    else if (baseWaistdouble > 105)
                    {
                        this.radioButton136.Checked = true;
                    }
                }
            }
        }
    }
}
