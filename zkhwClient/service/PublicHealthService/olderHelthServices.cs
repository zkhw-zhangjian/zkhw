﻿using System;
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
        public DataTable queryOlderHelthService(string pCa, string time1, string time2,string code)
        {
            return hPD.queryOlderHelthService(pCa, time1, time2,code);
        }
        public DataTable queryOlderHelthService0()
        {
            return hPD.queryOlderHelthService0();
        }
        public DataTable query(string archive_no)
        {
            return hPD.query(archive_no);
        }
        public DataTable query1(string archive_no,string barcode)
        {
            return hPD.query1(archive_no, barcode);
        }
        public DataTable queryForExamID(string examid)
        {
            return hPD.queryForExamID(examid);
        }
        public bool aUelderly_selfcare_estimate(bean.elderly_selfcare_estimateBean hm, string id)
        {
            return hPD.aUelderly_selfcare_estimate(hm, id);
        }
        public bool aUelderly_selfcare_estimateForExamID(bean.elderly_selfcare_estimateBean hm, string id)
        {
            return hPD.aUelderly_selfcare_estimateForExamID(hm, id);
        }
        public DataTable queryOlderHelthService(string id)
        {
            return hPD.queryOlderHelthService(id);
        }
        public DataTable queryOlderHelthServiceForExamID(string id)
        {
            return hPD.queryOlderHelthServiceForExamID(id);
        }
        public DataTable queryOlderHelthService1(string pCa, string time1, string time2, string code)
        {
            return hPD.queryOlderHelthService1(pCa, time1, time2, code);
        }

    }
}
