using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkhwClient.dao;

namespace zkhwClient.view.updateTjResult
{
    public partial class updateXuechanggui : Form
    {
        public string time = "";
        public string name = "";
        public string aichive_no = "";
        public string id_number = "";
        public string bar_code = "";
        bool flag = false;
        tjcheckDao tjdao = new tjcheckDao();
        public updateXuechanggui()
        {
            InitializeComponent();
        }
        
        private void updateBichao_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = name;
            this.textBox3.Text = time;
            this.textBox9.Text = aichive_no;
            this.textBox4.Text = id_number;
            this.textBox2.Text = bar_code;
            DataTable dtbichao = tjdao.selectXuechangguiInfo(aichive_no, bar_code);
            if (dtbichao != null && dtbichao.Rows.Count > 0)
            {
                   flag = true;
                this.textBox5.Text = dtbichao.Rows[0]["WBC"].ToString();
                this.textBox6.Text = dtbichao.Rows[0]["RBC"].ToString();
                this.textBox8.Text = dtbichao.Rows[0]["PCT"].ToString();
                this.textBox7.Text = dtbichao.Rows[0]["PLT"].ToString();
                this.textBox11.Text = dtbichao.Rows[0]["HGB"].ToString();
                this.textBox10.Text = dtbichao.Rows[0]["HCT"].ToString();
                this.textBox13.Text = dtbichao.Rows[0]["MCV"].ToString();
                this.textBox12.Text = dtbichao.Rows[0]["MCH"].ToString();
                this.textBox15.Text = dtbichao.Rows[0]["MCHC"].ToString();
                this.textBox14.Text = dtbichao.Rows[0]["RDWCV"].ToString();
                this.textBox17.Text = dtbichao.Rows[0]["RDWSD"].ToString();
                this.textBox16.Text = dtbichao.Rows[0]["MONO"].ToString();
                this.textBox19.Text = dtbichao.Rows[0]["MONOP"].ToString();
                this.textBox18.Text = dtbichao.Rows[0]["GRAN"].ToString();
                this.textBox21.Text = dtbichao.Rows[0]["GRANP"].ToString();
                this.textBox20.Text = dtbichao.Rows[0]["NEUT"].ToString();
                this.textBox23.Text = dtbichao.Rows[0]["NEUTP"].ToString();
                this.textBox22.Text = dtbichao.Rows[0]["EO"].ToString();
                this.textBox25.Text = dtbichao.Rows[0]["EOP"].ToString();
                this.textBox24.Text = dtbichao.Rows[0]["BASO"].ToString();
                this.textBox29.Text = dtbichao.Rows[0]["BASOP"].ToString();
                this.textBox28.Text = dtbichao.Rows[0]["LYM"].ToString();
                this.textBox31.Text = dtbichao.Rows[0]["LYMP"].ToString();
                this.textBox30.Text = dtbichao.Rows[0]["MPV"].ToString();
                this.textBox33.Text = dtbichao.Rows[0]["PDW"].ToString();
                this.textBox32.Text = dtbichao.Rows[0]["MXD"].ToString();
                this.textBox35.Text = dtbichao.Rows[0]["MXDP"].ToString();
                this.textBox34.Text = dtbichao.Rows[0]["PLCR"].ToString();
                this.textBox36.Text = dtbichao.Rows[0]["OTHERS"].ToString();
            }
            else {
                flag = false;
                MessageBox.Show("未查询到数据!");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (flag) {
                string WBC= this.textBox5.Text;
                string RBC =  this.textBox6.Text;
                string PCT = this.textBox8.Text;
                string PLT = this.textBox7.Text;
                string HGB = this.textBox11.Text;
                string HCT = this.textBox10.Text;
                string MCV = this.textBox13.Text;
                string MCH = this.textBox12.Text;
                string MCHC = this.textBox15.Text;
                string RDWCV = this.textBox14.Text;
                string RDWSD = this.textBox17.Text;
                string MONO = this.textBox16.Text;
                string MONOP = this.textBox19.Text;
                string GRAN = this.textBox18.Text;
                string GRANP = this.textBox21.Text;
                string NEUT = this.textBox20.Text;
                string NEUTP = this.textBox23.Text;
                string EO = this.textBox22.Text;
                string EOP = this.textBox25.Text;
                string BASO = this.textBox24.Text;
                string BASOP = this.textBox29.Text;
                string LYM = this.textBox28.Text ;
                string LYMP = this.textBox31.Text ;
                string MPV = this.textBox30.Text;
                string PDW = this.textBox33.Text;
                string MXD = this.textBox32.Text;
                string MXDP = this.textBox35.Text;
                string PLCR = this.textBox34.Text;
                string OTHERS = this.textBox36.Text;
                bool istrue= tjdao.updateXuechangguiInfo(aichive_no, bar_code, WBC, RBC, PCT, PLT, HGB, HCT, MCV, MCH, MCHC, RDWCV, RDWSD, MONO, MONOP, GRAN, GRANP, NEUT, NEUTP, EO, EOP, BASO, BASOP, LYM, LYMP, MPV, PDW, MXD, MXDP, PLCR, OTHERS);
                if (istrue)
                {
                    MessageBox.Show("数据保存成功!");
                }
                else {
                    MessageBox.Show("数据保存失败!");
                }
            }
        }

    }
}
