using MassTransit;
using MediatR;
using SaldoConsolidado.Application.Events;
using SaldoConsolidado.Application.Features;
using SaldoConsolidado.Application.Features.NovoLancamento;

namespace SaldoConsolidado.Application.Consumers
{
    public class NovoLancamentoConsumer : IConsumer<OutboxEvent<NovoLancamentoEvent>>
    {
        private readonly IMediator _mediator;

        public NovoLancamentoConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<OutboxEvent<NovoLancamentoEvent>> context)
        {
            var outboxMessage = context.Message;

            var novoLancamento = outboxMessage.Body;

            var command = new ProcessarNovoLancamentoCommand
            {
                Id = outboxMessage.Id,
                ComercianteId = novoLancamento.ComercianteId,
                DataCriacao = novoLancamento.DataCriacao,
                IsCredito = novoLancamento.IsCredito,
                Valor = novoLancamento.Valor,
            };

            await _mediator.Send(command);
        }
    }
}
