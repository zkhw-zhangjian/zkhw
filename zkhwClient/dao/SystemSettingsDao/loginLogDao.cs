using System;
using System.Data;

namespace zkhwClient.dao
{
    class loginLogDao
    {
        public DataTable checkLog(string time1, string time2,string flag)
        {
            DataSet ds = new DataSet();
            string sql = "select createtime,type,userName,eventInfo from zkhw_log_syslog a where a.createtime > '" + time1 + "' and  a.createtime <  '" + time2 + "' and  a.type =  '" + flag + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public bool addCheckLog(bean.loginLogBean lb)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string type = "1";
             String sql = "insert into zkhw_log_syslog (id,userName,type,createtime,eventInfo) values ('" + id + "','" + lb.name + "','" + type + "', '" + lb.createTime + "', '" + lb.eventInfo + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
  }
}
