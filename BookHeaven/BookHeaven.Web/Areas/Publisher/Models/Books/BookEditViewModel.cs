using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Services.Models.Books;
using System;
using BookHeaven.Common.Extensions;

namespace BookHeaven.Web.Areas.Publisher.Models.Books
{
    public class BookEditViewModel : BookCreateViewModel, IMapFrom<BookEditServiceModel>, IHaveCustomMapping
    {
        public string BookPicture { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<BookEditServiceModel, BookEditViewModel>()
                .ForMember(b => b.BookPicture,
                    cfg => cfg.MapFrom(b => b.BookPicture.ConvertToBase64String()));
        }
    }
}