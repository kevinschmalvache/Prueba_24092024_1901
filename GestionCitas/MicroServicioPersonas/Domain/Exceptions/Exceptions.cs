using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroServicioPersonas.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}