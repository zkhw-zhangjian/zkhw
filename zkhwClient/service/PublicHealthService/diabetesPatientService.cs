using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.service
{
    class diabetesPatientService
    {
        dao.diabetesPatientDao hPD = new dao.diabetesPatientDao();
        public DataTable querydiabetesPatient(string pCa, string time1, string time2,string code)
        {
            return hPD.querydiabetesPatient(pCa, time1, time2,code);
        }
        public bool deleteDiabetesPatient(string id)
        {
            return hPD.deleteDiabetesPatient(id);
        }
        public bool aUdiabetesPatient(bean.diabetes_follow_recordBean hm, string id, DataTable goodsList)
        {
            return hPD.aUdiabetesPatient(hm, id,goodsList);
        }
        public DataTable queryDiabetesPatient0(string id)
        {
            return hPD.queryDiabetesPatient0(id);
        }
    }
}
