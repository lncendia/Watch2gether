using System.ComponentModel.DataAnnotations;
using Overoom.Application.Abstractions.Content.DTOs;

namespace Overoom.WEB.Contracts.Content;

public class PlaylistsSearchParameters
{
    [Display(Name = "Название подборки")] public string? Query { get; set; }
    [Display(Name = "Жанр")] public string? Genre { get; set; }
    [Display(Name = "Сортировать по")] public PlaylistSortBy SortBy { get; set; } = PlaylistSortBy.Date;
    [Display(Name = "Порядок")] public bool InverseOrder { get; set; } = true;
    public int Page { get; set; } = 1;
}