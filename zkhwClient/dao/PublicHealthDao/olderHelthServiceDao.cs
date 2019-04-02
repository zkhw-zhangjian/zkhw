using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class olderHelthServiceDao
    {
        public bool deleteOlderHelthService(string id)
        {
            int rt = 0;
            string sql = "delete from elderly_selfcare_estimate  where follow_id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public DataTable queryOlderHelthService(string pCa, string time1, string time2)
        {
            DataSet ds = new DataSet();
            string sql = "select id,name,id_number,total_score,judgement_result,test_date,test_doctor from elderly_selfcare_estimate where test_date >= '" + time1 + "' and test_date <= '" + time2 + "'";
            if (pCa != "") { sql += " and (name like '%" + pCa + "%'  or id_number like '%" + pCa + "%'  or aichive_no like '%" + pCa + "%')"; }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
