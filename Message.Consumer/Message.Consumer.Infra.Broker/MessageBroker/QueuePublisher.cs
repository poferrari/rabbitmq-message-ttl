using Message.Consumer.Domain.MessageBroker;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace Message.Consumer.Infra.Broker.MessageBroker
{
    public class QueuePublisher : IQueuePublisher
    {
        private readonly IQueueConnection _queueConnection;
        private const byte Persistent = 2;

        public QueuePublisher(IQueueConnection queueConnection, IQueueSetup queueSetup)
        {
            _queueConnection = queueConnection;

            using var channel = _queueConnection.CreateModel();
            queueSetup.Initialize(channel);
        }

        public void Publish<TMessage>(string exchangeName, TMessage message)
        {
            var serializedMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            Publish(new MessageData(exchangeName, body));
        }

        public void Publish(MessageData messageData)
        {
            using var channel = _queueConnection.CreateModel();

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = Persistent;
            AttachHeaders(properties, messageData.Headers);

            channel.BasicPublish(
                        exchange: messageData.ExchangeName,
                        routingKey: string.Empty,
                        mandatory: true,
                        basicProperties: properties,
                        body: messageData.Body);
        }

        private static void AttachHeaders(IBasicProperties properties, IDictionary<string, object> headersToAdd)
        {
            if (headersToAdd != null)
            {
                if (properties.Headers is null)
                {
                    properties.Headers = new Dictionary<string, object>();
                }

                foreach (var header in headersToAdd)
                {
                    properties.Headers.Add(header);
                }
            }
        }
    }
}
