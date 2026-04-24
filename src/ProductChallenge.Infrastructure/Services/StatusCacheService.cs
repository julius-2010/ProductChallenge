using Microsoft.Extensions.Caching.Memory;
using ProductChallenge.Application.Abstractions.Services;

namespace ProductChallenge.Infrastructure.Services
{
    public class StatusCacheService : IStatusCacheService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "product-status-dictionary";

        public StatusCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<string> GetStatusNameAsync(int status, CancellationToken cancellationToken)
        {
            var statuses = _cache.GetOrCreate(CacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                return new Dictionary<int, string>
            {
                { 1, "Active" },
                { 0, "Inactive" }
            };
            }) ?? throw new InvalidOperationException("No se pudo cargar el diccionario de estado.");

            if (!statuses.TryGetValue(status, out var statusName))
                throw new InvalidOperationException($"El estado '{status}' no es válido.");

            return Task.FromResult(statusName);
        }
    }
}
