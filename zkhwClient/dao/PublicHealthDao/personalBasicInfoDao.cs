using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.view.setting;

namespace zkhwClient.dao
{
    class personalBasicInfoDao
    {
        public DataTable queryPersonalBasicInfo(string pCa, string time1, string time2,string code)
        {
            DataSet ds = new DataSet();
            //string sql = "select id,name,archive_no,id_number,create_name,create_time,doctor_name from resident_base_info where create_time >= '" + time1 + "' and create_time <= '" + time2 + "'";
            string sql = @"select id,name,archive_no,id_number,create_name,create_time,
                       doctor_name,age,is_hypertension,is_diabetes ,is_psychosis ,
                       is_tuberculosis,is_poor  from resident_base_info where create_time >= '" + time1 + "' and create_time <= '" + time2 + "'";
            if (code != "") { sql += " AND village_code='" + code + "'"; }
            if (pCa != "") { sql += " and (name like '%" + pCa + "%'  or id_number like '%" + pCa + "%'  or archive_no like '%" + pCa + "%')"; }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable query(string id_number)
        {
            DataSet ds = new DataSet();
            string sql = "select * from resident_base_info where id_number = '" + id_number + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public bool deletePersonalBasicInfo(string id)
        {
            int rt = 0;
            string sql = "delete from resident_base_info where id='" + id + "';delete from resident_diseases  where resident_base_info_id = '" + id + "';delete from operation_record  where resident_base_info_id = '" + id + "';delete from traumatism_record  where resident_base_info_id = '" + id + "';delete from metachysis_record  where resident_base_info_id = '" + id + "';delete from family_record  where resident_base_info_id = '" + id + "';";
            rt = DbHelperMySQL.ExecuteSql(sql);
            return rt == 0 ? false : true;
        }
        public bool aUpersonalBasicInfo(bean.resident_base_infoBean hm, string id, DataTable goodsList, DataTable goodsList0, DataTable goodsList1, DataTable goodsList2, DataTable goodsList3,int intbian)
        {
            int ret = 0;
            String sql = "";
            String sql0 = "";
            String sql1 = "";
            String sql2 = "";
            String sql3 = "";
            String sql4 = "";
            if (id == "")
            {
                #region 新插入
                id = Result.GetNewId();
                sql = @"insert into resident_base_info (id,archive_no,pb_archive,name,sex,birthday,id_number,address,nation,company,phone,link_name,link_phone,resident_type,residence_address,blood_group,blood_rh,education,profession,marital_status,pay_type,pay_other,drug_allergy,allergy_other,exposure,disease_other,is_hypertension,is_diabetes,is_psychosis,is_tuberculosis,is_heredity,heredity_name,is_deformity,deformity_name,is_poor,kitchen,fuel,other_fuel,drink,other_drink,toilet,poultry,medical_code,photo_code,aichive_org,doctor_name,upload_status,upload_time,create_user,create_name,create_time,create_org,create_org_name,is_signing,update_time) values ";
                sql += @" ('" + id + "','" + hm.archive_no + "', '" + hm.pb_archive + "', '" + hm.name + "', '" + hm.sex + "', '" + hm.birthday + "', '" + hm.id_number + "', '" + hm.address + "', '" + hm.nation + "','" + hm.company + "', '" + hm.phone + "', '" + hm.link_name + "', '" + hm.link_phone + "', '" + hm.resident_type + "', '" + hm.address + "', '" + hm.blood_group + "', '" + hm.blood_rh + "', '" + hm.education + "', '" + hm.profession + "','" + hm.marital_status + "', '" + hm.pay_type + "', '" + hm.pay_other + "', '" + hm.drug_allergy + "', '" + hm.allergy_other + "', '" + hm.exposure + "', '" + hm.disease_other + "', '" + hm.is_hypertension + "', '" + hm.is_diabetes + "', '" + hm.is_psychosis + "','" + hm.is_tuberculosis + "', '" + hm.is_heredity + "', '" + hm.heredity_name + "', '" + hm.is_deformity + "', '" + hm.deformity_name + "', '" + hm.is_poor + "', '" + hm.kitchen + "', '" + hm.fuel + "', '" + hm.other_fuel + "', '" + hm.drink + "','" + hm.other_drink + "', '" + hm.toilet + "', '" + hm.poultry + "', '" + hm.medical_code + "', '" + hm.photo_code + "', '" + basicInfoSettings.organ_name + "', '" + hm.is_signing + "', '" + hm.synchro_time + "', '" + hm.create_user + "','" + hm.create_name + "', '" + hm.create_time + "', '" + hm.create_org + "', '" + hm.create_org_name + "',"+hm.is_signing+ "," + hm.update_time + ")";
                if (goodsList.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql0 += "insert into resident_diseases(archive_no,id_number,name,resident_base_info_id,disease_name,disease_date,disease_type) values ('"+ hm.archive_no + "','"+ hm.id_number + "','"+hm.name+"','" + id + "','" + goodsList.Rows[i]["disease_name"] + "','" + goodsList.Rows[i]["disease_date"] + "','" + goodsList.Rows[i]["disease_type"] + "')";

                        }
                        else
                        {
                            sql0 += ",('" + hm.archive_no + "','" + hm.id_number + "','" + hm.name + "','" + id + "','" + goodsList.Rows[i]["disease_name"] + "','" + goodsList.Rows[i]["disease_date"] + "','" + goodsList.Rows[i]["disease_type"] + "')";

                        }
                    }
                }
                if (goodsList0.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList0.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql1 += "insert into operation_record(resident_base_info_id,operation_name,operation_time,operation_code) values ('" + id + "','" + goodsList0.Rows[i]["operation_name"] + "','" + goodsList0.Rows[i]["operation_time"] + "','" + goodsList0.Rows[i]["operation_code"] + "')"; 

                        }
                        else
                        {
                            sql1 += ",('" + id + "','" + goodsList0.Rows[i]["operation_name"] + "','" + goodsList0.Rows[i]["operation_time"] + "','" + goodsList0.Rows[i]["operation_code"] + "')";

                        }
                    }
                }
                if (goodsList1.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList1.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql2 += "insert into traumatism_record(resident_base_info_id,traumatism_name,traumatism_time,traumatism_code) values ('" + id + "','" + goodsList1.Rows[i]["traumatism_name"] + "','" + goodsList1.Rows[i]["traumatism_time"] + "','" + goodsList1.Rows[i]["traumatism_code"] + "')"; 

                        }
                        else
                        {
                            sql2 += ",('" + id + "','" + goodsList1.Rows[i]["traumatism_name"] + "','" + goodsList1.Rows[i]["traumatism_time"] + "','" + goodsList1.Rows[i]["traumatism_code"] + "')";

                        }
                    }
                }
                if (goodsList2.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList2.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql3 += "insert into metachysis_record(resident_base_info_id,metachysis_reasonn,metachysis_time,metachysis_code) values ('" + id + "','" + goodsList2.Rows[i]["metachysis_reasonn"] + "','" + goodsList2.Rows[i]["metachysis_time"] + "','" + goodsList2.Rows[i]["metachysis_code"] + "')"; 

                        }
                        else
                        {
                            sql3 += ",('" + id + "','" + goodsList2.Rows[i]["metachysis_reasonn"] + "','" + goodsList2.Rows[i]["metachysis_time"] + "','" + goodsList2.Rows[i]["metachysis_code"] + "')";

                        }
                    }
                }
                if (goodsList3.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList3.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql4 += "insert into family_record(resident_base_info_id,relation,disease_name,disease_type) values ('" + id + "','" + goodsList3.Rows[i]["relation"] + "','" + goodsList3.Rows[i]["disease_name"] + "','" + goodsList3.Rows[i]["disease_type"] + "')"; 

                        }
                        else
                        {
                            sql4 += ",('" + id + "','" + goodsList3.Rows[i]["relation"] + "','" + goodsList3.Rows[i]["disease_name"] + "','" + goodsList3.Rows[i]["disease_type"] + "')";

                        }
                    }
                }
                #endregion

                ret = DbHelperMySQL.ExecuteSql(sql);
                if (ret == 0) { return false; }
                if (sql0 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql0);
                }
                if (sql1 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql1);
                }
                if (sql2 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql2);
                }
                if (sql3 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql3);
                }
                if (sql4 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql4);
                }

                return ret == 0 ? false : true;
            }
            else
            {
                List<string> _lst = new List<string>();
                #region 更新
                sql = @"update resident_base_info set name='"+hm.name+"', address='"+hm.address+"', id_number='" + hm.id_number + "',nation='" + hm.nation + "',company='" + hm.company + "',phone= '" + hm.phone + "',link_name= '" + hm.link_name + "',link_phone= '" + hm.link_phone + "',resident_type= '" + hm.resident_type + "',residence_address='" + hm.residence_address + "',blood_group= '" + hm.blood_group + "',blood_rh='" + hm.blood_rh + "',education='" + hm.education + "',profession='" + hm.profession + "',marital_status='" + hm.marital_status + "',pay_type='" + hm.pay_type + "',pay_other= '" + hm.pay_other + "',drug_allergy='" + hm.drug_allergy + "',allergy_other='" + hm.allergy_other + "',exposure= '" + hm.exposure + "',disease_other='" + hm.disease_other + "',is_hypertension='" + hm.is_hypertension + "',is_diabetes= '" + hm.is_diabetes + "',is_psychosis='" + hm.is_psychosis + "',is_tuberculosis='" + hm.is_tuberculosis + "',is_heredity= '" + hm.is_heredity + "',heredity_name= '" + hm.heredity_name + "',is_deformity='" + hm.is_deformity + "',deformity_name='" + hm.deformity_name + "',is_poor= '" + hm.is_poor + "',kitchen='" + hm.kitchen + "',fuel='" + hm.fuel + "',other_fuel='" + hm.other_fuel + "',drink= '" + hm.drink + "',other_drink='" + hm.other_drink + "',toilet= '" + hm.toilet + "',poultry= '" + hm.poultry + "',medical_code='" + hm.medical_code + "',is_signing= '" + hm.is_signing + "',update_user='" + hm.update_user + "',update_name='" + hm.update_name + "',update_time='" + hm.update_time + "',upload_status=0 where id = '" + id + "'";
                _lst.Add(sql);

                sql0 = @"delete from resident_diseases  where resident_base_info_id = '" + id + "';";
                sql1 = @"delete from operation_record  where resident_base_info_id = '" + id + "';";
                sql2 = @"delete from traumatism_record  where resident_base_info_id = '" + id + "';";
                sql3 = @"delete from metachysis_record  where resident_base_info_id = '" + id + "';";
                sql4 = @"delete from family_record  where resident_base_info_id = '" + id + "';";
                if (goodsList.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql0 += "insert into resident_diseases(archive_no,id_number,name,resident_base_info_id,disease_name,disease_date,disease_type) values ('" + hm.archive_no + "','" + hm.id_number + "','" + hm.name + "','" + id + "','" + goodsList.Rows[i]["disease_name"] + "','" + goodsList.Rows[i]["disease_date"] + "','" + goodsList.Rows[i]["disease_type"] + "')";

                        }
                        else
                        {
                            sql0 += ",('" + hm.archive_no + "','" + hm.id_number + "','" + hm.name + "','" + id + "','" + goodsList.Rows[i]["disease_name"] + "','" + goodsList.Rows[i]["disease_date"] + "','" + goodsList.Rows[i]["disease_type"] + "')";

                        }
                    }
                }
                if (goodsList0.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList0.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql1 += "insert into operation_record(resident_base_info_id,operation_name,operation_time,operation_code) values ('" + id + "','" + goodsList0.Rows[i]["operation_name"] + "','" + goodsList0.Rows[i]["operation_time"] + "','" + goodsList0.Rows[i]["operation_code"] + "')";

                        }
                        else
                        {
                            sql1 += ",('" + id + "','" + goodsList0.Rows[i]["operation_name"] + "','" + goodsList0.Rows[i]["operation_time"] + "','" + goodsList0.Rows[i]["operation_code"] + "')";

                        }
                    }
                }
                if (goodsList1.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList1.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql2 += "insert into traumatism_record(resident_base_info_id,traumatism_name,traumatism_time,traumatism_code) values ('" + id + "','" + goodsList1.Rows[i]["traumatism_name"] + "','" + goodsList1.Rows[i]["traumatism_time"] + "','" + goodsList1.Rows[i]["traumatism_code"] + "')";

                        }
                        else
                        {
                            sql2 += ",('" + id + "','" + goodsList1.Rows[i]["traumatism_name"] + "','" + goodsList1.Rows[i]["traumatism_time"] + "','" + goodsList1.Rows[i]["traumatism_code"] + "')";

                        }
                    }
                }
                if (goodsList2.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList2.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql3 += "insert into metachysis_record(resident_base_info_id,metachysis_reasonn,metachysis_time,metachysis_code) values ('" + id + "','" + goodsList2.Rows[i]["metachysis_reasonn"] + "','" + goodsList2.Rows[i]["metachysis_time"] + "','" + goodsList2.Rows[i]["metachysis_code"] + "')";

                        }
                        else
                        {
                            sql3 += ",('" + id + "','" + goodsList2.Rows[i]["metachysis_reasonn"] + "','" + goodsList2.Rows[i]["metachysis_time"] + "','" + goodsList2.Rows[i]["metachysis_code"] + "')";

                        }
                    }
                }
                if (goodsList3.Rows.Count > 0)
                {
                    for (int i = 0; i < goodsList3.Rows.Count; i++)
                    {
                        string relation = "";
                        if (goodsList3.Rows[i]["relation"].ToString() == "父亲")
                        {
                            relation = "1";
                        }
                        else if (goodsList3.Rows[i]["relation"].ToString() == "母亲")
                        {
                            relation = "2";
                        }
                        else if (goodsList3.Rows[i]["relation"].ToString() == "兄弟姐妹")
                        {
                            relation = "3";
                        }
                        else if (goodsList3.Rows[i]["relation"].ToString() == "子女")
                        {
                            relation = "4";
                        }
                        if (i == 0)
                        {
                            sql4 += "insert into family_record(resident_base_info_id,relation,disease_name,disease_type) values ('" + id + "','" + relation + "','" + goodsList3.Rows[i]["disease_name"] + "','" + goodsList3.Rows[i]["disease_type"] + "')";
                        }
                        else
                        {
                            sql4 += ",('" + id + "','" + relation + "','" + goodsList3.Rows[i]["disease_name"] + "','" + goodsList3.Rows[i]["disease_type"] + "')";

                        }
                    }
                }
                #endregion
                #region 身份证、名字变化就要改变别的表
                if(intbian>=2 )
                {
                    sql =string.Format("update zkhw_tj_bc set id_number='{0}' Where aichive_no='{1}'", hm.id_number,hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update zkhw_tj_ncg set id_number='{0}' Where aichive_no='{1}'", hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update zkhw_tj_sgtz set id_number='{0}' Where aichive_no='{1}'", hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update zkhw_tj_sh set id_number='{0}' Where aichive_no='{1}'", hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update zkhw_tj_xcg set id_number='{0}' Where aichive_no='{1}'", hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update zkhw_tj_xdt set id_number='{0}' Where aichive_no='{1}'", hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update zkhw_tj_xy set id_number='{0}' Where aichive_no='{1}'", hm.id_number, hm.archive_no);
                    _lst.Add(sql);  
                }

                if(intbian>0)
                {
                    sql = string.Format("update physical_examination_record set name='{0}', id_number='{1}' Where aichive_no='{2}'",hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update children_health_record set name='{0}', id_number='{1}' Where archive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update children_tcm_record set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update diabetes_follow_record set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update elderly_selfcare_estimate set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update elderly_tcm_record set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update fuv_hypertension set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update gravida_after_record set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update gravida_follow_record set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update gravida_info set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update neonatus_info set name='{0}', id_number='{1}' Where archive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update poor_follow_record set name='{0}', id_number='{1}' Where archive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update psychosis_info set name='{0}', id_number='{1}' Where archive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update sign_service_info set name='{0}', id_number='{1}' Where archive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update tuberculosis_follow_record set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update tuberculosis_info set name='{0}', id_number='{1}' Where archive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update zkhw_tj_bgdc set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                    sql = string.Format("update zkhw_tj_jk set name='{0}', id_number='{1}' Where aichive_no='{2}'", hm.name, hm.id_number, hm.archive_no);
                    _lst.Add(sql);

                }
                #endregion
                ret = DbHelperMySQL.ExecuteSqlTran(_lst);

                if (ret == 0) { return false; }
                if (sql0 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql0);
                }
                if (sql1 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql1);
                }
                if (sql2 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql2);
                }
                if (sql3 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql3);
                }
                if (sql4 != "")
                {
                    DbHelperMySQL.ExecuteSql(sql4);
                }

                return ret == 0 ? false : true;

            }

        }
        public DataTable queryPersonalBasicInfo0(string id)
        {
            DataSet ds = new DataSet();
            string sql = "select * from resident_base_info where id = '" + id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryResident_diseases(string resident_base_info_id)
        {
            DataSet ds = new DataSet();
            string sql = "select id,resident_base_info_id,disease_name,disease_date,disease_type from resident_diseases where resident_base_info_id = '" + resident_base_info_id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryOperation_record(string resident_base_info_id)
        {
            DataSet ds = new DataSet();
            string sql = "select id,resident_base_info_id,operation_name,operation_time,operation_code from operation_record where resident_base_info_id = '" + resident_base_info_id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryTraumatism_record(string resident_base_info_id)
        {
            DataSet ds = new DataSet();
            string sql = "select id,resident_base_info_id,traumatism_name,traumatism_time,traumatism_code from traumatism_record where resident_base_info_id = '" + resident_base_info_id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryMetachysis_record(string resident_base_info_id)
        {
            DataSet ds = new DataSet();
            string sql = "select id,resident_base_info_id,metachysis_reasonn,metachysis_time,metachysis_code from metachysis_record where resident_base_info_id = '" + resident_base_info_id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        public DataTable queryFamily_record(string resident_base_info_id)
        {
            DataSet ds = new DataSet();
            string sql = "select id,resident_base_info_id,relation,disease_name,disease_type from family_record where resident_base_info_id = '" + resident_base_info_id + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        


    }
}

