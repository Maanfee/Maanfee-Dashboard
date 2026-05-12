using MudBlazor;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maanfee.Dashboard.Core
{
    public class DateRangeJsonConverter : JsonConverter<DateRange>
    {
        public override DateRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DateTime? start = null;
            DateTime? end = null;

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string name = reader.GetString()!;
                    reader.Read();

                    if (name == "start" || name == "Start")
                        start = reader.TokenType == JsonTokenType.Null
                            ? null
                            : reader.GetDateTime();

                    if (name == "end" || name == "End")
                        end = reader.TokenType == JsonTokenType.Null
                            ? null
                            : reader.GetDateTime();
                }
            }

            return new DateRange(start, end);
        }

        public override void Write(Utf8JsonWriter writer, DateRange value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("start");
            if (value.Start.HasValue)
                writer.WriteStringValue(value.Start.Value);
            else
                writer.WriteNullValue();

            writer.WritePropertyName("end");
            if (value.End.HasValue)
                writer.WriteStringValue(value.End.Value);
            else
                writer.WriteNullValue();

            writer.WriteEndObject();
        }
    }
}
