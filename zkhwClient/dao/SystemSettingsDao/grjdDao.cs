using System;
using System.Data;
using System.Windows.Forms;

namespace zkhwClient.dao
{
    class grjdDao
    {
        //根据身份证号判断是否已存在居民档案信息
        public DataTable judgeRepeat(string cardcode)
        {
            DataSet ds = new DataSet();
            string sql = "select name,sex,birthday,id_number,card_pic,archive_no from resident_base_info a where a.id_number = '" + cardcode + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //添加居民信息档案
        public bool addgrjdInfo(bean.grjdxxBean grjd)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = "0";
            String sql = "insert into resident_base_info (id,archive_no,name,sex,birthday,id_number,card_pic,register_address,photo_code,create_time,create_name,aichive_org,is_synchro) values ('" + id + "','" + grjd.archive_no + "','" + grjd.name + "','" + grjd.Sex + "', '" + grjd.Birthday + "', '" + grjd.Cardcode + "', '" + grjd.CardPic + "', '" + grjd.Zhuzhi + "', '" + grjd.photo_code + "', '" + time + "', '" + grjd.create_name + "', '" + grjd.aichive_org + "', '" + status + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //打印条码并保存个人基础信息
        public bool addJkInfo(bean.jkBean jkbar)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = "0";
            String sql = "insert into zkhw_tj_jk (ID,aichive_no,id_number,bar_code,batch_no,Pic1,Pic2,Xzdm,Xianzhudi,XzjdName,CjwhName,JddwName,JdrName,ZrysName,Caozuoyuan,SjName,Carcode,createtime,upload_status,duns) values ('" + id + "','" + jkbar.aichive_no + "','" + jkbar.id_number + "', '" + jkbar.bar_code + "', '" + jkbar.batch_no + "', '" + jkbar.Pic1 + "', '" + jkbar.Pic2 + "', '" + jkbar.Xzdm + "', '" + jkbar.Xianzhudi + "', '" + jkbar.XzjdName + "', '" + jkbar.CjwhName + "', '" + jkbar.JddwName + "', '" + jkbar.JdrName + "', '" + jkbar.ZrysName + "', '" + jkbar.Caozuoyuan + "', '" + jkbar.SjName + "', '" + jkbar.Carcode + "', '" + time + "', '" + status + "', '" + jkbar.duns + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }

        //保存体检统计信息
        public bool addBgdcInfo(bean.grjdxxBean grjbxx,string barcode,string archive_no)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String sql = "insert into zkhw_tj_bgdc (ID,aichive_no,id_number,bar_code,name,sex,birthday,healthchecktime,createtime) value('" + id + "','" + archive_no + "','" + grjbxx.Cardcode + "', '" + barcode + "', '" + grjbxx.name + "', '" + grjbxx.Sex + "', '" + grjbxx.Birthday + "', '" + time + "', '" + time + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //登记界面右侧 查询功能
        public DataTable registrationRecordInfo(string name)
        {
            DataSet ds = new DataSet();
            string sql = "select a.name,a.sex,a.id_number,a.archive_no,k.bar_code from resident_base_info a left join zkhw_tj_jk k on a.id_number=k.id_number and a.id_number like '%" + name + "%' or a.name like '%" + name + "%' or a.archive_no like '%" + name + "%' GROUP BY a.id_number";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
