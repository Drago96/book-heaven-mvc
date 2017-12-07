using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface ISiteVisitService : IServce
    {
        Task AddVisitAsync();

        Task<int> VisitsByDateAsync(DateTime date);

        Task<int> MostSiteVisitsInADayAsync();
    }
}
