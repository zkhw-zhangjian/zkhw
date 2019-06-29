using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkhwClient.bean
{
    class elderly_selfcare_estimateBean
    {
        //,,,,,,,,,,,,,,,,,,,,
        public string id { get; set; }
        //姓名
        public string name { get; set; }
        //电子档案编号
        public string aichive_no { get; set; }
        //身份证号
        public string id_number { get; set; }
        //sex
        public string sex { get; set; }
        //测试日期
        public string test_date { get; set; }
        //回答结果
        public string answer_result { get; set; }
        //总分
        public string total_score { get; set; }
        //评判结果
        public string judgement_result { get; set; }
        //测试医生
        public string test_doctor { get; set; }
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

        public string exam_id { get; set; }
    }
}
//id,name,aichive_no,id_number,sex,test_date,answer_result,total_score,judgement_result,test_doctor,create_user,create_name,create_org,create_org_name,create_time,update_user,update_name,update_time,upload_status,upload_time,upload_result
