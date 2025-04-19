using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Domain.Entities
{
    public class EventoLancamento
    {
        public Guid Id { get; private set; }
        public Guid ComercianteId { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public decimal Valor { get; private set; }
        public bool IsCredito { get; private set; }

        protected EventoLancamento()
        {

        }

        public EventoLancamento(Guid id, Guid comercianteId, DateTime dataCriacao, decimal valor, bool isCredito)
        {
            Id = id;
            ComercianteId = comercianteId;
            DataCriacao = dataCriacao;
            Valor = valor;
            IsCredito = isCredito;
        }
    }
}
