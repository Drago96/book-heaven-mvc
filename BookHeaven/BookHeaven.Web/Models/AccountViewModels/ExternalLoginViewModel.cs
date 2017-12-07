using System.ComponentModel.DataAnnotations;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;

using static BookHeaven.Data.Infrastructure.Constants.UserData;

namespace BookHeaven.Web.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = CommonErrors.InvalidParameterLength, MinimumLength = FirstNameMinLength)]
        [Display(Name = UserDisplay.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = CommonErrors.InvalidParameterLength, MinimumLength = LastNameMinLength)]
        [Display(Name = UserDisplay.LastName)]
        public string LastName { get; set; }
    }
}