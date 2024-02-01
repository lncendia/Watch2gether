using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.PlaylistManagement.InputModels;

public class ChangePlaylistInputModel
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
    public string? PosterUrl { get; init; }

    [Display(Name = "Постер")] public string? PosterBase64 { get; init; }

    [Display(Name = "Фильмы")] public PlaylistFilmInputModel[]? Films { get; init; }
}