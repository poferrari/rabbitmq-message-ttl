using Message.Publisher.Domain.Consts;
using Message.Publisher.Domain.Extensions;
using Message.Publisher.Domain.MessageBroker;
using Message.Publisher.Infra.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Message.Publisher.Worker
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");

            IConfiguration configuration = builder.Build();

            var services = new ServiceCollection();
            services.ConfigureServices(configuration);

            try
            {
                var serviceProvider = services.BuildServiceProvider();

                var content = @$"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Conteúdo da Mensagem: {Guid.NewGuid()}";
                var message = new MessageExample(content);

                var service = serviceProvider.GetRequiredService<IQueueBroker>();
                service.Publish(QueueConst.PrefixTransaction.GetExchange(), message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            Console.WriteLine("Publish message.");
        }
    }
}
