using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using System.ComponentModel.DataAnnotations;
using static BookHeaven.Data.Infrastructure.Constants.CategoryDataConstants;

namespace BookHeaven.Web.Areas.Admin.Models.Categories
{
    public class CategoryBasicInfoViewModel
    {
        [Required]
        [StringLength(CategoryNameMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = CategoryNameMinLength)]
        public string Name { get; set; }
    }
}