using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.PlaylistManagement.InputModels;

public class CreatePlaylistInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(200, ErrorMessage = "Не больше 200 символов")]
    [Display(Name = "Название")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(500, ErrorMessage = "Не больше 500 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; init; }

    [Display(Name = "Ссылка на постер")]
    [DataType(DataType.ImageUrl)]
    public string? PosterUrl { get; init; }

    [Display(Name = "Постер")]
    public string? PosterBase64 { get; init; }
}