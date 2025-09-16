using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    namespace Application.Interfaces.Services
    {
        public interface IDiscountStrategy
        {
            string Type { get; } // "Percentage" or "Fixed"
            decimal Calculate(decimal subtotal, decimal amount);
        }
    }

}
