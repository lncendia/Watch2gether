using System.ComponentModel.DataAnnotations;

namespace Watch2gether.WEB.Models.Room.YoutubeRoom;

public class CreateYoutubeRoomViewModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Введите имя пользователя")]
    [RegularExpression("^[a-zA-Zа-яА-Я0-9_ ]{3,20}$",
        ErrorMessage = "Имя пользователя должно содержать от 3 до 20 символов")]
    public string Name { get; set; } = null!;

    [Display(Name = "Вставьте ссылку на видео")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Url)]
    public string Url { get; set; } = null!;
}