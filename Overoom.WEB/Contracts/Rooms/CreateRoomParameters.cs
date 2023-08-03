using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Rooms;

public class CreateRoomParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Тип комнаты")]
    public bool? IsOpen { get; set; }
}