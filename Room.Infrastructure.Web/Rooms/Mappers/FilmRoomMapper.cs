using Room.Application.Abstractions.DTOs.FilmRooms;
using Room.Infrastructure.Web.Rooms.ViewModels.Common;
using Room.Infrastructure.Web.Rooms.ViewModels.FilmRooms;

namespace Room.Infrastructure.Web.Rooms.Mappers;

public class FilmRoomMapper
{
    public static FilmRoomViewModel Map(FilmRoomDto dto) => new()
    {
        Title = dto.Title,
        CdnName = dto.Cdn.Name,
        CdnUrl = dto.Cdn.Url.ToString(),
        IsSerial = dto.IsSerial,
        Id = dto.Id,
        OwnerId = dto.OwnerId,
        Viewers = dto.Viewers.Select(v => new FilmViewerViewModel
        {
            Id = v.Id,
            Username = v.Username,
            PhotoUrl = v.PhotoUrl?.ToString(),
            Pause = v.Pause,
            FullScreen = v.FullScreen,
            Online = v.Online,
            Second = (int)v.TimeLine.TotalSeconds,
            Season = v.Season,
            Series = v.Series,
            Allows = new AllowsViewModel
            {
                Beep = v.Allows.Beep,
                Scream = v.Allows.Scream,
                Change = v.Allows.Change
            }
        }).ToArray()
    };
}