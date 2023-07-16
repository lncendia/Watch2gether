using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Film;

public class AddCommentParameters
{
    [Required] public Guid FilmId { get; set; }

    [Display(Name = "Текст комментария")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(1000, ErrorMessage = "Не больше 1000 символов")]
    public string? Text { get; set; }
}