using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;
using System;

namespace BookHeaven.Services.Models.Books
{
    public class BookPublisherListingServiceModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime PublishedDate { get; set; }
    }
}