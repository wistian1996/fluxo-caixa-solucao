using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Interfaces.OutboxService
{
    public enum EEOutboxEventType
    {
        FluxoCaixaNovoLancamento
    }

    public static class OutboxEventExtensions
    {
        public static string ToEventString(this EEOutboxEventType @event) => @event switch
        {
            EEOutboxEventType.FluxoCaixaNovoLancamento => "fluxo-caixa-novo-lancamento",
            _ => throw new NotImplementedException()
        };
    }
}
