namespace Message.Publisher.Domain.MessageBroker
{
    public interface IQueuePublisher
    {
        void Publish<TMessage>(string exchangeName, TMessage message);
        void Publish<TMessage>(string exchangeName, string routingKey, TMessage message);
        void Publish(MessageData messageData);
    }
}
