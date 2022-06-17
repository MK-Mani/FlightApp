using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedServices.Models;

namespace LoginAPIService.Models
{
    public class Consumer : BackgroundService
    {
        private IConnection _connection;

        private IModel _channel;

        private IUserRepository _userRepository;

        private IServiceProvider _serviceProvider;
        public Consumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            CreateConnection();
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("Airline",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var scope = _serviceProvider.CreateScope();
            _userRepository = scope.ServiceProvider.GetService<IUserRepository>();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var content = System.Text.Encoding.UTF8.GetString(body);
                var userDetails = JsonConvert.DeserializeObject<User>(content);

                HandleMessage(userDetails);
            };
            _channel.BasicConsume("Airline", true, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(User userDetails)
        {
            _userRepository.UpdateLastName(userDetails);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
