using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.dao
{
    class olderHelthServiceDao
    {
        public bool deleteOlderHelthService(string archive_no)
        {
            int rt = 0;
            string sql = "delete from elderly_selfcare_estimate where aichive_no = '" + archive_no + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public DataTable queryOlderHelthService(string pCa, string time1, string time2,string code)
        {
            string sql = "";
            DataSet ds = new DataSet();
            //这里判断起始日期如果不等于当前日期那么就调用内连接 2019-6-13
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime st = DateTime.Parse(time1);
            if(dt !=st)
            {
                sql = "SELECT bb.name,bb.archive_no,bb.id_number,aa.total_score,aa.judgement_result,aa.test_date,aa.test_doctor,bb.sex FROM (select b.name, b.archive_no, b.id_number,b.sex from resident_base_info b where 1=1 and age >= '65'";
                if (code != null && !"".Equals(code)) { sql += " AND b.village_code='" + code + "'"; }
                if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
                sql += ") bb INNER JOIN(select a.aichive_no, a.total_score, a.judgement_result, a.test_date, a.test_doctor from elderly_selfcare_estimate a where a.test_date >= '" + time1 + "' and a.test_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no";
            }
            else
            {
                sql = "SELECT bb.name,bb.archive_no,bb.id_number,aa.total_score,aa.judgement_result,aa.test_date,aa.test_doctor,bb.sex FROM (select b.name, b.archive_no, b.id_number,b.sex from resident_base_info b where 1=1 and age >= '65'";
                if (code != null && !"".Equals(code)) { sql += " AND b.village_code='" + code + "'"; }
                if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
                sql += ") bb LEFT JOIN(select a.aichive_no, a.total_score, a.judgement_result, a.test_date, a.test_doctor from elderly_selfcare_estimate a where a.test_date >= '" + time1 + "' and a.test_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryOlderHelthService0()
        {
            DataSet ds = new DataSet();
            string sql = "select count(*) as label10,count(CASE WHEN sex = '1' THEN 1 END) as label11,count(CASE WHEN sex = '2' THEN 1 END) as label13,count(CASE WHEN total_score >=0 and total_score<= 3 THEN 1 END) as label15,count(CASE WHEN total_score>=4 and total_score<= 8 THEN 1 END) as label17,count(CASE WHEN total_score>=9 and total_score <= 18 THEN 1 END) as label18,count(CASE WHEN total_score >= 19 THEN 1 END) as label21 from elderly_selfcare_estimate where test_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable query(string archive_no)
        {
            DataSet ds = new DataSet();
            string sql = "select * from elderly_selfcare_estimate where aichive_no = '" + archive_no + "'";
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
                sql = @"insert into elderly_selfcare_estimate (id,name,aichive_no,id_number,sex,test_date,answer_result,total_score,judgement_result,test_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) values ";
                sql += @" ('" + id + "','" + hm.name + "', '" + hm.aichive_no + "', '" + hm.id_number + "', '" + hm.sex + "', '" + hm.test_date + "', '" + hm.answer_result + "', '" + hm.total_score + "', '" + hm.judgement_result + "', '" + hm.test_doctor + "','" + frmLogin.userCode + "','" + frmLogin.name + "', '" + frmLogin.organCode + "', '" + frmLogin.organName + "', '" + hm.create_time + "', '" + hm.upload_status + "')";
            }
            else
            {
                sql = @"update elderly_selfcare_estimate set test_date='" + hm.test_date + "',answer_result='" + hm.answer_result + "',total_score='" + hm.total_score + "',judgement_result='" + hm.judgement_result + "',test_doctor= '" + hm.test_doctor + "',update_user= '" + frmLogin.userCode + "',update_name= '" + frmLogin.name + "',update_time='" + hm.update_time + "',upload_status= '" + hm.upload_status + "' where aichive_no = '" + id + "'";
            }
            ret = DbHelperMySQL.ExecuteSql(sql);

            return ret == 0 ? false : true;
        }
        public DataTable queryOlderHelthService(string aichive_no)
        {
            DataSet ds = new DataSet();
            string sql = "select * from elderly_selfcare_estimate where aichive_no = '" + aichive_no + "' order by create_time desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }


        #region 为了上传重新编写 2019-6-12
        public DataTable queryOlderHelthService1(string pCa, string time1, string time2, string code)
        {
            DataSet ds = new DataSet();
            string sql = "";
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime st = DateTime.Parse(time1);
            if(dt !=st)   // 这里判断起始日期如果不等于当前日期那么就调用内连接 2019 - 6 - 13
            {
                sql = "SELECT bb.name,bb.archive_no,bb.id_number,aa.total_score,aa.judgement_result,aa.test_date,aa.test_doctor,bb.sex,aa.upload_status ,(case aa.upload_status when '1' then '是' ELSE '否' END) cstatus FROM (select b.name, b.archive_no, b.id_number,b.sex from resident_base_info b where 1=1 and age >= '65'";
                if (code != null && !"".Equals(code)) { sql += " AND b.village_code='" + code + "'"; }
                if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
                sql += ") bb INNER JOIN(select a.aichive_no, a.total_score, a.judgement_result, a.test_date, a.test_doctor,a.upload_status from elderly_selfcare_estimate a where a.test_date >= '" + time1 + "' and a.test_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no";
            }
            else
            {
                sql = "SELECT bb.name,bb.archive_no,bb.id_number,aa.total_score,aa.judgement_result,aa.test_date,aa.test_doctor,bb.sex,aa.upload_status ,(case aa.upload_status when '1' then '是' ELSE '否' END) cstatus FROM (select b.name, b.archive_no, b.id_number,b.sex from resident_base_info b where 1=1 and age >= '65'";
                if (code != null && !"".Equals(code)) { sql += " AND b.village_code='" + code + "'"; }
                if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
                sql += ") bb LEFT JOIN(select a.aichive_no, a.total_score, a.judgement_result, a.test_date, a.test_doctor,a.upload_status from elderly_selfcare_estimate a where a.test_date >= '" + time1 + "' and a.test_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        #endregion
    }
}
