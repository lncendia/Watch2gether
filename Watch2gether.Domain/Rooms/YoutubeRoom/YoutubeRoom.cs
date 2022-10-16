using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;
using Watch2gether.Domain.Rooms.YoutubeRoom.Entities;

namespace Watch2gether.Domain.Rooms.YoutubeRoom;

public class YoutubeRoom : BaseRoom.BaseRoom
{
    public YoutubeRoom(string id, string name, string avatarFileName)
    {
        AddId(id);
        Owner = new YoutubeViewer(name, Id, avatarFileName, id);
    }

    public YoutubeViewer Owner { get; }
    public List<YoutubeViewer> Viewers => ViewersList.Select(x => (YoutubeViewer) x).ToList();
    public List<string> VideoIds => _ids.ToList();
    private readonly List<string> _ids = new();

    public YoutubeViewer Connect(string name, string avatarFileName)
    {
        if (ViewersList.Count >= 10) throw new RoomIsFullException();

        var viewer = new YoutubeViewer(name, Id, avatarFileName, Owner.CurrentVideoId);
        ViewersList.Add(viewer);
        return viewer;
    }


    public void AddId(string id) => _ids.Add(id);

    public void RemoveId(string id)
    {
        if (_ids.Count == 1) throw new InvalidOperationException("Cannot remove the last video from a room");
        if (ViewersList.Any(x => ((YoutubeViewer) x).CurrentVideoId == id))
            throw new Exception(); //TODO: Create custom exception
        _ids.Remove(id);
    }
}