using GestaoFluxo.Application.Interfaces.OutboxService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Events
{
    public class NovoLancamentoEvent
    {
        public Guid ComercianteId { get; set; }
        public bool IsCredito { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }

        public NovoLancamentoEvent(Guid comercianteId, 
            bool isCredito, decimal valor, DateTime dataCriacao)
        {
            ComercianteId = comercianteId;
            IsCredito = isCredito;
            Valor = valor;
            DataCriacao = dataCriacao;
        }
    }
}
