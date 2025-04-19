namespace GestaoFluxo.Application.Interfaces.OutboxService
{
    public record OutboxDestination(string Exchange, string RoutingKey, EOutboxDestinationType BrokerType)
    {
        protected OutboxDestination() : this(default, default, default)
        {

        }
    }
}
