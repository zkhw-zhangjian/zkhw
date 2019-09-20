using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace zkhwClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            if (IsRight() == false) return;
            bool ret;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                KillProc("ASNetWks");
                KillProc("httpceshi");
                Application.EnableVisualStyles();   //这两行实现   XP   可视风格   
                Application.DoEvents();             //这两行实现   XP   可视风格  
                Application.Run(new frmLoginn());
                //Application.Run(new personRegistt());


                //Application.Run(new zkhwClient.view.PublicHealthView.examinatReport());
                //Application.Run(new zkhwClient.view.HomeDoctorSigningView.teamMembers());
                //Application.Run(new frmLogin());//有数据库
                //Application.Run(new frmMain());//无数据库
                //Main   为你程序的主窗体，如果是控制台程序不用这句   
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show(null, "有一个和本程序相同的应用程序正在运行，请先关闭！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //提示信息，可以删除
                Application.Exit();//退出程序
            }
        }
        public static void KillProc(string strProcName)
        {
            try
            {
                //精确进程名  用GetProcessesByName
                foreach (Process p in Process.GetProcessesByName(strProcName))
                {
                    if (!p.CloseMainWindow())
                    {
                        p.Kill();
                    }
                }
            }
            catch
            {

            }
        }

        private static bool IsRight()
        {
            bool flag = false;
            string fpath = Application.StartupPath + "\\sysstem.ini";
            int iret = sysstem.JudgeExpirationDate(fpath);
            switch(iret)
            {
                case 0:
                    flag = true;
                    break;
                case 1: 
                    view.setting.frmEmpower frm = new view.setting.frmEmpower();
                    frm._EditType = 1;
                    if(frm.ShowDialog()==DialogResult.OK)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                    break;
                case 3:
                    MessageBox.Show("请校对电脑系统时间！");
                    break;
                default:
                    MessageBox.Show("系统文件丢失，请联系中科弘卫，24小时服务电话：4008150101");
                    break;
            } 
            return flag;
        }
        // <summary>
        // 线程异常处理
        // </summary>
        // <param name = "sender" ></ param >
        // < param name="e"></param>
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //在此处添加上你要写日志的方法
            //MessageBox.Show(e.Exception.Message);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "全局异常1：" + e.Exception.Message+"--"+e.Exception.StackTrace);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)

        {
            //此处写获取的日志 ;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "全局异常2：" + e.ExceptionObject.ToString());
            }
        }
    }
}
