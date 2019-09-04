/*
 * 此类为判断生化阈值范围
 * 2019-8-8 li
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class rangeJudgeForSHInfo
    {
        public static DataTable dttv = null;  //阈值表


        public static void GetRangeValue(string typevalue,out double warning_min,out double warning_max,out double threshold_min,out double threshold_max)
        {
            warning_min = 0;
            warning_max = 0;
            threshold_min = 0;
            threshold_max = 0;
            DataRow[] dr = dttv.Select("type='"+ typevalue + "'");
            if(dr !=null)
            {
                warning_min = double.Parse(dr[0]["warning_min"].ToString());
                warning_max = double.Parse(dr[0]["warning_max"].ToString());

                threshold_min = double.Parse(dr[0]["threshold_min"].ToString());
                threshold_max = double.Parse(dr[0]["threshold_max"].ToString());
            }
        }
        public static int GetResultSh(string typeValue,string strvalue)
        {
            int _result = 1;
            if (strvalue != "" && strvalue != "*")
            {
                double dblvalue = 0;
                bool a = double.TryParse(strvalue, out dblvalue);
                if(a==true)
                {
                    double warning_min = 0;
                    double warning_max = 0;
                    double threshold_min = 0;
                    double threshold_max = 0;
                    GetRangeValue(typeValue, out warning_min, out warning_max, out threshold_min, out threshold_max);
                    if (dblvalue > warning_max || dblvalue < warning_min)
                    {
                        _result = 2;
                    }
                    if (dblvalue > threshold_max || dblvalue < threshold_min)
                    {
                        _result = 3;
                    }
                } 
            }
            return _result;
        }
         
        public static string GetItemResultForValue(string typeValue, string strvalue)
        {
            string _result = "";
            if (strvalue != "" && strvalue != "*")
            {
                double dblvalue = 0;
                bool a = double.TryParse(strvalue, out dblvalue);
                if (a == true)
                { 
                    DataRow[] dr = dttv.Select("type='" + typeValue + "'");
                    if (dr != null)
                    {
                        double warning_min = double.Parse(dr[0]["warning_min"].ToString());
                        double warning_max = double.Parse(dr[0]["warning_max"].ToString());
                        string unit = dr[0]["unit"].ToString();
                        string chinaName = dr[0]["chinaName"].ToString();
                        if (dblvalue< warning_min)
                        {
                            _result = chinaName + "偏低：" + dblvalue.ToString() + " "+unit;
                        }
                        else if(dblvalue> warning_max)
                        {
                            _result = chinaName + "偏高：" + dblvalue.ToString() + " " + unit;
                        }
                    } 
                }
            }
            return _result;
        }
    }
}
