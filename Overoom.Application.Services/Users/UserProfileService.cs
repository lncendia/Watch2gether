using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.Application.Abstractions.Users.Entities;
using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ratings;
using Overoom.Domain.Ratings.Ordering;
using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Domain.Ratings.Specifications;

namespace Overoom.Application.Services.Users;

public class UserProfileService : IUserProfileService
{
    private readonly IUserThumbnailService _thumbnailService;
    private readonly UserManager<UserData> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IProfileMapper _profileMapper;

    public UserProfileService(IUserThumbnailService thumbnailService, UserManager<UserData> userManager,
        IUnitOfWork unitOfWork, IEmailService emailService, IProfileMapper profileMapper)
    {
        _thumbnailService = thumbnailService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _profileMapper = profileMapper;
    }

    public async Task<ProfileDto> GetProfileAsync(Guid id)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();

        var history = user.History.OrderByDescending(x => x.Date).Select(x => x.FilmId).ToList();
        var watchlist = user.Watchlist.OrderByDescending(x => x.Date).Select(x => x.FilmId).ToList();

        var watchlistSpec = new FilmByIdsSpecification(watchlist);
        var historySpec = new FilmByIdsSpecification(history);

        var watchlistFilms = await _unitOfWork.FilmRepository.Value.FindAsync(watchlistSpec);
        var historyFilms = await _unitOfWork.FilmRepository.Value.FindAsync(historySpec);
        return _profileMapper.Map(user, historyFilms.OrderBy(film => history.IndexOf(film.Id)),
            watchlistFilms.OrderBy(film => watchlist.IndexOf(film.Id)));
    }

    public async Task<List<RatingDto>> GetRatingsAsync(Guid id, int page)
    {
        var spec = new RatingByUserSpecification(id);
        var ratings =
            await _unitOfWork.RatingRepository.Value.FindAsync(spec,
                new DescendingOrder<Rating, IRatingSortingVisitor>(new RatingOrderByDate()), (page - 1) * 10, 10);
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(
            new FilmByIdsSpecification(ratings.Select(x => x.FilmId)));
        return ratings.Select(x => _profileMapper.Map(x, films.First(f => f.Id == x.FilmId))).ToList();
    }

    public async Task ChangeNameAsync(Guid id, string name)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        user.Name = name;
        var userData = await _userManager.FindByEmailAsync(user.Email);
        if (userData == null) throw new UserNotFoundException();
        userData.UserName = name;
        await _userManager.UpdateAsync(userData);
        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Uri> ChangeAvatarAsync(Guid id, Stream avatar)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        var userData = await _userManager.FindByEmailAsync(user.Email);
        if (userData == null) throw new UserNotFoundException();
        if (user.AvatarUri.ToString() != ApplicationConstants.DefaultAvatar.ToString())
            await _thumbnailService.DeleteAsync(user.AvatarUri);
        user.AvatarUri = await _thumbnailService.SaveAsync(avatar);
        var claims = await _userManager.GetClaimsAsync(userData);
        await _userManager.ReplaceClaimAsync(userData,
            claims.First(x => x.Type == ApplicationConstants.AvatarClaimType),
            new Claim(ApplicationConstants.AvatarClaimType, user.AvatarUri.ToString()));
        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user.AvatarUri;
    }

    public async Task RequestResetEmailAsync(Guid id, string newEmail, string resetUrl)
    {
        var users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
        var user = users.FirstOrDefault();
        if (user == null || !await _userManager.IsEmailConfirmedAsync(user)) throw new UserNotFoundException();
        var code = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        var url = resetUrl + $"?email={Uri.EscapeDataString(user.Email!)}&code={Uri.EscapeDataString(code)}";
        try
        {
            await _emailService.SendEmailAsync(user.Email!,
                $"Подтвердите смену почты, перейдя по <a href = \"{url}\">ссылке</a>.");
        }
        catch (Exception ex)
        {
            throw new EmailException(ex);
        }
    }

    public async Task ResetEmailAsync(Guid id, string newEmail, string code)
    {
        var users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
        var user = users.FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        var result = await _userManager.ChangeEmailAsync(user, newEmail, code);
        if (!result.Succeeded) throw new InvalidCodeException();
        var userDomain = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (userDomain == null) throw new UserNotFoundException();
        userDomain.Email = newEmail;
        await _unitOfWork.UserRepository.Value.UpdateAsync(userDomain);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(Guid id, string oldPassword, string newPassword)
    {
        if (oldPassword == newPassword)
        {
            throw new ArgumentException("The new password should be different from the current one.",
                nameof(newPassword));
        }

        var users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
        var user = users.FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded)
            throw new ArgumentException("The old password is specified incorrectly.", nameof(oldPassword));
    }
}