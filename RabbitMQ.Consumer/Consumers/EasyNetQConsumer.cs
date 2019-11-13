using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.Topology;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Consumers
{
    public class EasyNetQConsumer : IConsumer
    {
        private readonly IAdvancedBus _bus;
        private readonly IDistributedCache _cache;

        public EasyNetQConsumer(IAdvancedBus bus, IDistributedCache cache)
        {
            _bus = bus;
            _cache = cache;
        }

        public async Task Consume()
        {
            var exchange = await _bus.ExchangeDeclareAsync("EasyNetQExchange", ExchangeType.Direct);
            var queue = await _bus.QueueDeclareAsync("EasyNetQQueue");
            var routeKey = "ImRoutingKey";
            await _bus.BindAsync(exchange, queue, routeKey);
            _bus.Consume(queue, async (body, property, info) =>
             {
                 Console.WriteLine($"Get data : {Encoding.UTF8.GetString(body)}");
                 await _cache.SetAsync($"KeyOf{Encoding.UTF8.GetString(body)}", body);
             }, o => o.WithPrefetchCount(1));
        }
    }
}
