using System.Globalization;
using Newtonsoft.Json;

namespace Overoom.Infrastructure.MovieDownloader.Converters;

public class DateTimeConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var date = reader.Value?.ToString();
        return DateOnly.ParseExact(date!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(DateOnly);
}