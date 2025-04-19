using SaldoConsolidado.Application.JsonConverts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SaldoConsolidado.Application.Dtos
{
    public record SaldoConsolidadoDto(
        Guid ComercianteId, 
        [property: JsonConverter(typeof(DateOnlyJsonConverter))] DateTime DataReferencia, 
        decimal Saldo, 
        DateTime DataUltimaAtualizacao);
}
