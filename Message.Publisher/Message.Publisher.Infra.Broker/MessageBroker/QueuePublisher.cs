using Message.Publisher.Domain.MessageBroker;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Publisher.Infra.Broker.MessageBroker
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
            Publish(exchangeName, string.Empty, message);
        }

        public void Publish<TMessage>(string exchangeName, string routingKey, TMessage message)
        {
            var serializedMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            Publish(new MessageData(exchangeName, routingKey, body));
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
                        routingKey: messageData.RoutingKey,
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
