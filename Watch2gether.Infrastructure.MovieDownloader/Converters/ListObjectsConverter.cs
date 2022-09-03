using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Watch2gether.Infrastructure.MovieDownloader.Converters;

public class ListObjectsConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var jo = JArray.Load(reader);
        return jo.Select(token => token.First?.First?.ToObject<string>(serializer)).ToList();
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(List<string>);
}