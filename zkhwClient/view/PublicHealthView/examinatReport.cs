/*
 * 改登记时间为：zkhw_tj_bgdc中的createtime 
 * 导出 健康体检表 按照 id_number、bar_code、createttime
 */
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class examinatReport : Form
    {
        string str = Application.StartupPath;//项目路径
        service.loginLogService lls = new service.loginLogService();
        bool isfirst = false;
        #region 初始化数据
        public examinatReport()
        {
            InitializeComponent();

        }
        private void GetAllPersonInfo()
        {
            #region 报告统计数据绑定
            string _xuncode = basicInfoSettings.xcuncode;

            string time1 = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            string time2 = dateTimePicker4.Value.ToString("yyyy-MM-dd");
            if (DateTime.Parse(time1) != DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                _xuncode = "";
            }
            string sql = $@"SELECT count(sex) sun,sex from zkhw_tj_bgdc where area_duns 
                    like '%{_xuncode}%' and  date_format(createtime,'%Y-%m-%d') between '{time1}' and '{time2}' GROUP BY sex ";
            if (isfirst == true)
            {
                time1 = Common.GetCreateTime(basicInfoSettings.createtime);
                sql = $@"SELECT count(sex) sun,sex from zkhw_tj_bgdc where area_duns like '%{_xuncode}%' and date_format(createtime,'%Y-%m-%d')>='{time1}' GROUP BY sex ";
            }

            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable data = dataSet.Tables[0];
            if (data != null && data.Rows.Count > 0)
            {
                DataRow[] rows = data.Select("sex='2'");
                if (rows.Length > 0)
                {
                    女.Text = rows[0]["sun"].ToString();
                }
                else
                {
                    女.Text = "0";
                }
                DataRow[] rowsn = data.Select("sex='1'");
                if (rowsn.Length > 0)
                {
                    男.Text = rowsn[0]["sun"].ToString();
                }
                else
                {
                    男.Text = "0";
                }
                总数.Text = data.Compute("sum(sun)", "true").ToString();
            }
            #endregion
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void BinData()
        {

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
            //this.dateTimePicker1.Value = this.dateTimePicker2.Value.AddDays(-1);
            this.dateTimePicker1.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            this.dateTimePicker3.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/check.png");
            this.统计查询.BackgroundImage = Image.FromFile(@str + "/images/check.png");

            isfirst = true;
            QueryTongJi();
            BinData();
            isfirst = false;
            //pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);
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
DATE_FORMAT(bgdc.createtime,'%Y%m%d') 登记时间,
concat(base.province_name,base.city_name,base.county_name,base.towns_name,base.village_name) 区域,
base.archive_no 编码,
base.name 姓名,
(case base.sex when '1'then '男' when '2' then '女' when '9' then '未说明的性别' when '0' then '未知的性别' ELSE ''
END)性别,
base.id_number 身份证号,
bgdc.BaoGaoShengChan 报告生成时间,
(case bgdc.ShiFouTongBu when '1' then '是' ELSE '否' END) 是否上传数据,
bgdc.BChaoImage B超图片,bgdc.XinDianImage 心电图片,bgdc.bar_code 条码号,base.card_pic
from resident_base_info base
left join 
(select * from zkhw_tj_bgdc order by createtime desc) bgdc
on base.id_number=bgdc.id_number
where 1=1";
            if (pairs != null && pairs.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(pairs["timesta"]) && !string.IsNullOrWhiteSpace(pairs["timeend"]))
                {
                    sql += $" and date_format(bgdc.createtime,'%Y-%m-%d') between '{pairs["timesta"]}' and '{pairs["timeend"]}'";
                }
                if (!string.IsNullOrWhiteSpace(pairs["juming"]))
                {
                    sql += $" and base.name like '%{pairs["juming"]}%' or base.id_number like '%{pairs["juming"]}%' or base.archive_no like '%{pairs["juming"]}%'";
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
            sql += $@"; select found_rows();";//limit {pagesize} and base.id >=( select id From zkhw_tj_bgdc Order By id limit {pageindex},1)
            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable dt = dataSet.Tables[0];
            count = Convert.ToInt32(dataSet.Tables[1].Rows[0][0]);
            return dt;
        }
        //(select * from (select * from zkhw_tj_bgdc order by createtime desc) as a group by aichive_no order by createtime desc) bgdc
        //声明静态类变量
        private static DataGridViewCheckBoxColumn checkColumn = null;
        private static DataGridViewButtonColumn buttonColumn = null;

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            //这一句很重要结束编辑状态
            dataGridView1.EndEdit();
            dataGridView1.Rows.OfType<DataGridViewRow>().ToList().ForEach(t => t.Cells[0].Value = state);
        }
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="data"></param>
        private void queryExaminatProgress(DataTable data)
        {
            if (dataGridView1.ColumnCount > 0)
            {
                dataGridView1.Columns.Clear();
            }
            if (data != null)
            {
                this.dataGridView1.DataSource = data;
                //if (buttonColumn == null)
                //{
                buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "btnModify";
                buttonColumn.HeaderText = "";
                buttonColumn.DefaultCellStyle.NullValue = "查看报告";
                dataGridView1.Columns.Add(buttonColumn);
                //}
                //else {
                //    buttonColumn.Dispose();
                //    buttonColumn = null;
                //}
                checkColumn = new DataGridViewCheckBoxColumn(); //插入第0列 
                DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
                cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);
                checkColumn.HeaderCell = cbHeader;
                checkColumn.HeaderText = "";
                checkColumn.Name = "cb_check";
                checkColumn.TrueValue = true;
                checkColumn.FalseValue = false;
                checkColumn.DataPropertyName = "IsChecked";
                dataGridView1.Columns.Insert(0, checkColumn);    //添加的checkbox在第一列 


                this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                this.dataGridView1.ReadOnly = true;

                //dataGridView1.Columns[0].Width = 30;
                //dataGridView1.Columns[1].Width = 80;
                //dataGridView1.Columns[4].Width = 80;
                //dataGridView1.Columns[5].Width = 70;
                //dataGridView1.Columns[6].Width = 120;
                //dataGridView1.Columns[7].Width = 100;
                //dataGridView1.Columns[8].Width = 110;
                //dataGridView1.Columns[9].Width = 70;
                //dataGridView1.Columns[10].Width = 70; 
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                //int rows = this.dataGridView1.Rows.Count - 1 <= 0 ? 0 : this.dataGridView1.Rows.Count - 1;
                //if(rows>0)
                //for (int x = 0; x <= rows; x++)
                //{
                //    this.dataGridView1.Rows[x].HeaderCell.Value = String.Format("{0}", x + 1);
                //} 
            }
            DisplayShangChuanInfo(data);
        }

        private void DisplayShangChuanInfo(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                lblShangChuan.Text = "已上传:0";
                lblWeiShangChuan.Text = "未上传:0";
            }
            else
            {
                DataRow[] dr = dt.Select("是否上传数据 = '否'");
                if (dr.Length > 0)
                {
                    lblWeiShangChuan.Text = "未上传:" + dr.Length.ToString();
                }
                else
                {
                    lblWeiShangChuan.Text = "未上传:0";
                }

                DataRow[] dr1 = dt.Select("是否上传数据 = '是'");
                if (dr1.Length > 0)
                {
                    lblShangChuan.Text = "已上传:" + dr1.Length.ToString();
                }
                else
                {
                    lblShangChuan.Text = "已上传:0";
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
        /// CheckBox勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //checkbox 勾上
            if (e.RowIndex == -1) { return; }
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
        private void QueryTongJi()
        {
            GetAllPersonInfo();   //同步处理下
            string stan = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            string end = dateTimePicker4.Value.ToString("yyyy-MM-dd");

            string sql = "";
            if (DateTime.Parse(stan) != DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                sql = $@"SELECT sex,'64',COUNT(sex) 人数,
                COUNT(CASE
                    WHEN(BChao = '2' or BChao = '3') THEN '1'
                END
                ) as B超异常,
                COUNT(CASE
                    WHEN(XinDian = '2' or XinDian = '3') THEN
                        '1'
                END
                ) as 心电异常,
                COUNT(CASE
                    WHEN(NiaoChangGui = '2' or NiaoChangGui = '3') THEN
                        '1'
                END
                ) as 尿常规异常,
                COUNT(CASE
                    WHEN(XueYa = '2' or XueYa = '3') THEN
                        '1'
                END
                ) as 血压异常,
                COUNT(CASE
                    WHEN(ShengHua = '2' or ShengHua = '3') THEN
                        '1'
                END
                ) as 生化异常
                from zkhw_tj_bgdc where age >= '0' and age<= '64' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
                GROUP BY sex;

                SELECT sex,'70',COUNT(sex) 人数,
                COUNT(CASE
                    WHEN(BChao = '2' or BChao = '3') THEN
                        '1'
                END
                ) as B超异常,
                COUNT(CASE
                    WHEN(XinDian = '2' or XinDian = '3') THEN
                        '1'
                END
                ) as 心电异常,
                COUNT(CASE
                    WHEN(NiaoChangGui = '2' or NiaoChangGui = '3') THEN
                        '1'
                END
                ) as 尿常规异常,
                COUNT(CASE
                    WHEN(XueYa = '2' or XueYa = '3') THEN
                        '1'
                END
                ) as 血压异常,
                COUNT(CASE
                    WHEN(ShengHua = '2' or ShengHua = '3') THEN
                        '1'
                END
                ) as 生化异常
                from zkhw_tj_bgdc where  age >= '65' and age<= '70' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
                GROUP BY sex;

                SELECT sex,'75',COUNT(sex) 人数,
                COUNT(CASE
                    WHEN(BChao = '2' or BChao = '3') THEN
                        '1'
                END
                ) as B超异常,
                COUNT(CASE
                    WHEN(XinDian = '2' or XinDian = '3') THEN
                        '1'
                END
                ) as 心电异常,
                COUNT(CASE
                    WHEN(NiaoChangGui = '2' or NiaoChangGui = '3') THEN
                        '1'
                END
                ) as 尿常规异常,
                COUNT(CASE
                    WHEN(XueYa = '2' or XueYa = '3') THEN
                        '1'
                END
                ) as 血压异常,
                COUNT(CASE
                    WHEN(ShengHua = '2' or ShengHua = '3') THEN
                        '1'
                END
                ) as 生化异常
                from zkhw_tj_bgdc where  age > '70' and age<= '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
                GROUP BY sex;

                SELECT sex,'76',COUNT(sex) 人数,
                COUNT(CASE
                    WHEN(BChao = '2' or BChao = '3') THEN
                        '1'
                END
                ) as B超异常,
                COUNT(CASE
                    WHEN(XinDian = '2' or XinDian = '3') THEN
                        '1'
                END
                ) as 心电异常,
                COUNT(CASE
                    WHEN(NiaoChangGui = '2' or NiaoChangGui = '3') THEN
                        '1'
                END
                ) as 尿常规异常,
                COUNT(CASE
                    WHEN(XueYa = '2' or XueYa = '3') THEN
                        '1'
                END
                ) as 血压异常,
                COUNT(CASE
                    WHEN(ShengHua = '2' or ShengHua = '3') THEN
                        '1'
                END
                ) as 生化异常
                from zkhw_tj_bgdc where  age > '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
                GROUP BY sex";
            }
            else
            {
                sql = $@"SELECT sex,'64',COUNT(sex) 人数,
                COUNT(CASE
                    WHEN(BChao = '2' or BChao = '3') THEN '1'
                END
                ) as B超异常,
                COUNT(CASE
                    WHEN(XinDian = '2' or XinDian = '3') THEN
                        '1'
                END
                ) as 心电异常,
                COUNT(CASE
                    WHEN(NiaoChangGui = '2' or NiaoChangGui = '3') THEN
                        '1'
                END
                ) as 尿常规异常,
                COUNT(CASE
                    WHEN(XueYa = '2' or XueYa = '3') THEN
                        '1'
                END
                ) as 血压异常,
                COUNT(CASE
                    WHEN(ShengHua = '2' or ShengHua = '3') THEN
                        '1'
                END
                ) as 生化异常
                from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and age >= '0' and age<= '64' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
                GROUP BY sex;

                SELECT sex,'70',COUNT(sex) 人数,
                COUNT(CASE
                    WHEN(BChao = '2' or BChao = '3') THEN
                        '1'
                END
                ) as B超异常,
                COUNT(CASE
                    WHEN(XinDian = '2' or XinDian = '3') THEN
                        '1'
                END
                ) as 心电异常,
                COUNT(CASE
                    WHEN(NiaoChangGui = '2' or NiaoChangGui = '3') THEN
                        '1'
                END
                ) as 尿常规异常,
                COUNT(CASE
                    WHEN(XueYa = '2' or XueYa = '3') THEN
                        '1'
                END
                ) as 血压异常,
                COUNT(CASE
                    WHEN(ShengHua = '2' or ShengHua = '3') THEN
                        '1'
                END
                ) as 生化异常
                from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and age >= '65' and age<= '70' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
                GROUP BY sex;

                SELECT sex,'75',COUNT(sex) 人数,
                COUNT(CASE
                    WHEN(BChao = '2' or BChao = '3') THEN
                        '1'
                END
                ) as B超异常,
                COUNT(CASE
                    WHEN(XinDian = '2' or XinDian = '3') THEN
                        '1'
                END
                ) as 心电异常,
                COUNT(CASE
                    WHEN(NiaoChangGui = '2' or NiaoChangGui = '3') THEN
                        '1'
                END
                ) as 尿常规异常,
                COUNT(CASE
                    WHEN(XueYa = '2' or XueYa = '3') THEN
                        '1'
                END
                ) as 血压异常,
                COUNT(CASE
                    WHEN(ShengHua = '2' or ShengHua = '3') THEN
                        '1'
                END
                ) as 生化异常
                from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and age > '70' and age<= '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
                GROUP BY sex;

                SELECT sex,'76',COUNT(sex) 人数,
                COUNT(CASE
                    WHEN(BChao = '2' or BChao = '3') THEN
                        '1'
                END
                ) as B超异常,
                COUNT(CASE
                    WHEN(XinDian = '2' or XinDian = '3') THEN
                        '1'
                END
                ) as 心电异常,
                COUNT(CASE
                    WHEN(NiaoChangGui = '2' or NiaoChangGui = '3') THEN
                        '1'
                END
                ) as 尿常规异常,
                COUNT(CASE
                    WHEN(XueYa = '2' or XueYa = '3') THEN
                        '1'
                END
                ) as 血压异常,
                COUNT(CASE
                    WHEN(ShengHua = '2' or ShengHua = '3') THEN
                        '1'
                END
                ) as 生化异常
                from zkhw_tj_bgdc where area_duns='{basicInfoSettings.xcuncode}' and age > '75' and date_format(healthchecktime,'%Y-%m-%d') between '{stan}' and '{end}'
                GROUP BY sex";
            }

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
                                DataRow[] rows = data.Select("sex='2'");
                                if (rows != null && rows.Length > 0)
                                {
                                    女064.Text = rows[0]["人数"].ToString();
                                }
                                else
                                {
                                    女064.Text = "0";
                                }
                                DataRow[] rowss = data.Select("sex='1'");
                                if (rowss != null && rowss.Length > 0)
                                {
                                    男064.Text = rowss[0]["人数"].ToString();
                                }
                                else
                                {
                                    男064.Text = "0";
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
                                DataRow[] nv6570 = data.Select("sex='2'");
                                if (nv6570 != null && nv6570.Length > 0)
                                {
                                    女6570.Text = nv6570[0]["人数"].ToString();
                                }
                                else
                                {
                                    女6570.Text = "0";
                                }
                                DataRow[] nan6570 = data.Select("sex='1'");
                                if (nan6570 != null && nan6570.Length > 0)
                                {
                                    男6570.Text = nan6570[0]["人数"].ToString();
                                }
                                else
                                {
                                    男6570.Text = "0";
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
                                DataRow[] nv7075 = data.Select("sex='2'");
                                if (nv7075 != null && nv7075.Length > 0)
                                {
                                    女7075.Text = nv7075[0]["人数"].ToString();
                                }
                                else
                                {
                                    女7075.Text = "0";
                                }
                                DataRow[] nan7075 = data.Select("sex='1'");
                                if (nan7075 != null && nan7075.Length > 0)
                                {
                                    男7075.Text = nan7075[0]["人数"].ToString();
                                }
                                else
                                {
                                    男7075.Text = "0";
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
                                DataRow[] nv75 = data.Select("sex='2'");
                                if (nv75 != null && nv75.Length > 0)
                                {
                                    女75.Text = nv75[0]["人数"].ToString();
                                }
                                else
                                {
                                    女75.Text = "0";
                                }
                                DataRow[] nan75 = data.Select("sex='1'");
                                if (nan75 != null && nan75.Length > 0)
                                {
                                    男75.Text = nan75[0]["人数"].ToString();
                                }
                                else
                                {
                                    男75.Text = "0";
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
        private void 统计查询_Click(object sender, EventArgs e)
        {
            QueryTongJi();
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
                List<ComboBoxData> ide = new List<ComboBoxData>();
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
                    ComboBoxData combo = new ComboBoxData();
                    if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                    {
                        string id = dataGridView1["身份证号", i].Value.ToString();
                        combo.ID = "'" + id + "'";
                        combo.Name = dataGridView1["姓名", i].Value.ToString();
                        combo.BarCode = dataGridView1["条码号", i].Value.ToString();
                        ide.Add(combo);
                    }
                }
                if (ide.Count < 1)
                {
                    MessageBox.Show("请选择要导出报告的人员!"); return;
                }
                string sql = string.Empty;
                sql = $@"select * from resident_base_info base where base.id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})";
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
                MessageBox.Show("报告导出异常，请联系管理员！" + ex.Message + "/r/n" + ex.StackTrace);
            }
        }
        private void CallbackFunc(IAsyncResult result)
        {
            //AddHandler handler = (AddHandler)((AsyncResult)result).AsyncDelegate;
            //Console.WriteLine(handler.EndInvoke(result));
            //Console.WriteLine(result.AsyncState);
        }
        private bool PDF(List<string> list, DataSet dataSet, List<ComboBoxData> ide)
        {
            Document doc = null;
            LoadingHelper.myCaption = "正在导出...";
            LoadingHelper.myLabel = "正在导出第1份";
            LoadingHelper.ShowLoadingScreen();
            Thread.Sleep(50);
            if (LoadingHelper.loadingForm != null)
            {
                LoadingHelper.loadingForm.mystr = "正在导出第 1 份";
            }
            try
            {
                if (list.Count > 1)
                {
                    #region 多个

                    int intNum = 0;
                    foreach (var item in ide)
                    {
                        List<Report> reports = new List<Report>();
                        foreach (var items in list)
                        { 
                            Report report = new Report();
                            DataRow data = dataSet.Tables["个人"].Select($"id_number={item.ID}")[0];
                            string age = data["age"].ToString();
                            if (age.Length > 0)
                            {
                                int ageint = Int32.Parse(age);
                                if (ageint < 65)
                                {
                                    if (items == "中医体质" || items == "老年人生活自理能力评估")
                                    {
                                        continue;
                                    }
                                }
                            }
                            report.Name = items;
                            report.Doc = PdfProcessing(items, data, item.BarCode);
                            reports.Add(report);
                        }
                        if (reports.Count == 0) continue;
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
                            string urls = @str + $"/up/result/{item.Name + item.ID.Replace("'", "")}.pdf";
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
                            string urls = @str + $"/up/result/{item.Name + item.ID.Replace("'", "")}.pdf";
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
                            string urls = @str + $"/up/result/{item.Name + item.ID.Replace("'", "")}.pdf";
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
                            string urls = @str + $"/up/result/{item.Name + item.ID.Replace("'", "")}.pdf";
                            DeteleFile(urls);
                            rp.Doc.Save(urls, SaveFormat.Pdf);
                        }
                        intNum = intNum + 1;
                        if (LoadingHelper.loadingForm != null)
                        {
                            LoadingHelper.loadingForm.mystr = string.Format("已导出 {0} 份共 {1} 份", intNum, list.Count);
                        }
                    }
                    LoadingHelper.CloseForm();
                    if(intNum == 0)
                    {
                        MessageBox.Show("没有导出的报告！");
                        return false;
                    }
                    return true;
                    #endregion
                }
                else
                {
                    #region 单个
                    if (list == null || list.Count == 0)
                    {
                        LoadingHelper.CloseForm();
                        MessageBox.Show("请选择你要生成的报告类型！");

                        return false;
                    }
                    if (list[0] == "综合报告单")
                    {
                        int intNum = 0;
                        List<string> vs = new List<string>();
                        vs.Add("封面");
                        //vs.Add("个人信息");
                        vs.Add("健康体检表");
                        vs.Add("化验报告单");
                        vs.Add("心电图");
                        vs.Add("B超");
                        vs.Add("老年人生活自理能力评估");
                        vs.Add("中医体质");
                        vs.Add("结果");
                        for (int i = 0; i < dataSet.Tables["个人"].Rows.Count; i++)
                        {
                            List<Report> reports = new List<Report>();
                            DataRow data = dataSet.Tables["个人"].Rows[i];
                            string age = data["age"].ToString();
                            string idnum = data["id_number"].ToString();
                            string barcode = "";
                            string aa = "'" + idnum + "'";
                            var q = (from l in ide where l.ID == aa select l).ToList();
                            if (q.Count > 0)
                            {
                                barcode = q[0].BarCode;
                            }
                            foreach (var item in vs)
                            {
                                if (age.Length > 0)
                                {
                                    int ageint = Int32.Parse(age);
                                    if (ageint < 65)
                                    {
                                        if (item == "中医体质" || item == "老年人生活自理能力评估")
                                        {
                                            continue;
                                        }
                                    }
                                }

                                Report report = new Report();
                                report.Doc = PdfProcessing(item, data, barcode);
                                report.Name = item;
                                reports.Add(report);
                            }
                            Report re = reports.Where(m => m.Name == "封面").FirstOrDefault();
                            //Report res = reports.Where(m => m.Name == "个人信息").FirstOrDefault();
                            //re.Doc.AppendDocument(res.Doc, ImportFormatMode.KeepSourceFormatting);
                            reports.Remove(re);
                            //reports.Remove(res);
                            foreach (var item in reports)
                            {
                                re.Doc.AppendDocument(item.Doc, ImportFormatMode.KeepSourceFormatting);

                            }
                            string urls = @str + $"/up/result/{"综合报告单-" + data["name"].ToString() + data["id_number"].ToString()}.pdf";
                            DeteleFile(urls);

                            re.Doc.Save(urls, SaveFormat.Pdf);

                            intNum = intNum + 1;
                            if (LoadingHelper.loadingForm != null)
                            {
                                LoadingHelper.loadingForm.mystr = string.Format("已导出 {0} 份共 {1} 份", intNum, dataSet.Tables["个人"].Rows.Count);
                            }
                        }
                    }
                    else
                    {
                        int intNum = 0;
                        for (int i = 0; i < dataSet.Tables["个人"].Rows.Count; i++)
                        {
                            DataRow data = dataSet.Tables["个人"].Rows[i];
                            string aa = "'" + data["id_number"].ToString() + "'";
                            var q = (from l in ide where l.ID == aa select l).ToList();
                            string barcode = "";
                            if (q.Count > 0)
                            {
                                barcode = q[0].BarCode;
                            }
                            string age = data["age"].ToString();
                            if (age.Length > 0)
                            {
                                int ageint = Int32.Parse(age);
                                if (ageint < 65)
                                {
                                    if (list[0] == "中医体质" || list[0] == "老年人生活自理能力评估")
                                    {
                                        LoadingHelper.CloseForm();
                                        MessageBox.Show("年龄小于65岁未做中医体质和老年人生活自理能力评估！"); 
                                        return false;
                                    }
                                }
                            }
                            doc = PdfProcessing(list[0], data, barcode);
                            string urls = @str + $"/up/result/{list[0] + "-" + data["name"].ToString() + data["id_number"].ToString()}.pdf";
                            DeteleFile(urls);
                            doc.Save(urls, SaveFormat.Pdf);
                            intNum = intNum + 1;
                            if (LoadingHelper.loadingForm != null)
                            {
                                LoadingHelper.loadingForm.mystr = string.Format("已导出 {0} 份共 {1} 份", intNum, dataSet.Tables["个人"].Rows.Count);
                            }
                        }
                    }
                    LoadingHelper.CloseForm();
                    return true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (LoadingHelper.loadingForm != null)
                {
                    LoadingHelper.CloseForm();
                }
                bean.loginLogBean lb = new bean.loginLogBean();
                lb.name = frmLogin.name;
                lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lb.eventInfo = "报告导出异常！" + ex.Message;
                lb.type = "2";
                lls.addCheckLog(lb);
                MessageBox.Show("报告导出异常，请联系管理员！" + ex.Message + "/r/n" + ex.StackTrace);
                return false;
            }
        }
        private bool isDouble(string a, out double b)
        {
            bool t = double.TryParse(a, out b);
            return t;
        }

        private Dictionary<string, string> GetShHuaXCGDic(DataTable dtSh,string type)
        {
            var hy = new Dictionary<string, string>();
            try
            {
                DataRow[] dr = dtSh.Select("type='" + type + "'");
                if (dr != null)
                { 
                    if(type== "HDLC")
                    {
                        type = "HDL_C";
                    }
                    if (type == "LDLC")
                    {
                        type = "LDL_C";
                    }
                    if (type == "LYM")
                    {
                        type = "LYMN";
                    }
                    if (type == "MXD")
                    {
                        type = "MXDN";
                    }
                    if (type == "NEUT")
                    {
                        type = "NEUTN";
                    }
                    if (type == "RDWCV")
                    {
                        type = "RDW_CV";
                    }
                    if (type == "RDWSD")
                    {
                        type = "RDW_SD";
                    }
                    if (type == "GRAN")
                    {
                        type = "GRANN";
                    }
                    string chinaName = type + "中文名";
                    hy.Add(chinaName, dr[0]["chinaName"].ToString());

                    string CheckMethod = type + "检验方法";
                    hy.Add(CheckMethod, dr[0]["CheckMethod"].ToString());

                    string unit = type + "单位";
                    hy.Add(unit, dr[0]["unit"].ToString());

                    string fanwei = type + "参考范围";
                    string tmp = dr[0]["warning_min"].ToString() + "-" + dr[0]["warning_max"].ToString();
                    hy.Add(fanwei, tmp);
                }
            }
            catch
            { }
            return hy;
        }

        private string GetTiShiForShHa(DataTable dtSh, string type,string strValus)
        {
            string tishi = "";
            try
            {
                DataRow[] dr = dtSh.Select("type='" + type + "'");
                if (dr != null)
                {
                    double warning_min = double.Parse(dr[0]["warning_min"].ToString());
                    double warning_max = double.Parse(dr[0]["warning_max"].ToString());
                    double valuedouble = 0;
                    if (isDouble(strValus, out valuedouble))
                    {
                        if(valuedouble> warning_max)
                        {
                            tishi = "↑";
                        }
                        else if(valuedouble< warning_min)
                        {
                            tishi = "↓";
                        }
                        else
                        {
                            tishi = "-1";
                        }
                    }
                }
            }
            catch
            {

            }
            return tishi;
        }
          
        private Document PdfProcessing(string lx, DataRow data, string barcode)
        {
            DateTime date = DateTime.Now;
            Document doc = null;
            DocumentBuilder builder = null;
            DataTable jkdata = null;
            DataSet jk = null;
            if (barcode == "")
            {
                jk = DbHelperMySQL.Query($"select * from physical_examination_record where id_number='{data["id_number"].ToString()}' order by create_time desc LIMIT 1");
            }
            else
            {
                jk = DbHelperMySQL.Query($"select * from physical_examination_record where bar_code='{barcode}' and id_number='{data["id_number"].ToString()}' order by create_time desc LIMIT 1");
            }
            if (jk != null && jk.Tables.Count > 0 && jk.Tables[0].Rows.Count > 0)
            {
                jkdata = jk.Tables[0];
            }
            switch (lx)
            {
                #region 封面
                case "封面":
                    doc = new Document(@str + $"/up/template/封面1.doc");
                    builder = new DocumentBuilder(doc);
                    var dic = new Dictionary<string, string>();
                    string bh = data["archive_no"].ToString();
                    for (int i = 0; i < bh.Length; i++)
                    {
                        dic.Add("编号" + (i + 1), bh[i].ToString());
                    }
                    string photo_pic = data["photo_code"].ToString();
                    if (photo_pic != null && !"".Equals(photo_pic) && File.Exists(@str + @"\photoImg\" + photo_pic))
                    {
                        builder.MoveToBookmark("图片");
                        builder.InsertImage(resizeImageFromFile(@str + @"\photoImg\" + photo_pic, 172, 184));
                    }
                    dic.Add("姓名", data["name"].ToString());
                    //dic.Add("现住址", data["county_name"].ToString() + data["towns_name"].ToString() + data["village_name"].ToString());
                    if (!string.IsNullOrWhiteSpace(data["residence_address"].ToString()))
                    {
                        dic.Add("现住址", data["residence_address"].ToString()); 
                    }
                    else
                    {
                        dic.Add("现住址", data["county_name"].ToString() + data["towns_name"].ToString() + data["village_name"].ToString());
                    }
                    
                    dic.Add("户籍地址", data["address"].ToString());
                    dic.Add("联系电话", data["phone"].ToString());
                    dic.Add("乡镇名称", data["towns_name"].ToString());
                    dic.Add("村委会名称", data["village_name"].ToString());
                    if (!string.IsNullOrWhiteSpace(data["aichive_org"].ToString()))
                    {
                        dic.Add("建档单位", data["aichive_org"].ToString());
                        dic.Add("体检单位", data["aichive_org"].ToString());
                    }
                    if (!string.IsNullOrWhiteSpace(data["create_archives_name"].ToString()))
                    {
                        dic.Add("建档人", data["create_archives_name"].ToString());
                    }
                    if (!string.IsNullOrWhiteSpace(data["doctor_name"].ToString()))
                    {
                        dic.Add("责任医生", data["doctor_name"].ToString());
                    }
                    DateTime timecreate = Convert.ToDateTime(data["create_time"].ToString());
                    dic.Add("年", timecreate.Year.ToString());
                    dic.Add("月", timecreate.Month.ToString());
                    dic.Add("日", timecreate.Day.ToString());
                    dic.Add("姓名1", data["name"].ToString());
                    //if (!string.IsNullOrWhiteSpace(basicInfoSettings.organ_name))
                    //{
                    //    dic.Add("体检单位", basicInfoSettings.organ_name);
                    //}
                    //书签替换
                    foreach (var key in dic.Keys)
                    {
                        builder.MoveToBookmark(key);
                        builder.Write(dic[key]);
                    }
                    /*页码*/
                    //builder.MoveToHeaderFooter(HeaderFooterType.FooterPrimary); 
                    //builder.InsertField("PAGE", "");
                    //builder.Write(" / ");
                    //builder.InsertField("NUMPAGES", "");
                    ////builder.Write("页");
                    //builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                    /*end*/
                    break;
                #endregion

                #region 个人信息
                case "个人信息":
                    doc = new Document(@str + $"/up/template/个人信息档案.doc");
                    builder = new DocumentBuilder(doc);
                    var dics = new Dictionary<string, string>();
                    string grbh = data["archive_no"].ToString();
                    if (grbh != "" && grbh.Length > 9)
                    {
                        grbh = grbh.Substring(9, grbh.Length - 9);
                        for (int i = 0; i < grbh.Length; i++)
                        {
                            dics.Add("编号" + (i + 1), grbh[i].ToString());
                        }
                    }
                    dics.Add("姓名", data["name"].ToString());
                    dics.Add("性别", data["sex"].ToString());
                    string birthday = data["birthday"].ToString();
                    if (birthday != "")
                    {
                        string[] sr = birthday.Split('-');
                        string r = sr[0] + sr[1] + sr[2];
                        for (int i = 0; i < r.Length; i++)
                        {
                            dics.Add("出生日期" + (i + 1), r[i].ToString());
                        }
                    }
                    dics.Add("身份证号", data["id_number"].ToString());

                    dics.Add("工作单位", data["company"].ToString());
                    dics.Add("本人电话", data["phone"].ToString());
                    dics.Add("联系人姓名", data["link_name"].ToString());
                    dics.Add("联系人电话", data["link_phone"].ToString());
                    dics.Add("常住类型", data["resident_type"].ToString());
                    string nation = data["nation"].ToString();
                    if (nation != "" && !"".Equals(nation))
                    {
                        if (nation.Length == 1)
                        {
                            nation = "0" + nation;
                        }
                        if ("01".Equals(nation))
                        {
                            dics.Add("民族", nation);
                        }
                        else
                        {
                            dics.Add("民族", "99");
                            dics.Add("少数民族名称", Result.GetNationId(nation));
                        }
                    }
                    dics.Add("血型", data["blood_group"].ToString());
                    dics.Add("RH", data["blood_rh"].ToString());
                    dics.Add("文化程度", data["education"].ToString());
                    dics.Add("职业", data["profession"].ToString());
                    dics.Add("婚姻状况", data["marital_status"].ToString());
                    string yyf = data["pay_type"].ToString();
                    if (yyf.IndexOf(',') >= 0)
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
                    if (ywgm.IndexOf(',') >= 0)
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
                    dics.Add("药物过敏史其他", data["allergy_other"].ToString());
                    string bls = data["exposure"].ToString();
                    if (bls.IndexOf(',') >= 0)
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
                        DataRow[] fq = da.Select("relation='1'");
                        if (fq != null && fq.Count() > 0)
                        {
                            for (int j = 0; j < fq.Count(); j++)
                            {
                                string fqs = fq[j]["disease_type"].ToString();
                                if (fqs.IndexOf(',') >= 0)
                                {
                                    string[] y = fqs.Split(',');
                                    for (int i = 0; i < y.Length; i++)
                                    {
                                        dics.Add("家族史父亲" + (i + 1), y[i]);
                                    }
                                }
                            }
                        }
                        DataRow[] mq = da.Select("relation='2'");
                        if (mq != null && mq.Count() > 0)
                        {
                            for (int j = 0; j < mq.Count(); j++)
                            {
                                string fqs = mq[j]["disease_type"].ToString();
                                if (fqs.IndexOf(',') >= 0)
                                {
                                    string[] y = fqs.Split(',');
                                    for (int i = 0; i < y.Length; i++)
                                    {
                                        dics.Add("家族史母亲" + (i + 1), y[i]);
                                    }
                                }
                            }
                        }
                        DataRow[] jm = da.Select("relation='3'");
                        if (jm != null && jm.Count() > 0)
                        {

                            for (int j = 0; j < jm.Count(); j++)
                            {
                                string fqs = jm[j]["disease_type"].ToString();
                                if (fqs.IndexOf(',') >= 0)
                                {
                                    string[] y = fqs.Split(',');
                                    for (int i = 0; i < y.Length; i++)
                                    {
                                        dics.Add("家族史兄弟" + (i + 1), y[i]);
                                    }
                                }
                            }
                        }
                        DataRow[] zn = da.Select("relation='4'");
                        if (zn != null && zn.Count() > 0)
                        {
                            for (int j = 0; j < zn.Count(); j++)
                            {
                                string fqs = zn[j]["disease_type"].ToString();
                                if (fqs.IndexOf(',') >= 0)
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
                    if (cjqk.IndexOf(',') >= 0)
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
                    dics.Add("残疾其他", data["deformity_name"].ToString());
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

                    #region 心电图
                    DataSet hyxdts = DbHelperMySQL.Query($"select * from zkhw_tj_xdt where id_number='{data["id_number"].ToString()}' and bar_code='{barcode}' order by createtime desc LIMIT 1");
                    if (hyxdts != null && hyxdts.Tables.Count > 0 && hyxdts.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = hyxdts.Tables[0];
                        string XdtDesc = da.Rows[0]["XdtDesc"].ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("***", "").Replace("~", "");
                        hy.Add("心电图", XdtDesc);
                    }
                    #endregion

                    #region B超
                    DataSet bcs = DbHelperMySQL.Query($"select * from zkhw_tj_bc where id_number='{data["id_number"].ToString()}' and bar_code='{barcode}' order by createtime desc LIMIT 1");
                    if (bcs != null && bcs.Tables.Count > 0 && bcs.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = bcs.Tables[0];
                        string FubuResult = da.Rows[0]["FubuResult"].ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                        int intwz = FubuResult.IndexOf("结果");
                        if (intwz > -1)
                            hy.Add("B超诊断", FubuResult.Substring(intwz - 2));
                    }
                    if (jkdata != null && jkdata.Rows.Count > 0)
                    {
                        for (int j = 0; j < jkdata.Rows.Count; j++)
                        {
                            hy.Add("条码号", jkdata.Rows[j]["bar_code"].ToString());
                            hy.Add("条码号1", jkdata.Rows[j]["bar_code"].ToString());
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
                            hy.Add("送检日期", string.IsNullOrWhiteSpace(jkdata.Rows[j]["create_time"].ToString()) ? "" : Convert.ToDateTime(jkdata.Rows[j]["create_time"]).ToString("yyyy-MM-dd"));
                            hy.Add("审核", "");
                            hy.Add("审核1", "");
                            hy.Add("报告日期", DateTime.Now.ToString("yyyy-MM-dd"));
                            hy.Add("报告日期1", DateTime.Now.ToString("yyyy-MM-dd"));
                        }
                    }
                    #endregion

                    #region 生化
                   
                    grjdDao grjddao = new grjdDao();
                    DataTable dtSh = grjddao.checkThresholdValues("生化");
                    #region 生化内容
                    Dictionary<string, string>  tt=GetShHuaXCGDic(dtSh, "ALT");
                    if(tt.Count>0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "AST");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "CHO");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "CREA");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "GLU");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "HDLC");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "LDLC");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "TBIL");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "TG");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtSh, "UREA");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    #endregion
                    #region 生化处理值
                    DataSet sh = DbHelperMySQL.Query($"select * from zkhw_tj_sh where id_number='{data["id_number"].ToString()}' and bar_code='{barcode}' order by createtime desc LIMIT 1");
                    if (sh != null && sh.Tables.Count > 0 && sh.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = sh.Tables[0];
                        hy.Add("检验", da.Rows[0]["ZrysSH"].ToString());
                        for (int j = 0; j < da.Rows.Count; j++)
                        { 
                            string alt = da.Rows[j]["ALT"].ToString();
                            string tmp = GetTiShiForShHa(dtSh, "ALT", alt);
                            if(tmp !="")
                            {
                                if(tmp !="-1")
                                {
                                    hy.Add("ALT提示", tmp);
                                } 
                                hy.Add("ALT结果", alt);
                            }
                            
                            string ast = da.Rows[j]["AST"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "AST", ast);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("AST提示", tmp);
                                } 
                                hy.Add("AST结果", ast);
                            } 
                             
                            string cho = da.Rows[j]["CHO"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "CHO", cho);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("CHO提示", tmp);
                                } 
                                hy.Add("CHO结果", cho);
                            } 
                            string crea = da.Rows[j]["CREA"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "CREA", crea);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("CREA提示", tmp);
                                } 
                                hy.Add("CREA结果", crea);
                            } 
                            string glu = da.Rows[j]["GLU"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "GLU", glu);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("GLU提示", tmp);
                                } 
                                hy.Add("GLU结果", glu);
                            }
                             
                            string hdlc = da.Rows[j]["HDLC"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "HDLC", hdlc);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("HDL_C提示", tmp);
                                } 
                                hy.Add("HDL_C结果", hdlc);
                            }
                             
                            string ldlc = da.Rows[j]["LDLC"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "LDLC", ldlc);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("LDL_C提示", tmp);
                                } 
                                hy.Add("LDL_C结果", ldlc);
                            }
                             
                            string tbil = da.Rows[j]["TBIL"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "TBIL", tbil);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("TBIL提示", tmp);
                                } 
                                hy.Add("TBIL结果", tbil);
                            } 
                            string tg = da.Rows[j]["TG"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "TG", tg);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("TG提示", tmp);
                                } 
                                hy.Add("TG结果", tg);
                            } 
                            string tp = da.Rows[j]["TP"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "TP", tp);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("TP提示", tmp);
                                } 
                                hy.Add("TP结果", tp);
                            } 
                            string urea = da.Rows[j]["UREA"].ToString();
                            tmp = GetTiShiForShHa(dtSh, "UREA", urea);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("UREA提示", tmp);
                                } 
                                hy.Add("UREA结果", urea);
                            } 
                        }
                    }
                    #endregion
                    #endregion

                    #region 血常规
                    #region 内容
                    DataTable dtXcg = grjddao.checkThresholdValues("血常规");
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "HCT");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "HGB");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "LYM");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "LYMP");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "MCH");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "MCHC");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "MCV");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "MPV");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "MXD");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "MXDP");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "NEUT");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "NEUTP");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "PCT");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "PDW");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "PLT");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "RBC");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "RDWCV");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }

                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "RDWSD");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "WBC");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "GRAN");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "GRANP");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    tt.Clear();
                    tt = GetShHuaXCGDic(dtXcg, "PLCR");
                    if (tt.Count > 0)
                    {
                        foreach (var k in tt)
                        {
                            hy.Add(k.Key, k.Value);
                        }
                    }
                    #endregion
                    #region 值
                    DataSet xcg = DbHelperMySQL.Query($"select * from zkhw_tj_xcg where id_number='{data["id_number"].ToString()}' and bar_code='{barcode}' order by createtime desc LIMIT 1");
                    if (xcg != null && xcg.Tables.Count > 0 && xcg.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = xcg.Tables[0];
                        for (int j = 0; j < da.Rows.Count; j++)
                        {
                            string hct = da.Rows[j]["HCT"].ToString();
                            string tmp = GetTiShiForShHa(dtXcg, "HCT", hct);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("HCT提示", tmp);
                                } 
                                hy.Add("HCT结果", hct);
                            } 
                           
                            string hgb = da.Rows[j]["HGB"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "HGB", hgb);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("HGB提示", tmp);
                                } 
                                hy.Add("HGB结果", hgb);
                            } 
                             
                            string lym = da.Rows[j]["LYM"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "LYM", lym);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("LYMN提示", tmp);
                                } 
                                hy.Add("LYMN结果", lym);
                            }

                             
                            string lymp = da.Rows[j]["LYMP"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "LYMP", lymp);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("LYMP提示", tmp);
                                } 
                                hy.Add("LYMP结果", lymp);
                            } 

                            string mch = da.Rows[j]["MCH"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "MCH", mch);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("MCH提示", tmp);
                                } 
                                hy.Add("MCH结果", mch);
                            }
                            
                            string mchc = da.Rows[j]["MCHC"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "MCHC", mchc);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("MCHC提示", tmp);
                                } 
                                hy.Add("MCHC结果", mchc);
                            }
                             
                            string mcv = da.Rows[j]["MCV"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "MCV", mcv);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("MCV提示", tmp);
                                } 
                                hy.Add("MCV结果", mcv);
                            } 
                            string mpv = da.Rows[j]["MPV"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "MPV", mpv);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("MPV提示", tmp);
                                } 
                                hy.Add("MPV结果", mpv);
                            }
                             
                            string mxd = da.Rows[j]["MXD"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "MXD", mxd);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("MXDN提示", tmp);
                                } 
                                hy.Add("MXDN结果", mxd);
                            } 
                            string mxdp = da.Rows[j]["MXDP"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "MXDP", mxdp);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("MXDP提示", tmp);
                                } 
                                hy.Add("MXDP结果", mxdp);
                            } 
                            string neut = da.Rows[j]["NEUT"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "NEUT", neut);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("NEUTN提示", tmp);
                                } 
                                hy.Add("NEUTN结果", neut);
                            } 
                            string neutp = da.Rows[j]["NEUTP"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "NEUTP", neutp);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("NEUTP提示", tmp);
                                } 
                                hy.Add("NEUTP结果", neutp);
                            }
                             
                            string pct = da.Rows[j]["PCT"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "PCT", pct);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("PCT提示", tmp);
                                } 
                                hy.Add("PCT结果", pct);
                            }
                             
                            string pdw = da.Rows[j]["PDW"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "PDW", pdw);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("PDW提示", tmp);
                                } 
                                hy.Add("PDW结果", pdw);
                            }
                             
                            string plt = da.Rows[j]["PLT"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "PLT", plt);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("PLT提示", tmp);
                                } 
                                hy.Add("PLT结果", plt);
                            }
                             
                            string rbc = da.Rows[j]["RBC"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "RBC", rbc);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("RBC提示", tmp); 
                                } 
                                hy.Add("RBC结果", rbc);
                            }
                             
                            string rdwcv = da.Rows[j]["RDWCV"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "RDWCV", rdwcv);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("RDW_CV提示", tmp);
                                } 
                                hy.Add("RDW_CV结果", rdwcv);
                            } 
                            string rdwsd = da.Rows[j]["RDWSD"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "RDWSD", rdwsd);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("RDW_SD提示", tmp);
                                } 
                                hy.Add("RDW_SD结果", rdwsd);
                            } 
                             
                            string wbc = da.Rows[j]["WBC"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "WBC", wbc);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("WBC提示", tmp);
                                } 
                                hy.Add("WBC结果", wbc);
                            }

                            string gran = da.Rows[j]["GRAN"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "GRAN", gran);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("GRANN提示", tmp);
                                } 
                                hy.Add("GRANN结果", gran);
                            }

                            string granp = da.Rows[j]["GRANP"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "GRANP", granp);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("GRANP提示", tmp);
                                } 
                                hy.Add("GRANP结果", granp);
                            }

                            string plcr = da.Rows[j]["PLCR"].ToString();
                            tmp = GetTiShiForShHa(dtXcg, "PLCR", plcr);
                            if (tmp != "")
                            {
                                if (tmp != "-1")
                                {
                                    hy.Add("PLCR提示", tmp);
                                } 
                                hy.Add("PLCR结果", plcr);
                            }
                        }
                    }
                    #endregion
                    #endregion

                    #region 尿常规 
                    DataTable dtNCG = grjddao.checkThresholdValues("尿常规");

                    DataSet ncg = DbHelperMySQL.Query($"select * from zkhw_tj_ncg where id_number='{data["id_number"].ToString()}' and bar_code='{barcode}' order by createtime desc LIMIT 1");
                    if (ncg != null && ncg.Tables.Count > 0 && ncg.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = ncg.Tables[0];
                        hy.Add("白细胞结果", da.Rows[0]["WBC"].ToString());
                        hy.Add("酮体结果", da.Rows[0]["KET"].ToString());
                        hy.Add("亚硝酸盐结果", da.Rows[0]["NIT"].ToString());
                        hy.Add("尿胆原结果", da.Rows[0]["URO"].ToString());
                        hy.Add("胆红素结果", da.Rows[0]["BIL"].ToString());
                        hy.Add("蛋白质结果", da.Rows[0]["PRO"].ToString());
                        hy.Add("尿液葡萄糖结果", da.Rows[0]["GLU"].ToString());
                        string sg=da.Rows[0]["SG"].ToString();
                        string tmp = GetTiShiForShHa(dtNCG, "SG", sg);
                        if (tmp != "")
                        {
                            if (tmp != "-1")
                            {
                                hy.Add("尿比重箭头", tmp);
                            } 
                            hy.Add("尿比重结果", sg);
                        } 
                             
                        hy.Add("隐血结果", da.Rows[0]["BLD"].ToString());
                        
                        string ph = da.Rows[0]["PH"].ToString();
                        tmp = GetTiShiForShHa(dtNCG, "PH", ph);
                        if (tmp != "")
                        {
                            if (tmp != "-1")
                            {
                                hy.Add("酸碱度箭头", tmp);
                            } 
                            hy.Add("酸碱度结果", ph);
                        }
                         
                        hy.Add("维生素C结果", da.Rows[0]["Vc"].ToString());
                        hy.Add("检验1", da.Rows[0]["ZrysNCG"].ToString());
                        hy.Add("送检日期1", Convert.ToDateTime(da.Rows[0]["createtime"].ToString()).ToString("yyyy-MM-dd"));
                    } 
                    #endregion
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
                    if (jkbh != "" && jkbh.Length > 9)
                    {
                        jkbh = jkbh.Substring(9, jkbh.Length - 9);
                        for (int i = 0; i < jkbh.Length; i++)
                        {
                            jktj.Add("编号" + (i + 1), jkbh[i].ToString());
                        }
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
                            if (zz.IndexOf(',') >= 0)
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
                            jktj.Add("症状其他", jkdata.Rows[j]["symptom_other"].ToString());
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
                            if (ysxg.IndexOf(',') >= 0)
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
                            if (yjzl.IndexOf(',') >= 0)
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
                            jktj.Add("饮酒种类其他", jkdata.Rows[j]["lifeway_drink_other"].ToString());
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
                            jktj.Add("缺齿右上", jkdata.Rows[j]["organ_hypodontia_topright"].ToString());
                            jktj.Add("缺齿右下", jkdata.Rows[j]["organ_hypodontia_bottomright"].ToString());
                            jktj.Add("缺齿左上", jkdata.Rows[j]["organ_hypodontia_topleft"].ToString());
                            jktj.Add("缺齿左下", jkdata.Rows[j]["organ_hypodontia_bottomleft"].ToString());
                            jktj.Add("龋齿右上", jkdata.Rows[j]["organ_caries_topright"].ToString());
                            jktj.Add("龋齿右下", jkdata.Rows[j]["organ_caries_bottomright"].ToString());
                            jktj.Add("龋齿左上", jkdata.Rows[j]["organ_caries_topleft"].ToString());
                            jktj.Add("龋齿左下", jkdata.Rows[j]["organ_caries_bottomleft"].ToString());
                            jktj.Add("义齿右上", jkdata.Rows[j]["organ_denture_topright"].ToString());
                            jktj.Add("义齿右下", jkdata.Rows[j]["organ_denture_bottomright"].ToString());
                            jktj.Add("义齿左上", jkdata.Rows[j]["organ_denture_topleft"].ToString());
                            jktj.Add("义齿左下", jkdata.Rows[j]["organ_denture_bottomleft"].ToString());
                            string cl = jkdata.Rows[j]["organ_tooth"].ToString();
                            if (cl.IndexOf(',') >= 0)
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
                            jktj.Add("眼底异常", jkdata.Rows[j]["examination_eye_other"].ToString());
                            jktj.Add("皮肤", jkdata.Rows[j]["examination_skin"].ToString());
                            jktj.Add("皮肤其他", jkdata.Rows[j]["examination_skin_other"].ToString());
                            jktj.Add("巩膜", jkdata.Rows[j]["examination_sclera"].ToString());
                            jktj.Add("巩膜其他", jkdata.Rows[j]["examination_sclera_other"].ToString());
                            jktj.Add("淋巴结", jkdata.Rows[j]["examination_lymph"].ToString());
                            jktj.Add("淋巴结其他", jkdata.Rows[j]["examination_lymph_other"].ToString());
                            jktj.Add("桶状胸", jkdata.Rows[j]["examination_barrel_chest"].ToString());
                            jktj.Add("呼吸音", jkdata.Rows[j]["examination_breath_sounds"].ToString());
                            jktj.Add("呼吸音异常", jkdata.Rows[j]["examination_breath_other"].ToString());
                            jktj.Add("罗音", jkdata.Rows[j]["examination_rale"].ToString());
                            jktj.Add("罗音其他", jkdata.Rows[j]["examination_rale_other"].ToString());
                            jktj.Add("心率", jkdata.Rows[j]["examination_heart_rate"].ToString());
                            jktj.Add("心律", jkdata.Rows[j]["examination_heart_rhythm"].ToString());
                            jktj.Add("杂音", jkdata.Rows[j]["examination_heart_noise"].ToString());
                            jktj.Add("杂音有", jkdata.Rows[j]["examination_noise_other"].ToString());
                            jktj.Add("压痛", jkdata.Rows[j]["examination_abdomen_tenderness"].ToString());
                            jktj.Add("压痛有", jkdata.Rows[j]["examination_tenderness_memo"].ToString());
                            jktj.Add("包块", jkdata.Rows[j]["examination_abdomen_mass"].ToString());
                            jktj.Add("包块有", jkdata.Rows[j]["examination_mass_memo"].ToString());
                            jktj.Add("肝大", jkdata.Rows[j]["examination_abdomen_hepatomegaly"].ToString());
                            jktj.Add("肝大有", jkdata.Rows[j]["examination_hepatomegaly_memo"].ToString());
                            jktj.Add("脾大", jkdata.Rows[j]["examination_abdomen_splenomegaly"].ToString());
                            jktj.Add("脾大有", jkdata.Rows[j]["examination_splenomegaly_memo"].ToString());
                            jktj.Add("移动性浊音", jkdata.Rows[j]["examination_abdomen_shiftingdullness"].ToString());
                            jktj.Add("移动性浊音有", jkdata.Rows[j]["examination_shiftingdullness_memo"].ToString());
                            jktj.Add("下肢水肿", jkdata.Rows[j]["examination_lowerextremity_edema"].ToString());
                            jktj.Add("足背动脉搏动", jkdata.Rows[j]["examination_dorsal_artery"].ToString());
                            jktj.Add("肛门指诊", jkdata.Rows[j]["examination_anus"].ToString());
                            jktj.Add("肛门指诊其他", jkdata.Rows[j]["examination_anus_other"].ToString());
                            string lxx = jkdata.Rows[j]["examination_breast"].ToString();
                            if (lxx.IndexOf(',') >= 0)
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
                            jktj.Add("乳腺其他", jkdata.Rows[j]["examination_breast_other"].ToString());
                            jktj.Add("外阴", jkdata.Rows[j]["examination_woman_vulva"].ToString());
                            jktj.Add("外阴异常", jkdata.Rows[j]["examination_vulva_memo"].ToString());
                            jktj.Add("阴道", jkdata.Rows[j]["examination_woman_vagina"].ToString());
                            jktj.Add("阴道异常", jkdata.Rows[j]["examination_vagina_memo"].ToString());
                            jktj.Add("宫颈", jkdata.Rows[j]["examination_woman_cervix"].ToString());
                            jktj.Add("宫颈异常", jkdata.Rows[j]["examination_cervix_memo"].ToString());
                            jktj.Add("宫体", jkdata.Rows[j]["examination_woman_corpus"].ToString());
                            jktj.Add("宫体异常", jkdata.Rows[j]["examination_corpus_memo"].ToString());
                            jktj.Add("附件", jkdata.Rows[j]["examination_woman_accessories"].ToString());
                            jktj.Add("附件异常", jkdata.Rows[j]["examination_accessories_memo"].ToString());
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

                            jktj.Add("空腹血糖1", jkdata.Rows[j]["blood_glucose_mmol"].ToString());
                            jktj.Add("空腹血糖2", jkdata.Rows[j]["blood_glucose_mg"].ToString());
                            jktj.Add("心电图", jkdata.Rows[j]["cardiogram"].ToString());
                            jktj.Add("心电图异常", jkdata.Rows[j]["cardiogram_memo"].ToString());
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
                            jktj.Add("胸部X线片异常", jkdata.Rows[j]["chestx_memo"].ToString());
                            jktj.Add("腹部B超", jkdata.Rows[j]["ultrasound_abdomen"].ToString());
                            jktj.Add("腹部B超异常", jkdata.Rows[j]["ultrasound_memo"].ToString().Replace("|", "").Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
                            jktj.Add("B超其他", jkdata.Rows[j]["other_b"].ToString());
                            jktj.Add("B超其他异常", jkdata.Rows[j]["otherb_memo"].ToString());
                            jktj.Add("宫颈涂片", jkdata.Rows[j]["cervical_smear"].ToString());
                            jktj.Add("宫颈涂片异常", jkdata.Rows[j]["cervical_smear_memo"].ToString());
                            jktj.Add("辅助检查其它", jkdata.Rows[j]["other"].ToString());
                            string lxgjb = jkdata.Rows[j]["cerebrovascular_disease"].ToString();
                            if (lxgjb.IndexOf(',') >= 0)
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
                            jktj.Add("脑血管疾病其他", jkdata.Rows[j]["cerebrovascular_disease_other"].ToString());
                            string szjb = jkdata.Rows[j]["kidney_disease"].ToString();
                            if (szjb.IndexOf(',') >= 0)
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
                            jktj.Add("肾脏疾病其他", jkdata.Rows[j]["kidney_disease_other"].ToString());
                            string xzjb = jkdata.Rows[j]["heart_disease"].ToString();
                            if (xzjb.IndexOf(',') >= 0)
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
                            jktj.Add("心脏疾病其他", jkdata.Rows[j]["heart_disease_other"].ToString());
                            string xgjb = jkdata.Rows[j]["vascular_disease"].ToString();
                            if (xgjb.IndexOf(',') >= 0)
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
                            jktj.Add("血管疾病其他", jkdata.Rows[j]["vascular_disease_other"].ToString());
                            string ybjb = jkdata.Rows[j]["ocular_diseases"].ToString();
                            if (ybjb.IndexOf(',') >= 0)
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
                            jktj.Add("眼部疾病其他", jkdata.Rows[j]["ocular_diseases_other"].ToString());
                            jktj.Add("神经系统疾病", jkdata.Rows[j]["nervous_system_disease"].ToString());
                            jktj.Add("神经系统疾病有", jkdata.Rows[j]["nervous_disease_memo"].ToString());
                            jktj.Add("其他系统疾病", jkdata.Rows[j]["other_disease"].ToString());
                            jktj.Add("其他系统疾病有", jkdata.Rows[j]["other_disease_memo"].ToString());
                            DataSet zys = DbHelperMySQL.Query($"select * from hospitalized_record where archive_no='{data["archive_no"].ToString()}'");
                            if (zys != null && zys.Tables.Count > 0 && zys.Tables[0].Rows.Count > 0)
                            {
                                DataTable da = zys.Tables[0];
                                int a = 0;
                                int b = 0;
                                for (int k = 0; k < da.Rows.Count; k++)
                                {
                                    if (da.Rows[k]["hospitalized_type"].ToString() == "1")
                                    {
                                        a++;
                                        jktj.Add("住院入时间" + a, da.Rows[k]["in_hospital_time"].ToString());
                                        jktj.Add("住院出时间" + a, da.Rows[k]["leave_hospital_time"].ToString());
                                        jktj.Add("住院原因" + a, da.Rows[k]["reason"].ToString());
                                        jktj.Add("医疗机构" + a, da.Rows[k]["hospital_organ"].ToString());
                                        jktj.Add("病案号" + a, da.Rows[k]["case_code"].ToString());
                                    }
                                    else if (da.Rows[k]["hospitalized_type"].ToString() == "2")
                                    {
                                        b++;
                                        jktj.Add("家庭病床建" + b, da.Rows[k]["in_hospital_time"].ToString());
                                        jktj.Add("家庭病床撤" + b, da.Rows[k]["leave_hospital_time"].ToString());
                                        jktj.Add("家庭病床原因" + b, da.Rows[k]["reason"].ToString());
                                        jktj.Add("家庭病床医疗机构" + b, da.Rows[k]["hospital_organ"].ToString());
                                        jktj.Add("家庭病床病案号" + b, da.Rows[k]["case_code"].ToString());
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
                                    jktj.Add("预防接种机构" + (k + 1), da.Rows[k]["vaccination_organ_name"].ToString());
                                }
                            }
                            jktj.Add("健康评价", jkdata.Rows[j]["health_evaluation"].ToString());
                            jktj.Add("健康评价异常1", jkdata.Rows[j]["abnormal1"].ToString());
                            jktj.Add("健康评价异常2", jkdata.Rows[j]["abnormal2"].ToString());
                            jktj.Add("健康评价异常3", jkdata.Rows[j]["abnormal3"].ToString());
                            jktj.Add("健康评价异常4", jkdata.Rows[j]["abnormal4"].ToString());
                            string jkzd = jkdata.Rows[j]["health_guidance"].ToString();
                            if (jkzd.IndexOf(',') >= 0)
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
                            if (wxyskz.IndexOf(',') >= 0)
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
                            jktj.Add("建议接种疫苗", jkdata.Rows[j]["advise_bacterin"].ToString());
                            jktj.Add("危险因素控制其他", jkdata.Rows[j]["danger_controlling_other"].ToString());
                            jktj.Add("健康建议", jkdata.Rows[j]["healthAdvice"].ToString());
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
                    DataSet xdts = DbHelperMySQL.Query($"select * from zkhw_tj_xdt where id_number='{data["id_number"].ToString()}'  and bar_code='{barcode}' order by createtime desc LIMIT 1");
                    if (xdts != null && xdts.Tables.Count > 0 && xdts.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = xdts.Tables[0];
                        builder.MoveToBookmark("图片");
                        string imageUrl = da.Rows[0]["imageUrl"].ToString();
                        if (imageUrl != null && !"".Equals(imageUrl) && File.Exists(@str + "/xdtImg/" + imageUrl))
                        {
                            builder.InsertImage(resizeImageFromFile(@str + "/xdtImg/" + imageUrl, 678, 960));
                        }
                        xdt.Add("条码号", da.Rows[0]["bar_code"].ToString());
                        xdt.Add("诊断医师", da.Rows[0]["XdtDoctor"].ToString());
                        xdt.Add("诊断意见", da.Rows[0]["XdtDesc"].ToString());
                        xdt.Add("检查时间", Convert.ToDateTime(da.Rows[0]["createtime"].ToString()).ToString("yyyy-MM-dd"));
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
                    DataSet bcss = DbHelperMySQL.Query($"select * from zkhw_tj_bc where id_number='{data["id_number"].ToString()}' and  bar_code='{barcode}' order by createtime desc LIMIT 1");
                    if (bcss != null && bcss.Tables.Count > 0 && bcss.Tables[0].Rows.Count > 0)
                    {
                        DataTable da = bcss.Tables[0];
                        string BuPic01 = da.Rows[0]["BuPic01"].ToString();
                        if (BuPic01 != null && !"".Equals(BuPic01) && File.Exists(@str + @"\bcImg\" + BuPic01))
                        {
                            builder.MoveToBookmark("图1");
                            builder.InsertImage(resizeImageFromFile(@str + @"\bcImg\" + BuPic01, 400, 650));
                        }
                        string BuPic02 = da.Rows[0]["BuPic02"].ToString();
                        if (BuPic02 != null && !"".Equals(BuPic02) && File.Exists(@str + @"\bcImg\" + BuPic02))
                        {
                            builder.MoveToBookmark("图2");
                            builder.InsertImage(resizeImageFromFile(@str + @"\bcImg\" + BuPic02, 400, 650));
                        }
                        //string BuPic03 = da.Rows[j]["BuPic03"].ToString();
                        //if (BuPic03 != null && !"".Equals(BuPic03) && File.Exists(@str + @"\bcImg\" + BuPic03))
                        //{
                        //    builder.MoveToBookmark("图3");
                        //    builder.InsertImage(resizeImageFromFile(@str + @"\bcImg\" + BuPic03, 400, 650));
                        //}
                        //string BuPic04 = da.Rows[j]["BuPic04"].ToString();
                        //if (BuPic04 != null && !"".Equals(BuPic04) && File.Exists(@str + @"\bcImg\" + BuPic04))
                        //{
                        //    builder.MoveToBookmark("图4");
                        //    builder.InsertImage(resizeImageFromFile(@str + @"\bcImg\" + BuPic04, 400, 650));
                        //}
                        bc.Add("条码号", da.Rows[0]["bar_code"].ToString());
                        bc.Add("诊断医师", da.Rows[0]["ZrysBC"].ToString());
                        //bc.Add("检查所见", da.Rows[j]["FubuDesc"].ToString());
                        bc.Add("诊断结果", da.Rows[0]["FubuResult"].ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
                        bc.Add("检查时间", Convert.ToDateTime(da.Rows[0]["createtime"].ToString()).ToString("yyyy-MM-dd"));
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
                        grjdDao grjddao1 = new grjdDao();
                        DataTable dtSh1= grjddao1.checkThresholdValues("生化");
                        DataTable dtXcg1 = grjddao1.checkThresholdValues("血常规");
                        rangeJudgeForSHInfo.dttv = dtSh1;
                        rangeJudgeForXCGInfo.dttv = dtXcg1;
                        for (int j = 0; j < jkdata.Rows.Count; j++)
                        {

                            string sm = string.Empty;
                            int flagxcg = 0;
                            //jktj.Add("血红蛋白", 
                            string blood_hemoglobin = jkdata.Rows[j]["blood_hemoglobin"].ToString();
                            string tmp=rangeJudgeForXCGInfo.GetItemResultForValue("HGB", blood_hemoglobin);
                            if(tmp !="")
                            {
                                sm += tmp;
                                flagxcg = 1;
                            }
                            
                            //jktj.Add("血小板", 
                            string blood_platelet = jkdata.Rows[j]["blood_platelet"].ToString();
                            tmp = rangeJudgeForXCGInfo.GetItemResultForValue("PLT", blood_platelet);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagxcg = 1;
                            } 
                            //jktj.Add("白细胞", 
                            string blood_leukocyte = jkdata.Rows[j]["blood_leukocyte"].ToString();
                            tmp = rangeJudgeForXCGInfo.GetItemResultForValue("WBC", blood_leukocyte);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagxcg = 1;
                            } 
                            if (flagxcg == 1) sm += "\r\n";
                            int flagncg = 0;
                            //jktj.Add("尿蛋白", 
                            string urine_protein = jkdata.Rows[j]["urine_protein"].ToString();
                            if (!string.IsNullOrWhiteSpace(urine_protein))
                            {
                                if (urine_protein != "-")
                                {
                                    sm += @"尿蛋白：" + urine_protein + "   ";
                                    flagncg = 1;
                                }
                            }
                            //jktj.Add("尿糖", 
                            string glycosuria = jkdata.Rows[j]["glycosuria"].ToString();
                            if (!string.IsNullOrWhiteSpace(glycosuria))
                            {
                                if (glycosuria != "-")
                                {
                                    sm += @"尿糖：" + glycosuria + "   ";
                                    flagncg = 1;
                                }
                            }
                            //jktj.Add("尿酮体", 
                            string urine_acetone_bodies = jkdata.Rows[j]["urine_acetone_bodies"].ToString();
                            if (!string.IsNullOrWhiteSpace(urine_acetone_bodies))
                            {
                                if (urine_acetone_bodies != "-")
                                {
                                    sm += @"尿酮体：" + urine_acetone_bodies + "   ";
                                    flagncg = 1;
                                }
                            }
                            //jktj.Add("尿潜血", 
                            string bld = jkdata.Rows[j]["bld"].ToString();
                            if (!string.IsNullOrWhiteSpace(bld))
                            {
                                if (bld != "-")
                                {
                                    sm += @"尿潜血：" + bld + "   ";
                                    flagncg = 1;
                                }
                            }
                            if (flagncg == 1) sm += "\r\n";
                            int flagsh = 0;
                            //jktj.Add("空腹血糖1", 
                            int bgm = 0;
                            string blood_glucose_mmol = jkdata.Rows[j]["blood_glucose_mmol"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("GLU", blood_glucose_mmol);
                            if (tmp != "")
                            {
                                sm += tmp;
                                bgm = 1;
                                flagsh = 1;
                            }
                             
                            //jktj.Add("血清谷丙转氨酶", 
                            string sgft = jkdata.Rows[j]["sgft"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("ALT", sgft);
                            if (tmp != "")
                            {
                                sm += tmp; 
                                flagsh = 1;
                            } 
                            //jktj.Add("血清谷草转氨酶",
                            string ast = jkdata.Rows[j]["ast"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("AST", ast);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagsh = 1;
                            } 
                            //jktj.Add("白蛋白",
                            string albumin = jkdata.Rows[j]["albumin"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("ALB", albumin);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagsh = 1;
                            } 
                            //jktj.Add("总胆红素", 
                            string total_bilirubin = jkdata.Rows[j]["total_bilirubin"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("TBIL", total_bilirubin);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagsh = 1;
                            } 
                             
                            //jktj.Add("直接胆红素", 
                            string conjugated_bilirubin = jkdata.Rows[j]["conjugated_bilirubin"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("DBIL", conjugated_bilirubin);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagsh = 1;
                            } 

                            //jktj.Add("血清肌酐", 
                            string scr = jkdata.Rows[j]["scr"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("CREA", scr);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagsh = 1;
                            } 
                            //jktj.Add("尿素", 
                            string blood_urea = jkdata.Rows[j]["blood_urea"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("UREA", blood_urea);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagsh = 1;
                            } 
                            //jktj.Add("总胆固醇", 
                            string tc = jkdata.Rows[j]["tc"].ToString();
                            //jktj.Add("甘油三酯", 
                            string tg = jkdata.Rows[j]["tg"].ToString();
                            int flg = 0;
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("CHO", tc);
                            if (tmp != "")
                            {
                                if(tmp.IndexOf("偏高")>=0)
                                {
                                    flg = 1;
                                    sm += tmp;
                                    flagsh = 1;
                                } 
                            }

                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("TG", tg);
                            if (tmp != "")
                            {
                                if (tmp.IndexOf("偏高") >= 0)
                                {
                                    flg = 1;
                                    sm += tmp;
                                    flagsh = 1;
                                }
                            } 
                            //jktj.Add("低密度脂蛋白", 
                            string ldl = jkdata.Rows[j]["ldl"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("LDLC", ldl);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagsh = 1;
                            }
                            
                            //jktj.Add("高密度脂蛋白", 
                            string hdl = jkdata.Rows[j]["hdl"].ToString();
                            tmp = rangeJudgeForSHInfo.GetItemResultForValue("HDLC", hdl);
                            if (tmp != "")
                            {
                                sm += tmp;
                                flagsh = 1;
                            } 
                            if (flagsh == 1) sm += "\r\n";

                            int gyyfiag = 0;
                            //string bbplh = jkdata.Rows[j]["base_blood_pressure_left_high"].ToString();
                            //if (!string.IsNullOrWhiteSpace(bbplh) && !string.IsNullOrWhiteSpace(jkdata.Rows[j]["base_blood_pressure_left_low"].ToString()))
                            //{
                            //    if (Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_left_high"].ToString()) > 140 || Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_left_low"].ToString()) > 90)
                            //    {
                            //        sm += "血压值偏高(左)：" + Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_left_high"]) + "/" + Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_left_low"]);
                            //        gyyfiag = 1;
                            //    }
                            //}
                            //else 
                            if (!string.IsNullOrWhiteSpace(jkdata.Rows[j]["base_blood_pressure_right_high"].ToString()) || !string.IsNullOrWhiteSpace(jkdata.Rows[j]["base_blood_pressure_right_low"].ToString()))
                            {
                                if (Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_right_high"]) > 140 || Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_right_low"]) > 90)
                                {
                                    sm += "血压值偏高：" + Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_right_high"]) + "/" + Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_right_low"]);
                                    gyyfiag = 1;
                                    sm += "\r\n";
                                }
                                else if (Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_right_high"]) < 90 || Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_right_low"]) < 60)
                                {
                                    sm += "血压值偏低：" + Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_right_high"]) + "/" + Convert.ToInt32(jkdata.Rows[j]["base_blood_pressure_right_low"]);
                                    gyyfiag = 2;
                                    sm += "\r\n";
                                }
                            }

                            if (jkdata.Rows[j]["cardiogram"].ToString() == "2")
                            {
                                sm += @"不正常心电图：" + jkdata.Rows[j]["cardiogram_memo"].ToString().Replace("~", "");
                                sm += "\r\n";
                            }

                            if (jkdata.Rows[j]["ultrasound_abdomen"].ToString() == "2")
                            {
                                sm += jkdata.Rows[j]["ultrasound_memo"].ToString();
                                sm += "\r\n";
                            }

                            string health_evaluation = jkdata.Rows[j]["health_evaluation"].ToString();
                            if (health_evaluation == "2")
                            {
                                sm += "健康评价：\r\n";
                                if (!string.IsNullOrWhiteSpace(jkdata.Rows[j]["abnormal1"].ToString()))
                                {
                                    sm += "异常1：" + jkdata.Rows[j]["abnormal1"].ToString() + "\r\n";
                                }
                                if (!string.IsNullOrWhiteSpace(jkdata.Rows[j]["abnormal2"].ToString()))
                                {
                                    sm += "异常2：" + jkdata.Rows[j]["abnormal2"].ToString() + "\r\n";
                                }
                                if (!string.IsNullOrWhiteSpace(jkdata.Rows[j]["abnormal3"].ToString()))
                                {
                                    sm += "异常3：" + jkdata.Rows[j]["abnormal3"].ToString() + "\r\n";
                                }
                                if (!string.IsNullOrWhiteSpace(jkdata.Rows[j]["abnormal4"].ToString()))
                                {
                                    sm += "异常4：" + jkdata.Rows[j]["abnormal4"].ToString() + "\r\n";
                                }
                            }
                            sm += "健康建议：建议复查,医院就诊,明确诊断。";
                            sm += "\r\n";
                            if (gyyfiag == 1)
                            {
                                sm += "高血压健康指导：\r\n";
                                //sm += @"    高血压是指以体循环动脉血压（收缩压和/或舒张压）增高为主要特征（收缩压≥140毫米汞柱，舒张压≥90毫米汞柱），可伴有心、脑、肾等器官的功能或器质性损害的临床综合征。高血压是最常见的慢性病，也是心脑血管病最主要的危险因素。";
                                //sm += "\r\n健康指导：\r\n";
                                sm += @"    改善生活行为，减轻并控制体重、少盐少脂，增加运动、戒烟限酒、减轻精神压力、保持心理平衡。高血压患者应用药物控制血压。应定期随访和测量血压，预防心脑肾并发症的发生，降低心脑血管事件的发生率。";
                                sm += "\r\n";
                            }
                            //                            else if (gyyfiag == 2)
                            //                            {
                            //                                sm += "低血压：\r\n";
                            //                                sm += @"    低血压是指以体循环动脉血压（收缩压和/或舒张压）降低为主要特征（收缩压≤90毫米汞柱，舒张压≤60毫米汞柱），可伴有心、脑、肾等器官的功能或器质性损害的临床综合征。
                            //健康指导：改善生活行为：减轻并控制体重、少盐少脂，增加运动、戒烟限酒、减轻精神压力、保持心理平衡。低血压患者应生活要有规律，防止过度疲劳，因为过度疲劳会使血压下降，并且要保持良好的精神状态。";
                            //                                sm += "\r\n";
                            //                            }

                            if (flg == 1)
                            {
                                sm += "高血脂健康指导：\r\n";
                                //sm += @"    高血脂是体内脂类代谢紊乱，导致血脂水平增高，并由此引发一系列临床病理表现的病症。高脂血症可引发许多疾病，是形成冠心病的主要因素之一。";
                                //sm += "\r\n健康指导：\r\n";
                                sm += @"     高血脂症注意清淡膳食，粗细搭配。常吃蔬菜、豆类及其制品，适量吃鱼、禽、瘦肉，减少脂肪和盐摄入，戒烟限酒，合理增加运动。血脂高的人群最好每年常规化验一次血脂，及时应用药物进行系统治疗。";
                                sm += "\r\n";
                            }
                            //if (xdtfiag==1) {
                            //    sm += "健康指导：注意锻炼身体，保持良好的生活习惯，注意饮食，不吸烟避免饮酒及高脂饮食。";
                            //    sm += "\r\n";
                            //}
                            if (bgm == 1)
                            {
                                sm += "糖尿病健康指导：\r\n";
                                //sm += "    糖尿病是一种常见的代谢障碍疾病，即血糖（葡萄糖）升高，接着从尿液中流走，所以尿里有糖。具体讲，与过多摄入总热能、脂肪、碳水化合物，少运动，营养过剩有关，故被谑称为“富贵病”。若病势控制不好，日后就会引起并发症，如心脏病、冠心病、脑血管病、视网膜血管病，肾动脉硬化、肢体动脉硬化等。";
                                //sm += "\r\n健康指导：\r\n";
                                sm += "    糖尿病患者要在医生的指导下，增强控制好血糖的信心。定期监测血糖指标，改变生活习惯和方式，药物治疗和锻炼相结合，适当增加运动锻炼，循序渐进。戒烟戒酒，控制饮食（低热量），低盐低脂，优质蛋白，控制碳水化合物，补足维生素，保持情绪稳定。";
                                sm += "\r\n";
                            }
                            string healthAdvice = jkdata.Rows[j]["healthAdvice"].ToString();
                            if (!string.IsNullOrWhiteSpace(healthAdvice))
                            {
                                sm += "健康建议：" + healthAdvice;
                            }
                            jg.Add("结果", sm);
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

                #region 老年人生活自理能力评估   //这样子查找有问题
                case "老年人生活自理能力评估":
                    doc = new Document(@str + $"/up/template/老年人生活自理能力评估.doc");
                    builder = new DocumentBuilder(doc);
                    var zlpg = new Dictionary<string, string>();
                    if (jkdata != null && jkdata.Rows.Count > 0)
                    {
                        for (int h = 0; h < jkdata.Rows.Count; h++)
                        {
                            string _id = jkdata.Rows[h]["id"].ToString();
                            DataSet zlpgs = null;
                            if (_id=="")
                            {
                                zlpgs = DbHelperMySQL.Query($"select * from elderly_selfcare_estimate where id_number='{data["id_number"].ToString()}' order by create_time desc LIMIT 1");
                            }
                            else
                            {
                                zlpgs = DbHelperMySQL.Query($"select * from elderly_selfcare_estimate where exam_id='{_id}' order by create_time desc LIMIT 1");
                            }
                            if (zlpgs != null && zlpgs.Tables.Count > 0 && zlpgs.Tables[0].Rows.Count > 0)
                            {
                                DataTable da = zlpgs.Tables[0];
                                for (int j = 0; j < da.Rows.Count; j++)
                                {
                                    string zz = da.Rows[j]["answer_result"].ToString();
                                    if (zz.IndexOf(',') >= 0)
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
                    if (zybh != "" && zybh.Length > 9)
                    {
                        zybh = zybh.Substring(9, zybh.Length - 9);
                        for (int i = 0; i < zybh.Length; i++)
                        {
                            zytz.Add("编号" + (i + 1), zybh[i].ToString());
                        }
                    }
                    zytz.Add("姓名", data["name"].ToString());
                    if (jkdata != null && jkdata.Rows.Count > 0)
                    {
                        for (int h = 0; h < jkdata.Rows.Count; h++)
                        {
                            string _id = jkdata.Rows[h]["id"].ToString();
                            DataSet zytzs = null;
                            if (_id=="")
                            {
                                zytzs = DbHelperMySQL.Query($"select * from elderly_tcm_record where id_number='{data["id_number"].ToString()}' order by create_time desc LIMIT 1");
                            }
                            else
                            {
                                zytzs = DbHelperMySQL.Query($"select * from elderly_tcm_record where exam_id='{_id}' order by create_time desc LIMIT 1");
                            }
                            if (zytzs != null && zytzs.Tables.Count > 0 && zytzs.Tables[0].Rows.Count > 0)
                            {
                                DataTable da = zytzs.Tables[0];
                                for (int j = 0; j < da.Rows.Count; j++)
                                {
                                    string[] zz = da.Rows[j]["answer_result"].ToString().Split('|');
                                    for (int i = 0; i < zz.Length; i++)
                                    {
                                        int aa = Int32.Parse(zz[i].Split(':')[0]) - 1;
                                        zytz.Add("a" + aa + zz[i].Split(':')[1], "√");
                                    }
                                    int qz = 0;
                                    qz = Convert.ToInt32(da.Rows[j]["qixuzhi_score"]);
                                    zytz.Add("气虚质得分", qz.ToString());
                                    if (da.Rows[j]["qixuzhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("气虚质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("气虚质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("气虚质" + qx, "√");
                                        }
                                    }

                                    qz = Convert.ToInt32(da.Rows[j]["yangxuzhi_score"]);
                                    zytz.Add("阳虚质得分", qz.ToString());
                                    if (da.Rows[j]["yangxuzhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("阳虚质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("阳虚质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("阳虚质" + qx, "√");
                                        }
                                    }

                                    qz = Convert.ToInt32(da.Rows[j]["yinxuzhi_score"]);
                                    zytz.Add("阴虚质得分", qz.ToString());
                                    if (da.Rows[j]["yinxuzhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("阴虚质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("阴虚质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("阴虚质" + qx, "√");
                                        }
                                    }

                                    qz = Convert.ToInt32(da.Rows[j]["tanshizhi_score"]);
                                    zytz.Add("痰湿质得分", qz.ToString());
                                    if (da.Rows[j]["tanshizhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("痰湿质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("痰湿质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("痰湿质" + qx, "√");
                                        }
                                    }

                                    qz = Convert.ToInt32(da.Rows[j]["shirezhi_score"]);
                                    zytz.Add("湿热质得分", qz.ToString());
                                    if (da.Rows[j]["shirezhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("湿热质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("湿热质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("湿热质" + qx, "√");
                                        }
                                    }

                                    qz = Convert.ToInt32(da.Rows[j]["xueyuzhi_score"]);
                                    zytz.Add("血瘀质得分", qz.ToString());
                                    if (da.Rows[j]["xueyuzhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("血瘀质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("血瘀质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("血瘀质" + qx, "√");
                                        }
                                    }

                                    qz = Convert.ToInt32(da.Rows[j]["qiyuzhi_score"]);
                                    zytz.Add("气郁质得分", qz.ToString());
                                    if (da.Rows[j]["qiyuzhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("气郁质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("气郁质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("气郁质" + qx, "√");
                                        }
                                    }

                                    qz = Convert.ToInt32(da.Rows[j]["tebingzhi_sorce"]);
                                    zytz.Add("特禀质得分", qz.ToString());
                                    if (da.Rows[j]["tebingzhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("特禀质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("特禀质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("特禀质" + qx, "√");
                                        }
                                    }

                                    qz = Convert.ToInt32(da.Rows[j]["pinghezhi_sorce"]);
                                    zytz.Add("平和质得分", qz.ToString());
                                    if (da.Rows[j]["pinghezhi_result"].ToString() == "1")
                                    {
                                        zytz.Add("平和质是", "√");
                                        string qx = da.Rows[j]["tcm_guidance"].ToString();
                                        if (qx.IndexOf(',') >= 0)
                                        {
                                            string[] y = qx.Split(',');
                                            for (int i = 0; i < y.Length; i++)
                                            {
                                                zytz.Add("平和质" + y[i], "√");
                                            }
                                        }
                                        else
                                        {
                                            zytz.Add("平和质" + qx, "√");
                                        }
                                    }

                                    string time = da.Rows[j]["test_date"].ToString();
                                    zytz.Add("填表日期年", time?.Split('-')[0]);
                                    zytz.Add("填表日期月", time?.Split('-')[1]);
                                    zytz.Add("填表日期日", time?.Split('-')[2].Split(' ')[0]);
                                    zytz.Add("医生签名", da.Rows[j]["test_doctor"].ToString());
                                }
                            }
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
            string sql = $@"UPDATE zkhw_tj_bgdc set BaoGaoShengChan='{DateTime.Now.ToString("yyyy-MM-dd")}' where id_number='{data["id_number"].ToString()}'";
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

        private bool IsNumber(string strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            string strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            string strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                   !objTwoDotPattern.IsMatch(strNumber) &&
                   !objTwoMinusPattern.IsMatch(strNumber) &&
                   objNumberPattern.IsMatch(strNumber);
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
        /// 数据上传  2019-7-30  可以多次上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            LoadingHelper.myCaption = "正在上传...";
            LoadingHelper.myLabel = "正在上传数据";
            LoadingHelper.ShowLoadingScreen();

            try
            {
                List<ComboBoxData> ide = new List<ComboBoxData>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //string istb= dataGridView1["是否上传数据", i].Value.ToString();  //上传过的一样能上传
                    //if (istb=="是") {
                    //    continue;
                    //}
                    ComboBoxData combo = new ComboBoxData();
                    if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                    {
                        string id = dataGridView1["身份证号", i].Value.ToString();
                        combo.ID = "'" + id + "'";
                        ide.Add(combo);
                    }
                }
                if (ide.Count < 1)
                {
                    LoadingHelper.CloseForm();
                    MessageBox.Show("请选择要数据上传的人员!"); return;
                }
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
                string rtjkfwid = string.Empty;
                string xsrjtid = string.Empty;
                string zyjlid = string.Empty;
                string yyjlid = string.Empty;
                string ymjlid = string.Empty;
                string yfchid = string.Empty;
                string yfcqid = string.Empty;
                string yfdycid = string.Empty;
                string fjhdycid = string.Empty;
                string nlrzyyjkid = string.Empty;
                string fjhsfid = string.Empty;
                string wsjlid = string.Empty;
                string sxzbid = string.Empty;
                string ssjlid = string.Empty;
                string jbjlid = string.Empty;
                string jtbsid = string.Empty;
                string jsbgrid = string.Empty;
                string jsbsfid = string.Empty;
                string xsrtid = string.Empty;
                string pfrid = string.Empty;

                #region 健康扶贫信息
                DataSet infopf = DbHelperMySQL.Query($@"select * from poor_follow_record where (upload_status =0 or upload_status =2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (infopf != null && infopf.Tables.Count > 0 && infopf.Tables[0].Rows.Count > 0)
                {
                    DataTable datapf = infopf.Tables[0];
                    for (int i = 0; i < datapf.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From poor_follow_record where id={0}", Ifnull(datapf.Rows[i]["id"]));
                        sqllist.Add(sql);
                        sqllist.Add($@"insert into poor_follow_record(id,name,archive_no,id_number,visit_date,visit_type,sex,birthday,visit_doctor,work_info,advice,create_user,create_name,create_org,create_org_name,create_time) 
                                    values({Ifnull(datapf.Rows[i]["id"])},{Ifnull(datapf.Rows[i]["name"])},{Ifnull(datapf.Rows[i]["archive_no"])},{Ifnull(datapf.Rows[i]["id_number"])},{Ifnull(datapf.Rows[i]["visit_date"])},{Ifnull(datapf.Rows[i]["visit_type"])},{Ifnull(datapf.Rows[i]["sex"])},{Ifnull(datapf.Rows[i]["birthday"])},{Ifnull(datapf.Rows[i]["visit_doctor"])},{Ifnull(datapf.Rows[i]["work_info"])},{Ifnull(datapf.Rows[i]["advice"])},{Ifnull(datapf.Rows[i]["create_user"])},{Ifnull(datapf.Rows[i]["create_name"])},{Ifnull(datapf.Rows[i]["create_org"])},{Ifnull(datapf.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(datapf.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");

                        pfrid += $"'{datapf.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 个人信息
                DataSet info = DbHelperMySQL.Query($@"select * from resident_base_info where (upload_status =0 or upload_status =2) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (info != null && info.Tables.Count > 0 && info.Tables[0].Rows.Count > 0)
                {
                    DataTable data = info.Tables[0];
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string _residentbaseinfoid = data.Rows[i]["id"].ToString();
                        string _idnumber = data.Rows[i]["id_number"].ToString();
                        string _archiveno = data.Rows[i]["archive_no"].ToString();

                        DataSet wsjl = DbHelperMySQL.Query($@"select * from traumatism_record where resident_base_info_id='{_residentbaseinfoid}'");
                        if (wsjl != null && wsjl.Tables.Count > 0 && wsjl.Tables[0].Rows.Count > 0)
                        {
                            DataTable data1 = wsjl.Tables[0];
                            string id = string.Empty;
                            if (data1.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From traumatism_record where archive_no='{0}'", _archiveno);
                                sqllist.Add(sql);
                            }
                            for (int a = 0; a < data1.Rows.Count; a++)
                            {
                                string  _id= Result.GetNewId();
                                sqllist.Add($@"insert into traumatism_record (id,archive_no,id_number,traumatism_name,traumatism_time) 
                                values({Ifnull(_id)},{Ifnull(_archiveno)},{Ifnull(_idnumber)},{Ifnull(data1.Rows[a]["traumatism_name"])},{Ifnull(Convert.ToDateTime(data1.Rows[a]["traumatism_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");

                            }
                        }

                        DataSet sxzb = DbHelperMySQL.Query($@"select * from metachysis_record where resident_base_info_id='{_residentbaseinfoid}'");
                        if (sxzb != null && sxzb.Tables.Count > 0 && sxzb.Tables[0].Rows.Count > 0)
                        {
                            DataTable data2 = sxzb.Tables[0];
                            string id = string.Empty;
                            if (data2.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From metachysis_record where archive_no='{0}'", _archiveno);
                                sqllist.Add(sql);
                            }
                            for (int b = 0; b < data2.Rows.Count; b++)
                            {
                                string _id = Result.GetNewId();
                                sqllist.Add($@"insert into metachysis_record (id,archive_no,id_number,metachysis_reasonn,metachysis_time) 
                                    values({Ifnull(_id)},{Ifnull(_archiveno)},{Ifnull(_idnumber)},{Ifnull(data2.Rows[b]["metachysis_reasonn"])},{Ifnull(Convert.ToDateTime(data2.Rows[b]["metachysis_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                            }
                        }

                        DataSet ssjl = DbHelperMySQL.Query($@"select * from operation_record where resident_base_info_id='{_residentbaseinfoid}'");
                        if (ssjl != null && ssjl.Tables.Count > 0 && ssjl.Tables[0].Rows.Count > 0)
                        {
                            DataTable data3 = ssjl.Tables[0];
                            string id = string.Empty;
                            if (data3.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From operation_record where archive_no='{0}'", _archiveno);
                                sqllist.Add(sql);
                            }
                            for (int c = 0; c < data3.Rows.Count; c++)
                            {
                                string _id = Result.GetNewId();
                                sqllist.Add($@"insert into operation_record (id,archive_no,id_number,operation_name,operation_time) 
                                    values({Ifnull(_id)},{Ifnull(_archiveno)},{Ifnull(_idnumber)},{Ifnull(data3.Rows[c]["operation_name"])},{Ifnull(Convert.ToDateTime(data3.Rows[c]["operation_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                            }
                        }

                        DataSet jbjl = DbHelperMySQL.Query($@"select * from resident_diseases where resident_base_info_id='{_residentbaseinfoid}'");
                        if (jbjl != null && jbjl.Tables.Count > 0 && jbjl.Tables[0].Rows.Count > 0)
                        {
                            DataTable data4 = jbjl.Tables[0];
                            string id = string.Empty;
                            if (data4.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From resident_diseases where archive_no='{0}'", _archiveno);
                                sqllist.Add(sql);
                            }
                            for (int d = 0; d < data4.Rows.Count; d++)
                            {
                                string _id = Result.GetNewId();
                                sqllist.Add($@"insert into resident_diseases (id,archive_no,id_number,disease_type,disease_name,disease_date) 
                                    values({Ifnull(_id)},{Ifnull(_archiveno)},{Ifnull(_idnumber)},{Ifnull(data4.Rows[d]["disease_type"])},{Ifnull(data4.Rows[d]["disease_name"])},{Ifnull(data4.Rows[d]["disease_date"])});");
                            }
                        }

                        DataSet jtbs = DbHelperMySQL.Query($@"select * from family_record where resident_base_info_id='{_residentbaseinfoid}'");
                        if (jtbs != null && jtbs.Tables.Count > 0 && jtbs.Tables[0].Rows.Count > 0)
                        {
                            DataTable data5 = jtbs.Tables[0];
                            string id = string.Empty;
                            if (data5.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From family_record where archive_no='{0}'", _archiveno);
                                sqllist.Add(sql);
                            }
                            for (int f = 0; f < data5.Rows.Count; f++)
                            {
                                string _id = Result.GetNewId();
                                sqllist.Add($@"insert into family_record (id,archive_no,id_number,relation,disease_code,disease_name) 
                                    values({Ifnull(_id)},{Ifnull(_archiveno)},{Ifnull(_idnumber)},{Ifnull(data5.Rows[f]["relation"])},{Ifnull(data5.Rows[f]["disease_type"])},{Ifnull(data5.Rows[f]["disease_name"])});");
                            }
                        }
                        string sql1 = string.Format("Delete From resident_info_temp where id='{0}'", _residentbaseinfoid);
                        sqllist.Add(sql1);
                        sqllist.Add($@"insert into resident_info_temp (id,archive_no,pb_archive,name,sex,birthday,id_number,card_pic,company,phone,link_name,link_phone,resident_type,register_address,residence_address,nation,blood_group,blood_rh,education,profession,marital_status,pay_type,pay_other,drug_allergy,allergy_other,exposure,disease_other,is_hypertension,is_diabetes,is_psychosis,is_tuberculosis,is_heredity,heredity_name,is_deformity,deformity_name,is_poor,kitchen,fuel,other_fuel,drink,other_drink,toilet,poultry,medical_code,photo_code,aichive_org,doctor_name,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,status,remark,create_user,create_name,create_time,create_org,create_org_name
                            ) values({Ifnull(data.Rows[i]["id"])},{Ifnull(_archiveno)},{Ifnull(data.Rows[i]["pb_archive"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["sex"])},{Ifnull(data.Rows[i]["birthday"])},{Ifnull(_idnumber)},{Ifnull(data.Rows[i]["card_pic"])},{Ifnull(data.Rows[i]["company"])},{Ifnull(data.Rows[i]["phone"])},{Ifnull(data.Rows[i]["link_name"])},{Ifnull(data.Rows[i]["link_phone"])},{Ifnull(data.Rows[i]["resident_type"])},{Ifnull(data.Rows[i]["address"])},{Ifnull(data.Rows[i]["residence_address"])},{Ifnull(data.Rows[i]["nation"])},{Ifnull(data.Rows[i]["blood_group"])},{Ifnull(data.Rows[i]["blood_rh"])},{Ifnull(data.Rows[i]["education"])},{Ifnull(data.Rows[i]["profession"])},{Ifnull(data.Rows[i]["marital_status"])},{Ifnull(data.Rows[i]["pay_type"])},{Ifnull(data.Rows[i]["pay_other"])},{Ifnull(data.Rows[i]["drug_allergy"])},{Ifnull(data.Rows[i]["allergy_other"])},{Ifnull(data.Rows[i]["exposure"])},{Ifnull(data.Rows[i]["disease_other"])},{Ifnull(data.Rows[i]["is_hypertension"])},{Ifnull(data.Rows[i]["is_diabetes"])},{Ifnull(data.Rows[i]["is_psychosis"])},{Ifnull(data.Rows[i]["is_tuberculosis"])},{Ifnull(data.Rows[i]["is_heredity"])},{Ifnull(data.Rows[i]["heredity_name"])},{Ifnull(data.Rows[i]["is_deformity"])},{Ifnull(data.Rows[i]["deformity_name"])},{Ifnull(data.Rows[i]["is_poor"])},{Ifnull(data.Rows[i]["kitchen"])},{Ifnull(data.Rows[i]["fuel"])},{Ifnull(data.Rows[i]["other_fuel"])},{Ifnull(data.Rows[i]["drink"])},{Ifnull(data.Rows[i]["other_drink"])},{Ifnull(data.Rows[i]["toilet"])},{Ifnull(data.Rows[i]["poultry"])},{Ifnull(data.Rows[i]["medical_code"])},{Ifnull(data.Rows[i]["photo_code"])},{Ifnull(data.Rows[i]["aichive_org"])},{Ifnull(data.Rows[i]["doctor_name"])},{Ifnull(data.Rows[i]["province_code"])},{Ifnull(data.Rows[i]["province_name"])},{Ifnull(data.Rows[i]["city_code"])},{Ifnull(data.Rows[i]["city_name"])},{Ifnull(data.Rows[i]["county_code"])},{Ifnull(data.Rows[i]["county_name"])},{Ifnull(data.Rows[i]["towns_code"])},{Ifnull(data.Rows[i]["towns_name"])},{Ifnull(data.Rows[i]["village_code"])},{Ifnull(data.Rows[i]["village_name"])},{Ifnull(data.Rows[i]["status"])},{Ifnull(data.Rows[i]["remark"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])});");
                        infoid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 健康体检
                DataSet record = DbHelperMySQL.Query($@"select * from physical_examination_record where (upload_status=0 or upload_status=2) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (record != null && record.Tables.Count > 0 && record.Tables[0].Rows.Count > 0)
                {
                    DataTable data = record.Tables[0];
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string exam_id = data.Rows[i]["id"].ToString();
                        DataSet zyjl = DbHelperMySQL.Query($@"select * from hospitalized_record where exam_id='{exam_id}'");
                        if (zyjl != null && zyjl.Tables.Count > 0 && zyjl.Tables[0].Rows.Count > 0)
                        {
                            DataTable data1 = zyjl.Tables[0];
                            string id = string.Empty;
                            if (data1.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From hospitalized_record where exam_id='{0}'", exam_id);
                                sqllist.Add(sql);
                            }
                            for (int j = 0; j < data1.Rows.Count; j++)
                            {
                                sqllist.Add($@"insert into hospitalized_record (id,exam_id,archive_no,id_number,service_name,hospitalized_type,in_hospital_time,leave_hospital_time,reason,hospital_organ,case_code,remark,create_org,create_name,create_time) 
                                    values({Ifnull(data1.Rows[j]["id"])},{Ifnull(data1.Rows[j]["exam_id"])},{Ifnull(data1.Rows[j]["archive_no"])},{Ifnull(data1.Rows[j]["id_number"])},{Ifnull(data1.Rows[j]["service_name"])},{Ifnull(data1.Rows[j]["hospitalized_type"])},{Ifnull(data1.Rows[j]["in_hospital_time"])},{Ifnull(data1.Rows[j]["leave_hospital_time"])},{Ifnull(data1.Rows[j]["reason"])},{Ifnull(data1.Rows[j]["hospital_organ"])},{Ifnull(data1.Rows[j]["case_code"])},{Ifnull(data1.Rows[j]["remark"])},{Ifnull(data1.Rows[j]["create_org"])},{Ifnull(data1.Rows[j]["create_name"])},{Ifnull(Convert.ToDateTime(data1.Rows[j]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                                zyjlid += $"'{data1.Rows[j]["id"]}',";
                            }
                        }
                        DataSet yyjl = DbHelperMySQL.Query($@"select * from take_medicine_record where exam_id='{exam_id}'");
                        if (yyjl != null && yyjl.Tables.Count > 0 && yyjl.Tables[0].Rows.Count > 0)
                        {
                            DataTable data2 = yyjl.Tables[0];
                            string id = string.Empty;
                            if (data2.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From take_medicine_record where exam_id='{0}'", exam_id);
                                sqllist.Add(sql);
                            }
                            for (int k = 0; k < data2.Rows.Count; k++)
                            {
                                sqllist.Add($@"insert into take_medicine_record (id,exam_id,archive_no,id_number,service_name,medicine_type,medicine_name,medicine_usage,frequency,medicine_dosage,unit,medicine_time,medicine_time_unit,medicine_compliance,other,create_org,create_name,create_time) 
                                    values({Ifnull(data2.Rows[k]["id"])},{Ifnull(data2.Rows[k]["exam_id"])},{Ifnull(data2.Rows[k]["archive_no"])},{Ifnull(data2.Rows[k]["id_number"])},{Ifnull(data2.Rows[k]["service_name"])},{Ifnull(data2.Rows[k]["medicine_type"])},{Ifnull(data2.Rows[k]["medicine_name"])},{Ifnull(data2.Rows[k]["medicine_usage"])},{Ifnull(data2.Rows[k]["frequency"])},{Ifnull(data2.Rows[k]["medicine_dosage"])},{Ifnull(data2.Rows[k]["unit"])},{Ifnull(data2.Rows[k]["medicine_time"])},{Ifnull(data2.Rows[k]["medicine_time_unit"])},{Ifnull(data2.Rows[k]["medicine_compliance"])},{Ifnull(data2.Rows[k]["other"])},{Ifnull(data2.Rows[k]["create_org"])},{Ifnull(data2.Rows[k]["create_name"])},{Ifnull(Convert.ToDateTime(data2.Rows[k]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                                yyjlid += $"'{data2.Rows[k]["id"]}',";
                            }
                        }
                        DataSet ymjl = DbHelperMySQL.Query($@"select * from vaccination_record where exam_id='{exam_id}'");
                        if (ymjl != null && ymjl.Tables.Count > 0 && ymjl.Tables[0].Rows.Count > 0)
                        {
                            DataTable data3 = ymjl.Tables[0];
                            string id = string.Empty;
                            if (data3.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From vaccination_record where exam_id='{0}'", exam_id);
                                sqllist.Add(sql);
                            }
                            for (int a = 0; a < data3.Rows.Count; a++)
                            {
                                sqllist.Add($@"insert into vaccination_record (id,exam_id,archive_no,id_number,service_name,card_id,vaccination_type,vaccination_id,vaccination_name,acuscount,dose,descnption,inocu_state,sinocu_date,vaccination_time,inocu_doctor,register_person,dzjgm,batch_number,county,inoculation_site,inoculation_way,vaccination_organ,vaccination_organ_name,remark,validdate,manufacturer,manufact_code,create_name,create_time) 
                                    values({Ifnull(data3.Rows[a]["id"])},{Ifnull(data3.Rows[a]["exam_id"])},{Ifnull(data3.Rows[a]["archive_no"])},{Ifnull(data3.Rows[a]["id_number"])},{Ifnull(data3.Rows[a]["service_name"])},{Ifnull(data3.Rows[a]["card_id"])},{Ifnull(data3.Rows[a]["vaccination_type"])},{Ifnull(data3.Rows[a]["vaccination_id"])},{Ifnull(data3.Rows[a]["vaccination_name"])},{Ifnull(data3.Rows[a]["acuscount"])},{Ifnull(data3.Rows[a]["dose"])},{Ifnull(data3.Rows[a]["descnption"])},{Ifnull(data3.Rows[a]["inocu_state"])},{Ifnull(data3.Rows[a]["sinocu_date"])},{Ifnull(data3.Rows[a]["vaccination_time"])},{Ifnull(data3.Rows[a]["inocu_doctor"])},{Ifnull(data3.Rows[a]["register_person"])},{Ifnull(data3.Rows[a]["dzjgm"])},{Ifnull(data3.Rows[a]["batch_number"])},{Ifnull(data3.Rows[a]["county"])},{Ifnull(data3.Rows[a]["inoculation_site"])},{Ifnull(data3.Rows[a]["inoculation_way"])},{Ifnull(data3.Rows[a]["vaccination_organ"])},{Ifnull(data3.Rows[a]["vaccination_organ_name"])},{Ifnull(data3.Rows[a]["remark"])},{Ifnull(data3.Rows[a]["validdate"])},{Ifnull(data3.Rows[a]["manufacturer"])},{Ifnull(data3.Rows[a]["manufact_code"])},{Ifnull(data3.Rows[a]["create_name"])},{Ifnull(Convert.ToDateTime(data3.Rows[a]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                                ymjlid += $"'{data3.Rows[a]["id"]}',";
                            }
                        }

                        string sql1 = string.Format("Delete From physical_examination_record where id='{0}'", exam_id);
                        sqllist.Add(sql1);
                        sqllist.Add($@"insert into physical_examination_record (id,name,archive_no,id_number,batch_no,bar_code,dutydoctor,symptom,symptom_other,check_date,base_temperature,base_heartbeat,base_respiratory,base_blood_pressure_left_high,base_blood_pressure_left_low,base_blood_pressure_right_high,base_blood_pressure_right_low,base_height,base_weight,base_waist,base_bmi,base_health_estimate,base_selfcare_estimate,base_cognition_estimate,base_cognition_score,base_feeling_estimate,base_feeling_score,base_doctor,lifeway_exercise_frequency,lifeway_exercise_time,lifeway_exercise_year,lifeway_exercise_type,lifeway_diet,lifeway_smoke_status,lifeway_smoke_number,lifeway_smoke_startage,lifeway_smoke_endage,lifeway_drink_status,lifeway_drink_number,lifeway_drink_stop,lifeway_drink_stopage,lifeway_drink_startage,lifeway_drink_oneyear,lifeway_drink_type,lifeway_drink_other,lifeway_occupational_disease,lifeway_job,lifeway_job_period,lifeway_hazardous_dust,lifeway_dust_preventive,lifeway_hazardous_radiation,lifeway_radiation_preventive,lifeway_hazardous_physical,lifeway_physical_preventive,lifeway_hazardous_chemical,lifeway_chemical_preventive,lifeway_hazardous_other,lifeway_other_preventive,lifeway_doctor,organ_lips,organ_tooth,organ_hypodontia,organ_hypodontia_topleft,organ_hypodontia_topright,organ_hypodontia_bottomleft,organ_hypodontia_bottomright,organ_caries,organ_caries_topleft,organ_caries_topright,organ_caries_bottomleft,organ_caries_bottomright,organ_denture,organ_denture_topleft,organ_denture_topright,organ_denture_bottomleft,organ_denture_bottomright,organ_guttur,organ_vision_left,organ_vision_right,organ_correctedvision_left,organ_correctedvision_right,organ_hearing,organ_movement,organ_doctor,examination_eye,examination_eye_other,examination_skin,examination_skin_other,examination_sclera,examination_sclera_other,examination_lymph,examination_lymph_other,examination_barrel_chest,examination_breath_sounds,examination_breath_other,examination_rale,examination_rale_other,examination_heart_rate,examination_heart_rhythm,examination_heart_noise,examination_noise_other,examination_abdomen_tenderness,examination_tenderness_memo,examination_abdomen_mass,examination_mass_memo,examination_abdomen_hepatomegaly,examination_hepatomegaly_memo,examination_abdomen_splenomegaly,examination_splenomegaly_memo,examination_abdomen_shiftingdullness,examination_shiftingdullness_memo,examination_lowerextremity_edema,examination_dorsal_artery,examination_anus,examination_anus_other,examination_breast,examination_breast_other,examination_doctor,examination_woman_vulva,examination_vulva_memo,examination_woman_vagina,examination_vagina_memo,examination_woman_cervix,examination_cervix_memo,examination_woman_corpus,examination_corpus_memo,examination_woman_accessories,examination_accessories_memo,examination_woman_doctor,examination_other,blood_hemoglobin,blood_leukocyte,blood_platelet,blood_other,urine_protein,glycosuria,urine_acetone_bodies,bld,urine_other,blood_glucose_mmol,blood_glucose_mg,cardiogram,cardiogram_memo,cardiogram_img,microalbuminuria,fob,glycosylated_hemoglobin,hb,sgft,ast,albumin,total_bilirubin,conjugated_bilirubin,scr,blood_urea,blood_k,blood_na,tc,tg,ldl,hdl,chest_x,x_memo,chestx_img,ultrasound_abdomen,ultrasound_memo,abdomenB_img,other_b,otherb_memo,otherb_img,cervical_smear,cervical_smear_memo,other,cerebrovascular_disease,cerebrovascular_disease_other,kidney_disease,kidney_disease_other,heart_disease,heart_disease_other,vascular_disease,vascular_disease_other,ocular_diseases,ocular_diseases_other,nervous_system_disease,nervous_disease_memo,other_disease,other_disease_memo,health_evaluation,abnormal1,abnormal2,abnormal3,abnormal4,health_guidance,danger_controlling,target_weight,advise_bacterin,danger_controlling_other,health_advice,create_user,create_name,create_org,create_org_name,create_time)
                values('{data.Rows[i]["id"]}','{data.Rows[i]["name"]}','{data.Rows[i]["aichive_no"]}','{data.Rows[i]["id_number"]}','{data.Rows[i]["batch_no"]}','{data.Rows[i]["bar_code"]}','{data.Rows[i]["dutydoctor"]}','{data.Rows[i]["symptom"]}','{data.Rows[i]["symptom_other"]}','{data.Rows[i]["check_date"]}','{data.Rows[i]["base_temperature"]}','{data.Rows[i]["base_heartbeat"]}','{data.Rows[i]["base_respiratory"]}',{Ifnull(data.Rows[i]["base_blood_pressure_left_high"])},{Ifnull(data.Rows[i]["base_blood_pressure_left_low"])},{Ifnull(data.Rows[i]["base_blood_pressure_right_high"])},{Ifnull(data.Rows[i]["base_blood_pressure_right_low"])},'{data.Rows[i]["base_height"]}','{data.Rows[i]["base_weight"]}','{data.Rows[i]["base_waist"]}','{data.Rows[i]["base_bmi"]}','{data.Rows[i]["base_health_estimate"]}','{data.Rows[i]["base_selfcare_estimate"]}','{data.Rows[i]["base_cognition_estimate"]}','{data.Rows[i]["base_cognition_score"]}','{data.Rows[i]["base_feeling_estimate"]}','{data.Rows[i]["base_feeling_score"]}','{data.Rows[i]["base_doctor"]}','{data.Rows[i]["lifeway_exercise_frequency"]}','{data.Rows[i]["lifeway_exercise_time"]}','{data.Rows[i]["lifeway_exercise_year"]}','{data.Rows[i]["lifeway_exercise_type"]}','{data.Rows[i]["lifeway_diet"]}','{data.Rows[i]["lifeway_smoke_status"]}','{data.Rows[i]["lifeway_smoke_number"]}','{data.Rows[i]["lifeway_smoke_startage"]}','{data.Rows[i]["lifeway_smoke_endage"]}','{data.Rows[i]["lifeway_drink_status"]}','{data.Rows[i]["lifeway_drink_number"]}','{data.Rows[i]["lifeway_drink_stop"]}','{data.Rows[i]["lifeway_drink_stopage"]}','{data.Rows[i]["lifeway_drink_startage"]}','{data.Rows[i]["lifeway_drink_oneyear"]}','{data.Rows[i]["lifeway_drink_type"]}','{data.Rows[i]["lifeway_drink_other"]}','{data.Rows[i]["lifeway_occupational_disease"]}','{data.Rows[i]["lifeway_job"]}','{data.Rows[i]["lifeway_job_period"]}','{data.Rows[i]["lifeway_hazardous_dust"]}','{data.Rows[i]["lifeway_dust_preventive"]}','{data.Rows[i]["lifeway_hazardous_radiation"]}','{data.Rows[i]["lifeway_radiation_preventive"]}','{data.Rows[i]["lifeway_hazardous_physical"]}','{data.Rows[i]["lifeway_physical_preventive"]}','{data.Rows[i]["lifeway_hazardous_chemical"]}','{data.Rows[i]["lifeway_chemical_preventive"]}','{data.Rows[i]["lifeway_hazardous_other"]}','{data.Rows[i]["lifeway_other_preventive"]}','{data.Rows[i]["lifeway_doctor"]}','{data.Rows[i]["organ_lips"]}','{data.Rows[i]["organ_tooth"]}','{data.Rows[i]["organ_hypodontia"]}','{data.Rows[i]["organ_hypodontia_topleft"]}','{data.Rows[i]["organ_hypodontia_topright"]}','{data.Rows[i]["organ_hypodontia_bottomleft"]}','{data.Rows[i]["organ_hypodontia_bottomright"]}','{data.Rows[i]["organ_caries"]}','{data.Rows[i]["organ_caries_topleft"]}','{data.Rows[i]["organ_caries_topright"]}','{data.Rows[i]["organ_caries_bottomleft"]}','{data.Rows[i]["organ_caries_bottomright"]}','{data.Rows[i]["organ_denture"]}','{data.Rows[i]["organ_denture_topleft"]}','{data.Rows[i]["organ_denture_topright"]}','{data.Rows[i]["organ_denture_bottomleft"]}','{data.Rows[i]["organ_denture_bottomright"]}','{data.Rows[i]["organ_guttur"]}','{data.Rows[i]["organ_vision_left"]}','{data.Rows[i]["organ_vision_right"]}','{data.Rows[i]["organ_correctedvision_left"]}','{data.Rows[i]["organ_correctedvision_right"]}','{data.Rows[i]["organ_hearing"]}','{data.Rows[i]["organ_movement"]}','{data.Rows[i]["organ_doctor"]}','{data.Rows[i]["examination_eye"]}','{data.Rows[i]["examination_eye_other"]}','{data.Rows[i]["examination_skin"]}','{data.Rows[i]["examination_skin_other"]}','{data.Rows[i]["examination_sclera"]}','{data.Rows[i]["examination_sclera_other"]}','{data.Rows[i]["examination_lymph"]}','{data.Rows[i]["examination_lymph_other"]}','{data.Rows[i]["examination_barrel_chest"]}','{data.Rows[i]["examination_breath_sounds"]}','{data.Rows[i]["examination_breath_other"]}','{data.Rows[i]["examination_rale"]}','{data.Rows[i]["examination_rale_other"]}','{data.Rows[i]["examination_heart_rate"]}','{data.Rows[i]["examination_heart_rhythm"]}','{data.Rows[i]["examination_heart_noise"]}','{data.Rows[i]["examination_noise_other"]}','{data.Rows[i]["examination_abdomen_tenderness"]}','{data.Rows[i]["examination_tenderness_memo"]}','{data.Rows[i]["examination_abdomen_mass"]}','{data.Rows[i]["examination_mass_memo"]}','{data.Rows[i]["examination_abdomen_hepatomegaly"]}','{data.Rows[i]["examination_hepatomegaly_memo"]}','{data.Rows[i]["examination_abdomen_splenomegaly"]}','{data.Rows[i]["examination_splenomegaly_memo"]}','{data.Rows[i]["examination_abdomen_shiftingdullness"]}','{data.Rows[i]["examination_shiftingdullness_memo"]}','{data.Rows[i]["examination_lowerextremity_edema"]}','{data.Rows[i]["examination_dorsal_artery"]}','{data.Rows[i]["examination_anus"]}','{data.Rows[i]["examination_anus_other"]}','{data.Rows[i]["examination_breast"]}','{data.Rows[i]["examination_breast_other"]}','{data.Rows[i]["examination_doctor"]}','{data.Rows[i]["examination_woman_vulva"]}','{data.Rows[i]["examination_vulva_memo"]}','{data.Rows[i]["examination_woman_vagina"]}','{data.Rows[i]["examination_vagina_memo"]}','{data.Rows[i]["examination_woman_cervix"]}','{data.Rows[i]["examination_cervix_memo"]}','{data.Rows[i]["examination_woman_corpus"]}','{data.Rows[i]["examination_corpus_memo"]}','{data.Rows[i]["examination_woman_accessories"]}','{data.Rows[i]["examination_accessories_memo"]}','{data.Rows[i]["examination_woman_doctor"]}','{data.Rows[i]["examination_other"]}','{data.Rows[i]["blood_hemoglobin"]}','{data.Rows[i]["blood_leukocyte"]}','{data.Rows[i]["blood_platelet"]}','{data.Rows[i]["blood_other"]}','{data.Rows[i]["urine_protein"]}','{data.Rows[i]["glycosuria"]}','{data.Rows[i]["urine_acetone_bodies"]}','{data.Rows[i]["bld"]}','{data.Rows[i]["urine_other"]}','{data.Rows[i]["blood_glucose_mmol"]}','{data.Rows[i]["blood_glucose_mg"]}','{data.Rows[i]["cardiogram"]}','{data.Rows[i]["cardiogram_memo"]}','{data.Rows[i]["cardiogram_img"]}','{data.Rows[i]["microalbuminuria"]}','{data.Rows[i]["fob"]}','{data.Rows[i]["glycosylated_hemoglobin"]}','{data.Rows[i]["hb"]}','{data.Rows[i]["sgft"]}','{data.Rows[i]["ast"]}','{data.Rows[i]["albumin"]}','{data.Rows[i]["total_bilirubin"]}','{data.Rows[i]["conjugated_bilirubin"]}','{data.Rows[i]["scr"]}','{data.Rows[i]["blood_urea"]}','{data.Rows[i]["blood_k"]}','{data.Rows[i]["blood_na"]}','{data.Rows[i]["tc"]}','{data.Rows[i]["tg"]}','{data.Rows[i]["ldl"]}','{data.Rows[i]["hdl"]}','{data.Rows[i]["chest_x"]}','{data.Rows[i]["chestx_memo"]}','{data.Rows[i]["chestx_img"]}','{data.Rows[i]["ultrasound_abdomen"]}','{data.Rows[i]["ultrasound_memo"]}','{data.Rows[i]["abdomenB_img"]}','{data.Rows[i]["other_b"]}','{data.Rows[i]["otherb_memo"]}','{data.Rows[i]["otherb_img"]}','{data.Rows[i]["cervical_smear"]}','{data.Rows[i]["cervical_smear_memo"]}','{data.Rows[i]["other"]}','{data.Rows[i]["cerebrovascular_disease"]}','{data.Rows[i]["cerebrovascular_disease_other"]}','{data.Rows[i]["kidney_disease"]}','{data.Rows[i]["kidney_disease_other"]}','{data.Rows[i]["heart_disease"]}','{data.Rows[i]["heart_disease_other"]}','{data.Rows[i]["vascular_disease"]}','{data.Rows[i]["vascular_disease_other"]}','{data.Rows[i]["ocular_diseases"]}','{data.Rows[i]["ocular_diseases_other"]}','{data.Rows[i]["nervous_system_disease"]}','{data.Rows[i]["nervous_disease_memo"]}','{data.Rows[i]["other_disease"]}','{data.Rows[i]["other_disease_memo"]}','{data.Rows[i]["health_evaluation"]}','{data.Rows[i]["abnormal1"]}','{data.Rows[i]["abnormal2"]}','{data.Rows[i]["abnormal3"]}','{data.Rows[i]["abnormal4"]}','{data.Rows[i]["health_guidance"]}','{data.Rows[i]["danger_controlling"]}','{data.Rows[i]["target_weight"]}','{data.Rows[i]["advise_bacterin"]}','{data.Rows[i]["danger_controlling_other"]}','{data.Rows[i]["healthAdvice"]}','{data.Rows[i]["create_user"]}','{data.Rows[i]["create_name"]}','{data.Rows[i]["create_org"]}','{data.Rows[i]["create_org_name"]}',{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        recordid += $"'{data.Rows[i]["id"]}',";
                    }
                }

                #endregion

                #region 老年人生活自理评估
                DataSet estimate = DbHelperMySQL.Query($@"select * from elderly_selfcare_estimate where id_number in({string.Join(",", ide.Select(m => m.ID).ToList())}) and (upload_status=0 or upload_status is null or upload_status=2)");
                if (estimate != null && estimate.Tables.Count > 0 && estimate.Tables[0].Rows.Count > 0)
                {
                    DataTable data = estimate.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From elderly_selfcare_estimate where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);
                        sqllist.Add($@"insert into elderly_selfcare_estimate (id,name,archive_no,id_number,test_date,answer_result,total_score,judgement_result,test_doctor,create_user,create_name,create_org,create_org_name,create_time
                            ) values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["test_date"])},{Ifnull(data.Rows[i]["answer_result"])},{Ifnull(data.Rows[i]["total_score"])},{Ifnull(data.Rows[i]["judgement_result"])},{Ifnull(data.Rows[i]["test_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        estimateid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 高血压
                DataSet fuv = DbHelperMySQL.Query($@"select * from fuv_hypertension where (upload_status=0 or upload_status=2) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (fuv != null && fuv.Tables.Count > 0 && fuv.Tables[0].Rows.Count > 0)
                {
                    DataTable data = fuv.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        DataSet follow = DbHelperMySQL.Query($@"select * from follow_medicine_record where follow_id='{data.Rows[i]["id"]}'");
                        if (follow != null && follow.Tables.Count > 0 && follow.Tables[0].Rows.Count > 0)
                        {
                            DataTable data1 = follow.Tables[0];
                            if(data1.Rows.Count>0)
                            {
                                string sql = string.Format("Delete From follow_medicine_record where follow_id='{0}'", data.Rows[i]["id"].ToString());
                                sqllist.Add(sql);
                            } 
                            for (int k = 0; k < data1.Rows.Count; k++)
                            {
                                sqllist.Add($@"insert into follow_medicine_record (id,follow_id,drug_name,num,dosage,create_user,create_name,create_time) values({Ifnull(data1.Rows[k]["id"])},{Ifnull(data1.Rows[k]["follow_id"])},{Ifnull(data1.Rows[k]["drug_name"])},{Ifnull(data1.Rows[k]["num"])},{Ifnull(data1.Rows[k]["dosage"])},{Ifnull(data1.Rows[k]["create_user"])},{Ifnull(data1.Rows[k]["create_name"])},{Ifnull(Convert.ToDateTime(data1.Rows[k]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                                followid += $"'{data1.Rows[i]["id"]}',";
                            }
                        }
                        string sql1 = string.Format("Delete From fuv_hypertension where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql1);
                        sqllist.Add($@"insert into fuv_hypertension (id,name,archive_no,id_number,visit_date,visit_type,symptom,other_symptom,sbp,dbp,weight,target_weight,bmi,target_bmi,heart_rate,other_sign,smoken,target_somken,wine,target_wine,sport_week,sport_once,target_sport_week,target_sport_once,salt_intake,target_salt_intake,mind_adjust,doctor_obey,assist_examine,drug_obey,untoward_effect,untoward_effect_drug,visit_class,referral_code,next_visit_date,visit_doctor,advice,create_user,create_name,create_time,create_org,create_org_name,transfer_organ,transfer_reason
                            ) values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["visit_type"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["other_symptom"])},{Ifnull(data.Rows[i]["sbp"])},{Ifnull(data.Rows[i]["dbp"])},{Ifnull(data.Rows[i]["weight"])},{Ifnull(data.Rows[i]["target_weight"])},{Ifnull(data.Rows[i]["bmi"])},{Ifnull(data.Rows[i]["target_bmi"])},{Ifnull(data.Rows[i]["heart_rate"])},{Ifnull(data.Rows[i]["other_sign"])},{Ifnull(data.Rows[i]["smoken"])},{Ifnull(data.Rows[i]["target_somken"])},{Ifnull(data.Rows[i]["wine"])},{Ifnull(data.Rows[i]["target_wine"])},{Ifnull(data.Rows[i]["sport_week"])},{Ifnull(data.Rows[i]["sport_once"])},{Ifnull(data.Rows[i]["target_sport_week"])},{Ifnull(data.Rows[i]["target_sport_once"])},{Ifnull(data.Rows[i]["salt_intake"])},{Ifnull(data.Rows[i]["target_salt_intake"])},{Ifnull(data.Rows[i]["mind_adjust"])},{Ifnull(data.Rows[i]["doctor_obey"])},{Ifnull(data.Rows[i]["assist_examine"])},{Ifnull(data.Rows[i]["drug_obey"])},{Ifnull(data.Rows[i]["untoward_effect"])},{Ifnull(data.Rows[i]["untoward_effect_drug"])},{Ifnull(data.Rows[i]["visit_class"])},{Ifnull(data.Rows[i]["referral_code"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["advice"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(data.Rows[i]["transfer_organ"])},{Ifnull(data.Rows[i]["transfer_reason"])});");
                        fuvid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 糖尿病
                DataSet diabetes = DbHelperMySQL.Query($@"select * from diabetes_follow_record where upload_status='0' and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (diabetes != null && diabetes.Tables.Count > 0 && diabetes.Tables[0].Rows.Count > 0)
                {
                    DataTable data = diabetes.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        DataSet follow = DbHelperMySQL.Query($@"select * from follow_medicine_record where follow_id='{data.Rows[i]["id"]}'");
                        if (follow != null && follow.Tables.Count > 0 && follow.Tables[0].Rows.Count > 0)
                        {
                            DataTable data1 = follow.Tables[0];
                            if (data1.Rows.Count > 0)
                            {
                                string sql = string.Format("Delete From follow_medicine_record where follow_id='{0}'", data.Rows[i]["id"].ToString());
                                sqllist.Add(sql);
                            }
                            for (int k = 0; k < data.Rows.Count; k++)
                            {
                                sqllist.Add($@"insert into follow_medicine_record (id,follow_id,drug_name,num,dosage,create_user,create_name,create_time) values({Ifnull(data1.Rows[k]["id"])},{Ifnull(data1.Rows[k]["follow_id"])},{Ifnull(data1.Rows[k]["drug_name"])},{Ifnull(data1.Rows[k]["num"])},{Ifnull(data1.Rows[k]["dosage"])},{Ifnull(data1.Rows[k]["create_user"])},{Ifnull(data1.Rows[k]["create_name"])},{Ifnull(Convert.ToDateTime(data1.Rows[k]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                                followid += $"'{data1.Rows[i]["id"]}',";
                            }
                        }
                        string sql1 = string.Format("Delete From diabetes_follow_record where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql1);
                        sqllist.Add($@"insert into diabetes_follow_record (id,name,archive_no,id_number,visit_date,visit_type,symptom,symptom_other,blood_pressure_high,blood_pressure_low,weight_now,weight_next,bmi_now,bmi_next,dorsal_artery,other,smoke_now,smoke_next,drink_now,drink_next,sports_num_now,sports_time_now,sports_num_next,sports_time_next,staple_food_now,staple_food_next,psychological_recovery,medical_compliance,blood_glucose,glycosylated_hemoglobin,check_date,compliance,untoward_effect,reactive_hypoglycemia,follow_type,insulin_name,insulin_usage,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time
) values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["visit_type"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["symptom_other"])},{Ifnull(data.Rows[i]["blood_pressure_high"])},{Ifnull(data.Rows[i]["blood_pressure_low"])},{Ifnull(data.Rows[i]["weight_now"])},{Ifnull(data.Rows[i]["weight_next"])},{Ifnull(data.Rows[i]["bmi_now"])},{Ifnull(data.Rows[i]["bmi_next"])},{Ifnull(data.Rows[i]["dorsal_artery"])},{Ifnull(data.Rows[i]["other"])},{Ifnull(data.Rows[i]["smoke_now"])},{Ifnull(data.Rows[i]["smoke_next"])},{Ifnull(data.Rows[i]["drink_now"])},{Ifnull(data.Rows[i]["drink_next"])},{Ifnull(data.Rows[i]["sports_num_now"])},{Ifnull(data.Rows[i]["sports_time_now"])},{Ifnull(data.Rows[i]["sports_num_next"])},{Ifnull(data.Rows[i]["sports_time_next"])},{Ifnull(data.Rows[i]["staple_food_now"])},{Ifnull(data.Rows[i]["staple_food_next"])},{Ifnull(data.Rows[i]["psychological_recovery"])},{Ifnull(data.Rows[i]["medical_compliance"])},{Ifnull(data.Rows[i]["blood_glucose"])},{Ifnull(data.Rows[i]["glycosylated_hemoglobin"])},{Ifnull(data.Rows[i]["check_date"])},{Ifnull(data.Rows[i]["compliance"])},{Ifnull(data.Rows[i]["untoward_effect"])},{Ifnull(data.Rows[i]["reactive_hypoglycemia"])},{Ifnull(data.Rows[i]["follow_type"])},{Ifnull(data.Rows[i]["insulin_name"])},{Ifnull(data.Rows[i]["insulin_usage"])},{Ifnull(data.Rows[i]["transfer_treatment"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        diabetesid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 尿常规记录表
                DataSet ncg = DbHelperMySQL.Query($@"select * from zkhw_tj_ncg where id_number in({string.Join(",", ide.Select(m => m.ID).ToList())}) and (upload_status=0 or upload_status=2 or upload_status is null)");
                if (ncg != null && ncg.Tables.Count > 0 && ncg.Tables[0].Rows.Count > 0)
                {
                    DataTable data = ncg.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From zkhw_tj_ncg where ID='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        string sqlncg = $@"insert into zkhw_tj_ncg (ID,aichive_no,id_number,bar_code,WBC,LEU,NIT,URO,PRO,PH,BLD,SG,KET,BIL,GLU,Vc,MA,ACR,Ca,CR,type,createtime,synchronize_type,ZrysNCG) values({ Ifnull(data.Rows[i]["id"])},{ Ifnull(data.Rows[i]["aichive_no"])},{ Ifnull(data.Rows[i]["id_number"])},{ Ifnull(data.Rows[i]["bar_code"])},{ Ifnull(data.Rows[i]["WBC"])},{ Ifnull(data.Rows[i]["LEU"])},{ Ifnull(data.Rows[i]["NIT"])},{ Ifnull(data.Rows[i]["URO"])},{ Ifnull(data.Rows[i]["PRO"])},{ Ifnull(data.Rows[i]["PH"])},{ Ifnull(data.Rows[i]["BLD"])},{ Ifnull(data.Rows[i]["SG"])},{ Ifnull(data.Rows[i]["KET"])},{ Ifnull(data.Rows[i]["BIL"])},{ Ifnull(data.Rows[i]["GLU"])},{ Ifnull(data.Rows[i]["Vc"])},{ Ifnull(data.Rows[i]["MA"])},{ Ifnull(data.Rows[i]["ACR"])},{ Ifnull(data.Rows[i]["Ca"])},{ Ifnull(data.Rows[i]["CR"])},{ Ifnull(data.Rows[i]["type"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{ Ifnull(data.Rows[i]["synchronize_type"])},{ Ifnull(data.Rows[i]["ZrysNCG"])});";
                        sqllist.Add(sqlncg);
                        ncgid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 身高体重
                DataSet sgtz = DbHelperMySQL.Query($@"select * from zkhw_tj_sgtz where id_number in({string.Join(",", ide.Select(m => m.ID).ToList())}) and (upload_status=0 or upload_status=2 or upload_status is null)");
                if (sgtz != null && sgtz.Tables.Count > 0 && sgtz.Tables[0].Rows.Count > 0)
                {
                    DataTable data = sgtz.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From zkhw_tj_sgtz where ID='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into zkhw_tj_sgtz (ID,aichive_no,id_number,bar_code,BMI,Height,Weight,createtime) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["BMI"])},{Ifnull(data.Rows[i]["Height"])},{Ifnull(data.Rows[i]["Weight"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        sgtzid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 生化记录表
                DataSet sh = DbHelperMySQL.Query($@"select * from zkhw_tj_sh where id_number in({string.Join(",", ide.Select(m => m.ID).ToList())}) and (upload_status=0 or upload_status=2 or upload_status is null)");
                if (sh != null && sh.Tables.Count > 0 && sh.Tables[0].Rows.Count > 0)
                {
                    DataTable data = sh.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From zkhw_tj_sh where ID='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into zkhw_tj_sh (ID,aichive_no,id_number,bar_code,ALT,AST,TBIL,DBIL,CREA,UREA,GLU,TG,CHO,HDLC,LDLC,ALB,UA,HCY,AFP,CEA,Ka,Na,TP,ALP,GGT,CHE,TBA,APOA1,APOB,CK,CKMB,LDHL,HBDH,aAMY,createtime,synchronize_type,ZrysSH,low,high,timeCodeUnique) 
                        values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["ALT"])},{Ifnull(data.Rows[i]["AST"])},{Ifnull(data.Rows[i]["TBIL"])},{Ifnull(data.Rows[i]["DBIL"])},{Ifnull(data.Rows[i]["CREA"])},{Ifnull(data.Rows[i]["UREA"])},{Ifnull(data.Rows[i]["GLU"])},{Ifnull(data.Rows[i]["TG"])},{Ifnull(data.Rows[i]["CHO"])},{Ifnull(data.Rows[i]["HDLC"])},{Ifnull(data.Rows[i]["LDLC"])},{Ifnull(data.Rows[i]["ALB"])},{Ifnull(data.Rows[i]["UA"])},{Ifnull(data.Rows[i]["HCY"])},{Ifnull(data.Rows[i]["AFP"])},{Ifnull(data.Rows[i]["CEA"])},{Ifnull(data.Rows[i]["Ka"])},{Ifnull(data.Rows[i]["Na"])},{Ifnull(data.Rows[i]["TP"])},{Ifnull(data.Rows[i]["ALP"])},{Ifnull(data.Rows[i]["GGT"])},{Ifnull(data.Rows[i]["CHE"])},{Ifnull(data.Rows[i]["TBA"])},{Ifnull(data.Rows[i]["APOA1"])},{Ifnull(data.Rows[i]["APOB"])},{Ifnull(data.Rows[i]["CK"])},{Ifnull(data.Rows[i]["CKMB"])},{Ifnull(data.Rows[i]["LDHL"])},{Ifnull(data.Rows[i]["HBDH"])},{Ifnull(data.Rows[i]["aAMY"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["synchronize_type"])},{Ifnull(data.Rows[i]["ZrysSH"])},{Ifnull(data.Rows[i]["low"])},{Ifnull(data.Rows[i]["high"])},{Ifnull(data.Rows[i]["timeCodeUnique"])});");
                        shid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 血常规记录表
                DataSet xcg = DbHelperMySQL.Query($@"select * from zkhw_tj_xcg where id_number in({string.Join(",", ide.Select(m => m.ID).ToList())}) and (upload_status=0 or upload_status=2 or upload_status is null)");
                if (xcg != null && xcg.Tables.Count > 0 && xcg.Tables[0].Rows.Count > 0)
                {
                    DataTable data = xcg.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From zkhw_tj_xcg where ID='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into zkhw_tj_xcg (ID,aichive_no,id_number,bar_code,WBC,RBC,PCT,PLT,HGB,HCT,MCV,MCH,MCHC,RDWCV,RDWSD,MONO,MONOP,GRAN,GRANP,NEUT,NEUTP,EO,EOP,BASO,BASOP,LYM,LYMP,MPV,PDW,MXD,MXDP,PLCR,OTHERS,createtime,synchronize_type,ZrysXCG,timeCodeUnique) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["WBC"])},{Ifnull(data.Rows[i]["RBC"])},{Ifnull(data.Rows[i]["PCT"])},{Ifnull(data.Rows[i]["PLT"])},{Ifnull(data.Rows[i]["HGB"])},{Ifnull(data.Rows[i]["HCT"])},{Ifnull(data.Rows[i]["MCV"])},{Ifnull(data.Rows[i]["MCH"])},{Ifnull(data.Rows[i]["MCHC"])},{Ifnull(data.Rows[i]["RDWCV"])},{Ifnull(data.Rows[i]["RDWSD"])},{Ifnull(data.Rows[i]["MONO"])},{Ifnull(data.Rows[i]["MONOP"])},{Ifnull(data.Rows[i]["GRAN"])},{Ifnull(data.Rows[i]["GRANP"])},{Ifnull(data.Rows[i]["NEUT"])},{Ifnull(data.Rows[i]["NEUTP"])},{Ifnull(data.Rows[i]["EO"])},{Ifnull(data.Rows[i]["EOP"])},{Ifnull(data.Rows[i]["BASO"])},{Ifnull(data.Rows[i]["BASOP"])},{Ifnull(data.Rows[i]["LYM"])},{Ifnull(data.Rows[i]["LYMP"])},{Ifnull(data.Rows[i]["MPV"])},{Ifnull(data.Rows[i]["PDW"])},{Ifnull(data.Rows[i]["MXD"])},{Ifnull(data.Rows[i]["MXDP"])},{Ifnull(data.Rows[i]["PLCR"])},{Ifnull(data.Rows[i]["OTHERS"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["synchronize_type"])},{Ifnull(data.Rows[i]["ZrysXCG"])},{Ifnull(data.Rows[i]["timeCodeUnique"])});");
                        xcgid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 心电图记录表
                DataSet xdt = DbHelperMySQL.Query($@"select * from zkhw_tj_xdt where id_number in({string.Join(",", ide.Select(m => m.ID).ToList())}) and (upload_status=0 or upload_status=2 or upload_status is null)");
                if (xdt != null && xdt.Tables.Count > 0 && xdt.Tables[0].Rows.Count > 0)
                {
                    DataTable data = xdt.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From zkhw_tj_xdt where ID='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into zkhw_tj_xdt (ID,aichive_no,id_number,bar_code,XdtResult,XdtDesc,XdtDoctor,XdtName,Ventrate,PR,QRS,QT,QTc,P_R_T,DOB,Age,Gen,Dep,createtime,synchronize_type,ZrysXDT,imageUrl,hr,p,pqrs,t,rv5,sv1,baseline_drift,myoelectricity,frequency
) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["XdtResult"])},{Ifnull(data.Rows[i]["XdtDesc"])},{Ifnull(data.Rows[i]["XdtDoctor"])},{Ifnull(data.Rows[i]["XdtName"])},{Ifnull(data.Rows[i]["Ventrate"])},{Ifnull(data.Rows[i]["PR"])},{Ifnull(data.Rows[i]["QRS"])},{Ifnull(data.Rows[i]["QT"])},{Ifnull(data.Rows[i]["QTc"])},{Ifnull(data.Rows[i]["P_R_T"])},{Ifnull(data.Rows[i]["DOB"])},{Ifnull(data.Rows[i]["Age"])},{Ifnull(data.Rows[i]["Gen"])},{Ifnull(data.Rows[i]["Dep"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["synchronize_type"])},{Ifnull(data.Rows[i]["ZrysXDT"])},{Ifnull(data.Rows[i]["imageUrl"])},{Ifnull(data.Rows[i]["hr"])},{Ifnull(data.Rows[i]["p"])},{Ifnull(data.Rows[i]["pqrs"])},{Ifnull(data.Rows[i]["t"])},{Ifnull(data.Rows[i]["rv5"])},{Ifnull(data.Rows[i]["sv1"])},{Ifnull(data.Rows[i]["baseline_drift"])},{Ifnull(data.Rows[i]["myoelectricity"])},{Ifnull(data.Rows[i]["frequency"])});");
                        xdtid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 血压表
                DataSet xy = DbHelperMySQL.Query($@"select * from zkhw_tj_xy where id_number in({string.Join(",", ide.Select(m => m.ID).ToList())}) and (upload_status=0 or upload_status=2 or upload_status is null)");
                if (xy != null && xy.Tables.Count > 0 && xy.Tables[0].Rows.Count > 0)
                {
                    DataTable data = xy.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From zkhw_tj_xy where ID='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into zkhw_tj_xy (ID,aichive_no,id_number,bar_code,DBP,SBP,Pulse,createtime) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["DBP"])},{Ifnull(data.Rows[i]["SBP"])},{Ifnull(data.Rows[i]["Pulse"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        xyid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region B超表
                DataSet bc = DbHelperMySQL.Query($@"select * from zkhw_tj_bc where id_number in({string.Join(",", ide.Select(m => m.ID).ToList())}) and (upload_status=0 or upload_status=2 or upload_status is null)");
                if (bc != null && bc.Tables.Count > 0 && bc.Tables[0].Rows.Count > 0)
                {
                    DataTable data = bc.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From zkhw_tj_bc where ID='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into zkhw_tj_bc (ID,aichive_no,id_number,bar_code,FubuBC,FubuResult,FubuDesc,QitaBC,QitaResult,QitaDesc,BuPic01,BuPic02,BuPic03,BuPic04,createtime,synchronize_type,ZrysBC,imageUrl_a,imageUrl_b,imageUrl_c,imageUrl_d) 
values({Ifnull(data.Rows[i]["ID"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["bar_code"])},{Ifnull(data.Rows[i]["FubuBC"])},{Ifnull(data.Rows[i]["FubuResult"])},{Ifnull(data.Rows[i]["FubuDesc"])},{Ifnull(data.Rows[i]["QitaBC"])},{Ifnull(data.Rows[i]["QitaResult"])},{Ifnull(data.Rows[i]["QitaDesc"])},{Ifnull(data.Rows[i]["BuPic01"])},{Ifnull(data.Rows[i]["BuPic02"])},{Ifnull(data.Rows[i]["BuPic03"])},{Ifnull(data.Rows[i]["BuPic04"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["createtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["synchronize_type"])},{Ifnull(data.Rows[i]["ZrysBC"])},{Ifnull(data.Rows[i]["imageUrl_a"])},{Ifnull(data.Rows[i]["imageUrl_b"])},{Ifnull(data.Rows[i]["imageUrl_c"])},{Ifnull(data.Rows[i]["imageUrl_d"])});");
                        bcid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 儿童健康服务
                DataSet rtjkfw = DbHelperMySQL.Query($@"select * from children_health_record where (upload_status=0 or upload_status=2) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (rtjkfw != null && rtjkfw.Tables.Count > 0 && rtjkfw.Tables[0].Rows.Count > 0)
                {
                    DataTable data = rtjkfw.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From children_health_record where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into children_health_record (id,name,archive_no,id_number,age,visit_date,weight,weight_evaluate,height,height_evaluate,weight_height,physical_assessment,head_circumference,complexion,complexion_other,skin,anterior_fontanelle_wide,anterior_fontanelle_high,anterior_fontanelle,neck_mass,eye,vision,ear,hearing,oral_cavity,teething_num,caries_num,breast,abdominal,umbilical_cord,extremity,gait,rickets_symptom,rickets_sign,anus,hemoglobin,other,outdoor_time,vitamind_name,vitamind_num,growth,sicken_stasus,pneumonia_num,diarrhea_num,trauma_num,sicken_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,guidance,guidance_other,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["archive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["age"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["weight"])},{Ifnull(data.Rows[i]["weight_evaluate"])},{Ifnull(data.Rows[i]["height"])},{Ifnull(data.Rows[i]["height_evaluate"])},{Ifnull(data.Rows[i]["weight_height"])},{Ifnull(data.Rows[i]["physical_assessment"])},{Ifnull(data.Rows[i]["head_circumference"])},{Ifnull(data.Rows[i]["complexion"])},{Ifnull(data.Rows[i]["complexion_other"])},{Ifnull(data.Rows[i]["skin"])},{Ifnull(data.Rows[i]["anterior_fontanelle_wide"])},{Ifnull(data.Rows[i]["anterior_fontanelle_high"])},{Ifnull(data.Rows[i]["anterior_fontanelle"])},{Ifnull(data.Rows[i]["neck_mass"])},{Ifnull(data.Rows[i]["eye"])},{Ifnull(data.Rows[i]["vision"])},{Ifnull(data.Rows[i]["ear"])},{Ifnull(data.Rows[i]["hearing"])},{Ifnull(data.Rows[i]["oral_cavity"])},{Ifnull(data.Rows[i]["teething_num"])},{Ifnull(data.Rows[i]["caries_num"])},{Ifnull(data.Rows[i]["breast"])},{Ifnull(data.Rows[i]["abdominal"])},{Ifnull(data.Rows[i]["umbilical_cord"])},{Ifnull(data.Rows[i]["extremity"])},{Ifnull(data.Rows[i]["gait"])},{Ifnull(data.Rows[i]["rickets_symptom"])},{Ifnull(data.Rows[i]["rickets_sign"])},{Ifnull(data.Rows[i]["anus"])},{Ifnull(data.Rows[i]["hemoglobin"])},{Ifnull(data.Rows[i]["other"])},{Ifnull(data.Rows[i]["outdoor_time"])},{Ifnull(data.Rows[i]["vitamind_name"])},{Ifnull(data.Rows[i]["vitamind_num"])},{Ifnull(data.Rows[i]["growth"])},{Ifnull(data.Rows[i]["sicken_stasus"])},{Ifnull(data.Rows[i]["pneumonia_num"])},{Ifnull(data.Rows[i]["diarrhea_num"])},{Ifnull(data.Rows[i]["trauma_num"])},{Ifnull(data.Rows[i]["sicken_other"])},{Ifnull(data.Rows[i]["transfer_treatment"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["guidance"])},{Ifnull(data.Rows[i]["guidance_other"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        rtjkfwid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 新生儿家庭记录
                DataSet xsrjt = DbHelperMySQL.Query($@"select * from neonatus_info where (upload_status=0 or upload_status=2) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (xsrjt != null && xsrjt.Tables.Count > 0 && xsrjt.Tables[0].Rows.Count > 0)
                {
                    DataTable data = xsrjt.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From neonatus_info where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into neonatus_info (id,name,archive_no,id_number,sex,birthday,home_address,father_name,father_profession,father_phone,father_birthday,mother_name,mother_profession,mother_phone,mother_birthday,gestational_weeks,sicken_stasus,sicken_other,midwife_org,birth_situation,birth_other,asphyxia_neonatorum,asphyxia_time,deformity,deformity_other,hearing,disease,disease_other,birth_weight,weight,birth_height,feeding_patterns,milk_num,milk_intake,vomit,shit,defecation_num,temperature,heart_rate,breathing_rate,complexion,complexion_other,aurigo,aurigo_other,anterior_fontanelle_wide,anterior_fontanelle_high,anterior_fontanelle,anterior_fontanelle_other,eye,extremity_mobility,ear,neck_mass,nose,skin,skin_other,oral_cavity,anus,heart_lung,breast,abdominal_touch,spine,aedea,umbilical_cord,umbilical_cord_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,guidance,guidance_other,visit_date,next_visit_date,next_visit_address,visit_doctor,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["archive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["sex"])},{Ifnull(data.Rows[i]["birthday"])},{Ifnull(data.Rows[i]["home_address"])},{Ifnull(data.Rows[i]["father_name"])},{Ifnull(data.Rows[i]["father_profession"])},{Ifnull(data.Rows[i]["father_phone"])},{Ifnull(data.Rows[i]["father_birthday"])},{Ifnull(data.Rows[i]["mother_name"])},{Ifnull(data.Rows[i]["mother_profession"])},{Ifnull(data.Rows[i]["mother_phone"])},{Ifnull(data.Rows[i]["mother_birthday"])},{Ifnull(data.Rows[i]["gestational_weeks"])},{Ifnull(data.Rows[i]["sicken_stasus"])},{Ifnull(data.Rows[i]["sicken_other"])},{Ifnull(data.Rows[i]["midwife_org"])},{Ifnull(data.Rows[i]["birth_situation"])},{Ifnull(data.Rows[i]["birth_other"])},{Ifnull(data.Rows[i]["asphyxia_neonatorum"])},{Ifnull(data.Rows[i]["asphyxia_time"])},{Ifnull(data.Rows[i]["deformity"])},{Ifnull(data.Rows[i]["deformity_other"])},{Ifnull(data.Rows[i]["hearing"])},{Ifnull(data.Rows[i]["disease"])},{Ifnull(data.Rows[i]["disease_other"])},{Ifnull(data.Rows[i]["birth_weight"])},{Ifnull(data.Rows[i]["weight"])},{Ifnull(data.Rows[i]["birth_height"])},{Ifnull(data.Rows[i]["feeding_patterns"])},{Ifnull(data.Rows[i]["milk_num"])},{Ifnull(data.Rows[i]["milk_intake"])},{Ifnull(data.Rows[i]["vomit"])},{Ifnull(data.Rows[i]["shit"])},{Ifnull(data.Rows[i]["defecation_num"])},{Ifnull(data.Rows[i]["temperature"])},{Ifnull(data.Rows[i]["heart_rate"])},{Ifnull(data.Rows[i]["breathing_rate"])},{Ifnull(data.Rows[i]["complexion"])},{Ifnull(data.Rows[i]["complexion_other"])},{Ifnull(data.Rows[i]["aurigo"])},{Ifnull(data.Rows[i]["aurigo_other"])},{Ifnull(data.Rows[i]["anterior_fontanelle_wide"])},{Ifnull(data.Rows[i]["anterior_fontanelle_high"])},{Ifnull(data.Rows[i]["anterior_fontanelle"])},{Ifnull(data.Rows[i]["anterior_fontanelle_other"])},{Ifnull(data.Rows[i]["eye"])},{Ifnull(data.Rows[i]["extremity_mobility"])},{Ifnull(data.Rows[i]["ear"])},{Ifnull(data.Rows[i]["neck_mass"])},{Ifnull(data.Rows[i]["nose"])},{Ifnull(data.Rows[i]["skin"])},{Ifnull(data.Rows[i]["skin_other"])},{Ifnull(data.Rows[i]["oral_cavity"])},{Ifnull(data.Rows[i]["anus"])},{Ifnull(data.Rows[i]["heart_lung"])},{Ifnull(data.Rows[i]["breast"])},{Ifnull(data.Rows[i]["abdominal_touch"])},{Ifnull(data.Rows[i]["spine"])},{Ifnull(data.Rows[i]["aedea"])},{Ifnull(data.Rows[i]["umbilical_cord"])},{Ifnull(data.Rows[i]["umbilical_cord_other"])},{Ifnull(data.Rows[i]["transfer_treatment"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["guidance"])},{Ifnull(data.Rows[i]["guidance_other"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["next_visit_address"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["province_code"])},{Ifnull(data.Rows[i]["province_name"])},{Ifnull(data.Rows[i]["city_code"])},{Ifnull(data.Rows[i]["city_name"])},{Ifnull(data.Rows[i]["county_code"])},{Ifnull(data.Rows[i]["county_name"])},{Ifnull(data.Rows[i]["towns_code"])},{Ifnull(data.Rows[i]["towns_name"])},{Ifnull(data.Rows[i]["village_code"])},{Ifnull(data.Rows[i]["village_name"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        xsrjtid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 孕妇产后服务记录
                DataSet yfch = DbHelperMySQL.Query($@"select * from gravida_after_record where (upload_status=0 or upload_status=2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (yfch != null && yfch.Tables.Count > 0 && yfch.Tables[0].Rows.Count > 0)
                {
                    DataTable data = yfch.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From gravida_after_record where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into gravida_after_record (id,name,archive_no,id_number,visit_date,childbirth,discharge_date,temperature,general_health_status,general_psychology_status,blood_pressure_high,blood_pressure_low,breast,breast_error,lyma,lyma_error,womb,womb_error,wound,wound_error,other,`condition`,guidance,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,visit_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["childbirth"])},{Ifnull(data.Rows[i]["discharge_date"])},{Ifnull(data.Rows[i]["temperature"])},{Ifnull(data.Rows[i]["general_health_status"])},{Ifnull(data.Rows[i]["general_psychology_status"])},{Ifnull(data.Rows[i]["blood_pressure_high"])},{Ifnull(data.Rows[i]["blood_pressure_low"])},{Ifnull(data.Rows[i]["breast"])},{Ifnull(data.Rows[i]["breast_error"])},{Ifnull(data.Rows[i]["lyma"])},{Ifnull(data.Rows[i]["lyma_error"])},{Ifnull(data.Rows[i]["womb"])},{Ifnull(data.Rows[i]["womb_error"])},{Ifnull(data.Rows[i]["wound"])},{Ifnull(data.Rows[i]["wound_error"])},{Ifnull(data.Rows[i]["other"])},{Ifnull(data.Rows[i]["condition"])},{Ifnull(data.Rows[i]["guidance"])},{Ifnull(data.Rows[i]["transfer_treatment"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        yfchid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 孕妇产前服务记录
                DataSet yfcq = DbHelperMySQL.Query($@"select * from gravida_follow_record where (upload_status=0 or upload_status=2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (yfcq != null && yfcq.Tables.Count > 0 && yfcq.Tables[0].Rows.Count > 0)
                {
                    DataTable data = yfcq.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From gravida_follow_record where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into gravida_follow_record (id,name,archive_no,id_number,order_num,visit_date,gestational_weeks,symptom,weight,fundus_height,abdomen_circumference,fetus_position,fetal_heart_rate,blood_pressure_high,blood_pressure_low,hemoglobin,urine_protein,check_other,`condition`,error_info,guidance,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status
) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["order_num"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["gestational_weeks"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["weight"])},{Ifnull(data.Rows[i]["fundus_height"])},{Ifnull(data.Rows[i]["abdomen_circumference"])},{Ifnull(data.Rows[i]["fetus_position"])},{Ifnull(data.Rows[i]["fetal_heart_rate"])},{Ifnull(data.Rows[i]["blood_pressure_high"])},{Ifnull(data.Rows[i]["blood_pressure_low"])},{Ifnull(data.Rows[i]["hemoglobin"])},{Ifnull(data.Rows[i]["urine_protein"])},{Ifnull(data.Rows[i]["check_other"])},{Ifnull(data.Rows[i]["condition"])},{Ifnull(data.Rows[i]["error_info"])},{Ifnull(data.Rows[i]["guidance"])},{Ifnull(data.Rows[i]["transfer_treatment"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        yfcqid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                #region 孕妇第一次检查
                DataSet yfdyc = DbHelperMySQL.Query($@"select * from gravida_info where (upload_status=0 or upload_status=2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (yfdyc != null && yfdyc.Tables.Count > 0 && yfdyc.Tables[0].Rows.Count > 0)
                {
                    DataTable data = yfdyc.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From gravida_info where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into gravida_info (id,name,archive_no,id_number,visit_date,gestational_weeks,gravida_age,husband_name,husband_age,husband_phone,pregnant_num,natural_labour_num,cesarean_num,last_menstruation_date,due_date,past_illness,past_illness_other,family_history,family_history_other,habits_customs,habits_customs_other,isoperation,operation_name,natural_abortion_num,abactio_num,fetaldeath_num,stillbirth_num,neonatal_death_num,birth_defect_num,height,weight,bmi,blood_pressure_high,blood_pressure_low,heart,heart_other,lungs,lungs_other,vulva,vulva_other,vagina,vagina_other,cervix,cervix_other,corpus,corpus_other,accessories,accessories_other,hemoglobin,leukocyte,platelet,blood_other,urine_protein,glycosuria,urine_acetone_bodies,bld,urine_other,blood_sugar,blood_group,blood_rh,sgft,ast,albumin,total_bilirubin,conjugated_bilirubin,scr,blood_urea,vaginal_fluid,vaginal_fluid_other,vaginal_cleaning,hb,hbsab,hbeag,hbeab,hbcab,syphilis,hiv,b_ultrasonic,other,general_assessment,assessment_error,health_guidance,health_guidance_other,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["gestational_weeks"])},{Ifnull(data.Rows[i]["gravida_age"])},{Ifnull(data.Rows[i]["husband_name"])},{Ifnull(data.Rows[i]["husband_age"])},{Ifnull(data.Rows[i]["husband_phone"])},{Ifnull(data.Rows[i]["pregnant_num"])},{Ifnull(data.Rows[i]["natural_labour_num"])},{Ifnull(data.Rows[i]["cesarean_num"])},{Ifnull(data.Rows[i]["last_menstruation_date"])},{Ifnull(data.Rows[i]["due_date"])},{Ifnull(data.Rows[i]["past_illness"])},{Ifnull(data.Rows[i]["past_illness_other"])},{Ifnull(data.Rows[i]["family_history"])},{Ifnull(data.Rows[i]["family_history_other"])},{Ifnull(data.Rows[i]["habits_customs"])},{Ifnull(data.Rows[i]["habits_customs_other"])},{Ifnull(data.Rows[i]["isoperation"])},{Ifnull(data.Rows[i]["operation_name"])},{Ifnull(data.Rows[i]["natural_abortion_num"])},{Ifnull(data.Rows[i]["abactio_num"])},{Ifnull(data.Rows[i]["fetaldeath_num"])},{Ifnull(data.Rows[i]["stillbirth_num"])},{Ifnull(data.Rows[i]["neonatal_death_num"])},{Ifnull(data.Rows[i]["birth_defect_num"])},{Ifnull(data.Rows[i]["height"])},{Ifnull(data.Rows[i]["weight"])},{Ifnull(data.Rows[i]["bmi"])},{Ifnull(data.Rows[i]["blood_pressure_high"])},{Ifnull(data.Rows[i]["blood_pressure_low"])},{Ifnull(data.Rows[i]["heart"])},{Ifnull(data.Rows[i]["heart_other"])},{Ifnull(data.Rows[i]["lungs"])},{Ifnull(data.Rows[i]["lungs_other"])},{Ifnull(data.Rows[i]["vulva"])},{Ifnull(data.Rows[i]["vulva_other"])},{Ifnull(data.Rows[i]["vagina"])},{Ifnull(data.Rows[i]["vagina_other"])},{Ifnull(data.Rows[i]["cervix"])},{Ifnull(data.Rows[i]["cervix_other"])},{Ifnull(data.Rows[i]["corpus"])},{Ifnull(data.Rows[i]["corpus_other"])},{Ifnull(data.Rows[i]["accessories"])},{Ifnull(data.Rows[i]["accessories_other"])},{Ifnull(data.Rows[i]["hemoglobin"])},{Ifnull(data.Rows[i]["leukocyte"])},{Ifnull(data.Rows[i]["platelet"])},{Ifnull(data.Rows[i]["blood_other"])},{Ifnull(data.Rows[i]["urine_protein"])},{Ifnull(data.Rows[i]["glycosuria"])},{Ifnull(data.Rows[i]["urine_acetone_bodies"])},{Ifnull(data.Rows[i]["bld"])},{Ifnull(data.Rows[i]["urine_other"])},{Ifnull(data.Rows[i]["blood_sugar"])},{Ifnull(data.Rows[i]["blood_group"])},{Ifnull(data.Rows[i]["blood_rh"])},{Ifnull(data.Rows[i]["sgft"])},{Ifnull(data.Rows[i]["ast"])},{Ifnull(data.Rows[i]["albumin"])},{Ifnull(data.Rows[i]["total_bilirubin"])},{Ifnull(data.Rows[i]["conjugated_bilirubin"])},{Ifnull(data.Rows[i]["scr"])},{Ifnull(data.Rows[i]["blood_urea"])},{Ifnull(data.Rows[i]["vaginal_fluid"])},{Ifnull(data.Rows[i]["vaginal_fluid_other"])},{Ifnull(data.Rows[i]["vaginal_cleaning"])},{Ifnull(data.Rows[i]["hb"])},{Ifnull(data.Rows[i]["hbsab"])},{Ifnull(data.Rows[i]["hbeag"])},{Ifnull(data.Rows[i]["hbeab"])},{Ifnull(data.Rows[i]["hbcab"])},{Ifnull(data.Rows[i]["syphilis"])},{Ifnull(data.Rows[i]["hiv"])},{Ifnull(data.Rows[i]["b_ultrasonic"])},{Ifnull(data.Rows[i]["other"])},{Ifnull(data.Rows[i]["general_assessment"])},{Ifnull(data.Rows[i]["assessment_error"])},{Ifnull(data.Rows[i]["health_guidance"])},{Ifnull(data.Rows[i]["health_guidance_other"])},{Ifnull(data.Rows[i]["transfer_treatment"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        yfdycid += $"'{data.Rows[i]["id"]}',";
                    }
                }
                #endregion

                DataSet fjhdyc = DbHelperMySQL.Query($@"select * from tuberculosis_info where (upload_status=0 or upload_status=2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (fjhdyc != null && fjhdyc.Tables.Count > 0 && fjhdyc.Tables[0].Rows.Count > 0)
                {
                    DataTable data = fjhdyc.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From tuberculosis_info where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into tuberculosis_info (id,name,archive_no,id_number,visit_date,visit_type,patient_type,sputum_bacterium_type,drug_fast_type,symptom,symptom_other,chemotherapy_plan,`usage`,drugs_type,supervisor_type,supervisor_other,single_room,ventilation,smoke_now,smoke_next,drink_now,drink_next,get_medicine_address,get_medicine_date,medicine_record,medicine_leave,treatment_course,erratically,untoward_effect,further_consultation,insist,habits_customs,intimate_contact,next_visit_date,estimate_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["archive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["visit_type"])},{Ifnull(data.Rows[i]["patient_type"])},{Ifnull(data.Rows[i]["sputum_bacterium_type"])},{Ifnull(data.Rows[i]["drug_fast_type"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["symptom_other"])},{Ifnull(data.Rows[i]["chemotherapy_plan"])},{Ifnull(data.Rows[i]["usage"])},{Ifnull(data.Rows[i]["drugs_type"])},{Ifnull(data.Rows[i]["supervisor_type"])},{Ifnull(data.Rows[i]["supervisor_other"])},{Ifnull(data.Rows[i]["single_room"])},{Ifnull(data.Rows[i]["ventilation"])},{Ifnull(data.Rows[i]["smoke_now"])},{Ifnull(data.Rows[i]["smoke_next"])},{Ifnull(data.Rows[i]["drink_now"])},{Ifnull(data.Rows[i]["drink_next"])},{Ifnull(data.Rows[i]["get_medicine_address"])},{Ifnull(data.Rows[i]["get_medicine_date"])},{Ifnull(data.Rows[i]["medicine_record"])},{Ifnull(data.Rows[i]["medicine_leave"])},{Ifnull(data.Rows[i]["treatment_course"])},{Ifnull(data.Rows[i]["erratically"])},{Ifnull(data.Rows[i]["untoward_effect"])},{Ifnull(data.Rows[i]["further_consultation"])},{Ifnull(data.Rows[i]["insist"])},{Ifnull(data.Rows[i]["habits_customs"])},{Ifnull(data.Rows[i]["intimate_contact"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["estimate_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        fjhdycid += $"'{data.Rows[i]["id"]}',";
                    }
                }

                DataSet nlrzyyjk = DbHelperMySQL.Query($@"select * from elderly_tcm_record where (upload_status=0 or upload_status=2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (nlrzyyjk != null && nlrzyyjk.Tables.Count > 0 && nlrzyyjk.Tables[0].Rows.Count > 0)
                {
                    DataTable data = nlrzyyjk.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From elderly_tcm_record where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into elderly_tcm_record (id,name,archive_no,id_number,test_date,answer_result,qixuzhi_score,qixuzhi_result,yangxuzhi_score,yangxuzhi_result,yinxuzhi_score,yinxuzhi_result,tanshizhi_score,tanshizhi_result,shirezhi_score,shirezhi_result,xueyuzhi_score,xueyuzhi_result,qiyuzhi_score,qiyuzhi_result,tebingzhi_sorce,tebingzhi_result,pinghezhi_sorce,pinghezhi_result,test_doctor,tcm_guidance,create_user,create_name,create_org,create_org_name,create_time,upload_status,exam_id) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["test_date"])},{Ifnull(data.Rows[i]["answer_result"])},{Ifnull(data.Rows[i]["qixuzhi_score"])},{Ifnull(data.Rows[i]["qixuzhi_result"])},{Ifnull(data.Rows[i]["yangxuzhi_score"])},{Ifnull(data.Rows[i]["yangxuzhi_result"])},{Ifnull(data.Rows[i]["yinxuzhi_score"])},{Ifnull(data.Rows[i]["yinxuzhi_result"])},{Ifnull(data.Rows[i]["tanshizhi_score"])},{Ifnull(data.Rows[i]["tanshizhi_result"])},{Ifnull(data.Rows[i]["shirezhi_score"])},{Ifnull(data.Rows[i]["shirezhi_result"])},{Ifnull(data.Rows[i]["xueyuzhi_score"])},{Ifnull(data.Rows[i]["xueyuzhi_result"])},{Ifnull(data.Rows[i]["qiyuzhi_score"])},{Ifnull(data.Rows[i]["qiyuzhi_result"])},{Ifnull(data.Rows[i]["tebingzhi_sorce"])},{Ifnull(data.Rows[i]["tebingzhi_result"])},{Ifnull(data.Rows[i]["pinghezhi_sorce"])},{Ifnull(data.Rows[i]["pinghezhi_result"])},{Ifnull(data.Rows[i]["test_doctor"])},{Ifnull(data.Rows[i]["tcm_guidance"])},
{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])},{Ifnull(data.Rows[i]["exam_id"])}
);");
                        nlrzyyjkid += $"'{data.Rows[i]["id"]}',";
                    }
                }

                DataSet fjhsf = DbHelperMySQL.Query($@"select * from tuberculosis_follow_record where (upload_status=0 or upload_status=2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (fjhsf != null && fjhsf.Tables.Count > 0 && fjhsf.Tables[0].Rows.Count > 0)
                {
                    DataTable data = fjhsf.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From tuberculosis_follow_record where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into tuberculosis_follow_record (id,name,archive_no,id_number,visit_date,month_order,supervisor_type,visit_type,symptom,symptom_other,smoke_now,smoke_next,drink_now,drink_next,chemotherapy_plan,`usage`,drugs_type,miss,untoward_effect,untoward_effect_info,complication,complication_info,transfer_treatment_department,transfer_treatment_reason,twoweek_visit_result,handling_suggestion,next_visit_date,visit_doctor,stop_date,stop_reason,must_visit_num,actual_visit_num,must_medicine_num,actual_medicine_num,medicine_rate,estimate_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["month_order"])},{Ifnull(data.Rows[i]["supervisor_type"])},{Ifnull(data.Rows[i]["visit_type"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["symptom_other"])},{Ifnull(data.Rows[i]["smoke_now"])},{Ifnull(data.Rows[i]["smoke_next"])},{Ifnull(data.Rows[i]["drink_now"])},{Ifnull(data.Rows[i]["drink_next"])},{Ifnull(data.Rows[i]["chemotherapy_plan"])},{Ifnull(data.Rows[i]["usage"])},{Ifnull(data.Rows[i]["drugs_type"])},{Ifnull(data.Rows[i]["miss"])},{Ifnull(data.Rows[i]["untoward_effect"])},{Ifnull(data.Rows[i]["untoward_effect_info"])},{Ifnull(data.Rows[i]["complication"])},{Ifnull(data.Rows[i]["complication_info"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["twoweek_visit_result"])},{Ifnull(data.Rows[i]["handling_suggestion"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["stop_date"])},{Ifnull(data.Rows[i]["stop_reason"])},{Ifnull(data.Rows[i]["must_visit_num"])},{Ifnull(data.Rows[i]["actual_visit_num"])},{Ifnull(data.Rows[i]["must_medicine_num"])},{Ifnull(data.Rows[i]["actual_medicine_num"])},{Ifnull(data.Rows[i]["medicine_rate"])},{Ifnull(data.Rows[i]["estimate_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        fjhsfid += $"'{data.Rows[i]["id"]}',";
                    }
                }

                DataSet jsbgr = DbHelperMySQL.Query($@"select * from psychosis_info where (upload_status=0 or upload_status=2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (jsbgr != null && jsbgr.Tables.Count > 0 && jsbgr.Tables[0].Rows.Count > 0)
                {
                    DataTable data = jsbgr.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From psychosis_info where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into psychosis_info (id,name,archive_no,id_number,guardian_name,guardian_relation,guardian_address,guardian_phone,neighborhood_committee_linkman,neighborhood_committee_linktel,resident_type,employment_status,agree_manage,agree_name,agree_date,first_morbidity_date,symptom,isolation,outpatient,first_medicine_date,hospitalized_num,diagnosis,diagnosis_hospital,diagnosis_date,recently_treatment_effect,dangerous_act,slight_trouble_num,cause_trouble_num,cause_accident_num,harm_other_num,autolesion_num,attempted_suicide_num,economics,specialist_suggestion,record_date,record_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["archive_no"])},{Ifnull(data.Rows[i]["id_number"])},{Ifnull(data.Rows[i]["guardian_name"])},{Ifnull(data.Rows[i]["guardian_relation"])},{Ifnull(data.Rows[i]["guardian_address"])},{Ifnull(data.Rows[i]["guardian_phone"])},{Ifnull(data.Rows[i]["neighborhood_committee_linkman"])},{Ifnull(data.Rows[i]["neighborhood_committee_linktel"])},{Ifnull(data.Rows[i]["resident_type"])},{Ifnull(data.Rows[i]["employment_status"])},{Ifnull(data.Rows[i]["agree_manage"])},{Ifnull(data.Rows[i]["agree_name"])},{Ifnull(data.Rows[i]["agree_date"])},{Ifnull(data.Rows[i]["first_morbidity_date"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["isolation"])},{Ifnull(data.Rows[i]["outpatient"])},{Ifnull(data.Rows[i]["first_medicine_date"])},{Ifnull(data.Rows[i]["hospitalized_num"])},{Ifnull(data.Rows[i]["diagnosis"])},{Ifnull(data.Rows[i]["diagnosis_hospital"])},{Ifnull(data.Rows[i]["diagnosis_date"])},{Ifnull(data.Rows[i]["recently_treatment_effect"])},{Ifnull(data.Rows[i]["dangerous_act"])},{Ifnull(data.Rows[i]["slight_trouble_num"])},{Ifnull(data.Rows[i]["cause_trouble_num"])},{Ifnull(data.Rows[i]["cause_accident_num"])},{Ifnull(data.Rows[i]["harm_other_num"])},{Ifnull(data.Rows[i]["autolesion_num"])},{Ifnull(data.Rows[i]["attempted_suicide_num"])},{Ifnull(data.Rows[i]["economics"])},{Ifnull(data.Rows[i]["specialist_suggestion"])},{Ifnull(data.Rows[i]["record_date"])},{Ifnull(data.Rows[i]["record_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        jsbgrid += $"'{data.Rows[i]["id"]}',";
                    }
                }

                DataSet jsbsf = DbHelperMySQL.Query($@"select * from psychosis_follow_record where (upload_status=0 or upload_status=2 ) and Cardcode in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (jsbsf != null && jsbsf.Tables.Count > 0 && jsbsf.Tables[0].Rows.Count > 0)
                {
                    DataTable data = jsbsf.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From psychosis_follow_record where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into psychosis_follow_record (id,name,archive_no,id_number,visit_date,visit_type,miss_reason,miss_reason_other,die_date,die_reason,physical_disease,die_reason_other,fatalness,symptom,symptom_other,insight,sleep_status,dietary_status,self_help,housework,work,learning_ability,interpersonal,dangerous_act,slight_trouble_num,cause_trouble_num,cause_accident_num,harm_other_num,autolesion_num,attempted_suicide_num,isolation,hospitalized_status,out_hospital_date,laboratory_examination,compliance,untoward_effect,untoward_effect_info,treatment_effect,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,rehabilitation_measure,rehabilitation_measure_other,next_visit_classify,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["archive_no"])},{Ifnull(data.Rows[i]["Cardcode"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["visit_type"])},{Ifnull(data.Rows[i]["miss_reason"])},{Ifnull(data.Rows[i]["miss_reason_other"])},{Ifnull(data.Rows[i]["die_date"])},{Ifnull(data.Rows[i]["die_reason"])},{Ifnull(data.Rows[i]["physical_disease"])},{Ifnull(data.Rows[i]["die_reason_other"])},{Ifnull(data.Rows[i]["fatalness"])},{Ifnull(data.Rows[i]["symptom"])},{Ifnull(data.Rows[i]["symptom_other"])},{Ifnull(data.Rows[i]["insight"])},{Ifnull(data.Rows[i]["sleep_status"])},{Ifnull(data.Rows[i]["dietary_status"])},{Ifnull(data.Rows[i]["self_help"])},{Ifnull(data.Rows[i]["housework"])},{Ifnull(data.Rows[i]["work"])},{Ifnull(data.Rows[i]["learning_ability"])},{Ifnull(data.Rows[i]["interpersonal"])},{Ifnull(data.Rows[i]["dangerous_act"])},{Ifnull(data.Rows[i]["slight_trouble_num"])},{Ifnull(data.Rows[i]["cause_trouble_num"])},{Ifnull(data.Rows[i]["cause_accident_num"])},{Ifnull(data.Rows[i]["harm_other_num"])},{Ifnull(data.Rows[i]["autolesion_num"])},{Ifnull(data.Rows[i]["attempted_suicide_num"])},{Ifnull(data.Rows[i]["isolation"])},{Ifnull(data.Rows[i]["hospitalized_status"])},{Ifnull(data.Rows[i]["out_hospital_date"])},{Ifnull(data.Rows[i]["laboratory_examination"])},{Ifnull(data.Rows[i]["compliance"])},{Ifnull(data.Rows[i]["untoward_effect"])},{Ifnull(data.Rows[i]["untoward_effect_info"])},{Ifnull(data.Rows[i]["treatment_effect"])},{Ifnull(data.Rows[i]["transfer_treatment"])},{Ifnull(data.Rows[i]["transfer_treatment_reason"])},{Ifnull(data.Rows[i]["transfer_treatment_department"])},{Ifnull(data.Rows[i]["rehabilitation_measure"])},{Ifnull(data.Rows[i]["rehabilitation_measure_other"])},{Ifnull(data.Rows[i]["next_visit_classify"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        jsbsfid += $"'{data.Rows[i]["id"]}',";
                    }
                }

                DataSet xsrt = DbHelperMySQL.Query($@"select * from children_tcm_record where (upload_status=0 or upload_status=2 ) and id_number in({string.Join(",", ide.Select(m => m.ID).ToList())})");
                if (xsrt != null && xsrt.Tables.Count > 0 && xsrt.Tables[0].Rows.Count > 0)
                {
                    DataTable data = xsrt.Tables[0];
                    string id = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string sql = string.Format("Delete From children_tcm_record where id='{0}'", data.Rows[i]["id"].ToString());
                        sqllist.Add(sql);

                        sqllist.Add($@"insert into children_tcm_record (id,name,archive_no,id_number,age,visit_date,tcm_info,tcm_other,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) 
values({Ifnull(data.Rows[i]["id"])},{Ifnull(data.Rows[i]["name"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["aichive_no"])},{Ifnull(data.Rows[i]["age"])},{Ifnull(data.Rows[i]["visit_date"])},{Ifnull(data.Rows[i]["tcm_info"])},{Ifnull(data.Rows[i]["tcm_other"])},{Ifnull(data.Rows[i]["next_visit_date"])},{Ifnull(data.Rows[i]["visit_doctor"])},{Ifnull(data.Rows[i]["create_user"])},{Ifnull(data.Rows[i]["create_name"])},{Ifnull(data.Rows[i]["create_org"])},{Ifnull(data.Rows[i]["create_org_name"])},{Ifnull(Convert.ToDateTime(data.Rows[i]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))},{Ifnull(data.Rows[i]["upload_status"])}
);");
                        xsrtid += $"'{data.Rows[i]["id"]}',";
                    }
                }

                if (sqllist.Count < 1) { LoadingHelper.CloseForm(); MessageBox.Show("没有需要上传的数据,请稍后再试！"); return; }
                int run = DbHelperMySQL.ExecuteSqlTranYpt(sqllist);
                if (run > 0)
                {
                    if (pfrid != null && !"".Equals(pfrid))
                    {
                        sqllistz.Add($"update poor_follow_record set upload_status='1' where id in({pfrid.TrimEnd(',')});");
                    }
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
                    if (rtjkfwid != null && !"".Equals(rtjkfwid))
                    {
                        sqllistz.Add($"update children_health_record set upload_status='1' where id in({rtjkfwid.TrimEnd(',')});");
                    }
                    if (xsrjtid != null && !"".Equals(xsrjtid))
                    {
                        sqllistz.Add($"update neonatus_info set upload_status='1' where id in({xsrjtid.TrimEnd(',')});");
                    }
                    if (yfchid != null && !"".Equals(yfchid))
                    {
                        sqllistz.Add($"update gravida_after_record set upload_status='1' where id in({yfchid.TrimEnd(',')});");
                    }
                    if (yfcqid != null && !"".Equals(yfcqid))
                    {
                        sqllistz.Add($"update gravida_follow_record set upload_status='1' where id in({yfcqid.TrimEnd(',')});");
                    }
                    if (yfdycid != null && !"".Equals(yfdycid))
                    {
                        sqllistz.Add($"update gravida_info set upload_status='1' where id in({yfdycid.TrimEnd(',')});");
                    }
                    if (fjhdycid != null && !"".Equals(fjhdycid))
                    {
                        sqllistz.Add($"update tuberculosis_info set upload_status='1' where id in({fjhdycid.TrimEnd(',')});");
                    }
                    if (nlrzyyjkid != null && !"".Equals(nlrzyyjkid))
                    {
                        sqllistz.Add($"update elderly_tcm_record set upload_status='1' where id in({nlrzyyjkid.TrimEnd(',')});");
                    }
                    if (fjhsfid != null && !"".Equals(fjhsfid))
                    {
                        sqllistz.Add($"update tuberculosis_follow_record set upload_status='1' where id in({fjhsfid.TrimEnd(',')});");
                    }
                    if (jsbgrid != null && !"".Equals(jsbgrid))
                    {
                        sqllistz.Add($"update psychosis_info set upload_status='1' where id in({jsbgrid.TrimEnd(',')});");
                    }
                    if (jsbsfid != null && !"".Equals(jsbsfid))
                    {
                        sqllistz.Add($"update psychosis_follow_record set upload_status='1' where id in({jsbsfid.TrimEnd(',')});");
                    }
                    if (xsrtid != null && !"".Equals(xsrtid))
                    {
                        sqllistz.Add($"update children_tcm_record set upload_status='1' where id in({xsrtid.TrimEnd(',')});");
                    }
                    sqllistz.Add($"update zkhw_tj_bgdc set ShiFouTongBu='1' where id_number in ({string.Join(",", ide.Select(m => m.ID).ToList())});");
                    int reu1 = DbHelperMySQL.ExecuteSqlTran(sqllistz);
                    if (reu1 > 0)
                    {
                        LoadingHelper.CloseForm();
                        bean.loginLogBean lb = new bean.loginLogBean();
                        lb.name = frmLogin.name;
                        lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        lb.eventInfo = "数据上传成功！";
                        lb.type = "2";
                        lls.addCheckLog(lb);
                        button1_Click(null, null);
                        MessageBox.Show("数据上传成功！");
                    }
                    else
                    {
                        LoadingHelper.CloseForm();
                        MessageBox.Show("数据状态修改异常,请联系运维人员!");
                    }
                }
                else
                {
                    LoadingHelper.CloseForm();
                    bean.loginLogBean lb = new bean.loginLogBean();
                    lb.name = frmLogin.name;
                    lb.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    lb.eventInfo = "数据上传异常！";
                    lb.type = "2";
                    lls.addCheckLog(lb);
                    MessageBox.Show("数据上传异常,请联系运维人员!");
                    return;
                }
            }
            catch (Exception ex)
            {
                LoadingHelper.CloseForm();
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
                return "NULL";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(dataRow.ToString()))
                {
                    return "'" + dataRow.ToString().Trim() + "'";
                }
                else
                {
                    return "NULL";
                }
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
                //得到最新的文件

                //说明点击的列是DataGridViewButtonColumn列
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
                string id = dataGridView1["身份证号", e.RowIndex].Value.ToString();
                string name = dataGridView1["姓名", e.RowIndex].Value.ToString();
                string nameidstr = name + id;
                List<FileTimeInfo> list = new List<FileTimeInfo>();
                string dir = @str + $"/up/result";
                list = Common.GetLatestFileTimeInfo(dir, nameidstr);
                if (list.Count > 0)
                {
                    OpenPdf(list[0].FileName);
                    //OpenPdf(@str + $"/up/result/{name + id}.pdf");
                }
                else
                {
                    MessageBox.Show("找不到文件！");
                }

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string idnum = dataGridView1["身份证号", e.RowIndex].Value.ToString();
            DataTable dtgrgd = grjdDao.selectResdentDoctorId(idnum);
            string doctor_id = "";
            if (dtgrgd.Rows.Count > 0)
            {
                doctor_id = dtgrgd.Rows[0]["doctor_id"].ToString();
            }
            if (idnum == "" && doctor_id == "") { MessageBox.Show("患者身份证号或医生编号不正确!"); return; }
            string newurl = "http://1.85.36.75:8077/ehr/sdc/ehr/browse/noArchive_msg.jsp?duns=" + frmLogin.organCode + "&verifyCode=123&archiveId=" + idnum + "&flag=brows&doctorNo=" + doctor_id + "&random=0.9859470667327924";
            Form2 f2 = new Form2();
            f2.url = newurl;
            f2.ShowDialog();
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

        private void button4_Click(object sender, EventArgs e)
        {
            //得到选上传图片的人员
            List<ComboBoxData> _lst = new List<ComboBoxData>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                ComboBoxData combo = new ComboBoxData();
                if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                {
                    string id = dataGridView1["身份证号", i].Value.ToString();
                    combo.ID = id;
                    combo.BarCode = dataGridView1["条码号", i].Value.ToString();

                    combo.Name = dataGridView1["card_pic", i].Value.ToString();
                    _lst.Add(combo);
                }
            }
            if (_lst.Count <= 0)
            {
                MessageBox.Show("请选择要上传的人员！");
                return;
            }
            LoadingHelper.myCaption = "正在上传...";
            LoadingHelper.myLabel = "正在上传...";
            LoadingHelper.ShowLoadingScreen();
            Thread.Sleep(50);
            try
            {
                //根据id_number和bar_code找到对应的b超图片
                string sWhere = "";
                for (int i = 0; i < _lst.Count; i++)
                {
                    string tmp = string.Format(" (id_number='{0}' and bar_code='{1}')", _lst[i].ID, _lst[i].BarCode);
                    if (sWhere == "")
                    {
                        sWhere = tmp;
                    }
                    else
                    {
                        sWhere = sWhere + " or " + tmp;
                    }
                }
                if (sWhere != "")
                {
                    sWhere = " Where " + sWhere;
                }
                DownLoadBChaoTuPian(sWhere);
                //根据id_number和bar_code找到对应的心电图图片 
                DownLoadXinDianTuPian(sWhere);
                //发送拍照的图片
                for (int i = 0; i < _lst.Count; i++)
                {
                    PushPaiZhaoImg(_lst[i].Name);
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    ComboBoxData combo = new ComboBoxData();
                    if ((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue == true)
                    {
                        dataGridView1["B超图片", i].Value = "1";
                        dataGridView1["心电图片", i].Value = "1";
                    }
                }

                LoadingHelper.CloseForm();
            }
            catch
            {
                LoadingHelper.CloseForm();
            }
        }

        #region 上传图片
        private bool PushPaiZhaoImg(string t)
        {
            bool flag = false;
            try
            {
                string fileName = Application.StartupPath + "\\cardImg\\" + t;
                if (File.Exists(fileName))
                {
                    byte[] a = File.ReadAllBytes(fileName);
                    flag = OSSClientHelper.PushImg(a, t, "cardtp2019");
                }
            }
            catch
            {

            }
            return flag;
        }

        private void DownLoadBChaoTuPian(string s)
        {
            string sql = string.Format("select id_number,bar_code,BuPic01,BuPic02,BuPic03,BuPic04 from zkhw_tj_bc {0} ", s);
            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable dt = dataSet.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string t1 = dt.Rows[i]["BuPic01"].ToString();
                string t2 = dt.Rows[i]["BuPic02"].ToString();
                string t3 = dt.Rows[i]["BuPic03"].ToString();
                string t4 = dt.Rows[i]["BuPic04"].ToString();
                //上传
                bool flag1 = true;
                bool flag2 = true;
                bool flag3 = true;
                bool flag4 = true;
                if (t1 != "")
                {
                    flag1 = PushBChaoImg(t1);
                }

                if (t2 != "")
                {
                    flag2 = PushBChaoImg(t2);
                }
                if (t3 != "")
                {
                    flag3 = PushBChaoImg(t3);
                }
                if (t4 != "")
                {
                    flag4 = PushBChaoImg(t4);
                }
                if (t1 == "" && t2 == "" && t3 == "" && t4 == "") continue;

                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true)
                {
                    string id = dt.Rows[i]["id_number"].ToString();
                    string bcode = dt.Rows[i]["bar_code"].ToString();
                    sql = string.Format("update zkhw_tj_bgdc set BChaoImage='1' Where id_number='{0}' and bar_code='{1}'", id, bcode);
                    DbHelperMySQL.ExecuteSql(sql);
                }
            }
        }

        private bool PushBChaoImg(string t)
        {
            bool flag = false;
            try
            {
                string fileName = Application.StartupPath + "\\bcImg\\" + t;
                if (File.Exists(fileName))
                {
                    byte[] a = File.ReadAllBytes(fileName);
                    flag = OSSClientHelper.PushImg(a, t, "bctp2019");
                }
            }
            catch
            {

            }
            return flag;
        }

        private void DownLoadXinDianTuPian(string s)
        {
            string sql = string.Format("select id_number,bar_code,imageUrl from zkhw_tj_xdt {0} ", s);
            DataSet dataSet = DbHelperMySQL.Query(sql);
            DataTable dt = dataSet.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string t1 = dt.Rows[i]["imageUrl"].ToString();
                //上传
                if (t1 != "")
                {
                    bool flag = PushXinDianImg(t1);
                    if (flag == true)
                    {
                        string id = dt.Rows[i]["id_number"].ToString();
                        string bcode = dt.Rows[i]["bar_code"].ToString();
                        sql = string.Format("update zkhw_tj_bgdc set XinDianImage='1' Where id_number='{0}' and bar_code='{1}'", id, bcode);
                        DbHelperMySQL.ExecuteSql(sql);
                    }
                }
            }
        }
        private bool PushXinDianImg(string t)
        {
            bool flag = false;
            try
            {
                string fileName = Application.StartupPath + "\\xdtImg\\" + t;
                if (File.Exists(fileName))
                {
                    byte[] a = File.ReadAllBytes(fileName);
                    flag = OSSClientHelper.PushImg(a, t, "xdtp2019");
                }
            }
            catch
            {

            }
            return flag;
        }
        #endregion

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 9 || e.ColumnIndex == 10 || e.ColumnIndex == 11)
            {
                string t = e.Value.ToString();
                if (t == "1")
                {
                    e.Value = "是";
                }
                else
                {
                    e.Value = "否";
                }
            }
        }
    }

    public class Report
    {
        public string Name { get; set; }
        public Document Doc { get; set; }
    }
}
