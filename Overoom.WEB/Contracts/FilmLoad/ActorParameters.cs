using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.FilmLoad;

public class ActorParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Имя")]
    public string Name { get; set; } = null!;
    
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; set; }
}