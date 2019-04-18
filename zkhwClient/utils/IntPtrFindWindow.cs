using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.utils
{
    class IntPtrFindWindow
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        private static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        public static void intptrwindows()
        {
            const uint BM_CLICK = 0xF5; //鼠标点击的消息，对于各种消息的数值，大家还是得去API手册

            IntPtr hwndPhoto = FindWindow(null, "iMAC FTP - JN120.05"); //查找拍照程序的句柄【任务管理器中的应用程序名称】

            if (hwndPhoto != IntPtr.Zero)
            {
                IntPtr hwndThree = FindWindowEx(hwndPhoto, 0, null, "启动服务"); //获取按钮快照的句柄

                SetForegroundWindow(hwndPhoto);    //将UcDemo程序设为当前活动窗口

                System.Threading.Thread.Sleep(100);   //暂停500毫秒

                SendMessage(hwndThree, BM_CLICK, 0, 0);//模拟点击拍照按钮
            }
            else
            {
                MessageBox.Show("没有启动 Demo");
            }
        }
        public static void showwindow(IntPtr intptr)
        {
            ShowWindow(intptr, 0);
        }
   }
}
