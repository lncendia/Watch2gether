using System.ComponentModel.DataAnnotations;
using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Contracts.Rooms;

public class CreateFilmRoomParameters : CreateRoomParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid FilmId { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Выберите CDN")]
    public CdnType Cdn { get; set; }
}