using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.Rooms;

public class CreateRoomParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Тип комнаты")]
    public bool? IsOpen { get; init; }
}