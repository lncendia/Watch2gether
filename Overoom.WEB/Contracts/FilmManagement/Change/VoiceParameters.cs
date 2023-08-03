using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.FilmManagement.Change;

public class VoiceParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Название")]
    public string? Name { get; set; }
}