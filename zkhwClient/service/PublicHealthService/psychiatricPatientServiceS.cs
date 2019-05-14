using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace zkhwClient.service
{
    class psychiatricPatientServiceS
    {
        dao.psychiatricPatientServicesDaoS hPD = new dao.psychiatricPatientServicesDaoS();
        public DataTable queryPsychosis_follow_record(string pCa, string time1, string time2, string xcuncode)
        {
            return hPD.queryPsychosis_follow_record(pCa, time1, time2, xcuncode);
        }
        public bool deletePsychosis_info(string id)
        {
            return hPD.deletePsychosis_info(id);
        }
        public bool aUPsychosis_follow_record(bean.psychosis_follow_recordBean hm, string id)
        {
            return hPD.aUPsychosis_follow_record(hm, id);
        }
        public bool aUPsychosis_follow_record(bean.psychosis_follow_recordBean hm, string id, DataTable goodsList)
        {
            return hPD.aUPsychosis_follow_record(hm, id, goodsList);
        }
        public DataTable queryPsychosis_follow_record(string id)
        {
            return hPD.queryPsychosis_follow_record(id);
        }
        public DataTable queryPsychosis_follow_record0(string archive_no)
        {
            return hPD.queryPsychosis_follow_record0(archive_no);
        }
        public DataTable queryFollow_medicine_record(string follow_id)
        {
            return hPD.queryFollow_medicine_record(follow_id);
        }
    }
}
