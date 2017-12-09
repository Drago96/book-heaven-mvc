using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using static BookHeaven.Data.Infrastructure.Constants.CategoryDataConstants;

namespace BookHeaven.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CategoryNameMinLength)]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();
    }
}
