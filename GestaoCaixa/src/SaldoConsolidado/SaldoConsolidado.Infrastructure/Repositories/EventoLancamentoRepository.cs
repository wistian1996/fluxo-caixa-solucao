using Dapper;
using MySqlConnector;
using SaldoConsolidado.Domain.Entities;
using SaldoConsolidado.Domain.Exceptions;
using SaldoConsolidado.Domain.Interfaces;
using SaldoConsolidado.Domain.SeedWork;
using SaldoConsolidado.Infrastructure.Persistence;

namespace SaldoConsolidado.Infrastructure.Repositories
{
    public class EventoLancamentoRepository : IEventoLancamentoRepository
    {
        public IUnitOfWork UnitOfWork => _dapperContext;

        private readonly DapperContext _dapperContext;

        public EventoLancamentoRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task AddEvento(EventoLancamento eventoLancamento)
        {
            try
            {
                var sql = @"insert into evento_lancamento (id, comerciante_id, data_criacao, valor, is_credito) 
                values (@Id, @ComercianteId, @DataCriacao, @Valor, @IsCredito)";

                await _dapperContext.Connection.ExecuteAsync(sql, eventoLancamento,
                transaction: _dapperContext.Transaction);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                throw new EventoJaRegistradoException("Evento já registrado");
            }
        }
    }
}
