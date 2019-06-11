using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.service
{
    class tcmHealthService
    {
        dao.tcmHealthServicesDao hPD = new dao.tcmHealthServicesDao();
        public DataTable querytcmHealthServices(string pCa, string time1, string time2)
        {
            return hPD.querytcmHealthServices(pCa, time1, time2);
        }
        public bool deletetcmHealthServices(string id)
        {
            return hPD.deletetcmHealthServices(id);
        }

        public DataTable checkTcmHealthServicesByno(string code,string idnum)
        {
            return hPD.checkTcmHealthServicesByno(code,idnum);
        }
    }
}
