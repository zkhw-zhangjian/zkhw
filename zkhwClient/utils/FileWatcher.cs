using System;
using System.Collections.Generic;
using System.IO;

using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient
{
    public delegate void DelegateFileWatch(string msg);
    public delegate void DelegateFileWatchForAod(string msg);
    /// <summary>
    /// 监控文档
    /// </summary>
    public static class FileWatcher
    {
        /// <summary>
        /// 系统监控文件
        /// </summary>
        private static FileSystemWatcher m_watcherAoup = new FileSystemWatcher();

        public static String ErrorMsg = String.Empty;
        static String str = Application.StartupPath;
        static string path = @"config.xml";
        static string bcJudge = "";
        /// <summary>
        /// 改变时候触发
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <remarks>创建人员(日期): ★刘腾飞★(100202 18:16)</remarks> 
        public static void OnChangedForXinDianTu(object source, FileSystemEventArgs e)
        {
            //if (e.ChangeType == WatcherChangeTypes.Changed)
            //{
               // List<string> orderIdList = new List<string>();
                try
                {
                    //1.由于客户机器首次读取时乱码，故先修改该文件后再读取内容
                    //m_watcherAoup.EnableRaisingEvents = false;
                    try
                    {
                        File.SetLastWriteTime(e.FullPath, DateTime.Now);//修改txt文件的时间
                        #region 心电图
                        XmlDocument doc = new XmlDocument();
                        doc.Load(e.FullPath);
                        XmlNode xNode = doc.SelectSingleNode("zqecg/base/time");
                        string time = xNode.InnerText;
                        time= DateTime.Parse(time).ToString("yyyyMMddHHmmss");
                        XmlNode id = doc.SelectSingleNode("zqecg/patient/id");
                        string ids = id.InnerText;
                        XmlNode baseline_drift = doc.SelectSingleNode("zqecg/record/baseline_drift");
                        string baseline_drifts = baseline_drift.InnerText;
                        XmlNode myoelectricity = doc.SelectSingleNode("zqecg/record/myoelectricity");
                        string myoelectricitys = myoelectricity.InnerText;
                        XmlNode frequency = doc.SelectSingleNode("zqecg/record/frequency");
                        string frequencys = frequency.InnerText;
                        XmlNode hr = doc.SelectSingleNode("zqecg/measure/hr");
                        string hrs = hr.InnerText;
                        XmlNode pr = doc.SelectSingleNode("zqecg/measure/pr");
                        string prs = pr.InnerText;
                        XmlNode qrs = doc.SelectSingleNode("zqecg/measure/qrs");
                        string qrss = qrs.InnerText;
                        XmlNode qt_ = doc.SelectSingleNode("zqecg/measure/qt_");
                        string qt_s = qt_.InnerText;
                        XmlNode qtc = doc.SelectSingleNode("zqecg/measure/qtc");
                        string qtcs = qtc.InnerText;
                        XmlNode p_ = doc.SelectSingleNode("zqecg/measure/p_");
                        string p_s = p_.InnerText;
                        XmlNode qrs_ = doc.SelectSingleNode("zqecg/measure/qrs_");
                        string qrs_s = qrs_.InnerText;
                        XmlNode t = doc.SelectSingleNode("zqecg/measure/t");
                        string ts = t.InnerText;
                        XmlNode rv5 = doc.SelectSingleNode("zqecg/measure/rv5");
                        string rv5s = rv5.InnerText;
                        XmlNode sv1 = doc.SelectSingleNode("zqecg/measure/sv1");
                        string sv1s = sv1.InnerText;
                        XmlNode diagnosis = doc.SelectSingleNode("zqecg/result/diagnosis");
                        string diagnosiss = diagnosis.InnerText;
                        XmlNode advicetext = doc.SelectSingleNode("zqecg/result/advicetext");
                        string advicetexts = advicetext.InnerText;
                        jkInfoDao jkInfoDao = new jkInfoDao();
                        DataTable data = jkInfoDao.selectjkInfoBybarcode(ids);
                    if (data != null && data.Rows.Count > 0)
                    {
                        string aichive_no = data.Rows[0]["aichive_no"].ToString();
                        string barcode = data.Rows[0]["bar_code"].ToString();
                        string id_number = data.Rows[0]["id_number"].ToString();
                        DataTable dtnum = jkInfoDao.queryChongfuXdtData(aichive_no, barcode);
                        advicetexts = advicetexts.Replace("在不知道病人的性别/年龄情况下做的诊断结论", "").Replace("---", "").Replace("***","").Replace("~", "");
                        string issql = "";
                        MySqlParameter[] args = null;
                        string imgurl = id_number + "_" + time + ".jpg";
                        if (dtnum.Rows.Count < 1)
                        {
                            issql = "insert into zkhw_tj_xdt(id,aichive_no,id_number,bar_code,XdtResult,XdtDesc,XdtDoctor,PR,QRS,QT,QTc,hr,p,pqrs,t,rv5,sv1,baseline_drift,myoelectricity,frequency,createtime,imageUrl) values(@id,@aichive_no,@id_number,@bar_code,@XdtResult,@XdtDesc,@XdtDoctor,@PR,@QRS,@QT,@QTc,@hr,@p,@pqrs,@t,@rv5,@sv1,@baseline_drift,@myoelectricity,@frequency,@createtime,@imageUrl)";
                            args = new MySqlParameter[] {
                            new MySqlParameter("@id",Result.GetNewId()),
                            new MySqlParameter("@aichive_no", data.Rows[0]["aichive_no"].ToString()),
                            new MySqlParameter("@id_number", data.Rows[0]["id_number"].ToString()),
                            new MySqlParameter("@bar_code", data.Rows[0]["bar_code"].ToString()),
                            new MySqlParameter("@XdtResult", diagnosiss),
                            new MySqlParameter("@XdtDesc", advicetexts),
                            new MySqlParameter("@XdtDoctor", basicInfoSettings.xdt),
                            new MySqlParameter("@PR", prs),
                            new MySqlParameter("@QRS", qrss),
                            new MySqlParameter("@QT", qt_s),
                            new MySqlParameter("@QTc", qtcs),
                            new MySqlParameter("@hr", hrs),
                            new MySqlParameter("@p",p_s),
                            new MySqlParameter("@pqrs", qrs_s),
                            new MySqlParameter("@t", ts),
                            new MySqlParameter("@rv5", rv5s),
                            new MySqlParameter("@sv1", sv1s),
                            new MySqlParameter("@baseline_drift", baseline_drifts),
                            new MySqlParameter("@myoelectricity", myoelectricitys),
                            new MySqlParameter("@frequency", frequencys),
                            new MySqlParameter("@createtime", time),
                            new MySqlParameter("@imageUrl",imgurl)
                        };
                        }
                        else {
                            string issql1 = "update zkhw_tj_xdt set XdtResult='"+ diagnosiss + "',XdtDesc='" + advicetexts + "',PR='" + prs + "',QRS='" + qrss + "',QT='" + qt_s + "',QTc='" + qtcs + "',hr='" + hrs + "',p='" + p_s + "',pqrs='" + qrs_s + "',t='" + ts + "',rv5='" + rv5s + "',sv1='" + sv1s + "',baseline_drift='" + baseline_drifts + "',myoelectricity='" + myoelectricitys + "',frequency='" + frequencys + "',createtime='" + time + "',imageUrl='" + imgurl + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                            DbHelperMySQL.ExecuteSql(issql1);
                        }
                        string pName = e.FullPath.Replace("xml", "jpg");
                        FileInfo inf = new FileInfo(pName);
                        try
                        {
                            if (File.Exists(str + "\\xdtImg\\" + imgurl))
                            {
                                File.Delete(str + "\\xdtImg\\" + imgurl);
                                GC.Collect();
                            }
                        }
                        catch {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                            {
                                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\r\n" + "心电2次上传删除报错："+ imgurl);
                            }
                        }
                        inf.MoveTo(str + "\\xdtImg\\" + imgurl);
                        //inf.CopyTo(str + "\\xdtImg\\" + data.Rows[0]["aichive_no"].ToString() + "_" + ids + ".jpg");
                        string hxpl = (Int32.Parse(hrs) / 4).ToString().Trim();//计算呼吸频率
                        if (advicetexts.IndexOf("正常") > -1)
                        {
                            int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set cardiogram='1',cardiogram_img='{imgurl}',base_heartbeat='{hrs}',base_respiratory='{hxpl} ',examination_heart_rate='{hrs}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'and bar_code= '{data.Rows[0]["bar_code"].ToString()}'");
                            string istruedgbc = "update zkhw_tj_bgdc set XinDian='1' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                            DbHelperMySQL.ExecuteSql(istruedgbc);
                        }
                        else
                        {
                            if (advicetexts.Length>=50) {//限制健康体检表心电图检查结果长度50
                                advicetexts = advicetexts.Replace("异常心电图", "").Replace("窦性心律", "");
                            }
                            int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set cardiogram='2',cardiogram_memo='{advicetexts}',cardiogram_img='{imgurl}',base_heartbeat='{hrs}',base_respiratory='{hxpl} ',examination_heart_rate='{hrs}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'and bar_code= '{data.Rows[0]["bar_code"].ToString()}'");
                            string issqdgbc = "update zkhw_tj_bgdc set XinDian='3' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                            DbHelperMySQL.ExecuteSql(issqdgbc);
                        }

                        if (issql != "") { int rue = DbHelperMySQL.ExecuteSql(issql, args); }
                    }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        // RegisterAoupTrackLog("文件被占用！正在请求重试 ... ");
                        MessageBox.Show(ex.StackTrace);
                        //进程阻塞2秒钟
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.StackTrace);
                }
            //}
        }
        /// <summary>
        /// 改变时候触发
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <remarks>创建人员(日期): ★刘腾飞★(100202 18:16)</remarks> 
        public static void OnChangedForBChao(object source, FileSystemEventArgs e)
        {
            //if (e.ChangeType == WatcherChangeTypes.Changed)
            //{
            //List<string> orderIdList = new List<string>();
            int innum = 0;
            try
                {
                    selectXmlBcJudge();//获取B超检查结果是否正常
                    //1.由于客户机器首次读取时乱码，故先修改该文件后再读取内容
                    //m_watcherAoup.EnableRaisingEvents = false;
                    File.SetLastWriteTime(e.FullPath, DateTime.Now);//修改txt文件的时间
                    #region B超
                    XmlDocument doc = new XmlDocument();
                    doc.Load(e.FullPath);
                    XmlNodeList xNode = doc.SelectNodes("//Report[@Index='图片信息']/图片信息");
                    string id = doc.SelectSingleNode("//Report[@Index='病人信息']/ID").InnerText.Trim();
                    string nam = doc.SelectSingleNode("//Report[@Index='基本信息']/报告名称").InnerText.Trim();
                    string cs = doc.SelectSingleNode("//Report[@Index='超声诊断']/超声诊断").InnerText.Trim();
                    string csresult = "";
                    if (cs.Length >0)
                    {
                        cs = cs.Replace("|","").Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                        int num = cs.IndexOf("诊断结果");
                        csresult=cs.Substring(num);
                    }
                    string BuPic01 = string.Empty;
                    string BuPic02 = string.Empty;
                    string BuPic03 = string.Empty;
                    string BuPic04 = string.Empty;
                    
                    Thread.Sleep(5000);
                    GC.Collect();
                    jkInfoDao jkInfoDao = new jkInfoDao();
                    DataTable data = jkInfoDao.selectjkInfoBybarcode(id);
                if (data != null && data.Rows.Count > 0)
                {
                    string aichive_no = data.Rows[0]["aichive_no"].ToString();
                    string barcode = data.Rows[0]["bar_code"].ToString();
                    string id_number = data.Rows[0]["id_number"].ToString();

                    if (xNode != null && xNode.Count > 0)
                    {
                        innum = e.FullPath.LastIndexOf("\\");
                        string fullPath = e.FullPath.Substring(0, innum + 1);
                        for (int i = 0; i < xNode.Count; i++)
                        //for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                BuPic01 = xNode[i].InnerText;
                                string pName = fullPath + BuPic01;
                                FileInfo inf = new FileInfo(pName);
                                if (File.Exists(str + "\\bcImg\\" + id_number + "_" + BuPic01))
                                {
                                    File.Delete(str + "\\bcImg\\" + id_number + "_" + BuPic01);
                                }
                                inf.MoveTo(str + "\\bcImg\\" + id_number + "_" + BuPic01);
                            }
                            else if (i == 1)
                            {
                                BuPic02 = xNode[i].InnerText;
                                string pName = fullPath + BuPic02;
                                FileInfo inf = new FileInfo(pName);
                                if (File.Exists(str + "\\bcImg\\" + id_number + "_" + BuPic02))
                                {
                                    File.Delete(str + "\\bcImg\\" + id_number + "_" + BuPic02);
                                }
                                inf.MoveTo(str + "\\bcImg\\" + id_number + "_" + BuPic02);
                            }
                            else if (i == 2)
                            {
                                BuPic03 = xNode[i].InnerText;
                                string pName = fullPath + BuPic03;
                                FileInfo inf = new FileInfo(pName);
                                if (File.Exists(str + "\\bcImg\\" + id_number + "_" + BuPic03))
                                {
                                    File.Delete(str + "\\bcImg\\" + id_number + "_" + BuPic03);
                                }
                                inf.MoveTo(str + "\\bcImg\\" + id_number + "_" + BuPic03);
                            }
                            else if (i == 3)
                            {
                                BuPic04 = xNode[i].InnerText;
                                string pName = fullPath + BuPic04;
                                FileInfo inf = new FileInfo(pName);
                                if (File.Exists(str + "\\bcImg\\" + id_number + "_" + BuPic04))
                                {
                                    File.Delete(str + "\\bcImg\\" + id_number + "_" + BuPic04);
                                }
                                inf.MoveTo(str + "\\bcImg\\" + id_number + "_" + BuPic04);
                            }
                        }
                    }

                    DataTable dtnum = jkInfoDao.queryChongfuBcData(aichive_no, barcode);
                    string issql = "";
                    MySqlParameter[] args = null;
                    BuPic01 = id_number + "_" + BuPic01;
                    BuPic02 = id_number + "_" + BuPic02;
                    if (dtnum.Rows.Count < 1)
                    {
                        issql = "insert into zkhw_tj_bc(id,aichive_no,id_number,bar_code,FubuResult,FubuBC,BuPic01,BuPic02,createtime,ZrysBC) values(@id,@aichive_no,@id_number,@bar_code,@FubuResult,@FubuBC,@BuPic01,@BuPic02,@createtime,@ZrysBC)";
                        args = new MySqlParameter[] {
                        new MySqlParameter("@id",Result.GetNewId()),
                        new MySqlParameter("@aichive_no", data.Rows[0]["aichive_no"].ToString()),
                        new MySqlParameter("@id_number", data.Rows[0]["id_number"].ToString()),
                        new MySqlParameter("@bar_code", data.Rows[0]["bar_code"].ToString()),
                        new MySqlParameter("@FubuResult", cs),
                        new MySqlParameter("@FubuBC", nam),
                        new MySqlParameter("@BuPic01", BuPic01),
                        new MySqlParameter("@BuPic02", BuPic02),
                        new MySqlParameter("@createtime", DateTime.Now),
                        new MySqlParameter("@ZrysBC", basicInfoSettings.bc)
                    };
                    }
                    else {
                        string issql1 = "update zkhw_tj_bc set FubuResult= '" + cs + "',FubuBC= '" + nam + "',BuPic01= '" +BuPic01 + "',BuPic02= '" + BuPic02 + "' where aichive_no = '" + data.Rows[0]["aichive_no"].ToString() + "' and bar_code='" + data.Rows[0]["bar_code"].ToString() + "'";
                        DbHelperMySQL.ExecuteSql(issql1);
                    }

                    if (!string.IsNullOrWhiteSpace(BuPic01))
                    {
                        BuPic01 = id_number + "_"+BuPic01;
                    }

                    if (bcJudge != "" && bcJudge.Length >= 10) {
                       string[] bcJudgeArray= bcJudge.Split('#');
                        if (bcJudgeArray.Length>0) {
                            bool istrue = false;
                            for (int i=0;i< bcJudgeArray.Length;i++) {
                                if (cs.IndexOf(bcJudgeArray[i]) >-1) {
                                    istrue = true;
                                    break;
                                }
                            }
                            if (istrue)
                            {
                                int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set ultrasound_abdomen='1',abdomenB_img='{BuPic01}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'");
                                string issqdgbc = "update zkhw_tj_bgdc set BChao='1' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                                DbHelperMySQL.ExecuteSql(issqdgbc);
                            }
                            else {
                                int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set ultrasound_abdomen='2',ultrasound_memo='{csresult}',abdomenB_img='{BuPic01}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'");
                                string issqdgbc = "update zkhw_tj_bgdc set BChao='3' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                                DbHelperMySQL.ExecuteSql(issqdgbc);
                            }
                        }
                    }
                    else {
                        if (cs.IndexOf("肝,胆,胰,脾未见异常") > -1 || cs.IndexOf("未见明显异常") > -1)
                        {
                            int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set ultrasound_abdomen='1',abdomenB_img='{BuPic01}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'");
                            string issqdgbc = "update zkhw_tj_bgdc set BChao='1' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                            DbHelperMySQL.ExecuteSql(issqdgbc);
                        }
                        else
                        {
                            int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set ultrasound_abdomen='2',ultrasound_memo='{csresult}',abdomenB_img='{BuPic01}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'");
                            string issqdgbc = "update zkhw_tj_bgdc set BChao='3' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                            DbHelperMySQL.ExecuteSql(issqdgbc);
                        }
                    }
                    if (issql!="") { int rue = DbHelperMySQL.ExecuteSql(issql, args); }
                    string filepath = e.FullPath.Substring(0, innum);
                    DeleteDir(filepath);
                    //插入数据库
                }
                    #endregion
                }
                catch (Exception ex)
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ex.Message + "\r\n" + ex.StackTrace);
                    }
                string filepath = e.FullPath.Substring(0, innum);
                DeleteDir(filepath);
            }
           // }
        }
        public static void DeleteDir(string file)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
                //判断文件夹是否还存在

                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f)) 
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f);
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(file);
               }
            }
            catch // 异常处理
            {
            }
        }
        public static void selectXmlBcJudge()
        {
            try
            { 
                bean.ConfigInfo obj = null;
                string s = "Where Name='安盛B超'";
                ConfigInfoManage cdal = new ConfigInfoManage();
                obj = cdal.GetObj(s);
                if(obj == null)
                {
                    bcJudge = "未见明显异常#肝,胆,胰,脾未见异常";
                }
                else
                {
                    bcJudge = obj.Content;
                }
            }
            catch // 异常处理
            {
                bcJudge = "未见明显异常#肝,胆,胰,脾未见异常";
            }
        }
   }
}
