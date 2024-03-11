using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Profile.InputModels;

public class AddRatingInputModel
{
    [Required] public Guid FilmId { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Range(0d, 10d, ErrorMessage = "Рейтинг должен быть в диапазоне от 0 до 10")]
    public double Score { get; init; }
}