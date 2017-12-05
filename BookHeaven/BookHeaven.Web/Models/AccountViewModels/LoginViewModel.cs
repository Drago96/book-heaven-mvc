using BookHeaven.Data;

namespace BookHeaven.Web.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;
    using static WebConstants;

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(UserPasswordMaxLength, ErrorMessage = InvalidParameterLengthErrorMessage, MinimumLength = UserPasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = RememberMeDisplay)]
        public bool RememberMe { get; set; }
    }
}
