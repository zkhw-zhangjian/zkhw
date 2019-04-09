using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class hypertensionPatientDao
    {
        public DataTable queryHypertensionPatient(string pCa, string time1, string time2)
        {
            DataSet ds = new DataSet();
            string sql = "select id,patientName,patientAge,create_name,visit_date,next_visit_date,dataSate from fuv_hypertension where visit_date >= '" + time1 + "' and visit_date <= '" + time2 + "'";
            if (pCa != "") { sql += " and (patientName like '%" + pCa + "%'  or Cardcode like '%" + pCa + "%'  or aichive_no like '%" + pCa + "%')"; }
            ds = DbHelperMySQL.Query(sql); 
            return ds.Tables[0];
        }
        
        public bool deleteHypertensionPatient(string id)
        {
            int rt = 0;
            string sql = "delete from fuv_hypertension where id='" + id + "';delete from follow_medicine_record  where follow_id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public bool aUfuv_hypertension(bean.fuv_hypertensionBean hm, string id, DataTable goodsList)
        {
            int ret = 0;
            String sql = "";
            String sql0 = "";
            if (id == "") {
                id = Result.GetNewId();
                sql = @"insert into fuv_hypertension (id,aichive_no,Cardcode,Codebar,SocialSecuritycode,patientName,patientAge,dataSate,hypertension_code,visit_date,visit_type,symptom,other_symptom,sbp,dbp,weight,target_weight,bmi,target_bmi,heart_rate,other_sign,smoken,target_somken,wine,target_wine,sport_week,sport_once,target_sport_week,target_sport_once,salt_intake,target_salt_intake,mind_adjust,doctor_obey,assist_examine,drug_obey,untoward_effect,untoward_effect_drug,visit_class,referral_code,next_visit_date,visit_doctor,advice,create_name,create_time,update_name,update_time,transfer_organ,transfer_reason) values ";
                sql += @" ('" + id + "','" + hm.aichive_no + "', '" + hm.Cardcode + "', '" + hm.Codebar + "', '" + hm.SocialSecuritycode + "', '" + hm.patientName + "', '" + hm.patientAge + "', '" + hm.dataSate + "', '" + hm.hypertension_code + "', '" + hm.visit_date + "', '" + hm.visit_type + "','" + hm.symptom + "', '" + hm.other_symptom + "', '" + hm.sbp + "', '" + hm.dbp + "', '" + hm.weight + "', '" + hm.target_weight + "', '" + hm.bmi + "', '" + hm.target_bmi + "', '" + hm.heart_rate + "', '" + hm.other_sign + "','" + hm.smoken + "', '" + hm.target_somken + "', '" + hm.wine + "', '" + hm.target_wine + "', '" + hm.sport_week + "', '" + hm.sport_once + "', '" + hm.target_sport_week + "', '" + hm.target_sport_once + "', '" + hm.salt_intake + "', '" + hm.target_salt_intake + "','" + hm.mind_adjust + "', '" + hm.doctor_obey + "', '" + hm.assist_examine + "', '" + hm.drug_obey + "', '" + hm.untoward_effect + "', '" + hm.untoward_effect_drug + "', '" + hm.visit_class + "', '" + hm.referral_code + "', '" + hm.next_visit_date + "', '" + hm.visit_doctor + "','" + hm.advice + "', '" + hm.create_name + "', '" + hm.create_time + "', '" + hm.update_name + "', '" + hm.update_time + "', '" + hm.transfer_organ + "', '" + hm.transfer_reason + "')";

                if (goodsList.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql0 += "insert into follow_medicine_record(follow_id,drug_name,num,dosage) values ('" + id + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["num"] + "','" + goodsList.Rows[i]["dosage"] + "')";

                        }
                        else
                        {
                            sql0 += ",('" + id + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["num"] + "','" + goodsList.Rows[i]["dosage"] + "')";

                        }
                    }
                }
            }
            else {
                //sql = @"update fuv_hypertension set aichive_no ='" + hm.aichive_no + "',Cardcode='" + hm.Cardcode + "',Codebar='" + hm.Codebar + "',SocialSecuritycode='" + hm.SocialSecuritycode + "',patientName='" + hm.patientName + "',patientAge='" + hm.patientAge + "',dataSate='" + hm.dataSate + "',hypertension_code='" + hm.hypertension_code + "',visit_date='" + hm.visit_date + "',visit_type='" + hm.visit_type + "',symptom='" + hm.symptom + "',other_symptom='" + hm.other_symptom + "',sbp='" + hm.sbp + "',dbp='" + hm.dbp + "',weight='" + hm.weight + "',target_weight='" + hm.target_weight + "',bmi='" + hm.bmi + "',target_bmi='" + hm.target_bmi + "',heart_rate='" + hm.heart_rate + "',other_sign= '" + hm.other_sign + "',smoken='" + hm.smoken + "',target_somken='" + hm.target_somken + "',wine= '" + hm.wine + "',target_wine= '" + hm.target_wine + "',sport_week='" + hm.sport_week + "',sport_once='" + hm.sport_once + "',target_sport_week= '" + hm.target_sport_week + "',target_sport_once='" + hm.target_sport_once + "',salt_intake='" + hm.salt_intake + "',target_salt_intake='" + hm.target_salt_intake + "',mind_adjust='" + hm.mind_adjust + "',doctor_obey= '" + hm.doctor_obey + "',assist_examine='" + hm.assist_examine + "',drug_obey='" + hm.drug_obey + "',untoward_effect='" + hm.untoward_effect + "',untoward_effect_drug='" + hm.untoward_effect_drug + "',visit_class='" + hm.visit_class + "',referral_code='" + hm.referral_code + "',next_visit_date='" + hm.next_visit_date + "',visit_doctor='" + hm.visit_doctor + "',advice='" + hm.advice + "',create_name='" + hm.create_name + "',create_time='" + hm.create_time + "',update_name='" + hm.update_name + "',update_time='" + hm.update_time + "',transfer_organ='" + hm.transfer_organ + "',transfer_reason='" + hm.transfer_reason + "' where id = '" + id + "'";
                sql = @"update fuv_hypertension set aichive_no ='" + hm.aichive_no + "',Cardcode='" + hm.Cardcode + "',Codebar='" + hm.Codebar + "',SocialSecuritycode='" + hm.SocialSecuritycode + "',patientName='" + hm.patientName + "',patientAge='" + hm.patientAge + "',dataSate='" + hm.dataSate + "',hypertension_code='" + hm.hypertension_code + "',visit_date='" + hm.visit_date + "',visit_type='" + hm.visit_type + "',symptom='" + hm.symptom + "',other_symptom='" + hm.other_symptom + "',sbp='" + hm.sbp + "',dbp='" + hm.dbp + "',weight='" + hm.weight + "',target_weight='" + hm.target_weight + "',bmi='" + hm.bmi + "',target_bmi='" + hm.target_bmi + "',heart_rate='" + hm.heart_rate + "',other_sign= '" + hm.other_sign + "',smoken='" + hm.smoken + "',target_somken='" + hm.target_somken + "',wine= '" + hm.wine + "',target_wine= '" + hm.target_wine + "',sport_week='" + hm.sport_week + "',sport_once='" + hm.sport_once + "',target_sport_week= '" + hm.target_sport_week + "',target_sport_once='" + hm.target_sport_once + "',salt_intake='" + hm.salt_intake + "',target_salt_intake='" + hm.target_salt_intake + "',mind_adjust='" + hm.mind_adjust + "',doctor_obey= '" + hm.doctor_obey + "',assist_examine='" + hm.assist_examine + "',drug_obey='" + hm.drug_obey + "',untoward_effect='" + hm.untoward_effect + "',untoward_effect_drug='" + hm.untoward_effect_drug + "',visit_class='" + hm.visit_class + "',referral_code='" + hm.referral_code + "',next_visit_date='" + hm.next_visit_date + "',visit_doctor='" + hm.visit_doctor + "',advice='" + hm.advice + "',create_name='" + hm.create_name + "',update_name='" + hm.update_name + "',update_time='" + hm.update_time + "',transfer_organ='" + hm.transfer_organ + "',transfer_reason='" + hm.transfer_reason + "' where id = '" + id + "'";

                sql0 = @"delete from follow_medicine_record  where follow_id = '" + id + "';";
                if (goodsList.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql0 += "insert into follow_medicine_record(follow_id,drug_name,num,dosage) values ('" + id + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["num"] + "','" + goodsList.Rows[i]["dosage"] + "')";

                        }
                        else
                        {
                            sql0 += ",('" + id + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["num"] + "','" + goodsList.Rows[i]["dosage"] + "')";

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
        public DataTable queryHypertensionPatient0(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select * from fuv_hypertension where id = '" + id + "'";
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


