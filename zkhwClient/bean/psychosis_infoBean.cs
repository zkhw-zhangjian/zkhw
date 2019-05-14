using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkhwClient.bean
{
    class psychosis_infoBean
    {
        public string id { get; set; }
        //姓名
        public string name { get; set; }
        //电子档案编号
        public string archive_no { get; set; }
        //身份证号
        public string id_number { get; set; }
        //监护人姓名
        public string guardian_name { get; set; }
        //监护人与患者关系
        public string guardian_relation { get; set; }
        //监护人地址
        public string guardian_address { get; set; }
        //监护人电话
        public string guardian_phone { get; set; }
        //居委会联系人
        public string neighborhood_committee_linkman { get; set; }
        //居委会联系电话
        public string neighborhood_committee_linktel { get; set; }
        //户别
        public string resident_type { get; set; }
        //就业情况

        public string employment_status { get; set; }
        //是否同意管理
        public string agree_manage { get; set; }

        //同意签字人
        public string agree_name { get; set; }
        //同意日期
        public string agree_date { get; set; }
        //初次发病日期
        public string first_morbidity_date { get; set; }
        //既往主要症状
        public string symptom { get; set; }
        // 既往关锁情况
        public string isolation { get; set; }
        //门诊
        public string outpatient { get; set; }
        //首次抗精神药治疗时间
        public string first_medicine_date { get; set; }
        //住院次数
        public string hospitalized_num { get; set; }
        //诊断
        public string diagnosis { get; set; }
        //确诊医院
        public string diagnosis_hospital { get; set; }
        //确诊日期
        public string diagnosis_date { get; set; }
        //最近一次治疗效果
        public string recently_treatment_effect { get; set; }

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



        //经济状况
        public string economics { get; set; }
        //专科医生意见
        public string specialist_suggestion { get; set; }
        //填表日期
        public string record_date { get; set; }
        //医生签字
        public string record_doctor { get; set; }
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
//id,name,aichive_no,id_number,guardian_name,guardian_relation,guardian_address,guardian_phone,neighborhood_committee_linkman,neighborhood_committee_linktel,resident_type,employment_status,agree_manage,agree_name,agree_date,first_morbidity_date,symptom,isolation,outpatient,first_medicine_date,hospitalized_num,diagnosis,diagnosis_hospital,diagnosis_date,recently_treatment_effect,dangerous_act,slight_trouble_num,cause_trouble_num,cause_accident_num,harm_other_num,autolesion_num,attempted_suicide_num,economics,specialist_suggestion,record_date,record_doctor,create_user,create_name,create_org,create_org_name,create_time,update_user,update_name,update_time,upload_status,upload_time,upload_result
