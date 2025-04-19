using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Dtos
{
    public class LancamentoDto
    {
        public long Id { get; set; }
        public Guid ComercianteId { get; set; }
        public bool IsCredito { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
