using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Implementations;
using BookHeaven.Tests.Mocks;
using BookHeaven.Tests.Services.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Services
{
    public class OrderServiceTests
    {
        private const string TestUserOneId = "RandomUserId1";
        private const string TestUserTwoId = "RandomUserId2";
        private const int OrdersPerUser = 100;

        private readonly BookHeavenDbContext databaseMock;
        private readonly OrderService sut;

        public OrderServiceTests()
        {
            AutoMapperInitializer.Initialize();

            this.databaseMock = BookHeavenDbContextInMemory.New();
            this.sut = new OrderService(this.databaseMock);
        }

        [Fact]
        public async Task ByUserIdAsyncShouldReturnOrdersOnlyForGivenUser()
        {
            //Arrange
            this.FillDatabase();

            //Act
            var orders = await this.sut.ByUserIdAsync<OrderTestModel>(TestUserOneId, int.MaxValue);

            //Assert
            orders.Count().Should().Be(OrdersPerUser);
            orders.ToList().ForEach(o => o.UserId.ShouldBeEquivalentTo(TestUserOneId));
        }

        [Fact]
        public async Task ByUserIdAsyncShouldReturnNoMoreThanPageSize()
        {
            const int ordersToTake = OrdersPerUser - OrdersPerUser / 2;

            //Arrange
            this.FillDatabase();

            //Act
            var orders = await this.sut.ByUserIdAsync<OrderTestModel>(TestUserOneId, ordersToTake);

            //Assert
            orders.Count().Should().Be(ordersToTake);
        }

        [Fact]
        public async Task SalesForMonthShouldReturnResultsForEveryMonth()
        {
            //Arrange
            this.FillDatabase();

            //Act
            var salesForYear = await this.sut.SalesForYear(int.MaxValue, TestUserOneId);

            //Assert
            salesForYear.Count().ShouldBeEquivalentTo(12);
        }

        private void FillDatabase()
        {
            this.databaseMock.Users.AddRange(new List<User>()
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
                this.databaseMock.Orders.Add(order);
            }
            for (int i = OrdersPerUser + 1; i <= OrdersPerUser * 2; i++)
            {
                var order = new Order
                {
                    Id = i,
                    Date = DateTime.MaxValue,
                    UserId = TestUserTwoId
                };
                this.databaseMock.Orders.Add(order);
            }

            this.databaseMock.SaveChanges();
        }
    }
}