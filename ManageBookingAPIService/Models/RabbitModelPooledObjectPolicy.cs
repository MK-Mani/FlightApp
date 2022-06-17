using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SharedServices.Models;

namespace ManageBookingAPIService.Models
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {

        private readonly RabbitMqCredentials _rabbitCredentials;

        private readonly IConnection _connection;

        public RabbitModelPooledObjectPolicy(IOptions<RabbitMqCredentials> rabbitCredentials)
        {
            _rabbitCredentials = rabbitCredentials.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitCredentials.HostName,
                UserName = _rabbitCredentials.UserName,
                Password = _rabbitCredentials.Password,
                Port = _rabbitCredentials.Port,
                VirtualHost = _rabbitCredentials.VHost,
            };

            return factory.CreateConnection();
        }
        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel channel)
        {
            if(channel.IsOpen)
            {
                return true;
            }
            else
            {
                channel?.Dispose();
                return false;
            }
        }
    }
}
