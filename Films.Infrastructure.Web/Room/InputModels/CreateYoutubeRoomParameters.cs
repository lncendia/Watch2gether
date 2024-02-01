using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.Rooms;

public class CreateYoutubeRoomParameters : CreateRoomParameters
{
    [Display(Name = "Доступ к добавлению видео")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public bool Access { get; init; }

    [Display(Name = "Вставьте ссылку на видео")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Url)]
    public string? Url { get; init; }
}