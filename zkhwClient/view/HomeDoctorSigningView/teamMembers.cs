using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.HomeDoctorSigningView
{
    public partial class teamMembers : Form
    {
        public teamMembers()
        {
            InitializeComponent();
            ComboBoxBin();
        }

        private void ComboBoxBin()
        {
            string sql = "select team_no ID,team_name Name from team_info";
            DataSet datas = DbHelperMySQL.Query(sql);
            if (datas != null && datas.Tables.Count > 0)
            {
                List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(datas.Tables[0]);
                Result.Bind(comboBox1, ts, "Name", "ID", "--请选择--");
            }
        }

        private void GroupBoxBin(string fid)
        {

            string sql = $"select id,ZhiWei,ZhiWu,XingMing,LianXiFangShi from zkhw_qy_tdcy where FuID='{fid}' ORDER  BY ZhiWei asc";
            DataSet datas = DbHelperMySQL.Query(sql);
            if (datas != null && datas.Tables.Count > 0)
            {
                #region 设置groupbox控件
                DataTable data = datas.Tables[0];
                int x, y, row = 0;
                Label label = null;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (i % 5 == 0 && i != 0)
                    {
                        row++;
                    }
                    x = 26 + i % 5 * 198;
                    y = 26 + row * 270;
                    GroupBox groubox = new GroupBox();
                    groubox.Text = "";
                    groubox.Size = new Size(182, 258);
                    groubox.Location = new Point(x, y);
                    #region 设置groupbox中控件
                    label = new Label();
                    label.Location = new Point(64, 17);
                    label.Font = new Font("宋体", 15, FontStyle.Bold);
                    label.Text = data.Rows[i]["XingMing"].ToString();
                    groubox.Controls.Add(label);

                    label = new Label();
                    label.Location = new Point(66, 50);
                    label.Text = data.Rows[i]["ZhiWei"].ToString();
                    groubox.Controls.Add(label);

                    label = new Label();
                    label.Location = new Point(21, 182);
                    label.AutoSize = true;
                    label.Text = $"联系方式：{data.Rows[i]["LianXiFangShi"].ToString()}";
                    groubox.Controls.Add(label);

                    label = new Label();
                    label.AutoSize = true;
                    label.Location = new Point(45, 208);
                    label.Text = $"职务：{data.Rows[i]["ZhiWu"].ToString()}";
                    groubox.Controls.Add(label);

                    groubox.Controls.Add(label);
                    #endregion
                    groupBox1.Controls.Add(groubox);
                }

                #endregion
            }
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fid = comboBox1.SelectedValue?.ToString();
            while (groupBox1.Controls.Count != 0)
            {
                groupBox1.Controls.Clear();
                //groupBox1.Dispose();
            }
            if (!string.IsNullOrWhiteSpace(fid) && fid.Length == 36)
            {
                GroupBoxBin(fid);
            }
        }
    }

}
