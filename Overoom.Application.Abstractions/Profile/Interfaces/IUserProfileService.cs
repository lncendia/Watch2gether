using Overoom.Application.Abstractions.Profile.DTOs;

namespace Overoom.Application.Abstractions.Profile.Interfaces;

public interface IUserProfileService
{

    Task<ProfileDto> GetProfileAsync(Guid id);
    Task<IReadOnlyCollection<RatingDto>> GetRatingsAsync(Guid id, int page);
    Task<IReadOnlyCollection<string>> GetGenresAsync(Guid id);
}