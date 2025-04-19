using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Infrastructure.MessageBroker.RabbitMq
{
    public class RabbitMqConnection
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly string _connectionString;

        //public RabbitMqConnection(string host, string user, string password, int port = 5672)
        //{
        //    _factory = new ConnectionFactory
        //    {
        //        HostName = host,
        //        UserName = user,
        //        Password = password,
        //        Port = port,
        //        AutomaticRecoveryEnabled = true,
        //        NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
        //        Uri = new Uri()
        //    };
        //}

        public RabbitMqConnection(string connectionString)
        {
            // _connectionString = connectionString;

            _factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            if (_connection is { IsOpen: true }) return _connection;

            await _semaphore.WaitAsync();

            try
            {
                if (_connection is { IsOpen: true }) return _connection;

                var retryPolicy = Policy
                    .Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetryAsync(
                        retryCount: 1,
                        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        onRetry: (exception, timeSpan) =>
                        {
                            // Aqui você pode logar o erro se quiser
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    _connection = await _factory.CreateConnectionAsync();
                });

                return _connection;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
