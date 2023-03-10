using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class IntegrationBasedEvent
    {
        public IntegrationBasedEvent()
        {
            Id= Guid.NewGuid();
            CreationDate= DateTime.UtcNow;
        }

        public IntegrationBasedEvent(Guid id, DateTime createDate)
        {
            Id = id; 
            CreationDate = createDate;
        }
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}
