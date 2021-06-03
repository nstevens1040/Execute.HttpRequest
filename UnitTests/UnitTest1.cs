using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Execute;
using System.Collections.Specialized;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;

namespace UnitTest1
{
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
        public static Dictionary<string, HttpMethod> methods = new Dictionary<string, HttpMethod>()
        {
            {"get",HttpMethod.Get },
            {"put",HttpMethod.Put },
            {"post",HttpMethod.Post },
            {"delete",HttpMethod.Delete }
        };
        public JsonDocument ConvertFromJson(string jsonData)
        {
            return JsonDocument.Parse(jsonData);
        }
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow("get")]
        [DataRow("post")]
        [DataRow("put")]
        [DataRow("delete")]
        public void HeadersAndCookies(string method)
        {
            Utils jsonParser = new Utils();
            jsonParser.Ctor();
            string guid = Guid.NewGuid().ToString();
            string cookie_value = Guid.NewGuid().ToString();
            CookieCollection cc = new CookieCollection()
            {
                new Cookie()
                {
                    Name = "test_cookie",
                    Value = cookie_value,
                    Path = "/" + method,
                    Domain = "postman-echo.com",
                    Secure = true,
                    Expires = DateTime.Parse(@"1970-01-01").AddSeconds(1654093426),
                    HttpOnly = false
                }
            };
            OrderedDictionary headers = new OrderedDictionary();
            headers.Add("x-test-header", guid);
            RetObject r = Execute.HttpRequest.Send(
                "https://postman-echo.com/" + method,
                Utils.methods[method],
                headers,
                cc
            );
            JsonDocument json = jsonParser.ConvertFromJson(r.ResponseText);
            JsonElement returnHeaders = json.RootElement.GetProperty("headers");
            string returnGuid = returnHeaders.GetProperty("x-test-header").GetString();
            string cookie_comparison = "test_cookie=" + cookie_value;
            string returnCookie = returnHeaders.GetProperty("cookie").GetString();
            Assert.IsTrue(new Regex(guid).Match(returnGuid).Success);
            Assert.IsTrue(new Regex(cookie_comparison).Match(returnCookie).Success);
        }
    }
}
