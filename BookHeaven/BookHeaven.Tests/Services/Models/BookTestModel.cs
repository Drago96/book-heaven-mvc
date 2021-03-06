﻿using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Tests.Services.Models
{
    public class BookTestModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}