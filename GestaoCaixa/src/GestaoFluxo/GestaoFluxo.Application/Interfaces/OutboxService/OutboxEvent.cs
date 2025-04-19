using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Interfaces.OutboxService
{
    public class OutboxEvent<T>
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public T Body { get; private set; }

        public OutboxEvent(T body)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Body = body;
        }
    }
}
