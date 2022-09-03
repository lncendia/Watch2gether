using Newtonsoft.Json;
using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.Infrastructure.MovieDownloader.Converters;

public class FilmTypeConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        return reader.Value!.ToString() switch
        {
            "movie" => FilmType.Film,
            "serial" => FilmType.Serial,
        _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(FilmType);
}