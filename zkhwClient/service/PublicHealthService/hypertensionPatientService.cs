using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.service
{
    class hypertensionPatientService
    {
        dao.hypertensionPatientDao hPD = new dao.hypertensionPatientDao();
        public DataTable queryHypertensionPatient(string pCa, string time1, string time2,string code)
        {
            return hPD.queryHypertensionPatient(pCa, time1, time2,code);
        }
        public bool deleteHypertensionPatient(string id)
        {
            return hPD.deleteHypertensionPatient(id);
        }
        public bool aUfuv_hypertension(bean.fuv_hypertensionBean hm,string id,DataTable goodsList)
        {
            return hPD.aUfuv_hypertension(hm,id, goodsList);
        }
        public DataTable queryHypertensionPatient0(string id)
        {
            return hPD.queryHypertensionPatient0(id);
        }
        public DataTable queryFollow_medicine_record(string follow_id)
        {
            return hPD.queryFollow_medicine_record(follow_id);
        }
    }
}
