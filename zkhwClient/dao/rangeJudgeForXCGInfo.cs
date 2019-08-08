/*
 * 此类为判断血常规阈值范围
 * 2019-8-8 li
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class rangeJudgeForXCGInfo
    {
        public static DataTable dttv = null;  //阈值表


        public static void GetRangeValue(string typevalue, out double warning_min, out double warning_max, out double threshold_min, out double threshold_max)
        {
            warning_min = 0;
            warning_max = 0;
            threshold_min = 0;
            threshold_max = 0;
            DataRow[] dralt = dttv.Select("type='" + typevalue + "'");
            if (dralt != null)
            {
                warning_min = double.Parse(dralt[0]["warning_min"].ToString());
                warning_max = double.Parse(dralt[0]["warning_max"].ToString());

                threshold_min = double.Parse(dralt[0]["threshold_min"].ToString());
                threshold_max = double.Parse(dralt[0]["threshold_max"].ToString());
            }
        }
       

        public static int GetResultXCG(string typeValue, string strvalue)
        {
            int _result = 1;
            if (strvalue != "" && strvalue != "*")
            {
                double dblvalue = double.Parse(strvalue);
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
            return _result;
        }
    }
}
