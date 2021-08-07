using Message.Consumer.Worker.Configurations;
using RabbitMQ.Client;
using System;

namespace Message.Consumer.Infra.Broker.MessageBroker
{
    public class QueueConnection : IQueueConnection
    {
        private readonly IConnection _connection;

        public QueueConnection(QueueConfig queueConfig)
        {
            var factory = GetConnection(queueConfig);

            _connection = factory.CreateConnection();
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available.");
            }

            return _connection.CreateModel();
        }

        private ConnectionFactory GetConnection(QueueConfig queueConfig)
            => new ConnectionFactory
            {
                HostName = queueConfig.HostName,
                Port = queueConfig.Port,
                UserName = queueConfig.UserName,
                Password = queueConfig.Password,
                VirtualHost = queueConfig.VirtualHost,
                AutomaticRecoveryEnabled = true,
                DispatchConsumersAsync = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
    }
}
