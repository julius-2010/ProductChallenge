using System;
using System.Collections.Generic;
using System.Text;

namespace ProductChallenge.Application.Abstractions.Services
{
    public interface IStatusCacheService
    {
        Task<string> GetStatusNameAsync(int status, CancellationToken cancellationToken);
    }
}
