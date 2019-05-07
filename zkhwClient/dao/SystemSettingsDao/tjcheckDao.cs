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
            String sql = "insert into zkhw_tj_xcg(ID,aichive_no,id_number,bar_code,HCT,HGB,LYM,LYMP,MCH,MCHC,MCV,MPV,MXD,MXDP,NEUT,NEUTP,PCT,PDW,PLT,RBC,RDWCV,RDWSD,WBC,MONO,MONOP,GRAN,GRANP,PLCR,createtime,timeCodeUnique) values('" + id + "','" + xcg.aichive_no + "','" + xcg.id_number + "','" + xcg.bar_code + "','" + xcg.HCT + "','" + xcg.HGB + "','" + xcg.LYM + "','" + xcg.LYMP + "','" + xcg.MCH + "','" + xcg.MCHC + "','" + xcg.MCV + "','" + xcg.MPV + "','" + xcg.MXD + "','" + xcg.MXDP + "','" + xcg.NEUT + "','" + xcg.NEUTP + "','" + xcg.PCT + "','" + xcg.PDW + "','" + xcg.PLT + "','" + xcg.RBC + "','" + xcg.RDW_CV + "','" + xcg.RDW_SD + "','" + xcg.WBC + "','" + xcg.MONO + "','" + xcg.MONOP + "','" + xcg.GRAN + "','" + xcg.GRANP + "','" + xcg.PLCR + "','" + xcg.createTime + "','" + timeCodeUnique + "')";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        //查询B超检查信息根据档案号和条码号
        public DataTable selectBichaoInfo(string aichive_no, string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select FubuBC,FubuResult,FubuDesc,QitaBC,QitaResult,QitaDesc from zkhw_tj_bc a where aichive_no = '" + aichive_no + "' and bar_code='"+ barcode + "' order by createtime desc limit 1";
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
            string sql = "select XdtResult,XdtDesc,Ventrate,PR,QRS,QT,QTc,P_R_T,DOB,Age,Gen,Dep from zkhw_tj_xdt a where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "' order by createtime desc limit 1";
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
            string sql = "select  ALT, AST, TBIL, DBIL, CREA, UREA, GLU, TG, CHO, HDLC, LDLC, ALB, UA, HCY, AFP, CEA, Ka, Na, TP, ALP, GGT, CHE, TBA, APOA1, APOB, CK, CKMB, LDHL, HBDH, aAMY from zkhw_tj_sh a where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "' order by createtime desc limit 1";
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
        //查询血常规检查信息根据档案号和条码号
        public DataTable selectXuechangguiInfo(string aichive_no, string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select WBC,RBC,PCT,PLT,HGB,HCT,MCV,MCH,MCHC,RDWCV,RDWSD,MONO,MONOP,GRAN,GRANP,NEUT,NEUTP,EO,EOP,BASO,BASOP,LYM,LYMP,MPV,PDW,MXD,MXDP,PLCR,OTHERS from zkhw_tj_xcg a where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "' order by createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //修改血常规检查信息根据档案号和条码号
        public bool updateXuechangguiInfo(string aichive_no, string barcode, string WBC, string RBC, string PCT, string PLT, string HGB, string HCT, string MCV, string MCH, string MCHC, string RDWCV, string RDWSD, string MONO, string MONOP, string GRAN, string GRANP, string NEUT, string NEUTP, string EO, string EOP, string BASO, string BASOP, string LYM, string LYMP, string MPV, string PDW, string MXD, string MXDP, string PLCR, string OTHERS)
        {
            int ret = 0;
            String sql = "update zkhw_tj_xcg set WBC='" + WBC + "',RBC='" + RBC + "',PCT='" + PCT + "',PLT='" + PLT + "',HGB='" + HGB + "',HCT='" + HCT + "',MCV='" + MCV + "',MCH='" + MCH + "',MCHC='" + MCHC + "',RDWCV='" + RDWCV + "',RDWSD='" + RDWSD + "',MONO='" + MONO + "',MONOP='" + MONOP + "',GRAN='" + GRAN + "',GRANP='" + GRANP + "',NEUT='" + NEUT + "',NEUTP='" + NEUTP + "',EO='" + EO + "',EOP='" + EOP + "',BASO='" + BASO + "',BASOP='" + BASOP + "',LYM='" + LYM + "',LYMP='" + LYMP + "',MPV='" + MPV + "',PDW='" + PDW + "',MXD='" + MXD + "',MXDP='" + MXDP + "',PLCR='" + PLCR + "',OTHERS='" + OTHERS + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //查询尿常规检查信息根据档案号和条码号
        public DataTable selectNiaochangguiInfo(string aichive_no, string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select WBC,LEU,NIT,URO,PRO,PH,BLD,SG,KET,BIL,GLU,Vc,MA,ACR,Ca,CR from zkhw_tj_ncg a where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "' order by createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //修改尿常规检查信息根据档案号和条码号
        public bool updateNiaochangguiInfo(string aichive_no, string barcode, string WBC, string LEU, string NIT, string URO, string PRO, string PH, string BLD, string SG, string KET, string BIL, string GLU, string Vc, string MA, string ACR, string Ca, string CR)
        {
            int ret = 0;
            String sql = "update zkhw_tj_ncg set WBC='" + WBC + "',LEU='" + LEU + "',NIT='" + NIT + "',URO='" + URO + "',PRO='" + PRO + "',PH='" + PH + "',BLD='" + BLD + "',SG='" + SG + "',KET='" + KET + "',BIL='" + BIL + "',GLU='" + GLU + "',Vc='" + Vc + "',MA='" + MA + "',ACR='" + ACR + "',Ca='" + Ca + "',CR='" + CR + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //查询血压检查信息根据档案号和条码号
        public DataTable selectXueyaInfo(string aichive_no, string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select DBP,SBP,Pulse from zkhw_tj_xy a where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "' order by createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //修改血压检查信息根据档案号和条码号
        public bool updateXueyaInfo(string aichive_no, string barcode, string DBP, string SBP, string Pulse)
        {
            int ret = 0;
            String sql = "update zkhw_tj_xy set DBP='" + DBP + "',SBP='" + SBP + "',Pulse='" + Pulse + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //查询身高体重检查信息根据档案号和条码号
        public DataTable selectSgtzInfo(string aichive_no, string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select Height,Weight,BMI from zkhw_tj_sgtz a where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "' order by createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //修改身高体重检查信息根据档案号和条码号
        public bool updateSgtzInfo(string aichive_no, string barcode, string Height, string Weight, string BMI)
        {
            int ret = 0;
            String sql = "update zkhw_tj_sgtz set Height='" + Height + "',Weight='" + Weight + "',BMI='" + BMI + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }

        //更新体检进度tj_bgdc--血常规
        public bool updateTJbgdcXuechanggui(string aichive_no, string barcode, int flag)
        {
            int ret = 0;
            String sql = "update zkhw_tj_bgdc set XueChangGui='" + flag + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        
        //更新体检进度tj_bgdc--生化
        public bool updateTJbgdcShenghua(string aichive_no, string barcode,int flag)
        {
            int ret = 0;
            String sql = "update zkhw_tj_bgdc set ShengHua='" + flag + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }

        //查询B超体检信息
        public DataTable checkBichaoInfo(string time1, string time2,string xcuncode)
        {
            DataSet ds = new DataSet();
            string sql = "select healthchecktime,name,aichive_no,id_number,bar_code,(case when BChao='1' then '完成' else '未完成' end) as type from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time1 != null && !"".Equals(time1))
            {
                sql += " and createtime >='" + time1 + "'";
            }
            if (time2 != null && !"".Equals(time2))
            {
                sql += " and createtime <='" + time2 + "'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //查询生化体检信息
        public DataTable checkShenghuaInfo(string time1, string time2, string xcuncode)
        {
            DataSet ds = new DataSet();
            string sql = "select healthchecktime,name,aichive_no,id_number,bar_code,(case when ShengHua='1' then '完成' else '未完成' end) as type from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time1 != null && !"".Equals(time1))
            {
                sql += " and createtime >='" + time1 + "'";
            }
            if (time2 != null && !"".Equals(time2))
            {
                sql += " and createtime <='" + time2 + "'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //查询尿常规体检信息
        public DataTable checkNcgInfo(string time1, string time2, string xcuncode)
        {
            DataSet ds = new DataSet();
            string sql = "select healthchecktime,name,aichive_no,id_number,bar_code,(case when NiaoChangGui='1' then '完成' else '未完成' end) as type from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time1 != null && !"".Equals(time1))
            {
                sql += " and createtime >='" + time1 + "'";
            }
            if (time2 != null && !"".Equals(time2))
            {
                sql += " and createtime <='" + time2 + "'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //查询血常规体检信息
        public DataTable checkXcgInfo(string time1, string time2, string xcuncode)
        {
            DataSet ds = new DataSet();
            string sql = "select healthchecktime,name,aichive_no,id_number,bar_code,(case when XueChangGui='1' then '完成' else '未完成' end) as type from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time1 != null && !"".Equals(time1))
            {
                sql += " and createtime >='" + time1 + "'";
            }
            if (time2 != null && !"".Equals(time2))
            {
                sql += " and createtime <='" + time2 + "'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //查询身高体重体检信息
        public DataTable checkSgtzInfo(string time1, string time2, string xcuncode)
        {
            DataSet ds = new DataSet();
            string sql = "select healthchecktime,name,aichive_no,id_number,bar_code,(case when Shengaotizhong='1' then '完成' else '未完成' end) as type from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time1 != null && !"".Equals(time1))
            {
                sql += " and createtime >='" + time1 + "'";
            }
            if (time2 != null && !"".Equals(time2))
            {
                sql += " and createtime <='" + time2 + "'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //查询心电图体检信息
        public DataTable checkXdtInfo(string time1, string time2, string xcuncode)
        {
            DataSet ds = new DataSet();
            string sql = "select healthchecktime,name,aichive_no,id_number,bar_code,(case when XinDian='1' then '完成' else '未完成' end) as type from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time1 != null && !"".Equals(time1))
            {
                sql += " and createtime >='" + time1 + "'";
            }
            if (time2 != null && !"".Equals(time2))
            {
                sql += " and createtime <='" + time2 + "'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //查询血压体检信息
        public DataTable checkXueyaInfo(string time1, string time2, string xcuncode)
        {
            DataSet ds = new DataSet();
            string sql = "select healthchecktime,name,aichive_no,id_number,bar_code,(case when XueYa='1' then '完成' else '未完成' end) as type from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time1 != null && !"".Equals(time1))
            {
                sql += " and createtime >='" + time1 + "'";
            }
            if (time2 != null && !"".Equals(time2))
            {
                sql += " and createtime <='" + time2 + "'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        //更新设备的状态
        public bool updateShDevice(int sfzflag,int sxtflag,int dyjflag,int xcgflag,int shflag,int ncgflag,int xdtflag,int sgtzflag,int xyflag,int bcflag)
        {
            int ret = 0;
            String sql = "update zkhw_state_device set";
            if (sfzflag>-1) {
                sql += " sfz_online='" + sfzflag + "',sfz_tate='" + sfzflag + "'";
            }
            if (sxtflag>-1) {
                sql += " sxt_online='" + sxtflag + "',sxt_state='" + sxtflag + "'";
            }
            if (dyjflag > -1)
            {
                sql += " dyj_online ='" + dyjflag + "',dyj_state='" + dyjflag + "'";
            }
            if (xcgflag > -1)
            {
                sql += " xcg_online ='" + xcgflag + "',xcg_state='" + xcgflag + "'";
            }
            if (shflag > -1)
            {
                sql += " sh_online ='" + shflag + "',sh_state='" + shflag + "'";
            }
            if (ncgflag > -1)
            {
                sql += " ncg_online ='" + ncgflag + "',ncg_state='" + ncgflag + "'";
            }
            if (xdtflag > -1)
            {
                sql += " xdt_online ='" + xdtflag + "',xdt_state='" + xdtflag + "'";
            }
            if (sgtzflag > -1)
            {
                sql += " sgtz_online ='" + sgtzflag + "',sgtz_state='" + sgtzflag + "'";
            }
            if (xyflag > -1)
            {
                sql += " xy_online ='" + xyflag + "',xy_state='" + xyflag + "'";
            }
            if (bcflag > -1)
            {
                sql += " bc_online ='" + bcflag + "',bc_state='" + bcflag + "'";
            }
            sql +=  " where ID ='1'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }

        //获取设备的状态
        public DataTable checkDevice()
        {
            DataSet ds = new DataSet();
            String sql = "select sfz_online,sfz_tate,sxt_online,sxt_state,dyj_online,dyj_state,xcg_online,xcg_state,sh_online,sh_state,ncg_online,ncg_state,xdt_online,xdt_state,sgtz_online,sgtz_state,xy_online,xy_state,bc_online,bc_state from zkhw_state_device where ID='1'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        //根据血常规检查结果修改健康体检表对应信息
        public bool updatePEXcgInfo(string aichive_no, string barcode, string blood_hemoglobin, string blood_leukocyte, string blood_platelet)
        {
            int ret = 0;
            String sql = "update physical_examination_record set blood_hemoglobin='" + blood_hemoglobin + "',blood_leukocyte='" + blood_leukocyte + "',blood_platelet='" + blood_platelet + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
        //根据生化检查结果修改健康体检表对应信息
        public bool updatePEShInfo(string aichive_no, string barcode, string tc, string tg, string ldl,string hdl)
        {
            int ret = 0;
            String sql = "update physical_examination_record set tc='" + tc + "',tg='" + tg + "',ldl='" + ldl + "',hdl='" + hdl + "' where aichive_no = '" + aichive_no + "' and bar_code='" + barcode + "'";
            ret = DbHelperMySQL.ExecuteSql(sql);
            return ret == 0 ? false : true;
        }
    }
}
