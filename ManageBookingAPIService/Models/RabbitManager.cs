using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageBookingAPIService.Models
{
    public class RabbitManager : IRabbitManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;


        public RabbitManager(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }
        public void Publish<T>(T message, string routeKey) where T : class
        {
            if (message == null)
            {
                return;
            }

            var channel = _objectPool.Get();

            try
            {
                channel.QueueDeclare(routeKey,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                );

                var sendBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("", routeKey, null, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }
    }
}
