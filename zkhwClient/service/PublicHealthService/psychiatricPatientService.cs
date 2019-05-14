using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace zkhwClient.service
{
    class psychiatricPatientService
    {
        dao.psychiatricPatientServicesDao hPD = new dao.psychiatricPatientServicesDao();
        public DataTable queryPsychosis_info(string pCa, string time1, string time2, string xcuncode)
        {
            return hPD.queryPsychosis_info(pCa, time1, time2, xcuncode);
        }
        public bool deletePsychosis_info(string id)
        {
            return hPD.deletePsychosis_info(id);
        }
        public bool aUpsychosis_info(bean.psychosis_infoBean hm, string id)
        {
            return hPD.aUpsychosis_info(hm, id);
        }
        public DataTable queryPsychosis_info(string id)
        {
            return hPD.queryPsychosis_info(id);
        }
    }
}
