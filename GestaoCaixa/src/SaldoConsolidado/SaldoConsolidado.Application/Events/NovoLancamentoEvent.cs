using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Application.Events
{
    public class NovoLancamentoEvent
    {
        public Guid ComercianteId { get; set; }
        public bool IsCredito { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
