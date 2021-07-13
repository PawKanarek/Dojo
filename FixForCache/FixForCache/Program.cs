using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FixForCache
{
    internal class Program
    {
        private static async Task Main(string[] mainArgs)
        {
            var platformCodename = new[] {"android-tv", "android", "ios"};
            var sorts = new[] {"PublishDate", "Title", "PlayCounts"};
            var categories = new[] {"movies", "beebikino", "eesti-filmiklassika", "eesti-dokumentaalid", "eesti-animatsioonid"};

            while (true)
            {
                foreach (var platform in platformCodename)
                foreach (var sort in sorts)
                foreach (var category in categories)
                {
                    await FilterTiles(category, sort, platform);
                    await Task.Delay(10000); // wait 10s
                }
                await Task.Delay(60000); // wait minute
            }
        }

        private static async Task FilterTiles(string category, string sort, string platformCodename)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.apollo.tv");
            using var reqMsg = new HttpRequestMessage(HttpMethod.Post, "v2/Tile/FilterTiles");

            var strContent =
                $@"{{""TileTypes"":[""vod"",""ser""],""PlatformCodename"":""{platformCodename}"",""Page"":1,""Limit"":30,""Sort"":[{{""Field"":""{sort}"",""Direction"":1,""IsRaw"":false,""ShouldUseCollation"":false}}],""OrProductCodenames"":null,""OrCategoryCodenames"":null,""AndCategoryCodenames"":[],""OrCollectionCodenames"":[""{category}""],""IsPurchased"":false,""ProductType"":null,""IsEpisode"":null,""AggregateSeries"":null,""AdvancedFilters"":null}}";
            Console.WriteLine(strContent);

            reqMsg.Content = new StringContent(strContent, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(reqMsg);

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}