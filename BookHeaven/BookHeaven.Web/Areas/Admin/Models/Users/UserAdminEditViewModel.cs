using AutoMapper;
using BookHeaven.Common.Extensions;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;
using BookHeaven.Services.Models.Users;
using BookHeaven.Web.Infrastructure.Constants.Display;
using BookHeaven.Web.Infrastructure.Constants.ErrorMessages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookHeaven.Web.Models.Account;
using static BookHeaven.Data.Infrastructure.Constants.UserDataConstants;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace BookHeaven.Web.Areas.Admin.Models.Users
{
    public class UserAdminEditViewModel : UserEditViewModel
    {
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public IEnumerable<string> AllRoles { get; set; } = new List<string>();

    }
}