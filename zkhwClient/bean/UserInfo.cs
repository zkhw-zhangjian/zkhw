using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkhwClient.bean
{
    class UserInfo
    {
        public String id;
        public String userName;
        public String password;

        public string user_code { get; set; }
        public string pub_usercode { get; set; }
        public string user_name { get; set; }
        public string username { get; set; }
        public string sex { get; set; }
        public string job_num { get; set; }
        public string tele_phone { get; set; }
        public string mail { get; set; }
        public string birthday { get; set; }
        public string organ_code { get; set; }
        public string parent_organ { get; set; }
        public string depart_code { get; set; }
        public string user_type_code { get; set; }
        public string data_level { get; set; }
        public string status { get; set; }
        public string is_delete { get; set; }
        public string create_time { get; set; }
        public string create_user_code { get; set; }
        public string update_time { get; set; }
        public string update_user_code { get; set; }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
    }
}
