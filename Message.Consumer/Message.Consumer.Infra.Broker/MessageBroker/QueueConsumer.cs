using Message.Consumer.Domain.Consts;
using Message.Consumer.Domain.DomainExample;
using Message.Consumer.Domain.Extensions;
using Message.Consumer.Domain.MessageBroker;
using Message.Consumer.DomainExample;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Consumer.Infra.Broker.MessageBroker
{
    public class QueueConsumer : IQueueConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IQueuePublisher _queuePublisher;
        private readonly IModel _channel;
        private const string DeathCountHeaderName = "x-death";
        private const int MaxRetries = 3;
        private const int MaxConsumption = 2;
        private readonly AsyncEventingBasicConsumer _consumer;

        public QueueConsumer(IQueueConnection queueConnection,
                             IServiceScopeFactory scopeFactory,
                             IQueuePublisher queuePublisher)
        {
            _scopeFactory = scopeFactory;
            _queuePublisher = queuePublisher;
            _channel = queueConnection.CreateModel();

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += async (_, @event) => await ConsumerReceived(@event);
        }

        public void Consume(string queueName)
        {
            _channel.BasicQos(prefetchSize: 0, prefetchCount: MaxConsumption, false);

            _channel.BasicConsume(queue: queueName,
                        autoAck: false,
                        consumer: _consumer);
        }

        private async Task ConsumerReceived(BasicDeliverEventArgs @event)
        {
            try
            {
                await HandleMessage(@event);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling message. DeliveryTag: {@event.DeliveryTag}. Exception: {ex.Message}");
            }
        }

        private async Task HandleMessage(BasicDeliverEventArgs @event)
        {
            MessageExample message = null;
            byte[] body = null;

            try
            {
                body = @event.Body.ToArray();
                var serializedMessage = Encoding.UTF8.GetString(body);
                message = JsonConvert.DeserializeObject<MessageExample>(serializedMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling message. DeliveryTag: {@event.DeliveryTag}. Exception: {ex.Message}");

                _channel.BasicReject(@event.DeliveryTag, false);
            }

            Console.WriteLine($"Message: {@event.DeliveryTag}");

            try
            {
                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider.GetRequiredService<IMessageTransactionHandler>();
                await handler.Handle(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}. DeliveryTag: {@event.DeliveryTag}. Exception: {ex.Message}");

                HandleRejectTtlRetry(@event, body);
            }
            finally
            {
                _channel.BasicAck(@event.DeliveryTag, false);
            }
        }

        private void HandleRejectTtlRetry(BasicDeliverEventArgs @event, byte[] body)
        {
            var retriesCount = GetXDeathCount(@event.BasicProperties.Headers);

            if (retriesCount >= MaxRetries)
            {
                return;
            }

            _queuePublisher.Publish(new MessageData(QueueConst.FinancialTransactionEventsDeadLetter.GetExchange(), body)
            {
                Headers = @event.BasicProperties.Headers,
            });
        }

        private static int GetXDeathCount(IDictionary<string, object> headers)
        {
            if (headers is null || !headers.ContainsKey(DeathCountHeaderName))
            {
                return 0;
            }

            var xDeath = (List<object>)headers[DeathCountHeaderName];

            if (xDeath is null || xDeath.Count == 0)
            {
                return 0;
            }

            var xDeathValues = (IDictionary<string, object>)xDeath.FirstOrDefault();

            if (xDeathValues is null || !xDeathValues.ContainsKey("count"))
            {
                return 0;
            }

            return int.Parse(xDeathValues["count"].ToString());
        }
    }
}
