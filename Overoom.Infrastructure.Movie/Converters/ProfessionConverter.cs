using Newtonsoft.Json;
using Overoom.Infrastructure.Movie.Enums;

namespace Overoom.Infrastructure.Movie.Converters;

public class ProfessionConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        return reader.Value!.ToString() switch
        {
            "DIRECTOR" => Profession.Director,
            "ACTOR" => Profession.Actor,
            "VOICE_DIRECTOR" => Profession.VoiceDirector,
            "WRITER" => Profession.Writer,
            "COMPOSER" => Profession.Composer,
            "PRODUCER" => Profession.Producer,
            "OPERATOR" => Profession.Operator,
            "DESIGN" => Profession.Design,
            "EDITOR" => Profession.Editor,
            "TRANSLATOR" => Profession.Translator,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(Profession);
}