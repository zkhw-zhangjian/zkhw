using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.dao
{
    class tcmHealthServicesDao
    {
        public DataTable querytcmHealthServices(string pCa, string time1, string time2)
        {
            DataSet ds = new DataSet();
            string sql = "select id,name,id_number,aichive_no,test_date,test_doctor,upload_status from elderly_tcm_record where test_date >= '" + time1 + "' and test_date <= '" + time2 + "'";
            if (pCa != "") { sql += " and (patientName like '%" + pCa + "%'  or id_number like '%" + pCa + "%'  or aichive_no like '%" + pCa + "%')"; }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool deletetcmHealthServices(string id)
        {
            int rt = 0;
            string sql = "delete from elderly_tcm_record where id='" + id + "'";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public DataTable checkTcmHealthServicesByno(string code,string idnum)
        {
            DataSet ds = new DataSet();
            string sql = "select id from elderly_tcm_record where aichive_no = '" + code + "' and id_number = '"+ idnum + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable checkTcmHealthServicesByno1(string code, string idnum,string barcode)
        {
            DataSet ds = new DataSet();
            string sql = @"select * from elderly_tcm_record e inner join physical_examination_record p on e.exam_id = p.id
                where e.aichive_no = '" + code + "' and e.id_number = '" + idnum + "' and p.bar_code='"+ barcode + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable checkTcmHealthServicesByExamID(string examid)
        {
            DataSet ds = new DataSet();
            string sql = "select * from elderly_tcm_record where exam_id = '" + examid + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
