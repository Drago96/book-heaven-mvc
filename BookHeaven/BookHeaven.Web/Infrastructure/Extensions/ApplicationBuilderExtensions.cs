namespace BookHeaven.Web.Infrastructure.Extensions
{
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

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
    }
}
