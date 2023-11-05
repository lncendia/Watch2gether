using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.PlaylistManagement;

public class PlaylistsSearchParameters
{
    [Display(Name = "Название подборки")] public string? Query { get; set; }
    [Required] public int Page { get; set; } = 1;
}