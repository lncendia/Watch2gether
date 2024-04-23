using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Films.Infrastructure.Load.Kodik.Converters;

internal class KodikConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var array = (JArray)jo.Property("results")!.First!;
        return array.ToObject<Models.FilmData[]>()!;
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(Models.FilmData[]);
}