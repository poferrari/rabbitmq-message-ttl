namespace Message.Consumer.Domain.MessageBroker
{
    public interface IQueuePublisher
    {
        void Publish<TMessage>(string exchangeName, TMessage message);
        void Publish(MessageData messageData);
    }
}
