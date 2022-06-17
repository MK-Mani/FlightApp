using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageAirlinesAPIService.Models
{
    public class Producer
    {
        private IConnection _connection;
        public Producer()
        {
            CreateConnection();
        }

        private void CreateConnection()
        {
            
        }
        public static async void Publish(object airlineDetails)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("Airline",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(airlineDetails));

            channel.BasicPublish("", "Airline", null, body);
        }

    }
}
