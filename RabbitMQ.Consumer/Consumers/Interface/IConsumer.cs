using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Consumers
{
    public interface IConsumer
    {
        Task Consume();
    }
}
