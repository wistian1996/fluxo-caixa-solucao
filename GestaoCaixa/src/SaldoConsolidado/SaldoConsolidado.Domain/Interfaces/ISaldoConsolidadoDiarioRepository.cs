using SaldoConsolidado.Domain.Entities;
using SaldoConsolidado.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Domain.Interfaces
{
    public interface ISaldoConsolidadoDiarioRepository : IRepository
    {
        Task<SaldoConsolidadoDiario?> GetSaldoDiario(Guid comercianteId, DateTime data);

        Task UpsertSaldoDiario(Guid comercianteId, DateTime data, decimal valor, bool isCredito);
    }
}
