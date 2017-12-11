using AutoMapper;
using BookHeaven.Common.Extensions;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static BookHeaven.Data.Infrastructure.Constants.UserDataConstants;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace BookHeaven.Web.Areas.Admin.Models.Users
{
    public class UserEditViewModel : IMapFrom<User>, IHaveCustomMapping, IValidatableObject
    {
        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = FirstNameMinLength)]
        [Display(Name = UserDisplayConstants.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = LastNameMinLength)]
        [Display(Name = UserDisplayConstants.LastName)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = UserErrorConstants.InvalidEmail)]
        public string Email { get; set; }

        public string ProfilePicture { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public IFormFile NewProfilePicture { get; set; }

        public IEnumerable<string> AllRoles { get; set; } = new List<string>();

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<UserDetailsServiceModel, UserEditViewModel>()
                .ForMember(u => u.ProfilePicture,
                    cfg => cfg.MapFrom(u => u.ProfilePicture.ConvertToBase64String()));
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.NewProfilePicture != null &&
                (this.NewProfilePicture.Length > ProfilePictureMaxLength ||
                 !this.NewProfilePicture.ContentType.IsValidImage()))
            {
                yield return new ValidationResult(UserErrorConstants.InvalidProfilePicture, new[] { nameof(this.NewProfilePicture) });
            }
        }
    }
}