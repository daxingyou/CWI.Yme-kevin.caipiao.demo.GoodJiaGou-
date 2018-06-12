using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Net.Http;
using Yme.Util.Exceptions;
using Yme.Util.Extension;
using System.Security.Cryptography.X509Certificates;
using Yme.Util.Log;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Yme.Util
{
    public class HttpRequestUtil
    {
        #region WeiXin请求

        /// <summary>
        ///  微信Get请求接口
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string WeChatSendGetRequest(string url)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string result = SendRequest(url, "GET", parameters);
            return result;
        }

        /// <summary>
        ///  微信POST请求
        /// </summary>
        /// <param name="tokenUrl"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string WechatSendPostRequest(string tokenUrl, string json)
        {
            byte[] postBytes = Encoding.UTF8.GetBytes(json);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(postBytes, 0, postBytes.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string responseText = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            responseStream.Close();
            return responseText;
        }

        #endregion

        #region 微信支付请求

        /// <summary>
        /// 校验是否有效请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //总是接受  
            return true;
        }

        /// <summary>
        /// 微信支付Post请求
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="url"></param>
        /// <param name="isUseCert"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string WxPayPost(string xml, string url, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(PayConfigUtil.WechatProxyUrl);    //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "Thread - caught ThreadAbortException - resetting."));
                LogUtil.Error(string.Format("{0}-{1}", "Exception message: {0}", e.Message));
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", e.ToString()));
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode));
                    LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription));
                }

                throw new WxPayException("请求网络发生错误：" + e.Message + "，请稍候再试");
            }
            catch (Exception e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", e.ToString()));
                throw new WxPayException("请求网络发生错误：" + e.Message + "，请稍候再试");
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 带证书的请求
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="url"></param>
        /// <param name="isUseCert"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string WxPayPostWithCert(string merchantId, string xml, string url, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(PayConfigUtil.WechatProxyUrl);    //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (!string.IsNullOrEmpty(merchantId))
                {
                    string path = string.Empty;
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    if (merchantId == ConfigUtil.WechatMerchantIdForApp)
                    {
                        path = Path.Combine(baseDir, PayConfigUtil.WechatPaySslcertPathForApp);
                    }
                    else if (merchantId == ConfigUtil.WechatMerchantIdForJSAPI)
                    {
                        path = Path.Combine(baseDir, PayConfigUtil.WechatPaySslcertPathForJSAPI);
                    }

                    LogUtil.Debug(string.Format("path:{0}", path));
                    X509Certificate2 cert = new X509Certificate2(path, merchantId, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
                    request.ClientCertificates.Add(cert);
                }
                else
                {
                    throw new MessageException("商户号不能为空");
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "Thread - caught ThreadAbortException - resetting."));
                LogUtil.Error(string.Format("{0}-{1}", "Exception message: {0}", e.Message));
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", e.ToString()));
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode));
                    LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription));
                }

                throw new WxPayException("请求网络发生错误：" + e.Message + "，请稍候再试");
            }
            catch (Exception e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", e.ToString()));
                throw new WxPayException("请求网络发生错误：" + e.Message + "，请稍候再试");
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 微信支付Get请求
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string WxPayGet(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                WebProxy proxy = new WebProxy();
                proxy.Address = new Uri(PayConfigUtil.WechatProxyUrl);
                request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "Thread - caught ThreadAbortException - resetting."));
                LogUtil.Error(string.Format("Exception message: {0}", e.Message));
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", e.ToString()));
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode));
                    LogUtil.Error(string.Format("{0}-{1}", "NetUtil", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription));
                }
                throw new WxPayException("请求网络发生错误：" + e.Message + "，请稍候再试");
            }
            catch (Exception e)
            {
                LogUtil.Error(string.Format("{0}-{1}", "NetUtil", e.ToString()));
                throw new WxPayException("请求网络发生错误：" + e.Message + "，请稍候再试");
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        #endregion

        #region 基础请求

        /// <summary>
        /// 同步方式发起http post请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">参数</param>
        /// <returns>请求返回值</returns>
        public static string SendPostRequest(string url, Dictionary<string, string> parameters)
        {
            return SendRequest(url, "POST", parameters);
        }

        /// <summary>
        /// 同步方式发起http get请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">参数</param>
        /// <returns>请求返回值</returns>
        public static string SendGetRequest(string url, Dictionary<string, string> parameters)
        {
            return SendRequest(url, "GET", parameters);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="httpMethod">方法</param>
        /// <param name="parameters">参数</param>
        /// <returns>响应结果</returns>
        private static string SendRequest(string url, string httpMethod, Dictionary<string, string> parameters)
        {
            StreamWriter requestWriter = null;
            StreamReader responseReader = null;
            string responseData = null;

            if (httpMethod == "GET" && parameters.Count > 0)
            {
                url += "?" + parameters.ToParamString();
            }
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

            webRequest.Method = httpMethod;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;
            Stream responseStream = null;

            try
            {
                if (httpMethod == "POST")
                {
                    requestWriter = new StreamWriter(webRequest.GetRequestStream());
                    requestWriter.Write(parameters.ToParamString());
                    requestWriter.Close();
                    requestWriter = null;
                }

                responseStream = webRequest.GetResponse().GetResponseStream();
                responseReader = new StreamReader(responseStream);
                responseData = responseReader.ReadToEnd();
            }
            finally
            {
                if (requestWriter != null)
                {
                    requestWriter.Close();
                    requestWriter = null;
                }

                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream = null;
                }

                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader = null;
                }

                webRequest = null;
            }

            return responseData;
        }

        #endregion

        #region Get

        /// <summary>
        /// 通过GET请求获得响应文本
        /// </summary>
        /// <param name="url">将要被请求的URL</param>
        /// <param name="param">请求参数，各参数key=value之间使用参数连接符进行连接</param>
        /// <returns>响应文本</returns>
        public static string HttpGet(string url, string param)
        {
            if (String.IsNullOrEmpty(url))
                throw new MessageException("url不能为空值");

            url = url.Trim();
            if (!String.IsNullOrEmpty(param))
            {
                if (url.IndexOf("?") <= 0) url += "?";
                url += param;
            }

            LogUtil.Debug(string.Format("url:{0}", url));

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string text = sr.ReadToEnd();

                sr.Close();
                return text;
            }
            catch (Exception ex)
            {
                throw new MessageException("请求网络发生错误：" + ex.Message + "，请稍候再试");
            }
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url, Hashtable headht = null)
        {
            HttpWebRequest request;

            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            WebResponse response = null;
            string responseStr = null;
            if (headht != null)
            {
                foreach (DictionaryEntry item in headht)
                {
                    request.Headers.Add(item.Key.ToString(), item.Value.ToString());
                }
            }

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return responseStr;
        }

        public static string HttpGet(string url, Encoding encodeing, Hashtable headht = null)
        {
            HttpWebRequest request;

            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            WebResponse response = null;
            string responseStr = null;
            if (headht != null)
            {
                foreach (DictionaryEntry item in headht)
                {
                    request.Headers.Add(item.Key.ToString(), item.Value.ToString());
                }
            }

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encodeing);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return responseStr;
        }

        #endregion

        #region POST
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param = null, bool isJasonParm = false, string header = "")
        {
            HttpWebRequest request;

            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.Method = "POST";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            if (!string.IsNullOrWhiteSpace(header))
            {
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                request.Headers.Add(string.Format("Authorization: Basic {0}", header));
            }
            else
            {
                if (isJasonParm)
                {
                    request.ContentType = "Content-type: application/json; charset=utf-8";
                }
                else
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                request.Accept = "*/*";
            }

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("请求失败,请求地址：{0}, 请求参数：{1},错误信息:{2}", url, param, ex.StackTrace));
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        public static string BuildRequest(string strUrl, Dictionary<string, string> dicPara, string fileName)
        {
            string contentType = "image/jpeg";
            //待请求参数数组
            FileStream Pic = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] PicByte = new byte[Pic.Length];
            Pic.Read(PicByte, 0, PicByte.Length);
            int lengthFile = PicByte.Length;

            //构造请求地址

            //设置HttpWebRequest基本信息
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            //设置请求方式：get、post
            request.Method = "POST";
            //设置boundaryValue
            string boundaryValue = DateTime.Now.Ticks.ToString("x");
            string boundary = "--" + boundaryValue;
            request.ContentType = "\r\nmultipart/form-data; boundary=" + boundaryValue;
            //设置KeepAlive
            request.KeepAlive = true;
            //设置请求数据，拼接成字符串
            StringBuilder sbHtml = new StringBuilder();
            foreach (KeyValuePair<string, string> key in dicPara)
            {
                sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"" + key.Key + "\"\r\n\r\n" + key.Value + "\r\n");
            }
            sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"pic\"; filename=\"");
            sbHtml.Append(fileName);
            sbHtml.Append("\"\r\nContent-Type: " + contentType + "\r\n\r\n");
            string postHeader = sbHtml.ToString();
            //将请求数据字符串类型根据编码格式转换成字节流
            Encoding code = Encoding.GetEncoding("UTF-8");
            byte[] postHeaderBytes = code.GetBytes(postHeader);
            byte[] boundayBytes = Encoding.ASCII.GetBytes("\r\n" + boundary + "--\r\n");
            //设置长度
            long length = postHeaderBytes.Length + lengthFile + boundayBytes.Length;
            request.ContentLength = length;

            //请求远程HTTP
            Stream requestStream = request.GetRequestStream();
            Stream myStream = null;
            try
            {
                //发送数据请求服务器
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Write(PicByte, 0, lengthFile);
                requestStream.Write(boundayBytes, 0, boundayBytes.Length);
                HttpWebResponse HttpWResp = (HttpWebResponse)request.GetResponse();
                myStream = HttpWResp.GetResponseStream();
            }
            catch (WebException ex)
            {
                LogUtil.Error(ex.Message);
                return "";
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
            }

            //读取处理结果
            StreamReader reader = new StreamReader(myStream, code);
            StringBuilder responseData = new StringBuilder();

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                responseData.Append(line);
            }
            myStream.Close();
            Pic.Close();

            return responseData.ToString();
        }
        #endregion

        #region Put
        /// <summary>
        /// HTTP Put方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpPut(string url, string param = null)
        {
            HttpWebRequest request;

            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }
        #endregion

        #region Delete
        /// <summary>
        /// HTTP Delete方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpDelete(string url, string param = null)
        {
            HttpWebRequest request;

            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "Delete";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return responseStr;
        }
        #endregion

        #region Post With Pic
        private string HttpPost(string url, IDictionary<object, object> param, string filePath)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();
            string responseStr = null;

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in param.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, param[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "pic", filePath, "text/plain");
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseStr = reader2.ReadToEnd();
                //logger.Debug(string.Format("File uploaded, server response is: {0}", responseStr));
            }
            catch (Exception ex)
            {
                LogUtil.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                throw;
            }
            return responseStr;
        }
        #endregion

        #region Post With Pic
        /// <summary>
        /// HTTP POST方式请求数据(带图片)
        /// </summary>
        /// <param name="url">URL</param>        
        /// <param name="param">POST的数据</param>
        /// <param name="fileByte">图片</param>
        /// <returns></returns>
        public static string HttpPost(string url, IDictionary<object, object> param, byte[] fileByte)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();
            string responseStr = null;

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in param.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, param[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "pic", fileByte, "text/plain");//image/jpeg
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            rs.Write(fileByte, 0, fileByte.Length);

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseStr = reader2.ReadToEnd();
                // logger.Error(string.Format("File uploaded, server response is: {0}", responseStr));
            }
            catch (Exception ex)
            {
                LogUtil.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                throw;
            }
            return responseStr;
        }
        #endregion

        #region HttpsClient
        /// <summary>
        /// 创建HttpClient
        /// </summary>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(string url)
        {
            HttpClient httpclient;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                httpclient = new HttpClient();
            }
            else
            {
                httpclient = new HttpClient();
            }
            return httpclient;
        }
        #endregion

        public static NameValueCollection GetNameValueCollection(string request, bool isDecode = true)
        {
            var nv = new NameValueCollection();
            if (!string.IsNullOrWhiteSpace(request))
            {
                List<string> kvs = request.Trim().Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var kv in kvs)
                {
                    string[] values = kv.Trim().Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    string key = values[0];
                    string value = values.Length >= 2 ? values[1] : string.Empty;

                    if (!string.IsNullOrWhiteSpace(key) && !nv.AllKeys.Contains(key))
                    {
                        if (isDecode)
                        {
                            nv.Add(key, HttpUtility.UrlDecode(value));
                        }
                        else
                        {
                            nv.Add(key, value);
                        }
                    }
                }
            }
            return nv;
        }

        /// <summary>
        /// 获取请求参数键值对
        /// </summary>
        /// <param name="request">请求参数字符传</param>
        /// <returns>请求参数键值对</returns>
        public static NameValueCollection GetNameValueCollection(string request, out SortedDictionary<string, object> sortDics, bool isDecode = true, string signKey = "sign")
        {
            var nv = new NameValueCollection();
            sortDics = new SortedDictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(request))
            {
                List<string> kvs = request.Trim().Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var kv in kvs)
                {
                    string[] values = kv.Trim().Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    string key = values[0];
                    string value = values.Length >= 2 ? values[1] : string.Empty;

                    if (!string.IsNullOrWhiteSpace(key) && !nv.AllKeys.Contains(key))
                    {
                        if (isDecode)
                        {
                            nv.Add(key, HttpUtility.UrlDecode(value));
                        }
                        else
                        {
                            nv.Add(key, value);
                        }

                        if (key.ToLower() != signKey.ToLower())
                        {
                            if (isDecode)
                            {
                                sortDics.Add(key, HttpUtility.UrlDecode(value));
                            }
                            else
                            {
                                sortDics.Add(key, value);
                            }
                        }
                    }
                }
            }
            return nv;
        }
    }
}
