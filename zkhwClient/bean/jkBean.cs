using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkhwClient.bean
{
    class jkBean
    {   //电子档案编号
        public string aichive_no { get; set; }
        //身份证号
        public string id_number { get; set; }
        //条码编号
        public string bar_code { get; set; }
        //社保卡号
        public string SocialSecuritycode { get; set; }
        //名字
        public string name { get; set; }
        //年龄
        public int age { get; set; }
        //性别
        public string sex { get; set; }
        //体检批次号
        public string batch_no { get; set; }
        //身份证照片地址
        public string Pic1 { get; set; }
        //摄像头拍摄照片地址
        public string Pic2 { get; set; }
        // 村区域编号
        public string village_code { get; set; }
        //户籍地址
        public string address { get; set; }
        //现住地
        public string Xzhudi { get; set; }
        //乡镇街道名称
        public string XzjdName { get; set; }
        //村（局）委会名称
        public string CjwhName { get; set; }
        //建档单位
        public string JddwName { get; set; }
        //建档人
        public string JdrName { get; set; }
        //责任医生
        public string ZrysName { get; set; }
        //操作员
        public string Caozuoyuan { get; set; }
        //司机名称
        public string SjName { get; set; }
        //车牌号
        public string Carcode { get; set; }
        //建档时间
        public string createtime { get; set; }
        //上传状态
        public string upload_status { get; set; }
        //所属机构
        public string duns { get; set; }
    }
}
