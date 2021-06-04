using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Execute;
using System.Collections.Specialized;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using System.IO;
using mshtml;
using System.Linq;
using System.Collections;

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
            {"delete",HttpMethod.Delete },
            {"head",HttpMethod.Head },
            {"options",HttpMethod.Options }

        };
        public JsonDocument ConvertFromJson(string jsonData)
        {
            return JsonDocument.Parse(jsonData);
        }
    }
    [TestClass]
    public class UnitTest1
    {
        public static Int32 content_length = 25165824;
        public static string title_match = "Nicholas Stevens CV 2021";
        public static string dluri = "https://raw.githubusercontent.com/nstevens1040/images/main/empty.vhdx";
        public static string title_check_uri = "https://nanick.org/resume";
        public static string base_64_readme = "IyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIw0KfCBuc3RldmVuczEwNDAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfA0KfCAgIF9fX19fXyAgICAgICAgICAgICAgICAgICAgIF8gICAgICAgICBfICAgIF8gXyAgIF8gICAgICAgICBfX19fXyAgICAgICAgICAgICAgICAgICAgICAgICAgICBfICAgfA0KfCB8ICBfX19ffCAgICAgICAgICAgICAgICAgICB8IHwgICAgICAgfCB8ICB8IHwgfCB8IHwgICAgICAgfCAgX18gXCAgICAgICAgICAgICAgICAgICAgICAgICAgfCB8ICAgfA0KfCB8IHxfXyAgX18gIF9fX19fICBfX18gXyAgIF98IHxfIF9fXyAgfCB8X198IHwgfF98IHxfIF8gX18gfCB8X18pIHxfX18gIF9fIF8gXyAgIF8gIF9fXyAgX19ffCB8XyAgfA0KfCB8ICBfX3wgXCBcLyAvIF8gXC8gX198IHwgfCB8IF9fLyBfIFwgfCAgX18gIHwgX198IF9ffCAnXyBcfCAgXyAgLy8gXyBcLyBfYCB8IHwgfCB8LyBfIFwvIF9ffCBfX3wgfA0KfCB8IHxfX19fID4gIDwgIF9fLyAoX198IHxffCB8IHx8ICBfXy9ffCB8ICB8IHwgfF98IHxffCB8XykgfCB8IFwgXCAgX18vIChffCB8IHxffCB8ICBfXy9cX18gXCB8XyAgfA0KfCB8X19fX19fL18vXF9cX19ffFxfX198XF9fLF98XF9fXF9fXyhfKV98ICB8X3xcX198XF9ffCAuX18vfF98ICBcX1xfX198XF9fLCB8XF9fLF98XF9fX3x8X19fL1xfX3wgfA0KfCAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfCB8ICAgICAgICAgICAgICAgICAgfCB8ICAgICAgICAgICAgICAgICAgICAgfA0KfCAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfF98ICAgICAgICAgICAgICAgICAgfF98ICAgICAgICAgICAgICAgICAgICAgfA0KfCAuTkVUIEZyYW1ld29yayBjbGFzcyBsaWJyYXJ5IHVzZWQgdGhlIHNlbmQgSFRUUCByZXF1ZXN0cyBhbmQgcGFyc2UgdGhlIHJlc3BvbnNlLiAgICAgICAgICAgICAgICAgfA0KIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIw0KDQojIyMjIyMjIyMjIyMjIyMjDQojIEluc3RhbGxhdGlvbiAjDQojIyMjIyMjIyMjIyMjIyMjDQoNCi5ORVQgRnJhbWV3b3JrIHByb2plY3QgaW4gVmlzdWFsIFN0dWRpbw0KPT09PSA9PT09PT09PT0gPT09PT09PSA9PSA9PT09PT0gPT09PT09IA0KQ2xvbmUgdGhlIHJlcG9zaXRvcnkgYW5kIGJ1aWxkIGl0IGluIFZpc3VhbCBTdHVkaW8uIEl0IHdpbGwgY3JlYXRlIHRoZSBsaWJyYXJ5IGZpbGUsICoqRXhlY3V0ZS5IdHRwUmVxdWVzdC5kbGwqKiBpbiBgLlxzb3VyY2VccmVwb3NcRXhlY3V0ZS5IdHRwUmVxdWVzdFxFeGVjdXRlLkh0dHBSZXF1ZXN0XGJpblxEZWJ1Z1xFeGVjdXRlLkh0dHBSZXF1ZXN0LmRsbGAuICANCk1ha2UgYSByZWZlcmVuY2UgdG8gKipFeGVjdXRlLkh0dHBSZXF1ZXN0LmRsbCoqIGFuZCB1c2UgdGhlICoqU2VuZCoqIG1ldGhvZDogIA0KDQp1c2luZyBFeGVjdXRlOw0KdXNpbmcgU3lzdGVtOw0KdXNpbmcgU3lzdGVtLk5ldDsNCnVzaW5nIFN5c3RlbS5Db2xsZWN0aW9ucy5TcGVjaWFsaXplZDsNCnVzaW5nIFN5c3RlbS5OZXQuSHR0cDsNCm5hbWVzcGFjZSBKdXN0QW5FeGFtcGxlDQp7DQogICAgcHVibGljIGNsYXNzIFRlc3QNCiAgICB7DQogICAgICAgIHB1YmxpYyBSZXRPYmplY3QgUmVxdWVzdChDb29raWVDb2xsZWN0aW9uIGNvb2tpZXMsIE9yZGVyZWREaWN0aW9uYXJ5IGhlYWRlcnMpDQogICAgICAgIHsNCiAgICAgICAgICAgIFJldE9iamVjdCByZXNwb25zZSA9IEh0dHBSZXF1ZXN0LlNlbmQoDQogICAgICAgICAgICAgICAgImh0dHBzOi8vZmFrZWRvbWFpbi5jb20vcG9zdCIsDQogICAgICAgICAgICAgICAgSHR0cE1ldGhvZC5Qb3N0LA0KICAgICAgICAgICAgICAgIGhlYWRlcnMsDQogICAgICAgICAgICAgICAgY29va2llcywNCiAgICAgICAgICAgICAgICAiYXBwbGljYXRpb24veC13d3ctZm9ybS11cmxlbmNvZGVkIiwNCiAgICAgICAgICAgICAgICAiZGF0YT10aGF0JmklMjdtPXNlbmRpbmcmdmlhPWh0dHBvc3QiDQogICAgICAgICAgICApOw0KICAgICAgICAgICAgcmV0dXJuIHJlc3BvbnNlOw0KICAgICAgICB9DQogICAgfQ0KfQ0KDQpXaW5kb3dzIFBvd2VyU2hlbGwgNS4xDQo9PT09PT09ID09PT09PT09PT0gPT09DQoqIEFkZC1UeXBlICh2aWEgVHlwZURlZmluaXRpb24pDQpUaGUgUG93ZXJTaGVsbCBjb21tYW5kIGJlbG93IHdpbGwgcHVsbCB0aGUgQyMgY29kZSBkaXJlY3RseSBmcm9tIEdpdEh1YiBhbmQgbWFrZSB0aGUgbGlicmFyeSBhdmFpbGFibGUgdG8gdXNlIHdpdGhpbiB0aGUgY3VycmVudCBQb3dlclNoZWxsIHNlc3Npb24gdXNpbmcgdGhlICoqVHlwZURlZmluaXRpb24qKiBwYXJhbWV0ZXIgdG8gdGhlIEFkZC1UeXBlIGNvbW1hbmQuICANCg0KQWRkLVR5cGUgLVR5cGVEZWZpbml0aW9uIChbU3lzdGVtLk5ldC5XZWJDbGllbnRdOjpuZXcoKSkuRG93bmxvYWRTdHJpbmcoDQogICAgImh0dHBzOi8vcmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbS9uc3RldmVuczEwNDAvRXhlY3V0ZS5IdHRwUmVxdWVzdC9tYXN0ZXIvRXhlY3V0ZS5IdHRwUmVxdWVzdC9FeGVjdXRlLkh0dHBSZXF1ZXN0LmNzIg0KKSAtUmVmZXJlbmNlZEFzc2VtYmxpZXMgQCgNCiAgICAiQzpcV2luZG93c1xNaWNyb3NvZnQuTmV0XGFzc2VtYmx5XEdBQ19NU0lMXFN5c3RlbS5OZXQuSHR0cFx2NC4wXzQuMC4wLjBfX2IwM2Y1ZjdmMTFkNTBhM2FcU3lzdGVtLk5ldC5IdHRwLmRsbCIsDQogICAgIkM6XFdpbmRvd3NcTWljcm9zb2Z0Lk5ldFxhc3NlbWJseVxHQUNfTVNJTFxNaWNyb3NvZnQuQ1NoYXJwXHY0LjBfNC4wLjAuMF9fYjAzZjVmN2YxMWQ1MGEzYVxNaWNyb3NvZnQuQ1NoYXJwLmRsbCINCikNCg0KKiBBZGQtVHlwZSAodmlhIERMTCBwYXRoKQ0KQ2xvbmUgdGhlIHJlcG9zaXRvcnkgYW5kIGJ1aWxkIGl0IGluIFZpc3VhbCBTdHVkaW8uIEl0IHdpbGwgY3JlYXRlIHRoZSBsaWJyYXJ5IGZpbGUsICoqRXhlY3V0ZS5IdHRwUmVxdWVzdC5kbGwqKiBpbiBgLlxzb3VyY2VccmVwb3NcRXhlY3V0ZS5IdHRwUmVxdWVzdFxFeGVjdXRlLkh0dHBSZXF1ZXN0XGJpblxEZWJ1Z1xFeGVjdXRlLkh0dHBSZXF1ZXN0LmRsbGAuICANCk5vdyB5b3UgY2FuIHVzZSB0aGUgKipQYXRoKiogcGFyYW1ldGVyLCBzcGVjaWZ5aW5nIHRoZSBwYXRoIHRvIHRoZSBETEwgZmlsZSBhcyB0aGUgcGFyYW1ldGVyIHRvIEFkZC1UeXBlIGluc3RlYWQgb2YgVHlwZURlZmluaXRpb24uICANCg0KQWRkLVR5cGUgLVBhdGggLlxzb3VyY2VccmVwb3NcRXhlY3V0ZS5IdHRwUmVxdWVzdFxFeGVjdXRlLkh0dHBSZXF1ZXN0XGJpblxEZWJ1Z1xFeGVjdXRlLkh0dHBSZXF1ZXN0LmRsbA0KDQpFaXRoZXIgd2F5LCBvbmNlIHRoZSBjb21tYW5kIGNvbXBsZXRlcywgeW91IGNhbiB1c2UgdGhlIGxpYnJhcnkgaW4gUG93ZXJTaGVsbCBsaWtlIHRoaXM6DQoNCiRyZSA9IFtFeGVjdXRlLkh0dHBSZXF1ZXN0XTo6U2VuZCgNCiAgICAiaHR0cHM6Ly9mYWtlZG9tYWluLmNvbS9wb3N0IiwNCiAgICBbU3lzdGVtLk5ldC5IdHRwLkh0dHBNZXRob2RdOjpQb3N0LA0KICAgIChbb3JkZXJlZF1AeyJ4LWNzcmYtdG9rZW4iPSJibGFoYmxhaGJsYWhmYWtldG9rZW5ibGFoYmxhaCJ9KSwNCiAgICAkY29va2llcywgIyBDb29raWVDb250YWluZXIgZXhhbXBsZSBiZWxvdw0KICAgICJhcHBsaWNhdGlvbi94LXd3dy1mb3JtLXVybGVuY29kZWQiLA0KICAgICJkYXRhPXRoYXQmaSUyN209c2VuZGluZyZ2aWE9aHR0cG9zdCINCikNCg0KIyMjIyMjIyMjDQojIFVzYWdlICMNCiMjIyMjIyMjIw0KDQpQb3NpdGlvbmFsIHBhcmFtZXRlcnM6DQo9PT09PT09PT09ID09PT09PT09PT09DQoNCnwgVXJpICAgICAgICAgICAgfCAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwtLS0tLS0tLS0tLS0tLS0tfC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLXwNCnwgKipUeXBlKiogICAgICAgfCAgIFN5c3RlbS5TdHJpbmcgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwgKipQb3NpdGlvbioqICAgfCAgIDAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwgKipQdXJwb3NlKiogICAgfCAgIFRoZSBmdWxsIFVSSSB0byB0aGUgcmVzb3VyY2UgeW91IGFyZSByZXF1ZXN0aW5nLiAgICAgIHwNCnwgKipSZXF1aXJlZD8qKiAgfCAgIFllcyAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwgIA0KICANCnN0cmluZyB1cmkgPSBAImh0dHBzOi8vdHdpdHRlci5jb20vc2Vzc2lvbnMiOw0KDQp8IE1ldGhvZCAgICAgICAgIHwgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwtLS0tLS0tLS0tLS0tLS0tfC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tfA0KfCAqKlR5cGUqKiAgICAgICB8ICAgU3lzdGVtLk5ldC5IdHRwLkh0dHBNZXRob2QgICAgICAgICAgICAgICAgICAgICAgICB8DQp8ICoqUG9zaXRpb24qKiAgIHwgICAxICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwgKipQdXJwb3NlKiogICAgfCAgIFRoZSBIVFRQIG1ldGhvZCB5b3UncmUgdXNpbmcgdG8gc2VuZCB0aGUgcmVxdWVzdC4gTXVzdCBiZSBvbmUgb2YgKipHRVQsIFBPU1QsIFBVVCwgREVMRVRFLCBUUkFDRSwgT1BUSU9OUywgb3IgSEVBRCoqLiB8DQp8ICoqUmVxdWlyZWQ/KiogIHwgICBObyAoKipEZWZhdWx0cyB0byBHRVQqKiBpZiBub3Qgc3BlY2lmaWVkKSAgICAgICAgIHwgIA0KDQpIdHRwTWV0aG9kIG1ldGhvZCA9IEh0dHBNZXRob2QuUG9zdDsNCg0KfCBIZWFkZXJzICAgICAgICB8ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8DQp8LS0tLS0tLS0tLS0tLS0tLXwtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLXwNCnwgKipUeXBlKiogICAgICAgfCAgIFN5c3RlbS5Db2xsZWN0aW9ucy5TcGVjaWFsaXplZC5PcmRlcmVkRGljdGlvbmFyeSAgfA0KfCAqKlBvc2l0aW9uKiogICB8ICAgMiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8DQp8ICoqUHVycG9zZSoqICAgIHwgICBIdHRwIGhlYWRlcnMgKCoqbm90IENvbnRlbnQtXCoqKikgdG8gc2VuZCBhbG9uZyB3aXRoIHRoZSBIVFRQIHJlcXVlc3QuIHwNCnwgKipSZXF1aXJlZD8qKiAgfCAgIE5vICgqKkhUVFAgaGVhZGVycyBQYXRoLCBVc2VyLUFnZW50LCBhbmQgQ29udGVudC1MZW5ndGggYXJlIHNlbnQgYXV0b21hdGljYWxseSoqKSAgICAgICB8ICANCg0KT3JkZXJlZERpY3Rpb25hcnkgaGVhZGVycyA9IG5ldyBPcmRlcmVkRGljdGlvbmFyeSgpOw0KaGVhZGVycy5BZGQoIngtY3NyZi10b2tlbiIsImJsYWhibGFoYmxhaGZha2V0b2tlbmJsYWhibGFoIik7DQogDQp8IENvb2tpZXMgICAgICAgIHwgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwtLS0tLS0tLS0tLS0tLS0tfC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tfA0KfCAqKlR5cGUqKiAgICAgICB8ICAgU3lzdGVtLk5ldC5Db29raWVDb2xsZWN0aW9uICAgICAgICAgICAgICAgICAgICAgICB8DQp8ICoqUG9zaXRpb24qKiAgIHwgICAzICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwgKipQdXJwb3NlKiogICAgfCAgIENvb2tpZUNvbGxlY3Rpb24gb2JqZWN0IHBvcHVsYXRlZCB3aXRoIDEgb3IgbW9yZSBTeXN0ZW0uTmV0LkNvb2tpZSBvYmplY3RzIHRvIHNlbmQgYWxvbmcgd2l0aCB0aGUgSFRUUCByZXF1ZXN0LiB8DQp8ICoqUmVxdWlyZWQ/KiogIHwgICBObyAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwgIA0KDQpDb29raWVDb2xsZWN0aW9uIGNvb2tpZXMgPSBuZXcgQ29va2llQ29sbGVjdGlvbigpOw0KY29va2llcy5BZGQoKG5ldyBDb29raWUoKXsNCiAgICBOYW1lPSJNeUNvb2tpZU5hbWUiLA0KICAgIFZhbHVlPSJNeUNvb2tpZVZhbHVlIiwNCiAgICBQYXRoPSIvIiwNCiAgICBEb21haW49Ii5mYWtlZG9tYWluLmNvbSIsDQogICAgRXhwaXJlcz1EYXRlVGltZS5Ob3cuQWRkRGF5cygzNjUpDQp9KSk7DQoNCnwgQ29udGVudC1UeXBlICAgfCAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8DQp8LS0tLS0tLS0tLS0tLS0tLXwtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tfA0KfCAqKlR5cGUqKiAgICAgICB8ICAgU3lzdGVtLlN0cmluZyAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwgKipQb3NpdGlvbioqICAgfCAgIDQgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8DQp8ICoqUHVycG9zZSoqICAgIHwgICBNaW1ldHlwZSBzdHJpbmcgdG8gaW5jbHVkZSBpZiB5b3UncmUgc2VuZGluZyBkYXRhIGFsb25nIHdpdGggeW91ciBIVFRQIHJlcXVlc3QuIHwNCnwgKipSZXF1aXJlZD8qKiAgfCAgIE5vICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8ICANCg0Kc3RyaW5nIGNvbnRlbnRUeXBlID0gQCJhcHBsaWNhdGlvbi94LXd3dy1mb3JtLXVybGVuY29kZWQiOw0KDQp8IEJvZHkgICAgICAgICAgIHwgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfA0KfC0tLS0tLS0tLS0tLS0tLS18LS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLXwNCnwgKipUeXBlKiogICAgICAgfCAgIFN5c3RlbS5TdHJpbmcgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8DQp8ICoqUG9zaXRpb24qKiAgIHwgICA1ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfA0KfCAqKlB1cnBvc2UqKiAgICB8ICAgRGF0YSB0aGF0IHlvdSdyZSBzZW5kaW5nIGFsb25nIHdpdGggeW91ciBIVFRQIHJlcXVlc3QuICoqSFRUUCBtZXRob2QgbXVzdCBiZSBlaXRoZXIgUE9TVCBvciBQVVQuKiogfA0KfCAqKlJlcXVpcmVkPyoqICB8ICAgTm8gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwgIA0KDQpzdHJpbmcgYm9keSA9IEAiQW55PXN0cmluZyZ3aWxsPWRvJTJjJnNvPWxvbmcmYXM9aXQmc2VydmVzPXlvdXImcHVycG9zZT13ZWxsIjsNCg0KIyMjIyMjIyMjIyMjIyMjIyMNCiMgUmV0dXJuIG9iamVjdCAjDQojIyMjIyMjIyMjIyMjIyMjIyAgDQpUaGUgU2VuZCBtZXRob2QgcmV0dXJucyBhbmQgaW5zdGFuY2Ugb2YgYW4gb2JqZWN0IHdpdGggdGhlIHR5cGVuYW1lICoqRXhlY3V0ZS5SZXRPYmplY3QqKi4gIA0KVGhlIG9iamVjdCBjb250YWlucyA1IHByb3BlcnRpZXM6ICANCg0KfCBOYW1lICAgICAgICAgICAgICAgICAgICAgfCAgVHlwZSAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8ICBEZXNjcmlwdGlvbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwNCnwtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLXwtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tfC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS18DQp8ICoqQ29va2llQ29sbGVjdGlvbioqICAgICB8ICBTeXN0ZW0uTmV0LkNvb2tpZUNvbGxlY3Rpb24gICAgICAgICAgICAgICAgICAgICAgIHwgIENvb2tpZXMgcmV0dXJuZWQgZnJvbSBIVFRQIHJlcXVlc3QgICAgICAgICAgICAgICAgICAgfA0KfCAqKkh0bWxEb2N1bWVudCoqICAgICAgICAgfCAgU3lzdGVtLk9iamVjdCAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8ICBIVE1MIGRvY3VtZW50IHBhcnNlZCB2aWEgKipIVE1MRmlsZSBDT01PYmplY3QqKiAgICAgIHwNCnwgKipIdHRwUmVzcG9uc2VIZWFkZXJzKiogIHwgIFN5c3RlbS5Db2xsZWN0aW9ucy5TcGVjaWFsaXplZC5PcmRlcmVkRGljdGlvbmFyeSAgfCAgSGVhZGVycyByZXR1cm5lZCBmcm9tIEhUVFAgcmVxdWVzdCAgICAgICAgICAgICAgICAgICB8DQp8ICoqSHR0cFJlc3BvbnNlTWVzc2FnZSoqICB8ICBTeXN0ZW0uTmV0Lkh0dHAuSHR0cFJlc3BvbnNlTWVzc2FnZSAgICAgICAgICAgICAgIHwgIEluaXRpYWwgb2JqZWN0IHJldHVybmVkIGZyb20gSHR0cENsaWVudC5TZW5kQXN5bmMoKSAgfCANCnwgKipSZXNwb25zZVRleHQqKiAgICAgICAgIHwgIFN5c3RlbS5TdHJpbmcgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfCAgVGV4dCBib2R5IG9mIHRoZSBIVFRQIHJlc3BvbnNlICAgICAgICAgICAgICAgICAgICAgICB8ICANCiAgDQpUaGUgYWR2YW50YWdlIG9mIHVzaW5nIHRoaXMgbGlicmFyeSBpcyB0aGUgYWJpbGl0eSB0byB0YWtlLCBmb3IgZXhhbXBsZSwgdGhlIENvb2tpZUNvbGxlY3Rpb24gb2JqZWN0IHJldHVybmVkIGZyb20gb25lIHJlcXVlc3QgYW5kIHBpcGUgdGhvc2UgY29va2llcyBpbnRvIHRoZSBuZXh0IHJlcXVlc3QuICANClRvIGlsbHVzdHJhdGU6DQoNCiRyZSA9IFtFeGVjdXRlLkh0dHBSZXF1ZXN0XTo6U2VuZCgNCiAgICAiaHR0cHM6Ly9mYWtlZG9tYWluLmNvbS9wb3N0IiwNCiAgICBbU3lzdGVtLk5ldC5IdHRwLkh0dHBNZXRob2RdOjpQb3N0LA0KICAgIChbb3JkZXJlZF1AeyJ4LWNzcmYtdG9rZW4iPSJibGFoYmxhaGJsYWhmYWtldG9rZW5ibGFoYmxhaCJ9KSwNCiAgICAkY29va2llcywNCiAgICAiYXBwbGljYXRpb24veC13d3ctZm9ybS11cmxlbmNvZGVkIiwNCiAgICAiZGF0YT10aGF0JmklMjdtPXNlbmRpbmcmdmlhPWh0dHBvc3QiDQopDQokbmV4dCA9IFtFeGVjdXRlLkh0dHBSZXF1ZXN0XTo6U2VuZCgNCiAgICAiaHR0cHM6Ly9mYWtlZG9tYWluLmNvbS9zb21lb3RoZXJyZXNvdXJjZSIsDQogICAgW1N5c3RlbS5OZXQuSHR0cC5IdHRwTWV0aG9kXTo6R2V0LA0KICAgICRyZS5IdHRwUmVzcG9uc2VIZWFkZXJzLA0KICAgICRyZS5Db29raWVDb2xsZWN0aW9uDQopDQoNCipCdWlsZCBTdGF0dXMgPT4gWyFbQnVpbGQgc3RhdHVzXShodHRwczovL2NpLmFwcHZleW9yLmNvbS9hcGkvcHJvamVjdHMvc3RhdHVzL2dpdGh1Yi9uc3RldmVuczEwNDAvRXhlY3V0ZS5IdHRwUmVxdWVzdD9zdmc9dHJ1ZSldKGh0dHBzOi8vY2kuYXBwdmV5b3IuY29tL3Byb2plY3QvbnN0ZXZlbnMxMDQwL0V4ZWN1dGUuSHR0cFJlcXVlc3QpDQo=";
        public static string tw_uri = "https://api.twitter.com/1.1/guest/activate.json";
        [TestMethod]
        [DataRow("get")]
        [DataRow("post")]
        [DataRow("put")]
        [DataRow("delete")]
        public void HeadersAndCookies(string method)
        {
            string payload = "guid1=" + Guid.NewGuid().ToString() + "&guid2=" + Guid.NewGuid().ToString();
            RetObject r = null;
            List<string> include_payload = new List<string>();
            include_payload.Add("post");
            include_payload.Add("put");
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
            bool preq = false;
            if(include_payload.Contains(method))
            {
                preq = true;
                r = Execute.HttpRequest.Send(
                    "https://postman-echo.com/" + method,
                    Utils.methods[method],
                    headers,
                    cc,
                    "application/x-www-form-urlencoded",
                    payload
                );
            } else
            {
                r = Execute.HttpRequest.Send(
                    "https://postman-echo.com/" + method,
                    Utils.methods[method],
                    headers,
                    cc
                );
            }
            JsonDocument json = jsonParser.ConvertFromJson(r.ResponseText);
            JsonElement returnHeaders = json.RootElement.GetProperty("headers");
            string returnGuid = returnHeaders.GetProperty("x-test-header").GetString();
            string cookie_comparison = "test_cookie=" + cookie_value;
            string returnCookie = returnHeaders.GetProperty("cookie").GetString();
            Assert.IsTrue(new Regex(guid).Match(returnGuid).Success);
            Assert.IsTrue(new Regex(cookie_comparison).Match(returnCookie).Success);
            if (preq)
            {
                JsonElement f = json.RootElement.GetProperty("form");
                string payload_echo = "guid1=" + f.GetProperty("guid1").GetString() + "&guid2=" + f.GetProperty("guid2").GetString();
                Assert.IsTrue(payload_echo.Equals(payload));
            }
        }
        [TestMethod]
        [DataRow("head")]
        [DataRow("options")]
        public void HeadAndOptions(string method)
        {
            if (method.Equals("head"))
            {
                RetObject r = Execute.HttpRequest.Send(
                    dluri,
                    Utils.methods[method]
                );
                Int32 returnedLength = Int32.Parse(r.HttpResponseMessage.Content.Headers.ContentLength.ToString());
                Assert.IsTrue(returnedLength.Equals(UnitTest1.content_length));
            } else
            {
                List<string> tw_return_allow_headers = new List<string>();
                OrderedDictionary headers = new OrderedDictionary();
                headers.Add("method", "OPTIONS");
                headers.Add("authority", "api.twitter.com");
                headers.Add("scheme", "https");
                headers.Add("path", "/1.1/guest/activate.json");
                headers.Add("pragma", "no-cache");
                headers.Add("cache-control", "no-cache");
                headers.Add("accept", "*/*");
                headers.Add("access-control-request-method", "POST");
                headers.Add("access-control-request-headers", "authorization,x-csrf-token,x-twitter-active-user,x-twitter-client-language");
                headers.Add("origin", "https://twitter.com");
                headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36");
                headers.Add("sec-fetch-mode", "cors");
                headers.Add("sec-fetch-site", "same-site");
                headers.Add("sec-fetch-dest", "empty");
                headers.Add("referer", "https://twitter.com/");
                headers.Add("accept-encoding", "gzip, deflate");
                headers.Add("accept-language", "en-US,en;q=0.9");
                RetObject r = Execute.HttpRequest.Send(
                    tw_uri,
                    HttpMethod.Options,
                    headers
                );
                Assert.IsTrue(r.HttpResponseMessage.Headers.Contains("access-control-allow-headers"));
            }
        }
        [TestMethod]
        [DataRow("get")]
        [DataRow("post")]
        [DataRow("put")]
        [DataRow("delete")]
        public void HTML_DOM_Test(string method)
        {
            RetObject r = Execute.HttpRequest.Send(
                title_check_uri,
                Utils.methods[method]
            );
            Assert.IsTrue(r.HtmlDocument.title.ToString().Equals(title_match));
        }
        [TestMethod]
        public void MultipartFormData()
        {
            Utils jsonParser = new Utils();
            jsonParser.Ctor();
            string readmefile = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Execute.HttpRequest\\readme.txt";
            string control_compare = File.ReadAllText(readmefile, Encoding.UTF8);
            RetObject r = Execute.HttpRequest.Send(
                "https://httpbin.org/post",
                HttpMethod.Post,
                null,
                null,
                "multipart/form-data",
                null,
                readmefile
            );
            JsonDocument json = jsonParser.ConvertFromJson(r.ResponseText);
            JsonElement returnFiles = json.RootElement.GetProperty("files");
            string exp_compare = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(returnFiles.GetProperty("file").GetString()));
            Assert.IsTrue(control_compare.Equals(exp_compare));
        }
        [TestMethod]
        public void TestCookieParsing()
        {

            OrderedDictionary headers = new OrderedDictionary();
            headers.Add("method", "GET");
            headers.Add("authority", "www.facebook.com");
            headers.Add("scheme", "https");
            headers.Add("path", "/");
            headers.Add("pragma", "no-cache");
            headers.Add("cache-control", "no-cache");
            headers.Add("dnt", "1");
            headers.Add("upgrade-insecure-requests", "1");
            headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.77 Safari/537.36");
            headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            headers.Add("sec-fetch-site", "none");
            headers.Add("sec-fetch-mode", "navigate");
            headers.Add("sec-fetch-user", "?1");
            headers.Add("sec-fetch-dest", "document");
            headers.Add("sec-ch-ua", "\" Not; A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
            headers.Add("sec-ch-ua-mobile", "?0");
            headers.Add("accept-encoding", "gzip, deflate");
            headers.Add("accept-language", "en-US,en;q=0.9");
            RetObject r = Execute.HttpRequest.Send(
                "https://www.facebook.com/",
                HttpMethod.Get,
                headers
            );
            Assert.IsTrue(r.CookieCollection.Count > 0);
        }
        [TestMethod]
        public void TestCookieNullReturn()
        {
            CookieCollection collection = new CookieCollection();
            Cookie cookie = new Cookie()
            {
                Name = "fakecookie",
                Value = "fakevalue",
                Domain = ".nstevens1040.github.io",
                Path = "/",
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                HttpOnly = false
            };
            collection.Add(cookie);
            RetObject r = Execute.HttpRequest.Send(
                "https://nstevens1040.github.io/Execute.HttpRequest/",
                HttpMethod.Get,
                null,
                collection
            );
            Assert.IsTrue(r.CookieCollection.Count == 1);
        }
        [TestMethod]
        public void TestHttpResponseHeaders()
        {
            RetObject r = Execute.HttpRequest.Send("https://nstevens1040.github.io/Execute.HttpRequest/");
            Assert.IsTrue(r.HttpResponseHeaders.Count > 0);
        }
        [TestMethod]
        public void MakeRetObject()
        {
            RetObject r = new RetObject();
            Assert.IsTrue(r != null);
        }
    }
}
