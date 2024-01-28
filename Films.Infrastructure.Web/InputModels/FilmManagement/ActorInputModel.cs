using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.FilmManagement.Load;

public class ActorInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Имя")]
    public string? Name { get; init; }
    
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; init; }
}