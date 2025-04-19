using System.Text.Json;

namespace GestaoFluxo.Application.Interfaces.EventPublisher
{
    public class PublishEventRequest(string Event, string RoutingKey, string Exchange)
    {
        public string Event { get; private set; } = Event;
        public string RoutingKey { get; private set; } = RoutingKey;
        public string Exchange { get; private set; } = Exchange;

        public static PublishEventRequest FromRabbitMQEvent(string @event, string exchange, string routingKey)
        {
            return new PublishEventRequest(@event, routingKey, exchange);
        }
    }
}
