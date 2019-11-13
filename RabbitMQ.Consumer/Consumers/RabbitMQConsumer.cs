using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Consumers
{
    public class RabbitMQConsumer : IConsumer
    {
        private readonly IModel _model;
        private readonly IDistributedCache _cache;

        public RabbitMQConsumer(IModel model, IDistributedCache cache)
        {
            _model = model;
            _cache = cache;
        }

        public async Task Consume()
        {
            _model.ExchangeDeclare("testExchange", ExchangeType.Fanout, true, false, null);
            _model.QueueDeclare("testQueue", false, false, false, null);
            _model.QueueBind("testQueue", "testExchange", "routeKey", null);
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (sender, args) =>
            {
                _cache.SetAsync("MqData", args.Body);
            };
            _model.BasicConsume(queue: "testQueue", autoAck: true, consumer: consumer);
        }
    }
}
