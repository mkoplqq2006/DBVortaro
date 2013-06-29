using System;
using System.IO;
using log4net;
using log4net.Config;
using NHibernate;
using NHibernate.Cfg;
using System.Text;
using System.Security.Cryptography;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// Nhibernate处理
    /// </summary>
    public class NHibernateHelper
    {
        private static readonly ISessionFactory SessionFactory;
        public static readonly ILog loginfo;   //选择<logger name="loginfo">的配置
        public static readonly ILog logerror;   //选择<logger name="logerror">的配置
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };//DES密钥向量
        private static byte[] Keys2 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };//AES密钥向量

        static NHibernateHelper()
        {
            loginfo = LogManager.GetLogger("loginfo");
            logerror = LogManager.GetLogger("logerror");
            SetConfig();
            Configuration cfg = new Configuration();
            cfg.AddAssembly("VortaroModel");
            SessionFactory = cfg.BuildSessionFactory();
        }
        /// <summary>
        /// 获得当前NHibernate实例
        /// </summary>
        /// <returns></returns>
        public static ISession GetCurrentSession()
        {
            return SessionFactory.OpenSession();
        }
        /// <summary>
        /// 关闭NHibernate实例
        /// </summary>
        public static void CloseSessionFactory()
        {
            if (SessionFactory != null)
            {
                SessionFactory.Close();
            }
        }
        /// <summary>
        /// 初始化log4net
        /// </summary>
        public static void SetConfig()
        {
            XmlConfigurator.Configure();
        }
        /// <summary>
        /// 初始化log4net
        /// </summary>
        public static void SetConfig(FileInfo configFile)
        {
            XmlConfigurator.Configure(configFile);
        }
        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }
        public static void WriteErrorLog(string info)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info);
            }
        }
        public static void WriteErrorLog(string info, Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, ex);
            }
        }
        #region 加密解密
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        /// <summary>
        /// AES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为16位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptAES(string encryptString, string encryptKey)
        {
            try
            {
                SymmetricAlgorithm des = Rijndael.Create();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                des.Key = Encoding.UTF8.GetBytes(encryptKey);
                des.IV = Keys2;
                byte[] cipherBytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cipherBytes = ms.ToArray();
                        cs.Close();
                        ms.Close();
                    }
                }
                return Convert.ToBase64String(cipherBytes);
            }
            catch
            {
                return encryptString;
            }
        }
        /// <summary>
        /// AES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为16位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptAES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] cipherText = Convert.FromBase64String(decryptString);
                SymmetricAlgorithm des = Rijndael.Create();
                des.Key = Encoding.UTF8.GetBytes(decryptKey);
                des.IV = Keys2;
                byte[] decryptBytes = new byte[cipherText.Length];
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cs.Read(decryptBytes, 0, decryptBytes.Length);
                        cs.Close();
                        ms.Close();
                    }
                }
                return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion
    }
}