using System;
using System.Collections.Generic;
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
    }
}
