using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface ISiteDateVisitService : IServce
    {
        Task AddVisitAsync();
    }
}
