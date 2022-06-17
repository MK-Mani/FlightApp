using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedServices.Models;

namespace ManageSchedulesAPIService.Models
{
    public class RabbitMqConsumer : BackgroundService
    {
        private IConnection _connection;

        private IModel _channel;

        private IScheduleRepository _scheduleRepository;

        private IServiceProvider _serviceProvider;
        public RabbitMqConsumer(IServiceProvider serviceProvider)
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

            _channel.QueueDeclare("Ticket",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var scope = _serviceProvider.CreateScope();
            _scheduleRepository = scope.ServiceProvider.GetService<IScheduleRepository>();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var content = System.Text.Encoding.UTF8.GetString(body);
                var producerMessage = JsonConvert.DeserializeObject<RabbitMqTicket>(content);

                HandleMessage(producerMessage);
            };
            _channel.BasicConsume("Ticket", true, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(RabbitMqTicket producerMessage)
        {
           if(producerMessage == null)
            {
                return;
            }

            _scheduleRepository.UpdateSeats(producerMessage);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}

