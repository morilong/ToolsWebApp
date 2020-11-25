/// <summary>
/// 类说明：HttpHelper类，用来实现Http访问，Post或者Get方式的，直接访问，带Cookie的，带证书的等方式，可以设置代理
/// 重要提示：请不要自行修改本类，如果因为你自己修改后将无法升级到新版本。如果确实有什么问题请到官方网站提建议，
/// 我们一定会及时修改
/// 编码日期：2011-09-20
/// 编 码 人：苏飞
/// 联系方式：361983679  
/// 官方网址：http://www.sufeinet.com/thread-3-1-1.html
/// 修改日期：2015-09-08
/// 版 本 号：1.5
/// </summary>

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net
{
    /// <summary>
    /// Http连接操作帮助类
    /// </summary>
    public class HttpHelper
    {
        #region 获取广域网ip信息 + WANipInfo GetWANipInfo()
        /// <summary>
        /// 获取指定ip的WANipInfo
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static WANipInfo GetWANipInfo(string ip)
        {
            ip = ip ?? "myip";
            HttpItem item = new HttpItem
            {
                URL = "http://ip.taobao.com/service/getIpInfo.php?ip=" + ip,
                Timeout = 30 * 1000
            };
            HttpResult res = new HttpHelper().GetHtml(item);
            string pattern = @"{""code"":0,""data"":{""country"":""(.*?)"",""country_id"":"".*?"",""area"":"".*?"",""area_id"":""\d+"",""region"":""(.*?)"",""region_id"":""\d+"",""city"":""(.*?)"",""city_id"":""\d+"",""county"":"".*?"",""county_id"":"".*?"",""isp"":""(.*?)"",""isp_id"":""\d+"",""ip"":""(.*?)""}}";
            Match m = Regex.Match(Regex.Unescape(res.Html), pattern);
            var info = new WANipInfo();
            if (m.Success && m.Groups.Count == 6)
            {
                info.Country = m.Groups[1].Value;
                info.Region = m.Groups[2].Value;
                info.City = m.Groups[3].Value;
                info.ISP = m.Groups[4].Value;
                info.IpAddress = m.Groups[5].Value.Trim();
                if (!Regex.IsMatch(info.IpAddress, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                {
                    info.IpAddress = string.Empty;
                }
            }
            return info;
        }
        /// <summary>
        /// 获取当前电脑的广域网ip信息
        /// </summary>
        /// <returns></returns>
        public static WANipInfo GetWANipInfo()
        {
            return GetWANipInfo(null);
        }
        #endregion

        #region 生成提交到服务器的参数结构 + string GetPostData(IDictionary<string, object> parameters, string ticks = null)

        /// <summary>
        /// <para>通过传入的Dictionary生成提交到服务器的参数结构。</para>
        /// <para>如果传入<paramref name="ticks"/>参数就生成多行post数据，否则生成普通post数据。</para>
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="ticks">DateTime.Now.Ticks.ToString("x")</param>
        /// <param name="isUrlEncode"></param>
        /// <returns></returns>
        public static string GetPostData(IDictionary<string, object> parameters, string ticks = null, bool isUrlEncode = true)
        {
            var sb = new StringBuilder();
            if (ticks == null)
            {
                foreach (var kv in parameters)
                {
                    if (sb.Length > 0)
                        sb.Append("&");

                    var value = kv.Value;
                    if (isUrlEncode && value != null)
                        value = Uri.EscapeDataString(value.ToString());

                    sb.AppendFormat("{0}={1}", kv.Key, value);
                }
            }
            else
            {
                foreach (var kv in parameters)
                {
                    var value = kv.Value;
                    if (isUrlEncode && value != null)
                        value = Uri.EscapeDataString(value.ToString());

                    sb.AppendFormat("-----------------------------{0}\r\n", ticks);
                    sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", kv.Key);
                    sb.Append("\r\n\r\n");
                    sb.AppendLine(value == null ? "" : value.ToString());
                }
                sb.AppendFormat("-----------------------------{0}--", ticks);
            }
            return sb.ToString();
        }

        /// <summary>
        /// <para>通过传入的Dictionary生成提交到服务器的参数结构。</para>
        /// <para>如果传入<paramref name="ticks"/>参数就生成多行post数据，否则生成普通post数据。</para>
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="ticks">DateTime.Now.Ticks.ToString("x")</param>
        /// <param name="isUrlEncode"></param>
        /// <returns></returns>
        public static string GetPostData(IDictionary<string, string> parameters, string ticks = null, bool isUrlEncode = true)
        {
            var sb = new StringBuilder();
            if (ticks == null)
            {
                foreach (var kv in parameters)
                {
                    if (sb.Length > 0)
                        sb.Append("&");

                    var value = kv.Value;
                    if (isUrlEncode && value != null)
                        value = Uri.EscapeDataString(value);

                    sb.AppendFormat("{0}={1}", kv.Key, value);
                }
            }
            else
            {
                foreach (var kv in parameters)
                {
                    var value = kv.Value;
                    if (isUrlEncode && value != null)
                        value = Uri.EscapeDataString(value);

                    sb.AppendFormat("-----------------------------{0}\r\n", ticks);
                    sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", kv.Key);
                    sb.Append("\r\n\r\n");
                    sb.AppendLine(value);
                }
                sb.AppendFormat("-----------------------------{0}--", ticks);
            }
            return sb.ToString();
        }

        public static string GetPostDataEx<T>(T param, bool isUrlEncode = true)
        {
            var sb = new StringBuilder();
            foreach (var pInfo in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty))
            {
                if (sb.Length > 0)
                    sb.Append("&");

                var value = pInfo.GetValue(param, null);

                if (isUrlEncode && value != null)
                    value = Uri.EscapeDataString(value.ToString());

                sb.AppendFormat("{0}={1}", pInfo.Name, value);
            }
            return sb.ToString();
        }

        public static string GetPostDataEx(object param, bool isUrlEncode = true)
        {
            var sb = new StringBuilder();
            foreach (var pInfo in param.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (sb.Length > 0)
                    sb.Append("&");

                var value = pInfo.GetValue(param, null);

                if (isUrlEncode && value != null)
                    value = Uri.EscapeDataString(value.ToString());

                sb.AppendFormat("{0}={1}", pInfo.Name, value);
            }
            return sb.ToString();
        }

        #endregion

        #region 生成post的bytes数据 + byte[] GetPostBytes(*)
        /// <summary>
        /// 生成post的bytes数据
        /// </summary>
        /// <param name="parameters">普通参数集合</param>
        /// <param name="ticks">DateTime.Now.Ticks.ToString("x")</param>
        /// <param name="name">图片参数名</param>
        /// <param name="data">图片数据</param>
        /// <param name="fileName">图片文件名</param>
        /// <param name="contentType">图片类型</param>
        /// <returns></returns>
        public static byte[] GetPostBytes(IDictionary<string, object> parameters, string ticks, string name, byte[] data, string fileName = "System.Byte[]", string contentType = "image/jpeg")
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (data == null)
                throw new ArgumentNullException("data");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(ticks))
                throw new ArgumentNullException("ticks");

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] bArr = null;
                foreach (var kv in parameters)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("-----------------------------{0}\r\n", ticks);
                    sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", kv.Key);
                    sb.Append("\r\n\r\n");
                    sb.AppendLine(kv.Value.ToString());

                    bArr = Encoding.Default.GetBytes(sb.ToString());
                    ms.Write(bArr, 0, bArr.Length);
                }

                StringBuilder sb2 = new StringBuilder();
                sb2.AppendFormat("-----------------------------{0}\r\n", ticks);
                sb2.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n", name, fileName);
                sb2.AppendFormat("Content-Type: {0}\r\n\r\n", contentType);

                bArr = Encoding.Default.GetBytes(sb2.ToString());
                ms.Write(bArr, 0, bArr.Length);

                ms.Write(data, 0, data.Length);//图片数据写入流

                bArr = Encoding.Default.GetBytes(string.Format("\r\n-----------------------------{0}--", ticks));
                ms.Write(bArr, 0, bArr.Length);//写结尾boundary

                return ms.ToArray();
            }
        }
        #endregion

        #region 取一个随机的浏览器类型 + string GetRandomUserAgent()

        #region int Next(int minValue, int maxValue)

        private static int _rep;
        private static Random GetNewRandom()
        {
            long num = DateTime.Now.Ticks + _rep;
            _rep++;
            return new Random(((int)(((ulong)num) & 0xffffffffL)) | ((int)(num >> _rep)));
        }
        /// <summary>
        /// 返回一个指定范围内的随机数。
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private static int Next(int minValue, int maxValue)
        {
            return GetNewRandom().Next(minValue, maxValue);
        }
        #endregion

        /// <summary>
        /// 取一个随机的浏览器类型
        /// </summary>
        /// <returns></returns>
        public static string GetRandomUserAgent()
        {
            string[] uas =
            {
                //———————————64位——————————
                //360急速（高速）
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36",
                //360急速（兼容）
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)",
                //百度（高速）
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.43 BIDUBrowser/6.x Safari/537.31",
                 //百度（兼容）
                "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; .NET4.0C; .NET4.0E; BIDUBrowser 6.x)",
                //搜狗（高速）
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.95 Safari/537.36 SE 2.X MetaSr 1.0",
                //搜狗（兼容）
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SE 2.X MetaSr 1.0)",
                //火狐
                "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:24.0) Gecko/20100101 Firefox/24.0",
                //Opera
                "Opera/9.80 (Windows NT 6.1; U; zh-cn) Presto/2.9.168 Version/11.51",
                //谷歌
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36",
                //———————————32位——————————
                 //360急速（高速）
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36",
                //360急速（兼容）
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",
                //百度（高速）
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.43 BIDUBrowser/6.x Safari/537.31",
                 //百度（兼容）
                "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; .NET4.0C; .NET4.0E; BIDUBrowser 6.x)",
                //搜狗（高速）
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.95 Safari/537.36 SE 2.X MetaSr 1.0",
                //搜狗（兼容）
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; SE 2.X MetaSr 1.0)",
                //火狐
                "Mozilla/5.0 (Windows NT 6.1; rv:24.0) Gecko/20100101 Firefox/24.0",
                //Opera
                "Opera/9.80 (Windows NT 6.1; U; zh-cn) Presto/2.9.168 Version/11.51",
                //谷歌
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36"
            };
            int n = Next(0, uas.Length);
            return uas[n];
        }
        #endregion

        #region 预定义方变量
        //默认的返回数据编码
        private Encoding _encoding = null;
        //Post数据编码
        private Encoding _postencoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求
        private HttpWebRequest _request = null;
        //获取影响流的数据对象
        private HttpWebResponse _response = null;

        /// <summary>
        /// 上次请求url
        /// </summary>
        private string _lastRequestURL = null;

        #endregion

        #region Public
        /// <summary>
        /// 上次请求的cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        public HttpResult GetHtml(HttpItem item)
        {
            if (string.IsNullOrEmpty(item.URL))
                throw new ArgumentNullException();

            HttpResult result = new HttpResult();
            try
            {
                SetRequest(item);
            }
            catch (Exception ex)
            {
                result.Cookie = string.Empty;
                result.Headers = null;
                result.Html = String.Empty;
                result.StatusDescription = "配置参数时出错：" + ex.Message;
                return result;
            }
            try
            {
                using (_response = (HttpWebResponse)_request.GetResponse())
                {
                    //处理cookies
                    if (!string.IsNullOrEmpty(_response.Headers["Set-Cookie"]))
                    {
                        if (string.IsNullOrEmpty(Cookie))
                            Cookie = CookieHelper.CookieFormat(CookieHelper.ToCookieList(_response.Headers["Set-Cookie"]));
                        else
                            Cookie = CookieHelper.CookieMergeUpdate(Cookie, _response.Headers["Set-Cookie"]);
                    }
                    //处理301/302的请求
                    result.RedirectUrl = this.GetRedirectUrl();//获取重定向url
                    if ((_response.StatusCode == HttpStatusCode.Moved || _response.StatusCode == HttpStatusCode.Found) && item.AutoRedirected && !string.IsNullOrEmpty(result.RedirectUrl))
                    {
                        item.Method = "GET";
                        item.URL = result.RedirectUrl;
                        item.Referer = _lastRequestURL;
                        item.Cookie = Cookie;
                        return this.GetHtml(item);
                    }
                    this.GetData(item, result);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (_response = (HttpWebResponse)ex.Response)
                    {
                        this.GetData(item, result);
                    }
                }
                else
                {
                    result.Html = String.Empty;
                    result.StatusDescription = ex.Message;
                }
            }
            catch (Exception ex)
            {
                result.Html = String.Empty;
                result.StatusDescription = ex.Message;
            }
            if (result.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(result.Html))
            {
                result.IsEmptyOrNotOk = true;
            }
            return result;
        }

        #endregion

        #region SetRequest

        /// <summary>
        /// 为请求准备参数
        /// </summary>
        ///<param name="item">参数列表</param>
        private void SetRequest(HttpItem item)
        {
            SetCer(item);// 验证证书
            SetProxy(item);// 设置代理

            _request.Method = item.Method;
            _request.Accept = item.Accept;
            _request.UserAgent = item.UserAgent;//UserAgent客户端的访问类型，包括浏览器版本和操作系统信息

            _request.Referer = item.Referer ?? (_lastRequestURL ?? item.URL);
            _lastRequestURL = item.URL;

            _request.ContentType = item.ContentType == null && item.Method.Equals("POST", StringComparison.CurrentCultureIgnoreCase) ? "application/x-www-form-urlencoded" : item.ContentType;
            _request.KeepAlive = item.KeepAlive;

            SetCookie(item);//设置Cookie
            SetHeaders(item);//设置Header参数

            _request.Timeout = item.Timeout;
            _request.ReadWriteTimeout = item.ReadWriteTimeout;
            _request.Credentials = item.Credentials;//设置安全凭证
            _request.ServicePoint.Expect100Continue = item.Expect100Continue;
            _request.AllowAutoRedirect = item.Allowautoredirect;//是否执行跳转功能

            SetPostData(item);//设置Post数据

            if (item.MaximumAutomaticRedirections > 0)
                _request.MaximumAutomaticRedirections = item.MaximumAutomaticRedirections;
            //设置最大连接
            if (item.Connectionlimit > 0)
                _request.ServicePoint.ConnectionLimit = item.Connectionlimit;

            _encoding = item.Encoding;// 返回数据编码
        }
        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCer(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.CerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => { return true; };
                //初始化对像，并设置请求的URL地址
                _request = (HttpWebRequest)WebRequest.Create(item.URL);
                SetCerList(item);
                //将证书添加到请求里
                _request.ClientCertificates.Add(new X509Certificate(item.CerPath));
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                _request = (HttpWebRequest)WebRequest.Create(item.URL);
                SetCerList(item);
            }
        }
        /// <summary>
        /// 设置多个证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCerList(HttpItem item)
        {
            if (item.ClentCertificates != null && item.ClentCertificates.Count > 0)
            {
                foreach (X509Certificate c in item.ClentCertificates)
                {
                    _request.ClientCertificates.Add(c);
                }
            }
        }
        /// <summary>
        /// 设置Header
        /// </summary>
        private void SetHeaders(HttpItem item)
        {
            if (item.Headers != null && item.Headers.Count > 0)
            {
                foreach (string name in _request.Headers)
                {
                    item.Headers.Add(name, _request.Headers[name]);
                }
                var t = _request.GetType();
                var f = t.GetField("_HttpRequestHeaders", BindingFlags.Instance | BindingFlags.NonPublic);
                f.SetValue(_request, item.Headers);
            }
            else
            {
                _request.Headers.Add("Accept-Language", "zh-CN");
                _request.Headers.Add("Accept-Encoding", "gzip, deflate");
            }
        }
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="item">参数对象</param>
        private void SetProxy(HttpItem item)
        {
            bool isIeProxy = false;
            if (!string.IsNullOrEmpty(item.ProxyIp))
            {
                isIeProxy = item.ProxyIp.ToLower().Contains("ieproxy");
            }
            if (!string.IsNullOrEmpty(item.ProxyIp) && !isIeProxy)
            {
                //设置代理服务器
                if (item.ProxyIp.Contains(":"))
                {
                    string[] plist = item.ProxyIp.Split(':');
                    WebProxy myProxy = new WebProxy(plist[0].Trim(), Convert.ToInt32(plist[1].Trim()));
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    //给当前请求对象
                    _request.Proxy = myProxy;
                }
                else
                {
                    WebProxy myProxy = new WebProxy(item.ProxyIp, false);
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    //给当前请求对象
                    _request.Proxy = myProxy;
                }
            }
            else if (isIeProxy)
            {
                //设置为IE代理
            }
            else
            {
                _request.Proxy = item.WebProxy;
            }
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetCookie(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.Cookie))
            {
                Cookie = string.IsNullOrEmpty(Cookie) ? item.Cookie : CookieHelper.CookieMergeUpdate(Cookie, item.Cookie);
            }
            else if (item.CookieCollection != null && item.CookieCollection.Count > 0)
            {
                Cookie = string.IsNullOrEmpty(Cookie) ? item.CookieCollection.ToStringEx() : CookieHelper.CookieMergeUpdate(Cookie, item.CookieCollection.ToStringEx());
            }
            if (!string.IsNullOrEmpty(Cookie))
            {
                _request.Headers["Cookie"] = Cookie;
            }
        }
        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetPostData(HttpItem item)
        {
            //验证在得到结果时是否有传入数据
            if (!_request.Method.Equals("GET", StringComparison.CurrentCultureIgnoreCase))
            {
                if (item.PostEncoding != null)
                    _postencoding = item.PostEncoding;

                byte[] buffer = null;
                //写入Byte类型
                if (item.PostDataType == PostDataType.Byte && item.PostdataByte != null && item.PostdataByte.Length > 0)
                {
                    //验证在得到结果时是否有传入数据
                    buffer = item.PostdataByte;
                }//写入文件
                else if (item.PostDataType == PostDataType.FilePath && !string.IsNullOrEmpty(item.Postdata))
                {
                    StreamReader r = new StreamReader(item.Postdata, _postencoding);
                    buffer = _postencoding.GetBytes(r.ReadToEnd());
                    r.Close();
                } //写入字符串
                else if (!string.IsNullOrEmpty(item.Postdata))
                {
                    buffer = _postencoding.GetBytes(item.Postdata);
                }
                if (buffer != null)
                {
                    _request.ContentLength = buffer.Length;
                    _request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
            }
        }

        #endregion

        #region GetData

        /// <summary>
        /// 发送请求成功后获取重定向url
        /// </summary>
        /// <returns></returns>
        private string GetRedirectUrl()
        {
            if (_response.Headers == null || _response.Headers.Count <= 0)
                return string.Empty;

            string locationUrl = _response.Headers["location"];
            if (string.IsNullOrEmpty(locationUrl))
                return string.Empty;

            if (!locationUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) && !locationUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                locationUrl = new Uri(_response.ResponseUri, locationUrl).AbsoluteUri;
            }
            return locationUrl;
        }
        /// <summary>
        /// 获取数据的并解析的方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        private void GetData(HttpItem item, HttpResult result)
        {
            result.StatusCode = _response.StatusCode;
            result.StatusDescription = _response.StatusDescription;
            result.ResponseUrl = _response.ResponseUri.ToString();//获取最后访问的URl
            result.Headers = _response.Headers;
            result.CookieCollection = string.IsNullOrEmpty(Cookie) ? null : CookieHelper.ToCookieCollection(Cookie);
            result.Cookie = Cookie;
            //处理网页Byte
            byte[] responseByte = this.GetByte();
            //只返回ResultByte
            if (item.ResultType == ResultType.Byte)
            {
                result.ResultByte = responseByte;
                return;
            }
            //处理html数据
            if (responseByte != null & responseByte.Length > 0)
            {
                if (_encoding == null)
                    this.SetEncoding(item, result, responseByte);
                //得到返回的HTML
                if (_encoding != null)
                    result.Html = _encoding.GetString(responseByte);
            }
            else
            {
                result.Html = string.Empty;//没有返回任何Html代码
            }
            //是否返回ResultByte
            if (item.ResultType == (ResultType.String | ResultType.Byte))
            {
                result.ResultByte = responseByte;
            }
        }
        /// <summary>
        /// 提取网页Byte
        /// </summary>
        /// <returns></returns>
        private byte[] GetByte()
        {
            using (Stream stream = _response.ContentEncoding.Equals("gzip", StringComparison.CurrentCultureIgnoreCase)
                    ? new GZipStream(_response.GetResponseStream(), CompressionMode.Decompress)
                    : _response.GetResponseStream())
            {
                if (stream == null) return null;

                using (MemoryStream ms = new MemoryStream())
                {
                    const int bufferLen = 256;
                    byte[] buffer = new byte[bufferLen];
                    int count = 0;
                    while ((count = stream.Read(buffer, 0, bufferLen)) > 0)
                    {
                        ms.Write(buffer, 0, count);
                    }
                    return ms.ToArray();
                }
            }
        }
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="item">HttpItem</param>
        /// <param name="result">HttpResult</param>
        /// <param name="responseByte">byte[]</param>
        private void SetEncoding(HttpItem item, HttpResult result, byte[] responseByte)
        {
            Match meta = Regex.Match(Encoding.Default.GetString(responseByte), "<meta[^<]*charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
            string charset = string.Empty;
            if (meta.Groups.Count > 0)
            {
                charset = meta.Groups[1].Value.Trim().ToLower();
            }
            if (charset.Length > 2)
            {
                try
                {
                    _encoding = Encoding.GetEncoding(charset.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Replace("utf8", "utf-8").Trim());
                    return;
                }
                catch { }
            }
            _encoding = string.IsNullOrEmpty(_response.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(_response.CharacterSet.ToLower().Replace("utf8", "utf-8"));
        }

        #endregion
    }
    /// <summary>
    /// Http请求参考类
    /// </summary>
    public class HttpItem
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpItem() { }
        /// <summary>
        /// 向服务器发送GET请求
        /// </summary>
        /// <param name="url"></param>
        public HttpItem(string url)
        {
            this.URL = url;
        }
        /// <summary>
        /// 向服务器发送POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        public HttpItem(string url, string postData)
        {
            this.URL = url;
            this.Method = "POST";
            this.Postdata = postData;
        }

        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string URL { get; set; }

        string _Method = "GET";
        /// <summary>
        /// 请求方式默认为GET方式,当为POST方式时必须设置Postdata的值
        /// </summary>
        public string Method
        {
            get { return _Method; }
            set { _Method = value; }
        }

        string _Accept = "text/html, application/xhtml+xml, */*";
        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        {
            get { return _Accept; }
            set { _Accept = value; }
        }

        /// <summary>
        /// 请求返回类型。
        /// <para>GET默认没有</para>
        /// <para>POST默认：application/x-www-form-urlencoded</para>
        /// </summary>
        public string ContentType { get; set; }

        string _UserAgent = "Haooyou.Ticket.WebApp Api 1.0";
        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent
        {
            get { return _UserAgent; }
            set { _UserAgent = value; }
        }

        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别,一般为utf-8,gbk,gb2312
        /// </summary>
        public Encoding Encoding { get; set; }

        private PostDataType _PostDataType = PostDataType.String;
        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType
        {
            get { return _PostDataType; }
            set { _PostDataType = value; }
        }

        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string Postdata { get; set; }
        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostdataByte { get; set; }
        /// <summary>
        /// 设置代理对象，不想使用IE默认配置就设置为Null，而且不要设置ProxyIp
        /// </summary>
        public WebProxy WebProxy { get; set; }
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// 来源URL。（默认为上次访问的URL，""(空字符串)去掉该属性。）
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath { get; set; }
        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面，默认是不跳转
        /// </summary>
        public Boolean Allowautoredirect { get; set; }

        private int connectionlimit = 1024;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit
        {
            get { return connectionlimit; }
            set { connectionlimit = value; }
        }

        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName { get; set; }
        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd { get; set; }
        /// <summary>
        /// 代理 服务IP ,如果要使用IE代理就设置为ieproxy
        /// </summary>
        public string ProxyIp { get; set; }

        private ResultType resulttype = ResultType.String;
        /// <summary>
        /// 设置返回类型String和Byte
        /// </summary>
        public ResultType ResultType
        {
            get { return resulttype; }
            set { resulttype = value; }
        }

        private InheritWebHeaders headers = new InheritWebHeaders();
        /// <summary>
        /// WebHeaderCollection 对象
        /// </summary>
        public InheritWebHeaders Headers
        {
            get { return headers; }
            set { headers = value; }
        }

        /// <summary>
        ///  获取或设置一个 System.Boolean 值，该值确定是否使用 100-Continue 行为。如果 POST 请求需要 100-Continue 响应，则为 true；否则为 false。默认值为 false。
        /// </summary>
        public Boolean Expect100Continue { get; set; }
        /// <summary>
        /// 设置509证书集合
        /// </summary>
        public X509CertificateCollection ClentCertificates { get; set; }
        /// <summary>
        /// 设置或获取Post参数编码,默认的为Default编码
        /// </summary>
        public Encoding PostEncoding { get; set; }

        private ICredentials _credentials = CredentialCache.DefaultCredentials;
        /// <summary>
        /// 获取或设置请求的身份验证信息。
        /// </summary>
        public ICredentials Credentials
        {
            get { return _credentials; }
            set { _credentials = value; }
        }

        int _Timeout = 60000;
        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        int _ReadWriteTimeout = 20000;
        /// <summary>
        /// 默认写入Post数据超时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return _ReadWriteTimeout; }
            set { _ReadWriteTimeout = value; }
        }

        Boolean _KeepAlive = true;
        /// <summary>
        ///  获取或设置一个值，该值指示是否与 Internet 资源建立持久性连接默认为true。
        /// </summary>
        public Boolean KeepAlive
        {
            get { return _KeepAlive; }
            set { _KeepAlive = value; }
        }

        /// <summary>
        /// 设置请求将跟随的重定向的最大数目
        /// </summary>
        public int MaximumAutomaticRedirections { get; set; }
        /// <summary>
        /// 是否 自动处理301/302的请求（默认为false 不处理，true 自动处理。）
        /// </summary>
        public bool AutoRedirected { get; set; }
    }

    public class InheritWebHeaders : WebHeaderCollection
    {
        /// <summary>
        /// 将标头插入到集合中，不检查此标头是否在受限制的标头列表上。
        /// </summary>
        /// <param name="name">要添加到集合中的标头。</param>
        /// <param name="value">标头的内容。</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="name"/> 为 null，或者为 <see cref="F:System.String.Empty"/>，或者包含无效字符。- 或 - <paramref name="value"/> 包含无效字符。</exception>
        public void AddNoValidate(string name, string value)
        {
            base.AddWithoutValidate(name, value);
        }
        /// <summary>
        /// 将标头插入到集合中，不检查此标头是否在受限制的标头列表上。
        /// </summary>
        public void AddNoValidate(string header)
        {
            if (header == null)
                throw new ArgumentNullException();

            int length = header.IndexOf(':');
            if (length < 0)
                throw new ArgumentException("参数有误", "header");

            string name = header.Substring(0, length);
            string value = header.Substring(length + 1);
            base.AddWithoutValidate(name.Trim(), value.Trim());
        }
    }

    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// 返回的String类型数据
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// 返回的Byte数组
        /// </summary>
        public byte[] ResultByte { get; set; }
        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Headers { get; set; }
        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string StatusDescription { get; set; }
        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 最后访问的URL
        /// </summary>
        public string ResponseUrl { get; set; }
        /// <summary>
        /// 获取重定向的URL
        /// </summary>
        public string RedirectUrl { get; set; }
        /// <summary>
        /// 如果响应状态不是 HttpStatusCode.OK 或 Html为"" 返回true，否则返回false。
        /// </summary>
        public bool IsEmptyOrNotOk { get; set; }
    }

    /// <summary>
    /// 返回类型，ResultType.String | ResultTyte.Byte 两个都返回。
    /// </summary>
    [Flags]
    public enum ResultType
    {
        /// <summary>
        /// 表示只返回字符串 只有Html有数据
        /// </summary>
        String = 1,
        /// <summary>
        /// 只返回字节流 ResultByte，不会自动处理网页编码
        /// </summary>
        Byte = 2
    }
    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        /// <summary>
        /// 字符串类型，这时编码Encoding可不设置
        /// </summary>
        String,
        /// <summary>
        /// Byte类型，需要设置PostdataByte参数的值编码Encoding可设置为空
        /// </summary>
        Byte,
        /// <summary>
        /// 传文件，Postdata必须设置为文件的绝对路径，必须设置Encoding的值
        /// </summary>
        FilePath
    }

    /// <summary>
    /// 外网IP信息
    /// </summary>
    public struct WANipInfo
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 地区/省份
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 互联网服务提供商。（如：电信、联通）
        /// </summary>
        public string ISP { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }
    }


    /// <summary>
    /// cookie操作类
    /// </summary>
    public static class CookieHelper
    {
        #region private
        private enum ErrorFlags
        {
            /// <summary>
            /// 
            /// </summary>
            ERROR_INSUFFICIENT_BUFFER = 122,
            /// <summary>
            /// 
            /// </summary>
            ERROR_INVALID_PARAMETER = 87,
            /// <summary>
            /// 
            /// </summary>
            ERROR_NO_MORE_ITEMS = 259
        }
        private enum InternetFlags
        {
            /// <summary>
            /// 
            /// </summary>
            INTERNET_COOKIE_HTTPONLY = 8192, //Requires IE 8 or higher   
            /// <summary>
            /// 
            /// </summary>
            INTERNET_COOKIE_THIRD_PARTY = 131072,
            /// <summary>
            /// 
            /// </summary>
            INTERNET_FLAG_RESTRICTED_ZONE = 16
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookie(string url, string name, StringBuilder data, ref int dataSize);

        [SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("wininet.dll", EntryPoint = "InternetGetCookieExW", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        static extern bool InternetGetCookieEx([In] string Url, [In] string cookieName, [Out] StringBuilder cookieData, [In, Out] ref uint pchCookieData, uint flags, IntPtr reserved);

        #endregion

        #region 添加一个Cookie + bool SetCookie(string url, string name, string value, DateTime? dt = null)
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="url">http:// + 域名</param>
        /// <param name="name">cookie名</param>
        /// <param name="value">cookie值</param>
        /// <param name="dt">cookie有效期，默认为1天</param>
        /// <returns>设置成功返回true</returns>
        public static bool SetCookie(string url, string name, string value, DateTime? dt = null)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (dt == null)
            {
                dt = DateTime.Now.AddDays(1);
            }
            return InternetSetCookie(url, name, value + "; " + "expires=" + dt.Value.ToUniversalTime().ToString("r"));
        }
        #endregion

        #region 设置整个Cookie + bool SetCookieAll(string url, string cookie, DateTime? dt=null)
        /// <summary>
        /// 设置整个Cookie
        /// </summary>
        /// <param name="url">http:// + 域名</param>
        /// <param name="cookie">整个Cookie字符串</param>
        /// <param name="dt">Cookie有效期，默认为1天</param>
        /// <returns></returns>
        public static int SetCookieAll(string url, string cookie, DateTime? dt = null)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException();

            if (dt == null)
                dt = DateTime.Now.AddDays(1);

            int count = 0;
            foreach (var item in ToCookieList(cookie))
            {
                if (SetCookie(url, item.Name, item.Value, dt))
                    count++;
            }
            return count;
        }
        #endregion

        #region 获取指定url的全部Cookie + string GetCookieAll(string url)
        /// <summary>
        /// 获取指定url的全部Cookie
        /// </summary>
        /// <param name="url">http:// + 域名</param>
        /// <returns></returns>
        public static string GetCookieAll(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException();

            StringBuilder cookie = new StringBuilder(new String(' ', 2048));
            int datasize = cookie.Length;
            InternetGetCookie(url, null, cookie, ref datasize);
            return cookie.ToString().Trim();
        }
        #endregion

        #region 取得WebBrowser的完整Cookie + string GetCookieAllEx(string url)
        /// <summary>
        /// 取得WebBrowser的完整Cookie。因为默认的webBrowser1.Document.Cookie取不到HttpOnly的Cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [SecurityCritical]
        public static string GetCookieAllEx(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException();

            uint pchCookieData = 0;
            uint flag = (uint)InternetFlags.INTERNET_COOKIE_HTTPONLY;

            //Gets the size of the string builder   
            if (InternetGetCookieEx(url, null, null, ref pchCookieData, flag, IntPtr.Zero))
            {
                pchCookieData++;
                StringBuilder cookieData = new StringBuilder((int)pchCookieData);

                //Read the cookie   
                if (InternetGetCookieEx(url, null, cookieData, ref pchCookieData, flag, IntPtr.Zero))
                {
                    DemandWebPermission(new Uri(url));
                    return cookieData.ToString();
                }
            }

            int lastErrorCode = Marshal.GetLastWin32Error();

            if (lastErrorCode != (int)ErrorFlags.ERROR_NO_MORE_ITEMS)
            {
                throw new Win32Exception(lastErrorCode);
            }
            return null;
        }
        private static void DemandWebPermission(Uri uri)
        {
            string uriString = UriToString(uri);

            if (uri.IsFile)
            {
                string localPath = uri.LocalPath;
                new FileIOPermission(FileIOPermissionAccess.Read, localPath).Demand();
            }
            else
            {
                new WebPermission(NetworkAccess.Connect, uriString).Demand();
            }
        }
        private static string UriToString(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            UriComponents components = (uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString);
            return new StringBuilder(uri.GetComponents(components, UriFormat.SafeUnescaped), 2083).ToString();
        }
        #endregion

        #region 清除指定url的全部cookie + void ClearCookieAll(string url)
        /// <summary>
        /// 清除指定url的全部cookie
        /// </summary>
        /// <param name="url">http:// + 域名</param>
        /// <returns></returns>
        public static int ClearCookieAll(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException();

            string cookie = GetCookieAll(url);//获取全部cookie
            if (string.IsNullOrEmpty(cookie) || !cookie.Contains(";"))
                return -1;

            int count = 0;
            foreach (var nameValue in cookie.Split(';'))
            {
                int n = nameValue.IndexOf('=');
                if (n > 0)
                {
                    if (InternetSetCookie(url, nameValue.Substring(0, n).Trim(), "expires=Wed, 19 Feb 1999 13:01:49 GMT"))
                        count++;
                }
            }
            return count;
        }
        #endregion

        #region 根据cookie字符串生成Cookie列表 + List<CookieItem> GetCookieList(string cookie)
        /// <summary>
        /// 检测指定cookie名是否应该排除，应该排除返回true
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsExcludeName(string name)
        {
            if (name.Equals("expires", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("path", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("domain", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("max-age", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private static CookieItem GetCookieItem(string cookie)
        {
            var m = Regex.Match(cookie, @"([\s\S]*?)=([\s\S]*?)$");
            if (m.Success)
            {
                var name = m.Groups[1].Value.Trim();
                if (!IsExcludeName(name))
                {
                    return new CookieItem(name, m.Groups[2].Value.Trim());
                }
            }
            return null;
        }
        /// <summary>
        /// 根据字符生成Cookie列表
        /// </summary>
        /// <param name="cookies">Cookie字符串</param>
        /// <returns></returns>
        public static List<CookieItem> ToCookieList(string cookies)
        {
            if (string.IsNullOrEmpty(cookies))
                throw new ArgumentNullException();

            var list = new List<CookieItem>();
            var cookieArr = cookies.Split(';');
            foreach (var c in cookieArr)
            {
                var cArr = c.Split(',');
                if (cArr.Length == 1)
                {
                    var ci = GetCookieItem(cArr[0]);
                    if (ci != null)
                        list.Add(ci);
                }
                else if (cArr.Length == 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var ci = GetCookieItem(cArr[i]);
                        if (ci != null)
                            list.Add(ci);
                    }
                }
                else if (cArr.Length > 2)
                {
                    string c1 = "", c2 = "";
                    for (int i = 0; i < cArr.Length; i++)
                    {
                        if (i == 0)
                            c1 = cArr[i];
                        else
                        {
                            c2 += cArr[i];
                            if (i != cArr.Length - 1)
                                c2 += ",";
                        }
                    }
                    var ci = GetCookieItem(c1);
                    if (ci != null)
                        list.Add(ci);
                    ci = GetCookieItem(c2);
                    if (ci != null)
                        list.Add(ci);
                }
            }
            return list;
        }

        /*public static List<CookieItem> ToCookieList(string cookie)
        {
            if (string.IsNullOrEmpty(cookie))
                throw new ArgumentNullException();

            var list = new List<CookieItem>();
            foreach (var item in cookie.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                Match m = Regex.Match(item, @"([\s\S]*?)=([\s\S]*?)$");
                if (m.Success)
                {
                    var name = m.Groups[1].Value.Trim();
                    if (!IsExcludeName(name))
                    {
                        list.Add(new CookieItem(name, m.Groups[2].Value.Trim()));
                    }
                }
            }
            return list;
        }*/
        #endregion

        #region 根据cookie名称得到Cookie值 + string GetCookieValue(string cookie, string name)
        /// <summary>
        /// 根据cookie名称得到Cookie值,name区分大小写
        /// </summary>
        /// <param name="cookie">字符串Cookie</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetCookieValue(string cookie, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException();

            foreach (CookieItem item in ToCookieList(cookie))
            {
                if (item.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return item.Value;
            }
            return string.Empty;
        }
        #endregion

        #region 修改cookie值，没有找到name返回旧cookie
        /// <summary>
        /// 修改cookie值，没有找到name返回旧cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="name"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string SetCookieValue(string cookie, string name, string newValue)
        {
            var items = CookieHelper.ToCookieList(cookie);
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == name)
                {
                    items[i].Value = newValue;
                    return CookieFormat(items);
                }
            }
            return cookie;
        }
        #endregion

        #region List＜CookieItem＞拼接为标准格式。 + string CookieFormat(List<CookieItem> cookies)
        /// <summary>
        /// List＜CookieItem＞拼接为标准格式。（拼接结果：{name}={value}; {name}={value};）
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static string CookieFormat(List<CookieItem> cookies)
        {
            if (cookies == null)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            foreach (CookieItem cookie in cookies)
            {
                sb.AppendFormat("{0}={1}; ", cookie.Name, cookie.Value);
            }
            return sb.ToString();
        }
        #endregion

        #region Cookie合并更新 + List<CookieItem> CookieMergeUpdate(*)
        /// <summary>
        /// Cookie合并更新
        /// </summary>
        /// <param name="oldCookies">旧cookie列表，同时返回合并后cookie</param>
        /// <param name="newCookies">新cookie列表</param>
        /// <returns></returns>
        public static List<CookieItem> CookieMergeUpdate(List<CookieItem> oldCookies, List<CookieItem> newCookies)
        {
            if (oldCookies == null || newCookies == null)
            {
                return oldCookies ?? newCookies;
            }
            foreach (CookieItem cookie in newCookies)//遍历新cookie列表
            {
                //从旧cookie列表里根据cookie名称搜索新cookie
                int index = oldCookies.FindIndex(c => c.Name.Equals(cookie.Name, StringComparison.CurrentCultureIgnoreCase));
                if (index != -1)
                {
                    //在新cookie里找到与旧cookie一样的cookie名称，把折这条旧cookie值替换成找到的新cookie值
                    oldCookies[index].Value = cookie.Value;
                }
                else
                {
                    oldCookies.Add(cookie);
                }
            }
            return oldCookies;
        }
        /// <summary>
        /// Cookie合并更新
        /// </summary>
        /// <param name="oldCookie">旧cookie</param>
        /// <param name="newCookie">新cookie</param>
        /// <returns></returns>
        public static string CookieMergeUpdate(string oldCookie, string newCookie)
        {
            if (string.IsNullOrEmpty(oldCookie) || string.IsNullOrEmpty(newCookie))
            {
                return string.IsNullOrEmpty(oldCookie) ? newCookie : oldCookie;
            }
            return CookieFormat(CookieMergeUpdate(ToCookieList(oldCookie), ToCookieList(newCookie)));
        }
        /// <summary>
        /// Cookie合并更新
        /// </summary>
        /// <param name="oldCookies">旧cookie</param>
        /// <param name="newCookies">新cookie</param>
        /// <returns></returns>
        public static CookieCollection CookieMergeUpdate(CookieCollection oldCookies, CookieCollection newCookies)
        {
            if (oldCookies == null || newCookies == null)
                return oldCookies ?? newCookies;

            CookieCollection resCookies = new CookieCollection();
            foreach (Cookie newCookie in newCookies)//遍历新cookie列表
            {
                foreach (Cookie oldCookie in oldCookies)//遍历旧cookie列表
                {
                    if (oldCookie.Name.Equals(newCookie.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        //在新cookie里找到与旧cookie一样的cookie名称，把折这条旧cookie值替换成找到的新cookie值
                        oldCookie.Value = newCookie.Value;
                        resCookies.Add(oldCookie);
                    }
                    else
                    {
                        resCookies.Add(oldCookie);
                        resCookies.Add(newCookie);
                    }
                }
            }
            return resCookies;
        }
        #endregion

        #region 字符串cookie 与 CookieCollection 相互转换
        /// <summary>
        /// 字符串cookie转为CookieCollection
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static CookieCollection ToCookieCollection(string cookie)
        {
            CookieCollection resCookies = new CookieCollection();
            List<CookieItem> cookies = ToCookieList(cookie);
            foreach (CookieItem item in cookies)
            {
                resCookies.Add(new Cookie(item.Name, item.Value));
            }
            return resCookies;
        }
        /// <summary>
        /// CookieCollection.ToString扩展方法
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static string ToStringEx(this CookieCollection cookies)
        {
            if (cookies == null)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            foreach (Cookie cookie in cookies)
            {
                sb.AppendFormat("{0}; ", cookie);
            }
            return sb.ToString();
        }
        #endregion

    }

    /// <summary>
    /// Cookie对象
    /// </summary>
    public class CookieItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public CookieItem(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}