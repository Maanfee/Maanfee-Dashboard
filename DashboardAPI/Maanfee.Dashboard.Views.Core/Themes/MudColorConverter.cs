using MudBlazor.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maanfee.Dashboard.Views.Core
{
    public class MudColorConverter : JsonConverter<MudColor>
    {
        public override MudColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // خواندن مقدار از JSON به صورت String
            string value = reader.GetString()!;
            if (string.IsNullOrEmpty(value))
                return new MudColor("#1E88E5"); // رنگ پیش‌فرض

            return new MudColor(value);
        }

        public override void Write(Utf8JsonWriter writer, MudColor value, JsonSerializerOptions options)
        {
            // ذخیره کردن به صورت String (مثل "#FF5733")
            writer.WriteStringValue(value.ToString());
        }
    }
}
