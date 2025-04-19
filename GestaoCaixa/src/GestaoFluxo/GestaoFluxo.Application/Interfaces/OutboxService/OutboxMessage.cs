using GestaoFluxo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Interfaces.OutboxService
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public string TipoEvento { get; private set; }
        public string Payload { get; private set; }
        public string Destino { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataProcessamento { get; private set; }

        protected OutboxMessage()
        {

        }

        public static OutboxMessage FromDestinationRabbitMq<T>(
            OutboxEvent<T> @event, string eventType, string exchange = "amq.direct")
        {
            var destino = new OutboxDestination(exchange, eventType, EOutboxDestinationType.RabbitMq);

            return new OutboxMessage
            {
                Id = @event.Id,
                TipoEvento = eventType,
                Destino = JsonSerializer.Serialize(destino, JsonConfigurationDefault.Serialize()),
                Payload = JsonSerializer.Serialize(@event, JsonConfigurationDefault.Serialize())
            };
        }

        public void SetPublicado()
        {
            DataProcessamento = DateTime.UtcNow;
        }
    }
}
