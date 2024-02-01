using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Comments.InputModels;

public class GetCommentsInputModel
{
    [Required] public Guid? FilmId { get; init; }
    [Range(1, 100)] public int CountPerPage { get; init; } = 50;
    [Range(1, int.MaxValue)] public int Page { get; init; } = 1;
}