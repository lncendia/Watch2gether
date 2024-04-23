namespace Room.Infrastructure.Web.YoutubeRoom.Hubs.Models;

public class YoutubeViewerModel(
    int id,
    string username,
    Uri avatar,
    int time,
    string videoId,
    bool allowBeep,
    bool allowScream,
    bool allowChange)
    : ViewerModel(id, username, avatar, time, allowBeep, allowScream, allowChange)
{
    public string VideoId { get; } = videoId;
}