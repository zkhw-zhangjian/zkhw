﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace zkhwClient.dao
{
    /// <summary>
    /// 监控心电图xml文件
    /// </summary>
    public class FSWControl
    {
        static FileSystemWatcher watcher = new FileSystemWatcher();
        static FileSystemWatcher watcherb = new FileSystemWatcher();
        /// <summary>
        /// 初始化监听
        /// </summary>
        /// <param name="StrWarcherPath">需要监听的目录</param>
        /// <param name="FilterType">需要监听的文件类型(筛选器字符串)</param>
        /// <param name="IsEnableRaising">是否启用监听</param>
        /// <param name="IsInclude">是否监听子目录</param>
        public static void WatcherStrat(string StrWarcherPath, string FilterType, bool IsEnableRaising, bool IsInclude)
        {
            //初始化监听
            watcher.BeginInit();
            //设置监听文件类型
            watcher.Filter = FilterType;
            //设置是否监听子目录
            watcher.IncludeSubdirectories = IsInclude;
            //设置是否启用监听?
            watcher.EnableRaisingEvents = IsEnableRaising;
            //设置需要监听的更改类型(如:文件或者文件夹的属性,文件或者文件夹的创建时间;NotifyFilters枚举的内容)
            watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            //设置监听的路径
            watcher.Path = StrWarcherPath;
            //注册创建文件或目录时的监听事件
            watcher.Created += new FileSystemEventHandler(watch_created);
            //注册当指定目录的文件或者目录发生改变的时候的监听事件
            //watcher.Changed += new FileSystemEventHandler(watch_created);
            //注册当删除目录的文件或者目录的时候的监听事件
            //watcher.Deleted += new FileSystemEventHandler(watch_deleted);
            //当指定目录的文件或者目录发生重命名的时候的监听事件
            //watcher.Renamed += new RenamedEventHandler(watch_renamed);
            //结束初始化
            watcher.EndInit();
        }

        /// <summary>
        /// 创建文件或者目录时的监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void watch_created(object sender, FileSystemEventArgs e)
        {
            //事件内容
            while (!IsFileReady(e.FullPath))
            {
                if (!File.Exists(e.FullPath))
                    return;
                Thread.Sleep(100);
            }
            Thread.Sleep(2000);
            FileWatcher.OnChangedForXinDianTu(sender, e);
        }

        /// <summary>
        /// 启动或者停止监听
        /// </summary>
        /// <param name="IsEnableRaising">True:启用监听,False:关闭监听</param>
        private void WatchStartOrSopt(bool IsEnableRaising)
        {
            watcher.EnableRaisingEvents = IsEnableRaising;
        }

        public static void WatcherStratBchao(string StrWarcherPath, string FilterType, bool IsEnableRaising, bool IsInclude)
        {
            //初始化监听
            watcherb.BeginInit();
            watcherb.InternalBufferSize = 5 * 1024 * 1024;
            //设置监听文件类型
            watcherb.Filter = FilterType;
            //设置是否监听子目录
            watcherb.IncludeSubdirectories = IsInclude;
            //设置是否启用监听?
            watcherb.EnableRaisingEvents = IsEnableRaising;
            //设置需要监听的更改类型(如:文件或者文件夹的属性,文件或者文件夹的创建时间;NotifyFilters枚举的内容)
            watcherb.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            //设置监听的路径
            watcherb.Path = StrWarcherPath;
            //注册创建文件或目录时的监听事件
            watcherb.Created += new FileSystemEventHandler(watch_createdbchao);
            watcherb.EndInit();
        }
        private static void watch_createdbchao(object sender, FileSystemEventArgs e)
        {
            ////事件内容
            //while (!IsFileReady(e.FullPath))
            //{
            //    if (!File.Exists(e.FullPath))
            //        return;
            //    Thread.Sleep(100);
            //}
            //FileWatcher.OnChangedForBChao(sender, e);
            Thread thread = new Thread(new ParameterizedThreadStart(FileWatcher.OnChangedForBChao1));
            thread.IsBackground = true;
            thread.Start(e.FullPath); 
        }
 
        static bool IsFileReady(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            FileStream fs = null;
            try
            {
                fs = fi.Open(FileMode.Open, FileAccess.ReadWrite,FileShare.None);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
         
    }
}
