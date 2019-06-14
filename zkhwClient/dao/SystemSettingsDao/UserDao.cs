using System;
using System.Data;
using zkhwClient.bean;
using System.Linq;
using System.Windows.Forms;

namespace zkhwClient.dao
{
    class UserDao
    {
        public DataTable exists(UserInfo user)
        {
            String sql = "select * from zkhw_user_info where username = '" + user.UserName + "'AND password = '" + user.Password + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public bool updatePassWord(bean.UserInfo user)
        {
            int ret = 0;
            String sql = "update zkhw_user_info set password='" + user.Password + "' where username = '" + user.UserName + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public DataTable listUser()
        {
            String sql = "select username,password,user_name,sex,birthday from zkhw_user_info where 1=1 and username <> 'admin'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            if (ds.Tables.Count==0) { MessageBox.Show("未查询到数据!");return null; }
            return ds.Tables[0];
        }
        public bool addUser(bean.UserInfo ui)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int rt = 0;
            String sql = "select * from zkhw_user_info where name='" + ui.UserName + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                string sql1 = "insert into zkhw_user_info (username,password) values ('" + ui.UserName + "', '" + ui.Password + "')";
                rt = DbHelperMySQL.ExecuteSql(sql1);
            }

            return rt == 0 ? false : true;
        }
        public bool updateUser(bean.UserInfo ui)
        {
            int ret = 0;
            String sql = "update zkhw_user_info set username='" + ui.UserName + "' where id='" + ui.Id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        public bool deleteUser(string id, string name)
        {
            int ret = 0;
            String sql = "delete from zkhw_user_info where id = '" + id + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);

            return ret == 0 ? false : true;
        }
        //基本设置中，获取乡镇卫生院的医护人员信息   2019-6-13按照人名排序同时过滤重复
        public DataTable listUserbyOrganCode(String code)
        {
            String sql = "select DISTINCT  user_code ucode,user_name uname from zkhw_user_info where username !='admin' ORDER BY convert(uname using gbk) ASC ";// and organ_code = '" + code + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //通过机构编号获取机构名称
        public DataTable checkOrganNameBycode(string organcode)
        {
            String sql = "select organ_name from ltd_organization where organ_code = '" + organcode + "' limit 1";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
