using Microsoft.AspNetCore.Http;
using Moq;

namespace OgcApi.Net.Styles.Tests.Mocks;

public static class HttpContextAccessorMock
{
    public static IHttpContextAccessor Instance
    {
        get
        {
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock
                .Setup(request => request.Scheme)
                .Returns("http");
            httpRequestMock
                .Setup(request => request.Host)
                .Returns(new HostString("localhost"));
            httpRequestMock
                .Setup(request => request.PathBase)
                .Returns(string.Empty);
            httpRequestMock
                .Setup(request => request.Headers["X-Forwarded-Proto"])
                .Returns("http");


            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor
                .Setup(accessor => accessor.HttpContext!.Request)
                .Returns(httpRequestMock.Object);

            return httpContextAccessor.Object;
        }
    }
}
