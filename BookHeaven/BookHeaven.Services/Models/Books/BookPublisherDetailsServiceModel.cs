using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;
using System.Linq;

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
                .ForMember(b => b.TotalSales, cfg => cfg.MapFrom(b => b.Orders.Sum(o => o.Quantity)))
                .ForMember(b => b.TotalRevenue, cfg => cfg.MapFrom(b => b.Orders.Sum(o => o.Quantity * o.BookPrice)))
                .ForMember(b => b.Rating, cfg => cfg.MapFrom(b => b.Votes.Where(v => v.VoteValue != null).Sum(v => (int)v.VoteValue)));
        }
    }
}