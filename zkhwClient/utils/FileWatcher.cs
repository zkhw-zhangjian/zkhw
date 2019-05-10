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
        /// 察看监听状态
        /// </summary>
        /// <remarks>创建人员(日期):★刘腾飞★(100319 15:09)</remarks>
        public static event DelegateFileWatch EventFileWatch;
        /// <summary>
        /// 察看 AD和 AOD文件监听状态
        /// </summary>
        /// <remarks>创建人员(日期):★刘腾飞★(100319 15:09)</remarks>
        public static event DelegateFileWatchForAod EventFileWatchForAod;
        /// <summary>
        /// 系统监控文件
        /// </summary>
        private static FileSystemWatcher m_watcherAoup = new FileSystemWatcher();
        /// <summary>
        /// 系统监控 AD和 AOD 文件
        /// </summary>
        private static FileSystemWatcher m_watcherForAod = new FileSystemWatcher();
        /// <summary>
        /// 错误信息
        /// <remarks>创建人员(日期):★刘腾飞★(100319 14:16)</remarks>
        /// </summary>
        public static String ErrorMsg = String.Empty;
        static String str = Application.StartupPath;
        #region 方法

        /// <summary>
        /// 注册跟踪日志
        /// </summary>
        /// <param name="msg">信息</param>
        ///  <remarks>创建人员(日期):★刘腾飞★(100608 14:56)</remarks>
        private static void RegisterAoupTrackLog(string msg)
        {
            try
            {
                //AOUP监听成功！
                if (EventFileWatch != null)
                    EventFileWatch(msg);

                //WatcherMsgList.RegisterMsg(string.Format("[{0}.{1}] {2}",
                //    DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.Millisecond, msg));
            }
            catch (Exception ex)
            {
                // RegisterLog.ExceptionsStack.RegisterError(ex);
            }
        }
        /// <summary>
        /// 注册跟踪日志
        /// </summary>
        /// <param name="msg">信息</param>
        ///  <remarks>创建人员(日期):★刘腾飞★(100624 11:23)</remarks>
        private static void RegisterAodTrackLog(string msg)
        {
            try
            {
                //AOUP监听成功！
                if (EventFileWatchForAod != null)
                    EventFileWatchForAod(msg);

                //WatcherMsgList.RegisterMsg(string.Format("[{0}.{1}] {2}",
                //    DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.Millisecond, msg));
            }
            catch (Exception ex)
            {
                // RegisterLog.ExceptionsStack.RegisterError(ex);
            }
        }

        #endregion

        #region 监听目录

        /// <summary>
        /// 监听 心电图
        /// </summary>
        ///  <remarks>创建人员(日期): ★刘腾飞★(100202 18:16)</remarks>
        public static void WatcheDirForXinDianTu()
        {
            try
            {
                string watchPath = @"D:\Examine\xindiantu";//去掉文件夹的只读权限

                m_watcherAoup.Path = watchPath;
                m_watcherAoup.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime |
                    NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess |
                    NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
                // Only watch text files.

                m_watcherAoup.Filter = "*.xml";

                m_watcherAoup.Changed += new FileSystemEventHandler(OnChangedForXinDianTu);
                m_watcherAoup.EnableRaisingEvents = true;
                //修改txt文件的时间，用于系统启动时监听一次AOUP的修改
                //File.SetLastWriteTime(watchPath, DateTime.Now);


                #region B超

                #endregion


                return;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 监听 b 超
        /// </summary>
        ///  <remarks>创建人员(日期): ★刘腾飞★(100202 18:16)</remarks>
        public static void WatcheDirForBChao()
        {
            try
            {
                string watchPath = @"D:\Examine\bc";//去掉文件夹的只读权限

                m_watcherAoup.Path = watchPath;
                m_watcherAoup.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime |
                    NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess |
                    NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
                // Only watch text files.

                m_watcherAoup.Filter = "*.xml";

                m_watcherAoup.Changed += new FileSystemEventHandler(OnChangedForBChao);
                m_watcherAoup.EnableRaisingEvents = true;
                //修改txt文件的时间，用于系统启动时监听一次AOUP的修改
                //File.SetLastWriteTime(watchPath, DateTime.Now);


                #region B超

                #endregion


                return;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }



        /// <summary>
        /// AOD 文件创建时出发
        /// </summary>
        ///  <remarks>创建人员(日期):★刘腾飞★(100603 23:34)</remarks>
        private static void OnCreatedForAod(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                try
                {
                    RegisterAodTrackLog("AOD 创建成功，等待 读取 ...");
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }
                finally
                {
                    //如果遇到异常关闭了监听，则重新打开监听
                    if (!m_watcherForAod.EnableRaisingEvents)
                        m_watcherForAod.EnableRaisingEvents = true;
                }
            }
        }


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
                        //File.SetLastWriteTime(e.FullPath, DateTime.Now);//修改txt文件的时间
                        #region 心电图
                        XmlDocument doc = new XmlDocument();
                        doc.Load(e.FullPath);
                        XmlNode xNode = doc.SelectSingleNode("zqecg/base/time");
                        string time = xNode.InnerText;
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
                            DataTable dtnum= jkInfoDao.queryChongfuXdtData(aichive_no, barcode);
                            if (dtnum.Rows.Count>0) { return; }
                            string issql = "insert into zkhw_tj_xdt(id,aichive_no,id_number,bar_code,XdtResult,XdtDesc,PR,QRS,QT,QTc,hr,p,pqrs,t,rv5,sv1,baseline_drift,myoelectricity,frequency,createtime,imageUrl) values(@id,@aichive_no,@id_number,@bar_code,@XdtResult,@XdtDesc,@PR,@QRS,@QT,@QTc,@hr,@p,@pqrs,@t,@rv5,@sv1,@baseline_drift,@myoelectricity,@frequency,@createtime,@imageUrl)";
                            MySqlParameter[] args = new MySqlParameter[] {
                            new MySqlParameter("@id",Result.GetNewId()),
                            new MySqlParameter("@aichive_no", data.Rows[0]["aichive_no"].ToString()),
                            new MySqlParameter("@id_number", data.Rows[0]["id_number"].ToString()),
                            new MySqlParameter("@bar_code", data.Rows[0]["bar_code"].ToString()),
                            new MySqlParameter("@XdtResult", diagnosiss),
                            new MySqlParameter("@XdtDesc", advicetexts),
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
                            new MySqlParameter("@imageUrl", data.Rows[0]["aichive_no"].ToString()+"_"+ids+".jpg")
                        };
                        string pName = e.FullPath.Replace("xml","jpg");
                        FileInfo inf = new FileInfo(pName);
                        if (File.Exists(str + "\\xdtImg\\" + data.Rows[0]["aichive_no"].ToString() + "_" + ids + ".jpg"))
                        {
                            File.Delete(str + "\\xdtImg\\" + data.Rows[0]["aichive_no"].ToString() + "_" + ids + ".jpg");
                        }
                        inf.MoveTo(str + "\\xdtImg\\" + data.Rows[0]["aichive_no"].ToString()+"_" + ids + ".jpg");

                        if (diagnosiss.IndexOf("正常")>-1)
                            {
                                int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set cardiogram='1',cardiogram_img='{ids + ".jpg"}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'and bar_code= '{data.Rows[0]["bar_code"].ToString()}'");
                            }
                            else
                            {
                                int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set cardiogram='2',cardiogram_img='{ids + ".jpg"}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'and bar_code= '{data.Rows[0]["bar_code"].ToString()}'");
                            }
                            string issqdgbc = "update zkhw_tj_bgdc set XinDian='1' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                            DbHelperMySQL.ExecuteSql(issqdgbc);
                            int rue = DbHelperMySQL.ExecuteSql(issql, args);
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
                try
                {
                    //1.由于客户机器首次读取时乱码，故先修改该文件后再读取内容
                    //m_watcherAoup.EnableRaisingEvents = false;
                    File.SetLastWriteTime(e.FullPath, DateTime.Now);//修改txt文件的时间
                    #region B超
                    XmlDocument doc = new XmlDocument();
                    doc.Load(e.FullPath);
                    XmlNodeList xNode = doc.SelectNodes("//Report[@Index='图片信息']/图片信息");
                    string id = doc.SelectSingleNode("//Report[@Index='病人信息']/ID").InnerText.Trim();
                    string nam = doc.SelectSingleNode("//Report[@Index='基本信息']/报告名称").InnerText.Trim();
                    string cs = doc.SelectSingleNode("//Report[@Index='超声诊断']/超声诊断").InnerText;
                    string BuPic01 = string.Empty;
                    string BuPic02 = string.Empty;
                    string BuPic03 = string.Empty;
                    string BuPic04 = string.Empty;
                    int innum = 0;
                if (xNode != null && xNode.Count > 0)
                    {
                    innum=e.FullPath.LastIndexOf("\\");
                    Thread.Sleep(3000);
                    string fullPath=e.FullPath.Substring(0, innum + 1);
                        for (int i = 0; i < xNode.Count; i++)
                        {
                            if (i == 0)
                            {
                                BuPic01 = xNode[i].InnerText;
                                string pName = fullPath + BuPic01;
                                FileInfo inf = new FileInfo(pName);
                                if (File.Exists(str + "\\bcImg\\" + BuPic01))
                                {
                                    File.Delete(str + "\\bcImg\\" + BuPic01);
                                }
                                inf.MoveTo(str + "\\bcImg\\" + BuPic01);
                            }
                            else if (i == 1)
                            {
                                BuPic02 = xNode[i].InnerText;
                                string pName = fullPath + BuPic02;
                                FileInfo inf = new FileInfo(pName);
                                if (File.Exists(str + "\\bcImg\\" + BuPic02))
                                {
                                    File.Delete(str + "\\bcImg\\" + BuPic02);
                                }
                                inf.MoveTo(str + "\\bcImg\\" + BuPic02);
                            }
                            else if (i == 2)
                            {
                                BuPic03 = xNode[i].InnerText;
                                string pName = fullPath + BuPic03;
                                FileInfo inf = new FileInfo(pName);
                                if (File.Exists(str + "\\bcImg\\" + BuPic03))
                                {
                                    File.Delete(str + "\\bcImg\\" + BuPic03);
                                }
                                inf.MoveTo(str + "\\bcImg\\" + BuPic03);
                            }
                            else if (i == 3)
                            {
                                BuPic04 = xNode[i].InnerText;
                                string pName = fullPath + BuPic04;
                                FileInfo inf = new FileInfo(pName);
                                if (File.Exists(str + "\\bcImg\\" + BuPic04))
                                {
                                    File.Delete(str + "\\bcImg\\" + BuPic04);
                                }
                                inf.MoveTo(str + "\\bcImg\\" + BuPic04);
                            }
                      }
                    }
                    jkInfoDao jkInfoDao = new jkInfoDao();
                    DataTable data = jkInfoDao.selectjkInfoBybarcode(id);
                    if (data != null && data.Rows.Count > 0)
                    {
                        string aichive_no = data.Rows[0]["aichive_no"].ToString();
                        string barcode = data.Rows[0]["bar_code"].ToString();
                        DataTable dtnum = jkInfoDao.queryChongfuBcData(aichive_no, barcode);
                        if (dtnum.Rows.Count > 0) { return; }
                        string issql = "insert into zkhw_tj_bc(id,aichive_no,id_number,bar_code,FubuResult,FubuBC,BuPic01,BuPic02,BuPic03,BuPic04,createtime) values(@id,@aichive_no,@id_number,@bar_code,@FubuResult,@FubuBC,@BuPic01,@BuPic02,@BuPic03,@BuPic04,@createtime)";
                        MySqlParameter[] args = new MySqlParameter[] {
                        new MySqlParameter("@id",Result.GetNewId()),
                        new MySqlParameter("@aichive_no", data.Rows[0]["aichive_no"].ToString()),
                        new MySqlParameter("@id_number", data.Rows[0]["id_number"].ToString()),
                        new MySqlParameter("@bar_code", data.Rows[0]["bar_code"].ToString()),
                        new MySqlParameter("@FubuResult", cs),
                        new MySqlParameter("@FubuBC", nam),
                        new MySqlParameter("@BuPic01", BuPic01),
                        new MySqlParameter("@BuPic02", BuPic02),
                        new MySqlParameter("@BuPic03", BuPic03),
                        new MySqlParameter("@BuPic04", BuPic04),
                        new MySqlParameter("@createtime", DateTime.Now)
                    };
                        int tup = 0;
                        if (!string.IsNullOrWhiteSpace(BuPic01))
                        {
                            tup = 1;
                        }
                        int run = DbHelperMySQL.ExecuteSql($"update physical_examination_record set ultrasound_abdomen='1',abdomenB_img='{tup}',other_b='1',otherb_img='{tup}' where aichive_no='{data.Rows[0]["aichive_no"].ToString()}'and bar_code= '{data.Rows[0]["bar_code"].ToString()}'");
                        int rue = DbHelperMySQL.ExecuteSql(issql, args);
                        string issqdgbc = "update zkhw_tj_bgdc set BChao='1' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
                        DbHelperMySQL.ExecuteSql(issqdgbc);
                    string filepath = e.FullPath.Substring(0, innum);
                    DeleteDir(filepath);
                    //插入数据库
                }
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message+ "\n"+ex.StackTrace);
                    // RegisterAoupTrackLog(string.Format("监听异常！异常信息：{0}", ex.Message));
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
            catch (Exception ex) // 异常处理
            {

            }
        }
        #endregion
    }
}
