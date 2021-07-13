using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bitbucket.Cloud.Net;
using Bitbucket.Cloud.Net.Common.Authentication;
using Bitbucket.Cloud.Net.Models.v2;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChangeLogger
{
    public class ConfluenceClient
    {
        public async Task<string> GetChangeLogUrlPageAsync()
        {
            // get this page children https://insysvt.atlassian.net/wiki/spaces/DM/pages/217187157/ChangeLog
            var changeLogPage = await this.HttpGetAsync("https://insysvt.atlassian.net/wiki/rest/api/content/217187157/child?expand=page");

            var master64ApiLink = changeLogPage.page.results[0]._links.self;

            return master64ApiLink;
        }

        public async Task<(string, int)> GetChangeLogBodyAndVersionPageAsync(string url)
        {
            https: //insysvt.atlassian.net/wiki/rest/api/content/217187157?expand=body.view
            var changeLogPage = await this.HttpGetAsync(url.SetQueryParam("expand=body.view,version"));

            var body = changeLogPage.body.view.value;
            int version = changeLogPage.version.number;
            return (body, version);
        }

        public async Task UpdateChangeLog(List<ChangeLogModel> changeLogs, string changeLogUrl)
        {
            // get this page children https://insysvt.atlassian.net/wiki/spaces/DM/pages/217187157/ChangeLog
            var (changeLogPage, version) = await this.GetChangeLogBodyAndVersionPageAsync(changeLogUrl);
            // {
            //     "type": "page",
            //     "title": "master64",
            //     "version": {
            //         "number": 3
            //     },
            //     "body": {
            //         "storage" : {
            //             "value": "New data",
            //             "representation": "storage"
            //         }
            //     }
            // }
            var json = JObject.FromObject(new
            {
                type = "page",
                title = "master64 changelog",
                version = new
                {
                    number = version + 1
                },
                body = new
                {
                    storage = new
                    {
                        //"<p><a href=\"https://insysvt.atlassian.net/browse/ORSOTT-7387\" data-card-appearance=\"inline\">https://insysvt.atlassian.net/browse/ORSOTT-7387</a></p>
                        value = changeLogPage + string.Join("", changeLogs
                            .Select(c => $"<p><a href=\"https://insysvt.atlassian.net/browse/{c.JiraTask}\" data-card-appearance=\"inline\">https://insysvt.atlassian.net/browse/{c.JiraTask}</a></p>")),
                        representation = "storage"
                    }
                }
            });
            // json.json.json.version = new { };
            // json.json.body = new { };
            // json.body.storage = new { };
            // json.body.storage.value = changeLogPage + string.Join("<br>", changeLogs.Select(c => c.JiraTask + ": " + c.Description));
            // json.body.storage.representation = "storage";
            var jsonStr = json.ToString();

            var response = await this.HttpPutAsync(changeLogUrl, jsonStr);
            var current = response.status;
            var a = 1;
        }


        private async Task<dynamic> HttpPutAsync(string url, string json)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Basic cC5rYW5hcmVrQGluc3lzLnBsOklrT3N3dWtyOENTWWUzYk9ud09YNDFGQg");
            var response = await client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            var stream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);

            var serializer = new JsonSerializer();
            dynamic content = serializer.Deserialize(jsonReader);

            return content;
        }

        private async Task<dynamic> HttpGetAsync(string url)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Basic cC5rYW5hcmVrQGluc3lzLnBsOklrT3N3dWtyOENTWWUzYk9ud09YNDFGQg");
            
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cC5rYW5hcmVrQGluc3lzLnBsOklrT3N3dWtyOENTWWUzYk9ud09YNDFGQg==");

            var response = await client.GetAsync(url).ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);

            var serializer = new JsonSerializer();
            dynamic content = serializer.Deserialize(jsonReader);

            return content;
        }
    }
}