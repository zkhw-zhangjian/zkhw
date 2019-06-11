using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkhwClient.bean
{
    class healthPoor
    {
        public string id { get; set; }

        public string name { get; set; }
        //电子档案编号
        public string archive_no { get; set; }
        //身份证号
        public string id_number { get; set; }
        public string visit_date { get; set; }
        public string visit_type { get; set; }
        public string sex { get; set; }
        public string birthday { get; set; }
        public string visit_doctor { get; set; }
        public string work_info { get; set; }
        public string advice { get; set; }
        public string create_user { get; set; }
        public string create_name { get; set; }
        public string create_org { get; set; }
        public string create_org_name { get; set; }
        public string create_time { get; set; }
        public string update_user { get; set; }
        public string update_name { get; set; }
        public string update_time { get; set; }
        public string upload_status { get; set; }
        public string upload_time { get; set; }
        public string upload_result { get; set; }
    }
}
