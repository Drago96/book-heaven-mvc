using System.ComponentModel.DataAnnotations;
using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;

namespace BookHeaven.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(UserDataConstants.PasswordMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = UserDataConstants.PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = UserDisplayConstants.RememberMe)]
        public bool RememberMe { get; set; }
    }
}