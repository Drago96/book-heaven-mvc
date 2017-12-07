using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BookHeaven.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                BookHeavenDbContext db = scope.ServiceProvider.GetService<BookHeavenDbContext>();
                db.Database.Migrate();
            }

            return app;
        }

        public static IApplicationBuilder SeedRoles(this IApplicationBuilder app)
        {
            string[] roles = new string[]
            {
                Roles.Admin,
                Roles.SuperUser,
                Roles.Publisher
            };

            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                UserManager<User> userManager = scope.ServiceProvider.GetService<UserManager<User>>();

                Task.Run(async () =>
                {
                    foreach (string role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            IdentityRole identityRole = new IdentityRole(role);
                            await roleManager.CreateAsync(identityRole);

                            User user = await userManager.FindByNameAsync(role);
                            if (user == null)
                            {
                                string username = role.Split(" ").Join("");
                                user = new User()
                                {
                                    UserName = $"{username}@gmail.com",
                                    FirstName = username,
                                    LastName = username,
                                    Email = $"{username}@gmail.com",
                                };
                                string password = $"{username}123";
                                await userManager.CreateAsync(user, password);
                            }
                            await userManager.AddToRoleAsync(user, role);
                        }
                    }
                }).Wait();
            }

            return app;
        }
    }
}