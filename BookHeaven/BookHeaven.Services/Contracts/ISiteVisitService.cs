using System;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface ISiteVisitService : IService
    {
        Task AddVisitAsync();

        Task<int> VisitsByDateAsync(DateTime date);

        Task<int> MostSiteVisitsInADayAsync();
    }
}