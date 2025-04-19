using Dapper;
using MySqlConnector;
using SaldoConsolidado.Domain.Entities;
using SaldoConsolidado.Domain.Interfaces;
using SaldoConsolidado.Domain.SeedWork;
using SaldoConsolidado.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Infrastructure.Repositories
{
    public class SaldoConsolidadoDiarioRepository : ISaldoConsolidadoDiarioRepository
    {
        public IUnitOfWork UnitOfWork => _dapperContext;

        private readonly DapperContext _dapperContext;

        public SaldoConsolidadoDiarioRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<SaldoConsolidadoDiario?> GetSaldoDiario(Guid comercianteId, DateTime data)
        {
            data = new DateTime(data.Year, data.Month, data.Day);

            var sql = @"select
                comerciante_id as ComercianteId,
                data_referencia as DataReferencia,
                saldo as Saldo,
                data_ultima_atualizacao DataUltimaAtualizacao
            from saldo_consolidado_diario where comerciante_id = @comercianteId and data_referencia = @data
            limit 1";

            return await _dapperContext.Connection.QueryFirstOrDefaultAsync<SaldoConsolidadoDiario>(sql, new
            {
                comercianteId,
                data
            });
        }

        public async Task UpsertSaldoDiario(Guid comercianteId, DateTime data, decimal valor, bool isCredito)
        {
            data = new DateTime(data.Year, data.Month, data.Day);

            var valorFinal = isCredito ? valor : -valor;

            var sql = @"
                    INSERT INTO saldo_consolidado_diario (comerciante_id, data_referencia, saldo, data_ultima_atualizacao)
                    VALUES (@comercianteId, @data, @valor, UTC_TIMESTAMP())
                    ON DUPLICATE KEY UPDATE saldo = saldo + VALUES(saldo);
                ";

            await _dapperContext.Connection.ExecuteAsync(sql, new
            {
                comercianteId,
                data,
                valor = valorFinal
            }, transaction: _dapperContext.Transaction);
        }
    }
}
