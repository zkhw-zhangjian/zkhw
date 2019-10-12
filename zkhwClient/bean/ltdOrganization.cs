using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkhwClient.bean
{
    class ltdOrganization
    {
        public string id { get; set; }
        public string pub_orgcode { get; set; }
        public string organ_code { get; set; }
        public string organ_name { get; set; }
        public string organ_short_name { get; set; }
        public string organ_level { get; set; }
        public string organ_parent_code { get; set; }
        public string create_user_code { get; set; }
        public string create_time { get; set; }
        public string update_user_code { get; set; }
        public string update_time { get; set; }
        public string is_delete { get; set; }
        public string remark { get; set; }
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
        public string address { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
    }

    public class slowdiseases
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string healthAdvice { get; set; }
        public string man_healthAdvice { get; set; }
        public string woman_healthAdvice { get; set; }
        public string note { get; set; }
        public string create_user { get; set; }
        public string create_name { get; set; }
        public string create_time { get; set; }
        public string update_user { get; set; }
        public string update_name { get; set; }
        public string update_time { get; set; }
    }
}
