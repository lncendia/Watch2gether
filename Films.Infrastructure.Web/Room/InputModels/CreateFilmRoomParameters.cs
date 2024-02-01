using System.ComponentModel.DataAnnotations;
using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.Contracts.Rooms;

public class CreateFilmRoomParameters : CreateRoomParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid FilmId { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Выберите CDN")]
    public CdnType Cdn { get; init; }
}