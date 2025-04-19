using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Interfaces.OutboxService
{
    public interface IOutboxService
    {
        Task AddAsync(OutboxMessage @event, CancellationToken cancellationToken = default);

        Task<List<OutboxMessage>> GetPendingMessages(CancellationToken cancellationToken = default);

        Task UpdateMessage(OutboxMessage outboxMessage);
    }
}
