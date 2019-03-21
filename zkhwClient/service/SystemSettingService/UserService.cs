﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace zkhwClient.service
{
    class UserService
    {
        static dao.UserDao user = new dao.UserDao();
        public static DataTable UserExists(string name, string password)
        {
            bean.UserInfo userinfo = new bean.UserInfo();
            userinfo.UserName = name;
            userinfo.Pwd = password;
            
            return user.exists(userinfo);
        }
        public static bool updatePassWord(string name, string password)
        {
            bean.UserInfo userinfo = new bean.UserInfo();
            userinfo.UserName = name;
            userinfo.Pwd = password;
            return user.updatePassWord(userinfo);
        }
        public DataTable listUser()
        {
            return user.listUser();
        }
        public bool addUser(bean.UserInfo ui)
        {
            return user.addUser(ui);
        }
        public bool updateUser(bean.UserInfo ui)
        {
            return user.updateUser(ui);
        }
        public bool deleteUser(string id, string name)
        {
            return user.deleteUser(id, name);
        }
    }
}
