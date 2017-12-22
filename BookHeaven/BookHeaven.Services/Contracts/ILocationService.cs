using BookHeaven.Services.Models.Locations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface ILocationService : IService
    {
        Task<LocationFromApiDto> CurrentLocationAsync(string ipAddress);

        Task<IEnumerable<T>> LocationsWithMostVisitsAsync<T>(int countryVisitsToDisplay);

        Task AddLocationVisitAsync(string city, string country);
    }
}