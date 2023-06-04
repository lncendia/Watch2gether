namespace Overoom.Application.Abstractions.StartPage.DTOs;

public class RoomStartPageDto
{
    public RoomStartPageDto(Guid id, Type type, int countUsers, string nowPlaying)
    {
        Id = id;
        Type = type;
        CountUsers = countUsers;
        NowPlaying = nowPlaying;
    }

    public Guid Id { get; }
    public Type Type { get; }
    public int CountUsers { get; }
    public string NowPlaying { get; }
}

public enum Type
{
    Film,
    Youtube
}