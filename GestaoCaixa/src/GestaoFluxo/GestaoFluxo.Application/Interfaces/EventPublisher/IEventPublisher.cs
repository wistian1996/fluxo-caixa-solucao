using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Interfaces.EventPublisher
{
    public interface IEventPublisher
    {
        public EEventPublisherBrokerType BrokerType { get; }

        Task PublishAsync(PublishEventRequest @event);
    }
}
