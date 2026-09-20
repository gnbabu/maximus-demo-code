using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReportingService.Services.Data
{
    public class ConnectionString
    {
        public string Value { get; private set; }

        public ConnectionString(string connectionString)
        {
            Value = connectionString;
        }

        public override string ToString()
        {
            return Value; 
        }
    }

    public class ConnectionStringConverter : JsonConverter<ConnectionString>
    {
        public override ConnectionString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new ConnectionString(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, ConnectionString value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}
