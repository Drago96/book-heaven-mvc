using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookHeaven.Common.Extensions;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Remotion.Linq.Clauses;

using static BookHeaven.Data.Infrastructure.Constants.BookDataConstants;

namespace BookHeaven.Web.Areas.Publisher.Models.Books 
{
    public class BookCreateViewModel : IValidatableObject
    {
        [Required]
        [StringLength(BookTitleMaxLength, ErrorMessage = CommonErrorConstants.InvalidParameterLength, MinimumLength = BookTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(BookDescriptionMinLength,ErrorMessage = CommonErrorConstants.InvalidMinLength)]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public IFormFile BookPicture { get; set; }

        public IEnumerable<int> Categories { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> AllCategories { get; set; } = new List<SelectListItem>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.BookPicture != null &&
                (this.BookPicture.Length > BookPictureMaxLength ||
                 !this.BookPicture.ContentType.IsValidImage()))
            {
                yield return new ValidationResult(BookErrorConstants.InvalidProfilePicture, new[] { nameof(this.BookPicture) });
            }
        }
    }
}
