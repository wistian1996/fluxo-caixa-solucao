using Microsoft.Extensions.DependencyInjection;
using SaldoConsolidado.Domain.Interfaces;
using SaldoConsolidado.Infrastructure.Persistence;
using SaldoConsolidado.Infrastructure.Repositories;

namespace SaldoConsolidado.Infrastructure
{
    public static class ConfigureServices
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<DapperContext>();
            serviceCollection.AddScoped<ISaldoConsolidadoDiarioRepository, SaldoConsolidadoDiarioRepository>();
            serviceCollection.AddScoped<IEventoLancamentoRepository, EventoLancamentoRepository>();
        }
    }
}
