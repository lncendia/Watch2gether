namespace Overoom.WEB.Hubs.Models;

public class YoutubeViewerModel : ViewerModel
{
    public YoutubeViewerModel(int id, string username, Uri avatar, int time, string videoId) : base(id, username,
        avatar, time)
    {
        VideoId = videoId;
    }

    public string VideoId { get; }
}