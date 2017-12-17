using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;

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
        [Compare(nameof(NewPassword),ErrorMessage = UserErrorConstants.PasswordsMustMatch)]
        public string ConfirmPassword { get; set; }
    }
}
