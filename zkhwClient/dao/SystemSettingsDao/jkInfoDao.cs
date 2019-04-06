using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.bean;

namespace zkhwClient.dao
{
    class jkInfoDao
    {
        //根据条码号获取对应的身份证号和档案编号（取最新的一条记录）
        public DataTable selectjkInfoBybarcode(string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select k.aichive_no,k.id_number,k.bar_code from zkhw_tj_jk k where k.bar_code='" + barcode + "' order by k.createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //登记界面右侧 查询统计男女各多少人功能
        public DataTable selectjktjInfo(string areacode,string time)
        {
            DataSet ds = new DataSet();
            string sql = "select * from zkhw_tj_jk k where k.village_code='" + areacode + "' and k.createtime >= '" + time + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //查询体检进度
        public DataTable querytjjd(string time1, string time2,string xcuncode, string jmxx)
        {
            DataSet ds = new DataSet();
            string sql = "select healthchecktime,name,aichive_no,id_number,bar_code,BChao,XinDian,ShengHua,XueChangGui,NiaoChangGui,XueYa,Shengaotizhong from zkhw_tj_bgdc where createtime >= '" + time1 + "' and createtime <= '" + time2 + "'";
            if (xcuncode!=null&&!"".Equals(xcuncode)) {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (jmxx != null && !"".Equals(jmxx)) {
                sql += " and name like '%" + jmxx + "%' or aichive_no like '%" + jmxx + "%' or id_number like '%" + jmxx + "%'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
