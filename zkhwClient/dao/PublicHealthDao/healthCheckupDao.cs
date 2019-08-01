using System;
using System.Data;
using System.Windows.Forms;
using zkhwClient.bean;

namespace zkhwClient.dao
{
    class healthCheckupDao
    {
        public DataTable GetExaminationRecordList(string _archiveno, string _idnumber, string _barcode)
        {
            DataSet ds = new DataSet();
            string sql = string.Format(@"select * from physical_examination_record 
                      where aichive_no='{0}' and id_number='{1}' and bar_code='{2}' LIMIT 1  ",
                     _archiveno, _idnumber, _barcode);
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public string GetExaminationRecord(string _archiveno, string _idnumber, string _barcode)
        {
            string id = "";
            string sql = string.Format(@"select id from physical_examination_record 
                      where aichive_no='{0}' and id_number='{1}' and bar_code='{2}' LIMIT 1  ", 
                      _archiveno, _idnumber, _barcode);
            object o = DbHelperMySQL.GetSingle(sql);
            if(o!=null)
            {
                id = o.ToString();
            }

            return id; 
        }

        public bool deletePhysical_examination_record(string id)
        {
            int rt = 0;
            string sql = "delete from physical_examination_record  where id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //删除疫苗信息
        public bool deleteVaccination_record(string id) {
            int rt = 0;
            string sql = "delete from vaccination_record  where exam_id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //删除用药信息
        public bool deleteTake_medicine_record(string id)
        {
            int rt = 0;
            string sql = "delete from take_medicine_record  where exam_id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //删除住院史信息
        public bool deleteHospitalized_record(string id)
        {
            int rt = 0;
            string sql = "delete from hospitalized_record  where exam_id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public DataTable queryhealthCheckup(string pCa, string time1, string time2,string code)
        {
            DataSet ds = new DataSet();
            string sql = "SELECT bb.name,bb.archive_no,bb.id_number,aa.bar_code,aa.check_date,aa.doctor_name,aa.id FROM (select b.name, b.archive_no, b.id_number from resident_base_info b where 1=1";
            //if (code != "") { sql += " AND b.village_code='" + code + "'"; }
            if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
            sql += ") bb JOIN (select a.id,a.aichive_no,a.id_number,a.bar_code,a.check_date,a.doctor_name from physical_examination_record a where a.check_date >= '" + time1 + "' and a.check_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no and bb.id_number=aa.id_number";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable queryhealthCheckupUploadstatus(string pCa, string time1, string time2, string code)
        {
            DataSet ds = new DataSet();
            string sql = "SELECT bb.name,bb.archive_no,bb.id_number,aa.bar_code,aa.check_date,aa.doctor_name,aa.id,(case aa.upload_status when '1' then '是' ELSE '否' END) cstatus FROM (select b.name, b.archive_no, b.id_number from resident_base_info b where 1=1";
            //if (code != "") { sql += " AND b.village_code='" + code + "'"; }
            if (pCa != "") { sql += " AND (b.name like '%" + pCa + "%' or b.id_number like '%" + pCa + "%'  or b.archive_no like '%" + pCa + "%')"; }
            sql += ") bb JOIN (select a.id,a.aichive_no,a.id_number,a.bar_code,a.check_date,a.doctor_name,a.upload_status from physical_examination_record a where a.check_date >= '" + time1 + "' and a.check_date <= '" + time2 + "') aa on bb.archive_no = aa.aichive_no and bb.id_number=aa.id_number";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        //祖
        public DataTable queryhealthCheckup(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select * from physical_examination_record where id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable queryhealthCheckupAndAge(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select p.*,r.age from physical_examination_record p INNER JOIN resident_base_info r on p.id_number=r.id_number  where p.id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable queryhealthCheckupByid(string idnumber)
        {
            DataSet ds = new DataSet();
            string sql = "select base_waist,base_bmi from physical_examination_record where id_number = '" + idnumber + "' order by create_time desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //根据档案号查询老年人生活自理能力评估的分数
        public DataTable queryelderlySelfcareEstimate(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select id,total_score from elderly_selfcare_estimate where aichive_no= '" + id + "' order by create_time desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable queryelderlySelfcareEstimateForExamID(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select id,total_score from elderly_selfcare_estimate where exam_id = '" + id + "' order by create_time desc limit 1";
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
        //添加健康体检表  第一页
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
                + "',lifeway_doctor='" + per.lifeway_doctor + "',upload_status =0 where id = '" + per.id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "/log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\r\n" + sql);
            }
            return ret == 0 ? false : true;
        }
        //添加健康体检表  第二页
        public bool addPhysicalExaminationRecord2(physical_examination_recordBean per)
        {
            int ret = 0;
            String sql = @"update physical_examination_record set organ_lips='" + per.organ_lips + "',organ_tooth='" + per.organ_tooth + "',organ_hypodontia='"
                + per.organ_hypodontia + "',organ_hypodontia_topleft='" + per.organ_hypodontia_topleft + "',organ_hypodontia_topright='" + per.organ_hypodontia_topright + "',organ_hypodontia_bottomleft='" + per.organ_hypodontia_bottomleft + "',organ_hypodontia_bottomright='" + per.organ_hypodontia_bottomright 
                + "',organ_caries='" + per.organ_caries + "',organ_caries_topleft='" + per.organ_caries_topleft + "',organ_caries_topright='" + per.organ_caries_topright + "',organ_caries_bottomleft='" + per.organ_caries_bottomleft + "',organ_caries_bottomright='" + per.organ_caries_bottomright 
                + "',organ_denture='" + per.organ_denture + "',organ_denture_topleft='" + per.organ_denture_topleft + "',organ_denture_topright='" + per.organ_denture_topright + "',organ_denture_bottomleft='" + per.organ_denture_bottomleft + "',organ_denture_bottomright='" + per.organ_denture_bottomright 
                + "',organ_guttur='"+ per.organ_guttur + "',organ_vision_left='"+ per.organ_vision_left + "',organ_vision_right='"
                + per.organ_vision_right + "',organ_correctedvision_left= '" + per.organ_correctedvision_left + "',organ_correctedvision_right='"
                + per.organ_correctedvision_right + "',organ_hearing='" + per.organ_hearing + "',organ_movement='" + per.organ_movement + "',examination_eye= '" + per.examination_eye
                + "',examination_eye_other= '" + per.examination_eye_other + "',examination_skin= '" + per.examination_skin + "',examination_skin_other='"
                + per.examination_skin_other + "',examination_sclera= '" + per.examination_sclera + "',examination_sclera_other='" + per.examination_sclera_other
                + "',examination_lymph='" + per.examination_lymph + "',examination_lymph_other='" + per.examination_lymph_other + "',examination_barrel_chest='" + per.examination_barrel_chest
                + "',examination_breath_sounds='" + per.examination_breath_sounds + "',examination_breath_other='" + per.examination_breath_other + "',examination_rale='" + per.examination_rale
                + "',examination_rale_other='" + per.examination_rale_other + "',examination_heart_rate='" + per.examination_heart_rate + "',examination_heart_rhythm='" + per.examination_heart_rhythm
                + "',examination_heart_noise='" + per.examination_heart_noise + "',examination_noise_other='" + per.examination_noise_other + "',examination_abdomen_tenderness='" + per.examination_abdomen_tenderness
                + "',examination_tenderness_memo='" + per.examination_tenderness_memo + "',examination_abdomen_mass='" + per.examination_abdomen_mass + "',examination_mass_memo='" + per.examination_mass_memo
                + "',examination_abdomen_hepatomegaly='" + per.examination_abdomen_hepatomegaly + "',examination_hepatomegaly_memo='" + per.examination_hepatomegaly_memo + "',examination_abdomen_splenomegaly='" + per.examination_abdomen_splenomegaly
                + "',examination_splenomegaly_memo='" + per.examination_splenomegaly_memo + "',examination_abdomen_shiftingdullness='" + per.examination_abdomen_shiftingdullness + "',examination_shiftingdullness_memo='" + per.examination_shiftingdullness_memo
                + "',examination_lowerextremity_edema='" + per.examination_lowerextremity_edema + "',examination_dorsal_artery='" + per.examination_dorsal_artery + "',examination_anus='" + per.examination_anus
                + "',examination_anus_other='" + per.examination_anus_other + "',examination_breast='" + per.examination_breast + "',examination_breast_other='"
                + per.examination_breast_other + "',examination_woman_vulva='" + per.examination_woman_vulva + "',examination_vulva_memo='" + per.examination_vulva_memo
                + "',examination_woman_vagina='" + per.examination_woman_vagina + "',examination_vagina_memo='" + per.examination_vagina_memo + "',examination_woman_cervix='" + per.examination_woman_cervix
                + "',examination_cervix_memo='" + per.examination_cervix_memo + "',examination_woman_corpus='" + per.examination_woman_corpus + "',examination_corpus_memo='" + per.examination_corpus_memo + "',examination_woman_accessories='" + per.examination_woman_accessories
                + "',examination_accessories_memo='" + per.examination_accessories_memo + "',examination_other='" + per.examination_other + "',blood_hemoglobin ='" + per.blood_hemoglobin + "',blood_leukocyte='" + per.blood_leukocyte + "',blood_platelet='" + per.blood_platelet + "',blood_other='" + per.blood_other + "',urine_protein='" + 
                per.urine_protein + "',glycosuria='" + per.glycosuria + "',urine_acetone_bodies='" + per.urine_acetone_bodies + "',bld='" + per.bld + "',urine_other='" + per.urine_other + "',blood_glucose_mmol='" + per.blood_glucose_mmol + "' ,blood_glucose_mg='" + per.blood_glucose_mg + "',cardiogram='" + per.cardiogram 
                + "',cardiogram_memo='" + per.cardiogram_memo + "',upload_status=0  where id = '" + per.id + "'"; 
                ret = DbHelperMySQL.ExecuteSql(sql);
                return ret == 0 ? false : true;
        }
        //添加健康体检表  第三页
        public bool addPhysicalExaminationRecord3(physical_examination_recordBean per)
        {
            int ret = 0;
            String sql = @"update physical_examination_record set microalbuminuria='" + per.microalbuminuria + "',fob='" + per.fob + "',glycosylated_hemoglobin='"
                + per.glycosylated_hemoglobin + "',hb='" + per.hb + "',sgft='" + per.sgft + "',ast='"
                + per.ast + "',albumin='" + per.albumin + "',total_bilirubin='"
                + per.total_bilirubin + "',conjugated_bilirubin= '" + per.conjugated_bilirubin + "',scr='"
                + per.scr + "',blood_urea='" + per.blood_urea + "',blood_k='" + per.blood_k + "',blood_na= '" + per.blood_na
                + "',tc= '" + per.tc + "',tg= '" + per.tg + "',ldl='"+ per.ldl + "',hdl= '" + per.hdl + "',chest_x='" + per.chest_x
                + "',chestx_memo='" + per.chestx_memo + "',ultrasound_abdomen='" + per.ultrasound_abdomen + "',ultrasound_memo='" + per.ultrasound_memo
                + "',other_b='" + per.other_b + "',otherb_memo='" + per.otherb_memo + "',cervical_smear='" + per.cervical_smear
                + "',cervical_smear_memo='" + per.cervical_smear_memo + "',other='" + per.other + "',cerebrovascular_disease='" + per.cerebrovascular_disease
                + "',cerebrovascular_disease_other='" + per.cerebrovascular_disease_other + "',kidney_disease='" + per.kidney_disease + "',kidney_disease_other='" + per.kidney_disease_other
                + "',heart_disease='" + per.heart_disease + "',heart_disease_other ='" + per.heart_disease_other + "',vascular_disease='" + per.vascular_disease + "',vascular_disease_other='" + per.vascular_disease_other
                + "',ocular_diseases='" + per.ocular_diseases + "',ocular_diseases_other='" + per.ocular_diseases_other + "',nervous_system_disease='" + per.nervous_system_disease
                + "',nervous_disease_memo='" + per.nervous_disease_memo + "',other_disease='" + per.other_disease + "',other_disease_memo='" + per.other_disease_memo + "',upload_status=0 where id = '" + per.id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //添加健康体检表  第三页 添加住院史
        public bool addHospitalizedRecord(hospitalizedRecord hr,string hospitalized_type)
        {
            string sql1 = "delete from hospitalized_record  where exam_id = '" + hr.exam_id + "' and hospitalized_type='" + hospitalized_type + "';";
            DbHelperMySQL.ExecuteSql(sql1);
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string id = Result.GetNewId();
            string sql = @"insert into hospitalized_record (id,exam_id,archive_no,id_number,hospitalized_type,in_hospital_time,leave_hospital_time,reason,hospital_organ,case_code,create_org,create_name,create_time) values ('" +id + "','" + hr.exam_id + "','" + hr.archive_no + "', '"+hr.id_number+"', '"+hr.hospitalized_type + "', '" + hr.in_hospital_time + "', '" + hr.leave_hospital_time + "', '" + hr.reason + "', '" + hr.hospital_organ + "', '" + hr.case_code + "', '" + frmLogin.userCode + "', '" + frmLogin.name + "', '" + time + "')";
            int ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //祖
        public DataTable queryHospitalizedRecord(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select * from hospitalized_record where exam_id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //健康体检表第四页  加载用药记录信息
        public DataTable queryTake_medicine_record(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select medicine_name drug_name,medicine_usage drug_usage,medicine_dosage drug_use,medicine_time drug_time,medicine_compliance drug_type from take_medicine_record where exam_id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //健康体检表第四页  加载疫苗记录信息
        public DataTable queryVaccination_record(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select vaccination_name,vaccination_time,vaccination_organ_name from vaccination_record where exam_id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //添加健康体检表  第四页
        public bool addPhysicalExaminationRecord4(physical_examination_recordBean per,DataTable goodsList, DataTable goodsListym)
        {
            int ret = 0;    
            String sql = @"update physical_examination_record set health_evaluation='" + per.health_evaluation + "',abnormal1='" + per.abnormal1 + "',abnormal2='"
                + per.abnormal2 + "',abnormal3='" + per.abnormal3 + "',abnormal4='" + per.abnormal4 + "',health_guidance='"
                + per.health_guidance + "',danger_controlling='" + per.danger_controlling + "',target_weight='"
                + per.target_weight + "',advise_bacterin= '" + per.advise_bacterin + "',danger_controlling_other='"
                + per.danger_controlling_other + "',healthAdvice='" + per.healthAdvice + "',create_user='" + frmLogin.userCode + "',create_name='" + frmLogin.name + "',create_org='" + frmLogin.organCode + "',create_org_name='" + frmLogin.organName + "',update_user='" + frmLogin.userCode + "',update_name='" + frmLogin.name + "',update_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',upload_status=0 where id = '" + per.id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            if (ret>0) {
                if (goodsList.Rows.Count > 0) {
                    string sql1 = "delete from take_medicine_record  where exam_id = '" + per.id + "';";
                    DbHelperMySQL.ExecuteSql(sql1);
                    string sql0 = "";
                    for (int i = 0; i < goodsList.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql0 += "insert into take_medicine_record(id,exam_id,archive_no,id_number,medicine_name,medicine_usage,medicine_dosage,medicine_time,medicine_compliance,create_org,create_name,create_time) values ('" + Result.GetNewId() + "','" + per.id + "','" + per.aichive_no + "','" + per.id_number + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["drug_usage"] + "','" + goodsList.Rows[i]["drug_use"] + "','" + goodsList.Rows[i]["drug_time"] + "','" + goodsList.Rows[i]["drug_type"] + "','" + frmLogin.userCode + "','" + frmLogin.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        }
                        else
                        {
                            sql0 += ",('" + Result.GetNewId() + "','" + per.id + "','" + per.aichive_no + "','" + per.id_number + "','" + goodsList.Rows[i]["drug_name"] + "','" + goodsList.Rows[i]["drug_usage"] + "','" + goodsList.Rows[i]["drug_use"] + "','" + goodsList.Rows[i]["drug_time"] + "','" + goodsList.Rows[i]["drug_type"] + "','" + frmLogin.userCode + "','" + frmLogin.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        }
                    }
                    DbHelperMySQL.ExecuteSql(sql0);
                }
                if (goodsListym.Rows.Count > 0)
                {
                    string sql1 = "delete from vaccination_record  where exam_id = '" + per.id + "';";
                    DbHelperMySQL.ExecuteSql(sql1);
                    string sql0 = "";
                    for (int i = 0; i < goodsListym.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql0 += "insert into vaccination_record(id,exam_id,archive_no,id_number,vaccination_name,vaccination_time,vaccination_organ_name,create_name,create_time) values ('" + Result.GetNewId() + "','" + per.id + "','" + per.aichive_no + "','" + per.id_number + "','" + goodsListym.Rows[i]["vaccination_name"] + "','" + goodsListym.Rows[i]["vaccination_time"] + "','" + goodsListym.Rows[i]["vaccination_organ_name"] + "','" + frmLogin.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        }
                        else
                        {
                            sql0 += ",('" + Result.GetNewId() + "','" + per.id + "','" + per.aichive_no + "','" + per.id_number + "','" + goodsListym.Rows[i]["vaccination_name"] + "','" + goodsListym.Rows[i]["vaccination_time"] + "','" + goodsListym.Rows[i]["vaccination_organ_name"] + "','" + frmLogin.name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        }
                    }
                    DbHelperMySQL.ExecuteSql(sql0);
                }
            }
            return ret == 0 ? false : true;
        }
    }
}
