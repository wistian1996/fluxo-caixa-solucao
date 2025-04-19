using GestaoFluxo.Application.Interfaces.EventPublisher;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace GestaoFluxo.Infrastructure.MessageBroker.RabbitMq
{
    public class EventPublisherRabbitMq : IEventPublisher
    {
        private readonly RabbitMqConnection _rabbitConnection;
        private IChannel _channel;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public EEventPublisherBrokerType BrokerType => EEventPublisherBrokerType.RabbitMq;

        public EventPublisherRabbitMq(RabbitMqConnection rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        public async Task PublishAsync(PublishEventRequest @event)
        {
            try
            {
                var channel = await GetOrCreateChannelAsync();

                await EnsureExchange(channel, @event.Exchange);

                await EnsureQueue(channel, @event.RoutingKey, @event.Exchange);

                var body = Encoding.UTF8.GetBytes(@event.Event);

                var props = new BasicProperties
                {
                    Persistent = true
                };

                await channel.BasicPublishAsync(
                    exchange: @event.Exchange,
                    routingKey: @event.RoutingKey,
                    mandatory: true,
                    basicProperties: props,
                    body: body);
            }
            catch (AlreadyClosedException ex)
            {
                if (ex.Message.Contains("shutdown")) throw;

                // await RecreateChannelAndRetryAsync(@event);
            }
            catch (OperationInterruptedException ex)
            {
                // await RecreateChannelAndRetryAsync(@event);
            }
            catch (Exception ex)
            {
                throw;
                // ignore
            }
        }

        private async Task EnsureExchange(IChannel channel, string exchangeName)
        {
            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true);
        }

        private async Task EnsureQueue(IChannel channel, string queueName, string exchangeName)
        {
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(queueName, exchangeName, routingKey: queueName, null);
        }

        private async Task RecreateChannelAndRetryAsync(PublishEventRequest @event)
        {
            await _semaphore.WaitAsync();

            try
            {
                _channel?.Dispose();
                var connection = await _rabbitConnection.GetConnectionAsync();
                _channel = await connection.CreateChannelAsync();
            }
            finally
            {
                _semaphore.Release();
            }

            await PublishAsync(@event);
        }

        private async Task<IChannel> GetOrCreateChannelAsync()
        {
            if (_channel != null) return _channel;

            await _semaphore.WaitAsync();

            try
            {
                if (_channel == null)
                {
                    var connection = await _rabbitConnection.GetConnectionAsync();

                    _channel = await connection.CreateChannelAsync();
                }
                return _channel;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
