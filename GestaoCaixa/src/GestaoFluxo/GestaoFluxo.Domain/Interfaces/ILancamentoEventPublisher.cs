using GestaoFluxo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Domain.Interfaces
{
    public interface ILancamentoEventPublisher
    {
        Task Publish(Lancamento lancamento);
    }
}
