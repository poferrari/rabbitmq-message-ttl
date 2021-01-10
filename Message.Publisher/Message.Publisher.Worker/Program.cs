using Message.Publisher.Domain.Consts;
using Message.Publisher.Domain.Extensions;
using Message.Publisher.Domain.MessageBroker;
using Message.Publisher.DomainExample;
using Message.Publisher.Infra.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IQueuePublisher>();

            var manualResetEvent = new ManualResetEvent(false);
            manualResetEvent.Reset();

            try
            {
                Task.Run(() =>
                {
                    int count = 0;

                    while (true)
                    {
                        Console.WriteLine("Press any key to insert 100 messages:");
                        Console.ReadLine();

                        for (var index = 0; index < 100; index++)
                        {
                            var content = @$"Ordem: {count++} - {DateTime.Now:dd/MM/yyyy HH:mm:ss} - Conteúdo da Mensagem: {Guid.NewGuid()}";
                            var message = new MessageExample(content);

                            service.Publish(QueueConst.PrefixTransaction.GetExchange(), message);

                            Console.WriteLine($"Publish {count} message: {content}");
                        }
                    }
                });

                manualResetEvent.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
