using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReportingService.Common.Text
{
    [JsonConverter(typeof(Base64StringConverter))]
    public struct Base64String
    {
        public Base64String(byte[] bytes)
        {
            DecodedBytes = bytes;
            _value = Convert.ToBase64String(bytes);
        }

        public Base64String(string encodedString)
        {
            DecodedBytes = null;
            _value = null;
            Value = encodedString;
        }

        private string _value;

        /// <summary>
        /// Decoded bytes
        /// </summary>
        [JsonIgnore]
        public byte[] DecodedBytes { get; private set; }

        /// <summary>
        /// Base64 Encoded string
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                if (value == null)
                {
                    _value = null;
                    DecodedBytes = null;
                    return;
                }

                // support Base64URL encoded strings
                var s = value
                    .Replace(' ', '+')
                    .Replace('-', '+')
                    .Replace('_', '/');

                s = s.PadRight(4 * ((s.Length + 3) / 4), '=');

                DecodedBytes = Convert.FromBase64String(s);
                _value = value;
            }
        }

        /// <summary>
        /// Decoded string value
        /// </summary>
        [JsonIgnore]
        public string DecodedString
        {
            get
            {
                if (DecodedBytes == null) return null;
                return Encoding.UTF8.GetString(DecodedBytes).TrimEnd('\0');
            }
        }

        /// <summary>
        /// Base64 Encoded string
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }

    public class Base64StringConverter : JsonConverter<Base64String>
    {
        public override Base64String Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new Base64String(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Base64String value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}