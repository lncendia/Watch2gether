using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.PlaylistManagement;

public class PlaylistsSearchParameters
{
    [Display(Name = "Название подборки")] public string? Query { get; init; }
    [Required] public int Page { get; init; } = 1;
}