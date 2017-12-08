using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Users
{
    public class UserSearchDto : IMapFrom<User>, IHaveCustomMapping
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<User, UserSearchDto>()
                .ForMember(u => u.Name, cfg => cfg.MapFrom(u => $"{u.FirstName} {u.LastName}"));
        }
    }
}
