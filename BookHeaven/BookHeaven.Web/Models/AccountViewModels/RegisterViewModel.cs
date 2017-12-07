using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static BookHeaven.Data.Infrastructure.Constants.UserData;

namespace BookHeaven.Web.Models.AccountViewModels
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = CommonErrors.InvalidParameterLength, MinimumLength = FirstNameMinLength)]
        [Display(Name = UserDisplay.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = CommonErrors.InvalidParameterLength, MinimumLength = LastNameMinLength)]
        [Display(Name = UserDisplay.LastName)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = UserErrors.InvalidEmail)]
        public string Email { get; set; }

        [Required]
        [StringLength(PasswordMaxLength, ErrorMessage = CommonErrors.InvalidParameterLength, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = UserErrors.PasswordsMustMatch)]
        [Display(Name = UserDisplay.ConfirmPassword)]
        public string ConfirmPassword { get; set; }

        public IFormFile ProfilePicture { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ProfilePicture != null &&
                (this.ProfilePicture.Length > ProfilePictureMaxLength ||
                !this.ProfilePicture.ContentType.StartsWith("image")))
            {
                yield return new ValidationResult(UserErrors.InvalidProfilePicture, new[] { nameof(this.ProfilePicture) });
            }
        }
    }
}