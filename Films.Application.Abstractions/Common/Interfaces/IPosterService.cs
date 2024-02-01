namespace Films.Application.Abstractions.Common.Interfaces;

public interface IPosterService
{
    Task<Uri> SaveAsync(string base64);
    Task<Uri> SaveAsync(Uri url);
    Task DeleteAsync(Uri url);
}