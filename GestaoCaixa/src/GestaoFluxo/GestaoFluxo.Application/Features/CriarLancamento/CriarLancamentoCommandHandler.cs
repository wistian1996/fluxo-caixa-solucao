using AutoMapper;
using GestaoFluxo.Application.Dtos;
using GestaoFluxo.Application.Events;
using GestaoFluxo.Application.Interfaces.EventPublisher;
using GestaoFluxo.Application.Interfaces.OutboxService;
using GestaoFluxo.Domain.Entities;
using GestaoFluxo.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Features.CriarLancamento
{
    public class CriarLancamentoCommandHandler : IRequestHandler<CriarLancamentoCommand, LancamentoDto>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IOutboxService _outboxService;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CriarLancamentoCommandHandler> _logger;

        public CriarLancamentoCommandHandler(ILancamentoRepository lancamentoRepository,
            IMapper mapper, IEventPublisher eventPublisher,
            ILogger<CriarLancamentoCommandHandler> logger,
            IOutboxService outboxService)
        {
            _lancamentoRepository = lancamentoRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _outboxService = outboxService;
        }

        public async Task<LancamentoDto> Handle(CriarLancamentoCommand request, CancellationToken cancellationToken)
        {
            var lancamento = new Lancamento(
                request.ComercianteId!.Value,
                request.IsCredito!.Value,
                request.Valor!.Value);

            try
            {
                await _lancamentoRepository.UnitOfWork.BeginTransactionAsync();

                await _lancamentoRepository.InserirLancamento(lancamento, cancellationToken);

                await InserirOutboxMessage(lancamento, cancellationToken);

                await _lancamentoRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                await _lancamentoRepository.UnitOfWork.CommitAsync(cancellationToken);

                var lancamentoDto = _mapper.Map<LancamentoDto>(lancamento);

                return lancamentoDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao processar criação do lançamento");

                throw;
            }
        }

        private async Task InserirOutboxMessage(Lancamento lancamento, CancellationToken cancellationToken)
        {
            var @event = new NovoLancamentoEvent(lancamento.ComercianteId,
                lancamento.IsCredito, lancamento.Valor, lancamento.DataCriacao);

            var novoLancamentoEvent = new OutboxEvent<NovoLancamentoEvent>(@event);

            var outboxMessage = OutboxMessage.FromDestinationRabbitMq(
                novoLancamentoEvent,
                EEOutboxEventType.FluxoCaixaNovoLancamento.ToEventString());

            await _outboxService.AddAsync(outboxMessage, cancellationToken);
        }
    }
}
