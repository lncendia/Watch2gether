using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.User.InputModels;

public class GetRatingsInputModel
{
    [Range(1, 20)] public int CountPerPage { get; init; } = 10;
    [Range(1, int.MaxValue)] public int Page { get; init; } = 1;
}