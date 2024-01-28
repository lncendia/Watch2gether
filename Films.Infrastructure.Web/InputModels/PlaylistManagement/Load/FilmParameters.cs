using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.PlaylistManagement.Load;

public class FilmParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Идентификатор фильма")]
    public Guid? Id { get; init; }

    [Display(Name = "Имя")] public string? Name { get; init; }

    [Display(Name = "Описание")] public string? Description { get; init; }

    [Display(Name = "Постер")]
    [DataType(DataType.Url)]
    public string? Uri { get; init; }
}