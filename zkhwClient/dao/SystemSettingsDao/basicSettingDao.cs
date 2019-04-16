using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class basicSettingDao
    {
        public bool addBasicSetting(string sheng_code, string shi_code, string qx_code, string xz_code, string cun_code, string organ_code, string organ_name, string input_name, string zeren_doctor, string bc, string xcg, string sh, string sgtz, string ncg, string xdt, string xy, string wx, string other, string captain, string members, string operation, string car_name, string create_user, string create_name,string allname)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");                                                                 
            String sql = "insert into basicInfo_setting (id,sheng_code,shi_code,qx_code,xz_code,cun_code,organ_code,organ_name,input_name,zeren_doctor,bc,xcg,sh,sgtz,ncg,xdt,xy,wx,other,captain,members,operation,car_name,create_user,create_name,create_time,allFullName) values ('" + id + "', '" + sheng_code + "', '" + shi_code + "', '" + qx_code + "', '" + xz_code + "', '" + cun_code + "', '" + organ_code + "', '" + organ_name + "', '" + input_name + "', '" + zeren_doctor + "', '" + bc + "', '" + xcg + "', '" + sh + "', '" + sgtz + "', '" + ncg + "', '" + xdt + "', '" + xy + "', '" + wx + "', '" + other + "', '" + captain + "', '" + members + "', '" + operation + "', '" + car_name + "', '" + create_user + "', '" + create_name + "', '" + time + "', '" + allname + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public DataTable checkBasicsettingInfo()
        {
            DataSet ds = new DataSet();
            String sql = "select sheng_code, shi_code, qx_code, xz_code, cun_code, organ_code, organ_name, input_name, zeren_doctor, bc, xcg, sh, sgtz, ncg, xdt, xy, wx, other, captain, members, operation, car_name,create_time,allFullName from basicInfo_setting where 1=1 order by create_time desc";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //根据区域编号获取区域名称
        public DataTable selectAreaBycode(string code)
        {
            DataSet ds = new DataSet();
            String sql = "select name from code_area_config where code='"+code+"' ";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        //根据团队编号获取队员信息
        public DataTable checkTeamInfoBycode(string code)
        {
            DataSet ds = new DataSet();
            String sql = "select doctor_name from team_doctor where team_no='" + code + "' ";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
