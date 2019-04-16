using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class healthCheckupDao
    {
        public bool deleteOlderHelthService(string id)
        {
            int rt = 0;
            string sql = "delete from elderly_selfcare_estimate  where id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public DataTable queryhealthCheckup(string pCa, string time1, string time2)
        {
            DataSet ds = new DataSet();
            string sql = "select aichive_no,id_number,bar_code,name,check_date,doctor_name from physical_examination_record where check_date >= '" + time1 + "' and check_date <= '" + time2 + "'";
            if (pCa != "") { sql += " and (name like '%" + pCa + "%'  or id_number like '%" + pCa + "%'  or aichive_no like '%" + pCa + "%')"; }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryOlderHelthService0()
        {
            DataSet ds = new DataSet();
            string sql = "select count(*) as label10,count(sex = '男') as label11,count(sex = '女') as label13,count(0<=total_score <= 3) as label15,count(4<=total_score <= 8) as label17,count(9<=total_score <= 18) as label18,count(total_score >= 19) as label21 from elderly_selfcare_estimate where test_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable query(string id_number)
        {
            DataSet ds = new DataSet();
            string sql = "select * from elderly_selfcare_estimate where id_number = '" + id_number + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public bool aUelderly_selfcare_estimate(bean.elderly_selfcare_estimateBean hm, string id)
        {
            int ret = 0;
            String sql = "";
            if (id == "")
            {
                id = Result.GetNewId();
                sql = @"insert into elderly_selfcare_estimate (id,name,aichive_no,id_number,sex,test_date,answer_result,total_score,judgement_result,test_doctor,create_user,create_name,create_org,create_org_name,create_time,update_user,update_name,update_time,upload_status,upload_time,upload_result) values ";
                sql += @" ('" + id + "','" + hm.name + "', '" + hm.aichive_no + "', '" + hm.id_number + "', '" + hm.sex + "', '" + hm.test_date + "', '" + hm.answer_result + "', '" + hm.total_score + "', '" + hm.judgement_result + "', '" + hm.test_doctor + "','" + hm.create_user + "','" + hm.create_name + "', '" + hm.create_org + "', '" + hm.create_org_name + "', '" + hm.create_time + "', '" + hm.update_user + "', '" + hm.update_name + "', '" + hm.update_time + "', '" + hm.upload_status + "', '" + hm.upload_time + "', '" + hm.upload_result + "')";
            }
            else
            {
                //sql = @"update elderly_selfcare_estimate set name='" + hm.name + "',aichive_no='" + hm.aichive_no + "',id_number='" + hm.id_number + "',sex='" + hm.sex + "',test_date='" + hm.test_date + "',answer_result='" + hm.answer_result + "',total_score='" + hm.total_score + "',judgement_result='" + hm.judgement_result + "',test_doctor= '" + hm.test_doctor + "',create_user='" + hm.create_user + "',create_name='" + hm.create_name + "',create_org='" + hm.create_org + "',create_org_name= '" + hm.create_org_name + "',create_time= '" + hm.create_time + "',update_user= '" + hm.update_user + "',update_name= '" + hm.update_name + "',update_time='" + hm.update_time + "',upload_status= '" + hm.upload_status + "',upload_time='" + hm.upload_time + "',upload_result='" + hm.upload_result + "' where id = '" + id + "'";
                sql = @"update elderly_selfcare_estimate set name='" + hm.name + "',aichive_no='" + hm.aichive_no + "',id_number='" + hm.id_number + "',sex='" + hm.sex + "',test_date='" + hm.test_date + "',answer_result='" + hm.answer_result + "',total_score='" + hm.total_score + "',judgement_result='" + hm.judgement_result + "',test_doctor= '" + hm.test_doctor + "',create_user='" + hm.create_user + "',create_name='" + hm.create_name + "',create_org='" + hm.create_org + "',create_org_name= '" + hm.create_org_name + "',update_user= '" + hm.update_user + "',update_name= '" + hm.update_name + "',update_time='" + hm.update_time + "',upload_status= '" + hm.upload_status + "',upload_time='" + hm.upload_time + "',upload_result='" + hm.upload_result + "' where id = '" + id + "'";

            }

            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public DataTable queryOlderHelthService(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select * from elderly_selfcare_estimate where id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
