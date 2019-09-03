using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkhwClient.bean
{
    class thresholdValueBean
    {
        public string id { get; set; }
        public string class_type { get; set; }
        public string type { get; set; }
        public string warning_min { get; set; }
        public string warning_max { get; set; }
        public string threshold_min { get; set; }
        public string threshold_max { get; set; }
        public string create_user { get; set; }
        public string create_name { get; set; }
        public string create_time { get; set; }
        public string update_user { get; set; }
        public string update_name { get; set; }
        public string update_time { get; set; }
        public string chinaName { get; set; }
        public string CheckMethod { get; set; }
        public string unit { get; set; }

    }
}
