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
        public DataTable query(string id_number)
        {
            return hPD.query(id_number);
        }
        public bool aUelderly_selfcare_estimate(bean.elderly_selfcare_estimateBean hm, string id)
        {
            return hPD.aUelderly_selfcare_estimate(hm, id);
        }
        public DataTable queryOlderHelthService(string id)
        {
            return hPD.queryOlderHelthService(id);
        }
    }
}
