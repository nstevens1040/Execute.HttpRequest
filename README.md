[![Build status](https://ci.appveyor.com/api/projects/status/github/nstevens1040/Execute.HttpRequest?svg=true)](https://ci.appveyor.com/project/nstevens1040/Execute.HttpRequest)  
# Execute.HttpRequest
.NET Framework class library used the send HTTP requests and parse the response.  
# Installation  
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
            RetObject response = Create.Send(
                "https://fakedomain.com/post",
                headers,
                HttpMethod.Post,
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
### Add-Type  
#### via TypeDefinition  
The PowerShell command below will pull the C# code directly from GitHub and make the library available to use within the current PowerShell session using the **TypeDefinition** parameter to the Add-Type command.  
```ps1
Add-Type -TypeDefinition ([System.Net.WebClient]::new()).DownloadString(
    "https://raw.githubusercontent.com/nstevens1040/Execute.HttpRequest/master/Execute.HttpRequest/Execute.HttpRequest.cs"
) -ReferencedAssemblies @(
    "C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Net.Http\v4.0_4.0.0.0__b03f5f7f11d50a3a\System.Net.Http.dll",
    "C:\Windows\Microsoft.Net\assembly\GAC_MSIL\Microsoft.CSharp\v4.0_4.0.0.0__b03f5f7f11d50a3a\Microsoft.CSharp.dll"
)
```  
#### via DLL path  
Clone the repository and build it in Visual Studio. It will create the library file, **Execute.HttpRequest.dll** in `.\source\repos\Execute.HttpRequest\Execute.HttpRequest\bin\Debug\Execute.HttpRequest.dll`.  
Now you can use the **Path** parameter, specifying the path to the DLL file as the parameter to Add-Type instead of TypeDefinition.  
```ps1
Add-Type -Path .\source\repos\Execute.HttpRequest\Execute.HttpRequest\bin\Debug\Execute.HttpRequest.dll
```  
Either way, once the command completes, you can use the library in PowerShell like this:
```ps1
$re = [Execute.HttpRequest]::Send(
    "https://fakedomain.com/post",
    ([ordered]@{"x-csrf-token"="blahblahblahfaketokenblahblah"}),
    [System.Net.Http.HttpMethod]::Post,
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
### Headers  
|                |                                                     |
|----------------|-----------------------------------------------------|
| **Type**       |   System.Collections.Specialized.OrderedDictionary  |
| **Position**   |   1                                                 |
| **Purpose**    |   Http headers (**not Content-\***) to send along with the HTTP request. |
| **Required?**  |   No (**HTTP headers Path, User-Agent, and Content-Length are sent automatically**)       |  
```cs
OrderedDictionary headers = new OrderedDictionary();
headers.Add("x-csrf-token","blahblahblahfaketokenblahblah");
```  
### Method  
|                |                                                     |
|----------------|-----------------------------------------------------|
| **Type**       |   System.Net.Http.HttpMethod                        |
| **Position**   |   2                                                 |
| **Purpose**    |   The HTTP method you're using to send the request. Must be one of **GET, POST, PUT, DELETE, TRACE, OPTIONS, or HEAD**. |
| **Required?**  |   No (**Defaults to GET** if not specified)         |  
```cs
HttpMethod method = HttpMethod.Post;
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
## Return object  
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
```ps1
$re = [Execute.HttpRequest]::Send(
    "https://fakedomain.com/post",
    ([ordered]@{"x-csrf-token"="blahblahblahfaketokenblahblah"}),
    [System.Net.Http.HttpMethod]::Post,
    $cookies,
    "application/x-www-form-urlencoded",
    "data=that&i%27m=sending&via=httpost"
)
$next = [Execute.HttpRequest]::Send(
    "https://fakedomain.com/someotherresource",
    $re.HttpResponseHeaders,
    [System.Net.Http.HttpMethod]::Get,
    $re.CookieCollection
)
```  
