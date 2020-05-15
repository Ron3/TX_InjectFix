using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    /// <summary>
    /// 请求方法
    /// </summary>
    public static class HttpMethod
    {
        public const string GET = "GET";
        public const string POST = "POST";
    }


    /// <summary>
    /// HTTP/HTTPS请求辅助类
    /// </summary>
    public static class BPHttpHelper
    {
        /// <summary>
        /// HttpResponse转Json字符串
        /// </summary>
        /// <param name="response"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public static string ConvertHttpResponseToStr(HttpWebResponse response, string requestType=HttpMethod.GET, Encoding encoding=null)
        {   
            // 因为参数如果填写成UTF-8.会编译不过去.那么就先改成这样吧
            if(encoding == null){
                encoding = Encoding.UTF8;
            }

            string responseResult = "";
            string strEncoding = "UTF-8";
            if (string.Equals(requestType, HttpMethod.POST, StringComparison.OrdinalIgnoreCase))
            {
                strEncoding = response.ContentEncoding;
                if (strEncoding == null || strEncoding.Length < 1)
                {
                    strEncoding = "UTF-8";
                    encoding = Encoding.GetEncoding(strEncoding);
                }
            }

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
            {
                responseResult = reader.ReadToEnd();
            }

            return responseResult;
        }

        #region ==============by Ron===================

        /// <summary>
        /// 处理请求数据
        /// 压缩 --> 加密 --> base64 --> 发送
        /// </summary>
        /// <param name="bodyData"></param>
        /// <param name="isZip"></param>
        /// <param name="isEncrypt"></param>
        /// /// <param name="aesKey"></param>
        /// <returns></returns>
        public static byte[] _HandleRequestData(string bodyData, bool isZip=true, bool isEncrypt=false, string aesKey="")
        {
            if(bodyData == string.Empty || bodyData == "")
                return null;

            
            byte[] byteArray = bodyData.ToByteArray();

            // 判定是否压缩
            if(isZip == true)
            {
                byteArray = BPZipHelper.Compress(byteArray);
            }

            if(isEncrypt == true) 
            {
                byteArray = BPAESHelper.Encrypt_byte(byteArray, aesKey);
            }
            
            return byteArray;
        }


        /// <summary>
        /// 处理response
        /// base64decode -> 解密 --> 解压缩
        /// </summary>
        /// <param name="responseData"></param>
        /// <param name="isZip"></param>
        /// <param name="isEncrypt"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static byte[] _HandleResponseByte(byte[] responseByteArray, bool isZip=true, bool isEncrypt=false, string aesKey="")
        {
            if(responseByteArray == null)
                return null;

            if(isEncrypt == true)
            {
                responseByteArray = BPAESHelper.Decrypt_byte(responseByteArray, aesKey);
            }

            if (isZip == true)
            {
                responseByteArray = BPZipHelper.Decompress(responseByteArray);
            }

            return responseByteArray;
        }


        /// <summary>
        /// 同步http post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bodyData"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        /// <param name="isZip"></param>
        /// <param name="isEncrypt"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static string BPPostRequest(string url, 
                                            string bodyData="", 
                                            int timeout=5000, 
                                            string contentType="application/x-www-form-urlencoded", 
                                            bool isZip=true, 
                                            bool isEncrypt=false, 
                                            string aesKey="")
        {
            string respStr = string.Empty;
            try
            {
                HttpWebResponse postResponse = BPPostRequest_RawResponse(url, bodyData, timeout, contentType, isZip, isEncrypt, aesKey);
                respStr = ConvertHttpResponseToStr(postResponse, HttpMethod.POST);
                // 现在服务器返回的,都是base64编码过的
                // 所以客户端先base64decode --> unzip --> decrypt --> 还原回来
                byte[] responseRawByteArray = BPMainCommon.Base64decode(respStr);
                byte[] responseByteArray = BPHttpHelper._HandleResponseByte(responseRawByteArray, isZip, isEncrypt, aesKey);
                return BPMainCommon.ByteToString(responseByteArray);
            }
            catch (Exception ex)
            {
                respStr = ex.Message;
                Log.Error(ex.Message);
                return "";
            }
        }


        /// <summary>
        /// 支持请求原生的http
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bodyData"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        /// <param name="isZip"></param>
        /// <param name="isEncrypt"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static HttpWebResponse BPPostRequest_RawResponse(string url, 
                                                                string bodyData="", 
                                                                int timeout=5000, 
                                                                string contentType="application/x-www-form-urlencoded", 
                                                                bool isZip=true, 
                                                                bool isEncrypt=false, 
                                                                string aesKey="")
        {
            string respStr = string.Empty;
            try
            {
                HttpWebRequest postRequest = HttpWebRequest.Create(url) as HttpWebRequest;
                postRequest.KeepAlive = false;
                postRequest.Timeout = timeout;
                postRequest.Method = HttpMethod.POST;
                postRequest.ContentType = contentType;
                
                // 处理bodyData
                if(bodyData != string.Empty && bodyData != "")
                {
                    byte[] byteArray = BPHttpHelper._HandleRequestData(bodyData, isZip, isEncrypt, aesKey);
                    string base64Str = BPMainCommon.Base64encode(byteArray);
                    byte[] requestByteData = BPMainCommon.StringToByte(base64Str);

                    // 设置http的body
                    postRequest.ContentLength = requestByteData.Length;
                    Stream bodyStream = postRequest.GetRequestStream();
                    bodyStream.Write(requestByteData, 0, requestByteData.Length);
                    bodyStream.Flush();
                    bodyStream.Close();
                }
                
                return postRequest.GetResponse() as HttpWebResponse;
            }
            catch (Exception ex)
            {
                respStr = ex.Message;
                Log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// http异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bodyData"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        /// <param name="isZip"></param>
        /// <param name="isEncrypt"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static async Task<string> BPPostRequestAsync(string url, 
                                                            string bodyData="", 
                                                            int timeout=5000, 
                                                            string contentType="application/x-www-form-urlencoded", 
                                                            bool isZip=true, 
                                                            bool isEncrypt=false, 
                                                            string aesKey="")
        {
            string respStr = string.Empty;
            try
            {
                HttpWebResponse response = await BPPostRequestAsync_RawResponse(url, bodyData, timeout, contentType, isZip, isEncrypt, aesKey);
                respStr = ConvertHttpResponseToStr(response, HttpMethod.POST);
                
                // 现在服务器返回的,都是base64编码过的
                // 所以客户端先base64decode --> unzip --> decrypt --> 还原回来
                byte[] responseRawByteArray = BPMainCommon.Base64decode(respStr);
                byte[] responseByteArray = BPHttpHelper._HandleResponseByte(responseRawByteArray, isZip, isEncrypt, aesKey);
                return BPMainCommon.ByteToString(responseByteArray);
            }
            catch (Exception ex)
            {
                // TODO Ron.为什么不直接判定response.HttpCode.
                // 因为根本还等不到判定,就直接进来这个异常包或者.然后返回给上层了
                // 2019-04-03 跟Jeff协商后,当出错了.暂时先返回一个"" 给上层
                respStr = ex.Message;
                Log.Error(ex.Message);
                return "";
            }
        }


        /// <summary>
        /// http异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bodyData"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        /// <param name="isZip"></param>
        /// <param name="isEncrypt"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static async Task<HttpWebResponse> BPPostRequestAsync_RawResponse(string url, 
                                                                                string bodyData="", 
                                                                                int timeout=5000, 
                                                                                string contentType="application/x-www-form-urlencoded", 
                                                                                bool isZip=true, 
                                                                                bool isEncrypt=false, 
                                                                                string aesKey="")
        {
            string respStr = string.Empty;
            try
            {
                HttpWebRequest postRequest = HttpWebRequest.Create(url) as HttpWebRequest;
                postRequest.KeepAlive = false;
                postRequest.Timeout = timeout;
                postRequest.Method = HttpMethod.POST;
                postRequest.ContentType = contentType; 

                if(bodyData != string.Empty && bodyData != "")
                {
                    // 最终得到请求数据
                    byte[] byteArray = BPHttpHelper._HandleRequestData(bodyData, isZip, isEncrypt, aesKey);
                    string base64Str = BPMainCommon.Base64encode(byteArray);
                    byte[] requestByteData = BPMainCommon.StringToByte(base64Str);

                    // 设置http的body
                    postRequest.ContentLength = requestByteData.Length;
                    Stream bodyStream = postRequest.GetRequestStream();
                    bodyStream.Write(requestByteData, 0, requestByteData.Length);
                    bodyStream.Flush();
                    bodyStream.Close();
                }

                return await postRequest.GetResponseAsync() as HttpWebResponse;
            }
            catch (Exception ex)
            {
                // TODO Ron.为什么不直接判定response.HttpCode.
                // 因为根本还等不到判定,就直接进来这个异常包或者.然后返回给上层了
                // 2019-04-03 跟Jeff协商后,当出错了.暂时先返回一个"" 给上层
                respStr = ex.Message;
                Log.Error(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// http get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <param name="isZip"></param>
        /// <param name="isEncrypt"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static async Task<string> BPGetRequestAsync(string url, int timeout=5000, bool isZip=true, bool isEncrypt=false, string aesKey="")
        {
            string respStr = string.Empty;
            try
            {
                HttpWebResponse response = await BPGetRequestAsync_RawResponse(url, timeout);
                respStr = ConvertHttpResponseToStr(response, HttpMethod.GET);

                // 解压,解密出来
                byte[] responseRawByteArray = BPMainCommon.Base64decode(respStr);
                byte[] responseByteArray = BPHttpHelper._HandleResponseByte(responseRawByteArray, isZip, isEncrypt, aesKey);
                return BPMainCommon.ByteToString(responseByteArray);
            }
            catch (Exception ex)
            {
                respStr = ex.Message;
                Log.Error(ex.Message);
            }

            // return JsonHelper.FromJson<TResult>(respStr);
            return "";
        }


        /// <summary>
        /// 异步原始http get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType="text/html;charset></param>
        /// <returns></returns>
        public static async Task<HttpWebResponse> BPGetRequestAsync_RawResponse(string url, int timeout=5000, string contentType="text/html;charset=UTF-8")
        {
            try
            {
                HttpWebRequest getRequest = HttpWebRequest.Create(url) as HttpWebRequest;
                getRequest.Method = HttpMethod.GET;
                getRequest.Timeout = timeout;
                getRequest.ContentType = contentType;

                return await getRequest.GetResponseAsync() as HttpWebResponse; 
            }
            catch (Exception ex)
            {
                // string errMsg = ex.Message;
                Log.Error(ex.Message);
            }

            return null;
        }


        /// <summary>
        /// 同步与bp的http服务器get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <param name="isZip"></param>
        /// <param name="isEncrypt"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static string BPGetRequest(string url, int timeout=5000, bool isZip=true, bool isEncrypt=false, string aesKey="")
        {
            string respStr = string.Empty;
            try
            {
                HttpWebResponse httpWebResponse = BPGetRequest_RawResponse(url, timeout);
                respStr = ConvertHttpResponseToStr(httpWebResponse, HttpMethod.GET);
                
                // 解压,解密出来
                byte[] responseRawByteArray = BPMainCommon.Base64decode(respStr);
                byte[] responseByteArray = BPHttpHelper._HandleResponseByte(responseRawByteArray, isZip, isEncrypt, aesKey);
                return BPMainCommon.ByteToString(responseByteArray);
            }
            catch (Exception ex)
            {
                respStr = ex.Message;
                Log.Error(ex.Message);
            }
            
            return "";
        }
        
        
        /// <summary>
        /// 原生的http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType="text/html;charset></param>
        /// <returns></returns>
        public static HttpWebResponse BPGetRequest_RawResponse(string url, int timeout=5000, string contentType="text/html;charset=UTF-8")
        {
            HttpWebRequest getRequest = HttpWebRequest.Create(url) as HttpWebRequest;
            getRequest.Method = HttpMethod.GET;
            getRequest.Timeout = timeout;
            getRequest.ContentType = contentType;
            getRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpWebResponse httpWebResponse = getRequest.GetResponse() as HttpWebResponse;
            return httpWebResponse;
        }

        #endregion
    }
}
