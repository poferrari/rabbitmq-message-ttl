using Message.Consumer.Domain.MessageBroker;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Consumer.Infra.Broker.MessageBroker
{
    public class QueuePublisher : IQueuePublisher
    {
        private readonly IQueueConnection _queueConnection;
        private const byte Persistent = 2;

        public QueuePublisher(IQueueConnection queueConnection)
        {
            _queueConnection = queueConnection;
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
            channel.ConfirmSelect();

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = Persistent;
            if (properties.Headers is null)
            {
                properties.Headers = new Dictionary<string, object>
                {
                    ["content-type"] = "application/json"
                };
            }
            AttachHeaders(properties, messageData.Headers);

            channel.BasicPublish(
                        exchange: messageData.ExchangeName,
                        routingKey: string.Empty,
                        mandatory: true,
                        basicProperties: properties,
                        body: messageData.Body);
            channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));
        }

        private static void AttachHeaders(IBasicProperties properties, IDictionary<string, object> headersToAdd)
        {
            if (headersToAdd != null)
            {
                foreach (var header in headersToAdd)
                {
                    properties.Headers.Add(header);
                }
            }
        }
    }
}
