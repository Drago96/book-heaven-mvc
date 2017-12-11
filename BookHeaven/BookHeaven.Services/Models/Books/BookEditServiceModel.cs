using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookHeaven.Services.Models.Books
{
    public class BookEditServiceModel : IMapFrom<Book>, IHaveCustomMapping
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public byte[] BookPicture { get; set; }

        public IEnumerable<int> Categories { get; set; } = new List<int>();

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Book, BookEditServiceModel>()
                .ForMember(b => b.Categories, cfg => cfg.MapFrom(b => b.Categories.Select(c => c.Category.Id)));
        }
    }
}