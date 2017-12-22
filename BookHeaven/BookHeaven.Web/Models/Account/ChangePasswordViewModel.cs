using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using System.ComponentModel.DataAnnotations;

namespace BookHeaven.Web.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = UserDisplayConstants.CurrentPassword)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(UserDataConstants.PasswordMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = UserDataConstants.PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = UserDisplayConstants.NewPassword)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = UserDisplayConstants.ConfirmPassword)]
        [Compare(nameof(NewPassword), ErrorMessage = UserErrorConstants.PasswordsMustMatch)]
        public string ConfirmPassword { get; set; }
    }
}