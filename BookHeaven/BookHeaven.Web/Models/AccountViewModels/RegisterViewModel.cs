using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BookHeaven.Web.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;
    using static WebConstants;

    public class RegisterViewModel : IValidatableObject
    {
        [Required]
        [StringLength(UserFirstNameMaxLength, ErrorMessage = InvalidParameterLengthErrorMessage, MinimumLength = UserFirstNameMinLength)]
        [Display(Name = UserFirstNameDisplay)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(UserLastNameMaxLength, ErrorMessage = InvalidParameterLengthErrorMessage, MinimumLength = UserLastNameMinLength)]
        [Display(Name = UserLastNameDisplay)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = EmailErrorMessage)]
        public string Email { get; set; }

        [Required]
        [StringLength(UserPasswordMaxLength, ErrorMessage = InvalidParameterLengthErrorMessage, MinimumLength = UserPasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = PasswordsMustMatchErrorMessage)]
        [Display(Name = UserConfirmPasswordDisplay)]
        public string ConfirmPassword { get; set; }

        public IFormFile ProfilePicture { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ProfilePicture!=null &&
                (this.ProfilePicture.Length > UserProfilePictureMaxLength ||
                !this.ProfilePicture.ContentType.StartsWith("image")))
            {
                yield return new ValidationResult(ProfilePictureErrorMessage,new [] {nameof(this.ProfilePicture)});
            }
        }
    }
}
