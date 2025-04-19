using GestaoFluxo.Domain.Entities;
using GestaoFluxo.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Domain.Interfaces
{
    public interface ILancamentoRepository : IRepository
    {
        Task InserirLancamento(Lancamento lancamento, CancellationToken cancellationToken = default);
    }
}
