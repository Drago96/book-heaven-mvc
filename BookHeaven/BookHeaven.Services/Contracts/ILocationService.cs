using System.Collections.Generic;
using System.Threading.Tasks;
using BookHeaven.Services.Models;
using BookHeaven.Services.Models.Locations;

namespace BookHeaven.Services.Contracts
{
    public interface ILocationService : IServce
    {
        Task<LocationFromApiDto> GetCurrentLocationAsync(string ipAddress);

        Task AddLocationVisitAsync(string city, string country);

        Task<IEnumerable<T>> GetLocationsWithMostVisitsAsync<T>(int countryVisitsToDisplay);
    }
}
