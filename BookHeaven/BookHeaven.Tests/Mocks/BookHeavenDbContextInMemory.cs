using System;
using System.Collections.Generic;
using System.Text;
using BookHeaven.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookHeaven.Tests.Mocks
{
    public class BookHeavenDbContextInMemory
    {
        public static BookHeavenDbContext New()
        {
            var dbOptions = new DbContextOptionsBuilder<BookHeavenDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            return new BookHeavenDbContext(dbOptions);
        }
    }
}
