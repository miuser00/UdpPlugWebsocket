///<summary>
         ///模块编号：20180730
         ///模块名：本地配置模块
         ///作用：生成本地的存储信息
         ///作者：Miuser
         ///编写日期：20180730
         ///版本：1.0
///</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Common reference
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
//mongo DB
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography;


namespace UserLogin
{

    public partial class SetupForm : Form
    {
        Config cfg;
        public SetupForm(ref Config config)
        {
            cfg = config;
            InitializeComponent();
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            prg_config.SelectedObject = cfg;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            cfg.SavetoFile("config.xml");
            Application.Restart();

        }

        private void SetupForm_FormClosed(object sender, FormClosedEventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            cfg.ShopList.Add("大岛店[01]");
            cfg.ShopList.Add("洞庭路店[02]");
        }
    }
    public class Config
    {
        [CategoryAttribute("1.MongoDB设置")]
        public String url { get; set; } //192.168.0.33
        [CategoryAttribute("1.MongoDB设置")]
        public String port { get; set; }
        [CategoryAttribute("1.MongoDB设置")]
        public String user { get; set; }

        [CategoryAttribute("1.MongoDB设置")]

        [BrowsableAttribute(false)]
        public String Epass { get; set; }

        [CategoryAttribute("1.MongoDB设置")]
        [PasswordPropertyText(true)]
        [System.Xml.Serialization.XmlIgnore]
        public String pass
        {
            get
            {
                return Encryption.Decode(Epass);
            }
            set
            {
                Epass = Encryption.Encode(value);
            }
        }

        [CategoryAttribute("1.MongoDB设置")]
        public String database { get; set; }

        [CategoryAttribute("2.球机设定")]
        public int GolfPrivilegeTime { get; set; }
        [CategoryAttribute("2.球机设定")]
        public string ShopName { get; set; }
        [CategoryAttribute("2.球机设定")]
        public List<string> ShopList { get; set; }
        public int SavetoFile(String filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                TextWriter writer = new StreamWriter(filename);
                serializer.Serialize(writer, this);
                writer.Close();
            }

            catch (Exception ee)
            {
                MessageBox.Show(ee.StackTrace, ee.Message);
                return 0;
            }

            return 1;
        }
        public static Config LoadfromFile(String filename)
        {
            try
            {
                Config sptr;

                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                TextReader reader = new StreamReader(filename);
                sptr = (Config)(serializer.Deserialize(reader));
                reader.Close();
                return sptr;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.StackTrace, ee.Message);
                return null;
            }

        }

    }
    public class Encryption
    {

        /// <summary>
        /// 作用：将字符串内容转化为16进制数据编码，其逆过程是Decode
        /// 参数说明：
        /// strEncode 需要转化的原始字符串
        /// 转换的过程是直接把字符转换成Unicode字符,比如数字"3"-->0033,汉字"我"-->U+6211
        /// 函数decode的过程是encode的逆过程.
        /// </summary>
        public static string Encode(string strEncode)
        {
            string strReturn = "";//  存储转换后的编码
            try
            {
                foreach (short shortx in strEncode.ToCharArray())
                {
                    strReturn += shortx.ToString("X4");
                }
            }
            catch { }
            return strReturn;
        }

        /// <summary>
        /// 作用：将16进制数据编码转化为字符串，是Encode的逆过程
        /// </summary>
        public static string Decode(string strDecode)
        {
            string sResult = "";
            try
            {
                for (int i = 0; i < strDecode.Length / 4; i++)
                {
                    sResult += (char)short.Parse(strDecode.Substring(i * 4, 4),
                        global::System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch { }
            return sResult;
        }

        /// <summary>
        /// 将数字转换成16进制字符串，后两位加入随机字符，其可逆方法为DecodeForNum
        /// </summary>
        public static string EncodeForNum(int id)
        {
            //用户加上起始位置后的
            int startUserIndex = id;
            //转换成16进制
            string hexStr = Convert.ToString(startUserIndex, 16);

            //后面两位加入随机数
            string randomchars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string tmpstr = "";

            //整除的后得到的数可能大于被除数
            tmpstr += randomchars[(id / randomchars.Length) > randomchars.Length ? randomchars.Length - 1 : (id / randomchars.Length)];

            //余数不可能大于被除数
            tmpstr += randomchars[(id % randomchars.Length) > randomchars.Length ? randomchars.Length - 1 : (id % randomchars.Length)];

            //返回拼接后的字符，转成大写
            string retStr = (hexStr + tmpstr).ToUpper();

            return retStr;
        }

        /// <summary>
        /// 解密16进制字符串，此方法只适合后面两位有随机字符的
        /// </summary>
        public static int DecodeForNum(string strDecode)
        {
            if (strDecode.Length > 2)
            {
                strDecode = strDecode.Substring(0, strDecode.Length - 2);
                return Convert.ToInt32(strDecode, 16);
            }
            return 0;
        }

    }
}
