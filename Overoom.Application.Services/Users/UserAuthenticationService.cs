using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.Application.Abstractions.Users.Entities;
using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Users.Entities;
using Overoom.Domain.Users.Exceptions;

namespace Overoom.Application.Services.Users;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly UserManager<UserData> _userManager;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserThumbnailService _thumbnailService;

    public UserAuthenticationService(UserManager<UserData> userManager, IEmailService emailService,
        IUnitOfWork unitOfWork, IUserThumbnailService thumbnailService)
    {
        _userManager = userManager;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _thumbnailService = thumbnailService;
    }

    public async Task CreateAsync(UserCreateDto userDto, string confirmUrl)
    {
        var user = new UserData(userDto.Username, userDto.Email);
        var result = await _userManager.CreateAsync(user, userDto.Password);
        CheckResult(result, user);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var url = confirmUrl + $"?email={Uri.EscapeDataString(user.Email!)}&code={Uri.EscapeDataString(code)}";
        try
        {
            await _emailService.SendEmailAsync(user.Email!,
                $"Подтвердите регистрацию, перейдя по <a href=\"{url}\">ссылке</a>.");
        }
        catch (Exception ex)
        {
            await _userManager.DeleteAsync(user);
            throw new EmailException(ex);
        }
    }

    public async Task<UserData> ExternalAuthenticateAsync(ExternalLoginInfo info)
    {
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        if (user != null) return user;

        var avatarUri = info.LoginProvider switch
        {
            "Vkontakte" => await _thumbnailService.SaveAsync(
                new Uri(info.Principal.FindFirstValue("urn:vkontakte:photo:link")!)),
            "Yandex" => await _thumbnailService.SaveAsync(new Uri(
                @$"https://avatars.yandex.net/get-yapic/{info.Principal.FindFirstValue("urn:yandex:user:avatar")}/islands-75")),
            _ => ApplicationConstants.DefaultAvatar
        };

        user = new UserData(
            info.Principal.FindFirstValue(ClaimTypes.GivenName) + ' ' +
            info.Principal.FindFirstValue(ClaimTypes.Surname),
            info.Principal.FindFirstValue(ClaimTypes.Email)!)
        {
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(user);
        CheckResult(result, user);

        await _userManager.AddLoginAsync(user, info);
        var userDomain = new User(user.UserName!, user.Email!, avatarUri);
        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, userDomain.Id.ToString()));
        await _userManager.AddClaimAsync(user,
            new Claim(ApplicationConstants.AvatarClaimType, avatarUri.ToString()));
        await AddAsync(userDomain);

        return user;
    }

    public async Task<UserData> CodeAuthenticateAsync(string email, string code)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) throw new UserNotFoundException();
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded) throw new InvalidCodeException();
        var userDomain = new User(user.UserName!, user.Email!, ApplicationConstants.DefaultAvatar);
        await AddAsync(userDomain);
        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, userDomain.Id.ToString()));
        await _userManager.AddClaimAsync(user,
            new Claim(ApplicationConstants.AvatarClaimType, userDomain.AvatarUri.ToString()));
        return user;
    }

    public async Task<UserData> PasswordAuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new UserNotFoundException();
        var success = await _userManager.CheckPasswordAsync(user, password);
        if (!success) throw new InvalidPasswordException();
        return user;
    }

    public async Task RequestResetPasswordAsync(string email, string resetUrl)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user)) throw new UserNotFoundException();
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var url = resetUrl + $"?email={Uri.EscapeDataString(email)}&code={Uri.EscapeDataString(code)}";
        try
        {
            await _emailService.SendEmailAsync(email,
                $"Подтвердите смену пароля, перейдя по <a href = \"{url}\">ссылке</a>.");
        }
        catch (Exception ex)
        {
            await _userManager.DeleteAsync(user);
            throw new EmailException(ex);
        }
    }


    public async Task ResetPasswordAsync(string email, string code, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) throw new UserNotFoundException();
        var result = await _userManager.ResetPasswordAsync(user, code, newPassword);
        if (!result.Succeeded) throw new InvalidCodeException();
    }

    private async Task AddAsync(User user)
    {
        await _unitOfWork.UserRepository.Value.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    private static void CheckResult(IdentityResult result, UserData data)
    {
        if (result.Succeeded) return;
        Exception ex = result.Errors.First().Code switch
        {
            "MailUsed" => new UserAlreadyExistException(),
            "MailIncorrect" => new InvalidEmailException(data.Email!),
            "NameIncorrect" => new InvalidNicknameException(data.UserName!),
            _ => new UserCreationException(result.Errors.First().Description)
        };
        throw ex;
    }
}