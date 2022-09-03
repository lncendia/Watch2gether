using System.Globalization;
using Newtonsoft.Json;

namespace Watch2gether.Infrastructure.MovieDownloader.Converters;

public class DateTimeConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var date = reader.Value?.ToString();
        return DateTime.ParseExact(date!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(DateTime);
}