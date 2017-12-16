using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BookHeaven.Common.Extensions;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Books
{
    public class BookShoppingCartServiceModel : IMapFrom<Book>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string BookListingPicture { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Book, BookShoppingCartServiceModel>()
                .ForMember(b => b.BookListingPicture,
                    cfg => cfg.MapFrom(b => b.BookListingPicture.ConvertToBase64String()));
        }
    }
}
