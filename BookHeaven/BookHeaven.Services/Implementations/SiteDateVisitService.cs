using System;
using System.Threading.Tasks;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Services.Implementations
{
    public class SiteDateVisitService : ISiteDateVisitService
    {
        private readonly BookHeavenDbContext db;

        public SiteDateVisitService(BookHeavenDbContext db)
        {
            this.db = db;
        }

        public async Task AddVisitAsync()
        {
            var currentDate = DateTime.Today;

            SiteDateVisit siteDateVisit = await this.GetCurrentDateVisits(currentDate);

            if (siteDateVisit == null)
            {
                await this.AddNewSiteDateVisitAsync(currentDate);
            }
            else
            {
                siteDateVisit.Visits++;
                await this.db.SaveChangesAsync();
            }

        }

        private async Task AddNewSiteDateVisitAsync(DateTime currentDate)
        {
            SiteDateVisit visit = new SiteDateVisit
            {
                Date = currentDate.Date,
                Visits = 1
            };

            this.db.Visits.Add(visit);
            await this.db.SaveChangesAsync();
        }

        public async Task<SiteDateVisit> GetCurrentDateVisits(DateTime date)
            => await this.db.Visits.FirstOrDefaultAsync(v => v.Date.Date == date.Date);
    }
}
