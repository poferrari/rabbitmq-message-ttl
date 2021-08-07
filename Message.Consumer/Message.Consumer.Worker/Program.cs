using Message.Consumer.Domain.MessageBroker;
using Message.Consumer.Infra.IoC;
using Message.Consumer.Worker.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Message.Consumer.Worker
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();

            IConfiguration configuration = builder.Build();

            var services = new ServiceCollection();
            services.ConfigureServices(configuration);
            var serviceProvider = services.BuildServiceProvider();

            var queueConfig = serviceProvider.GetRequiredService<QueueConfig>();

            try
            {
                Console.WriteLine($"Consumer {queueConfig.QueueName}");
                var service = serviceProvider.GetRequiredService<IQueueConsumer>();
                service.Consume(queueConfig.QueueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
