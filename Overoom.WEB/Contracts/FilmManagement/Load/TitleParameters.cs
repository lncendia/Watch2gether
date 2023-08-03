using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.FilmManagement.Load;

public class TitleParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Название")]
    public string? Name { get; set; }
}