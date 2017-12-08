using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure;
using BookHeaven.Services.Models;
using BookHeaven.Services.Models.Locations;
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

        public async Task<LocationFromApiDto> GetCurrentLocationAsync(string ipAddress)
        {
            string resultJson = await this.httpClient.GetResponseAsync(ServiceConstants.GeoLocationApi + ipAddress);
            LocationFromApiDto result = null;
            if (resultJson != null)
            {
                result = this.json.DeserializeObject<LocationFromApiDto>(resultJson);
            }
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

        public async Task<IEnumerable<T>> GetLocationsWithMostVisitsAsync<T>(int countryVisitsToDisplay)
            => await this.db.Locations
                .GroupBy(l => l.Country)
                .OrderByDescending(l => l.Sum(cl => cl.SiteVisits))
                .Take(countryVisitsToDisplay)
                .ProjectTo<T>()
                .ToListAsync();

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
