using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Watch2gether.Infrastructure.PersistentStorage.Models.Users;

public class UserModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AvatarFileName { get; set; } = null!;

    public string FavoriteFilmsList { get; set; } = null!;

    [NotMapped]
    public List<Guid> FavoriteFilms
    {
        get => JsonSerializer.Deserialize<List<Guid>>(FavoriteFilmsList,
            new JsonSerializerOptions
                {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}) ?? new List<Guid>();
        set => FavoriteFilmsList = JsonSerializer.Serialize(value,
            new JsonSerializerOptions
                {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    public string WatchedFilmsList { get; set; } = null!;

    [NotMapped]
    public List<Guid> WatchedFilms
    {
        get => JsonSerializer.Deserialize<List<Guid>>(WatchedFilmsList,
            new JsonSerializerOptions
                {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}) ?? new List<Guid>();
        set => WatchedFilmsList = JsonSerializer.Serialize(value,
            new JsonSerializerOptions
                {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }
}