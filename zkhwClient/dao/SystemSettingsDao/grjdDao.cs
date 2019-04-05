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
        //根据姓名和出生年月判断是否已存在居民档案信息
        public DataTable judgeRepeatBync(string name,string birthday)
        {
            DataSet ds = new DataSet();
            string sql = "select name,sex,birthday,id_number,card_pic,address,nation,archive_no from resident_base_info a where a.name = '" + name + "' and a.birthday = '" + birthday + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //添加居民信息档案
        public bool addgrjdInfo(bean.grjdxxBean grjd)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = "1";
            String sql = "insert into resident_base_info (id,archive_no,name,sex,birthday,age,id_number,card_pic,register_address,photo_code,create_time,create_name,aichive_org,is_synchro) values ('" + id + "','" + grjd.archive_no + "','" + grjd.name + "','" + grjd.Sex + "', '" + grjd.Birthday + "', '" + grjd.age + "', '" + grjd.Cardcode + "', '" + grjd.CardPic + "', '" + grjd.Zhuzhi + "', '" + grjd.photo_code + "', '" + time + "', '" + grjd.create_name + "', '" + grjd.aichive_org + "', '" + status + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //打印条码并保存个人基础信息
        public bool addJkInfo(bean.jkBean jkbar)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string uploadstatus = "0";
            String sql = "insert into zkhw_tj_jk (ID,aichive_no,id_number,bar_code,name,age,sex,Pic1,Pic2,village_code,address,Xzhuzhi,XzjdName,CjwhName,JddwName,JdrName,ZrysName,Caozuoyuan,SjName,Carcode,createtime,upload_status) values ('" + id + "','" + jkbar.aichive_no + "','" + jkbar.id_number + "', '" + jkbar.bar_code + "', '" + jkbar.name + "', '" + jkbar.age + "', '" + jkbar.sex + "', '" + jkbar.Pic1 + "', '" + jkbar.Pic2 + "', '" + jkbar.village_code + "', '" + jkbar.address + "', '" + jkbar.Xzhudi + "', '" + jkbar.XzjdName + "', '" + jkbar.CjwhName + "', '" + jkbar.JddwName + "', '" + jkbar.JdrName + "', '" + jkbar.ZrysName + "', '" + jkbar.Caozuoyuan + "', '" + jkbar.SjName + "', '" + jkbar.Carcode + "', '" + time + "', '" + uploadstatus + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }

        //保存体检统计信息
        public bool addBgdcInfo(bean.grjdxxBean grjbxx,string barcode,string archive_no,string xcuncode)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string time1 = DateTime.Now.ToString("yyyy-MM-dd");
            String sql = "insert into zkhw_tj_bgdc (ID,aichive_no,id_number,bar_code,name,sex,birthday,healthchecktime,createtime,area_duns) value('" + id + "','" + archive_no + "','" + grjbxx.Cardcode + "', '" + barcode + "', '" + grjbxx.name + "', '" + grjbxx.Sex + "', '" + grjbxx.Birthday + "', '" + time1 + "', '" + time + "', '" + xcuncode + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //登记界面右侧 查询功能
        public DataTable registrationRecordInfo(string name)
        {
            DataSet ds = new DataSet();
            string sql = "select a.name,a.sex,a.id_number,a.archive_no,k.bar_code from resident_base_info a left join zkhw_tj_jk k on a.id_number=k.id_number and a.id_number like '%" + name + "%' or a.name like '%" + name + "%' or a.archive_no like '%" + name + "%' ";//GROUP BY a.id_number";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        //获取要体检的总人数
        public DataTable residentNum(string areacode)
        {
            DataSet ds = new DataSet();
            string sql = "select count(1) from resident_base_info a where a.village_code = '" + areacode + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        //获取已登记的总人数
        public DataTable jkAllNum(string areacode)
        {
            DataSet ds = new DataSet();
            string sql = "select count(1) from zkhw_tj_jk a where a.village_code = '" + areacode + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
