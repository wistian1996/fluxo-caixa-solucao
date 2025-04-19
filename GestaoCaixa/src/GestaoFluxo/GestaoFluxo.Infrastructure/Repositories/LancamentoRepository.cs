using GestaoFluxo.Domain.Entities;
using GestaoFluxo.Domain.Interfaces;
using GestaoFluxo.Domain.SeedWork;
using GestaoFluxo.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Infrastructure.Repositories
{
    public class LancamentoRepository : ILancamentoRepository
    {
        public IUnitOfWork UnitOfWork => _dbContext;

        private readonly GestaoFluxoDbContext _dbContext;

        public LancamentoRepository(GestaoFluxoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InserirLancamento(Lancamento lancamento, CancellationToken cancellationToken = default)
        {
            await _dbContext.Lancamentos.AddAsync(lancamento, cancellationToken);
        }
    }
}
