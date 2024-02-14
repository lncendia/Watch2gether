using MediatR;
using Room.Domain.Users.ValueObjects;

namespace Room.Application.Abstractions.Commands.Users;

/// <summary>
/// Команда на изменение пользователя
/// </summary>
public class ChangeUserCommand : IRequest
{
    public required Guid Id { get; init; }
    
    public required string UserName { get; init; }

    public required Uri PhotoUrl { get; init; }
    
    public required Allows Allows { get; init; } 
}