using System;
using AutoMapper;
using BookHeaven.Common.Extensions;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Books
{
    public class BookSearchListingServiceModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string BookListingPicture { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public DateTime PublishedDate { get; set; }

    }
}
