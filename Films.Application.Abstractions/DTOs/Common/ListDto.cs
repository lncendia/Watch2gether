namespace Films.Application.Abstractions.DTOs.Common;

public class ListDto<T>
{
    public required IReadOnlyCollection<T> List { get; init; }
    public  required int TotalCount { get; init; }
}