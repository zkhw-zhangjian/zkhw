using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkhwClient.bean
{
    class fuv_hypertensionBean
    {

        public string id { get; set; }
        //电子档案编号
        public string aichive_no { get; set; }
        //身份证号
        public string id_number { get; set; }//标记页面未用
        //条码编号
        public string Codebar { get; set; }//
        //患者姓名
        public string name { get; set; }
        //数据状态
        public string dataSate { get; set; }//
        //高血压随访编码
        public string hypertension_code { get; set; }//
        //访问日期
        public string visit_date { get; set; }
        //访问方式
        public string visit_type { get; set; }
        //症状
        public string symptom { get; set; }
        //其他症状
        public string other_symptom { get; set; }
        //收缩压
        public string sbp { get; set; }
        //舒张压
        public string dbp { get; set; }
        //本次体重(kg)
        public string weight { get; set; }
        //下次目标体重
        public string target_weight { get; set; }
        ////身高(cm)
        //public string height { get; set; }

        //本次体质指数
        public string bmi { get; set; }
        //下次目标体质指数
        public string target_bmi { get; set; }
        //心率（次/分）
        public string heart_rate { get; set; }
        //其它体征
        public string other_sign { get; set; }
        //日吸烟量
        public string smoken { get; set; }
        //目标日吸烟量（支）
        public string target_somken { get; set; }
        //日饮酒量（两）
        public string wine { get; set; }
        //目标日饮酒量（两）
        public string target_wine { get; set; }
        //运动（次/周）
        public string sport_week { get; set; }
        //运动（分钟/次）
        public string sport_once { get; set; }
        //目标运动(次/周)
        public string target_sport_week { get; set; }
        //目标运动（分钟/次）
        public string target_sport_once { get; set; }
        //摄盐情况
        public string salt_intake { get; set; }
        //目标摄盐情况
        public string target_salt_intake { get; set; }
        //心理调整
        public string mind_adjust { get; set; }
        //遵医行为
        public string doctor_obey { get; set; }
        //辅助检查
        public string assist_examine { get; set; }
        //服药依从性 
        public string drug_obey { get; set; }
        //有无药物不良反应
        public string untoward_effect { get; set; }
        //药物不良反应说明
        public string untoward_effect_drug { get; set; }
        //此次随访分类
        public string visit_class { get; set; }

        //转诊记录Id
        public string referral_code { get; set; }//
        //下次随访日期
        public string next_visit_date { get; set; }
        //访问医生签名
        public string visit_doctor { get; set; }
        //评价与建议
        public string advice { get; set; }
        //创建者
        public string create_name { get; set; }//
        //创建时间
        public string create_time { get; set; }//
        //修改者
        public string update_name { get; set; }//
        //修改时间
        public string update_time { get; set; }//
        //转诊机构及科别
        public string transfer_organ { get; set; }
        //转诊原因
        public string transfer_reason { get; set; }
    }
}
