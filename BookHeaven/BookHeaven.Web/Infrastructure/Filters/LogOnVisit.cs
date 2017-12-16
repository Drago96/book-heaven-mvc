using BookHeaven.Services.Contracts;
using BookHeaven.Services.Models.Locations;
using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace BookHeaven.Web.Infrastructure.Filters
{
    public class LogOnVisit : ActionFilterAttribute
    {
        private readonly ILocationService locations;
        private readonly ISiteVisitService visits;

        public LogOnVisit(ILocationService locations, ISiteVisitService visits)
        {
            this.locations = locations;
            this.visits = visits;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var requestSession = context.HttpContext.Session;

            if (requestSession.Keys.Contains(SessionConstants.LocationKey))
            {
                await next();
                return;
            }

            var currentLocation = await this.GetLocation(context);

            if (currentLocation != null &&
                !string.IsNullOrEmpty(currentLocation.Country) &&
                !string.IsNullOrEmpty(currentLocation.City))
            {
                await this.locations.AddLocationVisitAsync(currentLocation.City, currentLocation.Country);
            }

            await this.visits.AddVisitAsync();

            requestSession.SetString(SessionConstants.LocationKey, "True");

            await next();
        }

        private async Task<LocationFromApiDto> GetLocation(ActionExecutingContext context)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var currentLocation = await this.locations.CurrentLocationAsync(ipAddress);
            return currentLocation;
        }
    }
}