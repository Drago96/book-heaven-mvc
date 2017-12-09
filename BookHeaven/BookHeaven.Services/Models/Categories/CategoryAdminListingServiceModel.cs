using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Categories
{
    public class CategoryAdminListingServiceModel : IMapFrom<Category>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Books { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Category, CategoryAdminListingServiceModel>()
                .ForMember(c => c.Books, cfg => cfg.MapFrom(c => c.Books.Count));
        }
    }
}
