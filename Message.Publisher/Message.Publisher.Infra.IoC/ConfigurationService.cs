using Message.Publisher.Domain.MessageBroker;
using Message.Publisher.Infra.Broker.MessageBroker;
using Message.Publisher.Worker.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Message.Publisher.Infra.IoC
{
    public static class ConfigurationService
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQConfig = configuration.GetSection(nameof(QueueConfig)).Get<QueueConfig>();
            services.AddSingleton(rabbitMQConfig);

            services.AddScoped<IQueueConnection, QueueConnection>();
            services.AddScoped<IQueueSetup, QueueSetup>();

            services.AddScoped<IQueueBroker, QueueBroker>();

            return services;
        }
    }
}
