using RabbitMQ.Client;

namespace Message.Publisher.Infra.Broker.MessageBroker
{
    public interface IQueueConnection
    {
        bool IsConnected { get; }
        IModel CreateModel();
    }
}
