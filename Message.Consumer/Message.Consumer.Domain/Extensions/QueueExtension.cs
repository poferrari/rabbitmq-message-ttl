namespace Message.Consumer.Domain.Extensions
{
    public static class QueueExtension
    {
        public static string GetExchange(this string prefix) => $"{prefix}-exchange";

        public static string GetQueue(this string prefix) => $"{prefix}-queue";

        public static string GetExchangeTtl(this string prefix) => $"{prefix}-exchange-ttl";

        public static string GetQueueTtl(this string prefix) => $"{prefix}-queue-ttl";
    }
}
