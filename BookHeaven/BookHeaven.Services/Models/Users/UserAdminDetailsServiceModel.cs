using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Users
{
    public class UserAdminDetailsServiceModel : UserDetailsServiceModel, IHaveCustomMapping
    {
        public int TotalPurchases { get; set; }

        public decimal MoneySpent { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<User, UserAdminDetailsServiceModel>()
                .ForMember(u => u.TotalPurchases,
                    cfg => cfg.MapFrom(u => u.Orders.Sum(o => o.OrderItems.Sum(oi => oi.Quantity))))
                .ForMember(u => u.MoneySpent,
                    cfg => cfg.MapFrom(u => u.Orders.Sum(o => o.OrderItems.Sum(oi => oi.Quantity * oi.BookPrice))));

        }
    }
}
