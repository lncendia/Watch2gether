using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Rooms.DTOs.Film;

public class CreateFilmRoomDto : CreateRoomDto
{
    public CreateFilmRoomDto(bool isOpen, Guid filmId, CdnType cdnType) : base(isOpen)
    {
        FilmId = filmId;
        CdnType = cdnType;
    }

    public Guid FilmId { get; }
    public CdnType CdnType { get; }
}