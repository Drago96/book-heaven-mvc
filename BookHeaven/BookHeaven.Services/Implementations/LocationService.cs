using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure;
using BookHeaven.Services.Models;
using BookHeaven.Services.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookHeaven.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly BookHeavenDbContext db;
        private readonly IHttpClientService httpClient;
        private readonly IJsonService json;

        public LocationService(IHttpClientService httpClient, IJsonService json, BookHeavenDbContext db)
        {
            this.httpClient = httpClient;
            this.json = json;
            this.db = db;
        }

        public async Task<LocationDto> GetCurrentLocationAsync(string ipAddress)
        {
            string resultJson = await this.httpClient.GetStringAsync(ServiceConstants.GeoLocationApi + ipAddress);
            LocationDto result = this.json.DeserializeObject<LocationDto>(resultJson);
            return result;
        }

        public async Task AddLocationVisitAsync(string city, string country)
        {
            var location = await this.GetLocationAsync(city, country);
            if (location == null)
            {
                await this.AddNewLocationWithVisiAsync(city, country);
            }
            else
            {
                location.SiteVisits++;
                await this.db.SaveChangesAsync();
            }
        }

        private async Task AddNewLocationWithVisiAsync(string city, string country)
        {
            Location location = new Location
            {
                City = city,
                Country = country,
                SiteVisits = 1
            };

            this.db.Add(location);
            await this.db.SaveChangesAsync();
        }

        private Task<Location> GetLocationAsync(string city, string country)
            => this.db.Locations.FirstOrDefaultAsync(l => l.Country == country && l.City == city);

    }
}
