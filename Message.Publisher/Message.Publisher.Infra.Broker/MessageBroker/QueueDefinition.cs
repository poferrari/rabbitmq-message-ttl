using System.Collections.Generic;

namespace Message.Publisher.Infra.Broker.MessageBroker
{
    public class QueueDefinition
    {
        public QueueDefinition(string exchangeName, string queueName, Dictionary<string, object> arguments)
        {
            ExchangeName = exchangeName;
            QueueName = queueName;
            Arguments = arguments;
        }

        public string ExchangeName { get; private set; }
        public string QueueName { get; private set; }
        public Dictionary<string, object> Arguments { get; private set; }
    }
}
