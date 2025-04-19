using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Application.Features.NovoLancamento
{
    public class ProcessarNovoLancamentoCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid ComercianteId { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal Valor { get; set; }
        public bool IsCredito { get; set; }
    }
}
