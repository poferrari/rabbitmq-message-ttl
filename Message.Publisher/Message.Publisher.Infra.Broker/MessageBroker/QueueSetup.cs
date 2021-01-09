using Message.Publisher.Domain.Consts;
using Message.Publisher.Domain.Extensions;
using RabbitMQ.Client;
using System.Collections.Generic;

namespace Message.Publisher.Infra.Broker.MessageBroker
{
    public class QueueSetup : IQueueSetup
    {
        private const string ExchangeType = "direct";

        public void Initialize(IModel channel)
        {
            var queuesDefinitions = new List<QueueDefinition>
            {
                new QueueDefinition(QueueConst.PrefixTransaction.GetExchange(), QueueConst.PrefixTransaction.GetQueue(),
                    new Dictionary<string, object>()
                    {
                        ["x-queue-mode"] = "lazy"
                    }),
                new QueueDefinition(QueueConst.PrefixTransaction.GetExchangeTtl(), QueueConst.PrefixTransaction.GetQueueTtl(),
                    new Dictionary<string, object>()
                    {
                        ["x-message-ttl"] = 100000,
                        ["x-dead-letter-exchange"] = QueueConst.PrefixTransaction.GetExchange()
                    })
            };

            queuesDefinitions.ForEach(definition => CreateExchangeAndQueue(channel, definition));
        }

        private void CreateExchangeAndQueue(IModel channel, QueueDefinition definition)
        {
            channel.ExchangeDeclare(definition.ExchangeName, ExchangeType, durable: true, autoDelete: false);
            channel.QueueDeclare(definition.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: definition.Arguments);
            channel.QueueBind(definition.QueueName, definition.ExchangeName, string.Empty);
        }
    }
}
