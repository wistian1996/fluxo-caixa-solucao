using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Application.Events
{
    public class OutboxEvent<T>
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public T Body { get; set; }
    }
}
