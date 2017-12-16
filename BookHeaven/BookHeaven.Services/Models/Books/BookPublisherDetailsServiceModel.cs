using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using BookHeaven.Common.Extensions;

namespace BookHeaven.Services.Models.Books
{
    public class BookPublisherDetailsServiceModel : BookDetailsServiceModel, IHaveCustomMapping
    {
        public new void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Book, BookPublisherDetailsServiceModel>()
                .ForMember(b => b.Categories, cfg => cfg.MapFrom(b => b.Categories.Select(c => c.Category.Name)));
        }
    }
}