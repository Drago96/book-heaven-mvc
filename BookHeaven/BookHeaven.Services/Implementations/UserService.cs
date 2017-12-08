using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BookHeaven.Common.Extensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
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

        public async Task<IEnumerable<T>> AllPaginatedAsync<T>(string search = "", int page = 1)
            => await this.FindUsersBySearchTerm(search)
                .Skip((page - 1) * UserServiceConstants.UserListingPageSize)
                .Take(UserServiceConstants.UserListingPageSize)
                .ProjectTo<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> AllByTakeAsync<T>(string search = "", int usersToTake = 10)
            => await this.FindUsersBySearchTerm(search)
                .Take(usersToTake)
                .ProjectTo<T>()
                .ToListAsync();


        public async Task<int> GetCountAsync()
            => await this.db.Users.CountAsync();

        public async Task<int> GetCountBySearchTermAsync(string search)
            => await this.FindUsersBySearchTerm(search)
            .CountAsync();

        public async Task<bool> ExistsAsync(string id)
            => await this.db.Users.AnyAsync(u => u.Id == id);

        public async Task<T> GetByIdAsync<T>(string id)
            => await this.db.Users.Where(u => u.Id == id).ProjectTo<T>().FirstOrDefaultAsync();

        public async Task<IEnumerable<string>> GetRolesByIdAsync(string id)
        {
            var userRolesIds = await this.db.UserRoles.Where(ur => ur.UserId == id).Select(r => r.RoleId).ToListAsync();
            var roles = await this.db.Roles.Where(r => userRolesIds.Contains(r.Id)).Select(r => r.Name).ToListAsync();
            return roles;
        }


        private IQueryable<User> FindUsersBySearchTerm(string search)
            => this.db
                .Users
                .Where(u => $"{u.FirstName} {u.LastName}".ContainsInsensitive(search));
    }
}
