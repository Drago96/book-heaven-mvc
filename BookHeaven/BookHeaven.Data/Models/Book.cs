using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static BookHeaven.Data.Infrastructure.Constants.BookDataConstants;

namespace BookHeaven.Data.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(BookTitleMaxLength)]
        [MinLength(BookTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(BookDescriptionMinLength)]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string BookPicture { get; set; }

        public string BookListingPicture { get; set; }

        [Required]
        public string PublisherId { get; set; }

        public DateTime PublishedDate { get; set; }

        public User Publisher { get; set; }

        public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>();

        public ICollection<OrderItem> Orders { get; set; } = new List<OrderItem>();

        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}