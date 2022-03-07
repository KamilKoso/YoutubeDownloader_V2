using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YoutubeDownloader.Common.Dispatchers.CommandDispatcher;

namespace YoutubeDownloader.Integration.Tests.Utility
{
    public static class Extensions
    {
        public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string url, ICommand command)
        {
            return httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
        }
    }
}
