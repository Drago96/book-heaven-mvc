using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Implementations;
using BookHeaven.Tests.Mocks;
using BookHeaven.Tests.Services.Models;
using FluentAssertions;
using Xunit;

namespace BookHeaven.Tests.Services
{
    public class OrderServiceTests
    {
        private const string TestUserOneId = "RandomUserId1";
        private const string TestUserTwoId = "RandomUserId2";
        private const int OrdersPerUser = 100;

        public OrderServiceTests()
        {
            AutoMapperInitializer.Initialize();
        }

        [Fact]
        public async Task ByUserIdAsyncShouldReturnOrdersOnlyForGivenUser()
        {
            //Arrange
            var db = BookHeavenDbContextInMemory.New();
            this.FillDbData(db);

            var service = new OrderService(db);

            //Act
            var orders = await service.ByUserIdAsync<OrderTestModel>(TestUserOneId, int.MaxValue);

            //Assert
            orders.Count().Should().Be(OrdersPerUser);
            orders.ToList().ForEach(o => o.UserId.ShouldBeEquivalentTo(TestUserOneId));
        }

        [Fact]
        public async Task ByUserIdAsyncShouldReturnNoMoreThanPageSize()
        {
            const int ordersToTake = OrdersPerUser - OrdersPerUser / 2;

            //Arrange
            var db = BookHeavenDbContextInMemory.New();
            this.FillDbData(db);

            var service = new OrderService(db);

            //Act
            var orders = await service.ByUserIdAsync<OrderTestModel>(TestUserOneId, ordersToTake);

            //Assert
            orders.Count().Should().Be(ordersToTake);
        }

        [Fact]
        public async Task SalesForMonthShouldReturnResultsForEveryMonth()
        {
            //Arrange
            var db = BookHeavenDbContextInMemory.New();
            this.FillDbData(db);

            var service = new OrderService(db);

            //Act
            var salesForYear = await service.SalesForYear(int.MaxValue, TestUserOneId);

            //Assert
            salesForYear.Count().ShouldBeEquivalentTo(12);

        }

        private void FillDbData(BookHeavenDbContext db)
        {
            db.Users.AddRange(new List<User>()
            {
                new User
                {
                    Id = TestUserOneId
                },
                new User
                {
                    Id = TestUserTwoId
                }
            });

            for (int i = 1; i <= OrdersPerUser; i++)
            {
                var order = new Order
                {
                    Id = i,
                    Date = DateTime.MaxValue,
                    UserId = TestUserOneId
                };
                db.Orders.Add(order);
            }
            for (int i = OrdersPerUser + 1; i <= OrdersPerUser * 2; i++)
            {
                var order = new Order
                {
                    Id = i,
                    Date = DateTime.MaxValue,
                    UserId = TestUserTwoId
                };
                db.Orders.Add(order);
            }

            db.SaveChanges();
        }
    }
}
