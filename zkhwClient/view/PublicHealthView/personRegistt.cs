using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient
{
    public partial class personRegistt : Form
    {
        public personRegistt()
        {
            InitializeComponent();
        }

        private void personRegistt_Load(object sender, EventArgs e)
        {
            label46.Text = "";
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            //ControlCircular.Draw(e.ClipRectangle, e.Graphics, 18, false, Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255));
            //base.OnPaint(e);
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            Button bt = (Button)sender;
            string stag = bt.Tag.ToString();
            string wenzi = "";
            int starti = 20;
            Color color = Color.FromArgb(81, 95, 154);
            switch (stag)
            {
                case "0":    //读卡
                    color = Color.FromArgb(77, 177, 81);
                    wenzi = "读  卡";
                    break;
                case "1":    //拍照 
                    wenzi = "拍  照";
                    break;
                case "2":    //打印 
                    wenzi = "打  印";
                    break;
                case "3":    //补打条码
                    wenzi = "补打条码";
                    starti = 10;
                    break;
            }
            ControlCircular.Draw(e.ClipRectangle, e.Graphics, 6, false, color, color);
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawString(wenzi, new Font("宋体", 9, FontStyle.Regular), new SolidBrush(Color.White), new PointF(starti, 8));
        }
    }
}
