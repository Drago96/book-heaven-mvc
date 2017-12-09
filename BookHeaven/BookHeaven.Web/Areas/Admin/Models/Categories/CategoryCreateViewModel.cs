using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Data.Infrastructure.Constants;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;

using static BookHeaven.Data.Infrastructure.Constants.CategoryDataConstants;

namespace BookHeaven.Web.Areas.Admin.Models.Categories
{
    public class CategoryCreateViewModel
    {
        [Required]
        [StringLength(CategoryNameMaxLength,ErrorMessage = CommonErrorConstants.InvalidParameterLength,MinimumLength = CategoryNameMinLength)]
        public string Name { get; set; }
    }
}
