using System;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Services.Implementations
{
    public class SiteVisitService : ISiteVisitService
    {
        private readonly BookHeavenDbContext db;

        public SiteVisitService(BookHeavenDbContext db)
        {
            this.db = db;
        }

        public async Task AddVisitAsync()
        {
            var currentDate = DateTime.Today;

            SiteVisit siteVisit = await this.GetCurrentDateVisits(currentDate);

            if (siteVisit == null)
            {
                await this.AddNewSiteDateVisitAsync(currentDate);
            }
            else
            {
                siteVisit.Visits++;
                await this.db.SaveChangesAsync();
            }

        }

        public async Task<int> VisitsByDateAsync(DateTime date)
            => await this.db.Visits
                .Where(v => v.Date.Date == date.Date)
                .Select(v => v.Visits)
                .FirstOrDefaultAsync();

        public async Task<int> MostSiteVisitsInADayAsync()
            => await this.db
                .Visits
                .OrderByDescending(v => v.Visits)
                .Select(v => v.Visits)
                .FirstOrDefaultAsync();

        private async Task AddNewSiteDateVisitAsync(DateTime currentDate)
        {
            SiteVisit visit = new SiteVisit
            {
                Date = currentDate.Date,
                Visits = 1
            };

            this.db.Visits.Add(visit);
            await this.db.SaveChangesAsync();
        }

        public async Task<SiteVisit> GetCurrentDateVisits(DateTime date)
            => await this.db.Visits.FirstOrDefaultAsync(v => v.Date.Date == date.Date);
    }
}
