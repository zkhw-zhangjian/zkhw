using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkhwClient.bean
{
    class hospitalizedRecord
    { 
        public string id { get; set; }
        public string archive_no { get; set; }
        public string id_number { get; set; }
        public string hospitalized_type { get; set; }
        public string in_hospital_time { get; set; }
        public string leave_hospital_time { get; set; }
        public string reason { get; set; }
        public string hospital_organ { get; set; }
        public string case_code { get; set; }
        public string create_name { get; set; }
        public string create_time { get; set; }
        public string update_name { get; set; }
    }
}
