using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Domain.Extensions
{
    public class GestaoFluxoDomainException : Exception
    {
        public GestaoFluxoDomainException()
        {

        }

        public GestaoFluxoDomainException(string message) : base(message)
        {

        }

        public GestaoFluxoDomainException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
