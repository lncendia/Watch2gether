using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;

namespace Films.Application.Services.Profile;

public class SettingsService(IUnitOfWork unitOfWork) : ISettingsService
{
    public async Task ChangeAllowsAsync(Guid id, bool beep, bool scream, bool change)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        user.UpdateAllows(beep, scream, change);
        await unitOfWork.UserRepository.Value.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}