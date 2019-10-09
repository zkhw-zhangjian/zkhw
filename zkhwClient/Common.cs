using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient
{
    public class PersonExport
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string RiQi { get; set; }
        public string ZhuangTai { get; set; }
        public string Memo { get; set; }
    }
    public class FileTimeInfo
    {
        public string FileName;  //文件名
        public DateTime FileCreateTime; //创建时间
    }
    public class Common
    {
        #region
        public static void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        public static void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    if (currentSize == 0) currentSize = 9;
                    con.Font = new System.Drawing.Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }
        #endregion

        public static int m_nWindwMetricsY = 900;    //分辨率
        public static string _deviceModel = "";  //设备型号 包括生化、血球
        public static void txtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                TextBox b = (TextBox)sender;
                int kc = (int)e.KeyChar;
                if ((kc < 48 || kc > 57) && kc != 8 && kc != 46)
                    e.Handled = true;
                if (kc == 46)                       //小数点
                {
                    if (b.Text.Length <= 0)
                        e.Handled = true;           //小数点不能在第一位
                    else
                    {
                        float f;
                        float oldf;
                        bool b1 = false, b2 = false;
                        b1 = float.TryParse(b.Text, out oldf);
                        b2 = float.TryParse(b.Text + e.KeyChar.ToString(), out f);
                        if (b2 == false)
                        {
                            if (b1 == true)
                                e.Handled = true;
                            else
                                e.Handled = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        /// <summary>
        /// 判断是不是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public static string GetCreateTime(string s)
        {
            if(s=="" || s==null)
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                return DateTime.Parse(s).ToString("yyyy-MM-dd");
            }
        }
        public static List<FileTimeInfo> GetLatestFileTimeInfo(string dir, string filename)
        {
            List<FileTimeInfo> list = new List<FileTimeInfo>();
            DirectoryInfo d = new DirectoryInfo(dir);
            DateTime dt = DateTime.Parse("2001-1-1"); 
            foreach (FileInfo fi in d.GetFiles())
            {
                string fname=Path.GetFileNameWithoutExtension(fi.FullName);
                string[] a = fname.Split('-');
                string _namefile = "";
                bool isret = false;
                if(a.Length<=1)
                {
                    _namefile = fname;
                    if(fname.IndexOf(filename) > -1)
                    {
                        isret = true;
                    }
                }
                else
                {
                    _namefile = a[1].ToString();
                    if(_namefile==filename)
                    {
                        isret = true;
                    }
                }
                if (isret==true)
                {
                    if(fi.LastAccessTime>dt)
                    {
                        list.Clear();
                        dt = fi.LastAccessTime;
                        list.Add(new FileTimeInfo()
                        {
                            FileName = fi.FullName,
                            FileCreateTime = fi.LastAccessTime
                        });
                    } 
                } 
            }
            return list; 
        }
        
        public static DataTable GetNationDataTable(int itype)
        {
            DataTable dtno = new DataTable();
            dtno.Columns.Add("id", Type.GetType("System.String"));
            dtno.Columns.Add("name", Type.GetType("System.String"));
            DataRow newRow;
            if(itype==0)
            {
                newRow = dtno.NewRow();
                newRow["id"] = "1";
                newRow["name"] = "汉族";
                dtno.Rows.Add(newRow);
            } 
            newRow = dtno.NewRow();
            newRow["id"] = "02";
            newRow["name"] = "蒙古族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "03";
            newRow["name"] = "回族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "04";
            newRow["name"] = "藏族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "05";
            newRow["name"] = "维吾尔族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "06";
            newRow["name"] = "苗族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "07";
            newRow["name"] = "彝族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "08";
            newRow["name"] = "壮族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "09";
            newRow["name"] = "布依族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "10";
            newRow["name"] = "朝鲜族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "11";
            newRow["name"] = "满族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "12";
            newRow["name"] = "侗族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "13";
            newRow["name"] = "瑶族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "14";
            newRow["name"] = "白族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "15";
            newRow["name"] = "土家族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "16";
            newRow["name"] = "哈尼族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "17";
            newRow["name"] = "哈萨克族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "18";
            newRow["name"] = "傣族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "19";
            newRow["name"] = "黎族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "20";
            newRow["name"] = "傈僳族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "21";
            newRow["name"] = "佤族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "22";
            newRow["name"] = "畲族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "23";
            newRow["name"] = "高山族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "24";
            newRow["name"] = "拉祜族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "25";
            newRow["name"] = "水族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "26";
            newRow["name"] = "东乡族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "27";
            newRow["name"] = "纳西族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "28";
            newRow["name"] = "景颇族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "29";
            newRow["name"] = "柯尔克孜族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "30";
            newRow["name"] = "土族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "31";
            newRow["name"] = "达斡尔族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "32";
            newRow["name"] = "仫佬族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "33";
            newRow["name"] = "羌族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "34";
            newRow["name"] = "布朗族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "35";
            newRow["name"] = "撒拉族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "36";
            newRow["name"] = "毛南族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "37";
            newRow["name"] = "仡佬族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "38";
            newRow["name"] = "锡伯族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "39";
            newRow["name"] = "阿昌族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "40";
            newRow["name"] = "普米族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "41";
            newRow["name"] = "塔吉克族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "42";
            newRow["name"] = "怒族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "43";
            newRow["name"] = "乌兹别克族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "44";
            newRow["name"] = "俄罗斯族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "45";
            newRow["name"] = "鄂温克族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "46";
            newRow["name"] = "德昂族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "47";
            newRow["name"] = "保安族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "48";
            newRow["name"] = "裕固族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "49";
            newRow["name"] = "京族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "50";
            newRow["name"] = "塔塔尔族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "51";
            newRow["name"] = "独龙族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "52";
            newRow["name"] = "鄂伦春族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "53";
            newRow["name"] = "赫哲族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "54";
            newRow["name"] = "门巴族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "55";
            newRow["name"] = "珞巴族";
            dtno.Rows.Add(newRow);
            newRow = dtno.NewRow();
            newRow["id"] = "56";
            newRow["name"] = "基诺族";
            dtno.Rows.Add(newRow);
            return dtno;
        }

        public static int JudgeValueForSh(DataTable dttv, shenghuaBean sh)
        {
            int flag = 1;
            if (rangeJudgeForSHInfo.dttv != null)
            {
                rangeJudgeForSHInfo.dttv.Clear();
            }
            rangeJudgeForSHInfo.dttv = dttv.Copy();
            flag = rangeJudgeForSHInfo.GetResultSh("ALT", sh.ALT);
            if (flag <= 2)
            {
               int  a = rangeJudgeForSHInfo.GetResultSh("AST", sh.AST);
                if(a>flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("TBIL", sh.TBIL);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("DBIL", sh.DBIL);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("CREA", sh.Crea);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("UREA", sh.UREA);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("GLU", sh.GLU);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("TG", sh.TG);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("CHO", sh.CHO);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("HDLC", sh.HDL_C);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForSHInfo.GetResultSh("LDLC", sh.LDL_C);
                if (a > flag)
                {
                    flag = a;
                }
            }
            return flag;
        }

        public static int JudgeValueForXCG(DataTable dttv, xuechangguiBean xcg)
        {
            int flag = 1;
            if (rangeJudgeForXCGInfo.dttv != null)
            {
                rangeJudgeForXCGInfo.dttv.Clear();
            }
            rangeJudgeForXCGInfo.dttv = dttv.Copy();
            flag = rangeJudgeForXCGInfo.GetResultXCG("WBC", xcg.WBC);
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("RBC", xcg.RBC);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("PCT", xcg.PCT);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("PLT", xcg.PLT);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("HGB", xcg.HGB);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("HCT", xcg.HCT);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("MCV", xcg.MCV);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("MCH", xcg.MCH);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("MCHC", xcg.MCHC);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("RDWCV", xcg.RDW_CV);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("RDWSD", xcg.RDW_SD);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("NEUT", xcg.NEUT);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("NEUTP", xcg.NEUTP);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("LYM", xcg.LYM);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("LYMP", xcg.LYMP);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("MPV", xcg.MPV);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("PDW", xcg.PDW);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("MXD", xcg.MXD);
                if (a > flag)
                {
                    flag = a;
                }
            }
            if (flag <= 2)
            {
                int a = rangeJudgeForXCGInfo.GetResultXCG("MXDP", xcg.MXDP);
                if (a > flag)
                {
                    flag = a;
                }
            }
            return flag;
        }

        public static void SetComboBoxInfo(ComboBox cb,DataTable dt,string  DisplayMember,string ValueMember)
        {
            DataRow dr = dt.NewRow();
            dr[0] = null;
            dr[1] = "--请选择--";
            dt.Rows.InsertAt(dr, 0);

            cb.DataSource = dt;
            cb.DisplayMember = DisplayMember;//显示给用户的数据集表项
            cb.ValueMember = ValueMember;//操作时获取的值 
        }

        public static void SetComboBoxInfo(ComboBox cb, DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                List<ComboBoxData> ts = Result.ToDataList<ComboBoxData>(dt);
                Result.Bind(cb, ts, "Name", "ID", "--请选择--");
            } 
        }

        public static void SetComboBoxSelectIndex(ComboBox cb, string code)
        {
            if (code == null || code == "") return;
            for (int i = 0; i < cb.Items.Count; i++)
            {
                ComboBoxData obj = (ComboBoxData)cb.Items[i];
                if (obj.ID == code)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    public class ControlCircular
    { 
        public static void Draw(System.Drawing.Rectangle rectangle, Graphics g, int _radius, bool cusp, Color begin_color, Color end_color)
        {
            if (rectangle.Width == 0 || rectangle.Height == 0) return;
            int span = 2;
            //抗锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //渐变填充
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush(rectangle, begin_color, end_color, LinearGradientMode.Vertical);
            //画尖角
            if (cusp)
            {
                span = 10;
                PointF p1 = new PointF(rectangle.Width - 12, rectangle.Y + 10);
                PointF p2 = new PointF(rectangle.Width - 12, rectangle.Y + 30);
                PointF p3 = new PointF(rectangle.Width, rectangle.Y + 20);
                PointF[] ptsArray = { p1, p2, p3 };
                g.FillPolygon(myLinearGradientBrush, ptsArray);
            }
            //填充
            g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height - 1, _radius));
        }

        public static GraphicsPath DrawRoundRect(int x, int y, int width, int height, int radius)
        {
            //四边圆角
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, radius, radius, 180, 90);
            gp.AddArc(width - radius, y, radius, radius, 270, 90);
            gp.AddArc(width - radius, height - radius, radius, radius, 0, 90);
            gp.AddArc(x, height - radius, radius, radius, 90, 90);
            gp.CloseAllFigures();
            return gp;
        }

        public static void DrawFont(PaintEventArgs e, string wenzi, System.Drawing.Font font, Brush bush)
        {
            if(font==null)
            {
                font = new System.Drawing.Font("微软雅黑", 12F);
            }
            if (bush == null)
            {
                bush = Brushes.White;
            }
            Graphics g = e.Graphics;  
            SizeF size = g.MeasureString(wenzi, font);
            int posX = (e.ClipRectangle.Width - Convert.ToInt16(size.Width)) / 2;
            int posY = (e.ClipRectangle.Height - Convert.ToInt16(size.Height)) / 2;
            g.DrawString(wenzi, font, bush, new PointF(posX, posY));
        }
    }
}
