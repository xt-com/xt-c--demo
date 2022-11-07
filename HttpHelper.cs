using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ApiDemo
{
    public class HttpHelper
    {

        public static HttpStatusCode TimeoutHttpStatus = HttpStatusCode.GatewayTimeout;

         static string Timeout = "6000";

         static string AppKey = "";
         static string SecretKey = "";

        static string Encry = "HmacSHA256";
        static string ContentType = "application/json";
        static string BaseUrl = "http://sapi.xt.com";

        static HttpHelper()
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.DefaultConnectionLimit = 200;
                ServicePointManager.MaxServicePointIdleTime = 2000;
                ServicePointManager.DnsRefreshTimeout = 0;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                ServicePointManager.ServerCertificateValidationCallback = PinPublicKey;
            }
            catch (Exception ex)
            {
                // log.Error(ex.Message);
            }

        }

      
        /// <returns></returns>
        public static ApiReturnInfo Post(string uri, string  json)
        {
            ApiReturnInfo resultInfo = new ApiReturnInfo();

            string timeStamp = TimeHelper.DateTime2Long(DateTime.Now).ToString();

            string sign = GetHMACSHA256(json, timeStamp, "POST", uri);

            Uri address = new Uri(BaseUrl+uri);

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            //request.KeepAlive = false;
            request.ServicePoint.Expect100Continue = false;

            request.ServicePoint.UseNagleAlgorithm = false;
            request.ServicePoint.ConnectionLimit = 65500;
            request.AllowWriteStreamBuffering = false;
          

            try
            {

                request.Headers.Set("xt-validate-algorithms", Encry);
                request.Headers.Set("xt-validate-appkey", AppKey);
                request.Headers.Set("xt-validate-recvwindow", Timeout);
                request.Headers.Set("xt-validate-timestamp", timeStamp);

                request.Headers.Set("xt-validate-signature", sign);

                //Set type to POST  
                request.Method = "POST";

                request.ContentType = ContentType;

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteData.Length;


                // Write data  
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                    postStream.Close();
                }

                // Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    resultInfo.Json = reader.ReadToEnd();

                    reader.Close();
                    response.Close();
                }

            }
            catch (WebException ex)
            {
                resultInfo.Success = false;

                HttpWebResponse response = (HttpWebResponse)ex.Response;

                if (response != null)
                {

                    using (Stream data = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            resultInfo.Json = reader.ReadToEnd();
                            reader.Close();
                        }
                    }

                    resultInfo.StatusCode = response.StatusCode;

                }
                else
                {
                    resultInfo.Json = string.Empty;
                    resultInfo.Msg = "Network disconnected";
                }
            }
            finally
            {
                request.Abort();
            }

            return resultInfo;

        }

        public static ApiReturnInfo Get(string uri,string query)
        {
            ApiReturnInfo resultInfo = new ApiReturnInfo();

            string timeStamp = TimeHelper.DateTime2Long(DateTime.Now).ToString();

            string sign= GetHMACSHA256(query,timeStamp,"GET",uri);

            string param = string.Empty;

            if (!string.IsNullOrEmpty(query))
            {
                param = "?" + query;
            }

            HttpWebRequest request = WebRequest.Create(BaseUrl+uri+param) as HttpWebRequest;

            request.ServicePoint.Expect100Continue = false;

            request.ServicePoint.UseNagleAlgorithm = false;
            request.ServicePoint.ConnectionLimit = 65500;
            request.AllowWriteStreamBuffering = false;


            try
            {

                request.Headers.Set("xt-validate-algorithms", Encry);
                request.Headers.Set("xt-validate-appkey", AppKey);
                request.Headers.Set("xt-validate-recvwindow", Timeout);
                request.Headers.Set("xt-validate-timestamp", timeStamp);
                request.Headers.Set("xt-validate-signature", sign);
              
                //Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    //Console application output  
                    resultInfo.Json = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                }
            }
            catch (WebException ex)
            {
                resultInfo.Success = false;

                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response == null)
                {

                    resultInfo.StatusCode = TimeoutHttpStatus;
                    resultInfo.Json = string.Empty;

                }
                else
                {

                    using (Stream data = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            resultInfo.Json = reader.ReadToEnd();

                            reader.Close();
                        }
                    }

                    resultInfo.StatusCode = response.StatusCode;

                }
            }
            finally
            {
                request.Abort();
            }

            return resultInfo;
        }


        public static ApiReturnInfo Delete(string uri,string json="")
        {
            ApiReturnInfo resultInfo = new ApiReturnInfo();

            string timeStamp = TimeHelper.DateTime2Long(DateTime.Now).ToString();

            string sign = GetHMACSHA256(json, timeStamp, "DELETE", uri);


            HttpWebRequest request = WebRequest.Create(BaseUrl+ uri) as HttpWebRequest;

            request.ServicePoint.Expect100Continue = false;

            request.ServicePoint.UseNagleAlgorithm = false;
            request.ServicePoint.ConnectionLimit = 65500;
            request.AllowWriteStreamBuffering = false;
          
            request.Method = "DELETE";

            try
            {

                request.Headers.Set("xt-validate-algorithms", Encry);
                request.Headers.Set("xt-validate-appkey", AppKey);
                request.Headers.Set("xt-validate-recvwindow", Timeout);
                request.Headers.Set("xt-validate-timestamp", timeStamp);
                request.Headers.Set("xt-validate-signature", sign);

                if (!string.IsNullOrEmpty(json))
                {
                    request.ContentType = ContentType;

                    byte[] byteData = UTF8Encoding.UTF8.GetBytes(json);

                    request.ContentLength = byteData.Length;


                    // Write data  
                    using (Stream postStream = request.GetRequestStream())
                    {
                        postStream.Write(byteData, 0, byteData.Length);
                        postStream.Close();
                    }
                }

                //Get response  
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    //Console application output  
                    resultInfo.Json = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                }

            }
            catch (WebException ex)
            {
                resultInfo.Success = false;

                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response == null)
                {

                    resultInfo.StatusCode = TimeoutHttpStatus;
                    resultInfo.Json = string.Empty;
                    resultInfo.Msg = "Network disconnected";
                }
                else
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            resultInfo.Json = reader.ReadToEnd();
                            reader.Close();
                        }
                    }

                    resultInfo.StatusCode = response.StatusCode;

                }
            }
            finally
            {
                request.Abort();
            }

            return resultInfo;
        }

        static bool PinPublicKey(object sender, X509Certificate certificate, X509Chain chain,
                                       SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        static string GetHMACSHA256(string queryString,string time,string method,string url)
        {
            Encoding ascii = Encoding.ASCII;

            string s1 = "xt-validate-algorithms="+Encry+"&xt-validate-appkey="+AppKey+"&xt-validate-recvwindow="+Timeout+"&xt-validate-timestamp="+time;
          
            string s2 = string.Format("#{0}#{1}",method,url);

            if (!string.IsNullOrEmpty(queryString))
            {
                s2 += "#" + queryString;
            }

            HMACSHA256 hmac = new HMACSHA256(ascii.GetBytes(SecretKey));

            String signature = BitConverter.ToString(hmac.ComputeHash(ascii.GetBytes(s1+s2))).Replace("-", "");

            return signature;
        }

    }
}
