using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.dao
{
    class downloadDataForYunDao
    {
        private static jkInfoDao jkdao = new jkInfoDao();
        public static shenghuaBean GetShengHuaObj(string ZrysSH, DataRow dr, out string timecodeUnique,out string shid)
        {
            timecodeUnique = "";
            shid = dr["ID"].ToString();
            shenghuaBean sh = new shenghuaBean();
            sh.ZrysSH = ZrysSH;
            sh.bar_code = dr["bar_code"].ToString();
            
            DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(sh.bar_code);
            if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
            {
                sh.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                sh.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
            }
            else
            {
                return null;
            }
            sh.ALT = dr["ALT"].ToString();
            sh.AST = dr["AST"].ToString();
            sh.TBIL = dr["TBIL"].ToString();
            sh.DBIL = dr["DBIL"].ToString();
            sh.Crea = dr["CREA"].ToString();
            sh.UREA = dr["UREA"].ToString();
            sh.GLU = dr["GLU"].ToString();
            sh.TG = dr["TG"].ToString();
            sh.CHO = dr["CHO"].ToString();
            sh.HDL_C = dr["HDLC"].ToString();
            sh.LDL_C = dr["LDLC"].ToString();
            sh.ALB = dr["ALB"].ToString();
            sh.UA = dr["UA"].ToString();
            sh.TP = dr["TP"].ToString();
            sh.ALP = dr["ALP"].ToString();
            sh.GGT = dr["GGT"].ToString();
            sh.createTime = dr["createtime"].ToString();
            timecodeUnique = dr["timecodeUnique"].ToString();
            
            return sh;
        }
        public static DataTable GetShenHuaDataForYun(string a,string b,string c)
        {
            string sql = string.Format(@"select * from zkhw_temp_sh where status=0 and bar_code in ({0}) and 
                           (DATE_FORMAT(createtime,'%Y-%m-%d')>='{1}' and DATE_FORMAT(createtime,'%Y-%m-%d')<='{2}')", a,b,c);
            DataSet ds = DbHelperMySQL.QueryYpt(sql);
            if(ds !=null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public static string GetSqlForShenHua(shenghuaBean sh, string timecodeUnique)
        {
            string sql1 = "select count(id) from zkhw_tj_sh where timeCodeUnique='" + timecodeUnique + "'";
            bool flag = DbHelperMySQL.Exists(sql1);
            string sql = "";
            if(flag==false)
            {
                string id = Result.GetNewId(); 
                sql = "insert into zkhw_tj_sh (ID,aichive_no,id_number,bar_code,ALB,ALP,ALT,AST,CHO,Crea,DBIL,GGT,GLU,HDLC,LDLC,TBIL,TG,TP,UA,UREA,createtime,ZrysSH,timeCodeUnique,upload_status) values('" + id + "','" + sh.aichive_no + "','" + sh.id_number + "','" + sh.bar_code + "','" + sh.ALB + "','" + sh.ALP + "','" + sh.ALT + "','" + sh.AST + "','" + sh.CHO + "','" + sh.Crea + "','" + sh.DBIL + "','" + sh.GGT + "','" + sh.GLU + "','" + sh.HDL_C + "','" + sh.LDL_C + "','" + sh.TBIL + "','" + sh.TG + "','" + sh.TP + "','" + sh.UA + "','" + sh.UREA + "','" + sh.createTime + "','" + sh.ZrysSH + "','" + timecodeUnique + "',0)";
            }
            else
            {
                sql = "update zkhw_tj_sh set ALB='" + sh.ALB + "',ALP='" + sh.ALP + "',ALT='" + sh.ALT + "',AST='" + sh.AST + "',CHO='" + sh.CHO + "',Crea='" + sh.Crea + "',DBIL='" + sh.DBIL + "',GGT='" + sh.GGT + "',GLU='" + sh.GLU + "',HDLC='" + sh.HDL_C + "',LDLC='" + sh.LDL_C + "',TBIL='" + sh.TBIL + "',TG='" + sh.TG + "',TP='" + sh.TP + "',UA='" + sh.UA + "',UREA='" + sh.UREA + "',upload_status=0 where timeCodeUnique = '" + timecodeUnique + "'" ;
            }
            return sql;
        } 

        public static string GetUpdatePEShInfoSql(shenghuaBean sh)
        {
            String sql = @"update physical_examination_record set sgft='" + sh.ALT + "', ast='" + sh.AST + "', albumin='" + sh.ALB + 
                "', total_bilirubin='" + sh.TBIL + "', conjugated_bilirubin='" + sh.DBIL + "', scr='" + sh.Crea + 
                "', blood_urea='" + sh.UREA + "', tc='" + sh.CHO + "',tg='" + sh.TG + "',ldl='" + sh.LDL_C + 
                "',hdl='" + sh.HDL_C + "',blood_glucose_mmol='" + sh.GLU + "' where aichive_no = '" + sh.aichive_no + "' and bar_code='" + sh.bar_code + "'";
            return sql;
        }
        public static string GetUpdateBgdcShSql(DataTable dttv, shenghuaBean sh)
        {
            int flag =  Common.JudgeValueForSh(dttv, sh); 
            String sql = "update zkhw_tj_bgdc set ShengHua='" + flag + "' where aichive_no = '" + sh.aichive_no + "' and bar_code='" + sh.bar_code + "'";
            return sql;
        }

        public static string GetUpdateShToYun(string id)
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql =string.Format("update zkhw_temp_sh set status=1,updatetime='{0}' where id='{1}'",dt, id);
            return sql;
        }


        public static DataTable GetXueChangGuiDataForYun(string a, string b, string c)
        {
            string sql = string.Format(@"select * from zkhw_temp_xcg where status=0 and bar_code in ({0}) and 
                           (DATE_FORMAT(createtime,'%Y-%m-%d')>='{1}' and DATE_FORMAT(createtime,'%Y-%m-%d')<='{2}')", a, b, c);
            DataSet ds = DbHelperMySQL.QueryYpt(sql);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public static xuechangguiBean GetXCGObj(string ZrysSH, DataRow dr, out string xcgid)
        {
            xcgid = dr["ID"].ToString();
            xuechangguiBean obj = new xuechangguiBean();
            try
            {
                obj.ZrysXCG = ZrysSH;
                obj.bar_code = dr["bar_code"].ToString();
                DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(obj.bar_code);
                if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                {
                    obj.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                    obj.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                }
                else
                {
                    return null;
                }
                obj.HCT = dr["HCT"].ToString();
                obj.HGB = dr["HGB"].ToString();
                obj.LYM = dr["LYM"].ToString();
                obj.LYMP = dr["LYMP"].ToString();
                obj.MCH = dr["MCH"].ToString();
                obj.MCHC = dr["MCHC"].ToString();
                obj.MCV = dr["MCV"].ToString();
                obj.MPV = dr["MPV"].ToString();
                obj.MXD = dr["MXD"].ToString();
                obj.MXDP = dr["MXDP"].ToString();
                obj.NEUT = dr["NEUT"].ToString();
                obj.NEUTP = dr["NEUTP"].ToString();
                obj.PCT = dr["PCT"].ToString();
                obj.PDW = dr["PDW"].ToString();
                obj.PLT = dr["PLT"].ToString();
                obj.RBC = dr["RBC"].ToString();
                obj.RDW_CV = dr["RDWCV"].ToString();
                obj.RDW_SD = dr["RDWSD"].ToString();
                obj.WBC = dr["WBC"].ToString();
                obj.MONO = dr["MONO"].ToString();
                obj.MONOP = dr["MONOP"].ToString();
                obj.GRAN = dr["GRAN"].ToString();
                obj.GRANP = dr["GRANP"].ToString();
                obj.PLCR = dr["PLCR"].ToString();
                obj.createTime = dr["createtime"].ToString();
                obj.timeCodeUnique = dr["timecodeUnique"].ToString();
            }
            catch(Exception dd)
            {

            }
            
            return obj;
        }

        public static string GetUpdateXCGToYun(string id)
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = string.Format("update zkhw_temp_xcg set status=1,updatetime='{0}' where id='{1}'", dt, id);
            return sql;
        }
        public static string GetSqlForXCG(xuechangguiBean xcg)
        {
            string sql1 = "select count(id) from zkhw_tj_xcg where timeCodeUnique='" + xcg.timeCodeUnique + "'";
            bool flag = DbHelperMySQL.Exists(sql1);
            string sql = "";
            if (flag == false)
            {
                string id = Result.GetNewId();
                sql = @"insert into zkhw_tj_xcg(ID,aichive_no,id_number,bar_code,HCT,HGB,LYM,LYMP,MCH,MCHC,
                        MCV,MPV,MXD,MXDP,NEUT,NEUTP,PCT,PDW,PLT,RBC,RDWCV,RDWSD,WBC,MONO,MONOP,GRAN,GRANP,
                        PLCR,createtime,ZrysXCG,timeCodeUnique) values('" + id + "','" + xcg.aichive_no + 
                        "','" + xcg.id_number + "','" + xcg.bar_code + "','" + xcg.HCT + "','" + xcg.HGB + 
                        "','" + xcg.LYM + "','" + xcg.LYMP + "','" + xcg.MCH + "','" + xcg.MCHC + "','" + 
                        xcg.MCV + "','" + xcg.MPV + "','" + xcg.MXD + "','" + xcg.MXDP + "','" + xcg.NEUT + 
                        "','" + xcg.NEUTP + "','" + xcg.PCT + "','" + xcg.PDW + "','" + xcg.PLT + "','" + 
                        xcg.RBC + "','" + xcg.RDW_CV + "','" + xcg.RDW_SD + "','" + xcg.WBC + "','" + 
                        xcg.MONO + "','" + xcg.MONOP + "','" + xcg.GRAN + "','" + xcg.GRANP + "','" +
                        xcg.PLCR + "','" + xcg.createTime + "','" + xcg.ZrysXCG + "','" + xcg.timeCodeUnique + "')";
            }
            else
            {
               sql = @"update zkhw_tj_xcg set HCT='" + xcg.HCT + "',HGB='" + xcg.HGB + "',LYM='" + xcg.LYM + 
                    "',LYMP='" + xcg.LYMP + "',MCH='" + xcg.MCH + "',MCHC='" + xcg.MCHC + "',MCV='" + xcg.MCV 
                    + "',MPV='" + xcg.MPV + "',MXD='" + xcg.MXD + "',MXDP='" + xcg.MXDP + "',NEUT='" + 
                    xcg.NEUT + "',NEUTP='" + xcg.NEUTP + "',PCT='" + xcg.PCT + "',PDW='" + xcg.PDW + 
                    "',PLT='" + xcg.PLT + "',RBC='" + xcg.RBC + "',RDWCV='" + xcg.RDW_CV + "',RDWSD='" + 
                    xcg.RDW_SD + "',WBC='" + xcg.WBC + "',MONO='" + xcg.MONO + "',MONOP='" + xcg.MONOP + 
                    "',GRAN='" + xcg.GRAN + "',GRANP='" + xcg.GRANP + "',PLCR='" + xcg.PLCR +
                    "',upload_status=0 where timeCodeUnique = '" + xcg.timeCodeUnique+ "'"; 
            }
            return sql;
        }

        public static string GetUpdatePEXCGInfoSql(xuechangguiBean xcg)
        {
            String sql = @"update physical_examination_record set blood_hemoglobin='" + xcg.HGB + 
                "',blood_leukocyte='" + xcg.WBC + "',blood_platelet='" + xcg.PLT + 
                "' where aichive_no = '" + xcg.aichive_no + "' and bar_code='" + xcg.bar_code + "'";
            return sql;
        }

        public static string GetUpdateBgdcXCGSql(DataTable dttv, xuechangguiBean xcg)
        { 
            int flag = Common.JudgeValueForXCG(dttv, xcg); 
            String sql = "update zkhw_tj_bgdc set XueChangGui='" + flag + "' where aichive_no = '" + xcg.aichive_no + "' and bar_code='" + xcg.bar_code + "'";
            return sql;
        }

    }
}
