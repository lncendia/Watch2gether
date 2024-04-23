using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Rooms.InputModels;

public class CreateFilmRoomInputModel
{
    public bool Open { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid FilmId { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(30, ErrorMessage = "Не больше 30 символов")]
    public string? CdnName { get; init; }
}