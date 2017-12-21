using BookHeaven.Data.Models;

namespace LearningSystem.Test.Mocks
{
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class UserManagerMock
    {
        public static Mock<UserManager<User>> Get()
            => new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
    }
}