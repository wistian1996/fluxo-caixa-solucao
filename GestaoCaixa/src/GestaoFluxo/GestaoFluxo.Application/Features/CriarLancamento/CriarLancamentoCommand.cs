using GestaoFluxo.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Features.CriarLancamento
{
    public class CriarLancamentoCommand : IRequest<LancamentoDto>
    {
        public Guid? ComercianteId { get; set; }

        public bool? IsCredito { get; set; }
        public decimal? Valor { get; set; }
    }
}
