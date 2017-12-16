using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BookHeaven.Data.Infrastructure.Constants;

namespace BookHeaven.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int? BookId { get; set; }

        public Book Book { get; set; }

        [Required]
        [MinLength(BookDataConstants.BookTitleMinLength)]
        [MaxLength(BookDataConstants.BookTitleMaxLength)]
        public string BookTitle { get; set; }

        [Range(0,double.MaxValue)]
        public decimal BookPrice { get; set; }

        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
    }
}
