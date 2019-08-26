using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
