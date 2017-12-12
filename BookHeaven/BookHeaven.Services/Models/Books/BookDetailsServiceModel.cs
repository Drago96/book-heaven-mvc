﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using BookHeaven.Common.Extensions;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Books
{
    public class BookDetailsServiceModel : IMapFrom<Book>, IHaveCustomMapping
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string BookPicture { get; set; }

        public DateTime PublishedDate { get; set; }

        public IEnumerable<string> Categories { get; set; } = new List<string>();

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<Book, BookDetailsServiceModel>()
                .ForMember(b => b.Categories, cfg => cfg.MapFrom(b => b.Categories.Select(c => c.Category.Name)))
                .ForMember(u => u.BookPicture,
                    cfg => cfg.MapFrom(u => u.BookPicture.ConvertToBase64String()));
        }
    }
}