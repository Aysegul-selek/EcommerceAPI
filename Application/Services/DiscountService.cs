using Application.Dtos.Order;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IEnumerable<IDiscountStrategy> _strategies;

        public DiscountService(IEnumerable<IDiscountStrategy> strategies)
        {
            _strategies = strategies;
        }

        public decimal CalculateDiscount(decimal subtotal, DiscountDto? discountDto)
        {
            if (discountDto == null) return 0m;
            var strat = _strategies.FirstOrDefault(s =>
                s.Type.Equals(discountDto.Type, StringComparison.OrdinalIgnoreCase));
            if (strat == null) return 0m;
            return strat.Calculate(subtotal, discountDto.Amount);
        }
    }
}
