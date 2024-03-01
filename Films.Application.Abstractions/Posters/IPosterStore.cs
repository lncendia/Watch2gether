namespace Films.Application.Abstractions.Posters;

public interface IPosterStore
{
    Task<Uri> SaveAsync(string base64);
    Task<Uri> SaveAsync(Uri url);
    Task DeleteAsync(Uri url);
}