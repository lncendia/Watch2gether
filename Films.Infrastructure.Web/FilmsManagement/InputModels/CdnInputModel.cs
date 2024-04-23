using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.FilmsManagement.InputModels;

public class CdnInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Тип")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Url)]
    [Display(Name = "Ссылка")]
    public string? Url { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Качество")]
    public string? Quality { get; init; }
}