﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.service
{
    class diabetesPatientService
    {
        dao.diabetesPatientDao hPD = new dao.diabetesPatientDao();
        public DataTable querydiabetesPatient(string patientName, string id_number, string create_name, string dataSate)
        {
            return hPD.querydiabetesPatient(patientName, id_number, create_name, dataSate);
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
