using System.Net.Http;
using System.Threading.Tasks;

namespace BookHeaven.Services.Utilities
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient client;

        public HttpClientService()
        {
            this.client = new HttpClient();
        }

        public async Task<string> GetStringAsync(string requestUrl)
        {
            string result = await this.client.GetStringAsync(requestUrl);
            return result;
        }
    }
}
