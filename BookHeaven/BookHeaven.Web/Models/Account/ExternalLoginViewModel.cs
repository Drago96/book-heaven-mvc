using System.ComponentModel.DataAnnotations;
using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;

namespace BookHeaven.Web.Models.Account
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(UserDataConstants.FirstNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = UserDataConstants.FirstNameMinLength)]
        [Display(Name = UserDisplayConstants.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(UserDataConstants.LastNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = UserDataConstants.LastNameMinLength)]
        [Display(Name = UserDisplayConstants.LastName)]
        public string LastName { get; set; }
    }
}