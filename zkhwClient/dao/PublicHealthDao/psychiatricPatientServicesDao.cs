using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.dao
{
    class psychiatricPatientServicesDao
    {
        public DataTable queryPsychosis_info(string pCa, string time1, string time2, string code)
        {
            DataSet ds = new DataSet();
            string sql = "SELECT bb.name,bb.archive_no,bb.id_number,aa.record_date,aa.guardian_name,aa.record_doctor,aa.id FROM (select b.name, b.archive_no, b.id_number from resident_base_info b where 1=1";
            if (code != "") { sql += " AND b.village_code='" + code + "'"; }
            if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
            sql += ") bb LEFT JOIN(select id,archive_no,record_date,guardian_name,record_doctor from psychosis_info where record_date >= '" + time1 + "' and record_date <= '" + time2 + "') aa on bb.archive_no = aa.archive_no";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool deletePsychosis_info(string id)
        {
            int rt = 0;
            string sql = "delete from psychosis_info where id='" + id + "'";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public bool aUpsychosis_info(bean.psychosis_infoBean hm, string id)
        {
            int ret = 0;
            String sql = "";
            if (id == "")
            {
                id = Result.GetNewId();
                sql = @"insert into psychosis_info(id,name,archive_no,id_number,guardian_name,guardian_relation,guardian_address,guardian_phone,neighborhood_committee_linkman,neighborhood_committee_linktel,resident_type,employment_status,agree_manage,agree_name,agree_date,first_morbidity_date,symptom,isolation,outpatient,first_medicine_date,hospitalized_num,diagnosis,diagnosis_hospital,diagnosis_date,recently_treatment_effect,dangerous_act,slight_trouble_num,cause_trouble_num,cause_accident_num,harm_other_num,autolesion_num,attempted_suicide_num,economics,specialist_suggestion,record_date,record_doctor,create_user,create_name,create_org,create_org_name,create_time,upload_status) values ";
                sql += @" ('" + id + "','" + hm.name + "', '" + hm.archive_no + "', '" + hm.id_number + "', '" + hm.guardian_name + "', '" + hm.guardian_relation + "', '" + hm.guardian_address + "', '" + hm.guardian_phone + "', '" + hm.neighborhood_committee_linkman + "', '" + hm.neighborhood_committee_linktel + "', '" + hm.resident_type + "','" + hm.employment_status + "', '" + hm.agree_manage + "', '" + hm.agree_name + "', '" + hm.agree_date + "', '" + hm.first_morbidity_date + "', '" + hm.symptom + "', '" + hm.isolation + "', '" + hm.outpatient + "', '" + hm.first_medicine_date + "', '" + hm.hospitalized_num + "','" + hm.diagnosis + "', '" + hm.diagnosis_hospital + "', '" + hm.diagnosis_date + "', '" + hm.recently_treatment_effect + "', '" + hm.dangerous_act + "', '" + hm.slight_trouble_num + "', '" + hm.cause_trouble_num + "', '" + hm.cause_accident_num + "', '" + hm.harm_other_num + "', '" + hm.autolesion_num + "','" + hm.attempted_suicide_num + "', '" + hm.economics + "', '" + hm.specialist_suggestion + "', '" + hm.record_date + "', '" + hm.record_doctor + "', '" + frmLogin.userCode + "', '" + frmLogin.name + "', '" + frmLogin.organCode + "', '" + frmLogin.organName + "','" + hm.create_time + "', '" + hm.upload_status + "')";
            }
            else
            {
                sql = @"update psychosis_info set guardian_name ='" + hm.guardian_name + "',guardian_relation='" + hm.guardian_relation + "',guardian_address='" + hm.guardian_address + "',guardian_phone='" + hm.guardian_phone + "',neighborhood_committee_linkman='" + hm.neighborhood_committee_linkman + "',neighborhood_committee_linktel='" + hm.neighborhood_committee_linktel + "',resident_type='" + hm.resident_type + "',employment_status='" + hm.employment_status + "',agree_manage='" + hm.agree_manage + "',agree_name='" + hm.agree_name + "',agree_date='" + hm.agree_date + "',first_morbidity_date='" + hm.first_morbidity_date + "',symptom='" + hm.symptom + "',isolation='" + hm.isolation + "',outpatient='" + hm.outpatient + "',first_medicine_date='" + hm.first_medicine_date + "',hospitalized_num='" + hm.hospitalized_num + "',diagnosis='" + hm.diagnosis + "',diagnosis_hospital='" + hm.diagnosis_hospital + "',diagnosis_date= '" + hm.diagnosis_date + "',recently_treatment_effect='" + hm.recently_treatment_effect + "',dangerous_act='" + hm.dangerous_act + "',slight_trouble_num= '" + hm.slight_trouble_num + "',cause_trouble_num= '" + hm.cause_trouble_num + "',cause_accident_num='" + hm.cause_accident_num + "',harm_other_num='" + hm.harm_other_num + "',autolesion_num= '" + hm.autolesion_num + "',attempted_suicide_num='" + hm.attempted_suicide_num + "',economics='" + hm.economics + "',specialist_suggestion='" + hm.specialist_suggestion + "',record_date='" + hm.record_date + "',record_doctor= '" + hm.record_doctor + "',update_user='" + frmLogin.userCode + "',update_name='" + frmLogin.name + "',update_time='" + hm.update_time + "' where id = '" + id + "'";
            }
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //祖
        public DataTable queryPsychosis_info(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select * from psychosis_info where id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
