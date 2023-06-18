using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.Application.Abstractions.Users.Entities;
using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Specifications;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Application.Services.User;

public class UserManager : IUserManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserData> _userManager;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    private readonly IPasswordValidator<UserData> _passwordValidator;

    public UserManager(IUnitOfWork unitOfWork, UserManager<UserData> userManager,
        IPasswordHasher<UserData> passwordHasher, IPasswordValidator<UserData> passwordValidator)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _passwordValidator = passwordValidator;
    }

    public async Task<List<UserShortDto>> FindAsync(SearchQuery query)
    {
        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>? spec = null;
        if (!string.IsNullOrEmpty(query.Email)) spec = new UserByEmailSpecification(query.Email);
        if (!string.IsNullOrEmpty(query.Name))
        {
            spec = spec == null
                ? new UserByNameSpecification(query.Name)
                : new AndSpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>(spec, new UserByNameSpecification(query.Name));
        }

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec, null, (query.Page - 1) * 30,
            30);
        return users.Select(x => new UserShortDto(x.Name, x.Email, x.Id)).ToList();
    }

    public async Task<UserDto> GetAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        return new UserDto(user.Name, user.Email, user.Id);
    }

    public async Task EditAsync(EditUserDto editData)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(editData.Id);
        if (user == null) throw new UserNotFoundException();
        var userApplication = await _userManager.FindByEmailAsync(user.Email);
        if (userApplication == null) throw new UserNotFoundException();
        if (!string.IsNullOrEmpty(editData.Email))
        {
            user.Email = editData.Email;
            var result = await _userManager.SetEmailAsync(userApplication, editData.Email);
            if (!result.Succeeded) throw new UserAlreadyExistException();
            userApplication.EmailConfirmed = true;
            await _userManager.UpdateAsync(userApplication);
        }

        if (!string.IsNullOrEmpty(editData.Username))
        {
            user.Name = editData.Username;
            await _userManager.SetUserNameAsync(userApplication, editData.Username);
        }

        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(string email, string password)
    {
        var userApplication = await _userManager.FindByEmailAsync(email);
        if (userApplication == null) throw new UserNotFoundException();
        var result = await _passwordValidator.ValidateAsync(_userManager, userApplication, password);
        if (!result.Succeeded) throw new InvalidPasswordFormatException();
        userApplication.PasswordHash = _passwordHasher.HashPassword(userApplication, password);
        await _userManager.UpdateAsync(userApplication);
    }

    public async Task<UserData> GetAuthenticationDataAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        var userApplication = await _userManager.FindByEmailAsync(user.Email);
        if (userApplication == null) throw new UserNotFoundException();
        return userApplication;
    }
}