using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.HomeDoctorSigningView
{
    public partial class onSiteSigning : Form
    {
        public onSiteSigning()
        {
            InitializeComponent();
            ComboBoxBin();
        }
        private void ComboBoxBin()
        {
            string sql = "select ID,TuanDuiMingCheng from zkhw_qy_tdcy where TuanDuiMingCheng is not null";
            DataSet datas = DbHelperMySQL.Query(sql);
            if (datas != null && datas.Tables.Count > 0)
            {
                //List<TDMC> ts = Result.ToDataList<TDMC>(datas.Tables[0]);
                //Result.Bind(comboBox1, ts, "TuanDuiMingCheng", "ID", "--请选择--");
            }
        }
    }
}
