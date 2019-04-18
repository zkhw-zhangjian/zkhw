using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zkhwClient
{
    public partial class userManage : Form
    {
        service.UserService us = new service.UserService();
        public userManage()
        {
            InitializeComponent();
        }
        //页面加载
        private void userManage_Load(object sender, EventArgs e)
        {

            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/insert.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/update.png");
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/delete.png");
            this.button4.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            queryUser();
        }
        //屏蔽双击
        protected override void DefWndProc(ref Message m)
        {
            byte msg = 0x00A3;
            if (m.Msg == Convert.ToInt32(msg))
            {
                return;
            }
            base.DefWndProc(ref m);
        }
        //查询系统用户
        private void queryUser()
        {   //查询所有用户信息
            DataTable dd = us.listUser();
            dd.Columns.Add("类别", typeof(string));
            foreach (DataRow rows in dd.Rows)
            {
                if (rows[0].ToString() == "admin")
                {
                    rows["类别"] = "管理员";
                }
                else
                {
                    rows["类别"] = "普通用户";
                }
                rows[1] = "******";
            }
            this.dataGridView1.DataSource = dd;
            this.dataGridView1.Columns[0].HeaderCell.Value = "用户名称(登录用)";
            this.dataGridView1.Columns[1].HeaderCell.Value = "密码";
            this.dataGridView1.Columns[2].HeaderCell.Value = "用户名称";
            this.dataGridView1.Columns[3].HeaderCell.Value = "性别";
            this.dataGridView1.Columns[4].HeaderCell.Value = "出生年月";
            this.dataGridView1.Columns[5].HeaderCell.Value = "类别";
            //this.dataGridView1.Columns[1].Width = 110;
            //this.dataGridView1.Columns[4].Width = 130;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

        }
        //增加按钮事件
        private void button1_Click(object sender, EventArgs e)
        {
            addUser au = new addUser();
            if (au.ShowDialog() == DialogResult.OK)
            {
                queryUser();
                MessageBox.Show("添加成功！");
            }
        }

        //修改按钮事件
        private void button2_Click(object sender, EventArgs e)
        {
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string name = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string enable = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            if (name == "user")
            {
                MessageBox.Show("管理员账号不能被修改！");
                return;
            }
            if (name != null && !"".Equals(name))
            {
                updateUser up = new updateUser();
                up.textBox1.Text = name;
                up.textBox2.Text = id;

                if (up.ShowDialog() == DialogResult.OK)
                {
                    queryUser();
                    MessageBox.Show("修改成功！");
                }
            }


        }
        //删除按钮事件
        private void button3_Click(object sender, EventArgs e)
        {
            bool istrue = false;
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string name = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (name == "user")
            {
                MessageBox.Show("管理员账号不能被删除！");
            }
            else
            {
                if (id != null)
                {
                    DialogResult rr = MessageBox.Show("是否要删除当前用户信息？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    int tt = (int)rr;
                    if (tt == 1)
                    {//删除用户       
                        istrue = us.deleteUser(id, name);
                        if (istrue)
                        {
                            queryUser();
                            MessageBox.Show("删除成功！");
                        }
                    }
                }
            }
        }
        //关闭按钮事件
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
