namespace Overoom.Infrastructure.Web.Hubs.Models;

public class YoutubeViewerModel : ViewerModel
{
    public YoutubeViewerModel(int id, string username, Uri avatar, int time, string videoId, bool allowBeep,
        bool allowScream, bool allowChange) : base(id, username, avatar, time, allowBeep, allowScream, allowChange)
    {
        VideoId = videoId;
    }

    public string VideoId { get; }
}