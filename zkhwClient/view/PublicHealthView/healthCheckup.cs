using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkhwClient.dao;
using zkhwClient.view.setting;

namespace zkhwClient.view.PublicHealthView
{
    public partial class healthCheckup : Form
    {
        healthCheckupDao hcd = new healthCheckupDao();
        areaConfigDao areadao = new areaConfigDao();
        string str = Application.StartupPath;//项目路径
        public string time1 = null;
        public string time2 = null;
        public string pCa = "";
        string xcuncode = "";
        string xzcode = null;
        string qxcode = null;
        string shicode = null;
        string shengcode = null;
        string TarStr = "yyyy-MM-dd";
        IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
        public healthCheckup()
        {
            InitializeComponent();
        }

        private void healthCheckup_Load(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            //区域
            this.comboBox1.DataSource = areadao.shengInfo();//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "code";//操作时获取的值 
            xcuncode = basicInfoSettings.xcuncode;
            queryOlderHelthService();
        }

        private void queryOlderHelthService()
        {
            //展示
            time1 = this.dateTimePicker1.Text.ToString();//开始时间
            time2 = this.dateTimePicker2.Text.ToString();//结束时间
            this.dataGridView1.DataSource = null;
            //DataTable dt = hcd.queryhealthCheckup(pCa, time1, time2,xcuncode);
            DataTable dt = hcd.queryhealthCheckupUploadstatus(pCa, time1, time2, xcuncode);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderCell.Value = "姓名";
            this.dataGridView1.Columns[1].HeaderCell.Value = "档案编号";
            this.dataGridView1.Columns[2].HeaderCell.Value = "身份证号";
            this.dataGridView1.Columns[3].HeaderCell.Value = "条码号";
            this.dataGridView1.Columns[4].HeaderCell.Value = "检查日期";
            this.dataGridView1.Columns[5].HeaderCell.Value = "责任医生";
            this.dataGridView1.Columns[6].Visible = false;
            this.dataGridView1.Columns[7].HeaderCell.Value = "是否上传";

            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pCa = this.textBox1.Text;
            if (pCa != "")
            {
                this.label5.Text = "";
            }
            else { this.label5.Text = "---姓名/身份证号/档案号---"; }
            queryOlderHelthService();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.label5.Visible = this.textBox1.Text.Length < 1;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            shengcode = this.comboBox1.SelectedValue.ToString();
            this.comboBox2.DataSource = areadao.shiInfo(shengcode);//绑定数据源
            this.comboBox2.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox2.ValueMember = "code";//操作时获取的值 
            this.comboBox3.DataSource = null;
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            shicode = this.comboBox2.SelectedValue.ToString();
            this.comboBox3.DataSource = areadao.quxianInfo(shicode);//绑定数据源
            this.comboBox3.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox3.ValueMember = "code";//操作时获取的值 
            this.comboBox4.DataSource = null;
            this.comboBox5.DataSource = null;
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            qxcode = this.comboBox3.SelectedValue.ToString();
            this.comboBox4.DataSource = areadao.zhenInfo(qxcode);//绑定数据源
            this.comboBox4.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox4.ValueMember = "code";//操作时获取的值 
            this.comboBox5.DataSource = null;
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            xzcode = this.comboBox4.SelectedValue.ToString();
            this.comboBox5.DataSource = areadao.cunInfo(xzcode);//绑定数据源
            this.comboBox5.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox5.ValueMember = "code";//操作时获取的值
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            xcuncode = this.comboBox5.SelectedValue.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string name = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string aichive_no = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string id_number = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string bar_code = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string check_date = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                string doctor_name = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                //if (id==null||"".Equals(id)) { MessageBox.Show("未查询到此人的健康体检信息,请调整时间间隔，再点击查询！！"); return; }
                DataTable dtup= hcd.queryhealthCheckup(id);
                if (dtup.Rows.Count>0) { MessageBox.Show(name+"已有健康体检基本信息，请点击修改按钮!");return; }
                if (aichive_no != null && !"".Equals(aichive_no))
                {
                    aUhealthcheckupServices1 auhcs = new aUhealthcheckupServices1();
                    auhcs.textBox1.Text = name;
                    auhcs.textBox118.Text = bar_code;
                    auhcs.textBox119.Text = id_number;
                    auhcs.textBox120.Text = id;
                    auhcs.textBox2.Text = aichive_no;
                    if (check_date != "")
                    {
                        auhcs.dateTimePicker1.Value = DateTime.ParseExact(check_date, TarStr, format);
                    }
                    auhcs.textBox51.Text = doctor_name;
                    auhcs.id = id;
                    auhcs.Show();
                    //aUhealthcheckupServices3 auhcs = new aUhealthcheckupServices3();
                    //auhcs.id = id;
                    //auhcs.textBox107.Text = id_number;
                    //auhcs.textBox106.Text = aichive_no;
                    //auhcs.textBox105.Text = bar_code;
                    //auhcs.textBox108.Text = id;
                    //auhcs.Show();
                }
            }
            else {
                MessageBox.Show("请选择一行！");
            }
        }
        //删除数据
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count < 1) { MessageBox.Show("未选中任何行！"); return; }
            string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            DialogResult rr = MessageBox.Show("确认删除？", "确认删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            int tt = (int)rr;
            if (tt == 1)
            {//删除用户       
                bool istrue = hcd.deletePhysical_examination_record(id);
                if (istrue)
                {
                    hcd.deleteVaccination_record(id);
                    hcd.deleteTake_medicine_record(id);
                    hcd.deleteHospitalized_record(id);
                    //刷新页面
                    queryOlderHelthService();
                    MessageBox.Show("删除成功！");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string name = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string aichive_no = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string id_number = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string bar_code = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string check_date = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                string doctor_name = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                if (id == null || "".Equals(id)) { MessageBox.Show("未查询到此人的健康体检信息,请调整时间间隔，再点击查询！"); return; }
                if (aichive_no != null && !"".Equals(aichive_no))
                { 
                    aUhealthcheckupServices1 auhcs = new aUhealthcheckupServices1();
                    auhcs.textBox1.Text = name;
                    auhcs.textBox118.Text = bar_code;
                    auhcs.textBox119.Text = id_number;
                    auhcs.textBox120.Text = id;
                    auhcs.textBox2.Text = aichive_no;
                    if (check_date != "")
                    {
                        auhcs.dateTimePicker1.Value = DateTime.ParseExact(check_date, TarStr, format);
                    }
                    auhcs.textBox51.Text = doctor_name;

                    auhcs.id = id;//祖
                    auhcs.Show(); 
                }
            }
            else
            {
                MessageBox.Show("请选择一行！");
            }
        }

        #region 数据上传
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string name = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string aichive_no = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string id_number = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string bar_code = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string check_date = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                string doctor_name = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                string _cstatus= this.dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                if (id == null || "".Equals(id)) { MessageBox.Show("未查询到此人的健康体检信息,请调整时间间隔，再点击查询！"); return; }
                //根据ID进入 physical_examination_record 查找数据 
                if(_cstatus=="是")
                {
                    MessageBox.Show("数据已经上传！");
                    return;
                }
                //否则数据上传 查找对应的信息
                DataTable dtup = hcd.queryhealthCheckup(id);
                if(dtup !=null && dtup.Rows.Count>0)
                {
                    #region 以前的判断方式
                    //string _healthAdvice = dtup.Rows[0]["healthAdvice"].ToString();
                    //string _checkdate = dtup.Rows[0]["check_date"].ToString();
                    //if (_healthAdvice == "" || _checkdate == "")
                    //{
                    //    MessageBox.Show("还未检查完成不能上传！");
                    //    return;
                    //}
                    #endregion
                    #region 现在判断方式
                    string strempty = "";
                    string _id= dtup.Rows[0]["id"].ToString(); 
                    string _checkdate = dtup.Rows[0]["check_date"].ToString(); 
                    string _dutydoctor = dtup.Rows[0]["dutydoctor"].ToString();
                    if (id == "" || _checkdate == "" || _dutydoctor == "")
                    {
                        MessageBox.Show("还未完成问询不能上传！");
                        return;
                    }
                     
                    string _symptom = dtup.Rows[0]["symptom"].ToString();
                    if(_symptom=="")
                    {
                         strempty = "症状"; 
                    }
                    string _basetemperature = dtup.Rows[0]["base_temperature"].ToString();
                    if (_basetemperature == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "体温";
                        }
                        else
                        {
                            strempty = strempty + "、体温";
                        }
                    }
                    string _baseheartbeat = dtup.Rows[0]["base_heartbeat"].ToString();
                    if (_baseheartbeat == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "脉率";
                        }
                        else
                        {
                            strempty = strempty + "、脉率";
                        }
                    }
                    string _baserespiratory = dtup.Rows[0]["base_respiratory"].ToString();
                    if (_baserespiratory == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "呼吸频率";
                        }
                        else
                        {
                            strempty = strempty + "、呼吸频率";
                        }
                    }
                    string _basebloodpressureleftlow = dtup.Rows[0]["base_blood_pressure_left_low"].ToString();
                    if (_basebloodpressureleftlow == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "左侧舒张压";
                        }
                        else
                        {
                            strempty = strempty + "、左侧舒张压";
                        }
                    }
                    string _basebloodpressurelefthigh = dtup.Rows[0]["base_blood_pressure_left_high"].ToString();
                    if (_basebloodpressurelefthigh == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "左侧收缩压";
                        }
                        else
                        {
                            strempty = strempty + "、左侧收缩压";
                        }
                    }
                    string _basebloodpressurerightlow = dtup.Rows[0]["base_blood_pressure_right_low"].ToString();
                    if (_basebloodpressurerightlow == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "右侧舒张压";
                        }
                        else
                        {
                            strempty = strempty + "、右侧舒张压";
                        }
                    }
                    string _basebloodpressurerighthigh = dtup.Rows[0]["base_blood_pressure_right_high"].ToString();
                    if (_basebloodpressurerighthigh == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "右侧收缩压";
                        }
                        else
                        {
                            strempty = strempty + "、右侧收缩压";
                        }
                    }
                    string _baseheight = dtup.Rows[0]["base_height"].ToString();
                    if (_baseheight == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "身高";
                        }
                        else
                        {
                            strempty = strempty + "、身高";
                        }
                    }
                    string _baseweight = dtup.Rows[0]["base_weight"].ToString();
                    if (_baseweight == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "体重";
                        }
                        else
                        {
                            strempty = strempty + "、体重";
                        }
                    }
                    string _basewaist = dtup.Rows[0]["base_waist"].ToString();
                    if (_basewaist == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "腰围";
                        }
                        else
                        {
                            strempty = strempty + "、腰围";
                        }
                    }
                    string _basebmi = dtup.Rows[0]["base_bmi"].ToString();
                    if (_basebmi == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "体质指数";
                        }
                        else
                        {
                            strempty = strempty + "、体质指数";
                        }
                    }
                    string _lifewayexercisfrequency = dtup.Rows[0]["lifeway_exercise_frequency"].ToString();
                    if (_lifewayexercisfrequency == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "锻炼频率";
                        }
                        else
                        {
                            strempty = strempty + "、锻炼频率";
                        }
                    }
                    string _lifewaydiet = dtup.Rows[0]["lifeway_diet"].ToString();
                    if (_lifewaydiet == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "饮食习惯";
                        }
                        else
                        {
                            strempty = strempty + "、饮食习惯";
                        }
                    }
                    string _lifewaysmokestatus = dtup.Rows[0]["lifeway_smoke_status"].ToString();
                    if (_lifewaysmokestatus == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "吸烟状况";
                        }
                        else
                        {
                            strempty = strempty + "、吸烟状况";
                        }
                    }
                    string _lifewaydrinkstatus = dtup.Rows[0]["lifeway_drink_status"].ToString();
                    if (_lifewaydrinkstatus == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "饮酒频率";
                        }
                        else
                        {
                            strempty = strempty + "、饮酒频率";
                        }
                    }
                    string _lifewayoccupationaldisease = dtup.Rows[0]["lifeway_occupational_disease"].ToString();
                    if (_lifewayoccupationaldisease == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "有无职业暴露";
                        }
                        else
                        {
                            strempty = strempty + "、有无职业暴露";
                        }
                    }
                    string _organlips = dtup.Rows[0]["organ_lips"].ToString();
                    if (_organlips == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "口唇";
                        }
                        else
                        {
                            strempty = strempty + "、口唇";
                        }
                    }
                    string _organtooth = dtup.Rows[0]["organ_tooth"].ToString();
                    if (_organtooth == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "齿列";
                        }
                        else
                        {
                            strempty = strempty + "、齿列";
                        }
                    }
                    string _organguttur = dtup.Rows[0]["organ_guttur"].ToString();
                    if (_organguttur == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "咽部";
                        }
                        else
                        {
                            strempty = strempty + "、咽部";
                        }
                    }
                    string _organvisionleft = dtup.Rows[0]["organ_vision_left"].ToString();
                    if (_organvisionleft == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "左眼视力";
                        }
                        else
                        {
                            strempty = strempty + "、左眼视力";
                        }
                    }
                    string _organvisionright = dtup.Rows[0]["organ_vision_right"].ToString();
                    if (_organvisionright == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "右眼视力";
                        }
                        else
                        {
                            strempty = strempty + "、右眼视力";
                        }
                    }
                    string _organhearing = dtup.Rows[0]["organ_hearing"].ToString();
                    if (_organhearing == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "听力";
                        }
                        else
                        {
                            strempty = strempty + "、听力";
                        }
                    }
                    string _organmovement = dtup.Rows[0]["organ_movement"].ToString();
                    if (_organmovement == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "运动功能";
                        }
                        else
                        {
                            strempty = strempty + "、运动功能";
                        }
                    }
                    string _examinationskin = dtup.Rows[0]["examination_skin"].ToString();
                    if (_examinationskin == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "皮肤";
                        }
                        else
                        {
                            strempty = strempty + "、皮肤";
                        }
                    }
                    string _examinationsclera = dtup.Rows[0]["examination_sclera"].ToString();
                    if (_examinationsclera == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "巩膜";
                        }
                        else
                        {
                            strempty = strempty + "、巩膜";
                        }
                    }
                    string _examinationlymph = dtup.Rows[0]["examination_lymph"].ToString();
                    if (_examinationlymph == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "淋巴结";
                        }
                        else
                        {
                            strempty = strempty + "、淋巴结";
                        }
                    }
                    string _examinationbarrelchest = dtup.Rows[0]["examination_barrel_chest"].ToString();
                    if (_examinationbarrelchest == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "桶状胸";
                        }
                        else
                        {
                            strempty = strempty + "、桶状胸";
                        }
                    }
                    string _examinationbreathsounds = dtup.Rows[0]["examination_breath_sounds"].ToString();
                    if (_examinationbreathsounds == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "呼吸音";
                        }
                        else
                        {
                            strempty = strempty + "、呼吸音";
                        }
                    }
                    string _examinationrale = dtup.Rows[0]["examination_rale"].ToString();
                    if (_examinationrale == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "罗音";
                        }
                        else
                        {
                            strempty = strempty + "、罗音";
                        }
                    }
                    string _examinationheartrate = dtup.Rows[0]["examination_heart_rate"].ToString();
                    if (_examinationheartrate == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "心率";
                        }
                        else
                        {
                            strempty = strempty + "、心率";
                        }
                    }
                    string _examinationheartrhythm = dtup.Rows[0]["examination_heart_rhythm"].ToString();
                    if (_examinationheartrhythm == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "心律";
                        }
                        else
                        {
                            strempty = strempty + "、心律";
                        }
                    }
                    string _examinationheartnoise = dtup.Rows[0]["examination_heart_noise"].ToString();
                    if (_examinationheartnoise == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "杂音";
                        }
                        else
                        {
                            strempty = strempty + "、杂音";
                        }
                    }
                    string _examinationabdomentenderness = dtup.Rows[0]["examination_abdomen_tenderness"].ToString();
                    if (_examinationabdomentenderness == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "压痛";
                        }
                        else
                        {
                            strempty = strempty + "、压痛";
                        }
                    }
                    string _examinationabdomenmass = dtup.Rows[0]["examination_abdomen_mass"].ToString();
                    if (_examinationabdomenmass == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "包块";
                        }
                        else
                        {
                            strempty = strempty + "、包块";
                        }
                    }
                    string _examinationabdomenhepatomegaly = dtup.Rows[0]["examination_abdomen_hepatomegaly"].ToString();
                    if (_examinationabdomenhepatomegaly == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "肝大";
                        }
                        else
                        {
                            strempty = strempty + "、肝大";
                        }
                    }
                    string _examinationabdomensplenomegaly = dtup.Rows[0]["examination_abdomen_splenomegaly"].ToString();
                    if (_examinationabdomensplenomegaly == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "脾大";
                        }
                        else
                        {
                            strempty = strempty + "、脾大";
                        }
                    }
                    string _examinationabdomenshiftingdullness = dtup.Rows[0]["examination_abdomen_shiftingdullness"].ToString();
                    if (_examinationabdomenshiftingdullness == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "移动性浊音";
                        }
                        else
                        {
                            strempty = strempty + "、移动性浊音";
                        }
                    }
                    string _examinationlowerextremityedema = dtup.Rows[0]["examination_lowerextremity_edema"].ToString();
                    if (_examinationlowerextremityedema == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "下肢水肿";
                        }
                        else
                        {
                            strempty = strempty + "、下肢水肿";
                        }
                    }
                    string _examinationdorsalartery = dtup.Rows[0]["examination_dorsal_artery"].ToString();
                    if (_examinationdorsalartery == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "足背动脉搏动";
                        }
                        else
                        {
                            strempty = strempty + "、足背动脉搏动";
                        }
                    }
                    string _healthevaluation = dtup.Rows[0]["health_evaluation"].ToString();
                    if (_healthevaluation == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "健康评价";
                        }
                        else
                        {
                            strempty = strempty + "、健康评价";
                        }
                    }
                    string _healthAdvice = dtup.Rows[0]["healthAdvice"].ToString();
                    if (_healthAdvice == "")
                    {
                        if (strempty == "")
                        {
                            strempty = "健康建议";
                        }
                        else
                        {
                            strempty = strempty + "、健康建议";
                        }
                    }

                    #endregion
                    if(strempty!="")
                    {
                        string s = "以下字段未填：" + strempty + " 是否继续？";
                        DialogResult rr = MessageBox.Show(s, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        int tt = (int)rr;
                        if (tt!=1)
                        {
                            return;
                        }
                    } 

                    List<string> sqllist = new List<string>();
                    //1：住院记录表
                    string sql = string.Format("select * from hospitalized_record where exam_id='{0}';", id);
                    DataSet zyjl = DbHelperMySQL.Query(sql);
                    if (zyjl != null && zyjl.Tables.Count > 0 && zyjl.Tables[0].Rows.Count > 0)
                    {
                        DataTable data1 = zyjl.Tables[0]; 
                        for (int j = 0; j < data1.Rows.Count; j++)
                        {
                            sqllist.Add($@"delete from hospitalized_record where id={Ifnull(data1.Rows[j]["id"])};");  //先把对应ID的删掉在写入新的
                            sqllist.Add($@"insert into hospitalized_record (id,exam_id,archive_no,id_number,service_name,hospitalized_type,in_hospital_time,leave_hospital_time,reason,hospital_organ,case_code,remark,create_org,create_name,create_time) 
                             values({Ifnull(data1.Rows[j]["id"])},{Ifnull(data1.Rows[j]["exam_id"])},{Ifnull(data1.Rows[j]["archive_no"])},{Ifnull(data1.Rows[j]["id_number"])},{Ifnull(data1.Rows[j]["service_name"])},{Ifnull(data1.Rows[j]["hospitalized_type"])},{Ifnull(data1.Rows[j]["in_hospital_time"])},{Ifnull(data1.Rows[j]["leave_hospital_time"])},{Ifnull(data1.Rows[j]["reason"])},{Ifnull(data1.Rows[j]["hospital_organ"])},{Ifnull(data1.Rows[j]["case_code"])},{Ifnull(data1.Rows[j]["remark"])},{Ifnull(data1.Rows[j]["create_org"])},{Ifnull(data1.Rows[j]["create_name"])},{Ifnull(Convert.ToDateTime(data1.Rows[j]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");                            
                        }
                    }
                    //2：用药记录表
                    sql = string.Format("select * from take_medicine_record where exam_id='{0}';", id);
                    DataSet yyjl = DbHelperMySQL.Query(sql);
                    if (yyjl != null && yyjl.Tables.Count > 0 && yyjl.Tables[0].Rows.Count > 0)
                    {
                        DataTable data2 = yyjl.Tables[0];
                        for (int k = 0; k < data2.Rows.Count; k++)
                        {
                            sqllist.Add($@"delete from take_medicine_record where id={Ifnull(data2.Rows[k]["id"])};");
                            sqllist.Add($@"insert into take_medicine_record (id,exam_id,archive_no,id_number,service_name,medicine_type,medicine_name,medicine_usage,frequency,medicine_dosage,unit,medicine_time,medicine_time_unit,medicine_compliance,other,create_org,create_name,create_time) 
                            values({Ifnull(data2.Rows[k]["id"])},{Ifnull(data2.Rows[k]["exam_id"])},{Ifnull(data2.Rows[k]["archive_no"])},{Ifnull(data2.Rows[k]["id_number"])},{Ifnull(data2.Rows[k]["service_name"])},{Ifnull(data2.Rows[k]["medicine_type"])},{Ifnull(data2.Rows[k]["medicine_name"])},{Ifnull(data2.Rows[k]["medicine_usage"])},{Ifnull(data2.Rows[k]["frequency"])},{Ifnull(data2.Rows[k]["medicine_dosage"])},{Ifnull(data2.Rows[k]["unit"])},{Ifnull(data2.Rows[k]["medicine_time"])},{Ifnull(data2.Rows[k]["medicine_time_unit"])},{Ifnull(data2.Rows[k]["medicine_compliance"])},{Ifnull(data2.Rows[k]["other"])},{Ifnull(data2.Rows[k]["create_org"])},{Ifnull(data2.Rows[k]["create_name"])},{Ifnull(Convert.ToDateTime(data2.Rows[k]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                         
                        }
                    }
                    //3：疫苗记录表
                    sql = string.Format("select * from vaccination_record where exam_id='{0}';", id);
                    DataSet ymjl = DbHelperMySQL.Query(sql);
                    if (ymjl != null && ymjl.Tables.Count > 0 && ymjl.Tables[0].Rows.Count > 0)
                    {
                        DataTable data3 = ymjl.Tables[0]; 
                        for (int a = 0; a < data3.Rows.Count; a++)
                        {
                            sqllist.Add($@"delete from vaccination_record where id={Ifnull(data3.Rows[a]["id"])};");
                            sqllist.Add($@"insert into vaccination_record (id,exam_id,archive_no,id_number,service_name,card_id,vaccination_type,vaccination_id,vaccination_name,acuscount,dose,descnption,inocu_state,sinocu_date,vaccination_time,inocu_doctor,register_person,dzjgm,batch_number,county,inoculation_site,inoculation_way,vaccination_organ,vaccination_organ_name,remark,validdate,manufacturer,manufact_code,create_name,create_time) 
                            values({Ifnull(data3.Rows[a]["id"])},{Ifnull(data3.Rows[a]["exam_id"])},{Ifnull(data3.Rows[a]["archive_no"])},{Ifnull(data3.Rows[a]["id_number"])},{Ifnull(data3.Rows[a]["service_name"])},{Ifnull(data3.Rows[a]["card_id"])},{Ifnull(data3.Rows[a]["vaccination_type"])},{Ifnull(data3.Rows[a]["vaccination_id"])},{Ifnull(data3.Rows[a]["vaccination_name"])},{Ifnull(data3.Rows[a]["acuscount"])},{Ifnull(data3.Rows[a]["dose"])},{Ifnull(data3.Rows[a]["descnption"])},{Ifnull(data3.Rows[a]["inocu_state"])},{Ifnull(data3.Rows[a]["sinocu_date"])},{Ifnull(data3.Rows[a]["vaccination_time"])},{Ifnull(data3.Rows[a]["inocu_doctor"])},{Ifnull(data3.Rows[a]["register_person"])},{Ifnull(data3.Rows[a]["dzjgm"])},{Ifnull(data3.Rows[a]["batch_number"])},{Ifnull(data3.Rows[a]["county"])},{Ifnull(data3.Rows[a]["inoculation_site"])},{Ifnull(data3.Rows[a]["inoculation_way"])},{Ifnull(data3.Rows[a]["vaccination_organ"])},{Ifnull(data3.Rows[a]["vaccination_organ_name"])},{Ifnull(data3.Rows[a]["remark"])},{Ifnull(data3.Rows[a]["validdate"])},{Ifnull(data3.Rows[a]["manufacturer"])},{Ifnull(data3.Rows[a]["manufact_code"])},{Ifnull(data3.Rows[a]["create_name"])},{Ifnull(Convert.ToDateTime(data3.Rows[a]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                        }
                    }
                    //健康体检表
                    sqllist.Add($@"delete from physical_examination_record where id={Ifnull(dtup.Rows[0]["id"])};");

                    sqllist.Add($@"insert into physical_examination_record (id,name,archive_no,id_number,batch_no,bar_code,dutydoctor,symptom,symptom_other,check_date,base_temperature,base_heartbeat,base_respiratory,base_blood_pressure_left_high,base_blood_pressure_left_low,base_blood_pressure_right_high,base_blood_pressure_right_low,base_height,base_weight,base_waist,base_bmi,base_health_estimate,base_selfcare_estimate,base_cognition_estimate,base_cognition_score,base_feeling_estimate,base_feeling_score,base_doctor,lifeway_exercise_frequency,lifeway_exercise_time,lifeway_exercise_year,lifeway_exercise_type,lifeway_diet,lifeway_smoke_status,lifeway_smoke_number,lifeway_smoke_startage,lifeway_smoke_endage,lifeway_drink_status,lifeway_drink_number,lifeway_drink_stop,lifeway_drink_stopage,lifeway_drink_startage,lifeway_drink_oneyear,lifeway_drink_type,lifeway_drink_other,lifeway_occupational_disease,lifeway_job,lifeway_job_period,lifeway_hazardous_dust,lifeway_dust_preventive,lifeway_hazardous_radiation,lifeway_radiation_preventive,lifeway_hazardous_physical,lifeway_physical_preventive,lifeway_hazardous_chemical,lifeway_chemical_preventive,lifeway_hazardous_other,lifeway_other_preventive,lifeway_doctor,organ_lips,organ_tooth,organ_hypodontia,organ_hypodontia_topleft,organ_hypodontia_topright,organ_hypodontia_bottomleft,organ_hypodontia_bottomright,organ_caries,organ_caries_topleft,organ_caries_topright,organ_caries_bottomleft,organ_caries_bottomright,organ_denture,organ_denture_topleft,organ_denture_topright,organ_denture_bottomleft,organ_denture_bottomright,organ_guttur,organ_vision_left,organ_vision_right,organ_correctedvision_left,organ_correctedvision_right,organ_hearing,organ_movement,organ_doctor,examination_eye,examination_eye_other,examination_skin,examination_skin_other,examination_sclera,examination_sclera_other,examination_lymph,examination_lymph_other,examination_barrel_chest,examination_breath_sounds,examination_breath_other,examination_rale,examination_rale_other,examination_heart_rate,examination_heart_rhythm,examination_heart_noise,examination_noise_other,examination_abdomen_tenderness,examination_tenderness_memo,examination_abdomen_mass,examination_mass_memo,examination_abdomen_hepatomegaly,examination_hepatomegaly_memo,examination_abdomen_splenomegaly,examination_splenomegaly_memo,examination_abdomen_shiftingdullness,examination_shiftingdullness_memo,examination_lowerextremity_edema,examination_dorsal_artery,examination_anus,examination_anus_other,examination_breast,examination_breast_other,examination_doctor,examination_woman_vulva,examination_vulva_memo,examination_woman_vagina,examination_vagina_memo,examination_woman_cervix,examination_cervix_memo,examination_woman_corpus,examination_corpus_memo,examination_woman_accessories,examination_accessories_memo,examination_woman_doctor,examination_other,blood_hemoglobin,blood_leukocyte,blood_platelet,blood_other,urine_protein,glycosuria,urine_acetone_bodies,bld,urine_other,blood_glucose_mmol,blood_glucose_mg,cardiogram,cardiogram_memo,cardiogram_img,microalbuminuria,fob,glycosylated_hemoglobin,hb,sgft,ast,albumin,total_bilirubin,conjugated_bilirubin,scr,blood_urea,blood_k,blood_na,tc,tg,ldl,hdl,chest_x,x_memo,chestx_img,ultrasound_abdomen,ultrasound_memo,abdomenB_img,other_b,otherb_memo,otherb_img,cervical_smear,cervical_smear_memo,other,cerebrovascular_disease,cerebrovascular_disease_other,kidney_disease,kidney_disease_other,heart_disease,heart_disease_other,vascular_disease,vascular_disease_other,ocular_diseases,ocular_diseases_other,nervous_system_disease,nervous_disease_memo,other_disease,other_disease_memo,health_evaluation,abnormal1,abnormal2,abnormal3,abnormal4,health_guidance,danger_controlling,target_weight,advise_bacterin,danger_controlling_other,health_advice,create_user,create_name,create_org,create_org_name,create_time)
                    values('{dtup.Rows[0]["id"]}','{dtup.Rows[0]["name"]}','{dtup.Rows[0]["aichive_no"]}','{dtup.Rows[0]["id_number"]}','{dtup.Rows[0]["batch_no"]}','{dtup.Rows[0]["bar_code"]}',
                     '{dtup.Rows[0]["dutydoctor"]}','{dtup.Rows[0]["symptom"]}','{dtup.Rows[0]["symptom_other"]}','{dtup.Rows[0]["check_date"]}','{dtup.Rows[0]["base_temperature"]}','{dtup.Rows[0]["base_heartbeat"]}','{dtup.Rows[0]["base_respiratory"]}',
                     {Ifnull(dtup.Rows[0]["base_blood_pressure_left_high"])},{Ifnull(dtup.Rows[0]["base_blood_pressure_left_low"])},{Ifnull(dtup.Rows[0]["base_blood_pressure_right_high"])},
                     {Ifnull(dtup.Rows[0]["base_blood_pressure_right_low"])},'{dtup.Rows[0]["base_height"]}','{dtup.Rows[0]["base_weight"]}',
                     '{dtup.Rows[0]["base_waist"]}','{dtup.Rows[0]["base_bmi"]}','{dtup.Rows[0]["base_health_estimate"]}','{dtup.Rows[0]["base_selfcare_estimate"]}',
                    '{dtup.Rows[0]["base_cognition_estimate"]}','{dtup.Rows[0]["base_cognition_score"]}','{dtup.Rows[0]["base_feeling_estimate"]}',
                    '{dtup.Rows[0]["base_feeling_score"]}','{dtup.Rows[0]["base_doctor"]}','{dtup.Rows[0]["lifeway_exercise_frequency"]}','{dtup.Rows[0]["lifeway_exercise_time"]}','{dtup.Rows[0]["lifeway_exercise_year"]}','{dtup.Rows[0]["lifeway_exercise_type"]}','{dtup.Rows[0]["lifeway_diet"]}','{dtup.Rows[0]["lifeway_smoke_status"]}','{dtup.Rows[0]["lifeway_smoke_number"]}','{dtup.Rows[0]["lifeway_smoke_startage"]}','{dtup.Rows[0]["lifeway_smoke_endage"]}','{dtup.Rows[0]["lifeway_drink_status"]}','{dtup.Rows[0]["lifeway_drink_number"]}','{dtup.Rows[0]["lifeway_drink_stop"]}',
                    '{dtup.Rows[0]["lifeway_drink_stopage"]}','{dtup.Rows[0]["lifeway_drink_startage"]}','{dtup.Rows[0]["lifeway_drink_oneyear"]}','{dtup.Rows[0]["lifeway_drink_type"]}','{dtup.Rows[0]["lifeway_drink_other"]}','{dtup.Rows[0]["lifeway_occupational_disease"]}','{dtup.Rows[0]["lifeway_job"]}',
                    '{dtup.Rows[0]["lifeway_job_period"]}','{dtup.Rows[0]["lifeway_hazardous_dust"]}','{dtup.Rows[0]["lifeway_dust_preventive"]}','{dtup.Rows[0]["lifeway_hazardous_radiation"]}','{dtup.Rows[0]["lifeway_radiation_preventive"]}','{dtup.Rows[0]["lifeway_hazardous_physical"]}','{dtup.Rows[0]["lifeway_physical_preventive"]}','{dtup.Rows[0]["lifeway_hazardous_chemical"]}','{dtup.Rows[0]["lifeway_chemical_preventive"]}','{dtup.Rows[0]["lifeway_hazardous_other"]}','{dtup.Rows[0]["lifeway_other_preventive"]}','{dtup.Rows[0]["lifeway_doctor"]}','{dtup.Rows[0]["organ_lips"]}','{dtup.Rows[0]["organ_tooth"]}','{dtup.Rows[0]["organ_hypodontia"]}','{dtup.Rows[0]["organ_hypodontia_topleft"]}',
                    '{dtup.Rows[0]["organ_hypodontia_topright"]}','{dtup.Rows[0]["organ_hypodontia_bottomleft"]}','{dtup.Rows[0]["organ_hypodontia_bottomright"]}','{dtup.Rows[0]["organ_caries"]}','{dtup.Rows[0]["organ_caries_topleft"]}','{dtup.Rows[0]["organ_caries_topright"]}',
                    '{dtup.Rows[0]["organ_caries_bottomleft"]}','{dtup.Rows[0]["organ_caries_bottomright"]}','{dtup.Rows[0]["organ_denture"]}','{dtup.Rows[0]["organ_denture_topleft"]}','{dtup.Rows[0]["organ_denture_topright"]}','{dtup.Rows[0]["organ_denture_bottomleft"]}','{dtup.Rows[0]["organ_denture_bottomright"]}','{dtup.Rows[0]["organ_guttur"]}','{dtup.Rows[0]["organ_vision_left"]}','{dtup.Rows[0]["organ_vision_right"]}','{dtup.Rows[0]["organ_correctedvision_left"]}','{dtup.Rows[0]["organ_correctedvision_right"]}','{dtup.Rows[0]["organ_hearing"]}','{dtup.Rows[0]["organ_movement"]}','{dtup.Rows[0]["organ_doctor"]}','{dtup.Rows[0]["examination_eye"]}','{dtup.Rows[0]["examination_eye_other"]}','{dtup.Rows[0]["examination_skin"]}',
                    '{dtup.Rows[0]["examination_skin_other"]}','{dtup.Rows[0]["examination_sclera"]}','{dtup.Rows[0]["examination_sclera_other"]}','{dtup.Rows[0]["examination_lymph"]}','{dtup.Rows[0]["examination_lymph_other"]}','{dtup.Rows[0]["examination_barrel_chest"]}','{dtup.Rows[0]["examination_breath_sounds"]}','{dtup.Rows[0]["examination_breath_other"]}','{dtup.Rows[0]["examination_rale"]}','{dtup.Rows[0]["examination_rale_other"]}','{dtup.Rows[0]["examination_heart_rate"]}','{dtup.Rows[0]["examination_heart_rhythm"]}','{dtup.Rows[0]["examination_heart_noise"]}','{dtup.Rows[0]["examination_noise_other"]}','{dtup.Rows[0]["examination_abdomen_tenderness"]}','{dtup.Rows[0]["examination_tenderness_memo"]}','{dtup.Rows[0]["examination_abdomen_mass"]}','{dtup.Rows[0]["examination_mass_memo"]}','{dtup.Rows[0]["examination_abdomen_hepatomegaly"]}','{dtup.Rows[0]["examination_hepatomegaly_memo"]}','{dtup.Rows[0]["examination_abdomen_splenomegaly"]}','{dtup.Rows[0]["examination_splenomegaly_memo"]}','{dtup.Rows[0]["examination_abdomen_shiftingdullness"]}','{dtup.Rows[0]["examination_shiftingdullness_memo"]}','{dtup.Rows[0]["examination_lowerextremity_edema"]}','{dtup.Rows[0]["examination_dorsal_artery"]}','{dtup.Rows[0]["examination_anus"]}','{dtup.Rows[0]["examination_anus_other"]}','{dtup.Rows[0]["examination_breast"]}','{dtup.Rows[0]["examination_breast_other"]}','{dtup.Rows[0]["examination_doctor"]}','{dtup.Rows[0]["examination_woman_vulva"]}','{dtup.Rows[0]["examination_vulva_memo"]}','{dtup.Rows[0]["examination_woman_vagina"]}','{dtup.Rows[0]["examination_vagina_memo"]}','{dtup.Rows[0]["examination_woman_cervix"]}','{dtup.Rows[0]["examination_cervix_memo"]}','{dtup.Rows[0]["examination_woman_corpus"]}','{dtup.Rows[0]["examination_corpus_memo"]}','{dtup.Rows[0]["examination_woman_accessories"]}','{dtup.Rows[0]["examination_accessories_memo"]}','{dtup.Rows[0]["examination_woman_doctor"]}','{dtup.Rows[0]["examination_other"]}','{dtup.Rows[0]["blood_hemoglobin"]}','{dtup.Rows[0]["blood_leukocyte"]}','{dtup.Rows[0]["blood_platelet"]}','{dtup.Rows[0]["blood_other"]}','{dtup.Rows[0]["urine_protein"]}','{dtup.Rows[0]["glycosuria"]}','{dtup.Rows[0]["urine_acetone_bodies"]}','{dtup.Rows[0]["bld"]}','{dtup.Rows[0]["urine_other"]}','{dtup.Rows[0]["blood_glucose_mmol"]}','{dtup.Rows[0]["blood_glucose_mg"]}','{dtup.Rows[0]["cardiogram"]}','{dtup.Rows[0]["cardiogram_memo"]}','{dtup.Rows[0]["cardiogram_img"]}','{dtup.Rows[0]["microalbuminuria"]}','{dtup.Rows[0]["fob"]}','{dtup.Rows[0]["glycosylated_hemoglobin"]}','{dtup.Rows[0]["hb"]}','{dtup.Rows[0]["sgft"]}','{dtup.Rows[0]["ast"]}','{dtup.Rows[0]["albumin"]}','{dtup.Rows[0]["total_bilirubin"]}','{dtup.Rows[0]["conjugated_bilirubin"]}','{dtup.Rows[0]["scr"]}','{dtup.Rows[0]["blood_urea"]}','{dtup.Rows[0]["blood_k"]}','{dtup.Rows[0]["blood_na"]}','{dtup.Rows[0]["tc"]}','{dtup.Rows[0]["tg"]}','{dtup.Rows[0]["ldl"]}','{dtup.Rows[0]["hdl"]}','{dtup.Rows[0]["chest_x"]}','{dtup.Rows[0]["chestx_memo"]}','{dtup.Rows[0]["chestx_img"]}','{dtup.Rows[0]["ultrasound_abdomen"]}','{dtup.Rows[0]["ultrasound_memo"]}','{dtup.Rows[0]["abdomenB_img"]}','{dtup.Rows[0]["other_b"]}','{dtup.Rows[0]["otherb_memo"]}','{dtup.Rows[0]["otherb_img"]}','{dtup.Rows[0]["cervical_smear"]}','{dtup.Rows[0]["cervical_smear_memo"]}','{dtup.Rows[0]["other"]}','{dtup.Rows[0]["cerebrovascular_disease"]}','{dtup.Rows[0]["cerebrovascular_disease_other"]}','{dtup.Rows[0]["kidney_disease"]}','{dtup.Rows[0]["kidney_disease_other"]}',
                    '{dtup.Rows[0]["heart_disease"]}','{dtup.Rows[0]["heart_disease_other"]}','{dtup.Rows[0]["vascular_disease"]}','{dtup.Rows[0]["vascular_disease_other"]}','{dtup.Rows[0]["ocular_diseases"]}','{dtup.Rows[0]["ocular_diseases_other"]}','{dtup.Rows[0]["nervous_system_disease"]}','{dtup.Rows[0]["nervous_disease_memo"]}','{dtup.Rows[0]["other_disease"]}','{dtup.Rows[0]["other_disease_memo"]}','{dtup.Rows[0]["health_evaluation"]}',
                    '{dtup.Rows[0]["abnormal1"]}','{dtup.Rows[0]["abnormal2"]}','{dtup.Rows[0]["abnormal3"]}','{dtup.Rows[0]["abnormal4"]}','{dtup.Rows[0]["health_guidance"]}','{dtup.Rows[0]["danger_controlling"]}','{dtup.Rows[0]["target_weight"]}','{dtup.Rows[0]["advise_bacterin"]}','{dtup.Rows[0]["danger_controlling_other"]}',
                    '{dtup.Rows[0]["healthAdvice"]}','{dtup.Rows[0]["create_user"]}','{dtup.Rows[0]["create_name"]}','{dtup.Rows[0]["create_org"]}','{dtup.Rows[0]["create_org_name"]}',{Ifnull(Convert.ToDateTime(dtup.Rows[0]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))});");
                    
                    int ret = DbHelperMySQL.ExecuteSqlTranYpt(sqllist);
                    if (ret > 0)
                    {
                        MessageBox.Show("上传成功！");
                        sql = string.Format("update physical_examination_record set  upload_status=1 where   id={0};", Ifnull(dtup.Rows[0]["id"]));
                        ret = 0;
                        ret = DbHelperMySQL.ExecuteSql(sql);
                        dataGridView1.SelectedRows[0].Cells[7].Value = "是";
                        dataGridView1.Refresh();
                    } 
                    else
                    {
                        MessageBox.Show("上传失败！");
                    }
                }
                else
                {
                    MessageBox.Show("无信息不能上传！");
                }
            }
        }

        private string Ifnull(object dataRow)
        {
            if (Convert.IsDBNull(dataRow))
            {
                return "NULL";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(dataRow.ToString()))
                {
                    return "'" + dataRow.ToString() + "'";
                }
                else
                {
                    return "NULL";
                }
            }
        }
        #endregion

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < e.RowCount; i++)
            {
                dataGridView1.Rows[e.RowIndex + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Rows[e.RowIndex + i].HeaderCell.Value = (e.RowIndex + i + 1).ToString();
            }

            for (int i = e.RowIndex + e.RowCount; i < this.dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string name = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string aichive_no = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string id_number = this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string bar_code = this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string check_date = this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                string doctor_name = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                string id = this.dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                if (id == null || "".Equals(id)) { MessageBox.Show("未查询到此人的健康体检信息,请调整时间间隔，再点击查询！"); return; }
                if (aichive_no != null && !"".Equals(aichive_no))
                {
                    aUhealthcheckupServices1 auhcs = new aUhealthcheckupServices1();
                    auhcs.textBox1.Text = name;
                    auhcs.textBox118.Text = bar_code;
                    auhcs.textBox119.Text = id_number;
                    auhcs.textBox120.Text = id;
                    auhcs.textBox2.Text = aichive_no;
                    if (check_date != "")
                    {
                        auhcs.dateTimePicker1.Value = DateTime.ParseExact(check_date, TarStr, format);
                    }
                    auhcs.textBox51.Text = doctor_name;

                    auhcs.id = id;//祖
                    auhcs.Show(); 
                }
            }
            else
            {
                MessageBox.Show("请选择一行！");
            }
        }
    }
}
