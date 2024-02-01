using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Films.InputModels;

public class PopularFilmsInputModel
{
    [Range(1, 30)] public int Count { get; init; } = 15;
}