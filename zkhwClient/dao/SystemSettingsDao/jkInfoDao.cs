﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.bean;

namespace zkhwClient.dao
{
    class jkInfoDao
    {
        //根据条码号获取对应的身份证号和档案编号（取最新的一条记录）
        public DataTable selectjkInfoBybarcode(string barcode)
        {
            DataSet ds = new DataSet();
            string sql = "select k.aichive_no,k.id_number,k.bar_code from zkhw_tj_jk k where k.bar_code='" + barcode + "' order by k.createtime desc limit 1";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //登记界面右侧 查询统计男女各多少人功能
        public DataTable selectjktjInfo(string areacode,string time)
        {
            DataSet ds = new DataSet();
            string sql = "select * from zkhw_tj_jk k where k.village_code='" + areacode + "' and k.createtime >= '" + time + "'";
            ds = DbHelperMySQL.Query(sql);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else {
                return null;
            }
        }
        //查询体检进度
        public DataTable querytjjd(string time1, string time2,string xcuncode, string jmxx)
        {
            DataSet ds = new DataSet();
            string sql = @"select healthchecktime,name,aichive_no,id_number,bar_code,BChao,XinDian,ShengHua,
                       XueChangGui,NiaoChangGui,XueYa,Shengaotizhong,jktjb,lnrzlnlpg,lnrzytzbs,age  
                       from zkhw_tj_bgdc where date_format(createtime,'%Y-%m-%d') >= '" + time1 + "' and date_format(createtime,'%Y-%m-%d') <= '" + time2 + "'";
            if (xcuncode!=null&&!"".Equals(xcuncode)) {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (jmxx != null && !"".Equals(jmxx)) {
                sql += " and name like '%" + jmxx + "%' or aichive_no like '%" + jmxx + "%' or id_number like '%" + jmxx + "%'";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        //查询体检进度生成pdf花名册  获取未完成的人数
        public DataTable querytjjdTopdf(string xcuncode, string time)
        {
            DataSet ds = new DataSet();
            string sql = @"select name,(case sex when '1' then '男' when '2' then '女' ELSE ''END) sex,birthday,
                     (case when BChao>='1' and XinDian>='1' and ShengHua>='1' and XueChangGui>='1' 
                     and NiaoChangGui>='1' and XueYa>='1' and Shengaotizhong>='1' and jktjb>='1' and lnrzlnlpg>='1' and lnrzytzbs>='1' then '完成' else '未完成' end) as type
                     ,BChao,XinDian,ShengHua,XueChangGui,NiaoChangGui,XueYa,Shengaotizhong,jktjb,lnrzlnlpg,lnrzytzbs from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time != null && !"".Equals(time))
            {
                sql += " and createtime >='" + time + "'";
            }
            if(sql !="")
            {
                sql += " order by convert(type using GBK) DESC,convert(name using GBK) ASC ";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable querytjjdTopdf(string xcuncode, string time1,string time2)
        {
            DataSet ds = new DataSet();
            string sql = @"select name,(case sex when '1' then '男' when '2' then '女' ELSE ''END) sex,birthday,age
                     ,BChao,XinDian,ShengHua,XueChangGui,NiaoChangGui,XueYa,Shengaotizhong,jktjb,lnrzlnlpg,lnrzytzbs from zkhw_tj_bgdc where 1=1";
            if (xcuncode != null && !"".Equals(xcuncode))
            {
                sql += " and area_duns='" + xcuncode + "'";
            }
            if (time1 != null && !"".Equals(time1))
            {
                sql += " and date_format(createtime,'%Y-%m-%d') >='" + time1 + "'";
            }
            if (time2 != null && !"".Equals(time2))
            {
                sql += " and date_format(createtime,'%Y-%m-%d') <='" + time2 + "'";
            }
            if (sql != "")
            {
                sql += " order by convert(name using GBK) ASC ";
            }
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        //判断心电图是否有重复数据
        public DataTable queryChongfuXdtData(string aichive_no, string bar_code)
        {
            DataSet ds = new DataSet();
            string sql = "select id from zkhw_tj_xdt where aichive_no = '" + aichive_no + "' and bar_code = '" + bar_code + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
        //判断B超是否有重复数据
        public DataTable queryChongfuBcData (string aichive_no, string bar_code)
        {
            DataSet ds = new DataSet();
            string sql = "select id from zkhw_tj_bc where aichive_no = '" + aichive_no + "' and bar_code = '" + bar_code + "'";
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }

        public DataTable queryGeRenArchivesInfo(string s)
        {
            DataSet ds = new DataSet();
            string sql = string.Format(@"select r.id,r.archive_no,r.name,
                         (case r.sex when '1'then '男' when '2' then '女' when '9' then '未说明的性别' when '0' then '未知的性别' ELSE ''
                         END)  sex    ,r.birthday,
                         r.age,r.id_number,z.bar_code,z.BChao,z.XinDian,z.XueChangGui,
                         z.NiaoChangGui,z.Shengaotizhong,z.XueYa,z.ShengHua,z.jktjb,z.lnrzlnlpg,
                         z.lnrzytzbs,z.healthchecktime from resident_base_info r left join zkhw_tj_bgdc z 
                        on r.id_number= z.id_number {0} order by CONVERT(r.name using  gbk) ASC", s);
            ds = DbHelperMySQL.Query(sql);
            return ds.Tables[0];
        }
    }
}
