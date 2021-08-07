namespace Message.Consumer.Domain.MessageBroker
{
    public interface IQueueConsumer
    {
        void Consume(string queueName);
    }
}
