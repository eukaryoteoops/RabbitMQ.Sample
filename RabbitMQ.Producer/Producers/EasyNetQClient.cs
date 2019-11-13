using EasyNetQ;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Producer.Producers
{
    public class EasyNetQClient : IProducer
    {
        private readonly IAdvancedBus _bus;

        public EasyNetQClient(IAdvancedBus bus)
        {
            _bus = bus;
        }

        public async Task PublishMq(string data)
        {
            var exchange = await _bus.ExchangeDeclareAsync("EasyNetQExchange", ExchangeType.Direct);
            var queue = await _bus.QueueDeclareAsync(
                name: "EasyNetQQueue",
                passive: false,     // passive queue 代表若queue不存在也不主動創建返回失敗，若存在就繼續執行
                durable: true,      // persist this queue
                exclusive: false,   // 只允許首次聲明他的連結可見，並在斷開後自動刪除，用在單一客戶端發送/接收
                autoDelete: false,   // 若最後一個consumer也斷開與此queue的連線，則自動刪除此queue
                perQueueMessageTtl: null,  // message丟到queue的存活時間，單位ms
                expires: null,      // queue的存活時間，單位ms
                maxPriority: null      // 此queue的優先級，message也有優先級
                );
            var routeKey = "ImRoutingKey";
            await _bus.BindAsync(exchange, queue, routeKey);
            var body = Encoding.UTF8.GetBytes(data);
            await _bus.PublishAsync(exchange, routeKey, false, new MessageProperties()
            {
                DeliveryMode = 2,   // persist此訊息
                Priority = 10       // message的優先級
            }, body);

        }
    }
}
