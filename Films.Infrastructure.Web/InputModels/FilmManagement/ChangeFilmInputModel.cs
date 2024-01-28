using System.ComponentModel.DataAnnotations;
using Films.Infrastructure.Web.Contracts.FilmManagement.Load;

namespace Films.Infrastructure.Web.Contracts.FilmManagement;

public class ChangeFilmInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid Id { get; init; }
    
    [StringLength(200, ErrorMessage = "Не больше 200 символов")]
    [Display(Name = "Название")]
    public string? Name { get; init; }

    [StringLength(1500, ErrorMessage = "Не больше 1500 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; init; }

    [StringLength(500, ErrorMessage = "Не больше 500 символов")]
    [Display(Name = "Короткое описание")]
    public string? ShortDescription { get; init; }


    [Display(Name = "Ссылка на постер")]
    [DataType(DataType.ImageUrl)]
    public string? NewPosterUri { get; init; }

    [Display(Name = "Постер")]
    [DataType(DataType.Upload)]
    public IFormFile? NewPoster { get; init; }

    [Display(Name = "Рейтинг")]
    [Range(0d, 10d, ErrorMessage = "Рейтинг должен быть в диапазоне от 0 до 10")]
    public double? Rating { get; init; }

    [Display(Name = "Количество сезонов")] public int? CountSeasons { get; init; }

    [Display(Name = "Количество серий")] public int? CountEpisodes { get; init; }

    [Display(Name = "Cdn")] public List<CdnInputModel>? Cdns { get; init; }
}