using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Domain.Entities
{
    public class SaldoConsolidadoDiario
    {
        public Guid ComercianteId { get; private set; }
        public DateTime DataReferencia { get; private set; }
        public decimal Saldo { get; private set; }
        public DateTime DataUltimaAtualizacao { get; private set; }

        protected SaldoConsolidadoDiario()
        {

        }

        public SaldoConsolidadoDiario(Guid comercianteId, DateTime dataReferencia, decimal saldo)
        {
            ComercianteId = comercianteId;
            Saldo = saldo;
            DataReferencia = dataReferencia;
            DataUltimaAtualizacao = DateTime.UtcNow;
        }
    }
}
