using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Films.Infrastructure.Movie.Converters;

public class VideoCdnTranslationsConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        try
        {
            var jo = JArray.Load(reader);
            return jo.Children().Select(x => x.Value<string>()).ToList();
        }
        catch (JsonReaderException)
        {
            var jo = JObject.Load(reader);
            return jo.Children().Select(x => ((JProperty)x).Value.Value<string>()).ToList();
        }
        
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(List<string>);
}