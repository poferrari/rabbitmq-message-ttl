using System;

namespace Message.Consumer.DomainExample
{
    public class MessageExample
    {
        public MessageExample(string content)
        {
            Id = Guid.NewGuid();
            Content = content;
            CreatedDate = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}
