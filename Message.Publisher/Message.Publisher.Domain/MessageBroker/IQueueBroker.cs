namespace Message.Publisher.Domain.MessageBroker
{
    public interface IQueueBroker
    {
        void Publish<TMessage>(string exchangeName, TMessage message);
        void Publish(MessageData messageData);
    }
}
