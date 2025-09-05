using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AuthDto
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public DateTime GecerlilikTarihi { get; set; }
    }
}
