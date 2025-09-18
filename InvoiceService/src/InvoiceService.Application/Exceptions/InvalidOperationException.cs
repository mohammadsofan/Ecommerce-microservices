using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceService.Application.Exceptions
{
    public class InvalidOperationException : AppException
    {
        public InvalidOperationException(string message) : base(message)
        {
        }
    }
}