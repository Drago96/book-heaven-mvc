using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BookHeaven.Common.Extensions;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.ShoppingCart
{
    public class ShoppingCartItemServiceModel : IMapFrom<ShoppingCartItem>, IHaveCustomMapping
    {
        public int Id { get; set; }
        
        public int Quantity { get; set; }

        public string BookTitle { get; set; }

        public string BookPicture { get; set; }

        public decimal BookPrice { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<ShoppingCartItem, ShoppingCartItemServiceModel>()
                .ForMember(s => s.BookPicture,
                    cfg => cfg.MapFrom(s => s.Book.BookListingPicture.ConvertToBase64String()))
                .ForMember(s => s.BookTitle, cfg => cfg.MapFrom(s => s.Book.Title))
                .ForMember(s => s.BookPrice, cfg => cfg.MapFrom(b => b.Book.Price));
        }
    }
}
