using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models.Enums;
using BookHeaven.Services.Models.Books;

namespace BookHeaven.Web.Models.Books
{
    public class BookDetailsViewModel :BookDetailsServiceModel, IMapFrom<BookDetailsServiceModel>
    {
        public VoteValue? UserVote { get; set; }

        public override void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<BookDetailsServiceModel, BookDetailsViewModel>()
                .ForMember(b => b.UserVote, cfg => cfg.Ignore());
        }
    }
}
