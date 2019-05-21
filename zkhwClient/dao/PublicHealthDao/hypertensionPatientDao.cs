using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.dao
{
    class hypertensionPatientDao
    {
        public DataTable queryHypertensionPatient(string pCa, string time1, string time2,string code)
        {
            DataSet ds = new DataSet();
            string sql = "SELECT bb.name,bb.archive_no,bb.id_number,(case when aa.visit_type='1' then '门诊' when aa.visit_type='2' then '家庭' when aa.visit_type='3' then '电话' end) visit_type,aa.visit_date,aa.visit_doctor,aa.id FROM (select b.name, b.archive_no, b.id_number from resident_base_info b where 1=1";
            if (code != "") { sql += " AND b.village_code='" + code + "'"; }
            if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
            sql += ") bb LEFT JOIN(select a.id,a.aichive_no,a.visit_type,a.visit_date,a.visit_doctor from fuv_hypertension a where a.visit_date >= '" + time1 + "' and a.visit_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no";
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
                sql = @"insert into fuv_hypertension (id,aichive_no,Codebar,id_number,name,visit_date,visit_type,symptom,other_symptom,sbp,dbp,weight,target_weight,bmi,target_bmi,heart_rate,other_sign,smoken,target_somken,wine,target_wine,sport_week,sport_once,target_sport_week,target_sport_once,salt_intake,target_salt_intake,mind_adjust,doctor_obey,assist_examine,drug_obey,untoward_effect,untoward_effect_drug,visit_class,referral_code,next_visit_date,visit_doctor,advice,create_user,create_name,create_time,create_org,create_org_name,transfer_organ,transfer_reason,upload_status) values ";
                sql += @" ('" + id + "','" + hm.aichive_no + "', '" + hm.Codebar + "', '" + hm.id_number + "', '" + hm.name + "', '" + hm.visit_date + "', '" + hm.visit_type + "','" + hm.symptom + "', '" + hm.other_symptom + "', '" + hm.sbp + "', '" + hm.dbp + "', '" + hm.weight + "', '" + hm.target_weight + "', '" + hm.bmi + "', '" + hm.target_bmi + "', '" + hm.heart_rate + "', '" + hm.other_sign + "','" + hm.smoken + "', '" + hm.target_somken + "', '" + hm.wine + "', '" + hm.target_wine + "', '" + hm.sport_week + "', '" + hm.sport_once + "', '" + hm.target_sport_week + "', '" + hm.target_sport_once + "', '" + hm.salt_intake + "', '" + hm.target_salt_intake + "','" + hm.mind_adjust + "', '" + hm.doctor_obey + "', '" + hm.assist_examine + "', '" + hm.drug_obey + "', '" + hm.untoward_effect + "', '" + hm.untoward_effect_drug + "', '" + hm.visit_class + "', '" + hm.referral_code + "', '" + hm.next_visit_date + "', '" + hm.visit_doctor + "','" + hm.advice + "', '" + frmLogin.userCode + "', '" + frmLogin.name + "', '" + hm.create_time + "', '" + frmLogin.organCode + "', '" + frmLogin.organName + "', '" + hm.transfer_organ + "', '" + hm.transfer_reason + "','0')";

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
            else {
                sql = @"update fuv_hypertension set aichive_no ='" + hm.aichive_no + "',id_number='" + hm.id_number + "',Codebar='" + hm.Codebar + "',name='" + hm.name + "',visit_date='" + hm.visit_date + "',visit_type='" + hm.visit_type + "',symptom='" + hm.symptom + "',other_symptom='" + hm.other_symptom + "',sbp='" + hm.sbp + "',dbp='" + hm.dbp + "',weight='" + hm.weight + "',target_weight='" + hm.target_weight + "',bmi='" + hm.bmi + "',target_bmi='" + hm.target_bmi + "',heart_rate='" + hm.heart_rate + "',other_sign= '" + hm.other_sign + "',smoken='" + hm.smoken + "',target_somken='" + hm.target_somken + "',wine= '" + hm.wine + "',target_wine= '" + hm.target_wine + "',sport_week='" + hm.sport_week + "',sport_once='" + hm.sport_once + "',target_sport_week= '" + hm.target_sport_week + "',target_sport_once='" + hm.target_sport_once + "',salt_intake='" + hm.salt_intake + "',target_salt_intake='" + hm.target_salt_intake + "',mind_adjust='" + hm.mind_adjust + "',doctor_obey= '" + hm.doctor_obey + "',assist_examine='" + hm.assist_examine + "',drug_obey='" + hm.drug_obey + "',untoward_effect='" + hm.untoward_effect + "',untoward_effect_drug='" + hm.untoward_effect_drug + "',visit_class='" + hm.visit_class + "',referral_code='" + hm.referral_code + "',next_visit_date='" + hm.next_visit_date + "',visit_doctor='" + hm.visit_doctor + "',advice='" + hm.advice + "',update_name='" + frmLogin.name + "',update_time='" + hm.update_time + "',transfer_organ='" + hm.transfer_organ + "',transfer_reason='" + hm.transfer_reason + "' where id = '" + id + "'";
                if (goodsList.Rows.Count > 0)
                {
                    sql0 = @"delete from follow_medicine_record  where follow_id = '" + id + "';";
                    for (int i = 0; i < goodsList.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql0 += "insert into follow_medicine_record(follow_id,drug_name,num,dosage,upload_status,create_name,create_time) values ('" + id + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["num"] + "','" + goodsList.Rows[i]["dosage"] + "','0','"+frmLogin.name+ "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

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


