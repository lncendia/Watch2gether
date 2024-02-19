using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Rooms.InputModels;

public class ConnectRoomInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid Id { get; init; }

    [StringLength(5, ErrorMessage = "Не больше 5 символов")]
    public string? Code { get; init; }
}