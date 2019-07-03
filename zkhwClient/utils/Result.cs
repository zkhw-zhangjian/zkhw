using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace zkhwClient.dao
{
    /// <summary>
    /// 系统公共使用部分
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// 获取字符串类型的主键
        /// </summary>
        /// <returns></returns>
        public static string GetNewId()
        {
            string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            string guid = Guid.NewGuid().ToString().Replace("-", "");

            id += guid.Substring(0, 15);
            return id;
        }

        /// <summary>
        /// 为ComboBox绑定数据源并提供下拉提示
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <param name="list">数据源</param>
        /// <param name="displayMember">显示字段</param>
        /// <param name="valueMember">隐式字段</param>
        /// <param name="displayText">下拉提示文字</param>
        public static void Bind<T>(this ComboBox combox, IList<T> list, string displayMember, string valueMember, string displayText)
        {
            AddItem(list, displayMember, displayText);
            combox.DisplayMember = displayMember;
            combox.ValueMember = valueMember;
            combox.DataSource = list;
            combox.DisplayMember = displayMember;
            if (!string.IsNullOrEmpty(valueMember))
                combox.ValueMember = valueMember;
        }
        private static void AddItem<T>(IList<T> list, string displayMember, string displayText)
        {
            Object _obj = Activator.CreateInstance<T>();
            Type _type = _obj.GetType();
            if (!string.IsNullOrEmpty(displayMember))
            {
                PropertyInfo _displayProperty = _type.GetProperty(displayMember);
                _displayProperty.SetValue(_obj, displayText, null);
            }
            list.Insert(0, (T)_obj);
        }

        /// <summary>
        /// DataTable转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToDataList<T>(this DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                object v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    v = Convert.ChangeType(item[i], Nullable.GetUnderlyingType(info.PropertyType));
                                }
                                else
                                {
                                    v = Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        /// <summary>
        /// 获取少数民族的名称  根据id
        /// </summary>
        /// <returns></returns>
        public static string GetNationId(string id)
        {
            DataTable dtno = new DataTable();
            dtno.Columns.Add("id", Type.GetType("System.String"));
            dtno.Columns.Add("name", Type.GetType("System.String"));
            DataRow newRow;
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
            DataRow [] dr= dtno.Select("id='"+id+"'");
            return dr[0]["name"].ToString();
        }

        public static bool Validate(string str, string regexStr)
        {
            Regex regex = new Regex(regexStr);
            Match match = regex.Match(str);
            if (match.Success)
                return true;
            else
                return false;
        }
    }

    public class ComboBoxData
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string BarCode { get; set; }
    }
}
