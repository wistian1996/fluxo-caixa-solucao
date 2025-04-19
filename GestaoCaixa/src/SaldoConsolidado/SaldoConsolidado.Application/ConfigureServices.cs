using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaldoConsolidado.Application.Consumers;
using System.Reflection;

namespace SaldoConsolidado.Application
{
    public static class ConfigureServices
    {
        public static void AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            serviceCollection.AddMassTransit(x =>
            {
                x.AddConsumer<NovoLancamentoConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration.GetConnectionString("RabbitMq")!), h =>
                    {

                    });

                    cfg.ReceiveEndpoint("fluxo-caixa-novo-lancamento", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.UseRawJsonDeserializer(RawSerializerOptions.All, isDefault: true);
                        e.ConfigureConsumer<NovoLancamentoConsumer>(context);
                    });
                });
            });
        }
    }
}
