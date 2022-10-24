using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Models.Room.YoutubeRoom;

public class CreateYoutubeRoomForUserViewModel
{
    [Display(Name = "Доступ к добавлению видео")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public bool AddAccess { get; set; }
    
    [Display(Name = "Вставьте ссылку на видео")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Url)]
    public string Url { get; set; } = null!;
}