using Overoom.Application.Abstractions.Kinopoisk.DTOs;

namespace Overoom.Application.Abstractions.Kinopoisk.Interfaces;

public interface IVideoCdnApiService
{
    Task<Cdn> GetInfoAsync(long kpId);
}