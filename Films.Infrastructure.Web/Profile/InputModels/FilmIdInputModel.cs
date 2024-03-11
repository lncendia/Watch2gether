using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Profile.InputModels;

public class FilmIdInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid FilmId { get; init; }
}