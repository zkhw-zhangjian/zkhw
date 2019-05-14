using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.PublicHealthView
{
    public partial class deleteToChildHealthServices : Form
    {
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
        public deleteToChildHealthServices(string names, string aichive_nos, string id_numbers)
        {
            Names = names;
            aichive_no = aichive_nos;
            id_number = id_numbers;
            InitializeComponent();
        }

        private void 确定_Click(object sender, EventArgs e)
        {
            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {
                List<string> sql = new List<string>();
                foreach (Control item in 月份.Controls)
                {
                    if (item is CheckBox)
                    {
                        if (((CheckBox)item).Checked)
                        {
                            sql.Add($"delete from children_health_record where name='{Names}' and archive_no='{aichive_no}' and id_number='{id_number}' and age='{((CheckBox)item).Tag.ToString()}';");
                        }
                    }
                }
                if (DbHelperMySQL.ExecuteSqlTran(sql) > 0)
                {
                    MessageBox.Show("删除成功！");
                }
                else
                {
                    MessageBox.Show("删除失败！");
                };
            }
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
