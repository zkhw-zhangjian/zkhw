using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    public class ConfigInfoManage
    {
        public bool Insert(bean.ConfigInfo obj )
        {
            int ret = 0;
            string sql = string.Format(@"Insert into tblconfig(Name,Content)
                         Values('{0}','{1}')",obj.Name,obj.Content);
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true; 
        }

        public bool Update(bean.ConfigInfo obj)
        {
            int ret = 0;
            string sql = string.Format(@"update tblconfig set Name='{0}',Content='{1}' 
                         where ID={2}",obj.Name,obj.Content,obj.ID);
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }

        public bean.ConfigInfo GetObj(string s)
        {
            bean.ConfigInfo obj = null;
            string sql = string.Format("select  * from tblconfig ",s);
            DataSet ds = null;
            ds = DbHelperMySQL.Query(sql);
            if(ds!=null)
            {
                DataTable dt = ds.Tables[0];
                string tmp = dt.Rows[0]["ID"].ToString();
                if (tmp != "")
                {
                    obj = new bean.ConfigInfo();
                    obj.ID = int.Parse(tmp);
                    obj.Name = dt.Rows[0]["Name"].ToString();
                    obj.Content = dt.Rows[0]["Content"].ToString();
                } 
            }
            return obj;
        }

        
    }
}
