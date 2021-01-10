using Message.Consumer.DomainExample;
using System.Threading.Tasks;

namespace Message.Consumer.Domain.DomainExample
{
    public interface IMessageTransactionHandler
    {
        public Task Handle(MessageExample message);
    }
}
