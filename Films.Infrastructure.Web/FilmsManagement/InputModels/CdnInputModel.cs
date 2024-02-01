using System.ComponentModel.DataAnnotations;
using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.FilmsManagement.InputModels;

public class CdnInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Тип")]
    public CdnType? Type { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Url)]
    [Display(Name = "Ссылка")]
    public string? Url { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Качество")]
    public string? Quality { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Озвучки")]
    public VoiceInputModel[]? Voices { get; init; }
}