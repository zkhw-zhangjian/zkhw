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
            string sql = "select name,sex,birthday,id_number,card_pic,archive_no,doctor_id from resident_base_info a where a.id_number = '" + cardcode + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //根据姓名和出生年月判断是否已存在居民档案信息
        public DataTable judgeRepeatBync(string name, string birthday)
        {
            DataSet ds = new DataSet();
            string sql = "select name,sex,birthday,id_number,card_pic,address,nation,archive_no,doctor_id from resident_base_info a where a.name = '" + name + "' and a.birthday = '" + birthday + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //添加居民信息档案
        public bool addgrjdInfo(bean.grjdxxBean grjd)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = "1";
            String sql = "insert into resident_base_info (id,archive_no,name,sex,birthday,age,id_number,card_pic,address,residence_address,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,photo_code,status,create_user,create_time,create_name,aichive_org,doctor_id,doctor_name,create_archives_name,create_org,create_org_name,upload_status) values ('" +
                id + "','" + grjd.archive_no + "','" + grjd.name + "','" + grjd.Sex + "', '" + grjd.Birthday + "', '" + grjd.age + "', '" + grjd.Cardcode + "', '" + grjd.CardPic + "', '" + grjd.Zhuzhi + "', '" + grjd.residence_address + "', '" + grjd.province_code + "', '" + grjd.province_name + "', '" + grjd.city_code + "', '" + grjd.city_name + "', '" + grjd.county_code + "', '" + grjd.county_name + "', '" + grjd.towns_code + "', '" + grjd.towns_name + "', '" + grjd.village_code + "', '" + grjd.village_name + "', '" + grjd.photo_code + "', '" + status + "', '" + frmLogin.userCode + "', '"
                + time + "', '" + frmLogin.name + "', '" + grjd.aichive_org + "', '" + grjd.doctor_id + "', '" + grjd.doctor_name + "', '" + grjd.create_archives_name + "', '" + frmLogin.organCode + "', '" + frmLogin.organName + "','0')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //添加居民健康体检表信息
        public bool addPhysicalExaminationInfo(bean.grjdxxBean grjd, string barcode)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string ctime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String sql = "insert into physical_examination_record (id,aichive_no,name,id_number,bar_code,check_date,doctor_name,create_user,create_name,create_org,create_org_name,create_time,upload_status,dutydoctor) values ('" + id + "','" + grjd.archive_no + "','" + grjd.name + "','" + grjd.Cardcode + "', '" + barcode + "', '" + time + "', '" + grjd.doctor_name + "', '" + frmLogin.userCode + "', '" + grjd.create_name + "', '" + grjd.create_org + "', '"+ frmLogin.organName + "','" + ctime + "', '" + rt + "', '" + grjd.doctor_id + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //打印条码并保存个人基础信息
        public bool addJkInfo(bean.jkBean jkbar)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
            string uploadstatus = "0";
            String sql = "insert into zkhw_tj_jk (ID,aichive_no,id_number,bar_code,name,age,sex,Pic1,Pic2,village_code,address,Xzhuzhi,XzjdName,CjwhName,JddwName,JdrName,ZrysName,Caozuoyuan,SjName,Carcode,createtime,upload_status) values ('" + id + "','" + jkbar.aichive_no + "','" + jkbar.id_number + "', '" + jkbar.bar_code + "', '" + jkbar.name + "', '" + jkbar.age + "', '" + jkbar.sex + "', '" + jkbar.Pic1 + "', '" + jkbar.Pic2 + "', '" + jkbar.village_code + "', '" + jkbar.address + "', '" + jkbar.Xzhudi + "', '" + jkbar.XzjdName + "', '" + jkbar.CjwhName + "', '" + jkbar.JddwName + "', '" + jkbar.JdrName + "', '" + jkbar.ZrysName + "', '" + jkbar.Caozuoyuan + "', '" + jkbar.SjName + "', '" + jkbar.Carcode + "', '" + time + "', '" + uploadstatus + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }

        //保存体检统计信息
        public bool addBgdcInfo(bean.grjdxxBean grjbxx, string barcode, string xcuncode)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string time1 = DateTime.Now.ToString("yyyy-MM-dd");
            String sql = "";
            if (grjbxx.age < 65)
            {
                sql = "insert into zkhw_tj_bgdc (ID,aichive_no,id_number,bar_code,name,sex,birthday,age,healthchecktime,createtime,area_duns,lnrzlnlpg,lnrzytzbs) value('" + id + "','" + grjbxx.archive_no + "','" + grjbxx.Cardcode + "', '" + barcode + "', '" + grjbxx.name + "', '" + grjbxx.Sex + "', '" + grjbxx.Birthday + "', '" + grjbxx.age + "', '" + time1 + "', '" + time + "', '" + xcuncode + "', '1', '1')";
            }
            else {
                sql = "insert into zkhw_tj_bgdc (ID,aichive_no,id_number,bar_code,name,sex,birthday,age,healthchecktime,createtime,area_duns) value('" + id + "','" + grjbxx.archive_no + "','" + grjbxx.Cardcode + "', '" + barcode + "', '" + grjbxx.name + "', '" + grjbxx.Sex + "', '" + grjbxx.Birthday + "', '" + grjbxx.age + "', '" + time1 + "', '" + time + "', '" + xcuncode + "')";
            }
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //登记界面右侧 查询功能
        public DataTable registrationRecordInfo(string name)
        {
            DataSet ds = new DataSet();
            //string sql = "select a.name,(case when a.sex='1' then '男' else '女' end) sex  ,a.id_number,a.archive_no,k.bar_code from resident_base_info a right join zkhw_tj_jk k on a.id_number=k.id_number and a.id_number like '%" + name + "%' or a.name like '%" + name + "%' or a.archive_no like '%" + name + "%' GROUP BY k.id_number";
            string sql = "select k.name,(case when k.sex ='1' then '男' else '女' end) sex,k.id_number,k.aichive_no,k.bar_code from zkhw_tj_jk k where k.id_number like '%" + name + "%' or k.name like '%" + name + "%' or k.aichive_no like '%" + name + "%'";
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
        public DataTable jkAllNum(string areacode, string time)
        {
            DataSet ds = new DataSet(); 
            string sql = "select count(1) from zkhw_tj_jk a where a.village_code = '" + areacode + "' and  a.createtime >= '" + time + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //获取阈值信息表数据
        public DataTable checkThresholdValues()
        {
            DataSet ds = new DataSet();
            string sql = "select class_type,type,warning_min,warning_max,threshold_min,threshold_max from threshold_value a where 1=1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public void updateGrjdInfo(string id,string photocode)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String sql = @"update resident_base_info set create_time='" + time + "',photo_code='" + photocode + "' where archive = '" + id + "'";
           DbHelperMySQL.ExecuteSql(sql);
        }
        //
        public void updategejdInfo(bean.grjdxxBean grjd)
        {
           string sql = @"update resident_base_info set phone= '" + grjd.phone + "',address='" + grjd.Zhuzhi + "',photo_code='" + grjd.photo_code + "' where id_number = '" + grjd.Cardcode + "'";
           DbHelperMySQL.ExecuteSql(sql);
        }

        //根据身份证号查询居民档案信息中的
        public static DataTable selectResdentDoctorId(string cardcode)
        {
            DataSet ds = new DataSet();
            string sql = "select a.doctor_id from resident_base_info a where a.id_number = '" + cardcode + "' order by create_time desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable selectTjjk(string cardcode)
        {
            DataSet ds = new DataSet();
            string sql = "select a.bar_code from zkhw_tj_jk a where a.id_number = '" + cardcode + "' order by createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            if (ds.Tables.Count<1) { return null; }
            return ds.Tables[0];
        }
        public DataTable selectTjjk(string cardcode, string time1)
        {
            DataSet ds = new DataSet();
            string sql = "select a.bar_code,a.createtime from zkhw_tj_jk a where a.createtime >='" + time1 + "' and a.id_number = '" + cardcode + "' order by createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            if (ds.Tables.Count < 1) { return null; }
            return ds.Tables[0];
        }
        public DataTable selectTjjk(string cardcode,string time1,string time2)
        {
            DataSet ds = new DataSet();
            string sql = "select a.bar_code,a.createtime from zkhw_tj_jk a where a.createtime >='" + time1 + "' and a.createtime <='" + time2 + "' and a.id_number = '" + cardcode + "' order by createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            if (ds.Tables.Count < 1) { return null; }
            return ds.Tables[0];
        }
    }
}
