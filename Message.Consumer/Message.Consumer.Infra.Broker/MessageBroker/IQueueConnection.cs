using RabbitMQ.Client;

namespace Message.Consumer.Infra.Broker.MessageBroker
{
    public interface IQueueConnection
    {
        bool IsConnected { get; }
        IModel CreateModel();
    }
}
