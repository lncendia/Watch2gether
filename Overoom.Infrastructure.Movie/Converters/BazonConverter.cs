using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Overoom.Application.Abstractions.MovieApi.Exceptions;
using Overoom.Infrastructure.Movie.Models;

namespace Overoom.Infrastructure.Movie.Converters;

public class BazonConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        if (jo.Property("error")?.Value.Value<string>() == "no file") throw new ApiNotFoundException();
        var array = (JArray)jo.Property("results")!.First!;
        return array.ToObject<List<Bazon>>()!;
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(List<Bazon>);
}