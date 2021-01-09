using RabbitMQ.Client;

namespace Message.Publisher.Infra.Broker.MessageBroker
{
    public interface IQueueSetup
    {
        void Initialize(IModel channel);
    }
}
