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
        public bool addLtdOrganization(List<ltdOrganization> list)
        {
            int rt = 0;
            string id = Result.GetNewId();
            String sql = "insert into ltd_organization (id,pub_orgcode,organ_code,organ_name,organ_short_name,organ_level,organ_parent_code,create_user_code,create_time,update_user_code,update_time,is_delete.remark,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,address,lng.lat) values ";
            for (int j = 0; j < list.Count; j++)
            {
                if (j > 0)
                {
                    sql += " , ('" + list[j].id + "','" + list[j].pub_orgcode + "','" + list[j].organ_code + "', '" + list[j].organ_name + "', '" + list[j].organ_short_name + "', '" + list[j].organ_level + "', '" + list[j].organ_parent_code + "', '" + list[j].create_user_code + "', '" + list[j].create_time + "', '" + list[j].update_user_code + "', '" + list[j].update_time + "', '" + list[j].is_delete + "', '" + list[j].remark + "', '" + list[j].province_code + "', '" + list[j].province_name + "', '" + list[j].city_code + "', '" + list[j].city_name + "', '" + list[j].county_code + "', '" + list[j].county_name + "', '" + list[j].towns_code + "', '" + list[j].towns_name + "', '" + list[j].village_code + "', '" + list[j].village_name + "', '" + list[j].address + "', '" + list[j].lng + "', '" + list[j].lat + "')";
                }
                else
                {
                    sql += "('" + list[j].id + "','" + list[j].pub_orgcode + "','" + list[j].organ_code + "', '" + list[j].organ_name + "', '" + list[j].organ_short_name + "', '" + list[j].organ_level + "', '" + list[j].organ_parent_code + "', '" + list[j].create_user_code + "', '" + list[j].create_time + "', '" + list[j].update_user_code + "', '" + list[j].update_time + "', '" + list[j].is_delete + "', '" + list[j].remark + "', '" + list[j].province_code + "', '" + list[j].province_name + "', '" + list[j].city_code + "', '" + list[j].city_name + "', '" + list[j].county_code + "', '" + list[j].county_name + "', '" + list[j].towns_code + "', '" + list[j].towns_name + "', '" + list[j].village_code + "', '" + list[j].village_name + "', '" + list[j].address + "', '" + list[j].lng + "', '" + list[j].lat + "')";
                }
            }
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
            String sql = "insert into zkhw_user_info (ID,username,password,user_code,user_name,pub_usercode,sex,job_num,tele_phone,mail,birthday,organ_code,parent_organ,depart_code,user_type_code,data_level,status,is_delete,create_time,create_user_code,update_time,update_user_code) values ";
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

        //根据获取乡镇信息编号，获取对应的个人信息档案
        public DataTable checkResidentBycode(string areacode)
        {
            DataSet ds = new DataSet();
            string sql = "select * from resident_base_info where towns_code > '" + areacode + "'";
            ds = DbHelperMySQL.QueryYpt(sql);
            return ds.Tables[0];
        }
        //获取对应的个人信息档案信息，在本机的数据库机构表中存储
        public bool addResidents(List<resident_base_infoBean> list)
        {
            int rt = 0;
            String sql = "insert into resident_base_info (id,archive_no,pb_archive,name,sex,birthday,age,id_number,card_pic,company,phone,link_name,link_phone,resident_type,address,residence_address,nation,blood_group,blood_rh,education,profession,marital_status,pay_type,pay_other,drug_allergy,allergy_other,exposure,disease_other,is_hypertension,is_diabetes,is_psychosis,is_tuberculosis,is_heredity,heredity_name,is_deformity,deformity_name,is_poor,kitchen,fuel,other_fuel,drink,other_drink,toilet,poultry,medical_code,photo_code,aichive_org,doctor_name,create_archives_name,is_signing,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,status,remark,create_user,create_name,create_time,create_org,create_org_name,update_user,update_name) values ";
            for (int j = 0; j < list.Count; j++)
            {
                if (j > 0)
                {
                    sql += " , ('" + list[j].id + "','" + list[j].archive_no + "', '" + list[j].pb_archive + "','" + list[j].name + "','" + list[j].sex + "','" + list[j].birthday + "','" + list[j].age + "','" + list[j].id_number + "','" + list[j].CardPic + "','" + list[j].company + "','" + list[j].phone + "','" + list[j].link_name + "','" + list[j].link_phone + "','" + list[j].resident_type + "','" + list[j].address + "','" + list[j].residence_address + "', '" + list[j].nation + "', '" + list[j].blood_group + "', '" + list[j].blood_rh + "', '" + list[j].education + "', '" + list[j].profession + "', '" + list[j].marital_status + "', '" + list[j].pay_type + "', '" + list[j].pay_other + "', '" + list[j].drug_allergy + "', '" + list[j].allergy_other + "', '" + list[j].exposure + "', '" + list[j].disease_other + "', '" + list[j].is_hypertension + "', '" + list[j].is_diabetes + "', '" + list[j].is_psychosis + "', '" + list[j].is_tuberculosis + "', '" + list[j].is_heredity + "', '" + list[j].heredity_name + "', '" + list[j].is_deformity + "', '" + list[j].deformity_name + "', '" + list[j].is_poor + "', '" + list[j].kitchen + "', '" + list[j].fuel + "', '" + list[j].other_fuel + "', '" + list[j].drink + "', '" + list[j].other_drink + "', '" + list[j].toilet + "', '" + list[j].poultry + "', '" + list[j].medical_code + "', '" + list[j].photo_code + "', '" + list[j].aichive_org + "', '" + list[j].doctor_name + "', '" + list[j].create_name + "', '" + list[j].is_signing + "', '" + list[j].province_code + "', '" + list[j].province_name + "', '" + list[j].city_code + "', '" + list[j].city_name + "', '" + list[j].county_code + "', '" + list[j].county_name + "', '" + list[j].towns_code + "', '" + list[j].towns_name + "', '" + list[j].village_code + "', '" + list[j].village_name + "', '" + list[j].status + "', '" + list[j].remark + "', '" + list[j].create_user + "', '" + list[j].create_name + "', '" + list[j].create_time + "', '" + list[j].create_org + "', '" + list[j].create_org_name + "', '" + list[j].update_user + "', '" + list[j].update_name + "')";
                }
                else
                {
                    sql += "('" + list[j].id + "','" + list[j].archive_no + "', '" + list[j].pb_archive + "','" + list[j].name + "','" + list[j].sex + "','" + list[j].birthday + "','" + list[j].age + "','" + list[j].id_number + "','" + list[j].CardPic + "','" + list[j].company + "','" + list[j].phone + "','" + list[j].link_name + "','" + list[j].link_phone + "','" + list[j].resident_type + "','" + list[j].address + "','" + list[j].residence_address + "', '" + list[j].nation + "', '" + list[j].blood_group + "', '" + list[j].blood_rh + "', '" + list[j].education + "', '" + list[j].profession + "', '" + list[j].marital_status + "', '" + list[j].pay_type + "', '" + list[j].pay_other + "', '" + list[j].drug_allergy + "', '" + list[j].allergy_other + "', '" + list[j].exposure + "', '" + list[j].disease_other + "', '" + list[j].is_hypertension + "', '" + list[j].is_diabetes + "', '" + list[j].is_psychosis + "', '" + list[j].is_tuberculosis + "', '" + list[j].is_heredity + "', '" + list[j].heredity_name + "', '" + list[j].is_deformity + "', '" + list[j].deformity_name + "', '" + list[j].is_poor + "', '" + list[j].kitchen + "', '" + list[j].fuel + "', '" + list[j].other_fuel + "', '" + list[j].drink + "', '" + list[j].other_drink + "', '" + list[j].toilet + "', '" + list[j].poultry + "', '" + list[j].medical_code + "', '" + list[j].photo_code + "', '" + list[j].aichive_org + "', '" + list[j].doctor_name + "', '" + list[j].create_name + "', '" + list[j].is_signing + "', '" + list[j].province_code + "', '" + list[j].province_name + "', '" + list[j].city_code + "', '" + list[j].city_name + "', '" + list[j].county_code + "', '" + list[j].county_name + "', '" + list[j].towns_code + "', '" + list[j].towns_name + "', '" + list[j].village_code + "', '" + list[j].village_name + "', '" + list[j].status + "', '" + list[j].remark + "', '" + list[j].create_user + "', '" + list[j].create_name + "', '" + list[j].create_time + "', '" + list[j].create_org + "', '" + list[j].create_org_name + "', '" + list[j].update_user + "', '" + list[j].update_name + "')";
                }
            }
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        }

        //根据获取乡镇信息编号，获取对应的家医团队信息
        public DataTable checkTeanInfoBycode(string areacode)
        {
            DataSet ds = new DataSet();
            string sql = "select * from team_info where towns_code > '" + areacode + "'";
            ds = DbHelperMySQL.QueryYpt(sql);
            return ds.Tables[0];
        }

        //获取到家医团队信息，在本机的数据库机构表中存储
        public bool addTeanInfos(List<teamInfoBean> list)
        {
            int rt = 0;
            String sql = "insert into team_info (id,team_no,team_name,team_lead_doctor,team_lead_name,team_lead_phone,team_type,member_num,status,remark,org_code,province_code,province_name,city_code,city_name,county_code,county_name,towns_code,towns_name,village_code,village_name,create_user,create_name,create_time,update_user,update_name,update_time) values ";
            for (int j = 0; j < list.Count; j++)
            {
                if (j > 0)
                {
                    sql += " , ('" + list[j].id + "','" + list[j].team_no + "','" + list[j].team_name + "', '" + list[j].team_lead_doctor + "', '" + list[j].team_lead_name + "', '" + list[j].team_lead_phone + "', '" + list[j].team_type + "', '" + list[j].member_num + "', '" + list[j].status + "', '" + list[j].remark + "', '" + list[j].org_code + "', '" + list[j].province_code + "', '" + list[j].province_name + "', '" + list[j].city_code + "', '" + list[j].city_name + "', '" + list[j].county_code + "', '" + list[j].county_name + "', '" + list[j].towns_code + "', '" + list[j].towns_name + "', '" + list[j].village_code + "', '" + list[j].village_name + "', '" + list[j].create_user + "', '" + list[j].create_name + "', '" + list[j].create_time + "', '" + list[j].update_user + "', '" + list[j].update_name + "', '" + list[j].update_time + "')";
                }
                else
                {
                    sql += "('" + list[j].id + "','" + list[j].team_no + "','" + list[j].team_name + "', '" + list[j].team_lead_doctor + "', '" + list[j].team_lead_name + "', '" + list[j].team_lead_phone + "', '" + list[j].team_type + "', '" + list[j].member_num + "', '" + list[j].status + "', '" + list[j].remark + "', '" + list[j].org_code + "', '" + list[j].province_code + "', '" + list[j].province_name + "', '" + list[j].city_code + "', '" + list[j].city_name + "', '" + list[j].county_code + "', '" + list[j].county_name + "', '" + list[j].towns_code + "', '" + list[j].towns_name + "', '" + list[j].village_code + "', '" + list[j].village_name + "', '" + list[j].create_user + "', '" + list[j].create_name + "', '" + list[j].create_time + "', '" + list[j].update_user + "', '" + list[j].update_name + "', '" + list[j].update_time + "')";
                }
            }
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //根据团队编号，获取对应的团队成员信息
        public DataTable checkTeanDoctorBycode(string code)
        {
            DataSet ds = new DataSet();
            string sql = "select * from team_doctor where tean_no > '" + code + "'";
            ds = DbHelperMySQL.QueryYpt(sql);
            return ds.Tables[0];
        }
        //获取到团队成员信息，在本机的数据库机构表中存储
        public bool addTeanDoctors(List<teamDoctorBean> list)
        {
            int rt = 0;
            String sql = "insert into team_doctor (id,team_no,team_name,doctor_no,doctor_name,position,create_user,create_name,create_time,update_user,update_name,update_time) values ";
            for (int j = 0; j < list.Count; j++)
            {
                if (j > 0)
                {
                    sql += " , ('" + list[j].id + "','" + list[j].team_no + "','" + list[j].team_name + "', '" + list[j].doctor_no + "', '" + list[j].doctor_name + "', '" + list[j].position + "', '" + list[j].create_user + "', '" + list[j].create_name + "', '" + list[j].create_time + "', '" + list[j].update_user + "', '" + list[j].update_name + "', '" + list[j].update_time + "')";
                }
                else
                {
                    sql += "('" + list[j].id + "','" + list[j].team_no + "','" + list[j].team_name + "', '" + list[j].doctor_no + "', '" + list[j].doctor_name + "', '" + list[j].position + "', '" + list[j].create_user + "', '" + list[j].create_name + "', '" + list[j].create_time + "', '" + list[j].update_user + "', '" + list[j].update_name + "', '" + list[j].update_time + "')";
                }
            }
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
    }
}
