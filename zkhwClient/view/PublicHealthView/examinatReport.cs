using Aspose.Words;
using MySql.Data.MySqlClient;
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
        string str = Application.StartupPath;//项目路径
        service.loginLogService lls = new service.loginLogService();
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
            string sql = $@"SELECT count(sex) sun,sex
from zkhw_tj_bgdc where area_duns like '%{basicInfoSettings.xcuncode}%' and createtime>='{basicInfoSettings.createtime}'
GROUP BY sex
";
            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable data = dataSet.Tables[0];
            if (data != null && data.Rows.Count > 0)
            {
                DataRow[] rows = data.Select("sex='女'");
                if (rows.Length > 0)
                {
                    女.Text = rows[0]["sun"].ToString();
                }
                DataRow[] rowsn = data.Select("sex='男'");
                if (rowsn.Length > 0)
                {
                    男.Text = rowsn[0]["sun"].ToString();
                }
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
(case base.sex when '1'then '男' when '2' then '女' when '9' then '未说明的性别' when '0' then '未知的性别' ELSE ''
END)性别,
base.id_number 身份证号,
base.upload_status 是否同步,
bgdc.BaoGaoShengChan 报告生成时间
from resident_base_info base
join 
(select * from zkhw_tj_bgdc group by aichive_no order by createtime desc) bgdc
on base.archive_no=bgdc.aichive_no
where base.village_code='{basicInfoSettings.xcuncode}' and bgdc.createtime>='{basicInfoSettings.createtime}'";//base.village_code='{basicInfoSettings.xcuncode}' and base.create_time>='{basicInfoSettings.createtime}'
            if (pairs != null && pairs.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(pairs["timesta"]) && !string.IsNullOrWhiteSpace(pairs["timeend"]))
                {
                    sql += $" and date_format(base.create_time,'%Y-%m-%d') between '{pairs["timesta"]}' and '{pairs["timeend"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["juming"]))
                {
                    sql += $" or base.name like '%{pairs["juming"]}%' or base.id_number like '%{pairs["juming"]}%'";
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
            sql += $@" limit {pagesize}; select found_rows()";// and base.id >=( select id From zkhw_tj_bgdc Order By id limit {pageindex},1)
            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable dt = dataSet.Tables[0];
            count = Convert.ToInt32(dataSet.Tables[1].Rows[0][0]);
            return dt;
        }
        //(select * from (select * from zkhw_tj_bgdc order by createtime desc) as a group by aichive_no order by createtime desc) bgdc
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
            from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and createtime>='{basicInfoSettings.createtime}' and age >= '0' and age<= '64' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
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
            from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and createtime>='{basicInfoSettings.createtime}' and age >= '65' and age<= '70' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
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
            from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and createtime>='{basicInfoSettings.createtime}' and age >= '70' and age<= '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
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
            from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and createtime>='{basicInfoSettings.createtime}' and age >= '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
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
                    return;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                    {
                        string id = dataGridView1["编码", i].Value.ToString();
                        ide.Add("'" + id + "'");
                    }
                }
                if (ide.Count < 1)
                {
                    MessageBox.Show("请选择要导出报告的人员!"); return;
                }
                string sql = string.Empty;
                sql = $@"select * from resident_base_info base where base.archive_no in({string.Join(",", ide)})";
                DataSet datas = DbHelperMySQL.Query(sql);
                if (datas != null && datas.Tables.Count > 0)
                {
                    ge = datas.Tables[0].Copy();
                    ge.TableName = "个人";
                    dataSet.Tables.Add(ge);
                }

                bool istue = PDF(list, dataSet, ide);
                if (istue)
                {
                    DialogResult dr = MessageBox.Show("成功！是否打开文件夹",
                                     "提示",
                                     MessageBoxButtons.OKCancel,
                                     MessageBoxIcon.Warning);
                    if (dr == DialogResult.OK)
                    {
                        OpenPdf(@str + $"/up/result/");
                    }
                }
            }
            catch (Exception ex)
            {
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "报告导出异常！" + ex.Message;
                lb.type = "2";
                lls.addCheckLog(lb);
                MessageBox.Show("错误请联系管理员！11" + ex.StackTrace);
            }
        }

        private bool PDF(List<string> list, DataSet dataSet, List<string> ide)
        {
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
                return true;
            }
            else
            {
                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("请选择你要生成的报告类型！");
                    return false;
                }
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
                        re.Doc.Save(urls, SaveFormat.Pdf);
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
                return true;
            }
        }

        private Document PdfProcessing(string lx, DataRow data)
        {
            DateTime date = DateTime.Now;
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
                    break;
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
                    break;
                #endregion

                #region 化验报告单
                case "化验报告单":
                    doc = new Document(@str + $"/up/template/化验报告单.doc");
                    builder = new DocumentBuilder(doc);
                    var hy = new Dictionary<string, string>();
                    hy.Add("地址", data["address"].ToString());
                    hy.Add("姓名", data["name"].ToString());
                    hy.Add("姓名1", data["name"].ToString());
                    hy.Add("性别", Sex(data["sex"].ToString()));
                    hy.Add("性别1", Sex(data["sex"].ToString()));
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

                    DataSet sh = DbHelperMySQL.Query($"select * from zkhw_tj_sh where aichive_no='{data["archive_no"].ToString()}' order by createtime desc LIMIT 1");
                    if (sh != null && sh.Tables.Count > 0 && sh.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = sh.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            string alb = da.Rows[j]["ALB"].ToString();
                            if (alb != null && !"".Equals(alb))
                            {
                                hy.Add("白蛋白箭头", Convert.ToDouble(da.Rows[j]["ALB"].ToString()) > 54 ? "↑" : "↓");
                                hy.Add("白蛋白结果", da.Rows[j]["ALB"].ToString());
                            }
                            string alp = da.Rows[j]["ALP"].ToString();
                            if (alp != null && !"".Equals(alp))
                            {
                                hy.Add("碱性磷酸酶箭头", Convert.ToDouble(alp) > 100 ? "↑" : "↓");
                                hy.Add("碱性磷酸酶结果", da.Rows[j]["ALP"].ToString());
                            }
                            hy.Add("谷丙转氨酶箭头", Convert.ToDouble(da.Rows[j]["ALT"].ToString()) > 40 ? "↑" : "↓");
                            hy.Add("谷丙转氨酶结果", da.Rows[j]["ALT"].ToString());
                            hy.Add("谷草转氨酶箭头", Convert.ToDouble(da.Rows[j]["AST"].ToString()) > 40 ? "↑" : "↓");
                            hy.Add("谷草转氨酶结果", da.Rows[j]["AST"].ToString());
                            hy.Add("胆固醇箭头", Convert.ToDouble(da.Rows[j]["CHO"].ToString()) > 5 ? "↑" : "↓");
                            hy.Add("胆固醇结果", da.Rows[j]["CHO"].ToString());
                            hy.Add("肌酐箭头", Convert.ToDouble(da.Rows[j]["CREA"].ToString()) > 104 ? "↑" : "↓");
                            hy.Add("肌酐结果", da.Rows[j]["CREA"].ToString());
                            string dbil = da.Rows[j]["DBIL"].ToString();
                            if (dbil != null && !"".Equals(dbil))
                            {
                                hy.Add("直接胆红素箭头", Convert.ToDouble(dbil) > 7 ? "↑" : "↓");
                                hy.Add("直接胆红素结果", da.Rows[j]["DBIL"].ToString());
                            }
                            string ggt = da.Rows[j]["GGT"].ToString();
                            if (ggt != null && !"".Equals(ggt))
                            {
                                hy.Add("谷氨酰氨基箭头", Convert.ToDouble(ggt) > 50 ? "↑" : "↓");
                                hy.Add("谷氨酰氨基结果", da.Rows[j]["GGT"].ToString());
                            }
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
                            string tp = da.Rows[j]["TP"].ToString();
                            if (tp != null && !"".Equals(tp))
                            {
                                hy.Add("总蛋白箭头", Convert.ToDouble(tp) > 19 ? "↑" : "↓");
                                hy.Add("总蛋白结果", da.Rows[j]["TP"].ToString());
                            }
                            string ua = da.Rows[j]["UA"].ToString();
                            if (ua != null && !"".Equals(ua))
                            {
                                hy.Add("尿酸箭头", Convert.ToDouble(ua) > 2 ? "↑" : "↓");
                                hy.Add("尿酸结果", da.Rows[j]["UA"].ToString());
                            }
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
                    break;
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
                            jktj.Add("老年人认知能力得分", jkdata.Rows[j]["base_cognition_score"].ToString());
                            jktj.Add("老年人情感状态得分", jkdata.Rows[j]["base_feeling_score"].ToString());
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
                    break;
                #endregion

                #region 心电图
                case "心电图":
                    doc = new Document(@str + $"/up/template/心电图.doc");
                    builder = new DocumentBuilder(doc);
                    var xdt = new Dictionary<string, string>();
                    xdt.Add("地址", data["address"].ToString());
                    xdt.Add("姓名", data["name"].ToString());
                    xdt.Add("性别", Sex(data["sex"].ToString()));
                    xdt.Add("生日", data["birthday"].ToString());
                    xdt.Add("身份证号", data["id_number"].ToString());
                    DataSet xdts = DbHelperMySQL.Query($"select * from zkhw_tj_xdt where aichive_no='{data["archive_no"].ToString()}' order by createtime desc LIMIT 1");
                    if (xdts != null && xdts.Tables.Count > 0 && xdts.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = xdts.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            builder.MoveToBookmark("图片");
                            string imageUrl = da.Rows[j]["imageUrl"].ToString();
                            if (imageUrl != null && !"".Equals(imageUrl) && File.Exists(@str + "/xdtImg/" + imageUrl))
                            {
                                builder.InsertImage(resizeImageFromFile(@str + "/xdtImg/" + imageUrl, 678, 960));
                            }
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
                    break;
                #endregion

                #region B超
                case "B超":
                    doc = new Document(@str + $"/up/template/B超.doc");
                    builder = new DocumentBuilder(doc);
                    var bc = new Dictionary<string, string>();
                    bc.Add("地址", data["address"].ToString());
                    bc.Add("姓名", data["name"].ToString());
                    bc.Add("性别", Sex(data["sex"].ToString()));
                    bc.Add("生日", data["birthday"].ToString());
                    bc.Add("身份证号", data["id_number"].ToString());
                    DataSet bcs = DbHelperMySQL.Query($"select * from zkhw_tj_bc where aichive_no='{data["archive_no"].ToString()}' order by createtime desc LIMIT 1");
                    if (bcs != null && bcs.Tables.Count > 0 && bcs.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = bcs.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            string imageUrla = da.Rows[j]["BuPic01"].ToString();
                            if (imageUrla != null && !"".Equals(imageUrla) && File.Exists(@str + "/bcImg/" + imageUrla))
                            {
                                builder.MoveToBookmark("图片1");
                                builder.InsertImage(resizeImageFromFile(@str + "/bcImg/" + imageUrla, 768, 1024));
                            }
                            string imageUrlb = da.Rows[j]["BuPic02"].ToString();
                            if (imageUrlb != null && !"".Equals(imageUrlb) && File.Exists(@str + "/bcImg/" + imageUrla))
                            {
                                builder.MoveToBookmark("图片2");
                                builder.InsertImage(resizeImageFromFile(@str + "/bcImg/" + imageUrlb, 768, 1024));
                            }
                            string imageUrlc = da.Rows[j]["BuPic03"].ToString();
                            if (imageUrlc != null && !"".Equals(imageUrlc) && File.Exists(@str + "/bcImg/" + imageUrla))
                            {
                                builder.MoveToBookmark("图片3");
                                builder.InsertImage(resizeImageFromFile(@str + "/bcImg/" + imageUrlc, 768, 1024));
                            }
                            string imageUrld = da.Rows[j]["BuPic04"].ToString();
                            if (imageUrld != null && !"".Equals(imageUrld) && File.Exists(@str + "/bcImg/" + imageUrla))
                            {
                                builder.MoveToBookmark("图片4");
                                builder.InsertImage(resizeImageFromFile(@str + "/bcImg/" + imageUrld, 768, 1024));
                            }
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
                    break;
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
                    break;
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
                    break;
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
                    break;
                #endregion
                default:
                    break;
            }
            string sql = $@"UPDATE zkhw_tj_bgdc set BaoGaoShengChan='{DateTime.Now.ToString("yyyy-MM-dd")}' where aichive_no='{data["archive_no"].ToString()}'";
            int rue = DbHelperMySQL.ExecuteSql(sql);
            return doc;
        }
        private string Sex(string sex)
        {
            switch (sex)
            {
                case "1":
                    return "男";
                case "2":
                    return "女";
                case "9":
                    return "未说明的性别";
                case "0":
                    return "未知的性别";
                default:
                    break;
            }
            return "";
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
            info.WorkingDirectory = str;
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

        #region 数据上传
        /// <summary>
        /// 数据上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> sqllist = new List<string>();
                List<string> sqllistz = new List<string>();
                string infoid = string.Empty;
                string recordid = string.Empty;
                string estimateid = string.Empty;
                string followid = string.Empty;
                string fuvid = string.Empty;
                string diabetesid = string.Empty;
                string ncgid = string.Empty;
                string sgtzid = string.Empty;
                string shid = string.Empty;
                string xcgid = string.Empty;
                string xdtid = string.Empty;
                string xyid = string.Empty;
                string bcid = string.Empty;

                #region 个人信息
                DataSet info = DbHelperMySQL.Query($@"select * from resident_base_info where upload_status='0'");
                if (info != null && info.Tables.Count > 0 && info.Tables[0].Rows.Count > 0)
                {
                    DataTable data = info.Tables[0];
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into resident_base_info (id,archive_no,pb_archive,name,sex,birthday,id_number,card_pic,company,phone,link_name,link_phone,resident_type,register_address,residence_address,nation,blood_group,blood_rh,education,profession,marital_status,pay_type,pay_other,drug_allergy,allergy_other,exposure,disease_other,is_hypertension,is_diabetes,is_psychosis,is_tuberculosis,is_heredity,heredity_name,is_deformity,deformity_name,is_poor,kitchen,fuel,other_fuel,drink,other_drink,toilet,poultry,medical_code,photo_code,aichive_org,doctor_name,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,status,remark,create_user,create_name,create_time,create_org,create_org_name
) values('{data.Rows[i]["id"]}','{data.Rows[i]["archive_no"]}','{data.Rows[i]["pb_archive"]}','{data.Rows[i]["name"]}','{data.Rows[i]["sex"]}','{data.Rows[i]["birthday"]}','{data.Rows[i]["id_number"]}','{data.Rows[i]["card_pic"]}','{data.Rows[i]["company"]}','{data.Rows[i]["phone"]}','{data.Rows[i]["link_name"]}','{data.Rows[i]["link_phone"]}','{data.Rows[i]["resident_type"]}','{data.Rows[i]["address"]}','{data.Rows[i]["residence_address"]}','{data.Rows[i]["nation"]}','{data.Rows[i]["blood_group"]}','{data.Rows[i]["blood_rh"]}','{data.Rows[i]["education"]}','{data.Rows[i]["profession"]}','{data.Rows[i]["marital_status"]}','{data.Rows[i]["pay_type"]}','{data.Rows[i]["pay_other"]}','{data.Rows[i]["drug_allergy"]}','{data.Rows[i]["allergy_other"]}','{data.Rows[i]["exposure"]}','{data.Rows[i]["disease_other"]}','{data.Rows[i]["is_hypertension"]}','{data.Rows[i]["is_diabetes"]}','{data.Rows[i]["is_psychosis"]}','{data.Rows[i]["is_tuberculosis"]}','{data.Rows[i]["is_heredity"]}','{data.Rows[i]["heredity_name"]}','{data.Rows[i]["is_deformity"]}','{data.Rows[i]["deformity_name"]}',{data.Rows[i]["is_poor"]},'{data.Rows[i]["kitchen"]}','{data.Rows[i]["fuel"]}','{data.Rows[i]["other_fuel"]}','{data.Rows[i]["drink"]}','{data.Rows[i]["other_drink"]}','{data.Rows[i]["toilet"]}','{data.Rows[i]["poultry"]}','{data.Rows[i]["medical_code"]}','{data.Rows[i]["photo_code"]}','{data.Rows[i]["aichive_org"]}','{data.Rows[i]["doctor_name"]}','{data.Rows[i]["province_code"]}','{data.Rows[i]["province_name"]}','{data.Rows[i]["city_code"]}','{data.Rows[i]["city_name"]}','{data.Rows[i]["county_code"]}','{data.Rows[i]["county_name"]}','{data.Rows[i]["towns_code"]}','{data.Rows[i]["towns_name"]}','{data.Rows[i]["village_code"]}','{data.Rows[i]["village_name"]}','{data.Rows[i]["status"]}','{data.Rows[i]["remark"]}','{data.Rows[i]["create_user"]}','{data.Rows[i]["create_name"]}','{Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")}','{data.Rows[i]["create_org"]}','{data.Rows[i]["create_org_name"]}');");
                        infoid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 健康体检
                DataSet record = DbHelperMySQL.Query($@"select * from physical_examination_record where upload_status='0'");
                if (record != null && record.Tables.Count > 0 && record.Tables[0].Rows.Count > 0)
                {
                    DataTable data = record.Tables[0];
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into physical_examination_record (id,name,archive_no,id_number,batch_no,bar_code,symptom,symptom_other,check_date,base_temperature,base_heartbeat,base_respiratory,base_blood_pressure_left_high,base_blood_pressure_left_low,base_blood_pressure_right_high,base_blood_pressure_right_low,base_height,base_weight,base_waist,base_bmi,base_health_estimate,base_selfcare_estimate,base_cognition_estimate,base_cognition_score,base_feeling_estimate,base_feeling_score,base_doctor,lifeway_exercise_frequency,lifeway_exercise_time,lifeway_exercise_year,lifeway_exercise_type,lifeway_diet,lifeway_smoke_status,lifeway_smoke_number,lifeway_smoke_startage,lifeway_smoke_endage,lifeway_drink_status,lifeway_drink_number,lifeway_drink_stop,lifeway_drink_stopage,lifeway_drink_startage,lifeway_drink_oneyear,lifeway_drink_type,lifeway_drink_other,lifeway_occupational_disease,lifeway_job,lifeway_job_period,lifeway_hazardous_dust,lifeway_dust_preventive,lifeway_hazardous_radiation,lifeway_radiation_preventive,lifeway_hazardous_physical,lifeway_physical_preventive,lifeway_hazardous_chemical,lifeway_chemical_preventive,lifeway_hazardous_other,lifeway_other_preventive,lifeway_doctor,organ_lips,organ_tooth,organ_hypodontia,organ_hypodontia_topleft,organ_hypodontia_topright,organ_hypodontia_bottomleft,organ_hypodontia_bottomright,organ_caries,organ_caries_topleft,organ_caries_topright,organ_caries_bottomleft,organ_caries_bottomright,organ_denture,organ_denture_topleft,organ_denture_topright,organ_denture_bottomleft,organ_denture_bottomright,organ_guttur,organ_vision_left,organ_vision_right,organ_correctedvision_left,organ_correctedvision_right,organ_hearing,organ_movement,organ_doctor,examination_eye,examination_eye_other,examination_skin,examination_skin_other,examination_sclera,examination_sclera_other,examination_lymph,examination_lymph_other,examination_barrel_chest,examination_breath_sounds,examination_breath_other,examination_rale,examination_rale_other,examination_heart_rate,examination_heart_rhythm,examination_heart_noise,examination_noise_other,examination_abdomen_tenderness,examination_tenderness_memo,examination_abdomen_mass,examination_mass_memo,examination_abdomen_hepatomegaly,examination_hepatomegaly_memo,examination_abdomen_splenomegaly,examination_splenomegaly_memo,examination_abdomen_shiftingdullness,examination_shiftingdullness_memo,examination_lowerextremity_edema,examination_dorsal_artery,examination_anus,examination_anus_other,examination_breast,examination_breast_other,examination_doctor,examination_woman_vulva,examination_vulva_memo,examination_woman_vagina,examination_vagina_memo,examination_woman_cervix,examination_cervix_memo,examination_woman_corpus,examination_corpus_memo,examination_woman_accessories,examination_accessories_memo,examination_woman_doctor,examination_other,blood_hemoglobin,blood_leukocyte,blood_platelet,blood_other,urine_protein,glycosuria,urine_acetone_bodies,bld,urine_other,blood_glucose_mmol,blood_glucose_mg,cardiogram,cardiogram_memo,cardiogram_img,microalbuminuria,fob,glycosylated_hemoglobin,hb,sgft,ast,albumin,total_bilirubin,conjugated_bilirubin,scr,blood_urea,blood_k,blood_na,tc,tg,ldl,hdl,chest_x,x_memo,chestx_img,ultrasound_abdomen,ultrasound_memo,abdomenB_img,other_b,otherb_memo,otherb_img,cervical_smear,cervical_smear_memo,other,cerebrovascular_disease,cerebrovascular_disease_other,kidney_disease,kidney_disease_other,heart_disease,heart_disease_other,vascular_disease,vascular_disease_other,ocular_diseases,ocular_diseases_other,nervous_system_disease,nervous_disease_memo,other_disease,other_disease_memo,health_evaluation,abnormal1,abnormal2,abnormal3,abnormal4,health_guidance,danger_controlling,target_weight,advise_bacterin,danger_controlling_other,create_user,create_name,create_org,create_org_name,create_time)
values('{data.Rows[i]["id"]}','{data.Rows[i]["name"]}','{data.Rows[i]["aichive_no"]}','{data.Rows[i]["id_number"]}','{data.Rows[i]["batch_no"]}','{data.Rows[i]["bar_code"]}','{data.Rows[i]["symptom"]}','{data.Rows[i]["symptom_other"]}','{data.Rows[i]["check_date"]}','{data.Rows[i]["base_temperature"]}','{data.Rows[i]["base_heartbeat"]}','{data.Rows[i]["base_respiratory"]}',{Ifnull(data.Rows[i]["base_blood_pressure_left_high"])},{Ifnull(data.Rows[i]["base_blood_pressure_left_low"])},{Ifnull(data.Rows[i]["base_blood_pressure_right_high"])},{Ifnull(data.Rows[i]["base_blood_pressure_right_low"])},'{data.Rows[i]["base_height"]}','{data.Rows[i]["base_weight"]}','{data.Rows[i]["base_waist"]}','{data.Rows[i]["base_bmi"]}','{data.Rows[i]["base_health_estimate"]}','{data.Rows[i]["base_selfcare_estimate"]}','{data.Rows[i]["base_cognition_estimate"]}','{data.Rows[i]["base_cognition_score"]}','{data.Rows[i]["base_feeling_estimate"]}','{data.Rows[i]["base_feeling_score"]}','{data.Rows[i]["base_doctor"]}','{data.Rows[i]["lifeway_exercise_frequency"]}','{data.Rows[i]["lifeway_exercise_time"]}','{data.Rows[i]["lifeway_exercise_year"]}','{data.Rows[i]["lifeway_exercise_type"]}','{data.Rows[i]["lifeway_diet"]}','{data.Rows[i]["lifeway_smoke_status"]}','{data.Rows[i]["lifeway_smoke_number"]}','{data.Rows[i]["lifeway_smoke_startage"]}','{data.Rows[i]["lifeway_smoke_endage"]}','{data.Rows[i]["lifeway_drink_status"]}','{data.Rows[i]["lifeway_drink_number"]}','{data.Rows[i]["lifeway_drink_stop"]}','{data.Rows[i]["lifeway_drink_stopage"]}','{data.Rows[i]["lifeway_drink_startage"]}','{data.Rows[i]["lifeway_drink_oneyear"]}','{data.Rows[i]["lifeway_drink_type"]}','{data.Rows[i]["lifeway_drink_other"]}','{data.Rows[i]["lifeway_occupational_disease"]}','{data.Rows[i]["lifeway_job"]}','{data.Rows[i]["lifeway_job_period"]}','{data.Rows[i]["lifeway_hazardous_dust"]}','{data.Rows[i]["lifeway_dust_preventive"]}','{data.Rows[i]["lifeway_hazardous_radiation"]}','{data.Rows[i]["lifeway_radiation_preventive"]}','{data.Rows[i]["lifeway_hazardous_physical"]}','{data.Rows[i]["lifeway_physical_preventive"]}','{data.Rows[i]["lifeway_hazardous_chemical"]}','{data.Rows[i]["lifeway_chemical_preventive"]}','{data.Rows[i]["lifeway_hazardous_other"]}','{data.Rows[i]["lifeway_other_preventive"]}','{data.Rows[i]["lifeway_doctor"]}','{data.Rows[i]["organ_lips"]}','{data.Rows[i]["organ_tooth"]}','{data.Rows[i]["organ_hypodontia"]}','{data.Rows[i]["organ_hypodontia_topleft"]}','{data.Rows[i]["organ_hypodontia_topright"]}','{data.Rows[i]["organ_hypodontia_bottomleft"]}','{data.Rows[i]["organ_hypodontia_bottomright"]}','{data.Rows[i]["organ_caries"]}','{data.Rows[i]["organ_caries_topleft"]}','{data.Rows[i]["organ_caries_topright"]}','{data.Rows[i]["organ_caries_bottomleft"]}','{data.Rows[i]["organ_caries_bottomright"]}','{data.Rows[i]["organ_denture"]}','{data.Rows[i]["organ_denture_topleft"]}','{data.Rows[i]["organ_denture_topright"]}','{data.Rows[i]["organ_denture_bottomleft"]}','{data.Rows[i]["organ_denture_bottomright"]}','{data.Rows[i]["organ_guttur"]}','{data.Rows[i]["organ_vision_left"]}','{data.Rows[i]["organ_vision_right"]}','{data.Rows[i]["organ_correctedvision_left"]}','{data.Rows[i]["organ_correctedvision_right"]}','{data.Rows[i]["organ_hearing"]}','{data.Rows[i]["organ_movement"]}','{data.Rows[i]["organ_doctor"]}','{data.Rows[i]["examination_eye"]}','{data.Rows[i]["examination_eye_other"]}','{data.Rows[i]["examination_skin"]}','{data.Rows[i]["examination_skin_other"]}','{data.Rows[i]["examination_sclera"]}','{data.Rows[i]["examination_sclera_other"]}','{data.Rows[i]["examination_lymph"]}','{data.Rows[i]["examination_lymph_other"]}','{data.Rows[i]["examination_barrel_chest"]}','{data.Rows[i]["examination_breath_sounds"]}','{data.Rows[i]["examination_breath_other"]}','{data.Rows[i]["examination_rale"]}','{data.Rows[i]["examination_rale_other"]}','{data.Rows[i]["examination_heart_rate"]}','{data.Rows[i]["examination_heart_rhythm"]}','{data.Rows[i]["examination_heart_noise"]}','{data.Rows[i]["examination_noise_other"]}','{data.Rows[i]["examination_abdomen_tenderness"]}','{data.Rows[i]["examination_tenderness_memo"]}','{data.Rows[i]["examination_abdomen_mass"]}','{data.Rows[i]["examination_mass_memo"]}','{data.Rows[i]["examination_abdomen_hepatomegaly"]}','{data.Rows[i]["examination_hepatomegaly_memo"]}','{data.Rows[i]["examination_abdomen_splenomegaly"]}','{data.Rows[i]["examination_splenomegaly_memo"]}','{data.Rows[i]["examination_abdomen_shiftingdullness"]}','{data.Rows[i]["examination_shiftingdullness_memo"]}','{data.Rows[i]["examination_lowerextremity_edema"]}','{data.Rows[i]["examination_dorsal_artery"]}','{data.Rows[i]["examination_anus"]}','{data.Rows[i]["examination_anus_other"]}','{data.Rows[i]["examination_breast"]}','{data.Rows[i]["examination_breast_other"]}','{data.Rows[i]["examination_doctor"]}','{data.Rows[i]["examination_woman_vulva"]}','{data.Rows[i]["examination_vulva_memo"]}','{data.Rows[i]["examination_woman_vagina"]}','{data.Rows[i]["examination_vagina_memo"]}','{data.Rows[i]["examination_woman_cervix"]}','{data.Rows[i]["examination_cervix_memo"]}','{data.Rows[i]["examination_woman_corpus"]}','{data.Rows[i]["examination_corpus_memo"]}','{data.Rows[i]["examination_woman_accessories"]}','{data.Rows[i]["examination_accessories_memo"]}','{data.Rows[i]["examination_woman_doctor"]}','{data.Rows[i]["examination_other"]}','{data.Rows[i]["blood_hemoglobin"]}','{data.Rows[i]["blood_leukocyte"]}','{data.Rows[i]["blood_platelet"]}','{data.Rows[i]["blood_other"]}','{data.Rows[i]["urine_protein"]}','{data.Rows[i]["glycosuria"]}','{data.Rows[i]["urine_acetone_bodies"]}','{data.Rows[i]["bld"]}','{data.Rows[i]["urine_other"]}','{data.Rows[i]["blood_glucose_mmol"]}','{data.Rows[i]["blood_glucose_mg"]}','{data.Rows[i]["cardiogram"]}','{data.Rows[i]["cardiogram_memo"]}','{data.Rows[i]["cardiogram_img"]}','{data.Rows[i]["microalbuminuria"]}','{data.Rows[i]["fob"]}','{data.Rows[i]["glycosylated_hemoglobin"]}','{data.Rows[i]["hb"]}','{data.Rows[i]["sgft"]}','{data.Rows[i]["ast"]}','{data.Rows[i]["albumin"]}','{data.Rows[i]["total_bilirubin"]}','{data.Rows[i]["conjugated_bilirubin"]}','{data.Rows[i]["scr"]}','{data.Rows[i]["blood_urea"]}','{data.Rows[i]["blood_k"]}','{data.Rows[i]["blood_na"]}','{data.Rows[i]["tc"]}','{data.Rows[i]["tg"]}','{data.Rows[i]["ldl"]}','{data.Rows[i]["hdl"]}','{data.Rows[i]["chest_x"]}','{data.Rows[i]["chestx_memo"]}','{data.Rows[i]["chestx_img"]}','{data.Rows[i]["ultrasound_abdomen"]}','{data.Rows[i]["ultrasound_memo"]}','{data.Rows[i]["abdomenB_img"]}','{data.Rows[i]["other_b"]}','{data.Rows[i]["otherb_memo"]}','{data.Rows[i]["otherb_img"]}','{data.Rows[i]["cervical_smear"]}','{data.Rows[i]["cervical_smear_memo"]}','{data.Rows[i]["other"]}','{data.Rows[i]["cerebrovascular_disease"]}','{data.Rows[i]["cerebrovascular_disease_other"]}','{data.Rows[i]["kidney_disease"]}','{data.Rows[i]["kidney_disease_other"]}','{data.Rows[i]["heart_disease"]}','{data.Rows[i]["heart_disease_other"]}','{data.Rows[i]["vascular_disease"]}','{data.Rows[i]["vascular_disease_other"]}','{data.Rows[i]["ocular_diseases"]}','{data.Rows[i]["ocular_diseases_other"]}','{data.Rows[i]["nervous_system_disease"]}','{data.Rows[i]["nervous_disease_memo"]}','{data.Rows[i]["other_disease"]}','{data.Rows[i]["other_disease_memo"]}','{data.Rows[i]["health_evaluation"]}','{data.Rows[i]["abnormal1"]}','{data.Rows[i]["abnormal2"]}','{data.Rows[i]["abnormal3"]}','{data.Rows[i]["abnormal4"]}','{data.Rows[i]["health_guidance"]}','{data.Rows[i]["danger_controlling"]}','{data.Rows[i]["target_weight"]}','{data.Rows[i]["advise_bacterin"]}','{data.Rows[i]["danger_controlling_other"]}','{data.Rows[i]["create_user"]}','{data.Rows[i]["create_name"]}','{data.Rows[i]["create_org"]}','{data.Rows[i]["create_org_name"]}','{Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")}');");
                        recordid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 老年人健康服务
                DataSet estimate = DbHelperMySQL.Query($@"select * from elderly_selfcare_estimate where upload_status='0'");
                if (estimate != null && estimate.Tables.Count > 0 && estimate.Tables[0].Rows.Count > 0)
                {
                    DataTable data = estimate.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into elderly_selfcare_estimate (id,name,archive_no,id_number,test_date,answer_result,total_score,judgement_result,test_doctor,create_user,create_name,create_org,create_org_name,create_time
                ) values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["test_date"])},{Ifnull(data.Rows[i]["answer_result"])},{Ifnull(data.Rows[i]["total_score"])},{Ifnull(data.Rows[i]["judgement_result"])},{Ifnull(data.Rows[i]["test_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        estimateid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 高血压
                DataSet follow = DbHelperMySQL.Query($@"select * from follow_medicine_record where upload_status='0'");
                if (follow != null && follow.Tables.Count > 0 && follow.Tables[0].Rows.Count > 0)
                {
                    DataTable data = follow.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into follow_medicine_record (id,follow_id,drug_name,num,dosage,create_user,create_name,create_time

                ) values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["follow_id"])},{Ifnull(data.Rows[i]["drug_name"])},{Ifnull(data.Rows[i]["num"])},{Ifnull(data.Rows[i]["dosage"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        followid += $"'{data.Rows[i]["id"]}',";
                    }
                }

                DataSet fuv = DbHelperMySQL.Query($@"select * from fuv_hypertension where upload_status='0'");
                if (fuv != null && fuv.Tables.Count > 0 && fuv.Tables[0].Rows.Count > 0)
                {
                    DataTable data = fuv.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into fuv_hypertension (id,name,archive_no,Codebar,id_number,visit_date,visit_type,symptom,other_symptom,sbp,dbp,weight,target_weight,bmi,target_bmi,heart_rate,other_sign,smoken,target_somken,wine,target_wine,sport_week,sport_once,target_sport_week,target_sport_once,salt_intake,target_salt_intake,mind_adjust,doctor_obey,assist_examine,drug_obey,untoward_effect,untoward_effect_drug,visit_class,referral_code,next_visit_date,visit_doctor,advice,create_name,create_time,transfer_organ,transfer_reason
) values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["Codebar"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["visit_type"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["other_symptom"])},{Ifnull(data.Rows[i]["sbp"])},{Ifnull(data.Rows[i]["dbp"])},{Ifnull(data.Rows[i]["weight"])},{Ifnull(data.Rows[i]["target_weight"])},{Ifnull(data.Rows[i]["bmi"])},{Ifnull(data.Rows[i]["target_bmi"])},{Ifnull(data.Rows[i]["heart_rate"])},{Ifnull(data.Rows[i]["other_sign"])},{Ifnull(data.Rows[i]["smoken"])},{Ifnull(data.Rows[i]["target_somken"])},{Ifnull(data.Rows[i]["wine"])},{Ifnull(data.Rows[i]["target_wine"])},{Ifnull(data.Rows[i]["sport_week"])},{Ifnull(data.Rows[i]["sport_once"])},{Ifnull(data.Rows[i]["target_sport_week"])},{Ifnull(data.Rows[i]["target_sport_once"])},{Ifnull(data.Rows[i]["salt_intake"])},{Ifnull(data.Rows[i]["target_salt_intake"])},{Ifnull(data.Rows[i]["mind_adjust"])},{Ifnull(data.Rows[i]["doctor_obey"])},{Ifnull(data.Rows[i]["assist_examine"])},{Ifnull(data.Rows[i]["drug_obey"])},{Ifnull(data.Rows[i]["untoward_effect"])},{Ifnull(data.Rows[i]["untoward_effect_drug"])},{Ifnull(data.Rows[i]["visit_class"])},{Ifnull(data.Rows[i]["referral_code"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["advice"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["transfer_organ"])},{Ifnull(data.Rows[i]["transfer_reason"])});");
                        fuvid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 糖尿病
                DataSet diabetes = DbHelperMySQL.Query($@"select * from diabetes_follow_record where upload_status='0'");
                if (diabetes != null && diabetes.Tables.Count > 0 && diabetes.Tables[0].Rows.Count > 0)
                {
                    DataTable data = diabetes.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into diabetes_follow_record (id,name,archive_no,id_number,visit_date,visit_type,symptom,symptom_other,blood_pressure_high,blood_pressure_low,weight_now,weight_next,bmi_now,bmi_next,dorsal_artery,other,smoke_now,smoke_next,drink_now,drink_next,sports_num_now,sports_time_now,sports_num_next,sports_time_next,staple_food_now,staple_food_next,psychological_recovery,medical_compliance,blood_glucose,glycosylated_hemoglobin,check_date,compliance,untoward_effect,reactive_hypoglycemia,follow_type,insulin_name,insulin_usage,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time
) values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["visit_type"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["symptom_other"])},{Ifnull(data.Rows[i]["blood_pressure_high"])},{Ifnull(data.Rows[i]["blood_pressure_low"])},{Ifnull(data.Rows[i]["weight_now"])},{Ifnull(data.Rows[i]["weight_next"])},{Ifnull(data.Rows[i]["bmi_now"])},{Ifnull(data.Rows[i]["bmi_next"])},{Ifnull(data.Rows[i]["dorsal_artery"])},{Ifnull(data.Rows[i]["other"])},{Ifnull(data.Rows[i]["smoke_now"])},{Ifnull(data.Rows[i]["smoke_next"])},{Ifnull(data.Rows[i]["drink_now"])},{Ifnull(data.Rows[i]["drink_next"])},{Ifnull(data.Rows[i]["sports_num_now"])},{Ifnull(data.Rows[i]["sports_time_now"])},{Ifnull(data.Rows[i]["sports_num_next"])},{Ifnull(data.Rows[i]["sports_time_next"])},{Ifnull(data.Rows[i]["staple_food_now"])},{Ifnull(data.Rows[i]["staple_food_next"])},{Ifnull(data.Rows[i]["psychological_recovery"])},{Ifnull(data.Rows[i]["medical_compliance"])},{Ifnull(data.Rows[i]["blood_glucose"])},{Ifnull(data.Rows[i]["glycosylated_hemoglobin"])},{Ifnull(data.Rows[i]["check_date"])},{Ifnull(data.Rows[i]["compliance"])},{Ifnull(data.Rows[i]["untoward_effect"])},{Ifnull(data.Rows[i]["reactive_hypoglycemia"])},{Ifnull(data.Rows[i]["follow_type"])},{Ifnull(data.Rows[i]["insulin_name"])},{Ifnull(data.Rows[i]["insulin_usage"])},{Ifnull(data.Rows[i]["transfer_treatment"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        diabetesid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 尿常规记录表
                DataSet ncg = DbHelperMySQL.Query($@"select * from zkhw_tj_ncg where upload_status='0'");
                if (ncg != null && ncg.Tables.Count > 0 && ncg.Tables[0].Rows.Count > 0)
                {
                    DataTable data = ncg.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sqlncg = $@"insert into zkhw_tj_ncg (ID,aichive_no,id_number,bar_code,WBC,LEU,NIT,URO,PRO,PH,BLD,SG,KET,BIL,GLU,Vc,MA,ACR,Ca,CR,type,createtime,synchronize_type,ZrysNCG) values({ Ifnull(data.Rows[i]["id"])},{ Ifnull(data.Rows[i]["aichive_no"])},{ Ifnull(data.Rows[i]["id_number"])},{ Ifnull(data.Rows[i]["bar_code"])},{ Ifnull(data.Rows[i]["WBC"])},{ Ifnull(data.Rows[i]["LEU"])},{ Ifnull(data.Rows[i]["NIT"])},{ Ifnull(data.Rows[i]["URO"])},{ Ifnull(data.Rows[i]["PRO"])},{ Ifnull(data.Rows[i]["PH"])},{ Ifnull(data.Rows[i]["BLD"])},{ Ifnull(data.Rows[i]["SG"])},{ Ifnull(data.Rows[i]["KET"])},{ Ifnull(data.Rows[i]["BIL"])},{ Ifnull(data.Rows[i]["GLU"])},{ Ifnull(data.Rows[i]["Vc"])},{ Ifnull(data.Rows[i]["MA"])},{ Ifnull(data.Rows[i]["ACR"])},{ Ifnull(data.Rows[i]["Ca"])},{ Ifnull(data.Rows[i]["CR"])},{ Ifnull(data.Rows[i]["type"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{ Ifnull(data.Rows[i]["synchronize_type"])},{ Ifnull(data.Rows[i]["ZrysNCG"])});";
                        sqllist.Add(sqlncg);
                        ncgid += $"'{data.Rows[i]["id"]}',";
                        //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                        //{
                        //    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\n"+sqlncg);
                        //}
                    }
                }
                #endregion

                #region 身高体重
                DataSet sgtz = DbHelperMySQL.Query($@"select * from zkhw_tj_sgtz where upload_status='0'");
                if (sgtz != null && sgtz.Tables.Count > 0 && sgtz.Tables[0].Rows.Count > 0)
                {
                    DataTable data = sgtz.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into zkhw_tj_sgtz (ID,aichive_no,id_number,bar_code,BMI,Height,Weight,createtime) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["BMI"])},{Ifnull(data.Rows[i]["Height"])},{Ifnull(data.Rows[i]["Weight"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        sgtzid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 生化记录表
                DataSet sh = DbHelperMySQL.Query($@"select * from zkhw_tj_sh where upload_status='0'");
                if (sh != null && sh.Tables.Count > 0 && sh.Tables[0].Rows.Count > 0)
                {
                    DataTable data = sh.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into zkhw_tj_sh (ID,aichive_no,id_number,bar_code,ALT,AST,TBIL,DBIL,CREA,UREA,GLU,TG,CHO,HDLC,LDLC,ALB,UA,HCY,AFP,CEA,Ka,Na,TP,ALP,GGT,CHE,TBA,APOA1,APOB,CK,CKMB,LDHL,HBDH,aAMY,createtime,synchronize_type,ZrysSH,low,high,timeCodeUnique) 
                        values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["ALT"])},{Ifnull(data.Rows[i]["AST"])},{Ifnull(data.Rows[i]["TBIL"])},{Ifnull(data.Rows[i]["DBIL"])},{Ifnull(data.Rows[i]["CREA"])},{Ifnull(data.Rows[i]["UREA"])},{Ifnull(data.Rows[i]["GLU"])},{Ifnull(data.Rows[i]["TG"])},{Ifnull(data.Rows[i]["CHO"])},{Ifnull(data.Rows[i]["HDLC"])},{Ifnull(data.Rows[i]["LDLC"])},{Ifnull(data.Rows[i]["ALB"])},{Ifnull(data.Rows[i]["UA"])},{Ifnull(data.Rows[i]["HCY"])},{Ifnull(data.Rows[i]["AFP"])},{Ifnull(data.Rows[i]["CEA"])},{Ifnull(data.Rows[i]["Ka"])},{Ifnull(data.Rows[i]["Na"])},{Ifnull(data.Rows[i]["TP"])},{Ifnull(data.Rows[i]["ALP"])},{Ifnull(data.Rows[i]["GGT"])},{Ifnull(data.Rows[i]["CHE"])},{Ifnull(data.Rows[i]["TBA"])},{Ifnull(data.Rows[i]["APOA1"])},{Ifnull(data.Rows[i]["APOB"])},{Ifnull(data.Rows[i]["CK"])},{Ifnull(data.Rows[i]["CKMB"])},{Ifnull(data.Rows[i]["LDHL"])},{Ifnull(data.Rows[i]["HBDH"])},{Ifnull(data.Rows[i]["aAMY"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["synchronize_type"])},{Ifnull(data.Rows[i]["ZrysSH"])},{Ifnull(data.Rows[i]["low"])},{Ifnull(data.Rows[i]["high"])},{Ifnull(data.Rows[i]["timeCodeUnique"])});");
                        shid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 血常规记录表
                DataSet xcg = DbHelperMySQL.Query($@"select * from zkhw_tj_xcg where upload_status='0'");
                if (xcg != null && xcg.Tables.Count > 0 && xcg.Tables[0].Rows.Count > 0)
                {
                    DataTable data = xcg.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into zkhw_tj_xcg (ID,aichive_no,id_number,bar_code,WBC,RBC,PCT,PLT,HGB,HCT,MCV,MCH,MCHC,RDWCV,RDWSD,MONO,MONOP,GRAN,GRANP,NEUT,NEUTP,EO,EOP,BASO,BASOP,LYM,LYMP,MPV,PDW,MXD,MXDP,PLCR,OTHERS,createtime,synchronize_type,ZrysXCG,timeCodeUnique) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["WBC"])},{Ifnull(data.Rows[i]["RBC"])},{Ifnull(data.Rows[i]["PCT"])},{Ifnull(data.Rows[i]["PLT"])},{Ifnull(data.Rows[i]["HGB"])},{Ifnull(data.Rows[i]["HCT"])},{Ifnull(data.Rows[i]["MCV"])},{Ifnull(data.Rows[i]["MCH"])},{Ifnull(data.Rows[i]["MCHC"])},{Ifnull(data.Rows[i]["RDWCV"])},{Ifnull(data.Rows[i]["RDWSD"])},{Ifnull(data.Rows[i]["MONO"])},{Ifnull(data.Rows[i]["MONOP"])},{Ifnull(data.Rows[i]["GRAN"])},{Ifnull(data.Rows[i]["GRANP"])},{Ifnull(data.Rows[i]["NEUT"])},{Ifnull(data.Rows[i]["EO"])},{Ifnull(data.Rows[i]["EOP"])},{Ifnull(data.Rows[i]["BASO"])},{Ifnull(data.Rows[i]["BASOP"])},{Ifnull(data.Rows[i]["LYM"])},{Ifnull(data.Rows[i]["LYMP"])},{Ifnull(data.Rows[i]["MPV"])},{Ifnull(data.Rows[i]["PDW"])},{Ifnull(data.Rows[i]["MXD"])},{Ifnull(data.Rows[i]["MXDP"])},{Ifnull(data.Rows[i]["PLCR"])},{Ifnull(data.Rows[i]["OTHERS"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["synchronize_type"])},{Ifnull(data.Rows[i]["ZrysXCG"])},{Ifnull(data.Rows[i]["timeCodeUnique"])});");
                        xcgid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 心电图记录表
                DataSet xdt = DbHelperMySQL.Query($@"select * from zkhw_tj_xdt where upload_status='0'");
                if (xdt != null && xdt.Tables.Count > 0 && xdt.Tables[0].Rows.Count > 0)
                {
                    DataTable data = xdt.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into zkhw_tj_xdt (ID,aichive_no,id_number,bar_code,XdtResult,XdtDesc,XdtDoctor,XdtName,Ventrate,PR,QRS,QT,QTc,P_R_T,DOB,Age,Gen,Dep,createtime,synchronize_type,ZrysXDT,imageUrl,hr,p,pqrs,t,rv5,sv1,baseline_drift,myoelectricity,frequency
) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["XdtResult"])},{Ifnull(data.Rows[i]["XdtDesc"])},{Ifnull(data.Rows[i]["XdtDoctor"])},{Ifnull(data.Rows[i]["XdtName"])},{Ifnull(data.Rows[i]["Ventrate"])},{Ifnull(data.Rows[i]["PR"])},{Ifnull(data.Rows[i]["QRS"])},{Ifnull(data.Rows[i]["QT"])},{Ifnull(data.Rows[i]["QTc"])},{Ifnull(data.Rows[i]["P_R_T"])},{Ifnull(data.Rows[i]["DOB"])},{Ifnull(data.Rows[i]["Age"])},{Ifnull(data.Rows[i]["Gen"])},{Ifnull(data.Rows[i]["Dep"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["synchronize_type"])},{Ifnull(data.Rows[i]["ZrysXDT"])},{Ifnull(data.Rows[i]["imageUrl"])},{Ifnull(data.Rows[i]["hr"])},{Ifnull(data.Rows[i]["p"])},{Ifnull(data.Rows[i]["pqrs"])},{Ifnull(data.Rows[i]["t"])},{Ifnull(data.Rows[i]["rv5"])},{Ifnull(data.Rows[i]["sv1"])},{Ifnull(data.Rows[i]["baseline_drift"])},{Ifnull(data.Rows[i]["myoelectricity"])},{Ifnull(data.Rows[i]["frequency"])});");
                        xdtid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 血压表
                DataSet xy = DbHelperMySQL.Query($@"select * from zkhw_tj_xy where upload_status='0'");
                if (xy != null && xy.Tables.Count > 0 && xy.Tables[0].Rows.Count > 0)
                {
                    DataTable data = xy.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into zkhw_tj_xy (ID,aichive_no,id_number,bar_code,DBP,SBP,Pulse,createtime) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["DBP"])},{Ifnull(data.Rows[i]["SBP"])},{Ifnull(data.Rows[i]["Pulse"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        xyid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region B超表
                DataSet bc = DbHelperMySQL.Query($@"select * from zkhw_tj_bc where upload_status='0'");
                if (bc != null && bc.Tables.Count > 0 && bc.Tables[0].Rows.Count > 0)
                {
                    DataTable data = bc.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        sqllist.Add($@"insert into zkhw_tj_bc (ID,aichive_no,id_number,bar_code,FubuBC,FubuResult,FubuDesc,QitaBC,QitaResult,QitaDesc,BuPic01,BuPic02,BuPic03,BuPic04,createtime,synchronize_type,ZrysBC,imageUrl_a,imageUrl_b,imageUrl_c,imageUrl_d) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["FubuBC"])},{Ifnull(data.Rows[i]["FubuResult"])},{Ifnull(data.Rows[i]["FubuDesc"])},{Ifnull(data.Rows[i]["QitaBC"])},{Ifnull(data.Rows[i]["QitaResult"])},{Ifnull(data.Rows[i]["QitaDesc"])},{Ifnull(data.Rows[i]["BuPic01"])},{Ifnull(data.Rows[i]["BuPic02"])},{Ifnull(data.Rows[i]["BuPic03"])},{Ifnull(data.Rows[i]["BuPic04"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["synchronize_type"])},{Ifnull(data.Rows[i]["ZrysBC"])},{Ifnull(data.Rows[i]["imageUrl_a"])},{Ifnull(data.Rows[i]["imageUrl_b"])},{Ifnull(data.Rows[i]["imageUrl_c"])},{Ifnull(data.Rows[i]["imageUrl_d"])});");
                        bcid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion
                if (sqllist.Count<1) {MessageBox.Show("没有需要上传的数据,请稍后再试！"); return; }
                int run = DbHelperMySQL.ExecuteSqlTranYpt(sqllist);
                if (run > 0)
                {
                    if (infoid != null && !"".Equals(infoid))
                    {
                        sqllistz.Add($"update resident_base_info set upload_status='1' where id in({infoid.TrimEnd(',')});");
                    }
                    if (recordid != null && !"".Equals(recordid))
                    {
                        sqllistz.Add($"update physical_examination_record set upload_status='1' where id in({recordid.TrimEnd(',')});");
                    }
                    if (estimateid != null && !"".Equals(estimateid))
                    {
                        sqllistz.Add($"update elderly_selfcare_estimate set upload_status='1' where id in({estimateid.TrimEnd(',')});");
                    }
                    if (followid != null && !"".Equals(followid))
                    {
                        sqllistz.Add($"update follow_medicine_record set upload_status='1' where id in({followid.TrimEnd(',')});");
                    }
                    if (fuvid != null && !"".Equals(fuvid))
                    {
                        sqllistz.Add($"update fuv_hypertension set upload_status='1' where id in({fuvid.TrimEnd(',')});");
                    }
                    if (diabetesid != null && !"".Equals(diabetesid))
                    {
                        sqllistz.Add($"update diabetes_follow_record set upload_status='1' where id in({diabetesid.TrimEnd(',')});");
                    }
                    if (ncgid != null && !"".Equals(ncgid))
                    {
                        sqllistz.Add($"update zkhw_tj_ncg set upload_status='1' where id in({ncgid.TrimEnd(',')});");
                    }
                    if (sgtzid != null && !"".Equals(sgtzid))
                    {
                        sqllistz.Add($"update zkhw_tj_sgtz set upload_status='1' where id in({sgtzid.TrimEnd(',')});");
                    }
                    if (shid != null && !"".Equals(shid))
                    {
                        sqllistz.Add($"update zkhw_tj_sh set upload_status='1' where id in({shid.TrimEnd(',')});");
                    }
                    if (xcgid != null && !"".Equals(xcgid))
                    {
                        sqllistz.Add($"update zkhw_tj_xcg set upload_status='1' where id in({xcgid.TrimEnd(',')});");
                    }
                    if (xdtid != null && !"".Equals(xdtid))
                    {
                        sqllistz.Add($"update zkhw_tj_xdt set upload_status='1' where id in({xdtid.TrimEnd(',')});");
                    }
                    if (xyid != null && !"".Equals(xyid))
                    {
                        sqllistz.Add($"update zkhw_tj_xy set upload_status='1' where id in({xyid.TrimEnd(',')});");
                    }
                    if (bcid != null && !"".Equals(bcid))
                    {
                        sqllistz.Add($"update zkhw_tj_bc set upload_status='1' where id in({bcid.TrimEnd(',')});");
                    }
                    int reu1 = DbHelperMySQL.ExecuteSqlTran(sqllistz);
                    if (reu1 > 0)
                    {
                        bean.loginLogBean lb = new bean.loginLogBean();
                        lb.name = frmLogin.name;
                        lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        lb.eventInfo = "数据上传成功！";
                        lb.type = "2";
                        lls.addCheckLog(lb);
                        MessageBox.Show("数据上传成功！");
                    }
                    else
                    {
                        MessageBox.Show("上传错误！222");
                    }
                }
                else
                {
                    bean.loginLogBean lb = new bean.loginLogBean();
                    lb.name = frmLogin.name;
                    lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    lb.eventInfo = "数据上传异常！";
                    lb.type = "2";
                    lls.addCheckLog(lb);
                    MessageBox.Show("数据上传异常，请重试！");
                    return;
                }
            }
            catch (Exception ex)
            {
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "数据上传异常！";
                lb.type = "2";
                lls.addCheckLog(lb);
                MessageBox.Show("错误请联系管理员！" + ex.Message + "\n" + ex.StackTrace);
                return;
            }

        }

        private string Ifnull(object dataRow)
        {
            if (Convert.IsDBNull(dataRow))
            {
                return "null";
            }
            else
            {
                return "'" + dataRow.ToString() + "'";
            }
        }
        #endregion
        /// <summary>
        /// 查看报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击button按钮事件
            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnModify" && e.RowIndex >= 0)
            {
                //说明点击的列是DataGridViewButtonColumn列
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
                string id = dataGridView1["编码", e.RowIndex].Value.ToString();
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
