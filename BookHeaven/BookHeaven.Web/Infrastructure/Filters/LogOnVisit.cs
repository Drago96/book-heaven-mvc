using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models;
using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookHeaven.Web.Infrastructure.Filters
{
    public class LogOnVisit : ActionFilterAttribute
    {
        private readonly ILocationService locations;
        private readonly ISiteDateVisitService visits;

        public LogOnVisit(ILocationService locations, ISiteDateVisitService visits)
        {
            this.locations = locations;
            this.visits = visits;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var requestSession = context.HttpContext.Session;

            if (requestSession.Keys.Contains(DictionaryKeys.LocationKey))
            {
                return;
            }

            var currentLocation = await this.GetLocation(context);

            if (currentLocation?.Country != null && currentLocation.City != null)
            {

                await this.locations.AddLocationVisitAsync(currentLocation.City, currentLocation.Country);
            }

            await this.visits.AddVisitAsync();

            requestSession.Keys.Append(DictionaryKeys.LocationKey);

        }

        private async Task<LocationDto> GetLocation(ActionExecutingContext context)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var currentLocation = await this.locations.GetCurrentLocationAsync(ipAddress);
            return currentLocation;
        }
    }
}
