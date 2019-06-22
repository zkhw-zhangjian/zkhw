using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace zkhwClient
{
    public partial class LoaderForm : Form
    {
        public bool isclose = false;
        public string mystr = "正在导出第1份";
        public LoaderForm()
        {
            InitializeComponent();
        }

        private void LoaderForm_Load(object sender, EventArgs e)
        {
            isclose = true;
            Thread fThread = new Thread(new ThreadStart(DisplayLabel));
            fThread.Start();
        }
        private void DisplayLabel()
        {
            while (isclose)
            {
                try
                {
                    this.Invoke((EventHandler)delegate { this.label1.Text = mystr; });
                }
                catch
                { }
                Thread.Sleep(100);
            }
        }
        public void closeOrder()
        {
            if (this.InvokeRequired)
            {
                //这里利用委托进行窗体的操作，避免跨线程调用时抛异常，后面给出具体定义
                CONSTANTDEFINE.SetUISomeInfo UIinfo = new CONSTANTDEFINE.SetUISomeInfo(new Action(() =>
                {
                    while (!this.IsHandleCreated)
                    {
                        ;
                    }
                    if (this.IsDisposed)
                        return;
                    if (!this.IsDisposed)
                    {
                        this.Dispose();
                    }

                }));
                this.Invoke(UIinfo);
            }
            else
            {
                if (this.IsDisposed)
                    return;
                if (!this.IsDisposed)
                {
                    this.Dispose();
                }
            }
        }

        private void LoaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isclose = false;
            if (!this.IsDisposed)
            {
                this.Dispose(true);
            }
        }
    }
}
