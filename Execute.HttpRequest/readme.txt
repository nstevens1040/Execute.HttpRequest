####################################################################################################
| nstevens1040                                                                                     |
|   ______                     _         _    _ _   _         _____                            _   |
| |  ____|                   | |       | |  | | | | |       |  __ \                          | |   |
| | |__  __  _____  ___ _   _| |_ ___  | |__| | |_| |_ _ __ | |__) |___  __ _ _   _  ___  ___| |_  |
| |  __| \ \/ / _ \/ __| | | | __/ _ \ |  __  | __| __| '_ \|  _  // _ \/ _` | | | |/ _ \/ __| __| |
| | |____ >  <  __/ (__| |_| | ||  __/_| |  | | |_| |_| |_) | | \ \  __/ (_| | |_| |  __/\__ \ |_  |
| |______/_/\_\___|\___|\__,_|\__\___(_)_|  |_|\__|\__| .__/|_|  \_\___|\__, |\__,_|\___||___/\__| |
|                                                     | |                  | |                     |
|                                                     |_|                  |_|                     |
| .NET Framework class library used the send HTTP requests and parse the response.                 |
####################################################################################################

################
# Installation #
################

Quick Start
===== =====
Make Execute.HttpRequest available in your current Windows PowerShell session using the script below.

[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12
iex (irm "https://github.com/nstevens1040/Execute.HttpRequest/releases/download/v1.1.8/Quick-Start.ps1")

Test it.

$r = [Execute.HttpRequest]::Send("https://nstevens1040.github.io/Execute.HttpRequest/")
$r.ResponseText

.NET Framework project in Visual Studio
==== ========= ======= == ====== ====== 
Clone the repository and build it in Visual Studio. It will create the library file, **Execute.HttpRequest.dll** in `.\source\repos\Execute.HttpRequest\Execute.HttpRequest\bin\Debug\Execute.HttpRequest.dll`.  
Make a reference to **Execute.HttpRequest.dll** and use the **Send** method:  

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

Windows PowerShell 5.1
======= ========== ===
* Add-Type using the path to the DLL
Clone the repository and build it in Visual Studio. It will create the library file, **Execute.HttpRequest.dll** in `.\source\repos\Execute.HttpRequest\Execute.HttpRequest\bin\Debug\Execute.HttpRequest.dll`.  
Now you can use the **Path** parameter, specifying the path to the DLL file as the parameter to Add-Type instead of TypeDefinition.  

Add-Type -Path .\source\repos\Execute.HttpRequest\Execute.HttpRequest\bin\Debug\Execute.HttpRequest.dll

Either way, once the command completes, you can use the library in PowerShell like this:

$re = [Execute.HttpRequest]::Send(
    "https://fakedomain.com/post",
    [System.Net.Http.HttpMethod]::Post,
    ([ordered]@{"x-csrf-token"="blahblahblahfaketokenblahblah"}),
    $cookies, # CookieContainer example below
    "application/x-www-form-urlencoded",
    "data=that&i%27m=sending&via=httpost"
)

#########
# Usage #
#########

Positional parameters:
========== ===========

| Uri            |                                                         |
|----------------|---------------------------------------------------------|
| **Type**       |   System.String                                         |
| **Position**   |   0                                                     |
| **Purpose**    |   The full URI to the resource you are requesting.      |
| **Required?**  |   Yes                                                   |  
  
string uri = @"https://twitter.com/sessions";

| Method         |                                                     |
|----------------|-----------------------------------------------------|
| **Type**       |   System.Net.Http.HttpMethod                        |
| **Position**   |   1                                                 |
| **Purpose**    |   The HTTP method you're using to send the request. Must be one of **GET, POST, PUT, DELETE, TRACE, OPTIONS, or HEAD**. |
| **Required?**  |   No (**Defaults to GET** if not specified)         |  

HttpMethod method = HttpMethod.Post;

| Headers        |                                                     |
|----------------|-----------------------------------------------------|
| **Type**       |   System.Collections.Specialized.OrderedDictionary  |
| **Position**   |   2                                                 |
| **Purpose**    |   Http headers (**not Content-\***) to send along with the HTTP request. |
| **Required?**  |   No (**HTTP headers Path, User-Agent, and Content-Length are sent automatically**)       |  

OrderedDictionary headers = new OrderedDictionary();
headers.Add("x-csrf-token","blahblahblahfaketokenblahblah");
 
| Cookies        |                                                     |
|----------------|-----------------------------------------------------|
| **Type**       |   System.Net.CookieCollection                       |
| **Position**   |   3                                                 |
| **Purpose**    |   CookieCollection object populated with 1 or more System.Net.Cookie objects to send along with the HTTP request. |
| **Required?**  |   No                                                |  

CookieCollection cookies = new CookieCollection();
cookies.Add((new Cookie(){
    Name="MyCookieName",
    Value="MyCookieValue",
    Path="/",
    Domain=".fakedomain.com",
    Expires=DateTime.Now.AddDays(365)
}));

| Content-Type   |                                                 |
|----------------|-------------------------------------------------|
| **Type**       |   System.String                                 |
| **Position**   |   4                                             |
| **Purpose**    |   Mimetype string to include if you're sending data along with your HTTP request. |
| **Required?**  |   No                                            |  

string contentType = @"application/x-www-form-urlencoded";

| Body           |                                                 |
|----------------|-------------------------------------------------|
| **Type**       |   System.String                                 |
| **Position**   |   5                                             |
| **Purpose**    |   Data that you're sending along with your HTTP request. **HTTP method must be either POST or PUT.** |
| **Required?**  |   No                                            |  

string body = @"Any=string&will=do%2c&so=long&as=it&serves=your&purpose=well";

#################
# Return object #
#################  
The Send method returns and instance of an object with the typename **Execute.RetObject**.  
The object contains 5 properties:  

| Name                     |  Type                                              |  Description                                          |
|--------------------------|----------------------------------------------------|-------------------------------------------------------|
| **CookieCollection**     |  System.Net.CookieCollection                       |  Cookies returned from HTTP request                   |
| **HtmlDocument**         |  System.Object                                     |  HTML document parsed via **HTMLFile COMObject**      |
| **HttpResponseHeaders**  |  System.Collections.Specialized.OrderedDictionary  |  Headers returned from HTTP request                   |
| **HttpResponseMessage**  |  System.Net.Http.HttpResponseMessage               |  Initial object returned from HttpClient.SendAsync()  | 
| **ResponseText**         |  System.String                                     |  Text body of the HTTP response                       |  
  
The advantage of using this library is the ability to take, for example, the CookieCollection object returned from one request and pipe those cookies into the next request.  
To illustrate:

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

*Build Status => [![Build status](https://ci.appveyor.com/api/projects/status/github/nstevens1040/Execute.HttpRequest?svg=true)](https://ci.appveyor.com/project/nstevens1040/Execute.HttpRequest)
