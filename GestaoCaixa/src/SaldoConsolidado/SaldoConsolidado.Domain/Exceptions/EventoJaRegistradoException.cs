using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Domain.Exceptions
{
    public class EventoJaRegistradoException : Exception
    {
        public EventoJaRegistradoException(string message)
       : base(message)
        { }
    }
}
