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

        public DataTable checkTcmHealthServicesByno1(string code, string idnum,string barcode)
        {
            return hPD.checkTcmHealthServicesByno1(code, idnum, barcode);
        }

        public DataTable checkTcmHealthServicesByExamID(string examid)
        {
            return hPD.checkTcmHealthServicesByExamID(examid);
        }
    }
}
