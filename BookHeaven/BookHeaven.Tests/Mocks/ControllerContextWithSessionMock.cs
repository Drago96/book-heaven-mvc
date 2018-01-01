using Microsoft.AspNetCore.Http;
using Moq;

namespace BookHeaven.Tests.Mocks
{
    public class HttpContextWithSessionMock
    {
        public static Mock<HttpContext> New()
        {
            var sessionMock = new Mock<ISession>();

            var value = new byte[0];

            sessionMock.Setup(_ => _.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((k, v) => value = v);

            sessionMock.Setup(_ => _.TryGetValue(It.IsAny<string>(), out value))
                .Returns(true);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(h => h.Session).Returns(sessionMock.Object);
            return httpContextMock;
        }
    }
}