using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class diabetesPatientDao
    {
        public DataTable querydiabetesPatient(string pCa, string time1, string time2,string code)
        {
            DataSet ds = new DataSet();
            string sql = "SELECT bb.name,bb.archive_no,bb.id_number,(case when aa.visit_type='1' then '门诊' when aa.visit_type='2' then '家庭' when aa.visit_type='3' then '电话' end) visit_type,aa.visit_date,aa.visit_doctor,aa.id FROM (select b.name, b.archive_no, b.id_number from resident_base_info b where 1=1";
            if (code != "") { sql += " AND b.village_code='" + code + "'"; }
            if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
            sql += ") bb LEFT JOIN(select a.id,a.aichive_no,a.visit_type,a.visit_date,a.visit_doctor from diabetes_follow_record a where a.visit_date >= '" + time1 + "' and a.visit_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public bool deleteDiabetesPatient(string id)
        {
            int rt = 0;
            string sql = "delete from diabetes_follow_record where id='" + id + "';delete from follow_medicine_record  where follow_id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public bool aUdiabetesPatient(bean.diabetes_follow_recordBean hm, string id, DataTable goodsList)
        {
            int ret = 0;
            String sql = "";
            String sql0 = "";
            if (id == "")
            {
                id = Result.GetNewId();
                sql = @"insert into diabetes_follow_record (id,name,aichive_no,id_number,visit_date,visit_type,symptom,symptom_other,blood_pressure_high,blood_pressure_low,weight_now,weight_next,bmi_now,bmi_next,dorsal_artery,other,smoke_now,smoke_next,drink_now,drink_next,sports_num_now,sports_time_now,sports_num_next,sports_time_next,staple_food_now,staple_food_next,psychological_recovery,medical_compliance,blood_glucose,glycosylated_hemoglobin,check_date,compliance,untoward_effect,reactive_hypoglycemia,follow_type,insulin_name,insulin_usage,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time,update_user,update_name,update_time,upload_status,upload_time,upload_result,advice) values ";
                sql += @" ('" + id + "','" + hm.name + "', '" + hm.aichive_no + "', '" + hm.id_number + "', '" + hm.visit_date + "', '" + hm.visit_type + "', '" + hm.symptom + "', '" + hm.symptom_other + "', '" + hm.blood_pressure_high + "', '" + hm.blood_pressure_low + "', '" + hm.weight_now + "','" + hm.weight_next + "', '" + hm.bmi_now + "', '" + hm.bmi_next + "', '" + hm.dorsal_artery + "', '" + hm.other + "', '" + hm.smoke_now + "', '" + hm.smoke_next + "','" + hm.drink_now + "', '" + hm.drink_next + "', '" + hm.sports_num_now + "', '" + hm.sports_time_now + "','" + hm.sports_num_next + "', '" + hm.sports_time_next + "','" + hm.staple_food_now + "', '" + hm.staple_food_next + "', '" + hm.psychological_recovery + "', '" + hm.medical_compliance + "', '" + hm.blood_glucose + "','" + hm.glycosylated_hemoglobin + "', '" + hm.check_date + "', '" + hm.compliance + "','" + hm.untoward_effect + "', '" + hm.reactive_hypoglycemia + "', '" + hm.follow_type + "', '" + hm.insulin_name + "', '" + hm.insulin_usage + "', '" + hm.transfer_treatment + "', '" + hm.transfer_treatment_reason + "', '" + hm.transfer_treatment_department + "','" + hm.next_visit_date + "', '" + hm.visit_doctor + "','" + hm.create_user + "', '" + hm.create_name + "', '" + hm.create_org + "', '" + hm.create_org_name + "', '" + hm.create_time + "', '" + hm.update_user + "', '" + hm.update_name + "', '" + hm.update_time + "','" + hm.upload_status + "', '" + hm.upload_time + "', '" + hm.upload_result + "', '" + hm.advice + "')";
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
            else
            {
                //sql = @"update diabetes_follow_record set name='" + hm.name + "',aichive_no='" + hm.aichive_no + "',id_number='" + hm.id_number + "',visit_date= '" + hm.visit_date + "',visit_type='" + hm.visit_type + "',symptom='" + hm.symptom + "',symptom_other='" + hm.symptom_other + "',blood_pressure_high='" + hm.blood_pressure_high + "',blood_pressure_low= '" + hm.blood_pressure_low + "',weight_now='" + hm.weight_now + "',weight_next='" + hm.weight_next + "',bmi_now= '" + hm.bmi_now + "',bmi_next='" + hm.bmi_next + "',dorsal_artery='" + hm.dorsal_artery + "',other='" + hm.other + "',smoke_now= '" + hm.smoke_now + "',smoke_next='" + hm.smoke_next + "',drink_now='" + hm.drink_now + "',drink_next='" + hm.drink_next + "',sports_num_now='" + hm.sports_num_now + "',sports_time_now='" + hm.sports_time_now + "',sports_num_next='" + hm.sports_num_next + "',sports_time_next='" + hm.sports_time_next + "',staple_food_now='" + hm.staple_food_now + "',staple_food_next='" + hm.staple_food_next + "',psychological_recovery= '" + hm.psychological_recovery + "',medical_compliance='" + hm.medical_compliance + "',blood_glucose='" + hm.blood_glucose + "',glycosylated_hemoglobin='" + hm.glycosylated_hemoglobin + "',check_date='" + hm.check_date + "',compliance='" + hm.compliance + "',untoward_effect='" + hm.untoward_effect + "',reactive_hypoglycemia='" + hm.reactive_hypoglycemia + "',follow_type='" + hm.follow_type + "',insulin_name= '" + hm.insulin_name + "',insulin_usage='" + hm.insulin_usage + "',transfer_treatment='" + hm.transfer_treatment + "',transfer_treatment_reason='" + hm.transfer_treatment_reason + "',transfer_treatment_department= '" + hm.transfer_treatment_department + "',next_visit_date='" + hm.next_visit_date + "',visit_doctor='" + hm.visit_doctor + "',create_user='" + hm.create_user + "',create_name='" + hm.create_name + "',create_org='" + hm.create_org + "',create_org_name='" + hm.create_org_name + "',create_time='" + hm.create_time + "',update_user='" + hm.update_user + "',update_name= '" + hm.update_name + "',update_time='" + hm.update_time + "',upload_status='" + hm.upload_status + "',upload_time='" + hm.upload_time + "',upload_result='" + hm.upload_result + "',advice= '" + hm.advice + "' where id = '" + id + "'";
                sql = @"update diabetes_follow_record set name='" + hm.name + "',aichive_no='" + hm.aichive_no + "',id_number='" + hm.id_number + "',visit_date= '" + hm.visit_date + "',visit_type='" + hm.visit_type + "',symptom='" + hm.symptom + "',symptom_other='" + hm.symptom_other + "',blood_pressure_high='" + hm.blood_pressure_high + "',blood_pressure_low= '" + hm.blood_pressure_low + "',weight_now='" + hm.weight_now + "',weight_next='" + hm.weight_next + "',bmi_now= '" + hm.bmi_now + "',bmi_next='" + hm.bmi_next + "',dorsal_artery='" + hm.dorsal_artery + "',other='" + hm.other + "',smoke_now= '" + hm.smoke_now + "',smoke_next='" + hm.smoke_next + "',drink_now='" + hm.drink_now + "',drink_next='" + hm.drink_next + "',sports_num_now='" + hm.sports_num_now + "',sports_time_now='" + hm.sports_time_now + "',sports_num_next='" + hm.sports_num_next + "',sports_time_next='" + hm.sports_time_next + "',staple_food_now='" + hm.staple_food_now + "',staple_food_next='" + hm.staple_food_next + "',psychological_recovery= '" + hm.psychological_recovery + "',medical_compliance='" + hm.medical_compliance + "',blood_glucose='" + hm.blood_glucose + "',glycosylated_hemoglobin='" + hm.glycosylated_hemoglobin + "',compliance='" + hm.compliance + "',untoward_effect='" + hm.untoward_effect + "',reactive_hypoglycemia='" + hm.reactive_hypoglycemia + "',follow_type='" + hm.follow_type + "',insulin_name= '" + hm.insulin_name + "',insulin_usage='" + hm.insulin_usage + "',transfer_treatment='" + hm.transfer_treatment + "',transfer_treatment_reason='" + hm.transfer_treatment_reason + "',transfer_treatment_department= '" + hm.transfer_treatment_department + "',next_visit_date='" + hm.next_visit_date + "',visit_doctor='" + hm.visit_doctor + "',create_user='" + hm.create_user + "',create_name='" + hm.create_name + "',create_org='" + hm.create_org + "',create_org_name='" + hm.create_org_name + "',update_user='" + hm.update_user + "',update_name= '" + hm.update_name + "',update_time='" + hm.update_time + "',upload_status='" + hm.upload_status + "',upload_time='" + hm.upload_time + "',upload_result='" + hm.upload_result + "',advice= '" + hm.advice + "' where id = '" + id + "'";
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
        public DataTable queryDiabetesPatient0(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select * from diabetes_follow_record where id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

    }
}

