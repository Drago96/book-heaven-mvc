using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static BookHeaven.Data.Infrastructure.Constants.UserDataConstants;

namespace BookHeaven.Web.Models.AccountViewModels
{
    public class RegisterViewModel : IValidatableObject
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

        [Required]
        [StringLength(PasswordMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = PasswordMinLength)]
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
                (this.ProfilePicture.Length > ProfilePictureMaxLength ||
                !this.ProfilePicture.ContentType.StartsWith("image")))
            {
                yield return new ValidationResult(UserErrorConstants.InvalidProfilePicture, new[] { nameof(this.ProfilePicture) });
            }
        }
    }
}