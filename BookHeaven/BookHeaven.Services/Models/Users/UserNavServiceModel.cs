using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BookHeaven.Common.Extensions;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Users
{
    public class UserNavServiceModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string ProfilePictureNav { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<User, UserNavServiceModel>()
                .ForMember(u => u.ProfilePictureNav, cfg => cfg.MapFrom(u => u.ProfilePictureNav.ConvertToBase64String()));
        }
    }
}
