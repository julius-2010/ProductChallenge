using System;
using System.Collections.Generic;
using System.Text;

namespace ProductChallenge.Application.Abstractions.Services
{
    public interface IDiscountService
    {
        Task<decimal> GetDiscountByProductIdAsync(int productId, CancellationToken cancellationToken);
    }
}
