using Message.Consumer.Domain.DomainExample;
using Message.Consumer.Domain.MessageBroker;
using Message.Consumer.Infra.Broker.MessageBroker;
using Message.Consumer.Worker.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Message.Consumer.Infra.IoC
{
    public static class ConfigurationService
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQConfig = configuration.GetSection(nameof(QueueConfig)).Get<QueueConfig>();
            services.AddSingleton(rabbitMQConfig);

            services.AddScoped<IQueueConnection, QueueConnection>();
            services.AddScoped<IQueueSetup, QueueSetup>();

            services.AddScoped<IQueuePublisher, QueuePublisher>();
            services.AddScoped<IQueueConsumer, QueueConsumer>();

            services.AddScoped<IMessageTransactionHandler, MessageTransactionHandler>();

            return services;
        }
    }
}
