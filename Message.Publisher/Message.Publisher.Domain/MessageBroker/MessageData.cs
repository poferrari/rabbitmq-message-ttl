using System;
using System.Collections.Generic;

namespace Message.Publisher.Domain.MessageBroker
{
    public class MessageData
    {
        public MessageData(string exchangeName, string routingKey, ReadOnlyMemory<byte> body)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            Body = body;
        }

        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public ReadOnlyMemory<byte> Body { get; set; }
        public IDictionary<string, object> Headers { get; set; }
    }
}
