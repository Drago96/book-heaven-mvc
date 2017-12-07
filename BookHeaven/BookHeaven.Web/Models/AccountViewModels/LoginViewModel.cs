using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using System.ComponentModel.DataAnnotations;

using static BookHeaven.Data.Infrastructure.Constants.UserDataConstants;

namespace BookHeaven.Web.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(PasswordMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = UserDisplayConstants.RememberMe)]
        public bool RememberMe { get; set; }
    }
}