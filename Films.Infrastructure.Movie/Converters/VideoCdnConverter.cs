using Films.Infrastructure.Movie.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Films.Infrastructure.Movie.Converters;

public class VideoCdnConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var array = (JArray)jo.Property("data")!.First!;
        return array.First!.ToObject<VideoCdn>(new JsonSerializer { Converters = { new VideoCdnTranslationsConverter() } })!;
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(VideoCdn);
}