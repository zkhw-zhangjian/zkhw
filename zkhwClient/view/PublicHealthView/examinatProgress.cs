/*
 * 
 * 如果村代码为空那么就默认最后一个村的机构，否则按照选择的村的机构选择
 * 
 */
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;  
using System.Text;
using System.Threading;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;
using zkhwClient.view.updateTjResult;

namespace zkhwClient.view.PublicHealthView
{
   
    public partial class examinatProgress : Form 
    { 
        public string time1 = null; 
        public string time2 = null; 
        DataTable dt = null; 
        areaConfigDao areadao = new areaConfigDao(); 
        grjdDao grjddao = new grjdDao(); 
        jkInfoDao jkdao = new jkInfoDao(); 
        string xzcode = null; 
        string qxcode = null; 
        string shicode = null; 
        string shengcode = null; 
        string xcuncode = null; 
        string jmxx = null; 
        string str = Application.StartupPath;//项目路径 
        DataTable dttv = null;
        bool isfirst = true;
        public examinatProgress() 
        { 
            InitializeComponent(); 
        }

        private void examinatProgress_Load(object sender, EventArgs e) 
        {
            isfirst = true;
            //让默认的日期时间减一天 
            this.dateTimePicker1.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            this.button1.BackgroundImage = System.Drawing.Image.FromFile(@str + "/images/check.png");
            this.btnDel.BackgroundImage = System.Drawing.Image.FromFile(@str + "/images/delete.png");

            Common.SetComboBoxInfo(comboBox1, areadao.shengInfo());

            dttv = grjddao.checkThresholdValues(Common._deviceModel,"");//获取阈值信息
            registrationRecordCheck();//体检人数统计
        } 
        public void queryExaminatProgress() 
        { 
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            jmxx = this.textBox1.Text;
            if (this.comboBox5.Text == "" || this.comboBox5.Text == "--请选择--" || comboBox5.SelectedValue==null)
           {
                xcuncode = null;
            }
            string ytj = "1";
            if (time1 != null && !"".Equals(time1) && time2 != null && !"".Equals(time2))
            { 
                dt = jkdao.querytjjd(time1, time2, xcuncode, jmxx);
            } 
            else { this.dataGridView1.DataSource = null; MessageBox.Show("时间段不能为空!"); return; };
             if (dt != null && dt.Rows.Count > 0)
            {
                if (this.radioButton1.Checked)
                {
                    DataRow[] dr = dt.Select("BChao>='" + ytj + "' and XinDian>='" + ytj + "' and XueChangGui>='" + ytj + "' and NiaoChangGui>='" + ytj + "' and Shengaotizhong>='" + ytj + "' and XueYa>='" + ytj + "' and ShengHua>='" + ytj + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        for (int i = dr.Length - 1; i >= 0; i--)
                        {
                            dt.Rows.Remove(dr[i]);
                        }
                        dt.AcceptChanges();
                    }
                }
                else if (this.radioButton2.Checked)
                {
                    DataRow[] dr = dt.Select("BChao>='" + ytj + "' and XinDian>='" + ytj + "' and XueChangGui>='" + ytj + "' and NiaoChangGui>='" + ytj + "' and Shengaotizhong>='" + ytj + "' and XueYa>='" + ytj + "' and ShengHua>='" + ytj + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        DataTable tmp = dr[0].Table.Clone();  // 复制DataRow的表结构  
                        foreach (DataRow row in dr)
                        {
                            tmp.Rows.Add(row.ItemArray);  // 将DataRow添加到DataTable中  
                        }
                        dt = tmp;
                    }
                    else
                    {
                        this.dataGridView1.DataSource = null;
                        return;
                    }
                }
                if (dt.Rows.Count < 1) { this.dataGridView1.DataSource = null; MessageBox.Show("未查询出数据!"); return; }
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[0].HeaderCell.Value = "体检时间";
                this.dataGridView1.Columns[1].HeaderCell.Value = "姓名";
                this.dataGridView1.Columns[2].HeaderCell.Value = "档案编号";
                this.dataGridView1.Columns[3].HeaderCell.Value = "身份证号";
                this.dataGridView1.Columns[4].HeaderCell.Value = "条码号";
                this.dataGridView1.Columns[5].HeaderCell.Value = "B超";
                this.dataGridView1.Columns[6].HeaderCell.Value = "心电图";
                this.dataGridView1.Columns[7].HeaderCell.Value = "生化";
                this.dataGridView1.Columns[8].HeaderCell.Value = "血常规";
                this.dataGridView1.Columns[9].HeaderCell.Value = "尿常规";
                this.dataGridView1.Columns[10].HeaderCell.Value = "血压";
                this.dataGridView1.Columns[11].HeaderCell.Value = "身高体重";
                this.dataGridView1.Columns[12].HeaderCell.Value = "健康体检表";
                this.dataGridView1.Columns[13].HeaderCell.Value = "老年人生活自理能力评估";
                this.dataGridView1.Columns[14].HeaderCell.Value = "老年人中医体质辨识";
                this.dataGridView1.Columns[15].HeaderCell.Value = "年龄";
                this.dataGridView1.Columns[0].Width = 120;
                this.dataGridView1.Columns[1].Width = 150;
                this.dataGridView1.Columns[2].Width = 190;
                this.dataGridView1.Columns[3].Width = 190;
                this.dataGridView1.Columns[4].Width = 150;
                this.dataGridView1.Columns[5].Width = 125;
                this.dataGridView1.Columns[6].Width = 125;
                this.dataGridView1.Columns[7].Width = 125;
                this.dataGridView1.Columns[8].Width = 125;
                this.dataGridView1.Columns[9].Width = 125;
                this.dataGridView1.Columns[10].Width = 125;
                this.dataGridView1.Columns[15].Visible = false;
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                int rows = this.dataGridView1.Rows.Count - 1 <= 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                for (int x = 0; x <= rows; x++)
                {
                    this.dataGridView1.Rows[x].HeaderCell.Value = String.Format("{0}", x + 1);
                    if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "--";
                    }
                    if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "已完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "2")
                    {
                       this.dataGridView1.Rows[x].Cells[6].Value = "已完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "已完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "--";
                    }
                    if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "--";
                    }
                    if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "--";
                    }
                    if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "--";
                    }
                    if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "--";
                    }
                    if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "--";
                    }
                    if (this.dataGridView1.Rows[x].Cells[12].Value.ToString() == "1")
                    {
                        dataGridView1.Rows[x].Cells[12].Value = "已完成";
                        dataGridView1.Rows[x].Cells[12].Style.ForeColor = Color.Green;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[12].Value = "--";
                        dataGridView1.Rows[x].Cells[12].Style.ForeColor = Color.Red;
                    }
                    double age = 0;
                    string tmp = this.dataGridView1.Rows[x].Cells[15].Value.ToString();
                    double.TryParse(tmp, out age); 
                    if (age < 65)
                    { 
                        dataGridView1.Rows[x].Cells[13].Value = "年龄不符";
                        dataGridView1.Rows[x].Cells[13].Style.ForeColor = Color.Green;
                        dataGridView1.Rows[x].Cells[14].Value = "年龄不符";
                        dataGridView1.Rows[x].Cells[14].Style.ForeColor = Color.Green;
                    }
                    else
                    {
                        if (this.dataGridView1.Rows[x].Cells[13].Value.ToString() == "1") 
                        {
                            dataGridView1.Rows[x].Cells[13].Value = "已完成";
                            dataGridView1.Rows[x].Cells[13].Style.ForeColor = Color.Green; 
                        }
                        else
                        { 
                            this.dataGridView1.Rows[x].Cells[13].Value = "--";
                            dataGridView1.Rows[x].Cells[13].Style.ForeColor = Color.Red;
                        }
                        if (this.dataGridView1.Rows[x].Cells[14].Value.ToString() == "1") 
                        { 
                            dataGridView1.Rows[x].Cells[14].Value = "已完成"; 
                            dataGridView1.Rows[x].Cells[14].Style.ForeColor = Color.Green; 
                        } 
                        else 
                        { 
                            this.dataGridView1.Rows[x].Cells[14].Value = "--";
                            dataGridView1.Rows[x].Cells[14].Style.ForeColor = Color.Red;
                        } 
                    } 
                } 
            } 
            else
            {
                this.dataGridView1.DataSource = null;
                if(isfirst==false)
                {
                    MessageBox.Show("未查询出数据！");
                }
                isfirst = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            queryExaminatProgress();
            registrationRecordCheck();//体检人数统计
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
        //体检人数统计  原来参数使用到basicInfoSettings.xcuncode 变更到xcuncode
        public void registrationRecordCheck()
        {
            if (xcuncode == "" || xcuncode == null) xcuncode = basicInfoSettings.xcuncode;
            DataTable dt16num = grjddao.residentNum(xcuncode);
            if (dt16num != null && dt16num.Rows.Count > 0)
            {
                label9.Text = dt16num.Rows[0][0].ToString();//计划体检人数
            }
             
            string createTime = Common.GetCreateTime(basicInfoSettings.createtime);
            DataTable dt19num = grjddao.jkAllNum(xcuncode, createTime);
            if (dt19num != null && dt19num.Rows.Count > 0)
            {
                label11.Text = dt19num.Rows[0][0].ToString();//登记人数

            }
            DataTable dt20num = jkdao.querytjjdTopdf(xcuncode, createTime);
            if (dt20num != null && dt20num.Rows.Count > 0)
            {
                DataRow[] row = dt20num.Select("type='未完成'");
                label13.Text = row.Length.ToString();//未完成人数
            }
            else
            {
                label13.Text = "0";//未完成人数
            }
            string ydjnum = label11.Text;
            string jhtjum = label9.Text;
            if (ydjnum != null && !"".Equals(ydjnum) && jhtjum != null && !"".Equals(jhtjum))
            {
                if (Int32.Parse(jhtjum) - Int32.Parse(ydjnum) > 0)
                {
                    label15.Text = (Int32.Parse(jhtjum) - Int32.Parse(ydjnum)).ToString();
                }
                else
                {
                    label15.Text = "0";//未到人数
                };
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) { return; }
            string str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string str1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string str2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            string str3 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            string str4 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            int columnIndex = e.ColumnIndex;
            if (columnIndex == 5)
            {
                updateBichao ubichao = new updateBichao();
                ubichao.time = str0;
                ubichao.name = str1;
                ubichao.aichive_no = str2;
                ubichao.id_number = str3;
                ubichao.bar_code = str4;
                ubichao.rowIndex = e.RowIndex;
                ubichao.testFunDelegate = DealGridColour;
                ubichao.Show();
            }
            else if (columnIndex == 6)
            {
                updateXindiantu uxindt = new updateXindiantu();
                uxindt.time = str0;
                uxindt.name = str1;
                uxindt.aichive_no = str2;
                uxindt.id_number = str3;
                uxindt.bar_code = str4;
                uxindt.rowIndex = e.RowIndex;
                uxindt.testFunDelegate = DealGridColour;
                uxindt.Show();
            }
            else if (columnIndex == 7)
            {
                updateShenghua ush = new updateShenghua();
                ush.time = str0;
                ush.name = str1;
                ush.aichive_no = str2;
                ush.id_number = str3;
                ush.bar_code = str4;
                //ush.dttv = dttv;
                ush.rowIndex = e.RowIndex;
                ush.testFunDelegate = DealGridColour;
                ush.Show();
            }
            else if (columnIndex == 8)
            {
                updateXuechanggui uxcg = new updateXuechanggui();
                uxcg.time = str0;
                uxcg.name = str1;
                uxcg.aichive_no = str2;
                uxcg.id_number = str3;
                uxcg.bar_code = str4;
                //uxcg.dttv = dttv;
                uxcg.rowIndex = e.RowIndex;
                uxcg.testFunDelegate = DealGridColour;
                uxcg.Show();
            }
            else if (columnIndex == 9)
            {
                updateNiaochanggui uncg = new updateNiaochanggui();
                uncg.time = str0;
                uncg.name = str1;
                uncg.aichive_no = str2;
                uncg.id_number = str3;
                uncg.bar_code = str4;
                uncg.dttv = dttv;
                uncg.rowIndex = e.RowIndex;
                uncg.testFunDelegate = DealGridColour;
                uncg.Show();
            }
            else if (columnIndex == 10)
            {
                updateXueya uxy = new updateXueya();
                uxy.time = str0;
                uxy.name = str1;
                uxy.aichive_no = str2;
                uxy.id_number = str3;
                uxy.bar_code = str4;
                uxy.dttv = dttv;
                uxy.rowIndex = e.RowIndex;
                uxy.testFunDelegate = DealGridColour;
                uxy.Show();
            }
            else if (columnIndex == 11)
            {
                updateShengoaTizhong usgtz = new updateShengoaTizhong();
                usgtz.time = str0;
                usgtz.name = str1;
                usgtz.aichive_no = str2;
                usgtz.id_number = str3;
                usgtz.bar_code = str4;
                usgtz.dttv = dttv;
                usgtz.rowIndex = e.RowIndex;
                usgtz.testFunDelegate = DealGridColour;
                usgtz.Show();
            }
            else if(columnIndex == 12)
            {
                //弹出健康信息表 
                string _id = "";
                string check_date = "";
                string doctor_name = "";
                //这里通过aichive_no、id_number 、bar_code找到对应的 physical_examination_record表的id 
                healthCheckupDao hcd = new healthCheckupDao();
                DataTable dt = hcd.GetExaminationRecordList(str2, str3, str4);
                if(dt !=null || dt.Rows.Count>0)
                {
                    _id = dt.Rows[0]["id"].ToString();
                    check_date = dt.Rows[0]["check_date"].ToString();
                    doctor_name= dt.Rows[0]["doctor_name"].ToString();
                } 
                aUhealthcheckupServices1 auhcs = new aUhealthcheckupServices1();
                auhcs.textBox1.Text = str1;
                auhcs.textBox118.Text = str4;
                auhcs.textBox119.Text = str3;
                auhcs.textBox120.Text = _id;
                auhcs.textBox2.Text = str2;
                if (check_date != "")
                {
                    string TarStr = "yyyy-MM-dd";
                    IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                    auhcs.dateTimePicker1.Value = DateTime.ParseExact(check_date, TarStr, format);
                }
                auhcs.textBox51.Text = doctor_name; 
                auhcs.id = _id; 
                auhcs.Show(); 
            }
        }
        
        public void DealGridColour(int _result, int _colIndex, int _rowIndex)
        {
            if (_rowIndex <= -1) return;
            try
            {
                dataGridView1.Rows[_rowIndex].Cells[_colIndex].Value = "已完成";
                if (_result == 1)
                {
                    dataGridView1.Rows[_rowIndex].Cells[_colIndex].Style.ForeColor = Color.Green;
                }
                else if (_result == 2)
                {
                    dataGridView1.Rows[_rowIndex].Cells[_colIndex].Style.ForeColor = Color.Blue;
                }
                else if (_result == 3)
                {
                    dataGridView1.Rows[_rowIndex].Cells[_colIndex].Style.ForeColor = Color.Red;
                }
            }
            catch
            {
            }
        }
        //生成PDF xcuncode   
        private void label6_Click(object sender, EventArgs e)
        {
            //DataTable dts = jkdao.querytjjdTopdf(basicInfoSettings.xcuncode, basicInfoSettings.createtime);  //2019-6-17改成下面的方式
            if (xcuncode == "" || xcuncode == null) xcuncode = basicInfoSettings.xcuncode;
            time1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd"); 
            time2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd"); 
            DataTable dts = jkdao.querytjjdTopdf(xcuncode, time1, time2);
            if (dts != null && dts.Rows.Count > 0)
            {
                string localFilePath = String.Empty;
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.InitialDirectory = "C://";
                fileDialog.Filter = "All files (*.*)|*.*";
                string xcunName = basicInfoSettings.xcName;
                //设置文件名称：
                if (this.comboBox5.Text.Trim() != "")
                {
                    xcunName = this.comboBox5.Text;
                }
                fileDialog.FileName = DateTime.Now.ToString("yyyyMMdd") + xcunName + "花名册.pdf";
                fileDialog.FilterIndex = 2;
                fileDialog.RestoreDirectory = true;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {   //获得文件路径
                    localFilePath = fileDialog.FileName.ToString();
                    CreateTable(dts.Copy(), localFilePath);
                    //MessageBox.Show("PDF文件生成成功!");
                }
            }
            else
            {
                MessageBox.Show("无历史数据，请先查询历史数据后再生成PDF文件!");
            }
        }

        private void CreateTable(DataTable dts, string path)
        {
            registrationRecordCheck();
            //定义一个Document，并设置页面大小为A4，竖向 
            Document doc = new Document(PageSize.A4);
            try
            {
                string xcunName = basicInfoSettings.xcName;
                if (this.comboBox5.Text.Trim() != "")
                {
                    xcunName = this.comboBox5.Text;
                }

                List<PersonExport> _lst = new List<PersonExport>();
                String timejg = this.comboBox1.Text;
                //写实例 
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                // #endregion //打开document
                doc.Open();
                //载入字体 
                string str = Application.StartupPath;//项目路径   
                BaseFont baseFT = BaseFont.CreateFont(@str + "/fonts/simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fonttitle = new iTextSharp.text.Font(baseFT, 22); //标题字体 Paragraph 
                iTextSharp.text.Font fonttitle2 = new iTextSharp.text.Font(baseFT, 18);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT, 10);//内容字体
                iTextSharp.text.Font fontID = new iTextSharp.text.Font(baseFT, 16);//内容字体
                string titletime = this.dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");//开始时间
                //标题  
                //Paragraph pdftitle = new Paragraph(Convert.ToDateTime(titletime).ToString("yyyy-MM-dd HH:mm:ss") + " - "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), font);
                Paragraph pdftitle = new Paragraph(Convert.ToDateTime(time1).ToString("yyyy-MM-dd HH:mm:ss") + " - " + titletime, font);
                pdftitle.Alignment = 1;
                doc.Add(pdftitle);
                Paragraph null4 = new Paragraph("  ", fontID);
                null4.Leading = 20;
                doc.Add(null4);
                Paragraph pdftitlename = new Paragraph(xcunName + "花名册", fonttitle);
                pdftitlename.Alignment = 1;
                pdftitlename.Leading = 20;
                doc.Add(pdftitlename);
                //标题和内容间的空白行
                Paragraph null5 = new Paragraph("  ", fontID);
                null5.Leading = 20;
                doc.Add(null5);
                Paragraph null6 = new Paragraph("人员统计", fontID);
                null6.Leading = 20;
                doc.Add(null6);
                Paragraph null9 = new Paragraph("  ", fontID);
                null9.Leading = 10;
                doc.Add(null9);
                PdfPTable table1 = new PdfPTable(1);
                //table1.WidthPercentage = 100;//table占宽度百分比 100% 
                //table1.SetWidths(new int[] { 100 });
                //table1.AddCell(new Phrase("应到人数：" + label9.Text, fontID));
                //table1.AddCell(new Phrase("登记人数：" + label11.Text, fontID));//+"    其中男性："+ label16.Text+"    女性："+ label17.Text
                //table1.AddCell(new Phrase("未到人数：" + label15.Text, fontID));
                //table1.AddCell(new Phrase("建档单位：" + basicInfoSettings.organ_name, fontID));
                //table1.AddCell(new Phrase("", fontID));
                //doc.Add(table1);
                #region 导出时 人员统计 信息
                PdfPCell cellTop0 = new PdfPCell(new Phrase("应到人数：" + label9.Text, fontID));
                cellTop0.DisableBorderSide(15);  //去掉边框
                table1.AddCell(cellTop0);
                PdfPCell cellTop1 = new PdfPCell(new Phrase("登记人数：" + label11.Text, fontID));
                cellTop1.DisableBorderSide(15);
                table1.AddCell(cellTop1);
                PdfPCell cellTop2 = new PdfPCell(new Phrase("未到人数：" + label15.Text, fontID));
                cellTop2.DisableBorderSide(15);
                table1.AddCell(cellTop2);
                PdfPCell cellTop3 = new PdfPCell(new Phrase("建档单位：" + basicInfoSettings.organ_name, fontID));
                cellTop3.DisableBorderSide(15);
                table1.AddCell(cellTop3);
                PdfPCell cellTop4 = new PdfPCell(new Phrase("", fontID));
                cellTop4.DisableBorderSide(15);
                table1.AddCell(cellTop4);
                doc.Add(table1);
                #endregion
                Paragraph null7 = new Paragraph("  ", fontID);
                null7.Leading = 20;
                doc.Add(null7);
                Paragraph null3 = new Paragraph("花名册列表", fontID);
                null3.Leading = 20;
                doc.Add(null3);
                Paragraph null8 = new Paragraph("  ", fontID);
                null8.Leading = 10;
                doc.Add(null8);
                //调整列顺序 ，列排序从0开始  
                //dts.Columns["devtime"].SetOrdinal(0);
                //dts.Columns.Remove("编号");
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;//table占宽度百分比 100%
                table.SetWidths(new int[] { 7, 14, 6, 14, 10, 49 });
                string[] columnsnames = { "编号", "姓名", "性别", "出生日期", "状态", "未完成项" };
                PdfPCell cell;
                for (int i = 0; i < columnsnames.Length; i++)
                {
                    cell = new PdfPCell(new Phrase(columnsnames[i], font));
                    table.AddCell(cell);
                }
                PersonExport obj = new PersonExport();
                #region 整理list
                for (int rowNum = 0; rowNum != dts.Rows.Count; rowNum++)
                {
                    obj = new PersonExport();
                    obj.ID = (rowNum + 1).ToString();
                   //table.AddCell(new Phrase((rowNum + 1).ToString(), font));
                    for (int columNum = 0; columNum != dts.Columns.Count; columNum++)
                    {
                        string colstr= dts.Rows[rowNum][columNum].ToString();
                        if (columNum==0)
                        {
                            obj.Name = colstr;
                        }
                        else if (columNum == 1)
                        {
                            obj.Sex = colstr;
                        }
                        else if (columNum == 2)
                        {
                            obj.RiQi = colstr;
                        }
                        else if(columNum == 4)
                        {
                            string tage = dts.Rows[rowNum][3].ToString();
                            if (tage == "") tage = "0";
                            int age = int.Parse(tage);
                            string columlastname = "";
                            #region 特殊处理
                            for (int j = columNum; j < dts.Columns.Count; j++)
                            {
                                string cd = dts.Rows[rowNum][j].ToString();
                                if (cd == "1" || cd == "2" || cd == "3")
                                {
                                }
                                else
                                {
                                    string tmp = "";
                                    switch (j)
                                    {
                                        case 4:
                                            tmp = "B超";
                                            break;
                                        case 5:
                                            tmp = "心电图";
                                            break;
                                        case 6:
                                            tmp = "生化";
                                            break;
                                        case 7:
                                            tmp = "血常规";
                                            break;
                                        case 8:
                                            tmp = "尿常规";
                                            break;
                                        case 9:
                                            tmp = "血压";
                                            break;
                                        case 10:
                                            tmp = "身高体重";
                                            break;
                                        case 11:
                                            tmp = "体检健康表";
                                            break;
                                    }
                                    if (age >= 65)
                                    {
                                        switch (j)
                                        {
                                            case 12:
                                                tmp = "老年人生活自理能力评估";
                                                break;
                                            case 13:
                                                tmp = "老年人中医体质辨识";
                                                break;
                                        }

                                    }
                                    if (tmp != "")
                                    {
                                        if (columlastname == "")
                                        {
                                            columlastname = tmp;
                                        }
                                        else
                                        {
                                            columlastname = columlastname + "、" + tmp;
                                        }
                                    }
                                }
                            }
                            #endregion

                            string sType = "未完成";
                            if (columlastname == "")
                            {
                                sType = "已完成";
                            }
                            obj.ZhuangTai = sType;
                            obj.Memo = columlastname;
                            _lst.Add(obj);
                        } 
                    }
                }
                #endregion
                var q = (from l in _lst orderby l.ZhuangTai ascending select l).ToList();
                for(int i=0;i< q.Count ;i++)
                {
                    table.AddCell(new Phrase(q[i].ID, font));
                    table.AddCell(new Phrase(q[i].Name, font));
                    table.AddCell(new Phrase(q[i].Sex, font));
                    table.AddCell(new Phrase(q[i].RiQi, font));
                    table.AddCell(new Phrase(q[i].ZhuangTai, font));
                    table.AddCell(new Phrase(q[i].Memo, font));
                }
                doc.Add(table);
                //关闭document 
                doc.Close();
                //打开PDF，看效果 
                DialogResult result = MessageBox.Show("是否打开生成的PDF文件！", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Process.Start(path);
                }
            }
            catch (DocumentException de)
            {
                Console.WriteLine(de.Message);
                MessageBox.Show(de.Message + de.StackTrace);
            }
            catch (IOException io)
            {
                Console.WriteLine(io.Message);
                MessageBox.Show(io.Message + io.StackTrace);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        { 
            label6_Click(null, null);
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
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string _idnumber = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string _barcode = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {
                #region 整理记录
                List<string> _lst = new List<string>();
                //string sql = string.Format(" Delete From resident_base_info where archive_no='{0}'", archive_no);
                //_lst.Add(sql);
                string sql = string.Format(" Delete From zkhw_tj_jk where id_number='{0}' and bar_code='{1}'", _idnumber,_barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From zkhw_tj_bgdc where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From physical_examination_record where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From zkhw_tj_bc where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From zkhw_tj_ncg where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From zkhw_tj_sgtz where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From zkhw_tj_sh where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From zkhw_tj_xcg where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From zkhw_tj_xdt where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                sql = string.Format(" Delete From zkhw_tj_xy where id_number='{0}' and bar_code='{1}'", _idnumber, _barcode);
                _lst.Add(sql);
                #endregion
                int ret = DbHelperMySQL.ExecuteSqlTran(_lst);
                if (ret > 0)
                {
                    MessageBox.Show("删除成功！");
                    queryExaminatProgress();
                    registrationRecordCheck();//体检人数统计
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            //根据下面grid中的barcode从云平台获取对应的生化、血球数据
            string _barcode = "";
            for(int i=0;i< dataGridView1.Rows.Count;i++)
            {
                string a = this.dataGridView1.Rows[i].Cells["bar_code"].Value.ToString();
                string b = "'" + a + "'";
                if(_barcode=="")
                {
                    _barcode = b;
                }
                else
                {
                    _barcode = _barcode + "," + b;
                }
            }
            if(_barcode !="")
            {
                DownLoadData(_barcode);
            } 
        }

        private void DownLoadData(string s)
        {
            LoadingHelper.myCaption = "正在拉取数据...";
            LoadingHelper.myLabel = "准备拉取数据...";
            LoadingHelper.ShowLoadingScreen();
            Thread.Sleep(50);
            if (LoadingHelper.loadingForm != null)
            {
                LoadingHelper.loadingForm.mystr = "准备拉取数据...";
            }
            try
            {
                string b = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string c = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                List<string> _idlst = new List<string>();
                List<string> _sqlList = new List<string>();

                #region 生化
                DataTable dt = downloadDataForYunDao.GetShenHuaDataForYun(s, b, c);
                if (dt != null)
                {
                    //整理生化数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i] != null)
                        {
                            string timecodeUnique = "";
                            string shid = "";
                            shenghuaBean obj = downloadDataForYunDao.GetShengHuaObj(basicInfoSettings.sh, dt.Rows[i], out timecodeUnique, out shid);
                            if (obj != null)
                            {
                                //这里处理对应的表
                                string sql = downloadDataForYunDao.GetSqlForShenHua(obj, timecodeUnique);
                                _sqlList.Add(sql);
                                //处理对应的表  zkhw_tj_bgdc、physical_examination_record

                                DataTable dt0 = grjddao.checkThresholdValues(obj.deviceModel, "生化");
                                _sqlList.Add(downloadDataForYunDao.GetUpdateBgdcShSql(dt0, obj));
                                _sqlList.Add(downloadDataForYunDao.GetUpdatePEShInfoSql(obj));
                            }
                            _idlst.Add(downloadDataForYunDao.GetUpdateShToYun(shid));
                        }
                    }
                    if(dt.Rows.Count>0)
                    {
                        LoadingHelper.loadingForm.mystr = "已拉取数据 20%";
                    } 
                } 
                #endregion

                #region 血常规
                DataTable dt1 = downloadDataForYunDao.GetXueChangGuiDataForYun(s, b, c);
                if (dt1 != null)
                {
                    //整理血常规数据
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i] != null)
                        {
                            string xcgid = "";
                            xuechangguiBean obj = downloadDataForYunDao.GetXCGObj(basicInfoSettings.xcg, dt1.Rows[i], out xcgid);
                            if (obj != null)
                            {
                                //这里处理对应的表
                                string sql = downloadDataForYunDao.GetSqlForXCG(obj);
                                _sqlList.Add(sql);
                                //处理对应的表  zkhw_tj_bgdc、physical_examination_record
                                DataTable dt0 = grjddao.checkThresholdValues(obj.deviceModel, "血常规");
                                _sqlList.Add(downloadDataForYunDao.GetUpdateBgdcXCGSql(dt0, obj));
                                _sqlList.Add(downloadDataForYunDao.GetUpdatePEXCGInfoSql(obj));
                            }
                            _idlst.Add(downloadDataForYunDao.GetUpdateXCGToYun(xcgid));
                        }
                    }
                    if(dt1.Rows.Count>0)
                    {
                        LoadingHelper.loadingForm.mystr = "已拉取数据40%";
                    } 
                }
                #endregion

                #region 这里插入数据
                if (_sqlList.Count > 0)
                {
                    int ret = DbHelperMySQL.ExecuteSqlTran(_sqlList);
                    if (ret > 0)
                    {
                        LoadingHelper.loadingForm.mystr = "已拉取数据 99%";
                        if (_idlst.Count > 0)
                        {
                            DbHelperMySQL.ExecuteSqlTranYpt(_idlst);
                            LoadingHelper.loadingForm.mystr = "已拉取数据 100%";
                        }
                        if (LoadingHelper.loadingForm != null)
                        {
                            LoadingHelper.CloseForm();
                        }
                        MessageBox.Show("成功！");
                        queryExaminatProgress();
                    }
                    else
                    { 
                        if (LoadingHelper.loadingForm != null)
                        {
                            LoadingHelper.CloseForm();
                        }
                        MessageBox.Show("失败！");
                    }
                }
                else
                {
                    LoadingHelper.loadingForm.mystr = "无拉取数据";
                    if (_idlst.Count > 0)
                    {
                        DbHelperMySQL.ExecuteSqlTranYpt(_idlst);
                    }
                }
                #endregion
                if (LoadingHelper.loadingForm != null)
                {
                    LoadingHelper.CloseForm();
                }
            }
            catch
            {
                if (LoadingHelper.loadingForm != null)
                {
                    LoadingHelper.CloseForm();
                }
            } 
            
        } 
    }
}
