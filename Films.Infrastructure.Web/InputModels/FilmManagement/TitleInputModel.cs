using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.FilmManagement.Load;

public class TitleInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Название")]
    public string? Name { get; init; }
}