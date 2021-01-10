using Message.Consumer.DomainExample;
using System;
using System.Threading.Tasks;

namespace Message.Consumer.Domain.DomainExample
{
    public class MessageTransactionHandler : IMessageTransactionHandler
    {
        public async Task Handle(MessageExample message)
        {
            Console.WriteLine($"Get message: {message.Content}");

            Console.WriteLine($"Task Delay: {message.Id}");
            await Task.Delay(2000);

            RandomException();

            await Task.CompletedTask;
        }

        private void RandomException()
        {
            var rnd = new Random();
            var randomValue = rnd.Next(1, 100);

            if (randomValue % 5 == 0)
            {
                var ex = new Exception("Random exception --> Queue TTL!");
                throw ex;
            }
        }
    }
}
