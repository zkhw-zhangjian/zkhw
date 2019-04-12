using Aspose.Words;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class examinatReport : Form
    {
        #region 初始化数据
        public examinatReport()
        {
            InitializeComponent();
            BinData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void BinData()
        {
            #region 报告统计数据绑定
            string sql = $@"SELECT count(sex)sun,sex
from zkhw_tj_bgdc where area_duns like '%{basicInfoSettings.xcuncode}%' and createtime>='{basicInfoSettings.createtime}'
GROUP BY sex
";
            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable data = dataSet.Tables[0];
            if (data != null && data.Rows.Count > 0)
            {
                DataRow[] rows = data.Select("sex='女'");
                女.Text = rows[0]["sun"].ToString();
                DataRow[] rowsn = data.Select("sex='男'");
                男.Text = rowsn[0]["sun"].ToString();
                总数.Text = data.Compute("sum(sun)", "true").ToString();
            }
            #endregion

            #region 报告查询 区域数据绑定
            string sql1 = "select code as ID,name as Name from code_area_config where parent_code='-1';";
            DataSet datas = DbHelperMySQL.Query(sql1);
            if (datas != null && datas.Tables.Count > 0)
            {
                List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(datas.Tables[0]);
                Result.Bind(comboBox1, ts, "Name", "ID", "--请选择--");
            }
            #endregion
        }

        private void examinatProgress_Load(object sender, EventArgs e)
        {
            //让默认的日期时间减一天
            this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/check.png");
            this.统计查询.BackgroundImage = Image.FromFile(@str + "/images/check.png");

            pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);
            int count = 0;
            queryExaminatProgress(GetData(pagerControl1.PageIndex, pagerControl1.PageSize, out count));
            pagerControl1.DrawControl(count);
        }
        #endregion

        #region 列表展示
        void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            int count = 0;
            queryExaminatProgress(GetData(pagerControl1.PageIndex, pagerControl1.PageSize, out count));
            pagerControl1.DrawControl(count);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageindex">当前页面</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        private DataTable GetData(int pageindex, int pagesize, out int count)
        {
            pageindex = pageindex != 0 ? pageindex - 1 : pageindex;
            string timesta = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string timeend = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string sheng = comboBox1.SelectedValue?.ToString();
            string shi = comboBox2.SelectedValue?.ToString();
            string xian = comboBox3.SelectedValue?.ToString();
            string cun = comboBox4.SelectedValue?.ToString();
            string zu = comboBox5.SelectedValue?.ToString();
            string juming = textBox1.Text;
            var pairs = new Dictionary<string, string>();
            pairs.Add("timesta", timesta);
            pairs.Add("timeend", timeend);
            pairs.Add("juming", juming);
            pairs.Add("sheng", sheng);
            pairs.Add("xian", xian);
            pairs.Add("cun", cun);
            pairs.Add("zu", zu);
            pairs.Add("shi", shi);
            string sql = $@"select 
DATE_FORMAT(base.create_time,'%Y%m%d') 登记时间,
concat(base.province_name,base.city_name,base.county_name,base.towns_name,base.village_name) 区域,
base.archive_no 编码,
base.name 姓名,
base.sex 性别,
base.id_number 身份证号,
base.is_synchro 是否同步,
bgdc.BaoGaoShengChan 报告生产时间
from resident_base_info base
join 
(select * from (select * from zkhw_tj_bgdc order by createtime desc) as a group by aichive_no order by createtime desc) bgdc
on base.archive_no=bgdc.aichive_no
where base.village_code='{basicInfoSettings.xcuncode}' and base.create_time>='{basicInfoSettings.createtime}'";
            if (pairs != null && pairs.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(pairs["timesta"]) && !string.IsNullOrWhiteSpace(pairs["timeend"]))
                {
                    sql += $" and base.create_time(healthchecktime,'%Y-%m-%d') between '{pairs["timesta"]}' and '{pairs["timeend"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["juming"]))
                {
                    sql += $" or name like '%{pairs["juming"]}%' or bar_code like '%{pairs["juming"]}%' or id_number like '%{pairs["juming"]}%'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["sheng"]))
                {
                    sql += $" and base.province_code='{pairs["sheng"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["shi"]))
                {
                    sql += $" and base.city_code='{pairs["shi"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["xian"]))
                {
                    sql += $" and base.county_code='{pairs["xian"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["cun"]))
                {
                    sql += $" and base.towns_code='{pairs["cun"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["zu"]))
                {
                    sql += $" and base.village_code='{pairs["zu"]}'";
                }
            }
            sql += $@" and id >=(
            select id From zkhw_tj_bgdc Order By id limit {pageindex},1
            ) limit {pagesize}; select found_rows()";
            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable dt = dataSet.Tables[0];
            count = Convert.ToInt32(dataSet.Tables[1].Rows[0][0]);
            return dt;
        }

        //声明静态类变量
        private static DataGridViewCheckBoxColumn checkColumn = null;
        private static DataGridViewButtonColumn buttonColumn = null;
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="data"></param>
        private void queryExaminatProgress(DataTable data)
        {
            if (dataGridView1.DataSource != null)
            {
                DataTable dts = (DataTable)dataGridView1.DataSource;
                dts.Rows.Clear();
                dataGridView1.DataSource = dts;
            }
            else
            {
                dataGridView1.Rows.Clear();
            }

            if (data != null)
            {
                this.dataGridView1.DataSource = data;
                this.dataGridView1.Columns[0].Visible = false;
                if (buttonColumn == null)
                {
                    buttonColumn = new DataGridViewButtonColumn();
                    buttonColumn.Name = "btnModify";
                    buttonColumn.HeaderText = "";
                    buttonColumn.DefaultCellStyle.NullValue = "查看报告";
                    dataGridView1.Columns.Add(buttonColumn);
                }
                checkColumn = new DataGridViewCheckBoxColumn(); //插入第0列 
                checkColumn.HeaderText = "选择";
                checkColumn.Name = "cb_check";
                checkColumn.TrueValue = true;
                checkColumn.FalseValue = false;
                checkColumn.DataPropertyName = "IsChecked";
                dataGridView1.Columns.Insert(0, checkColumn);    //添加的checkbox在第一列
                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                this.dataGridView1.ReadOnly = true;
            }
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView1.RowHeadersWidth - 4,
               e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView1.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        /// <summary>
        /// CheckBox勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //checkbox 勾上
            if ((bool)dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
            {
                this.dataGridView1.Rows[e.RowIndex].Cells[0].Value = false;
            }
            else
            {
                this.dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            queryExaminatProgress(GetData(pagerControl1.PageIndex, pagerControl1.PageSize, out count));
            pagerControl1.DrawControl(count);
        }
        #endregion
        private void 统计查询_Click(object sender, EventArgs e)
        {
            string stan = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            string end = dateTimePicker4.Value.ToString("yyyy-MM-dd");
            string sql = $@"SELECT sex,'64',COUNT(sex) 人数,
            COUNT(CASE
                WHEN(bchao = '2') THEN '0'
            END
            ) as B超异常,
            COUNT(CASE
                WHEN(XinDian = '2') THEN
                    '0'
            END
            ) as 心电异常,
            COUNT(CASE
                WHEN(NiaoChangGui = '2') THEN
                    '0'
            END
            ) as 尿常规异常,
            COUNT(CASE
                WHEN(XueYa = '2') THEN
                    '0'
            END
            ) as 血压异常,
            COUNT(CASE
                WHEN(ShengHua = '2') THEN
                    '0'
            END
            ) as 生化异常
            from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and createtime>='{basicInfoSettings.createtime}' and birthday >= '0' and birthday<= '64' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
            GROUP BY sex;

            SELECT sex,'70',COUNT(sex) 人数,
            COUNT(CASE
                WHEN(bchao = '2') THEN
                    '0'
            END
            ) as B超异常,
            COUNT(CASE
                WHEN(XinDian = '2') THEN
                    '0'
            END
            ) as 心电异常,
            COUNT(CASE
                WHEN(NiaoChangGui = '2') THEN
                    '0'
            END
            ) as 尿常规异常,
            COUNT(CASE
                WHEN(XueYa = '2') THEN
                    '0'
            END
            ) as 血压异常,
            COUNT(CASE
                WHEN(ShengHua = '2') THEN
                    '0'
            END
            ) as 生化异常
            from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and createtime>='{basicInfoSettings.createtime}' and birthday >= '65' and birthday<= '70' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
            GROUP BY sex;

            SELECT sex,'75',COUNT(sex) 人数,
            COUNT(CASE
                WHEN(bchao = '2') THEN
                    '0'
            END
            ) as B超异常,
            COUNT(CASE
                WHEN(XinDian = '2') THEN
                    '0'
            END
            ) as 心电异常,
            COUNT(CASE
                WHEN(NiaoChangGui = '2') THEN
                    '0'
            END
            ) as 尿常规异常,
            COUNT(CASE
                WHEN(XueYa = '2') THEN
                    '0'
            END
            ) as 血压异常,
            COUNT(CASE
                WHEN(ShengHua = '2') THEN
                    '0'
            END
            ) as 生化异常
            from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and createtime>='{basicInfoSettings.createtime}' and birthday >= '70' and birthday<= '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
            GROUP BY sex;

            SELECT sex,'76',COUNT(sex) 人数,
            COUNT(CASE
                WHEN(bchao = '2') THEN
                    '0'
            END
            ) as B超异常,
            COUNT(CASE
                WHEN(XinDian = '2') THEN
                    '0'
            END
            ) as 心电异常,
            COUNT(CASE
                WHEN(NiaoChangGui = '2') THEN
                    '0'
            END
            ) as 尿常规异常,
            COUNT(CASE
                WHEN(XueYa = '2') THEN
                    '0'
            END
            ) as 血压异常,
            COUNT(CASE
                WHEN(ShengHua = '2') THEN
                    '0'
            END
            ) as 生化异常
            from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and createtime>='{basicInfoSettings.createtime}' and birthday >= '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
            GROUP BY sex";
            DataSet dataSet = DbHelperMySQL.Query(sql);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    DataTable data = dataSet.Tables[i];
                    if (data != null && data.Rows.Count > 0)
                    {
                        switch (data.Rows[0][1].ToString())
                        {
                            case "64":
                                #region 064
                                DataRow[] rows = data.Select("sex='女'");
                                if (rows != null && rows.Length > 0)
                                {
                                    女064.Text = rows[0]["人数"].ToString();
                                }
                                DataRow[] rowss = data.Select("sex='男'");
                                if (rowss != null && rowss.Length > 0)
                                {
                                    男064.Text = rowss[0]["人数"].ToString();
                                }
                                B超064.Text = data.Compute("sum(B超异常)", "true").ToString();
                                心电064.Text = data.Compute("sum(心电异常)", "true").ToString();
                                尿常规064.Text = data.Compute("sum(尿常规异常)", "true").ToString();
                                血压064.Text = data.Compute("sum(血压异常)", "true").ToString();
                                生化064.Text = data.Compute("sum(生化异常)", "true").ToString();
                                #endregion
                                break;
                            case "70":
                                #region 6570
                                DataRow[] nv6570 = data.Select("sex='女'");
                                if (nv6570 != null && nv6570.Length > 0)
                                {
                                    女6570.Text = nv6570[0]["人数"].ToString();
                                }
                                DataRow[] nan6570 = data.Select("sex='男'");
                                if (nan6570 != null && nan6570.Length > 0)
                                {
                                    男6570.Text = nan6570[0]["人数"].ToString();
                                }
                                B超6570.Text = data.Compute("sum(B超异常)", "true").ToString();
                                心电6570.Text = data.Compute("sum(心电异常)", "true").ToString();
                                尿常规6570.Text = data.Compute("sum(尿常规异常)", "true").ToString();
                                血压6570.Text = data.Compute("sum(血压异常)", "true").ToString();
                                生化6570.Text = data.Compute("sum(生化异常)", "true").ToString();
                                #endregion
                                break;
                            case "75":
                                #region 7075   
                                DataRow[] nv7075 = data.Select("sex='女'");
                                if (nv7075 != null && nv7075.Length > 0)
                                {
                                    女7075.Text = nv7075[0]["人数"].ToString();
                                }
                                DataRow[] nan7075 = data.Select("sex='男'");
                                if (nan7075 != null && nan7075.Length > 0)
                                {
                                    男7075.Text = nan7075[0]["人数"].ToString();
                                }
                                B超7075.Text = data.Compute("sum(B超异常)", "true").ToString();
                                心电7075.Text = data.Compute("sum(心电异常)", "true").ToString();
                                尿常规7075.Text = data.Compute("sum(尿常规异常)", "true").ToString();
                                血压7075.Text = data.Compute("sum(血压异常)", "true").ToString();
                                生化7075.Text = data.Compute("sum(生化异常)", "true").ToString();
                                #endregion
                                break;
                            case "76":
                                #region 75
                                DataRow[] nv75 = data.Select("sex='女'");
                                if (nv75 != null && nv75.Length > 0)
                                {
                                    女75.Text = nv75[0]["人数"].ToString();
                                }
                                DataRow[] nan75 = data.Select("sex='男'");
                                if (nan75 != null && nan75.Length > 0)
                                {
                                    男75.Text = nan75[0]["人数"].ToString();
                                }
                                B超75.Text = data.Compute("sum(B超异常)", "true").ToString();
                                心电75.Text = data.Compute("sum(心电异常)", "true").ToString();
                                尿常规75.Text = data.Compute("sum(尿常规异常)", "true").ToString();
                                血压75.Text = data.Compute("sum(血压异常)", "true").ToString();
                                生化75.Text = data.Compute("sum(生化异常)", "true").ToString();
                                #endregion
                                break;
                        }
                    }
                }
            }
        }

        #region 下拉框绑定
        /// <summary>
        /// 绑定下拉选项
        /// </summary>
        /// <param name="combo">获取值</param>
        /// <param name="box">绑定值</param>
        private void comboBoxBin(ComboBox combo, ComboBox box)
        {
            string id = combo.SelectedValue?.ToString();
            if (!string.IsNullOrWhiteSpace(id))
            {
                string sql1 = $"select code as ID,name as Name from code_area_config where parent_code='{id}'";
                DataSet datas = DbHelperMySQL.Query(sql1);
                if (datas != null && datas.Tables.Count > 0)
                {
                    List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(datas.Tables[0]);
                    Result.Bind(box, ts, "Name", "ID", "--请选择--");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBin(comboBox1, comboBox2);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBin(comboBox2, comboBox3);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBin(comboBox3, comboBox4);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBin(comboBox4, comboBox5);
        }
        #endregion

        #region 报告导出
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ge = new DataTable();
                List<string> list = new List<string>();
                List<string> ide = new List<string>();
                DataSet dataSet = new DataSet();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                    {
                        string id = dataGridView1["编码", i].Value.ToString();
                        ide.Add(id);
                    }
                }

                string sql = string.Empty;
                sql = $@"select * from resident_base_info base where base.archive_no in('{string.Join(",", ide)}')";
                DataSet datas = DbHelperMySQL.Query(sql);
                if (datas != null && datas.Tables.Count > 0)
                {
                    ge = datas.Tables[0].Copy();
                    ge.TableName = "个人";
                    dataSet.Tables.Add(ge);
                }
                foreach (Control ctrl in groupBox4.Controls)
                {
                    if (ctrl is CheckBox)
                    {
                        if (((CheckBox)ctrl).Checked)
                        {
                            list.Add(ctrl.Text);
                        }
                    }
                }
                if (list.Count > 1 && list.Exists(m => m == "综合报告单"))
                {
                    MessageBox.Show("综合报告单不能和其它报告一起！");
                }
                PDF(list, dataSet, ide);
                DialogResult dr = MessageBox.Show("成功！是否打开文件夹",
                                 "提示",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    string str = Application.StartupPath;//项目路径
                    OpenPdf(@str + $"/up/result/");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误请联系管理员！");
            }
        }

        private void PDF(List<string> list, DataSet dataSet, List<string> ide)
        {
            string str = Application.StartupPath;//项目路径
            Document doc = null;
            if (list.Count > 1)
            {
                foreach (var item in ide)
                {
                    List<Report> reports = new List<Report>();
                    foreach (var items in list)
                    {
                        Report report = new Report();
                        DataRow data = dataSet.Tables["个人"].Select($"archive_no={item}")[0];
                        report.Name = items;
                        report.Doc = PdfProcessing(items, data);
                        reports.Add(report);
                    }
                    Report re = reports.Where(m => m.Name == "封面").FirstOrDefault();
                    Report res = reports.Where(m => m.Name == "个人信息").FirstOrDefault();
                    if (re != null && res != null)
                    {
                        re.Doc.AppendDocument(res.Doc, ImportFormatMode.KeepSourceFormatting);
                        reports.Remove(re);
                        reports.Remove(res);
                        if (reports != null && reports.Count > 0)
                        {
                            foreach (var rs in reports)
                            {
                                re.Doc.AppendDocument(rs.Doc, ImportFormatMode.KeepSourceFormatting);
                            }
                        }
                        string urls = @str + $"/up/result/{item}.pdf";
                        DeteleFile(urls);
                        re.Doc.Save(urls, SaveFormat.Pdf);
                    }
                    else if (re != null)
                    {
                        reports.Remove(re);
                        if (reports != null && reports.Count > 0)
                        {
                            foreach (var rs in reports)
                            {
                                re.Doc.AppendDocument(rs.Doc, ImportFormatMode.KeepSourceFormatting);
                            }
                        }
                        string urls = @str + $"/up/result/{item}.pdf";
                        DeteleFile(urls);
                        re.Doc.Save(urls, SaveFormat.Pdf);
                    }
                    else if (res != null)
                    {
                        reports.Remove(res);
                        if (reports != null && reports.Count > 0)
                        {
                            foreach (var rs in reports)
                            {
                                res.Doc.AppendDocument(rs.Doc, ImportFormatMode.KeepSourceFormatting);
                            }
                        }
                        string urls = @str + $"/up/result/{item}.pdf";
                        DeteleFile(urls);
                        res.Doc.Save(urls, SaveFormat.Pdf);
                    }
                    else
                    {
                        Report rp = reports.Select(m => m).FirstOrDefault();
                        reports.Remove(rp);
                        if (reports != null && reports.Count > 0)
                        {
                            foreach (var rs in reports)
                            {
                                rp.Doc.AppendDocument(rs.Doc, ImportFormatMode.KeepSourceFormatting);
                            }
                        }
                        string urls = @str + $"/up/result/{item}.pdf";
                        DeteleFile(urls);
                        rp.Doc.Save(urls, SaveFormat.Pdf);
                    }
                }
            }
            else
            {
                if (list[0] == "综合报告单")
                {
                    List<string> vs = new List<string>();
                    vs.Add("封面");
                    vs.Add("个人信息");
                    vs.Add("化验报告单");
                    vs.Add("健康体检表");
                    vs.Add("心电图");
                    vs.Add("B超");
                    vs.Add("老年人生活自理能力评估");
                    vs.Add("中医体质");
                    vs.Add("结果");
                    for (int i = 0; i < dataSet.Tables["个人"].Rows.Count; i++)
                    {
                        List<Report> reports = new List<Report>();
                        DataRow data = dataSet.Tables["个人"].Rows[i];
                        foreach (var item in vs)
                        {
                            Report report = new Report();
                            report.Doc = PdfProcessing(item, data);
                            report.Name = item;
                            reports.Add(report);
                        }
                        Report re = reports.Where(m => m.Name == "封面").FirstOrDefault();
                        Report res = reports.Where(m => m.Name == "个人信息").FirstOrDefault();
                        re.Doc.AppendDocument(res.Doc, ImportFormatMode.KeepSourceFormatting);
                        reports.Remove(re);
                        reports.Remove(res);
                        foreach (var item in reports)
                        {
                            re.Doc.AppendDocument(item.Doc, ImportFormatMode.KeepSourceFormatting);
                        }
                        string urls = @str + $"/up/result/{data["archive_no"].ToString()}.pdf";
                        DeteleFile(urls);
                        doc.Save(urls, SaveFormat.Pdf);
                    }
                }
                else
                {
                    for (int i = 0; i < dataSet.Tables["个人"].Rows.Count; i++)
                    {
                        DataRow data = dataSet.Tables["个人"].Rows[i];
                        doc = PdfProcessing(list[0], data);
                        string urls = @str + $"/up/result/{data["archive_no"].ToString()}.pdf";
                        DeteleFile(urls);
                        doc.Save(urls, SaveFormat.Pdf);
                    }
                }
            }
        }

        private Document PdfProcessing(string lx, DataRow data)
        {
            DateTime date = DateTime.Now;
            string str = Application.StartupPath;//项目路径
            Document doc = null;
            DocumentBuilder builder = null;
            DataTable jkdata = null;
            DataSet jk = DbHelperMySQL.Query($"select * from physical_examination_record where aichive_no='{data["archive_no"].ToString()}' order by create_time desc LIMIT 1");
            if (jk != null && jk.Tables.Count > 0 && jk.Tables[0].Rows.Count > 0)
            {
                jkdata = jk.Tables[0];
            }
            switch (lx)
            {
                #region 封面
                case "封面":
                    doc = new Document(@str + $"/up/template/封面.doc");
                    builder = new DocumentBuilder(doc);
                    var dic = new Dictionary<string, string>();
                    string bh = data["archive_no"].ToString();
                    for (int i = 0; i < bh.Length; i++)
                    {
                        dic.Add("编号" + (i + 1), bh[i].ToString());
                    }
                    dic.Add("姓名", data["name"].ToString());
                    dic.Add("现住址", data["residence_address"].ToString());
                    dic.Add("户籍地址", data["address"].ToString());
                    dic.Add("联系电话", data["phone"].ToString());
                    dic.Add("乡镇名称", data["towns_name"].ToString());
                    dic.Add("村委会名称", data["village_name"].ToString() + "村委会");
                    dic.Add("建档单位", data["aichive_org"].ToString());
                    dic.Add("建档人", data["create_archives_name"].ToString());
                    dic.Add("责任医生", data["doctor_name"].ToString());
                    dic.Add("年", date.Year.ToString());
                    dic.Add("月", date.Month.ToString());
                    dic.Add("日", date.Day.ToString());
                    //书签替换
                    foreach (var key in dic.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(dic[key]);
                    }
                    return doc;
                #endregion

                #region 个人信息
                case "个人信息":
                    doc = new Document(@str + $"/up/template/个人信息.doc");
                    builder = new DocumentBuilder(doc);
                    var dics = new Dictionary<string, string>();
                    string grbh = data["archive_no"].ToString();
                    grbh = grbh.Substring(9, grbh.Length - 9);
                    for (int i = 0; i < grbh.Length; i++)
                    {
                        dics.Add("编号" + (i + 1), grbh[i].ToString());
                    }
                    dics.Add("姓名", data["name"].ToString());
                    dics.Add("性别", data["sex"].ToString());
                    string[] sr = data["birthday"].ToString().Split('-');
                    string r = sr[0] + sr[1] + sr[2];
                    for (int i = 0; i < r.Length; i++)
                    {
                        dics.Add("出生日期" + (i + 1), r[i].ToString());
                    }
                    dics.Add("身份证号", data["id_number"].ToString());
                    dics.Add("工作单位", data["company"].ToString());
                    dics.Add("本人电话", data["phone"].ToString());
                    dics.Add("联系人姓名", data["link_name"].ToString());
                    dics.Add("联系人电话", data["link_phone"].ToString());
                    dics.Add("常住类型", data["resident_type"].ToString());
                    dics.Add("民族", data["nation"].ToString());
                    dics.Add("血型", data["blood_group"].ToString());
                    dics.Add("RH", data["blood_rh"].ToString());
                    dics.Add("文化程度", data["education"].ToString());
                    dics.Add("职业", data["profession"].ToString());
                    dics.Add("婚姻状况", data["marital_status"].ToString());
                    string yyf = data["pay_type"].ToString();
                    if (yyf.IndexOf(',') > 0)
                    {
                        string[] y = yyf.Split(',');
                        for (int i = 0; i < y.Length; i++)
                        {
                            dics.Add("医疗费用" + (i + 1), y[i]);
                        }
                    }
                    else
                    {
                        dics.Add("医疗费用1", yyf);
                    }
                    string ywgm = data["drug_allergy"].ToString();
                    if (ywgm.IndexOf(',') > 0)
                    {
                        string[] y = ywgm.Split(',');
                        for (int i = 0; i < y.Length; i++)
                        {
                            dics.Add("药物过敏史" + (i + 1), y[i]);
                        }
                    }
                    else
                    {
                        dics.Add("药物过敏史1", ywgm);
                    }
                    string bls = data["exposure"].ToString();
                    if (bls.IndexOf(',') > 0)
                    {
                        string[] y = bls.Split(',');
                        for (int i = 0; i < y.Length; i++)
                        {
                            dics.Add("暴露史" + (i + 1), y[i]);
                        }
                    }
                    else
                    {
                        dics.Add("暴露史1", bls);
                    }
                    DataSet jb = DbHelperMySQL.Query($"SELECT * from resident_diseases where resident_base_info_id='{data["id"].ToString()}'");
                    if (jb != null && jb.Tables.Count > 0 && jb.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = jb.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            dics.Add("疾病" + (j + 1), da.Rows[j]["disease_type"].ToString());
                            string time = da.Rows[j]["disease_date"].ToString();
                            dics.Add("疾病时间" + (j + 1) + "年", time?.Split('-')[0]);
                            dics.Add("疾病时间" + (j + 1) + "月", time?.Split('-')[1]);
                        }
                    }
                    DataSet datas = DbHelperMySQL.Query($"SELECT * from operation_record where resident_base_info_id='{data["id"].ToString()}'");
                    if (datas != null && datas.Tables.Count > 0 && datas.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = datas.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            dics.Add("手术" + (j + 1), da.Rows[j]["operation_name"].ToString());
                            dics.Add("手术" + (j + 1) + "时间", da.Rows[j]["operation_time"].ToString());
                        }
                        dics.Add("手术", "2");
                    }
                    else
                    {
                        dics.Add("手术", "1");
                    }
                    DataSet ws = DbHelperMySQL.Query($"SELECT * from traumatism_record where resident_base_info_id='{data["id"].ToString()}'");
                    if (ws != null && ws.Tables.Count > 0 && ws.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = ws.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            dics.Add("外伤" + (j + 1), da.Rows[j]["traumatism_name"].ToString());
                            dics.Add("外伤" + (j + 1) + "时间", da.Rows[j]["traumatism_time"].ToString());
                        }
                        dics.Add("外伤", "2");
                    }
                    else
                    {
                        dics.Add("外伤", "1");
                    }
                    DataSet sx = DbHelperMySQL.Query($"SELECT * from metachysis_record where resident_base_info_id='{data["id"].ToString()}'");
                    if (sx != null && sx.Tables.Count > 0 && sx.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = sx.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            dics.Add("输血" + (j + 1), da.Rows[j]["metachysis_reasonn"].ToString());
                            dics.Add("输血" + (j + 1) + "时间", da.Rows[j]["metachysis_time"].ToString());
                        }
                        dics.Add("输血", "2");
                    }
                    else
                    {
                        dics.Add("输血", "1");
                    }
                    DataSet jz = DbHelperMySQL.Query($"SELECT * from family_record where resident_base_info_id='{data["id"].ToString()}'");
                    if (jz != null && jz.Tables.Count > 0 && jz.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = jz.Tables[0];
                        DataRow[] fq = da.Select("relation='父亲'");
                        if (fq != null && fq.Count() > 0)
                        {
                            for (int j = 0; j < fq.Count(); j++)
                            {
                                string fqs = fq[j]["disease_type"].ToString();
                                if (fqs.IndexOf(',') > 0)
                                {
                                    string[] y = fqs.Split(',');
                                    for (int i = 0; i < y.Length; i++)
                                    {
                                        dics.Add("家族史父亲" + (i + 1), y[i]);
                                    }
                                }
                            }
                        }
                        DataRow[] mq = da.Select("relation='母亲'");
                        if (mq != null && mq.Count() > 0)
                        {
                            for (int j = 0; j < mq.Count(); j++)
                            {
                                string fqs = mq[j]["disease_type"].ToString();
                                if (fqs.IndexOf(',') > 0)
                                {
                                    string[] y = fqs.Split(',');
                                    for (int i = 0; i < y.Length; i++)
                                    {
                                        dics.Add("家族史母亲" + (i + 1), y[i]);
                                    }
                                }
                            }
                        }
                        DataRow[] jm = da.Select("relation='兄弟姐妹'");
                        if (jm != null && jm.Count() > 0)
                        {

                            for (int j = 0; j < jm.Count(); j++)
                            {
                                string fqs = jm[j]["disease_type"].ToString();
                                if (fqs.IndexOf(',') > 0)
                                {
                                    string[] y = fqs.Split(',');
                                    for (int i = 0; i < y.Length; i++)
                                    {
                                        dics.Add("家族史兄弟" + (i + 1), y[i]);
                                    }
                                }
                            }
                        }
                        DataRow[] zn = da.Select("relation='子女'");
                        if (zn != null && zn.Count() > 0)
                        {
                            for (int j = 0; j < zn.Count(); j++)
                            {
                                string fqs = zn[j]["disease_type"].ToString();
                                if (fqs.IndexOf(',') > 0)
                                {
                                    string[] y = fqs.Split(',');
                                    for (int i = 0; i < y.Length; i++)
                                    {
                                        dics.Add("家族史子女" + (i + 1), y[i]);
                                    }
                                }
                            }
                        }
                    }
                    dics.Add("遗传病史名", data["heredity_name"].ToString());
                    dics.Add("遗传病史", data["is_heredity"].ToString());
                    string cjqk = data["is_deformity"].ToString();
                    if (cjqk.IndexOf(',') > 0)
                    {
                        string[] y = cjqk.Split(',');
                        for (int i = 0; i < y.Length; i++)
                        {
                            dics.Add("残疾情况" + (i + 1), y[i]);
                        }
                    }
                    else
                    {
                        dics.Add("残疾情况1", cjqk);
                    }
                    dics.Add("厨房排风设施", data["kitchen"].ToString());
                    dics.Add("燃料类型", data["fuel"].ToString());
                    dics.Add("饮水", data["drink"].ToString());
                    dics.Add("厕所", data["toilet"].ToString());
                    dics.Add("禽畜栏", data["poultry"].ToString());
                    //书签替换
                    foreach (var key in dics.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(dics[key]);
                    }
                    return doc;
                #endregion

                #region 化验报告单
                case "化验报告单":
                    doc = new Document(@str + $"/up/template/化验报告单.doc");
                    builder = new DocumentBuilder(doc);
                    var hy = new Dictionary<string, string>();
                    hy.Add("地址", data["address"].ToString());
                    hy.Add("姓名", data["name"].ToString());
                    hy.Add("姓名1", data["name"].ToString());
                    hy.Add("性别", data["sex"].ToString());
                    hy.Add("性别1", data["sex"].ToString());
                    hy.Add("生日", data["birthday"].ToString());
                    hy.Add("生日1", data["birthday"].ToString());
                    hy.Add("身份证号", data["id_number"].ToString());
                    hy.Add("身份证号1", data["id_number"].ToString());

                    if (jkdata != null && jkdata.Rows.Count > 0)
                    {
                        for (int j = 0; j < jkdata.Rows.Count; j++)
                        {
                            hy.Add("条码号", jkdata.Rows[j]["bar_code"].ToString());
                            hy.Add("身高", jkdata.Rows[j]["base_height"].ToString());
                            hy.Add("体重", jkdata.Rows[j]["base_weight"].ToString());
                            hy.Add("BMI", jkdata.Rows[j]["base_bmi"].ToString());
                            hy.Add("腰围", jkdata.Rows[j]["base_waist"].ToString());
                            hy.Add("体温", jkdata.Rows[j]["base_temperature"].ToString());
                            hy.Add("呼吸频率", jkdata.Rows[j]["base_respiratory"].ToString());

                            hy.Add("脉率", jkdata.Rows[j]["base_heartbeat"].ToString());
                            hy.Add("左侧高压", jkdata.Rows[j]["base_blood_pressure_left_high"].ToString());
                            hy.Add("左侧低压", jkdata.Rows[j]["base_blood_pressure_left_low"].ToString());
                            hy.Add("右侧高压", jkdata.Rows[j]["base_blood_pressure_right_high"].ToString());
                            hy.Add("右侧低压", jkdata.Rows[j]["base_blood_pressure_right_low"].ToString());
                            hy.Add("送检日期", jkdata.Rows[j]["check_da.Rowste"].ToString());
                            hy.Add("审核", "");
                            hy.Add("审核1", "");
                            hy.Add("报告日期", DateTime.Now.ToString("yyyy-MM-dd"));
                            hy.Add("报告日期1", DateTime.Now.ToString("yyyy-MM-dd"));
                        }
                    }

                    DataSet sh = DbHelperMySQL.Query($"select * from zkhw_tj_sh where aichive_no='{data["archive_no"].ToString()}' order by create_time desc LIMIT 1");
                    if (sh != null && sh.Tables.Count > 0 && sh.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = sh.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            hy.Add("白蛋白箭头", Convert.ToDouble(da.Rows[j]["ALB"].ToString()) > 54 ? "↑" : "↓");
                            hy.Add("白蛋白结果", da.Rows[j]["ALB"].ToString());
                            hy.Add("碱性磷酸酶箭头", Convert.ToDouble(da.Rows[j]["ALP"].ToString()) > 100 ? "↑" : "↓");
                            hy.Add("碱性磷酸酶结果", da.Rows[j]["ALP"].ToString());
                            hy.Add("谷丙转氨酶箭头", Convert.ToDouble(da.Rows[j]["ALT"].ToString()) > 40 ? "↑" : "↓");
                            hy.Add("谷丙转氨酶结果", da.Rows[j]["ALT"].ToString());
                            hy.Add("谷草转氨酶箭头", Convert.ToDouble(da.Rows[j]["AST"].ToString()) > 40 ? "↑" : "↓");
                            hy.Add("谷草转氨酶结果", da.Rows[j]["AST"].ToString());
                            hy.Add("胆固醇箭头", Convert.ToDouble(da.Rows[j]["CHO"].ToString()) > 5 ? "↑" : "↓");
                            hy.Add("胆固醇结果", da.Rows[j]["CHO"].ToString());
                            hy.Add("肌酐箭头", Convert.ToDouble(da.Rows[j]["CREA"].ToString()) > 104 ? "↑" : "↓");
                            hy.Add("肌酐结果", da.Rows[j]["CREA"].ToString());
                            hy.Add("直接胆红素箭头", Convert.ToDouble(da.Rows[j]["DBIL"].ToString()) > 7 ? "↑" : "↓");
                            hy.Add("直接胆红素结果", da.Rows[j]["DBIL"].ToString());
                            hy.Add("谷氨酰氨基箭头", Convert.ToDouble(da.Rows[j]["GGT"].ToString()) > 50 ? "↑" : "↓");
                            hy.Add("谷氨酰氨基结果", da.Rows[j]["GGT"].ToString());
                            hy.Add("葡萄糖箭头", Convert.ToDouble(da.Rows[j]["GLU"].ToString()) > 6 ? "↑" : "↓");
                            hy.Add("葡萄糖结果", da.Rows[j]["GLU"].ToString());
                            hy.Add("高密度脂蛋白箭头", Convert.ToDouble(da.Rows[j]["HDLC"].ToString()) > 2 ? "↑" : "↓");
                            hy.Add("高密度脂蛋白结果", da.Rows[j]["HDLC"].ToString());
                            hy.Add("低密度脂蛋白箭头", Convert.ToDouble(da.Rows[j]["LDLC"].ToString()) > 4 ? "↑" : "↓");
                            hy.Add("低密度脂蛋白结果", da.Rows[j]["LDLC"].ToString());
                            hy.Add("总胆红素箭头", Convert.ToDouble(da.Rows[j]["TBIL"].ToString()) > 19 ? "↑" : "↓");
                            hy.Add("总胆红素结果", da.Rows[j]["TBIL"].ToString());
                            hy.Add("甘油三酯箭头", Convert.ToDouble(da.Rows[j]["TG"].ToString()) > 2 ? "↑" : "↓");
                            hy.Add("甘油三酯结果", da.Rows[j]["TG"].ToString());
                            hy.Add("总蛋白箭头", Convert.ToDouble(da.Rows[j]["TP"].ToString()) > 19 ? "↑" : "↓");
                            hy.Add("总蛋白结果", da.Rows[j]["TP"].ToString());
                            hy.Add("尿酸箭头", Convert.ToDouble(da.Rows[j]["UA"].ToString()) > 2 ? "↑" : "↓");
                            hy.Add("尿酸结果", da.Rows[j]["UA"].ToString());
                            hy.Add("尿素箭头", Convert.ToDouble(da.Rows[j]["UREA"].ToString()) > 83 ? "↑" : "↓");
                            hy.Add("尿素结果", da.Rows[j]["UREA"].ToString());
                        }
                    }

                    DataSet xcg = DbHelperMySQL.Query($"select * from zkhw_tj_xcg where aichive_no='{data["archive_no"].ToString()}' order by createtime desc LIMIT 1");
                    if (xcg != null && xcg.Tables.Count > 0 && xcg.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = xcg.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            hy.Add("红细胞压积箭头", Convert.ToDouble(da.Rows[j]["HCT"].ToString()) > 50 ? "↑" : "↓");
                            hy.Add("红细胞压积结果", da.Rows[j]["HCT"].ToString());
                            hy.Add("血红蛋白箭头", Convert.ToDouble(da.Rows[j]["HGB"].ToString()) > 160 ? "↑" : "↓");
                            hy.Add("血红蛋白结果", da.Rows[j]["HGB"].ToString());
                            hy.Add("淋巴细胞数目箭头", Convert.ToDouble(da.Rows[j]["LYM"].ToString()) > 4 ? "↑" : "↓");
                            hy.Add("淋巴细胞数目结果", da.Rows[j]["LYM"].ToString());
                            hy.Add("淋巴细胞百分比箭头", Convert.ToDouble(da.Rows[j]["LYMP"].ToString()) > 40 ? "↑" : "↓");
                            hy.Add("淋巴细胞百分比结果", da.Rows[j]["LYMP"].ToString());
                            hy.Add("平均血红蛋白含量箭头", Convert.ToDouble(da.Rows[j]["MCH"].ToString()) > 40 ? "↑" : "↓");
                            hy.Add("平均血红蛋白含量结果", da.Rows[j]["MCH"].ToString());
                            hy.Add("平均血红蛋白浓度箭头", Convert.ToDouble(da.Rows[j]["MCHC"].ToString()) > 360 ? "↑" : "↓");
                            hy.Add("平均血红蛋白浓度结果", da.Rows[j]["MCHC"].ToString());
                            hy.Add("平均红细胞体积箭头", Convert.ToDouble(da.Rows[j]["MCV"].ToString()) > 95 ? "↑" : "↓");
                            hy.Add("平均红细胞体积结果", da.Rows[j]["MCV"].ToString());
                            hy.Add("平均血小板体积箭头", Convert.ToDouble(da.Rows[j]["MPV"].ToString()) > 11 ? "↑" : "↓");
                            hy.Add("平均血小板体积结果", da.Rows[j]["MPV"].ToString());
                            hy.Add("中间细胞数目箭头", Convert.ToDouble(da.Rows[j]["MXD"].ToString()) > 0.9 ? "↑" : "↓");
                            hy.Add("中间细胞数目结果", da.Rows[j]["MXD"].ToString());
                            hy.Add("中间细胞百分比箭头", Convert.ToDouble(da.Rows[j]["MXDP"].ToString()) > 12 ? "↑" : "↓");
                            hy.Add("中间细胞百分比结果", da.Rows[j]["MXDP"].ToString());
                            hy.Add("中性粒细胞数目箭头", Convert.ToDouble(da.Rows[j]["NEUT"].ToString()) > 7 ? "↑" : "↓");
                            hy.Add("中性粒细胞数目结果", da.Rows[j]["NEUT"].ToString());
                            hy.Add("中性粒细胞百分比箭头", Convert.ToDouble(da.Rows[j]["NEUTP"].ToString()) > 70 ? "↑" : "↓");
                            hy.Add("中性粒细胞百分比结果", da.Rows[j]["NEUTP"].ToString());
                            hy.Add("血小板压积箭头", Convert.ToDouble(da.Rows[j]["PCT"].ToString()) > 0.4 ? "↑" : "↓");
                            hy.Add("血小板压积结果", da.Rows[j]["PCT"].ToString());
                            hy.Add("血小板分布宽度箭头", Convert.ToDouble(da.Rows[j]["PDW"].ToString()) > 17 ? "↑" : "↓");
                            hy.Add("血小板分布宽度结果", da.Rows[j]["PDW"].ToString());
                            hy.Add("血小板数目箭头", Convert.ToDouble(da.Rows[j]["PLT"].ToString()) > 300 ? "↑" : "↓");
                            hy.Add("血小板数目结果", da.Rows[j]["PLT"].ToString());
                            hy.Add("红细胞数目箭头", Convert.ToDouble(da.Rows[j]["RBC"].ToString()) > 5.5 ? "↑" : "↓");
                            hy.Add("红细胞数目结果", da.Rows[j]["RBC"].ToString());
                            hy.Add("红细胞分布宽度CV箭头", Convert.ToDouble(da.Rows[j]["RDWCV"].ToString()) > 18 ? "↑" : "↓");
                            hy.Add("红细胞分布宽度CV结果", da.Rows[j]["RDWCV"].ToString());
                            hy.Add("红细胞分布宽度SD箭头", Convert.ToDouble(da.Rows[j]["RDWSD"].ToString()) > 56 ? "↑" : "↓");
                            hy.Add("红细胞分布宽度SD结果", da.Rows[j]["RDWSD"].ToString());
                            hy.Add("白细胞数目箭头", Convert.ToDouble(da.Rows[j]["WBC"].ToString()) > 10 ? "↑" : "↓");
                            hy.Add("白细胞数目结果", da.Rows[j]["WBC"].ToString());
                        }
                    }

                    DataSet ncg = DbHelperMySQL.Query($"select * from zkhw_tj_ncg where aichive_no='{data["archive_no"].ToString()}' order by createtime desc LIMIT 1");
                    if (ncg != null && ncg.Tables.Count > 0 && ncg.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = ncg.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            hy.Add("白细胞结果", da.Rows[j]["WBC"].ToString());
                            hy.Add("酮体结果", da.Rows[j]["KET"].ToString());
                            hy.Add("亚硝酸盐结果", da.Rows[j]["NIT"].ToString());
                            hy.Add("胆红素结果", da.Rows[j]["BIL"].ToString());
                            hy.Add("蛋白质结果", da.Rows[j]["PRO"].ToString());
                            hy.Add("尿液葡萄糖结果", da.Rows[j]["GLU"].ToString());
                            hy.Add("尿比重箭头", Convert.ToDouble(da.Rows[j]["SG"].ToString()) > 1.035 ? "↑" : "↓");
                            hy.Add("尿比重结果", da.Rows[j]["SG"].ToString());
                            hy.Add("隐血结果", da.Rows[j]["BLD"].ToString());
                            hy.Add("酸碱度箭头", Convert.ToDouble(da.Rows[j]["PH"].ToString()) > 8 ? "↑" : "↓");
                            hy.Add("酸碱度结果", da.Rows[j]["PH"].ToString());
                            hy.Add("维生素C箭头", Convert.ToDouble(da.Rows[j]["Vc"].ToString()) > 40 ? "↑" : "↓");
                            hy.Add("维生素C结果", da.Rows[j]["Vc"].ToString());
                        }
                    }

                    //书签替换
                    foreach (var key in hy.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(hy[key]);
                    }
                    return doc;
                #endregion

                #region 健康体检表
                case "健康体检表":
                    doc = new Document(@str + $"/up/template/健康体检表.doc");
                    builder = new DocumentBuilder(doc);
                    var jktj = new Dictionary<string, string>();
                    string jkbh = data["archive_no"].ToString();
                    jkbh = jkbh.Substring(9, jkbh.Length - 9);
                    for (int i = 0; i < jkbh.Length; i++)
                    {
                        jktj.Add("编号" + (i + 1), jkbh[i].ToString());
                    }
                    jktj.Add("姓名", data["name"].ToString());
                    if (jkdata != null && jkdata.Rows.Count > 0)
                    {
                        for (int j = 0; j < jkdata.Rows.Count; j++)
                        {
                            string time = jkdata.Rows[j]["check_date"].ToString();
                            jktj.Add("体检日期年", time?.Split('-')[0]);
                            jktj.Add("体检日期月", time?.Split('-')[1]);
                            jktj.Add("体检日期日", time?.Split('-')[2]);
                            jktj.Add("责任医生", jkdata.Rows[j]["doctor_name"].ToString());
                            string zz = jkdata.Rows[j]["symptom"].ToString();
                            if (zz.IndexOf(',') > 0)
                            {
                                string[] y = zz.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("症状" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("症状1", zz);
                            }
                            jktj.Add("体温", jkdata.Rows[j]["base_temperature"].ToString());
                            jktj.Add("脉率", jkdata.Rows[j]["base_heartbeat"].ToString());
                            jktj.Add("呼吸频率", jkdata.Rows[j]["base_respiratory"].ToString());
                            jktj.Add("左侧高", jkdata.Rows[j]["base_blood_pressure_left_high"].ToString());
                            jktj.Add("左侧低", jkdata.Rows[j]["base_blood_pressure_left_low"].ToString());
                            jktj.Add("右侧高", jkdata.Rows[j]["base_blood_pressure_right_high"].ToString());
                            jktj.Add("右侧低", jkdata.Rows[j]["base_blood_pressure_right_low"].ToString());
                            jktj.Add("身高", jkdata.Rows[j]["base_height"].ToString());
                            jktj.Add("体重", jkdata.Rows[j]["base_weight"].ToString());
                            jktj.Add("腰围", jkdata.Rows[j]["base_waist"].ToString());
                            jktj.Add("体质指数", jkdata.Rows[j]["base_bmi"].ToString());
                            jktj.Add("健康自我评估", jkdata.Rows[j]["base_health_estimate"].ToString());
                            jktj.Add("生活自我评估", jkdata.Rows[j]["base_selfcare_estimate"].ToString());
                            jktj.Add("认知能力", jkdata.Rows[j]["base_cognition_estimate"].ToString());
                            jktj.Add("情感状态", jkdata.Rows[j]["base_feeling_estimate"].ToString());
                            jktj.Add("锻炼频率", jkdata.Rows[j]["lifeway_exercise_frequency"].ToString());
                            jktj.Add("锻炼时间", jkdata.Rows[j]["lifeway_exercise_time"].ToString());
                            jktj.Add("锻炼时间年", jkdata.Rows[j]["lifeway_exercise_year"].ToString());
                            jktj.Add("锻炼方式", jkdata.Rows[j]["lifeway_exercise_type"].ToString());
                            string ysxg = jkdata.Rows[j]["lifeway_diet"].ToString();
                            if (ysxg.IndexOf(',') > 0)
                            {
                                string[] y = ysxg.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("饮食习惯" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("饮食习惯1", ysxg);
                            }
                            jktj.Add("吸烟状况", jkdata.Rows[j]["lifeway_smoke_status"].ToString());
                            jktj.Add("日吸烟量", jkdata.Rows[j]["lifeway_smoke_number"].ToString());
                            jktj.Add("开始吸烟年龄", jkdata.Rows[j]["lifeway_smoke_startage"].ToString());
                            jktj.Add("戒烟年龄", jkdata.Rows[j]["lifeway_smoke_endage"].ToString());
                            jktj.Add("饮酒频率", jkdata.Rows[j]["lifeway_drink_status"].ToString());
                            jktj.Add("日饮酒量", jkdata.Rows[j]["lifeway_drink_number"].ToString());
                            jktj.Add("是否戒酒", jkdata.Rows[j]["lifeway_drink_stop"].ToString());
                            jktj.Add("戒酒年龄", jkdata.Rows[j]["lifeway_drink_stopage"].ToString());
                            jktj.Add("开始饮酒年龄", jkdata.Rows[j]["lifeway_drink_startage"].ToString());
                            jktj.Add("是否曾醉酒", jkdata.Rows[j]["lifeway_drink_oneyear"].ToString());
                            string yjzl = jkdata.Rows[j]["lifeway_drink_type"].ToString();
                            if (yjzl.IndexOf(',') > 0)
                            {
                                string[] y = yjzl.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("饮酒种类" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("饮酒种类1", yjzl);
                            }
                            jktj.Add("工种", jkdata.Rows[j]["lifeway_occupational_disease"].ToString());
                            jktj.Add("工种名", jkdata.Rows[j]["lifeway_job"].ToString());
                            jktj.Add("工种年限", jkdata.Rows[j]["lifeway_job_period"].ToString());
                            jktj.Add("毒物种类1", jkdata.Rows[j]["lifeway_dust_preventive"].ToString());
                            jktj.Add("毒物种类名1", jkdata.Rows[j]["lifeway_hazardous_dust"].ToString());
                            jktj.Add("毒物种类2", jkdata.Rows[j]["lifeway_radiation_preventive"].ToString());
                            jktj.Add("毒物种类名2", jkdata.Rows[j]["lifeway_hazardous_radiation"].ToString());
                            jktj.Add("毒物种类3", jkdata.Rows[j]["lifeway_physical_preventive"].ToString());
                            jktj.Add("毒物种类名3", jkdata.Rows[j]["lifeway_hazardous_physical"].ToString());
                            jktj.Add("毒物种类4", jkdata.Rows[j]["lifeway_chemical_preventive"].ToString());
                            jktj.Add("毒物种类名4", jkdata.Rows[j]["lifeway_hazardous_chemical"].ToString());
                            jktj.Add("毒物种类5", jkdata.Rows[j]["lifeway_other_preventive"].ToString());
                            jktj.Add("毒物种类名5", jkdata.Rows[j]["lifeway_hazardous_other"].ToString());
                            jktj.Add("口唇", jkdata.Rows[j]["organ_lips"].ToString());
                            string cl = jkdata.Rows[j]["organ_tooth"].ToString();
                            if (cl.IndexOf(',') > 0)
                            {
                                string[] y = cl.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("齿列" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("齿列1", cl);
                            }
                            jktj.Add("咽部", jkdata.Rows[j]["organ_guttur"].ToString());
                            jktj.Add("左眼", jkdata.Rows[j]["organ_vision_left"].ToString());
                            jktj.Add("右眼", jkdata.Rows[j]["organ_vision_right"].ToString());
                            jktj.Add("矫正视力左眼", jkdata.Rows[j]["organ_correctedvision_left"].ToString());
                            jktj.Add("矫正视力右眼", jkdata.Rows[j]["organ_correctedvision_right"].ToString());
                            jktj.Add("听力", jkdata.Rows[j]["organ_hearing"].ToString());
                            jktj.Add("运动功能", jkdata.Rows[j]["organ_movement"].ToString());
                            jktj.Add("眼底", jkdata.Rows[j]["examination_eye"].ToString());
                            jktj.Add("皮肤", jkdata.Rows[j]["examination_skin"].ToString());
                            jktj.Add("巩膜", jkdata.Rows[j]["examination_sclera"].ToString());
                            jktj.Add("淋巴结", jkdata.Rows[j]["examination_lymph"].ToString());
                            jktj.Add("桶状胸", jkdata.Rows[j]["examination_barrel_chest"].ToString());
                            jktj.Add("呼吸音", jkdata.Rows[j]["examination_breath_sounds"].ToString());
                            jktj.Add("罗音", jkdata.Rows[j]["examination_rale"].ToString());
                            jktj.Add("心率", jkdata.Rows[j]["examination_heart_rate"].ToString());
                            jktj.Add("心律", jkdata.Rows[j]["examination_heart_rhythm"].ToString());
                            jktj.Add("杂音", jkdata.Rows[j]["examination_heart_noise"].ToString());
                            jktj.Add("压痛", jkdata.Rows[j]["examination_abdomen_tenderness"].ToString());
                            jktj.Add("包块", jkdata.Rows[j]["examination_abdomen_mass"].ToString());
                            jktj.Add("肝大", jkdata.Rows[j]["examination_abdomen_hepatomegaly"].ToString());
                            jktj.Add("脾大", jkdata.Rows[j]["examination_abdomen_splenomegaly"].ToString());
                            jktj.Add("移动性浊音", jkdata.Rows[j]["examination_abdomen_shiftingdullness"].ToString());
                            jktj.Add("下肢水肿", jkdata.Rows[j]["examination_lowerextremity_edema"].ToString());
                            jktj.Add("足背动脉搏动", jkdata.Rows[j]["examination_dorsal_artery"].ToString());
                            jktj.Add("肛门指诊", jkdata.Rows[j]["examination_anus"].ToString());
                            string lxx = jkdata.Rows[j]["examination_breast"].ToString();
                            if (lxx.IndexOf(',') > 0)
                            {
                                string[] y = lxx.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("乳腺" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("乳腺1", lxx);
                            }
                            jktj.Add("外阴", jkdata.Rows[j]["examination_woman_vulva"].ToString());
                            jktj.Add("阴道", jkdata.Rows[j]["examination_woman_vagina"].ToString());
                            jktj.Add("宫颈", jkdata.Rows[j]["examination_woman_cervix"].ToString());
                            jktj.Add("宫体", jkdata.Rows[j]["examination_woman_corpus"].ToString());
                            jktj.Add("附件", jkdata.Rows[j]["examination_woman_accessories"].ToString());
                            jktj.Add("妇科其它", jkdata.Rows[j]["examination_other"].ToString());
                            jktj.Add("血红蛋白", jkdata.Rows[j]["blood_hemoglobin"].ToString());
                            jktj.Add("白细胞", jkdata.Rows[j]["blood_leukocyte"].ToString());
                            jktj.Add("血小板", jkdata.Rows[j]["blood_platelet"].ToString());
                            jktj.Add("尿蛋白", jkdata.Rows[j]["urine_protein"].ToString());
                            jktj.Add("血常规其它", jkdata.Rows[j]["blood_other"].ToString());
                            jktj.Add("尿糖", jkdata.Rows[j]["glycosuria"].ToString());
                            jktj.Add("尿酮体", jkdata.Rows[j]["urine_acetone_bodies"].ToString());
                            jktj.Add("尿潜血", jkdata.Rows[j]["bld"].ToString());
                            jktj.Add("尿常规其它", jkdata.Rows[j]["urine_other"].ToString());
                            jktj.Add("心电图", jkdata.Rows[j]["cardiogram"].ToString());
                            jktj.Add("尿微量白蛋白", jkdata.Rows[j]["microalbuminuria"].ToString());
                            jktj.Add("大便潜血", jkdata.Rows[j]["fob"].ToString());
                            jktj.Add("糖化血红蛋白", jkdata.Rows[j]["glycosylated_hemoglobin"].ToString());
                            jktj.Add("乙型肝炎", jkdata.Rows[j]["hb"].ToString());
                            jktj.Add("血清谷丙转氨酶", jkdata.Rows[j]["sgft"].ToString());
                            jktj.Add("血清谷草转氨酶", jkdata.Rows[j]["ast"].ToString());
                            jktj.Add("白蛋白", jkdata.Rows[j]["albumin"].ToString());
                            jktj.Add("总胆红素", jkdata.Rows[j]["total_bilirubin"].ToString());
                            jktj.Add("结合胆红素", jkdata.Rows[j]["conjugated_bilirubin"].ToString());
                            jktj.Add("血清肌酐", jkdata.Rows[j]["scr"].ToString());
                            jktj.Add("血尿素", jkdata.Rows[j]["blood_urea"].ToString());
                            jktj.Add("血钾浓度", jkdata.Rows[j]["blood_k"].ToString());
                            jktj.Add("血钠浓度", jkdata.Rows[j]["blood_na"].ToString());
                            jktj.Add("总胆固醇", jkdata.Rows[j]["tc"].ToString());
                            jktj.Add("甘油三酯", jkdata.Rows[j]["tg"].ToString());
                            jktj.Add("血清低密度脂蛋白胆固醇", jkdata.Rows[j]["ldl"].ToString());
                            jktj.Add("血清高密度脂蛋白胆固醇", jkdata.Rows[j]["hdl"].ToString());
                            jktj.Add("胸部X线片", jkdata.Rows[j]["chest_x"].ToString());
                            jktj.Add("腹部B超", jkdata.Rows[j]["ultrasound_abdomen"].ToString());
                            jktj.Add("B超其他", jkdata.Rows[j]["other_b"].ToString());
                            jktj.Add("宫颈涂片", jkdata.Rows[j]["cervical_smear"].ToString());
                            jktj.Add("辅助检查其它", jkdata.Rows[j]["other"].ToString());
                            string lxgjb = jkdata.Rows[j]["cerebrovascular_disease"].ToString();
                            if (lxgjb.IndexOf(',') > 0)
                            {
                                string[] y = lxgjb.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("脑血管疾病" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("脑血管疾病1", lxgjb);
                            }
                            string szjb = jkdata.Rows[j]["kidney_disease"].ToString();
                            if (szjb.IndexOf(',') > 0)
                            {
                                string[] y = szjb.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("肾脏疾病" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("肾脏疾病1", szjb);
                            }
                            string xzjb = jkdata.Rows[j]["heart_disease"].ToString();
                            if (xzjb.IndexOf(',') > 0)
                            {
                                string[] y = xzjb.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("心脏疾病" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("心脏疾病1", xzjb);
                            }
                            string xgjb = jkdata.Rows[j]["vascular_disease"].ToString();
                            if (xgjb.IndexOf(',') > 0)
                            {
                                string[] y = xgjb.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("血管疾病" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("血管疾病1", xgjb);
                            }
                            string ybjb = jkdata.Rows[j]["ocular_diseases"].ToString();
                            if (ybjb.IndexOf(',') > 0)
                            {
                                string[] y = ybjb.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("眼部疾病" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("眼部疾病1", ybjb);
                            }
                            jktj.Add("神经系统疾病", jkdata.Rows[j]["nervous_system_disease"].ToString());
                            jktj.Add("其他系统疾病", jkdata.Rows[j]["other_disease"].ToString());
                            DataSet zys = DbHelperMySQL.Query($"select * from hospitalized_record where archive_no='{data["archive_no"].ToString()}'");
                            if (zys != null && zys.Tables.Count > 0 && zys.Tables[0].Rows.Count > 0)
                            {
                                DataTable da = zys.Tables[0];
                                for (int k = 0; k < da.Rows.Count; k++)
                                {
                                    if (jkdata.Rows[j]["hospitalized_type"].ToString() == "1")
                                    {
                                        jktj.Add("住院入时间" + (k + 1), da.Rows[k]["in_hospital_time"].ToString());
                                        jktj.Add("住院出时间" + (k + 1), da.Rows[k]["leave_hospital_time"].ToString());
                                        jktj.Add("住院原因" + (k + 1), da.Rows[k]["reason"].ToString());
                                        jktj.Add("医疗机构" + (k + 1), da.Rows[k]["hospital_organ"].ToString());
                                        jktj.Add("病案号" + (k + 1), da.Rows[k]["case_code"].ToString());
                                    }
                                    else if (jkdata.Rows[j]["hospitalized_type"].ToString() == "2")
                                    {
                                        jktj.Add("家庭病床建" + (k + 1), da.Rows[k]["in_hospital_time"].ToString());
                                        jktj.Add("家庭病床撤" + (k + 1), da.Rows[k]["leave_hospital_time"].ToString());
                                        jktj.Add("家庭病床原因" + (k + 1), da.Rows[k]["reason"].ToString());
                                        jktj.Add("家庭病床医疗机构" + (k + 1), da.Rows[k]["hospital_organ"].ToString());
                                        jktj.Add("家庭病床病案号" + (k + 1), da.Rows[k]["case_code"].ToString());
                                    }
                                }
                            }
                            DataSet yyqk = DbHelperMySQL.Query($"select * from take_medicine_record where archive_no='{data["archive_no"].ToString()}'");
                            if (yyqk != null && yyqk.Tables.Count > 0 && yyqk.Tables[0].Rows.Count > 0)
                            {
                                DataTable da = yyqk.Tables[0];
                                for (int k = 0; k < da.Rows.Count; k++)
                                {
                                    jktj.Add("药物名称" + (k + 1), da.Rows[k]["medicine_name"].ToString());
                                    jktj.Add("药物用法" + (k + 1), da.Rows[k]["medicine_usage"].ToString());
                                    jktj.Add("药物用量" + (k + 1), da.Rows[k]["medicine_dosage"].ToString());
                                    jktj.Add("药物用药时间" + (k + 1), da.Rows[k]["medicine_time"].ToString());
                                    jktj.Add("药物服药依从性" + (k + 1), da.Rows[k]["medicine_compliance"].ToString());
                                }
                            }
                            DataSet jzym = DbHelperMySQL.Query($"select * from vaccination_record where archive_no='{data["archive_no"].ToString()}'");
                            if (jzym != null && jzym.Tables.Count > 0 && jzym.Tables[0].Rows.Count > 0)
                            {
                                DataTable da = jzym.Tables[0];
                                for (int k = 0; k < da.Rows.Count; k++)
                                {
                                    jktj.Add("预防接种名称" + (k + 1), da.Rows[k]["vaccination_name"].ToString());
                                    jktj.Add("预防接种时间" + (k + 1), da.Rows[k]["vaccination_time"].ToString());
                                    jktj.Add("预防接种机构" + (k + 1), da.Rows[k]["vaccination_organ"].ToString());
                                }
                            }
                            jktj.Add("健康评价", jkdata.Rows[j]["health_evaluation"].ToString());
                            jktj.Add("健康评价异常1", jkdata.Rows[j]["abnormal1"].ToString());
                            jktj.Add("健康评价异常2", jkdata.Rows[j]["abnormal2"].ToString());
                            jktj.Add("健康评价异常3", jkdata.Rows[j]["abnormal3"].ToString());
                            jktj.Add("健康评价异常4", jkdata.Rows[j]["abnormal4"].ToString());
                            string jkzd = jkdata.Rows[j]["health_guidance"].ToString();
                            if (jkzd.IndexOf(',') > 0)
                            {
                                string[] y = jkzd.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("健康指导" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("健康指导1", jkzd);
                            }
                            string wxyskz = jkdata.Rows[j]["danger_controlling"].ToString();
                            if (wxyskz.IndexOf(',') > 0)
                            {
                                string[] y = wxyskz.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    jktj.Add("危险因素控制" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                jktj.Add("危险因素控制1", wxyskz);
                            }
                            jktj.Add("减体重目标", jkdata.Rows[j]["target_weight"].ToString());
                        }
                    }

                    //书签替换
                    foreach (var key in jktj.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(jktj[key]);
                    }
                    return doc;
                #endregion

                #region 心电图
                case "心电图":
                    doc = new Document(@str + $"/up/template/心电图.doc");
                    builder = new DocumentBuilder(doc);
                    var xdt = new Dictionary<string, string>();
                    xdt.Add("地址", data["address"].ToString());
                    xdt.Add("姓名", data["name"].ToString());
                    xdt.Add("性别", data["sex"].ToString());
                    xdt.Add("生日", data["birthday"].ToString());
                    xdt.Add("身份证号", data["id_number"].ToString());
                    DataSet xdts = DbHelperMySQL.Query($"select * from zkhw_tj_xdt where aichive_no='{data["archive_no"].ToString()}' order by create_time desc LIMIT 1");
                    if (xdts != null && xdts.Tables.Count > 0 && xdts.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = xdts.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            builder.MoveToBookmark("图片");
                            builder.InsertImage(resizeImageFromFile(da.Rows[j]["imageUrl"].ToString(), 678, 960));
                            xdt.Add("条码号", da.Rows[j]["bar_code"].ToString());
                            xdt.Add("诊断医师", da.Rows[j]["XdtDoctor"].ToString());
                            xdt.Add("诊断意见", da.Rows[j]["XdtResult"].ToString());
                            xdt.Add("检查时间", da.Rows[j]["createtime"].ToString());
                        }
                    }
                    //书签替换
                    foreach (var key in xdt.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(xdt[key]);
                    }
                    return doc;
                #endregion

                #region B超
                case "B超":
                    doc = new Document(@str + $"/up/template/B超.doc");
                    builder = new DocumentBuilder(doc);
                    var bc = new Dictionary<string, string>();
                    bc.Add("地址", data["address"].ToString());
                    bc.Add("姓名", data["name"].ToString());
                    bc.Add("性别", data["sex"].ToString());
                    bc.Add("生日", data["birthday"].ToString());
                    bc.Add("身份证号", data["id_number"].ToString());
                    DataSet bcs = DbHelperMySQL.Query($"select * from zkhw_tj_bc where aichive_no='{data["archive_no"].ToString()}' order by create_time desc LIMIT 1");
                    if (bcs != null && bcs.Tables.Count > 0 && bcs.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = bcs.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            builder.MoveToBookmark("图片1");
                            builder.InsertImage(resizeImageFromFile(da.Rows[j]["imageUrl_a"].ToString(), 768, 1024));
                            builder.MoveToBookmark("图片2");
                            builder.InsertImage(resizeImageFromFile(da.Rows[j]["imageUrl_b"].ToString(), 768, 1024));
                            builder.MoveToBookmark("图片3");
                            builder.InsertImage(resizeImageFromFile(da.Rows[j]["imageUrl_c"].ToString(), 768, 1024));
                            builder.MoveToBookmark("图片4");
                            builder.InsertImage(resizeImageFromFile(da.Rows[j]["imageUrl_d"].ToString(), 768, 1024));
                            bc.Add("条码号", da.Rows[j]["bar_code"].ToString());
                            bc.Add("诊断医师", "");
                            bc.Add("检查所见", da.Rows[j]["FubuDesc"].ToString());
                            bc.Add("诊断结果", da.Rows[j]["FubuResult"].ToString());
                            bc.Add("检查时间", da.Rows[j]["createtime"].ToString());
                        }
                    }
                    //书签替换
                    foreach (var key in bc.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(bc[key]);
                    }
                    return doc;
                #endregion

                #region 结果
                case "结果":
                    doc = new Document(@str + $"/up/template/结果.doc");
                    builder = new DocumentBuilder(doc);
                    var jg = new Dictionary<string, string>();

                    if (jkdata != null && jkdata.Rows.Count > 0)
                    {
                        for (int j = 0; j < jkdata.Rows.Count; j++)
                        {
                            if (jkdata.Rows[j]["health_evaluation"].ToString() == "2")
                            {
                                jg.Add("结果", jkdata.Rows[j]["abnormal1"].ToString() + jkdata.Rows[j]["abnormal2"].ToString() + jkdata.Rows[j]["abnormal3"].ToString() + jkdata.Rows[j]["abnormal4"].ToString());
                            }
                        }
                    }
                    //书签替换
                    foreach (var key in jg.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(jg[key]);
                    }
                    return doc;
                #endregion

                #region 老年人生活自理能力评估
                case "老年人生活自理能力评估":
                    doc = new Document(@str + $"/up/template/老年人生活自理能力评估.doc");
                    builder = new DocumentBuilder(doc);
                    var zlpg = new Dictionary<string, string>();
                    DataSet zlpgs = DbHelperMySQL.Query($"select * from elderly_selfcare_estimate where aichive_no='{data["archive_no"].ToString()}' order by create_time desc LIMIT 1");
                    if (zlpgs != null && zlpgs.Tables.Count > 0 && zlpgs.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = zlpgs.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            string zz = da.Rows[j]["answer_result"].ToString();
                            if (zz.IndexOf(',') > 0)
                            {
                                string[] y = zz.Split(',');
                                for (int i = 0; i < y.Length; i++)
                                {
                                    zlpg.Add("评分" + (i + 1), y[i]);
                                }
                            }
                            else
                            {
                                zlpg.Add("评分1", zz);
                            }
                            zlpg.Add("总分", da.Rows[j]["total_score"].ToString());
                        }
                    }
                    //书签替换
                    foreach (var key in zlpg.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(zlpg[key]);
                    }
                    return doc;
                #endregion

                #region 中医体质
                case "中医体质":
                    doc = new Document(@str + $"/up/template/中医体质.doc");
                    builder = new DocumentBuilder(doc);
                    var zytz = new Dictionary<string, string>();
                    string zybh = data["archive_no"].ToString();
                    zybh = zybh.Substring(9, zybh.Length - 9);
                    for (int i = 0; i < zybh.Length; i++)
                    {
                        zytz.Add("编号" + (i + 1), zybh[i].ToString());
                    }
                    zytz.Add("姓名", data["name"].ToString());
                    DataSet zytzs = DbHelperMySQL.Query($"select * from elderly_tcm_record where aichive_no='{data["archive_no"].ToString()}' order by create_time desc LIMIT 1");
                    if (zytzs != null && zytzs.Tables.Count > 0 && zytzs.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = zytzs.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            string[] zz = da.Rows[j]["answer_result"].ToString().Split('|');
                            for (int i = 0; i < zz.Length; i++)
                            {
                                zytz.Add("a" + i + zz[i].Split(':')[1], "√");
                            }
                            int qz = 0;
                            qz = Convert.ToInt32(da.Rows[j]["qixuzhi_score"]);
                            zytz.Add("气虚质得分", qz.ToString());
                            if (qz >= 11)
                            {
                                zytz.Add("气虚质是", "√");
                            }
                            else if (qz <= 9 && qz <= 10)
                            {
                                zytz.Add("气虚质倾向是", "√");
                            }
                            zytz.Add("气虚质" + da.Rows[j]["qixuzhi_result"].ToString(), "√");
                            qz = Convert.ToInt32(da.Rows[j]["yangxuzhi_score"]);
                            zytz.Add("阳虚质得分", qz.ToString());
                            if (qz >= 11)
                            {
                                zytz.Add("阳虚质是", "√");
                            }
                            else if (qz <= 9 && qz <= 10)
                            {
                                zytz.Add("阳虚质倾向是", "√");
                            }
                            zytz.Add("阳虚质" + da.Rows[j]["yangxuzhi_result"].ToString(), "√");
                            qz = Convert.ToInt32(da.Rows[j]["yinxuzhi_score"]);
                            zytz.Add("阴虚质得分", qz.ToString());
                            if (qz >= 11)
                            {
                                zytz.Add("阴虚质是", "√");
                            }
                            else if (qz <= 9 && qz <= 10)
                            {
                                zytz.Add("阴虚质倾向是", "√");
                            }
                            zytz.Add("阴虚质" + da.Rows[j]["yinxuzhi_result"].ToString(), "√");
                            qz = Convert.ToInt32(da.Rows[j]["tanshizhi_score"]);
                            zytz.Add("痰湿质得分", qz.ToString());
                            if (qz >= 11)
                            {
                                zytz.Add("痰湿质是", "√");
                            }
                            else if (qz <= 9 && qz <= 10)
                            {
                                zytz.Add("痰湿质倾向是", "√");
                            }
                            zytz.Add("痰湿质" + da.Rows[j]["tanshizhi_result"].ToString(), "√");
                            qz = Convert.ToInt32(da.Rows[j]["shirezhi_score"]);
                            zytz.Add("湿热质得分", qz.ToString());
                            if (qz >= 11)
                            {
                                zytz.Add("湿热质是", "√");
                            }
                            else if (qz <= 9 && qz <= 10)
                            {
                                zytz.Add("湿热质倾向是", "√");
                            }
                            zytz.Add("湿热质" + da.Rows[j]["shirezhi_result"].ToString(), "√");
                            qz = Convert.ToInt32(da.Rows[j]["xueyuzhi_score"]);
                            zytz.Add("血瘀质得分", qz.ToString());
                            if (qz >= 11)
                            {
                                zytz.Add("血瘀质是", "√");
                            }
                            else if (qz <= 9 && qz <= 10)
                            {
                                zytz.Add("血瘀质倾向是", "√");
                            }
                            zytz.Add("血瘀质" + da.Rows[j]["xueyuzhi_result"].ToString(), "√");
                            qz = Convert.ToInt32(da.Rows[j]["qiyuzhi_score"]);
                            zytz.Add("气郁质得分", qz.ToString());
                            if (qz >= 11)
                            {
                                zytz.Add("气郁质是", "√");
                            }
                            else if (qz <= 9 && qz <= 10)
                            {
                                zytz.Add("气郁质倾向是", "√");
                            }
                            zytz.Add("气郁质" + da.Rows[j]["qiyuzhi_result"].ToString(), "√");
                            qz = Convert.ToInt32(da.Rows[j]["tebingzhi_sorce"]);
                            zytz.Add("特禀质得分", qz.ToString());
                            if (qz >= 11)
                            {
                                zytz.Add("特禀质是", "√");
                            }
                            else if (qz <= 9 && qz <= 10)
                            {
                                zytz.Add("特禀质倾向是", "√");
                            }
                            zytz.Add("气郁质" + da.Rows[j]["tebingzhi_result"].ToString(), "√");
                            qz = Convert.ToInt32(da.Rows[j]["pinghezhi_sorce"]);
                            zytz.Add("平和质得分", qz.ToString());
                            if (qz >= 17)
                            {
                                zytz.Add("平和质是", "√");
                            }
                            else if (qz <= 10 && qz <= 16)
                            {
                                zytz.Add("平和质倾向是", "√");
                            }
                            zytz.Add("平和质" + da.Rows[j]["pinghezhi_result"].ToString(), "√");
                            string time = jkdata.Rows[j]["test_date"].ToString();
                            zytz.Add("填表日期年", time?.Split('-')[0]);
                            zytz.Add("填表日期月", time?.Split('-')[1]);
                            zytz.Add("填表日期日", time?.Split('-')[2]);
                            zytz.Add("医生签名", "");
                        }
                    }
                    //书签替换
                    foreach (var key in zytz.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(zytz[key]);
                    }
                    return doc;
                #endregion
                default:
                    break;
            }
            return doc;
        }
        private void DeteleFile(string url)
        {
            if (File.Exists(url))
            {
                File.Delete(url);
            }
        }
        private void OpenPdf(string url)
        {
            //定义一个ProcessStartInfo实例
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            //设置启动进程的初始目录
            info.WorkingDirectory = Application.StartupPath;
            //设置启动进程的应用程序或文档名
            info.FileName = url;
            //设置启动进程的参数
            info.Arguments = "";
            //启动由包含进程启动信息的进程资源
            try
            {
                System.Diagnostics.Process.Start(info);
            }
            catch (System.ComponentModel.Win32Exception we)
            {
                MessageBox.Show(this, we.Message);
                return;
            }
        }
        #endregion

        #region 图片处理
        /// <summary>
        /// 使用目录作为源调整图像大小
        /// </summary>
        /// <param name="OriginalFileLocation">图像位置</param>
        /// <param name="heigth">新高度</param>
        /// <param name="width">新宽度</param>
        /// <param name="keepAspectRatio">保持纵横比</param>
        /// <param name="getCenter">返回图像的中心位</param>
        /// <returns>具有新维度的图像</returns>
        public Image resizeImageFromFile(String OriginalFileLocation, int heigth, int width, Boolean keepAspectRatio, Boolean getCenter)
        {
            int newheigth = heigth;
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFileLocation);

            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (keepAspectRatio || getCenter)
            {
                int bmpY = 0;
                double resize = (double)FullsizeImage.Width / (double)width;//get the resize vector
                if (getCenter)
                {
                    bmpY = (int)((FullsizeImage.Height - (heigth * resize)) / 2);// gives the Y value of the part that will be cut off, to show only the part in the center
                    Rectangle section = new Rectangle(new Point(0, bmpY), new Size(FullsizeImage.Width, (int)(heigth * resize)));// create the section to cut of the original image
                                                                                                                                 //System.Console.WriteLine("the section that will be cut off: " + section.Size.ToString() + " the Y value is minimized by: " + bmpY);
                    Bitmap orImg = new Bitmap((Bitmap)FullsizeImage);//for the correct effect convert image to bitmap.
                    FullsizeImage.Dispose();//clear the original image
                    using (Bitmap tempImg = new Bitmap(section.Width, section.Height))
                    {
                        Graphics cutImg = Graphics.FromImage(tempImg);//              set the file to save the new image to.
                        cutImg.DrawImage(orImg, 0, 0, section, GraphicsUnit.Pixel);// cut the image and save it to tempImg
                        FullsizeImage = tempImg;//save the tempImg as FullsizeImage for resizing later
                        orImg.Dispose();
                        cutImg.Dispose();
                        return FullsizeImage.GetThumbnailImage(width, heigth, null, IntPtr.Zero);
                    }
                }
                else newheigth = (int)(FullsizeImage.Height / resize);//  set the new heigth of the current image
            }//return the image resized to the given heigth and width
            return FullsizeImage.GetThumbnailImage(width, newheigth, null, IntPtr.Zero);
        }


        /// <summary>
        /// 使用目录作为源调整图像大小
        /// </summary>
        /// <param name="OriginalFileLocation">图像位置</param>
        /// <param name="heigth">新高度</param>
        /// <param name="width">新宽度</param>
        /// <returns>具有新维度的图像</returns>
        public Image resizeImageFromFile(String OriginalFileLocation, int heigth, int width)
        {
            return resizeImageFromFile(OriginalFileLocation, heigth, width, false, false);
        }

        /// <summary>
        /// 使用目录作为源调整图像大小
        /// </summary>
        /// <param name="OriginalFileLocation">图像位置</param>
        /// <param name="heigth">新高度</param>
        /// <param name="width">新宽度</param>
        /// <param name="keepAspectRatio">保持纵横比</param>
        /// <returns>具有新维度的图像</returns>
        public Image resizeImageFromFile(String OriginalFileLocation, int heigth, int width, Boolean keepAspectRatio)
        {
            return resizeImageFromFile(OriginalFileLocation, heigth, width, keepAspectRatio, false);
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击button按钮事件
            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnModify" && e.RowIndex >= 0)
            {
                //说明点击的列是DataGridViewButtonColumn列
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
                string id = dataGridView1["编码", e.RowIndex].Value.ToString();
                string str = Application.StartupPath;//项目路径
                OpenPdf(@str + $"/up/result/{id}.pdf");
            }
        }
    }

    public class Report
    {
        public string Name { get; set; }
        public Document Doc { get; set; }
    }
}
