using System;
using System.Data;

namespace zkhwClient.dao
{
    class tjcheckDao
    {
        public bool insertShenghuaInfo(shenghuaBean sh)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string timeCodeUnique = sh.bar_code + "_" + sh.createTime;
            String sql = "insert into zkhw_tj_sh (ID,aichive_no,id_number,bar_code,ALB,ALP,ALT,AST,CHO,Crea,DBIL,GGT,GLU,HDLC,LDLC,TBIL,TG,TP,UA,UREA,createtime,timeCodeUnique) values('" + id + "','" + sh.aichive_no + "','" + sh.id_number + "','" + sh.bar_code + "','" + sh.ALB + "','" + sh.ALP + "','" + sh.ALT + "','" + sh.AST + "','" + sh.CHO + "','" + sh.Crea + "','" + sh.DBIL + "','" + sh.GGT + "','" + sh.GLU + "','" + sh.HDL_C + "','" + sh.LDL_C + "','" + sh.TBIL + "','" + sh.TG + "','" + sh.TP + "','" + sh.UA + "','" + sh.UREA + "','" + sh.createTime + "','" + timeCodeUnique + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }

        public bool insertXuechangguiInfo(xuechangguiBean xcg)
        {
            int rt = 0;
            string id = Result.GetNewId();
            string timeCodeUnique = xcg.bar_code + "_" + xcg.createTime;
            String sql = "insert into zkhw_tj_xcg(ID,aichive_no,id_number,bar_code,HCT,HGB,LYM,LYMP,MCH,MCHC,MCV,MPV,MXD,MXDP,NEUT,NEUTP,PCT,PDW,PLT,RBC,RDWCV,RDWSD,WBC,createtime,timeCodeUnique) values('" + id + "','" + xcg.aichive_no + "','" + xcg.id_number + "','" + xcg.bar_code + "','" + xcg.HCT + "','" + xcg.HGB + "','" + xcg.LYM + "','" + xcg.LYMP + "','" + xcg.MCH + "','" + xcg.MCHC + "','" + xcg.MCV + "','" + xcg.MPV + "','" + xcg.MXD + "','" + xcg.MXDP + "','" + xcg.NEUT + "','" + xcg.NEUTP + "','" + xcg.PCT + "','" + xcg.PDW + "','" + xcg.PLT + "','" + xcg.RBC + "','" + xcg.RDW_CV + "','" + xcg.RDW_SD + "','" + xcg.WBC + "','" + xcg.createTime + "','" + timeCodeUnique + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }


        //更新设备的状态
        public bool updateShDevice()
        {
            int ret = 0;
            int flag = 1;
            String sql = "update zkhw_state_device set shonline='" + flag + "',shstate='" + flag + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
    }
}
