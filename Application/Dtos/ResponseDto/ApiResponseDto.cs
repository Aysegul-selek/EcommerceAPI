using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ResponseDto
{
    public class ApiResponseDto<T> where T : class
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; }
        public string? ErrorCodes { get; set; }
    }
}
