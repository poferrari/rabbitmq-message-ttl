using Message.Consumer.Infra.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Message.Consumer.Worker
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


        }
    }
}
