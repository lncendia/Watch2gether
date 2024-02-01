using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.FilmsManagement.InputModels;

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
    public string? PosterUrl { get; init; }

    [Display(Name = "Постер")]
    public string? PosterBase64 { get; init; }

    [Display(Name = "Рейтинг Кинопоиск")]
    [Range(0d, 10d, ErrorMessage = "Рейтинг должен быть в диапазоне от 0 до 10")]
    public double? RatingKp { get; init; }
    
    [Display(Name = "Рейтинг Imdb")]
    [Range(0d, 10d, ErrorMessage = "Рейтинг должен быть в диапазоне от 0 до 10")]
    public double? RatingImdb { get; init; }

    [Display(Name = "Количество сезонов")] public int? CountSeasons { get; init; }

    [Display(Name = "Количество серий")] public int? CountEpisodes { get; init; }

    [Display(Name = "Cdn")] public CdnInputModel[]? Cdns { get; init; }
}