using MediatR;

namespace Room.Application.Abstractions.Commands.Users;

/// <summary>
/// Команда на добавление пользователя
/// </summary>
public class AddUserCommand : IRequest
{
    public required Guid Id { get; init; }
    
    public required string UserName { get; init; }

    public required Uri PhotoUrl { get; init; }
}