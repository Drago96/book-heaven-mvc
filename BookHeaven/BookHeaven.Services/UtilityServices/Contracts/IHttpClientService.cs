using System.Threading.Tasks;

namespace BookHeaven.Services.Utilities
{
    public interface IHttpClientService : IService
    {
        Task<string> GetResponseAsync(string requestUrl);
    }
}