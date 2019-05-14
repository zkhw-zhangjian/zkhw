using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkhwClient.bean
{
    class psychosis_follow_recordBean
    {


        public string id { get; set; }
        //姓名
        public string name { get; set; }
        //电子档案编号
        public string archive_no { get; set; }
        //身份证号
        public string Cardcode { get; set; }
        //访问日期
        public string visit_date { get; set; }
        //访问类型
        public string visit_type { get; set; }
        //失访原因
        public string miss_reason { get; set; }
        //失访原因其他
        public string miss_reason_other { get; set; }
        //死亡日期
        public string die_date { get; set; }
        //死亡原因
        public string die_reason { get; set; }
        //躯体疾病
        public string physical_disease { get; set; }
        //死亡原因其他

        public string die_reason_other { get; set; }
        //危险性评估
        public string fatalness { get; set; }

        //症状
        public string symptom { get; set; }
        //症状其他
        public string symptom_other { get; set; }
        //自知力
        public string insight { get; set; }
        //睡眠状况
        public string sleep_status { get; set; }
        // 饮食状况
        public string dietary_status { get; set; }
        //个人生活自理
        public string self_help { get; set; }
        //家务劳动
        public string housework { get; set; }
        //生产劳动及工作
        public string work { get; set; }
        //学习能力
        public string learning_ability { get; set; }
        //社会人际交往
        public string interpersonal { get; set; }
        //危险行为
        public string dangerous_act { get; set; }
        //轻度滋事次数
        public string slight_trouble_num { get; set; }
        //肇事次数
        public string cause_trouble_num { get; set; }
        //肇祸次数
        public string cause_accident_num { get; set; }
        //其他危害行为次数
        public string harm_other_num { get; set; }
        //自伤次数
        public string autolesion_num { get; set; }
        //自杀未遂次数
        public string attempted_suicide_num { get; set; }
        //两次随访期间关锁情况
        public string isolation { get; set; }
        //两次随访期间住院情况
        public string hospitalized_status { get; set; }
        //末次出院日期
        public string out_hospital_date { get; set; }
        //实验室检查
        public string laboratory_examination { get; set; }
        //用药依从性
        public string compliance { get; set; }
        //用药不良反应
        public string untoward_effect { get; set; }
        //不良反应信息
        public string untoward_effect_info { get; set; }
        //治疗效果
        public string treatment_effect { get; set; }
        //是否转诊
        public string transfer_treatment { get; set; }
        //转诊原因
        public string transfer_treatment_reason { get; set; }
        //转诊机构和科别
        public string transfer_treatment_department { get; set; }
        //康复措施
        public string rehabilitation_measure { get; set; }
        //康复措施其他
        public string rehabilitation_measure_other { get; set; }
        //下次随访分类
        public string next_visit_classify { get; set; }
        //下次随访日期
        public string next_visit_date { get; set; }
        //visit_doctor
        public string visit_doctor { get; set; }
        //create_user
        public string create_user { get; set; }
        //create_name
        public string create_name { get; set; }
        //create_org
        public string create_org { get; set; }
        //create_org_name
        public string create_org_name { get; set; }
        //create_time
        public string create_time { get; set; }
        //update_user
        public string update_user { get; set; }
        //update_name
        public string update_name { get; set; }
        //update_time
        public string update_time { get; set; }
        //upload_status
        public string upload_status { get; set; }
        //upload_time
        public string upload_time { get; set; }
        //upload_result
        public string upload_result { get; set; }
    }
}
//id,name,aichive_no,Cardcode,visit_date,visit_type,miss_reason,miss_reason_other,die_date,die_reason,physical_disease,die_reason_other,fatalness,symptom,symptom_other,insight,sleep_status,dietary_status,self_help,housework,work,learning_ability,interpersonal,dangerous_act,slight_trouble_num,cause_trouble_num,cause_accident_num,harm_other_num,autolesion_num,attempted_suicide_num,isolation,hospitalized_status,out_hospital_date,laboratory_examination,compliance,untoward_effect,untoward_effect_info,treatment_effect,transfer_treatment,transfer_treatment_reason,transfer_treatment_department,rehabilitation_measure,rehabilitation_measure_other,next_visit_classify,next_visit_date,visit_doctor,create_user,create_name,create_org,create_org_name,create_time,update_user,update_name,update_time,upload_status,upload_time,upload_result
