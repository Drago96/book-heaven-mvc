using System.Threading.Tasks;

namespace BookHeaven.Services.Utilities
{
    public interface IHttpClientService : IServce
    {
        Task<string> GetResponseAsync(string requestUrl);
    }
}
