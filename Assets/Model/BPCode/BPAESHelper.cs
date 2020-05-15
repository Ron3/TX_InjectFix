using System;
using System.Text;
using System.Security.Cryptography;

namespace ETModel
{
    public static class BPAESHelper
    {
        public static string key = "0123456789123456";

        /// <summary>
        /// AES加密后,返回byte
        /// </summary>
        /// <param name="encryptStr"></param>
        /// <returns></returns>
        public static byte[] Encrypt_str(string encryptStr, string aesKey="")
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);
            return Encrypt_byte(toEncryptArray, aesKey);
        }


        /// <summary>
        /// AES加密后,返回byte
        /// </summary>
        /// <param name="encryptStr"></param>
        /// <returns></returns>
        public static byte[] Encrypt_byte(byte[] toEncryptArray, string aesKey="")
        {
            // 如果外面有传入key.则用外面的.否则就用默认的
            if(aesKey == string.Empty || aesKey == "")
                aesKey = BPAESHelper.key;

            try
            {
                //byte[] keyArray = Encoding.UTF8.GetBytes(Key);
                // byte[] keyArray = Convert.FromBase64String(BPAESHelper.key);
                byte[] keyArray = aesKey.ToByteArray();
                // byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                
                return resultArray;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptStr"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static string Encrypt_return_Base64(string encryptStr, string aesKey="")
        {
            try
            {
                byte[] resultArray = BPAESHelper.Encrypt_str(encryptStr, aesKey);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        #region ===============解密===============


        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64Str"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static string Decrypt_Base64(string  base64Str, string aesKey="")
        {
            try
            {
                byte[] encryptArray = Convert.FromBase64String(base64Str);
                byte[] resultArray = BPAESHelper.Decrypt_byte(encryptArray, aesKey);
                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                Log.Error("Err => " + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// AES解byte
        /// </summary>
        /// <param name="encryptArray"></param>
        /// <returns></returns>
        public static byte[] Decrypt_byte(byte[] encryptArray, string aesKey="")
        {   
            if (aesKey == string.Empty || aesKey == "")
                aesKey = BPAESHelper.key;
                
            try
            {
                // byte[] keyArray = Encoding.UTF8.GetBytes(Key);
                // byte[] keyArray = Convert.FromBase64String(Key);
                // byte[] toEncryptArray = Convert.FromBase64String(base64Str);

                byte[] keyArray = aesKey.ToByteArray();
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(encryptArray, 0, encryptArray.Length);

                // return Encoding.UTF8.GetString(resultArray);
                return resultArray;
            }
            catch (Exception ex)
            {
                Log.Error("Err => " + ex.Message);
                return null;
            }
        }
        
        #endregion
    }
}

