using Overoom.Domain.Rooms.YoutubeRoom.Exceptions;

namespace Room.Domain.Rooms.YoutubeRoom.ValueObjects;

public class Video
{
    public Video(int orderNumber, Uri url)
    {
        OrderNumber = orderNumber;
        Id = GetId(url);
    }

    public int OrderNumber { get; }
    public string Id { get; }

    private static string GetId(Uri uri)
    {
        string id;
        try
        {
            id = uri.Host switch
            {
                "www.youtube.com" => uri.Query[3..],
                "youtu.be" => uri.Segments[1],
                _ => string.Empty
            };
        }
        catch
        {
            throw new InvalidVideoUrlException();
        }

        if (string.IsNullOrEmpty(id)) throw new InvalidVideoUrlException();
        return id;
    }
}