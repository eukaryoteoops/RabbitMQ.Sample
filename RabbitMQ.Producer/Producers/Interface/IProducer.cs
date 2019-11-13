using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Producer.Producers
{
    public interface IProducer
    {
        Task PublishMq(string data);
    }
}
