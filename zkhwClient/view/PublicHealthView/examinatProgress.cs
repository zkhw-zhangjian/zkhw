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
        DataTable dt=null;
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
        public examinatProgress()
        {
            InitializeComponent();
        }
        private void examinatProgress_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            this.button1.BackgroundImage = System.Drawing.Image.FromFile(@str + "/images/check.png");

            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 
            registrationRecordCheck();//体检人数统计
        }
        public void queryExaminatProgress()
        {
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            jmxx = this.textBox1.Text;
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
                    DataRow[] dr = dt.Select("BChao='" + ytj + "' and XinDian='" + ytj + "' and XueChangGui='" + ytj + "' and NiaoChangGui='" + ytj + "' and Shengaotizhong='" + ytj + "' and XueYa='" + ytj + "' and ShengHua='" + ytj + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        for (int i = dr.Length - 1; i >= 0; i--)
                        {
                            dt.Rows.Remove(dr[i]);
                        }
                    }
                }
                else if (this.radioButton2.Checked)
                {
                    DataRow[] dr = dt.Select("BChao='" + ytj + "' and XinDian='" + ytj + "' and XueChangGui='" + ytj + "' and NiaoChangGui='" + ytj + "' and Shengaotizhong='" + ytj + "' and XueYa='" + ytj + "' and ShengHua='" + ytj + "'");
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
                if (dt.Rows.Count < 1) { this.dataGridView1.DataSource = null; MessageBox.Show("未查询出数据!");return; }
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
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                int rows = this.dataGridView1.Rows.Count - 1 <= 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                for (int x = 0; x <= rows; x++)
                {
                    this.dataGridView1.Rows[x].HeaderCell.Value = String.Format("{0}", x + 1);

                    if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[5].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[5].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[5].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[6].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[6].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[6].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[7].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[7].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[7].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[8].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[8].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[8].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[9].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[9].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[9].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[10].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[10].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[10].Value = "--";
                    }

                    if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "1")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Green;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "2")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Blue;
                    }
                    else if (this.dataGridView1.Rows[x].Cells[11].Value.ToString() == "3")
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "已经完成";
                        dataGridView1.Rows[x].Cells[11].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[x].Cells[11].Value = "--";
                    }
                }
            }
            else {
                this.dataGridView1.DataSource = null;
                MessageBox.Show("未查询出数据！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            queryExaminatProgress();
            registrationRecordCheck();//体检人数统计
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

        //体检人数统计
        public void registrationRecordCheck()
        {
            DataTable dt16num = grjddao.residentNum(basicInfoSettings.xcuncode);
            if (dt16num != null && dt16num.Rows.Count > 0)
            {
                label9.Text = dt16num.Rows[0][0].ToString();//计划体检人数
            }

            DataTable dt19num = grjddao.jkAllNum(basicInfoSettings.xcuncode, basicInfoSettings.createtime);
            if (dt19num != null && dt19num.Rows.Count > 0)
            {
                //DataRow[] rownan = dt19num.Select("sex='男'");
                //DataRow[] rownv = dt19num.Select("sex='女'");
                //label16.Text = rownan.Length.ToString();
                //label17.Text = rownan.Length.ToString();
                label11.Text = dt19num.Rows[0][0].ToString();//登记人数
            }

            DataTable dt20num = jkdao.querytjjdTopdf(basicInfoSettings.xcuncode, basicInfoSettings.createtime);
            if (dt20num != null && dt20num.Rows.Count > 0)
            {
                DataRow[] row =dt20num.Select("type='未完成'");
                label13.Text = row.Length.ToString();//未完成人数
            }
            else
            {
                label13.Text = "0";//未完成人数
            }
            string ydjnum = label11.Text;
            string jhtjum = label9.Text;
            if (ydjnum!=null&&!"".Equals(ydjnum)&& jhtjum != null && !"".Equals(jhtjum)) {
                if (Int32.Parse(jhtjum) - Int32.Parse(ydjnum) > 0)
                {
                    label15.Text = (Int32.Parse(jhtjum) - Int32.Parse(ydjnum)).ToString();
                }
                else {
                    label15.Text = "0";//未到人数
                };
            }
        }

        
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string str1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string str2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            string str3 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            string str4 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            int columnIndex=e.ColumnIndex;
            if (columnIndex == 5) {
                updateBichao ubichao = new updateBichao();
                ubichao.time = str0;
                ubichao.name = str1;
                ubichao.aichive_no = str2;
                ubichao.id_number = str3;
                ubichao.bar_code = str4;
                ubichao.Show();
            } else if (columnIndex == 6) {
                updateXindiantu uxindt = new updateXindiantu();
                uxindt.time = str0;
                uxindt.name = str1;
                uxindt.aichive_no = str2;
                uxindt.id_number = str3;
                uxindt.bar_code = str4;
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
                usgtz.Show();
            }
        }
        //生成PDF
        private void label6_Click(object sender, EventArgs e)
        {
            DataTable dts = jkdao.querytjjdTopdf(basicInfoSettings.xcuncode, basicInfoSettings.createtime);
            if (dts != null && dts.Rows.Count > 0)
            {
                string localFilePath = String.Empty;
                SaveFileDialog fileDialog = new SaveFileDialog();

                fileDialog.InitialDirectory = "C://";

                fileDialog.Filter = "All files (*.*)|*.*";

                //设置文件名称：
                fileDialog.FileName = DateTime.Now.ToString("yyyyMMdd") + basicInfoSettings.xcName + "花名册.pdf";

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

                //标题 
                Paragraph pdftitle = new Paragraph(basicInfoSettings.createtime + "-"+ DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), font);
                pdftitle.Alignment = 1;
                doc.Add(pdftitle);

                Paragraph null4 = new Paragraph("  ", fontID);
                null4.Leading = 20;
                doc.Add(null4);

                Paragraph pdftitlename = new Paragraph(basicInfoSettings.xcName+"花名册", fonttitle);
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
                table1.WidthPercentage = 100;//table占宽度百分比 100%
                table1.SetWidths(new int[] { 100 });
                table1.AddCell(new Phrase("应到人数：" + label9.Text, fontID));
                table1.AddCell(new Phrase("登记人数：" + label11.Text, fontID));//+"    其中男性："+ label16.Text+"    女性："+ label17.Text
                table1.AddCell(new Phrase("未到人数：" + label15.Text, fontID));
                table1.AddCell(new Phrase("建档单位：" + basicInfoSettings.organ_name, fontID));
                table1.AddCell(new Phrase("", fontID));
                doc.Add(table1);

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

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;//table占宽度百分比 100%
                table.SetWidths(new int[] { 15, 25, 20, 20, 20 });
                string[] columnsnames = { "编号", "姓名", "性别", "出生日期", "状态" };
                PdfPCell cell;

                for (int i = 0; i < columnsnames.Length; i++)
                {
                    cell = new PdfPCell(new Phrase(columnsnames[i], font));
                    table.AddCell(cell);
                }
                for (int rowNum = 0; rowNum != dts.Rows.Count; rowNum++)
                {
                    table.AddCell(new Phrase((rowNum + 1).ToString(), font));
                    for (int columNum = 0; columNum != dts.Columns.Count; columNum++)
                    {
                        table.AddCell(new Phrase(dts.Rows[rowNum][columNum].ToString(), font));
                    }
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
                MessageBox.Show(de.Message+de.StackTrace);
            }
            catch (IOException io)
            {
                Console.WriteLine(io.Message);
                MessageBox.Show(io.Message + io.StackTrace);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            label6_Click(null,null);
        }
    }
}
