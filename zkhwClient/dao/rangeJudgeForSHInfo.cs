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


        public static void GetRangeValue(string typevalue,out double warning_min,out double warning_max,out double threshold_min,out double threshold_max,out bool flag)
        {
            flag = false;
            warning_min = 0;
            warning_max = 0;
            threshold_min = 0;
            threshold_max = 0;
            try
            {
                DataRow[] dr = dttv.Select("type='" + typevalue + "'");
                if (dr != null)
                {
                    if (dr.Length == 0)
                    {
                        if (typevalue == "LDLC")
                        {
                            typevalue = "LDL";
                            dr = dttv.Select("type='" + typevalue + "'");
                        }

                        if (typevalue == "HDLC")
                        {
                            typevalue = "HDL";
                            dr = dttv.Select("type='" + typevalue + "'");
                        }
                    }

                    warning_min = double.Parse(dr[0]["warning_min"].ToString());
                    warning_max = double.Parse(dr[0]["warning_max"].ToString());

                    threshold_min = double.Parse(dr[0]["threshold_min"].ToString());
                    threshold_max = double.Parse(dr[0]["threshold_max"].ToString());
                    flag = true;
                }
            }
            catch
            {
                //有可能这个 阈值表 没有对应的项
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
                    bool flag = false;
                    GetRangeValue(typeValue, out warning_min, out warning_max, out threshold_min, out threshold_max,out flag);
                    if(flag==true)
                    {
                        if (dblvalue > warning_max || dblvalue < warning_min)
                        {
                            _result = 2;
                        }
                        if (dblvalue > threshold_max || dblvalue < threshold_min)
                        {
                            _result = 3;
                        }
                    }
                    else
                    {
                        _result = 1;   //不参与或者没有这项
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
                        if(dr.Length==0)
                        {
                            if(typeValue== "LDLC")
                            {
                                typeValue = "LDL";
                                dr = dttv.Select("type='" + typeValue + "'");
                            }

                            if (typeValue == "HDLC")
                            {
                                typeValue = "HDL";
                                dr = dttv.Select("type='" + typeValue + "'");
                            }

                            if (dr.Length == 0) return "";
                        }
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
