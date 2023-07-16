using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Film;

public class AddRatingParameters
{
    [Required] public Guid FilmId { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Range(0d, 10d, ErrorMessage = "Рейтинг должен быть в диапазоне от 0 до 10")]
    public double Score { get; set; }
}