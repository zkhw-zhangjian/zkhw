using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.bean;

namespace zkhwClient.dao
{
    class healthPoorServicesDao
    {
        public DataTable querytcmHealthServices(string pCa, string time1, string time2,string cun)
        {
            DataSet ds = new DataSet();
            string sql = "SELECT aa.id,bb.name,bb.archive_no,bb.id_number,(case sex when '1' then '男' when '2' then '女' ELSE ''END) sex,birthday,aa.visit_date,aa.visit_type,bb.doctor_id FROM (select b.name, b.archive_no, b.id_number,sex,birthday,doctor_id from resident_base_info b where 1=1 and is_poor = '1'";
            if (cun != null && !"".Equals(cun)) { sql += " AND b.village_code='" + cun + "'"; }
            if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
            sql += ") bb LEFT JOIN(select id,archive_no,visit_date,(case visit_type when '1' then '门诊' when '2' then '家庭' when '3' then '电话' ELSE ''END) visit_type from poor_follow_record where visit_date >= '" + time1 + "' and visit_date <= '" + time2 + "') aa on bb.archive_no = aa.archive_no";
            ds = DbHelperMySQL.Query(sql);
            if (ds.Tables.Count<1) { return null; }
            return ds.Tables[0];
        }

        public DataTable checkTcmHealthServicesByno(string code,string idnum)
        {
            DataSet ds = new DataSet();
            string sql = "select id from poor_follow_record where archive_no = '" + code + "' and id_number = '"+ idnum + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool aUelderly_selfcare_estimate(healthPoor hp , string id)
        {
            int ret = 0;
            String sql = "";
            if (id == "")
            {
                id = Result.GetNewId();
                sql = @"insert into poor_follow_record (id,name,archive_no,id_number,visit_date,visit_type,sex,birthday,visit_doctor,work_info,advice,create_user,create_name,create_org,create_org_name,create_time,upload_status) values ";
                sql += @" ('" + id + "','" + hp.name + "', '" + hp.archive_no + "', '" + hp.id_number + "', '" + hp.visit_date + "', '" + hp.visit_type + "', '" + hp.sex + "', '" + hp.birthday + "', '" + hp.visit_doctor + "', '" + hp.work_info + "', '" + hp.advice + "','" + frmLogin.userCode + "','" + frmLogin.name + "','" + frmLogin.organCode + "','" + frmLogin.organName + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '0')";
            }
            else
            {
                sql = @"update poor_follow_record set visit_date='" + hp.visit_date + "',visit_type='" + hp.visit_type + "',work_info='" + hp.work_info + "',advice='" + hp.advice + "',update_user= '" + frmLogin.userCode + "',update_name= '" + frmLogin.name + "',update_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where archive_no = '" + hp.archive_no + "' and id_number='"+ hp.id_number+ "'";
            }
            ret = DbHelperMySQL.ExecuteSql(sql);

            return ret == 0 ? false : true;
        }
    }
}
