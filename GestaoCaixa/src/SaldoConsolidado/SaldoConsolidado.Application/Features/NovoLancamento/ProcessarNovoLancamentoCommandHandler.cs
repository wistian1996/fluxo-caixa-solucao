using MediatR;
using Microsoft.Extensions.Logging;
using SaldoConsolidado.Domain.Entities;
using SaldoConsolidado.Domain.Exceptions;
using SaldoConsolidado.Domain.Interfaces;

namespace SaldoConsolidado.Application.Features.NovoLancamento
{
    public class ProcessarNovoLancamentoCommandHandler : IRequestHandler<ProcessarNovoLancamentoCommand, bool>
    {
        private readonly ISaldoConsolidadoDiarioRepository _consolidadoDiarioRepository;
        private readonly IEventoLancamentoRepository _eventoLancamentoRepository;
        private readonly ILogger<ProcessarNovoLancamentoCommandHandler> _logger;

        public ProcessarNovoLancamentoCommandHandler(ISaldoConsolidadoDiarioRepository consolidadoDiarioRepository, IEventoLancamentoRepository eventoLancamentoRepository, ILogger<ProcessarNovoLancamentoCommandHandler> logger)
        {
            _consolidadoDiarioRepository = consolidadoDiarioRepository;
            _eventoLancamentoRepository = eventoLancamentoRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(ProcessarNovoLancamentoCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _eventoLancamentoRepository.UnitOfWork.BeginTransaction();

                var eventoLancamento = new EventoLancamento(
                    command.Id, command.ComercianteId, command.DataCriacao,
                    command.Valor, command.IsCredito);

                await _eventoLancamentoRepository.AddEvento(eventoLancamento);

                await _consolidadoDiarioRepository.UpsertSaldoDiario(
                    eventoLancamento.ComercianteId,
                    eventoLancamento.DataCriacao,
                    eventoLancamento.Valor,
                    eventoLancamento.IsCredito);

                _eventoLancamentoRepository.UnitOfWork.Commit();

                return true;
            }
            catch (EventoJaRegistradoException ex)
            {
                _logger.LogWarning(ex, "Lançamento {Id} já processado", command.Id);

                _eventoLancamentoRepository.UnitOfWork.Rollback();

                // add logs

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao processar lançamento {Id}", command.Id);

                _eventoLancamentoRepository.UnitOfWork.Rollback();

                // add logs

                throw;
            }
        }
    }
}
