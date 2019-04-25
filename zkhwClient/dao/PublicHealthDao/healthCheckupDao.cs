using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.bean;

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
        public DataTable queryhealthCheckup(string pCa, string time1, string time2,string code)
        {
            DataSet ds = new DataSet();
            string sql = "select a.aichive_no,a.id_number,a.bar_code,a.name,a.check_date,a.doctor_name from physical_examination_record a,resident_base_info b where a.aichive_no = b.archive_no and a.check_date >= '" + time1 + "' and a.check_date <= '" + time2 + "'";
            if (code != "") { sql += " AND b.village_code='" + code + "'"; }
            if (pCa != "") { sql += " and (a.name like '%" + pCa + "%'  or a.id_number like '%" + pCa + "%'  or a.aichive_no like '%" + pCa + "%')"; }
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

        public bool addPhysicalExaminationRecord1(physical_examination_recordBean per)
        {
            int ret = 0;
            String sql = @"update physical_examination_record set symptom='" + per.symptom + "',symptom_other='" + per.symptom_other + "',base_temperature='" 
                +per.base_temperature + "',base_heartbeat='" + per.base_heartbeat + "',base_respiratory='" + per.base_respiratory + "',base_blood_pressure_left_high='" 
                + per.base_blood_pressure_left_high + "',base_blood_pressure_left_low='" + per.base_blood_pressure_left_low + "',base_blood_pressure_right_high='" 
                + per.base_blood_pressure_right_high + "',base_blood_pressure_right_low= '" + per.base_blood_pressure_right_low + "',base_height='" 
                + per.base_height + "',base_weight='" + per.base_weight + "',base_waist='" + per.base_waist + "',base_bmi= '" + per.base_bmi 
                + "',base_health_estimate= '" + per.base_health_estimate + "',base_selfcare_estimate= '" + per.base_selfcare_estimate + "',base_cognition_estimate='" 
                + per.base_cognition_estimate + "',base_cognition_score= '" + per.base_cognition_score + "',base_feeling_estimate='" + per.base_feeling_estimate 
                + "',base_feeling_score='" + per.base_feeling_score + "',base_doctor='" + per.base_doctor + "',lifeway_exercise_frequency='" + per.lifeway_exercise_frequency
                + "',lifeway_exercise_time='" + per.lifeway_exercise_time + "',lifeway_exercise_year='" + per.lifeway_exercise_year + "',lifeway_exercise_type='" + per.lifeway_exercise_type 
                + "',lifeway_diet='" + per.lifeway_diet + "',lifeway_smoke_status='" + per.lifeway_smoke_status + "',lifeway_smoke_number='" + per.lifeway_smoke_number 
                + "',lifeway_smoke_startage='" + per.lifeway_smoke_startage + "',lifeway_smoke_endage='" + per.lifeway_smoke_endage + "',lifeway_drink_status='" + per.lifeway_drink_status 
                + "',lifeway_drink_number='" + per.lifeway_drink_number + "',lifeway_drink_stop='" + per.lifeway_drink_stop + "',lifeway_drink_stopage='" + per.lifeway_drink_stopage 
                + "',lifeway_drink_startage='" + per.lifeway_drink_startage + "',lifeway_drink_oneyear='" + per.lifeway_drink_oneyear + "',lifeway_drink_type='" + per.lifeway_drink_type 
                + "',lifeway_drink_other='" + per.lifeway_drink_other + "',lifeway_occupational_disease='" + per.lifeway_occupational_disease + "',lifeway_job='" + per.lifeway_job 
                + "',lifeway_job_period='" + per.lifeway_job_period + "',lifeway_hazardous_dust='" + per.lifeway_hazardous_dust + "',lifeway_dust_preventive='" + per.lifeway_dust_preventive 
                + "',lifeway_hazardous_radiation='" + per.lifeway_hazardous_radiation + "',lifeway_radiation_preventive='" + per.lifeway_radiation_preventive + "',lifeway_hazardous_physical='" 
                + per.lifeway_hazardous_physical + "',lifeway_physical_preventive='" + per.lifeway_physical_preventive + "',lifeway_hazardous_chemical='" + per.lifeway_hazardous_chemical 
                + "',lifeway_chemical_preventive='" + per.lifeway_chemical_preventive + "',lifeway_hazardous_other='" + per.lifeway_hazardous_other + "',lifeway_other_preventive='" + per.lifeway_other_preventive
                + "',lifeway_doctor='" + per.lifeway_doctor + "' where aichive_no = '" + per.aichive_no + "' and bar_code = '" + per.bar_code + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
//organ_lips
//organ_tooth
//organ_hypodontia
//organ_hypodontia_topleft
//organ_hypodontia_topright
//organ_hypodontia_bottomleft
//organ_hypodontia_bottomright
//organ_caries
//organ_caries_topleft
//organ_caries_topright
//organ_caries_bottomleft
//organ_caries_bottomright
//organ_denture
//organ_denture_topleft
//organ_denture_topright
//organ_denture_bottomleft
//organ_denture_bottomright
//organ_guttur
//organ_vision_left
//organ_vision_right
//organ_correctedvision_left
//organ_correctedvision_right
//organ_hearing
//organ_movement
//organ_doctor
//examination_eye
//examination_eye_other
//examination_skin
//examination_skin_other
//examination_sclera
//examination_sclera_other
//examination_lymph
//examination_lymph_other
//examination_barrel_chest
//examination_breath_sounds
//examination_rale
//examination_rale_other
//examination_heart_rate
//examination_heart_rhythm
//examination_heart_noise
//examination_noise_other
//examination_abdomen_tenderness
//examination_tenderness_memo
//examination_abdomen_mass
//examination_mass_memo
//examination_abdomen_hepatomegaly
//examination_hepatomegaly_memo
//examination_abdomen_splenomegaly
//examination_splenomegaly_memo
//examination_abdomen_shiftingdullness
//examination_lowerextremity_edema
//examination_dorsal_artery
//examination_anus
//examination_anus_other
//examination_breast
//examination_breast_other
//examination_doctor
//examination_woman_vulva
//examination_vulva_memo
//examination_woman_vagina
//examination_vagina_memo
//examination_woman_cervix
//examination_cervix_memo
//examination_woman_corpus
//examination_corpus_memo
//examination_woman_accessories
//examination_accessories_memo
//examination_woman_doctor
//examination_other
//blood_hemoglobin
//blood_leukocyte
//blood_platelet
//blood_other
//urine_protein
//glycosuria
//urine_acetone_bodies
//bld
//urine_other
//blood_glucose_mmol
//blood_glucose_mg
//cardiogram
//cardiogram_memo
//cardiogram_img
//microalbuminuria
//fob
//glycosylated_hemoglobin
//hb
//sgft
//ast
//albumin
//total_bilirubin
//conjugated_bilirubin
//scr
//blood_urea
//blood_k
//blood_na
//tc
//tg
//ldl
//hdl
//chest_x
//chestx_memo
//chestx_img
//ultrasound_abdomen
//ultrasound_memo
//abdomenB_img
//other_b
//otherb_memo
//otherb_img
//cervical_smear
//cervical_smear_memo
//other
//cerebrovascular_disease
//cerebrovascular_disease_other
//kidney_disease
//kidney_disease_other
//heart_disease
//heart_disease_other
//vascular_disease
//vascular_disease_other
//ocular_diseases
//ocular_diseases_other
//nervous_system_disease
//other_disease
//health_evaluation
//abnormal1
//abnormal2
//abnormal3
//abnormal4
//health_guidance
//danger_controlling
//target_weight
//proposal_accination
//danger_controlling_other
//create_user
//create_name
//create_org
//create_org_name
//create_time
//update_user
//update_name
//update_time
//upload_status
//upload_time
//upload_result

        }
    }
}
