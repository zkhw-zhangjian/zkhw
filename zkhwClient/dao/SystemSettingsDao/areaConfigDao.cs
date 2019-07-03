using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class areaConfigDao
    {
        public DataTable shengInfo()
        {
            string code = "-1";
            String sql = "select code,name from code_area_config where parent_code = '" + code + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable shiInfo(string shengcode)
        {
            String sql = "select code,name from code_area_config where parent_code = '" + shengcode + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }


        public DataTable quxianInfo(string shicode)
        {
            String sql = "select code,name from code_area_config where parent_code = '" + shicode + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable zhenInfo(string qxcode)
        {
            String sql = "select code,name from code_area_config where parent_code = '" + qxcode + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable cunInfo(string xzcode)
        {
            String sql = "select code,name from code_area_config where parent_code = '" + xzcode + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //根据区域编号获取区域全名称
        public DataTable selectAreaBycode(string code)
        {
            DataSet ds = new DataSet();
            String sql = "select full_name from code_area_config where code='" + code + "' ";
            ds = DbHelperMySQL.Query(sql);
            if (ds.Tables.Count == 0) return null;
            return ds.Tables[0];
        }
    }
}
