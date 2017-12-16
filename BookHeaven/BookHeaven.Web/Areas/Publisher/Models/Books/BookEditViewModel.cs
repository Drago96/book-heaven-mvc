using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Services.Models.Books;
using System;
using BookHeaven.Common.Extensions;

namespace BookHeaven.Web.Areas.Publisher.Models.Books
{
    public class BookEditViewModel : BookCreateViewModel, IMapFrom<BookEditServiceModel>
    {
        public string BookPicture { get; set; }

    }
}