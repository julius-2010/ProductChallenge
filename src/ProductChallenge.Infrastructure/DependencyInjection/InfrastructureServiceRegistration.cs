using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductChallenge.Application.Abstractions.Persistence;
using ProductChallenge.Application.Abstractions.Services;
using ProductChallenge.Infrastructure.Persistence.Contexts;
using ProductChallenge.Infrastructure.Repositories;
using ProductChallenge.Infrastructure.Services;

namespace ProductChallenge.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddMemoryCache();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IStatusCacheService, StatusCacheService>();

            var discountApiBaseUrl = configuration["ExternalServices:DiscountApi:BaseUrl"]
                ?? throw new InvalidOperationException("Missing configuration: ExternalServices:DiscountApi:BaseUrl");

            if (!discountApiBaseUrl.EndsWith("/"))
                discountApiBaseUrl += "/";

            services.AddHttpClient<IDiscountService, DiscountService>(client =>
            {
                client.BaseAddress = new Uri(discountApiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            return services;
        }
    }
}
