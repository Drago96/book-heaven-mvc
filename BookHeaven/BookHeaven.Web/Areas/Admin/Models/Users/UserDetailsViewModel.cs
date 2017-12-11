using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Infrastructure.Constants.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookHeaven.Common.Extensions;

namespace BookHeaven.Web.Areas.Admin.Models.Users
{
    public class UserDetailsViewModel : IMapFrom<UserDetailsServiceModel>, IHaveCustomMapping
    {
        [Display(Name = UserDisplayConstants.FirstName)]
        public string FirstName { get; set; }

        [Display(Name = UserDisplayConstants.LastName)]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string ProfilePicture { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<UserDetailsServiceModel, UserDetailsViewModel>()
                .ForMember(u => u.ProfilePicture,
                    cfg => cfg.MapFrom(u => u.ProfilePicture.ConvertToBase64String()));
        }
    }
}