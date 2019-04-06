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
        //查询B超检查信息根据档案号和条码号
        public DataTable selectBichaoInfo(string aichive_no, string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select FubuBC,FubuResult,FubuDesc,QitaBC,QitaResult,QitaDesc from zkhw_tj_bc a where aichive_no = '" + aichive_no + "' and bar_code='"+ barcode + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //修改B超检查信息根据档案号和条码号
        public bool updateBichaoInfo(string aichive_no, string barcode, string FubuBC, string FubuResult, string FubuDesc, string QitaBC, string QitaResult, string QitaDesc)
        {
            int ret = 0;
            String sql = "update zkhw_tj_bc set FubuBC='" + FubuBC + "',FubuResult='" + FubuResult + "',FubuDesc='" + FubuDesc + "',QitaBC='" + QitaBC + "',QitaResult='" + QitaResult + "',QitaDesc='" + QitaDesc + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //查询心电图检查信息根据档案号和条码号
        public DataTable selectXindiantuInfo(string aichive_no, string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select XdtResult,XdtDesc,Ventrate,PR,QRS,QT,QTc,P_R_T,DOB,Age,Gen,Dep from zkhw_tj_xdt a where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //修改心电图检查信息根据档案号和条码号
        public bool updateXindiantuInfo(string aichive_no, string barcode,string XdtResult, string XdtDesc, string Ventrate, string PR, string QRS, string QT, string QTc, string P_R_T, string DOB, string Age, string Gen, string Dep)
        {
            int ret = 0;
            String sql = "update zkhw_tj_xdt set XdtResult='" + XdtResult + "',XdtDesc='" + XdtDesc + "',Ventrate='" + Ventrate + "',PR='" + PR + "',QRS='" + QRS + "',QT='" + QT + "',QTc='" + QTc + "',P_R_T='" + P_R_T + "',DOB='" + DOB + "',Age='" + Age + "',Gen='" + Gen + "',Dep='" + Dep + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //查询生化检查信息根据档案号和条码号
        public DataTable selectShenghuaInfo(string aichive_no, string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select  ALT, AST, TBIL, DBIL, CREA, UREA, GLU, TG, CHO, HDLC, LDLC, ALB, UA, HCY, AFP, CEA, Ka, Na, TP, ALP, GGT, CHE, TBA, APOA1, APOB, CK, CKMB, LDHL, HBDH, aAMY from zkhw_tj_sh a where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //修改生化检查信息根据档案号和条码号
        public bool updateShenghuaInfo(string aichive_no, string barcode, string ALT, string AST, string TBIL, string DBIL, string CREA,string UREA, string GLU, string TG, string CHO, string HDLC, string LDLC, string ALB, string UA, string HCY, string AFP, string CEA, string Ka, string Na, string TP, string ALP, string GGT, string CHE, string TBA, string APOA1, string APOB, string CK, string CKMB, string LDHL, string HBDH, string aAMY)
        {
            int ret = 0;
            String sql = "update zkhw_tj_sh set ALT='" + ALT + "',AST='" + AST + "',TBIL='" + TBIL + "',DBIL='" + DBIL + "',CREA='" + CREA + "',UREA='" + UREA + "',GLU='" + GLU + "',TG='" + TG + "',CHO='" + CHO + "',HDLC='" + HDLC + "',LDLC='" + LDLC + "',ALB='" + ALB + "',UA='" + UA + "',HCY='" + HCY + "',AFP='" + AFP + "',CEA='" + CEA + "',Ka='" + Ka + "',Na='" + Na + "',TP='" + TP + "',ALP='" + ALP + "',GGT='" + GGT + "',CHE='" + CHE + "',TBA='" + TBA + "',APOA1='" + APOA1 + "',APOB='" + APOB + "',CK='" + CK + "',CKMB='" + CKMB + "',LDHL='" + LDHL + "',HBDH='" + HBDH + "',aAMY='" + aAMY + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
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
