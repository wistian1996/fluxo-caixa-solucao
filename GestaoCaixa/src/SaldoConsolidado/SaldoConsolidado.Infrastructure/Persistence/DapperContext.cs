using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SaldoConsolidado.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Infrastructure.Persistence
{
    public class DapperContext : IUnitOfWork
    {
        private Guid _id;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DapperContext(IConfiguration configuration)
        {
            _id = Guid.NewGuid();
            Connection = new MySqlConnection(configuration.GetConnectionString("Mysql"));
            Connection.Open();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }

        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();

            Dispose();
        }

        public void Rollback()
        {
            Transaction.Rollback();

            Dispose();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
