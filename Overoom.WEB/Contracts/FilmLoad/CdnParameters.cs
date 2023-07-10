using System.ComponentModel.DataAnnotations;
using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Contracts.FilmLoad;

public class CdnParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Тип")]
    public CdnType Type { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Url)]
    [Display(Name = "Ссылка")]
    public string Uri { get; set; } = null!;

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(100, ErrorMessage = "Не больше 100 символов")]
    [Display(Name = "Качество")]
    public string Quality { get; set; } = null!;

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Озвучки")]
    public List<TitleParameters> Voices { get; set; } = null!;
}