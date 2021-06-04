using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using mshtml;
using System.Reflection;
using System.Web;

namespace Execute
{
    public class RetObject
    {
        public string ResponseText
        {
            get;
            set;
        }
        public OrderedDictionary HttpResponseHeaders
        {
            get;
            set;
        }
        public CookieCollection CookieCollection
        {
            get;
            set;
        }
        public HTMLDocument HtmlDocument
        {
            get;
            set;
        }
        public HttpResponseMessage HttpResponseMessage
        {
            get;
            set;
        }
    }
    public class Utils
    {
        public void Ctor()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }
        private static HTMLDocument DOMParser(string responseText)
        {
            HTMLDocument domobj = new HTMLDocument();
            IHTMLDocument2 doc2 = (IHTMLDocument2)domobj;
            doc2.write(new object[] { responseText });
            doc2.close();
            return domobj;
        }
        private static CookieCollection SetCookieParser(List<string> setCookie, CookieCollection cooks, CookieCollection initCookies)
        {
            List<Exception> ex = new List<Exception>();
            List<Hashtable> rckevalues = new List<Hashtable>();
            List<Hashtable> ckevalues = new List<Hashtable>();
            List<Cookie> ckeList = new List<Cookie>();
            if (initCookies != null)
            {
                for (int i = 0; i < initCookies.Count; i++)
                {
                    ckeList.Add(initCookies[i]);
                    Hashtable h = new Hashtable();
                    h.Add(initCookies[i].Name, initCookies[i].Value);
                    ckevalues.Add(h);
                }
            }
            try
            {

                List<string> rckes = new List<string>();
                for (int i = 0; i < cooks.Count; i++)
                {
                    rckes.Add(cooks[i].Name);
                }
                foreach (string set in setCookie)
                {
                    Cookie cke = new Cookie();
                    for (int i = 0; i < set.Split(';').ToList().Count; i++)
                    {
                        List<string> v = new List<string>();
                        string item = set.Split(';').ToList()[i];
                        for (int ii = 1; ii < item.Split('=').ToList().Count; ii++)
                        {
                            v.Add(item.Split('=')[ii]);
                        }
                        string va = String.Join('='.ToString(), v);
                        string key = new Regex(@"^(\s*)").Replace(item.Split('=').ToList()[0], "");
                        string value = new Regex(@"^(\s*)").Replace(va, "");
                        if (i == 0)
                        {
                            cke.Name = key;
                            cke.Value = value;
                        }
                        else
                        {
                            switch (key.ToLower())
                            {
                                case "comment":
                                    cke.Comment = value;
                                    break;
                                case "commenturi":
                                    cke.CommentUri = new Uri(value);
                                    break;
                                case "httponly":
                                    cke.HttpOnly = bool.Parse(value);
                                    break;
                                case "discard":
                                    cke.Discard = bool.Parse(value);
                                    break;
                                case "domain":
                                    cke.Domain = value;
                                    break;
                                case "expires":
                                    cke.Expires = DateTime.Parse(value);
                                    break;
                                case "path":
                                    cke.Path = value;
                                    break;
                                case "port":
                                    cke.Port = value;
                                    break;
                                case "secure":
                                    cke.Secure = bool.Parse(value);
                                    break;
                                case "version":
                                    cke.Version = int.Parse(value);
                                    break;
                            }
                        }
                        if (!rckes.Contains(cke.Name))
                        {
                            cooks.Add(cke);
                        }
                        else
                        {
                            CookieCollection tempRCkes = new CookieCollection();
                            for (int ii = 0; ii < cooks.Count; ii++)
                            {
                                Cookie current = cooks[ii];
                                if (!current.Name.Equals(cke.Name))
                                {
                                    tempRCkes.Add(current);
                                }
                            }
                            tempRCkes.Add(cke);
                            cooks = new CookieCollection();
                            for (int ii = 0; ii < tempRCkes.Count; ii++)
                            {
                                cooks.Add(tempRCkes[ii]);
                            }
                            rckes = new List<string>();
                            for (int ii = 0; ii < cooks.Count; ii++)
                            {
                                rckes.Add(cooks[ii].Name);
                            }
                        }
                    }
                }
                if (cooks != null)
                {
                    for (int i = 0; i < cooks.Count; i++)
                    {
                        Hashtable h = new Hashtable();
                        h.Add(cooks[i].Name, cooks[i].Value);
                        rckevalues.Add(h);
                    }
                }
                if (ckevalues != null)
                {
                    if (rckevalues.Count > 0)
                    {
                        List<string> rNames = new List<string>();
                        List<string> rValue = new List<string>();
                        for (int i = 0; i < rckevalues.Count; i++)
                        {
                            string rcken = rckevalues[i].Keys.ToString();
                            string rckev = rckevalues[i].Values.ToString();
                            rNames.Add(rcken);
                            rValue.Add(rckev);
                        }
                        for (int i = 0; i < ckevalues.Count; i++)
                        {
                            string ckeName = ckevalues[i].Keys.ToString();
                            string ckeValu = ckevalues[i].Values.ToString();
                            if (!rValue.Contains(ckeValu))
                            {
                                if (!rNames.Contains(ckeName))
                                {
                                    cooks.Add(ckeList.Where(item => item.Name.Equals(ckeName)).FirstOrDefault());
                                }
                            }
                            else
                            {
                                if (!rNames.Contains(ckeName))
                                {
                                    cooks.Add(ckeList.Where(item => item.Name.Equals(ckeName)).FirstOrDefault());
                                }
                            }
                        }
                    }
                    else
                    {
                        ckeList.ForEach(i => cooks.Add(i));
                    }
                }
            }
            catch (Exception e)
            {
                ex.Add(e);
            }
            return cooks;
        }
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }
                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
        public async Task<RetObject> SendHttp(string uri, HttpMethod method = null, OrderedDictionary headers = null, CookieCollection cookies = null, string contentType = null, string body = null, string filepath = null)
        {
            byte[] reStream;
            RetObject retObj = new RetObject();
            HttpResponseMessage res = new HttpResponseMessage();
            OrderedDictionary httpResponseHeaders = new OrderedDictionary();
            CookieCollection responseCookies;
            CookieCollection rCookies = new CookieCollection();
            List<string> setCookieValue = new List<string>();
            CookieContainer coo = new CookieContainer();
            dynamic dom = new object();
            string htmlString = String.Empty;
            if (method == null)
            {
                method = HttpMethod.Get;
            }
            HttpClientHandler handle = new HttpClientHandler()
            {
                AutomaticDecompression = (DecompressionMethods)1 & (DecompressionMethods)2,
                UseProxy = false,
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = Int32.MaxValue,
                MaxConnectionsPerServer = Int32.MaxValue,
                MaxResponseHeadersLength = Int32.MaxValue,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handle);
            if (!client.DefaultRequestHeaders.Contains("User-Agent"))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");
            }
            client.DefaultRequestHeaders.Add("Path", (new Uri(uri).PathAndQuery));
            List<string> headersToSkip = new List<string>();
            headersToSkip.Add("Accept");
            headersToSkip.Add("pragma");
            headersToSkip.Add("Cache-Control");
            headersToSkip.Add("Date");
            headersToSkip.Add("Content-Length");
            headersToSkip.Add("Content-Type");
            headersToSkip.Add("Expires");
            headersToSkip.Add("Last-Modified");
            if (headers != null)
            {
                headersToSkip.ForEach((i) => {
                    headers.Remove(i);
                });
                IEnumerator enume = headers.Keys.GetEnumerator();
                while (enume.MoveNext())
                {
                    string key = enume.Current.ToString();
                    string value = String.Join("\n", headers[key]);
                    if (client.DefaultRequestHeaders.Contains(key))
                    {
                        client.DefaultRequestHeaders.Remove(key);
                    }
                    try
                    {
                        client.DefaultRequestHeaders.Add(key, value);
                    }
                    catch
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
                    }
                }
            }
            if (cookies != null)
            {
                IEnumerator cnume = cookies.GetEnumerator();
                while (cnume.MoveNext())
                {
                    Cookie cook = (Cookie)cnume.Current;
                    coo.Add(cook);
                }
                handle.CookieContainer = coo;
            }
            bool except = false;
            switch (method.ToString())
            {
                case "DELETE":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    if (res.Content.Headers.ContentEncoding.ToString().ToLower().Equals("gzip"))
                    {
                        reStream = res.Content.ReadAsByteArrayAsync().Result;
                        htmlString = Unzip(reStream);
                    }
                    else
                    {
                        htmlString = res.Content.ReadAsStringAsync().Result;
                    }
                    try
                    {
                        setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                    }
                    catch
                    { }
                    res.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    res.Content.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    responseCookies = handle.CookieContainer.GetCookies(new Uri(uri));
                    rCookies = SetCookieParser(setCookieValue, responseCookies, cookies);
                    if (!String.IsNullOrEmpty(htmlString))
                    {
                        dom = DOMParser(htmlString);
                        retObj.HtmlDocument = dom;
                    }
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "GET":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    if (res.Content.Headers.ContentEncoding.ToString().ToLower().Equals("gzip"))
                    {
                        reStream = res.Content.ReadAsByteArrayAsync().Result;
                        htmlString = Unzip(reStream);
                    } else
                    {
                        try
                        {
                            htmlString = res.Content.ReadAsStringAsync().Result;
                        }
                        catch
                        {
                            except = true;
                        }
                        if (except)
                        {
                            var responseStream = await res.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                htmlString = await sr.ReadToEndAsync().ConfigureAwait(false);
                            }
                        }

                    }
                    try
                    {
                        setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                    }
                    catch
                    { }
                    res.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    res.Content.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    responseCookies = handle.CookieContainer.GetCookies(new Uri(uri));
                    rCookies = SetCookieParser(setCookieValue, responseCookies, cookies);
                    if (!String.IsNullOrEmpty(htmlString))
                    {
                        dom = DOMParser(htmlString);
                        retObj.HtmlDocument = dom;
                    }
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "HEAD":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    try
                    {
                        setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                    }
                    catch
                    { }
                    res.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    res.Content.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    responseCookies = handle.CookieContainer.GetCookies(new Uri(uri));
                    rCookies = SetCookieParser(setCookieValue, responseCookies, cookies);
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "OPTIONS":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    if (res.Content.Headers.ContentEncoding.ToString().ToLower().Equals("gzip"))
                    {
                        reStream = res.Content.ReadAsByteArrayAsync().Result;
                        htmlString = Unzip(reStream);
                    }
                    else
                    {
                        htmlString = res.Content.ReadAsStringAsync().Result;
                    }
                    try
                    {
                        setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                    }
                    catch
                    { }
                    res.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    res.Content.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    responseCookies = handle.CookieContainer.GetCookies(new Uri(uri));
                    rCookies = SetCookieParser(setCookieValue, responseCookies, cookies);
                    if (!String.IsNullOrEmpty(htmlString))
                    {
                        dom = DOMParser(htmlString);
                        retObj.HtmlDocument = dom;
                    }
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "POST":
                    if (String.IsNullOrEmpty(contentType))
                    {
                        contentType = "application/x-www-form-urlencoded";
                    }
                    if (!String.IsNullOrEmpty(body))
                    {
                        switch (contentType)
                        {
                            case @"application/x-www-form-urlencoded":
                                res = await client.SendAsync(
                                    (new HttpRequestMessage(method, uri)
                                    {
                                        Content = (new StringContent(body, Encoding.UTF8, contentType))
                                    })
                                );
                                break;
                            case @"multipart/form-data":
                                MultipartFormDataContent mpc = new MultipartFormDataContent("Boundary----" + DateTime.Now.Ticks.ToString("x"));
                                if (!String.IsNullOrEmpty(filepath))
                                {
                                    if (File.Exists(filepath))
                                    {
                                        ByteArrayContent bac = new ByteArrayContent(File.ReadAllBytes(filepath));
                                        bac.Headers.Add("Content-Type", MimeMapping.GetMimeMapping(filepath));
                                        bac.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("attachment");
                                        bac.Headers.ContentDisposition.Name = "file";
                                        bac.Headers.ContentDisposition.FileName = new FileInfo(filepath).Name;
                                        mpc.Add(bac, new FileInfo(filepath).Name);
                                    }
                                }
                                if (!String.IsNullOrEmpty(body))
                                {
                                    StringContent sc = new StringContent(body, Encoding.UTF8, @"application/x-www-form-urlencoded");
                                    mpc.Add(sc);
                                }
                                res = await client.SendAsync(
                                    (new HttpRequestMessage(method, uri)
                                    {
                                        Content = mpc
                                    })
                                );
                                break;
                            default:
                                res = await client.SendAsync(
                                    (new HttpRequestMessage(method, uri)
                                    {
                                        Content = (new StringContent(body, Encoding.UTF8, contentType))
                                    })
                                );
                                break;
                        }
                        if (res.Content.Headers.ContentEncoding.ToString().ToLower().Equals("gzip"))
                        {
                            reStream = res.Content.ReadAsByteArrayAsync().Result;
                            htmlString = Unzip(reStream);
                        }
                        else
                        {
                            htmlString = res.Content.ReadAsStringAsync().Result;
                        }
                        try
                        {
                            setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                        }
                        catch
                        { }
                        res.Headers.ToList().ForEach((i) =>
                        {
                            httpResponseHeaders.Add(i.Key, i.Value);
                        });
                        res.Content.Headers.ToList().ForEach((i) =>
                        {
                            httpResponseHeaders.Add(i.Key, i.Value);
                        });
                    }
                    else
                    {
                        switch (contentType)
                        {
                            case @"application/x-www-form-urlencoded":
                                res = await client.SendAsync(
                                    (new HttpRequestMessage(method, uri)
                                    {
                                        Content = (new StringContent(String.Empty, Encoding.UTF8, contentType))
                                    })
                                );
                                break;
                            case @"multipart/form-data":
                                MultipartFormDataContent mpc = new MultipartFormDataContent("Boundary----" + DateTime.Now.Ticks.ToString("x"));
                                if (!String.IsNullOrEmpty(filepath))
                                {
                                    if (File.Exists(filepath))
                                    {
                                        ByteArrayContent bac = new ByteArrayContent(File.ReadAllBytes(filepath));
                                        bac.Headers.Add("Content-Type", MimeMapping.GetMimeMapping(filepath));
                                        bac.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("attachment");
                                        bac.Headers.ContentDisposition.Name = "file";
                                        bac.Headers.ContentDisposition.FileName = new FileInfo(filepath).Name;
                                        mpc.Add(bac, new FileInfo(filepath).Name);
                                    }
                                }
                                res = await client.SendAsync(
                                    (new HttpRequestMessage(method, uri)
                                    {
                                        Content = mpc
                                    })
                                );
                                break;
                            default:
                                res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                                break;
                        }
                        if (res.Content.Headers.ContentEncoding.ToString().ToLower().Equals("gzip"))
                        {
                            reStream = res.Content.ReadAsByteArrayAsync().Result;
                            htmlString = Unzip(reStream);
                        }
                        else
                        {
                            htmlString = res.Content.ReadAsStringAsync().Result;
                        }
                        try
                        {
                            setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                        }
                        catch
                        { }
                        res.Headers.ToList().ForEach((i) =>
                        {
                            httpResponseHeaders.Add(i.Key, i.Value);
                        });
                        res.Content.Headers.ToList().ForEach((i) =>
                        {
                            httpResponseHeaders.Add(i.Key, i.Value);
                        });
                    }
                    responseCookies = handle.CookieContainer.GetCookies(new Uri(uri));
                    rCookies = SetCookieParser(setCookieValue, responseCookies, cookies);
                    if (!String.IsNullOrEmpty(htmlString))
                    {
                        dom = DOMParser(htmlString);
                        retObj.HtmlDocument = dom;
                    }
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "PUT":
                    if (String.IsNullOrEmpty(contentType))
                    {
                        contentType = "application/x-www-form-urlencoded";
                    }
                    if (!String.IsNullOrEmpty(body))
                    {
                        res = await client.SendAsync(
                            (new HttpRequestMessage(method, uri)
                            {
                                Content = (new StringContent(body, Encoding.UTF8, contentType))
                            })
                        );
                        if (res.Content.Headers.ContentEncoding.ToString().ToLower().Equals("gzip"))
                        {
                            reStream = res.Content.ReadAsByteArrayAsync().Result;
                            htmlString = Unzip(reStream);
                        }
                        else
                        {
                            htmlString = res.Content.ReadAsStringAsync().Result;
                        }
                        try
                        {
                            setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                        }
                        catch
                        { }
                        res.Headers.ToList().ForEach((i) =>
                        {
                            httpResponseHeaders.Add(i.Key, i.Value);
                        });
                        res.Content.Headers.ToList().ForEach((i) =>
                        {
                            httpResponseHeaders.Add(i.Key, i.Value);
                        });
                    }
                    else
                    {
                        res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                        if (res.Content.Headers.ContentEncoding.ToString().ToLower().Equals("gzip"))
                        {
                            reStream = res.Content.ReadAsByteArrayAsync().Result;
                            htmlString = Unzip(reStream);
                        }
                        else
                        {
                            htmlString = res.Content.ReadAsStringAsync().Result;
                        }
                        try
                        {
                            setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                        }
                        catch
                        { }
                        res.Headers.ToList().ForEach((i) =>
                        {
                            httpResponseHeaders.Add(i.Key, i.Value);
                        });
                        res.Content.Headers.ToList().ForEach((i) =>
                        {
                            httpResponseHeaders.Add(i.Key, i.Value);
                        });
                    }
                    responseCookies = handle.CookieContainer.GetCookies(new Uri(uri));
                    rCookies = SetCookieParser(setCookieValue, responseCookies, cookies);
                    if (!String.IsNullOrEmpty(htmlString))
                    {
                        dom = DOMParser(htmlString);
                        retObj.HtmlDocument = dom;
                    }
                    retObj.HtmlDocument = dom;
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "TRACE":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    if (res.Content.Headers.ContentEncoding.ToString().ToLower().Equals("gzip"))
                    {
                        reStream = res.Content.ReadAsByteArrayAsync().Result;
                        htmlString = Unzip(reStream);
                    }
                    else
                    {
                        htmlString = res.Content.ReadAsStringAsync().Result;
                    }
                    try
                    {
                        setCookieValue = res.Headers.GetValues("Set-Cookie").ToList();
                    }
                    catch
                    { }
                    res.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    res.Content.Headers.ToList().ForEach((i) =>
                    {
                        httpResponseHeaders.Add(i.Key, i.Value);
                    });
                    responseCookies = handle.CookieContainer.GetCookies(new Uri(uri));
                    rCookies = SetCookieParser(setCookieValue, responseCookies, cookies);
                    if (!String.IsNullOrEmpty(htmlString))
                    {
                        dom = DOMParser(htmlString);
                        retObj.HtmlDocument = dom;
                    }
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
            }
            if (!String.IsNullOrEmpty(htmlString))
            {
                retObj.ResponseText = htmlString;
            }
            retObj.CookieCollection = rCookies;
            return retObj;
        }
    }
    public class HttpRequest
    {
        public static RetObject Send(string uri, HttpMethod method = null, OrderedDictionary headers = null, CookieCollection cookies = null, string contentType = null, string body = null,string filepath=null)
        {
            Utils utils = new Utils();
            utils.Ctor();
            Task<RetObject> r = utils.SendHttp(uri, method, headers, cookies, contentType, body, filepath);
            return r.Result;
        }
    }
}
