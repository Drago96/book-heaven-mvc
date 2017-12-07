using System.Threading.Tasks;

namespace BookHeaven.Services.Utilities
{
    public interface IHttpClientService : IServce
    {
        Task<string> GetStringAsync(string requestUrl);
    }
}
