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
                    buttonColumn.HeaderText = "修改";
                    buttonColumn.DefaultCellStyle.NullValue = "修改";
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
                            switch (ctrl.Text)
                            {
                                case "化验报告单":

                                    break;
                                case "个人信息":

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                if (list.Count > 1 && list.Exists(m => m == "综合报告单"))
                {
                    MessageBox.Show("综合报告单不能和其它报告一起！");
                }
                PDF(list, dataSet, ide);
                MessageBox.Show("成功！");
            }
            catch (Exception ex)
            {

                throw;
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
                    if (re != null)
                    {
                        if (res != null)
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
                        }
                        else
                        {
                            reports.Remove(re);
                            if (reports != null && reports.Count > 0)
                            {
                                foreach (var rs in reports)
                                {
                                    re.Doc.AppendDocument(rs.Doc, ImportFormatMode.KeepSourceFormatting);
                                }
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
                switch (list[0])
                {
                    case "封面":
                        for (int i = 0; i < dataSet.Tables["个人"].Rows.Count; i++)
                        {
                            DataRow data = dataSet.Tables["个人"].Rows[i];
                            doc = PdfProcessing(list[0], data);
                            string urls = @str + $"/up/result/{data["archive_no"].ToString()}.pdf";
                            DeteleFile(urls);
                            doc.Save(urls, SaveFormat.Pdf);
                        }
                        break;
                    case "个人信息":
                        for (int i = 0; i < dataSet.Tables["个人"].Rows.Count; i++)
                        {
                            DataRow data = dataSet.Tables["个人"].Rows[i];
                            doc = PdfProcessing(list[0], data);
                            string urls = @str + $"/up/result/{data["archive_no"].ToString()}.pdf";
                            DeteleFile(urls);
                            doc.Save(urls, SaveFormat.Pdf);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private Document PdfProcessing(string lx, DataRow data)
        {
            DateTime date = DateTime.Now;
            string str = Application.StartupPath;//项目路径
            Document doc = null;
            DocumentBuilder builder = null;
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
                            dics.Add("疾病时间" + (j + 1) + "年", time.Split('-')[0]);
                            dics.Add("疾病时间" + (j + 1) + "月", time.Split('-')[1]);
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
                    DataSet jk = DbHelperMySQL.Query($"select * from physical_examination_record where aichive_no='{data["archive_no"].ToString()}' order by create_time desc LIMIT 1");
                    if (jk != null && jk.Tables.Count > 0 && jk.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = jk.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            hy.Add("条码号", da.Rows[j]["bar_code"].ToString());
                            hy.Add("身高", da.Rows[j]["base_height"].ToString());
                            hy.Add("体重", da.Rows[j]["base_weight"].ToString());
                            hy.Add("BMI", da.Rows[j]["base_bmi"].ToString());
                            hy.Add("腰围", da.Rows[j]["base_waist"].ToString());
                            hy.Add("体温", da.Rows[j]["base_temperature"].ToString());
                            hy.Add("呼吸频率", da.Rows[j]["base_respiratory"].ToString());
                            hy.Add("脉率", da.Rows[j]["base_heartbeat"].ToString());
                            hy.Add("左侧高压", da.Rows[j]["base_blood_pressure_left_high"].ToString());
                            hy.Add("左侧低压", da.Rows[j]["base_blood_pressure_left_low"].ToString());
                            hy.Add("右侧高压", da.Rows[j]["base_blood_pressure_right_high"].ToString());
                            hy.Add("右侧低压", da.Rows[j]["base_blood_pressure_right_low"].ToString());
                            hy.Add("送检日期", da.Rows[j]["check_da.Rowste"].ToString());
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

                #region 
                case "健康体检表":
                    doc = new Document(@str + $"/up/template/健康体检表.doc");
                    builder = new DocumentBuilder(doc);
                    var jktj = new Dictionary<string, string>();
                    jktj.Add("地址", data["address"].ToString());


                    //书签替换
                    foreach (var key in jktj.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(jktj[key]);
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
        #endregion


        private void button3_Click(object sender, EventArgs e)
        {
        }
    }

    public class Report
    {
        public string Name { get; set; }
        public Document Doc { get; set; }
    }
}
