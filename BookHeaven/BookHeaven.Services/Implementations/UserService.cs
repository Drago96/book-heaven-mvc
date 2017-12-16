using AutoMapper.QueryableExtensions;
using BookHeaven.Common.Extensions;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.UtilityServices.Contracts;

namespace BookHeaven.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly BookHeavenDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IFileService fileService;

        public UserService(BookHeavenDbContext db, UserManager<User> userManager,IFileService fileService)
        {
            this.db = db;
            this.userManager = userManager;
            this.fileService = fileService;
        }

        public async Task<IEnumerable<T>> AllPaginatedAsync<T>(string search = "", int page = 1, int take = 10)
            => await this.FindUsersBySearchTerm(search)
                .Skip((page - 1) *take)
                .Take(take)
                .ProjectTo<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> AllByTakeAsync<T>(string search = "", int usersToTake = 10)
            => await this.FindUsersBySearchTerm(search)
                .Take(usersToTake)
                .ProjectTo<T>()
                .ToListAsync();

        public async Task<int> CountAsync()
            => await this.db.Users.CountAsync();

        public async Task<int> CountBySearchTermAsync(string search)
            => await this.FindUsersBySearchTerm(search)
            .CountAsync();

        public async Task<bool> ExistsAsync(string id)
            => await this.db.Users.AnyAsync(u => u.Id == id);

        public async Task<T> ByIdAsync<T>(string id)
            => await this.db.Users.Where(u => u.Id == id).ProjectTo<T>().FirstOrDefaultAsync();

        public async Task<IEnumerable<string>> GetRolesByIdAsync(string id)
        {
            var userRolesIds = await this.db.UserRoles.Where(ur => ur.UserId == id).Select(r => r.RoleId).ToListAsync();
            var roles = await this.db.Roles.Where(r => userRolesIds.Contains(r.Id)).Select(r => r.Name).ToListAsync();
            return roles;
        }

        public async Task EditAsync(string id, string firstName, string lastName, string email, string username, IEnumerable<string> roles, string profilePicture,string profilePictureNav)
        {
            var user = await this.db.Users.FindAsync(id);

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.UserName = username;
            if (profilePicture != null)
            {
                this.fileService.DeleteImage(user.ProfilePicture);
                user.ProfilePicture = profilePicture;
            }
            if (profilePictureNav != null)
            {
                this.fileService.DeleteImage(user.ProfilePictureNav);
                user.ProfilePictureNav = profilePictureNav;
            }

            var allRoles = await this.db.Roles.Select(r => r.Name).ToListAsync();

            foreach (var role in allRoles)
            {
                if (roles.Contains(role))
                {
                    await this.userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    if (await this.userManager.IsInRoleAsync(user, role))
                    {
                        await this.userManager.RemoveFromRoleAsync(user, role);
                    }
                }
            }

            await this.userManager.UpdateAsync(user);
        }

        public async Task<bool> AlreadyExistsAsync(string id, string username)
            => await this.db.Users.AnyAsync(u => u.Id != id && u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

        private IQueryable<User> FindUsersBySearchTerm(string search)
            => this.db
                .Users
                .Where(u => $"{u.FirstName} {u.LastName}".ContainsInsensitive(search));
    }
}