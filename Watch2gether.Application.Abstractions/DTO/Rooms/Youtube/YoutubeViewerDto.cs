namespace Watch2gether.Application.Abstractions.DTO.Rooms.Youtube;

public class YoutubeViewerDto : ViewerDto
{
    public YoutubeViewerDto(string username, Guid id, string avatarUrl, TimeSpan time, bool onPause,
        string currentVideoId) : base(username, id, avatarUrl, time, onPause)
    {
        CurrentVideoId = currentVideoId;
    }

    public string CurrentVideoId { get; }
}