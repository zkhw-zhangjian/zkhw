using System;
using System.Collections.Generic;
using System.Data;
using zkhwClient.bean;

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
             String sql = "insert into zkhw_log_syslog (id,userName,type,createtime,type,eventInfo) values ('" + id + "','" + lb.name + "','" + lb.type + "', '" + lb.createTime + "', '" + lb.eventInfo + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }

        //以下是初始化数据获取平台基础数据

        //根据区县编号或乡镇编号，获取机构信息
        public DataTable checkLtdOrganizationBycode(string areacode)
        {
            DataSet ds = new DataSet();
            string sql = "select * from ltd_organization  where towns_code > '" + areacode + "'";
            ds = DbHelperMySQL.QueryYpt(sql);
            return ds.Tables[0];
        }
        //获取到的机构信息，在本机的数据库机构表中存储
        public bool addLtdOrganization(ltdOrganization organ)
        {
            int rt = 0;
            string id = Result.GetNewId();
            String sql = "insert into ltd_organization (id,pub_orgcode,organ_code,organ_name,organ_short_name,organ_level,organ_parent_code,create_user_code,create_time,update_user_code,update_time,is_delete.remark,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,address,lng.lat) values ('" + 
                organ.id + "','" + organ.pub_orgcode + "','" + organ.organ_code + "', '" + organ.organ_name + "', '" + organ.organ_short_name + "', '" + organ.organ_level + "', '" + organ.organ_parent_code + "', '" + organ.create_user_code + "', '" + organ.create_time + "', '" + organ.update_user_code + "', '" + organ.update_time + "', '" + organ.is_delete + "', '" + organ.remark + "', '" 
                + organ.province_code + "', '" + organ.province_name + "', '" + organ.city_code + "', '" + organ.city_name + "', '" + organ.county_code + "', '" + organ.county_name + "', '" + organ.towns_code + "', '" + organ.towns_name + "', '" + organ.village_code + "', '" + organ.village_name + "', '" + organ.address + "', '" + organ.lng + "', '" + organ.lat + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //根据获取机构信息编号，获取对应的所有用户信息
        public DataTable checkUsersBycode(string organcode)
        {
            DataSet ds = new DataSet();
            string sql = "select * from ltd_user  where organ_code > '" + organcode + "'";
            ds = DbHelperMySQL.QueryYpt(sql);
            return ds.Tables[0];
        }
        //获取到的用户信息，在本机的数据库机构表中存储
        public bool addUsers(List<UserInfo> list)
        {
            int rt = 0;
            String sql = "insert into zkhw_user_info (ID,username,password,user_code,user_name,pub_usercode,sex,job_num,tele_phone,mail,birthday,organ_code,parent_organ,depart_code,user_type_code,data_level,status,is_delete,create_time,create_user_code,update_time,update_user_code) values";
            for (int j = 0; j < list.Count; j++)
            {
                string id = Result.GetNewId();
                if (j > 0)
                {
                    sql += " , ('" + id + "','" + list[j].username + "', '" + list[j].password + "','" + list[j].user_code + "','" + list[j].user_name + "','" + list[j].pub_usercode + "','" + list[j].sex + "','" + list[j].job_num + "','" + list[j].tele_phone + "','" + list[j].mail + "','" + list[j].birthday + "','" + list[j].organ_code + "','" + list[j].parent_organ + "','" + list[j].depart_code + "','" + list[j].user_type_code + "','" + list[j].data_level + "', '" + list[j].status + "', '" + list[j].is_delete + "', '" + list[j].create_time + "', '" + list[j].create_user_code + "', '" + list[j].create_user_code + "', '" + list[j].update_user_code + "')";
                }
                else
                {
                    sql += "('" + id + "','" + list[j].username + "', '" + list[j].password + "','" + list[j].user_code + "','" + list[j].user_name + "','" + list[j].pub_usercode + "','" + list[j].sex + "','" + list[j].job_num + "','" + list[j].tele_phone + "','" + list[j].mail + "','" + list[j].birthday + "','" + list[j].organ_code + "','" + list[j].parent_organ + "','" + list[j].depart_code + "','" + list[j].user_type_code + "','" + list[j].data_level + "', '" + list[j].status + "', '" + list[j].is_delete + "', '" + list[j].create_time + "', '" + list[j].create_user_code + "', '" + list[j].create_user_code + "', '" + list[j].update_user_code + "')";
                }
            }
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
    }
}
