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
        public dynamic HtmlDocument
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
    public class HttpRequest
    {
        private static dynamic DOMParser(string responseText)
        {
            dynamic domobj = Activator.CreateInstance(Type.GetTypeFromCLSID(Guid.Parse(@"{25336920-03F9-11cf-8FD0-00AA00686F13}")));
            List<string> memberNames = new List<string>();
            for (int i = 0; i < memberNames.Count; i++)
            {
                memberNames.Add(domobj.GetType().GetMembers()[i].Name);
            }
            if (memberNames.Contains("IHTMLDocument2_write"))
            {
                domobj.IHTMLDocument2_write(Encoding.Unicode.GetBytes(responseText));
            }
            else
            {
                domobj.write(Encoding.Unicode.GetBytes(responseText));
            }
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
                                case "name":
                                    cke.Name = value;
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
                                case "value":
                                    cke.Value = value;
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
                    if (rckevalues != null)
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
        private static async Task<RetObject> SendHttp(string uri, OrderedDictionary headers = null, HttpMethod method = null, CookieCollection cookies = null, string contentType = null, string body = null)
        {
            RetObject retObj = new RetObject();
            HttpResponseMessage res;
            OrderedDictionary httpResponseHeaders = new OrderedDictionary();
            CookieCollection responseCookies;
            CookieCollection rCookies = new CookieCollection();
            List<string> setCookieValue = new List<string>();
            CookieContainer coo = new CookieContainer();
            dynamic dom;
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
                MaxAutomaticRedirections = 500
            };
            HttpClient client = new HttpClient(handle);
            if (!client.DefaultRequestHeaders.Contains("User-Agent"))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");
            }
            if (client.DefaultRequestHeaders.Contains("Path"))
            {
                client.DefaultRequestHeaders.Remove("Path");
            }
            client.DefaultRequestHeaders.Add("Path", (new Uri(uri).PathAndQuery));
            if (headers != null)
            {
                IEnumerator enume = headers.Keys.GetEnumerator();
                while (enume.MoveNext())
                {
                    string key = enume.Current.ToString();
                    string value = headers[enume.Current.ToString()].ToString();
                    if (client.DefaultRequestHeaders.Contains(key))
                    {
                        client.DefaultRequestHeaders.Remove(key);
                    }
                    client.DefaultRequestHeaders.Add(key, value);
                }
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
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
            switch (method.ToString())
            {
                case "DELETE":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    htmlString = await res.Content.ReadAsStringAsync();
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
                    dom = DOMParser(htmlString);
                    retObj.HtmlDocument = dom;
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "GET":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    htmlString = await res.Content.ReadAsStringAsync();
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
                    dom = DOMParser(htmlString);
                    retObj.HtmlDocument = dom;
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
                    dom = DOMParser(htmlString);
                    retObj.HtmlDocument = dom;
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "OPTIONS":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    htmlString = await res.Content.ReadAsStringAsync();
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
                    dom = DOMParser(htmlString);
                    retObj.HtmlDocument = dom;
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
                        res = await client.SendAsync(
                            (new HttpRequestMessage(method, uri)
                            {
                                Content = (new StringContent(body, Encoding.UTF8, contentType))
                            })
                        );
                        htmlString = await res.Content.ReadAsStringAsync();
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
                        htmlString = await res.Content.ReadAsStringAsync();
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
                    dom = DOMParser(htmlString);
                    retObj.HtmlDocument = dom;
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
                        htmlString = await res.Content.ReadAsStringAsync();
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
                        htmlString = await res.Content.ReadAsStringAsync();
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
                    dom = DOMParser(htmlString);
                    retObj.HtmlDocument = dom;
                    retObj.HttpResponseHeaders = httpResponseHeaders;
                    retObj.HttpResponseMessage = res;
                    break;
                case "TRACE":
                    res = await client.SendAsync((new HttpRequestMessage(method, uri)));
                    htmlString = await res.Content.ReadAsStringAsync();
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
                    dom = DOMParser(htmlString);
                    retObj.HtmlDocument = dom;
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
        public static RetObject Send(string uri, OrderedDictionary headers = null, HttpMethod method = null, CookieCollection cookies = null, string contentType = null, string body = null)
        {
            Task<RetObject> r = SendHttp(uri, headers, method, cookies, contentType, body);
            return r.Result;
        }
    }
}
