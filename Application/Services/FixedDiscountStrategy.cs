using Application.Interfaces.Services;
using Application.Interfaces.Services.Application.Interfaces.Services;
using System;

namespace Application.Services
{
    public class FixedDiscountStrategy : IDiscountStrategy
    {
        public string Type => "Fixed";

        public decimal Calculate(decimal subtotal, decimal amount)
        {
            if (subtotal <= 0 || amount <= 0) return 0m;
            var discount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
            // İndirim toplam tutarı subtotal'ı geçmemeli
            return Math.Min(subtotal, discount);
        }
    }
}
