using GestaoFluxo.Application.Interfaces.EventPublisher;
using GestaoFluxo.Application.Interfaces.OutboxService;
using GestaoFluxo.Domain.Interfaces;
using GestaoFluxo.Infrastructure.Background;
using GestaoFluxo.Infrastructure.MessageBroker.RabbitMq;
using GestaoFluxo.Infrastructure.Outbox;
using GestaoFluxo.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoFluxo.Infrastructure
{
    public static class ConfigureServices
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("RabbitMq")!;
                return new RabbitMqConnection(connectionString);
            });

            serviceCollection.AddSingleton<IEventPublisher, EventPublisherRabbitMq>();

            serviceCollection.AddScoped<ILancamentoRepository, LancamentoRepository>();

            serviceCollection.AddScoped<IOutboxService, OutboxService>();

            serviceCollection.AddHostedService<OutboxPublisherService>();
        }
    }
}
