[![Build status](https://ci.appveyor.com/api/projects/status/github/nstevens1040/Execute.HttpRequest?branch=master&svg=true)](https://ci.appveyor.com/project/nstevens1040/execute-httprequest-scamo)
[![codecov](https://codecov.io/gh/nstevens1040/Execute.HttpRequest/branch/master/graph/badge.svg?token=M9gp01ySOr)](https://codecov.io/gh/nstevens1040/Execute.HttpRequest)
[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/nstevens1040/Execute.HttpRequest/blob/master/LICENSE)
[![Ko-Fi](https://img.shields.io/badge/donate-ko--fi-9cf)](https://ko-fi.com/M4M23DLL6)
[![PayPal](https://img.shields.io/badge/donate-paypal-yellow)](https://paypal.me/nstevens312)
[![Bitcoin](https://img.shields.io/badge/donate-bitcoin-orange)](https://nstevens1040.github.io/donthatedonate/)
[![Monero](https://img.shields.io/badge/donate-monero-red)](https://nstevens1040.github.io/sparesomechange/)
# Execute.HttpRequest
.NET Framework class library used the send HTTP requests and parse the response.  
# Installation  
## Quick Start  
Make Execute.HttpRequest available in your current Windows PowerShell session using the script below.  
```ps1
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12
iex (irm "https://github.com/nstevens1040/Execute.HttpRequest/releases/download/v1.1.8/Quick-Start.ps1")
```  
  
<img height=188 width=800 src="https://raw.githubusercontent.com/nstevens1040/Execute.HttpRequest/master/.ignore/render1623183507697.gif"/>  
  
Test it.  
```ps1
$r = [Execute.HttpRequest]::Send("https://google.com/")
$r.ResponseText
```  
The output should be Google.com's HTML response.  
## .NET Framework project in Visual Studio    
Clone the repository and build it in Visual Studio. It will create the library file, **Execute.HttpRequest.dll** in `.\source\repos\Execute.HttpRequest\Execute.HttpRequest\bin\Debug\Execute.HttpRequest.dll`.  
Make a reference to **Execute.HttpRequest.dll** and use the **Send** method:  
```cs
using Execute;
using System;
using System.Net;
using System.Collections.Specialized;
using System.Net.Http;
namespace JustAnExample
{
    public class Test
    {
        public RetObject Request(CookieCollection cookies, OrderedDictionary headers)
        {
            RetObject response = HttpRequest.Send(
                "https://fakedomain.com/post",
                HttpMethod.Post,
                headers,
                cookies,
                "application/x-www-form-urlencoded",
                "data=that&i%27m=sending&via=httpost"
            );
            return response;
        }
    }
}
```  
## Windows PowerShell 5.1  
### Add-Type using the path to the DLL  
Clone the repository and build it in Visual Studio. It will create the library file, **Execute.HttpRequest.dll** in `.\source\repos\Execute.HttpRequest\Execute.HttpRequest\bin\Debug\Execute.HttpRequest.dll`.  
Now you can use the **Path** parameter, specifying the path to the DLL file as the parameter to Add-Type instead of TypeDefinition.  
```ps1
Add-Type -Path .\source\repos\Execute.HttpRequest\Execute.HttpRequest\bin\Debug\Execute.HttpRequest.dll
```  
Once the command completes, you can use the library in PowerShell like this:
```ps1
$re = [Execute.HttpRequest]::Send(
    "https://fakedomain.com/post",
    [System.Net.Http.HttpMethod]::Post,
    ([ordered]@{"x-csrf-token"="blahblahblahfaketokenblahblah"}),
    $cookies, # CookieContainer example below
    "application/x-www-form-urlencoded",
    "data=that&i%27m=sending&via=httpost"
)
```  
# Usage  
## Positional parameters:  
### Uri  
|                |                                                         |
|----------------|---------------------------------------------------------|
| **Type**       |   System.String                                         |
| **Position**   |   0                                                     |
| **Purpose**    |   The full URI to the resource you are requesting.      |
| **Required?**  |   Yes                                                   |  
  
```cs
string uri = @"https://twitter.com/sessions";
```  
### Method  
|                |                                                     |
|----------------|-----------------------------------------------------|
| **Type**       |   System.Net.Http.HttpMethod                        |
| **Position**   |   1                                                 |
| **Purpose**    |   The HTTP method you're using to send the request. Must be one of **GET, POST, PUT, DELETE, TRACE, OPTIONS, or HEAD**. |
| **Required?**  |   No (**Defaults to GET** if not specified)         |  
```cs
HttpMethod method = HttpMethod.Post;
```  
### Headers  
|                |                                                     |
|----------------|-----------------------------------------------------|
| **Type**       |   System.Collections.Specialized.OrderedDictionary  |
| **Position**   |   2                                                 |
| **Purpose**    |   Http headers (**not Content-\***) to send along with the HTTP request. |
| **Required?**  |   No (**HTTP headers Path, User-Agent, and Content-Length are sent automatically**)       |  
```cs
OrderedDictionary headers = new OrderedDictionary();
headers.Add("x-csrf-token","blahblahblahfaketokenblahblah");
```  
### Cookies  
|                |                                                     |
|----------------|-----------------------------------------------------|
| **Type**       |   System.Net.CookieCollection                       |
| **Position**   |   3                                                 |
| **Purpose**    |   CookieCollection object populated with 1 or more System.Net.Cookie objects to send along with the HTTP request. |
| **Required?**  |   No                                                |  
```cs
CookieCollection cookies = new CookieCollection();
cookies.Add((new Cookie(){
    Name="MyCookieName",
    Value="MyCookieValue",
    Path="/",
    Domain=".fakedomain.com",
    Expires=DateTime.Now.AddDays(365)
}));
```  
### Content-Type  
|                |                                                 |
|----------------|-------------------------------------------------|
| **Type**       |   System.String                                 |
| **Position**   |   4                                             |
| **Purpose**    |   Mimetype string to include if you're sending data along with your HTTP request. |
| **Required?**  |   No                                            |  
```cs
string contentType = @"application/x-www-form-urlencoded";
```  
### Body  
|                |                                                 |
|----------------|-------------------------------------------------|
| **Type**       |   System.String                                 |
| **Position**   |   5                                             |
| **Purpose**    |   Data that you're sending along with your HTTP request. **HTTP method must be either POST or PUT.** |
| **Required?**  |   No                                            |  
```cs
string body = @"Any=string&will=do%2c&so=long&as=it&serves=your&purpose=well";
```  
### FilePath  
|                |                                                 |
|----------------|-------------------------------------------------|
| **Type**       |   System.String                                 |
| **Position**   |   6                                             |
| **Purpose**    |   If you're sending a file along with a multipart/form-data POST request, then specify the path to the file you are sending here. |
| **Required?**  |   No                                            |  
```cs
string filepath = Environment.GetEnvironmentVariable(@"userprofile") + "\\file.txt";
```  

## Return object  
The Send method returns and instance of an object with the typename **Execute.RetObject**.  
The object contains 5 properties:  

| Name                     |  Type                                              |  Description                                           |
|--------------------------|----------------------------------------------------|--------------------------------------------------------|
| **CookieCollection**     |  System.Net.CookieCollection                       |  Cookies returned from HTTP request                    |
| **HtmlDocument**         |  System.Object                                     |  HTML document parsed via mshtml.HTMLDocumentClass     |
| **HttpResponseHeaders**  |  System.Collections.Specialized.OrderedDictionary  |  Headers returned from HTTP request                    |
| **HttpResponseMessage**  |  System.Net.Http.HttpResponseMessage               |  Initial object returned from HttpClient.SendAsync()   | 
| **ResponseText**         |  System.String                                     |  Text body of the HTTP response                        |  
  
The advantage of using this library is the ability to take, for example, the CookieCollection object returned from one request and pipe those cookies into the next request.  
To illustrate:  
```ps1
$re = [Execute.HttpRequest]::Send(
    "https://fakedomain.com/post",
    [System.Net.Http.HttpMethod]::Post,
    ([ordered]@{"x-csrf-token"="blahblahblahfaketokenblahblah"}),
    $cookies,
    "application/x-www-form-urlencoded",
    "data=that&i%27m=sending&via=httpost"
)
$next = [Execute.HttpRequest]::Send(
    "https://fakedomain.com/someotherresource",
    [System.Net.Http.HttpMethod]::Get,
    $re.HttpResponseHeaders,
    $re.CookieCollection
)
```  
