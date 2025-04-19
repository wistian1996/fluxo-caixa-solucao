using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GestaoFluxo.Application
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        // Este método converte o DateTime para o formato desejado.
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString() ?? string.Empty);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Escreve o DateTime no formato UTC 'yyyy-MM-ddTHH:mm:ss.fff'Z'
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fff'Z'"));
        }
    }

    public static class JsonConfigurationDefault
    {
        public static JsonSerializerOptions Serialize()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                Converters = { new JsonDateTimeConverter() },
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }
    }
}
