using Application.Interfaces.Services;
using Application.Interfaces.Services.Application.Interfaces.Services;
using System;

namespace Application.Services
{
    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        public string Type => "Percentage";

        public decimal Calculate(decimal subtotal, decimal amount)
        {
            if (subtotal <= 0 || amount <= 0) return 0m;
            var discount = subtotal * (amount / 100m);
            return Math.Round(discount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
