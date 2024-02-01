using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.FilmsManagement.InputModels;

public class VoiceInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Название")]
    public string? Name { get; init; }
}