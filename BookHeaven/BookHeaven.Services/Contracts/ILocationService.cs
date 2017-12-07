using System.Threading.Tasks;
using BookHeaven.Services.Models;

namespace BookHeaven.Services.Contracts
{
    public interface ILocationService : IServce
    {
        Task<LocationDto> GetCurrentLocationAsync(string ipAddress);

        Task AddLocationVisitAsync(string city, string country);
    }
}
