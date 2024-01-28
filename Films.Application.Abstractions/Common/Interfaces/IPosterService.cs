namespace Films.Application.Abstractions.Common.Interfaces;

public interface IPosterService
{
    Task<Uri> SaveAsync(Stream stream);
    Task<Uri> SaveAsync(Uri url);
    Task DeleteAsync(Uri uri);
}