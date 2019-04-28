using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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
            bool ret;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                Application.EnableVisualStyles();   //这两行实现   XP   可视风格   
                Application.DoEvents();             //这两行实现   XP   可视风格
                                                    Application.Run(new frmLogin());
                                                    //Application.Run(new zkhwClient.view.PublicHealthView.examinatReport());
                                                    //Application.Run(new zkhwClient.view.PublicHealthView.addtcmHealthServices());
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
    }
}
