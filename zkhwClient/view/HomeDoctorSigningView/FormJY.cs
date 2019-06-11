using System;
using System.Windows.Forms;

namespace zkhwClient.view.setting
{
    public partial class FormJY : Form
    {
        public string url = "";
        public FormJY()
        {
            InitializeComponent();
        }

        private void FormJY_Load(object sender, EventArgs e)
        {
            string newurl = url.Replace(" ", "");
            this.webBrowser1.Navigate(newurl);
            //this.webBrowser1.Navigate("www.baidu.com");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser web = (WebBrowser)sender;
            HtmlElementCollection ElementCollection = web.Document.GetElementsByTagName("su");
            HtmlElement ElementCollection1 = web.Document.GetElementById("su");
        }
    }
}
