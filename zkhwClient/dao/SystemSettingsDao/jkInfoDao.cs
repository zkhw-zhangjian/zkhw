using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using zkhwClient.bean;

namespace zkhwClient.dao
{
    class jkInfoDao
    {
        //添加体检登记记录做统计使用
        public bool addJkInfo(grjdxxBean grjdxx)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");                                                                 
            //String sql = "insert into zkhw_tj_bgdc (ID,aichive_no,id_number,bar_code,name,sex,birthday,healthchecktime,createtime) values ('" + id + "', '" + grjdxx.archive_no + "', '" + grjdxx.Cardcode + "', '" + grjdxx.b + "', '" + grjdxx.name + "', '" + grjdxx.Sex + "', '" + grjdxx.Birthday + "', '" + time + "', '" + time + "')";
            //rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //登记界面右侧 查询统计男女各多少人功能
        public DataTable selectjktjInfo(string areacode,string time)
        {
            DataSet ds = new DataSet();
            string sql = "select * from zkhw_tj_jk k where k.village_code='" + areacode + "' and k.createtime >= '" + time + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

    }
}
