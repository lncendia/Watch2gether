using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.PlaylistManagement.Change;

public class ChangeParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid Id { get; init; }

    [StringLength(200, ErrorMessage = "Не больше 200 символов")]
    [Display(Name = "Название")]
    public string? Name { get; init; }

    [StringLength(500, ErrorMessage = "Не больше 500 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; init; }

    [Display(Name = "Ссылка на постер")]
    [DataType(DataType.ImageUrl)]
    public string? NewPosterUri { get; init; }

    [Display(Name = "Постер")]
    [DataType(DataType.Upload)]
    public IFormFile? NewPoster { get; init; }

    [Display(Name = "Фильмы")] public List<FilmParameters> Films { get; init; } = new();
}