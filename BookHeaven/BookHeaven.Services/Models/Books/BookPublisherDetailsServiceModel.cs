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

        public int TotalSales { get; set; }

        public decimal TotalRevenue { get; set; }

        public new void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Book, BookPublisherDetailsServiceModel>()
                .ForMember(b => b.Categories, cfg => cfg.MapFrom(b => b.Categories.Select(c => c.Category.Name)))
                .ForMember(b => b.TotalSales, cfg=> cfg.MapFrom(b => b.Orders.Sum(o => o.Quantity)))
                .ForMember(b => b.TotalRevenue, cfg => cfg.MapFrom(b => b.Orders.Sum(o => o.Quantity * o.BookPrice)));
        }
    }
}