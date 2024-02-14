using Room.Domain.Rooms.YoutubeRoom.Exceptions;

namespace Room.Domain.Rooms.YoutubeRoom.ValueObjects;

public class Video(Uri url)
{
    public DateTime Added { get; } = DateTime.UtcNow;
    public string Id { get; } = GetId(url);

    private static string GetId(Uri uri)
    {
        string id;
        try
        {
            id = uri.Host switch
            {
                "www.youtube.com" => uri.Query[3..],
                "youtu.be" => uri.Segments[1],
                _ => throw new ArgumentOutOfRangeException(nameof(uri))
            };
        }
        catch
        {
            throw new InvalidVideoUrlException();
        }
        
        return id;
    }
}