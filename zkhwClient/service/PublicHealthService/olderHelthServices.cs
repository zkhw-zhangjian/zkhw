using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace zkhwClient.service
{
    class olderHelthServices
    {
        dao.olderHelthServiceDao hPD = new dao.olderHelthServiceDao();
        public bool deleteOlderHelthService(string id)
        {
            return hPD.deleteOlderHelthService(id);
        }
        public DataTable queryOlderHelthService(string pCa, string time1, string time2)
        {
            return hPD.queryOlderHelthService(pCa, time1, time2);
        }
        public DataTable queryOlderHelthService0()
        {
            return hPD.queryOlderHelthService0();
        }
    }
}
