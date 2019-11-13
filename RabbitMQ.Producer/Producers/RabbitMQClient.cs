using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Producer.Producers
{
    public class RabbitMQClient : IProducer
    {
        private readonly IModel _model;

        public RabbitMQClient(IModel model)
        {
            _model = model;
        }

        public async Task PublishMq(string data)
        {
            var body = Encoding.UTF8.GetBytes(data);
            _model.ExchangeDeclare("testExchange", ExchangeType.Fanout, true, false, null);
            _model.QueueDeclare("testQueue", false, false, false, null);
            _model.QueueBind("testQueue", "testExchange", "routeKey", null);
            _model.BasicPublish("testExchange", "routeKey", null, body);
        }
    }
}
