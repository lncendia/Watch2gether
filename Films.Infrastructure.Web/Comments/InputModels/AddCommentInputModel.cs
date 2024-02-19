using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Comments.InputModels;

public class AddCommentInputModel
{
    [Required] public Guid FilmId { get; init; }

    [Display(Name = "Текст комментария")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(1000, ErrorMessage = "Не больше 1000 символов")]
    public string? Text { get; init; }
}