using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Domain.Entities
{
    public class Lancamento
    {
        public long Id { get; private set; }
        public Guid ComercianteId { get; private set; }
        public bool IsCredito { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCriacao { get; private set; }

        protected Lancamento() { }

        public Lancamento(Guid comercianteId, bool isCredito, decimal valor)
        {
            ComercianteId = comercianteId;
            IsCredito = isCredito;
            Valor = valor;
            DataCriacao = DateTime.UtcNow;
        }
    }
}
