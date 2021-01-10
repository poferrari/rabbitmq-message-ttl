using RabbitMQ.Client;

namespace Message.Consumer.Infra.Broker.MessageBroker
{
    public interface IQueueSetup
    {
        void Initialize(IModel channel);
    }
}
