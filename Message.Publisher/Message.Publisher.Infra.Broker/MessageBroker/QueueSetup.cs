using Message.Publisher.Domain.Consts;
using Message.Publisher.Domain.Extensions;
using RabbitMQ.Client;
using System.Collections.Generic;

namespace Message.Publisher.Infra.Broker.MessageBroker
{
    public class QueueSetup : IQueueSetup
    {
        private const string QueueModel = "x-queue-mode";
        private const string LazyQueueModel = "lazy";
        private const int DefaultTimeToLiveInMilliseconds = 60000;

        public void Initialize(IModel channel)
        {
            string exchangeFinancialTransactionUnrouted = CreateUnroutedEvents(channel);

            var prefixFinancialTransactionEvents = PrefixQueueConst.FinancialTransactionEvents;
            var exchangeFinancialTransactionEvents = prefixFinancialTransactionEvents.GetExchange();
            channel.ExchangeDeclare(exchangeFinancialTransactionEvents, "topic", durable: true, autoDelete: false,
                arguments: new Dictionary<string, object>()
                {
                    [QueueModel] = LazyQueueModel,
                    ["alternate-exchange"] = exchangeFinancialTransactionUnrouted
                });

            var prefixDOC = PrefixQueueConst.DOC;
            CreateQueueFinancialTransaction(channel, exchangeFinancialTransactionEvents, prefixDOC);

            var prefixTED = PrefixQueueConst.TED;
            CreateQueueFinancialTransaction(channel, exchangeFinancialTransactionEvents, prefixTED);

            var prefixPIX = PrefixQueueConst.PIX;
            CreateQueueFinancialTransaction(channel, exchangeFinancialTransactionEvents, prefixPIX);

            var prefixFinancialTransactionEventsDeadLetter = PrefixQueueConst.FinancialTransactionEventsDeadLetter;
            var exchangeFinancialTransactionEventsDeadLetter = prefixFinancialTransactionEventsDeadLetter.GetExchange();
            channel.ExchangeDeclare(exchangeFinancialTransactionEventsDeadLetter, "topic", durable: true, autoDelete: false,
                arguments: GetLazyQueueModelArguments());
            var queueFinancialTransactionEventsDeadLetter = prefixFinancialTransactionEventsDeadLetter.GetQueue();
            channel.QueueDeclare(queueFinancialTransactionEventsDeadLetter, durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object>()
                {
                    [QueueModel] = LazyQueueModel,
                    ["x-message-ttl"] = DefaultTimeToLiveInMilliseconds,
                    ["x-dead-letter-exchange"] = exchangeFinancialTransactionEvents
                });
            channel.QueueBind(queueFinancialTransactionEventsDeadLetter, exchangeFinancialTransactionEventsDeadLetter, "financial.*.*");
        }

        private static string CreateUnroutedEvents(IModel channel)
        {
            var prefixFinancialTransactionUnrouted = PrefixQueueConst.FinancialTransactionUnrouted;
            var exchangeFinancialTransactionUnrouted = prefixFinancialTransactionUnrouted.GetExchange();
            channel.ExchangeDeclare(exchangeFinancialTransactionUnrouted, "fanout", durable: true, autoDelete: false,
                arguments: GetLazyQueueModelArguments());
            var queueTransactionUnrouted = prefixFinancialTransactionUnrouted.GetQueue();
            channel.QueueDeclare(queueTransactionUnrouted, durable: true, exclusive: false, autoDelete: false,
                arguments: GetLazyQueueModelArguments());
            channel.QueueBind(queueTransactionUnrouted, exchangeFinancialTransactionUnrouted, string.Empty);
            return exchangeFinancialTransactionUnrouted;
        }

        private static Dictionary<string, object> GetLazyQueueModelArguments()
            => new Dictionary<string, object>()
            {
                [QueueModel] = LazyQueueModel
            };

        private static void CreateQueueFinancialTransaction(IModel channel, string exchangeName, string prefixQueue)
        {
            var queueDOC = prefixQueue.GetQueue();
            channel.QueueDeclare(queueDOC, durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object>()
                {
                    [QueueModel] = LazyQueueModel
                });
            channel.QueueBind(queueDOC, exchangeName, prefixQueue.GetRoutingKey());
        }
    }
}
