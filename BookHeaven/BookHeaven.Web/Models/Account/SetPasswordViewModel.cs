using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using System.ComponentModel.DataAnnotations;

namespace BookHeaven.Web.Models.Account
{
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(UserDataConstants.PasswordMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = UserDataConstants.PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = UserDisplayConstants.ConfirmPassword)]
        [Compare(nameof(Password), ErrorMessage = UserErrorConstants.PasswordsMustMatch)]
        public string ConfirmPassword { get; set; }
    }
}