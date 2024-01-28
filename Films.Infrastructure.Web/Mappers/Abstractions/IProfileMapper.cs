using Films.Application.Abstractions.Profile.DTOs;
using Films.Infrastructure.Web.Models.Settings;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IProfileMapper
{
    ProfileViewModel Map(ProfileDto dto, IReadOnlyCollection<string> genres);
    RatingViewModel Map(RatingDto dto);
}