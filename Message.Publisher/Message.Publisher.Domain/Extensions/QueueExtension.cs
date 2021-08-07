namespace Message.Publisher.Domain.Extensions
{
    public static class QueueExtension
    {
        public static string GetExchange(this string prefix) => $"{prefix}-exchange";
        public static string GetQueue(this string prefix) => $"{prefix}-queue";
        public static string GetRoutingKey(this string prefix) => $"{prefix}".Replace("-", ".");
    }
}
