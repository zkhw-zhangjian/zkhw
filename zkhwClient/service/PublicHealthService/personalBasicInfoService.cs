﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.service
{
    class personalBasicInfoService
    {
        dao.personalBasicInfoDao hPD = new dao.personalBasicInfoDao();
        public DataTable queryPersonalBasicInfo(string name, string id_number, string aichive_no)
        {
            return hPD.queryPersonalBasicInfo(name, id_number, aichive_no);
        }
        public DataTable query(string id_number)
        {
            return hPD.query(id_number);
        }
        public bool deletePersonalBasicInfo(string id)
        {
            return hPD.deletePersonalBasicInfo(id);
        }
        public bool aUpersonalBasicInfo(bean.resident_base_infoBean hm, string id, DataTable goodsList, DataTable goodsList0, DataTable goodsList1, DataTable goodsList2, DataTable goodsList3)
        {
            return hPD.aUpersonalBasicInfo(hm,id, goodsList, goodsList0, goodsList1, goodsList2, goodsList3);
        }
        public DataTable queryPersonalBasicInfo0(string id)
        {
            return hPD.queryPersonalBasicInfo0(id);
        }
        public DataTable queryResident_diseases(string resident_base_info_id)
        {
            return hPD.queryResident_diseases(resident_base_info_id);
        }
        public DataTable queryOperation_record(string resident_base_info_id)
        {
            return hPD.queryOperation_record(resident_base_info_id);
        }
        public DataTable queryTraumatism_record(string resident_base_info_id)
        {
            return hPD.queryTraumatism_record(resident_base_info_id);
        }
        public DataTable queryMetachysis_record(string resident_base_info_id)
        {
            return hPD.queryMetachysis_record(resident_base_info_id);
        }
        public DataTable queryFamily_record(string resident_base_info_id)
        {
            return hPD.queryFamily_record(resident_base_info_id);
        }
        

    }
}
