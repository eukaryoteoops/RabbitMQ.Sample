using EasyNetQ;
using EasyNetQ.DI;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Producer.Producers;
using System;

namespace RabbitMQ.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Producer Start.");
            var services = new ServiceCollection();
            services.AddSingleton<IAdvancedBus>(o =>
            {
                return RabbitHutch.CreateBus("host=localhost,prefetchcount=1").Advanced;
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

            //services.AddScoped<IProducer, RabbitMQClient>();
            services.AddScoped<IProducer, EasyNetQClient>();
            var cs = services.BuildServiceProvider();

            string input;
            do
            {
                Console.Clear();
                Console.Write("please type something : ");
                input = Console.ReadLine();
                cs.GetService<IProducer>().PublishMq(input);
            } while (input.Trim().ToLower() != "exit");
        }
    }
}
