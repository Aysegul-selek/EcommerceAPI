using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ResponseDto
{
    public class ErrorCodes
    {
        public const string NotFound = "NOT_FOUND";
        public const string Unauthorized = "UNAUTHORIZED";
        public const string Exception = "EXCEPTION";
        public const string ValidationError = "VALIDATION_ERROR";
        public const string Conflict = "CONFLICT";
        public const string Timeout = "TIMEOUT";
    }
}
