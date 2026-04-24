using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductChallenge.Api.Middleware;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductChallenge.Tests.Unit.Api.Middleware
{
    public class RequestTimingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_ShouldCreateLogFileAndWriteRequestData()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"productchallenge-tests-{Guid.NewGuid()}");
            Directory.CreateDirectory(tempPath);

            try
            {
                RequestDelegate next = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Task.CompletedTask;
                };

                var environmentMock = new Mock<IWebHostEnvironment>();
                environmentMock.Setup(x => x.ContentRootPath).Returns(tempPath);

                var middleware = new RequestTimingMiddleware(next, environmentMock.Object);

                var context = new DefaultHttpContext();
                context.Request.Method = HttpMethods.Get;
                context.Request.Path = "/api/products/1";

                await middleware.InvokeAsync(context);

                var logFilePath = Path.Combine(tempPath, "Logs", "request-log.txt");

                File.Exists(logFilePath).Should().BeTrue();

                var content = await File.ReadAllTextAsync(logFilePath);

                content.Should().Contain("GET /api/products/1");
                content.Should().Contain("200");
                content.Should().Contain("ms");
            }
            finally
            {
                if (Directory.Exists(tempPath))
                    Directory.Delete(tempPath, true);
            }
        }
    }
}
