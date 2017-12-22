using BookHeaven.Common.Extensions;
using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookHeaven.Web.Models.Account
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required]
        [StringLength(UserDataConstants.FirstNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = UserDataConstants.FirstNameMinLength)]
        [Display(Name = UserDisplayConstants.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(UserDataConstants.LastNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = UserDataConstants.LastNameMinLength)]
        [Display(Name = UserDisplayConstants.LastName)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = UserErrorConstants.InvalidEmail)]
        public string Email { get; set; }

        [Required]
        [StringLength(UserDataConstants.PasswordMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = UserDataConstants.PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = UserErrorConstants.PasswordsMustMatch)]
        [Display(Name = UserDisplayConstants.ConfirmPassword)]
        public string ConfirmPassword { get; set; }

        public IFormFile ProfilePicture { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ProfilePicture != null &&
                (this.ProfilePicture.Length > UserDataConstants.ProfilePictureMaxLength ||
                !this.ProfilePicture.ContentType.IsValidImage()))
            {
                yield return new ValidationResult(UserErrorConstants.InvalidProfilePicture, new[] { nameof(this.ProfilePicture) });
            }
        }
    }
}