using GestaoFluxo.Application.Interfaces.OutboxService;
using GestaoFluxo.Domain.Entities;
using GestaoFluxo.Domain.SeedWork;
using GestaoFluxo.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Infrastructure.DbContexts
{
    public class GestaoFluxoDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }


        public GestaoFluxoDbContext(DbContextOptions<GestaoFluxoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LancamentoEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityConfiguration());
        }

        public async Task BeginTransactionAsync()
        {
            await Database.BeginTransactionAsync();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await Database.CommitTransactionAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await Database.RollbackTransactionAsync(cancellationToken);
        }
    }
}
