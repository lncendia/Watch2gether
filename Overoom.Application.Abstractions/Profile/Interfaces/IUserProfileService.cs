using Overoom.Application.Abstractions.Profile.DTOs;

namespace Overoom.Application.Abstractions.Profile.Interfaces;

public interface IUserProfileService
{

    Task<ProfileDto> GetProfileAsync(Guid id);
    Task<List<RatingDto>> GetRatingsAsync(Guid id, int page);
    Task<List<string>> GetGenresAsync(Guid id);
}