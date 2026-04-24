using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductChallenge.Application.Abstractions.Services;
using ProductChallenge.Infrastructure.Persistence.Contexts;


namespace ProductChallenge.Tests.Integration.Common
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _databaseName = $"ProductChallengeTests-{Guid.NewGuid()}";
        private readonly InMemoryDatabaseRoot _databaseRoot = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();
                services.RemoveAll<AppDbContext>();

                services.RemoveAll<IStatusCacheService>();
                services.RemoveAll<IDiscountService>();

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName, _databaseRoot));

                services.AddScoped<IStatusCacheService, FakeStatusCacheService>();
                services.AddScoped<IDiscountService, FakeDiscountService>();

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            });
        }
    }

    internal sealed class FakeStatusCacheService : IStatusCacheService
    {
        public Task<string> GetStatusNameAsync(int status, CancellationToken cancellationToken)
        {
            var result = status switch
            {
                1 => "Active",
                0 => "Inactive",
                _ => throw new InvalidOperationException("Invalid status.")
            };

            return Task.FromResult(result);
        }
    }

    internal sealed class FakeDiscountService : IDiscountService
    {
        public Task<decimal> GetDiscountByProductIdAsync(int productId, CancellationToken cancellationToken)
        {
            return Task.FromResult(10m);
        }
    }
}
