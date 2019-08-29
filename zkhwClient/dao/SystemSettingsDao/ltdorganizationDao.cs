using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    public class ltdorganizationDao
    {
        /// <summary>
        /// 省
        /// </summary>
        /// <returns></returns>
        public static DataTable GetShengInfo()
        {
            string sql = "select distinct l.province_code as ID,c.name as Name from ltd_organization l inner join code_area_config c on l.province_code=c.code order by l.province_code asc";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        /// <summary>
        /// 市
        /// </summary>
        /// <param name="shcode"></param>
        /// <returns></returns>
        public static DataTable GetCityInfo(string shcode)
        {
            String sql = "select distinct l.city_code as ID,c.name as Name from ltd_organization l inner join code_area_config c on l.city_code=c.code   where l.province_code = '" + shcode + "' order by l.city_code asc";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        /// <summary>
        /// 县
        /// </summary>
        /// <param name="citycode"></param>
        /// <returns></returns>
        public static DataTable GetCountyInfo(string citycode)
        {
            String sql = "select distinct l.county_code as ID,c.name as Name from ltd_organization l inner join code_area_config c on l.county_code=c.code where l.city_code = '" + citycode + "' order by l.county_code asc";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        /// <summary>
        /// 镇
        /// </summary>
        /// <param name="countycode"></param>
        /// <returns></returns>
        public static DataTable GetTownsInfo(string countycode)
        {
            String sql = "select distinct l.towns_code as ID,c.name as Name from ltd_organization l inner join code_area_config c on l.towns_code=c.code   where l.county_code = '" + countycode + "' order by l.towns_code asc";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        /// <summary>
        /// 村
        /// </summary>
        /// <param name="townscode"></param>
        /// <returns></returns>
        public static DataTable GetVillageInfo(string townscode)
        {
            String sql = "select distinct l.village_code as ID,c.name as Name from ltd_organization l inner join code_area_config c on l.village_code=c.code   where l.towns_code = '" + townscode + "' order by l.village_code asc";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
