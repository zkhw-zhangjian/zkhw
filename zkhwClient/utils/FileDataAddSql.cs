using System;
using System.IO;
using System.Xml;
using System.Data;
using zkhwClient.dao;
using System.Data.OleDb;
using System.Windows.Forms;

namespace zkhwClient
{
    /// <summary>
    /// 监控文档
    /// </summary>
    public class FileDataAddSql
    {
        static tjcheckDao tjdao = new tjcheckDao();
        static jkInfoDao jkdao = new jkInfoDao();
        static private OleDbDataAdapter oda = null;
        static private DataSet myds_data = null;
        static string shenghuapath = "";
        static string xuechangguipath = "";
        static string lasttime = "";
        static XmlDocument xmlDoc = new XmlDocument();
        static string path = @"config.xml";
        static XmlNode node;

        public static void OnChangedForxcg(object source, FileSystemEventArgs e)
        {
            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/xuechangguiPath");
            xuechangguipath = node.InnerText;

            xmlDoc.Load(path);
            node = xmlDoc.SelectSingleNode("config/xcglasttime");
            lasttime = node.InnerText;

            // string sql1 = "select lop.patient_id,lop.send_time,lopr.* from LisOutput lop, LisOutputResult lopr where lop.sample_id=lopr.sample_id and lop.sample_id=(select top 1 l.sample_id from LisOutput l order by l.sample_id desc)";
            string sql1 = "select sample_id,patient_id,send_time from LisOutput where send_time > cdate('" + lasttime + "')";
            DataTable arr_dt1 = getXuechanggui(sql1).Tables[0];
            if (arr_dt1.Rows.Count > 0)
            {
                for (int j = 0; j < arr_dt1.Rows.Count; j++)
                {
                string sql2 = "select lop.patient_id,lop.send_time,lopr.* from LisOutput lop, LisOutputResult lopr where lop.sample_id=lopr.sample_id and lop.sample_id='" + arr_dt1.Rows[j]["sample_id"].ToString() + "'";
                DataTable arr_dt2 = getXuechanggui(sql2).Tables[0];
                    if (arr_dt2.Rows.Count > 0)
                    {
                        xuechangguiBean xcg = new xuechangguiBean();
                        xcg.bar_code = arr_dt1.Rows[0]["patient_id"].ToString();
                        DataTable dtjkinfo = jkdao.selectjkInfoBybarcode(xcg.bar_code);
                        if (dtjkinfo != null && dtjkinfo.Rows.Count > 0)
                        {
                            xcg.aichive_no = dtjkinfo.Rows[0]["aichive_no"].ToString();
                            xcg.id_number = dtjkinfo.Rows[0]["id_number"].ToString();
                        }
                        else
                        {
                            return;
                        }

                        DateTime newtime = Convert.ToDateTime(arr_dt1.Rows[0]["send_time"].ToString());
                        DateTime oldtime = Convert.ToDateTime(lasttime);
                        if (newtime <= oldtime)
                        {
                            return;
                        }
                        xcg.createTime = newtime.ToString("yyyy-MM-dd HH:mm:ss");
                        for (int i = 0; i < arr_dt2.Rows.Count; i++)
                        {
                            string item = arr_dt2.Rows[i]["item"].ToString();
                            switch (item)
                            {
                                case "HCT": xcg.HCT = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "HGB": xcg.HGB = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "LYM#": xcg.LYM = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "LYM%": xcg.LYMP = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "MCH": xcg.MCH = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "MCHC": xcg.MCHC = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "MCV": xcg.MCV = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "MPV": xcg.MPV = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "MXD#": xcg.MXD = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "MXD%": xcg.MXDP = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "NEUT#": xcg.NEUT = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "NEUT%": xcg.NEUTP = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "PCT": xcg.PCT = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "PDW": xcg.PDW = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "PLT": xcg.PLT = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "RBC": xcg.RBC = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "RDW_CV": xcg.RDW_CV = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "RDW_SD": xcg.RDW_SD = arr_dt2.Rows[i]["result"].ToString(); break;
                                case "WBC": xcg.WBC = arr_dt2.Rows[i]["result"].ToString(); break;
                                default: break;
                            }
                        }

                        bool istrue = tjdao.insertXuechangguiInfo(xcg);
                        if (istrue)
                        {
                            xmlDoc.Load(path);
                            XmlNode node;
                            node = xmlDoc.SelectSingleNode("config/xcglasttime");
                            node.InnerText = xcg.createTime;
                            xmlDoc.Save(path);
                            tjdao.updateTJbgdcXuechanggui(xcg.aichive_no, xcg.bar_code, 1);
                            tjdao.updatePEXcgInfo(xcg.aichive_no, xcg.bar_code, xcg.HGB, xcg.WBC, xcg.PLT);
                        }
                    }
                }
            }
            else {
                //MessageBox.Show("");
            }
        }
        /// <summary>
        /// 生化表
        /// </summary>
        //public static DataSet getShenghua(string strSQL)
        //{
        //    string strcon = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + shenghuapath + "";
        //    myds_data = new DataSet();
        //    oda = new OleDbDataAdapter(strSQL, strcon);
        //    oda.Fill(myds_data);
        //    return myds_data;
        //}
        /// <summary>
        /// 血球表
        /// </summary>
        public static DataSet getXuechanggui(string strSQL)
        {
            string strcon = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + xuechangguipath + "";
            myds_data = new DataSet();
            oda = new OleDbDataAdapter(strSQL, strcon);
            oda.Fill(myds_data);
            return myds_data;
        }
    }
}
