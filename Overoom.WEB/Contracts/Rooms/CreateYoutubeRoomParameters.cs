using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Rooms;

public class CreateYoutubeRoomParameters : CreateRoomParameters
{
    [Display(Name = "Доступ к добавлению видео")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public bool AddAccess { get; set; }

    [Display(Name = "Вставьте ссылку на видео")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Url)]
    public string? Url { get; set; }
}