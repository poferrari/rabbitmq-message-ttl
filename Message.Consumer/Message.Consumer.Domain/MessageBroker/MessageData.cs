using System;
using System.Collections.Generic;

namespace Message.Consumer.Domain.MessageBroker
{
    public class MessageData
    {
        public MessageData(string exchangeName, ReadOnlyMemory<byte> body)
        {
            ExchangeName = exchangeName;
            Body = body;
        }

        public string ExchangeName { get; set; }
        public ReadOnlyMemory<byte> Body { get; set; }
        public IDictionary<string, object> Headers { get; set; }
    }
}
