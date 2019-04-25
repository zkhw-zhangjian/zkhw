using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkhwClient.bean
{

    class physical_examination_recordBean
    {
        public string id { get; set; }
        //姓名
        public string name { get; set; }
        //电子档案编码
        public string aichive_no { get; set; }//标记页面未用
        //身份证号
        public string id_number { get; set; }//
        //检查批次
        public string batch_no { get; set; }//
        //条形码
        public string bar_code { get; set; }
        //症状
        public string symptom { get; set; }//
        //症状其他
        public string symptom_other { get; set; }//
        //检查日期
        public string check_date { get; set; }//
        //责任医生
        public string doctor_name { get; set; }
        //体温
        public string base_temperature { get; set; }
        //脉搏
        public string base_heartbeat { get; set; }
        //呼吸频率
        public string base_respiratory { get; set; }
        //左血压高压
        public string base_blood_pressure_left_high { get; set; }
        //左血压低压
        public string base_blood_pressure_left_low { get; set; }
        //右血压高压
        public string base_blood_pressure_right_high { get; set; }
        //右血压低压
        public string base_blood_pressure_right_low { get; set; }
        //身高
        public string base_height { get; set; }
        //体重
        public string base_weight { get; set; }
        //腰围
        public string base_waist { get; set; }
        //体质指数BMI
        public string base_bmi { get; set; }
        //老年人健康状况自我评估
        public string base_health_estimate { get; set; }
        //老年人生活自理能力自我评估
        public string base_selfcare_estimate { get; set; }
        //老年人认知功能
        public string base_cognition_estimate { get; set; }
        //智力状态得分
        public string base_cognition_score { get; set; }
        //老年人情感状态
        public string base_feeling_estimate { get; set; }
        //老年人抑郁评分
        public string base_feeling_score { get; set; }
        //处理医生
        public string base_doctor { get; set; }
        //锻炼频率
        public string lifeway_exercise_frequency { get; set; }
        //每次锻炼时间
        public string lifeway_exercise_time { get; set; }
        //坚持锻炼时间
        public string lifeway_exercise_year { get; set; }
        //锻炼方式
        public string lifeway_exercise_type { get; set; }
        //饮食习惯
        public string lifeway_diet { get; set; }
        //吸烟状况
        public string lifeway_smoke_status { get; set; }
        //日吸烟量
        public string lifeway_smoke_number { get; set; }
        //开始吸烟年龄
        public string lifeway_smoke_startage { get; set; }
        //戒烟年龄 
        public string lifeway_smoke_endage { get; set; }
        //饮酒频率
        public string lifeway_drink_status { get; set; }
        //日饮酒量
        public string lifeway_drink_number { get; set; }
        //是否戒酒
        public string lifeway_drink_stop { get; set; }
        //戒酒年龄
        public string lifeway_drink_stopage { get; set; }//
        //开始饮酒年龄
        public string lifeway_drink_startage { get; set; }
        //近一年内是否醉酒
        public string lifeway_drink_oneyear { get; set; }
        //饮酒种类
        public string lifeway_drink_type { get; set; }
        //饮酒种类其他
        public string lifeway_drink_other { get; set; }
        //是否有职业病危害接触
        public string lifeway_occupational_disease { get; set; }//
        //工种
        public string lifeway_job { get; set; }//
        //从业时间
        public string lifeway_job_period { get; set; }//
        //毒物粉尘
        public string lifeway_hazardous_dust { get; set; }//
        //粉尘预防措施
        public string lifeway_dust_preventive { get; set; }
        //放射物质
        public string lifeway_hazardous_radiation { get; set; }
        //放射物质预防措施
        public string lifeway_radiation_preventive { get; set; }
        //物理因素
        public string lifeway_hazardous_physical { get; set; }
        //物理因素预防措施
        public string lifeway_physical_preventive { get; set; }//
        //化学物质
        public string lifeway_hazardous_chemical { get; set; }//
        //化学物质预防措施
        public string lifeway_chemical_preventive { get; set; }
        //其他危险种类
        public string lifeway_hazardous_other { get; set; }//
        //其他预防措施
        public string lifeway_other_preventive { get; set; }//
        //处理医生
        public string lifeway_doctor { get; set; }//
        //口唇
        public string organ_lips { get; set; }
        //齿列
        public string organ_tooth { get; set; }
        //缺齿
        public string organ_hypodontia { get; set; }
        //左上缺齿
        public string organ_hypodontia_topleft { get; set; }
        //右上缺齿
        public string organ_hypodontia_topright { get; set; }
        //左下缺齿
        public string organ_hypodontia_bottomleft { get; set; }
        //右下缺齿
        public string organ_hypodontia_bottomright { get; set; }
        //龋齿
        public string organ_caries { get; set; }//
        //左上龋齿
        public string organ_caries_topleft { get; set; }//
        //右上龋齿
        public string organ_caries_topright { get; set; }
        //左下龋齿
        public string organ_caries_bottomleft { get; set; }//
        //右下龋齿
        public string organ_caries_bottomright { get; set; }//
        //假牙
        public string organ_denture { get; set; }//
        //左上假牙
        public string organ_denture_topleft { get; set; }
        //右上假牙
        public string organ_denture_topright { get; set; }
        //左下假牙
        public string organ_denture_bottomleft { get; set; }
        //右下假牙
        public string organ_denture_bottomright { get; set; }
        //咽喉
        public string organ_guttur { get; set; }
        //左眼视力
        public string organ_vision_left { get; set; }
        //右眼视力
        public string organ_vision_right { get; set; }
        //左眼矫正视力
        public string organ_correctedvision_left { get; set; }
        //右眼矫正视力
        public string organ_correctedvision_right { get; set; }
        //听力
        public string organ_hearing { get; set; }
        //运动功能
        public string organ_movement { get; set; }
        //处理医生
        public string organ_doctor { get; set; }
        //眼底
        public string examination_eye { get; set; }
        //皮肤
        public string examination_skin { get; set; }
        //皮肤其他
        public string examination_skin_other { get; set; }
        //巩膜
        public string examination_sclera { get; set; }
        //巩膜其他
        public string examination_sclera_other { get; set; }
        //淋巴结
        public string examination_lymph { get; set; }
        //淋巴结其他
        public string examination_lymph_other { get; set; }
        //桶状胸
        public string examination_barrel_chest { get; set; }
        //呼吸音
        public string examination_breath_sounds { get; set; }
        //罗音
        public string examination_rale { get; set; }
        //罗音其他
        public string examination_rale_other { get; set; }
        //心率
        public string examination_heart_rate { get; set; }
        //心律
        public string examination_heart_rhythm { get; set; }
        //心脏杂音
        public string examination_heart_noise { get; set; }
        //心脏杂音异常说明
        public string examination_noise_other { get; set; }
        //腹部压痛 
        public string examination_abdomen_tenderness { get; set; }
        //腹部压痛异常说明
        public string examination_tenderness_memo { get; set; }
        //腹部包块
        public string examination_abdomen_mass { get; set; }
        //腹部包块异常说明
        public string examination_mass_memo { get; set; }
        //腹部肝大
        public string examination_abdomen_hepatomegaly { get; set; }
        //腹部肝大异常说明
        public string examination_hepatomegaly_memo { get; set; }
        //腹部脾大
        public string examination_abdomen_splenomegaly { get; set; }
        //腹部脾大异常说明
        public string examination_splenomegaly_memo { get; set; }
        //移动性浊音
        public string examination_abdomen_shiftingdullness { get; set; }
        //下肢水肿
        public string examination_lowerextremity_edema { get; set; }
        //足背动脉搏动
        public string examination_dorsal_artery { get; set; }
        //肛门指诊
        public string examination_anus { get; set; }
        //肛门指诊其他
        public string examination_anus_other { get; set; }//
        //乳腺
        public string examination_breast { get; set; }//
        //乳腺其他
        public string examination_breast_other { get; set; }//
        //examination_doctor
        public string examination_doctor { get; set; }//
        //妇科外阴
        public string examination_woman_vulva { get; set; }
        //妇科外阴异常说明
        public string examination_vulva_memo { get; set; }
        //妇科阴道
        public string examination_woman_vagina { get; set; }
        //妇科阴道异常说明
        public string examination_vagina_memo { get; set; }
        //妇科宫颈
        public string examination_woman_cervix { get; set; }
        //妇科宫颈异常说明
        public string examination_cervix_memo { get; set; }
        //妇科宫体
        public string examination_woman_corpus { get; set; }//标记页面未用
        //妇科宫体异常说明
        public string examination_corpus_memo { get; set; }
        //妇科附件
        public string examination_woman_accessories { get; set; }//
        //妇科附件异常说明
        public string examination_accessories_memo { get; set; }
        //妇科检查医生
        public string examination_woman_doctor { get; set; }//
        //查体其他
        public string examination_other { get; set; }
        //血红蛋白
        public string blood_hemoglobin { get; set; }//
        //白细胞
        public string blood_leukocyte { get; set; }//
        //血小板
        public string blood_platelet { get; set; }//
        //血常规其他
        public string blood_other { get; set; }
        //尿蛋白
        public string urine_protein { get; set; }
        //尿糖
        public string glycosuria { get; set; }
        //尿酮体
        public string urine_acetone_bodies { get; set; }
        //尿潜血
        public string bld { get; set; }
        //尿常规其他
        public string urine_other { get; set; }//标记页面未用
        //空腹血糖mmol
        public string blood_glucose_mmol { get; set; }//
        //空腹血糖mg
        public string blood_glucose_mg { get; set; }//
        //心电图
        public string cardiogram { get; set; }
        //心电图异常说明
        public string cardiogram_memo { get; set; }
        //心电图图片
        public string cardiogram_img { get; set; }//
        //尿微量白蛋白
        public string microalbuminuria { get; set; }//
        //大便潜血
        public string fob { get; set; }//
        //糖化血红蛋白
        public string glycosylated_hemoglobin { get; set; }
        //乙型肝炎表面抗原
        public string hb { get; set; }
        //血清谷丙转氨酶
        public string sgft { get; set; }
        //血清谷草转氨酶
        public string ast { get; set; }
        //albumin
        public string albumin { get; set; }
        //总胆红素
        public string total_bilirubin { get; set; }
        //结合胆红素
        public string conjugated_bilirubin { get; set; }
        //血清肌酐
        public string scr { get; set; }
        //血尿素
        public string blood_urea { get; set; }
        //血钾浓度
        public string blood_k { get; set; }
        //血钠浓度
        public string blood_na { get; set; }
        //总胆固醇
        public string tc { get; set; }
        //甘油三酯
        public string tg { get; set; }
        //血清低密度脂蛋白胆固醇
        public string ldl { get; set; }
        //血清高密度脂蛋白胆固醇
        public string hdl { get; set; }
        //胸部X线片
        public string chest_x { get; set; }
        //胸部X线片异常说明
        public string chestx_memo { get; set; }
        //胸部X图片
        public string chestx_img { get; set; }
        //腹部B超
        public string ultrasound_abdomen { get; set; }
        //腹部B超异常说明
        public string ultrasound_memo { get; set; }
        //腹部b超图片
        public string abdomenB_img { get; set; }
        //其他B超
        public string other_b { get; set; }
        //其他B超异常说明
        public string otherb_memo { get; set; }
        //其他B超图片
        public string otherb_img { get; set; }
        //宫颈涂片
        public string cervical_smear { get; set; }
        //宫颈涂片异常说明
        public string cervical_smear_memo { get; set; }
        //其他
        public string other { get; set; }
        //脑血管疾病
        public string cerebrovascular_disease { get; set; }
        //其他脑血管疾病
        public string cerebrovascular_disease_other { get; set; }
        //肾脏疾病
        public string kidney_disease { get; set; }
        //肾脏疾病其他 
        public string kidney_disease_other { get; set; }
        //心脏疾病
        public string heart_disease { get; set; }
        //其他心脏疾病
        public string heart_disease_other { get; set; }
        //血管疾病
        public string vascular_disease { get; set; }
        //其他血管疾病
        public string vascular_disease_other { get; set; }//
        //眼部疾病
        public string ocular_diseases { get; set; }
        //其他眼部疾病
        public string ocular_diseases_other { get; set; }
        //神经系统疾病
        public string nervous_system_disease { get; set; }
        //其他系统疾病
        public string other_disease { get; set; }//
        //健康评价
        public string health_evaluation { get; set; }//
        //健康指导
        public string health_guidance { get; set; }//
        //危险因素控制编号(多个)
        public string danger_controlling { get; set; }//
        //目标体重
        public string target_weight { get; set; }
        //其他危险因素控制
        public string danger_controlling_other { get; set; }
        //create_user
        public string create_user { get; set; }
        //create_name
        public string create_name { get; set; }//标记页面未用
        //create_org
        public string create_org { get; set; }//
        //create_org_name
        public string create_org_name { get; set; }//
        //create_time
        public string create_time { get; set; }
        //update_user
        public string update_user { get; set; }//
        //update_name
        public string update_name { get; set; }//
        //update_time
        public string update_time { get; set; }//
        //upload_status
        public string upload_status { get; set; }
        //upload_time
        public string upload_time { get; set; }
        //upload_result
        public string upload_result { get; set; }
    }
}
//id,name,aichive_no,id_number,batch_no,bar_code,symptom,symptom_other,check_date,base_temperature,base_heartbeat,base_respiratory,base_blood_pressure_left_high,base_blood_pressure_left_low,base_blood_pressure_right_high,base_blood_pressure_right_low,base_height,base_weight,base_waist,base_bmi,base_health_estimate,base_selfcare_estimate,base_cognition_estimate,base_cognition_score,base_feeling_estimate,base_feeling_score,base_doctor,lifeway_exercise_frequency,lifeway_exercise_time,lifeway_exercise_year,lifeway_exercise_type,lifeway_diet,lifeway_smoke_status,lifeway_smoke_number,lifeway_smoke_startage,lifeway_smoke_endage,lifeway_drink_status,lifeway_drink_number,lifeway_drink_stop,lifeway_drink_stopage,lifeway_drink_startage,lifeway_drink_oneyear,lifeway_drink_type,lifeway_occupational_disease,lifeway_job,lifeway_job_period,lifeway_hazardous_dust,lifeway_dust_preventive,lifeway_hazardous_radiation,lifeway_radiation_preventive,lifeway_hazardous_physical,lifeway_physical_preventive,lifeway_hazardous_chemical,lifeway_chemical_preventive,lifeway_hazardous_other,lifeway_other_preventive,lifeway_doctor,organ_lips,organ_tooth,organ_hypodontia,organ_hypodontia_topleft,organ_hypodontia_topright,organ_hypodontia_bottomleft,organ_hypodontia_bottomright,organ_caries,organ_caries_topleft,organ_caries_topright,organ_caries_bottomleft,organ_caries_bottomright,organ_denture,organ_denture_topleft,organ_denture_topright,organ_denture_bottomleft,organ_denture_bottomright,organ_guttur,organ_vision_left,organ_vision_right,organ_correctedvision_left,organ_correctedvision_right,organ_hearing,organ_movement,organ_doctor,examination_eye,examination_skin,examination_skin_other,examination_sclera,examination_sclera_other,examination_lymph,examination_lymph_other,examination_barrel_chest,examination_breath_sounds,examination_rale,examination_rale_other,examination_heart_rate,examination_heart_rhythm,examination_heart_noise,examination_abdomen_tenderness,examination_abdomen_mass,examination_abdomen_hepatomegaly,examination_abdomen_splenomegaly,examination_abdomen_shiftingdullness,examination_lowerextremity_edema,examination_dorsal_artery,examination_anus,examination_anus_other,examination_breast,examination_breast_other,examination_doctor,examination_woman_vulva,examination_woman_vagina,examination_woman_cervix,examination_woman_corpus,examination_woman_accessories,examination_woman_doctor,examination_other,blood_hemoglobin,blood_leukocyte,blood_platelet,blood_other,urine_protein,glycosuria,urine_acetone_bodies,bld,urine_other,blood_glucose_mmol,blood_glucose_mg,cardiogram,cardiogram_img,microalbuminuria,fob,glycosylated_hemoglobin,hb,sgft,ast,albumin,total_bilirubin,conjugated_bilirubin,scr,blood_urea,blood_k,blood_na,tc,tg,ldl,hdl,chest_x,chestx_img,ultrasound_abdomen,abdomenB_img,other_b,otherb_img,cervical_smear,other,cerebrovascular_disease,cerebrovascular_disease_other,kidney_disease,kidney_disease_other,heart_disease,heart_disease_other,vascular_disease,vascular_disease_other,ocular_diseases,ocular_diseases_other,nervous_system_disease,other_disease,health_evaluation,health_guidance,danger_controlling,target_weight,danger_controlling_other,create_user,create_name,create_org,create_org_name,create_time,update_user,update_name,update_time,upload_status,upload_time,upload_result

