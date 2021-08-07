namespace Message.Publisher.Domain.Consts
{
    public static class PrefixQueueConst
    {
        public const string FinancialTransactionUnrouted = "financial-transactions-unrouted";
        public const string FinancialTransactionEvents = "financial-transactions-events";
        public const string DOC = "financial-transactions-doc";
        public const string TED = "financial-transactions-ted";
        public const string PIX = "financial-transactions-pix";
        public const string FinancialTransactionEventsDeadLetter = "financial-dead-letter";
    }
}
