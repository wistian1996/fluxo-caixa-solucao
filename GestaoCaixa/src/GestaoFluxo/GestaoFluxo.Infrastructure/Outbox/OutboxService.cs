using GestaoFluxo.Application.Interfaces.OutboxService;
using GestaoFluxo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFluxo.Infrastructure.Outbox
{
    public class OutboxService : IOutboxService
    {
        private readonly GestaoFluxoDbContext _dbContext;

        public OutboxService(GestaoFluxoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
        {
            await _dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }

        public async Task<List<OutboxMessage>> GetPendingMessages(CancellationToken cancellationToken = default)
        {
            var messages = await _dbContext.OutboxMessages
                .FromSqlRaw(@"
                    SELECT *
                    FROM outbox_messages
                    WHERE data_processamento IS NULL
                    ORDER BY data_criacao
                    LIMIT 50
                    -- FOR UPDATE SKIP LOCKED
                ")
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return messages;
        }

        public async Task UpdateMessage(OutboxMessage outboxMessage)
        {
            _dbContext.OutboxMessages.Update(outboxMessage);

            await _dbContext.SaveChangesAsync();
        }
    }
}
