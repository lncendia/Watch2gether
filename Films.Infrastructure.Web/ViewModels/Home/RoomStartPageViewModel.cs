using Type = Films.Application.Abstractions.StartPage.DTOs.Type;

namespace Films.Infrastructure.Web.Models.Home;

public class RoomStartPageViewModel
{
    public RoomStartPageViewModel(Guid id, Type type, int countUsers, string nowPlaying)
    {
        Id = id;
        Type = type;
        CountUsers = countUsers;
        NowPlaying = type == Type.Youtube ? "https://www.youtube.com/watch?v=" + nowPlaying : nowPlaying;
    }

    public Guid Id { get; }
    public Type Type { get; }
    public int CountUsers { get; }
    public string NowPlaying { get; }
}