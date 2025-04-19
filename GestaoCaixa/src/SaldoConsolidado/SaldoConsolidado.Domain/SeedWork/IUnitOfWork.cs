using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
