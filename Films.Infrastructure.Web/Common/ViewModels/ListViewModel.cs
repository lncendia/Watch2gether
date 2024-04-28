namespace Films.Infrastructure.Web.Common.ViewModels;

public class ListViewModel<T>
{
    public required IEnumerable<T> List { get; init; }
    public  required int TotalPages { get; init; }
}