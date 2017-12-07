using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BookHeaven.Common.Extensions;
using BookHeaven.Data;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly BookHeavenDbContext db;

        public UserService(BookHeavenDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<T>> AllAsync<T>(string search = "", int page = 1)
            => await this.db
                .Users
                .Where(u => $"{u.FirstName} {u.LastName}".ContainsInsensitive(search) ||
                            u.Email.ContainsInsensitive(search))
                .Skip((page - 1) * UserServiceConstants.UserListingPageSize)
                .Take(UserServiceConstants.UserListingPageSize)
                .ProjectTo<T>()
                .ToListAsync();


        public async Task<int> GetCountAsync()
            => await this.db.Users.CountAsync();

        public async Task<int> GetCountBySearchTermAsync(string search)
            => await this.db
            .Users
            .Where(u => $"{u.FirstName} {u.LastName}".ContainsInsensitive(search) ||
                            u.Email.ContainsInsensitive(search))
            .CountAsync();
    }
}
