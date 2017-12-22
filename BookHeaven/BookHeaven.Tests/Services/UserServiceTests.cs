using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Implementations;
using BookHeaven.Tests.Mocks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Services
{
    public class UserServiceTests
    {
        private const string TestUserOneId = "TestUser1Id";
        private const string TestUserTwoId = "TestUser2Id";

        private const string TestRoleOneId = "TestRole1Id";
        private const string TestRoleTwoId = "TestRole2Id";
        private const string TestRoleThreeId = "TestRole3Id";
        private const string TestRoleOneName = "TestRole1";
        private const string TestRoleTwoName = "TestRole2";
        private const string TestRoleThreeName = "TestRole3";

        public UserServiceTests()
        {
            AutoMapperInitializer.Initialize();
        }

        [Fact]
        public async Task ExistsAsyncShouldReturnTrueIfUserExists()
        {
            //Arrange
            var db = BookHeavenDbContextInMemory.New();
            this.FillDatabase(db);
            var service = new UserService(db, null, null);

            //Act
            var exists = await service.ExistsAsync(TestUserOneId);

            //Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsyncShouldReturnFalseIfUserDoesntExist()
        {
            //Arrange
            var db = BookHeavenDbContextInMemory.New();
            this.FillDatabase(db);
            var service = new UserService(db, null, null);

            //Act
            var exists = await service.ExistsAsync(Guid.NewGuid().ToString());

            //Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task GetRolesByIdAsyncShouldReturnAllRolesForGivenUser()
        {
            //Arrange
            const int expectedRolesCount = 2;

            var db = BookHeavenDbContextInMemory.New();
            this.FillDatabase(db);
            var service = new UserService(db, null, null);

            //Act
            var userRoles = await service.GetRolesByIdAsync(TestUserOneId);

            //Assert
            userRoles.Count().ShouldBeEquivalentTo(expectedRolesCount);
            userRoles.Should().Contain(TestRoleOneName, TestRoleThreeName);
        }

        private void FillDatabase(BookHeavenDbContext db)
        {
            db.Users.AddRange(new List<User>
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

            db.Roles.AddRange(new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = TestRoleOneId,
                    Name = TestRoleOneName
                },
                new IdentityRole
                {
                    Id = TestRoleTwoId,
                    Name = TestRoleTwoName
                },
                new IdentityRole
                {
                    Id = TestRoleThreeId,
                    Name = TestRoleThreeName
                }
            });

            db.UserRoles.AddRange(new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    UserId = TestUserOneId,
                    RoleId = TestRoleOneId
                },
                new IdentityUserRole<string>
                {
                    UserId = TestUserOneId,
                    RoleId = TestRoleThreeId
                }
            });

            db.SaveChanges();
        }
    }
}