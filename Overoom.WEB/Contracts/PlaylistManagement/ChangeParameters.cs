using System.ComponentModel.DataAnnotations;
using Overoom.WEB.Contracts.FilmManagement;

namespace Overoom.WEB.Contracts.PlaylistManagement;

public class ChangeParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid Id { get; set; }
    
    [StringLength(1500, ErrorMessage = "Не больше 1500 символов")]
    public string? Name { get; set; }

    [StringLength(1500, ErrorMessage = "Не больше 1500 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    [StringLength(500, ErrorMessage = "Не больше 500 символов")]
    [Display(Name = "Короткое описание")]
    public string? ShortDescription { get; set; }


    [Display(Name = "Ссылка на постер")]
    [DataType(DataType.ImageUrl)]
    public string? NewPosterUri { get; set; }

    [Display(Name = "Постер")]
    [DataType(DataType.Upload)]
    public IFormFile? NewPoster { get; set; }

    [Display(Name = "Рейтинг")]
    [Range(0d, 10d, ErrorMessage = "Рейтинг должен быть в диапазоне от 0 до 10")]
    public double? Rating { get; set; }

    [Display(Name = "Количество сезонов")] public int? CountSeasons { get; set; }

    [Display(Name = "Количество серий")] public int? CountEpisodes { get; set; }

    [Display(Name = "Cdn")] public List<CdnParameters> Cdns { get; set; } = new() { new CdnParameters() };
}