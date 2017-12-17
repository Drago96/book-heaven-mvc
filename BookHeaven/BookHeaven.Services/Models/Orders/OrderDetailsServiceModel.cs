using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;
using BookHeaven.Services.Models.OrderItems;

namespace BookHeaven.Services.Models.Orders
{
    public class OrderDetailsServiceModel : IMapFrom<Order>,IHaveCustomMapping
    {
        public DateTime Date { get; set; }

        public IEnumerable<OrderItemDetailsServiceModel> OrderItems { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Order, OrderDetailsServiceModel>()
                .ForMember(o => o.OrderItems, cfg => cfg.MapFrom(o => o.OrderItems));
        }
    }
}
