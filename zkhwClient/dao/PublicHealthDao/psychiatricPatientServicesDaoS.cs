using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class psychiatricPatientServicesDaoS
    {
        public DataTable queryPsychosis_follow_record(string pCa, string time1, string time2, string code)
        {
            DataSet ds = new DataSet();
            string sql = @"SELECT bb.name,bb.archive_no,bb.id_number,aa.visit_date,aa.visit_doctor,aa.next_visit_date,aa.id FROM (select b.name, b.archive_no, b.id_number from resident_base_info b where 1=1";
            if (code != "") { sql += " AND b.village_code='" + code + "'"; }
            if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
            sql += ") bb LEFT JOIN(select id,archive_no,visit_date,visit_doctor,next_visit_date from psychosis_follow_record where visit_date >= '" + time1 + "' and visit_date <= '" + time2 + "') aa on bb.archive_no = aa.archive_no";
            ds = DbHelperMySQL.Query(sql); 
            return ds.Tables[0];
        }
        public bool deletePsychosis_info(string id)
        {
            int rt = 0;
            string sql = "delete from psychosis_follow_record where id='" + id + "'";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public bool aUPsychosis_follow_record(bean.psychosis_follow_recordBean hm, string id)
        {
            int ret = 0;
            String sql = "";
            if (id == "")
            {
                id = Result.GetNewId();
                sql = @"insert into psychosis_follow_record(id,name,archive_no,Cardcode,visit_date,visit_type,miss_reason,miss_reason_other,die_date,die_reason,physical_disease,die_reason_other,fatalness,symptom,symptom_other,insight,sleep_status,dietary_status,self_help,housework,work,learning_ability,interpersonal,dangerous_act,slight_trouble_num,cause_trouble_num,cause_accident_num,harm_other_num,autolesion_num,attempted_suicide_num,isolation,hospitalized_status,out_hospital_date,laboratory_examination,compliance,untoward_effect,untoward_effect_info,treatment_effect,create_user,create_name,create_org,create_org_name,create_time,update_user,update_name,update_time,upload_status,upload_time,upload_result) values ";
                sql += @" ('" + id + "','" + hm.name + "', '" + hm.archive_no + "', '" + hm.Cardcode + "', '" + hm.visit_date + "', '" + hm.visit_type + "', '" + hm.miss_reason + "', '" + hm.miss_reason_other + "', '" + hm.die_date + "', '" + hm.die_reason + "', '" + hm.physical_disease + "','" + hm.die_reason_other + "', '" + hm.fatalness + "', '" + hm.symptom + "', '" + hm.symptom_other + "', '" + hm.insight + "', '" + hm.sleep_status + "', '" + hm.dietary_status + "', '" + hm.self_help + "', '" + hm.housework + "', '" + hm.work + "','" + hm.learning_ability + "', '" + hm.interpersonal + "', '" + hm.dangerous_act + "', '" + hm.slight_trouble_num + "', '" + hm.cause_trouble_num + "', '" + hm.cause_accident_num + "', '" + hm.harm_other_num + "', '" + hm.autolesion_num + "','" + hm.attempted_suicide_num + "', '" + hm.isolation + "', '" + hm.hospitalized_status + "', '" + hm.out_hospital_date + "', '" + hm.laboratory_examination + "', '" + hm.compliance + "', '" + hm.untoward_effect + "', '" + hm.untoward_effect_info + "', '" + hm.treatment_effect + "', '" + hm.create_user + "', '" + hm.create_name + "', '" + hm.create_org + "', '" + hm.create_org_name + "', '" + hm.create_time + "', '" + hm.update_user + "', '" + hm.update_name + "', '" + hm.update_time + "', '" + hm.upload_status + "', '" + hm.upload_time + "', '" + hm.upload_result + "')";       
            }
            else
            {
                sql = @"update psychosis_follow_record set visit_date ='" + hm.visit_date + "',visit_type='" + hm.visit_type + "',miss_reason='" + hm.miss_reason + "',miss_reason_other='" + hm.miss_reason_other + "',die_date='" + hm.die_date + "',die_reason='" + hm.die_reason + "',physical_disease='" + hm.physical_disease + "',die_reason_other='" + hm.die_reason_other + "',fatalness='" + hm.fatalness + "',symptom='" + hm.symptom + "',symptom_other='" + hm.symptom_other + "',insight='" + hm.insight + "',sleep_status='" + hm.sleep_status + "',dietary_status='" + hm.dietary_status + "',self_help='" + hm.self_help + "',housework='" + hm.housework + "',work='" + hm.work + "',learning_ability='" + hm.learning_ability + "',interpersonal='" + hm.interpersonal + "',dangerous_act= '" + hm.dangerous_act + "',slight_trouble_num= '" + hm.slight_trouble_num + "',cause_trouble_num= '" + hm.cause_trouble_num + "',cause_accident_num='" + hm.cause_accident_num + "',harm_other_num='" + hm.harm_other_num + "',autolesion_num= '" + hm.autolesion_num + "',attempted_suicide_num='" + hm.attempted_suicide_num + "',isolation='" + hm.isolation + "',hospitalized_status='" + hm.hospitalized_status + "',out_hospital_date='" + hm.out_hospital_date + "',laboratory_examination='" + hm.laboratory_examination + "',compliance='" + hm.compliance + "',untoward_effect= '" + hm.untoward_effect + "',untoward_effect_info='" + hm.untoward_effect_info + "',treatment_effect='" + hm.treatment_effect + "' where id = '" + id + "'";
            }
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }

        public bool aUPsychosis_follow_record(bean.psychosis_follow_recordBean hm, string id, DataTable goodsList)
        {
            int ret = 0;
            String sql = "";
            String sql0 = "";
            if (id != "")
            {
                sql = @"update psychosis_follow_record set transfer_treatment='" + hm.transfer_treatment + "',transfer_treatment_reason='" + hm.transfer_treatment_reason + "',transfer_treatment_department='" + hm.transfer_treatment_department + "',rehabilitation_measure='" + hm.rehabilitation_measure + "',rehabilitation_measure_other='" + hm.rehabilitation_measure_other + "',next_visit_classify='" + hm.next_visit_classify + "',next_visit_date='" + hm.next_visit_date + "',visit_doctor='" + hm.visit_doctor + "' where id = '" + id + "'";
                sql0 = @"delete from follow_medicine_record  where follow_id = '" + id + "';";
                if (goodsList.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql0 += "insert into follow_medicine_record(follow_id,drug_name,num,dosage,upload_status,create_name,create_time) values ('" + id + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["num"] + "','" + goodsList.Rows[i]["dosage"] + "','0','" + frmLogin.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        }
                        else
                        {
                            sql0 += ",('" + id + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["num"] + "','" + goodsList.Rows[i]["dosage"] + "','0','" + frmLogin.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        }
                    }
                }
            }
            if (sql0 != "")
            {
                DbHelperMySQL.ExecuteSql(sql0);
            }
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //祖
        public DataTable queryPsychosis_follow_record(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select * from psychosis_follow_record where id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryPsychosis_follow_record0(string archive_no)
        {
            DataSet ds = new DataSet();
            string sql = "select id,max(visit_date) from psychosis_follow_record where archive_no = '" + archive_no + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryFollow_medicine_record(string follow_id)
        {
            DataSet ds = new DataSet();
            string sql = "select id,follow_id,drug_name,num,dosage from follow_medicine_record where follow_id = '" + follow_id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
