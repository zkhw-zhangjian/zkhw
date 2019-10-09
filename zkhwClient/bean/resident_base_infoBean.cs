using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkhwClient.bean
{
    class resident_base_infoBean
    {
        public string id { get; set; }
        //电子档案唯一编码
        public string archive_no { get; set; }
        //公卫平台电子档案编码
        public string pb_archive { get; set; }//标记页面未用
        //姓名
        public string name { get; set; }
        //性别
        public string sex { get; set; }
        //出生日期
        public string birthday { get; set; }
        //年龄
        public string age { get; set; }
        //身份证号
        public string id_number { get; set; }
        //身份证地址
        public string address { get; set; }
        //民族
        public string nation { get; set; }
        //签发机关
        public string IssuingAgencies { get; set; }
        //身份证照片
        public string CardPic { get; set; }
        //工作单位
        public string company { get; set; }
        //电话
        public string phone { get; set; }
        //联系人
        public string link_name { get; set; }
        //联系人电话
        public string link_phone { get; set; }
        //常住类型
        public string resident_type { get; set; }
        //户籍所在地
        public string register_address { get; set; }
        //现住址
        public string residence_address { get; set; }
        //血型
        public string blood_group { get; set; }
        //RH血型
        public string blood_rh { get; set; }
        //文化程度
        public string education { get; set; }
        //职业
        public string profession { get; set; }
        //婚姻状况
        public string marital_status { get; set; }
        //医疗费用支付方式
        public string pay_type { get; set; }
        //其他支付方式
        public string pay_other { get; set; }
        //药物过敏史
        public string drug_allergy { get; set; }
        //其他过敏
        public string allergy_other { get; set; }
        //暴露史
        public string exposure { get; set; }
        //其他疾病
        public string disease_other { get; set; }
        //是否高血压
        public string is_hypertension { get; set; }
        //是否糖尿病
        public string is_diabetes { get; set; }
        //是否精神病
        public string is_psychosis { get; set; }
        //是否结核病
        public string is_tuberculosis { get; set; }
        //是否遗传病
        public string is_heredity { get; set; }
        //遗传病名称
        public string heredity_name { get; set; }
        //是否残疾 
        public string is_deformity { get; set; }
        //残疾名字
        public string deformity_name { get; set; }
        //是否贫困
        public string is_poor { get; set; }
        //厨房排风设施
        public string kitchen { get; set; }
        //燃料类型
        public string fuel { get; set; }
        //燃料类型(其它)
        public string other_fuel { get; set; }
        //饮水
        public string drink { get; set; }
        //饮水(其它)
        public string other_drink { get; set; }
        //厕所
        public string toilet { get; set; }
        //禽畜栏
        public string poultry { get; set; }
        //健康卡号
        public string medical_code { get; set; }
        //摄像头照片地址
        public string photo_code { get; set; }
        //建档机构
        public string aichive_org { get; set; }
        //责任医生id
        public string doctor_id { get; set; }
        //责任医生
        public string doctor_name { get; set; }
        //是否签约
        public string is_signing { get; set; }
        //是否同步
        public string is_synchro { get; set; }
        //同步结果（成功/失败）
        public string synchro_result { get; set; }
        //同步时间
        public string synchro_time { get; set; }
        public string province_code { get; set; }
        public string province_name { get; set; }
        public string city_code { get; set; }
        public string city_name { get; set; }
        public string county_code { get; set; }
        public string county_name { get; set; }
        public string towns_code { get; set; }
        public string towns_name { get; set; }
        public string village_code { get; set; }
        public string village_name { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        //create_user
        public string create_user { get; set; }
        //创建人
        public string create_name { get; set; }
        //创建时间
        public string create_time { get; set; }
        //创建机构
        public string create_org { get; set; }
        //创建机构名称
        public string create_org_name { get; set; }
        //update_user
        public string update_user { get; set; }
        //修改人
        public string update_name { get; set; }
        //修改时间
        public string update_time { get; set; }
        //是否是孕妇
        public int is_gravida { get; set; }
    }
}
//id,archive_no,pb_archive,name,sex,birthday,id_number,address,nation,IssuingAgencies,CardPic,company,phone,link_name,link_phone,resident_type,register_address,residence_address,blood_group,blood_rh,education,profession,marital_status,pay_type,pay_other,drug_allergy,allergy_other,exposure,disease_other,is_hypertension,is_diabetes,is_psychosis,is_tuberculosis,is_heredity,heredity_name,is_deformity,deformity_name,is_poor,kitchen,fuel,other_fuel,drink,other_drink,toilet,poultry,medical_code,photo_code,aichive_org,doctor_name,is_signing,is_synchro,synchro_result,synchro_time,create_user,create_name,create_time,create_org,create_org_name,update_user,update_name,update_time
