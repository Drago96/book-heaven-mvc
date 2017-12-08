using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;

namespace BookHeaven.Web.Areas.Admin.Models.Users
{
    public class UserDetailsViewModel :  IMapFrom<UserDetailsServiceModel>, IHaveCustomMapping
    {

        [Display(Name = UserDisplayConstants.FirstName)]
        public string FirstName { get; set; }

        [Display(Name = UserDisplayConstants.LastName)]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string ProfilePicture { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> AllRoles { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<UserDetailsServiceModel, UserDetailsViewModel>()
                .ForMember(u => u.ProfilePicture,
                    cfg => cfg.MapFrom(u => Convert.ToBase64String(u.ProfilePicture)));
        }
    }
}
