using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Consumer.Consumers;
using System;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consumer Start.");
            var services = new ServiceCollection();
            services.AddSingleton<IAdvancedBus>(o =>
            {
                return RabbitHutch.CreateBus("host=localhost").Advanced;
            });
            services.AddSingleton<IModel>(o =>
            {
                var factory = new ConnectionFactory
                {
                    UserName = ConnectionFactory.DefaultUser,
                    Password = ConnectionFactory.DefaultPass,
                    HostName = "localhost"
                };
                var connection = factory.CreateConnection();
                return connection.CreateModel();
            });
            services.AddDistributedRedisCache(o =>
            {
                o.Configuration = "localhost:6379";
            });
            //services.AddScoped<IConsumer, RabbitMQConsumer>();
            services.AddScoped<IConsumer, EasyNetQConsumer>();

            var cs = services.BuildServiceProvider();
            Console.WriteLine("Press enter to start.");
            Console.ReadLine();
            Console.WriteLine("start.");
            cs.GetService<IConsumer>().Consume();
            Console.ReadLine();
        }
    }
}
