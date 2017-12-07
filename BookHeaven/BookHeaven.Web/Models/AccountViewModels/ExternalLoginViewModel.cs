using System.ComponentModel.DataAnnotations;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;

using static BookHeaven.Data.Infrastructure.Constants.UserDataConstants;

namespace BookHeaven.Web.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = FirstNameMinLength)]
        [Display(Name = UserDisplayConstants.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = LastNameMinLength)]
        [Display(Name = UserDisplayConstants.LastName)]
        public string LastName { get; set; }
    }
}