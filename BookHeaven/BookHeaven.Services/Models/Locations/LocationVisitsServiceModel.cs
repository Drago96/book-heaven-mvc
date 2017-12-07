using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Locations
{
    public class LocationVisitsServiceModel : IMapFrom<IGrouping<string,Location>>, IHaveCustomMapping
    {
        public string Country { get; set; }

        public int Visits { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<IGrouping<string, Location>, LocationVisitsServiceModel>()
                .ForMember(l => l.Country, cfg => cfg.MapFrom(g => g.Key))
                .ForMember(l => l.Visits, cfg => cfg.MapFrom(g => g.Sum(l => l.SiteVisits)));

        }
    }
}
