using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;
using System;

namespace BookHeaven.Services.Models.Books
{
    public class BookSearchDto : IMapFrom<Book>
    {
        public string Title { get; set; }

        public DateTime PublishedDate { get; set; }
    }
}