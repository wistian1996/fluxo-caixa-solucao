using MediatR;
using SaldoConsolidado.Application.Dtos;
using SaldoConsolidado.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Application.Features.Saldo
{
    public class GetSaldoPorComercianteIdQuery : IRequest<SaldoConsolidadoDto?>
    {
        public Guid ComercianteId { get; }
        public DateTime DataReferencia { get; }

        public GetSaldoPorComercianteIdQuery(Guid comercianteId)
        {
            ComercianteId = comercianteId;
            DataReferencia = DateTime.UtcNow;
        }

        public GetSaldoPorComercianteIdQuery(Guid comercianteId, DateTime? dataReferencia)
        {
            ComercianteId = comercianteId;
            DataReferencia = dataReferencia ?? DateTime.UtcNow;
        }
    }

    public class GetSaldoPorComercianteIdQueryHandler : IRequestHandler<GetSaldoPorComercianteIdQuery, SaldoConsolidadoDto?>
    {
        private readonly ISaldoConsolidadoDiarioRepository _saldoRepository;

        public GetSaldoPorComercianteIdQueryHandler(ISaldoConsolidadoDiarioRepository saldoRepository)
        {
            _saldoRepository = saldoRepository;
        }

        public async Task<SaldoConsolidadoDto?> Handle(GetSaldoPorComercianteIdQuery request, CancellationToken cancellationToken)
        {
            var saldo = await _saldoRepository.GetSaldoDiario(request.ComercianteId, request.DataReferencia);

            if (saldo is null) return null;

            var saldoDto = new SaldoConsolidadoDto(saldo.ComercianteId,
                saldo.DataReferencia, saldo.Saldo,
                saldo.DataUltimaAtualizacao);

            return saldoDto;
        }
    }
}
