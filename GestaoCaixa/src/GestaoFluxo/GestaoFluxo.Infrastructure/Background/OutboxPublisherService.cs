using GestaoFluxo.Application;
using GestaoFluxo.Application.Dtos;
using GestaoFluxo.Application.Interfaces.EventPublisher;
using GestaoFluxo.Application.Interfaces.OutboxService;
using GestaoFluxo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GestaoFluxo.Infrastructure.Background
{
    public class OutboxPublisherService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxPublisherService> _logger;

        public OutboxPublisherService(IServiceProvider serviceProvider, ILogger<OutboxPublisherService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();

                    var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

                    var messages = await outboxService.GetPendingMessages(stoppingToken);

                    foreach (var message in messages)
                    {
                        try
                        {
                            var destino = JsonSerializer.Deserialize<OutboxDestination>(
                                message.Destino, JsonConfigurationDefault.Serialize());

                            var @event = PublishEventRequest.FromRabbitMQEvent(
                                message.Payload,
                                destino!.Exchange,
                                destino.RoutingKey);

                            await publisher.PublishAsync(@event);

                            message.SetPublicado();

                            await outboxService.UpdateMessage(message);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Falha ao processar mensagem Id={Id}, Payload={Payload}", message.Id, message.Payload);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Falha genérica ao processar mensagens pendentes");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
