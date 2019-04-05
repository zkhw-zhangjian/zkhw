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

namespace zkhwClient.view.PublicHealthView
{
    public partial class examinatReport : Form
    {
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
            string sql = @"SELECT count(sex)sun,sex
from zkhw_tj_bgdc
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
            string sql = $@"select SQL_CALC_FOUND_ROWS 
                            id,
                            DATE_FORMAT(healthchecktime,'%Y%m%d') 登记时间,
                            area_duns 区域,
                            aichive_no 编码,
                            name 姓名,
                            sex 性别,
                            id_number 身份证号,
                            ShiFouTongBu 是否同步,
                            BaoGaoShengChan 报告生产时间
                            from zkhw_tj_bgdc where 1=1 ";
            if (pairs != null && pairs.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(pairs["timesta"]) && !string.IsNullOrWhiteSpace(pairs["timeend"]))
                {
                    sql += $" and date_format(healthchecktime,'%Y-%m-%d') between '{pairs["timesta"]}' and '{pairs["timeend"]}'";
                }

                if (!string.IsNullOrWhiteSpace(pairs["juming"]))
                {
                    sql += $" or name='{pairs["juming"]}' or bar_code='{pairs["juming"]}' or id_number='{pairs["juming"]}'";
                }
            }
            //sql += $" and id > ({pageindex}-1)*{pagesize} limit {pagesize}; select found_rows()";
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
                //this.dataGridView1.Columns[1].HeaderCell.Value = "登记时间";
                //this.dataGridView1.Columns[2].HeaderCell.Value = "区域";
                //this.dataGridView1.Columns[3].HeaderCell.Value = "编号";
                //this.dataGridView1.Columns[4].HeaderCell.Value = "姓名";
                //this.dataGridView1.Columns[5].HeaderCell.Value = "性别";
                //this.dataGridView1.Columns[6].HeaderCell.Value = "身份证号";
                //this.dataGridView1.Columns[7].HeaderCell.Value = "是否同步";
                //this.dataGridView1.Columns[8].HeaderCell.Value = "报告生产时间";
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
            from zkhw_tj_bgdc where birthday >= '0' and birthday<= '64' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
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
            from zkhw_tj_bgdc where birthday >= '65' and birthday<= '70' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
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
            from zkhw_tj_bgdc where birthday >= '70' and birthday<= '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
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
            from zkhw_tj_bgdc where birthday >= '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
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
                foreach (Control ctrl in groupBox4.Controls)
                {
                    if (ctrl is CheckBox)
                    {
                        if (((CheckBox)ctrl).Checked)
                        {
                            list.Add(ctrl.Text);
                            switch (ctrl.Text)
                            {
                                case "封面":
                                    sql = $@"SELECT 
jk.id_number,
jk.aichive_no archive_no,
jk.Xzhuzhi,
jk.address,
jk.XzjdName,
jk.CjwhName,
jk.JddwName,
jk.JdrName,
jk.ZrysName,
info.name,
info.phone 
from resident_base_info info 
join 
(select * from (select * from zkhw_tj_jk order by createtime desc) as a group by aichive_no order by createtime desc) jk
on info.archive_no=jk.aichive_no
where info.archive_no in('{string.Join(",", ide)}')";
                                    DataSet datas = DbHelperMySQL.Query(sql);
                                    if (datas != null && datas.Tables.Count > 0)
                                    {
                                        DataTable data = datas.Tables[0].Copy();
                                        data.TableName = "封面";
                                        dataSet.Tables.Add(data);
                                    }
                                    break;
                                case "个人信息":
                                    sql = $@"select * from resident_base_info base where base.archive_no in('{string.Join(",", ide)}')";
                                    DataSet gr = DbHelperMySQL.Query(sql);
                                    if (gr != null && gr.Tables.Count > 0)
                                    {
                                        DataTable data = gr.Tables[0].Copy();
                                        data.TableName = "个人信息";
                                        dataSet.Tables.Add(data);
                                    }
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
                        DataRow data = dataSet.Tables[items].Select($"archive_no={item}")[0];
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
                        for (int i = 0; i < dataSet.Tables["封面"].Rows.Count; i++)
                        {
                            DataRow data = dataSet.Tables["封面"].Rows[i];
                            doc = PdfProcessing(list[0], data);
                            string urls = @str + $"/up/result/{data["archive_no"].ToString()}.pdf";
                            DeteleFile(urls);
                            doc.Save(urls, SaveFormat.Pdf);
                        }
                        break;
                    case "个人信息":
                        for (int i = 0; i < dataSet.Tables["个人信息"].Rows.Count; i++)
                        {
                            DataRow data = dataSet.Tables["个人信息"].Rows[i];
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
                    dic.Add("现住址", data["Xzhuzhi"].ToString());
                    dic.Add("户籍地址", data["address"].ToString());
                    dic.Add("联系电话", data["phone"].ToString());
                    dic.Add("乡镇名称", data["XzjdName"].ToString());
                    dic.Add("村委会名称", data["CjwhName"].ToString());
                    dic.Add("建档单位", data["JddwName"].ToString());
                    dic.Add("建档人", data["JdrName"].ToString());
                    dic.Add("责任医生", data["ZrysName"].ToString());
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
                    dics.Add("出生日期", data["birthday"].ToString());
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
                                string fqs = fq[j]["disease_type"].ToString();
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
                                string fqs = fq[j]["disease_type"].ToString();
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
                                string fqs = fq[j]["disease_type"].ToString();
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
                    dics.Add("遗传病史", data["profession"].ToString());
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
                default:
                    break;
            }
            return doc;
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

        private void DeteleFile(string url)
        {
            if (File.Exists(url))
            {
                File.Delete(url);
            }
        }

        private string ZF(string st)
        {
            string rule = string.Empty;
            if (st.IndexOf(',') > 0)
            {
                string[] zi = st.Split(',');
                foreach (var item in zi)
                {
                    switch (item)
                    {
                        case "1":
                            rule += "⑴/";
                            break;
                        case "2":
                            rule += "⑵/";
                            break;
                        case "3":
                            rule += "⑶/";
                            break;
                        case "4":
                            rule += "⑷/";
                            break;
                        case "5":
                            rule += "⑸/";
                            break;
                        case "6":
                            rule += "⑹/";
                            break;
                        case "7":
                            rule += "⑺/";
                            break;
                        case "8":
                            rule += "⑻/";
                            break;
                        case "9":
                            rule += "⑼/";
                            break;
                        case "10":
                            rule += "⑽/";
                            break;
                        case "11":
                            rule += "⑾/";
                            break;
                        case "12":
                            rule += "⑿/";
                            break;
                        case "13":
                            rule += "⒀/";
                            break;
                        case "14":
                            rule += "⒁/";
                            break;
                        case "15":
                            rule += "⒂/";
                            break;
                        case "16":
                            rule += "⒃/";
                            break;
                        case "17":
                            rule += "⒄/";
                            break;
                        case "18":
                            rule += "⒅/";
                            break;
                        case "19":
                            rule += "⒆/";
                            break;
                        case "20":
                            rule += "⒇/";
                            break;
                        default:
                            break;
                    }
                }
                return rule.TrimEnd('/');
            }
            else
            {
                switch (st)
                {
                    case "1":
                        rule = "⑴";
                        break;
                    case "2":
                        rule = "⑵";
                        break;
                    case "3":
                        rule = "⑶";
                        break;
                    case "4":
                        rule = "⑷";
                        break;
                    case "5":
                        rule = "⑸";
                        break;
                    case "6":
                        rule = "⑹";
                        break;
                    case "7":
                        rule = "⑺";
                        break;
                    case "8":
                        rule = "⑻";
                        break;
                    case "9":
                        rule = "⑼";
                        break;
                    case "10":
                        rule = "⑽";
                        break;
                    case "11":
                        rule = "⑾";
                        break;
                    case "12":
                        rule = "⑿";
                        break;
                    case "13":
                        rule = "⒀";
                        break;
                    case "14":
                        rule = "⒁";
                        break;
                    case "15":
                        rule = "⒂";
                        break;
                    case "16":
                        rule = "⒃";
                        break;
                    case "17":
                        rule = "⒄";
                        break;
                    case "18":
                        rule = "⒅";
                        break;
                    case "19":
                        rule = "⒆";
                        break;
                    case "20":
                        rule = "⒇";
                        break;
                    default:
                        break;

                }
                return rule;
            }
        }
    }

    public class Report
    {
        public string Name { get; set; }
        public Document Doc { get; set; }
    }
}
