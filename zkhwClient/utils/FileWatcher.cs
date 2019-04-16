using System;
using System.Collections.Generic;
using System.IO;

using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

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
        /// 监听
        /// </summary>
        ///  <remarks>创建人员(日期): ★刘腾飞★(100202 18:16)</remarks>
        public static void WatcheDirForAoup()
        {
            try
            {
                return;
                string watchPath = @"D:\xindiantu";//去掉文件夹的只读权限

                m_watcherAoup.Path = watchPath;
                m_watcherAoup.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime |
                    NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess |
                    NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
                // Only watch text files.

                m_watcherAoup.Filter = "*.xml";

                m_watcherAoup.Changed += new FileSystemEventHandler(OnChangedForAoup);
                m_watcherAoup.EnableRaisingEvents = true;
                //修改文件的时间，用于系统启动时监听一次修改
                File.SetLastWriteTime(watchPath, DateTime.Now);

            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// AOD 文件删除时出发
        /// </summary>
        ///  <remarks>创建人员(日期):★刘腾飞★(100603 23:34)</remarks>
        private static void OnDeleteForAod(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                try
                {
                    RegisterAodTrackLog("AOD 文件已经被 PP5000 读取，并成功删除，可以生成下一订单！");
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
        private static void OnChangedForAoup(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                List<string> orderIdList = new List<string>();
                try
                {
                    //1.由于客户机器首次读取时乱码，故先修改该文件后再读取内容
                    m_watcherAoup.EnableRaisingEvents = false;
                    try
                    {
                        File.SetLastWriteTime(e.FullPath, DateTime.Now);//修改txt文件的时间

                        XmlDocument doc = new XmlDocument();

                        doc.Normalize();
                        doc.Load("");
                        XElement edmx;
                        try
                        {

                            edmx = XElement.Load("".ToString()); //加载文件

                        }
                        catch
                        {

                            return;
                        }


                    }
                    catch
                    {
                        RegisterAoupTrackLog("文件被占用！正在请求重试 ... ");

                        //进程阻塞2秒钟
                        Thread.Sleep(2000);

                        File.SetLastWriteTime(e.FullPath, DateTime.Now);//修改txt文件的时间
                    }
                    m_watcherAoup.EnableRaisingEvents = true;

                    //插入数据库

                }
                catch (Exception ex)
                {


                    RegisterAoupTrackLog(string.Format("AOUP监听异常！异常信息：{0}", ex.Message));

                }
                finally
                {
                    //如果遇到异常关闭了监听，则重新打开监听
                    if (!m_watcherAoup.EnableRaisingEvents)
                        m_watcherAoup.EnableRaisingEvents = true;
                }
            }
        }


        #endregion
    }
    class OrderFileClass
    {
        #region 系统选项

        #region 私有字段

        private string m_txtName;
        private string m_directoryName;
        #endregion
        #region 属性

        public string TxtName
        {
            get
            {
                //if (string.IsNullOrEmpty(m_txtName))
                //{
                //    return null;
                //}
                return m_txtName;
            }
            set
            { m_txtName = value; }

            //get 
            //{
            //    string retRead = OprateFileClass.IniReadValue(startRun);
            //    if (string.IsNullOrEmpty(retRead))
            //    {
            //        return retRead;
            //    }
            //    return null; 
            //}
            //set { OprateFileClass.IniWriteValue(startRun, value); }
        }
        public string DirectoryName
        {
            get
            {
                //if (string.IsNullOrEmpty(m_directoryName))
                //{
                //    return null;
                //}
                return m_directoryName;
            }
            set
            { m_directoryName = value; }
        }

        #endregion
        #region 方法


        #endregion
        #endregion
    }
}
